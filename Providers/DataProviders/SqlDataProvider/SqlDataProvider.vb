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

Imports Microsoft.ApplicationBlocks.Data

Namespace DotNetNuke.Modules.Forum

#Region "SqlDataProvider"

	''' <summary>
	''' The Microsoft SQL specific provider to allow Forum module to communicate with a data store. 
	''' </summary>
	''' <remarks></remarks>
	Public Class SqlDataProvider
		Inherits DataProvider

#Region "Private Members"

		Private Const ProviderType As String = "data"
		Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
		Private _connectionString As String
		Private _providerPath As String
		Private _objectQualifier As String
		Private _databaseOwner As String
		Private _moduleDataPrefix As String = "Forum_"

#End Region

#Region "Constructors"

		Public Sub New()
			' Read the configuration specific information for this provider
			Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

			' This code handles getting the connection string from either the connectionString / appsetting section and uses the connectionstring section by default if it exists.  
			' Get Connection string from web.config
			_connectionString = DotNetNuke.Common.Utilities.Config.GetConnectionString()

			' If above funtion does not return anything then connectionString must be set in the dataprovider section.
			If _connectionString = String.Empty Then
				' Use connection string specified in provider
				_connectionString = objProvider.Attributes("connectionString")
			End If

			_providerPath = objProvider.Attributes("providerPath")

			_objectQualifier = objProvider.Attributes("objectQualifier")
			If _objectQualifier <> String.Empty And _objectQualifier.EndsWith("_") = False Then
				_objectQualifier += "_"
			End If

			_databaseOwner = objProvider.Attributes("databaseOwner")
			If _databaseOwner <> String.Empty And _databaseOwner.EndsWith(".") = False Then
				_databaseOwner += "."
			End If
		End Sub

#End Region

#Region "Properties"

		Public ReadOnly Property ConnectionString() As String
			Get
				Return _connectionString
			End Get
		End Property

		Public ReadOnly Property ProviderPath() As String
			Get
				Return _providerPath
			End Get
		End Property

		Public ReadOnly Property ObjectQualifier() As String
			Get
				Return _objectQualifier
			End Get
		End Property

		Public ReadOnly Property DatabaseOwner() As String
			Get
				Return _databaseOwner
			End Get
		End Property

#End Region

#Region "Public Methods"

#Region "General Public Methods"
		Private Function GetNull(ByVal Field As Object) As Object
			Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
		End Function
#End Region

