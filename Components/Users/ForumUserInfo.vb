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

Imports DotNetNuke.Services.FileSystem

Namespace DotNetNuke.Modules.Forum

#Region "ForumUser"

	''' <summary>
	''' Everything necessary for a Forum User, all based on PortalID.
	''' This means a forum user profile is specific to a portal instance.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class ForumUser
		Inherits DotNetNuke.Entities.Users.UserInfo

#Region "Constructors"

		''' <summary>
		''' Instantiates a new user by ModuleID (ultimately, a user is portal specific)
		''' </summary>
		''' <param name="ModuleId"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal ModuleId As Integer)
			objConfig = Forum.Config.GetForumConfig(ModuleId)
		End Sub

#End Region

#Region "Private Members"

		Dim _PostCount As Integer = 0
		Dim _UserAvatar As Integer = 0
		Dim _Avatar As String = String.Empty
		Dim _SystemAvatars As String = String.Empty
		Dim _RoleAvatars As String = String.Empty
		Dim _Signature As String = String.Empty
		Dim _IsTrusted As Boolean = False
		Dim _EnableThreadTracking As Boolean = False
		Dim _EnableDisplayUnreadThreadsOnly As Boolean = False
		Dim _EnableOnlineStatus As Boolean = True
		Dim _ThreadsPerPage As Integer = -1
		Dim _PostsPerPage As Integer = -1
		Dim _LastActivity As DateTime = Now
		Dim _ViewDescending As Boolean = False
		Dim _EnableModNotification As Boolean = True
		Dim _EmailFormat As Integer
		Dim _SiteAlias As String = "Anonymous"
		Dim _objConfig As Config
		Dim _EnablePublicEmail As Boolean = False
		Dim _AvatarComplete As String = String.Empty
		Dim _SystemAvatarComplete As String = String.Empty
		Dim _RoleAvatarComplete As String = String.Empty
		Dim _LockTrust As Boolean = False
		Dim _IsBanned As Boolean = False
		Dim _LiftBanDate As Date = Date.Now()
		Dim _TrackingDuration As Integer = 1000
		Dim _EnableProfileWeb As Boolean = True
		Dim _EnableDefaultPostNotify As Boolean = True
		Dim _EnableSelfNotifications As Boolean = True
		Dim _TotalRecords As Integer
		Dim _IsDeleted As Boolean = False
		Dim _StartBanDate As Date

#End Region

#Region "Public ReadOnly Properties"

#Region "Core Profile"

		''' <summary>
		''' User profile item of selected timezone. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property UserTimeZone() As Integer
			Get
				Dim objProfile As New Entities.Users.UserProfile

				Return objProfile.TimeZone
			End Get
		End Property

		''' <summary>
		''' userJoinedDate is based on registration date
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property UserJoinedDate() As Date
			Get
				Return Membership.CreatedDate
			End Get
		End Property

		''' <summary>
		''' takes a users website profile item and makes it 'clickable' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property UserWebsite() As String
			Get
				Dim _UserWebsite As String = Me.Profile.Website
				If Not _UserWebsite Is Nothing Then
					If _UserWebsite.Length > 0 AndAlso (Not _UserWebsite.StartsWith("http")) Then
						_UserWebsite = "http://" & _UserWebsite
					End If
				End If

				Return _UserWebsite
			End Get
		End Property

		''' <summary>
		''' The user's last activity, based on the core membership provider.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastActivity() As Date
			Get
				Return Membership.LastActivityDate
			End Get
		End Property

#End Region

		''' <summary>
		''' Sets the users referred to alias depending on forum configuration
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This will be either core username or core displayname</remarks>
		Public ReadOnly Property SiteAlias() As String
			Get
				If objConfig.ForumMemberName = 0 Then
					Return Me.Username
				Else
					Return Me.DisplayName
				End If
			End Get
		End Property

		''' <summary>
		''' list of forums being tracked for notifications by user. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property TrackedForums() As List(Of TrackingInfo)
			Get
				Dim ctlForumTracking As New TrackingController
				Dim lstTracking As List(Of TrackingInfo) = ctlForumTracking.TrackingForumGet(UserID, objConfig.ModuleID)
				Return lstTracking
			End Get
		End Property

		''' <summary>
		''' List of threads being tracked by user for notifications. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property TrackedThreads() As List(Of TrackingInfo)
			Get
				Dim ctlForumTracking As New TrackingController
				Dim lstTracking As List(Of TrackingInfo) = ctlForumTracking.TrackingThreadGet(UserID, objConfig.ModuleID)
				Return lstTracking
			End Get
		End Property

		''' <summary>
		''' The full path to the user's avatar for display, unless using core profile avatar (this returns fileid in that case). 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property AvatarComplete() As String
			Get
				Try
					If Not objConfig.CurrentPortalSettings Is Nothing Then
						Dim homeDirectory As String = objConfig.CurrentPortalSettings.HomeDirectory

						If objConfig.EnableProfileAvatar Then
							If ProfileAvatar IsNot Nothing Then
								If objConfig.EnableProfileUserFolders And (AvatarCoreFile IsNot Nothing) Then
									Dim userFolder As String = GetUserFolderPath(UserID)

									_AvatarComplete = homeDirectory + "Users/" + userFolder + "/" + AvatarCoreFile.FileName
								Else
									_AvatarComplete = homeDirectory + objConfig.UserAvatarPath + ProfileAvatar
								End If
							Else
								_AvatarComplete = String.Empty
							End If
						Else
							If _Avatar.Trim(";"c) <> String.Empty Then
								'[skeel] RETURN!! need to add none here as well, as UserAvatar has not been used before, 
								'even if it was there - eventually it has to go..
								If _UserAvatar = UserAvatarType.UserAvatar Or _UserAvatar = UserAvatarType.None Then
									_AvatarComplete = homeDirectory + objConfig.UserAvatarPath + _Avatar.Trim(";"c)
								ElseIf _UserAvatar = UserAvatarType.PoolAvatar Then
									_AvatarComplete = homeDirectory + objConfig.UserAvatarPoolPath + _Avatar.Trim(";"c)
								End If
							End If
						End If
						Return _AvatarComplete
					Else
						' We don't have context
						Return Nothing
					End If
				Catch ex As Exception
					LogException(ex)
					Return Nothing
				End Try
			End Get
		End Property

#Region "Special Core code duplication, this is for user folders because if Int16 bug in 5.4.0, 5.4.1."

		Private Enum EnumUserFolderElement
			Root = 0
			SubFolder = 1
		End Enum

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Returns path to a User Folder 
		''' </summary>
		''' <history>
		''' 	[jlucarino]	03/01/2010	Created
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Shared Function GetUserFolderPath(ByVal UserID As Integer) As String
			Dim RootFolder As String
			Dim SubFolder As String
			Dim FullPath As String

			RootFolder = GetUserFolderPathElement(UserID, EnumUserFolderElement.Root)
			SubFolder = GetUserFolderPathElement(UserID, EnumUserFolderElement.SubFolder)

			FullPath = System.IO.Path.Combine(RootFolder, SubFolder)
			FullPath = System.IO.Path.Combine(FullPath, UserID.ToString)

			Return FullPath
		End Function

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Returns Root and SubFolder elements of User Folder path
		''' </summary>
		''' <history>
		''' 	[jlucarino]	03/01/2010	Created
		''' </history>
		''' -----------------------------------------------------------------------------
		Private Shared Function GetUserFolderPathElement(ByVal UserID As Integer, ByVal Mode As EnumUserFolderElement) As String
			Const SUBFOLDER_SEED_LENGTH As Integer = 2
			Const BYTE_OFFSET As Integer = 255
			Dim Element As String = ""

			Select Case Mode
				Case EnumUserFolderElement.Root
					Element = (Convert.ToInt32(UserID) And BYTE_OFFSET).ToString("000")
				Case EnumUserFolderElement.SubFolder
					Element = UserID.ToString("00").Substring(UserID.ToString("00").Length - SUBFOLDER_SEED_LENGTH, SUBFOLDER_SEED_LENGTH)
			End Select

			Return Element
		End Function

#End Region

		''' <summary>
		''' This results in a core FileInfo object associated with a fileID provided via ProfileAvatar. 
		''' </summary>
		''' <value></value>
		''' <returns>A file stored in the DotNetNuke File system (ie. Files/Folders tables).</returns>
		''' <remarks>This is utilized for GenerateThumbnail method, stored here since we cache it and avoid the call multiple times.</remarks>
		Public ReadOnly Property AvatarCoreFile() As FileInfo
			Get
				If ProfileAvatar IsNot Nothing Then
					Try
						Dim FileID As Integer = CInt(ProfileAvatar.Trim())
						Dim objController As New FileController()

						Return objController.GetFileById(FileID, PortalID)
					Catch ex As Exception
						LogException(ex)
						Return Nothing
					End Try
				Else
					Return Nothing
				End If
			End Get
		End Property

		''' <summary>
		''' The full path to the user's admin assigned avatars for display. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property SystemAvatarsComplete() As String
			Get
				Dim avatars() As String = _SystemAvatars.Trim(";"c).Split(";"c)
				_SystemAvatarComplete = String.Empty

				For Each avt As String In avatars
					If avt.Trim(";"c) <> String.Empty Then
						_SystemAvatarComplete += objConfig.CurrentPortalSettings.HomeDirectory + objConfig.SystemAvatarPath + avt + ";"
					End If
				Next

				Return _SystemAvatarComplete
			End Get
		End Property

		''' <summary>
		''' The full path to the user's admin assigned rolebased avatar for display. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property RoleAvatarComplete() As String
			Get
				If _RoleAvatarComplete = String.Empty Then
					Dim avatars() As String = _RoleAvatars.Trim(";"c).Split(";"c)
					For Each avt As String In avatars
						If avt.Trim(";"c) <> String.Empty Then
							_RoleAvatarComplete += objConfig.CurrentPortalSettings.HomeDirectory + objConfig.RoleAvatarPath + avt + ";"
						End If
					Next
				End If

				Return _RoleAvatarComplete
			End Get
		End Property

		''' <summary>
		''' If the user is online or not. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property IsOnline() As Boolean
			Get
				Dim ctlUserOnline As New Users.UserOnlineController
				Dim userList As Hashtable = ctlUserOnline.GetUserList
				If (Not userList Is Nothing) AndAlso (userList.Count > 0) Then
					If Not userList(UserID.ToString) Is Nothing And EnableOnlineStatus Then
						Return True
					End If
				End If
				Return False
			End Get
		End Property

		''' <summary>
		''' Gets the avatar from the user's profile (which is stored in the UserProfile table). 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This can be stored as a fileid (as a string) or as an actual file name.</remarks>
		Public ReadOnly Property ProfileAvatar() As String
			Get
				If Not Me.IsSuperUser Then
					' we are using profile avatars, lets check for the property value
					Return Me.Profile.GetPropertyValue(objConfig.AvatarProfilePropName)
				Else
					Return Nothing
				End If
			End Get
		End Property

		''' <summary>
		''' This returns the core profile page, available as of DNN Core 5.3. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>We store this here because it never changes, once set, and is cached (vs. repeatededly calculating the path).</remarks>
		Public ReadOnly Property UserCoreProfileLink() As String
			Get
				Return DotNetNuke.Common.Globals.UserProfileURL(UserID)
			End Get
		End Property

#End Region

#Region "Public Properties"

		''' <summary>
		''' Module configuration settings. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property objConfig() As Forum.Config
			Get
				Return _objConfig
			End Get
			Set(ByVal Value As Config)
				_objConfig = Value
			End Set
		End Property

		''' <summary>
		''' If the user will view posts ascending or descending. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ViewDescending() As Boolean
			Get
				Return _ViewDescending
			End Get
			Set(ByVal Value As Boolean)
				_ViewDescending = Value
			End Set
		End Property

		''' <summary>
		''' Determines the type of user avatar image assigned. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UserAvatar() As Integer
			Get
				Return _UserAvatar
			End Get
			Set(ByVal Value As Integer)
				_UserAvatar = Value
			End Set
		End Property

		''' <summary>
		''' Name of the user's avatar image. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Avatar() As String
			Get
				Return _Avatar
			End Get
			Set(ByVal Value As String)
				_Avatar = Value
			End Set
		End Property

		''' <summary>
		''' String of admin assigned avatars. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property SystemAvatars() As String
			Get
				Return _SystemAvatars
			End Get
			Set(ByVal Value As String)
				_SystemAvatars = Value
			End Set
		End Property

		''' <summary>
		''' String of admin assigned rolebased avatars. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property RoleAvatar() As String
			Get
				If _RoleAvatars = String.Empty Then
					Dim ctlRoleAvatar As New RoleAvatarController
					_RoleAvatars = ctlRoleAvatar.GetUsersRoleAvatars(UserID, objConfig.ModuleID, PortalID)
				End If
				Return _RoleAvatars

			End Get
			Set(ByVal Value As String)
				_RoleAvatars = Value
			End Set
		End Property

		''' <summary>
		''' Total number of approved posts the user has contributed. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostCount() As Integer
			Get
				Return _PostCount
			End Get
			Set(ByVal Value As Integer)
				_PostCount = Value
			End Set
		End Property

		''' <summary>
		''' The user's signature in their forum profile. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Signature() As String
			Get
				Return _Signature
			End Get
			Set(ByVal Value As String)
				_Signature = Value
			End Set
		End Property

		''' <summary>
		''' If the user is moderated, they are not 'trusted' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsTrusted() As Boolean
			Get
				Return _IsTrusted
			End Get
			Set(ByVal Value As Boolean)
				_IsTrusted = Value
			End Set
		End Property

		''' <summary>
		''' If the user allows their online status to be displayed. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnableOnlineStatus() As Boolean
			Get
				Return _EnableOnlineStatus
			End Get
			Set(ByVal Value As Boolean)
				_EnableOnlineStatus = Value
			End Set
		End Property

		''' <summary>
		''' Number of threads per page shown to a user at once. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ThreadsPerPage() As Integer
			Get
				If Not _ThreadsPerPage > 0 Then
					Return objConfig.ThreadsPerPage
				Else
					Return _ThreadsPerPage
				End If
			End Get
			Set(ByVal Value As Integer)
				_ThreadsPerPage = Value
			End Set
		End Property

		''' <summary>
		''' Number of posts per page shown to a user at once. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostsPerPage() As Integer
			Get
				If Not _PostsPerPage > 0 Then
					Return objConfig.PostsPerPage
				Else
					Return _PostsPerPage
				End If
			End Get
			Set(ByVal Value As Integer)
				_PostsPerPage = Value
			End Set
		End Property

		''' <summary>
		''' If the user wants to receive emails about moderation issues. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnableModNotification() As Boolean
			Get
				Return _EnableModNotification
			End Get
			Set(ByVal Value As Boolean)
				_EnableModNotification = Value
			End Set
		End Property

		''' <summary>
		''' Text/HTML preference for email notifications. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Text = 0, HTML = 1</remarks>
		Public Property EmailFormat() As Integer
			Get
				Return _EmailFormat
			End Get
			Set(ByVal Value As Integer)
				_EmailFormat = Value
			End Set
		End Property

		''' <summary>
		''' Determines if a moderator can change a user's Trust status.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>If True, only module admin can change trust status for this user.</remarks>
		Public Property LockTrust() As Boolean
			Get
				Return _LockTrust
			End Get
			Set(ByVal Value As Boolean)
				_LockTrust = Value
			End Set
		End Property

		''' <summary>
		''' If the user is banned or not from posting.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsBanned() As Boolean
			Get
				Return _IsBanned
			End Get
			Set(ByVal Value As Boolean)
				_IsBanned = Value
			End Set
		End Property

		''' <summary>
		''' The date a ban on the user will be lifted.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LiftBanDate() As DateTime
			Get
				Return _LiftBanDate
			End Get
			Set(ByVal Value As DateTime)
				_LiftBanDate = Value
			End Set
		End Property

		''' <summary>
		''' The selection of threads to display (date filter)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TrackingDuration() As Integer
			Get
				Return _TrackingDuration
			End Get
			Set(ByVal Value As Integer)
				_TrackingDuration = Value
			End Set
		End Property

		''' <summary>
		''' If the user wants to receive notifications to all their threads replied to by default. If enbled, this will automaticly check the notify box on post add but they can remove it.
		''' </summary>
		''' <value></value>
		''' <returns>True if the user wants to receive notification for all their response threads, false otherwise.</returns>
		''' <remarks>Default = True</remarks>
		Public Property EnableDefaultPostNotify() As Boolean
			Get
				Return _EnableDefaultPostNotify
			End Get
			Set(ByVal Value As Boolean)
				_EnableDefaultPostNotify = Value
			End Set
		End Property

		''' <summary>
		''' If the user wants to receive notifications of their own posts. 
		''' </summary>
		''' <value></value>
		''' <returns>True if the user wants to receive notifications of their own posts, false otherwise.</returns>
		''' <remarks>Default = True</remarks>
		Public Property EnableSelfNotifications() As Boolean
			Get
				Return _EnableSelfNotifications
			End Get
			Set(ByVal Value As Boolean)
				_EnableSelfNotifications = Value
			End Set
		End Property

		''' <summary>
		''' Determines if the user is tracking entire module instance. 
		''' </summary>
		''' <value></value>
		''' <returns>True if the user wants to track notifications at the forum level.</returns>
		''' <remarks>Not Implemented YET</remarks>
		Public ReadOnly Property TrackedModule() As Boolean
			Get
				Return False
			End Get
		End Property

		''' <summary>
		''' Total number of records returned. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Only used for paging purposes.</remarks>
		Public Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal Value As Integer)
				_TotalRecords = Value
			End Set
		End Property

		''' <summary>
		''' The date a ban on the user started.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property StartBanDate() As Date
			Get
				Return _StartBanDate
			End Get
			Set(ByVal Value As DateTime)
				_StartBanDate = Value
			End Set
		End Property

#End Region

	End Class

#End Region

End Namespace
