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

<Assembly: WebResource("emoticon.js", "text/javascript")> 
Namespace DotNetNuke.Modules.Forum.WebControls

#Region "EmoticonControl"

	''' <summary>
	''' A subcontrol used to upload, display and select emoticons 
	''' </summary>
	Partial Public Class EmoticonControl
		Inherits ForumModuleBase

#Region " Private Members "

		Private Const FileFilter As String = "jpg,gif,png,zip,txt"
		Private mBaseFolder As String = String.Empty
		Private mlocalResourceFile As String = String.Empty
		Private mForumConfig As Forum.Config
		Private mRICH As Boolean

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
					fileRoot = Me.TemplateSourceDirectory & "/" & Localization.LocalResourceDirectory & "/EmoticonControl.ascx"
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
		''' Defines what to display 
		''' 'User' for displaying emoticons in post edit view 
		''' 'Admin' for adding or editing emoticons in ACP_Emoticon.ascx
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<System.ComponentModel.Bindable(True), System.ComponentModel.Category("Behavior"), System.ComponentModel.DefaultValue("Normal")> Property DisplayType() As String
			Get
				Return ViewState("DisplayType").ToString()
			End Get
			Set(ByVal value As String)
				ViewState("DisplayType") = value
			End Set
		End Property

		''' <summary>
		''' The selected dropdrowlist item 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Image() As String
			Get
				'added to check if no file is uploaded
				If cboFiles.SelectedItem Is Nothing Then Return ""
				Return cboFiles.SelectedItem.Text
			End Get
		End Property

#End Region

#Region " Private ReadOnly Properties "

		''' <summary>
		''' Post portal root folder path setting.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property BaseFolder() As String
			Get
				If mBaseFolder = String.Empty Then
					mBaseFolder = objConfig.EmoticonPath
				End If

				If mBaseFolder.EndsWith("/") = False Then mBaseFolder += "/"

				Return mBaseFolder
			End Get
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

#End Region

#Region " Private Methods "

		''' <summary>
		''' Gets a list of files in a folder. 
		''' </summary>
		''' <param name="NoneSpecified"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetFileList(ByVal NoneSpecified As Boolean) As ArrayList
			Return Common.Globals.GetFileList(PortalId, FileFilter, NoneSpecified, BaseFolder, False)
		End Function

		''' <summary>
		''' Binds a list of files to a drop down list. 
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindFileList()
			cboFiles.Items.Clear()

			Dim arrFiles As ArrayList
			arrFiles = GetFileList(False)

			If arrFiles.Count > 0 Then
				cboFiles.DataSource = GetFileList(False)
				cboFiles.DataBind()
				cboFiles.Visible = True
			Else
				cboFiles.Visible = False
			End If
		End Sub

#End Region