#Region "Attachments"

		Public Overrides Function Attachment_GetAllByPostID(ByVal PostID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Attachment_GetAllByPostID", PostID), IDataReader)
		End Function

		Public Overrides Function Attachment_GetAllByUserID(ByVal UserID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Attachment_GetAllByUserID", UserID), IDataReader)
		End Function

		Public Overrides Sub Attachment_Update(ByVal objAttachment As AttachmentInfo)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Attachment_Update", objAttachment.AttachmentID, objAttachment.FileID, objAttachment.PostID, objAttachment.UserID, objAttachment.LocalFileName, objAttachment.Inline)
		End Sub

#End Region

#Region "Emoticons"

		Public Overrides Function Emoticon_GetAll(ByVal ModuleID As Integer, ByVal IsDefault As Boolean) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Emoticon_GetAll", ModuleID, IsDefault), IDataReader)
		End Function

		Public Overrides Sub Emoticon_Update(ByVal objEmoticon As EmoticonInfo)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Emoticon_Update", objEmoticon.ID, objEmoticon.Code, objEmoticon.Emoticon, objEmoticon.SortOrder, objEmoticon.ToolTip, objEmoticon.IsDefault, objEmoticon.ModuleID)
		End Sub

		Public Overrides Sub Emoticon_Delete(ByVal ID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Emoticon_Delete", ID)
		End Sub

		Public Overrides Sub Emoticon_SetOrder(ByVal ID As Integer, ByVal ModuleID As Integer, ByVal MoveUp As Boolean)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Emoticon_SetOrder", ID, ModuleID, MoveUp)
		End Sub

#End Region

#Region "RoleAvatar"

		Public Overrides Function RoleAvatar_GetAll(ByVal PortalID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Avatar_Role_GetAll", PortalID), IDataReader)
		End Function

		Public Overrides Function RoleAvatar_GetUsers(ByVal PortalID As Integer, ByVal UserID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Avatar_Role_GetUsers", PortalID, UserID), IDataReader)
		End Function

		Public Overrides Sub RoleAvatar_Delete(ByVal RoleID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Avatar_Role_Delete", RoleID)
		End Sub
		Public Overrides Sub RoleAvatar_Add(ByVal RoleID As Integer, ByVal Avatar As String)
			SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "RoleAvatar_Add", RoleID, Avatar)
		End Sub
		Public Overrides Sub RoleAvatar_Update(ByVal RoleID As Integer, ByVal Avatar As String)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Avatar_Role_Update", RoleID, Avatar)
		End Sub

		Public Overrides Function RoleAvatar_GetUserRoles(ByVal UserId As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Avatar_Role_GetUserRoles", UserId), IDataReader)
		End Function

		Public Overrides Function RoleAvatar_Get(ByVal RoleId As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "RoleAvatar_Get", RoleId), IDataReader)
		End Function

#End Region

#Region "Groups"

		Public Overrides Function GroupGetByModuleID(ByVal ModuleID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Group_GetModule", ModuleID), IDataReader)
		End Function

		Public Overrides Function GroupGet(ByVal GroupID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Group_Get", GroupID), IDataReader)
		End Function

		Public Overrides Sub GroupDelete(ByVal GroupID As Integer, ByVal ModuleID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Group_Delete", GroupID, ModuleID)
		End Sub

		Public Overrides Function GroupAdd(ByVal Name As String, ByVal PortalID As Integer, ByVal ModuleID As Integer, ByVal CreatedByUser As Integer) As Integer
			Try
				Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Group_Add", Name, PortalID, ModuleID, CreatedByUser), Integer)
			Catch ex As Exception ' duplicate
				LogException(ex)
				Return -1
			End Try
		End Function

		Public Overrides Sub GroupUpdate(ByVal GroupID As Integer, ByVal Name As String, ByVal UpdatedByUser As Integer, ByVal SortOrder As Integer, ByVal ModuleID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Group_Update", GroupID, Name, UpdatedByUser, SortOrder, ModuleID)
		End Sub

		Public Overrides Sub GroupSortOrderUpdate(ByVal GroupID As Integer, ByVal MoveUp As Boolean)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Group_SetOrder", GroupID, MoveUp)
		End Sub

#End Region

#Region "Forums"

		Public Overrides Function ForumGetMostRecentInfo(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_GetMostRecentInfo", ForumID), IDataReader)
		End Function

		Public Overrides Function ForumGetAllByParentID(ByVal ParentID As Integer, ByVal GroupID As Integer, ByVal EnabledOnly As Boolean) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_GetAllByParentID", ParentID, GroupID, EnabledOnly), IDataReader)
		End Function

		Public Overrides Function ForumGetAll(ByVal GroupID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_GetAll", GroupID), IDataReader)
		End Function

		Public Overrides Function ForumGet(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_Get", ForumID), IDataReader)
		End Function

		Public Overrides Function ForumsGetByModuleID(ByVal ModuleID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_GetModule", ModuleID), IDataReader)
		End Function

		Public Overrides Sub ForumDelete(ByVal ForumID As Integer, ByVal GroupID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_Delete", ForumID, GroupID)
		End Sub

		Public Overrides Function ForumAdd(ByVal GroupID As Integer, ByVal IsActive As Boolean, ByVal ParentID As Integer, ByVal Name As String, ByVal Description As String, ByVal IsModerated As Boolean, ByVal EnablePostStatistics As Boolean, ByVal ForumType As Integer, ByVal IsIntegrated As Boolean, ByVal IntegratedModuleID As Integer, ByVal PublicView As Boolean, ByVal CreatedByUserId As Integer, ByVal PublicPosting As Boolean, ByVal EnableForumsThreadStatus As Boolean, ByVal EnableForumsRating As Boolean, ByVal ForumLink As String, ByVal ForumBehavior As Integer, ByVal AllowPolls As Boolean, ByVal EnableRSS As Boolean, ByVal EmailAddress As String, ByVal EmailFriendlyFrom As String, ByVal NotifyByDefault As Boolean, ByVal EmailStatusChange As Boolean, ByVal EmailServer As String, ByVal EmailUser As String, ByVal EmailPass As String, ByVal EmailEnableSSL As Boolean, ByVal EmailAuth As Integer, ByVal EmailPort As Integer) As Integer
			Try
				Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_Add", GroupID, IsActive, ParentID, Name, Description, IsModerated, EnablePostStatistics, ForumType, IsIntegrated, IntegratedModuleID, PublicView, CreatedByUserId, PublicPosting, EnableForumsThreadStatus, EnableForumsRating, ForumLink, ForumBehavior, AllowPolls, EnableRSS, EmailAddress, EmailFriendlyFrom, NotifyByDefault, EmailStatusChange, EmailServer, EmailUser, EmailPass, EmailEnableSSL, EmailAuth, EmailPort), Integer)
			Catch
				Return -1
			End Try
		End Function

		Public Overrides Sub ForumUpdate(ByVal GroupID As Integer, ByVal ForumID As Integer, ByVal IsActive As Boolean, ByVal ParentID As Integer, ByVal Name As String, ByVal Description As String, ByVal IsModerated As Boolean, ByVal EnablePostStatistics As Boolean, ByVal ForumType As Integer, ByVal IsIntegrated As Boolean, ByVal IntegratedModuleID As Integer, ByVal PublicView As Boolean, ByVal UpdatedByUserId As Integer, ByVal PublicPosting As Boolean, ByVal EnableForumsThreadStatus As Boolean, ByVal EnableForumsRating As Boolean, ByVal ForumLink As String, ByVal ForumBehavior As Integer, ByVal AllowPolls As Boolean, ByVal EnableRSS As Boolean, ByVal EmailAddress As String, ByVal EmailFriendlyFrom As String, ByVal NotifyByDefault As Boolean, ByVal EmailStatusChange As Boolean, ByVal EmailServer As String, ByVal EmailUser As String, ByVal EmailPass As String, ByVal EmailEnableSSL As Boolean, ByVal EmailAuth As Integer, ByVal EmailPort As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_Update", GroupID, ForumID, IsActive, ParentID, Name, Description, IsModerated, EnablePostStatistics, ForumType, IsIntegrated, IntegratedModuleID, PublicView, UpdatedByUserId, PublicPosting, EnableForumsThreadStatus, EnableForumsRating, ForumLink, ForumBehavior, AllowPolls, EnableRSS, EmailAddress, EmailFriendlyFrom, NotifyByDefault, EmailStatusChange, EmailServer, EmailUser, EmailPass, EmailEnableSSL, EmailAuth, EmailPort)
		End Sub

		Public Overrides Sub ForumSortOrderUpdate(ByVal GroupID As Integer, ByVal ForumID As Integer, ByVal MoveUp As Boolean)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Forum_SetOrder", GroupID, ForumID, MoveUp)
		End Sub

#End Region

#Region "Threads"

		Public Overrides Function ThreadGetAll(ByVal ModuleID As Integer, ByVal ForumID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As String, ByVal PortalID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Thread_GetAll", ModuleID, ForumID, PageSize, PageIndex, Filter, PortalID), IDataReader)
		End Function

		Public Overrides Function ThreadGetUnread(ByVal ModuleId As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal LoggedOnUserID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Thread_GetUnread", ModuleId, PageSize, PageIndex, LoggedOnUserID), IDataReader)
		End Function

		Public Overrides Function ThreadGetByForum(ByVal UserID As Integer, ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Thread_GetByForum", ForumID, UserID), IDataReader)
		End Function

		Public Overrides Function ThreadGet(ByVal ThreadID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Thread_Get", ThreadID), IDataReader)
		End Function

		Public Overrides Function ThreadMove(ByVal ThreadID As Integer, ByVal NewForumID As Integer, ByVal ModID As Integer, ByVal Notes As String) As IDataReader
			Return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Thread_Move", ThreadID, NewForumID, ModID, Notes)
		End Function

		Public Overrides Function ThreadSplit(ByVal PostID As Integer, ByVal ThreadID As Integer, ByVal NewForumID As Integer, ByVal ModeratorUserID As Integer, ByVal Subject As String, ByVal Notes As String) As IDataReader
			Return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Thread_Split", PostID, ThreadID, NewForumID, ModeratorUserID, Subject, Notes)
		End Function

		Public Overrides Function ThreadGetCount(ByVal ForumID As Integer, ByVal Filter As String) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "ThreadGetCount", ForumID, Filter), Integer)
		End Function

		Public Overrides Sub ThreadViewsIncrement(ByVal ThreadID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "ThreadViewsIncrement", ThreadID)
		End Sub

		' Thread Status
		Public Overrides Sub ThreadStatusChange(ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Status As Integer, ByVal AnswerPostID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "ThreadStatusChange", ThreadID, UserID, Status, AnswerPostID)
		End Sub

		Public Overrides Function ReadsGetFirstUnread(ByVal ThreadID As Integer, ByVal LastVisitDate As Date, ByVal ViewDecending As Boolean) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Reads_GetFirstUnread", ThreadID, LastVisitDate, ViewDecending), Integer)
		End Function

#Region "Rating"

		Public Overrides Sub ThreadRateAdd(ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Rate As Double)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "ThreadRating_Add", ThreadID, UserID, Rate)
		End Sub

		Public Overrides Function ThreadGetUserRating(ByVal ThreadID As Integer, ByVal UserID As Integer) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "ThreadRating_GetUser", ThreadID, UserID), Integer)
		End Function

#End Region

#End Region

#Region "Posts"

		Public Overrides Function PostGetAll(ByVal ThreadID As Integer, ByVal threadPage As Integer, ByVal postsPerPage As Integer, ByVal TreeView As Boolean, ByVal Descending As Boolean, ByVal PortalID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_GetAll", ThreadID, threadPage, postsPerPage, TreeView, Descending, PortalID), IDataReader)
		End Function

		Public Overrides Function PostGetEntireThread(ByVal ThreadID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_GetEntireThread", ThreadID), IDataReader)
		End Function

		Public Overrides Function PostGetChildren(ByVal PostID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PostGetChildren", PostID), IDataReader)
		End Function

		Public Overrides Function PostGet(ByVal PostID As Integer, ByVal PortalID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Get", PostID, PortalID), IDataReader)
		End Function

		Public Overrides Sub PostDelete(ByVal PostID As Integer, ByVal ModID As Integer, ByVal Notes As String, ByVal PortalID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Delete", PostID, ModID, Notes, PortalID)
		End Sub

		Public Overrides Function PostReportCheck(ByVal PostID As Integer, ByVal UserID As Integer) As Boolean
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PostReportCheck", PostID, UserID), Boolean)
		End Function

		Public Overrides Function PostMove(ByVal PostID As Integer, ByVal oldThreadID As Integer, ByVal newThreadID As Integer, ByVal newForumID As Integer, ByVal oldForumID As Integer, ByVal ModID As Integer, ByVal SortOrder As Integer, ByVal Notes As String) As IDataReader
			Return SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Move", PostID, oldThreadID, newThreadID, newForumID, oldForumID, ModID, SortOrder, Notes)
		End Function

		Public Overrides Function PostAdd(ByVal ParentPostID As Integer, ByVal ForumID As Integer, ByVal UserID As Integer, ByVal RemoteAddr As String, ByVal Notify As Boolean, ByVal Subject As String, ByVal Body As String, ByVal IsPinned As Boolean, ByVal PinnedDate As DateTime, ByVal IsClosed As Boolean, ByVal ObjectID As Integer, ByVal FileAttachmentURL As String, ByVal PortalID As Integer, ByVal ThreadIconID As Integer, ByVal PollID As Integer, ByVal IsModerated As Boolean, ByVal ParseInfo As Integer) As Integer
			Try
				Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Add", ParentPostID, ForumID, UserID, RemoteAddr, Notify, Subject, Body, IsPinned, GetNull(PinnedDate), IsClosed, ObjectID, FileAttachmentURL, PortalID, ThreadIconID, PollID, IsModerated, ParseInfo), Integer)
			Catch ex As Exception ' duplicate
				ex.ToString()
				Return Nothing
			End Try
		End Function

		Public Overrides Function PostUpdate(ByVal ThreadID As Integer, ByVal PostID As Integer, ByVal Notify As Boolean, ByVal Subject As String, ByVal Body As String, ByVal IsPinned As Boolean, ByVal PinnedDate As DateTime, ByVal IsClosed As Boolean, ByVal UpdatedBy As Integer, ByVal FileAttachmentURL As String, ByVal PortalID As Integer, ByVal ThreadIconID As Integer, ByVal PollID As Integer, ByVal ParseInfo As Integer) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Update", ThreadID, PostID, Notify, Subject, Body, IsPinned, PinnedDate, IsClosed, UpdatedBy, FileAttachmentURL, PortalID, ThreadIconID, PollID, ParseInfo), Integer)
		End Function

		Public Overrides Function PostSortOrderGet(ByVal PostID As Integer, ByVal flatView As Boolean) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PostSortOrderGet", PostID, flatView), Integer)
		End Function

		Public Overrides Sub PostUpdateParseInfo(ByVal PostID As Integer, ByVal ParseInfo As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_UpdateParseInfo", PostID, ParseInfo)
		End Sub

#End Region

#Region "Reported Posts"

		Public Overrides Function AddPostReport(ByVal PostID As Integer, ByVal UserID As Integer, ByVal Reason As String) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Reported_Add", PostID, UserID, Reason), Integer)
		End Function

		Public Overrides Function CheckPostReport(ByVal PostID As Integer, ByVal UserID As Integer) As Boolean
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Reported_Check", PostID, UserID), Boolean)
		End Function

		Public Overrides Function GetReportedPosts(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Reported_Get", PortalID, PageIndex, PageSize), IDataReader)
		End Function

		Public Overrides Function GetPostReportDetails(ByVal PostID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Reported_GetDetail", PostID), IDataReader)
		End Function

		Public Overrides Function AddressPostReport(ByVal PostReportedID As Integer, ByVal UserID As Integer, ByVal PortalID As Integer) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Reported_Address", PostReportedID, UserID, PortalID), Integer)
		End Function

#End Region

#Region "Reported Users"

		Public Overrides Function GetReportedUsers(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Post_Reported_GetUsers", PortalID, PageIndex, PageSize), IDataReader)
		End Function

#End Region

#Region "Search"

		' Site Search indexing
		Public Overrides Function ISearchable(ByVal ModuleID As Integer, ByVal StartDate As DateTime) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "ISearchable", ModuleID, GetNull(StartDate)), IDataReader)
		End Function

		' Search within the module
		Public Overrides Function SearchGetResults(ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal FromDate As DateTime, ByVal ToDate As DateTime, ByVal ThreadStatusID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "SearchGetResults", Filter, PageIndex, PageSize, UserID, ModuleID, FromDate, ToDate, ThreadStatusID), IDataReader)
		End Function

		' Search within the module
		Public Overrides Function Search(ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal FromDate As DateTime, ByVal ToDate As DateTime, ByVal ThreadStatusID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Search", Filter, PageIndex, PageSize, UserID, ModuleID, FromDate, ToDate, ThreadStatusID), IDataReader)
		End Function

#End Region

#Region "Moderation"

		Public Overrides Function ModeratePostGet(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Moderate_PostGet", ForumID), IDataReader)
		End Function

		Public Overrides Function ModerateForumGetByModeratorThreads(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PortalID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Moderate_GetModeratedForums", UserID, ModuleID, PortalID), IDataReader)
		End Function

		Public Overrides Function ModeratePostApprove(ByVal PostID As Integer, ByVal ApprovedBy As Integer, ByVal Notes As String) As Integer
			Return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Moderate_Approve", PostID, ApprovedBy, Notes))
		End Function

#End Region

#Region "Private Messaging"

#Region "Posts"

		Public Overrides Function PMGetAll(ByVal PMThreadID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal Descending As Boolean) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_GetAll", PMThreadID, PageIndex, PageSize, Descending), IDataReader)
		End Function

		Public Overrides Function PMGet(ByVal PMID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_Get", PMID), IDataReader)
		End Function

		Public Overrides Function PMAdd(ByVal ParentPMID As Integer, ByVal PMFromUserID As Integer, ByVal RemoteAddr As String, ByVal Subject As String, ByVal Body As String, ByVal PMToUserID As Integer, ByVal PortalID As Integer) As Integer
			Try
				Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_Add", ParentPMID, PMFromUserID, RemoteAddr, Subject, Body, PMToUserID, PortalID), Integer)
			Catch Ex As Exception ' duplicate
				DotNetNuke.Services.Exceptions.LogException(Ex)
				Return Nothing
			End Try
		End Function

		Public Overrides Sub PMDelete(ByVal PMID As Integer, ByVal PMThreadID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_Delete", PMID, PMThreadID)
		End Sub

		Public Overrides Sub PMUpdate(ByVal PMID As Integer, ByVal Subject As String, ByVal Body As String)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_Update", PMID, Subject, Body)
		End Sub

#End Region

#Region "Threads"

		Public Overrides Function PMThreadGet(ByVal PMThreadID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_ThreadGet", PMThreadID), IDataReader)
		End Function

		Public Overrides Function PMThreadGetAll(ByVal UserID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal PortalId As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_ThreadGetAll", UserID, PageSize, PageIndex, PortalId), IDataReader)
		End Function

		Public Overrides Sub PMThreadDelete(ByVal PMThreadID As Integer, ByVal UserID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_ThreadDelete", PMThreadID, UserID)
		End Sub

		Public Overrides Function PMThreadGetOutBox(ByVal UserID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal PortalId As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_ThreadGetOutBox", UserID, PageSize, PageIndex, PortalId), IDataReader)
		End Function

#End Region

#Region "Reads/Views"

		Public Overrides Function PMReadsGet(ByVal UserID As Integer, ByVal PMThreadID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_ReadsGet", UserID, PMThreadID), IDataReader)
		End Function

		Public Overrides Sub PMReadsAdd(ByVal UserID As Integer, ByVal PMThreadID As Integer, ByVal LastVisitDate As Date, ByVal PortalID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_ReadsAdd", UserID, PMThreadID, LastVisitDate, PortalID)
		End Sub

		Public Overrides Sub PMReadsUpdate(ByVal UserID As Integer, ByVal PMThreadID As Integer, ByVal LastVisitDate As Date)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_ReadsUpdate", UserID, PMThreadID, GetNull(LastVisitDate))
		End Sub

		Public Overrides Function PMGetUserNewMessageCount(ByVal UserID As Integer, ByVal PortalID As Integer) As Integer
			Return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_GetUserNewMessageCount", UserID, PortalID))
		End Function

		Public Overrides Function PMGetMessageStatus(ByVal UserId As Integer, ByVal PortalId As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_GetMessageStatus", UserId, PortalId), IDataReader)
		End Function
#End Region

#Region "Users"

		Public Overrides Function PMUsersGet(ByVal PortalId As Integer, ByVal NameToMatch As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal ByUsername As Boolean, ByVal Specific As Boolean) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "PM_UsersGet", PortalId, NameToMatch, PageIndex, PageSize, ByUsername, Specific), IDataReader)
		End Function

#End Region

#End Region

#Region "Users"

		Public Overrides Function UserGet(ByVal UserID As Integer, ByVal PortalID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "User_Get", UserID, PortalID), IDataReader)
		End Function

		Public Overrides Sub UserUpdate(ByVal UserId As Integer, ByVal UserAvatar As Integer, ByVal Avatar As String, ByVal AdditionalAvatars As String, ByVal Signature As String, ByVal IsTrusted As Boolean, ByVal EnableDisplayInMemberList As Boolean, ByVal EnableOnlineStatus As Boolean, ByVal ThreadsPerPage As Integer, ByVal PostsPerPage As Integer, ByVal EnableModNotification As Boolean, ByVal EnablePublicEmail As Boolean, ByVal EnablePM As Boolean, ByVal EnablePMNotifications As Boolean, ByVal EmailFormat As Integer, ByVal PortalID As Integer, ByVal LockTrust As Boolean, ByVal EnableProfileWeb As Boolean, ByVal EnableProfileRegion As Boolean, ByVal EnableDefaultPostNotify As Boolean, ByVal EnableSelfNotifications As Boolean, ByVal IsBanned As Boolean, ByVal LiftBanDate As Date, ByVal Biography As String, ByVal StartBanDate As Date)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "User_Update", UserId, UserAvatar, Avatar, AdditionalAvatars, Signature, IsTrusted, EnableDisplayInMemberList, EnableOnlineStatus, ThreadsPerPage, PostsPerPage, EnableModNotification, EnablePublicEmail, EnablePM, EnablePMNotifications, EmailFormat, PortalID, LockTrust, EnableProfileWeb, EnableProfileRegion, EnableDefaultPostNotify, EnableSelfNotifications, IsBanned, GetNull(LiftBanDate), Biography, GetNull(StartBanDate))
		End Sub

		' Requires db rename (to include _, Search) - Used only for search user lookup control
		Public Overrides Function UserGetAll(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "UserGetAll", PortalID, PageIndex, PageSize), IDataReader)
		End Function

		Public Overrides Function UserGetMultiPortal(ByVal UserID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "User_GetMultiPortal", UserID), IDataReader)
		End Function

		Public Overrides Sub UserAdd(ByVal UserId As Integer, ByVal UserAvatar As Integer, ByVal Avatar As String, ByVal AdditionalAvatars As String, ByVal Signature As String, ByVal IsTrusted As Boolean, ByVal EnableDisplayInMemberList As Boolean, ByVal EnableOnlineStatus As Boolean, ByVal ThreadsPerPage As Integer, ByVal PostsPerPage As Integer, ByVal EnablePublicEmail As Boolean, ByVal EnablePM As Boolean, ByVal EnablePMNotifications As Boolean, ByVal PortalID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "User_Add", UserId, UserAvatar, Avatar, AdditionalAvatars, Signature, IsTrusted, EnableDisplayInMemberList, EnableOnlineStatus, ThreadsPerPage, PostsPerPage, EnablePublicEmail, EnablePM, EnablePMNotifications, PortalID)
		End Sub
		' Requires db rename (to include _)
		Public Overrides Sub UserViewUpdate(ByVal UserId As Integer, ByVal FlatView As Boolean, ByVal ViewDescending As Boolean)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "UserViewUpdate", UserId, FlatView, ViewDescending)
		End Sub

#Region "Banning"

		Public Overrides Function GetBannedUsers(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "User_GetBanned", PortalID, PageIndex, PageSize), IDataReader)
		End Function

#End Region

#Region "Not Implemented"

		Public Overrides Sub UserUpdateLastVisit(ByVal UserId As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "UserUpdateLastVisit", UserId)
		End Sub

		Public Overrides Sub UserUpdateTrackingDuration(ByVal TrackingDuration As Integer, ByVal UserID As Integer, ByVal PortalID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "UserUpdateTrackingDuration", TrackingDuration, UserID, PortalID)
		End Sub

#End Region

#Region "MemberList"

		Public Overrides Function MembersGetAll(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Members_GetAll", PortalID, PageIndex, PageSize), IDataReader)
		End Function

		Public Overrides Function MembersGetByUsername(ByVal PortalID As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Members_GetByUsername", PortalID, Filter, PageIndex, PageSize), IDataReader)
		End Function

		Public Overrides Function MembersGetByDisplayName(ByVal PortalID As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Members_GetByDisplayName", PortalID, Filter, PageIndex, PageSize), IDataReader)
		End Function

		Public Overrides Function MembersGetByEmail(ByVal PortalID As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Members_GetByEmail", PortalID, Filter, PageIndex, PageSize), IDataReader)
		End Function

		Public Overrides Function MembersGetByProfileProp(ByVal PortalID As Integer, ByVal PropertyName As String, ByVal PropertyValue As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Members_GetByProfileProp", PortalID, PropertyName, PropertyValue, PageIndex, PageSize), IDataReader)
		End Function

		Public Overrides Function MembersGetOnline(ByVal PortalID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Members_GetOnline", PortalID), IDataReader)
		End Function

#End Region

#Region "Manage Users"

		Public Overrides Function ManageGetAllByDisplayName(ByVal PortalID As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "ManageUsers_GetAllByDisplayName", PortalID, Filter, PageIndex, PageSize), IDataReader)
		End Function

		Public Overrides Function ManageGetUsersByRolename(ByVal PortalID As Integer, ByVal RoleName As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "ManageUsers_GetUsersByRolename", PortalID, RoleName, PageIndex, PageSize), IDataReader)
		End Function

#End Region

#Region "UserForums Methods"
		Public Overrides Function GetSubForumIDs(ByVal ParentForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_Forum_GetSubForumId", ParentForumID), IDataReader)
		End Function

		Public Overrides Function GetUserForums(ByVal UserID As Integer, ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserForumsGet", UserID, ForumID), IDataReader)
		End Function

		Public Overrides Sub AddUserForums(ByVal userID As Integer, ByVal forumID As Integer, ByVal lastVisitDate As DateTime)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserForumsAdd", userID, forumID, GetNull(lastVisitDate))
		End Sub

		Public Overrides Sub UpdateUserForums(ByVal userID As Integer, ByVal forumID As Integer, ByVal lastVisitDate As DateTime)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserForumsUpdate", userID, forumID, GetNull(lastVisitDate))
		End Sub

		Public Overrides Sub DeleteUserForums(ByVal userID As Integer, ByVal forumID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserForumsDelete", userID, forumID)
		End Sub

		Public Overrides Sub UserDeleteReads(ByVal userID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserDeleteReads", userID)
		End Sub

#End Region

#Region "UserThreads Methods"

		Public Overrides Function GetUserThreads(ByVal userID As Integer, ByVal threadID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserThreadsGet", userID, threadID), IDataReader)
		End Function

		Public Overrides Sub AddUserThreads(ByVal userID As Integer, ByVal threadID As Integer, ByVal lastVisitDate As Date)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserThreadsAdd", userID, threadID, lastVisitDate)
		End Sub

		Public Overrides Sub UpdateUserThreads(ByVal userID As Integer, ByVal threadID As Integer, ByVal lastVisitDate As Date)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserThreadsUpdate", userID, threadID, GetNull(lastVisitDate))
		End Sub

		Public Overrides Sub DeleteUserThreads(ByVal userID As Integer, ByVal threadID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserThreadsDelete", userID, threadID)
		End Sub

		Public Overrides Sub DeleteUserThreadsByForum(ByVal userID As Integer, ByVal forumID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & "Forum_UserThreadsDeleteByForum", userID, forumID)
		End Sub

#End Region

#End Region

#Region "Filters"

		Public Overrides Function FilterWordGetAll(ByVal PortalID As Integer, ByVal Filter As String) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "FilterWordGetAll", PortalID, Filter), IDataReader)
		End Function

		Public Overrides Function FilterWordGet(ByVal ItemID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "FilterWordGet", ItemID), IDataReader)
		End Function

		Public Overrides Sub FilterWordUpdate(ByVal PortalID As Integer, ByVal BadWord As String, ByVal ReplacedWord As String, ByVal CreatedBy As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "FilterWordUpdate", PortalID, BadWord, ReplacedWord, CreatedBy)
		End Sub

		Public Overrides Sub FilterWordDelete(ByVal ItemID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "FilterWordDelete", ItemID)
		End Sub

		Public Overrides Function FilterWordGetByWord(ByVal FilteredWord As String, ByVal PortalID As Integer) As Boolean
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "FilterWordGetByWord", FilteredWord, PortalID), Boolean)
		End Function

#End Region

#Region "Forum Permissions"

		Public Overrides Function GetPermissionsByForumID(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Permissions_GetByForumID", ForumID), IDataReader)
		End Function


		Public Overrides Function GetForumPermission(ByVal ForumPermissionID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Permissions_GetForum", ForumPermissionID), IDataReader)
		End Function

		Public Overrides Function GetForumPermissionsCollection(ByVal ForumID As Integer, ByVal PermissionID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Permissions_GetCollection", ForumID, PermissionID), IDataReader)
		End Function

		Public Overrides Function GetForumPermissions(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Permissions_GetForumPermissions", ForumID), IDataReader)
		End Function

		Public Overrides Sub DeleteForumPermissionsByForumID(ByVal ForumID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Permissions_DeleteByForumID", ForumID)
		End Sub

		Public Overrides Sub DeleteForumPermission(ByVal ForumPermissionID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Permissions_DeleteForum", ForumPermissionID)
		End Sub

		Public Overrides Function AddForumPermission(ByVal ForumID As Integer, ByVal PermissionID As Integer, ByVal roleID As Integer, ByVal AllowAccess As Boolean, ByVal UserID As Integer) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Permissions_AddForum", ForumID, PermissionID, roleID, AllowAccess, UserID), Integer)
		End Function

		Public Overrides Sub UpdateForumPermission(ByVal ForumPermissionID As Integer, ByVal ForumID As Integer, ByVal PermissionID As Integer, ByVal roleID As Integer, ByVal AllowAccess As Boolean, ByVal UserID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Permissions_UpdateForum", ForumPermissionID, ForumID, PermissionID, roleID, AllowAccess, UserID)
		End Sub

#End Region

#Region "Email Templates"

		Public Overrides Function GetEmailTemplatesByModuleID(ByVal ModuleID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailTemplatesByModuleIDGet", ModuleID), IDataReader)
		End Function

		Public Overrides Function GetEmailTemplate(ByVal EmailTemplateID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailTemplateGet", EmailTemplateID), IDataReader)
		End Function

		Public Overrides Function GetEmailTemplateForMail(ByVal ModuleID As Integer, ByVal ForumEmailTypeID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailTemplatesByModuleAndTypeGet", ModuleID, ForumEmailTypeID), IDataReader)
		End Function

		Public Overrides Function GetDefaultEmailTemplates() As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailTemplatesDefaultsGet"), IDataReader)
		End Function

		Public Overrides Function AddEmailTemplateForModuleID(ByVal ForumTemplateTypeID As Integer, ByVal EmailSubject As String, ByVal HTMLBody As String, ByVal TextBody As String, ByVal ModuleID As Integer, ByVal IsActive As Boolean, ByVal ForumContentTypeID As Integer, ByVal EmailTemplateName As String, ByVal ForumEmailTypeID As Integer) As Integer
			Return CType(SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailTemplateForModuleIDAdd", ForumTemplateTypeID, EmailSubject, HTMLBody, TextBody, ModuleID, IsActive, ForumContentTypeID, EmailTemplateName, ForumEmailTypeID), Integer)
		End Function

		Public Overrides Sub UpdateEmailTemplate(ByVal EmailTemplateID As Integer, ByVal EmailSubject As String, ByVal HTMLBody As String, ByVal TextBody As String, ByVal IsActive As Boolean, ByVal ModuleID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailTemplateUpdate", EmailTemplateID, EmailSubject, HTMLBody, TextBody, IsActive)
		End Sub

		'Keywords
		Public Overrides Function GetKeywordsByType(ByVal ContentTypeID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "KeywordsByTypeGet", ContentTypeID), IDataReader)
		End Function

		' Distribution Lists
		Public Overrides Function GetNewThreadDistributionList(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Subscriptions_NewThread", ForumID), IDataReader)
		End Function

		Public Overrides Function GetModeratorDistributionList(ByVal ForumID As Integer, ByVal ModuleID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Subscriptions_ForumModerator", ForumID, ModuleID), IDataReader)
		End Function

		Public Overrides Function GetPrivateMessageDistributionList(ByVal PMID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Subscriptions_PrivateMessage", PMID), IDataReader)
		End Function


		'Not in db yet, so not implemented
		Public Overrides Function GetSingleUserDistribution(ByVal ForumID As Integer, ByVal ModuleID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Subscriptions_ForumModerator", ForumID, ModuleID), IDataReader)
		End Function

		Public Overrides Function GetNotificationForumDistributionList(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Subscriptions_NotificationForum", ForumID), IDataReader)
		End Function

		Public Overrides Function GetModeratorMessageDistributionList(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Subscriptions_ModeratorMessage", ForumID), IDataReader)
		End Function

#End Region

#Region "Forum Templates"

		Public Overrides Function TemplatesGetByType(ByVal ModuleID As Integer, ByVal ForumTemplateTypeID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "TemplatesGetByType", ModuleID, ForumTemplateTypeID), IDataReader)
		End Function

		Public Overrides Function TemplatesGetDefaults(ByVal ForumTemplateTypeID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "TemplatesGetDefaults", ForumTemplateTypeID), IDataReader)
		End Function

		Public Overrides Function TemplatesAddForModuleID(ByVal TemplateName As String, ByVal TemplateValue As String, ByVal ForumTemplateTypeID As Integer, ByVal ModuleID As Integer, ByVal IsActive As Boolean) As Integer
			Return CType(SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "TemplatesAddForModuleID", TemplateName, TemplateValue, ForumTemplateTypeID, ModuleID, IsActive), Integer)
		End Function

		Public Overrides Sub TemplatesUpdate(ByVal TemplateID As Integer, ByVal TemplateName As String, ByVal TemplateValue As String, ByVal ForumTemplateTypeID As Integer, ByVal ModuleID As Integer, ByVal IsActive As Boolean)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "TemplatesUpdate", TemplateID, TemplateName, TemplateValue, ForumTemplateTypeID, ModuleID, IsActive)
		End Sub

		Public Overrides Function TemplatesGetSingle(ByVal TemplateID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "TemplatesGetSingle", TemplateID), IDataReader)
		End Function

#End Region

#Region "Email Queue"

#Region "Tasks"

		Public Overrides Function EmailQueueTaskAdd(ByVal EmailFromAddress As String, ByVal FromFriendlyName As String, ByVal EmailPriority As Integer, ByVal EmailHTMLBody As String, ByVal EmailTextBody As String, ByVal EmailSubject As String, ByVal PortalID As Integer, ByVal QueuePriority As Integer, ByVal ModuleID As Integer, ByVal EnableFriendlyToName As Boolean, ByVal DistroCall As String, ByVal DistroIsSproc As Boolean, ByVal DistroParams As String, ByVal ScheduleStartDate As Date, ByVal PersonalizeEmail As Boolean, ByVal Attachment As String) As Integer
			Return CType(SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskAdd", EmailFromAddress, FromFriendlyName, EmailPriority, EmailHTMLBody, EmailTextBody, EmailSubject, PortalID, QueuePriority, ModuleID, EnableFriendlyToName, DistroCall, DistroIsSproc, DistroParams, GetNull(ScheduleStartDate), PersonalizeEmail, Attachment), Integer)
		End Function

		Public Overrides Function EmailQueueTaskGet(ByVal EmailQueueID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskGet", EmailQueueID), IDataReader)
		End Function

		Public Overrides Function EmailQueueTaskGetNext() As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskGetNext"), IDataReader)
		End Function

		Public Overrides Sub EmailQueueTaskCompleted(ByVal EmailQueueID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskCompleted", EmailQueueID)
		End Sub

		Public Overrides Sub EmailQueueTaskMarkFailed(ByVal EmailQueueID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskMarkFailed", EmailQueueID)
		End Sub

		Public Overrides Sub EmailQueueTaskStart(ByVal EmailQueueID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskStart", EmailQueueID)
		End Sub

		Public Overrides Sub EmailQueueTaskCleanEmails(ByVal DeleteDate As Date)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskCleanEmails", DeleteDate)
		End Sub

		Public Overrides Sub EmailQueueTaskCleanTasks(ByVal DeleteDate As Date)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskCleanTasks", DeleteDate)
		End Sub

		Public Overrides Function EmailQueueTaskScheduleItemIDGet(ByVal DeleteTask As Boolean) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskScheduleItemIDGet", DeleteTask), Integer)
		End Function

		Public Overrides Function GetPortalEmailSendTasks(ByVal PortalID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_GetPortalSendTasks", PortalID), IDataReader)
		End Function

#End Region

#Region "Task Emails"

		Public Overrides Function TaskEmailsGetIncomplete(ByVal EmailQueueID As Integer) As Boolean
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskEmailsGetIncomplete", EmailQueueID), Boolean)
		End Function

		Public Overrides Function TaskEmailsToSendGet(ByVal EmailQueueID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskEmailsToSendGet", EmailQueueID), IDataReader)
		End Function

		Public Overrides Sub TaskEmailsSendStatusUpdate(ByVal EmailQueueID As Integer, ByVal EmailAddress As String, ByVal Failed As Boolean)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskEmailsSendStatusUpdate", EmailQueueID, EmailAddress, Failed)
		End Sub

		Public Overrides Function TaskEmailsSprocInsertToSend(ByVal SprocName As String, ByVal params As String, ByVal EmailQueueID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & SprocName, params, EmailQueueID), IDataReader)
		End Function

		Public Overrides Function TaskEmailsGet(ByVal EmailQueueID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "EmailQueue_TaskEmailsGet", EmailQueueID), IDataReader)
		End Function

#End Region

#End Region

#Region "Tracking"

		Public Overrides Function TrackingForumGet(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Tracking_ForumGet", UserID, ModuleID, PageSize, PageIndex), IDataReader)
		End Function

		Public Overrides Function TrackingThreadGet(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Tracking_ThreadGet", UserID, ModuleID, PageSize, PageIndex), IDataReader)
		End Function

		Public Overrides Sub TrackingForumDeleteAll(ByVal UserID As Integer, ByVal ModuleID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Tracking_ForumDeleteAll", UserID, ModuleID)
		End Sub

		Public Overrides Sub TrackingForumCreateDelete(ByVal ForumID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Tracking_ForumCreateDelete", ForumID, UserID, Add, ModuleID)
		End Sub

		Public Overrides Sub TrackingThreadCreateDelete(ByVal ForumID As Integer, ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Tracking_ThreadCreateDelete", ForumID, ThreadID, UserID, Add, ModuleID)
		End Sub

#End Region

#Region "User Tracking"

		Public Overrides Function GetForumSubscribers(ByVal ForumID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "UserTracking_GetForum", ForumID), IDataReader)
		End Function

		Public Overrides Function GetThreadSubscribers(ByVal ThreadID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "UserTracking_GetThread", ThreadID), IDataReader)
		End Function

#End Region

#Region "Bookmark"

		Public Overrides Function BookmarkThreadGet(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal ForumMemberName As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Bookmark_Threads_Get", UserID, ModuleID, ForumMemberName, PageSize, PageIndex), IDataReader)
		End Function

		Public Overrides Sub BookmarkCreateDelete(ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Bookmark_Threads_CreateDelete", ThreadID, UserID, Add, ModuleID)
		End Sub

		Public Overrides Function BookmarkCheck(ByVal UserID As Integer, ByVal ThreadID As Integer, ByVal ModuleID As Integer) As Boolean
			Try
				Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Bookmark_Threads_Check", UserID, ThreadID, ModuleID), Boolean)
			Catch
				Return False
			End Try
		End Function

#End Region

#Region "Upgrade"

		Public Overrides Function Upgrade_GetForumPerms() As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Upgrade_GetForumPerms"), IDataReader)
		End Function

		Public Overrides Function Upgrade_GetPermByKey(ByVal PermKey As String) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Upgrade_GetPermByKey", PermKey), IDataReader)
		End Function

		Public Overrides Function Upgrade_GetForumMods() As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Upgrade_GetForumMods"), IDataReader)
		End Function

#End Region

#Region "Polls"

		' Polls table
		Public Overrides Function GetPoll(ByVal PollID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_PollGet", PollID), IDataReader)
		End Function

		Public Overrides Function AddPoll(ByVal Question As String, ByVal ShowResults As Boolean, ByVal EndDate As DateTime, ByVal TakenMessage As String, ByVal ModuleID As Integer) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_PollAdd", Question, ShowResults, GetNull(EndDate), TakenMessage, ModuleID), Integer)
		End Function

		Public Overrides Sub UpdatePoll(ByVal PollID As Integer, ByVal Question As String, ByVal ShowResults As Boolean, ByVal EndDate As DateTime, ByVal TakenMessage As String)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_PollUpdate", PollID, Question, ShowResults, GetNull(EndDate), TakenMessage)
		End Sub

		Public Overrides Sub DeletePoll(ByVal PollID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_PollDelete", PollID)
		End Sub

		Public Overrides Function GetOrphanedPolls(ByVal ModuleID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_PollGetOrphans", ModuleID), IDataReader)
		End Function

		' Answers table
		Public Overrides Function GetAnswer(ByVal AnswerID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_AnswerGet", AnswerID), IDataReader)
		End Function

		Public Overrides Function GetPollAnswers(ByVal PollID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_AnswersGetPoll", PollID), IDataReader)
		End Function

		Public Overrides Function AddAnswer(ByVal PollID As Integer, ByVal Answer As String, ByVal SortOrder As Integer) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_AnswerAdd", PollID, Answer, SortOrder), Integer)
		End Function

		Public Overrides Sub UpdateAnswer(ByVal AnswerID As Integer, ByVal PollID As Integer, ByVal Answer As String, ByVal SortOrder As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_AnswerUpdate", AnswerID, PollID, Answer, SortOrder)
		End Sub

		Public Overrides Sub DeleteAnswer(ByVal AnswerID As Integer)
			SqlHelper.ExecuteNonQuery(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_AnswerDelete", AnswerID)
		End Sub

		' UserAnswers table
		Public Overrides Function GetUserAnswers(ByVal PollID As Integer) As IDataReader
			Return CType(SqlHelper.ExecuteReader(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_UserAnswersGet", PollID), IDataReader)
		End Function

		Public Overrides Function AddUserAnswer(ByVal PollID As Integer, ByVal UserID As Integer, ByVal AnswerID As Integer) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Polls_UserAnswerAdd", PollID, UserID, AnswerID), Integer)
		End Function

#End Region

		Public Overrides Function GetModulesPortalID(ByVal ModuleID As Integer) As Integer
			Return CType(SqlHelper.ExecuteScalar(ConnectionString, DatabaseOwner & ObjectQualifier & _moduleDataPrefix & "Module_GetPortalID", ModuleID), Integer)
		End Function

#End Region

	End Class

#End Region

End Namespace

