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
Option Explicit On 
Option Strict On

Namespace DotNetNuke.Modules.Forum

#Region "DataProvider"

	''' <summary>
	''' The abstract provider class for the Forum module.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	7/13/2005	Created
	''' </history>
	Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

		' singleton reference to the instantiated object 
		Private Shared objProvider As DataProvider = Nothing

		' constructor
		Shared Sub New()
			CreateProvider()
		End Sub

		' dynamically create provider
		Private Shared Sub CreateProvider()
			objProvider = CType(Framework.Reflection.CreateObject("data", "DotNetNuke.Modules.Forum", ""), DataProvider)
		End Sub

		' return the provider
		Public Shared Shadows Function Instance() As DataProvider
			Return objProvider
		End Function

#End Region

#Region "Attachments"

		Public MustOverride Function Attachment_GetAllByPostID(ByVal PostID As Integer) As IDataReader
		Public MustOverride Function Attachment_GetAllByUserID(ByVal UserID As Integer) As IDataReader
		Public MustOverride Sub Attachment_Update(ByVal objAttachment As AttachmentInfo)

#End Region

#Region "Emoticons"

		Public MustOverride Function Emoticon_GetAll(ByVal ModuleID As Integer, ByVal IsDefault As Boolean) As IDataReader
		Public MustOverride Sub Emoticon_Update(ByVal objEmoticon As EmoticonInfo)
		Public MustOverride Sub Emoticon_Delete(ByVal ID As Integer)
		Public MustOverride Sub Emoticon_SetOrder(ByVal ID As Integer, ByVal ModuleID As Integer, ByVal MoveUp As Boolean)

#End Region

#Region "RoleAvatar"

		Public MustOverride Function RoleAvatar_GetAll(ByVal PortalID As Integer) As IDataReader
		Public MustOverride Function RoleAvatar_GetUsers(ByVal PortalID As Integer, ByVal UserID As Integer) As IDataReader
		Public MustOverride Function RoleAvatar_GetUserRoles(ByVal UserId As Integer) As IDataReader
		Public MustOverride Function RoleAvatar_Get(ByVal RoleId As Integer) As IDataReader
		Public MustOverride Sub RoleAvatar_Delete(ByVal RoleID As Integer)
		Public MustOverride Sub RoleAvatar_Add(ByVal RoleID As Integer, ByVal Avatar As String)
		Public MustOverride Sub RoleAvatar_Update(ByVal RoleID As Integer, ByVal Avatar As String)

#End Region

#Region "Groups"

		Public MustOverride Function GroupGetByModuleID(ByVal ModuleID As Integer) As IDataReader
		Public MustOverride Function GroupGet(ByVal GroupID As Integer) As IDataReader
		Public MustOverride Function GroupAdd(ByVal Name As String, ByVal PortalID As Integer, ByVal ModuleID As Integer, ByVal CreatedByUser As Integer) As Integer
		Public MustOverride Sub GroupUpdate(ByVal GroupID As Integer, ByVal Name As String, ByVal UpdatedByUser As Integer, ByVal SortOrder As Integer, ByVal ModuleID As Integer)
		Public MustOverride Sub GroupDelete(ByVal GroupID As Integer, ByVal ModuleID As Integer)
		Public MustOverride Sub GroupSortOrderUpdate(ByVal GroupID As Integer, ByVal MoveUp As Boolean)

#End Region

#Region "Forums"

		Public MustOverride Function ForumGetMostRecentInfo(ByVal ForumID As Integer) As IDataReader
		Public MustOverride Function ForumGetAll(ByVal GroupID As Integer) As IDataReader
		Public MustOverride Function ForumGetAllByParentID(ByVal ParentID As Integer, ByVal GroupID As Integer, ByVal EnabledOnly As Boolean) As IDataReader
		Public MustOverride Function ForumGet(ByVal ForumID As Integer) As IDataReader
		Public MustOverride Function ForumsGetByModuleID(ByVal ModuleID As Integer) As IDataReader
		Public MustOverride Function ForumAdd(ByVal GroupID As Integer, ByVal IsActive As Boolean, ByVal ParentID As Integer, ByVal Name As String, ByVal Description As String, ByVal IsModerated As Boolean, ByVal EnablePostStatistics As Boolean, ByVal ForumType As Integer, ByVal IsIntegrated As Boolean, ByVal IntegratedModuleID As Integer, ByVal PublicView As Boolean, ByVal CreatedByUserId As Integer, ByVal PublicPosting As Boolean, ByVal EnableForumsThreadStatus As Boolean, ByVal EnableForumsRating As Boolean, ByVal ForumLink As String, ByVal ForumBehavior As Integer, ByVal AllowPolls As Boolean, ByVal EnableRSS As Boolean, ByVal EmailAddress As String, ByVal EmailFriendlyFrom As String, ByVal NotifyByDefault As Boolean, ByVal EmailStatusChange As Boolean, ByVal EmailServer As String, ByVal EmailUser As String, ByVal EmailPass As String, ByVal EmailEnableSSL As Boolean, ByVal EmailAuth As Integer, ByVal EmailPort As Integer) As Integer
		Public MustOverride Sub ForumUpdate(ByVal GroupID As Integer, ByVal ForumID As Integer, ByVal IsActive As Boolean, ByVal ParentID As Integer, ByVal Name As String, ByVal Description As String, ByVal IsModerated As Boolean, ByVal EnablePostStatistics As Boolean, ByVal ForumType As Integer, ByVal IsIntegrated As Boolean, ByVal IntegratedModuleID As Integer, ByVal PublicView As Boolean, ByVal UpdatedByUserId As Integer, ByVal PublicPosting As Boolean, ByVal EnableForumsThreadStatus As Boolean, ByVal EnableForumsRating As Boolean, ByVal ForumLink As String, ByVal ForumBehavior As Integer, ByVal AllowPolls As Boolean, ByVal EnableRSS As Boolean, ByVal EmailAddress As String, ByVal EmailFriendlyFrom As String, ByVal NotifyByDefault As Boolean, ByVal EmailStatusChange As Boolean, ByVal EmailServer As String, ByVal EmailUser As String, ByVal EmailPass As String, ByVal EmailEnableSSL As Boolean, ByVal EmailAuth As Integer, ByVal EmailPort As Integer)
		Public MustOverride Sub ForumDelete(ByVal ForumID As Integer, ByVal GroupID As Integer)
		Public MustOverride Sub ForumSortOrderUpdate(ByVal GroupID As Integer, ByVal ForumID As Integer, ByVal Enable As Boolean)

#End Region

#Region "Search"

		Public MustOverride Function ISearchable(ByVal ModuleID As Integer, ByVal StartDate As DateTime) As IDataReader
		Public MustOverride Function SearchGetResults(ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal FromDate As DateTime, ByVal ToDate As DateTime, ByVal ThreadStatusID As Integer) As IDataReader
		Public MustOverride Function Search(ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal FromDate As DateTime, ByVal ToDate As DateTime, ByVal ThreadStatusID As Integer) As IDataReader

#End Region

#Region "Threads"

		Public MustOverride Function ThreadGetAll(ByVal ModuleID As Integer, ByVal ForumID As Integer, ByVal PageSize As Integer, ByVal pageIndex As Integer, ByVal Filter As String, ByVal PortalID As Integer) As IDataReader
		Public MustOverride Function ThreadGetByForum(ByVal userID As Integer, ByVal ForumID As Integer) As IDataReader
		Public MustOverride Function ThreadGetUnread(ByVal ModuleId As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal LoggedOnUserID As Integer) As IDataReader
		Public MustOverride Function ThreadGet(ByVal ThreadID As Integer) As IDataReader
		Public MustOverride Function ThreadMove(ByVal ThreadID As Integer, ByVal NewForumID As Integer, ByVal ModID As Integer, ByVal Notes As String) As IDataReader
		Public MustOverride Function ThreadSplit(ByVal PostID As Integer, ByVal ThreadID As Integer, ByVal NewForumID As Integer, ByVal ModeratorUserID As Integer, ByVal Subject As String, ByVal Notes As String) As IDataReader
		Public MustOverride Function ThreadGetCount(ByVal ForumID As Integer, ByVal Filter As String) As Integer
		Public MustOverride Sub ThreadViewsIncrement(ByVal ThreadID As Integer)
		Public MustOverride Sub ThreadStatusChange(ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Status As Integer, ByVal AnswerPostID As Integer)

#Region "Rating"

		Public MustOverride Sub ThreadRateAdd(ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Rate As Double)
		Public MustOverride Function ThreadGetUserRating(ByVal ThreadID As Integer, ByVal UserID As Integer) As Integer

#End Region

#End Region

#Region "Posts"

		Public MustOverride Function PostGetAll(ByVal ThreadID As Integer, ByVal ThreadPage As Integer, ByVal postsPerPage As Integer, ByVal TreeView As Boolean, ByVal Descending As Boolean, ByVal PortalID As Integer) As IDataReader
		Public MustOverride Function PostGet(ByVal PostID As Integer, ByVal PortalID As Integer) As IDataReader
		Public MustOverride Function PostAdd(ByVal ParentPostID As Integer, ByVal ForumID As Integer, ByVal UserID As Integer, ByVal RemoteAddr As String, ByVal Notify As Boolean, ByVal Subject As String, ByVal Body As String, ByVal IsPinned As Boolean, ByVal PinnedDate As DateTime, ByVal IsClosed As Boolean, ByVal ObjectID As Integer, ByVal FileAttachmentURL As String, ByVal PortalID As Integer, ByVal ThreadIconID As Integer, ByVal PollID As Integer, ByVal IsModerated As Boolean, ByVal ParseInfo As Integer) As Integer
		Public MustOverride Function PostUpdate(ByVal ThreadID As Integer, ByVal PostID As Integer, ByVal Notify As Boolean, ByVal Subject As String, ByVal Body As String, ByVal IsPinned As Boolean, ByVal PinnedDate As DateTime, ByVal IsClosed As Boolean, ByVal UpdatedBy As Integer, ByVal FileAttachmentURL As String, ByVal PortalID As Integer, ByVal ThreadIconID As Integer, ByVal PollID As Integer, ByVal ParseInfo As Integer) As Integer
		Public MustOverride Sub PostDelete(ByVal PostID As Integer, ByVal ModID As Integer, ByVal Notes As String, ByVal PortalID As Integer)
		Public MustOverride Function PostReportCheck(ByVal PostID As Integer, ByVal UserID As Integer) As Boolean
		Public MustOverride Function PostMove(ByVal PostID As Integer, ByVal oldThreadID As Integer, ByVal newThreadID As Integer, ByVal newForumID As Integer, ByVal oldForumID As Integer, ByVal ModID As Integer, ByVal SortOrder As Integer, ByVal Notes As String) As IDataReader
		Public MustOverride Function PostSortOrderGet(ByVal PostID As Integer, ByVal flatView As Boolean) As Integer
		' Get children Posts (for thread split)
		Public MustOverride Function PostGetChildren(ByVal PostID As Integer) As IDataReader
		Public MustOverride Function PostGetEntireThread(ByVal ThreadID As Integer) As IDataReader
		Public MustOverride Sub PostUpdateParseInfo(ByVal PostID As Integer, ByVal ParseInfo As Integer)

#End Region

#Region "Reported Posts"

		Public MustOverride Function AddPostReport(ByVal PostID As Integer, ByVal UserID As Integer, ByVal Reason As String) As Integer
		Public MustOverride Function CheckPostReport(ByVal PostID As Integer, ByVal UserID As Integer) As Boolean
		Public MustOverride Function GetReportedPosts(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
		Public MustOverride Function GetPostReportDetails(ByVal PostID As Integer) As IDataReader
		Public MustOverride Function AddressPostReport(ByVal PostReportedID As Integer, ByVal UserID As Integer, ByVal PortalID As Integer) As Integer

#End Region

#Region "ReportedUsers"

		Public MustOverride Function GetReportedUsers(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

#End Region

#Region "PrivateMessaging"

#Region "Posts"

		Public MustOverride Function PMGetAll(ByVal PMThreadID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal Descending As Boolean) As IDataReader
		Public MustOverride Function PMGet(ByVal PMID As Integer) As IDataReader
		Public MustOverride Function PMAdd(ByVal ParentPMID As Integer, ByVal PMFromUserID As Integer, ByVal RemoteAddr As String, ByVal Subject As String, ByVal Body As String, ByVal PMToUserID As Integer, ByVal PortalID As Integer) As Integer
		Public MustOverride Sub PMDelete(ByVal PMID As Integer, ByVal PMThreadID As Integer)
		Public MustOverride Sub PMUpdate(ByVal PMID As Integer, ByVal Subject As String, ByVal Body As String)

#End Region

#Region "Threads"

		Public MustOverride Function PMThreadGet(ByVal PMThreadID As Integer) As IDataReader
		Public MustOverride Function PMThreadGetAll(ByVal UserID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal PortalId As Integer) As IDataReader
		Public MustOverride Sub PMThreadDelete(ByVal PMThreadID As Integer, ByVal UserID As Integer)
		Public MustOverride Function PMThreadGetOutBox(ByVal UserID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal PortalId As Integer) As IDataReader

#End Region

#Region "Reads/Views"

		Public MustOverride Function PMReadsGet(ByVal UserID As Integer, ByVal PMThreadID As Integer) As IDataReader
		Public MustOverride Sub PMReadsAdd(ByVal UserID As Integer, ByVal PMThreadID As Integer, ByVal LastVisitDate As Date, ByVal PortalID As Integer)
		Public MustOverride Sub PMReadsUpdate(ByVal UserID As Integer, ByVal PMThreadID As Integer, ByVal LastVisitDate As Date)
		Public MustOverride Function PMGetUserNewMessageCount(ByVal UserID As Integer, ByVal PortalID As Integer) As Integer
		Public MustOverride Function PMGetMessageStatus(ByVal UserId As Integer, ByVal PortalId As Integer) As IDataReader

#End Region

#Region "Users"

		Public MustOverride Function PMUsersGet(ByVal PortalId As Integer, ByVal NameToMatch As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal ByUsername As Boolean, ByVal Specific As Boolean) As IDataReader

#End Region

#End Region

#Region "Moderate"

		Public MustOverride Function ModeratePostGet(ByVal ForumID As Integer) As IDataReader
		Public MustOverride Function ModerateForumGetByModeratorThreads(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PortalID As Integer) As IDataReader
		Public MustOverride Function ModeratePostApprove(ByVal PostID As Integer, ByVal ApprovedBy As Integer, ByVal Notes As String) As Integer
		Public MustOverride Function AddModeratorHistory(ByVal ObjectID As Integer, ByVal PortalID As Integer, ByVal ModeratorID As Integer, ByVal Notes As String, ByVal ActionID As Integer) As Integer

#End Region

#Region "Users"

		Public MustOverride Function UserGetAll(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
		Public MustOverride Function UserGet(ByVal UsersID As Integer, ByVal PortalID As Integer) As IDataReader
		Public MustOverride Function UserGetMultiPortal(ByVal UsersID As Integer) As IDataReader
		Public MustOverride Sub UserAdd(ByVal UserId As Integer, ByVal UserAvatar As Integer, ByVal Avatar As String, ByVal AdditionalAvatars As String, ByVal Signature As String, ByVal IsTrusted As Boolean, ByVal EnableDisplayInMemberList As Boolean, ByVal EnableOnlineStatus As Boolean, ByVal ThreadsPerPage As Integer, ByVal PostsPerPage As Integer, ByVal EnablePublicEmail As Boolean, ByVal EnablePM As Boolean, ByVal EnablePMNotifications As Boolean, ByVal PortalID As Integer)
		Public MustOverride Sub UserUpdate(ByVal UserId As Integer, ByVal UserAvatar As Integer, ByVal Avatar As String, ByVal AdditionalAvatars As String, ByVal Signature As String, ByVal IsTrusted As Boolean, ByVal EnableDisplayInMemberList As Boolean, ByVal EnableOnlineStatus As Boolean, ByVal ThreadsPerPage As Integer, ByVal PostsPerPage As Integer, ByVal EnableModNotification As Boolean, ByVal EnablePublicEmail As Boolean, ByVal EnablePM As Boolean, ByVal EnablePMNotifications As Boolean, ByVal EmailFormat As Integer, ByVal PortalID As Integer, ByVal LockTrust As Boolean, ByVal EnableProfileWeb As Boolean, ByVal EnableProfileRegion As Boolean, ByVal EnableDefaultPostNotify As Boolean, ByVal EnableSelfNotifications As Boolean, ByVal IsBanned As Boolean, ByVal LiftBanDate As Date, ByVal Biography As String, ByVal StartBanDate As Date)
		Public MustOverride Sub UserViewUpdate(ByVal UserId As Integer, ByVal FlatView As Boolean, ByVal ViewDescending As Boolean)

		Public MustOverride Function GetBannedUsers(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

#End Region

#Region "Not Implemented"

		Public MustOverride Sub UserUpdateLastVisit(ByVal UserId As Integer)
		Public MustOverride Sub UserUpdateTrackingDuration(ByVal TrackingDuration As Integer, ByVal UserID As Integer, ByVal PortalID As Integer)

#End Region

#Region "MemberList"

		Public MustOverride Function MembersGetByUsername(ByVal PortalID As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
		Public MustOverride Function MembersGetByDisplayName(ByVal PortalID As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
		Public MustOverride Function MembersGetAll(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
		Public MustOverride Function MembersGetByEmail(ByVal PortalID As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
		Public MustOverride Function MembersGetByProfileProp(ByVal PortalID As Integer, ByVal PropertyName As String, ByVal PropertyValue As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
		Public MustOverride Function MembersGetOnline(ByVal PortalID As Integer) As IDataReader

#End Region

#Region "Manage Users"

		Public MustOverride Function ManageGetAllByDisplayName(ByVal PortalID As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
		Public MustOverride Function ManageGetUsersByRolename(ByVal PortalID As Integer, ByVal RoleName As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

#End Region

#Region "Word Filter"

		Public MustOverride Function FilterWordGetAll(ByVal PortalID As Integer, ByVal Filter As String) As IDataReader
		Public MustOverride Function FilterWordGet(ByVal ItemID As Integer) As IDataReader
		Public MustOverride Sub FilterWordUpdate(ByVal PortalID As Integer, ByVal BadWord As String, ByVal ReplacedWord As String, ByVal CreatedBy As Integer)
		Public MustOverride Sub FilterWordDelete(ByVal ItemID As Integer)
		Public MustOverride Function FilterWordGetByWord(ByVal FilteredWord As String, ByVal PortalID As Integer) As Boolean

#End Region

#Region "UserThreads Methods"

		Public MustOverride Function GetUserThreads(ByVal userID As Integer, ByVal threadID As Integer) As IDataReader
		Public MustOverride Sub AddUserThreads(ByVal userID As Integer, ByVal threadID As Integer, ByVal lastVisitDate As Date)
		Public MustOverride Sub UpdateUserThreads(ByVal userID As Integer, ByVal threadID As Integer, ByVal lastVisitDate As Date)
		Public MustOverride Sub DeleteUserThreads(ByVal userID As Integer, ByVal threadID As Integer)
		Public MustOverride Sub DeleteUserThreadsByForum(ByVal userID As Integer, ByVal forumID As Integer)
		Public MustOverride Function ReadsGetFirstUnread(ByVal ThreadID As Integer, ByVal LastVisitDate As Date, ByVal ViewDecending As Boolean) As Integer

#End Region

#Region "UserForums Methods"

		Public MustOverride Function GetUserForums(ByVal userID As Integer, ByVal ForumID As Integer) As IDataReader
		Public MustOverride Sub AddUserForums(ByVal userID As Integer, ByVal forumID As Integer, ByVal lastVisitDate As DateTime)
		Public MustOverride Sub UpdateUserForums(ByVal userID As Integer, ByVal forumID As Integer, ByVal lastVisitDate As DateTime)
		Public MustOverride Sub DeleteUserForums(ByVal userID As Integer, ByVal forumID As Integer)
		Public MustOverride Sub UserDeleteReads(ByVal userID As Integer)
		Public MustOverride Function GetSubForumIDs(ByVal ParentForumID As Integer) As IDataReader

#End Region

#Region "Forum Permissions"

		Public MustOverride Function GetPermissionsByForumID(ByVal ForumID As Integer) As IDataReader
		Public MustOverride Function GetForumPermission(ByVal ForumPermissionID As Integer) As IDataReader
		Public MustOverride Function GetForumPermissionsCollection(ByVal ForumID As Integer, ByVal PermissionID As Integer) As IDataReader
		Public MustOverride Function GetForumPermissions(ByVal ForumID As Integer) As IDataReader
		Public MustOverride Sub DeleteForumPermissionsByForumID(ByVal ForumID As Integer)
		Public MustOverride Sub DeleteForumPermission(ByVal ForumPermissionID As Integer)
		Public MustOverride Function AddForumPermission(ByVal ForumID As Integer, ByVal PermissionID As Integer, ByVal roleID As Integer, ByVal AllowAccess As Boolean, ByVal UserID As Integer) As Integer
		Public MustOverride Sub UpdateForumPermission(ByVal ForumPermissionID As Integer, ByVal ForumID As Integer, ByVal PermissionID As Integer, ByVal roleID As Integer, ByVal AllowAccess As Boolean, ByVal UserID As Integer)

#End Region

#Region "Email Templates"

		Public MustOverride Function GetEmailTemplatesByModuleID(ByVal ModuleID As Integer) As IDataReader
		Public MustOverride Function GetEmailTemplate(ByVal EmailTemplateID As Integer) As IDataReader
		Public MustOverride Function GetEmailTemplateForMail(ByVal ModuleID As Integer, ByVal ForumEmailTypeID As Integer) As IDataReader
		Public MustOverride Function GetDefaultEmailTemplates() As IDataReader
		Public MustOverride Function AddEmailTemplateForModuleID(ByVal ForumTemplateTypeID As Integer, ByVal EmailSubject As String, ByVal HTMLBody As String, ByVal TextBody As String, ByVal ModuleID As Integer, ByVal IsActive As Boolean, ByVal ForumContentTypeID As Integer, ByVal EmailTemplateName As String, ByVal ForumEmailTypeID As Integer) As Integer
		Public MustOverride Sub UpdateEmailTemplate(ByVal EmailTemplateID As Integer, ByVal EmailSubject As String, ByVal HTMLBody As String, ByVal TextBody As String, ByVal IsActive As Boolean, ByVal ModuleID As Integer)

		'Keywords
		Public MustOverride Function GetKeywordsByType(ByVal ContentTypeID As Integer) As IDataReader

		' Distribution Lists
		Public MustOverride Function GetNewThreadDistributionList(ByVal ThreadID As Integer) As IDataReader
		Public MustOverride Function GetPrivateMessageDistributionList(ByVal PMID As Integer) As IDataReader
		Public MustOverride Function GetModeratorDistributionList(ByVal ForumID As Integer, ByVal ModuleID As Integer) As IDataReader

		'GetThreadMovedDisbributionList - Not implemented in db yet
		Public MustOverride Function GetNotificationForumDistributionList(ByVal ForumID As Integer) As IDataReader
		Public MustOverride Function GetModeratorMessageDistributionList(ByVal ForumID As Integer) As IDataReader
		Public MustOverride Function GetSingleUserDistribution(ByVal ForumID As Integer, ByVal ModuleID As Integer) As IDataReader

#End Region

#Region "Forum Templates"

		Public MustOverride Function TemplatesGetByType(ByVal ModuleID As Integer, ByVal ForumTemplateTypeID As Integer) As IDataReader
		Public MustOverride Function TemplatesGetDefaults(ByVal ForumTemplateTypeID As Integer) As IDataReader
		Public MustOverride Function TemplatesAddForModuleID(ByVal TemplateName As String, ByVal TemplateValue As String, ByVal ForumTemplateTypeID As Integer, ByVal ModuleID As Integer, ByVal IsActive As Boolean) As Integer
		Public MustOverride Sub TemplatesUpdate(ByVal TemplateID As Integer, ByVal TemplateName As String, ByVal TemplateValue As String, ByVal ForumTemplateTypeID As Integer, ByVal ModuleID As Integer, ByVal IsActive As Boolean)
		Public MustOverride Function TemplatesGetSingle(ByVal TemplateID As Integer) As IDataReader

#End Region

#Region "Email Queue"

#Region "Tasks"

		Public MustOverride Function EmailQueueTaskAdd(ByVal EmailFromAddress As String, ByVal FromFriendlyName As String, ByVal EmailPriority As Integer, ByVal EmailHTMLBody As String, ByVal EmailTextBody As String, ByVal EmailSubject As String, ByVal PortalID As Integer, ByVal QueuePriority As Integer, ByVal ModuleID As Integer, ByVal EnableFriendlyToName As Boolean, ByVal DistroCall As String, ByVal DistroIsSproc As Boolean, ByVal DistroParams As String, ByVal ScheduleStartDate As Date, ByVal PersonalizeEmail As Boolean, ByVal Attachment As String) As Integer
		Public MustOverride Function EmailQueueTaskGet(ByVal EmailQueueID As Integer) As IDataReader
		Public MustOverride Function EmailQueueTaskGetNext() As IDataReader
		Public MustOverride Sub EmailQueueTaskCompleted(ByVal EmailQueueID As Integer)
		Public MustOverride Sub EmailQueueTaskMarkFailed(ByVal EmailQueueID As Integer)
		Public MustOverride Sub EmailQueueTaskStart(ByVal EmailQueueID As Integer)
		Public MustOverride Sub EmailQueueTaskCleanEmails(ByVal DeleteDate As Date)
		Public MustOverride Sub EmailQueueTaskCleanTasks(ByVal DeleteDate As Date)
		Public MustOverride Function EmailQueueTaskScheduleItemIDGet(ByVal DeleteTask As Boolean) As Integer
		Public MustOverride Function GetPortalEmailSendTasks(ByVal PortalID As Integer) As IDataReader

#End Region

#Region "Task Emails"

		Public MustOverride Function TaskEmailsGetIncomplete(ByVal EmailQueueID As Integer) As Boolean
		Public MustOverride Function TaskEmailsToSendGet(ByVal EmailQueueID As Integer) As IDataReader
		Public MustOverride Sub TaskEmailsSendStatusUpdate(ByVal EmailQueueID As Integer, ByVal EmailAddress As String, ByVal Failed As Boolean)
		Public MustOverride Function TaskEmailsSprocInsertToSend(ByVal SprocName As String, ByVal params As String, ByVal EmailQueueID As Integer) As IDataReader
		Public MustOverride Function TaskEmailsGet(ByVal EmailQueueID As Integer) As IDataReader

#End Region

#End Region

#Region "Tracking"

		Public MustOverride Function TrackingForumGet(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As IDataReader
		Public MustOverride Function TrackingThreadGet(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As IDataReader
		Public MustOverride Sub TrackingForumDeleteAll(ByVal UserID As Integer, ByVal ModuleID As Integer)
		Public MustOverride Sub TrackingForumCreateDelete(ByVal ForumID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)
		Public MustOverride Sub TrackingThreadCreateDelete(ByVal ForumID As Integer, ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)

#End Region

#Region "User Tracking"

		Public MustOverride Function GetForumSubscribers(ByVal ForumID As Integer) As IDataReader
		Public MustOverride Function GetThreadSubscribers(ByVal ThreadID As Integer) As IDataReader

#End Region

#Region "Bookmark"

		Public MustOverride Function BookmarkThreadGet(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal ForumMemberName As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As IDataReader
		Public MustOverride Sub BookmarkCreateDelete(ByVal ForumID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)
		Public MustOverride Function BookmarkCheck(ByVal UserID As Integer, ByVal ThreadID As Integer, ByVal ModuleID As Integer) As Boolean

#End Region

#Region "Upgrade"

		Public MustOverride Function Upgrade_GetForumPerms() As IDataReader
		Public MustOverride Function Upgrade_GetPermByKey(ByVal PermKey As String) As IDataReader
		Public MustOverride Function Upgrade_GetForumMods() As IDataReader

#End Region

#Region "Polls"

		' Polls table
		Public MustOverride Function GetPoll(ByVal PollID As Integer) As IDataReader
		Public MustOverride Function AddPoll(ByVal Question As String, ByVal ShowResults As Boolean, ByVal EndDate As DateTime, ByVal TakenMessage As String, ByVal ModuleID As Integer) As Integer
		Public MustOverride Sub UpdatePoll(ByVal PollID As Integer, ByVal Question As String, ByVal ShowResults As Boolean, ByVal EndDate As DateTime, ByVal TakenMessage As String)
		Public MustOverride Sub DeletePoll(ByVal PollID As Integer)
		Public MustOverride Function GetOrphanedPolls(ByVal ModuleID As Integer) As IDataReader

		' Answers table
		Public MustOverride Function GetAnswer(ByVal AnswerID As Integer) As IDataReader
		Public MustOverride Function GetPollAnswers(ByVal PollID As Integer) As IDataReader
		Public MustOverride Function AddAnswer(ByVal PollID As Integer, ByVal Answer As String, ByVal SortOrder As Integer) As Integer
		Public MustOverride Sub UpdateAnswer(ByVal AnswerID As Integer, ByVal PollID As Integer, ByVal Answer As String, ByVal SortOrder As Integer)
		Public MustOverride Sub DeleteAnswer(ByVal AnswerID As Integer)

		' UserAnswers table
		Public MustOverride Function GetUserAnswers(ByVal PollID As Integer) As IDataReader
		Public MustOverride Function AddUserAnswer(ByVal PollID As Integer, ByVal UserID As Integer, ByVal AnswerID As Integer) As Integer

#End Region

		Public MustOverride Function GetModulesPortalID(ByVal ModuleID As Integer) As Integer

	End Class

#End Region

End Namespace



