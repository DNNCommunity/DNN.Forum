'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2010
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Option Strict On
Option Explicit On 

Namespace DotNetNuke.Modules.Forum

#Region "GroupController"

	''' <summary>
	''' CRUD (and all db methods) Group database methods
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	7/13/2005	Created
	''' </history>
	Public Class GroupController

#Region "Private Members"

		Private Const GroupCacheKeyPrefix As String = "Group-Cache-"
		Private Const GroupCollectionKeyPrefix As String = "ModuleGroup-Cache-"
		Private Const GroupCacheTimeout As Integer = 20

#End Region

#Region "Public Methods"

		''' <summary>
		''' Returns all groups based on a moduleId. This is not user specific and can be cached. 
		''' </summary>
		''' <param name="ModuleId">The ModuleID being used to retrieve groups.</param>
		''' <returns>A collection of all groups associated with a single module.</returns>
		''' <remarks>
		''' </remarks>
		Public Function GetCachedModuleGroups(ByVal ModuleId As Integer) As List(Of GroupInfo)
			Dim strCacheKey As String = GroupCollectionKeyPrefix & ModuleId.ToString()
			Dim arrGroups As New List(Of GroupInfo)
			arrGroups = CType(DataCache.GetCache(strCacheKey), List(Of GroupInfo))

			If arrGroups Is Nothing Then
				Dim timeOut As Int32 = GroupCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				arrGroups = GetModuleGroups(ModuleId)

				'Cache Group if timeout > 0 and Group is not null
				If timeOut > 0 And arrGroups IsNot Nothing Then
					DataCache.SetCache(strCacheKey, arrGroups, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return arrGroups
		End Function

		''' <summary>
		''' Attempts to load the group from cache, if not available it retrieves it and places it in cache. 
		''' </summary>
		''' <param name="GroupID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetCachedGroup(ByVal GroupID As Integer) As GroupInfo
			Dim strCacheKey As String = GroupCacheKeyPrefix & CStr(GroupID)
			Dim objGroup As New GroupInfo
			objGroup = CType(DataCache.GetCache(strCacheKey), GroupInfo)

			If objGroup Is Nothing Then
				objGroup = GetGroup(GroupID)
				DataCache.SetCache(strCacheKey, objGroup)
			End If

			Return objGroup
		End Function

		''' <summary>
		''' Removes the group from cache.
		''' </summary>
		''' <param name="GroupID"></param>
		''' <remarks></remarks>
		Public Sub ResetCachedGroup(ByVal GroupID As Integer, ByVal ModuleID As Integer)
			Dim strCacheKey As String = GroupCacheKeyPrefix & CStr(GroupID)

			DataCache.RemoveCache(strCacheKey)
			ResetCachedModuleGroups(ModuleID)
		End Sub

		''' <summary>
		''' Returns all groups with at least one authorized forum specific to a user. This is necessary so we know which forum groups to show the user (if there is not a single valid forum, don't show the group). 
		''' </summary>
		''' <param name="ModuleId">The Module we wish to get groups for.</param>
		''' <param name="UserID">The UserID to return results for. (Because of private permissions)</param>
		''' <param name="NoLinkForums">True if Link Type forums should be excluded.</param>
		''' <returns>A  collection of Groups.</returns>
		''' <remarks>This is user specific, should never be cached. 
		''' </remarks>
		Public Function GetUserAuthorizedGroups(ByVal ModuleId As Integer, ByVal UserID As Integer, ByVal NoLinkForums As Boolean) As List(Of GroupInfo)
			Dim arrAllGroups As New List(Of GroupInfo)
			Dim arrAuthGroups As New List(Of GroupInfo)
			arrAllGroups = GetModuleGroups(ModuleId)

			If arrAllGroups.Count > 0 Then
				For Each objGroup As GroupInfo In arrAllGroups
					Dim cntForum As New ForumController
					Dim arrAuthForums As New List(Of ForumInfo)

					arrAuthForums = cntForum.AuthorizedForums(objGroup.GroupID, ModuleId, objGroup.objConfig, UserID, NoLinkForums)

					If arrAuthForums.Count > 0 Then
						arrAuthGroups.Add(objGroup)
					End If
				Next
			End If

			Return arrAuthGroups
		End Function

		''' <summary>
		''' Adds a new group to the database. 
		''' </summary>
		''' <param name="Name"></param>
		''' <param name="PortalID"></param>
		''' <param name="ModuleId"></param>
		''' <param name="CreatedByUser"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GroupAdd(ByVal Name As String, ByVal PortalID As Integer, ByVal ModuleId As Integer, ByVal CreatedByUser As Integer) As Integer
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().GroupAdd(Name, PortalID, ModuleId, CreatedByUser)
		End Function

		''' <summary>
		''' Updates an existing group in the database. 
		''' </summary>
		''' <param name="GroupID"></param>
		''' <param name="Name"></param>
		''' <param name="UpdatedByUser"></param>
		''' <param name="SortOrder"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Sub GroupUpdate(ByVal GroupID As Integer, ByVal Name As String, ByVal UpdatedByUser As Integer, ByVal SortOrder As Integer, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().GroupUpdate(GroupID, Name, UpdatedByUser, SortOrder, ModuleID)

			ResetCachedGroup(GroupID, ModuleID)
		End Sub

		''' <summary>
		''' Delete's a forum group 
		''' (Should not happen when there are forums within it)
		''' </summary>
		''' <param name="GroupID">The PK value associated with the Group.</param>
		''' <param name="ModuleID">The module instance the group is tied to.</param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/11/2006	Created
		''' </history>
		Public Sub GroupDelete(ByVal GroupID As Integer, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().GroupDelete(GroupID, ModuleID)

			ResetCachedGroup(GroupID, ModuleID)
		End Sub

		''' <summary>
		''' Updates the view order of the groups (top to bottom) - Moves up
		''' or down one level
		''' </summary>
		''' <param name="GroupID">Integer</param>
		''' <param name="MoveUp">Boolean</param>
		''' <remarks>
		''' </remarks>
		Public Sub GroupSortOrderUpdate(ByVal GroupID As Integer, ByVal MoveUp As Boolean, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().GroupSortOrderUpdate(GroupID, MoveUp)

			ResetCachedGroup(GroupID, ModuleID)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Returns a single forum group.
		''' </summary>
		''' <param name="GroupId">Integer</param>
		''' <returns>GroupInfo</returns>
		''' <remarks>
		''' </remarks>
		Private Function GetGroup(ByVal GroupId As Integer) As GroupInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().GroupGet(GroupId), GetType(GroupInfo)), GroupInfo)
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ModuleId"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetModuleGroups(ByVal ModuleId As Integer) As List(Of GroupInfo)
			Return CBO.FillCollection(Of GroupInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GroupGetByModuleID(ModuleId))
		End Function

		''' <summary>
		''' Used to clear the group cache. (All groups are cached)
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Private Sub ResetCachedModuleGroups(ByVal ModuleID As Integer)
			Dim strCacheKey As String = GroupCollectionKeyPrefix & ModuleID.ToString()

			DataCache.RemoveCache(strCacheKey)
		End Sub

#End Region

	End Class

#End Region

End Namespace