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
	''' 
	''' </summary>
	''' <remarks></remarks>
	Public Class Constants

#Region "Settings"

		Public Const ENABLE_AGGREGATED_FORUM As String = "AggregatedForums"
		Public Const ENABLE_THREAD_STATUS As String = "EnableThreadStatus"
		Public Const ENABLE_POST_ABUSE As String = "EnablePostAbuse"
		Public Const DISABLE_HTML_POSTING As String = "DisableHTMLPosting"
		Public Const POPULAR_THREAD_VIEWS As String = "PopularThreadView"
		Public Const POPULAR_THREAD_REPLIES As String = "PopularThreadReply"
		Public Const POPULAR_THREAD_DAYS As String = "PopularThreadDays"
		Public Const ENABLE_RANKINGS As String = "Ranking"
		Public Const ENABLE_RANKING_IMAGES As String = "EnableRankingImage"
		Public Const FIRST_RANK As String = "FirstRankPosts"
		Public Const SECOND_RANK As String = "SecondRankPosts"
		Public Const THIRD_RANK As String = "ThirdRankPosts"
		Public Const FOURTH_RANK As String = "FourthRankPosts"
		Public Const FIFTH_RANK As String = "FifthRankPosts"
		Public Const SIXTH_RANK As String = "SixthRankPosts"
		Public Const SEVENTH_RANK As String = "SeventhRankPosts"
		Public Const EIGTH_RANK As String = "EigthRankPosts"
		Public Const NINTH_RANK As String = "NinthRankPosts"
		Public Const TENTH_RANK As String = "TenthRankPosts"
		Public Const RANKING_1_TITLE As String = "Rank_1_Title"
		Public Const RANKING_2_TITLE As String = "Rank_2_Title"
		Public Const RANKING_3_TITLE As String = "Rank_3_Title"
		Public Const RANKING_4_TITLE As String = "Rank_4_Title"
		Public Const RANKING_5_TITLE As String = "Rank_5_Title"
		Public Const RANKING_6_TITLE As String = "Rank_6_Title"
		Public Const RANKING_7_TITLE As String = "Rank_7_Title"
		Public Const RANKING_8_TITLE As String = "Rank_8_Title"
		Public Const RANKING_9_TITLE As String = "Rank_9_Title"
		Public Const RANKING_10_TITLE As String = "Rank_10_Title"
		Public Const RANKING_0_TITLE As String = "Rank_0_Title"
		Public Const ENABLE_RSS_FEEDS As String = "EnableRSS"
		Public Const RSS_FEEDS_PER_PAGE As String = "RSSThreadsPerFeed"
		Public Const RSS_UPDATE_INTERVAL As String = "RSSUpdateInterval"
		Public Const ENABLE_WORD_FILTER As String = "EnableBadWordFilter"
		Public Const FILTER_SUBJECTS As String = "FilterSubject"
		Public Const ENABLE_USERS_ONLINE As String = "EnableUsersOnline"
		Public Const ENABLE_EXTERNAL_PROFILE_PAGE As String = "EnableExternalProfile"
		Public Const EXTERNAL_PROFILE_PAGE As String = "ExternalProfilePage"
		Public Const EXTERNAL_PROFILE_USER_PARAM As String = "ExternalProfileUserParam"
		Public Const EXTERNAL_PROFILE_PARAM_NAME As String = "ExternalProfileParamName"
		Public Const EXTERNAL_PROFILE_PARAM_VALUE As String = "ExternalProfileParamValue"
		Public Const ENABLE_RATINGS As String = "EnableRatings"
		Public Const RATING_SCALE As String = "RatingScale"
		Public Const MEMBER_NAME_DISPLAY_FORMAT As String = "ForumMemberName"
		Public Const POST_EDIT_WINDOW As String = "PostEditWindow"
		Public Const TRUST_NEW_USERS As String = "TrustNewUsers"
		Public Const AUTO_LOCK_TRUST As String = "AutoLockTrust"
		Public Const ENABLE_USER_READ_MANAGEMENT As String = "EnableUserReadManagement"
		Public Const ENABLE_USER_SIGNATURES As String = "EnableUserSignatures"
		Public Const ENABLE_MOD_SIGNATURE_EDITS As String = "EnableModSigUpdates"
		Public Const ENABLE_HTML_IN_SIGNATURES As String = "EnableHTMLSignatures"
		Public Const HIDE_MODERATOR_EDITS As String = "HideModEdits"
		Public Const ENABLE_USER_BANNING As String = "EnableUserBanning"
		Public Const ENABLE_ATTACHMENT As String = "EnableAttachment"
		Public Const ENABLE_ANONYMOUS_DOWNLOADS As String = "AnonDownloads"
		Public Const ATTACHMENT_PATH As String = "AttachmentPath"
		Public Const MAX_ATTACHMENT_SIZE As String = "MaxAttachmentSize"
		Public Const ENABLE_USER_AVATARS As String = "EnableUserAvatar"
		Public Const ENABLE_PROFILE_USER_FOLDERS As String = "EnableProfileUserFolders"
		Public Const ENABLE_PROFILE_AVATAR As String = "EnableProfileAvatar"
		Public Const AVATAR_PROFILE_PROP_NAME As String = "AvatarProfilePropName"
		Public Const ENABLE_USER_AVATAR_POOL As String = "EnableUserAvatarPool"
		Public Const USER_AVATAR_PATH As String = "UserAvatarPath"
		Public Const USER_AVATAR_POOL_PATH As String = "UserAvatarPoolPath"
		Public Const USER_AVATAR_WIDTH As String = "UserAvatarWidth"
		Public Const USER_AVATAR_HEIGHT As String = "UserAvatarHeight"
		Public Const USER_AVATAR_MAX_SIZE As String = "UserAvatarMaxSize"
		Public Const ENABLE_SYSTEM_AVATARS As String = "EnableSystemAvatar"
		Public Const SYSTEM_AVATAR_PATH As String = "SystemAvatarPath"
		Public Const ENABLE_ROLE_AVATARS As String = "EnableRoleAvatar"
		Public Const ROLE_AVATAR_PATH As String = "RoleAvatarPath"
		Public Const EMAIL_AUTO_FROM_ADDRESS As String = "AutomatedEmailAddress"
		Public Const ENABLE_MAIL_NOTIFICATIONS As String = "MailNotification"
		Public Const ENABLE_PER_FORUM_EMAILS As String = "EnablePerForumFrom"
		Public Const EMAIL_ADDRESS_DISPLAY_NAME As String = "EmailAddressDisplayName"
		Public Const ENABLE_EMAILE_SEND_QUEUE As String = "EnableEmailQueueTask"
		Public Const ENABLE_EDIT_EMAILS As String = "EnableEditEmails"
		Public Const ENABLE_LIST_SERVER As String = "EnableListServer"
		Public Const LIST_SERVER_FOLDER As String = "ListServerFolder"
		Public Const THREADS_PER_PAGE As String = "ThreadsPerPage"
		Public Const POSTS_PER_PAGE As String = "PostsPerPage"
		Public Const MAX_POST_IMAGE_WIDTH As String = "MaxPostImageWidth"
		Public Const POST_PAGE_COUNT_LIMIT As String = "PostPagesCount"
		Public Const FORUM_THEME As String = "ForumSkin"
		Public Const IMAGE_EXTENSIONS As String = "ImageExtension"
		Public Const DISPLAY_POSTER_LOCATION As String = "DisplayPosterLocation"
		Public Const DISPLAY_POSTER_REGION As String = "DisplayPosterRegion"
		Public Const ENABLE_QUICK_REPLY As String = "EnableQuickReply"
		Public Const ENABLE_NO_FOLLOW_WEBSITE_LINK As String = "NoFollowWeb"
		Public Const ENABLE_OVERRIDE_PAGE_TITLE As String = "OverrideTitle"
		Public Const ENABLE_NO_FOLLOW_LATEST_THREAD_LINKS As String = "NoFollowLatestThreads"
		Public Const ENABLE_OVERRIDE_PAGE_DESCRIPTION As String = "OverrideDescription"
		Public Const ENABLE_OVERRIDE_PAGE_KEYWORDS As String = "OverrideKeywords"
		Public Const SITEMAP_PRIORITY As String = "SitemapPriority"
		Public Const PRIMARY_SITE_ALIAS As String = "PrimaryAlias"

#End Region

#Region "Misc Constants"

		Public Const HOST_SETTING_USERS_ONLINE As String = "DisableUsersOnline"
		Public Const SOURCE_DIRECTORY As String = "/DesktopModules/Forum"
		Public Const CACHE_KEY_PREFIX As String = "FORUM_MODULE_"
		Public Const CACHE_TIMEOUT As Integer = 20
		Public Const LAST_INDEX_DATE As String = "LastIndexDate"
		Public Const SEO_TITLE_LIMIT As Integer = 64
		Public Const SEO_DESCRIPTION_LIMIT As Integer = 150
		Public Const SEO_KEYWORDS_LIMIT As Integer = 15

#End Region

	End Class

End Namespace