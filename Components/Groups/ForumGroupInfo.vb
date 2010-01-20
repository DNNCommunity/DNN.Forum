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
	''' A single instance of the GroupInfo Object. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class GroupInfo

#Region "Private Members"

		Private Const GroupInfoCacheKeyPrefix As String = "GroupInfo"
		Private mGroupID As Integer
		Private mName As String
		Private mPortalID As Integer
		Private mModuleID As Integer
		Private mSortOrder As Integer
		Private mCreatedDate As DateTime
		Private mUpdatedDate As DateTime
		Private mCreatedByUser As Integer
		Private mUpdatedByUser As Integer
		'Not part of table
		Private mForumCount As Integer
		Private mActiveForumCount As Integer

#End Region

#Region "Constructors"

		''' <summary>
		''' Instantiats the Group object. 
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Attempts to load the group from cache, if not available it retrieves it and places it in cache. 
		''' </summary>
		''' <param name="GroupID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetGroupInfo(ByVal GroupID As Integer) As GroupInfo
			Dim strCacheKey As String = GroupInfoCacheKeyPrefix & CStr(GroupID)
			Dim objGroup As New GroupInfo
			objGroup = CType(DataCache.GetCache(strCacheKey), GroupInfo)

			If objGroup Is Nothing Then
				Dim cntGroup As New GroupController
				objGroup = cntGroup.GroupGet(GroupID)
				DataCache.SetCache(strCacheKey, objGroup)
			End If

			Return objGroup
		End Function

		''' <summary>
		''' Resets the group info in cache. 
		''' </summary>
		''' <param name="GroupID"></param>
		''' <remarks></remarks>
		Public Shared Sub ResetGroupInfo(ByVal GroupID As Integer)
			Dim strCacheKey As String = GroupInfoCacheKeyPrefix & CStr(GroupID)

			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' Builds a collection of authorized forums for the specified Group.
		''' </summary>
		''' <param name="UserID">The user to return results for. (Permissions)</param>
		''' <param name="NoLinkForums">True if no link type forums should be added to the collection.</param>
		''' <returns>A Generics collection of ForumInfo items.</returns>
		''' <remarks></remarks>
		Public Function AuthorizedForums(ByVal UserID As Integer, ByVal NoLinkForums As Boolean) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.ForumGetAll(GroupID)
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
				Dim Security As New Forum.ModuleSecurity(ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
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
		Public Function AuthorizedNoParentForums(ByVal UserID As Integer, ByVal NoLinkForums As Boolean) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.ForumGetAllByParentID(0, GroupID, True)
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
				Dim Security As New Forum.ModuleSecurity(ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
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
		Public Function AuthorizedSubForums(ByVal UserID As Integer, ByVal NoLinkForums As Boolean, ByVal ParentForumId As Integer) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.ForumGetAllByParentID(ParentForumId, GroupID, True)
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
				Dim Security As New Forum.ModuleSecurity(ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
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

#Region "Public Properties"

		''' <summary>
		''' The GroupID (PK)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property GroupID() As Integer
			Get
				Return mGroupID
			End Get
			Set(ByVal Value As Integer)
				mGroupID = Value
			End Set
		End Property

		''' <summary>
		''' The name of the group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Name() As String
			Get
				Return mName
			End Get
			Set(ByVal Value As String)
				mName = Value
			End Set
		End Property

		''' <summary>
		''' The PortalID the group belongs to. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PortalID() As Integer
			Get
				Return mPortalID
			End Get
			Set(ByVal Value As Integer)
				mPortalID = Value
			End Set
		End Property

		''' <summary>
		''' The moduleID this group is part of. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModuleID() As Integer
			Get
				Return mModuleID
			End Get
			Set(ByVal Value As Integer)
				mModuleID = Value
			End Set
		End Property

		''' <summary>
		''' The date the group was created. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedDate() As DateTime
			Get
				Return mCreatedDate
			End Get
			Set(ByVal Value As DateTime)
				mCreatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The UserID of the person who created the Group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedByUser() As Integer
			Get
				Return mCreatedByUser
			End Get
			Set(ByVal Value As Integer)
				mCreatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' The date the group was last updated. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedDate() As DateTime
			Get
				Return mUpdatedDate
			End Get
			Set(ByVal Value As DateTime)
				mUpdatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The last UserID to update the group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedByUser() As Integer
			Get
				Return mUpdatedByUser
			End Get
			Set(ByVal Value As Integer)
				mUpdatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' The sort order (ascending/descending) the group display in. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property SortOrder() As Integer
			Get
				Return mSortOrder
			End Get
			Set(ByVal Value As Integer)
				mSortOrder = Value
			End Set
		End Property

		''' <summary>
		''' The total number of forums in the group. (includes inactive)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Not stored in table, retrieved in Get Sprocs</remarks>
		Public Property ForumCount() As Integer
			Get
				Return mForumCount
			End Get
			Set(ByVal Value As Integer)
				mForumCount = Value
			End Set
		End Property

		''' <summary>
		''' Number of active forums in the group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Not stored in table, retrieved in Get Sprocs</remarks>
		Public Property ActiveForumCount() As Integer
			Get
				Return mActiveForumCount
			End Get
			Set(ByVal Value As Integer)
				mActiveForumCount = Value
			End Set
		End Property

#End Region

#Region "Public ReadOnly Properties"

		''' <summary>
		''' The modules configuration settings. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property objConfig() As Forum.Config
			Get
				Return Forum.Config.GetForumConfig(ModuleID)
			End Get
		End Property

		''' <summary>
		''' This would allow for multiple Group Levels, not used. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Parent() As GroupInfo
			Get
				If GroupID <> -1 Then
					Return GroupInfo.GetGroupInfo(GroupID)
				Else
					Dim objGroup As GroupInfo = New GroupInfo
					objGroup.ModuleID = ModuleID
					objGroup.GroupID = GroupID
					Return objGroup
				End If
			End Get
		End Property

#End Region

	End Class

End Namespace