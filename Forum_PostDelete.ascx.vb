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
			' Ajax
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdCancel)
			End If

			If Not Request.QueryString("threadid") Is Nothing Then
				Me.ModuleConfiguration.ModuleTitle = Localization.GetString("ModuleTitle", Me.LocalResourceFile)
				rowEmailUsers.Visible = False
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
				Dim PostID As Integer
				Dim ForumID As Integer

				If Not Request.QueryString("postid") Is Nothing Then
					Dim objPost As New PostInfo

					PostID = Int32.Parse(Request.QueryString("postid"))
					objPost = PostInfo.GetPostInfo(PostID, PortalId)
					ForumID = objPost.ForumID
					_IsThreadDelete = False
				Else
					If Not Request.QueryString("threadid") Is Nothing Then
						Dim ThreadID As Integer
						Dim objThread As New ThreadInfo

						ThreadID = Int32.Parse(Request.QueryString("threadid"))
						objThread = ThreadInfo.GetThreadInfo(ThreadID)
						ForumID = objThread.ForumID
						' we will get info for first post, since we are deleting the thread
						PostID = ThreadID
						_IsThreadDelete = True
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

						If Not Request.UrlReferrer Is Nothing Then
							ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
						End If
					End If
				Else
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				imgSpacerL.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				imgSpacerR.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")

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
						Dim ForumID As Integer

						' Get most recent info on this thread
						If Not Request.QueryString("threadid") Is Nothing Then
							Dim ThreadID As Integer

							ThreadID = Int32.Parse(Request.QueryString("threadid"))
							objThread = ThreadInfo.GetThreadInfo(ThreadID)
							ForumID = objThread.ForumID
						Else
							Exit Sub
						End If

						Dim Notes As String = txtReason.Text
						'Dim ProfileUrl As String = Utils.MySettingsLink(TabId, ModuleId)
						Dim ProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)
						Dim url As String = Utilities.Links.ContainerViewForumLink(TabId, ForumID, False)

						If objConfig.MailNotification Then
							'If chkEmailUsers.Checked Then
							'	' first send notice to the user
							'	Utils.SendForumMail(objThread.ThreadID, url, ForumEmailType.UserPostDeleted, Notes, mForumConfig, ProfileUrl, PortalId)
							'End If
							' now send notice to the forum moderators
							Utilities.ForumUtils.SendForumMail(objThread.ThreadID, url, ForumEmailType.ModeratorPostDeleted, Notes, objConfig, ProfileUrl, PortalId)
						End If

						' Delete thread (SEND MAIL BEFORE DELETE, we need the thread still in the db)
						cntThread.ThreadDelete(objThread.ThreadID, PortalId, Notes)

						ThreadInfo.ResetThreadInfo(objThread.ThreadID)
						ForumController.ResetForumInfoCache(objThread.ForumID)

						' We need to clear all users who posted in the thread. (Otherwise cached user objects will not reflect proper post count) KNOWN ISSUE
						'ForumUserController.ResetForumUser(objThread.)

						ThreadController.ResetThreadListCached(objThread.ForumID, objThread.ModuleID)
						If objConfig.AggregatedForums Then
							ThreadController.ResetThreadListCached(-1, objThread.ModuleID)
						End If

						' have to handle that the post we just removed deleted the entire thread
						Response.Redirect(GetReturnURL(-1, ForumID), True)
					Else
						Dim cntPost As New PostController
						Dim objPost As New PostInfo
						Dim ForumID As Integer
						Dim ThreadID As Integer
						Dim AuthorID As Integer

						' Get most recent info on this post
						If Not Request.QueryString("postid") Is Nothing Then
							Dim PostID As Integer

							PostID = Int32.Parse(Request.QueryString("postid"))
							objPost = PostInfo.GetPostInfo(PostID, PortalId)
							ForumID = objPost.ForumID
							ThreadID = objPost.ThreadID
							AuthorID = objPost.Author.UserID
						Else
							' we have some type of issue if we are here and no postid in url
							Exit Sub
						End If

						Dim Notes As String = txtReason.Text
						'Dim ProfileUrl As String = Utils.MySettingsLink(TabId, ModuleId)
						Dim ProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)
						Dim url As String = Utilities.Links.ContainerViewForumLink(TabId, ForumID, False)

						If objConfig.MailNotification Then
							If chkEmailUsers.Checked Then
								' first send notice to the user
								Utilities.ForumUtils.SendForumMail(objPost.PostID, url, ForumEmailType.UserPostDeleted, Notes, objConfig, ProfileUrl, PortalId)
							End If
							' now send notice to the forum moderators
							Utilities.ForumUtils.SendForumMail(objPost.PostID, url, ForumEmailType.ModeratorPostDeleted, Notes, objConfig, ProfileUrl, PortalId)
						End If
						' CP - TEMP Hack
						If objPost.PostID = objPost.ThreadID Then
							_IsThreadDelete = True
						End If
						' Delete post (SEND MAIL BEFORE DELETE, we need the post still in the db)
						cntPost.PostDelete(objPost.PostID, UserId, Notes, PortalId, objPost.ParentThread.HostForum.GroupID, False, objPost.ParentThread.HostForum.ParentId)

						PostInfo.ResetPostInfo(objPost.PostID)
						ThreadInfo.ResetThreadInfo(ThreadID)
						ForumController.ResetForumInfoCache(ForumID)
						ForumUserController.ResetForumUser(AuthorID, PortalId)

						ThreadController.ResetThreadListCached(ForumID, ModuleId)
						If objConfig.AggregatedForums Then
							ThreadController.ResetThreadListCached(-1, ModuleId)
						End If

						Response.Redirect(GetReturnURL(ThreadID, ForumID), True)
					End If
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Returns the user to where they came from
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			Try
				If Not ViewState("UrlReferrer") Is Nothing Then
					Response.Redirect(ViewState("UrlReferrer").ToString, True)
				Else
					Response.Redirect(NavigateURL(), True)
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
		''' <history>
		''' 	[hmnguyen]	10/29/2005	Localization
		''' </history>
		Private Sub PopulatePost(ByVal PostID As Integer, ByVal ThreadDelete As Boolean)
			Dim objPost As New PostInfo
			objPost = PostInfo.GetPostInfo(PostID, PortalId)

			Dim fTextDecode As Utilities.PostContent = New Utilities.PostContent(Server.HtmlDecode(objPost.Body), objConfig)
			lblMessage.Text = fTextDecode.ProcessHtml
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
				Dim objTemplateInfo As New ForumTemplateInfo
				Dim arrDefaultTemplates As ArrayList
				'Get Default templates
				arrDefaultTemplates = objTempCnt.TemplatesGetDefaults(ForumTemplateTypes.DeletePost)

				If arrDefaultTemplates.Count > 0 Then
					' for each default template, create one specific to this module
					For Each objTemplateInfo In arrDefaultTemplates
						Dim NewTemplateInfo As New ForumTemplateInfo

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
			For Each objForumTemplate As ForumTemplateInfo In arrTemplates
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
			Dim objTemplate As ForumTemplateInfo
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

			If _ModeratorReturn Then
				If Not ViewState("UrlReferrer") Is Nothing Then
					url = (CType(ViewState("UrlReferrer"), String))
				Else
					If _IsThreadDelete Then
						url = Utilities.Links.ContainerViewForumLink(TabId, ForumID, False)
					Else
						' behave as before (normal usage)
						url = Utilities.Links.ContainerViewThreadLink(TabId, ForumID, ThreadID)
					End If
				End If
			Else
				If _IsThreadDelete Then
					url = Utilities.Links.ContainerViewForumLink(TabId, ForumID, False)
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