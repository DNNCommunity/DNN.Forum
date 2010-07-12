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

	''' <summary>
	''' CRUD (and all db methods) Group database methods
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class GroupController

#Region "Private Members"

		Private Const GroupColCacheKeyPrefix As String = Constants.CACHE_KEY_PREFIX + "GROUP_MODULE_"
		Private Const GroupCacheKeyPrefix As String = Constants.CACHE_KEY_PREFIX + "GROUP_"

#End Region

#Region "Public Methods"

#Region "Public Methods"

		''' <summary>
		''' Builds a collection of authorized forums for the specified Group.
		''' </summary>
		''' <param name="UserID">The user to return results for. (Permissions)</param>
		''' <param name="NoLinkForums">True if no link type forums should be added to the collection.</param>
		''' <returns>A Generics collection of ForumInfo items.</returns>
		''' <remarks></remarks>
		Public Function AuthorizedForums(ByVal UserID As Integer, ByVal GroupID As Integer, ByVal NoLinkForums As Boolean, ByVal ModuleID As Integer, ByVal TabID As Integer) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.GetGroupForums(GroupID)
			' add Aggregated Forum option
			If GroupID = -1 Then
				objForum = New ForumInfo
				objForum.ModuleID = ModuleID
				objForum.GroupID = -1
				objForum.ForumID = -1
				objForum.ForumType = ForumType.Normal

				arrAuthForums.Add(objForum)
			End If

			For Each objForum In arrAllForums
				Dim Security As New Forum.ModuleSecurity(ModuleID, TabID, objForum.ForumID, UserID)
				If Not objForum.PublicView And objForum.IsActive Then
					If Security.IsAllowedToViewPrivateForum And objForum.IsActive Then
						If NoLinkForums Then
							If Not (objForum.ForumType = ForumType.Link) Then
								arrAuthForums.Add(objForum)
							End If
						Else
							arrAuthForums.Add(objForum)
						End If
					End If
				ElseIf objForum.IsActive Then
					'We handle non-private seperately because module security (core) handles the rest
					If NoLinkForums Then
						If Not (objForum.ForumType = ForumType.Link) Then
							arrAuthForums.Add(objForum)
						End If
					Else
						arrAuthForums.Add(objForum)
					End If
				End If
			Next

			Return arrAuthForums
		End Function

		''' <summary>
		''' Builds a collection of authorized forums with no parents for the specified Group.
		''' </summary>
		''' <param name="UserID">The user to return results for. (Permissions)</param>
		''' <param name="NoLinkForums">True if no link type forums should be added to the collection.</param>
		''' <returns>A Generics collection of ForumInfo items.</returns>
		''' <remarks></remarks>
		Public Function AuthorizedNoParentForums(ByVal UserID As Integer, ByVal GroupID As Integer, ByVal NoLinkForums As Boolean, ByVal ModuleID As Integer, ByVal TabID As Integer) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.GetChildForums(0, GroupID, True)
			' add Aggregated Forum option
			If GroupID = -1 Then
				objForum = New ForumInfo
				objForum.ModuleID = ModuleID
				objForum.GroupID = -1
				objForum.ForumID = -1
				objForum.ForumType = ForumType.Normal

				arrAuthForums.Add(objForum)
			End If

			For Each objForum In arrAllForums
				Dim Security As New Forum.ModuleSecurity(ModuleID, TabID, objForum.ForumID, UserID)
				If Not objForum.PublicView And objForum.IsActive Then
					If Security.IsAllowedToViewPrivateForum And objForum.IsActive Then
						If NoLinkForums Then
							If Not (objForum.ForumType = ForumType.Link) Then
								arrAuthForums.Add(objForum)
							End If
						Else
							arrAuthForums.Add(objForum)
						End If
					End If
				ElseIf objForum.IsActive Then
					'We handle non-private seperately because module security (core) handles the rest
					If NoLinkForums Then
						If Not (objForum.ForumType = ForumType.Link) Then
							arrAuthForums.Add(objForum)
						End If
					Else
						arrAuthForums.Add(objForum)
					End If
				End If
			Next

			Return arrAuthForums
		End Function

		''' <summary>
		''' Builds a collection of authorized subforums for the specified Group/Parrent Forum.
		''' </summary>
		''' <param name="UserID">The user to return results for. (Permissions)</param>
		''' <param name="NoLinkForums">True if no link type forums should be added to the collection.</param>
		''' <returns>A Generics collection of ForumInfo items.</returns>
		''' <remarks></remarks>
		Public Function AuthorizedSubForums(ByVal UserID As Integer, ByVal GroupID As Integer, ByVal NoLinkForums As Boolean, ByVal ParentForumId As Integer, ByVal ModuleID As Integer, ByVal TabID As Integer) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.GetChildForums(ParentForumId, GroupID, True)
			' add Aggregated Forum option
			If GroupID = -1 Then
				objForum = New ForumInfo
				objForum.ModuleID = ModuleID
				objForum.GroupID = -1
				objForum.ForumID = -1
				objForum.ForumType = ForumType.Normal

				arrAuthForums.Add(objForum)
			End If

			For Each objForum In arrAllForums
				Dim Security As New Forum.ModuleSecurity(ModuleID, TabID, objForum.ForumID, UserID)
				If Not objForum.PublicView And objForum.IsActive Then
					If Security.IsAllowedToViewPrivateForum And objForum.IsActive Then
						If NoLinkForums Then
							If Not (objForum.ForumType = ForumType.Link) Then
								arrAuthForums.Add(objForum)
							End If
						Else
							arrAuthForums.Add(objForum)
						End If
					End If
				ElseIf objForum.IsActive Then
					'We handle non-private seperately because module security (core) handles the rest
					If NoLinkForums Then
						If Not (objForum.ForumType = ForumType.Link) Then
							arrAuthForums.Add(objForum)
						End If
					Else
						arrAuthForums.Add(objForum)
					End If
				End If
			Next

			Return arrAuthForums
		End Function

#End Region

		''' <summary>
		''' Get all Groups based on moduleId. Cache this when possible.
		''' </summary>
		''' <param name="ModuleId">The ModuleID being used to retrieve all groups.</param>
		''' <returns>An arraylist of all groups corresponding to the supplied ModuleID.</returns>
		''' <remarks>
		''' </remarks>
		Public Function GroupsGetByModuleID(ByVal ModuleId As Integer) As List(Of GroupInfo)
			Dim strCacheKey As String = GroupColCacheKeyPrefix & ModuleId.ToString()
			Dim arrGroups As New List(Of GroupInfo)
			arrGroups = CType(DataCache.GetCache(strCacheKey), List(Of GroupInfo))

			If arrGroups Is Nothing Then
				Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)
				arrGroups = CBO.FillCollection(Of GroupInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GroupGetByModuleID(ModuleId))

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
		Public Function GetGroupInfo(ByVal GroupID As Integer) As GroupInfo
			Dim strCacheKey As String = GroupCacheKeyPrefix & CStr(GroupID)
			Dim objGroup As New GroupInfo
			objGroup = CType(DataCache.GetCache(strCacheKey), GroupInfo)

			If objGroup Is Nothing Then
				Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)
				objGroup = GroupGet(GroupID)

				If timeOut > 0 And objGroup IsNot Nothing Then
					DataCache.SetCache(strCacheKey, objGroup, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return objGroup
		End Function

		''' <summary>
		''' Used to clear the group cache. (All groups are cached)
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Friend Shared Sub ResetModuleGroups(ByVal ModuleID As Integer)
			Dim strCacheKey As String = GroupColCacheKeyPrefix & ModuleID.ToString()
			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' Resets the group info in cache. 
		''' </summary>
		''' <param name="GroupID"></param>
		''' <remarks></remarks>
		Friend Shared Sub ResetGroupCacheItem(ByVal GroupID As Integer)
			Dim strCacheKey As String = GroupCacheKeyPrefix & CStr(GroupID)
			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' Returns all groups with at least one authorized forum.
		''' </summary>
		''' <param name="ModuleId">Integer</param>
		''' <param name="UserID">The UserID to return results for. (Because of private permissions)</param>
		''' <param name="NoLinkForums">True if Link Type forums should be excluded.</param>
		''' <returns>A Generics arraylist of GroupInfo items.</returns>
		''' <remarks>
		''' </remarks>
		Public Function GroupGetAllAuthorized(ByVal ModuleId As Integer, ByVal UserID As Integer, ByVal NoLinkForums As Boolean, ByVal TabID As Integer) As List(Of GroupInfo)
			Dim arrAllGroups As New List(Of GroupInfo)
			Dim arrAuthGroups As New List(Of GroupInfo)
			arrAllGroups = GroupsGetByModuleID(ModuleId)

			If arrAllGroups.Count > 0 Then
				For Each objGroup As GroupInfo In arrAllGroups
					Dim arrAuthForums As New List(Of ForumInfo)
					arrAuthForums = AuthorizedForums(UserID, objGroup.GroupID, NoLinkForums, ModuleId, TabID)
					If arrAuthForums.Count > 0 Then
						arrAuthGroups.Add(objGroup)
					End If
				Next
			End If

			Return arrAuthGroups
		End Function

		''' <summary>
		''' Gets a single group
		''' </summary>
		''' <param name="GroupId">Integer</param>
		''' <returns>GroupInfo</returns>
		''' <remarks>
		''' </remarks>
		Public Function GroupGet(ByVal GroupId As Integer) As GroupInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().GroupGet(GroupId), GetType(GroupInfo)), GroupInfo)
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
		''' Delete's a forum group 
		''' (Should not happen when there are forums within it)
		''' </summary>
		''' <param name="GroupID">The PK value associated with the Group.</param>
		''' <param name="ModuleID">The module instance the group is tied to.</param>
		''' <remarks>
		''' </remarks>
		Public Sub GroupDelete(ByVal GroupID As Integer, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().GroupDelete(GroupID, ModuleID)
		End Sub

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
		End Sub

		''' <summary>
		''' Updates the view order of the groups (top to bottom) - Moves up
		''' or down one level
		''' </summary>
		''' <param name="GroupID">Integer</param>
		''' <param name="MoveUp">Boolean</param>
		''' <remarks>
		''' </remarks>
		Public Sub GroupSortOrderUpdate(ByVal GroupID As Integer, ByVal MoveUp As Boolean)
			DotNetNuke.Modules.Forum.DataProvider.Instance().GroupSortOrderUpdate(GroupID, MoveUp)
		End Sub

#End Region

	End Class

End Namespace