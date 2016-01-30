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

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' Options for how a forum behaves. Notifications forums are forced subscriptions. Link forums are just links, no actual forum inside.
	''' </summary>
	''' <remarks>In Lists table</remarks>
	Public Enum ForumType As Integer
		''' <summary>
		''' Normal posting/viewing type forum.
		''' </summary>
		''' <remarks></remarks>
		Normal = 0
		''' <summary>
		''' Similar to normal, except specified roles are forced to receive an email when new threads/posts are created.
		''' </summary>
		''' <remarks></remarks>
		Notification = 1
		''' <summary>
		''' Displays as a forum but is simply a link. Does not contain any post/thread data.
		''' </summary>
		''' <remarks></remarks>
		Link = 2
	End Enum

	''' <summary>
	''' Determines how a forum will behave in terms of viewing and post restrictions. Also includes if a forum is moderated or unmoderated. 
	''' </summary>
	''' <remarks></remarks>
	Public Enum ForumBehavior As Integer
		''' <summary>
		''' Moderated forum available to all who can see module, no posting restrictions beyond being logged on.
		''' </summary>
		''' <remarks></remarks>
		PublicModerated = 0
		''' <summary>
		''' Moderated forum available to all who can see module, posting restrictions are set in forum permissions grid.
		''' </summary>
		''' <remarks></remarks>
		PublicModeratedPostRestricted = 1
		''' <summary>
		''' Unmoderated forum available to all who can see module, no posting restrictions beyond being logged on.
		''' </summary>
		''' <remarks></remarks>
		PublicUnModerated = 2
		''' <summary>
		''' Unmoderated forum available to all who can see module, posting restirctions are set in forum permissions grid.
		''' </summary>
		''' <remarks></remarks>
		PublicUnModeratedPostRestricted = 3
		''' <summary>
		''' Moderated forum that can only be seen by users assigned perms in forum permissions grid.
		''' </summary>
		''' <remarks></remarks>
		PrivateModerated = 4
		''' <summary>
		''' Moderated forum that can only be seen by users assigned perms in forum permissions grid. Posting restrictions beyond being able to view apply here.
		''' </summary>
		''' <remarks></remarks>
		PrivateModeratedPostRestricted = 5
		''' <summary>
		''' Unmoderated forum that can only be seen by users assigned perms in forum permissions grid.
		''' </summary>
		''' <remarks></remarks>
		PrivateUnModerated = 6
		''' <summary>
		''' Unmoderated forum that can only be seen by users assigned perms in forum permissions grid. Posting restrictions beyond being able to view apply here. 
		''' </summary>
		''' <remarks></remarks>
		PrivateUnModeratedPostRestricted = 7
	End Enum

	''' <summary>
	''' The permission keys used for the module level permissions (ie. module settings).
	''' </summary>
	''' <remarks></remarks>
	Public Enum PermissionKeys
		''' <summary>
		''' This user is treated like an admin (ie. content editor)
		''' </summary>
		''' <remarks></remarks>
		FORUMADMIN
		''' <summary>
		''' This person is moderator of all forums they can see. 
		''' </summary>
		''' <remarks></remarks>
		FORUMGLBMOD
	End Enum

	''' <summary>
	''' The PostAction determines how a post is initially loaded in post edit and what other items to display in that screen. It also determines which sproc is called when submitting.
	''' </summary>
	''' <remarks>Not stored anywhere but here.</remarks>
	Public Enum PostAction As Integer
		''' <summary>
		''' New post.
		''' </summary>
		''' <remarks></remarks>
		[New] = 0
		''' <summary>
		''' Edit of an existing post.
		''' </summary>
		''' <remarks></remarks>
		[Edit] = 1
		''' <summary>
		''' Quote (reply) to an existing post.
		''' </summary>
		''' <remarks></remarks>
		[Quote] = 2
		''' <summary>
		''' Reply to an existing post.
		''' </summary>
		''' <remarks></remarks>
		[Reply] = 3
	End Enum

	''' <summary>
	''' ForumScope is used for controlling which class to instantiate in the Forum_Container.ascx dispatch page.
	''' </summary>
	''' <remarks>Used to determine which dynamically rendered UI to render. (ie. ForumThread, ForumPost, ForumGroup, etc)</remarks>
	Public Enum ForumScope As Integer
		''' <summary>
		''' Shows groups.
		''' </summary>
		''' <remarks></remarks>
		[Groups] = 0
		''' <summary>
		''' Shows threads view.
		''' </summary>
		''' <remarks></remarks>
		[Threads] = 2
		''' <summary>
		''' Shows posts view.
		''' </summary>
		''' <remarks></remarks>
		[Posts] = 3
		''' <summary>
		''' Shows thread search results.
		''' </summary>
		''' <remarks></remarks>
		[ThreadSearch] = 4
		''' <summary>
		''' Shows list of threads with new posts since last visit.
		''' </summary>
		''' <remarks>Added by Skeel</remarks>
		[Unread] = 6
		' ''' <summary>
		' ''' Shows search input area.
		' ''' </summary>
		' ''' <remarks></remarks>
		'[SearchInput] = 7
	End Enum

	''' <summary>
	''' Determines the type of action the moderator is performing.
	''' </summary>
	''' <remarks>Not stored anywhere but here.</remarks>
	Public Enum ModerateAction As Integer
		''' <summary>
		''' A post has been submitted for approval.
		''' </summary>
		''' <remarks>Could mean a thread was submitted for approval too (since a post can be the first post of a thread).</remarks>
		[PostSubmit] = 0
		''' <summary>
		''' A post has been approved.
		''' </summary>
		''' <remarks>Could mean a thread was approvaed too (since a post can be the first post of a thread).</remarks>
		[PostApprove] = 1
		''' <summary>
		''' A post has been rejected.
		''' </summary>
		''' <remarks>Inactive</remarks>
		[PostReject] = 2
		''' <summary>
		''' A post has been deleted.
		''' </summary>
		''' <remarks>Could mean a thread was deleted too (since a post can be the first post of a thread).</remarks>
		[PostDelete] = 3
		''' <summary>
		''' A post was edited (could be moderator, could be typical user).
		''' </summary>
		''' <remarks>Not Active</remarks>
		[PostEdit] = 4
		''' <summary>
		''' A thread has been split.
		''' </summary>
		''' <remarks>Means a new thread was created from existing posts.</remarks>
		[ThreadSplit] = 5
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>New for 4.5 or greater.</remarks>
		[ThreadMove] = 6
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>New for 4.5 or greater.</remarks>
		[ThreadDelete] = 7
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>New for 4.5 or greater.</remarks>
		[UserBanned] = 8
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>New for 4.5 or greater.</remarks>
		[UserBanLifted] = 9
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Not Active</remarks>
		[UserSignatureEdit] = 10
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Not Active</remarks>
		[UserTrusted] = 11
		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		[ForumDeleted] = 12
		''' <summary>
		''' The user's post report was addressed, thus it will no longer show up in current reporting unless an unaddressed report remains.
		''' </summary>
		''' <remarks>New for 4.5 or greater.</remarks>
		[PostReportAddressed] = 13
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>New for 4.5 or greater.</remarks>
		[PostMove] = 14
		[ThreadStatusChange] = 15
	End Enum

	''' <summary>
	''' The different levels of user post rankings, this is determined on a per portal basis by post count and settings (if enabled)
	''' </summary>
	''' <remarks>List of possible values for user rankings.</remarks>
	Public Enum PosterRank As Integer
		None = 0
		First = 1
		Second = 2
		Third = 3
		Fourth = 4
		Fifth = 5
		Sixth = 6
		Seventh = 7
		Eigth = 8
		Ninth = 9
		Tenth = 10
	End Enum

	''' <summary>
	''' The compare operator is used for building dynamic SQL for search
	''' </summary>
	''' <remarks>Used for inline SQL queries.</remarks>
	Public Enum CompareOperator As Integer
		''' <summary>
		''' LIKE type comparative operator.
		''' </summary>
		''' <remarks></remarks>
		Contains = 0
		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		NotContains = 1
		''' <summary>
		''' = comparative operator.
		''' </summary>
		''' <remarks></remarks>
		Equal = 2
		''' <summary>
		''' = 
		''' </summary>
		''' <remarks></remarks>
		EqualString = 3
		''' <summary>
		''' Does NOT = comparative operator.
		''' </summary>
		''' <remarks></remarks>
		NotEqual = 4
		NotEqualString = 5
		''' <summary>
		''' Between comparative operator.
		''' </summary>
		''' <remarks></remarks>
		Between = 6
		''' <summary>
		''' Not between comparative operator.
		''' </summary>
		''' <remarks></remarks>
		NotBetween = 7
		''' <summary>
		''' > comparative operator.
		''' </summary>
		''' <remarks></remarks>
		GreaterThan = 8
		''' <summary>
		''' >= comparative operator.
		''' </summary>
		''' <remarks></remarks>
		GreaterThanOrEqualTo = 9
		''' <summary>
		''' Less than comparative operator
		''' </summary>
		''' <remarks></remarks>
		LessThan = 10
		''' <summary>
		''' Less than or = comparative operator
		''' </summary>
		''' <remarks></remarks>
		LessThanOrEqualTo = 11
		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		HaveValueIn = 12
		NotHaveValueIn = 13
		''' <summary>
		''' LIKE % comparative operator
		''' </summary>
		''' <remarks></remarks>
		StartWith = 14
		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		NotStartWith = 15
		''' <summary>
		''' Similar to GreaterThan, specific to dates.
		''' </summary>
		''' <remarks>Doesn't add ' marks so DATEADD can be used</remarks>
		GreaterThanDate = 16
		''' <summary>
		''' Used to add a left parentes "(" to the SQL statement.
		''' </summary>
		''' <remarks></remarks>
		LeftParentes = 17
		''' <summary>
		''' Used to add a right parentes ")" to the SQL statement.
		''' </summary>
		''' <remarks></remarks>
		RightParentes = 18
		''' <summary>
		''' Used to add " AND " to the SQL statement.
		''' </summary>
		''' <remarks></remarks>
		[And] = 19
	End Enum

	''' <summary>
	''' These are the various email template types available for parsing (if active) used in notification emails.
	''' </summary>
	''' <remarks>Reverse lookup is done w/ db (string value used)</remarks>
	Public Enum ForumEmailType
		''' <summary>
		''' An approved post or thread was added.
		''' </summary>
		''' <remarks></remarks>
		UserPostAdded = 1
		''' <summary>
		''' An approved post was edited.
		''' </summary>
		''' <remarks>Right now, no post can be edited unless it is approved.</remarks>
		UserPostEdited = 2
		''' <summary>
		''' Forum moderators has a post to moderate.
		''' </summary>
		''' <remarks></remarks>
		ModeratorPostToModerate = 3
		''' <summary>
		''' A post was approved.
		''' </summary>
		''' <remarks>Only sent to moderated poster.</remarks>
		UserPostApproved = 4
		''' <summary>
		''' A user's post was deleted.
		''' </summary>
		''' <remarks></remarks>
		UserPostDeleted = 5
		''' <summary>
		''' A user's post was rejected.
		''' </summary>
		''' <remarks>Inactive</remarks>
		UserPostRejected = 6
		''' <summary>
		''' A thread was moved to a different forum.
		''' </summary>
		''' <remarks></remarks>
		UserThreadMoved = 7
		''' <summary>
		''' Post(s) in a thread were split into a new thread.
		''' </summary>
		''' <remarks></remarks>
		UserThreadSplit = 8
		''' <summary>
		''' A new thread was created.
		''' </summary>
		''' <remarks>This is handled by post added. Inactive.</remarks>
		UserNewThread = 10
		''' <summary>
		''' A forum was added to a module.
		''' </summary>
		''' <remarks>Inactive</remarks>
		UserForumAdded = 11
		''' <summary>
		''' Moderators are informed that a post has been deleted.
		''' </summary>
		''' <remarks></remarks>
		ModeratorPostDeleted = 12
		''' <summary>
		''' A post was reported to moderators.
		''' </summary>
		''' <remarks></remarks>
		ModeratorPostAbuse = 13
		''' <summary>
		''' User is notified when a warning was submitted for one of their posts.
		''' </summary>
		''' <remarks>Inactive</remarks>
		UserPostAbuse = 15
		''' <summary>
		''' Moderators are informed about a user being warned by a specific moderator.
		''' </summary>
		''' <remarks>Inactive</remarks>
		ModeratorWarnUser = 16
		''' <summary>
		''' Moderators are informed about a user being banned by a specific moderator.
		''' </summary>
		''' <remarks>Inactive</remarks>
		ModeratorUserBaned = 17
		''' <summary>
		''' A user is informed they have been banned from using the forum.
		''' </summary>
		''' <remarks>Inactive</remarks>
		UserBanedNotice = 18
		''' <summary>
		''' A user was warned by a moderator for conduct.
		''' </summary>
		''' <remarks>Inactive</remarks>
		UserModeratorWarning = 19
	End Enum

	''' <summary>
	''' ForumContentType is used for determining which type of keywords should be retrieved from the database. These are used for showing what options are available to be rendered during parsing
	''' </summary>
	''' <remarks>Not stored in DB, but keyword table in db stored as Integer value</remarks>
	Public Enum ForumContentTypeID
		''' <summary>
		''' A forum post or thread.
		''' </summary>
		''' <remarks></remarks>
		POST = 1
		''' <summary>
		''' A communication between moderators.
		''' </summary>
		''' <remarks></remarks>
		MODERATORCOMMUNICATION = 3
		''' <summary>
		''' A post being deleted. 
		''' </summary>
		''' <remarks></remarks>
		DELETEPOST = 4
	End Enum

	''' <summary>
	''' These are the various template types used in this module. This is only partially implemented/active.
	''' </summary>
	''' <remarks>Identity Insert and stored in DB</remarks>
	Public Enum ForumTemplateTypes
		''' <summary>
		''' Email template type
		''' </summary>
		''' <remarks>Placeholder, handled by seperate ForumEmailType</remarks>
		Email = 1
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Inactive</remarks>
		RSS = 2
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Inactive</remarks>
		PostStats = 3
		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		MovePost = 4
		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		DeletePost = 5
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Inactive</remarks>
		WhatsNew = 6
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Inactive</remarks>
		Skin = 7
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Inactive</remarks>
		ModeratorStats = 8
		PostAbuse = 9
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Inactive</remarks>
		ThreadMove = 10
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>Inactive</remarks>
		ThreadSplit = 11
	End Enum

	''' <summary>
	''' The different levels of forum thread status available to end users (if settings is enabled)
	''' </summary>
	''' <remarks>Currently only handles a few options, should expand later. Not stored anywhere in db for lookup.</remarks>
	Public Enum ThreadStatus
		None = -1
		''' <summary>
		''' No set thread status.
		''' </summary>
		''' <remarks>Also used if disbabled.</remarks>
		NotSet = 0
		''' <summary>
		''' An unanswered thread.
		''' </summary>
		''' <remarks></remarks>
		Unanswered = 1
		''' <summary>
		''' An answered thread.
		''' </summary>
		''' <remarks>This status can have an AnswerPostID associated with the Thread.</remarks>
		Answered = 2
		''' <summary>
		''' An informative thread.
		''' </summary>
		''' <remarks></remarks>
		Info = 3
		''' <summary>
		''' A forum poll.
		''' </summary>
		''' <remarks>Can be used even if thread status is disabled. Not in core Lists table.</remarks>
		Poll = 4
	End Enum

	''' <summary>
	''' Options available for displaying poster's location in each post. 
	''' </summary>
	''' <remarks>The various poster location options.</remarks>
	Public Enum ShowPosterLocation
		''' <summary>
		''' No IP/Country will be seen.
		''' </summary>
		''' <remarks></remarks>
		None = 0
		''' <summary>
		''' IP/Country seen by admin.
		''' </summary>
		''' <remarks></remarks>
		ToAdmin = 1
		''' <summary>
		''' All users see IP/Country.
		''' </summary>
		''' <remarks></remarks>
		ToAll = 2
	End Enum

	''' <summary>
	''' The options available for how the user's name is displayed throughout the module.
	''' </summary>
	''' <remarks>The format to display for user names throughout module.</remarks>
	Public Enum ForumDisplayName
		''' <summary>
		''' DNN core username.
		''' </summary>
		''' <remarks></remarks>
		Username = 0
		''' <summary>
		''' DNN core DisplayName.
		''' </summary>
		''' <remarks></remarks>
		DisplayName = 1
	End Enum

	''' <summary>
	''' The various avatar types are handled differently so we need this variable to determine how the control behaves. 
	''' </summary>
	''' <remarks></remarks>
	Public Enum AvatarControlType
		''' <summary>
		''' System avatar is accessible only to module admin.
		''' </summary>
		''' <remarks></remarks>
		System = 0
		''' <summary>
		''' Avatars assigned by role, only accessible to module admin.
		''' </summary>
		''' <remarks></remarks>
		Role = 2
	End Enum

	''' <summary>
	''' This enum represents the available forum control views. All Container named ones load in the Forum_Container.ascx dynamically
	''' </summary>
	''' <remarks></remarks>
	Public Enum ForumPage
		ACP = 0
		ContainerHome = 1
		ContainerSingleGroup = 2
		ContainerThread = 3
		ContainerPost = 4
		ContainerThreadSearch = 5
		ContainerPortalSearch = 6
		' soon to be legacy (below)
		ContainerModQueue = 7
		ContentRemoved = 8
		EmailTemplates = 9
		ForumEdit = 10
		ForumManage = 11
		ForumSearch = 12
		MCP = 13
		MemberList = 14
		PostDelete = 15
		PostEdit = 16
		PostModerate = 17 ' consider load in mcp via ajax
		PostReport = 18
		PublicProfile = 19
		RoleAvatar = 20
		ThreadMove = 21
		ThreadSplit = 22
		UCP = 23
	End Enum

	''' <summary>
	''' The various controls that are loaded into the UCP via Ajax. 
	''' </summary>
	''' <remarks></remarks>
	Public Enum UserAjaxControl
		Main
		Tracking
		Bookmark
		Settings
		Profile
		Avatar
		Signature
	End Enum

	''' <summary>
	''' The various controls that are loaded into the MCP via Ajax.
	''' </summary>
	''' <remarks></remarks>
	Public Enum ModeratorAjaxControl
		Main = 0
		ModQueue = 1
		ReportedPosts = 2
		BannedUsers = 3
		ReportedUsers = 4
		'UntrustedUsers = 5
	End Enum

	''' <summary>
	''' The various controls that are loaded into the ACP via Ajax.
	''' </summary>
	''' <remarks>Because of core issues, there are a few items not available via Aajx.</remarks>
	Public Enum AdminAjaxControl
		Main = 0
		General = 1
		Community = 2
		Attachment = 3
		RSS = 4
		SEO = 5
		ForumManage = 6
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>No Ajax</remarks>
		ForumEdit = 7
		Users = 8
		Avatar = 9
		RoleAvatar = 10
		UserSettings = 11
		UserInterface = 12
		FilterMain = 13
		FilterWord = 14
		Rating = 15
		Ranking = 16
		PopStatus = 17
		EmailSettings = 18
		''' <summary>
		''' 
		''' </summary>
		''' <remarks>No Ajax</remarks>
		EmailTemplate = 19
		EmailQueue = 20
		EmailQueueTaskDetail = 21
        EmailSubscribers = 22
        'Advertisement = 23
	End Enum

	''' <summary>
	''' The type of user avatar being used.
	''' </summary>
	''' <remarks></remarks>
	Public Enum UserAvatarType
		None = 0
		UserAvatar = 1
		PoolAvatar = 2
		ProfileAvatar = 3
	End Enum

	''' <summary>
	''' Depending on what needs to be processed, a unique sum will be generated
	''' eq: Sum of 11 = Emoticons + BBCode + Inline
	''' </summary>
	''' <remarks>[Skeel] Not implemented yet, but will speed up post generation</remarks>
	Friend Enum PostParserInfo As Integer
		''' <summary>
		''' Nothing to Parse
		''' </summary>
		''' <remarks></remarks>
		None = 0
		''' <summary>
		''' Emoticons needs parsing
		''' </summary>
		''' <remarks></remarks>
		Emoticon = 1
		''' <summary>
		''' BBCode needs parsing (quotes and code)
		''' </summary>
		''' <remarks></remarks>
		BBCode = 2
		''' <summary>
		''' Attachment needs parsing
		''' </summary>
		''' <remarks></remarks>
		File = 4
		''' <summary>
		''' Inline Attachment needs parsing
		''' </summary>
		''' <remarks></remarks>
		Inline = 8
	End Enum

	''' <summary>
	''' Provides the status of an attempted post.
	''' </summary>
	''' <remarks></remarks>
	Public Enum PostMessage
		ForumClosed = 0
		ForumDoesntExist = 1
		ForumIsParent = 2
		ForumNoAttachments = 3
		PostApproved = 4
		PostEditExpired = 5
		PostInvalidBody = 6
		PostInvalidSubject = 7
		PostModerated = 8
		ThreadLocked = 9
		UserAttachmentPerms = 10
		UserBanned = 11
		UserCannotEditPost = 12
		UserCannotPostReply = 13
		UserCannotStartThread = 14
		UserCannotViewForum = 15
	End Enum

End Namespace