'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2009
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

#Region "ForumUser"

	''' <summary>
	''' Everything necessary for a Forum User, all based on PortalID.
	''' This means a forum user profile is specific to a portal instance.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	12/4/2005	Created
	''' </history>
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
		Dim _EnableDisplayInMemberList As Boolean = False
		Dim _EnablePrivateMessages As Boolean = True
		Dim _EnableOnlineStatus As Boolean = True
		Dim _ThreadsPerPage As Integer = DefaultThreadsPerPage
		Dim _PostsPerPage As Integer = DefaultPostsPerPage
		Dim _LastActivity As DateTime = Now
		Dim _FlatView As Boolean = True
		Dim _ViewDescending As Boolean = False
		Dim _EnableModNotification As Boolean = True
		Dim _EnablePM As Boolean = True
		Dim _EnablePMNotifications As Boolean = True
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
		Dim _EnableProfileRegion As Boolean = False
		Dim _EnableDefaultPostNotify As Boolean = True
		Dim _EnableSelfNotifications As Boolean = True
		Dim _TotalRecords As Integer
		Dim _IsDeleted As Boolean = False
		Dim _UserWebsite As String
		Dim _Biography As String
		Dim _StartBanDate As Date
		' Not implemented
		Dim _IsAnonymous As Boolean = False

#End Region

#Region "Public ReadOnly Properties"

		''' <summary>
		''' The default number of threads shown per page. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property DefaultThreadsPerPage() As Integer
			Get
				Return Forum.Config.DefaultThreadsPerPage
			End Get
		End Property

		''' <summary>
		''' The default number of posts shown per page. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property DefaultPostsPerPage() As Integer
			Get
				Return Forum.Config.DefaultPostsPerPage
			End Get
		End Property

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
				_UserWebsite = Me.Profile.Website
				If Not _UserWebsite Is Nothing Then
					If _UserWebsite.Length > 0 AndAlso (Not _UserWebsite.StartsWith("http")) Then
						_UserWebsite = "http://" & _UserWebsite
					End If
				End If

				Return _UserWebsite
			End Get
		End Property

		''' <summary>
		''' Sets the users referred to alias depending on forum configuration
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This will be either core username or core displayname</remarks>
		Public ReadOnly Property SiteAlias() As String
			Get
				Dim strSiteAlias As String = String.Empty
				If objConfig.ForumMemberName = 0 Then
					strSiteAlias = Me.Username
				Else
					strSiteAlias = Me.DisplayName
				End If
				Return strSiteAlias
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
		''' The full path to the user's avatar for display. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property AvatarComplete() As String
			Get
				If _Avatar.Trim(";"c) <> String.Empty Then
					'[skeel] RETURN!! need to add none here as well, as UserAvatar has not been used before, 
					'even if it was there - eventually it has to go..
					If _UserAvatar = UserAvatarType.UserAvatar Or _UserAvatar = UserAvatarType.None Then
						_AvatarComplete = objConfig.CurrentPortalSettings.HomeDirectory + objConfig.UserAvatarPath + _Avatar.Trim(";"c)
					ElseIf _UserAvatar = UserAvatarType.PoolAvatar Then
						_AvatarComplete = objConfig.CurrentPortalSettings.HomeDirectory + objConfig.UserAvatarPoolPath + _Avatar.Trim(";"c)
					End If
				End If

				Return _AvatarComplete
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
		''' Determines if the user has unread Private Messages in their Inbox.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property UserHasNewMessages() As Boolean
			Get
				If UserID > 0 Then
					Dim ctlPMReads As New ForumPMReadsController
					Dim i As Integer = 0
					i = ctlPMReads.GetUserNewPMCount(UserID)
					If i > 0 Then
						Return True
					Else
						Return False
					End If
				Else
					Return False
				End If
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
		''' If the user has an avatar image assigned.  
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
					_RoleAvatars = ctlRoleAvatar.GetUserRoleAvatars(UserID, objConfig.ModuleID, PortalID)
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
		''' The last time the user posted. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LastActivity() As DateTime
			Get
				Return _LastActivity
			End Get
			Set(ByVal Value As DateTime)
				_LastActivity = Value
			End Set
		End Property

		''' <summary>
		''' The user's post view option (Flat/Tree)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property FlatView() As Boolean
			Get
				Return _FlatView
			End Get
			Set(ByVal Value As Boolean)
				_FlatView = Value
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
		''' if the user's profile shows in the memberlist. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnableDisplayInMemberList() As Boolean
			Get
				Return _EnableDisplayInMemberList
			End Get
			Set(ByVal Value As Boolean)
				_EnableDisplayInMemberList = Value
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
		''' If the user allows their email address (core profile) to be displayed in their forum profile. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnablePublicEmail() As Boolean
			Get
				Return _EnablePublicEmail
			End Get
			Set(ByVal Value As Boolean)
				_EnablePublicEmail = Value
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
				Return _ThreadsPerPage
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
				Return _PostsPerPage
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
		''' If the user allows Private Messages to be sent to him/her
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>You can only send PM's if you can receive PM's</remarks>
		Public Property EnablePM() As Boolean
			Get
				Return _EnablePM
			End Get
			Set(ByVal Value As Boolean)
				_EnablePM = Value
			End Set
		End Property

		''' <summary>
		''' If the user wants to receive an email each time they get a private message. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnablePMNotifications() As Boolean
			Get
				Return _EnablePMNotifications
			End Get
			Set(ByVal Value As Boolean)
				_EnablePMNotifications = Value
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
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Not Implemented</remarks>
		Public Property IsAnonymous() As Boolean
			Get
				Return _IsAnonymous
			End Get
			Set(ByVal Value As Boolean)
				_IsAnonymous = Value
			End Set
		End Property

		'CP - Everything here and below should be per moduleID
		''' <summary>
		''' Allows users to enable/disable their website shown in their profile and in posts view.
		''' </summary>
		''' <value></value>
		''' <returns>True if the user allows their website address to be displayed, false otherwise.</returns>
		''' <remarks>Default = True</remarks>
		Public Property EnableProfileWeb() As Boolean
			Get
				Return _EnableProfileWeb
			End Get
			Set(ByVal Value As Boolean)
				_EnableProfileWeb = Value
			End Set
		End Property

		''' <summary>
		''' Allows users to enable/disable their region shown in their profile and in posts view.
		''' </summary>
		''' <value></value>
		''' <returns>True if the user allows their region address to be displayed, false otherwise.</returns>
		''' <remarks>Default = False</remarks>
		Public Property EnableProfileRegion() As Boolean
			Get
				Return _EnableProfileRegion
			End Get
			Set(ByVal Value As Boolean)
				_EnableProfileRegion = Value
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
		''' Users biography. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Should be replaced with DNN Profile Biography when issue with html is solved in DNN core</remarks>
		Public Property Biography() As String
			Get
				Return _Biography
			End Get
			Set(ByVal Value As String)
				_Biography = Value
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
