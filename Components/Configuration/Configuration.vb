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

Imports DotNetNuke.Modules.Forum.Utilities
Imports DotNetNuke.Entities.Portals

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' This houses all the configuration items for use through this entire module.  
	''' Most of these are module settings.
	''' </summary>
	''' <remarks>This holds all the base settings (TabModuleSettings) used for the module. It applies defaults if they are not available.
	''' </remarks>
	Public Class Configuration

#Region "Private Members"

		' General Configuration
		Dim _ModuleID As Integer
		Dim _HasConfiguration As Boolean = False
		Dim _EnableAttachment As Boolean = False
		Dim _AggregatedForums As Boolean = False
		Dim _ThreadsPerPage As Integer = 10
		Dim _PostsPerPage As Integer = 5
		Dim _PostPagesCount As Integer = 3
		Dim _MaxPostImageWidth As Integer = 500
		Dim _DisplayPosterLocation As Integer = ShowPosterLocation.None
		Dim _DisplayPosterRegion As Boolean = False
		Dim _ForumMemberName As Integer = ForumDisplayName.Username
		Dim _ForumSkin As String = "Blue"
		Dim _EnableThreadStatus As Boolean = True
		Dim _EnableQuickReply As Boolean = False
		Dim _EnableUserSignatures As Boolean = True
		Dim _EnableModSigUpdates As Boolean = True
		Dim _EnableHTMLSignatures As Boolean = True
		Dim _PostEditWindow As Integer = 0
		Dim _EnableUserBanning As Boolean = False
		Dim _HideModEdits As Boolean = False
		'Email
		Dim _MailNotification As Boolean = True
		Dim _EnablePerForumFrom As Boolean = False
		Dim _AutomatedEmailAddress As String = "you@domain.com"
		Dim _EmailAddressDisplayName As String = String.Empty
		Dim _EnableEmailQueueTask As Boolean = False
		Dim _EnableEditEmails As Boolean = False
		Dim _EnableListServer As Boolean = False
		Dim _ListServerFolder As String = "Forums/ListServer/"
		'Stats & Rankings
		Dim _PopularThreadView As Integer = 200
		Dim _PopularThreadReply As Integer = 10
		' Poster Rankings
		Dim _Ranking As Boolean = True
		Dim _EnableRankingImage As Boolean = True
		Dim _FirstRankPosts As Integer = 1000
		Dim _SecondRankPosts As Integer = 900
		Dim _ThirdRankPosts As Integer = 800
		Dim _FourthRankPosts As Integer = 700
		Dim _FifthRankPosts As Integer = 600
		Dim _SixthRankPosts As Integer = 500
		Dim _SeventhRankPosts As Integer = 400
		Dim _EigthRankPosts As Integer = 300
		Dim _NinthRankPosts As Integer = 200
		Dim _TenthRankPosts As Integer = 100
		Dim _Rank_1_Title As String = String.Empty
		Dim _Rank_2_Title As String = String.Empty
		Dim _Rank_3_Title As String = String.Empty
		Dim _Rank_4_Title As String = String.Empty
		Dim _Rank_5_Title As String = String.Empty
		Dim _Rank_6_Title As String = String.Empty
		Dim _Rank_7_Title As String = String.Empty
		Dim _Rank_8_Title As String = String.Empty
		Dim _Rank_9_Title As String = String.Empty
		Dim _Rank_10_Title As String = String.Empty
		Dim _Rank_0_Title As String = String.Empty
		'Thread Ratings
		Dim _EnableRatings As Boolean = True
		Dim _RatingScale As Integer = 5
		' Syndication
		Dim _EnableRSS As Boolean = True
		Dim _RSSThreadsPerFeed As Integer = 20
		Dim _RSSUpdateInterval As Integer = 30
		' Bad word filtering
		Dim _EnableBadWordFilter As Boolean = True
		Dim _FilterSubject As Boolean = False
		' Community
		Dim _EnableUsersOnline As Boolean = DefaultEnableUsersOnline
		Dim _EnableExternalProfile As Boolean = False
		Dim _ExternalProfilePage As Integer = 0
		Dim _ExternalProfileParam As String = String.Empty
		Dim _ExternalProfileParamName As String = String.Empty
		Dim _ExternalProfileParamValue As String = String.Empty
		Dim _EnablePostAbuse As Boolean = True
		Dim _ImageExtension As String = "png"
		Dim _DisableHTMLPosting As Boolean = False
		Dim _TrustNewUsers As Boolean = False
		Dim _AutoLockTrust As Boolean = False
		' Attachments
		Dim _AttachmentPath As String = "Forums/Attachments/"
		Dim _CompleteAttachmentURI As String = String.Empty
		Dim _AnonDownloads As Boolean = True
		Dim _MaxAttachmentSize As Integer = 256
		' User Avatar
		Dim _EnableUserAvatar As Boolean = True
		Dim _EnableProfileAvatar As Boolean = True
		Dim _EnableProfileUserFolders As Boolean = True
		Dim _AvatarProfilePropName As String = "Photo"
		Dim _EnableUserAvatarPool As Boolean = False
		Dim _UserAvatarPoolPath As String = "Forums/PoolAvatar/"
		Dim _UserAvatarPath As String = "Forums/UserAvatar/"
		Dim _UserAvatarWidth As Integer = 128
		Dim _UserAvatarHeight As Integer = 128
		Dim _UserAvatarMaxSize As Integer = 64
		' System Avatar
		Dim _EnableSystemAvatar As Boolean = False
		Dim _SystemAvatarPath As String = "Forums/SystemAvatar/"
		' Role Avatar
		Dim _EnableRoleAvatar As Boolean = False
		Dim _RoleAvatarPath As String = "Forums/RoleAvatar/"
		' SEO
		Dim _NoFollowWeb As Boolean = False
		Dim _OverrideTitle As Boolean = True
		Dim _NoFollowLatestThreads As Boolean = True
		' new seo
		Dim _OverrideDescription As Boolean = True
		Dim _SitemapPriority As Double = 0.5
		' primary http alias (not implemented)
		Dim _PrimarySiteAlias As String
		Dim _EnableUserReadManagement As Boolean = True

#End Region

#Region "Public Vars"

		''' <summary>
		''' Used to start a quote block for a post quote reply type. 
		''' </summary>
		''' <remarks></remarks>
		Public Const QUOTE_OPEN As String = "[QUOTE]"

		''' <summary>
		''' Used to end a quote block for a post quote reply type. 
		''' </summary>
		''' <remarks></remarks>
		Public Const QUOTE_CLOSE As String = "[/QUOTE]"

#End Region

