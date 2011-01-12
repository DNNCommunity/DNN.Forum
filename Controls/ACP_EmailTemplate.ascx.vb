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
Imports DotNetNuke.Wrapper.UI.WebControls

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' Allows users to manage email templates for a single portal instance
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class EmailTemplate
		Inherits ForumModuleBase

#Region "Event Handlers"

		''' <summary>
		''' Runs each time the page is loaded to make sure dynamic controls are created/bound properly. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				If Not Security.IsForumAdmin Then
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Page.IsPostBack = False Then
					Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
					ForumUtils.LoadCssFile(DefaultPage, objConfig)

					Dim ModuleTemplates As New ListItem(Localization.GetString("ModuleTemplates", objConfig.SharedResourceFile), "0")
					Dim HostTemplates As New ListItem(Localization.GetString("HostTemplates", objConfig.SharedResourceFile), "1")

					lblUpdateDone.Text = "<br />" & Localization.GetString("lblUpdateDone", Me.LocalResourceFile)

					rblstDefaults.Items.Add(ModuleTemplates)
					rblstDefaults.Items.Add(HostTemplates)
					rblstDefaults.SelectedIndex = 0

					'CP - Comeback - only show to hosts
					rowDefaults.Visible = False
					'End

					BindEmailTemplates()
					BindSingleTemplate()
				End If

				'[skeel] enable menu
				ACPmenu.ControlToLoad = "ACP_EmailTemplate.ascx"
				ACPmenu.PortalID = PortalId
				ACPmenu.objConfig = objConfig
				ACPmenu.ModuleID = ModuleId
				ACPmenu.EnableAjax = False
			Catch ex As Exception
				ProcessModuleLoadException(Me, ex)
			End Try
		End Sub

		''' <summary>
		''' Updates the selected template in the database
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				Dim objTemplateCnt As New EmailTemplateController
				Dim objTemplateInfo As New EmailTemplateInfo

				objTemplateInfo.EmailSubject = txtEmailSubject.Text
                objTemplateInfo.EmailTemplateID = CType(rcbEmailTemplate.SelectedValue, Integer)
				objTemplateInfo.HTMLBody = teContent.Text
				objTemplateInfo.TextBody = txtEmailTextBody.Text
				objTemplateInfo.IsActive = True

				Dim tempModuleID As Integer
				If rblstDefaults.SelectedIndex = 0 Then
					tempModuleID = ModuleId
				Else
					tempModuleID = -1
				End If
				objTemplateInfo.ModuleID = tempModuleID

				objTemplateCnt.UpdateEmailTemplate(objTemplateInfo)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' When the selected template is changed, binding must occur again
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
        Protected Sub ddlEmailTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcbEmailTemplate.SelectedIndexChanged
            BindSingleTemplate()
        End Sub

		''' <summary>
		''' Bind the default templates
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub rblstDefaults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rblstDefaults.SelectedIndexChanged
			BindEmailTemplates()
			BindSingleTemplate()
		End Sub

		''' <summary>
		''' Formats content as it is bound to the data grid.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dlKeywords_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlKeywords.ItemDataBound
			Dim item As DataListItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.EditItem Then
				Dim imgColumnControl As System.Web.UI.Control

				imgColumnControl = item.Controls(0).FindControl("lblToken")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblToken As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					Dim objKeyword As KeywordInfo = CType(item.DataItem, KeywordInfo)

					lblToken.Text = objKeyword.Token
				End If

				imgColumnControl = item.Controls(0).FindControl("lblDescription")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblDescription As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					Dim objKeyword As KeywordInfo = CType(item.DataItem, KeywordInfo)

					lblDescription.Text = objKeyword.Description
				End If
			End If
		End Sub

		''' <summary>
		''' Return users back to the Forum Home page.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdHome.Click
			Response.Redirect(NavigateURL(), True)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds the list of email templates for this portal
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub BindEmailTemplates()
			Try
				Dim objTemplateCnt As New EmailTemplateController
				Dim arrTemplates As ArrayList
				Dim tempModuleID As Integer

				If rblstDefaults.SelectedIndex = 0 Then
					tempModuleID = ModuleId
				Else
					tempModuleID = -1
				End If

				arrTemplates = objTemplateCnt.GetEmailTemplatesByModuleID(tempModuleID)

                rcbEmailTemplate.Items.Clear()

				' Do a check, if it is nothing returned we need to create new templates based on the defaults
				If arrTemplates.Count > 0 Then
					BindTemplateList(arrTemplates)
				Else
					Dim objEmailTemplateInfo As New EmailTemplateInfo
					Dim arrDefaultTemplates As ArrayList
					'Get Default templates
					arrDefaultTemplates = objTemplateCnt.GetDefaultEmailTemplates()

					If arrDefaultTemplates.Count > 0 Then
						' for each default template, create one specific to this module
						For Each objEmailTemplateInfo In arrDefaultTemplates
							Dim NewTemplateInfo As New EmailTemplateInfo
							NewTemplateInfo.EmailSubject = objEmailTemplateInfo.EmailSubject
							NewTemplateInfo.ForumTemplateTypeID = objEmailTemplateInfo.ForumTemplateTypeID
							NewTemplateInfo.HTMLBody = objEmailTemplateInfo.HTMLBody
							NewTemplateInfo.TextBody = objEmailTemplateInfo.TextBody
							NewTemplateInfo.ModuleID = ModuleId
							NewTemplateInfo.IsActive = objEmailTemplateInfo.IsActive
							NewTemplateInfo.EmailTemplateName = objEmailTemplateInfo.EmailTemplateName
							NewTemplateInfo.ForumContentTypeID = objEmailTemplateInfo.ForumContentTypeID
							NewTemplateInfo.ForumEmailTypeID = objEmailTemplateInfo.ForumEmailTypeID

							objTemplateCnt.AddEmailTemplateForModuleID(NewTemplateInfo)
						Next

						' We now have to bind the new templates to the ddl
						arrTemplates = objTemplateCnt.GetEmailTemplatesByModuleID(ModuleId)
						BindTemplateList(arrTemplates)

					End If

				End If
			Catch exc As System.Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Does the actual binding of a list of available email templates for editing
		''' </summary>
		''' <param name="arrTemplates"></param>
		''' <remarks></remarks>
		Private Sub BindTemplateList(ByVal arrTemplates As ArrayList)
			For Each objEmailTemplate As EmailTemplateInfo In arrTemplates
                Dim HostTemplates As New DnnComboBoxItem(Localization.GetString(objEmailTemplate.EmailTemplateName, objConfig.SharedResourceFile), objEmailTemplate.EmailTemplateID.ToString)
                rcbEmailTemplate.Items.Add(HostTemplates)
			Next
		End Sub

		''' <summary>
		''' Binds a single template for editing
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub BindSingleTemplate()
			Dim objTemplateCnt As New EmailTemplateController
			Dim objTemplateInfo As EmailTemplateInfo
            Dim tempTemplateID As Integer = CType(rcbEmailTemplate.SelectedValue, Integer)

			objTemplateInfo = objTemplateCnt.GetEmailTemplate(tempTemplateID)

			txtEmailSubject.Text = String.Empty
			teContent.Text = String.Empty
			txtEmailTextBody.Text = String.Empty

			If Not objTemplateInfo Is Nothing Then
				txtEmailSubject.Text = objTemplateInfo.EmailSubject
				teContent.Text = objTemplateInfo.HTMLBody
				txtEmailTextBody.Text = objTemplateInfo.TextBody

				Dim ctlKeywords As New ForumKeywordController
				Dim arrKeys As ArrayList

				arrKeys = ctlKeywords.GetKeywordsByType(objTemplateInfo.ForumContentTypeID)
				If arrKeys.Count > 0 Then
					dlKeywords.DataSource = arrKeys
					dlKeywords.DataBind()
				End If
			End If
		End Sub

#End Region

	End Class

End Namespace