#Region " Event Handlers "

		''' <summary>
		''' Loads the emoticon javascript file (emoticon.js) onto the page.
		''' </summary>
		''' <remarks></remarks>
		Protected Sub Control_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			Me.Page.ClientScript.RegisterClientScriptResource(GetType(EmoticonControl), "emoticon.js")
		End Sub

		''' <summary>
		''' When the control is loaded, we want to make sure the cmdUpload is registered for postback because of the nature of the file upload control and security. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdUpload)
				'If we are showing emoticons for the users, then we enable ajax!
				If DisplayType.ToLower = "user" Then
					DotNetNuke.Framework.AJAX.RegisterScriptManager()
					DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, False)
				End If
			End If

			If DisplayType.ToLower = "admin" Then
				'ACP View
				tblAdmin.Visible = True
				tblUser.Visible = False
			Else
				'Post View
				tblAdmin.Visible = False
				tblUser.Visible = True

				'Bind Emoticon grid and list
				Dim cntEmoticon As New EmoticonController
				lstEmoticon.DataSource = cntEmoticon.GetAll(ModuleId, True)
				lstEmoticon.DataBind()
				dgEmoticon.DataSource = cntEmoticon.GetAll(ModuleId, False)
				dgEmoticon.DataBind()

				'Localization
				hypShowAll.Text = Localization.GetString("ShowAll", Me.LocalResourceFile)

				'Add javascript commands
				hypShowAll.NavigateUrl = "javascript:void(0)"
				hypShowAll.Attributes.Add("onclick", "javascript:EmoticonPopup()")

				'Add nessary javascript
				GenerateJavascript(objConfig.DefaultHtmlEditorProvider.ToLower)
			End If
		End Sub

		''' <summary>
		''' This uploads a file which generates a GUID name, uses original image extension as save type. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
			Try
				' if no file is selected exit
				If fuFile.PostedFile.FileName = String.Empty Then
					Exit Sub
				Else
					' see if we are within file size restrictions
					If fuFile.PostedFile.InputStream.Length > (objConfig.EmoticonMaxFileSize * 1024) Then
						lblMessage.Text = Localization.GetString("MaxFileSize", Me.LocalResourceFile) + objConfig.EmoticonMaxFileSize.ToString() + " KB"
						Exit Sub
					End If
				End If

				Dim ParentFolderName As String = PortalSettings.HomeDirectoryMapPath
				Dim FileName As String = System.IO.Path.GetFileName(fuFile.PostedFile.FileName)

				ParentFolderName += BaseFolder
				ParentFolderName = ParentFolderName.Replace("/", "\")
				If ParentFolderName.EndsWith("\") = False Then ParentFolderName += "\"

				Dim strExtension As String = Replace(IO.Path.GetExtension(fuFile.PostedFile.FileName), ".", "")
				If FileFilter <> String.Empty And InStr("," & FileFilter.ToLower, "," & strExtension.ToLower) = 0 Then
					' trying to upload a file not allowed for current filter
					lblMessage.Text = String.Format(Localization.GetString("UploadError", Me.LocalResourceFile), FileFilter, strExtension)
				End If

				'Are we uploading a pak file?
				Dim unzip As Boolean = False
				If strExtension.ToLower = "zip" Then
					unzip = True
				End If

				If lblMessage.Text = String.Empty Then
					lblMessage.Text += FileSystemUtils.UploadFile(ParentFolderName, fuFile.PostedFile, unzip)
				End If

				'Are we uploading a pak file?
				If unzip = True Then
					If System.IO.File.Exists(ParentFolderName & "emoticons.txt") = True Then
						lblMessage.Text = ImportEmoticonPackage(ParentFolderName, fuFile.FileName)
					Else
						lblMessage.Text = Localization.GetString("MissingPackFile", Me.LocalResourceFile)
					End If

					'Import had an error, but images might have been unzipped
					If lblMessage.Text <> String.Empty Then
						BindFileList()
					Else
						'Reload the emoticon datagrid
						Dim mtabid As Integer = CInt(Request.QueryString("tabid").ToString.ToLower())
						Response.Redirect(Utilities.Links.ACPEmoticonManagerLink(mtabid, ModuleId))
					End If

				Else
					If lblMessage.Text <> String.Empty Then Exit Sub
					BindFileList()
					cboFiles.SelectedIndex = cboFiles.Items.IndexOf(cboFiles.Items.FindByText(FileName))
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Formats items bound to the emoticon datalist. 
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub lstEmoticon_ItemDataBound(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles lstEmoticon.ItemDataBound
			Dim item As DataListItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then

				Dim imgColumnControl As System.Web.UI.Control
				Dim objEmoticon As EmoticonInfo = CType(item.DataItem, EmoticonInfo)

				imgColumnControl = item.Controls(0).FindControl("litEmoticon")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Literal Then
					Dim litEmoticon As Literal = CType(imgColumnControl, System.Web.UI.WebControls.Literal)
					'This is needed because of javascript...
					Dim strCode As String = objEmoticon.Code
					If strCode.StartsWith("\") = True Then
						strCode = "\" & objEmoticon.Code
					End If

					If mRICH = False Then
						strCode = " " & strCode & " "
					End If

					litEmoticon.Text = "<img src=""" & PortalSettings.HomeDirectory & objConfig.EmoticonPath & objEmoticon.Emoticon & """ border=""0"" alt=""" & objEmoticon.ToolTip & """ title=""" & objEmoticon.ToolTip & """ style=""cursor:pointer"" onclick=""javascript:AddText('" & strCode & "');"" />"
				End If

			End If
		End Sub

		''' <summary>
		''' Formats items bound to the emoticon datagrid. 
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgEmoticon_ItemDataBound(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgEmoticon.ItemDataBound
			Dim item As DataGridItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then

				Dim imgColumnControl As System.Web.UI.Control
				Dim objEmoticon As EmoticonInfo = CType(item.DataItem, EmoticonInfo)

				imgColumnControl = item.Controls(0).FindControl("litEmoticon")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Literal Then
					Dim litEmoticon As Literal = CType(imgColumnControl, System.Web.UI.WebControls.Literal)
					'This is needed because of javascript...
					Dim strCode As String = objEmoticon.Code
					If strCode.StartsWith("\") = True Then
						strCode = "\" & objEmoticon.Code
					End If

					If mRICH = False Then
						strCode = " " & strCode & " "
					End If

					litEmoticon.Text = "<img src=""" & PortalSettings.HomeDirectory & objConfig.EmoticonPath & objEmoticon.Emoticon & """ border=""0"" alt=""" & objEmoticon.ToolTip & """ title=""" & objEmoticon.ToolTip & """ style=""cursor:pointer"" onclick=""javascript:AddText('" & strCode & "');"" />"
				End If

			End If
		End Sub

#End Region

#Region " Private Methods "

		''' <summary>
		''' Generates the nessary javascript to place attachments inline the post body.
		''' Requires the name of the default HtmlEditorProvider, in order to generate a
		''' working script. We runs this at every page load because circumstances might have changed 
		''' </summary>
		''' <param name="HtmlProvider"></param>
		''' <remarks></remarks>
		Private Sub GenerateJavascript(ByVal HtmlProvider As String)

			'Are we posting or sending a Private Message?
			Dim strControl As String = String.Empty
			Select Case Request.QueryString("ctl").ToString.ToLower()
				Case "postedit"
					strControl = "Forum_PostEdit"
				Case "pm_edit"
					strControl = "PM_Edit"
			End Select

			'Dialog Start
			Dim sb As New StringBuilder
			sb.Append("<div id=""EmoticonPopup"" class=""d_drag"" style=""position:absolute;display:none;padding-top:10px"">")
			sb.Append("<table cellpadding=""0"" cellspacing=""0"" style=""cursor:default""><tr><td class=""d_tl""></td>")
			sb.Append("<td class=""d_tm""><span class=""d_t"">" & Localization.GetString("AddEmoticon", Me.LocalResourceFile) & "</span></td>")
			sb.Append("<td class=""d_tr""><img src=""" & objConfig.GetThemeImageURL("close.") & objConfig.ImageExtension & """ class=""d_ico"" width=""32"" height=""25""")
			sb.Append(" onmouseover=""this.src='" & objConfig.GetThemeImageURL("close-hover.") & objConfig.ImageExtension & "';""")
			sb.Append(" onmousedown=""this.src='" & objConfig.GetThemeImageURL("close-down.") & objConfig.ImageExtension & "';""")
			sb.Append(" onmouseup=""this.src='" & objConfig.GetThemeImageURL("close-hover.") & objConfig.ImageExtension & "';""")
			sb.Append(" onmouseout=""this.src='" & objConfig.GetThemeImageURL("close.") & objConfig.ImageExtension & "';""")
			sb.Append(" onclick=""javascript:CloseEmoticonPopup();""")
			sb.Append(" title=""" & Localization.GetString("Close", Me.LocalResourceFile) & """ /></td></tr></table>")
			sb.Append("<table cellpadding=""0"" cellspacing=""0""><tr><td class=""d_l""></td><td class=""d_c"">")
			sb.Append("<div style=""height:380px;overflow:auto;width:270px;cursor:default"">")
			litPopupStart.Text = sb.ToString()

			'Dialog End
			sb = New StringBuilder
			sb.Append("</div></td><td class=""d_r""></td></tr></table>")
			sb.Append("<table cellpadding=""0"" cellspacing=""0""><tr><td class=""d_bl""></td><td class=""d_b""></td><td class=""d_br""></td></tr></table>")
			sb.Append("</div>")
			litPopupEnd.Text = sb.ToString

			'Start javascript
			sb = New StringBuilder
			sb.AppendLine("<script language=""javascript"" type=""text/javascript"">")

			' Get the TextEditor Object
			Dim teContent As DotNetNuke.UI.UserControls.TextEditor
			teContent = CType(Parent.FindControl("teContent"), DotNetNuke.UI.UserControls.TextEditor)

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

				mRICH = False
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

						mRICH = True

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

						mRICH = False
				End Select

			End If

			'Close the javascript and add it to the literal 
			sb.AppendLine("</script>")
			litJavaScript.Text = sb.ToString

		End Sub

		''' <summary>
		''' Generates Javascript to display emoticon selected by the dropdownlist (cboFiles)
		''' </summary>
		''' <remarks></remarks>
		Private Sub GenerateAdminJavascript(ByVal SelectedIndex As Integer)

			' only handle preview if any items are present
			If cboFiles.Items.Count = 0 Then Exit Sub


			Dim sb As New StringBuilder
			Dim MyControl As String = String.Empty

			If SelectedIndex > -1 Then
				' we are editing from the grid
				Dim strIndex As String = String.Empty
				If SelectedIndex < 10 Then
					strIndex = "0" & CStr(SelectedIndex)
				Else
					strIndex = CStr(SelectedIndex)
				End If

				MyControl = "dnn_ctr" & CStr(ModuleId) & "_ACP_Emoticon_dgEmoticon_ctl" & strIndex & "_ctlEmoticon_cboFiles"
			Else
				' we are adding a new emoticon
				MyControl = "dnn_ctr" & CStr(ModuleId) & "_ACP_Emoticon_ctlEmoticon_cboFiles"
			End If

			sb.AppendLine("<div class=""Forum_EmoticonPreview""><img id=""EmoticonToSwap"" src=""" & PortalSettings.HomeDirectory & objConfig.EmoticonPath & cboFiles.SelectedItem.Text & """ border=""0"" /></div>")
			sb.AppendLine("<script language=""javascript"" type=""text/javascript"">")
			sb.Append("function swapEmoticon(){ ")
			sb.Append("var img = document.getElementById('EmoticonToSwap'); ")
			sb.Append("var ddl = document.getElementById('" & MyControl & "'); ")
			sb.Append("img.src = '" & PortalSettings.HomeDirectory & objConfig.EmoticonPath & "' + ddl[ddl.selectedIndex].text; ")
			sb.Append("};")
			sb.Append("</script>")

			' Write the script
			litDisplayEmoticon.Text = sb.ToString

			' Add the onchange statemen to the dropdownlist
			cboFiles.Attributes.Add("onchange", "swapEmoticon()")

		End Sub

		''' <summary>
		''' Imports an emoticon configuration text file 
		''' </summary>
		''' <param name="FilePath"></param>
		''' <param name="ZipFile"></param>
		''' <remarks></remarks>
		Private Function ImportEmoticonPackage(ByVal FilePath As String, ByVal ZipFile As String) As String

			Dim strMessage As String = String.Empty

			Try
				Dim strFileName As String = FilePath & "emoticons.txt"
				Dim reader As System.IO.StreamReader = New System.IO.StreamReader(strFileName, System.Text.Encoding.Default)
				Dim line As String = reader.ReadLine()
				Dim cntEmoticon As New EmoticonController

				While (Not line Is Nothing)
					Dim lineInfo As String()
					lineInfo = line.Split(Convert.ToChar(","))

					If Not lineInfo(0) Is Nothing Then

						Dim ImageName As String = CStr(lineInfo(0)).Trim()
						Dim strExtension As String = Replace(IO.Path.GetExtension(ImageName), ".", "")
						If FileFilter <> String.Empty And InStr("," & FileFilter.ToLower, "," & strExtension.ToLower) = 0 Then
							strMessage = Localization.GetString("PackError", Me.LocalResourceFile)
						Else
							'Make sure we have a code defined..
							If Len(CStr(lineInfo(1)).Trim()) >= 2 Then
								Dim objEmoticon As New EmoticonInfo
								With objEmoticon
									.ID = -1
									'Image Name
									.Emoticon = ImageName
									'Code
									.Code = CStr(lineInfo(1)).Trim()
									'ToolTip
									If Len(CStr(lineInfo(2)).Trim()) >= 2 Then
										.ToolTip = CStr(lineInfo(2)).Trim()
									End If
									'SortOrder
									If Len(CStr(lineInfo(3)).Trim()) >= 1 Then
										Try
											Dim SortOrder As Integer = CInt(CStr(lineInfo(3)).Trim())
											.SortOrder = SortOrder
										Catch
										End Try
									End If
									'Default
									If Len(CStr(lineInfo(4)).Trim()) >= 4 Then
										Try
											Dim IsDefault As Boolean = CBool(CStr(lineInfo(4)).Trim())
											.IsDefault = IsDefault
										Catch
										End Try
									End If
									'ModuleId
									.ModuleID = ModuleId
								End With
								cntEmoticon.Update(objEmoticon)
							End If
						End If

					End If

					'Read next line
					line = reader.ReadLine()

				End While

				'CleanUp
				reader.Close()
				reader = Nothing

				'Delete zip and pak file
				Dim arrPaths As System.Array
				arrPaths = System.Array.CreateInstance(GetType(String), 2)
				arrPaths.SetValue(strFileName, 0)
				arrPaths.SetValue(FilePath & ZipFile, 1)
				FileSystemUtils.DeleteFiles(arrPaths)

			Catch ex As Exception
				' for security reasons, capture the problem but DON'T show the end user.
				LogException(ex)
			End Try

			Return strMessage
		End Function

#End Region

#Region " Interfaces "

		''' <summary>
		''' This runs only the first time the control is loaded via Ajax. 
		''' </summary>
		''' <remarks>When called from postview, add ModuleID as SelectedItem</remarks>
		Public Sub LoadInitialView(ByVal SelectedEmoticon As String, ByVal SelectedIndex As Integer)

			If DisplayType.ToLower = "admin" Then
				'The current imagelist is already set by the parent
				BindFileList()

				If SelectedEmoticon <> String.Empty Then
					Try
						cboFiles.Items.FindByText(SelectedEmoticon).Selected = True
					Catch ex As Exception
						'Do nothing
					End Try
				End If

				'Add nessary javascript
				GenerateAdminJavascript(SelectedIndex)

				'Localization
				cmdUpload.Text = Localization.GetString("Upload", Me.LocalResourceFile)

			End If

		End Sub

#End Region

	End Class

#End Region

End Namespace