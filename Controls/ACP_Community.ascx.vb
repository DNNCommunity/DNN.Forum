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
			chkEnableMemberList.Checked = objConfig.EnableMemberList

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
				ctlModule.UpdateModuleSetting(ModuleId, "EnablePMSystem", chkEnablePMSystem.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "EnableMemberList", chkEnableMemberList.Checked.ToString)
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

#End Region

	End Class

End Namespace