#Region "Shared Methods"

		''' <summary>
		''' Gets the forum configuration from cache, if its not there it loads it.
		''' </summary>
		''' <param name="ModuleID">The ModuleID we are looking to see if its configuration is cached already.</param>
		''' <returns>A specific ModuleID's cached TabModuleSettings (defaults applied if not in settings)</returns>
		''' <remarks>This allows us access and assigning defaults for new items to help in upgrade scenarios and to allow further expansion.
		''' </remarks>
		Public Shared Function GetForumConfig(ByVal ModuleID As Integer) As Configuration
			Dim strCacheKey As String = Constants.CACHE_KEY_PREFIX & CStr(ModuleID)
			Dim objConfig As Configuration = CType(DataCache.GetCache(strCacheKey), Configuration)

			If objConfig Is Nothing Then
				'config caching settings
				Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				objConfig = New Configuration(ModuleID)

				'Cache Config if timeout > 0 and Config is not null
				If timeOut > 0 And objConfig IsNot Nothing Then
					DataCache.SetCache(strCacheKey, objConfig, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return objConfig
		End Function

		''' <summary>
		''' Resets the modules configuration cache.
		''' </summary>
		''' <param name="ModuleID">The ModuleID of the cached object being reset.</param>
		''' <remarks>Should only be used on update's
		''' </remarks>
		Public Shared Sub ResetForumConfig(ByVal ModuleID As Integer)
			Dim strCacheKey As String = Constants.CACHE_KEY_PREFIX & CStr(ModuleID)
			DataCache.RemoveCache(strCacheKey)
		End Sub

#End Region

#Region "Public ReadOnly Properties"

#Region "Not Configurable"

		''' <summary>
		''' Gets the default value to see if users online option is enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if user's online status should be displayed, false otherwise.</returns>
		''' <remarks>Can only be enabled if the host settings are enabled for it.</remarks>
		Public ReadOnly Property DefaultEnableUsersOnline() As Boolean
			Get
				Dim Enabled As Boolean
				If Entities.Host.Host.GetHostSettingsDictionary.ContainsKey(Constants.HOST_SETTING_USERS_ONLINE) Then
					If Entities.Host.Host.GetHostSettingsDictionary(Constants.HOST_SETTING_USERS_ONLINE).ToString = "Y" Then
						Enabled = False
					Else
						Enabled = True
					End If
				Else
					Enabled = False
				End If
				Return Enabled
			End Get
		End Property

		''' <summary>
		''' Gets the default value for the forum source directory path. 
		''' </summary>
		''' <value></value>
		''' <returns>The full path to the module's directory.</returns>
		''' <remarks>A string representing hte forum module's DEFAULT name.(POSSIBLE REMOVE)</remarks>
		Public ReadOnly Property SourceDirectory() As String
			Get
				Return ApplicationPath & Constants.SOURCE_DIRECTORY
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ThemeBaseDirectory() As String
			Get
				Return SourceDirectory + "/Themes"
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ThemeDirectory() As String
			Get
				Return ThemeBaseDirectory + "/" + ForumTheme
			End Get
		End Property

		''' <summary>
		''' Gets the css file for a forum theme. 
		''' </summary>
		''' <value></value>
		''' <returns>The path to the css file for the selected theme.</returns>
		''' <remarks>Injected into all UI's of module.</remarks>
		Public ReadOnly Property Css() As String
			Get
				Return ThemeDirectory + "/" & ForumTheme.ToLower & ".css"
			End Get
		End Property

		''' <summary>
		''' Gets the ModuleID of the module being accessed. 
		''' </summary>
		''' <value></value>
		''' <returns>The ModuleID of the module being accessed.</returns>
		''' <remarks>Essential to configuration.</remarks>
		Public ReadOnly Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
		End Property

		''' <summary>
		''' Gets if the module has been preconfigured or not. 
		''' </summary>
		''' <value></value>
		''' <returns>True if the module has been previously configurated, false otherwise.</returns>
		''' <remarks>This should only be false first time module is dropped on page.</remarks>
		Public ReadOnly Property HasConfiguration() As Boolean
			Get
				Return _HasConfiguration
			End Get
		End Property

		''' <summary>
		''' Gets the path to the shared resource file. 
		''' </summary>
		''' <value></value>
		''' <returns>The path to the shared resource localization file.</returns>
		''' <remarks>Used for commonly used words across multiple views/controls.</remarks>
		Public ReadOnly Property SharedResourceFile() As String
			Get
				Return SourceDirectory & "/App_LocalResources/SharedResources.resx"
			End Get
		End Property

		''' <summary>
		''' Gets the current portal settings. 
		''' </summary>
		''' <value></value>
		''' <returns>A populated instance of DotNetNuke.Entities.Portals.PortalSettings</returns>
		''' <remarks>Needed here because no PortalModuleBase.</remarks>
		Public ReadOnly Property CurrentPortalSettings() As PortalSettings
			Get
				If HttpContext.Current Is Nothing Then
					' This should only happen via SOME scheduled tasks (ie. no HttpContext available). 
					Dim objFPortalSettings As New PortalSettings
					Dim PortalID As Integer

					PortalID = DataProvider.Instance().GetModulesPortalID(ModuleID)
					objFPortalSettings = ForumPortalSettings.CreateNewPortalSettings(PortalID)

					Return CType(HttpContext.Current.Items("PortalSettings"), PortalSettings)
				Else
					Return PortalController.GetCurrentPortalSettings()
				End If
			End Get
		End Property

		''' <summary>
		''' Gets the image folder where the forum theme images are located. 
		''' </summary>
		''' <param name="IncludeHost">Boolean representing the host user.</param>
		''' <value></value>
		''' <returns>The path to the applied theme's image folder.</returns>
		''' <remarks>Don't think IncludeHost is every True</remarks>
		Public ReadOnly Property ThemeImageFolder(Optional ByVal IncludeHost As Boolean = False) As String
			Get
				Dim folder As String = SourceDirectory & "/Themes/" & ForumTheme & "/Images/"
				If IncludeHost Then
					Return ForumUtils.AddHost(folder, PrimaryAlias)
				Else
					Return folder
				End If
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property SearchTabID() As Integer
			Get
				Dim objModules As New DotNetNuke.Entities.Modules.ModuleController()
				Dim SearchModule As DotNetNuke.Entities.Modules.ModuleInfo
				SearchModule = objModules.GetModuleByDefinition(CurrentPortalSettings.PortalId, "Search Results")

				If SearchModule IsNot Nothing Then
					Return SearchModule.TabID
				Else
					Return -1
				End If
			End Get
		End Property

#End Region

#Region "General Settings"

		''' <summary>
		''' Gets if Aggregated Forum should be shown. 
		''' </summary>
		''' <value></value>
		''' <returns>True if the aggregated forum is enabled, false otherwise.</returns>
		''' <remarks>Default is disabled.</remarks>
		Public ReadOnly Property AggregatedForums() As Boolean
			Get
				Return _AggregatedForums
			End Get
		End Property

		''' <summary>
		''' Gets if thread status is enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if thread status is enabled, false otherwise.</returns>
		''' <remarks>This is further controlled at the forum level.</remarks>
		Public ReadOnly Property EnableThreadStatus() As Boolean
			Get
				Return _EnableThreadStatus
			End Get
		End Property

		''' <summary>
		''' Gets if post abuse warnings are enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if users are allowed to report posts.</returns>
		''' <remarks>True by default.</remarks>
		Public ReadOnly Property EnablePostAbuse() As Boolean
			Get
				Return _EnablePostAbuse
			End Get
		End Property

		''' <summary>
		''' Gets if HTML posting should be disabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if HTML posting is disabled.</returns>
		''' <remarks>False by default.</remarks>
		Public ReadOnly Property DisableHTMLPosting() As Boolean
			Get
				Return _DisableHTMLPosting
			End Get
		End Property

#End Region

#Region "Forum User Interface"

		''' <summary>
		''' Gets number of threads per page to display. 
		''' </summary>
		''' <value></value>
		''' <returns>The number of threads per page to display.</returns>
		''' <remarks>Registered users have their own value.</remarks>
		Public ReadOnly Property ThreadsPerPage() As Integer
			Get
				Return _ThreadsPerPage
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts per page to display. 
		''' </summary>
		''' <value></value>
		''' <returns>The number of posts per page to display.</returns>
		''' <remarks>Registered users have their own value.</remarks>
		Public ReadOnly Property PostsPerPage() As Integer
			Get
				Return _PostsPerPage
			End Get
		End Property

		''' <summary>
		''' The number of post pages to show in thread view next to the title of each post. 
		''' </summary>
		''' <value></value>
		''' <returns>The number of post pages to show in the thread UI.</returns>
		''' <remarks>The default is 3.(ie. Page 1,2,3,...x)</remarks>
		Public ReadOnly Property PostPagesCount() As Integer
			Get
				Return _PostPagesCount
			End Get
		End Property

		''' <summary>
		''' Gets the forum theme to be used when no user's skin specified
		''' </summary>
		''' <value></value>
		''' <returns>The forum theme to apply to the module.</returns>
		''' <remarks>If per user themes are enabled, they override this value.</remarks>
		Public ReadOnly Property ForumTheme() As String
			Get
				Return _ForumSkin
			End Get
		End Property

		''' <summary>
		''' Gets the image extension used for the forum theme. 
		''' </summary>
		''' <value></value>
		''' <returns>The image extension string without a period.</returns>
		''' <remarks>This is typically jpg, png, gif.</remarks>
		Public ReadOnly Property ImageExtension() As String
			Get
				Return _ImageExtension
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property EnableQuickReply() As Boolean
			Get
				Return _EnableQuickReply
			End Get
		End Property

#End Region

