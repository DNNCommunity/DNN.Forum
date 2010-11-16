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
	''' This class contains a set of constants used throughout the module. THese are generally divided into two groups: Settings (for module settings) and misc. items. 
	''' </summary>
	''' <remarks></remarks>
	Public Class Constants

#Region "Settings"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_AGGREGATED_FORUM As String = "AggregatedForums"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_THREAD_STATUS As String = "EnableThreadStatus"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_POST_ABUSE As String = "EnablePostAbuse"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const DISABLE_HTML_POSTING As String = "DisableHTMLPosting"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const POPULAR_THREAD_VIEWS As String = "PopularThreadView"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const POPULAR_THREAD_REPLIES As String = "PopularThreadReply"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const POPULAR_THREAD_DAYS As String = "PopularThreadDays"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_RANKINGS As String = "Ranking"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_RANKING_IMAGES As String = "EnableRankingImage"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const FIRST_RANK As String = "FirstRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const SECOND_RANK As String = "SecondRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const THIRD_RANK As String = "ThirdRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const FOURTH_RANK As String = "FourthRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const FIFTH_RANK As String = "FifthRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const SIXTH_RANK As String = "SixthRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const SEVENTH_RANK As String = "SeventhRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const EIGTH_RANK As String = "EigthRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const NINTH_RANK As String = "NinthRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const TENTH_RANK As String = "TenthRankPosts"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_1_TITLE As String = "Rank_1_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_2_TITLE As String = "Rank_2_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_3_TITLE As String = "Rank_3_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_4_TITLE As String = "Rank_4_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_5_TITLE As String = "Rank_5_Title"
		
		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_6_TITLE As String = "Rank_6_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_7_TITLE As String = "Rank_7_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_8_TITLE As String = "Rank_8_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_9_TITLE As String = "Rank_9_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_10_TITLE As String = "Rank_10_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RANKING_0_TITLE As String = "Rank_0_Title"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_RSS_FEEDS As String = "EnableRSS"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RSS_FEEDS_PER_PAGE As String = "RSSThreadsPerFeed"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RSS_UPDATE_INTERVAL As String = "RSSUpdateInterval"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_WORD_FILTER As String = "EnableBadWordFilter"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const FILTER_SUBJECTS As String = "FilterSubject"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_USERS_ONLINE As String = "EnableUsersOnline"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_EXTERNAL_PROFILE_PAGE As String = "EnableExternalProfile"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const EXTERNAL_PROFILE_PAGE As String = "ExternalProfilePage"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const EXTERNAL_PROFILE_USER_PARAM As String = "ExternalProfileUserParam"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const EXTERNAL_PROFILE_PARAM_NAME As String = "ExternalProfileParamName"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const EXTERNAL_PROFILE_PARAM_VALUE As String = "ExternalProfileParamValue"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_RATINGS As String = "EnableRatings"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const RATING_SCALE As String = "RatingScale"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const MEMBER_NAME_DISPLAY_FORMAT As String = "ForumMemberName"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const POST_EDIT_WINDOW As String = "PostEditWindow"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_AUTO_TRUST As String = "EnableAutoTrust"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const AUTO_TRUST_THRESHOLD As String = "AutoTrustThreashold"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const AUTO_LOCK_TRUST As String = "AutoLockTrust"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_USER_READ_MANAGEMENT As String = "EnableUserReadManagement"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_USER_SIGNATURES As String = "EnableUserSignatures"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_MOD_SIGNATURE_EDITS As String = "EnableModSigUpdates"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_HTML_IN_SIGNATURES As String = "EnableHTMLSignatures"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const HIDE_MODERATOR_EDITS As String = "HideModEdits"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_USER_BANNING As String = "EnableUserBanning"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_ATTACHMENT As String = "EnableAttachment"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_ANONYMOUS_DOWNLOADS As String = "AnonDownloads"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ATTACHMENT_PATH As String = "AttachmentPath"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const MAX_ATTACHMENT_SIZE As String = "MaxAttachmentSize"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_USER_AVATARS As String = "EnableUserAvatar"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_PROFILE_USER_FOLDERS As String = "EnableProfileUserFolders"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_PROFILE_AVATAR As String = "EnableProfileAvatar"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const AVATAR_PROFILE_PROP_NAME As String = "AvatarProfilePropName"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_USER_AVATAR_POOL As String = "EnableUserAvatarPool"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const USER_AVATAR_PATH As String = "UserAvatarPath"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const USER_AVATAR_POOL_PATH As String = "UserAvatarPoolPath"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const USER_AVATAR_WIDTH As String = "UserAvatarWidth"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const USER_AVATAR_HEIGHT As String = "UserAvatarHeight"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const USER_AVATAR_MAX_SIZE As String = "UserAvatarMaxSize"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_SYSTEM_AVATARS As String = "EnableSystemAvatar"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const SYSTEM_AVATAR_PATH As String = "SystemAvatarPath"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_ROLE_AVATARS As String = "EnableRoleAvatar"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ROLE_AVATAR_PATH As String = "RoleAvatarPath"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const EMAIL_AUTO_FROM_ADDRESS As String = "AutomatedEmailAddress"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_MAIL_NOTIFICATIONS As String = "MailNotification"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_PER_FORUM_EMAILS As String = "EnablePerForumFrom"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const EMAIL_ADDRESS_DISPLAY_NAME As String = "EmailAddressDisplayName"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_EMAILE_SEND_QUEUE As String = "EnableEmailQueueTask"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_EDIT_EMAILS As String = "EnableEditEmails"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_LIST_SERVER As String = "EnableListServer"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const LIST_SERVER_FOLDER As String = "ListServerFolder"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const THREADS_PER_PAGE As String = "ThreadsPerPage"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const POSTS_PER_PAGE As String = "PostsPerPage"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const MAX_POST_IMAGE_WIDTH As String = "MaxPostImageWidth"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const POST_PAGE_COUNT_LIMIT As String = "PostPagesCount"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const FORUM_THEME As String = "ForumSkin"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const IMAGE_EXTENSIONS As String = "ImageExtension"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const DISPLAY_POSTER_LOCATION As String = "DisplayPosterLocation"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const DISPLAY_POSTER_REGION As String = "DisplayPosterRegion"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_QUICK_REPLY As String = "EnableQuickReply"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_TAGGING As String = "EnableTagging"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_NO_FOLLOW_WEBSITE_LINK As String = "NoFollowWeb"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_OVERRIDE_PAGE_TITLE As String = "OverrideTitle"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_NO_FOLLOW_LATEST_THREAD_LINKS As String = "NoFollowLatestThreads"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_OVERRIDE_PAGE_DESCRIPTION As String = "OverrideDescription"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const ENABLE_OVERRIDE_PAGE_KEYWORDS As String = "OverrideKeywords"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const SITEMAP_PRIORITY As String = "SitemapPriority"

		''' <summary>
		''' A String used to identify a module setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const PRIMARY_SITE_ALIAS As String = "PrimaryAlias"

#End Region

#Region "Misc Constants"

		''' <summary>
		''' A string that represents the users online host setting.
		''' </summary>
		''' <remarks></remarks>
		Public Const HOST_SETTING_USERS_ONLINE As String = "DisableUsersOnline"

		''' <summary>
		''' The path to the module relative to the root of the website.
		''' </summary>
		''' <remarks></remarks>
		Public Const SOURCE_DIRECTORY As String = "/DesktopModules/Forum"

		''' <summary>
		''' A prefix used for all cached objects to ensure their name is unique from other modules.
		''' </summary>
		''' <remarks></remarks>
		Public Const CACHE_KEY_PREFIX As String = "FORUM_MODULE_"

		''' <summary>
		''' The number of minutes used to calculate cache timeout.
		''' </summary>
		''' <remarks></remarks>
		Public Const CACHE_TIMEOUT As Integer = 20

		''' <summary>
		''' A string used to represent the last time the forum was indexed via ISearchable.
		''' </summary>
		''' <remarks></remarks>
		Public Const LAST_INDEX_DATE As String = "LastIndexDate"

		''' <summary>
		''' The total number of characters permitted for a page's title.
		''' </summary>
		''' <remarks></remarks>
		Public Const SEO_TITLE_LIMIT As Integer = 64

		''' <summary>
		''' The total number of characters permitted for a page's description.
		''' </summary>
		''' <remarks></remarks>
		Public Const SEO_DESCRIPTION_LIMIT As Integer = 150

		''' <summary>
		''' The total number of words permitted for a page's keywords.
		''' </summary>
		''' <remarks></remarks>
		Public Const SEO_KEYWORDS_LIMIT As Integer = 15

#End Region

	End Class

End Namespace