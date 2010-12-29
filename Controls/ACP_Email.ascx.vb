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

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This screen is used to manage the email configuration for this forum module.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class Email
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			' First, enable proper rows based on saved config setting
			EnableEmail(objConfig.MailNotification)

			' Not implemented yet. (only show if chkEnablePerForumFrom is enabled too)
			rowEnableListServer.Visible = False
			' Not implemented yet. (only show if chkEnablePerForumFrom is enabled too)
			rowListServerFolder.Visible = False

			chkNotify.Checked = objConfig.MailNotification
			chkEnablePerForumFrom.Checked = objConfig.EnablePerForumFrom
			txtAutomatedAddress.Text = objConfig.AutomatedEmailAddress
			txtEmailAddressDisplayName.Text = objConfig.EmailAddressDisplayName
			chkEnableEditEmails.Checked = objConfig.EnableEditEmails

			' Not implemented yet.
			chkEnableListServer.Checked = False
			'chkEnableListServer.Checked = objConfig.EnableListServer
			' Not implemented yet.
			'txtListServerFolder.Text = objConfig.ListServerFolder

			' Add a check here to see if task is installed and enabled, if so enable control
			' we need to 'hack' a way to get the ScheduleID of the forum email queue item
			Dim ScheduleItemID As Integer = DataProvider.Instance().EmailQueueTaskScheduleItemIDGet(False)

			Dim objScheduleItem As Scheduling.ScheduleItem
			objScheduleItem = DotNetNuke.Services.Scheduling.SchedulingProvider.Instance.GetSchedule(ScheduleItemID)

			If Not objScheduleItem Is Nothing Then
				If objScheduleItem.Enabled Then
					chkEmailQueueTask.Enabled = True
					chkEmailQueueTask.Checked = objConfig.EnableEmailQueueTask
				Else
					chkEmailQueueTask.Enabled = False
					chkEmailQueueTask.Checked = False
				End If
			End If
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Updates the email settings in the database and redirects the user back to the forum admin page
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				Dim ctlModule As New Entities.Modules.ModuleController

				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_MAIL_NOTIFICATIONS, chkNotify.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_PER_FORUM_EMAILS, chkEnablePerForumFrom.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.EMAIL_AUTO_FROM_ADDRESS, txtAutomatedAddress.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.EMAIL_ADDRESS_DISPLAY_NAME, txtEmailAddressDisplayName.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_EDIT_EMAILS, chkEnableEditEmails.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_EMAILE_SEND_QUEUE, chkEmailQueueTask.Checked.ToString)

				' Not implemented (YET): NOTE: This can only be handled via a task when implemented.
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_LIST_SERVER, chkEnableListServer.Checked.ToString)
				' Not implemented (YET): NOTE: Don't forget to create folder in DNN File System too. 
				ctlModule.UpdateModuleSetting(ModuleId, Constants.LIST_SERVER_FOLDER, txtListServerFolder.Text)

				Configuration.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' When checked or unchecked, changes what is available for the user to configure. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub chkNotify_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkNotify.CheckedChanged
			EnableEmail(chkNotify.Checked)
		End Sub

		''' <summary>
		''' When checked or unchecked, changes what is available for the user to configure (in relation to list server options).
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub chkEnableListServer_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnableListServer.CheckedChanged
			EnableListServer(chkEnableListServer.Checked)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Used to determine if email notification detail rows should show or not. 
		''' </summary>
		''' <param name="Enabled">True, if email notifications are enabled for the forum.</param>
		''' <remarks></remarks>
		Private Sub EnableEmail(ByVal Enabled As Boolean)
			rowEnablePerForumFrom.Visible = Enabled
			rowEnableEditEmails.Visible = Enabled
			rowEmailQueueTask.Visible = Enabled
			'rowEnableListServer.Visible = Enabled
		End Sub

		''' <summary>
		''' Used to determine if list server configuration options should be displayed. 
		''' </summary>
		''' <param name="Enabled"></param>
		''' <remarks></remarks>
		Private Sub EnableListServer(ByVal Enabled As Boolean)
			rowListServerFolder.Visible = Enabled
		End Sub

#End Region

	End Class

End Namespace