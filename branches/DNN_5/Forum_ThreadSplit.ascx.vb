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

Imports DotNetNuke.Modules.Forum.Utilities

Namespace DotNetNuke.Modules.Forum

    ''' <summary>
    ''' Allows a moderator/admin to move a thread to a new forum.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public MustInherit Class ThreadSplit
        Inherits ForumModuleBase
        Implements Entities.Modules.IActionable

#Region "Private Members"

        Private _ThreadID As Integer
        Private _SplitMode As Boolean = False

#End Region

#Region "Optional Interfaces"

        ''' <summary>
        ''' Gets a list of module actions available to the user to provide it to DNN core.
        ''' </summary>
        ''' <value></value>
        ''' <returns>The collection of module actions available to the user</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
            Get
                Return Utilities.ForumUtils.PerUserModuleActions(objConfig, Me)
            End Get
        End Property

#End Region

#Region "Properties"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Property SplitMode() As Boolean
            Get
                Return _SplitMode
            End Get
            Set(ByVal Value As Boolean)
                _SplitMode = Value
            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property PostID() As Integer
            Get
                If HttpContext.Current.Request.QueryString("postid") Is Nothing Then
                    HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                    Return -1
                Else
                    Return Convert.ToInt32(HttpContext.Current.Request.QueryString("postid"))
                End If
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Property ThreadID() As Integer
            Get
                If CStr(ViewState("ThreadID")) <> String.Empty Then
                    _ThreadID = CInt(ViewState("ThreadID"))
                End If
                Return _ThreadID
            End Get
            Set(ByVal value As Integer)
                _ThreadID = value
                ViewState("ThreadID") = value
            End Set
        End Property

#End Region

#Region "Enums"

        ''' <summary>
        ''' The image type to use to represent the various levels in the treeview. 
        ''' </summary>
        ''' <remarks></remarks>
        Private Enum eImageType
            Group = 0
            Forum = 1
        End Enum

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Loads page settings.
        ''' </summary>
        ''' <param name="sender">System.Object,</param>
        ''' <param name="e">System.EventArgs</param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Dim objSecurity As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

                If Request.IsAuthenticated Then
                    If Not (objSecurity.IsModerator) Then
                        ' they don't belong here
                        HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                    End If
                Else
                    ' they don't belong here
                    HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                End If

                Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
                ForumUtils.LoadCssFile(DefaultPage, objConfig)

                If Page.IsPostBack = False Then
                    If Not Request.UrlReferrer Is Nothing Then
                        cmdCancel.NavigateUrl = Request.UrlReferrer.ToString()
                    Else
                        cmdCancel.NavigateUrl = NavigateURL()
                    End If

                    Dim cntPost As New PostController
                    Dim objPost As New PostInfo
                    objPost = cntPost.GetPostInfo(PostID, PortalId)

                    ' if the post is nothing here, it was deleted
                    If objPost Is Nothing Then
                        HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                    End If

                    ThreadID = objPost.ThreadID
                    txtSubject.Text = objPost.Subject
                    lblPost.Text = FormatBody(PostID)   ' Server.HtmlDecode(objPostInfo.Body)

                    ForumTreeview.PopulateTelerikTree(objConfig, rtvForums, UserId, TabId)
                    SelectDefaultForumTree()

                    ' Get all posts
                    Dim arrPosts As New List(Of PostInfo)
                    arrPosts = cntPost.PostGetAllForThread(ThreadID)

                    cmdMove.Attributes.Add("onClick", "javascript:return confirm('" & DotNetNuke.Services.Localization.Localization.GetString("ThreadSplitPostApprove.Text", Me.LocalResourceFile) & "');")

                    dlPostsForThread.DataSource = arrPosts
                    dlPostsForThread.DataBind()

                    If Not Request.UrlReferrer Is Nothing Then
                        ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
                    End If
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' Moves the thread to a new forum, unless user is attempting to move
        ''' the thread to the same forum.
        ''' </summary>
        ''' <param name="sender">System.Object,</param>
        ''' <param name="e">System.EventArgs</param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub cmdMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMove.Click
            Try
                ' See if user selected a forum to move thread to.
                Dim newForumID As Integer = -1

                For Each objNode As Telerik.Web.UI.RadTreeNode In Me.rtvForums.CheckedNodes
                    Dim strType As String = objNode.Value.Substring(0, 1)
                    Dim iID As Integer = CInt(objNode.Value.Substring(1, objNode.Value.Length - 1))

                    newForumID = iID
                    Exit For
                Next

                If newForumID = -1 Then
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, DotNetNuke.Services.Localization.Localization.GetString("NoForumSelected.Text", Me.LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                    Exit Sub
                Else
                    ' build notes (old forum, new forum, say post was moved in body (basically email body text)
                    Dim Notes As String = "Thread Split"
                    Dim cntForum As New ForumController()
                    Dim objForum As ForumInfo = cntForum.GetForumItemCache(ForumID)

                    Dim ctlThread As New ThreadController
                    'Dim MyProfileUrl As String = Utils.MySettingsLink(TabId, ModuleId)
                    Dim MyProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)

                    ' Split this post into a new thread
                    ctlThread.SplitThread(PostID, ThreadID, newForumID, UserId, txtSubject.Text, Notes, objForum.ParentID, ModuleId)

                    ' we need to grab all the selected posts, set their parent to new threadid.
                    ' we know the first posts in each of the threads have sort order set to 0, we need to start w/ 1 here and increment by 1 for each post per thread
                    Dim newThreadSortOrder As Integer = 1
                    Dim oldThreadSortOrder As Integer = 1
                    Dim ctlPosts As New PostController

                    For Each item As DataListItem In dlPostsForThread.Items
                        Dim chk As CheckBox = CType(item.FindControl("chkThreadToSplit"), CheckBox)
                        If chk.Checked Then
                            ' For each selected post in datagrid
                            ' set its parent to the mPostID (the new threadid we are creating), see if it is lastpostid/mostrecentpostid anywhere (thread, forums), check thread status
                            Dim PostIDToSplit As Integer = CInt(dlPostsForThread.DataKeys(item.ItemIndex))
                            If (Not PostIDToSplit = PostID) And (Not PostIDToSplit = ThreadID) Then
                                ctlPosts.PostMove(PostIDToSplit, ThreadID, PostID, newForumID, ForumID, UserId, newThreadSortOrder, Notes, objForum.ParentID)
                                Forum.Components.Utilities.Caching.UpdatePostCache(PostIDToSplit)
                            End If

                            newThreadSortOrder += 1
                        Else
                            ' make sure the parent post of this is an existing post that will remain in the thread. (Set to current threadID) 
                            Dim PostIDToSplit As Integer = CInt(dlPostsForThread.DataKeys(item.ItemIndex))
                            If (Not PostIDToSplit = PostID) And (Not PostIDToSplit = ThreadID) Then
                                ctlPosts.PostMove(PostIDToSplit, ThreadID, ThreadID, newForumID, ForumID, UserId, oldThreadSortOrder, Notes, objForum.ParentID)
                                Forum.Components.Utilities.Caching.UpdatePostCache(PostIDToSplit)
                            End If

                            oldThreadSortOrder += 1
                        End If
                    Next

                    ' reset cache of both threads, we don't do this above in each post so it is only called once per thread. 
                    Forum.Components.Utilities.Caching.UpdateThreadCache(ThreadID, newForumID, objForum.GroupID, ModuleId, objForum.ParentID)
                    Forum.Components.Utilities.Caching.UpdateThreadCache(PostID, ForumID, objForum.GroupID, ModuleId, objForum.ParentID)

                    ' Handle sending emails 
                    If chkEmailUsers.Checked And objConfig.MailNotification Then
                        Dim pCont As PostController
                        pCont = New PostController()
                        Dim pInfo As PostInfo
                        pInfo = pCont.GetPostInfo(PostID, PortalId)
                        Dim tCont As ThreadController
                        tCont = New ThreadController()
                        Dim tInfo As ThreadInfo
                        tInfo = tCont.GetThread(pInfo.ThreadID)

                        ' return to new forum page
                        Dim strURL As String = Utilities.Links.ContainerViewThreadLink(PortalId, TabId, newForumID, pInfo.ThreadID, tInfo.Subject)
                        Utilities.ForumUtils.SendForumMail(PostID, strURL, ForumEmailType.UserThreadSplit, Notes, objConfig, MyProfileUrl, PortalId)
                        ' we have several scenarios: post approved - send mod emails of split and users email of split
                        ' Post Not Approved - send mods email of split, user email of approved
                        ' If the post wasn't approved, just send approval notic
                    End If

                    Dim objPost As PostInfo
                    Dim PostCnt As New PostController
                    objPost = PostCnt.GetPostInfo(PostID, PortalId)

                    Response.Redirect(GetReturnURL(objPost), False)
                End If
            Catch ex As Exception
                LogException(ex)
            End Try
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Used to select the user's default forum in the tree (if applicable)
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Sub SelectDefaultForumTree()
            Dim node As Telerik.Web.UI.RadTreeNode = rtvForums.FindNodeByValue("F" + ForumID.ToString)
            node.Selected = True
            node.ParentNode.Expanded = True
            node.Checked = True
        End Sub

        ''' <summary>
        ''' Gets the URL to return the user too.
        ''' </summary>
        ''' <param name="Post"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetReturnURL(ByVal Post As PostInfo) As String
            Dim url As String
            Dim ModeratedReturn As Boolean = False

            ' This only needs to be handled for moderators (which are trusted by no matter what, so it only happens at this point)
            Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, Post.ForumID, UserId)

            If Security.IsForumModerator Then
                ' Check for querystring parameter to make sure this is where they came from.  Even if a direct link was typed in (should not happen) no damage will be done
                If (Not Request.QueryString("moderatorreturn") Is Nothing) Then
                    If Request.QueryString("moderatorreturn") = "1" Then
                        ModeratedReturn = True
                    End If
                End If
            End If

            If ModeratedReturn Then
                If Not ViewState("UrlReferrer") Is Nothing Then
                    url = (CType(ViewState("UrlReferrer"), String))
                Else
                    ' behave as before (normal usage)
                    url = Utilities.Links.ContainerViewPostLink(PortalId, TabId, Post.ForumID, Post.PostID, Post.Subject)
                End If
            Else
                ' behave as before (normal usage)
                url = Utilities.Links.ContainerViewPostLink(PortalId, TabId, Post.ForumID, Post.PostID, Post.Subject)
            End If

            Return url
        End Function

