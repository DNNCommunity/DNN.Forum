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

Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Services.FileSystem

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' Renders the Posts view UI.  
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class Posts
		Inherits ForumObject

#Region "Private Declarations"

#Region "Private Members"

		Dim _ForumID As Integer = 0
		Dim _ThreadID As Integer = 0
		Dim _PostID As Integer = 0
		Dim _PostCollection As New List(Of PostInfo)
		Dim _PostPage As Integer = 0
		Dim _ThreadInfo As ThreadInfo
		Dim _HostForum As ForumInfo
		Dim _TrackedModule As Boolean = False
		Dim _TrackedForum As Boolean = False
		Dim _TrackedThread As Boolean = False
		Dim _url As String
		Dim newpost As Boolean = False	'[skeel] added 

#End Region

#Region "Controls"

		Private trcRating As Telerik.Web.UI.RadRating
		Private ddlViewDescending As DotNetNuke.Web.UI.WebControls.DnnComboBox
		Private chkEmail As CheckBox
		Private ddlThreadStatus As DotNetNuke.Web.UI.WebControls.DnnComboBox
		Private cmdThreadAnswer As LinkButton
		Private txtForumSearch As TextBox
		Private cmdForumSearch As ImageButton
		Private hsThreadAnswers As New Hashtable
		Private rblstPoll As RadioButtonList
		Private cmdVote As LinkButton
		Private cmdBookmark As ImageButton	'[skeel] added
		Private txtQuickReply As TextBox
		Private cmdSubmit As LinkButton
		Private cmdThreadSubscribers As LinkButton

#End Region

#End Region

#Region "Public Properties"

		''' <summary>
		''' The ForumID of the thread being rendered.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ForumId() As Integer
			Get
				Return _ForumID
			End Get
		End Property

		''' <summary>
		''' The ThreadID for all the posts being rendered.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ThreadId() As Integer
			Get
				Return _ThreadID
			End Get
		End Property

		''' <summary>
		''' The PostID being rendered.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PostId() As Integer
			Get
				Return _PostID
			End Get
		End Property

		''' <summary>
		''' The collection of posts being rendered.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PostCollection() As List(Of PostInfo)
			Get
				Return _PostCollection
			End Get
		End Property

		''' <summary>
		''' The page being rendered of posts.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PostPage() As Integer
			Get
				Return _PostPage
			End Get
		End Property

		''' <summary>
		''' The ThreadInfo object of the ThreadID being rendered.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ThreadInfo() As ThreadInfo
			Get
				Return _ThreadInfo
			End Get
		End Property

		''' <summary>
		''' The parent ForumInfo object of the ForumID being rendered.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ParentForum() As ForumInfo
			Get
				Return _HostForum
			End Get
		End Property

#End Region

