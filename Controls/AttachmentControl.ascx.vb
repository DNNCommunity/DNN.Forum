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

Namespace DotNetNuke.Modules.Forum.WebControls

	''' <summary>
	''' A subcontrol used to upload, display and select Attachments 
	''' </summary>
	''' <remarks></remarks>
	Partial Public Class AttachmentControl
		Inherits ForumModuleBase

#Region " Private Members "

		Private mBaseFolder As String = String.Empty
		Private mlocalResourceFile As String = String.Empty
		Private FileFilter As String = String.Empty
		Private mForumConfig As Forum.Config

#End Region

#Region " Public Properties "

		''' <summary>
		''' Local Resource file for localization. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Shadows Property LocalResourceFile() As String
			Get
				Dim fileRoot As String

				If mlocalResourceFile = String.Empty Then
					fileRoot = Me.TemplateSourceDirectory & "/" & Localization.LocalResourceDirectory & "/AttachmentControl.ascx"
				Else
					fileRoot = mlocalResourceFile
				End If
				Return fileRoot
			End Get
			Set(ByVal Value As String)
				mlocalResourceFile = Value
			End Set
		End Property

		''' <summary>
		''' PostId
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostId() As Integer
			Get
				If Not ViewState("PostId") Is Nothing Then
					Return CInt(ViewState("PostId").ToString())
				Else
					Return -1
				End If
			End Get
			Set(ByVal value As Integer)
				ViewState("PostId") = CStr(value)
			End Set
		End Property

		''' <summary>
		''' Width
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<System.ComponentModel.Bindable(True), System.ComponentModel.Category("Behavior"), System.ComponentModel.DefaultValue("Normal")> Property Width() As String
			Get
				If Not ViewState("Width") Is Nothing Then
					Return ViewState("Width").ToString()
				Else
					Return "350px"
				End If
			End Get
			Set(ByVal value As String)
				ViewState("Width") = value
			End Set
		End Property

		''' <summary>
		''' A list of attachmentid's bound to the control.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property lstAttachmentIDs() As String
			Get
				If Not ViewState("lstAttachmentIDs") Is Nothing Then
					Return ViewState("lstAttachmentIDs").ToString()
				Else
					Return ""
				End If
			End Get
			Set(ByVal value As String)
				ViewState("lstAttachmentIDs") = value
			End Set
		End Property

#End Region

#Region " Private Properties "

		''' <summary>
		''' Post portal root folder path setting.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property BaseFolder() As String
			Get
				If mBaseFolder = String.Empty Then
					mBaseFolder = objConfig.AttachmentPath
				End If

				If mBaseFolder.EndsWith("/") = False Then mBaseFolder += "/"

				Return mBaseFolder
			End Get
		End Property

		''' <summary>
		''' Do we allow Inline placement of Attachments?
		''' </summary>
		''' <value></value>
		''' <returns>Boolean value</returns>
		''' <remarks></remarks>
		Private Property AllowInline() As Boolean
			Get
				If Not ViewState("AllowInline") Is Nothing Then
					Return CBool(ViewState("AllowInline"))
				Else
					Return False
				End If
			End Get
			Set(ByVal value As Boolean)
				ViewState("AllowInline") = CStr(value)
			End Set
		End Property

		''' <summary>
		''' ModuleId
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Shadows ReadOnly Property ModuleId() As Integer
			Get
				Return CInt(Request.QueryString("mid").ToString())
			End Get
		End Property

		''' <summary>
		''' Used to identify if the RichEditor Mode has been changed
		''' </summary>
		''' <value></value>
		''' <returns>RICH/BASIC/""</returns>
		''' <remarks></remarks>
		Private Property EditorMode() As String
			Get
				If Not ViewState("EditorMode") Is Nothing Then
					Return ViewState("EditorMode").ToString()
				Else
					Return String.Empty
				End If
			End Get
			Set(ByVal value As String)
				ViewState("EditorMode") = value
			End Set
		End Property

		''' <summary>
		''' DeleteItems Are Used to Delete files while not being locked
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property DeleteItems() As String
			Get
				If Not ViewState("DeleteItems") Is Nothing Then
					Return ViewState("DeleteItems").ToString()
				Else
					Return String.Empty
				End If
			End Get
			Set(ByVal value As String)
				ViewState("DeleteItems") = value
			End Set
		End Property

#End Region