#Region "User Settings"

		''' <summary>
		''' Gets the integer value which represents if module is using displayname or username for site alias in module. 
		''' </summary>
		''' <value></value>
		''' <returns>An interger which represpents how the user names should be displayed.</returns>
		''' <remarks>Corresponds w/ an Enum. (username or display name are options)</remarks>
		Public ReadOnly Property ForumMemberName() As Integer
			Get
				Return _ForumMemberName
			End Get
		End Property

		''' <summary>
		''' Gets if user signatures are enabled for the module.
		''' </summary>
		''' <value></value>
		''' <returns>True if users are permitted to have a signature.</returns>
		''' <remarks>Default is true.</remarks>
		Public ReadOnly Property EnableUserSignatures() As Boolean
			Get
				Return _EnableUserSignatures
			End Get
		End Property

		''' <summary>
		''' Gets if signatures should permit HTML. 
		''' </summary>
		''' <value></value>
		''' <returns>True if HTML signatures are enabled, false otherwise.</returns>
		''' <remarks>Default is True.</remarks>
		Public ReadOnly Property EnableHTMLSignatures() As Boolean
			Get
				Return _EnableHTMLSignatures
			End Get
		End Property

		''' <summary>
		''' Defines for how long a post can be edited by the user 
		''' </summary>
		''' <value></value>
		''' <returns>integer value for minutes (0=indefinitely)</returns>
		''' <remarks>Added by Skeel</remarks>
		Public ReadOnly Property PostEditWindow() As Integer
			Get
				Return _PostEditWindow
			End Get
		End Property

		''' <summary>
		''' Determines if users can manage their thread read status (ie. clear read data/mark all read are available if true).
		''' </summary>
		''' <value></value>
		''' <returns>True, if users can manage their own read status. False otherwise.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property EnableUserReadManagement() As Boolean
			Get
				Return _EnableUserReadManagement
			End Get
		End Property

#End Region

#Region "Moderator Settings"

		''' <summary>
		''' Gets integer value which represents how to handle poster location displaying. 
		''' </summary>
		''' <value></value>
		''' <returns>An integer representing how to display poster location.</returns>
		''' <remarks>Uses 3 list localized values, which correspond w/ an enum.</remarks>
		Public ReadOnly Property DisplayPosterLocation() As Integer
			Get
				Return _DisplayPosterLocation
			End Get
		End Property

		''' <summary>
		''' Gets integer value which represents how to handle poster location displaying. 
		''' </summary>
		''' <value></value>
		''' <returns>An integer representing how to display poster location.</returns>
		''' <remarks>Uses 3 list localized values, which correspond w/ an enum.</remarks>
		Public ReadOnly Property DisplayPosterRegion() As Boolean
			Get
				Return _DisplayPosterRegion
			End Get
		End Property

		''' <summary>
		''' Gets if the module automatically 'trusts' new users. 
		''' </summary>
		''' <value></value>
		''' <returns>True if newly created forum users should be automatically trusted.</returns>
		''' <remarks>Trusted users can post in moderated forums w/out awaiting post approval.</remarks>
		Public ReadOnly Property TrustNewUsers() As Boolean
			Get
				Return _TrustNewUsers
			End Get
		End Property

		''' <summary>
		''' Gets if the module locks 'trust' settings of new users
		''' </summary>
		''' <value></value>
		''' <returns>True if new users created should have trust setting locked for change from moderators.</returns>
		''' <remarks>If true, all new forum users created have their trust locked so it can only be changed by a forum admin</remarks>
		Public ReadOnly Property AutoLockTrust() As Boolean
			Get
				Return _AutoLockTrust
			End Get
		End Property

		''' <summary>
		''' Gets if moderators are permitted to edit user signatures. (shows in profile view if enabled)
		''' </summary>
		''' <value></value>
		''' <returns>True if moderators are permitted to update user's signatures, false otherwise.</returns>
		''' <remarks>Default is True.</remarks>
		Public ReadOnly Property EnableModSigUpdates() As Boolean
			Get
				Return _EnableModSigUpdates
			End Get
		End Property

		Public ReadOnly Property HideModEdits() As Boolean
			Get
				Return _HideModEdits
			End Get
		End Property

		''' <summary>
		''' Gets if user bans are enabled.
		''' </summary>
		''' <value></value>
		''' <returns>True if user banning is enabled, false otherwise.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property EnableUserBanning() As Boolean
			Get
				Return _EnableUserBanning
			End Get
		End Property

#End Region

#Region "Community Settings"

		''' <summary>
		''' Gets if users online status should be displayed. 
		''' </summary>
		''' <value></value>
		''' <returns>True if users online is enabled for the module.</returns>
		''' <remarks>Must be enabled in host settings as well.</remarks>
		Public ReadOnly Property EnableUsersOnline() As Boolean
			Get
				Return _EnableUsersOnline
			End Get
		End Property

		''' <summary>
		''' Gets if the module supports an external profile page.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property EnableExternalProfile() As Boolean
			Get
				Return _EnableExternalProfile
			End Get
		End Property

		''' <summary>
		''' Gets the page to be used for external profiles.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ExternalProfilePage() As Integer
			Get
				Return _ExternalProfilePage
			End Get
		End Property

		''' <summary>
		''' Deterines if a username should be passed for external user profile modules.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ExternalProfileUsername() As Boolean
			Get
				Return False
			End Get
		End Property

		''' <summary>
		''' Gets the parameter to be used to represent UserID in external profile pages.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ExternalProfileParam() As String
			Get
				Return _ExternalProfileParam
			End Get
		End Property

		''' <summary>
		''' Gets the settings for external profile URL parameter name.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ExternalProfileParamName() As String
			Get
				Return _ExternalProfileParamName
			End Get
		End Property

		''' <summary>
		''' Gets the setting for external profile URL parameter value.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ExternalProfileParamValue() As String
			Get
				Return _ExternalProfileParamValue
			End Get
		End Property

#End Region

#Region "Attachments"

		''' <summary>
		''' Gets if attachments are enabled for the module. 
		''' </summary>
		''' <value></value>
		''' <returns>True if attachments are enabled for the module.</returns>
		''' <remarks>Per forum permissions determine who can add attachments.</remarks>
		Public ReadOnly Property EnableAttachment() As Boolean
			Get
				Return _EnableAttachment
			End Get
		End Property

		''' <summary>
		''' Gets if anonymous attachment downloads are enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if anonymous users are permited to download attachments.</returns>
		''' <remarks>Still must factor in core folder permissions.</remarks>
		Public ReadOnly Property AnonDownloads() As Boolean
			Get
				Return _AnonDownloads
			End Get
		End Property

		''' <summary>
		''' Gets the location of where attachments are to be uploäded 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public ReadOnly Property AttachmentPath() As String
			Get
				Return _AttachmentPath
			End Get
		End Property

		''' <summary>
		''' The maximum file size (in KB) permitted for uploading attachments.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Users should be aware of file size restriction set in web.config.</remarks>
		Public ReadOnly Property MaxAttachmentSize() As Integer
			Get
				Return _MaxAttachmentSize
			End Get
		End Property

		''' <summary>
		''' Gets the complete location of where attachments are located 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Property CompleteAttachmentURI() As String
			Get
				If _CompleteAttachmentURI = String.Empty Then
					Dim myPath As String = CurrentPortalSettings.HomeDirectory & _AttachmentPath
					If myPath.EndsWith("/") = False Then myPath += "/"
					_CompleteAttachmentURI = myPath
				End If
				Return _CompleteAttachmentURI
			End Get
			Set(ByVal value As String)
				_CompleteAttachmentURI = value
			End Set
		End Property

#End Region

#Region "RSS"

		''' <summary>
		''' Gets if RSS Feed option is enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if RSS feeds are enabled.</returns>
		''' <remarks>Feeds can only be enabled for Public forums.</remarks>
		Public ReadOnly Property EnableRSS() As Boolean
			Get
				Return _EnableRSS
			End Get
		End Property

		''' <summary>
		''' Gets the number of Threads to display per RSS Feed
		''' </summary>
		''' <value></value>
		''' <returns>Number of threads to display in a feed.</returns>
		''' <remarks>Used to minimize the amount of data being sent.</remarks>
		Public ReadOnly Property RSSThreadsPerFeed() As Integer
			Get
				Return _RSSThreadsPerFeed
			End Get
		End Property

		''' <summary>
		''' Gets how often the RSS feeds should be updated (in minutes)
		''' </summary>
		''' <value></value>
		''' <returns>The number of minutes the RSS feeds should be updated.</returns>
		''' <remarks>Used to increase performance.</remarks>
		Public ReadOnly Property RSSUpdateInterval() As Integer
			Get
				Return _RSSUpdateInterval
			End Get
		End Property

#End Region

#Region "SEO"

		''' <summary>
		''' Gets if links in user web sites and profiles to external web sites will have the "nofollow" attribute set on the link.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property NoFollowWeb() As Boolean
			Get
				Return _NoFollowWeb
			End Get
		End Property

		''' <summary>
		''' Gets if the page title shown in browser will be overridden by the forum module. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property OverrideTitle() As Boolean
			Get
				Return _OverrideTitle
			End Get
		End Property

		''' <summary>
		''' Gets if links used for latest threads (6,12,24,48 hours)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property NoFollowLatestThreads() As Boolean
			Get
				Return _NoFollowLatestThreads
			End Get
		End Property

		''' <summary>
		''' Determines if the meta description will be overriden. This will get form description for Threads view, First post body (summary, 150 limit) with HTML stripped  when in Posts view. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property OverrideDescription() As Boolean
			Get
				Return _OverrideDescription
			End Get
		End Property

		''' <summary>
		''' This determines the sitemap priority for SEO generated pages. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Using Core, values should be between 1.0 and .1.</remarks>
		Public ReadOnly Property SitemapPriority() As Double
			Get
				Return _SitemapPriority
			End Get
		End Property

#End Region

#Region "Stats/Rankings/Popular Page"

#Region "Popular Thread Settings"

		''' <summary>
		''' The number of views required to make a thread 'popular' status. 
		''' </summary>
		''' <value></value>
		''' <returns>The number of views required to make a thread 'hot'.</returns>
		''' <remarks>Works as or situation w/ pop thread replies.</remarks>
		Public ReadOnly Property PopularThreadView() As Integer
			Get
				Return _PopularThreadView
			End Get
		End Property

		''' <summary>
		''' Gets the number of replies which are required to make a thread 'popular' status. 
		''' </summary>
		''' <value></value>
		''' <returns>The number of replies a thread should have before becoming 'hot'.</returns>
		''' <remarks>Works as or situation w/ number of pop thread views.</remarks>
		Public ReadOnly Property PopularThreadReply() As Integer
			Get
				Return _PopularThreadReply
			End Get
		End Property

