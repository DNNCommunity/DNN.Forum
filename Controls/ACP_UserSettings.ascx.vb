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
	''' This is the General Settings area from Forum Administration.
	''' Set basic configuration items for this forum module. (mod settings)
	''' </summary>
	''' <remarks>The majority of how the module operates is set here.
	''' </remarks>
	Partial Public Class UserSettings
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This interface is used to replace a If Page.IsPostBack typically used in page load. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			BindMemberNameTypes()
			ddlNameDisplay.SelectedIndex = objConfig.ForumMemberName
			chkTrustNewUsers.Checked = objConfig.TrustNewUsers
			chkAutoLockTrust.Checked = objConfig.AutoLockTrust
			chkEnableUserSignatures.Checked = objConfig.EnableUserSignatures
			chkEnableModSigUpdates.Checked = objConfig.EnableModSigUpdates
			chkEnableHTMLSignatures.Checked = objConfig.EnableHTMLSignatures
			txtPostEditWindow.Text = CStr(objConfig.PostEditWindow)
			chkEnableUserBanning.Checked = objConfig.EnableUserBanning
			'CP - COMEBACK: Handle when we are able to impersonate security of other users.
			chkHideModEdit.Checked = False 'mForumConfig.HideModEdits
			SetVisibleItems()
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
				' Update settings in the database
				Dim ctlModule As New Entities.Modules.ModuleController
				ctlModule.UpdateModuleSetting(ModuleId, "ForumMemberName", ddlNameDisplay.SelectedValue)
				ctlModule.UpdateModuleSetting(ModuleId, "EnableUserSignatures", chkEnableUserSignatures.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "EnableHTMLSignatures", chkEnableHTMLSignatures.Checked.ToString)
				If txtPostEditWindow.Text <> String.Empty Then
					ctlModule.UpdateModuleSetting(ModuleId, "PostEditWindow", txtPostEditWindow.Text)
				Else
					ctlModule.UpdateModuleSetting(ModuleId, "PostEditWindow", "0")
				End If
				ctlModule.UpdateModuleSetting(ModuleId, "TrustNewUsers", chkTrustNewUsers.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "AutoLockTrust", chkAutoLockTrust.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "EnableModSigUpdates", chkEnableModSigUpdates.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "HideModEdits", chkHideModEdit.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "EnableUserBanning", chkEnableUserBanning.Checked.ToString)

				Config.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Enables/Disables user signature related rows depending on selection.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Changes viewable/editable options when checked/unchecked.</remarks>
		Protected Sub chkEnableUserSignatures_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableUserSignatures.CheckedChanged
			SetVisibleItems()
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Sets visiblity for rows on page depending on user signatures being enabled.
		''' </summary>
		''' <remarks>Uses forum configuration settings to show/hide items.</remarks>
		Private Sub SetVisibleItems()
			If chkEnableUserSignatures.Checked Then
				rowModSigUpdates.Visible = True
				rowHTMLSignatures.Visible = True
			Else
				rowModSigUpdates.Visible = False
				rowHTMLSignatures.Visible = False
			End If
		End Sub

		''' <summary>
		''' Binds the various member name display options, pulled from core Lists table. 
		''' </summary>
		''' <remarks>Uses lists localized items to determine options.</remarks>
		Private Sub BindMemberNameTypes()
			' Use new Lists feature to provide DisplayPosterLocation entries (localization support)
			Dim ctlLists As New DotNetNuke.Common.Lists.ListController
			Dim LocationDisplayTypes As DotNetNuke.Common.Lists.ListEntryInfoCollection = ctlLists.GetListEntryInfoCollection("ForumMemberName")
			ddlNameDisplay.ClearSelection()

			For Each entry As DotNetNuke.Common.Lists.ListEntryInfo In LocationDisplayTypes
				Dim LocationEntryType As New ListItem(Localization.GetString(entry.Text, objConfig.SharedResourceFile), entry.Value)
				ddlNameDisplay.Items.Add(LocationEntryType)
			Next
			' Now Bind the items
			ddlNameDisplay.Items.FindByValue(objConfig.ForumMemberName.ToString).Selected = True
		End Sub

#End Region

	End Class

End Namespace