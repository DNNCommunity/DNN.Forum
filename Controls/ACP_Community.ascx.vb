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
	''' This area controls what community oriented features are exposed to forum users. 
	''' </summary>
	''' <remarks>\
	''' </remarks>
	Partial Public Class Community
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			BindTabs()

			If Entities.Host.Host.GetHostSettingsDictionary.ContainsKey("DisableUsersOnline") And (Not Entities.Host.Host.GetHostSettingsDictionary("DisableUsersOnline") Is Nothing) Then
				If Entities.Host.Host.GetHostSettingsDictionary("DisableUsersOnline").ToString = "Y" Then
					rowUserOnline.Visible = False
				Else
					rowUserOnline.Visible = True
					chkUserOnline.Checked = objConfig.EnableUsersOnline
				End If
			Else
				rowUserOnline.Visible = False
			End If

			chkEnableExtProfilePage.Checked = objConfig.EnableExternalProfile
			EnableExternalProfile(objConfig.EnableExternalProfile)
			ddlExtProfilePageID.SelectedValue = objConfig.ExternalProfilePage.ToString()
			txtExtProfileUserParam.Text = objConfig.ExternalProfileParam.ToString()
			txtExtProfileParamName.Text = objConfig.ExternalProfileParamName
			txtExtProfileParamValue.Text = objConfig.ExternalProfileParamValue
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Updates the module's configuration (module settings)
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Saves the module settings shown in this view.
		''' </remarks>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				Dim ctlModule As New Entities.Modules.ModuleController
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_USERS_ONLINE, chkUserOnline.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_EXTERNAL_PROFILE_PAGE, chkEnableExtProfilePage.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.EXTERNAL_PROFILE_PAGE, ddlExtProfilePageID.SelectedValue)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.EXTERNAL_PROFILE_USER_PARAM, txtExtProfileUserParam.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.EXTERNAL_PROFILE_PARAM_NAME, txtExtProfileParamName.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.EXTERNAL_PROFILE_PARAM_VALUE, txtExtProfileParamValue.Text)

				Configuration.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				Dim s As String = exc.ToString
				s = s & " "
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Enables/Disabled external profile fields in the user interface.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub chkEnableExtProfilePage_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnableExtProfilePage.CheckedChanged
			EnableExternalProfile(chkEnableExtProfilePage.Checked)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Populates drop down lists with tab collections.
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindTabs()
			Dim colTabs As New List(Of DotNetNuke.Entities.Tabs.TabInfo)
			colTabs = DotNetNuke.Entities.Tabs.TabController.GetPortalTabs(PortalId, 0, True, True, False, False)

			ddlExtProfilePageID.ClearSelection()
			ddlExtProfilePageID.DataSource = colTabs
			ddlExtProfilePageID.DataBind()
		End Sub

		''' <summary>
		''' Sets row visibility for External Profile support.
		''' </summary>
		''' <param name="Enabled">True if enabled, false otherwise.</param>
		''' <remarks></remarks>
		Private Sub EnableExternalProfile(ByVal Enabled As Boolean)
			rowExtProfilePageID.Visible = Enabled
			rowExtProfileUserParam.Visible = Enabled
			rowExtProfileParamName.Visible = Enabled
			rowExtProfileParamValue.Visible = Enabled
			rowEnableExtProfile.Visible = True
		End Sub

#End Region

	End Class

End Namespace