#Region " Event Handlers "

		''' <summary>
		''' When the control is loaded, we want to make sure the cmdUpload is registered for postback because of the nature of the file upload control and security. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, False)
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdUpload)
			End If
			tblAttachment.Width = System.Web.UI.WebControls.Unit.Parse(Width).ToString
		End Sub

		''' <summary>
		''' When the control is loaded, we want to make sure the cmdUpload is registered for postback because of the nature of the file upload control and security. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			'Localization (needed because of AJAX)
			cmdDelete.ToolTip = Localization.GetString("Delete", Me.LocalResourceFile)
			cmdUpload.Text = Localization.GetString("Upload", Me.LocalResourceFile)
			plInline.Text = Localization.GetString("plInline", Me.LocalResourceFile) & ":"
			plInline.HelpText = Localization.GetString("plInline.Help", Me.LocalResourceFile)
			plUpload.Text = Localization.GetString("plUpload", Me.LocalResourceFile) & ":"
			plUpload.HelpText = Localization.GetString("plUpload.Help", Me.LocalResourceFile)
			plAttachments.Text = Localization.GetString("plAttachments", Me.LocalResourceFile) & ":"
			plAttachments.HelpText = Localization.GetString("plAttachments.Help", Me.LocalResourceFile)

			' We can only delete files while not in use, look for Items to be deleted (PostID set to -2)
			If Page.IsPostBack = False Then
				Dim cntAttachment As New AttachmentController
				Dim lstAttachments As List(Of AttachmentInfo) = cntAttachment.GetAllByPostID(-2)
				If lstAttachments.Count > 0 Then
					For Each objFile As AttachmentInfo In lstAttachments
						DeleteItems += objFile.FileName & ";"
					Next
				End If

			Else
				'Delete any files set for deletion
				If DeleteItems <> String.Empty Then

					Dim ParentFolderName As String = PortalSettings.HomeDirectoryMapPath
					ParentFolderName += BaseFolder
					ParentFolderName = ParentFolderName.Replace("/", "\")
					If ParentFolderName.EndsWith("\") = False Then ParentFolderName += "\"

					Dim splitter As Char() = {";"c}
					Dim Array() As String = DeleteItems.Split(splitter)

					For Each item As String In Array
						If item <> String.Empty Then
							DotNetNuke.Common.Utilities.FileSystemUtils.DeleteFile(ParentFolderName & item, PortalSettings, True)
							' Remove the filename from DeleteItems.
							' If it was not deleted, it will be picked up next time
							DeleteItems = DeleteItems.Replace(item & ";", "")
						End If
					Next
				End If

				' For this combination, we need the javascript rewritten on all page loads
				If objConfig.EnableEmoticons = False And AllowInline = True Then
					GenerateJavascript(objConfig.DefaultHtmlEditorProvider.ToLower)
				End If

			End If
		End Sub

		''' <summary>
		''' This uploads a file which generates a GUID name, uses original image extension as save type. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
			Try
				' if no file is selected exit
				If fuFile.PostedFile.FileName = String.Empty Then
					Exit Sub
				End If

				If fuFile.PostedFile.InputStream.Length > (objConfig.MaxAttachmentSize * 1024) Then
					lblMessage.Text = Localization.GetString("MaxFileSize", Me.LocalResourceFile) + objConfig.MaxAttachmentSize.ToString() + " KB"
					Exit Sub
				End If

				Dim FileName As String = System.IO.Path.GetFileName(fuFile.PostedFile.FileName)

				'Get destination folder as mappath
				Dim ParentFolderName As String = PortalSettings.HomeDirectoryMapPath
				ParentFolderName += BaseFolder
				ParentFolderName = ParentFolderName.Replace("/", "\")
				If ParentFolderName.EndsWith("\") = False Then ParentFolderName += "\"

				Dim strExtension As String = Replace(IO.Path.GetExtension(fuFile.PostedFile.FileName), ".", "")
				If FileFilter <> String.Empty And InStr("," & FileFilter.ToLower, "," & strExtension.ToLower) = 0 Then
					' trying to upload a file not allowed for current filter
					lblMessage.Text = String.Format(Localization.GetString("UploadError", Me.LocalResourceFile), FileFilter, strExtension)
				End If

				If lblMessage.Text = String.Empty Then
					Dim destFileName As String = Guid.NewGuid().ToString().Replace("-", "") + "." + strExtension
					lblMessage.Text = FileSystemUtils.UploadFile(ParentFolderName, fuFile.PostedFile, False)

					If lblMessage.Text <> String.Empty Then Exit Sub

					'Rename the file using the GUID model
					FileSystemUtils.MoveFile(ParentFolderName + FileName, ParentFolderName + destFileName, PortalSettings)

					'Now get the FileID from DNN Filesystem
					Dim myFileID As Integer = 0
					Dim fileList As ArrayList = Common.Globals.GetFileList(PortalId, strExtension, False, BaseFolder, False)
					For Each objFile As FileItem In fileList
						If objFile.Text = destFileName Then
							myFileID = CInt(objFile.Value)
						End If
					Next

					If myFileID > 0 Then
						'Now save the Attachment info
						Dim objAttach As New AttachmentInfo
						With objAttach
							.PostID = PostId
							.UserID = UserId
							.FileID = myFileID
							.LocalFileName = FileName
							.Inline = False
						End With

						Dim cntAttachment As New AttachmentController
						cntAttachment.Update(objAttach)
						BindFileList()
					End If
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Deletes the selected uploaded file, using the DNN filesystem 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cmdDelete.Click
			lblMessage.Text = String.Empty
			If lstAttachments.SelectedIndex = -1 Then Exit Sub
			Dim doDelete As Boolean = False
			Try

				'First we need to make sure there is not an inline placement with the current item
				If AllowInline = True Then
					Dim myEditor As DotNetNuke.UI.UserControls.TextEditor
					myEditor = CType(Parent.FindControl("teContent"), DotNetNuke.UI.UserControls.TextEditor)
					If Not myEditor Is Nothing Then
						If myEditor.Text.ToLower.IndexOf("[attachment]" & lstAttachments.SelectedItem.Text.ToLower & "[/attachment]") >= 0 Then
							lblMessage.Text = String.Format(Localization.GetString("DeleteError", Me.LocalResourceFile), lstAttachments.SelectedItem.Text)
							doDelete = False
						Else
							doDelete = True
						End If
					End If
				Else
					doDelete = True
				End If

				'Okay, we can go a head and delete
				If doDelete = True Then

					Dim ParentFolderName As String = PortalSettings.HomeDirectoryMapPath
					ParentFolderName += BaseFolder
					ParentFolderName = ParentFolderName.Replace("/", "\")
					If ParentFolderName.EndsWith("\") = False Then ParentFolderName += "\"
					DotNetNuke.Common.Utilities.FileSystemUtils.DeleteFile(ParentFolderName & lstAttachments.SelectedItem.Value, PortalSettings, True)
					BindFileList()

				End If

			Catch ex As Exception
				' for security reasons, capture the problem but DON'T show the end user.
				LogException(ex)
			End Try
		End Sub

#End Region

#Region " Interfaces "

		''' <summary>
		''' This runs only the first time the control is loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Public Sub LoadInitialView()

			'Not get the host filefilter
			FileFilter = Entities.Host.Host.GetHostSettingsDictionary("FileExtensions").ToString()

			'Do we support inline placement?
			Select Case objConfig.DefaultHtmlEditorProvider.ToLower

				Case "fckhtmleditorprovider", "radeditorprovider"
					AllowInline = True
				Case Else
					'Unsupported provider..
					AllowInline = False
			End Select

			If AllowInline = True Then
				GenerateJavascript(objConfig.DefaultHtmlEditorProvider.ToLower)
				hypInline.Text = Localization.GetString("Inline", Me.LocalResourceFile)
				hypInline.Attributes.Add("onclick", "javascript:AddInlineItem()")
				hypInline.NavigateUrl = "javascript:void(0)"
			End If

			'Bind the lists
			BindFileList()

		End Sub

#End Region

#Region " Private Methods "

		''' <summary>
		''' Binds a list of files to a drop down list. 
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindFileList()

			lstAttachments.Items.Clear()
			ddlInline.Items.Clear()

			Dim cntAttachment As New AttachmentController
			Dim lstFiles As List(Of AttachmentInfo)

			If PostId > 0 Then
				lstFiles = cntAttachment.GetAllByPostID(PostId)
				'Check for "lost" uploads from previous uncompleted posts and add them to the list
				Dim lstTemp As List(Of AttachmentInfo) = cntAttachment.GetAllByUserID(UserId)
				If lstTemp.Count > 0 Then
					lstFiles.AddRange(lstTemp)
				End If
			Else
				'Check for "lost" uploads from previous uncompleted posts and add them to the list
				lstFiles = cntAttachment.GetAllByUserID(UserId)
			End If

			For Each objFile As AttachmentInfo In lstFiles

				Dim lstItem As New ListItem
				lstItem.Text = objFile.LocalFileName
				lstItem.Value = objFile.FileName
				lstAttachments.Items.Add(lstItem)

				lstAttachmentIDs += objFile.AttachmentID.ToString() + ";"

				Dim InlineItem As New ListItem
				InlineItem.Text = objFile.LocalFileName
				InlineItem.Value = objFile.LocalFileName
				ddlInline.Items.Add(InlineItem)

			Next

			'No need to show inline unless there is something to place..
			If AllowInline = True And lstAttachments.Items.Count > 0 Then
				rowInline.Visible = True
				rowInlineHelp.Visible = True
			Else
				rowInline.Visible = False
				rowInlineHelp.Visible = False
			End If

		End Sub

		''' <summary>
		''' Generates the nessary javascript to place attachments inline the post body.
		''' Requires the name of the default HtmlEditorProvider, in order to generate a
		''' working script. 
		''' </summary>
		''' <param name="HtmlProvider"></param>
		''' <remarks></remarks>
		Private Sub GenerateJavascript(ByVal HtmlProvider As String)

			If objConfig.DisableHTMLPosting = True And litScript.Text <> String.Empty Then
				Exit Sub
			End If


			'Are we posting or sending a Private Message?
			Dim strControl As String = String.Empty
			Select Case Request.QueryString("ctl").ToString.ToLower()
				Case "postedit"
					strControl = "Forum_PostEdit"
				Case "pm_edit"
					strControl = "PM_Edit"
			End Select

			'Start javascript
			Dim sb As New StringBuilder
			sb.Append("<script language=""javascript"" type=""text/javascript"">")

			If objConfig.EnableEmoticons = False Then

				' We are alone - get the TextEditor Object
				Dim teContent As DotNetNuke.UI.UserControls.TextEditor
				teContent = CType(Parent.FindControl("teContent"), DotNetNuke.UI.UserControls.TextEditor)

				' Do we really need to continue?
				If EditorMode = teContent.Mode And litScript.Text <> String.Empty Then
					Exit Sub
				End If

				If objConfig.DisableHTMLPosting = True Then
					' Same script regardless the editor provider, we have the 'textarea' object now
					Dim strScript As String = Utilities.ForumUtils.GenerateEditorJavascript(ModuleId, strControl)
					sb.Append(strScript)

					'Now we need to add some attributes to the textarea object
					Dim txtDesktopHTML As TextBox
					txtDesktopHTML = CType(teContent.FindControl("txtDesktopHTML"), TextBox)
					txtDesktopHTML.Attributes.Add("onclick", "storepos(this);")
					txtDesktopHTML.Attributes.Add("onselect", "storepos(this);")
					txtDesktopHTML.Attributes.Add("onkeyup", "storepos(this);")
				Else

					' What Mode is the RichText editor running?
					Select Case teContent.Mode.ToUpper

						Case "RICH"
							' Get the editor instance and add script related to the insert of the emoticon code
							Select Case HtmlProvider
								Case "fckhtmleditorprovider"
									sb.AppendLine("function AddText(code) { var FCK = FCKeditorAPI.GetInstance('dnn_ctr" & CStr(ModuleId) & "_" & strControl & "_teContent_teContent'); FCK.InsertHtml(' ' + code + ' '); }")

								Case "radeditorprovider"
									'dnn_ctr389_EditHTML_teContent_teContent
									sb.AppendLine("function AddText(code) { $find('dnn_ctr" & CStr(ModuleId) & "_" & strControl & "_teContent_teContent').pasteHtml(code); } ")
							End Select

						Case Else
							' User is running BASIC mode, we need a script for that!
							Dim strScript As String = Utilities.ForumUtils.GenerateEditorJavascript(ModuleId, strControl)
							sb.Append(strScript)

							'Now we need to add some attributes to the textarea object
							Dim txtDesktopHTML As TextBox
							txtDesktopHTML = CType(teContent.FindControl("txtDesktopHTML"), TextBox)
							txtDesktopHTML.Attributes.Add("onclick", "storepos(this);")
							txtDesktopHTML.Attributes.Add("onselect", "storepos(this);")
							txtDesktopHTML.Attributes.Add("onkeyup", "storepos(this);")
					End Select

					' Save to ViewState
					EditorMode = teContent.Mode.ToUpper

				End If

			End If

			' Either EmoticonControl is currently present and have written a nice javascript we can use
			' or we just wrote our own. Lets add the function to place the attachment inline.
			sb.Append("function AddInlineItem() { ")
			sb.Append("var ddi = document.getElementById('dnn_ctr" & CStr(ModuleId) & "_" & strControl & "_ctlAttachment_ddlInline').selectedIndex; ")
			sb.Append("if ( ddi != null ) { ")
			sb.Append("var ddv = document.getElementById('dnn_ctr" & CStr(ModuleId) & "_" & strControl & "_ctlAttachment_ddlInline')[ddi].value; ")
			sb.Append("if ( ddv != -1 ) { ")
			sb.Append("AddText(' [attachment]' + ddv + '[/attachment] '); } } }")

			'Close Javascript
			sb.Append("</script>")
			litScript.Text = sb.ToString

		End Sub

#End Region

	End Class

End Namespace