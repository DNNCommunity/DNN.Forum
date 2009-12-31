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
Option Explicit On
Option Strict On

Imports DotNetNuke.Entities.Portals

Namespace DotNetNuke.Modules.Forum.Utilities
	''' <summary>
	''' This class houses all the link generation methods for the entire forum module.
	''' </summary>
	''' <remarks></remarks>
	Public Class Links

#Region "ACP Links"

		''' <summary>
		''' Navigates the user to an admin control panel page.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ACPControlLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal AdminControl As AdminAjaxControl) As String
			Dim url As String
			Dim params As String()

			If AdminControl <> AdminAjaxControl.Main Then
				params = New String(1) {"mid=" & ModuleID.ToString, "view=" & CStr(AdminControl)}
			Else
				params = New String(0) {"mid=" & ModuleID.ToString}
			End If

			url = NavigateURL(TabId, ForumPage.ACP.ToString, params)

			Return url
		End Function

		Shared Function ForumEmailSubscribers(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal ForumID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"mid=" & ModuleID.ToString, "view=" & CStr(AdminAjaxControl.EmailSubscribers), "ForumID=" & ForumID.ToString()}

			url = NavigateURL(TabId, ForumPage.ACP.ToString, params)

			Return url
		End Function

		Shared Function ThreadEmailSubscribers(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal ForumID As Integer, ByVal ThreadID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(3) {"mid=" & ModuleID.ToString, "view=" & CStr(AdminAjaxControl.EmailSubscribers), "ForumID=" & ForumID, "ThreadID=" & ThreadID.ToString()}

			url = NavigateURL(TabId, ForumPage.ACP.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to the Forum Manage page. 
		''' This page allows access to specific forums/groups tied to the moduleID and also the ability to create new ones from here.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ACPForumsManageLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal GroupID As Integer) _
		 As String
			Dim params As String()
			Dim url As String

			If GroupID = -1 Then
				params = New String(0) {"mid=" & ModuleID}
				url = NavigateURL(TabId, ForumPage.ForumManage.ToString, params)
			Else
				params = New String(1) {"mid=" & ModuleID, "GroupID=" & GroupID}
				url = NavigateURL(TabId, ForumPage.ForumManage.ToString, params)
			End If

			Return url
		End Function

		''' <summary>
		''' Navigates the user to the email template manager page. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ACPEmailTemplateManagerLink(ByVal TabId As Integer, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(0) {"mid=" & ModuleID.ToString}
			url = NavigateURL(TabId, ForumPage.EmailTemplates.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates the user to the rolebased avatars manager page. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ACPRoleAvatarManagerLink(ByVal TabId As Integer, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(0) {"mid=" & ModuleID.ToString}
			url = NavigateURL(TabId, ForumPage.RoleAvatar.ToString, params)
			Return url
		End Function

		''' <summary>
		''' Navigates the user to the emoticons manager page. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ACPEmoticonManagerLink(ByVal TabId As Integer, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(0) {"mid=" & ModuleID.ToString}
			url = NavigateURL(TabId, ForumPage.Emoticon.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates the user to the forum admin control panel area.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ForumAdminLink(ByVal TabId As Integer, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(0) {"mid=" & ModuleID.ToString}
			url = NavigateURL(TabId, ForumPage.ACP.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to the edit forum page. If forumID = -1, this will be used to create a new forum tied to the moduleID.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="ForumID"></param>
		''' <param name="GroupID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ForumEditLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal ForumID As Integer, _
								 ByVal GroupID As Integer) As String
			Dim url As String
			Dim params As String()

			If ForumID = -1 Then
				params = New String(1) {"mid=" & ModuleID.ToString, "groupid=" & GroupID.ToString}
				url = NavigateURL(TabId, ForumPage.ForumEdit.ToString, params)
			Else
				params = New String(1) {"mid=" & ModuleID.ToString, "forumid=" & ForumID.ToString}
				url = NavigateURL(TabId, ForumPage.ForumEdit.ToString, params)
			End If

			Return url
		End Function

#End Region

#Region "Container Loaded Control Links"

		''' <summary>
		''' Navigates user to the forum home view (group view, unless overridden by tabModule settings)
		''' </summary>
		''' <param name="TabId"></param>
		''' <returns></returns>
		''' <remarks>Uses ForumContainer dispatch control.</remarks>
		Shared Function ContainerForumHome(ByVal TabId As Integer) As String
			Dim url As String
			url = NavigateURL(TabId, "", "")

			Return url
		End Function

		''' <summary>
		''' Navigates user to a single group view.  
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="GroupID"></param>
		''' <returns></returns>
		''' <remarks>Uses ForumContainer Dispatch control.</remarks>
		Shared Function ContainerSingleGroupLink(ByVal TabId As Integer, ByVal GroupID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(0) {"GroupID=" & GroupID}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to a single group view.  
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="GroupID"></param>
		''' <returns></returns>
		''' <remarks>Uses ForumContainer Dispatch control.</remarks>
		Shared Function ContainerParentForumLink(ByVal TabId As Integer, ByVal GroupID As Integer, ByVal ForumID As Integer) _
		 As String
			Dim url As String
			Dim params As String()

			params = New String(1) {"GroupID=" & GroupID, "ForumID=" & ForumID}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to a single group view filtered by date.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="dFilter"></param>
		''' <returns></returns>
		''' <remarks>Users ForumContainer dispatch control.</remarks>
		Shared Function ContainerGroupDateFilterLink(ByVal TabId As Integer, ByVal dFilter As String) As String
			Dim url As String
			Dim params As String()

			params = New String(0) {"datefilter=" & dFilter}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to thread view filtered by a date. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="dFilter"></param>
		''' <param name="ForumId"></param>
		''' <returns></returns>
		''' <remarks>Users ForumContainer dispatch control.</remarks>
		Shared Function ContainerThreadDateFilterLink(ByVal TabId As Integer, ByVal dFilter As String, _
											  ByVal ForumId As Integer, ByVal NoReply As Boolean) As String
			Dim url As String
			Dim params As String()

			If NoReply Then
				params = New String(3) {"forumid=" & ForumId, "datefilter=" & dFilter, "scope=threads", "noreply=1"}
				url = NavigateURL(TabId, "", params)
			Else
				params = New String(2) {"forumid=" & ForumId, "datefilter=" & dFilter, "scope=threads"}
				url = NavigateURL(TabId, "", params)
			End If

			Return url
		End Function

		''' <summary>
		''' Navigates user to thread view for a single forum.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <returns></returns>
		''' <remarks>Users ForumContainer dispatch control.</remarks>
		Public Shared Function ContainerViewForumLink(ByVal TabId As Integer, ByVal ForumId As Integer, ByVal NoReply As Boolean) As String
			Dim url As String
			Dim params As String()

			If NoReply Then
				params = New String(2) {"forumid=" & ForumId, "scope=threads", "noreply=1"}
				url = NavigateURL(TabId, "", params)
			Else
				params = New String(1) {"forumid=" & ForumId, "scope=threads"}
				url = NavigateURL(TabId, "", params)
			End If

			Return url
		End Function

		''' <summary>
		''' Navigates user to posts view for a specific threadID.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <param name="ThreadId"></param>
		''' <returns></returns>
		''' <remarks>Usere ForumContainer dispatch control.</remarks>
		Public Shared Function ContainerViewThreadLink(ByVal TabId As Integer, ByVal ForumId As Integer, ByVal ThreadId As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"forumid=" & ForumId, "threadid=" & ThreadId, "scope=posts"}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to list of unread posts.
		''' </summary>
		''' <param name="TabId"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Shared Function ContainerViewUnreadThreadsLink(ByVal TabId As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(0) {"scope=unread"}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to list of unread posts from the past X hours.
		''' </summary>
		''' <param name="TabId"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Shared Function ContainerViewLatestHoursLink(ByVal TabId As Integer, ByVal Hours As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(1) {"scope=threadsearch", "latesthours=" & CStr(Hours)}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to a specific page of results for a post view of a single thread.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <param name="ThreadId"></param>
		''' <param name="Page"></param>
		''' <returns></returns>
		''' <remarks>Users ForumContainer dispatch control.</remarks>
		Public Shared Function ContainerViewThreadPagedLink(ByVal TabId As Integer, ByVal ForumId As Integer, _
												   ByVal ThreadId As Integer, ByVal Page As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(3) {"forumid=" & ForumId, "threadid=" & ThreadId, "scope=posts", "threadpage=" & Page}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user directly to a post regardless of their posts/page, ascending/descending view options.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <param name="PostId"></param>
		''' <returns></returns>
		''' <remarks>Uses ForumContainer dispatch control.</remarks>
		Public Shared Function ContainerViewPostLink(ByVal TabId As Integer, ByVal ForumId As Integer, _
											 ByVal PostId As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"forumid=" & ForumId, "postid=" & PostId, "scope=posts"}
			url = NavigateURL(TabId, "", params)
			url = url + "#" + PostId.ToString()

			Return url
		End Function

		''' <summary>
		''' Navigates user to specific page of results for a thread view.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <param name="ThreadId"></param>
		''' <param name="PostId"></param>
		''' <param name="Page"></param>
		''' <returns></returns>
		''' <remarks>Uses ForumContainer dispatch control.</remarks>
		Public Shared Function ContainerViewPostPagedLink(ByVal TabId As Integer, ByVal ForumId As Integer, _
												 ByVal ThreadId As Integer, ByVal PostId As Integer, _
												 ByVal Page As Integer) As String
			Dim url As String
			Dim params As String()

			params = _
			 New String(4) _
			  {"forumid=" & ForumId, "threadid=" & ThreadId, "postid=" & PostId, "threadpage=" & Page, "scope=posts"}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to the post moderate screen for a specific ForumID.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks>Uses ForumContainer dispatch control.</remarks>
		Public Shared Function ContainerPostToModerateLink(ByVal TabId As Integer, ByVal ForumId As Integer, _
												  ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(1) {"forumid=" & ForumId.ToString, "mid=" & ModuleID.ToString}
			url = NavigateURL(TabId, ForumPage.PostModerate.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to a view of their posts.
		''' This is really the search results page, just being passed the logged on user's ID.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="AuthorID"></param>
		''' <returns></returns>
		''' <remarks>Uses ForumContainer dispatch control.</remarks>
		Public Shared Function ContainerMyPostsLink(ByVal TabId As Integer, ByVal AuthorID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(1) {"authors=" & AuthorID.ToString, "scope=threadsearch"}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to a view of threads they have started.
		''' This is really the search results page, just being passed the logged on user's ID.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Shared Function ContainerMyThreadsLink(ByVal TabId As Integer, ByVal UserID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(1) {"mythreads=" & UserID.ToString, "scope=threadsearch"}
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Nagivates user to the aggregated forum view. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="NoReply"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ContainerAggregatedLink(ByVal TabId As Integer, ByVal NoReply As Boolean) As String
			Dim url As String
			Dim params As String()

			If NoReply Then
				params = New String(2) {"scope=threadsearch", "noreply=1", "aggregated=1"}
			Else
				params = New String(1) {"scope=threadsearch", "aggregated=1"}
			End If
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to search results for a single ForumID which results
		''' in it searching posts across subject and body within that ForumID.
		''' If Aggregated (ForumId = -1) then it searches all forums the user can see.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <param name="SearchString"></param>
		''' <returns></returns>
		''' <remarks>Uses ForumContainer dispatch control.</remarks>
		Public Shared Function ContainerSingleForumSearchLink(ByVal TabId As Integer, ByVal ForumId As Integer, _
													ByVal SearchString As String) As String
			Dim url As String
			Dim params As String()

			If ForumId = -1 Then
				params = New String(2) {"body=" & SearchString, "subject=" & SearchString, "scope=threadsearch"}
			Else
				params = _
				 New String(3) {"forums=" & ForumId, "body=" & SearchString, "subject=" & SearchString, "scope=threadsearch"}
			End If
			url = NavigateURL(TabId, "", params)

			Return url
		End Function

#End Region

#Region "MCP Links"

		''' <summary>
		''' Navigates the user to an moderator control panel page.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="ModeratorControl"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function MCPControlLink(ByVal TabId As Integer, ByVal ModuleID As Integer, _
								  ByVal ModeratorControl As ModeratorAjaxControl) As String
			Dim url As String
			Dim params As String()

			If ModeratorControl <> ModeratorAjaxControl.Main Then
				params = New String(1) {"mid=" & ModuleID.ToString, "view=" & CStr(ModeratorControl)}
			Else
				params = New String(0) {"mid=" & ModuleID.ToString}
			End If

			url = NavigateURL(TabId, ForumPage.MCP.ToString, params)

			Return url
		End Function

#End Region

#Region "Moderator Actions"

		''' <summary>
		''' Navigates user to the move thread page. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="ForumId"></param>
		''' <param name="ThreadID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ThreadMoveLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal ForumId As Integer, _
								  ByVal ThreadID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"mid=" & ModuleID.ToString, "forumid=" & ForumId.ToString, "threadid=" & ThreadID}
			url = NavigateURL(TabId, ForumPage.ThreadMove.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates the user to the thread split page. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="ForumId"></param>
		''' <param name="PostID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ThreadSplitLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal ForumId As Integer, _
								   ByVal PostID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"mid=" & ModuleID.ToString, "forumid=" & ForumId.ToString, "postid=" & PostID}
			url = NavigateURL(TabId, ForumPage.ThreadSplit.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Naivates the user to the delete post control but sets a flag so the control knows we are deleting an entire thread. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="ForumId"></param>
		''' <param name="ThreadID"></param>
		''' <param name="FromModerationQueue"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ThreadDeleteLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal ForumId As Integer, _
								    ByVal ThreadID As Integer, ByVal FromModerationQueue As Boolean) As String
			Dim url As String
			Dim params As String()

			If FromModerationQueue Then
				params = _
				 New String(3) {"mid=" & ModuleID.ToString, "forumid=" & ForumId, "threadid=" & ThreadID, "moderatorreturn=1"}
				url = NavigateURL(TabId, ForumPage.PostDelete.ToString, params)
			Else
				params = New String(2) {"mid=" & ModuleID.ToString, "forumid=" & ForumId, "threadid=" & ThreadID}
				url = NavigateURL(TabId, ForumPage.PostDelete.ToString, params)
			End If

			Return url
		End Function

		''' <summary>
		''' Navigates the user to the post delete page. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="ForumId"></param>
		''' <param name="PostID"></param>
		''' <param name="FromModerationQueue"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function PostDeleteLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal ForumId As Integer, _
								  ByVal PostID As Integer, ByVal FromModerationQueue As Boolean) As String
			Dim url As String
			Dim params As String()

			If FromModerationQueue Then
				params = New String(3) {"mid=" & ModuleID.ToString, "forumid=" & ForumId, "postid=" & PostID, "moderatorreturn=1"}
				url = NavigateURL(TabId, ForumPage.PostDelete.ToString, params)
			Else
				params = New String(2) {"mid=" & ModuleID.ToString, "forumid=" & ForumId, "postid=" & PostID}
				url = NavigateURL(TabId, ForumPage.PostDelete.ToString, params)
			End If

			Return url
		End Function

#End Region

#Region "Other"

		''' <summary>
		''' Navigates the user to the report post abuse page. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="PostID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function ReportToModsLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal PostID As Integer) _
		 As String
			Dim url As String
			Dim params As String()

			params = New String(1) {"mid=" & ModuleID.ToString, "postid=" & PostID}
			url = NavigateURL(TabId, ForumPage.PostReport.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to a specific user's profile view.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="UserId"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function UserPublicProfileLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal UserId As Integer, ByVal IsExternal As Boolean, ByVal UserParam As String, ByVal ExtProfilePageID As Integer, ByVal UseName As Boolean, ByVal ProfileName As String) _
   As String

			Dim url As String
			Dim params As String()

			If IsExternal Then
				If UseName Then
					params = New String(0) {UserParam + "=" + ProfileName}
					url = NavigateURL(ExtProfilePageID, "", params)
				Else
					params = New String(0) {UserParam + "=" + UserId.ToString}
					url = NavigateURL(ExtProfilePageID, "", params)
				End If
			Else
				params = New String(1) {"mid=" & ModuleID.ToString, "userid=" & UserId.ToString}
				url = NavigateURL(TabId, ForumPage.PublicProfile.ToString, params)
			End If

			Return url
		End Function

		''' <summary>
		''' Navigates user to the member directory view.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function MemberListLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal IsExternal As Boolean, ByVal ParamName As String, ByVal ParamValue As String, ByVal ExtDirectoryPageID As Integer) As String
			Dim url As String
			Dim params As String()

			If IsExternal Then
				If ParamName.Trim() <> "" And ParamValue.Trim() <> "" Then
					params = New String(0) {ParamName + "=" + ParamValue}
					url = NavigateURL(ExtDirectoryPageID, "", params)
				Else
					url = NavigateURL(ExtDirectoryPageID, "", "")
				End If
			Else
				params = New String(0) {"mid=" & ModuleID}
				url = NavigateURL(TabId, ForumPage.MemberList.ToString, params)
			End If

			Return url
		End Function

		''' <summary>
		''' Navigates user to the search forums page view. 
		''' This is the page where users can input what parameters they want to filter their results against.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function SearchPageLink(ByVal TabId As Integer, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(0) {"mid=" & ModuleID.ToString}
			url = NavigateURL(TabId, ForumPage.ForumSearch.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to a page showing them there is no content to display.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleId"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function NoContentLink(ByVal TabId As Integer, ByVal ModuleId As Integer) As String
			Dim url As String
			Dim params() As String

			params = New String(0) {"mid=" & ModuleId.ToString}
			url = NavigateURL(TabId, ForumPage.ContentRemoved.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to the core unauthorized access page.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function UnAuthorizedLink() As String
			Dim url As String

			url = NavigateURL("Access Denied")

			Return url
		End Function

#End Region

#Region "Posting"

		''' <summary>
		''' Navigates user to postEdit, to create a reply/quote to an existing thread. (Means parent PostID present)
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <param name="PostID"></param>
		''' <param name="Action"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function NewPostLink(ByVal TabId As Integer, ByVal ForumId As Integer, ByVal PostID As Integer, _
							    ByVal Action As String, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(3) {"forumid=" & ForumId, "postid=" & PostID, "action=" & Action, "mid=" & ModuleID}
			url = NavigateURL(TabId, ForumPage.PostEdit.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to postEdit, to create a new thread. (Meaning no parent postID present)
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ForumId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function NewThreadLink(ByVal TabId As Integer, ByVal ForumId As Integer, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"forumid=" & ForumId, "action=new", "mid=" & ModuleID}
			url = NavigateURL(TabId, ForumPage.PostEdit.ToString, params)

			Return url
		End Function

#End Region

#Region "Private Messaging"

		''' <summary>
		''' Navigates user to the PMAdd page where they can reply/quote to an existing PMThreadID.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="PMID"></param>
		''' <param name="PMAction"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function NewPMReplyLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal PMID As Integer, _
								  ByVal PMAction As String) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"mid=" & ModuleID.ToString, "pmid=" & PMID, "pmaction=" & PMAction}
			url = NavigateURL(TabId, ForumPage.PMEdit.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to the PMAdd page where they can send a PM to a specific user
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Shared Function NewPMLink(ByVal TabId As Integer, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(1) {"mid=" & ModuleID.ToString, "pmaction=new"}
			url = NavigateURL(TabId, ForumPage.PMEdit.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to a specific PM for editing
		''' </summary>
		''' <param name="PMId"></param>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function PMEditLink(ByVal PMId As Integer, ByVal TabId As Integer, ByVal ModuleID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"mid=" & ModuleID, "pmid=" & PMId, "pmaction=edit"}
			url = NavigateURL(TabId, ForumPage.PMEdit.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to the AddPM page where user can start a new PMThread with the user being passed in here.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function PMUserLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal UserID As Integer) As String
			Dim params As String()
			Dim url As String
			params = New String(1) {"mid=" & ModuleID.ToString, "userid=" & UserID.ToString}
			url = NavigateURL(TabId, ForumPage.PMEdit.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to view all posts attached to a single PMThreadID.
		''' This is the PMThread page, not Inbox.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="PMThreadID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Shared Function PMThreadLink(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal PMThreadID As Integer) _
		 As String
			Dim url As String
			Dim params As String()

			params = New String(1) {"mid=" & ModuleID.ToString, "pmthreadid=" & PMThreadID}
			url = NavigateURL(TabId, ForumPage.PMThread.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to view a specific PM from the Outbox.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="PMID"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Shared Function PMThreadLinkFromOutbox(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal PMThreadID As Integer, _
										ByVal PMID As Integer) As String
			Dim url As String
			Dim params As String()

			params = New String(2) {"mid=" & ModuleID.ToString, "pmthreadid=" & PMThreadID, "pmid=" & PMID}
			url = NavigateURL(TabId, ForumPage.PMThread.ToString, params)

			Return url
		End Function

#End Region

#Region "UCP Links"

		''' <summary>
		''' Navigates user to UCP Profile. This is for a forum admin to manage the profile of a different user.
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="UserControl"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Shared Function UCP_AdminLinks(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal UserId As Integer, _
								  ByVal UserControl As UserAjaxControl) As String
			Dim url As String
			Dim params As String()

			If UserControl <> UserAjaxControl.Main Then
				params = New String(2) {"mid=" & ModuleID.ToString, "userid=" & UserId, "view=" & CStr(UserControl)}
			Else
				params = New String(1) {"mid=" & ModuleID.ToString, "userid=" & UserId}
			End If

			url = NavigateURL(TabId, ForumPage.UCP.ToString, params)

			Return url
		End Function

		''' <summary>
		''' Navigates user to THEIR UCP page. 
		''' </summary>
		''' <param name="TabId"></param>
		''' <param name="ModuleID"></param>
		''' <param name="UserControl"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Shared Function UCP_UserLinks(ByVal TabId As Integer, ByVal ModuleID As Integer, ByVal UserControl As UserAjaxControl, _
								 ByVal SiteSettings As PortalSettings) As String
			Dim url As String
			Dim params As String()

			If UserControl <> UserAjaxControl.Main Then
				params = New String(1) {"mid=" & ModuleID.ToString, "view=" & CStr(UserControl)}
			Else
				params = New String(0) {"mid=" & ModuleID.ToString}
			End If

			url = NavigateURL(TabId, SiteSettings, ForumPage.UCP.ToString, params)

			Return url
		End Function

#End Region

	End Class
End Namespace