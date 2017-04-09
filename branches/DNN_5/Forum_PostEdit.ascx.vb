Option Strict On
Option Explicit On
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
Imports DotNetNuke.Forum.Library.Data
Imports DotNetNuke.Modules.Forum.Utilities

Namespace DotNetNuke.Modules.Forum

    ''' <summary>
    ''' This is where all posts are added and/or edited from.  It also fires off 
    ''' email notification and factors in moderation.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Partial Class PostEdit
        Inherits ForumModuleBase
        Implements DotNetNuke.Entities.Modules.IActionable

#Region "Private Members"

        Const COLUMN_DELETE As Integer = 0
        Const COLUMN_MOVE_DOWN As Integer = 1
        Const COLUMN_MOVE_UP As Integer = 2
        Const COLUMN_ANSWER As Integer = 3

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property PostID As Integer
            Get
                If Request.QueryString("postid") IsNot Nothing Then
                    Return Int32.Parse(Request.QueryString("postid"))
                Else
                    Return -1
                End If
            End Get
        End Property

        ''' <summary>
        ''' The ThreadInfo object of the post being rendered.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property objThread() As ThreadInfo
            Get
                If PostID > 0 Then
                    Dim cntPost As New PostController()
                    Dim objParentPost As New PostInfo

                    objParentPost = cntPost.GetPostInfo(PostID, PortalId)
                    Return objParentPost.ParentThread
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property objForum() As ForumInfo
            Get
                If PostID > 0 Then
                    Return objThread.ContainingForum
                Else
                    Dim cntForum As New ForumController
                    Return cntForum.GetForumItemCache(ForumID)
                End If
            End Get
        End Property

        Protected teContent As UserControls.TextEditor
        Protected ctlAttachment As DotNetNuke.Modules.Forum.WebControls.AttachmentControl

#End Region

#Region "Optional Interfaces"

        ''' <summary>
        ''' Gets a list of module actions available to the user to provide it to DNN core.
        ''' </summary>
        ''' <value></value>
        ''' <returns>The collection of module actions available to the user</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Return Utilities.ForumUtils.PerUserModuleActions(objConfig, Me)
            End Get
        End Property

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Used to setup the client side reorder datagrid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        ''' 'This call is required by the Web Form Designer.
        Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
            ForumUtils.LoadCssFile(DefaultPage, objConfig)

            If DotNetNuke.Framework.AJAX.IsInstalled Then
                DotNetNuke.Framework.AJAX.RegisterScriptManager()
            End If

            For Each column As DataGridColumn In dgAnswers.Columns
                If column.GetType() Is GetType(DotNetNuke.UI.WebControls.ImageCommandColumn) Then
                    Dim imageColumn As DotNetNuke.UI.WebControls.ImageCommandColumn = CType(column, DotNetNuke.UI.WebControls.ImageCommandColumn)
                    Select Case imageColumn.CommandName
                        Case "Delete"
                            imageColumn.OnClickJS = Localization.GetString("DeleteItem")
                            imageColumn.Text = Localization.GetString("Delete", Me.LocalResourceFile)
                            imageColumn.ImageURL = objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
                        Case "MoveUp"
                            imageColumn.Text = Localization.GetString("MoveUp", Me.LocalResourceFile)
                            imageColumn.ImageURL = objConfig.GetThemeImageURL("s_up.") & objConfig.ImageExtension
                        Case "MoveDown"
                            imageColumn.Text = Localization.GetString("MoveDown", Me.LocalResourceFile)
                            imageColumn.ImageURL = objConfig.GetThemeImageURL("s_down.") & objConfig.ImageExtension
                    End Select
                End If
            Next
        End Sub

        ''' <summary>
        ''' The Page Load request of this control.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If PostID < 1 Then
                    If ForumID > 0 Then
                        If objForum Is Nothing Then
                            HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                        End If
                    Else
                        HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                    End If
                End If

                With Me.cmdCalEndDate
                    .ImageUrl = objConfig.GetThemeImageURL("s_calendar.") & objConfig.ImageExtension
                    .NavigateUrl = CType(Common.Utilities.Calendar.InvokePopupCal(txtEndDate), String)
                    .ToolTip = Localization.GetString("cmdCalEndDate", LocalResourceFile)
                End With

                If Page.IsPostBack = False Then
                    Localization.LocalizeDataGrid(dgAnswers, Me.LocalResourceFile)
                    txtPollID.Text = "-1"
                    Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, objForum.ForumID, UserId)

                    If Request.IsAuthenticated And CurrentForumUser.UserID > 0 Then
                        ' Before anything else, make sure the user is not banned
                        If CurrentForumUser.IsBanned Then
                            HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                        End If
                    Else
                        HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                    End If

                    ' Make sure user is not accessing a private forum via qs to reply/start new thread (and if they are, make sure they have perms)
                    If Not objForum.PublicView Then
                        If (Security.IsAllowedToViewPrivateForum = False) Then
                            HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                        End If
                    End If

                    ' Obtain post action
                    Dim objAction As PostAction

                    If (Not Request.QueryString("action") Is Nothing) Then
                        objAction = CType([Enum].Parse(GetType(PostAction), Request.QueryString("action"), True), PostAction)
                        cmdSubmit.CommandName = objAction.ToString
                    End If

                    Dim objParentPost As New PostInfo
                    Dim cntPost As New PostController()

                    If PostID > 0 Then
                        objParentPost = cntPost.GetPostInfo(PostID, PortalId)
                    End If

                    If objAction <> PostAction.[New] AndAlso objParentPost IsNot Nothing Then
                        If objParentPost.ParentPostID <> 0 Then chkIsPinned.Enabled = False
                        If objParentPost.ParentPostID <> 0 Then chkIsClosed.Enabled = False
                    End If

                    Dim AllowUserEdit As Boolean = False

                    ' Check to see what type of action we are doing here, our only concern is edit
                    Select Case objAction
                        Case PostAction.Edit
                            ' security check
                            If Not objThread.ContainingForum.PublicPosting Then
                                'restricted posting forum
                                If Not (Security.IsAllowedToPostRestrictedReply Or Security.IsAllowedToStartRestrictedThread) Then
                                    HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                                End If
                            End If

                            ' Make sure user IsTrusted too before they can edit (but only if a moderated forum, if its not moderated we don't care)
                            ' First check to see if user is original author 
                            If CurrentForumUser.UserID > 0 And (objParentPost.UserID = CurrentForumUser.UserID) And (objThread.ContainingForum.IsModerated = False Or CurrentForumUser.IsTrusted Or Security.IsUnmoderated) And (objThread.ContainingForum.IsActive) Then
                                AllowUserEdit = True
                            Else
                                ' The user is not the original author
                                ' See if they are admin or moderator - always have to be some type of mod or admin to edit (aka logged in)
                                If CurrentForumUser.UserID > 0 And (Security.IsForumModerator = True) Then
                                    AllowUserEdit = True
                                End If
                            End If
                        Case PostAction.[New]
                            ' security check
                            If Not objForum.PublicPosting Then
                                'restricted posting forum
                                If Not (Security.IsAllowedToStartRestrictedThread) Then
                                    HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                                End If
                            End If

                            If CurrentForumUser.UserID > 0 And (objForum.IsActive = True) Then
                                ' have to add some check to make sure the forum is still active

                                ' Rework if allowing anonymous posting, for now this is good
                                AllowUserEdit = True
                            End If
                        Case Else
                            ' If we reach this point, we know it is a reply, quote

                            ' security check
                            If Not objThread.ContainingForum.PublicPosting Then
                                'restricted posting forum
                                If Not (Security.IsAllowedToPostRestrictedReply) Then
                                    HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                                End If
                            End If

                            ' Rework if allowing anonymous posting, for now this is good
                            If CurrentForumUser.UserID > 0 And (objThread.ContainingForum.IsActive) Then
                                If (objThread.IsClosed = True) Then
                                    'see if reply is coming from the original thread author
                                    If objThread.StartedByUserID = CurrentForumUser.UserID Then
                                        AllowUserEdit = True
                                    End If
                                Else
                                    ' thre forum is active, the post is not closed, user is allowed to post a reply
                                    AllowUserEdit = True
                                End If
                            End If

                            txtSubject.Enabled = False
                            chkIsPinned.Enabled = False
                            chkIsClosed.Enabled = False
                    End Select

                    If AllowUserEdit = False Then
                        HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                    End If

                    'Spacer image
                    'imgAltHeader.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
                    imgAltHeaderPreview.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
                    imgAltHeaderReply.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
                    imgAltHeaderPoll.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")

                    'imglftHeader.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
                    'imgrghtHeader.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
                    imgReplyLeft.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
                    imgReplyRight.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")

                    imgPrevSpaceL.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
                    imgPrevSpaceR.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")

                    EnableControls(objAction)

                    If objConfig.DisableHTMLPosting = True Then
                        teContent.DefaultMode = "BASIC"
                        teContent.Mode = "BASIC"
                        teContent.ChooseMode = False
                        teContent.TextRenderMode = "T"
                        teContent.ChooseRender = False
                    End If

                    GeneratePost(objAction, objForum, objParentPost)

                    ' See if attachments are enabled
                    If objConfig.EnableAttachment Then
                        'CP - URL Controller integration
                        SetURLController(True)
                        If (Security.CanAddAttachments) Then
                            SetURLController(True)
                        Else
                            SetURLController(False)
                        End If
                    Else
                        SetURLController(False)
                    End If

                    divNotify.Visible = False
                    ' Make sure this is a logged on user (shouldn't get here if not logged in anyways)
                    If CurrentForumUser.UserID > 0 Then
                        ' If the user is admin or moderator or trusted
                        If (Security.CanLockThread) Then
                            ' Allow them to lock(close) a thread
                            divClose.Visible = True
                        Else
                            ' Otherwise, don't allow
                            divClose.Visible = False
                        End If

                        If (Security.CanPinThread) Then
                            divPinned.Visible = True
                        Else
                            divPinned.Visible = False
                        End If
                        ' load authorized forums
                        Dim cntGroup As New GroupController()
                        Dim arrForums As List(Of ForumInfo)

                        For Each objGroup As GroupInfo In cntGroup.GroupsGetByModuleID(ModuleId)
                            arrForums = cntGroup.AuthorizedForums(UserId, objGroup.GroupID, True, ModuleId, TabId)
                            If arrForums.Count > 0 Then
                                For Each objSingleForum As ForumInfo In arrForums
                                    ddlForum.Items.Add(New ListItem(objGroup.Name & " - " & objSingleForum.Name, objSingleForum.ForumID.ToString))
                                Next
                            End If
                        Next
                        ddlForum.Items.Insert(0, New ListItem("<" & Services.Localization.Localization.GetString("Not_Specified") & ">", "-1"))
                        ' the option to choose a forum is only available if there is no forumid passed to the module
                        If ForumID <> -1 Then
                            If Not ddlForum.Items.FindByValue(ForumID.ToString) Is Nothing Then
                                ddlForum.Items.FindByValue(ForumID.ToString).Selected = True
                            End If
                            ddlForum.Enabled = False
                        End If

                        Select Case objAction
                            Case PostAction.Edit
                                HandleTaxonomy(False, PostID, objForum.PublicView)
                            Case PostAction.New
                                ' should we show tagging control here (for all)?
                                HandleTaxonomy(True, -1, objForum.PublicView)
                            Case Else
                                ' quote/reply 
                        End Select

                        If objConfig.MailNotification Then
                            If Not CurrentForumUser.TrackedModule Then
                                ' handle Forum tracking
                                Dim blnTrackedForum As Boolean = False

                                For Each trackedForum As TrackingInfo In CurrentForumUser.TrackedForums(ModuleId)
                                    If trackedForum.ForumID = ForumID Then
                                        blnTrackedForum = True
                                        Exit For
                                    End If
                                Next

                                If (Not blnTrackedForum) Then
                                    Dim blnTrackedThread As Boolean = False
                                    ' Forum is NOT already being tracked, possibly show user the option to subscribe
                                    ' we need to check the case to see how to handle tracking at the thread level
                                    Select Case objAction
                                        Case PostAction.Edit
                                            ' user may already be tracking the thread
                                            ' we may not have threadID, we definately have postid
                                            For Each trackedThread As TrackingInfo In CurrentForumUser.TrackedThreads(ModuleId)
                                                If trackedThread.ThreadID = objThread.ThreadID Then
                                                    blnTrackedThread = True
                                                    Exit For
                                                End If
                                            Next
                                        Case PostAction.[New]
                                            ' Do nothing, its a new thread and impossible to track
                                        Case Else     ' reply/quote
                                            ' user may already be tracking the thread
                                            ' we may not have threadID, we definately have postid
                                            For Each trackedThread As TrackingInfo In CurrentForumUser.TrackedThreads(ModuleId)
                                                If trackedThread.ThreadID = objThread.ThreadID Then
                                                    blnTrackedThread = True
                                                    Exit For
                                                End If
                                            Next
                                    End Select
                                    ' If the user is not already tracking the thread, give them the option
                                    If (Not blnTrackedThread) Then
                                        divNotify.Visible = True
                                    End If
                                End If
                            Else
                                divNotify.Visible = True
                            End If
                        End If
                    End If

                    If Not Request.UrlReferrer Is Nothing Then
                        ' Store URL Referrer to return to portal
                        ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
                    End If
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' This submites the post from either initial view or preview view.  
        ''' This calls to PostToDataBase function which then takes the necessary action
        ''' and then navigates the user to the appropriate screen.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>ASP.NET 2.0 apparently does not allow two items to be handled by a single event. 
        ''' </remarks>
        Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
            Try
                If Len(teContent.Text) = 0 Then
                    If ViewState("PostContent") IsNot Nothing Then
                        teContent.Text = ViewState("PostContent").ToString()
                    Else
                        DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString(DotNetNuke.Forum.Library.Data.PostMessage.PostInvalidBody.ToString() + ".Text", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                        Exit Sub
                    End If
                End If

                Dim ParentPostID As Integer = 0
                Dim PostID As Integer = -1
                Dim PollID As Integer = -1
                Dim ThreadIconID As Integer = -1 ' NOT IMPLEMENTED
                Dim RemoteAddress As String = "0.0.0.0"
                Dim ThreadStatus As Forum.ThreadStatus
                Dim URLPostID As Integer = -1
                'Dim IsModerated As Boolean = False
                Dim IsQuote As Boolean = False

                If Request.IsAuthenticated = False Then
                    ' CP - Consider a way to save subject, body, etc so user can comeback and post if they timed out.
                    HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                    Exit Sub
                End If

                Dim objAction As New PostAction

                ' Validation (from UI)
                If Len(teContent.Text) = 0 Then
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString(DotNetNuke.Forum.Library.Data.PostMessage.PostInvalidBody.ToString() + ".Text", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                    Exit Sub
                End If
                If Len(txtSubject.Text) = 0 Then
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString(DotNetNuke.Forum.Library.Data.PostMessage.PostInvalidSubject.ToString() + ".Text", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                    Exit Sub
                End If
                If ddlForum.SelectedItem Is Nothing Or ddlForum.SelectedItem.Value = "-1" Then
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString(DotNetNuke.Forum.Library.Data.PostMessage.ForumDoesntExist.ToString() + ".Text", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                    Exit Sub
                End If

                If (Not Request.QueryString("action") Is Nothing) Then
                    objAction = CType([Enum].Parse(GetType(PostAction), Request.QueryString("action"), True), PostAction)
                End If

                If Not Request.QueryString("postid") Is Nothing Then
                    URLPostID = Integer.Parse(Request.QueryString("postid"))
                End If

                If dnncbThreadStatus.SelectedIndex > 0 Then
                    ThreadStatus = CType(dnncbThreadStatus.SelectedValue, Forum.ThreadStatus)
                Else
                    ThreadStatus = Forum.ThreadStatus.NotSet
                End If

                Dim cntForum As New ForumController
                Dim objForum As ForumInfo = cntForum.GetForumItemCache(Integer.Parse(ddlForum.SelectedItem.Value))
                Dim objModSecurity As New Forum.ModuleSecurity(ModuleId, TabId, objForum.ForumID, CurrentForumUser.UserID)
                Dim ThreadID As Integer = -1
                Dim Terms As List(Of DotNetNuke.Entities.Content.Taxonomy.Term)

                If objForum.AllowPolls Then
                    PollID = CType(txtPollID.Text, Integer)
                End If

                Select Case objAction
                    Case PostAction.Edit
                        Dim cntPost As New PostController
                        Dim objEditPost As New PostInfo

                        objEditPost = cntPost.GetPostInfo(URLPostID, PortalId)

                        ParentPostID = objEditPost.ParentPostID
                        PostID = objEditPost.PostID
                        ThreadID = objEditPost.ThreadID

                        ' if polls are enabled, make sure db is properly setup
                        If ParentPostID = 0 And PollID > 0 Then
                            If Not HandlePoll(PollID, False) Then
                                Exit Sub
                            End If
                        End If

                        Terms = objThread.Terms

                        If objThread.ThreadID = objEditPost.PostID Then
                            objThread.Terms.Clear()
                            objThread.Terms.AddRange(tsTerms.Terms)
                        End If
                    Case PostAction.New
                        ' not sure this is correct (first line below)
                        ParentPostID = URLPostID

                        If PollID > 0 Then
                            If Not HandlePoll(PollID, False) Then
                                Return
                            End If
                        ElseIf objForum.AllowPolls Then
                            OrphanPollCleanup()
                        End If

                        Dim tempThread As New ThreadInfo
                        tempThread.Terms.Clear()
                        tempThread.Terms.AddRange(tsTerms.Terms)

                        Terms = tempThread.Terms
                    Case PostAction.Quote
                        Dim cntPost As New PostController
                        Dim objReplyToPost As New PostInfo

                        objReplyToPost = cntPost.GetPostInfo(URLPostID, PortalId)

                        ParentPostID = URLPostID
                        IsQuote = True
                        ThreadID = objReplyToPost.ThreadID
                        Terms = objThread.Terms
                    Case Else     ' reply
                        Dim cntPost As New PostController
                        Dim objReplyToPost As New PostInfo

                        objReplyToPost = cntPost.GetPostInfo(URLPostID, PortalId)

                        ParentPostID = URLPostID
                        ThreadID = objReplyToPost.ThreadID
                        Terms = objThread.Terms
                End Select

                If Not Request.ServerVariables("REMOTE_ADDR") Is Nothing Then
                    RemoteAddress = Request.ServerVariables("REMOTE_ADDR")
                End If

                Dim cntPostConnect As New PostConnector
                Dim PostMessage As SubmitPostResult

                PostMessage = cntPostConnect.SubmitInternalPost(TabId, ModuleId, PortalId, CurrentForumUser.UserID, txtSubject.Text, teContent.Text, objForum.ForumID, ParentPostID, PostID, chkIsPinned.Checked, chkIsClosed.Checked, chkNotify.Checked, ThreadStatus, ctlAttachment.lstAttachmentIDs, RemoteAddress, PollID, IsQuote, ThreadID, Terms)

                Select Case PostMessage.Result
                    Case DotNetNuke.Forum.Library.Data.PostMessage.PostApproved
                        Dim ReturnURL As String = NavigateURL()

                        ReturnURL = Utilities.Links.ContainerViewPostLink(PortalId, TabId, objForum.ForumID, PostMessage.PostId, txtSubject.Text)

                        Response.Redirect(ReturnURL, False)
                    Case DotNetNuke.Forum.Library.Data.PostMessage.PostModerated
                        divNewPost.Visible = False
                        tblOldPost.Visible = False
                        tblPreview.Visible = False
                        cmdCancel.Visible = False
                        cmdBackToEdit.Visible = False
                        cmdSubmit.Visible = False
                        cmdPreview.Visible = False
                        cmdBackToForum.Visible = True
                        divModerate.Visible = True
                        tblPoll.Visible = False
                    Case Else
                        DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString(PostMessage.ToString() + ".Text", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End Select

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' Takes the user back to where they were.
        ''' Handle any poll cleanup, just in case.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Dim cntForum As New ForumController
                Dim objForum As ForumInfo = cntForum.GetForumItemCache(Integer.Parse(ddlForum.SelectedItem.Value))

                If objForum.AllowPolls Then
                    ' Make sure user didn't create poll here that they are about to orhpan
                    If txtPollID.Text <> String.Empty Then
                        Dim cntPoll As New PollController
                        Dim objPoll As New PollInfo

                        objPoll = cntPoll.GetPoll(CType(txtPollID.Text, Integer))

                        If Not objPoll Is Nothing Then
                            If objPoll.ThreadID = -1 Then
                                ' No thread assigned to the poll, delete it
                                cntPoll.DeletePoll(CType(txtPollID.Text, Integer))
                            Else
                                ' thread exists, need to make sure we still have valid # of answers
                                If Not HandlePoll(CType(txtPollID.Text, Integer), True) Then
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                    ' cleanup any other oprhans
                    OrphanPollCleanup()
                End If
                If Not ViewState("UrlReferrer") Is Nothing Then
                    Response.Redirect(CType(ViewState("UrlReferrer"), String), False)
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' Hides areas on the screen and shows user what their post will look like
        ''' once it is submitted.  (This does rendering)
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub cmdPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
            Try
                ' Obtain post action
                Dim objAction As PostAction
                If (Not Request.QueryString("action") Is Nothing) Then
                    objAction = CType([Enum].Parse(GetType(PostAction), Request.QueryString("action"), True), PostAction)
                End If

                Dim Connect As New PostConnector
                lblPreview.Text = System.Web.HttpUtility.HtmlDecode(Connect.ProcessPostBody(teContent.Text, objConfig, PortalId, objAction, UserId))

                tblPreview.Visible = True
                cmdSubmit.Visible = True
                cmdPreview.Visible = False
                cmdCancel.Visible = False
                cmdBackToEdit.Visible = True
                divNewPost.Visible = False
                lblNoAnswer.Visible = False

                If tblOldPost.Visible = True Then
                    ViewState("DisplayOldPost") = "True"
                    tblOldPost.Visible = False
                End If

                If tblPoll.Visible = True Then
                    ViewState("DisplayPoll") = "True"
                    tblPoll.Visible = False
                End If

                ViewState("PostContent") = teContent.Text

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' Takes the user back to edit post mode when clicked.  
        ''' (This is only visible in preview mode)
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub cmdBackToEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBackToEdit.Click
            tblPreview.Visible = False
            cmdCancel.Visible = True
            cmdSubmit.Visible = True
            cmdPreview.Visible = True
            cmdBackToEdit.Visible = False
            divNewPost.Visible = True

            If ViewState("PostContent") IsNot Nothing Then
                teContent.Text = ViewState("PostContent").ToString()
            End If

            If ViewState("DisplayOldPost") IsNot Nothing Then
                tblOldPost.Visible = True
            End If

            If ViewState("DisplayPoll") IsNot Nothing Then
                tblPoll.Visible = True
            End If
        End Sub

        ''' <summary>
        ''' For moderated post, redirect back to the forum threads page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub cmdBackToForum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBackToForum.Click
            Try
                If Not ViewState("UrlReferrer") Is Nothing Then
                    Response.Redirect(CType(ViewState("UrlReferrer"), String), False)
                Else
                    Response.Redirect(NavigateURL(), False)
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' Adds an answer to the available options for a poll.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub cmdAddAnswer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddAnswer.Click
            dnncbThreadStatus.Enabled = False
            ApplyAnswerOrder()
            AddPollAnswer()
            txtAddAnswer.Text = String.Empty
        End Sub

        ''' <summary>
        ''' Fired off when the thread status is changed to change what is available to the poster.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub dnncbThreadStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dnncbThreadStatus.SelectedIndexChanged
            If dnncbThreadStatus.SelectedValue = CInt(ThreadStatus.Poll).ToString() Then
                tblPoll.Visible = True
            Else
                tblPoll.Visible = False
            End If
        End Sub

        ''' <summary>
        ''' dgAnswers_ItemCommand runs when a Command event is raised in the Grid 
        ''' </summary>
        ''' <remarks>Only if DHTML is not supported</remarks>
        Protected Sub dgAnswers_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgAnswers.ItemCommand
            Dim commandName As String = e.CommandName
            Dim commandArgument As Integer = CType(e.CommandArgument, Integer)

            Select Case commandName
                Case "Delete"
                    Dim AnswerID As Integer = CType(dgAnswers.DataKeys(e.Item.ItemIndex), Integer)
                    DeleteAnswer(AnswerID)
                Case "MoveUp"
                    Dim index As Integer = e.Item.ItemIndex
                    MoveAnswerUp(index)
                Case "MoveDown"
                    Dim index As Integer = e.Item.ItemIndex
                    MoveAnswerDown(index)
            End Select
        End Sub

        ''' <summary>
        ''' When it is determined that the client supports a rich interactivity the dgdgAnswers_ItemCreated 
        ''' event is responsible for disabling all the unneeded AutoPostBacks, along with assiging the appropriate
        '''	client-side script for each event handler
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Protected Sub dgAnswers_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAnswers.ItemCreated
            If SupportsRichClient() Then
                Select Case e.Item.ItemType

                    Case ListItemType.AlternatingItem, ListItemType.Item
                        DotNetNuke.UI.Utilities.ClientAPI.EnableClientSideReorder(e.Item.Cells(COLUMN_MOVE_DOWN).Controls(0), Me.Page, False, Me.dgAnswers.ClientID)
                        DotNetNuke.UI.Utilities.ClientAPI.EnableClientSideReorder(e.Item.Cells(COLUMN_MOVE_UP).Controls(0), Me.Page, True, Me.dgAnswers.ClientID)
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Modifies items as they are bound to the datagrid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub dgAnswers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAnswers.ItemDataBound
            Dim item As System.Web.UI.WebControls.DataGridItem = e.Item

            If item.ItemType = System.Web.UI.WebControls.ListItemType.Item Or _
             item.ItemType = System.Web.UI.WebControls.ListItemType.AlternatingItem Or _
             item.ItemType = System.Web.UI.WebControls.ListItemType.SelectedItem Then
            End If
        End Sub

#End Region

#Region "Private Methods"

#Region "Answers Grid"

        ''' <summary>
        ''' Removes an answer from a poll.
        ''' </summary>
        ''' <param name="AnswerID">The Answer to delete.</param>
        ''' <remarks></remarks>
        Private Sub DeleteAnswer(ByVal AnswerID As Integer)
            ' '' Make sure answers are in proper order.
            'ApplyAnswerOrder()
            Dim cntAnswers As New AnswerController
            cntAnswers.DeleteAnswer(AnswerID)

            BindGrid()
        End Sub

        ''' <summary>
        ''' Adds an answer to a poll, if the poll doesn't exist it gets created.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub AddPollAnswer()
            If txtAddAnswer.Text <> String.Empty Then
                lblNoAnswer.Visible = False
                Dim objSecurity As New PortalSecurity
                Dim ctlWordFilter As New WordFilterController
                Dim PollQuestion As String
                Dim PollTakenMessage As String

                If txtPollID.Text = "-1" Then
                    ' Poll doesn't exist yet
                    Dim cntPoll As New PollController
                    Dim objPoll As New PollInfo

                    If txtQuestion.Text <> String.Empty Then
                        PollQuestion = objSecurity.InputFilter(txtQuestion.Text, PortalSecurity.FilterFlag.NoScripting)
                    Else
                        PollQuestion = objSecurity.InputFilter(Localization.GetString("DefaultNewPoll", Me.LocalResourceFile), PortalSecurity.FilterFlag.NoScripting)
                    End If
                    txtQuestion.Text = PollQuestion

                    If txtTakenMessage.Text <> String.Empty Then
                        PollTakenMessage = objSecurity.InputFilter(txtTakenMessage.Text, PortalSecurity.FilterFlag.NoScripting)
                    Else
                        PollTakenMessage = objSecurity.InputFilter(Localization.GetString("DefaultTakenMsg", Me.LocalResourceFile), PortalSecurity.FilterFlag.NoScripting)
                    End If
                    txtTakenMessage.Text = PollTakenMessage

                    If objConfig.EnableBadWordFilter Then
                        PollQuestion = ctlWordFilter.FilterBadWord(PollQuestion, PortalId)
                        PollTakenMessage = ctlWordFilter.FilterBadWord(PollTakenMessage, PortalId)
                    End If
                    objPoll.Question = PollQuestion
                    objPoll.TakenMessage = PollTakenMessage

                    objPoll.ShowResults = chkShowResults.Checked
                    If txtEndDate.Text <> String.Empty Then
                        objPoll.EndDate = CDate(txtEndDate.Text)
                    End If

                    objPoll.ShowResults = chkShowResults.Checked


                    objPoll.ModuleID = ModuleId
                    Dim PollID As Integer = -1
                    PollID = cntPoll.AddPoll(objPoll)
                    txtPollID.Text = PollID.ToString()

                    ' Now we have a poll, create the first answer
                    Dim cntAnswer As New AnswerController
                    Dim objAnswer As New AnswerInfo
                    objAnswer.PollID = PollID
                    objAnswer.SortOrder = 0
                    objAnswer.Answer = objSecurity.InputFilter(txtAddAnswer.Text, PortalSecurity.FilterFlag.NoScripting)

                    cntAnswer.AddAnswer(objAnswer)

                    BindGrid()
                Else
                    ' Poll exists already
                    ' First make sure we update the sort order in case the user changed it

                    ' Now add the answer item
                    Dim cntAnswer As New AnswerController
                    Dim objAnswer As New AnswerInfo
                    objAnswer.PollID = CType(txtPollID.Text, Integer)
                    objAnswer.SortOrder = dgAnswers.Items.Count
                    objAnswer.Answer = objSecurity.InputFilter(txtAddAnswer.Text, PortalSecurity.FilterFlag.NoScripting)

                    cntAnswer.AddAnswer(objAnswer)

                    BindGrid()
                End If
            Else
                ' show some warning/error
                lblNoAnswer.Visible = True
            End If
        End Sub

        ''' <summary>
        ''' Persists sort order changes to the database.
        ''' </summary>
        ''' <param name="objAnswer"></param>
        ''' <remarks></remarks>
        Private Sub UpdateAnswerSortOrder(ByVal objAnswer As AnswerInfo)
            Dim cntAnswers As New AnswerController

            cntAnswers.UpdateAnswer(objAnswer)
        End Sub

        ''' <summary>
        ''' Takes the client side grid and gets the new sort order.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ApplyAnswerOrder()
            Try
                Dim cntAnswers As New AnswerController
                Dim arrAnswers As List(Of AnswerInfo)
                arrAnswers = cntAnswers.GetPollAnswers(CType(txtPollID.Text, Integer))

                Dim aryNewOrder() As String = DotNetNuke.UI.Utilities.ClientAPI.GetClientSideReorder(Me.dgAnswers.ClientID, Me.Page)
                'assign sortorder
                For i As Integer = 0 To aryNewOrder.Length - 1
                    arrAnswers(CInt(aryNewOrder(i))).SortOrder = i
                    UpdateAnswerSortOrder(arrAnswers(CInt(aryNewOrder(i))))
                Next
                BindGrid()
            Catch ex As Exception
                LogException(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Binds a list of available answers to the poll answers grid.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindGrid()
            If Not txtPollID.Text = "-1" Then
                Dim cntAnswers As New AnswerController
                Dim arrAnswers As New List(Of AnswerInfo)

                arrAnswers = cntAnswers.GetPollAnswers(CType(txtPollID.Text, Integer))
                If arrAnswers.Count > 0 Then
                    dgAnswers.DataKeyField = "AnswerID"
                    dgAnswers.DataSource = arrAnswers
                    dgAnswers.DataBind()
                    dgAnswers.ShowHeader = True
                Else
                    dgAnswers.DataSource = New ArrayList()
                    dgAnswers.DataBind()
                    dgAnswers.ShowHeader = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Moves an Answer  (Only used when DHTML is not supported)
        ''' </summary>
        ''' <param name="index">The index of the Answer to move.</param>
        ''' <param name="destIndex">The new index of the Answer.</param>
        ''' <history>
        ''' </history>
        Private Sub MoveAnswer(ByVal index As Integer, ByVal destIndex As Integer)
            Dim cntAnswers As New AnswerController
            Dim arrAnswers As List(Of AnswerInfo)
            arrAnswers = cntAnswers.GetPollAnswers(CType(txtPollID.Text, Integer))

            Dim objAnswer As AnswerInfo = arrAnswers(index)
            Dim objNext As AnswerInfo = arrAnswers(destIndex)

            Dim currentOrder As Integer = objAnswer.SortOrder
            Dim nextOrder As Integer = objNext.SortOrder
            'Swap ViewOrders
            objAnswer.SortOrder = nextOrder
            objNext.SortOrder = currentOrder
            'Refresh Grid
            BindGrid()
        End Sub

        ''' <summary>
        ''' Moves an Answer down in the SortOrder.
        ''' </summary>
        ''' <param name="index">The index of the Answer to move.</param>
        ''' <history>
        ''' </history>
        Private Sub MoveAnswerDown(ByVal index As Integer)
            MoveAnswer(index, index + 1)
        End Sub

        ''' <summary>
        ''' Moves an Answer up in the SortOrder.
        ''' </summary>
        ''' <param name="index">The index of the Answer to move.</param>
        ''' <history>
        ''' </history>
        Private Sub MoveAnswerUp(ByVal index As Integer)
            MoveAnswer(index, index - 1)
        End Sub

        ''' <summary>
        ''' Helper function that determines whether the client-side functionality is possible
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SupportsRichClient() As Boolean
            Return DotNetNuke.UI.Utilities.ClientAPI.BrowserSupportsFunctionality(DotNetNuke.UI.Utilities.ClientAPI.ClientFunctionality.DHTML)
        End Function

#End Region

        ''' <summary>
        ''' Used to make sure polls have enough answers, if not they are to be removed.
        ''' </summary>
        ''' <param name="PollID"></param>
        ''' <remarks></remarks>
        Private Function HandlePoll(ByVal PollID As Integer, ByVal Cancel As Boolean) As Boolean
            Dim boolContinue As Boolean = True
            Dim objSecurity As New PortalSecurity

            If dgAnswers.Items.Count > 1 Then
                Dim cntPoll As New PollController
                Dim objPoll As New PollInfo
                Dim ctlWordFilter As New WordFilterController
                Dim PollQuestion As String
                Dim PollTakenMessage As String

                objPoll.PollID = PollID

                PollQuestion = objSecurity.InputFilter(txtQuestion.Text, PortalSecurity.FilterFlag.NoScripting)
                PollTakenMessage = objSecurity.InputFilter(txtTakenMessage.Text, PortalSecurity.FilterFlag.NoScripting)
                If objConfig.EnableBadWordFilter Then
                    PollQuestion = ctlWordFilter.FilterBadWord(PollQuestion, PortalId)
                    PollTakenMessage = ctlWordFilter.FilterBadWord(PollTakenMessage, PortalId)
                End If
                objPoll.Question = PollQuestion
                objPoll.TakenMessage = PollTakenMessage

                objPoll.ShowResults = chkShowResults.Checked
                If txtEndDate.Text <> String.Empty Then
                    objPoll.EndDate = CDate(txtEndDate.Text)
                End If

                cntPoll.UpdatePoll(objPoll)
                ' make sure answer sort order is handled.
                If Not Cancel Then
                    ApplyAnswerOrder()
                End If
            ElseIf dgAnswers.Items.Count = 1 Then
                ' show user they need more than a single answer for the poll
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("lblMoreAnswers.Text", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)

                boolContinue = False
            Else
                ' no answers for poll, delete it (first make sure thread status is not set to poll)
                If dnncbThreadStatus.SelectedValue = CInt(ThreadStatus.Poll).ToString() Then
                    dnncbThreadStatus.SelectedValue = CInt(ThreadStatus.NotSet).ToString()
                End If
                ' answers removed, delete poll
                Dim cntPoll As New PollController
                cntPoll.DeletePoll(PollID)
                ' make sure poll is no longer associated w/ post
                PollID = -1
            End If
            Return boolContinue
        End Function

        ''' <summary>
        ''' Removes orphaned polls from the database.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub OrphanPollCleanup()
            ' Forum allows polls, cleanup any orphans (which should very rarely occur) - only way to have orphan is poll creation, never creating thread
            Dim cntPoll As New PollController
            Dim arrPolls As List(Of PollInfo)
            arrPolls = cntPoll.GetOrphanedPolls(ModuleId)

            If arrPolls.Count > 0 Then
                For Each objPoll As PollInfo In arrPolls
                    cntPoll.DeletePoll(objPoll.PollID)
                Next
            End If

            dnncbThreadStatus.SelectedValue = CInt(ThreadStatus.NotSet).ToString()
        End Sub

        ''' <summary>
        ''' Sets the properties of the core's URLControl (used for attachments)
        ''' </summary>
        ''' <param name="Enabled">Boolean which signifies if attachments are enabled or not.</param>
        ''' <remarks></remarks>
        Private Sub SetURLController(ByVal Enabled As Boolean)
            If Enabled Then
                divAttachments.Visible = True
                'We only set the PostID if we are editing a post!
                If Not Request.QueryString("action") Is Nothing Then
                    If Request.QueryString("action").ToLower = "edit" Then
                        ctlAttachment.PostId = Int32.Parse(Request.QueryString("postid"))
                    Else
                        ctlAttachment.PostId = -1
                    End If
                Else
                    ctlAttachment.PostId = -1
                End If

                'AJAX
                ctlAttachment.ModuleId = ModuleId
                ctlAttachment.LoadInitialView()
            Else
                ctlAttachment.ModuleId = ModuleId
                ctlAttachment.Visible = False
                divAttachments.Visible = False
            End If
        End Sub

        ''' <summary>
        ''' Show/Hides replied to post if it exists. Also sets the intial visiblity for this screen.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Sub EnableControls(ByVal objAction As PostAction)
            If (objAction = PostAction.Reply) Then
                tblOldPost.Visible = True
            Else
                tblOldPost.Visible = False
            End If

            cmdBackToForum.Visible = False
            cmdBackToEdit.Visible = False
            cmdCancel.Visible = True
            cmdSubmit.Visible = True
            divModerate.Visible = False
        End Sub

        ''' <summary>
        ''' This gets the data for the post if in edit post mode.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Sub GeneratePost(ByVal objAction As PostAction, ByVal objForum As ForumInfo, ByVal objParentPost As PostInfo)
            Dim ctlForum As New ForumController
            ' generate post content
            If (Not objAction = PostAction.[New]) Then
                ' [skeel] no need to do anything here unless reply or edit
                Dim fTextDecode As Utilities.PostContent = Nothing
                If objAction = PostAction.Edit Or objAction = PostAction.Reply Then
                    If objParentPost.ParseInfo = PostParserInfo.None Or objParentPost.ParseInfo = PostParserInfo.File Then
                        'Nothing to Parse or just an Attachment not inline
                        fTextDecode = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objParentPost.Body), objConfig)
                    Else
                        If objParentPost.ParseInfo < PostParserInfo.Inline Then
                            'Something to parse, but not any inline instances
                            If objConfig.DisableHTMLPosting = True And objAction = PostAction.Edit Then
                                ' We are editing with HTML disabled, don't parse anything!
                                fTextDecode = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objParentPost.Body), objConfig)
                            Else
                                If objAction = PostAction.Edit Then
                                    'For editing, we only parse BBcode
                                    fTextDecode = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objParentPost.Body), objConfig, PostParserInfo.BBCode)
                                Else
                                    fTextDecode = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objParentPost.Body), objConfig, objParentPost.ParseInfo)
                                End If
                            End If
                        Else
                            'At lease Inline to Parse
                            If objAction = PostAction.Edit Then
                                If objConfig.DisableHTMLPosting = True Then
                                    ' We never parse for editing of HTML disabled posts
                                    fTextDecode = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objParentPost.Body), objConfig)
                                Else
                                    'Do the BBCode, we are editing
                                    fTextDecode = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objParentPost.Body), objConfig, PostParserInfo.BBCode)
                                End If
                            ElseIf objAction = PostAction.Reply Then
                                'Ignore the inlines, this is a parentpost
                                fTextDecode = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objParentPost.Body), objConfig, objParentPost.ParseInfo, objParentPost.AttachmentCollection(objConfig.EnableAttachment), True)
                            End If
                        End If
                    End If
                ElseIf objAction = PostAction.Quote Then
                    If objConfig.DisableHTMLPosting = True Then
                        ' We don't parse quotes either when HTML is disabled
                        fTextDecode = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objParentPost.Body), objConfig)
                    Else
                        ' When quoting a post, we should restrict to only parsing of BBcode and emoticons
                        Dim strQuoteBody As String = System.Web.HttpUtility.HtmlDecode(objParentPost.Body)
                        ' do we need to replace inline attachments?
                        If objParentPost.ParseInfo >= PostParserInfo.Inline Then
                            strQuoteBody = Utilities.ForumUtils.RemoveInlineAttachments(strQuoteBody)
                        End If
                        fTextDecode = New Utilities.PostContent(strQuoteBody, objConfig, PostParserInfo.BBCode + PostParserInfo.Emoticon)
                    End If
                End If

                With objParentPost
                    chkIsClosed.Checked = .ParentThread.IsClosed
                    chkIsPinned.Checked = .ParentThread.IsPinned
                End With

                hlAuthor.Text = objParentPost.Author.SiteAlias
                If Not objConfig.EnableExternalProfile Then
                    hlAuthor.NavigateUrl = objParentPost.Author.UserCoreProfileLink
                Else
                    hlAuthor.NavigateUrl = Utilities.Links.UserExternalProfileLink(objParentPost.UserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objParentPost.Author.Username)
                End If
                hlAuthor.ToolTip = Localization.GetString("ReplyToToolTip", Me.LocalResourceFile)

                If objAction = PostAction.Reply Then
                    lblMessage.Text = fTextDecode.Text
                End If

                Select Case objAction
                    Case PostAction.Edit
                        txtSubject.Text = HttpUtility.HtmlDecode(objParentPost.Subject)
                        teContent.Text = fTextDecode.Text
                        BindThreadStatus()

                        If objConfig.EnableThreadStatus And objParentPost.ParentPostID = 0 And objForum.EnableForumsThreadStatus Then
                            divThreadStatus.Visible = True
                            dnncbThreadStatus.SelectedIndex = CType(objParentPost.ParentThread.ThreadStatus, Integer)
                        Else
                            If objForum.AllowPolls And objParentPost.ParentPostID = 0 Then
                                dnncbThreadStatus.SelectedIndex = CType(objParentPost.ParentThread.ThreadStatus, Integer)
                            End If
                            divThreadStatus.Visible = False
                        End If

                        ' handle if editing first post, show polling options if enabled
                        If objForum.AllowPolls And objParentPost.ParentPostID = 0 And dnncbThreadStatus.SelectedValue = CInt(ThreadStatus.Poll).ToString() Then
                            tblPoll.Visible = True
                            txtPollID.Text = objParentPost.ParentThread.PollID.ToString()
                            Dim cntPoll As New PollController
                            Dim objPoll As New PollInfo

                            objPoll = cntPoll.GetPoll(objParentPost.ParentThread.PollID)

                            If Not objPoll Is Nothing Then
                                txtQuestion.Text = objPoll.Question
                                txtTakenMessage.Text = objPoll.TakenMessage
                                chkShowResults.Checked = objPoll.ShowResults
                                txtEndDate.Text = objPoll.EndDate.ToShortDateString()
                                ' Handle existing poll information
                                BindGrid()
                            End If
                        Else
                            tblPoll.Visible = False
                        End If
                    Case PostAction.Reply
                        tblPoll.Visible = False
                        txtSubject.Text = HttpUtility.HtmlDecode(Utilities.ForumUtils.SetReplySubject(objParentPost.Subject))
                        divThreadStatus.Visible = False
                    Case PostAction.Quote
                        tblPoll.Visible = False
                        txtSubject.Text = HttpUtility.HtmlDecode(Utilities.ForumUtils.SetReplySubject(objParentPost.Subject))
                        teContent.Text = fTextDecode.ProcessQuoteBody(objParentPost.Author.SiteAlias, objConfig)
                        divThreadStatus.Visible = False
                End Select
            Else
                ' This is a new thread
                If objConfig.EnableThreadStatus And objForum.EnableForumsThreadStatus Then
                    BindThreadStatus()
                    divThreadStatus.Visible = True
                    tblPoll.Visible = False
                Else
                    ' Either module or forum doesn't use thread status, handle showing of polling options if enabled
                    If objForum.AllowPolls Then
                        tblPoll.Visible = True
                    Else
                        tblPoll.Visible = False
                    End If
                    divThreadStatus.Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Binds the list of available thread status choices.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindThreadStatus()
            dnncbThreadStatus.Items.Clear()

            dnncbThreadStatus.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("NoneSpecified", LocalResourceFile), "0"))
            dnncbThreadStatus.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Unanswered", LocalResourceFile), "1"))
            dnncbThreadStatus.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Answered", LocalResourceFile), "2"))
            dnncbThreadStatus.Items.Insert(3, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Informative", LocalResourceFile), "3"))

            'polling changes
            Try
                Dim cntForum As New ForumController
                Dim objForum As ForumInfo = cntForum.GetForumItemCache(ForumID)

                If objForum.AllowPolls Then
                    Dim statusEntry As New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Poll", objConfig.SharedResourceFile), CInt(ThreadStatus.Poll).ToString())
                    dnncbThreadStatus.Items.Add(statusEntry)
                End If
            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Handles taxonomy controls for the user interface.
        ''' </summary>
        ''' <param name="NewThread"></param>
        ''' <param name="PostID"></param>
        ''' <param name="IsPublic"></param>
        ''' <remarks>We are only allowing tagging in public forums, because of security concerns in tag search results (we go one level deeper in perms than core).</remarks>
        Private Sub HandleTaxonomy(ByVal NewThread As Boolean, ByVal PostID As Integer, ByVal IsPublic As Boolean)
            tsTerms.PortalId = PortalId

            If IsPublic Then
                If NewThread And objConfig.EnableTagging Then
                    divTagging.Visible = True
                Else
                    ' this is either not a new thread or taxonomy is disabled
                    If Not NewThread Then
                        If (PostID = objThread.ThreadID) AndAlso (objConfig.EnableTagging) Then
                            Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, objForum.ForumID, UserId)

                            If Security.IsForumModerator Then
                                divTagging.Visible = True
                            End If
                        End If
                    End If
                End If

                If objThread IsNot Nothing Then
                    tsTerms.Terms = objThread.Terms
                Else
                    tsTerms.Terms = New List(Of DotNetNuke.Entities.Content.Taxonomy.Term)
                End If
            Else
                tsTerms.Terms = New List(Of DotNetNuke.Entities.Content.Taxonomy.Term)
                divTagging.Visible = False
            End If

            tsTerms.DataBind()
        End Sub

#End Region

    End Class

End Namespace