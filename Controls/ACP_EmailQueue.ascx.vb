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
Option Strict On
Option Explicit On

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This screen is used to manage the email configuration queue cleanup
	''' This can only be adjusted by SuperUsers
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class EmailQueue
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			txtTaskDeleteDays.Enabled = False
			txtEmailDeleteDays.Enabled = False
			cmdUpdate.Visible = False

			If Users.UserController.GetCurrentUserInfo.IsSuperUser Then
				' Add a check here to see if task is installed and enabled, if so enable control
				' we need to 'hack' a way to get the ScheduleID of the forum email queue item
				Dim ScheduleItemID As Integer = DataProvider.Instance().EmailQueueTaskScheduleItemIDGet(True)

				Dim objScheduleItem As Scheduling.ScheduleItem
				objScheduleItem = DotNetNuke.Services.Scheduling.SchedulingProvider.Instance.GetSchedule(ScheduleItemID)

				If Not objScheduleItem Is Nothing Then
					If objScheduleItem.Enabled Then
						txtTaskDeleteDays.Enabled = True
						txtEmailDeleteDays.Enabled = True
						cmdUpdate.Visible = True
					End If
				End If
			End If

			If Not Entities.Host.Host.GetHostSettingsDictionary("ForumTaskDeleteDays") Is Nothing Then
				If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("ForumTaskDeleteDays")) <> String.Empty Then
					txtTaskDeleteDays.Text = Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("ForumTaskDeleteDays"))
				Else
					txtTaskDeleteDays.Text = "30"
				End If
			Else
				txtTaskDeleteDays.Text = "30"
			End If

			If Not Entities.Host.Host.GetHostSettingsDictionary("ForumEmailDeleteDays") Is Nothing Then
				If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("ForumEmailDeleteDays")) <> String.Empty Then
					txtEmailDeleteDays.Text = Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("ForumEmailDeleteDays"))
				Else
					txtEmailDeleteDays.Text = "30"
				End If
			Else
				txtEmailDeleteDays.Text = "30"
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
		Private Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				lblUpdateDone.Visible = True
				Entities.Host.Host.GetHostSettingsDictionary("ForumTaskDeleteDays") = txtTaskDeleteDays.Text
				Entities.Host.Host.GetHostSettingsDictionary("ForumEmailDeleteDays") = txtEmailDeleteDays.Text
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

	End Class

End Namespace