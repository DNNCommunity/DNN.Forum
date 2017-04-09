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

Imports DotNetNuke.Entities.Content
Imports DotNetNuke.Modules.Forum.Utilities

Namespace DotNetNuke.Modules.Forum

    ''' <summary>
    ''' All post deletes are done from this page. 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public MustInherit Class PostDelete
        Inherits ForumModuleBase
        Implements Entities.Modules.IActionable

#Region "Private Members"

        Private _ModeratorReturn As Boolean = False
        Private _IsThreadDelete As Boolean = False

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

#Region "Event Handlers"

        ''' <summary>
        ''' Sets the forum configuration for user in the UI and other areas.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            If Not Request.QueryString("threadid") Is Nothing Then
                Me.ModuleConfiguration.ModuleTitle = Localization.GetString("ModuleTitle", Me.LocalResourceFile)
                divEmailUsers.Visible = False
            End If
        End Sub

        ''' <summary>
        ''' Loads the settings for the page
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Page.IsPostBack = False Then
                    If Not Request.UrlReferrer Is Nothing Then
                        cmdCancel.NavigateUrl = Request.UrlReferrer.ToString()
                    Else
                        cmdCancel.NavigateUrl = NavigateURL()
                    End If

                    If Not Request.UrlReferrer Is Nothing Then
                        ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
                    End If
                End If

                Dim PostID As Integer
                If Not Request.QueryString("postid") Is Nothing Then
                    Dim objPost As New PostInfo
                    Dim cntPost As New PostController()

                    PostID = Int32.Parse(Request.QueryString("postid"))
                    objPost = cntPost.GetPostInfo(PostID, PortalId)

                    If objPost Is Nothing Then
                        HttpContext.Current.Response.Redirect(GetReturnURL(-1, ForumID), True)
                    Else
                        _IsThreadDelete = False
                    End If
                Else
                    If Not Request.QueryString("threadid") Is Nothing Then
                        Dim ThreadID As Integer
                        Dim objThread As New ThreadInfo

                        ThreadID = Int32.Parse(Request.QueryString("threadid"))
                        Dim cntThread As New ThreadController()
                        objThread = cntThread.GetThread(ThreadID)

                        If objThread Is Nothing Then
                            HttpContext.Current.Response.Redirect(GetReturnURL(-1, ForumID), True)
                        Else
                            PostID = ThreadID
                            _IsThreadDelete = True
                        End If
                    End If
                End If

                Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
                ForumUtils.LoadCssFile(DefaultPage, objConfig)

                If ForumID > 0 Then
                    Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, ForumID, UserId)

                    If Request.IsAuthenticated Then
                        If Not (Security.IsForumModerator) Then
                            ' they don't belong here
                            HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                        End If
                    Else
                        ' they don't belong here
                        HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                    End If

                    If Not Page.IsPostBack Then
                        PopulatePost(PostID, _IsThreadDelete)
                        PopulateTemplateDDL()

                        If _IsThreadDelete Then
                            cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteThread", Me.LocalResourceFile) & "');")
                        Else
                            cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
                        End If
                    End If
                Else
                    HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' Deletes a post from the database
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
            Try
                If Page.IsValid Then
                    If _IsThreadDelete Then
                        Dim cntThread As New ThreadController
                        Dim objThread As New ThreadInfo

                        ' Get most recent info on this thread
                        If Not Request.QueryString("threadid") Is Nothing Then
                            Dim ThreadID As Integer

                            ThreadID = Int32.Parse(Request.QueryString("threadid"))
                            objThread = cntThread.GetThread(ThreadID)

                            If objThread Is Nothing Then
                                Response.Redirect(GetReturnURL(-1, ForumID), True)
                            End If
                        Else
                            Exit Sub
                        End If

                        Dim cntForum As ForumController
                        cntForum = New ForumController()
                        Dim objForum As ForumInfo = cntForum.GetForumItemCache(ForumID)

                        Dim Notes As String = txtReason.Text
                        Dim ProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)
                        Dim url As String = Utilities.Links.ContainerViewForumLink(PortalId, TabId, ForumID, False, objForum.Name)

                        If objConfig.MailNotification Then
                            If chkEmailUsers.Checked Then
                                ' first send notice to the user
                                ' CP - NOTE: We need to add a user thread deleted template and handle in parsing
                                Utilities.ForumUtils.SendForumMail(objThread.ThreadID, url, ForumEmailType.UserPostDeleted, Notes, objConfig, ProfileUrl, PortalId)
                            End If
                            ' now send notice to the forum moderators
                            Utilities.ForumUtils.SendForumMail(objThread.ThreadID, url, ForumEmailType.ModeratorPostDeleted, Notes, objConfig, ProfileUrl, PortalId)
                        End If

                        ' Delete thread (SEND MAIL BEFORE DELETE, we need the thread still in the db)
                        cntThread.DeleteThread(objThread, PortalId, Notes)

                        ' have to handle that the post we just removed deleted the entire thread
                        Response.Redirect(GetReturnURL(-1, ForumID), True)
                    Else
                        Dim cntPost As New PostController
                        Dim objPost As New PostInfo
                        Dim ThreadID As Integer
                        Dim cntThread As New ThreadController
                        Dim objThread As New ThreadInfo

                        ' Get most recent info on this post
                        If Not Request.QueryString("postid") Is Nothing Then
                            Dim PostID As Integer

                            PostID = Int32.Parse(Request.QueryString("postid"))
                            objPost = cntPost.GetPostInfo(PostID, PortalId)

                            If objPost Is Nothing Then
                                Response.Redirect(GetReturnURL(-1, ForumID), True)
                            Else
                                ThreadID = objPost.ThreadID
                            End If
                        Else
                            Exit Sub
                        End If

                        Dim cntForum As ForumController
                        cntForum = New ForumController()
                        Dim objForum As ForumInfo = cntForum.GetForumItemCache(ForumID)
                        objThread = cntThread.GetThread(ThreadID)

                        Dim Notes As String = txtReason.Text
                        Dim ProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)
                        Dim url As String = Utilities.Links.ContainerViewForumLink(PortalId, TabId, ForumID, False, objForum.Name)
                        Dim NewThreadID As Integer = ThreadID

                        If objConfig.MailNotification Then
                            If chkEmailUsers.Checked Then
                                ' first send notice to the user
                                Utilities.ForumUtils.SendForumMail(objPost.PostID, url, ForumEmailType.UserPostDeleted, Notes, objConfig, ProfileUrl, PortalId)
                            End If
                            ' now send notice to the forum moderators
                            Utilities.ForumUtils.SendForumMail(objPost.PostID, url, ForumEmailType.ModeratorPostDeleted, Notes, objConfig, ProfileUrl, PortalId)
                        End If

                        If objPost.PostID = objPost.ThreadID Then
                            If objThread IsNot Nothing Then
                                ' Delete post (SEND MAIL BEFORE DELETE, we need the post still in the db)
                                NewThreadID = cntPost.PostDelete(objPost.PostID, UserId, Notes, PortalId, objThread.ForumID, objThread.ModuleID, objThread.ThreadID, objPost.Author.UserID)
                                Dim objContent As ContentItem
                                objContent = DotNetNuke.Entities.Content.Common.Util.GetContentController().GetContentItem(objThread.ContentItemId)
                                objContent.ContentKey = "forumid=" + objThread.ForumID.ToString() + "&threadid=" + NewThreadID.ToString() + "&scope=posts"
                                DotNetNuke.Entities.Content.Common.Util.GetContentController().UpdateContentItem(objContent)
                            End If
                        Else
                            ' Delete post (SEND MAIL BEFORE DELETE, we need the post still in the db)
                            cntPost.PostDelete(objPost.PostID, UserId, Notes, PortalId, objThread.ForumID, objThread.ModuleID, objThread.ThreadID, objPost.Author.UserID)
                        End If

                        Forum.Components.Utilities.Caching.UpdatePostCache(objPost.PostID, ThreadID, ForumID, objThread.ContainingForum.GroupID, ModuleId, objThread.ContainingForum.ParentID)
                        Response.Redirect(GetReturnURL(NewThreadID, ForumID), True)
                    End If
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' Sets an email template for a delete reason
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        Protected Sub ddlDeleteTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlDeleteTemplate.SelectedIndexChanged
            BindDeleteTemplateBody()
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Gets the post content of the one the user wishes to delete
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Sub PopulatePost(ByVal PostID As Integer, ByVal ThreadDelete As Boolean)
            Dim cntPost As New PostController()
            Dim objPost As New PostInfo

            objPost = cntPost.GetPostInfo(PostID, PortalId)

            Dim fTextDecode As Utilities.PostContent = New Utilities.PostContent(Server.HtmlDecode(objPost.Body), objConfig)
            lblBody.Text = fTextDecode.ProcessHtml
            lblSubject.Text = objPost.Subject
            lblAuthor.Text = String.Format(Localization.GetString("PostedBy.Text", Me.LocalResourceFile), objPost.Author.Username, objPost.CreatedDate.ToString)
        End Sub

        ''' <summary>
        ''' Populates a list of available templates, if this is the first time it is run it gets defaults and creates available templates for this portal.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateTemplateDDL()
            Dim arrTemplates As ArrayList
            Dim objTempCnt As New ForumTemplateController

            'CP - COMEBACK: Could separate thread delete items from post delete items
            arrTemplates = objTempCnt.TemplatesGetByType(ModuleId, ForumTemplateTypes.DeletePost)

            If arrTemplates.Count > 0 Then
                BindTemplateList(arrTemplates)
            Else
                Dim objTemplateInfo As New TemplateInfo
                Dim arrDefaultTemplates As ArrayList
                'Get Default templates
                arrDefaultTemplates = objTempCnt.TemplatesGetDefaults(ForumTemplateTypes.DeletePost)

                If arrDefaultTemplates.Count > 0 Then
                    ' for each default template, create one specific to this module
                    For Each objTemplateInfo In arrDefaultTemplates
                        Dim NewTemplateInfo As New TemplateInfo

                        NewTemplateInfo.TemplateName = objTemplateInfo.TemplateName
                        NewTemplateInfo.TemplateValue = objTemplateInfo.TemplateValue
                        NewTemplateInfo.ForumTemplateTypeID = objTemplateInfo.ForumTemplateTypeID
                        NewTemplateInfo.ModuleID = ModuleId
                        NewTemplateInfo.IsActive = objTemplateInfo.IsActive

                        objTempCnt.TemplatesAddForModuleID(NewTemplateInfo)
                    Next

                    ' We now have to bind the new templates to the ddl
                    arrTemplates = objTempCnt.TemplatesGetByType(ModuleId, ForumTemplateTypes.DeletePost)
                    BindTemplateList(arrTemplates)
                End If
            End If
            BindDeleteTemplateBody()
        End Sub

        ''' <summary>
        ''' Does the actual binding of a list of available email templates for editing
        ''' </summary>
        ''' <param name="arrTemplates"></param>
        ''' <remarks></remarks>
        Private Sub BindTemplateList(ByVal arrTemplates As ArrayList)
            For Each objForumTemplate As TemplateInfo In arrTemplates
                Dim AvailableTemplates As New ListItem(Localization.GetString(objForumTemplate.TemplateName, objConfig.SharedResourceFile), objForumTemplate.TemplateID.ToString)
                ddlDeleteTemplate.Items.Add(AvailableTemplates)
            Next
        End Sub

        ''' <summary>
        ''' Binds a single email template to the textbox for the moderator to edit before sending
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindDeleteTemplateBody()
            ' get the single template
            Dim objTempCnt As New ForumTemplateController
            Dim objTemplate As TemplateInfo
            objTemplate = objTempCnt.TemplatesGetSingle(CType(ddlDeleteTemplate.SelectedValue, Integer))

            Dim Reason As String = objTemplate.TemplateValue
            txtReason.Text = Reason
        End Sub

        ''' <summary>
        ''' Sets the return URL after the user has completed their action.
        ''' </summary>
        ''' <param name="ThreadID"></param>
        ''' <param name="ForumID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetReturnURL(ByVal ThreadID As Integer, ByVal ForumID As Integer) As String
            Dim url As String

            ' This only needs to be handled for moderators (which are trusted by no matter what, so it only happens at this point)
            Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, ForumID, UserId)

            If Security.IsForumModerator Then
                ' Check for querystring parameter to make sure this is where they came from.  Even if a direct link was typed in (should not happen) no damage will be done
                If (Not Request.QueryString("moderatorreturn") Is Nothing) Then
                    If Request.QueryString("moderatorreturn") = "1" Then
                        _ModeratorReturn = True
                    End If
                End If
            End If

            Dim cntForum As ForumController
            cntForum = New ForumController()
            Dim objForum As ForumInfo = cntForum.GetForumItemCache(ForumID)
            If _ModeratorReturn Then
                If Not ViewState("UrlReferrer") Is Nothing Then
                    url = (CType(ViewState("UrlReferrer"), String))
                Else
                    If _IsThreadDelete Then
                        url = Utilities.Links.ContainerViewForumLink(PortalId, TabId, ForumID, False, objForum.Name)
                    Else
                        ' behave as before (normal usage)
                        url = Utilities.Links.ContainerViewThreadLink(TabId, ForumID, ThreadID)
                    End If
                End If
            Else
                If _IsThreadDelete Or ThreadID = -1 Then
                    url = Utilities.Links.ContainerViewForumLink(PortalId, TabId, ForumID, False, objForum.Name)
                Else
                    ' behave as before (normal usage)
                    url = Utilities.Links.ContainerViewThreadLink(TabId, ForumID, ThreadID)
                End If
            End If

            Return url
        End Function

#End Region

    End Class

End Namespace