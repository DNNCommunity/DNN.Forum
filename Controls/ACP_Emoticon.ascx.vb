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
	''' This is the administrator emoticon control module
	''' </summary>
	''' <remarks></remarks>
	Partial Public Class Emoticon
		Inherits ForumModuleBase

#Region "Event Handlers"

		''' <summary>
		''' Sets the page up for the user to view. 
		''' </summary>
		''' <history>
		''' 	[skeel]	1/2/2009 Created
		''' </history>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				If Not Security.IsForumAdmin Then
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Page.IsPostBack = False Then
					litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"
					ctlEmoticon.LoadInitialView("", -1)

					Localization.LocalizeDataGrid(dgEmoticon, Me.LocalResourceFile)
					imgAdd.ImageUrl = objConfig.GetThemeImageURL("s_add.") & objConfig.ImageExtension

					' Register scripts
					Utilities.ForumUtils.RegisterPageScripts(Page, objConfig)

					BindEmoticonList()
				End If

				'[skeel] enable menu
				ACPmenu.ControlToLoad = "ACP_Emoticon.ascx"
				ACPmenu.PortalID = PortalId
				ACPmenu.objConfig = objConfig
				ACPmenu.ModuleID = ModuleId
				ACPmenu.EnableAjax = False
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Determines the command used on the UI to perform a specific action
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	1/2/2009 Created
		''' </history>
		Protected Sub dgEmoticon_ItemCommand(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEmoticon.ItemCommand
			Try
				' Determine the command of the button 
				Dim cmd As String = String.Empty
				Dim argument As String = String.Empty

				If TypeOf e.CommandSource Is LinkButton Then
					cmd = CType(e.CommandSource, LinkButton).CommandName
					argument = CType(e.CommandSource, LinkButton).CommandArgument
				Else
					cmd = CType(e.CommandSource, ImageButton).CommandName
					argument = CType(e.CommandSource, ImageButton).CommandArgument
				End If

				Dim ID As Integer = -1
				Try
					ID = Int32.Parse(argument)
				Catch ex As Exception
					'Do nothing, required for adding
				End Try

				Dim ctlEmoticon As New EmoticonController

				Select Case cmd.ToLower
					Case "close"
						dgEmoticon.SelectedIndex = -1
					Case "select"
						dgEmoticon.SelectedIndex = e.Item.ItemIndex
						dgEmoticon.EditItemIndex = -1
					Case "delete"
						ctlEmoticon.Delete(ID, ModuleId)
					Case "up"
						ctlEmoticon.SortOrderUpdate(ID, ModuleId, True)
					Case "down"
						ctlEmoticon.SortOrderUpdate(ID, ModuleId, False)
				End Select

				BindEmoticonList()
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' The edit command fired from the group datalist.
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgEmoticon_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEmoticon.EditCommand
			dgEmoticon.EditItemIndex = e.Item.ItemIndex
			dgEmoticon.SelectedIndex = -1
			BindEmoticonList()
		End Sub

		''' <summary>
		''' The cancel command fired from the group datalist. 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgEmoticon_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEmoticon.CancelCommand
			dgEmoticon.EditItemIndex = -1
			dgEmoticon.SelectedIndex = -1
			BindEmoticonList()
		End Sub

		''' <summary>
		''' The update command event fired from the group datalist,
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgEmoticon_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgEmoticon.UpdateCommand
			Dim cntEmoticon As New EmoticonController
			Dim objEmoticon As New EmoticonInfo

			Dim imgColumnControl As System.Web.UI.Control

			objEmoticon.ID = CInt(e.CommandArgument)

			'Load up the ddl with available image files
			imgColumnControl = e.Item.Controls(0).FindControl("ctlEmoticon")
			If TypeOf imgColumnControl Is WebControls.EmoticonControl Then
				Dim ctlEmoticon As WebControls.EmoticonControl = CType(imgColumnControl, WebControls.EmoticonControl)
				objEmoticon.Emoticon = ctlEmoticon.Image
			End If

			imgColumnControl = e.Item.Controls(0).FindControl("txtCode")
			If TypeOf imgColumnControl Is System.Web.UI.WebControls.TextBox Then
				Dim txtCode As TextBox = CType(imgColumnControl, System.Web.UI.WebControls.TextBox)
				'We need to exclude these:
				Dim strCode As String = txtCode.Text.Trim
				strCode = strCode.Replace("'", "")
				strCode = strCode.Replace("""", "")
				objEmoticon.Code = strCode
			End If

			imgColumnControl = e.Item.Controls(0).FindControl("txtToolTip")
			If TypeOf imgColumnControl Is System.Web.UI.WebControls.TextBox Then
				Dim txtToopTip As TextBox = CType(imgColumnControl, System.Web.UI.WebControls.TextBox)
				objEmoticon.ToolTip = txtToopTip.Text.Trim
			End If

			imgColumnControl = e.Item.Controls(0).FindControl("chkDefault")
			If TypeOf imgColumnControl Is System.Web.UI.WebControls.CheckBox Then
				Dim chkDefault As CheckBox = CType(imgColumnControl, System.Web.UI.WebControls.CheckBox)
				objEmoticon.IsDefault = chkDefault.Checked
			End If

			objEmoticon.SortOrder = e.Item.ItemIndex
			objEmoticon.ModuleID = ModuleId

			cntEmoticon.Update(objEmoticon)

			dgEmoticon.EditItemIndex = -1
			dgEmoticon.SelectedIndex = -1

			BindEmoticonList()
		End Sub

		''' <summary>
		''' Formats items bound to the Emoticon datalist. 
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgEmoticon_ItemDataBound(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgEmoticon.ItemDataBound
			Dim item As DataGridItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim objEmoticon As EmoticonInfo = CType(item.DataItem, EmoticonInfo)
				Dim cntEmoticon As New EmoticonController
				Dim Count As Integer = dgEmoticon.Items.Count

				imgColumnControl = item.Controls(0).FindControl("litEmoticon")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Literal Then
					Dim litEmoticon As Literal = CType(imgColumnControl, System.Web.UI.WebControls.Literal)
					litEmoticon.Text = objEmoticon.EmoticonHTML
				End If

				imgColumnControl = item.Controls(0).FindControl("lblCode")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblCode As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					lblCode.Text = objEmoticon.Code
				End If

				imgColumnControl = item.Controls(0).FindControl("lblToolTip")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblToolTip As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					lblToolTip.Text = "&nbsp;" & objEmoticon.ToolTip
				End If

				imgColumnControl = item.Controls(0).FindControl("chkDefault")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.CheckBox Then
					Dim chkDefault As CheckBox = CType(imgColumnControl, System.Web.UI.WebControls.CheckBox)
					chkDefault.Checked = objEmoticon.IsDefault
				End If

				imgColumnControl = item.Controls(0).FindControl("imgDelete")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgDelete As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
					imgDelete.ImageUrl = objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
					imgDelete.CommandArgument = objEmoticon.ID.ToString()
				End If

				imgColumnControl = item.Controls(0).FindControl("imgEdit")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objEmoticon.ID.ToString()
					imgEdit.CommandName = "Edit"
				End If

				imgColumnControl = item.Controls(0).FindControl("imgUp")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_up.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objEmoticon.ID.ToString()
					If objEmoticon.SortOrder < 1 Then
						imgEdit.Enabled = False
					End If
				End If

				imgColumnControl = item.Controls(0).FindControl("imgDown")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_down.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objEmoticon.ID.ToString()
					If Count = objEmoticon.SortOrder + 1 Then
						imgEdit.Enabled = False
					End If
				End If

				'EditItem starts here!
			ElseIf item.ItemType = ListItemType.EditItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim objEmoticon As EmoticonInfo = CType(item.DataItem, EmoticonInfo)
				Dim cntEmoticon As New EmoticonController

				'Load up the ddl with available image files
				imgColumnControl = item.Controls(0).FindControl("ctlEmoticon")
				If TypeOf imgColumnControl Is WebControls.EmoticonControl Then
					Dim ctlEmoticon As WebControls.EmoticonControl = CType(imgColumnControl, WebControls.EmoticonControl)
					ctlEmoticon.LoadInitialView(objEmoticon.Emoticon, (item.ItemIndex + 2))
				End If

				imgColumnControl = item.Controls(0).FindControl("txtCode")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.TextBox Then
					Dim txtCode As TextBox = CType(imgColumnControl, System.Web.UI.WebControls.TextBox)
					txtCode.Text = objEmoticon.Code
				End If

				imgColumnControl = item.Controls(0).FindControl("txtToolTip")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.TextBox Then
					Dim txtToopTip As TextBox = CType(imgColumnControl, System.Web.UI.WebControls.TextBox)
					txtToopTip.Text = objEmoticon.ToolTip
				End If

				imgColumnControl = item.Controls(0).FindControl("chkDefault")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.CheckBox Then
					Dim chkDefault As CheckBox = CType(imgColumnControl, System.Web.UI.WebControls.CheckBox)
					chkDefault.Checked = objEmoticon.IsDefault
				End If

				imgColumnControl = item.Controls(0).FindControl("imgUpdate")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_update.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objEmoticon.ID.ToString()
					imgEdit.CommandName = "Update"
				End If

				imgColumnControl = item.Controls(0).FindControl("imgCancel")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_cancel.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objEmoticon.ID.ToString()
					imgEdit.CommandName = "Cancel"
				End If
			End If

		End Sub

		''' <summary>
		''' Navigates the user to add a new forum
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdAdd_Click(ByVal Sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAdd.Click
			Dim cntEmoticon As New EmoticonController
			Dim objEmoticon As New EmoticonInfo

			If txtCode.Text.Trim() = String.Empty Then
				Exit Sub
			End If

			'added to check if image is uploaded
			If ctlEmoticon.Image = String.Empty Then
				Exit Sub
			End If

			With objEmoticon
				.ID = -1
				.Emoticon = ctlEmoticon.Image
				.Code = txtCode.Text.Trim()
				.ToolTip = txtToolTip.Text
				.IsDefault = True
				.ModuleID = ModuleId
				.SortOrder = dgEmoticon.Items.Count + 1	'new emoticons goes last
			End With

			cntEmoticon.Update(objEmoticon)
			BindEmoticonList()

			txtCode.Text = String.Empty
			txtToolTip.Text = String.Empty
		End Sub

#End Region

#Region " Private Methods "

		''' <summary>
		''' Binds the list of default emoticons if any are available
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindEmoticonList()
			Dim ctlEmoticon As New EmoticonController
			Dim Emoticons As List(Of EmoticonInfo) = ctlEmoticon.GetAll(ModuleId, False)

			If Emoticons.Count > 0 Then
				dgEmoticon.DataSource = Emoticons
				dgEmoticon.DataBind()
				dgEmoticon.Visible = True
			Else
				dgEmoticon.Visible = False
			End If
		End Sub

#End Region

	End Class

End Namespace