#End Region

#Region "Protected Functions"

        ''' <summary>
        ''' Determines if a checkbox in the datalist should be shown for a specific post
        ''' </summary>
        ''' <param name="CurrentPostID"></param>
        ''' <param name="ThreadID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function EnabledSelector(ByVal CurrentPostID As Integer, ByVal ThreadID As Integer) As Boolean
            ' if the current post is the first post in the old thread
            If PostID = ThreadID Then
                Return False
            Else
                ' see if the post here is the post we are creating a new thread from
                If CurrentPostID = PostID Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Function

        ''' <summary>
        ''' Determines if the postmoderate image in the datalist should show for a specific post
        ''' </summary>
        ''' <param name="IsApproved"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function VisibleNeedsModImage(ByVal IsApproved As Boolean) As String
            If IsApproved = True Then
                Return objConfig.GetThemeImageURL("spacer.gif")
            Else
                Return objConfig.GetThemeImageURL("ma_moderate." & objConfig.ImageExtension)
            End If
        End Function

        ''' <summary>
        ''' Determines if the PostReported icon in the datalist should be displayed for a specific post
        ''' </summary>
        ''' <param name="PostReported"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function VisiblePostReported(ByVal PostReported As Integer) As String
            If PostReported > 0 Then
                Return objConfig.GetThemeImageURL("s_postabuse." & objConfig.ImageExtension)
            Else
                Return objConfig.GetThemeImageURL("spacer.gif")
            End If
        End Function

        ''' <summary>
        ''' Sets the total post count for a specific UserID
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function PostCount(ByVal UserID As Integer) As String
            Dim objUser As ForumUserInfo
            Dim cntForumUser As New ForumUserController

            objUser = cntForumUser.GetForumUser(UserID, False, ModuleId, PortalId)

            Return objUser.PostCount.ToString
        End Function

        ''' <summary>
        ''' Sets the SiteAlias for a specific UserID
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function UserAlias(ByVal UserID As Integer) As String
            Dim objUser As ForumUserInfo
            Dim cntForumUser As New ForumUserController

            objUser = cntForumUser.GetForumUser(UserID, False, ModuleId, PortalId)

            Return objUser.SiteAlias
        End Function

        ''' <summary>
        ''' Sets the height spacer image. 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function SpacerImage() As String
            Return objConfig.GetThemeImageURL("alt_headfoot_height.gif")
        End Function

        ''' <summary>
        ''' Sets the litle spacer image used throughout.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function LilSpacerImage() As String
            Return objConfig.GetThemeImageURL("spacer.gif")
        End Function

        ''' <summary>
        ''' Formats the posts body, including emoticons and quote but doesn't include user's signature. 
        ''' </summary>
        ''' <param name="PostID">An integer representing which post we are going to process the body of. </param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' This is mainly pulled from Posts.vb's RenderPostBody method.
        ''' </history>
        Protected Function FormatBody(ByVal PostID As Integer) As String
            Dim strFormatedBody As String = String.Empty
            Dim objPost As New PostInfo
            Dim cntPost As New PostController

            objPost = cntPost.GetPostInfo(PostID, PortalId)


            If Not objPost Is Nothing Then
                Dim bodyForumText As Utilities.PostContent

                If objPost.ParseInfo = PostParserInfo.None Or objPost.ParseInfo = PostParserInfo.File Then
                    'Nothing to Parse or just an Attachment not inline
                    bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objPost.Body), objConfig)
                Else
                    If objPost.ParseInfo < PostParserInfo.Inline Then
                        'Something to parse, but not any inline instances
                        bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objPost.Body), objConfig, objPost.ParseInfo)
                    Else
                        'At lease Inline to Parse
                        If Users.UserController.Instance.GetCurrentUserInfo.UserID > 0 Then
                            bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objPost.Body), objConfig, objPost.ParseInfo, objPost.AttachmentCollection(objConfig.EnableAttachment), True)
                        Else
                            bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objPost.Body), objConfig, objPost.ParseInfo, objPost.AttachmentCollection(objConfig.EnableAttachment), False)
                        End If
                    End If
                End If


                strFormatedBody = bodyForumText.ProcessHtml
            End If
            Return strFormatedBody
        End Function

        ''' <summary>
        ''' Formats the date a post was posted
        ''' </summary>
        ''' <param name="CreatedDate"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	1/9/2006	Created
        ''' </history>
        Protected Function FormatCreatedDate(ByVal CreatedDate As Object) As String
            Dim strCreatedDate As String = String.Empty
            If Not IsDBNull(CreatedDate) Then
                Dim displayCreatedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(CType(CreatedDate, DateTime), objConfig)
                strCreatedDate = displayCreatedDate.ToString
            End If

            Return strCreatedDate

        End Function

        ''' <summary>
        ''' Formats the users joined date according to TimeZone
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function FormatJoinedDate(ByVal UserID As Integer) As String
            Dim strCreatedDate As String = String.Empty
            Dim objUser As ForumUserInfo
            Dim cntForumUser As New ForumUserController

            objUser = cntForumUser.GetForumUser(UserID, False, ModuleId, PortalId)

            Dim displayCreatedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(CType(objUser.UserJoinedDate, DateTime), objConfig)
            strCreatedDate = displayCreatedDate.ToShortDateString

            Return strCreatedDate
        End Function

        ''' <summary>
        ''' This generates the url string for a link to the user's profile (author)
        ''' </summary>
        ''' <param name="UserId"></param>
        ''' <returns>String</returns>
        ''' <remarks>
        ''' </remarks>
        Protected Function UserProfileLink(ByVal UserId As Integer) As String
            Dim params As String()
            Dim url As String

            params = New String(1) {"mid=" & ModuleId.ToString, "userid=" & UserId.ToString}
            url = NavigateURL(TabId, "UserProfile", params)

            Return url
        End Function

#End Region

    End Class

End Namespace