#End Region

#Region "Rankings"

		''' <summary>
		''' Gets if user post rankings are enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if post rankings are enabled, false otherwise.</returns>
		''' <remarks>True by default.</remarks>
		Public ReadOnly Property Ranking() As Boolean
			Get
				Return _Ranking
			End Get
		End Property

		''' <summary>
		''' Gets if an image will be shown, or just a title for user rankings.
		''' </summary>
		''' <value></value>
		''' <returns>True if ranking images are being used instead of words, false otherwise.</returns>
		''' <remarks>This only matters if rankings are enabled. True by default.</remarks>
		Public ReadOnly Property EnableRankingImage() As Boolean
			Get
				Return _EnableRankingImage
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 1st ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve first ranked poster status.</returns>
		''' <remarks>Highest ranking</remarks>
		Public ReadOnly Property FirstRankPosts() As Integer
			Get
				Return _FirstRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 2nd ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve second ranked poster status.</returns>
		''' <remarks>Second highest ranking</remarks>
		Public ReadOnly Property SecondRankPosts() As Integer
			Get
				Return _SecondRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 3rd ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve second ranked poster status.</returns>
		''' <remarks>Third highest ranking.</remarks>
		Public ReadOnly Property ThirdRankPosts() As Integer
			Get
				Return _ThirdRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 4th ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve third ranked poster status.</returns>
		''' <remarks>Fourth highest ranking.</remarks>
		Public ReadOnly Property FourthRankPosts() As Integer
			Get
				Return _FourthRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 5th ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve fifth ranked poster status.</returns>
		''' <remarks>Middle of the scale ranking.</remarks>
		Public ReadOnly Property FifthRankPosts() As Integer
			Get
				Return _FifthRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 6th ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve sixth ranked poster status.</returns>
		''' <remarks>Third lowest ranking.</remarks>
		Public ReadOnly Property SixthRankPosts() As Integer
			Get
				Return _SixthRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 7th ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve seventh ranked poster status.</returns>
		''' <remarks>Fourth lowest ranking</remarks>
		Public ReadOnly Property SeventhRankPosts() As Integer
			Get
				Return _SeventhRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 8th ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve eigth ranked poster status.</returns>
		''' <remarks>Third lowest ranking.</remarks>
		Public ReadOnly Property EigthRankPosts() As Integer
			Get
				Return _EigthRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 9th ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve ninth ranked poster status.</returns>
		''' <remarks>Second lowest ranking.</remarks>
		Public ReadOnly Property NinthRankPosts() As Integer
			Get
				Return _NinthRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the number of posts required to achieve 10th ranked poster status. 
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts to achieve tenth ranked poster status.</returns>
		''' <remarks>Tenth is lowest number.</remarks>
		Public ReadOnly Property TenthRankPosts() As Integer
			Get
				Return _TenthRankPosts
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 1st post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent first ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_1_Title() As String
			Get
				Return _Rank_1_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 2nd post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent second ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_2_Title() As String
			Get
				Return _Rank_2_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 3rd post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent third ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_3_Title() As String
			Get
				Return _Rank_3_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 4th post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent fourth ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_4_Title() As String
			Get
				Return _Rank_4_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 5th post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent fifth ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_5_Title() As String
			Get
				Return _Rank_5_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 6th post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent sixth ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_6_Title() As String
			Get
				Return _Rank_6_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 7th post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent seventh ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_7_Title() As String
			Get
				Return _Rank_7_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 8th post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent eigth ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_8_Title() As String
			Get
				Return _Rank_8_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 9th post ranking title.
		''' </summary>
		''' <value>String to represent ninth ranked status.</value>
		''' <returns></returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_9_Title() As String
			Get
				Return _Rank_9_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding 10th post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent tenth ranked status.</returns>
		''' <remarks>If empty string, localization value will be used. (Lowest ranking if ranked)</remarks>
		Public ReadOnly Property Rank_10_Title() As String
			Get
				Return _Rank_10_Title
			End Get
		End Property

		''' <summary>
		''' Gets the localization overriding No post ranking title.
		''' </summary>
		''' <value></value>
		''' <returns>String to represent not ranked status.</returns>
		''' <remarks>If empty string, localization value will be used.</remarks>
		Public ReadOnly Property Rank_0_Title() As String
			Get
				Return _Rank_0_Title
			End Get
		End Property

#End Region

#Region "Ratings"

		''' <summary>
		''' Gets if Thread ratings are enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if post rankings are enabled, false otherwise.</returns>
		''' <remarks>Enabled by default.</remarks>
		Public ReadOnly Property EnableRatings() As Boolean
			Get
				Return _EnableRatings
			End Get
		End Property

		''' <summary>
		''' The highest number which a thread rating can be. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property RatingScale() As Integer
			Get
				Return _RatingScale
			End Get
		End Property

#End Region

#End Region

#Region "Email Settings"

		''' <summary>
		''' Gets the Send From email address of all outgoing emails from this module. 
		''' </summary>
		''' <value></value>
		''' <returns>The email address for outgoing emails.</returns>
		''' <remarks>Default value should be admin email.</remarks>
		Public ReadOnly Property AutomatedEmailAddress() As String
			Get
				Return _AutomatedEmailAddress
			End Get
		End Property

		''' <summary>
		''' Gets the "Send From" friendly name instead of just email address used on outgoing email sends. 
		''' </summary>
		''' <value></value>
		''' <returns>The outgoing email's friendly name.</returns>
		''' <remarks>Defaults to nothing.</remarks>
		Public ReadOnly Property EmailAddressDisplayName() As String
			Get
				Return _EmailAddressDisplayName
			End Get
		End Property

		''' <summary>
		''' Gets if email notification is enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if email notifications are enabled, false otherwise.</returns>
		''' <remarks>Default is enabled.</remarks>
		Public ReadOnly Property MailNotification() As Boolean
			Get
				Return _MailNotification
			End Get
		End Property

		''' <summary>
		''' Gets if email from address and friendly name can be overridden at the forum level. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property EnablePerForumFrom() As Boolean
			Get
				Return _EnablePerForumFrom
			End Get
		End Property

		''' <summary>
		''' Gets if email queue task is enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if email queueing is enabled, false otherwise.</returns>
		''' <remarks>Disabled by default, requires scheduled task to be enabled.</remarks>
		Public ReadOnly Property EnableEmailQueueTask() As Boolean
			Get
				Return _EnableEmailQueueTask
			End Get
		End Property

		''' <summary>
		''' Gets if edit email notifications should go out.
		''' </summary>
		''' <value></value>
		''' <returns>True if email notifications for post edits are enabled, false otherwise.</returns>
		''' <remarks>Default is False.</remarks>
		Public ReadOnly Property EnableEditEmails() As Boolean
			Get
				Return _EnableEditEmails
			End Get
		End Property

		Public ReadOnly Property EnableListServer() As Boolean
			Get
				Return _EnableListServer
			End Get
		End Property

		Public ReadOnly Property ListServerFolder() As String
			Get
				Return _ListServerFolder
			End Get
		End Property

		''' <summary>
		''' The primary Alias should only be used in email related items. This is used for full URL's that link to images in outgoing emails. 
		''' </summary>
		''' <value></value>
		''' <returns>A full http path for emails images.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property PrimaryAlias() As String
			Get
				If _PrimarySiteAlias = String.Empty Then
					' get the primary portal alias
					Dim pac As New PortalAliasController()
					Dim aliases As PortalAliasCollection = pac.GetPortalAliasByPortalID(CurrentPortalSettings.PortalId)

					Dim aliasKey As String = String.Empty
					Dim portalAlias As PortalAliasInfo = Nothing

					If Not aliases Is Nothing AndAlso aliases.Count > 0 Then
						'get the first portal alias in the list and use that
						For Each key As String In aliases.Keys
							aliasKey = key
							portalAlias = aliases(key)

							_PrimarySiteAlias = portalAlias.HTTPAlias
							Exit For
						Next
					End If

				End If
				Return _PrimarySiteAlias
			End Get
		End Property

#End Region

#Region "Filtering"

		''' <summary>
		''' Gets if the word filter is enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if filtering of bad words is enabled.</returns>
		''' <remarks>Admins must add words to filter for.</remarks>
		Public ReadOnly Property EnableBadWordFilter() As Boolean
			Get
				Return _EnableBadWordFilter
			End Get
		End Property

		''' <summary>
		''' Gets if post subjects should be filtered for bad words. 
		''' </summary>
		''' <value></value>
		''' <returns>True if post subjects should be filtered for bad words.</returns>
		''' <remarks>Requires users to add bad words.</remarks>
		Public ReadOnly Property FilterSubject() As Boolean
			Get
				If EnableBadWordFilter Then
					Return _FilterSubject
				Else
					Return False
				End If
			End Get
		End Property

