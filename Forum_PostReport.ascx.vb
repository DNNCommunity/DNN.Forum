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
	''' Post abuse reporting is done from this screen. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public MustInherit Class PostReport
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Private _ForumID As Integer
		Private _ThreadID As Integer
		Private _PostID As Integer
		Private _PostInfo As PostInfo

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
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdCancel)
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
				Dim cntThread As New ThreadController()

				If Not Request.QueryString("postid") Is Nothing Then
					Dim objThread As New ThreadInfo
					Dim cntPost As New PostController()

					_PostID = Int32.Parse(Request.QueryString("postid"))
					_PostInfo = cntPost.GetPostInfo(_PostID, PortalId)
					_ThreadID = _PostInfo.ThreadID
					objThread = cntThread.GetThread(_ThreadID)
					_ForumID = objThread.ForumID
				Else
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Request.IsAuthenticated Then
					Dim LoggedOnUserID As Integer
					LoggedOnUserID = Users.UserController.GetCurrentUserInfo.UserID

					' Do a check here to make sure the person trying to report this hasn't reported it before
					Dim cntPostReport As New PostReportedController
					If cntPostReport.CheckPostReport(_PostID, LoggedOnUserID) Then
						' disable submit button, let user know they have reported this before
						cmdReport.Visible = False
						lblAlreadyReported.Visible = True
					End If
				Else
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
				ForumUtils.LoadCssFile(DefaultPage, objConfig)

				If Page.IsPostBack = False Then
					PopulatePost(_PostInfo)
					PopulateTemplateDDL()

					cmdReport.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("ReportPost", LocalResourceFile) & "');")

					If Not Request.UrlReferrer Is Nothing Then
						ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
					End If

					imgHeadSpacerL.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
					imgHeadSpacerR.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				End If

			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Reports a post as abusive
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReport.Click
			Try
				' Get most recent info on this post
				If Not Request.QueryString("postid") Is Nothing Then
					_PostID = Int32.Parse(Request.QueryString("postid"))
					Dim cntPost As New PostController()
					_PostInfo = cntPost.GetPostInfo(_PostID, PortalId)
				End If

				Dim cntPostReport As New PostReportedController
				Dim Notes As String = String.Empty
				'Dim MyProfileUrl As String = Utils.MySettingsLink(TabId, ModuleId)
				Dim MyProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)

				' CP - Wrapped in IsValid to make sure email response is made
				If Page.IsValid Then
					Notes = txtReason.Text

					' send the report notice to the db (increments number of reports by 1 each time
					cntPostReport.AddPostReport(_PostInfo.PostID, Users.UserController.GetCurrentUserInfo.UserID, Notes)

					Dim url As String
					url = Utilities.Links.ContainerViewPostLink(TabId, _ForumID, _PostID)

					'Notes
					If objConfig.MailNotification Then
						' SendForumMail(mPostID, url, ForumEmailType.UserPostAbuse, Notes, mForumConfig, MyProfileUrl, PortalId)
						Utilities.ForumUtils.SendForumMail(_PostID, url, ForumEmailType.ModeratorPostAbuse, Notes, objConfig, MyProfileUrl, PortalId)
					End If

					' only thing that changes is post, so only reset it
					Forum.Components.Utilities.Caching.UpdatePostCache(_PostID)

					If Not ViewState("UrlReferrer") Is Nothing Then
						Response.Redirect(ViewState("UrlReferrer").ToString, False)
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
		''' <remarks></remarks>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			Try
				If Not ViewState("UrlReferrer") Is Nothing Then
					Response.Redirect(ViewState("UrlReferrer").ToString, False)
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
		Protected Sub ddlReportTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlReportTemplate.SelectedIndexChanged
			BindDeleteTemplateBody()
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Gets the post content of the one the user wishes to report
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[hmnguyen]	10/29/2005	Localization
		''' </history>
		Private Sub PopulatePost(ByVal PostInfo As PostInfo)
			Dim fTextDecode As Utilities.PostContent = New Utilities.PostContent(Server.HtmlDecode(_PostInfo.Body), objConfig)
			lblMessage.Text = fTextDecode.ProcessHtml
			lblSubject.Text = _PostInfo.Subject
			lblAuthor.Text = String.Format(Localization.GetString("PostedBy.Text", Me.LocalResourceFile), _PostInfo.Author.SiteAlias, _PostInfo.CreatedDate.ToString)
		End Sub

		''' <summary>
		''' Populates a list of available templates, if this is the first time it is run it gets defaults and creates available templates for this portal.
		''' </summary>
		''' <remarks></remarks>
		Private Sub PopulateTemplateDDL()
			Dim arrTemplates As ArrayList
			Dim objTempCnt As New ForumTemplateController

			arrTemplates = objTempCnt.TemplatesGetByType(ModuleId, ForumTemplateTypes.PostAbuse)

			If arrTemplates.Count > 0 Then
				BindTemplateList(arrTemplates)
			Else
				Dim objTemplateInfo As New TemplateInfo
				Dim arrDefaultTemplates As ArrayList
				'Get Default templates
				arrDefaultTemplates = objTempCnt.TemplatesGetDefaults(ForumTemplateTypes.PostAbuse)

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
					arrTemplates = objTempCnt.TemplatesGetByType(ModuleId, ForumTemplateTypes.PostAbuse)
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
				ddlReportTemplate.Items.Add(AvailableTemplates)
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
			objTemplate = objTempCnt.TemplatesGetSingle(CType(ddlReportTemplate.SelectedValue, Integer))

			Dim Reason As String = objTemplate.TemplateValue

			txtReason.Text = (Reason)
		End Sub

#End Region

	End Class

End Namespace