#Region "Private Properties"

		Public Property TrackedModule() As Boolean
			Get
				Return _TrackedModule
			End Get
			Set(ByVal Value As Boolean)
				_TrackedModule = Value
			End Set
		End Property

		Public Property TrackedForum() As Boolean
			Get
				Return _TrackedForum
			End Get
			Set(ByVal Value As Boolean)
				_TrackedForum = Value
			End Set
		End Property

		Public Property TrackedThread() As Boolean
			Get
				Return _TrackedThread
			End Get
			Set(ByVal Value As Boolean)
				_TrackedThread = Value
			End Set
		End Property

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Updates the thread status
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub ddlThreadStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim ThreadStatus As Integer = ddlThreadStatus.SelectedIndex
			If ThreadStatus > 0 Then
				Dim ctlThread As New ThreadController

				Dim ModeratorID As Integer = -1
				If CurrentForumUser.UserID <> ThreadInfo.StartedByUserID Then
					ModeratorID = CurrentForumUser.UserID
				End If

				ctlThread.ThreadStatusChange(ThreadId, CurrentForumUser.UserID, ThreadStatus, 0, ModeratorID, PortalID)
			End If

			ThreadController.ResetThreadInfo(ThreadId)
		End Sub

		''' <summary>
		''' This Event turns the users thread tracking on/off.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub chkEmail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
			Dim ctlTracking As New TrackingController
			ctlTracking.TrackingThreadCreateDelete(ForumId, _ThreadID, CurrentForumUser.UserID, chkEmail.Checked, ModuleID)
			ThreadController.ResetThreadInfo(_ThreadID)
		End Sub

		''' <summary>
		''' Applies the user's thread rating.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>This needs to have redirect link generated from link utils.</remarks>
		Protected Sub trcRating_Rate(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim rate As Double = trcRating.Value

			If rate > 0 Then
				Dim ctlThread As New ThreadController
				ctlThread.ThreadRateAdd(ThreadId, CurrentForumUser.UserID, rate)
			End If

			ThreadController.ResetThreadInfo(ThreadId)
		End Sub

		''' <summary>
		''' Adds a user's vote to the data store for a specific poll.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdVote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
			' get the user's vote and put it in the data store
			Dim intAnswerID As Integer = CInt(rblstPoll.SelectedValue)
			' update user voting, when page is redrawn it will handle checking if user voted
			Dim cntUserAnswer As New UserAnswerController
			Dim objUserAnswer As New UserAnswerInfo

			objUserAnswer.UserID = CurrentForumUser.UserID
			objUserAnswer.PollID = ThreadInfo.PollID
			objUserAnswer.AnswerID = intAnswerID

			cntUserAnswer.AddUserAnswer(objUserAnswer)
			' update user answer cache - 
		End Sub

		''' <summary>
		''' Adds or remove the current thread to the users bookmark list 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdBookmark_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
			Dim BookmarkCtl As New BookmarkController
			Select Case cmdBookmark.AlternateText
				Case ForumControl.LocalizedText("RemoveBookmark")
					BookmarkCtl.BookmarkCreateDelete(ThreadId, CurrentForumUser.UserID, False, ModuleID)
					'Change ImageButton to support AJAX
					cmdBookmark.AlternateText = ForumControl.LocalizedText("AddBookmark")
					cmdBookmark.ToolTip = ForumControl.LocalizedText("AddBookmark")
					cmdBookmark.ImageUrl = objConfig.GetThemeImageURL("forum_bookmark.") & objConfig.ImageExtension
				Case ForumControl.LocalizedText("AddBookmark")
					BookmarkCtl.BookmarkCreateDelete(ThreadId, CurrentForumUser.UserID, True, ModuleID)
					'Change ImageButton to support AJAX
					cmdBookmark.AlternateText = ForumControl.LocalizedText("RemoveBookmark")
					cmdBookmark.ToolTip = ForumControl.LocalizedText("RemoveBookmark")
					cmdBookmark.ImageUrl = objConfig.GetThemeImageURL("forum_nobookmark.") & objConfig.ImageExtension
			End Select
		End Sub

		''' <summary>
		''' This takes moderators/forum admin to moderator screen with the thread loaded to view subscribers. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdThreadSubscribers_Click(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim url As String
			url = Utilities.Links.ThreadEmailSubscribers(TabID, ModuleID, ForumId, ThreadId)
			MyBase.BasePage.Response.Redirect(url, False)
		End Sub

		''' <summary>
		''' This Event sets the users view preference ascending/descending and saves to 
		''' the db. (Descending by default)
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Anonymous users can see both views but it doesn't save to db when changed.
		''' </remarks>
		Protected Sub ddlViewDescending_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			ForumControl.Descending = CType(ddlViewDescending.SelectedIndex, Boolean)

			Dim ctlPost As New PostController
			_PostCollection = ctlPost.PostGetAll(ThreadId, PostPage, CurrentForumUser.PostsPerPage, False, ForumControl.Descending, PortalID)

			'<tam:note value=update database if it's an authenticated user>
			If CurrentForumUser.UserID > 0 Then
				CurrentForumUser.ViewDescending = (ForumControl.Descending)
				Dim ctlForumUser As New ForumUserController
				ctlForumUser.UserViewUpdate(CurrentForumUser.UserID, True, CurrentForumUser.ViewDescending)
				'Else
				'    If Not HttpContext.Current.Request.Cookies(".ASPXANONYMOUS") Is Nothing Then
				'        Dim c As System.Web.HttpCookie
				'        c = HttpContext.Current.Request.Cookies(".ASPXANONYMOUS")
				'        c.Values.Add("ForumDescending", ForumControl.Descending.ToString)
				'    End If
			End If
		End Sub

		''' <summary>
		''' This directs the user to the search results of this particular forum. It searches this forum and the subject, body of the post. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdForumSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
			If txtForumSearch.Text.Trim <> String.Empty Then
				_url = Utilities.Links.ContainerSingleForumSearchLink(TabID, ForumId, txtForumSearch.Text)
				MyBase.BasePage.Response.Redirect(_url, False)
			End If
		End Sub

		''' <summary>
		''' Submits a quickly reply to the posting API (which is related to an existing thread). 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
			If Len(txtQuickReply.Text) > 0 Then
				Dim RemoteAddress As String = "0.0.0.0"
				Dim strSubject As String = Utilities.ForumUtils.SetReplySubject(ThreadInfo.Subject)

				If Not HttpContext.Current.Request.UserHostAddress Is Nothing Then
					RemoteAddress = HttpContext.Current.Request.UserHostAddress
				End If

				Dim cntPostConnect As New PostConnector
				Dim PostMessage As PostMessage

				PostMessage = cntPostConnect.SubmitInternalPost(TabID, ModuleID, PortalID, CurrentForumUser.UserID, strSubject, txtQuickReply.Text, ForumId, ThreadInfo.ThreadID, -1, ThreadInfo.IsPinned, False, False, ThreadInfo.ThreadStatus, "", RemoteAddress, ThreadInfo.PollID, ThreadInfo.ThreadIconID, False)

				Select Case PostMessage
					Case PostMessage.PostApproved
						'	Dim ReturnURL As String = NavigateURL()

						'	If objModSecurity.IsModerator Then
						'		If Not ViewState("UrlReferrer") Is Nothing Then
						'			ReturnURL = (CType(ViewState("UrlReferrer"), String))
						'		Else
						'			ReturnURL = Utilities.Links.ContainerViewForumLink(TabID, objForum.ForumID, False)
						'		End If
						'	Else
						'		ReturnURL = Utilities.Links.ContainerViewForumLink(TabID, ForumId, False)
						'	End If

						'	Response.Redirect(ReturnURL, False)
					Case PostMessage.PostModerated
						'tblNewPost.Visible = False
						'tblOldPost.Visible = False
						'tblPreview.Visible = False
						'cmdCancel.Visible = False
						'cmdBackToEdit.Visible = False
						'cmdSubmit.Visible = False
						'cmdPreview.Visible = False
						'cmdBackToForum.Visible = True
						'rowModerate.Visible = True
						'tblPoll.Visible = False
					Case Else
						'lblInfo.Visible = True
						'lblInfo.Text = Localization.GetString(PostMessage.ToString() + ".Text", LocalResourceFile)
				End Select
				txtQuickReply.Text = ""
				'Forum.ThreadInfo.ResetThreadInfo(ThreadId)

				Dim ctlPost As New PostController
				_PostCollection = ctlPost.PostGetAll(ThreadId, PostPage, CurrentForumUser.PostsPerPage, False, ForumControl.Descending, PortalID)
				' we need to redirect the user here to make sure the page is redrawn.
			Else
				' there is no quick reply message entered, yet they clicked submit. Show end user. 
			End If
		End Sub

		''' <summary>
		''' Sets a specific post as an answer, only available when thread status is set to 'unresolved'. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdThreadAnswer_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
			Dim ctlThread As New ThreadController
			Dim answerPostID As Integer
			Dim Argument As String

			If e.CommandName = "MarkAnswer" Then
				Argument = CStr(e.CommandArgument)
				answerPostID = Int32.Parse(Argument)

				Dim ctlPost As New PostController
				Dim objPostInfo As PostInfo
				objPostInfo = ctlPost.PostGet(answerPostID, PortalID)

				Dim ModeratorID As Integer = -1
				If ThreadInfo.StartedByUserID <> CurrentForumUser.UserID Then
					ModeratorID = CurrentForumUser.UserID
				End If

				ctlThread.ThreadStatusChange(ThreadId, objPostInfo.UserID, ThreadStatus.Answered, answerPostID, ModeratorID, PortalID)

				ThreadController.ResetThreadInfo(ThreadId)
			End If
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Instantiates this class, sets the page title and does a security check.
		''' </summary>
		''' <param name="forum"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal forum As DNNForum)
			MyBase.New(forum)

			Dim user As ForumUserInfo = CurrentForumUser

			If Not HttpContext.Current.Request.QueryString("postid") Is Nothing Then
				' If a specific postid is in the url
				_PostID = Int32.Parse(HttpContext.Current.Request.QueryString("postid"))

				Dim objPostCnt As New PostController
				Dim objPost As PostInfo = objPostCnt.PostGet(PostId, PortalID)
				_ThreadID = objPost.ThreadID
				' we need to determine which page to return based on number of posts in this thread, the users posts per page count, and their asc/desc view, where this post is
				Dim cntThread As New ThreadController()
				_ThreadInfo = cntThread.GetThreadInfo(ThreadId)
				Dim TotalPosts As Integer = _ThreadInfo.Replies + 1
				Dim FlatSortOrder As Integer = objPost.FlatSortOrder
				Dim userPostsPerPage As Integer = 1

				If CurrentForumUser.UserID > 0 Then
					userPostsPerPage = user.PostsPerPage
				Else
					userPostsPerPage = objConfig.PostsPerPage
				End If

				Dim TotalPages As Integer = (CInt(TotalPosts / userPostsPerPage))
				Dim ThreadPageToShow As Integer = 1

				' we need to use flatsortorder and totalpages to determine which page to view          
				If user.ViewDescending Then
					ThreadPageToShow = CInt(Math.Ceiling((TotalPosts - FlatSortOrder) / userPostsPerPage))
				Else
					ThreadPageToShow = CInt(Math.Ceiling((FlatSortOrder + 1) / userPostsPerPage))
				End If
				' DO NOT FACTOR IN ThreadPage in URL HERE!!! (It will cause errors so never check what it says)
				_PostPage = ThreadPageToShow
			Else
				If Not HttpContext.Current.Request.QueryString("threadid") Is Nothing Then
					_ThreadID = Int32.Parse(HttpContext.Current.Request.QueryString("threadid"))
					Dim cntThread As New ThreadController()
					_ThreadInfo = cntThread.GetThreadInfo(ThreadId)

					' We need to make sure the user's thread pagesize can handle this 
					'(problem is, a link can be posted by one user w/ page size of 5 pointing to page 2, if logged in user has pagesize set to 15, there is no page 2)
					If Not HttpContext.Current.Request.QueryString("threadpage") Is Nothing Then
						Dim urlThreadPage As Integer = Int32.Parse(HttpContext.Current.Request.QueryString("threadpage"))
						Dim TotalPosts As Integer = ThreadInfo.Replies + 1
						Dim userPostsPerPage As Integer

						If CurrentForumUser.UserID > 0 Then
							userPostsPerPage = user.PostsPerPage
						Else
							userPostsPerPage = objConfig.PostsPerPage
						End If

						Dim TotalPages As Integer = CInt(Math.Ceiling(TotalPosts / userPostsPerPage))
						Dim ThreadPageToShow As Integer

						' We need to check if it is possible for a pagesize in the URL for the user browsing (happens when coming from posted link by other user)
						If TotalPages >= urlThreadPage Then
							ThreadPageToShow = urlThreadPage
						Else
							' We know for this user, total pages > user posts per page. Because of this, we know its not user using page change so show thread as normal
							ThreadPageToShow = 0
						End If
						_PostPage = ThreadPageToShow
					End If
				End If
			End If

			' If the thread info is nothing, it is probably a deleted thread
			If ThreadInfo Is Nothing Then
				' we should consider setting type of redirect here?

				MyBase.BasePage.Response.Redirect(Utilities.Links.NoContentLink(TabID, ModuleID), True)
			End If

			_HostForum = ThreadInfo.HostForum
			_ForumID = ParentForum.ForumID

			' Make sure the forum is active 
			If Not _HostForum.IsActive Then
				' we should consider setting type of redirect here?

				MyBase.BasePage.Response.Redirect(Utilities.Links.NoContentLink(TabID, ModuleID), True)
			End If

			' User might access this page by typing url so better check permission on parent forum
			If Not (ParentForum.PublicView) Then
				' The forum is private, see if we have proper view perms here
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

				If Not objSecurity.IsAllowedToViewPrivateForum Then
					' we should consider setting type of redirect here?

					MyBase.BasePage.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If
			End If

			'We are past knowing the user should be here, let's handle SEO oriented things
			If objConfig.OverrideTitle Then
				MyBase.BasePage.Title = ThreadInfo.Subject & " - " & ThreadInfo.HostForum.Name & " - " & Me.BaseControl.PortalName
			End If

			If objConfig.OverrideDescription Then
				MyBase.BasePage.Description = ThreadInfo.Subject + "," + ThreadInfo.HostForum.Name + "," + Me.BaseControl.PortalName
			End If
			' Consider add metakeywords via applied tags, when taxonomy is integrated

			If _PostPage > 0 Then
				_PostPage = _PostPage - 1
			Else
				_PostPage = 0
			End If
		End Sub

		''' <summary>
		''' This is the first class that runs as part of New().  This could be invoked in Render as well but is not
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Overrides Sub CreateChildControls()
			Controls.Clear()

			Me.trcRating = New Telerik.Web.UI.RadRating
			With trcRating
				.Skin = "Office2007"
				.SelectionMode = Telerik.Web.UI.RatingSelectionMode.Continuous
				.IsDirectionReversed = False
				.Orientation = Orientation.Horizontal
				.Precision = Telerik.Web.UI.RatingPrecision.Half
				.ItemCount = objConfig.RatingScale
				AddHandler trcRating.Rate, AddressOf trcRating_Rate
				.AutoPostBack = True
			End With

			' display tracking option only if user authenticated
			If CurrentForumUser.UserID > 0 Then
				' Thread Status Dropdownlist
				Me.ddlThreadStatus = New DotNetNuke.Web.UI.WebControls.DnnComboBox
				With ddlThreadStatus
					.Skin = "WebBlue"
					.ID = "lstThreadStatus"
					.Width = Unit.Parse("150")
					.AutoPostBack = True
					.ClearSelection()
				End With

				' Email notification checkbox
				chkEmail = New CheckBox
				With chkEmail
					.CssClass = "Forum_NormalTextBox"
					.ID = "chkEmail"
					.Text = ForumControl.LocalizedText("MailWhenReply").Replace("[ThreadSubject]", "<b>" & ThreadInfo.Subject & "</b>")
					.TextAlign = TextAlign.Left
					.AutoPostBack = True
					.Checked = False
				End With
			End If

			' Forum view (newest to oldest/oldest to newest) dropdownlist
			ddlViewDescending = New DotNetNuke.Web.UI.WebControls.DnnComboBox
			With ddlViewDescending
				.Skin = "WebBlue"
				.ID = "lstViewDescending"
				.Width = Unit.Parse("150")
				.AutoPostBack = True
				.Items.Add(New Telerik.Web.UI.RadComboBoxItem(ForumControl.LocalizedText("OldestToNewest")))
				.Items.Add(New Telerik.Web.UI.RadComboBoxItem(ForumControl.LocalizedText("NewestToOldest")))
				.ClearSelection()
			End With

			txtForumSearch = New TextBox
			With txtForumSearch
				.CssClass = "Forum_NormalTextBox"
				.ID = "txtForumSearch"
				.Width = Unit.Parse("150")
			End With

			Me.cmdForumSearch = New ImageButton
			With cmdForumSearch
				.CssClass = "Forum_Profile"
				.ID = "cmdForumSearch"
				.AlternateText = ForumControl.LocalizedText("Search")
				.ToolTip = ForumControl.LocalizedText("Search")
				.ImageUrl = objConfig.GetThemeImageURL("s_lookup.") & objConfig.ImageExtension
			End With

			'Polls
			Me.rblstPoll = New RadioButtonList
			With rblstPoll
				.CssClass = "Forum_NormalTextBox"
				.ID = "rblstPoll"
			End With

			Me.cmdVote = New LinkButton
			With cmdVote
				.CssClass = "Forum_Profile"
				.ID = "cmdVote"
				.Text = ForumControl.LocalizedText("Vote")
			End With

			'[skeel] added
			If CurrentForumUser.UserID > 0 Then
				Me.cmdBookmark = New ImageButton
				With cmdBookmark
					.CssClass = "Forum_Profile"
					.ID = "cmdBookmark"
				End With
				Dim BookmarkCtl As New BookmarkController
				If BookmarkCtl.BookmarkCheck(CurrentForumUser.UserID, ThreadId, ModuleID) = True Then
					With cmdBookmark
						.AlternateText = ForumControl.LocalizedText("RemoveBookmark")
						.ToolTip = ForumControl.LocalizedText("RemoveBookmark")
						.ImageUrl = objConfig.GetThemeImageURL("forum_nobookmark.") & objConfig.ImageExtension
					End With
				Else
					With cmdBookmark
						.AlternateText = ForumControl.LocalizedText("AddBookmark")
						.ToolTip = ForumControl.LocalizedText("AddBookmark")
						.ImageUrl = objConfig.GetThemeImageURL("forum_bookmark.") & objConfig.ImageExtension
					End With
				End If
			End If

			If Not CurrentForumUser.UserID > 0 Then
				ddlViewDescending.Visible = False
			End If

			' Quick Reply
			Me.txtQuickReply = New TextBox
			With txtQuickReply
				.CssClass = "Forum_NormalTextBox"
				.ID = "txtQuickReply"
				.Width = Unit.Percentage(99)
				.Height = 150
				.TextMode = TextBoxMode.MultiLine
			End With

			Me.cmdSubmit = New LinkButton
			With cmdSubmit
				.CssClass = "Forum_Link"
				.ID = "cmdSubmit"
				.Text = ForumControl.LocalizedText("cmdSubmit")
			End With

			Me.cmdThreadSubscribers = New LinkButton
			With cmdThreadSubscribers
				.CssClass = "Forum_Profile"
				.ID = "cmdThreadSubscribers"
				.Text = ForumControl.LocalizedText("cmdThreadSubscribers")
			End With

			BindControls()
			AddControlHandlers()
			AddControlsToTree()

			For Each post As PostInfo In PostCollection
				Me.cmdThreadAnswer = New System.Web.UI.WebControls.LinkButton
				With cmdThreadAnswer
					.CssClass = "Forum_AnswerText"
					.ID = "cmdThreadAnswer" + post.PostID.ToString()
					.Text = ForumControl.LocalizedText("MarkAnswer")
					.CommandName = "MarkAnswer"
					.CommandArgument = post.PostID.ToString()
					AddHandler cmdThreadAnswer.Command, AddressOf cmdThreadAnswer_Click
				End With
				hsThreadAnswers.Add(post.PostID, cmdThreadAnswer)
				Controls.Add(cmdThreadAnswer)
			Next
		End Sub

		''' <summary>
		''' Does the actual calls for rendering the UI in logical order to build wr
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Public Overrides Sub Render(ByVal wr As HtmlTextWriter)
			RenderTableBegin(wr, "tblForumContainer", "Forum_Container", "", "100%", "0", "0", "left", "top", "0")
			RenderNavBar(wr, objConfig, ForumControl)
			RenderSearchBar(wr)
			RenderBreadCrumbRow(wr)
			RenderThread(wr)
			RenderFooter(wr)
			RenderBottomBreadCrumbRow(wr)
			RenderQuickReply(wr)
			RenderTableEnd(wr)

			'increment the thread view count
			Dim ctlThread As New ThreadController
			ctlThread.ThreadViewsIncrement(ThreadId)

			'update the UserThread record
			If ForumControl.Page.Request.IsAuthenticated Then
				Dim userThreadController As New UserThreadsController
				Dim userThread As New UserThreadsInfo
				userThread = userThreadController.GetCachedUserThreadRead(CurrentForumUser.UserID, ThreadId)

				If Not userThread Is Nothing Then
					userThread.LastVisitDate = Now
					' Add error handling Just in case because of constraints and data integrity - This is highly unlikely to occur so do it here instead of the database(performance reasons)
					Try
						userThreadController.Update(userThread)
						UserThreadsController.ResetUserThreadReadCache(userThread.UserID, ThreadId)
					Catch exc As Exception
						LogException(exc)
					End Try
				Else
					userThread = New UserThreadsInfo
					With userThread
						.UserID = CurrentForumUser.UserID
						.ThreadID = ThreadId
						.LastVisitDate = Now
					End With
					userThreadController.Add(userThread)
					UserThreadsController.ResetUserThreadReadCache(userThread.UserID, userThread.ThreadID)
				End If
				ThreadController.ResetThreadInfo(ThreadId)
			End If
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Sets handlers for certain server controls
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	11/29/2005	Created
		''' </history>
		Private Sub AddControlHandlers()
			Try
				If objConfig.EnableThreadStatus And CurrentForumUser.UserID > 0 Then
					AddHandler ddlThreadStatus.SelectedIndexChanged, AddressOf ddlThreadStatus_SelectedIndexChanged
				End If

				If objConfig.MailNotification And CurrentForumUser.UserID > 0 Then
					AddHandler chkEmail.CheckedChanged, AddressOf chkEmail_CheckedChanged
				End If

				If objConfig.EnableRatings Then
					AddHandler trcRating.Rate, AddressOf trcRating_Rate
				End If

				AddHandler cmdVote.Click, AddressOf cmdVote_Click

				If CurrentForumUser.UserID > 0 Then
					AddHandler cmdBookmark.Click, AddressOf cmdBookmark_Click
					AddHandler cmdThreadSubscribers.Click, AddressOf cmdThreadSubscribers_Click

					' Remove for anon posting (if we allow quick reply via anonymous posting)
					AddHandler cmdSubmit.Click, AddressOf cmdSubmit_Click
				End If

				AddHandler ddlViewDescending.SelectedIndexChanged, AddressOf ddlViewDescending_SelectedIndexChanged
				AddHandler cmdForumSearch.Click, AddressOf cmdForumSearch_Click
			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Adds the controls to the control tree
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub AddControlsToTree()
			Try
				If objConfig.EnableThreadStatus And CurrentForumUser.UserID > 0 Then
					Controls.Add(ddlThreadStatus)
				End If

				If objConfig.MailNotification And CurrentForumUser.UserID > 0 Then
					Controls.Add(chkEmail)
				End If

				If objConfig.EnableRatings Then
					Controls.Add(trcRating)
				End If

				Controls.Add(rblstPoll)

				If CurrentForumUser.UserID > 0 Then
					Controls.Add(cmdBookmark)
					Controls.Add(cmdThreadSubscribers)

					' Remove for anon posting (if we allow quick reply via anonymous posting)
					Controls.Add(txtQuickReply)
					Controls.Add(cmdSubmit)
				End If

				Controls.Add(ddlViewDescending)
				Controls.Add(txtForumSearch)
				Controls.Add(cmdForumSearch)
			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Binds data to the available controls to the end user
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub BindControls()
			Try
				Dim ctlPost As New PostController

				' '' Use new Lists feature to provide rate entries (localization support)
				'Dim ctlLists As New DotNetNuke.Common.Lists.ListController

				If objConfig.EnableRatings Then
					BindRating()
				End If

				' All enclosed items are user specific, so we must have a userID
				If CurrentForumUser.UserID > 0 Then
					If objConfig.EnableThreadStatus And ThreadInfo.HostForum.EnableForumsThreadStatus Then
						ddlThreadStatus.Visible = True
						ddlThreadStatus.Items.Clear()

						ddlThreadStatus.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("NoneSpecified", objConfig.SharedResourceFile), "0"))
						ddlThreadStatus.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Unanswered", objConfig.SharedResourceFile), "1"))
						ddlThreadStatus.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Answered", objConfig.SharedResourceFile), "2"))
						ddlThreadStatus.Items.Insert(3, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Informative", objConfig.SharedResourceFile), "3"))
					Else
						ddlThreadStatus.Visible = False
					End If
					'polling changes
					If ThreadInfo.ThreadStatus = ThreadStatus.Poll Then
						Dim statusEntry As New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Poll", objConfig.SharedResourceFile), ThreadStatus.Poll.ToString())
						ddlThreadStatus.Items.Add(statusEntry)
					End If

					ddlThreadStatus.SelectedIndex = CType(ThreadInfo.ThreadStatus, Integer)

					' display tracking option only if user is authenticated and the forum module allows tracking
					If objConfig.MailNotification Then
						Dim objForumUser As ForumUserInfo = CurrentForumUser
						' check to see if the user is tracking at the module level (not implemented)
						If Not objForumUser.TrackedModule Then
							' check to see if the user is tracking at the forum level
							For Each objTrackForum As TrackingInfo In objForumUser.TrackedForums
								If objTrackForum.ForumID = ForumId Then
									TrackedForum = True
									Exit For
								End If
							Next

							If Not TrackedForum Then
								Dim arrTrackThreads As List(Of TrackingInfo) = objForumUser.TrackedThreads
								Dim objTrackThread As TrackingInfo

								' check to see if the user is tracking at the thread level
								For Each objTrackThread In arrTrackThreads
									If objTrackThread.ThreadID = ThreadId Then
										TrackedThread = True
										chkEmail.Checked = True
										Exit For
									End If
								Next
							End If
						Else
							'CP - since not implemented, should never happen
							TrackedModule = True
						End If
					End If

					If (CurrentForumUser.ViewDescending) Then
						ForumControl.Descending = True
						ddlViewDescending.Items.FindItemByText(ForumControl.LocalizedText("NewestToOldest")).Selected = True
					Else
						ForumControl.Descending = False
						ddlViewDescending.Items.FindItemByText(ForumControl.LocalizedText("OldestToNewest")).Selected = True
					End If

					' Handle Polls
					If ThreadInfo.PollID > 0 Then
						Dim cntAnswer As New AnswerController
						Dim arrAnswers As List(Of AnswerInfo)

						arrAnswers = cntAnswer.GetPollAnswers(ThreadInfo.PollID)
						If arrAnswers.Count > 0 Then
							rblstPoll.DataTextField = "Answer"
							rblstPoll.DataValueField = "AnswerID"
							rblstPoll.DataSource = arrAnswers
							rblstPoll.DataBind()

							rblstPoll.SelectedIndex = 0
						End If
					End If
				Else
					ForumControl.Descending = CType(ddlViewDescending.SelectedIndex, Boolean)
					'CP - COMEBACK: Add way to display rating but don't allow voting (for anonymous users)
					trcRating.Enabled = False
				End If

				_PostCollection = ctlPost.PostGetAll(ThreadId, PostPage, CurrentForumUser.PostsPerPage, False, ForumControl.Descending, PortalID)

				If _PostCollection.Count > 0 And _PostID = 0 Then
					_PostID = CType(_PostCollection.Item(0), PostInfo).PostID
				End If
			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindRating()
			trcRating.Value = CDec(ThreadInfo.Rating)
			trcRating.ToolTip = ThreadInfo.RatingText

			If Not CurrentForumUser.UserID > 0 Then
				trcRating.Enabled = False
			End If
		End Sub

		''' <summary>
		''' Renders the Rating selector, current rating image, search textbox and button
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderSearchBar(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>

			' left cap
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")

			RenderCellBegin(wr, "", "", "100%", "", "", "", "") ' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) '<tr>

			RenderCellBegin(wr, "", "", "100%", "left", "", "", "") ' <td>
			RenderTableBegin(wr, "", "", "", "", "2", "0", "", "", "0")	 ' <table>
			RenderRowBegin(wr) '<tr>

			'[skeel] Display bookmark image button here
			If CurrentForumUser.UserID > 0 Then
				RenderCellBegin(wr, "", "", "", "left", "", "", "") ' <td> 
				cmdBookmark.RenderControl(wr)
				RenderCellEnd(wr) ' </td>
			End If

			' Display rating only if user is authenticated
			If PostCollection.Count > 0 Then
				'check to see if new setting, enable ratings is enabled
				If objConfig.EnableRatings And ThreadInfo.HostForum.EnableForumsRating Then
					RenderCellBegin(wr, "", "", "", "left", "", "", "") ' <td> 
					'CP - Sub in ajax image rating solution here for ddl
					trcRating.RenderControl(wr)

					' See if user has set status, if so we need to bind it
					RenderCellEnd(wr) ' </td>

					RenderCellBegin(wr, "", "", "", "left", "", "", "")  ' <td> '
					RenderCellEnd(wr) ' </td>
				End If
			Else
				RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td> 
				wr.Write("&nbsp;")
				RenderCellEnd(wr) ' </td>
			End If

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "", "", "100%", "right", "middle", "", "")
			RenderTableBegin(wr, 0, 0, "InnerTable") '<table>
			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td>
			txtForumSearch.RenderControl(wr)
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td>
			cmdForumSearch.RenderControl(wr)
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' Renders the row w/ the navigation breadcrumb
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderBreadCrumbRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td></td>
			RenderCellBegin(wr, "", "", "100%", "left", "bottom", "", "") ' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "", "", "100%", "", "", "2", "") ' <td> 

			Dim tempForumID As Integer
			If Not HttpContext.Current.Request.QueryString("forumid") Is Nothing Then
				tempForumID = Int32.Parse(HttpContext.Current.Request.QueryString("forumid"))
			End If
			Dim ChildGroupView As Boolean = False
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				ChildGroupView = True
			End If
			wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Posts, ThreadInfo, objConfig, ChildGroupView))
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>
			RenderRowBegin(wr) '<tr>

			RenderCellBegin(wr, "", "", "100%", "", "", "2", "") ' <td> 
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>
			RenderRowBegin(wr) '<tr>

			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") ' <td> 
			RenderCellEnd(wr) ' </td>

			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </Td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderRowEnd(wr) ' </Tr>
		End Sub

		''' <summary>
		''' Renders the area directly above the post including: New Thread, prev/next
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderThread(ByVal wr As HtmlTextWriter)
			Dim fSubject As String

			If PostCollection.Count > 0 Then
				Dim firstPost As PostInfo = CType(PostCollection(0), PostInfo)
				fSubject = String.Format("&nbsp;{0}", firstPost.Subject)
				' filter bad words if required in forum settings
				If ForumControl.objConfig.FilterSubject Then
					fSubject = Utilities.ForumUtils.FormatProhibitedWord(fSubject, firstPost.CreatedDate, PortalID)
				End If
			Else
				fSubject = ForumControl.LocalizedText("NoPost")
			End If

			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")

			RenderCellBegin(wr, "", "", "100%", "left", "", "", "") '<td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "left", "", "0") ' <table>
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "70%", "left", "middle", "", "")  '<td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") '<Table>            
			RenderRowBegin(wr) '<tr>

			RenderCellBegin(wr, "", "", "", "", "middle", "", "")	'<td>           

			' new thread button
			'Remove LoggedOnUserID limitation if wishing to implement Anonymous Posting
			If (CurrentForumUser.UserID > 0) And (Not ForumId = -1) Then
				If Not ParentForum.PublicPosting Then
					Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

					If objSecurity.IsAllowedToStartRestrictedThread Then
						RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
						RenderRowBegin(wr) '<tr>
						_url = Utilities.Links.NewThreadLink(TabID, ForumId, ModuleID)
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

						If CurrentForumUser.IsBanned Then
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
						Else
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
						End If

						RenderCellEnd(wr) ' </Td>

						If CurrentForumUser.IsBanned Or (Not objSecurity.IsAllowedToPostRestrictedReply) Or (ThreadInfo.IsClosed) Then
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link", False)
							RenderCellEnd(wr) ' </Td>
						Else
							_url = Utilities.Links.NewPostLink(TabID, ForumId, ThreadInfo.ThreadID, "reply", ModuleID)
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link")
							RenderCellEnd(wr) ' </Td>
						End If

						'[skeel] moved delete thread here
						If CurrentForumUser.UserID > 0 AndAlso (objSecurity.IsForumModerator) Then

							_url = Utilities.Links.ThreadDeleteLink(TabID, ModuleID, ForumId, ThreadId, False)
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("DeleteThread"), "Forum_Link")
							RenderCellEnd(wr) ' </Td>
						End If

						RenderRowEnd(wr) ' </tr>
						RenderTableEnd(wr) ' </table>
					ElseIf objSecurity.IsAllowedToPostRestrictedReply Then
						RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
						RenderRowBegin(wr) '<tr>

						If CurrentForumUser.IsBanned Or ThreadInfo.IsClosed Then
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link", False)
							RenderCellEnd(wr) ' </Td>
						Else
							_url = Utilities.Links.NewPostLink(TabID, ForumId, ThreadInfo.ThreadID, "reply", ModuleID)
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link")
							RenderCellEnd(wr) ' </Td>
						End If

						RenderRowEnd(wr) ' </tr>
						RenderTableEnd(wr) ' </table>
					Else
						' user cannot start thread or make a reply
						wr.Write("&nbsp;")
					End If
				Else
					' no posting restrictions
					RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
					RenderRowBegin(wr) '<tr>
					_url = Utilities.Links.NewThreadLink(TabID, ForumId, ModuleID)
					RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

					If CurrentForumUser.IsBanned Then
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
					Else
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
					End If

					RenderCellEnd(wr) ' </Td>

					If CurrentForumUser.IsBanned Or ThreadInfo.IsClosed Then
						RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
						wr.Write("&nbsp;")
						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link", False)
						RenderCellEnd(wr) ' </Td>
					Else
						_url = Utilities.Links.NewPostLink(TabID, ForumId, ThreadInfo.ThreadID, "reply", ModuleID)
						RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
						wr.Write("&nbsp;")
						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link")
						RenderCellEnd(wr) ' </Td>
					End If

					'[skeel] moved delete thread here
					Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)
					If CurrentForumUser.UserID > 0 AndAlso (objSecurity.IsForumModerator) Then
						_url = Utilities.Links.ThreadDeleteLink(TabID, ModuleID, ForumId, ThreadId, False)
						RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
						wr.Write("&nbsp;")
						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("DeleteThread"), "Forum_Link")
						RenderCellEnd(wr) ' </Td>
					End If

					RenderRowEnd(wr) ' </tr>
					RenderTableEnd(wr) ' </table>
				End If
			End If

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			' Thread navigation
			RenderCellBegin(wr, "", "", "30%", "right", "", "", "")  '<td> 
			RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
			RenderRowBegin(wr) '<tr>
			Dim PreviousEnabled As Boolean = False
			Dim EnabledText As String = "Disabled"

			If Not (ThreadInfo.PreviousThreadID = 0) Then
				If Not (ThreadInfo.IsPinned) Then
					PreviousEnabled = True
					EnabledText = "Previous"
				End If
			End If

			If PreviousEnabled Then
				RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "", "", "")  ' <td> ' 
			Else
				RenderCellBegin(wr, "Forum_NavBarButtonDisabled", "", "", "", "", "", "")	' <td> ' 
			End If
			RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
			RenderRowBegin(wr) '<tr>

			_url = Utilities.Links.ContainerViewThreadLink(TabID, ForumId, ThreadInfo.PreviousThreadID)

			RenderCellBegin(wr, "", "", "", "", "", "", "")  ' <td> ' 
			If PreviousEnabled Then
				RenderLinkButton(wr, _url, ForumControl.LocalizedText("Previous"), "Forum_Link")
			Else
				RenderDivBegin(wr, "", "Forum_NormalBold")
				wr.Write(ForumControl.LocalizedText("Previous"))
				RenderDivEnd(wr)
			End If
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>    

			RenderCellBegin(wr, "", "", "", "", "", "", "")  ' <td> 
			wr.Write("&nbsp;")
			RenderCellEnd(wr) ' </td>

			Dim NextEnabled As Boolean = False
			Dim NextText As String = "Disabled"
			If Not (ThreadInfo.NextThreadID = 0) Then
				If Not (ThreadInfo.IsPinned = True) Then
					NextEnabled = True
					NextText = "Next"
				End If
			End If

			If NextEnabled Then
				RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "", "", "")  ' <td> '
			Else
				RenderCellBegin(wr, "Forum_NavBarButtonDisabled", "", "", "", "", "", "")	' <td> '
			End If

			RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "", "", "", "", "")  ' <td> ' 

			If NextEnabled Then
				_url = Utilities.Links.ContainerViewThreadLink(TabID, ForumId, ThreadInfo.NextThreadID)
				RenderLinkButton(wr, _url, ForumControl.LocalizedText("Next"), "Forum_Link", NextEnabled)
			Else
				RenderDivBegin(wr, "", "Forum_NormalBold")
				wr.Write(ForumControl.LocalizedText("Next"))
				RenderDivEnd(wr)
			End If
			RenderCellEnd(wr) ' </td>   
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table> 
			RenderCellEnd(wr) ' </td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderRowEnd(wr) ' </tr>       

			'CP - Spacer Row between final post footer and bottom panel
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") '<td>
			RenderCellEnd(wr) ' </td> 
			RenderCapCell(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
			RenderRowEnd(wr) ' </tr>
			'End spacer row

			' Handle polls
			If ThreadInfo.HostForum.AllowPolls And ThreadInfo.PollID > 0 And CurrentForumUser.UserID > 0 Then
				RenderPoll(wr)
			End If

			' Loop round rows in selected thread (These are rows w/ user avatar/alias, post body)
			RenderPosts(wr)
		End Sub

		''' <summary>
		''' Renders a poll or the results (possibly thank you message if show results are off) if one is attached to a thread.
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderPoll(ByVal wr As HtmlTextWriter)
			'new row
			RenderRowBegin(wr) '<tr>                
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")

			' middle master column (this will hold a table, the poll post will display here, then render a spacer row to seperate it from the other posts 
			RenderCellBegin(wr, "", "", "", "left", "middle", "", "")
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")	' <table> 
			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") ' <td>

			' table to hold poll header
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr, "") ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapLeft", "") ' <td><img /></td>
			RenderCellBegin(wr, "Forum_Header", "", "", "", "", "", "")	   '<td>

			Dim cntPoll As New PollController
			Dim objPoll As New PollInfo
			objPoll = cntPoll.GetPoll(ThreadInfo.PollID)

			RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
			wr.Write("&nbsp;" & ForumControl.LocalizedText("Poll") & ": " & objPoll.Question)
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td> 
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapRight", "") ' <td><img /></td>
			RenderRowEnd(wr) ' </tr>

			RenderTableEnd(wr) ' </table>

			RenderCellEnd(wr) ' </td> 
			RenderRowEnd(wr) ' </tr>

			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "Forum_Avatar", "", "100%", "center", "middle", "", "")    '<td>

			Dim showPoll As Boolean = True
			If Not objPoll.PollClosed Then
				For Each objUserAnswer As UserAnswerInfo In objPoll.UserAnswers
					If objUserAnswer.UserID = CurrentForumUser.UserID Then
						showPoll = False
						Exit For
					End If
				Next
			End If

			If showPoll And (Not objPoll.PollClosed) Then
				' if the user hasn't voted, show the the poll
				rblstPoll.RenderControl(wr)
				cmdVote.RenderControl(wr)
				' Not implemented
				'If (LoggedOnUserID = ThreadInfo.StartedByUserID) Or (Security.IsForumModerator) Then
				'    'cmdViewResults.RenderControl(wr)
				'End If
			Else
				' check to see if we are able to show user results
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

				If objPoll.ShowResults Or ((CurrentForumUser.UserID = ThreadInfo.StartedByUserID) Or (objSecurity.IsForumModerator)) Then
					' show results
					RenderTableBegin(wr, "", "", "", "", "0", "0", "center", "middle", "")  ' <table> 

					For Each objAnswer As AnswerInfo In objPoll.Answers
						Dim cntAnswer As New AnswerController
						objAnswer = cntAnswer.GetAnswer(objAnswer.AnswerID)

						' create a row representing results
						RenderRowBegin(wr) ' <tr>
						RenderCellBegin(wr, "", "", "", "left", "middle", "", "")	 '<td>

						' show answer
						RenderDivBegin(wr, "", "Forum_Normal") ' <span>
						wr.Write(objAnswer.Answer & "&nbsp;")
						RenderDivEnd(wr) ' </span>
						RenderCellEnd(wr) ' </td>

						' handle calculation
						Dim Percentage As Double
						If objPoll.TotalVotes = 0 Then
							Percentage = 0
						Else
							Percentage = (objAnswer.AnswerCount / objPoll.TotalVotes) * 100
						End If

						Dim strVoteCount As String
						strVoteCount = objAnswer.AnswerCount.ToString()
						strVoteCount = strVoteCount + " " + Localization.GetString("Votes", objConfig.SharedResourceFile)

						' show image
						RenderCellBegin(wr, "", "", "", "left", "middle", "", "")	 '<td>
						If CType(Percentage, Integer) > 0 Then
							RenderImage(wr, objConfig.GetThemeImageURL("poll_capleft.") & objConfig.ImageExtension, strVoteCount, "")
							' handle this biatch
							Dim i As Integer = 0
							For i = 0 To CType(Percentage, Integer)
								RenderImage(wr, objConfig.GetThemeImageURL("poll_bar.") & objConfig.ImageExtension, strVoteCount, "")
							Next
							RenderImage(wr, objConfig.GetThemeImageURL("poll_capright.") & objConfig.ImageExtension, strVoteCount, "")
						End If
						wr.Write("&nbsp;")
						RenderCellEnd(wr) ' </td>

						' show percentage
						RenderCellBegin(wr, "", "", "", "right", "middle", "", "")	  '<td>
						RenderDivBegin(wr, "", "Forum_Normal") ' <span>
						wr.Write(FormatNumber(Percentage, 2).ToString() & " %")
						RenderDivEnd(wr) ' </span>
						RenderCellEnd(wr) ' </td>
						RenderRowEnd(wr) ' </tr>
					Next

					RenderRowBegin(wr) ' <tr>
					RenderCellBegin(wr, "", "", "100%", "center", "middle", "3", "")	   '<td>
					RenderDivBegin(wr, "", "Forum_NormalBold") ' <span>
					wr.RenderBeginTag(HtmlTextWriterTag.B)
					wr.Write(Localization.GetString("TotalVotes", objConfig.SharedResourceFile) & " " & objPoll.TotalVotes.ToString())
					wr.RenderEndTag()
					RenderDivEnd(wr) ' </span>
					RenderCellEnd(wr) ' </td>
					RenderRowEnd(wr) ' </tr>

					'' View Details Row (Not Implemented)
					'RenderRowBegin(wr) ' <tr>
					'RenderCellBegin(wr, "", "", "100%", "center", "middle", "3", "")    '<td>
					'RenderSpanBegin(wr, "", "Forum_Normal") ' <span>
					'wr.Write("Total Votes: " & objPoll.TotalVotes.ToString())
					'RenderSpanEnd(wr) ' </span>
					'RenderCellEnd(wr) ' </td>
					'RenderRowEnd(wr) ' </tr>

					RenderTableEnd(wr) ' </table>
				Else
					RenderDivBegin(wr, "", "Forum_Normal") ' <span>
					wr.Write(objPoll.TakenMessage)
					RenderDivEnd(wr) ' </span>
				End If
			End If
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>

			RenderRowBegin(wr) '<tr> 
			RenderCellBegin(wr, "Forum_SpacerRow", "", "", "", "", "", "")  ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "")
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td> 
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")

			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' posts make up all rows in between (fourth row to third to last row, numerous rows)
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderPosts(ByVal wr As HtmlTextWriter)
			Try
				' use a counter to determine odd/even for alternating colors (via css)
				Dim intPostCount As Integer = 1
				Dim totalPostCount As Integer = PostCollection.Count
				Dim currentCount As Integer = 1

				RenderRowBegin(wr) '<tr>                
				RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
				RenderCellBegin(wr, "", "", "100%", "", "top", "", "")	' <td>
				RenderTableBegin(wr, "", "", "", "100%", "0", "0", "center", "", "0")	 ' <table> 

				For Each Post As PostInfo In PostCollection
					Dim parent As New ThreadInfo()

					Try
						parent = Post.ParentThread
					Catch exc As Exception
						If parent Is Nothing Then
							Services.Exceptions.ProcessPageLoadException(New Exception("Forum post " + Post.PostID.ToString + " has no parent.", exc))
						Else
							Services.Exceptions.ProcessPageLoadException(New Exception("Post object is nothing.", exc))
						End If
						Exit Sub
					End Try

					Dim postCountIsEven As Boolean = ThreadIsEven(intPostCount)
					Me.RenderPost(wr, Post, postCountIsEven, True)
					' spacer row should be displayed in flat view only
					If Not currentCount = totalPostCount Then
						RenderSpacerRow(wr)
						currentCount += 1
					End If
					intPostCount += 1
				Next
				RenderTableEnd(wr) ' </table>
				RenderCellEnd(wr) ' </td> 
				RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
				RenderRowEnd(wr) ' </tr>
			Catch ex As Exception
				LogException(ex)
			End Try
		End Sub

		''' <summary>
		''' Renders the entire table structure of a single post
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Post"></param>
		''' <param name="PostCountIsEven"></param>
		''' <param name="ShowDetails"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderPost(ByVal wr As HtmlTextWriter, ByVal Post As PostInfo, ByVal PostCountIsEven As Boolean, ByVal ShowDetails As Boolean)
			Dim authorCellClass As String
			Dim bodyCellClass As String

			' these classes to set bg color of cells
			If PostCountIsEven Then
				authorCellClass = "Forum_Avatar"
				bodyCellClass = "Forum_PostBody_Container"
			Else
				authorCellClass = "Forum_Avatar_Alt"
				bodyCellClass = "Forum_PostBody_Container_Alt"
			End If

			'Add per post header - better UI can add more info
			If ShowDetails Then
				Dim strPostedDate As String = String.Empty
				strPostedDate = Utilities.ForumUtils.GetCreatedDateInfo(Post.CreatedDate, objConfig, "").ToString

				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, "", "", "100%", "left", "middle", "2", "")	'<td>
				'[skeel] Check if first new post and add bookmark used for navigation
				If HttpContext.Current.Request IsNot Nothing Then
					If HttpContext.Current.Request.IsAuthenticated And newpost = False Then
						If Post.NewPost(CurrentForumUser.UserID) Then
							RenderPostBookmark(wr, "unread")
							newpost = True
						End If
					End If
				End If

				'[skeel] add Bookmark to post
				RenderPostBookmark(wr, "p" & CStr(Post.PostID))
				'Make table to hold per post header
				RenderTableBegin(wr, "", "", "", "100%", "0", "0", "center", "middle", "0")  ' <table> 
				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, "", "", "100%", "left", "middle", "", "")   '<td>
				RenderTableBegin(wr, "", "", "", "100%", "0", "0", "center", "middle", "0")  ' <table> 

				RenderRowBegin(wr) ' <tr>
				RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapLeft", "") ' <td><img /></td>


				' start post status image
				RenderCellBegin(wr, "Forum_Header", "", "1%", "left", "", "", "") '<td>
				' display "new" image if this post is new since last time user visited the thread
				If HttpContext.Current.Request IsNot Nothing Then
					If HttpContext.Current.Request.IsAuthenticated Then
						If Post.NewPost(CurrentForumUser.UserID) Then
							RenderImage(wr, objConfig.GetThemeImageURL("s_new.") & objConfig.ImageExtension, ForumControl.LocalizedText("UnreadPost"), "")
						Else
							RenderImage(wr, objConfig.GetThemeImageURL("s_old.") & objConfig.ImageExtension, ForumControl.LocalizedText("ReadPost"), "")
						End If
					Else
						RenderImage(wr, objConfig.GetThemeImageURL("s_new.") & objConfig.ImageExtension, ForumControl.LocalizedText("UnreadPost"), "")
					End If
				Else
					RenderImage(wr, objConfig.GetThemeImageURL("s_new.") & objConfig.ImageExtension, ForumControl.LocalizedText("UnreadPost"), "")
				End If
				RenderCellEnd(wr) ' </td> 

				RenderCellBegin(wr, "Forum_Header", "", "89%", "left", "", "", "")		'<td>
				RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
				wr.Write(strPostedDate)
				RenderDivEnd(wr) ' </span>
				RenderCellEnd(wr) ' </td> 

				RenderCellBegin(wr, "Forum_Header", "", "", "right", "", "", "")	   '<td>

				' if the user is the original author or a moderator AND this is the original post
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

				If ((CurrentForumUser.UserID = Post.ParentThread.StartedByUserID) Or (objSecurity.IsForumModerator)) And Post.ParentPostID = 0 Then
					If Post.ParentThread.ThreadStatus = ThreadStatus.Poll Then
						ddlThreadStatus.Enabled = False
					End If
					ddlThreadStatus.RenderControl(wr)
					'wr.Write("&nbsp;")
				Else
					' this is either not the original post or the user is not the author or a moderator
					' If the thread is answered AND this is the post accepted as the answer
					If Post.ParentThread.ThreadStatus = ThreadStatus.Answered And (Post.ParentThread.AnswerPostID = Post.PostID) And ThreadInfo.HostForum.EnableForumsThreadStatus Then
						RenderDivBegin(wr, "", "Forum_AnswerText") ' <span>
						wr.Write(ForumControl.LocalizedText("AcceptedAnswer"))
						wr.Write("&nbsp;")
						RenderDivEnd(wr) ' </span>
						' If the thread is NOT answered AND this user started the post or is a moderator of some sort
					ElseIf ((CurrentForumUser.UserID = Post.ParentThread.StartedByUserID) Or (objSecurity.IsForumModerator)) And (Post.ParentThread.ThreadStatus = ThreadStatus.Unanswered) And ThreadInfo.HostForum.EnableForumsThreadStatus Then
						' Select the proper command argument (set before rendering)
						If hsThreadAnswers.ContainsKey(Post.PostID) Then
							cmdThreadAnswer = CType(hsThreadAnswers(Post.PostID), LinkButton)
							cmdThreadAnswer.CommandArgument = Post.PostID.ToString
							cmdThreadAnswer.RenderControl(wr)
							wr.Write("&nbsp;")
							wr.Write("&nbsp;")
						End If
						' all that can be left worth displaying is if the post is the original, show the status icon
					Else
						wr.Write("&nbsp;")
					End If
				End If
				RenderCellEnd(wr) ' </td> 

				RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapRight", "")

				RenderRowEnd(wr) ' </tr>
				RenderTableEnd(wr) ' </table>
				RenderCellEnd(wr) ' </td> 

				RenderRowEnd(wr) ' </tr>
				RenderTableEnd(wr) ' </table>

				RenderCellEnd(wr) ' </td> 
				RenderRowEnd(wr) ' </tr>
			End If
			RenderRowBegin(wr) ' <tr>

			' Author area
			RenderCellBegin(wr, authorCellClass, "", "20%", "center", "top", "1", "1")	 ' <td>
			Me.RenderAuthor(wr, Post, PostCountIsEven, ShowDetails)
			RenderCellEnd(wr) ' </td> 

			' post area
			' cell for post details (subject, buttons)
			RenderCellBegin(wr, bodyCellClass, "100%", "80%", "left", "top", "", "")	  '<td>
			RenderPostHeader(wr, Post, PostCountIsEven, ShowDetails)
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' Builds the left cell for RenderPost (author, rank, avatar area)
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Post"></param>
		''' <param name="PostCountIsEven"></param>
		''' <param name="ShowDetails"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderAuthor(ByVal wr As HtmlTextWriter, ByVal Post As PostInfo, ByVal PostCountIsEven As Boolean, ByVal ShowDetails As Boolean)
			If Not Post Is Nothing Then
				Dim author As ForumUserInfo = Post.Author
				Dim authorOnline As Boolean = (author.EnableOnlineStatus AndAlso author.IsOnline AndAlso (ForumControl.objConfig.EnableUsersOnline))
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

				' table to display integrated media, user alias, poster rank, avatar, homepage, and number of posts.
				RenderTableBegin(wr, "", "Forum_PostAuthorTable", "", "100%", "0", "0", "", "", "")

				' row to display user alias and online status
				RenderRowBegin(wr) '<tr> 

				'link to user profile, always display in both views
				If Not objConfig.EnableExternalProfile Then
					_url = author.UserCoreProfileLink
				Else
					_url = Utilities.Links.UserExternalProfileLink(author.UserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, author.Username)
				End If

				RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td>

				' display user online status
				If objConfig.EnableUsersOnline Then
					RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "") ' <table>
					RenderRowBegin(wr) ' <tr>
					RenderCellBegin(wr, "", "", "", "", "middle", "", "")	' <td> 
					If authorOnline Then
						RenderImage(wr, objConfig.GetThemeImageURL("s_online.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgOnline"), "")
					Else
						RenderImage(wr, objConfig.GetThemeImageURL("s_offline.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgOffline"), "")
					End If
					RenderCellEnd(wr) ' </td>

					RenderCellBegin(wr, "", "", "", "", "middle", "", "")	 ' <td>
					wr.Write("&nbsp;")
					RenderTitleLinkButton(wr, _url, author.SiteAlias, "Forum_Profile", ForumControl.LocalizedText("ViewProfile"))
					RenderCellEnd(wr) ' </td>

					Dim objSecurity2 As New Forum.ModuleSecurity(ModuleID, TabID, -1, CurrentForumUser.UserID)

					If objSecurity2.IsModerator Then
						RenderCellBegin(wr, "", "", "", "", "middle", "", "")	 ' <td>
						wr.Write("&nbsp;")
						RenderImageButton(wr, Utilities.Links.UCP_AdminLinks(TabID, ModuleID, author.UserID, UserAjaxControl.Profile), objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension, ForumControl.LocalizedText("EditProfile"), "")
						RenderCellEnd(wr) ' </td>
					End If

					RenderRowEnd(wr) ' </tr>
					RenderTableEnd(wr) ' </table>
				Else
					RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "") ' <table>
					RenderRowBegin(wr) ' <tr>
					RenderCellBegin(wr, "", "", "", "", "middle", "", "")	' <td> 
					RenderTitleLinkButton(wr, _url, author.SiteAlias, "Forum_Profile", ForumControl.LocalizedText("ViewProfile"))
					RenderCellEnd(wr) ' </td>

					Dim objSecurity2 As New Forum.ModuleSecurity(ModuleID, TabID, -1, CurrentForumUser.UserID)

					If objSecurity2.IsModerator Then
						RenderCellBegin(wr, "", "", "", "", "middle", "", "")	 ' <td>
						wr.Write("&nbsp;")
						RenderImageButton(wr, Utilities.Links.UCP_AdminLinks(TabID, ModuleID, author.UserID, UserAjaxControl.Profile), objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension, ForumControl.LocalizedText("EditProfile"), "")
						RenderCellEnd(wr) ' </td>
					End If

					RenderRowEnd(wr) ' </tr>
					RenderTableEnd(wr) ' </table>
				End If

				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr> (end user alias/online)  

				If ShowDetails Then	' Display author details only in flatView
					' display user ranking 
					If (objConfig.Ranking) Then
						Dim authorRank As PosterRank = Utilities.ForumUtils.GetRank(author, ForumControl.objConfig)
						Dim rankImage As String = String.Format("Rank_{0}." & objConfig.ImageExtension, CType(authorRank, Integer).ToString)
						Dim rankURL As String = objConfig.GetThemeImageURL(rankImage)
						Dim RankTitle As String = Utilities.ForumUtils.GetRankTitle(authorRank, objConfig)

						RenderRowBegin(wr) ' <tr> (start ranking row)
						RenderCellBegin(wr, "", "", "", "", "top", "", "") ' <td>
						If objConfig.EnableRankingImage Then
							RenderImage(wr, rankURL, RankTitle, "")
						Else
							RenderDivBegin(wr, "", "Forum_NormalSmall")
							wr.Write(RankTitle)
							RenderDivEnd(wr)
						End If
						RenderCellEnd(wr) ' </td>
						RenderRowEnd(wr) ' </tr>
					End If

					' display user avatar
					If objConfig.EnableUserAvatar AndAlso (String.IsNullOrEmpty(author.AvatarComplete) = False) Then
						RenderRowBegin(wr) ' <tr> (start avatar row)
						RenderCellBegin(wr, "Forum_UserAvatar", "", "", "", "top", "", "") ' <td>
						wr.Write("<br />")
						If objConfig.EnableProfileAvatar And author.UserID > 0 Then
							If Not author.IsSuperUser Then
								Dim WebVisibility As UserVisibilityMode
								WebVisibility = author.Profile.ProfileProperties(objConfig.AvatarProfilePropName).Visibility

								Select Case WebVisibility
									Case UserVisibilityMode.AdminOnly
										If objSecurity.IsForumAdmin Then
											RenderProfileAvatar(author, wr)
										End If
									Case UserVisibilityMode.AllUsers
										RenderProfileAvatar(author, wr)
									Case UserVisibilityMode.MembersOnly
										If CurrentForumUser.UserID > 0 Then
											RenderProfileAvatar(author, wr)
										End If
								End Select
							End If
						Else
							If author.UserID > 0 Then
								RenderImage(wr, author.AvatarComplete, author.SiteAlias & "'s " & ForumControl.LocalizedText("Avatar"), "")
							End If
						End If

						RenderCellEnd(wr) ' </td>
						RenderRowEnd(wr) ' </tr>
					End If

					' display system avatars (ie. DNN Core avatar)
					If objConfig.EnableSystemAvatar AndAlso (Not author.SystemAvatars = String.Empty) Then
						Dim SystemAvatar As String
						For Each SystemAvatar In author.SystemAvatarsComplete.Trim(";"c).Split(";"c)
							If SystemAvatar.Length > 0 AndAlso (Not SystemAvatar.ToLower = "standard") Then
								Dim SystemAvatarUrl As String = SystemAvatar
								RenderRowBegin(wr) ' <tr> (start system avatar row) 
								RenderCellBegin(wr, "Forum_NormalSmall", "", "", "", "top", "", "") ' <td>
								wr.Write("<br />")
								RenderImage(wr, SystemAvatarUrl, author.SiteAlias & "'s " & ForumControl.LocalizedText("Avatar"), "")
								RenderCellEnd(wr) ' </td>
								RenderRowEnd(wr) ' </tr>
							End If
						Next

					End If

					'Now for RoleBased Avatars
					If objConfig.EnableRoleAvatar AndAlso (Not author.RoleAvatar = ";") Then
						Dim RoleAvatar As String
						For Each RoleAvatar In author.RoleAvatarComplete.Trim(";"c).Split(";"c)
							If RoleAvatar.Length > 0 AndAlso (Not RoleAvatar.ToLower = "standard") Then
								Dim RoleAvatarUrl As String = RoleAvatar
								RenderRowBegin(wr) ' <tr> (start system avatar row) 
								RenderCellBegin(wr, "Forum_NormalSmall", "", "", "", "top", "", "") ' <td>
								wr.Write("<br />")
								RenderImage(wr, RoleAvatarUrl, author.SiteAlias & "'s " & ForumControl.LocalizedText("Avatar"), "")
								RenderCellEnd(wr) ' </td>
								RenderRowEnd(wr) ' </tr>
							End If
						Next
					End If

					'Author information
					RenderRowBegin(wr) ' <tr> 
					RenderCellBegin(wr, "Forum_NormalSmall", "", "", "", "top", "", "")	' <td>

					'Homepage
					If author.UserID > 0 Then
						Dim WebSiteVisibility As UserVisibilityMode
						WebSiteVisibility = author.Profile.ProfileProperties("Website").Visibility

						Select Case WebSiteVisibility
							Case UserVisibilityMode.AdminOnly
								If objSecurity.IsForumAdmin Then
									RenderWebSiteLink(author, wr)
								End If
							Case UserVisibilityMode.AllUsers
								RenderWebSiteLink(author, wr)
							Case UserVisibilityMode.MembersOnly
								If CurrentForumUser.UserID > 0 Then
									RenderWebSiteLink(author, wr)
								End If
						End Select

						'Region
						Dim CountryVisibility As UserVisibilityMode
						CountryVisibility = author.Profile.ProfileProperties("Country").Visibility

						Select Case CountryVisibility
							Case UserVisibilityMode.AdminOnly
								If objSecurity.IsForumAdmin Then
									RenderCountry(author, wr)
								End If
							Case UserVisibilityMode.AllUsers
								RenderCountry(author, wr)
							Case UserVisibilityMode.MembersOnly
								If CurrentForumUser.UserID > 0 Then
									RenderCountry(author, wr)
								End If
						End Select
					End If

					'Joined
					Dim strJoinedDate As String
					Dim displayCreatedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(CType(author.Membership.CreatedDate, DateTime), objConfig)
					strJoinedDate = ForumControl.LocalizedText("Joined") & ": " & displayCreatedDate.ToShortDateString
					wr.Write("<br />" & strJoinedDate)

					'Post count
					RenderDivBegin(wr, "spAuthorPostCount", "Forum_NormalSmall")
					wr.Write(ForumControl.LocalizedText("PostCount").Replace("[PostCount]", author.PostCount.ToString))
					RenderDivEnd(wr)

					RenderCellEnd(wr) ' </td>
					RenderRowEnd(wr) ' </tr>
				End If

				RenderTableEnd(wr) ' </table>  (End of user avatar/alias table, close td next)
			End If
		End Sub

		''' <summary>
		''' Renders the user's profile avatar. 
		''' </summary>
		''' <param name="author"></param>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderProfileAvatar(ByVal author As ForumUserInfo, ByVal wr As HtmlTextWriter)
			' This needs to be rendered w/ specified size
			If objConfig.EnableProfileUserFolders Then
				' The link click below (duplicated from core profile page) presents some serious issues under volume. 
				'imgUserProfileAvatar.ImageUrl = DotNetNuke.Common.Globals.LinkClick("fileid=" & author.AvatarFile.FileId.ToString(), PortalSettings.ActiveTab.TabID, Null.NullInteger)
				If author.AvatarCoreFile IsNot Nothing Then
					Dim imgUserProfileAvatar As New Image

					imgUserProfileAvatar.ImageUrl = author.AvatarComplete
					DotNetNuke.Web.UI.Utilities.CreateThumbnail(author.AvatarCoreFile, imgUserProfileAvatar, objConfig.UserAvatarWidth, objConfig.UserAvatarHeight)

					imgUserProfileAvatar.RenderControl(wr)
					imgUserProfileAvatar.Visible = True
				End If
			Else
				' If we are here, file stored as name and not id (in UserProfile table).
				Dim rbiProfileAvatar As New Telerik.Web.UI.RadBinaryImage
				rbiProfileAvatar.Width = objConfig.UserAvatarWidth
				rbiProfileAvatar.Height = objConfig.UserAvatarHeight
				rbiProfileAvatar.ImageUrl = author.AvatarComplete

				rbiProfileAvatar.RenderControl(wr)
			End If

			' Below is for use when no Telerik integration is going on. (Uncomment line below, comment out lines above)
			'RenderImage(wr, author.AvatarComplete, author.SiteAlias & "'s " & ForumControl.LocalizedText("Avatar"), "", objConfig.UserAvatarWidth.ToString(), objConfig.UserAvatarHeight.ToString())
		End Sub

		''' <summary>
		''' Renders the user's website (as a link). 
		''' </summary>
		''' <param name="author"></param>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderWebSiteLink(ByVal author As ForumUserInfo, ByVal wr As HtmlTextWriter)
			If Len(author.UserWebsite) > 0 Then
				wr.Write("<br />")
				RenderLinkButton(wr, author.UserWebsite, Replace(author.UserWebsite, "http://", ""), "Forum_Profile", "", True, objConfig.NoFollowWeb)
			End If
		End Sub

		''' <summary>
		''' Renders the user's country. 
		''' </summary>
		''' <param name="author"></param>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderCountry(ByVal author As ForumUserInfo, ByVal wr As HtmlTextWriter)
			If objConfig.DisplayPosterRegion And Len(author.Profile.Region) > 0 Then
				wr.Write("<br />" & ForumControl.LocalizedText("Region") & ": " & author.Profile.Region)
			End If
		End Sub

		''' <summary>
		''' Builds the post details: subject, user location, edited, created date
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Post"></param>
		''' <param name="PostCountIsEven"></param>
		''' <param name="ShowDetails"></param>
		''' <remarks></remarks>
		Private Sub RenderPostHeader(ByVal wr As HtmlTextWriter, ByVal Post As PostInfo, ByVal PostCountIsEven As Boolean, ByVal ShowDetails As Boolean)
			Dim user As ForumUserInfo = CurrentForumUser
			Dim detailCellClass As String = String.Empty
			Dim buttonCellClass As String = String.Empty
			Dim strSubject As String = String.Empty
			Dim strCreatedDate As String = String.Empty
			Dim strAuthorLocation As String = String.Empty

			Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

			If PostCountIsEven Then
				detailCellClass = "Forum_PostDetails"
				buttonCellClass = "Forum_PostButtons"
			Else
				detailCellClass = "Forum_PostDetails_Alt"
				buttonCellClass = "Forum_PostButtons_Alt"
			End If

			If ForumControl.objConfig.FilterSubject Then
				strSubject = Utilities.ForumUtils.FormatProhibitedWord(Post.Subject, Post.CreatedDate, PortalID)
			Else
				strSubject = Post.Subject
			End If

			'CP - Possible change for foreign culture date displays
			strCreatedDate = ForumControl.LocalizedText("PostedDateTime")
			Dim displayCreatedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(Post.CreatedDate, objConfig)
			strCreatedDate = strCreatedDate.Replace("[CreatedDate]", displayCreatedDate.ToString("dd MMM yy"))
			strCreatedDate = strCreatedDate.Replace("[PostTime]", displayCreatedDate.ToString("t"))

			' display poster location 
			If (Not objConfig.DisplayPosterLocation = ShowPosterLocation.None) Then
				If ((objConfig.DisplayPosterLocation = ShowPosterLocation.ToAdmin) AndAlso (objSecurity.IsForumAdmin)) OrElse (objConfig.DisplayPosterLocation = ShowPosterLocation.ToAll) Then
					If (Not Post.RemoteAddr.Length = 0) AndAlso (Not Post.RemoteAddr = "127.0.0.1") AndAlso (Not Post.RemoteAddr = "::1") Then
						strAuthorLocation = String.Format("&nbsp;({0})", Utilities.ForumUtils.LookupCountry(Post.RemoteAddr))
						' This will show the ip in italics (This should only show to moderators) 
						If objSecurity.IsForumModerator Then
							strAuthorLocation = strAuthorLocation & "<EM> (" & Post.RemoteAddr & ")</EM>"
						End If
					End If
				End If
			End If
			RenderTableBegin(wr, Post.PostID.ToString, "", "100%", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>

			'Indent based on post level in treeview
			If Not ShowDetails Then
				Dim iCount As Integer

				RenderCellBegin(wr, detailCellClass, "", "70%", "left", "top", "", "") ' <td>
				For iCount = 1 To Post.PostLevel
					wr.Write("..")
				Next
			Else
				RenderCellBegin(wr, "", "", "100%", "", "", "", "") ' <td>
				RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
				RenderRowBegin(wr) ' <tr>

				RenderCellBegin(wr, detailCellClass, "", "100%", "left", "top", "", "") ' <td>
			End If

			If ShowDetails Then
				'[skeel] Subject now works as a direct link to a specific post!
				RenderDivBegin(wr, "spCreatedDate", "Forum_Normal") ' <span>
				Me.RenderLinkButton(wr, Utilities.Links.ContainerViewPostLink(TabID, Post.ForumID, Post.PostID), strSubject, "Forum_NormalBold")
				wr.Write("&nbsp;")
				wr.Write(strAuthorLocation)

				' display edited tag if post has been modified
				If (Post.UpdatedByUser > 0) Then
					' if the person who edited the post is a moderator and hide mod edits is enabled, we don't want to show edit details.
					'CP - Impersonate
					Dim objPosterSecurity As New ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)
					If Not (objConfig.HideModEdits And objPosterSecurity.IsForumModerator) Then
						wr.Write("&nbsp;")
						RenderImage(wr, objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension, String.Format(ForumControl.LocalizedText("ModifiedBy") & " {0} {1}", Post.LastModifiedAuthor.SiteAlias, " " & ForumControl.LocalizedText("on") & " " & Post.UpdatedDate.ToString), "")
					End If
				End If

				RenderDivEnd(wr) ' </span> 
			Else
				' link to select (open) this post when in tree view mode    
				If _PostPage = 0 Then
					_url = Utilities.Links.ContainerViewPostLink(TabID, ForumId, Post.PostID)
				Else
					_url = Utilities.Links.ContainerViewPostPagedLink(TabID, ForumId, ThreadId, Post.PostID, _PostPage + 1)
				End If
				Me.RenderLinkButton(wr, _url, strSubject, "Forum_NormalBold")
			End If

			RenderCellEnd(wr) ' </td> 

			If ShowDetails Then
				'CP- Add back in row seperation 
				RenderRowEnd(wr) '</tr>    
				RenderRowBegin(wr) ' <tr>

				RenderCellBegin(wr, buttonCellClass, "", "", "left", "top", "", "") ' <td>
				RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
				RenderRowBegin(wr) ' <tr>

				RenderCellBegin(wr, "", "", "5%", "left", "top", "", "") ' <td>
				' '' display edited tag if post has been modified
				''If (Post.UpdatedByUser > 0) Then
				''	' if the person who edited the post is a moderator and hide mod edits is enabled, we don't want to show edit details.
				''	'CP - Impersonate
				''	Dim objPosterSecurity As New ModuleSecurity(ModuleID, TabID, ForumId, LoggedOnUser.UserID)
				''	If Not (objConfig.HideModEdits And objPosterSecurity.IsForumModerator) Then
				''		wr.Write("&nbsp;")
				''		RenderImage(wr, objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension, String.Format(ForumControl.LocalizedText("ModifiedBy") & " {0} {1}", Post.LastModifiedAuthor.SiteAlias, " " & ForumControl.LocalizedText("on") & " " & Post.UpdatedDate.ToString), "")
				''	End If
				''End If
				RenderCellEnd(wr) ' </td> 

				' (in flatview or selected, display commands on right)
				RenderCellBegin(wr, "", "", "95%", "right", "middle", "", "") ' <td>
				Me.RenderCommands(wr, Post)
				RenderCellEnd(wr) ' </td> 

				RenderRowEnd(wr) '</tr>    
				RenderTableEnd(wr) ' </table> 
				RenderCellEnd(wr) ' </td> 
				RenderRowEnd(wr) '</tr>    
				RenderTableEnd(wr) ' </table> 
				RenderCellEnd(wr) ' </td> 
			Else
				' (we are in treeview and its not selected, display created date on right)
				RenderCellBegin(wr, detailCellClass, "", "30%", "right", "", "", "") ' <td>
				RenderDivBegin(wr, "spCreatedDate", "Forum_HeaderText") ' <div>
				wr.Write(strCreatedDate.ToString)
				RenderDivEnd(wr) ' </div>
				RenderCellEnd(wr) ' </td> 
			End If

			RenderRowEnd(wr) '</tr>    

			' Body and detail if in flatview (body, signature, attachement...)
			If ShowDetails Then
				RenderRowBegin(wr) ' <tr>

				' test
				Dim postBodyClass As String = String.Empty
				If PostCountIsEven Then
					postBodyClass = "Forum_PostBody"
				Else
					postBodyClass = "Forum_PostBody_Alt"
				End If

				RenderCellBegin(wr, postBodyClass, "100%", "80%", "left", "top", "", "") ' <td>
				Me.RenderPostBody(wr, Post, PostCountIsEven)
				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>
			End If

			RenderTableEnd(wr) ' </table> 
		End Sub

		''' <summary>
		''' Renders the body of a post including signature and attachments
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Post"></param>
		''' <param name="PostCountIsEven"></param>
		''' <remarks></remarks>
		Private Sub RenderPostBody(ByVal wr As HtmlTextWriter, ByVal Post As PostInfo, ByVal PostCountIsEven As Boolean)
			Dim author As ForumUserInfo = Post.Author
			Dim cleanBody As String = String.Empty
			Dim cleanSignature As String = String.Empty
			Dim attachmentClass As String = String.Empty
			Dim bodyForumText As Utilities.PostContent

			If Post.ParseInfo = PostParserInfo.None Or Post.ParseInfo = PostParserInfo.File Then
				'Nothing to Parse or just an Attachment not inline
				bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(Post.Body), objConfig)
			Else
				If Post.ParseInfo < PostParserInfo.Inline Then
					'Something to parse, but not any inline instances
					bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(Post.Body), objConfig, Post.ParseInfo)
				Else
					'At lease Inline to Parse
					If CurrentForumUser.UserID > 0 Then
						bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(Post.Body), objConfig, Post.ParseInfo, Post.Attachments, True)
					Else
						bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(Post.Body), objConfig, Post.ParseInfo, Post.Attachments, False)
					End If
				End If
			End If

			'We will NOT support emoticons or BBCode (quotes/code) in Signatures
			Dim Signature As Utilities.PostContent = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(author.Signature), objConfig)

			If ForumControl.objConfig.EnableBadWordFilter Then
				cleanBody = Utilities.ForumUtils.FormatProhibitedWord(bodyForumText.ProcessHtml(), Post.CreatedDate, PortalID)
				cleanSignature = Utilities.ForumUtils.FormatProhibitedWord(Signature.ProcessHtml(), Post.CreatedDate, PortalID)
			Else
				cleanBody = bodyForumText.ProcessHtml()
				cleanSignature = Signature.ProcessHtml()
			End If

			If PostCountIsEven Then
				attachmentClass = "Forum_Attachments"
			Else
				attachmentClass = "Forum_Attachments_Alt"
			End If

			RenderTableBegin(wr, "tblPostBody" & Post.PostID.ToString, "", "100%", "100%", "0", "0", "left", "", "0") ' should be 0, contains all post body elements already taking max height
			' row for post body
			RenderRowBegin(wr) '<Tr>
			' cell for post body, set cell attributes           
			RenderCellBegin(wr, "", "", "100%", "left", "top", "", "") ' <td>

			RenderDivBegin(wr, "spBody", "Forum_Normal")	' <span>
			wr.Write(cleanBody)
			RenderDivEnd(wr) ' </span>

			If objConfig.EnableUserSignatures Then
				' insert signature if exists
				If Len(author.Signature) > 0 Then
					RenderDivBegin(wr, "", "Forum_Normal")
					wr.RenderBeginTag(HtmlTextWriterTag.Hr)	' <hr>
					wr.RenderEndTag() ' </hr>
					If objConfig.EnableHTMLSignatures Then
						wr.Write(cleanSignature)
					Else
						wr.Write(cleanSignature)
					End If
					RenderDivEnd(wr) ' </span>
				End If
			End If

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr> done with post body

			' Report abuse
			RenderRowBegin(wr) '<tr> 
			'test bodycell
			RenderCellBegin(wr, "", "1px", "100%", "right", "", "", "")	' <td>

			If objConfig.EnablePostAbuse Then
				_url = Utilities.Links.ReportToModsLink(TabID, ModuleID, Post.PostID)

				' create table to hold link and image
				RenderTableBegin(wr, "", "", "", "", "0", "0", "", "middle", "0") ' <table>
				RenderRowBegin(wr) ' <tr>

				If Post.PostReported > 0 Then
					RenderCellBegin(wr, "", "", "", "right", "middle", "", "") ' <td>
					' make a link to take users to see whom reported this post and why
					RenderImage(wr, objConfig.GetThemeImageURL("s_postabuse.") & objConfig.ImageExtension, Post.PostReported.ToString & " " & Localization.GetString("AbuseReports", ForumControl.objConfig.SharedResourceFile), "")
					wr.Write("&nbsp;")
					RenderCellEnd(wr) ' </td>
				End If

				If CurrentForumUser.UserID > 0 Then
					RenderCellBegin(wr, "Forum_ReplyCell", "", "", "right", "middle", "", "") ' <td>
					' Warn link
					RenderLinkButton(wr, _url, ForumControl.LocalizedText("ReportAbuse"), "Forum_Link")
					RenderCellEnd(wr) ' </td>
				End If

				RenderRowEnd(wr) ' </tr> 
				RenderTableEnd(wr) ' </table>
			Else
				wr.Write("&nbsp;")
			End If

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr> 

			''CP-ADD - New per post rating (preparing UI) - Not Implemented
			'RenderRowBegin(wr) '<tr> 
			'RenderCellBegin(wr, postBodyClass, "100%", "100%", "", "", "", "")
			'RenderPerPostRating(wr)
			'RenderCellEnd(wr) ' </td>
			'RenderRowEnd(wr) ' </tr> done with perPostRating

			' done with per post rating, move to attachments (old version)
			If Post.FileAttachmentURL.Length > 0 Then
				RenderRowBegin(wr) '<tr> 
				RenderCellBegin(wr, attachmentClass, "1px", "100%", "left", "middle", "", "") ' <td>

				' create table to hold link and image
				RenderTableBegin(wr, "", "", "", "", "0", "0", "", "middle", "0") ' <table>
				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>

				Dim strLink As String
				Dim strFileName As String
				If (objConfig.AnonDownloads = False) Then
					If HttpContext.Current.Request.IsAuthenticated = False Then
						strFileName = Localization.GetString("NoAnonDownloads", ForumControl.objConfig.SharedResourceFile)

						RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
						RenderImage(wr, objConfig.GetThemeImageURL("s_attachment.") & objConfig.ImageExtension, "", "")
						RenderCellEnd(wr) ' </td>

						RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
						wr.Write("&nbsp;")
						wr.Write("<span class=Forum_NormalBold>" & strFileName & "</span>")
						RenderCellEnd(wr) ' </td>
					Else
						strLink = FormatURL(Post.FileAttachmentURL, False)
						strFileName = Post.FileAttachmentName

						RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
						RenderImageButton(wr, strLink, objConfig.GetThemeImageURL("s_attachment.") & objConfig.ImageExtension, "", "", True)
						RenderCellEnd(wr) ' </td>

						RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
						wr.Write("&nbsp;")
						RenderLinkButton(wr, strLink, strFileName, "Forum_Link", "", True, False)
						RenderCellEnd(wr) ' </td>
					End If
				Else
					strLink = FormatURL(Post.FileAttachmentURL, False)
					strFileName = Post.FileAttachmentName

					RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
					RenderImageButton(wr, strLink, objConfig.GetThemeImageURL("s_attachment.") & objConfig.ImageExtension, "", "", True)
					RenderCellEnd(wr) ' </td>

					RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
					wr.Write("&nbsp;")
					RenderLinkButton(wr, strLink, strFileName, "Forum_Link", "", True, False)
					RenderCellEnd(wr) ' </td>
				End If

				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr> 
				RenderTableEnd(wr) ' </table>

				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr> 
			End If

			'New Attachments type
			Select Case Post.ParseInfo

				Case 4, 5, 6, 7, 15

					RenderRowBegin(wr) '<tr> 
					RenderCellBegin(wr, attachmentClass, "1px", "100%", "left", "middle", "", "") ' <td>

					' create table to hold link and image
					RenderTableBegin(wr, "", "", "", "", "0", "0", "", "middle", "0") ' <table>

					For Each objFile As AttachmentInfo In Post.Attachments
						'Here we only handle attachments not inline type
						If objFile.Inline = False Then

							RenderRowBegin(wr) ' <tr>
							RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>

							Dim strlink As String
							Dim strFileName As String

							If (objConfig.AnonDownloads = False) Then
								If HttpContext.Current.Request.IsAuthenticated = False Then
									'AnonDownloads are Disabled
									strFileName = Localization.GetString("NoAnonDownloads", ForumControl.objConfig.SharedResourceFile)

									RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
									RenderImage(wr, objConfig.GetThemeImageURL("s_attachment.") & objConfig.ImageExtension, "", "")
									RenderCellEnd(wr) ' </td>

									RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
									wr.Write("&nbsp;")
									wr.Write("<span class=Forum_NormalBold>" & strFileName & "</span>")
									RenderCellEnd(wr) ' </td>

									'We only want to display this information once..
									RenderCellEnd(wr) ' </td>
									RenderRowEnd(wr) ' </tr>
									Exit For

								Else
									'User is Authenticated
									strlink = FormatURL("FileID=" & objFile.FileID, False, True)
									strFileName = objFile.LocalFileName

									RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
									RenderImageButton(wr, objFile.FileName, objConfig.GetThemeImageURL("s_attachment.") & objConfig.ImageExtension, "", "", True)
									RenderCellEnd(wr) ' </td>

									RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
									wr.Write("&nbsp;")
									RenderLinkButton(wr, strlink, strFileName, "Forum_Link", "", True, False)
									RenderCellEnd(wr) ' </td>
								End If

							Else
								'AnonDownloads are Enabled
								strlink = FormatURL("FileID=" & objFile.FileID, False, True)
								strFileName = objFile.LocalFileName

								RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
								RenderImageButton(wr, strlink, objConfig.GetThemeImageURL("s_attachment.") & objConfig.ImageExtension, "", "", True)
								RenderCellEnd(wr) ' </td>

								RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>
								wr.Write("&nbsp;")
								RenderLinkButton(wr, strlink, strFileName, "Forum_Link", "", True, False)
								RenderCellEnd(wr) ' </td>
							End If

							RenderCellEnd(wr) ' </td>
							RenderRowEnd(wr) ' </tr> 
						End If
					Next

					RenderTableEnd(wr) ' </table>
					RenderCellEnd(wr) ' </td>
					RenderRowEnd(wr) ' </tr> 

			End Select
			RenderTableEnd(wr) ' </table> 
		End Sub

		''' <summary>
		''' Formats the URL used for attachments
		''' </summary>
		''' <param name="Link"></param>
		''' <param name="TrackClicks"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function FormatURL(ByVal Link As String, ByVal TrackClicks As Boolean) As String
			Return Common.Globals.LinkClick(Link, TabID, ModuleID, TrackClicks)
		End Function

		''' <summary>
		''' Formats the URL used for attachments (new version)
		''' </summary>
		''' <param name="Link"></param>
		''' <param name="TrackClicks"></param>
		''' <returns></returns>
		''' <remarks>[skeel] 8/1/2009 created</remarks>
		Function FormatURL(ByVal Link As String, ByVal TrackClicks As Boolean, ByVal ForceDownload As Boolean) As String
			Return Common.Globals.LinkClick(Link, TabID, ModuleID, TrackClicks, ForceDownload)
		End Function

		''' <summary>
		''' This allows for spacing between posts
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderSpacerRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr> 
			RenderCellBegin(wr, "Forum_SpacerRow", "", "", "", "", "", "")  ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
			RenderCellEnd(wr)

			RenderCellBegin(wr, "Forum_SpacerRow", "", "", "", "", "", "")  ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
			RenderCellEnd(wr) '</td>
			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' Footer w/ paging (second to last row)
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderFooter(ByVal wr As HtmlTextWriter)
			Dim pageCount As Integer = CInt(Math.Floor((ThreadInfo.Replies) / CurrentForumUser.PostsPerPage)) + 1
			Dim pageCountInfo As New StringBuilder

			pageCountInfo.Append(ForumControl.LocalizedText("PageCountInfo"))
			pageCountInfo.Replace("[PageNumber]", (_PostPage + 1).ToString)
			pageCountInfo.Replace("[PageCount]", pageCount.ToString)

			' Start the footer row
			RenderRowBegin(wr)
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>

			RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td> 
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapLeft", "") ' <td><img/></td>

			RenderCellBegin(wr, "Forum_Footer", "", "", "", "", "", "")	' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>

			RenderCellBegin(wr, "", "", "20%", "", "", "", "")  ' <td>
			RenderDivBegin(wr, "spPageCounting", "Forum_FooterText") ' <span>
			wr.Write("&nbsp;" & pageCountInfo.ToString)
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td> 

			RenderCellBegin(wr, "", "", "80%", "right", "", "", "")   ' <td> 
			If (pageCount > 1) Then
				RenderDivBegin(wr, "", "Forum_FooterText") ' <span>
				RenderPostPaging(wr, pageCount)
				wr.Write("&nbsp;")
				RenderDivEnd(wr) ' </span>
			End If

			' Close paging
			RenderCellEnd(wr) ' </td>   
			RenderRowEnd(wr) ' </tr>   
			RenderTableEnd(wr) ' </table>  
			RenderCellEnd(wr) ' </td>   
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapRight", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>  
		End Sub

		''' <summary>
		''' bottom Breadcrumb and ddl's, along w/ subscribe chkbx (last row)
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderBottomBreadCrumbRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") '<td>
			RenderCellEnd(wr) ' </td> 
			RenderCapCell(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
			RenderRowEnd(wr) ' </tr>

			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") '<td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) '<tr>

			RenderCellBegin(wr, "", "", "50%", "left", "middle", "", "") ' <td> '
			' new thread button
			'Remove LoggedOnUserID limitation if wishing to implement Anonymous Posting
			If (CurrentForumUser.UserID > 0) And (Not ForumId = -1) Then
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

				If Not ParentForum.PublicPosting Then
					If objSecurity.IsAllowedToStartRestrictedThread Then

						RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
						RenderRowBegin(wr) '<tr>
						_url = Utilities.Links.NewThreadLink(TabID, ForumId, ModuleID)
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

						If CurrentForumUser.IsBanned Then
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
						Else
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
						End If

						RenderCellEnd(wr) ' </Td>

						If CurrentForumUser.IsBanned Or (Not objSecurity.IsAllowedToPostRestrictedReply) Or (ThreadInfo.IsClosed) Then
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link", False)
							RenderCellEnd(wr) ' </Td>
						Else
							_url = Utilities.Links.NewPostLink(TabID, ForumId, ThreadInfo.ThreadID, "reply", ModuleID)
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link")
							RenderCellEnd(wr) ' </Td>
						End If

						'[skeel] moved delete thread here
						If CurrentForumUser.UserID > 0 AndAlso (objSecurity.IsForumModerator) Then
							_url = Utilities.Links.ThreadDeleteLink(TabID, ModuleID, ForumId, ThreadId, False)
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("DeleteThread"), "Forum_Link")
							RenderCellEnd(wr) ' </Td>
						End If

						RenderRowEnd(wr) ' </tr>
						RenderTableEnd(wr) ' </table>
					ElseIf objSecurity.IsAllowedToPostRestrictedReply Then
						RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
						RenderRowBegin(wr) '<tr>

						If CurrentForumUser.IsBanned Or ThreadInfo.IsClosed Then
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link", False)
							RenderCellEnd(wr) ' </Td>
						Else
							_url = Utilities.Links.NewPostLink(TabID, ForumId, ThreadInfo.ThreadID, "reply", ModuleID)
							RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
							wr.Write("&nbsp;")
							RenderCellEnd(wr) ' </Td>
							RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
							RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link")
							RenderCellEnd(wr) ' </Td>
						End If

						RenderRowEnd(wr) ' </tr>
						RenderTableEnd(wr) ' </table>
					Else
						wr.Write("&nbsp;")
					End If
				Else
					RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
					RenderRowBegin(wr) '<tr>
					_url = Utilities.Links.NewThreadLink(TabID, ForumId, ModuleID)
					RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
					If CurrentForumUser.IsBanned Then
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
					Else
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
					End If
					RenderCellEnd(wr) ' </Td>

					If CurrentForumUser.IsBanned Or ThreadInfo.IsClosed Then
						RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
						wr.Write("&nbsp;")
						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link", False)
						RenderCellEnd(wr) ' </Td>
					Else
						_url = Utilities.Links.NewPostLink(TabID, ForumId, ThreadInfo.ThreadID, "reply", ModuleID)
						RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
						wr.Write("&nbsp;")
						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link")
						RenderCellEnd(wr) ' </Td>
					End If

					'[skeel] moved delete thread here
					If CurrentForumUser.UserID > 0 AndAlso (objSecurity.IsForumModerator) Then
						_url = Utilities.Links.ThreadDeleteLink(TabID, ModuleID, ForumId, ThreadId, False)
						RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
						wr.Write("&nbsp;")
						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("DeleteThread"), "Forum_Link")
						RenderCellEnd(wr) ' </Td>
					End If

					RenderRowEnd(wr) ' </tr>
					RenderTableEnd(wr) ' </table>
				End If
			End If

			RenderCellEnd(wr) ' </Td>

			RenderCellBegin(wr, "", "", "50%", "right", "", "", "") ' <td> ' 
			RenderTableBegin(wr, "", "", "100%", "", "0", "0", "", "", "0") '<Table>            
			RenderRowBegin(wr) '<tr>

			Dim PreviousEnabled As Boolean = False
			Dim EnabledText As String = "Disabled"
			If Not ThreadInfo.PreviousThreadID = 0 Then
				If Not ThreadInfo.IsPinned Then
					PreviousEnabled = True
					EnabledText = "Previous"
				End If
			End If

			If PreviousEnabled Then
				RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "", "", "")  ' <td> ' 
			Else
				RenderCellBegin(wr, "Forum_NavBarButtonDisabled", "", "", "", "", "", "")	' <td> ' 
			End If

			RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
			RenderRowBegin(wr) '<tr>

			_url = Utilities.Links.ContainerViewThreadLink(TabID, ForumId, ThreadInfo.PreviousThreadID)

			RenderCellBegin(wr, "", "", "", "", "", "", "")  ' <td> ' 

			If PreviousEnabled Then
				RenderLinkButton(wr, _url, ForumControl.LocalizedText("Previous"), "Forum_Link")
			Else
				RenderDivBegin(wr, "", "Forum_NormalBold")
				wr.Write(ForumControl.LocalizedText("Previous"))
				RenderDivEnd(wr)
			End If
			RenderCellEnd(wr) ' </td>

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "", "", "", "", "", "", "")  ' <td> 
			wr.Write("&nbsp;")
			RenderCellEnd(wr) ' </td>

			'next button
			Dim NextEnabled As Boolean = False
			Dim NextText As String = "Disabled"
			If Not ThreadInfo.NextThreadID = 0 Then
				If Not ThreadInfo.IsPinned Then
					NextEnabled = True
					NextText = "Next"
				End If
			End If

			If NextEnabled Then
				RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "", "", "")  ' <td> 
			Else
				RenderCellBegin(wr, "Forum_NavBarButtonDisabled", "", "", "", "", "", "")	' <td> 
			End If

			RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "", "", "", "", "")  ' <td> 

			If NextEnabled Then
				_url = Utilities.Links.ContainerViewThreadLink(TabID, ForumId, ThreadInfo.NextThreadID)
				RenderLinkButton(wr, _url, ForumControl.LocalizedText("Next"), "Forum_Link", NextEnabled)
			Else
				RenderDivBegin(wr, "", "Forum_NormalBold")
				wr.Write(ForumControl.LocalizedText("Next"))
				RenderDivEnd(wr)
			End If
			RenderCellEnd(wr) ' </td>   
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>

			' enclosing table for prev/next
			wr.RenderEndTag() ' </Td>
			wr.RenderEndTag() ' </Tr>
			RenderTableEnd(wr) ' </table> 

			wr.RenderEndTag() ' </Td>
			wr.RenderEndTag() ' </Tr>

			RenderRowBegin(wr) '<Tr>

			RenderCellBegin(wr, "", "", "", "left", "", "3", "") ' <td> 
			Dim ChildGroupView As Boolean = False
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				ChildGroupView = True
			End If
			wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Posts, ThreadInfo, objConfig, ChildGroupView))
			RenderCellEnd(wr) ' </td> 
			RenderRowEnd(wr) ' </tr> 

			'Treeview ViewOrder drop down lists
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "100%", "right", "", "2", "") ' <td> 

			If PostCollection.Count > 0 Then
				'If objConfig.EnableTreeView Then
				'	ddlForumView.RenderControl(wr)
				'End If

				wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("spacer.gif"))
				wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
				wr.RenderEndTag() ' </Img>
				ddlViewDescending.RenderControl(wr)
			End If

			RenderCellEnd(wr) ' </td> 
			RenderRowEnd(wr) ' </tr>   

			'[Skeel] Notifications row
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "", "right", "", "2", "")	' <td> 
			wr.Write("<br />")

			' Display tracking option if user is authenticated and post count > 0 and user not track parent forum (make sure tracking is enabled)
			'CP - Seperating so we can show user they are tracking at forum level if need be
			If (PostCollection.Count > 0) AndAlso (CurrentForumUser.UserID > 0) And (objConfig.MailNotification) Then
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

				If objSecurity.IsForumAdmin Then
					cmdThreadSubscribers.RenderControl(wr)
					wr.Write("<br />")
				End If

				If TrackedForum Then
				ElseIf TrackedModule Then
				Else
					chkEmail.RenderControl(wr)
				End If
			End If

			RenderCellEnd(wr) ' </td> 
			RenderRowEnd(wr) ' </tr>

			'Close the table
			RenderTableEnd(wr) ' </table> 

			RenderCellEnd(wr) ' </td> 
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderRowEnd(wr) ' </tr>   
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderQuickReply(ByVal wr As HtmlTextWriter)
			If objConfig.EnableQuickReply Then
				If (CurrentForumUser.UserID > 0) And (Not ForumId = -1) Then
					If Not ParentForum.PublicPosting Then
						If CurrentForumUser.IsBanned = False And ThreadInfo.IsClosed = False Then
							Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)
							If objSecurity.IsAllowedToPostRestrictedReply Then
								QuickReply(wr)
							End If
						End If
					Else
						If CurrentForumUser.IsBanned = False And ThreadInfo.IsClosed = False Then
							Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)
							QuickReply(wr)
						End If
					End If
				End If
			End If
		End Sub

		''' <summary>
		''' Renders available post reply/quote/moderate, etc.  buttons
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Post"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderCommands(ByVal wr As HtmlTextWriter, ByVal Post As PostInfo)
			Dim user As ForumUserInfo = CurrentForumUser
			Dim author As ForumUserInfo = Post.Author
			Dim HostThread As ThreadInfo = Post.ParentThread
			Dim HostForum As ForumInfo = HostThread.HostForum

			' Render reply/mod buttons if necessary
			' First see if the user has the ability to post
			' remove logged on limitation if wishing to implement Anonymous posting
			If CurrentForumUser.UserID > 0 Then
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, CurrentForumUser.UserID)

				If (Not HostForum.PublicPosting And objSecurity.IsAllowedToPostRestrictedReply) Or (HostForum.PublicPosting = True) Then
					' move link (logged user is admin and this is the first post in the thread)
					'start table for mod/reply buttons
					RenderTableBegin(wr, "tblCommand_" & Post.PostID.ToString, "", "", "", "4", "0", "", "", "0")	  ' <table>
					RenderRowBegin(wr)

					'Never Remove LoggedOnUserID limitation EVEN if wishing to implement Anonymous Posting - ParentPostID is so we know this is the first post in a thread to move it
					If CurrentForumUser.UserID > 0 And (objSecurity.IsForumModerator) AndAlso (Post.ParentPostID = 0) Then
						_url = Utilities.Links.ThreadMoveLink(TabID, ModuleID, ForumId, ThreadId)

						RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("Move"), "Forum_Link")
						RenderCellEnd(wr)
					ElseIf CurrentForumUser.UserID > 0 And (objSecurity.IsForumModerator) Then
						' Split thread
						_url = Utilities.Links.ThreadSplitLink(TabID, ModuleID, ForumId, Post.PostID)

						RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("Split"), "Forum_Link")
						RenderCellEnd(wr)
					End If

					'Never Remove LoggedOnUserID limitation EVEN if wishing to implement Anonymous Posting
					If CurrentForumUser.UserID > 0 AndAlso (objSecurity.IsForumModerator) Then
						_url = Utilities.Links.PostDeleteLink(TabID, ModuleID, ForumId, Post.PostID, False)

						RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("Delete"), "Forum_Link")
						RenderCellEnd(wr)
					End If

					'Never Remove LoggedOnUserID limitation EVEN if wishing to implement Anonymous Posting - Anonymous cannot edit post
					If CurrentForumUser.UserID > 0 AndAlso (objSecurity.IsForumModerator) Then
						_url = Utilities.Links.NewPostLink(TabID, ForumId, Post.PostID, "edit", ModuleID)

						RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")
						RenderLinkButton(wr, _url, ForumControl.LocalizedText("Edit"), "Forum_Link")
						RenderCellEnd(wr)
						'don't allow non mod, forum admin or anything other than a moderator to edit a closed forum post (if the forum is not moderated, or the user is trusted)
					ElseIf CurrentForumUser.UserID > 0 And (Post.ParentThread.HostForum.IsActive) And ((CurrentForumUser.UserID = Post.Author.UserID) AndAlso (Post.ParentThread.HostForum.IsModerated = False Or author.IsTrusted Or objSecurity.IsUnmoderated)) Then

						'[skeel] check for PostEditWindow
						If objConfig.PostEditWindow = 0 Then
							_url = Utilities.Links.NewPostLink(TabID, ForumId, Post.PostID, "edit", ModuleID)
							RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")

							If CurrentForumUser.IsBanned Then
								RenderLinkButton(wr, _url, ForumControl.LocalizedText("Edit"), "Forum_Link", False)
							Else
								RenderLinkButton(wr, _url, ForumControl.LocalizedText("Edit"), "Forum_Link")
							End If

							RenderCellEnd(wr)
						Else
							If Post.CreatedDate.AddMinutes(CDbl(objConfig.PostEditWindow)) > Now Then
								_url = Utilities.Links.NewPostLink(TabID, ForumId, Post.PostID, "edit", ModuleID)
								RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")

								If CurrentForumUser.IsBanned Then
									RenderLinkButton(wr, _url, ForumControl.LocalizedText("Edit"), "Forum_Link", False)
								Else
									RenderLinkButton(wr, _url, ForumControl.LocalizedText("Edit"), "Forum_Link")
								End If

								RenderCellEnd(wr)
							End If
						End If
					End If

					'First check if the thread is opened, if not then handle for single situation
					If CurrentForumUser.UserID > 0 AndAlso (Not Post.ParentThread.IsClosed) And (Post.ParentThread.HostForum.IsActive) Then
						If Not Post.ParentThread.HostForum.PublicPosting Then
							' see if user can reply
							If objSecurity.IsAllowedToPostRestrictedReply Then
								_url = Utilities.Links.NewPostLink(TabID, ForumId, Post.PostID, "quote", ModuleID)
								' Quote link
								RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")
								If CurrentForumUser.IsBanned Then
									RenderLinkButton(wr, _url, ForumControl.LocalizedText("Quote"), "Forum_Link", False)
								Else
									RenderLinkButton(wr, _url, ForumControl.LocalizedText("Quote"), "Forum_Link")
								End If
								RenderCellEnd(wr)

								_url = Utilities.Links.NewPostLink(TabID, ForumId, Post.PostID, "reply", ModuleID)

								' Reply link                    
								RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")
								If CurrentForumUser.IsBanned Then
									RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link", False)
								Else
									RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link")
								End If
								RenderCellEnd(wr)
							End If
						Else
							_url = Utilities.Links.NewPostLink(TabID, ForumId, Post.PostID, "quote", ModuleID)
							' Quote link
							RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")
							If CurrentForumUser.IsBanned Then
								RenderLinkButton(wr, _url, ForumControl.LocalizedText("Quote"), "Forum_Link", False)
							Else
								RenderLinkButton(wr, _url, ForumControl.LocalizedText("Quote"), "Forum_Link")
							End If
							RenderCellEnd(wr)

							_url = Utilities.Links.NewPostLink(TabID, ForumId, Post.PostID, "reply", ModuleID)

							' Reply link                    
							RenderCellBegin(wr, "Forum_ReplyCell", "", "", "", "", "", "")
							If CurrentForumUser.IsBanned Then
								RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link", False)
							Else
								RenderLinkButton(wr, _url, ForumControl.LocalizedText("Reply"), "Forum_Link")
							End If
							RenderCellEnd(wr)
						End If
					End If

					RenderRowEnd(wr) ' </tr>
					RenderTableEnd(wr) ' </table>
				Else
					' User cannot post, which means no moderation either
					RenderTableBegin(wr, "tblCommand_" & Post.PostID.ToString, "", "", "", "4", "0", "", "", "0")	  ' <table>
					RenderRowBegin(wr)
					RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "left")
					RenderRowEnd(wr) ' </tr>
					RenderTableEnd(wr) ' </table>
				End If
			Else
				' User cannot post, which means no moderation either
				RenderTableBegin(wr, "tblCommand_" & Post.PostID.ToString, "", "", "", "4", "0", "", "", "0")	  ' <table>
				RenderRowBegin(wr)
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "left")
				RenderRowEnd(wr) ' </tr>
				RenderTableEnd(wr) ' </table>
			End If
		End Sub

		''' <summary>
		''' Determins if post is even or odd numbered row
		''' </summary>
		''' <param name="Count"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Private Function ThreadIsEven(ByVal Count As Integer) As Boolean
			If Count Mod 2 = 0 Then
				'even
				Return True
			Else
				'odd
				Return False
			End If
		End Function

		''' <summary>
		''' Just relevant to paging
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="PageCount"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderPostPaging(ByVal wr As HtmlTextWriter, ByVal PageCount As Integer)
			' First, previous, next, last thread hyperlinks
			Dim backwards As Boolean
			Dim forwards As Boolean

			If PostPage <> 0 Then
				backwards = True
			End If

			If PostPage <> PageCount - 1 Then
				forwards = True
			End If

			If (backwards) Then
				' < Previous 
				_url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumId, ThreadId, PostPage)
				wr.AddAttribute(HtmlTextWriterAttribute.Href, _url)
				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_FooterText")
				wr.RenderBeginTag(HtmlTextWriterTag.A) '<a>
				wr.Write(ForumControl.LocalizedText("Previous"))
				wr.RenderEndTag() ' </A>

				wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("spacer.gif"))
				wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
				wr.RenderEndTag() ' </Img>
			End If

			' If thread spans several pages, then display text like (Page 1, 2, 3, ..., 5)
			Dim displayPage As Integer = PostPage + 1
			Dim startCap As Integer = Math.Max(4, displayPage - 1)
			Dim endCap As Integer = Math.Min(PageCount - 1, displayPage + 1)
			Dim sepStart As Boolean = False
			Dim sepEnd As Boolean = False
			Dim iPost As Integer

			For iPost = 1 To PageCount
				_url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumId, ThreadId, iPost)

				If iPost <= 3 Then
					If iPost <> displayPage Then
						wr.AddAttribute(HtmlTextWriterAttribute.Href, _url)
					End If
					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_FooterText")
					wr.RenderBeginTag(HtmlTextWriterTag.A) '<a>
					wr.Write(iPost)
					wr.RenderEndTag() ' </A>

					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("spacer.gif"))
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
					wr.RenderEndTag() ' </Img>
				End If

				If (iPost > 3 AndAlso iPost < startCap) AndAlso (Not sepStart) Then
					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Link")
					wr.AddAttribute(HtmlTextWriterAttribute.Id, "spStartCap")
					wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>
					wr.Write("...")
					wr.RenderEndTag() ' </Span>
					sepStart = True
				End If

				If iPost >= startCap AndAlso iPost <= endCap Then
					If iPost <> displayPage Then
						'wr.AddAttribute(HtmlTextWriterAttribute.Href, GetURL(Document, Page, String.Format("threadpage={0}", iPost), "postid=&action="))
						wr.AddAttribute(HtmlTextWriterAttribute.Href, _url)
					End If
					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_FooterText")
					wr.RenderBeginTag(HtmlTextWriterTag.A)
					wr.Write(iPost)
					wr.RenderEndTag() ' A

					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("spacer.gif"))
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
					wr.RenderEndTag() ' </Img>
				End If

				If (iPost > 3) AndAlso (iPost > endCap AndAlso iPost < PageCount) AndAlso (Not sepEnd) Then
					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_FooterText")
					wr.AddAttribute(HtmlTextWriterAttribute.Id, "spEndCap")
					wr.RenderBeginTag(HtmlTextWriterTag.Span) '<span>
					wr.Write("...")
					wr.RenderEndTag() ' </Span>
					sepEnd = True
				End If

				If iPost = PageCount AndAlso iPost > 3 Then
					If iPost <> displayPage Then
						wr.AddAttribute(HtmlTextWriterAttribute.Href, _url)
					End If
					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_FooterText")
					wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
					wr.Write(iPost)
					wr.RenderEndTag() ' </A>

					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("spacer.gif"))
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
					wr.RenderEndTag() ' </Img>
				End If
			Next

			If (forwards) Then
				' Next >
				_url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumId, ThreadId, PostPage + 2)
				wr.AddAttribute(HtmlTextWriterAttribute.Href, _url)
				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_FooterText")
				wr.RenderBeginTag(HtmlTextWriterTag.A) '<a>
				wr.Write(ForumControl.LocalizedText("Next"))
				wr.RenderEndTag() ' </A>

				wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("spacer.gif"))
				wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
				wr.RenderEndTag() ' </Img>
			End If
		End Sub

		''' <summary>
		''' CP-ADD - Not implemented yet
		''' </summary>
		''' <param name="wr">HtmlTextWriter</param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderPerPostRating(ByVal wr As HtmlTextWriter)
		End Sub

		''' <summary>
		''' Builds a bookmark for RenderPost, used to navigate directly to a specific post
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="BookMark"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	7/11/2008	Created
		''' </history>
		Private Sub RenderPostBookmark(ByVal wr As HtmlTextWriter, ByVal BookMark As String)
			wr.Write("<a name=""" & BookMark & """></a>")
		End Sub

		''' <summary>
		''' Renders a textbox on the screen for a quickly reply to threads.
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub QuickReply(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>

			RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td> 
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapLeft", "") ' <td><img/></td>

			RenderCellBegin(wr, "Forum_Header", "", "", "", "", "", "")	' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>

			RenderCellBegin(wr, "", "", "100%", "", "", "", "")  ' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
			wr.Write("&nbsp;" & "Quick Reply")
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td> 
			RenderRowEnd(wr) ' </tr>   
			RenderTableEnd(wr) ' </table>  
			RenderCellEnd(wr) ' </td>   
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapRight", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>  

			' Show quick reply textbox row
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>

			RenderCellBegin(wr, "Forum_UCP_HeaderInfo", "", "", "left", "middle", "", "") ' <td> 
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>

			RenderCellBegin(wr, "", "", "125px", "", "top", "", "")	' <td>
			RenderDivBegin(wr, "", "Forum_NormalBold") ' <span>
			wr.Write("&nbsp;" & "Body")
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td> 

			RenderCellBegin(wr, "", "", "", "left", "", "", "")	' <td>
			txtQuickReply.RenderControl(wr)
			RenderCellEnd(wr) ' </td> 

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>  

			' Submit Row
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>

			RenderCellBegin(wr, "", "", "", "center", "middle", "", "")	' <td> 
			RenderTableBegin(wr, "", "", "", "125px", "0", "0", "", "", "0")	' <table>
			RenderRowBegin(wr) ' <tr>


			RenderCellBegin(wr, "Forum_NavBarButton", "", "125px", "", "", "", "")	' <td>
			cmdSubmit.RenderControl(wr)
			RenderCellEnd(wr) ' </td> 
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td> 
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>  
		End Sub

#End Region

	End Class

End Namespace
