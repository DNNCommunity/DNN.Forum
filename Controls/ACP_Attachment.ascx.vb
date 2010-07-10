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

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This is the General Settings area from Forum Administration.
	''' Set basic configuration items for this forum module. (mod settings)
	''' </summary>
	''' <remarks>The majority of how the module operates is set here.
	''' </remarks>
	Partial Public Class Attachment
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			chkAttachment.Checked = objConfig.EnableAttachment
			chkAnonDownloads.Checked = objConfig.AnonDownloads
			txtAttachmentPath.Text = objConfig.AttachmentPath
			txtMaxAttachmentSize.Text = objConfig.MaxAttachmentSize.ToString()

			' If attachments are enabled, see if anon users can download
			If objConfig.EnableAttachment Then
				chkAnonDownloads.Checked = objConfig.AnonDownloads
				rowAnonDownloads.Visible = True
				rowAttachmentPath.Visible = True
				rowMaxAttachmentSize.Visible = True
			Else
				rowAnonDownloads.Visible = False
				rowAttachmentPath.Visible = False
				rowMaxAttachmentSize.Visible = False
			End If
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
				ctlModule.UpdateModuleSetting(ModuleId, "EnableAttachment", chkAttachment.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "AnonDownloads", chkAnonDownloads.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "AttachmentPath", txtAttachmentPath.Text.Trim())
				ctlModule.UpdateModuleSetting(ModuleId, "MaxAttachmentSize", txtMaxAttachmentSize.Text.Trim())

				Utilities.ForumUtils.CheckFolder(txtAttachmentPath.Text.Trim())

				Configuration.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				Dim s As String = exc.ToString
				s = s & " "
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Enables/Disables anon download checkbox depending on if
		''' attachments are permitted or not.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Changes viewable/editable items when checked/unchecked.
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/11/2006	Created
		''' </history>
		Protected Sub chkAttachment_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAttachment.CheckedChanged
			If chkAttachment.Checked Then
				rowAnonDownloads.Visible = True
				rowAttachmentPath.Visible = True
				rowMaxAttachmentSize.Visible = True
			Else
				rowAnonDownloads.Visible = False
				chkAnonDownloads.Checked = False
				rowAttachmentPath.Visible = False
				rowMaxAttachmentSize.Visible = False
			End If
		End Sub

#End Region

	End Class

End Namespace