#End Region

#Region "Avatar Settings"

		''' <summary>
		''' Gets if the ability for users to add/show avatars is enabled. 
		''' </summary>
		''' <value></value>
		''' <returns>True if user avatars are enabled, false otherwise.</returns>
		''' <remarks>Default is true.</remarks>
		Public ReadOnly Property EnableUserAvatar() As Boolean
			Get
				Return _EnableUserAvatar
			End Get
		End Property

		''' <summary>
		''' Determines if the module should be using profile property avatars.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property EnableProfileAvatar() As Boolean
			Get
				Return _EnableProfileAvatar
			End Get
		End Property

		''' <summary>
		''' Determines if user folders (part of 5.3 and greater) should be used for profile avatar paths. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property EnableProfileUserFolders() As Boolean
			Get
				Return _EnableProfileUserFolders
			End Get
		End Property

		''' <summary>
		''' Gets the profile property name for avatars.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property AvatarProfilePropName() As String
			Get
				Return _AvatarProfilePropName
			End Get
		End Property

		''' <summary>
		''' Gets if the user avatar is to only allow selectable items from a 'pool'. 
		''' </summary>
		''' <value></value>
		''' <returns>True if users can only choose from predefined folder of avatars, false otherwise.</returns>
		''' <remarks>Used to allow avatars, but avoid users uploading.</remarks>
		Public ReadOnly Property EnableUserAvatarPool() As Boolean
			Get
				Return _EnableUserAvatarPool
			End Get
		End Property

		''' <summary>
		''' Gets the User Avatar file path. 
		''' </summary>
		''' <value></value>
		''' <returns>The path to the user avatars folder.</returns>
		''' <remarks>Relative to Portals/PortalID/ folder.</remarks>
		Public ReadOnly Property UserAvatarPath() As String
			Get
				Return _UserAvatarPath
			End Get
		End Property

		''' <summary>
		''' Gets the User Avatar Pool file path. 
		''' </summary>
		''' <value></value>
		''' <returns>The path to the user avatars pool folder.</returns>
		''' <remarks>Relative to Portals/PortalID/ folder.</remarks>
		Public ReadOnly Property UserAvatarPoolPath() As String
			Get
				Return _UserAvatarPoolPath
			End Get
		End Property

		''' <summary>
		''' Gets the User Avatar width in pixels. 
		''' </summary>
		''' <value></value>
		''' <returns>The maximum pixel width used for user avatars.</returns>
		''' <remarks>Size in Piexels</remarks>
		Public ReadOnly Property UserAvatarWidth() As Integer
			Get
				Return _UserAvatarWidth
			End Get
		End Property

		''' <summary>
		''' Gets the User Avatar height in pixels. 
		''' </summary>
		''' <value></value>
		''' <returns>The maximum Pixel height used for User avatars.</returns>
		''' <remarks>Size in Piexels</remarks>
		Public ReadOnly Property UserAvatarHeight() As Integer
			Get
				Return _UserAvatarHeight
			End Get
		End Property

		''' <summary>
		''' Gets the User Avatar maximum file size in KB. 
		''' </summary>
		''' <value></value>
		''' <returns>The maximum file size for User Avatars.</returns>
		''' <remarks>Size is in KB.</remarks>
		Public ReadOnly Property UserAvatarMaxSize() As Integer
			Get
				Return _UserAvatarMaxSize
			End Get
		End Property

		''' <summary>
		''' Gets if the module allows System Avatars. 
		''' </summary>
		''' <value></value>
		''' <returns>True if System avatars are enabled, false otherwise.</returns>
		''' <remarks>True by default.</remarks>
		Public ReadOnly Property EnableSystemAvatar() As Boolean
			Get
				Return _EnableSystemAvatar
			End Get
		End Property

		''' <summary>
		''' Gets the System Avatar file path. 
		''' </summary>
		''' <value></value>
		''' <returns>The path to system avatars.</returns>
		''' <remarks>Relative to Portals/PortalID/ Directory</remarks>
		Public ReadOnly Property SystemAvatarPath() As String
			Get
				Return _SystemAvatarPath
			End Get
		End Property

		''' <summary>
		''' Gets if the module allows Role Avatars. 
		''' </summary>
		''' <value></value>
		''' <returns>True if Role avatars are enabled, false otherwise.</returns>
		''' <remarks>True by default.</remarks>
		Public ReadOnly Property EnableRoleAvatar() As Boolean
			Get
				Return _EnableRoleAvatar
			End Get
		End Property

		''' <summary>
		''' Path where role avatars are stored (relative to Portals directory)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property RoleAvatarPath() As String
			Get
				Return _RoleAvatarPath
			End Get
		End Property

		''' <summary>
		''' Gets the maximum allowed image width for post images 
		''' </summary>
		''' <value></value>
		''' <returns>Maximum allowed width for images inside the postbody</returns>
		''' <remarks>Added by Skeel</remarks>
		Public ReadOnly Property MaxPostImageWidth() As Integer
			Get
				Return _MaxPostImageWidth
			End Get
		End Property

#End Region

#End Region

