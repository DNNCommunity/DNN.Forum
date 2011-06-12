'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2011
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

Namespace DotNetNuke.Modules.Forum.Controls

	''' <summary>
	''' The ForumPermissionsGrid is a replication of the core permissions grid. 
	''' This allows the forums to maintain another level of security (which is more granular)
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' </history>
	Public Class ForumPermissionsGrid
		Inherits PermissionsGrid

#Region "Private Members"

		Private _PublicView As Boolean = True
		Private _PublicPosting As Boolean = True
		Private _NotificationForum As Boolean = False
		Private _ForumModerated As Boolean = False
		Private _ModuleID As Integer = -1
		Private _ForumID As Integer = -1
		Private _TabID As Integer = -1
		Private _EnableAttachments As Boolean = False
		' Perms Collections
		Private ForumPermissions As ForumPermissionCollection
		Private _ModulePermissions As Security.Permissions.ModulePermissionCollection
		' Columns
		Private _ViewColumnIndex As Integer
		Private _StartThreadColumnIndex As Integer
		Private _PostReplyColumnIndex As Integer
		Private _NotificationColumnIndex As Integer
		Private _AddAttachmentColumnIndex As Integer
		Private _PinThreadColumnIndex As Integer
		Private _LockThreadColumnIndex As Integer
		Private _ModeratorColumnIndex As Integer
		Private _UnmoderatedColumnIndex As Integer
		' Module Perms
		Private HasModuleForumAdminPermission As Boolean
		Private HasModuleGlobalModPermission As Boolean
		Private HasModuleEditPermission As Boolean
		Private HasModuleViewPermission As Boolean
		Private Const _HasForumAdminPermission As String = "FORUMADMIN"
		Private Const _HasGlobalModPermission As String = "FORUMGLBMOD"
		Private Const _HasModuleViewPermission As String = "VIEW"
		Private Const _HasModuleEditPermission As String = "EDIT"
		' Per Forum Perms
		Private HasPrivateViewPerms As Boolean
		Private HasForumModeratorPerms As Boolean
		Private HasUnmoderatedPerms As Boolean
		Private Const _HasPrivateViewPerms As String = "VIEW"
		Private Const _HasForumModeratorPerms As String = "MODERATE"
		Private Const _HasUnmoderatedPerms As String = "UNMODERATED"

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets/Sets if the forum allows public viewing (not private). 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PublicView() As Boolean
			Get
				Return _PublicView
			End Get
			Set(ByVal Value As Boolean)
				_PublicView = Value
			End Set
		End Property

		''' <summary>
		''' Gets/Sets if the forum has posting restrictions beyond that of the module view permissions. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PublicPosting() As Boolean
			Get
				Return _PublicPosting
			End Get
			Set(ByVal Value As Boolean)
				_PublicPosting = Value
			End Set
		End Property

		''' <summary>
		''' Gets/Sets if the forum is a notification type of forum.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property NotificationForum() As Boolean
			Get
				Return _NotificationForum
			End Get
			Set(ByVal Value As Boolean)
				_NotificationForum = Value
			End Set
		End Property

		''' <summary>
		''' Gets/Sets if the forum is moderated.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumModerated() As Boolean
			Get
				Return _ForumModerated
			End Get
			Set(ByVal Value As Boolean)
				_ForumModerated = Value
			End Set
		End Property

		''' <summary>
		''' Gets/Sets the Id of the Module this forum is a part of.
		''' </summary>
		''' <history>
		''' </history>
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal Value As Integer)
				_ModuleID = Value
				If Not Page.IsPostBack Then
					GetForumPermissions()
				End If
			End Set
		End Property

		''' <summary>
		''' Gets/Sets the TabID the module is on.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TabID() As Integer
			Get
				Return _TabID
			End Get
			Set(ByVal Value As Integer)
				_TabID = Value
			End Set
		End Property

		''' <summary>
		''' Gets/Sets if the forum allows attachments.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnableAttachments() As Boolean
			Get
				Return _EnableAttachments
			End Get
			Set(ByVal Value As Boolean)
				_EnableAttachments = Value
			End Set
		End Property

		''' <summary>
		''' The module permissions collection associated w/ the moduleID.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModulePermissions() As DotNetNuke.Security.Permissions.ModulePermissionCollection
			Get
				Return _ModulePermissions
			End Get
			Set(ByVal Value As DotNetNuke.Security.Permissions.ModulePermissionCollection)
				_ModulePermissions = Value
			End Set
		End Property

		''' <summary>
		''' Gets/Sets the ForumID. 
		''' </summary>
		''' <value></value>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Public Property ForumID() As Integer
			Get
				Return _ForumID
			End Get
			Set(ByVal Value As Integer)
				_ForumID = Value
				If Not Page.IsPostBack Then
					GetForumPermissions()
				End If
			End Set
		End Property

		''' <summary>
		''' Gets the ForumPermission Collection
		''' </summary>
		''' <history>
		''' </history>
		Public ReadOnly Property Permissions() As ForumPermissionCollection
			Get
				'First Update Permissions in case they have been changed
				UpdatePermissions()

				'Return the ForumPermissions
				Return ForumPermissions
			End Get
		End Property

#End Region

#Region "Private Methods"

		''' <summary>
		''' Gets the ForumPermissions from the Data Store
		''' </summary>
		Private Sub GetForumPermissions()
			Dim objForumPermissionController As New ForumPermissionController
			ForumPermissions = objForumPermissionController.GetForumPermissionsCollection(Me.ForumID)
		End Sub

		''' <summary>
		''' Parse the Permission Keys used to persist the Permissions in the ViewState
		''' </summary>
		''' <param name="Settings">A string array of settings</param>
		''' <param name="arrPermisions">An Arraylist to add the Permission object to</param>
		''' <remarks>The array index is based on the BuildKey method in PermissionsGrid.vb.</remarks>
		Private Sub ParseForumPermissionKeys(ByVal Settings As String(), ByVal arrPermisions As ArrayList)
			Dim objForumPermission As ForumPermissionInfo

			objForumPermission = New ForumPermissionInfo
			objForumPermission.PermissionID = Convert.ToInt32(Settings(1))
			objForumPermission.RoleID = Convert.ToInt32(Settings(4))

			If Settings(2) = String.Empty Then
				objForumPermission.ForumPermissionID = -1
			Else
				objForumPermission.ForumPermissionID = Convert.ToInt32(Settings(2))
			End If
			objForumPermission.RoleName = Settings(3)
			objForumPermission.AllowAccess = True
			objForumPermission.UserID = Convert.ToInt32(Settings(5))
			objForumPermission.DisplayName = Settings(6)
			objForumPermission.ModuleID = ModuleID
			objForumPermission.ForumID = ForumID
			arrPermisions.Add(objForumPermission)
		End Sub

		''' <summary>
		''' Parse the module permission keys used to persist the permissions in the viewstate.
		''' </summary>
		''' <param name="Settings">A string array of settings</param>
		''' <param name="arrPermisions">An Arraylist to add the Permission object to.</param>
		''' <remarks>The array index is based on the BuildKey method in PermissionsGrid.vb.</remarks>
		Private Sub ParseModulePermissionKeys(ByVal Settings As String(), ByVal arrPermisions As ArrayList)
			Dim objModulePermission As Permissions.ModulePermissionInfo

			objModulePermission = New Permissions.ModulePermissionInfo
			objModulePermission.PermissionID = Convert.ToInt32(Settings(1))
			objModulePermission.RoleID = Convert.ToInt32(Settings(4))

			If Settings(2) = String.Empty Then
				objModulePermission.ModulePermissionID = -1
			Else
				objModulePermission.ModulePermissionID = Convert.ToInt32(Settings(2))
			End If
			objModulePermission.RoleName = Settings(3)
			objModulePermission.AllowAccess = True
			' CP - Next two commented out until 4.5 Core - Not supported before this
			'objModulePermission.UserID = Convert.ToInt32(Settings(5))
			'objModulePermission.DisplayName = Settings(6)
			objModulePermission.ModuleID = ModuleID
			'objModulePermission.ModuleDefID = ModuleDefID
			arrPermisions.Add(objModulePermission)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PermissionID"></param>
		''' <param name="RoleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function ForumHasRolePermission(ByVal PermissionID As Integer, ByVal RoleID As Integer) As ForumPermissionInfo
			Dim i As Integer
			If Not ForumPermissions Is Nothing Then
				For i = 0 To ForumPermissions.Count - 1
					Dim objForumPermission As ForumPermissionInfo = ForumPermissions(i)
					If PermissionID = objForumPermission.PermissionID And objForumPermission.RoleID = RoleID Then
						Return objForumPermission
					End If
				Next
			End If
			Return Nothing
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PermissionID"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function ForumHasUserPermission(ByVal PermissionID As Integer, ByVal UserID As Integer) As ForumPermissionInfo
			Dim i As Integer
			For i = 0 To ForumPermissions.Count - 1
				Dim objForumPermission As ForumPermissionInfo = ForumPermissions(i)
				If PermissionID = objForumPermission.PermissionID And objForumPermission.UserID = UserID Then
					Return objForumPermission
				End If
			Next
			Return Nothing
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <param name="objModulePermissions"></param>
		''' <param name="PermissionKey"></param>
		''' <param name="RoleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function HasModulePermission(ByVal objModulePermissions As Security.Permissions.ModulePermissionCollection, ByVal PermissionKey As String, ByVal RoleID As Integer) As Boolean
			Dim m As Security.Permissions.ModulePermissionCollection = objModulePermissions
			Dim i As Integer

			If Not m Is Nothing Then
				For i = 0 To m.Count - 1
					Dim mp As Security.Permissions.ModulePermissionInfo
					mp = m(i)
					If mp.PermissionKey = PermissionKey AndAlso mp.RoleID = RoleID Then
						Return True
					End If
				Next
			End If

			Return False
		End Function

#End Region

#Region "Protected Methods"

		''' <summary>
		''' Gets the Enabled status of the (Role) permission
		''' </summary>
		''' <param name="objPerm">The permission being loaded</param>
		''' <param name="role">The role</param>
		''' <param name="column">The column of the Grid</param>
		Protected Overrides Function GetEnabled(ByVal objPerm As PermissionInfo, ByVal role As Security.Roles.RoleInfo, ByVal column As Integer) As Boolean
			Dim enabled As Boolean

			If column = _ViewColumnIndex Then
				If PublicView Then
					enabled = False
				Else
					enabled = True
				End If
			ElseIf column = _StartThreadColumnIndex Then
				If PublicPosting Then
					enabled = False
				Else
					enabled = True
				End If
			ElseIf column = _PostReplyColumnIndex Then
				If PublicPosting Then
					enabled = False
				Else
					enabled = True
				End If
			ElseIf column = _NotificationColumnIndex Then
				If NotificationForum Then
					enabled = True
				Else
					enabled = False
				End If
			ElseIf column = _PinThreadColumnIndex Then
				enabled = True
			ElseIf column = _LockThreadColumnIndex Then
				enabled = True
			ElseIf column = _ModeratorColumnIndex Then
				enabled = True
			ElseIf column = _AddAttachmentColumnIndex Then
				If EnableAttachments Then
					enabled = True
				End If
			ElseIf column = _UnmoderatedColumnIndex Then
				If ForumModerated Then
					enabled = True
				Else
					enabled = False
				End If
			End If

			Return enabled
		End Function

		''' <summary>
		''' Gets the Enabled status of the (User) permission
		''' </summary>
		''' <param name="objPerm">The permission being loaded</param>
		''' <param name="user">The User</param>
		''' <param name="column">The column of the Grid</param>
		Protected Overrides Function GetEnabled(ByVal objPerm As PermissionInfo, ByVal user As Users.UserInfo, ByVal column As Integer) As Boolean
			Dim enabled As Boolean

			If column = _ViewColumnIndex Then
				enabled = True
			ElseIf column = _StartThreadColumnIndex Then
				enabled = True
			ElseIf column = _PostReplyColumnIndex Then
				enabled = True
			ElseIf column = _NotificationColumnIndex Then
				If NotificationForum Then
					enabled = True
				Else
					enabled = False
				End If
			ElseIf column = _PinThreadColumnIndex Then
				enabled = True
			ElseIf column = _LockThreadColumnIndex Then
				enabled = True
			ElseIf column = _ModeratorColumnIndex Then
				enabled = True
			ElseIf column = _AddAttachmentColumnIndex Then
				If EnableAttachments Then
					enabled = True
				End If
			ElseIf column = _UnmoderatedColumnIndex Then
				If ForumModerated Then
					enabled = True
				Else
					enabled = False
				End If
			End If

			Return enabled
		End Function

		''' <summary>
		''' Gets the Value of the (Role) permission
		''' </summary>
		''' <param name="objPerm">The permission being loaded</param>
		''' <param name="role">The role</param>
		''' <param name="column">The column of the Grid</param>
		''' <returns>A Boolean (True or False)</returns>
		Protected Overrides Function GetPermission(ByVal objPerm As PermissionInfo, ByVal role As Security.Roles.RoleInfo, ByVal column As Integer) As Boolean
			Dim permission As Boolean = False

			' No need to get collection of perms, already in memory
			Dim objForumPermission As ForumPermissionInfo = ForumHasRolePermission(objPerm.PermissionID, role.RoleID)

			If column = _ViewColumnIndex Then
				' RoleHasViewPerms determines if the role/user is allowed to access
				If HasViewPerms(objForumPermission) Then
					permission = RoleHasPerms(role, objForumPermission)
				End If
			ElseIf column = _StartThreadColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					permission = RoleHasPerms(role, objForumPermission)
				End If
			ElseIf column = _PostReplyColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					permission = RoleHasPerms(role, objForumPermission)
				End If
			ElseIf column = _NotificationColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					If NotificationForum Then
						permission = RoleHasPerms(role, objForumPermission)
					Else
						permission = False
					End If
				End If
			ElseIf column = _PinThreadColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					permission = RoleHasPerms(role, objForumPermission)
				End If
			ElseIf column = _LockThreadColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					permission = RoleHasPerms(role, objForumPermission)
				End If
			ElseIf column = _ModeratorColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					permission = RoleHasPerms(role, objForumPermission)
				End If
			ElseIf column = _AddAttachmentColumnIndex Then
				If HasViewPerms(objForumPermission) And EnableAttachments Then
					permission = RoleHasPerms(role, objForumPermission)
				Else
					permission = False
				End If
			ElseIf column = _UnmoderatedColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					If ForumModerated Then
						permission = RoleHasPerms(role, objForumPermission)
					End If
				End If
			End If

			Return permission
		End Function

		''' <summary>
		''' Gets the Value of the (User) permission
		''' </summary>
		''' <param name="objPerm">The permission being loaded</param>
		''' <param name="User">The user</param>
		''' <param name="Column">The column of the Grid</param>
		''' <returns>A Boolean (True or False)</returns>
		Protected Overrides Function GetPermission(ByVal objPerm As PermissionInfo, ByVal User As Entities.Users.UserInfo, ByVal Column As Integer) As Boolean
			Dim Permission As Boolean = False

			' No need to get collection of perms, already in memory
			Dim objForumPermission As ForumPermissionInfo = ForumHasUserPermission(objPerm.PermissionID, User.UserID)

			If Column = _ViewColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					Permission = UserHasPerms(User, objForumPermission)
				End If
			ElseIf Column = _StartThreadColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					Permission = UserHasPerms(User, objForumPermission)
				End If
			ElseIf Column = _PostReplyColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					Permission = UserHasPerms(User, objForumPermission)
				End If
			ElseIf Column = _NotificationColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					If NotificationForum Then
						Permission = UserHasPerms(User, objForumPermission)
					End If
				End If
			ElseIf Column = _PinThreadColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					Permission = UserHasPerms(User, objForumPermission)
				End If
			ElseIf Column = _LockThreadColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					Permission = UserHasPerms(User, objForumPermission)
				End If
			ElseIf Column = _ModeratorColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					Permission = UserHasPerms(User, objForumPermission)
				End If
			ElseIf Column = _AddAttachmentColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					Permission = UserHasPerms(User, objForumPermission)
				End If
			ElseIf Column = _UnmoderatedColumnIndex Then
				If HasViewPerms(objForumPermission) Then
					If ForumModerated Then
						Permission = UserHasPerms(User, objForumPermission)
					End If
				End If
			End If

			Return Permission
		End Function

		''' <summary>
		''' If a role has view permission in a private forum.
		''' </summary>
		''' <param name="objForumPermission"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function HasViewPerms(ByVal objForumPermission As ForumPermissionInfo) As Boolean
			Dim allowed As Boolean = False

			If PublicView Then
				allowed = True
			Else
				If Not objForumPermission Is Nothing Then
					allowed = objForumPermission.AllowAccess
				End If
			End If

			Return allowed
		End Function

		''' <summary>
		''' Used to determine if a role in the role permissions grid has permissions for a column.
		''' </summary>
		''' <param name="role">The role to check permission for.</param>
		''' <param name="objForumPermission"></param>
		''' <returns>True if the role has permission, false otherwise.</returns>
		''' <remarks></remarks>
		Private Function RoleHasPerms(ByVal role As Security.Roles.RoleInfo, ByVal objForumPermission As ForumPermissionInfo) As Boolean
			HasModuleForumAdminPermission = HasModulePermission(ModulePermissions, _HasForumAdminPermission, role.RoleID)
			HasModuleGlobalModPermission = HasModulePermission(ModulePermissions, _HasGlobalModPermission, role.RoleID)
			HasModuleEditPermission = HasModulePermission(ModulePermissions, _HasModuleEditPermission, role.RoleID)
			HasModuleViewPermission = HasModulePermission(ModulePermissions, _HasModuleViewPermission, role.RoleID)
			Dim allowed As Boolean = False

			' If they can view the forum (which we know if this method is called) these roles are capable of doing everything else in that forum
			If role.RoleID = AdministratorRoleId Or HasModuleForumAdminPermission Or HasModuleGlobalModPermission Or HasModuleEditPermission Then
				allowed = True
			Else
				If Not ForumPermissions Is Nothing Then
					HasForumModeratorPerms = RoleCanAccessByKey(ForumPermissions, _HasForumModeratorPerms, role)
					If HasForumModeratorPerms Then
						allowed = True
					Else
						If Not objForumPermission Is Nothing Then
							allowed = objForumPermission.AllowAccess
						End If
					End If
				End If
			End If
			Return allowed
		End Function

		''' <summary>
		''' Used to determine if a user in the user permissions grid has permissions for a column
		''' </summary>
		''' <param name="user">The specified User.</param>
		''' <param name="objForumPermission">A ForumPermissionInfo object</param>
		''' <returns>True if the user has permission, false otherwise.</returns>
		''' <remarks></remarks>
		Private Function UserHasPerms(ByVal user As Entities.Users.UserInfo, ByVal objForumPermission As ForumPermissionInfo) As Boolean
			HasModuleForumAdminPermission = HasModulePermission(ModulePermissions, _HasForumAdminPermission, user.UserID)
			HasModuleGlobalModPermission = HasModulePermission(ModulePermissions, _HasGlobalModPermission, user.UserID)
			HasModuleEditPermission = HasModulePermission(ModulePermissions, _HasModuleEditPermission, user.UserID)
			HasModuleViewPermission = HasModulePermission(ModulePermissions, _HasModuleViewPermission, user.UserID)
			Dim allowed As Boolean = False

			If Not ForumPermissions Is Nothing Then
				HasForumModeratorPerms = UserCanAccessByKey(ForumPermissions, _HasForumModeratorPerms, user)
				If HasForumModeratorPerms Then
					allowed = True
				Else
					If Not objForumPermission Is Nothing Then
						allowed = objForumPermission.AllowAccess
					End If
				End If
			End If
			'End If
			Return allowed
		End Function

		''' <summary>
		''' Determines if a specific role has access.
		''' </summary>
		''' <param name="fpc"></param>
		''' <param name="Key"></param>
		''' <param name="role"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function RoleCanAccessByKey(ByVal fpc As ForumPermissionCollection, ByVal Key As String, ByVal role As Security.Roles.RoleInfo) As Boolean
			Dim allowed As Boolean = False
			Dim count As Integer

			' Now loop through the collection, see if the current role has  perms
			For count = 0 To fpc.Count - 1
				Dim fp As ForumPermissionInfo
				fp = fpc(count)
				If fp.PermissionKey = Key AndAlso fp.RoleName = role.RoleName Then
					allowed = True
				End If
			Next

			Return allowed
		End Function

		''' <summary>
		''' Determines if a specific user has access. 
		''' </summary>
		''' <param name="fpc"></param>
		''' <param name="Key"></param>
		''' <param name="user"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function UserCanAccessByKey(ByVal fpc As ForumPermissionCollection, ByVal Key As String, ByVal user As Entities.Users.UserInfo) As Boolean
			Dim allowed As Boolean = False
			Dim count As Integer

			' Now loop through the collection, see if the current role has  perms
			For count = 0 To fpc.Count - 1
				Dim fp As ForumPermissionInfo
				fp = fpc(count)
				If fp.PermissionKey = Key AndAlso fp.UserID = user.UserID Then
					allowed = True
				End If
			Next

			Return allowed
		End Function

		''' <summary>
		''' Gets the Permissions from the Data Store
		''' </summary>
		''' <history>
		''' </history>
		Protected Overrides Function GetPermissions() As ArrayList
			Dim objPermissionController As New ForumPermissionController
			Dim arrPermissions As ArrayList = objPermissionController.GetPermissionsByForumID(Me.ForumID)

			Dim i As Integer
			For i = 0 To arrPermissions.Count - 1
				Dim objPermission As PermissionInfo
				objPermission = CType(arrPermissions(i), PermissionInfo)
				If objPermission.PermissionKey = "VIEW" Then
					_ViewColumnIndex = i + 1
				ElseIf objPermission.PermissionKey = "STARTTHREAD" Then
					_StartThreadColumnIndex = i + 1
				ElseIf objPermission.PermissionKey = "POSTREPLY" Then
					_PostReplyColumnIndex = i + 1
				ElseIf objPermission.PermissionKey = "NOTIFICATION" Then
					_NotificationColumnIndex = i + 1
				ElseIf objPermission.PermissionKey = "ADDATTACHMENT" Then
					_AddAttachmentColumnIndex = i + 1
				ElseIf objPermission.PermissionKey = "PINTHREAD" Then
					_PinThreadColumnIndex = i + 1
				ElseIf objPermission.PermissionKey = "LOCKTHREAD" Then
					_LockThreadColumnIndex = i + 1
				ElseIf objPermission.PermissionKey = "MODERATE" Then
					_ModeratorColumnIndex = i + 1
				ElseIf objPermission.PermissionKey = "UNMODERATED" Then
					_UnmoderatedColumnIndex = i + 1
				End If
			Next

			Return arrPermissions
		End Function

		''' <summary>
		''' Gets the users from the Database
		''' </summary>
		Protected Overrides Function GetUsers() As ArrayList
			Dim arrUsers As New ArrayList
			Dim objForumPermission As ForumPermissionInfo
			Dim objUser As Users.UserInfo
			Dim blnExists As Boolean

			If Not ForumPermissions Is Nothing Then
				For Each objForumPermission In ForumPermissions
					If Not Null.IsNull(objForumPermission.UserID) Then
						blnExists = False
						For Each objUser In arrUsers
							If objForumPermission.UserID = objUser.UserID Then
								blnExists = True
							End If
						Next
						If Not blnExists Then
							objUser = New Users.UserInfo
							objUser.UserID = objForumPermission.UserID
							objUser.Username = objForumPermission.Username
							objUser.DisplayName = objForumPermission.DisplayName
							arrUsers.Add(objUser)
						End If
					End If
				Next
			End If

			Return arrUsers
		End Function

		''' <summary>
		''' Load the ViewState
		''' </summary>
		''' <param name="savedState">The saved state</param>
		''' <history>
		''' </history>
		Protected Overrides Sub LoadViewState(ByVal savedState As Object)
			If Not (savedState Is Nothing) Then
				' Load State from the array of objects that was saved with SaveViewState.

				Dim myState As Object() = CType(savedState, Object())

				'Load Base Controls ViewStte
				If Not (myState(0) Is Nothing) Then
					MyBase.LoadViewState(myState(0))
				End If

				'Load ForumID
				If Not (myState(1) Is Nothing) Then
					ForumID = CInt(myState(1))
				End If

				'Load PublicView
				If Not (myState(2) Is Nothing) Then
					PublicView = CBool(myState(2))
				End If

				'Load the PublicPosting
				If Not (myState(3) Is Nothing) Then
					PublicPosting = CBool(myState(3))
				End If

				'Load the NotificationForum
				If Not (myState(4) Is Nothing) Then
					NotificationForum = CBool(myState(4))
				End If

				'Load EnableAttachments
				If Not (myState(5) Is Nothing) Then
					EnableAttachments = CBool(myState(5))
				End If

				' Load Per User Perms
				If Not (myState(6) Is Nothing) Then
					ForumModerated = CBool(myState(6))
				End If

				'Load ModulePermissions
				If Not (myState(7) Is Nothing) Then
					Dim arrPermissions As New ArrayList
					Dim state As String = CStr(myState(7))
					If state <> String.Empty Then
						'First Break the String into individual Keys
						Dim permissionKeys As String() = Split(state, "##")
						For Each key As String In permissionKeys
							Dim Settings As String() = Split(key, "|")
							ParseModulePermissionKeys(Settings, arrPermissions)
						Next
					End If
					ModulePermissions = New DotNetNuke.Security.Permissions.ModulePermissionCollection(arrPermissions)
				End If

				'Load ForumPermissions
				If Not (myState(8) Is Nothing) Then
					Dim arrPermissions As New ArrayList
					Dim state As String = CStr(myState(8))
					If state <> String.Empty Then
						'First Break the String into individual Keys
						Dim permissionKeys As String() = Split(state, "##")
						For Each key As String In permissionKeys
							Dim Settings As String() = Split(key, "|")
							ParseForumPermissionKeys(Settings, arrPermissions)
						Next
					End If
					ForumPermissions = New ForumPermissionCollection(arrPermissions)
				End If
			End If
		End Sub

		''' <summary>
		''' Saves the ViewState
		''' </summary>
		''' <history>
		''' </history>
		Protected Overrides Function SaveViewState() As Object
			Dim allStates(8) As Object

			' Save the Base Controls ViewState
			allStates(0) = MyBase.SaveViewState()

			'Persist ForumID
			allStates(1) = ForumID

			'Persist PublicView
			allStates(2) = PublicView

			'Persist PublicPosting
			allStates(3) = PublicPosting

			'Persist NotificationForum
			allStates(4) = NotificationForum

			'Persist EnableAttachments
			allStates(5) = EnableAttachments

			'Persist ForumModerated
			allStates(6) = ForumModerated

			'Persist the ModulePermissions
			Dim sb2 As New StringBuilder
			Dim addDelimiter2 As Boolean = False

			Dim objModulePermission As New DotNetNuke.Security.Permissions.ModulePermissionInfo

			If Not objModulePermission Is Nothing Then
				For Each objModulePermission In ModulePermissions
					If addDelimiter2 Then
						sb2.Append("##")
					Else
						addDelimiter2 = True
					End If
					' CP - This will change post 4.5 core support 
					sb2.Append(BuildKey(objModulePermission.AllowAccess, objModulePermission.PermissionID, objModulePermission.ModulePermissionID, objModulePermission.RoleID, objModulePermission.RoleName))
				Next
				allStates(7) = sb2.ToString()
			End If

			'Persist the ModulePermissions
			Dim sb As New StringBuilder
			Dim addDelimiter As Boolean = False

			If Not ForumPermissions Is Nothing Then
				For Each objForumPermission As ForumPermissionInfo In ForumPermissions
					If addDelimiter Then
						sb.Append("##")
					Else
						addDelimiter = True
					End If
					sb.Append(BuildKey(objForumPermission.AllowAccess, objForumPermission.PermissionID, objForumPermission.ForumPermissionID, objForumPermission.RoleID, objForumPermission.RoleName, objForumPermission.UserID, objForumPermission.DisplayName))
				Next
			End If
			allStates(8) = sb.ToString()

			Return allStates
		End Function

		''' <summary>
		''' Updates a Permission
		''' </summary>
		''' <param name="permission">The permission being updated</param>
		''' <param name="roleName">The name of the role</param>
		''' <param name="roleId">The id of the role</param>
		''' <param name="allowAccess">The value of the permission</param>
		''' <history>
		''' </history>
		Protected Overrides Sub UpdatePermission(ByVal permission As PermissionInfo, ByVal roleid As Integer, ByVal roleName As String, ByVal allowAccess As Boolean)
			Dim isMatch As Boolean = False
			Dim objPermission As ForumPermissionInfo
			Dim permissionId As Integer = permission.PermissionID

			'Search ForumPermission Collection for the permission to Update
			For Each objPermission In ForumPermissions
				If objPermission.PermissionID = permissionId And objPermission.RoleID = roleid Then
					'ForumPermission is in collection
					If Not allowAccess Then
						'Remove from collection as we only keep AllowAccess permissions
						ForumPermissions.Remove(objPermission)
					End If
					isMatch = True
					Exit For
				End If
			Next

			'ForumPermission not found so add new
			If Not isMatch And allowAccess Then
				objPermission = New ForumPermissionInfo
				objPermission.PermissionID = permissionId
				objPermission.ForumID = ForumID
				objPermission.RoleID = roleid
				objPermission.RoleName = roleName
				objPermission.AllowAccess = allowAccess
				objPermission.UserID = Null.NullInteger
				objPermission.DisplayName = Null.NullString

				ForumPermissions.Add(objPermission)
			End If
		End Sub

		''' <summary>
		''' Updates the forum permissions in the data store.
		''' </summary>
		''' <param name="permission"></param>
		''' <param name="displayName"></param>
		''' <param name="userId"></param>
		''' <param name="allowAccess"></param>
		''' <remarks></remarks>
		Protected Overrides Sub UpdatePermission(ByVal permission As PermissionInfo, ByVal displayName As String, ByVal userId As Integer, ByVal allowAccess As Boolean)
			Dim isMatch As Boolean = False
			Dim objPermission As ForumPermissionInfo
			Dim permissionId As Integer = permission.PermissionID

			'Search ForumPermission Collection for the permission to Update
			For Each objPermission In ForumPermissions
				If objPermission.PermissionID = permissionId And objPermission.UserID = userId Then
					'ForumPermission is in collection
					If Not allowAccess Then
						'Remove from collection as we only keep AllowAccess permissions
						ForumPermissions.Remove(objPermission)
					End If
					isMatch = True
					Exit For
				End If
			Next

			'ForumPermission not found so add new
			If Not isMatch And allowAccess Then
				objPermission = New ForumPermissionInfo
				objPermission.PermissionID = permissionId
				objPermission.ForumID = ForumID
				'CP - COMEBACK - This is just until post 4.5 release (core doesn't have this var till then)
				' objPermission.RoleID = Integer.Parse(glbRoleNothing)
				objPermission.RoleID = -4
				objPermission.RoleName = Null.NullString
				objPermission.AllowAccess = allowAccess
				objPermission.UserID = userId
				objPermission.DisplayName = displayName

				ForumPermissions.Add(objPermission)
			End If
		End Sub

		''' <summary>
		''' Addes the forum permissions to the data store.
		''' </summary>
		''' <param name="permissions"></param>
		''' <param name="user"></param>
		''' <remarks></remarks>
		Protected Overrides Sub AddPermission(ByVal permissions As ArrayList, ByVal user As Entities.Users.UserInfo)
			Dim objForumPermission As ForumPermissionInfo
			Dim isMatch As Boolean = False

			For Each objForumPermission In ForumPermissions
				If objForumPermission.UserID = user.UserID Then
					isMatch = True
					Exit For
				End If
			Next

			'user not found so add new
			If Not isMatch Then
				Dim objPermission As PermissionInfo
				For Each objPermission In permissions
					objForumPermission = New ForumPermissionInfo
					objForumPermission.PermissionID = objPermission.PermissionID
					objForumPermission.ForumID = ForumID
					'CP - COMEBACK - This is just until post 4.5 release (core doesn't have this var till then)
					' objPermission.RoleID = Integer.Parse(glbRoleNothing)
					objForumPermission.RoleID = -4
					objForumPermission.RoleName = Null.NullString
					If objPermission.PermissionKey = "VIEW" Then
						objForumPermission.AllowAccess = True
					Else
						objForumPermission.AllowAccess = False
					End If
					objForumPermission.UserID = user.UserID
					objForumPermission.DisplayName = user.DisplayName

					ForumPermissions.Add(objForumPermission)
				Next
			End If
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Overrides the Base method to Generate the Data Grid
		''' </summary>
		''' <history>
		''' </history>
		Public Overrides Sub GenerateDataGrid()

		End Sub

#End Region

	End Class

End Namespace