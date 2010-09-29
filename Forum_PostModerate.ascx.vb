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

Imports DotNetNuke.Modules.Forum.Utilities

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' This is the moderation queue page for a specific Forum. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public MustInherit Class PostModerate
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Private mForumID As Integer
		Private mForumInfo As ForumInfo

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
		''' Loads up the page
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				If Not Request.QueryString("forumid") Is Nothing Then
					Dim cntForum As New ForumController

					mForumID = Int32.Parse(Request.QueryString("forumid"))
					mForumInfo = cntForum.GetForumItemCache(mForumID)
				End If

				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, mForumID, UserId)

				' (leave even if permitting anonymous posting)
				If Request.IsAuthenticated Then
					If Not (Security.IsForumModerator) Then
						' they don't belong here
						HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
					End If
				Else
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
				ForumUtils.LoadCssFile(DefaultPage, objConfig)

				If Not Page.IsPostBack Then
					hlForum.Text = mForumInfo.Name
					hlForum.NavigateUrl = Utilities.Links.ContainerViewForumLink(TabId, mForumID, False)

					imgHeadSpacer2.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
					imgHeadSpacer.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")

					BindList()
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Assigns values to the datalist control items. 
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub lstPost_Select(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles lstPost.ItemCommand
			Try
				' Determine the command of the button             
				Dim cmdButton As LinkButton = DirectCast(e.CommandSource, LinkButton)
				Dim postID As Integer = Int32.Parse(cmdButton.CommandArgument)
				Dim selListItem As DataListItem = DirectCast(e.Item, DataListItem)
				Dim bodyPanel As Panel = DirectCast(selListItem.FindControl("pnlBody"), Panel)
				'Dim ProfileUrl As String = Utils.MySettingsLink(TabId, ModuleId)
				Dim ProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)
				Dim cntPost As New PostController()
				Dim objPost As PostInfo = cntPost.GetPostInfo(postID, PortalId)

				Select Case cmdButton.CommandName.ToLower
					Case "approve"
						Dim _notes As String = "Approved"
						Dim _mailURL As String = Utilities.Links.ContainerViewPostLink(TabId, objPost.ForumID, objPost.PostID)

						ApprovePost(postID, CurrentForumUser.UserID, _notes, _mailURL, ProfileUrl, objPost.ForumID, objPost.ThreadID)

						If (objPost.Author.IsTrusted = False) AndAlso (objConfig.EnableAutoTrust) Then
							If objConfig.AutoTrustTime = objPost.Author.PostCount + 1 Then
								' we need to update the user's trust status
								Dim cntForumUser As New ForumUserController
								Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)
								ProfileUser.IsTrusted = True
								cntForumUser.Update(ProfileUser)
							Else
								DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(objPost.Author.UserID, PortalId)
							End If
						Else
							DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(objPost.Author.UserID, PortalId)
						End If

						' Rebind latest non-approved posts to datalist
						BindList()
					Case "move"
						' We have to approve the post before moving the thread, this decreases post count and also avoids this being stuck in moderation if the user cancels out (no email sent 4 this)
						ApproveMovePost(objPost.PostID, CurrentForumUser.UserID, "move", objPost.ForumID, objPost.ThreadID)

						'"moderatorreturn=1"
						Dim url As String = Utilities.Links.ThreadMoveLink(TabId, ModuleId, objPost.ForumID, objPost.ThreadID)
						' Still have to handle approval and emailing in new page
						Response.Redirect(url, False)
					Case "split"
						' in the case of split, we are not going to adjust moderation counts at all, as split sproc handles this for us
						Dim url As String = Utilities.Links.ThreadSplitLink(TabId, ModuleId, objPost.ForumID, objPost.PostID)

						' Still have to handle approval and emailing in new page
						Response.Redirect(url, False)
					Case "delete"
						Dim _nextURL As String = Utilities.Links.PostDeleteLink(TabId, ModuleId, objPost.ForumID, objPost.PostID, True)

						'"moderatorreturn=1"
						Response.Redirect(_nextURL, False)
					Case "edit"
						Dim _notes As String = "Approved with edit"
						Dim _mailURL As String = Utilities.Links.ContainerViewPostLink(TabId, objPost.ForumID, objPost.PostID)

						ApprovePost(postID, CurrentForumUser.UserID, _notes, _mailURL, ProfileUrl, objPost.ForumID, objPost.ThreadID)
						DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(objPost.Author.UserID, PortalId)

						'"moderatorreturn=1"
						Dim url As String = Utilities.Links.NewPostLink(TabId, objPost.ForumID, objPost.PostID, "edit", ModuleId)

						' Still have to handle approval and emailing in new page
						Response.Redirect(url, False)
					Case "approverespond"
						Dim _notes As String = "Approved and respond"
						Dim _mailURL As String = Utilities.Links.ContainerViewPostLink(TabId, objPost.ForumID, objPost.PostID)

						ApprovePost(postID, CurrentForumUser.UserID, _notes, _mailURL, ProfileUrl, objPost.ForumID, objPost.ThreadID)
						DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(objPost.Author.UserID, PortalId)

						' "moderatorreturn=1"
						Dim url As String = Utilities.Links.NewPostLink(TabId, objPost.ForumID, objPost.PostID, "reply", ModuleId)

						Response.Redirect(url, False)
				End Select
			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Sets items in the datalist for the post list.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub lstPost_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles lstPost.ItemDataBound
			Dim item As System.Web.UI.WebControls.DataListItem = e.Item

			If item.ItemType = System.Web.UI.WebControls.ListItemType.Item Or _
			    item.ItemType = System.Web.UI.WebControls.ListItemType.AlternatingItem Or _
			    item.ItemType = System.Web.UI.WebControls.ListItemType.SelectedItem Then
				Dim dataItem As PostInfo = CType(e.Item.DataItem, PostInfo)
				Dim lbl As System.Web.UI.WebControls.Label
				Dim rblst As System.Web.UI.WebControls.RadioButtonList

				Dim intPollID As Integer
				intPollID = dataItem.ParentThread.PollID

				If intPollID > 0 Then
					Dim cntPoll As New PollController
					Dim objPoll As New PollInfo
					objPoll = cntPoll.GetPoll(intPollID)

					If Not objPoll Is Nothing Then
						lbl = CType(e.Item.FindControl("lblQuestion"), System.Web.UI.WebControls.Label)
						lbl.Text = objPoll.Question

						rblst = CType(e.Item.FindControl("rblstAnswers"), System.Web.UI.WebControls.RadioButtonList)
						rblst.DataTextField = "Answer"
						rblst.DataValueField = "AnswerID"
						rblst.DataSource = objPoll.Answers
						rblst.DataBind()

						lbl = CType(e.Item.FindControl("lblEndDate"), System.Web.UI.WebControls.Label)
						lbl.Text = objPoll.EndDate.ToShortDateString()

						lbl = CType(e.Item.FindControl("lblTakenMessage"), System.Web.UI.WebControls.Label)
						lbl.Text = objPoll.TakenMessage
					End If
				End If
			End If
		End Sub

		''' <summary>
		''' Take the user back to where they came from
		''' </summary>
		''' <param name="sender">System.Object</param>
		''' <param name="e">System.EventArgs</param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBack.Click
			Dim url As String
			url = Utilities.Links.MCPControlLink(TabId, ModuleId, ModeratorAjaxControl.ModQueue)
			Response.Redirect(url, False)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds a list of the latests unapproved posts for a specific forum to the
		''' datalist on the page.
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub BindList()
			Dim ctlModerate As New Forum.PostModerationController
			Dim objPosts As New List(Of PostInfo)
			objPosts = ctlModerate.ModeratePostGet(mForumID)

			If objPosts.Count > 0 Then
				lstPost.DataSource = objPosts
				lstPost.DataBind()
			Else
				' There are no posts in this forum to moderate 
				' we need to make sure the forum is cleared out (set to 0)
				Dim url As String
				url = Utilities.Links.MCPControlLink(TabId, ModuleId, ModeratorAjaxControl.ModQueue)
				Response.Redirect(url, False)
			End If
		End Sub

		''' <summary>
		''' Approves a post from moderation queue and fires off emails to those subscribed
		''' and the author (saying post is approved)
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="UserID"></param>
		''' <param name="Notes"></param>
		''' <param name="URL"></param>
		''' <param name="ForumID"></param>
		''' <remarks>
		''' </remarks>
		Private Sub ApprovePost(ByVal PostID As Integer, ByVal UserID As Integer, ByVal Notes As String, ByVal URL As String, ByVal ProfileURL As String, ByVal ForumID As Integer, ByVal ThreadID As Integer)
			Dim ctlForum As New ForumController
			Dim forum As ForumInfo = ctlForum.GetForumItemCache(ForumID)
			Dim ctlForumModerate As New PostModerationController
			ctlForumModerate.ModeratePostApprove(PostID, UserID, Notes, ForumID, forum.ParentID, ThreadID, ModuleId)

			If objConfig.MailNotification Then
				' send notification mail to author
				Utilities.ForumUtils.SendForumMail(PostID, URL, ForumEmailType.UserPostApproved, Notes, objConfig, ProfileURL, PortalId)
				' send notification mail to subscribe
				Utilities.ForumUtils.SendForumMail(PostID, URL, ForumEmailType.UserPostAdded, Notes, objConfig, ProfileURL, PortalId)
			End If
		End Sub

		''' <summary>
		''' This decreases post count and also avoids this being stuck in moderation if the user cancels out (no email sent 4 this)
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="UserID"></param>
		''' <param name="Notes"></param>
		''' <param name="ForumID"></param>
		''' <remarks>
		''' </remarks>
		Private Sub ApproveMovePost(ByVal PostID As Integer, ByVal UserID As Integer, ByVal Notes As String, ByVal ForumID As Integer, ByVal ThreadID As Integer)
			Dim ctlForum As New ForumController
			Dim forum As ForumInfo = ctlForum.GetForumItemCache(ForumID)
			Dim ctlForumModerate As New PostModerationController
			ctlForumModerate.ModeratePostApprove(PostID, UserID, Notes, ForumID, forum.ParentID, ThreadID, ModuleId)
		End Sub

#End Region

#Region "Protected Functions"

		''' <summary>
		''' This generates the url string for a link to the user's profile (author)
		''' </summary>
		''' <param name="UserId"></param>
		''' <returns>String</returns>
		''' <remarks>
		''' </remarks>
		Protected Function UserProfileLink(ByVal UserId As Integer) As String
			Dim url As String
			Dim cntForumUser As New ForumUserController

			Dim objUser As ForumUserInfo = cntForumUser.GetForumUser(UserId, False, ModuleId, PortalId)
			If Not objConfig.EnableExternalProfile Then
				url = objUser.UserCoreProfileLink
			Else
				url = Utilities.Links.UserExternalProfileLink(UserId, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objUser.Username)
			End If

			Return url
		End Function

		''' <summary>
		''' Determines if a single post can be moved (only can move first post of 
		''' a thread)
		''' </summary>
		''' <param name="PostID"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Protected Function ThreadCanMove(ByVal PostID As Integer) As Boolean
			Dim cntPost As New PostController()
			Dim objPost As PostInfo = cntPost.GetPostInfo(PostID, PortalId)

			If objPost.ParentPostID = 0 Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' Determines if a single post can be split into a new thread (cannot split original post of a thread)
		''' </summary>
		''' <param name="PostID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function ThreadCanSplit(ByVal PostID As Integer) As Boolean
			Dim cntPost As New PostController()
			Dim objPost As PostInfo = cntPost.GetPostInfo(PostID, PortalId)

			If objPost.ParentPostID = 0 Then
				Return False
			Else
				Return True
			End If
		End Function

		''' <summary>
		''' Formats the posts boyd
		''' </summary>
		''' <param name="Body"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Protected Function FormatBody(ByVal Body As Object) As String
			Dim strFormatedBody As String = String.Empty
			If Not IsDBNull(Body) Then
				Dim bodyForumText As Utilities.PostContent = New Utilities.PostContent(Server.HtmlDecode(CType(Body, String)), objConfig)
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
		Protected Function FormatCreatedDate(ByVal CreatedDate As Object) As String
			Dim strCreatedDate As String = String.Empty
			If Not IsDBNull(CreatedDate) Then
				Dim displayCreatedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(CType(CreatedDate, DateTime), objConfig)
				strCreatedDate = displayCreatedDate.ToString
			End If

			Return strCreatedDate
		End Function

		''' <summary>
		''' Formats the user's alias for display.
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function FormatUserAlias(ByVal UserID As Integer) As String
			Dim objUser As ForumUserInfo
			Dim cntForumUser As New ForumUserController

			objUser = cntForumUser.GetForumUser(UserID, False, ModuleId, PortalId)

			Return objUser.SiteAlias
		End Function

		''' <summary>
		''' Formats the users post count and localizes the content.
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function FormatUserPostCount(ByVal UserID As Integer) As String
			Dim objUser As ForumUserInfo
			Dim cntForumUser As New ForumUserController

			objUser = cntForumUser.GetForumUser(UserID, False, ModuleId, PortalId)

			Return objUser.PostCount.ToString & " " & Localization.GetString("Posts", LocalResourceFile)
		End Function

		''' <summary>
		''' Formats the date an author joined the community
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Protected Function FormatJoinedDate(ByVal UserID As Integer) As String
			Dim strCreatedDate As String = String.Empty
			Dim objUser As ForumUserInfo
			Dim cntForumUser As New ForumUserController

			objUser = cntForumUser.GetForumUser(UserID, False, ModuleId, PortalId)

			Dim displayCreatedDate As DateTime
			displayCreatedDate = Utilities.ForumUtils.ConvertTimeZone(objUser.Membership.CreatedDate, objConfig)
			strCreatedDate = displayCreatedDate.ToShortDateString

			Return strCreatedDate
		End Function

		''' <summary>
		''' Provides a link to the user control panel's profile section for moderators. 
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function EditProfileLink(ByVal UserID As Integer) As String
			Return Utilities.Links.UCP_AdminLinks(TabId, ModuleId, UserID, UserAjaxControl.Profile)
		End Function

		''' <summary>
		''' Turns link visibility on/off - deprecated
		''' </summary>
		''' <param name="ParentPostId"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Protected Function VisibleLink(ByVal ParentPostId As Integer) As Boolean
			If ParentPostId > 0 Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' Returns a string link which is used to navigate to the posts original thread
		''' if this post is not the first of a new thread.  If it is, no link is returned
		''' </summary>
		''' <param name="ThreadId">Integer</param>
		''' <param name="ForumID">Integer</param>
		''' <param name="ParentPostId">Integer</param>
		''' <returns>String</returns>
		''' <remarks>
		''' </remarks>
		Protected Function ThreadLink(ByVal ThreadId As Integer, ByVal ForumID As Integer, ByVal ParentPostId As Integer) As String
			If ParentPostId > 0 Then
				Return Utilities.Links.ContainerViewThreadLink(TabId, ForumID, ThreadId)
			Else : Return String.Empty
			End If
		End Function

		''' <summary>
		''' Sets the spacer image used in the footer/header areas. 
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function SpacerImage() As String
			Return objConfig.GetThemeImageURL("alt_headfoot_height.gif")
		End Function

		''' <summary>
		''' Sets the image path for the edit profile image.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function EditImage() As String
			Return objConfig.GetThemeImageURL("s_edit." + objConfig.ImageExtension)
		End Function

#End Region

	End Class

End Namespace