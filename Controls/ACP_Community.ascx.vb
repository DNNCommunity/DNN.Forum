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

Imports DotNetNuke.Entities.Profile

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

			chkEnablePMSystem.Checked = objConfig.EnablePMSystem
			EnablePM(objConfig.EnablePMSystem)
			chkEnableMemberList.Checked = objConfig.EnableMemberList
			' we use memberlist below because it determines if we are showing rows based on it.
			EnableMemberList(objConfig.EnableMemberList)
			chkEnableExtDirectory.Checked = objConfig.EnableExternalDirectory
			EnableExternalDirectory(objConfig.EnableExternalDirectory)
			ddlExtDirectoryPageID.SelectedValue = objConfig.ExternalDirectoryPage.ToString()
			txtExtDirectoryParamName.Text = objConfig.ExternalDirectoryParamName
			txtExtDirectoryParamValue.Text = objConfig.ExternalDirectoryParamValue
			chkEnableExtProfilePage.Checked = objConfig.EnableExternalProfile
			EnableExternalProfile(objConfig.EnableExternalProfile)
			ddlExtProfilePageID.SelectedValue = objConfig.ExternalProfilePage.ToString()
			txtExtProfileUserParam.Text = objConfig.ExternalProfileParam.ToString()
			txtExtProfileParamName.Text = objConfig.ExternalProfileParamName
			txtExtProfileParamValue.Text = objConfig.ExternalProfileParamValue
			txtEmoticonPath.Text = objConfig.EmoticonPath
			txtEmoticonMaxFileSize.Text = objConfig.EmoticonMaxFileSize.ToString()

			'Do we support emoticons?
			Select Case objConfig.DefaultHtmlEditorProvider.ToLower
				' CP - UNCOMMENT: When ready to implement emoticons. (don't forget to set rowEnableEmoticon.Visible = True
				'Case "fckhtmleditorprovider", "radeditorprovider", "radhtmleditorprovider"
				'	chkEnableEmoticons.Enabled = True
				'	chkEnableEmoticons.Checked = objConfig.EnableEmoticons
				'	rowEmoticonPath.Visible = chkEnableEmoticons.Checked
				'	rowEmoticonMaxFileSize.Visible = objConfig.EnableEmoticons
				Case Else
					chkEnableEmoticons.Checked = False
					rowEmoticonPath.Visible = False
					chkEnableEmoticons.Enabled = False
					rowEmoticonMaxFileSize.Visible = False
					' uncomment:
					rowEmoticonEnable.Visible = False
			End Select
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
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				' Update settings in the database
				Dim ctlModule As New Entities.Modules.ModuleController
				ctlModule.UpdateModuleSetting(ModuleId, "EnableUsersOnline", chkUserOnline.Checked.ToString)
				' PM System
				ctlModule.UpdateModuleSetting(ModuleId, "EnablePMSystem", chkEnablePMSystem.Checked.ToString)


				' Member Directory
				ctlModule.UpdateModuleSetting(ModuleId, "EnableMemberList", chkEnableMemberList.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "EnableExternalDirectory", chkEnableExtDirectory.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "ExternalDirectoryPage", ddlExtDirectoryPageID.SelectedValue)
				ctlModule.UpdateModuleSetting(ModuleId, "ExternalDirectoryParamName", txtExtDirectoryParamName.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "ExternalDirectoryParamValue", txtExtDirectoryParamValue.Text)
				' User Profile
				ctlModule.UpdateModuleSetting(ModuleId, "EnableExternalProfile", chkEnableExtProfilePage.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "ExternalProfilePage", ddlExtProfilePageID.SelectedValue)
				ctlModule.UpdateModuleSetting(ModuleId, "ExternalProfileParam", txtExtProfileUserParam.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "ExternalProfileParamName", txtExtProfileParamName.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "ExternalProfileParamName", txtExtProfileParamValue.Text)
				' Emoticons (disabled)
				ctlModule.UpdateModuleSetting(ModuleId, "EnableEmoticons", chkEnableEmoticons.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "EmoticonMaxFileSize", txtEmoticonMaxFileSize.Text)

				'Check the folder
				Utilities.ForumUtils.CheckFolder(txtEmoticonPath.Text.Trim)
				ctlModule.UpdateModuleSetting(ModuleId, "EmoticonPath", txtEmoticonPath.Text.Trim)

				Config.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				Dim s As String = exc.ToString
				s = s & " "
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Enables/Disables Emoticon path
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Changes viewable/editable items when checked/unchecked.
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/22/2008	Created
		''' </history>
		Protected Sub chkEnableEmoticons_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableEmoticons.CheckedChanged
			rowEmoticonPath.Visible = chkEnableEmoticons.Checked
			rowEmoticonMaxFileSize.Visible = chkEnableEmoticons.Checked
		End Sub

		''' <summary>
		''' Enables/Disabled External Profile Pages.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub chkEnableExtProfilePage_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnableExtProfilePage.CheckedChanged
			EnableExternalProfile(chkEnableExtProfilePage.Checked)
		End Sub

		''' <summary>
		''' Enables/Disabled Member Directory items.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub chkEnableMemberList_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnableMemberList.CheckedChanged
			EnableMemberList(chkEnableMemberList.Checked)
		End Sub

		''' <summary>
		''' Enables/Disables External Directory items.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub chkEnableExtDirectory_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkEnableExtDirectory.CheckedChanged
			EnableExternalDirectory(chkEnableExtDirectory.Checked)
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

			ddlExtDirectoryPageID.ClearSelection()
			ddlExtDirectoryPageID.DataSource = colTabs
			ddlExtDirectoryPageID.DataBind()

			ddlExtProfilePageID.ClearSelection()
			ddlExtProfilePageID.DataSource = colTabs
			ddlExtProfilePageID.DataBind()

			'ddlExtProfilePageID.ClearSelection()
			'ddlExtProfilePageID.DataSource = colProfileProps
			'ddlExtProfilePageID.DataBind()
		End Sub

		''' <summary>
		''' Sets row visibility for member directory related items.
		''' </summary>
		''' <param name="Enabled"></param>
		''' <remarks></remarks>
		Private Sub EnableMemberList(ByVal Enabled As Boolean)
			rowEnableExtDirectory.Visible = Enabled
			rowExtDirectoryPageID.Visible = Enabled
			rowExtDirectoryParamName.Visible = Enabled
			rowExtDirectoryParamValue.Visible = Enabled

			If Not Enabled Then
				chkEnableExtDirectory.Checked = False
			End If
		End Sub

		''' <summary>
		''' Sets row visibility for External Directory support.
		''' </summary>
		''' <param name="Enabled"></param>
		''' <remarks></remarks>
		Private Sub EnableExternalDirectory(ByVal Enabled As Boolean)
			rowExtDirectoryPageID.Visible = Enabled
			rowExtDirectoryParamName.Visible = Enabled
			rowExtDirectoryParamValue.Visible = Enabled
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
		End Sub

		''' <summary>
		''' Sets row visibility for PM related items.
		''' </summary>
		''' <param name="Enabled"></param>
		''' <remarks></remarks>
		Private Sub EnablePM(ByVal Enabled As Boolean)
			rowEnableExtPM.Visible = Enabled
		End Sub

		''' <summary>
		''' Sets row visibility for External PM support.
		''' </summary>
		''' <param name="Enabled">True if enabled, false otherwise.</param>
		''' <remarks></remarks>
		Private Sub EnableExternalPM(ByVal Enabled As Boolean)

		End Sub

#End Region

	End Class

End Namespace