#Region "Constructors"

		''' <summary>
		''' Loads modules config settings from Module Settings based on moduleId.
		''' </summary>
		''' <param name="ModuleID">The ModuleID to load the configuration for.</param>
		''' <remarks>This is the base of the module settings, if none are found it creates defaults for a new module instance.
		''' </remarks>
		Private Sub New(ByVal ModuleID As Integer)
			_ModuleID = ModuleID
			Dim ctlModule As New Entities.Modules.ModuleController
			' Grab settings from the database
			Dim cntModule As New Entities.Modules.ModuleController
			Dim settings As Hashtable = cntModule.GetModuleSettings(ModuleID)

			If settings.Count > 0 Then
				_HasConfiguration = True
			Else
				Try
					' PreConfigure the Module, if context isn't here (and then configuration exists) let it throw an error. 
					Dim _context As HttpContext = HttpContext.Current
					Dim _portalSettings As PortalSettings = CType(_context.Items("PortalSettings"), PortalSettings)

					Dim mUserID As Integer
					If _context.Request.IsAuthenticated And ModuleID > 0 Then
						' Let's double check to make sure settings don't exist somehow for this module
						Dim settings2 As Hashtable = cntModule.GetModuleSettings(ModuleID)
						' This should ensure we don't overwrite things
						If Not settings2.Count > 0 Then
							mUserID = Users.UserController.GetCurrentUserInfo.UserID
							ForumPreConfig.PreConfig(_ModuleID, _portalSettings.PortalId, mUserID)

							Dim newsettings As Hashtable = cntModule.GetModuleSettings(ModuleID)

							If newsettings.Count > 0 Then
								_HasConfiguration = True
								settings = newsettings
							End If
						End If
					End If
				Catch ex As Exception
					' This is here to ensure we don't somehow have an unhandled error.
					LogException(ex)
				End Try
			End If

			' post attachments
			If Not settings(Constants.ENABLE_ATTACHMENT) Is Nothing Then
				If Not settings(Constants.ENABLE_ATTACHMENT).ToString = String.Empty Then
					_EnableAttachment = CBool(GetValue(settings(Constants.ENABLE_ATTACHMENT), CStr(_EnableAttachment)))
				End If
			End If

			' notification email
			If Not settings(Constants.EMAIL_AUTO_FROM_ADDRESS) Is Nothing Then
				If Not settings(Constants.EMAIL_AUTO_FROM_ADDRESS).ToString = String.Empty Then
					_AutomatedEmailAddress = CStr(GetValue(settings(Constants.EMAIL_AUTO_FROM_ADDRESS), CStr(_AutomatedEmailAddress)))
				End If
			End If

			If Not settings(Constants.ENABLE_MAIL_NOTIFICATIONS) Is Nothing Then
				If Not settings(Constants.ENABLE_MAIL_NOTIFICATIONS).ToString = String.Empty Then
					_MailNotification = CBool(GetValue(settings(Constants.ENABLE_MAIL_NOTIFICATIONS), CStr(_MailNotification)))
				End If
			End If

			If Not settings(Constants.ENABLE_PER_FORUM_EMAILS) Is Nothing Then
				If Not settings(Constants.ENABLE_PER_FORUM_EMAILS).ToString = String.Empty Then
					_EnablePerForumFrom = CBool(GetValue(settings(Constants.ENABLE_PER_FORUM_EMAILS), CStr(_EnablePerForumFrom)))
				End If
			End If

			' Not implemented
			If Not settings(Constants.ENABLE_LIST_SERVER) Is Nothing Then
				If Not settings(Constants.ENABLE_LIST_SERVER).ToString = String.Empty Then
					_EnableListServer = CBool(GetValue(settings(Constants.ENABLE_LIST_SERVER), CStr(_EnableListServer)))
				End If
			End If

			' Not implemented
			If Not settings(Constants.LIST_SERVER_FOLDER) Is Nothing Then
				If Not settings(Constants.LIST_SERVER_FOLDER).ToString = String.Empty Then
					_ListServerFolder = CStr(GetValue(settings(Constants.LIST_SERVER_FOLDER), CStr(_ListServerFolder)))
				End If
			End If

			If Not settings(Constants.ENABLE_AGGREGATED_FORUM) Is Nothing Then
				If Not settings(Constants.ENABLE_AGGREGATED_FORUM).ToString = String.Empty Then
					_AggregatedForums = CBool(GetValue(settings(Constants.ENABLE_AGGREGATED_FORUM), CStr(_AggregatedForums)))
				End If
			End If

			If Not settings(Constants.THREADS_PER_PAGE) Is Nothing Then
				If Not settings(Constants.THREADS_PER_PAGE).ToString = String.Empty Then
					_ThreadsPerPage = CInt(GetValue(settings(Constants.THREADS_PER_PAGE), CStr(_ThreadsPerPage)))
				End If
			End If

			If Not settings(Constants.POSTS_PER_PAGE) Is Nothing Then
				If Not settings(Constants.POSTS_PER_PAGE).ToString = String.Empty Then
					_PostsPerPage = CInt(GetValue(settings(Constants.POSTS_PER_PAGE), CStr(_PostsPerPage)))
				End If
			End If

			If Not settings(Constants.POPULAR_THREAD_VIEWS) Is Nothing Then
				If Not settings(Constants.POPULAR_THREAD_VIEWS).ToString = String.Empty Then
					_PopularThreadView = CInt(GetValue(settings(Constants.POPULAR_THREAD_VIEWS), CStr(_PopularThreadView)))
				End If
			End If

			If Not settings(Constants.POPULAR_THREAD_REPLIES) Is Nothing Then
				If Not settings(Constants.POPULAR_THREAD_REPLIES).ToString = String.Empty Then
					_PopularThreadReply = CInt(GetValue(settings(Constants.POPULAR_THREAD_REPLIES), CStr(_PopularThreadReply)))
				End If
			End If

			' Ranking post count
			If Not settings(Constants.ENABLE_RANKINGS) Is Nothing Then
				If Not settings(Constants.ENABLE_RANKINGS).ToString = String.Empty Then
					_Ranking = CBool(GetValue(settings(Constants.ENABLE_RANKINGS), CStr(_Ranking)))
				End If
			End If

			If Not settings(Constants.FIRST_RANK) Is Nothing Then
				If Not settings(Constants.FIRST_RANK).ToString = String.Empty Then
					_FirstRankPosts = CInt(GetValue(settings(Constants.FIRST_RANK), CStr(_FirstRankPosts)))
				End If
			End If

			If Not settings(Constants.SECOND_RANK) Is Nothing Then
				If Not settings(Constants.SECOND_RANK).ToString = String.Empty Then
					_SecondRankPosts = CInt(GetValue(settings(Constants.SECOND_RANK), CStr(_SecondRankPosts)))
				End If
			End If

			If Not settings(Constants.THIRD_RANK) Is Nothing Then
				If Not settings(Constants.THIRD_RANK).ToString = String.Empty Then
					_ThirdRankPosts = CInt(GetValue(settings(Constants.THIRD_RANK), CStr(_ThirdRankPosts)))
				End If
			End If

			If Not settings(Constants.FOURTH_RANK) Is Nothing Then
				If Not settings(Constants.FOURTH_RANK).ToString = String.Empty Then
					_FourthRankPosts = CInt(GetValue(settings(Constants.FOURTH_RANK), CStr(_FourthRankPosts)))
				End If
			End If

			If Not settings(Constants.FIFTH_RANK) Is Nothing Then
				If Not settings(Constants.FIFTH_RANK).ToString = String.Empty Then
					_FifthRankPosts = CInt(GetValue(settings(Constants.FIFTH_RANK), CStr(_FifthRankPosts)))
				End If
			End If

			If Not settings(Constants.SIXTH_RANK) Is Nothing Then
				If Not settings(Constants.SIXTH_RANK).ToString = String.Empty Then
					_SixthRankPosts = CInt(GetValue(settings(Constants.SIXTH_RANK), CStr(_SixthRankPosts)))
				End If
			End If

			If Not settings(Constants.SEVENTH_RANK) Is Nothing Then
				If Not settings(Constants.SEVENTH_RANK).ToString = String.Empty Then
					_SeventhRankPosts = CInt(GetValue(settings(Constants.SEVENTH_RANK), CStr(_SeventhRankPosts)))
				End If
			End If

			If Not settings(Constants.EIGTH_RANK) Is Nothing Then
				If Not settings(Constants.EIGTH_RANK).ToString = String.Empty Then
					_EigthRankPosts = CInt(GetValue(settings(Constants.EIGTH_RANK), CStr(_EigthRankPosts)))
				End If
			End If

			If Not settings(Constants.NINTH_RANK) Is Nothing Then
				If Not settings(Constants.NINTH_RANK).ToString = String.Empty Then
					_NinthRankPosts = CInt(GetValue(settings(Constants.NINTH_RANK), CStr(_NinthRankPosts)))
				End If
			End If

			If Not settings(Constants.TENTH_RANK) Is Nothing Then
				If Not settings(Constants.TENTH_RANK).ToString = String.Empty Then
					_TenthRankPosts = CInt(GetValue(settings(Constants.TENTH_RANK), CStr(_TenthRankPosts)))
				End If
			End If

			' RSS
			If Not settings(Constants.ENABLE_RSS_FEEDS) Is Nothing Then
				If Not settings(Constants.ENABLE_RSS_FEEDS).ToString = String.Empty Then
					_EnableRSS = CBool(GetValue(settings(Constants.ENABLE_RSS_FEEDS), CStr(_EnableRSS)))
				End If
			End If

			If Not settings(Constants.RSS_FEEDS_PER_PAGE) Is Nothing Then
				If Not settings(Constants.RSS_FEEDS_PER_PAGE).ToString = String.Empty Then
					_RSSThreadsPerFeed = CInt(GetValue(settings(Constants.RSS_FEEDS_PER_PAGE), CStr(_RSSThreadsPerFeed)))
				End If
			End If

			If Not settings(Constants.RSS_UPDATE_INTERVAL) Is Nothing Then
				If Not settings(Constants.RSS_UPDATE_INTERVAL).ToString = String.Empty Then
					_RSSUpdateInterval = CInt(GetValue(settings(Constants.RSS_UPDATE_INTERVAL), CStr(_RSSUpdateInterval)))
				End If
			End If

			If Not settings(Constants.FORUM_THEME) Is Nothing Then
				If Not settings(Constants.FORUM_THEME).ToString = String.Empty Then
					_ForumSkin = CStr(GetValue(settings(Constants.FORUM_THEME), CStr(_ForumSkin)))
				End If
			End If

			If Not settings(Constants.DISPLAY_POSTER_LOCATION) Is Nothing Then
				If Not settings(Constants.DISPLAY_POSTER_LOCATION).ToString = String.Empty Then
					_DisplayPosterLocation = _
					 CType( _
					  [Enum].Parse(GetType(ShowPosterLocation), _
					    CStr(GetValue(settings(Constants.DISPLAY_POSTER_LOCATION), _DisplayPosterLocation.ToString))),  _
					  ShowPosterLocation)
				End If
			End If

			If Not settings(Constants.DISPLAY_POSTER_REGION) Is Nothing Then
				If Not settings(Constants.DISPLAY_POSTER_REGION).ToString = String.Empty Then
					_DisplayPosterRegion = CBool(GetValue(settings(Constants.DISPLAY_POSTER_REGION), CStr(_DisplayPosterRegion)))
				End If
			End If

			' Bad words filter
			If Not settings(Constants.ENABLE_WORD_FILTER) Is Nothing Then
				If Not settings(Constants.ENABLE_WORD_FILTER).ToString = String.Empty Then
					_EnableBadWordFilter = CBool(GetValue(settings(Constants.ENABLE_WORD_FILTER), CStr(_EnableBadWordFilter)))
				End If
			End If

			If Not settings(Constants.FILTER_SUBJECTS) Is Nothing Then
				If Not settings(Constants.FILTER_SUBJECTS).ToString = String.Empty Then
					_FilterSubject = CBool(GetValue(settings(Constants.FILTER_SUBJECTS), CStr(_FilterSubject)))
				End If
			End If

			'Community
			If Not settings(Constants.ENABLE_USERS_ONLINE) Is Nothing Then
				If Not settings(Constants.ENABLE_USERS_ONLINE).ToString = String.Empty Then
					_EnableUsersOnline = CBool(GetValue(settings(Constants.ENABLE_USERS_ONLINE), CStr(_EnableUsersOnline)))
				End If
			End If

			If Not settings(Constants.EXTERNAL_PROFILE_PARAM_VALUE) Is Nothing Then
				If Not settings(Constants.EXTERNAL_PROFILE_PARAM_VALUE).ToString = String.Empty Then
					_ExternalProfileParamValue = CStr(GetValue(settings(Constants.EXTERNAL_PROFILE_PARAM_VALUE), CStr(_ExternalProfileParamValue)))
				End If
			End If

			If Not settings(Constants.ENABLE_RATINGS) Is Nothing Then
				If Not settings(Constants.ENABLE_RATINGS).ToString = String.Empty Then
					_EnableRatings = CBool(GetValue(settings(Constants.ENABLE_RATINGS), CStr(_EnableRatings)))
				End If
			End If

			If Not settings(Constants.ENABLE_THREAD_STATUS) Is Nothing Then
				If Not settings(Constants.ENABLE_THREAD_STATUS).ToString = String.Empty Then
					_EnableThreadStatus = CBool(GetValue(settings(Constants.ENABLE_THREAD_STATUS), CStr(_EnableThreadStatus)))
				End If
			End If

			If Not settings(Constants.ENABLE_POST_ABUSE) Is Nothing Then
				If Not settings(Constants.ENABLE_POST_ABUSE).ToString = String.Empty Then
					_EnablePostAbuse = CBool(GetValue(settings(Constants.ENABLE_POST_ABUSE), CStr(_EnablePostAbuse)))
				End If
			End If

			If Not settings(Constants.DISABLE_HTML_POSTING) Is Nothing Then
				If Not settings(Constants.DISABLE_HTML_POSTING).ToString = String.Empty Then
					_DisableHTMLPosting = CBool(GetValue(settings(Constants.DISABLE_HTML_POSTING), CStr(_DisableHTMLPosting)))
				End If
			End If

			If Not settings(Constants.MEMBER_NAME_DISPLAY_FORMAT) Is Nothing Then
				If Not settings(Constants.MEMBER_NAME_DISPLAY_FORMAT).ToString = String.Empty Then
					_ForumMemberName = CInt(GetValue(settings(Constants.MEMBER_NAME_DISPLAY_FORMAT), CStr(_ForumMemberName)))
				End If
			End If

			If Not settings(Constants.TRUST_NEW_USERS) Is Nothing Then
				If Not settings(Constants.TRUST_NEW_USERS).ToString = String.Empty Then
					_TrustNewUsers = CBool(GetValue(settings(Constants.TRUST_NEW_USERS), CStr(_TrustNewUsers)))
				End If
			End If

			If Not settings(Constants.AUTO_LOCK_TRUST) Is Nothing Then
				If Not settings(Constants.AUTO_LOCK_TRUST).ToString = String.Empty Then
					_AutoLockTrust = CBool(GetValue(settings(Constants.AUTO_LOCK_TRUST), CStr(_AutoLockTrust)))
				End If
			End If

			If Not settings(Constants.IMAGE_EXTENSIONS) Is Nothing Then
				If Not settings(Constants.IMAGE_EXTENSIONS).ToString = String.Empty Then
					_ImageExtension = CStr(GetValue(settings(Constants.IMAGE_EXTENSIONS), CStr(_ImageExtension)))
				End If
			End If

			' Attachments
			If Not settings("AnonDownloads") Is Nothing Then
				If Not settings("AnonDownloads").ToString = String.Empty Then
					_AnonDownloads = CBool(GetValue(settings("AnonDownloads"), CStr(_AnonDownloads)))
				End If
			End If

			If Not settings("AttachmentPath") Is Nothing Then
				If Not settings("AttachmentPath").ToString = String.Empty Then
					_AttachmentPath = CStr(GetValue(settings("AttachmentPath"), CStr(_AttachmentPath)))
				End If
			End If

			If Not settings("MaxAttachmentSize") Is Nothing Then
				If Not settings("MaxAttachmentSize").ToString = String.Empty Then
					_MaxAttachmentSize = CInt(GetValue(settings("MaxAttachmentSize"), CStr(_MaxAttachmentSize)))
				End If
			End If

			'Avatar properties
			If Not settings("EnableUserAvatar") Is Nothing Then
				If Not settings("EnableUserAvatar").ToString = String.Empty Then
					_EnableUserAvatar = CBool(GetValue(settings("EnableUserAvatar"), CStr(_EnableUserAvatar)))
				End If
			End If

			If Not settings("EnableProfileUserFolders") Is Nothing Then
				If Not settings("EnableProfileUserFolders").ToString = String.Empty Then
					_EnableProfileUserFolders = CBool(GetValue(settings("EnableProfileUserFolders"), CStr(_EnableProfileUserFolders)))
				End If
			End If

			If Not settings("EnableProfileAvatar") Is Nothing Then
				If Not settings("EnableProfileAvatar").ToString = String.Empty Then
					_EnableProfileAvatar = CBool(GetValue(settings("EnableProfileAvatar"), CStr(_EnableProfileAvatar)))
				End If
			End If

			If Not settings("AvatarProfilePropName") Is Nothing Then
				If Not settings("AvatarProfilePropName").ToString = String.Empty Then
					_AvatarProfilePropName = CStr(GetValue(settings("AvatarProfilePropName"), CStr(_AvatarProfilePropName)))
				End If
			End If

			If Not settings("EnableUserAvatarPool") Is Nothing Then
				If Not settings("EnableUserAvatarPool").ToString = String.Empty Then
					_EnableUserAvatarPool = CBool(GetValue(settings("EnableUserAvatarPool"), CStr(_EnableUserAvatarPool)))
				End If
			End If

			If Not settings("UserAvatarPath") Is Nothing Then
				If Not settings("UserAvatarPath").ToString = String.Empty Then
					_UserAvatarPath = CStr(GetValue(settings("UserAvatarPath"), _UserAvatarPath))
				End If
			End If

			If Not settings("UserAvatarPoolPath") Is Nothing Then
				If Not settings("UserAvatarPoolPath").ToString = String.Empty Then
					_UserAvatarPoolPath = CStr(GetValue(settings("UserAvatarPoolPath"), _UserAvatarPoolPath))
				End If
			End If

			If Not settings("UserAvatarWidth") Is Nothing Then
				If Not settings("UserAvatarWidth").ToString = String.Empty Then
					_UserAvatarWidth = CInt(GetValue(settings("UserAvatarWidth"), CStr(_UserAvatarWidth)))
				End If
			End If

			If Not settings("UserAvatarHeight") Is Nothing Then
				If Not settings("UserAvatarHeight").ToString = String.Empty Then
					_UserAvatarHeight = CInt(GetValue(settings("UserAvatarHeight"), CStr(_UserAvatarHeight)))
				End If
			End If

			If Not settings("UserAvatarMaxSize") Is Nothing Then
				If Not settings("UserAvatarMaxSize").ToString = String.Empty Then
					_UserAvatarMaxSize = CInt(GetValue(settings("UserAvatarMaxSize"), CStr(_UserAvatarMaxSize)))
				End If
			End If

			If Not settings("EnableSystemAvatar") Is Nothing Then
				If Not settings("EnableSystemAvatar").ToString = String.Empty Then
					_EnableSystemAvatar = CBool(GetValue(settings("EnableSystemAvatar"), CStr(_EnableSystemAvatar)))
				End If
			End If

			If Not settings("SystemAvatarPath") Is Nothing Then
				If Not settings("SystemAvatarPath").ToString = String.Empty Then
					_SystemAvatarPath = CStr(GetValue(settings("SystemAvatarPath"), _SystemAvatarPath))
				End If
			End If

			If Not settings("EnableRoleAvatar") Is Nothing Then
				If Not settings("EnableRoleAvatar").ToString = String.Empty Then
					_EnableRoleAvatar = CBool(GetValue(settings("EnableRoleAvatar"), CStr(_EnableRoleAvatar)))
				End If
			End If

			If Not settings("RoleAvatarPath") Is Nothing Then
				If Not settings("RoleAvatarPath").ToString = String.Empty Then
					_RoleAvatarPath = CStr(GetValue(settings("RoleAvatarPath"), _RoleAvatarPath))
				End If
			End If

			If Not settings("MaxPostImageWidth") Is Nothing Then
				If Not settings("MaxPostImageWidth").ToString = String.Empty Then
					_MaxPostImageWidth = CInt(GetValue(settings("MaxPostImageWidth"), CStr(_MaxPostImageWidth)))
				End If
			End If

			If Not settings("EmailAddressDisplayName") Is Nothing Then
				If Not settings("EmailAddressDisplayName").ToString = String.Empty Then
					_EmailAddressDisplayName = CStr(GetValue(settings("EmailAddressDisplayName"), CStr(_EmailAddressDisplayName)))
				End If
			End If

			If Not settings("EnableEmailQueueTask") Is Nothing Then
				If Not settings("EnableEmailQueueTask").ToString = String.Empty Then
					_EnableEmailQueueTask = CBool(GetValue(settings("EnableEmailQueueTask"), CStr(_EnableEmailQueueTask)))
				End If
			End If

			'Rankings
			If Not settings("EnableRankingImage") Is Nothing Then
				If Not settings("EnableRankingImage").ToString = String.Empty Then
					_EnableRankingImage = CBool(GetValue(settings("EnableRankingImage"), CStr(_EnableRankingImage)))
				End If
			End If

			If Not settings("Rank_1_Title") Is Nothing Then
				If Not settings("Rank_1_Title").ToString = String.Empty Then
					_Rank_1_Title = CStr(GetValue(settings("Rank_1_Title"), _Rank_1_Title))
				End If
			End If

			If Not settings("Rank_2_Title") Is Nothing Then
				If Not settings("Rank_2_Title").ToString = String.Empty Then
					_Rank_2_Title = CStr(GetValue(settings("Rank_2_Title"), _Rank_2_Title))
				End If
			End If

			If Not settings("Rank_3_Title") Is Nothing Then
				If Not settings("Rank_3_Title").ToString = String.Empty Then
					_Rank_3_Title = CStr(GetValue(settings("Rank_3_Title"), _Rank_3_Title))
				End If
			End If

			If Not settings("Rank_4_Title") Is Nothing Then
				If Not settings("Rank_4_Title").ToString = String.Empty Then
					_Rank_4_Title = CStr(GetValue(settings("Rank_4_Title"), _Rank_4_Title))
				End If
			End If

			If Not settings("Rank_5_Title") Is Nothing Then
				If Not settings("Rank_5_Title").ToString = String.Empty Then
					_Rank_5_Title = CStr(GetValue(settings("Rank_5_Title"), _Rank_5_Title))
				End If
			End If

			If Not settings("Rank_6_Title") Is Nothing Then
				If Not settings("Rank_6_Title").ToString = String.Empty Then
					_Rank_6_Title = CStr(GetValue(settings("Rank_6_Title"), _Rank_6_Title))
				End If
			End If

			If Not settings("Rank_7_Title") Is Nothing Then
				If Not settings("Rank_7_Title").ToString = String.Empty Then
					_Rank_7_Title = CStr(GetValue(settings("Rank_7_Title"), _Rank_7_Title))
				End If
			End If

			If Not settings("Rank_8_Title") Is Nothing Then
				If Not settings("Rank_8_Title").ToString = String.Empty Then
					_Rank_8_Title = CStr(GetValue(settings("Rank_8_Title"), _Rank_8_Title))
				End If
			End If

			If Not settings("Rank_9_Title") Is Nothing Then
				If Not settings("Rank_9_Title").ToString = String.Empty Then
					_Rank_9_Title = CStr(GetValue(settings("Rank_9_Title"), _Rank_9_Title))
				End If
			End If

			If Not settings("Rank_10_Title") Is Nothing Then
				If Not settings("Rank_10_Title").ToString = String.Empty Then
					_Rank_10_Title = CStr(GetValue(settings("Rank_10_Title"), _Rank_10_Title))
				End If
			End If

			If Not settings("Rank_0_Title") Is Nothing Then
				If Not settings("Rank_0_Title").ToString = String.Empty Then
					_Rank_0_Title = CStr(GetValue(settings("Rank_0_Title"), _Rank_0_Title))
				End If
			End If

			If Not settings("EnableQuickReply") Is Nothing Then
				If Not settings("EnableQuickReply").ToString = String.Empty Then
					_EnableQuickReply = CBool(GetValue(settings("EnableQuickReply"), CStr(_EnableQuickReply)))
				End If
			End If

			If Not settings("EnableUserSignatures") Is Nothing Then
				If Not settings("EnableUserSignatures").ToString = String.Empty Then
					_EnableUserSignatures = CBool(GetValue(settings("EnableUserSignatures"), CStr(_EnableUserSignatures)))
				End If
			End If

			If Not settings("EnableModSigUpdates") Is Nothing Then
				If Not settings("EnableModSigUpdates").ToString = String.Empty Then
					_EnableModSigUpdates = CBool(GetValue(settings("EnableModSigUpdates"), CStr(_EnableModSigUpdates)))
				End If
			End If

			If Not settings("EnableHTMLSignatures") Is Nothing Then
				If Not settings("EnableHTMLSignatures").ToString = String.Empty Then
					_EnableHTMLSignatures = CBool(GetValue(settings("EnableHTMLSignatures"), CStr(_EnableHTMLSignatures)))
				End If
			End If

			If Not settings("PostEditWindow") Is Nothing Then
				If Not settings("PostEditWindow").ToString = String.Empty Then
					_PostEditWindow = CInt(GetValue(settings("PostEditWindow"), CStr(_PostEditWindow)))
				End If
			End If

			If Not settings("HideModEdits") Is Nothing Then
				If Not settings("HideModEdits").ToString = String.Empty Then
					_HideModEdits = CBool(GetValue(settings("HideModEdits"), CStr(_HideModEdits)))
				End If
			End If

			If Not settings("PostPagesCount") Is Nothing Then
				If Not settings("PostPagesCount").ToString = String.Empty Then
					_PostPagesCount = CInt(GetValue(settings("PostPagesCount"), CStr(_PostPagesCount)))
				End If
			End If

			If Not settings("EnableUserBanning") Is Nothing Then
				If Not settings("EnableUserBanning").ToString = String.Empty Then
					_EnableUserBanning = CBool(GetValue(settings("EnableUserBanning"), CStr(_EnableUserBanning)))
				End If
			End If

			If Not settings("EnableEditEmails") Is Nothing Then
				If Not settings("EnableEditEmails").ToString = String.Empty Then
					_EnableEditEmails = CBool(GetValue(settings("EnableEditEmails"), CStr(_EnableEditEmails)))
				End If
			End If

			'SEO
			If Not settings("NoFollowWeb") Is Nothing Then
				If Not settings("NoFollowWeb").ToString = String.Empty Then
					_NoFollowWeb = CBool(GetValue(settings("NoFollowWeb"), CStr(_NoFollowWeb)))
				End If
			End If

			If Not settings("OverrideTitle") Is Nothing Then
				If Not settings("OverrideTitle").ToString = String.Empty Then
					_OverrideTitle = CBool(GetValue(settings("OverrideTitle"), CStr(_OverrideTitle)))
				End If
			End If

			If Not settings("NoFollowLatestThreads") Is Nothing Then
				If Not settings("NoFollowLatestThreads").ToString = String.Empty Then
					_NoFollowLatestThreads = CBool(GetValue(settings("NoFollowLatestThreads"), CStr(_NoFollowLatestThreads)))
				End If
			End If

			If Not settings("OverrideDescription") Is Nothing Then
				If Not settings("OverrideDescription").ToString = String.Empty Then
					_OverrideDescription = CBool(GetValue(settings("OverrideDescription"), CStr(_OverrideDescription)))
				End If
			End If

			If Not settings("SitemapPriority") Is Nothing Then
				If Not settings("SitemapPriority").ToString = String.Empty Then
					_SitemapPriority = CType(GetValue(settings("SitemapPriority"), CStr(_SitemapPriority)), Double)
				End If
			End If

			If Not settings("PrimaryAlias") Is Nothing Then
				If Not settings("PrimaryAlias").ToString = String.Empty Then
					_PrimarySiteAlias = GetValue(settings("PrimaryAlias"), CStr(_PrimarySiteAlias))
				End If
			End If

			If Not settings("RatingScale") Is Nothing Then
				If Not settings("RatingScale").ToString = String.Empty Then
					_RatingScale = CInt(GetValue(settings("RatingScale"), CStr(_RatingScale)))
				End If
			End If

			If Not settings(Constants.ENABLE_USER_READ_MANAGEMENT) Is Nothing Then
				If Not settings(Constants.ENABLE_USER_READ_MANAGEMENT).ToString = String.Empty Then
					_EnableUserReadManagement = CBool(GetValue(settings(Constants.ENABLE_USER_READ_MANAGEMENT), CStr(_EnableUserReadManagement)))
				End If
			End If

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Gets an image path for a forum theeme. 
		''' </summary>
		''' <param name="Image">THe image name.</param>
		''' <param name="IncludeHost">If the host folder directories should be included.</param>
		''' <returns>The full image path including the name of the image.</returns>
		''' <remarks>IncludeHost should always be false (must test)</remarks>
		Public Function GetThemeImageURL(ByVal Image As String, Optional ByVal IncludeHost As Boolean = False) As String
			If IncludeHost Then
				Return ThemeImageFolder(True) & Image
			Else
				Return ThemeImageFolder & Image
			End If
		End Function

		''' <summary>
		''' Used to determine if a valid input is provided, if not, return default value
		''' </summary>
		''' <param name="Input">The object being compared.</param>
		''' <param name="DefaultValue">The default value to use if the input object is nothing.</param>
		''' <returns>The string value to use for the module setting.</returns>
		''' <remarks>This avoids any non-set configuration items and sets defaults for them.
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/11/2006	Created
		''' </history>
		Public Shared Function GetValue(ByVal Input As Object, ByVal DefaultValue As String) As String
			If Input Is Nothing Then
				Return DefaultValue
			Else
				Return CStr(Input)
			End If
		End Function

#End Region

	End Class
End Namespace