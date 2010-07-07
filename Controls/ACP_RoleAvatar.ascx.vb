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

Imports DotNetNuke.Modules.Forum.Utilities

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' Page to manage various avatar related settings. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class RoleAvatar
		Inherits ForumModuleBase

#Region "Event Handlers"

		''' <summary>
		''' Run to enable Ajax if the core install has it enabled. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
			End If
		End Sub

		''' <summary>
		''' Runs each time the page is loaded to make sure dynamic controls are created/bound properly. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				If Not Security.IsForumAdmin Then
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Not Page.IsPostBack Then
					Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
					ForumUtils.LoadCssFile(DefaultPage, objConfig)

					BindData()
				End If

				'[skeel] enable menu
				ACPmenu.ControlToLoad = "ACP_RoleAvatar.ascx"
				ACPmenu.PortalID = PortalId
				ACPmenu.objConfig = objConfig
				ACPmenu.ModuleID = ModuleId
				ACPmenu.EnableAjax = False
			Catch ex As Exception
				ProcessModuleLoadException(Me, ex)
			End Try
		End Sub

		''' <summary>
		''' Formats items bound to the group datalist. 
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dlRoleAvatar_ItemDataBound(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlRoleAvatar.ItemDataBound
			Dim item As DataListItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim objTemp As RoleAvatarInfo = CType(item.DataItem, RoleAvatarInfo)
				Dim RoleID As Integer = objTemp.RoleID
				Dim cntRoleAvatar As New RoleAvatarController
				Dim AvatarCount As Integer = cntRoleAvatar.GetAllFromCache(ModuleId, PortalId).Count

				'RoleName
				imgColumnControl = item.Controls(0).FindControl("lblRoleName")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblRoleName As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					lblRoleName.Text = objTemp.RoleName
				End If

				'Avatar
				imgColumnControl = item.Controls(0).FindControl("lblAvatar")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblAvatar As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					If objTemp.Avatar <> String.Empty Then
						lblAvatar.Text = objTemp.Avatar
					Else
						lblAvatar.Text = "&nbsp;"
					End If
				End If

				'Delete Button
				imgColumnControl = item.Controls(0).FindControl("imgDelete")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgDelete As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					If objTemp.Avatar <> String.Empty Then
						imgDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
						imgDelete.ImageUrl = objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
						imgDelete.CommandArgument = objTemp.RoleID.ToString()
					Else
						imgDelete.Visible = False
					End If
				End If

				'Edit Button
				imgColumnControl = item.Controls(0).FindControl("imgEdit")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					If objTemp.Avatar <> String.Empty Then
						imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
						imgEdit.CommandArgument = objTemp.RoleID.ToString()
						imgEdit.CommandName = "Edit"
					Else
						imgEdit.Visible = False
					End If
				End If

				'Add Button
				imgColumnControl = item.Controls(0).FindControl("imgAdd")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgAdd As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					If objTemp.Avatar <> String.Empty Then
						imgAdd.Visible = False
					Else
						imgAdd.ImageUrl = objConfig.GetThemeImageURL("s_add.") & objConfig.ImageExtension
						imgAdd.CommandArgument = objTemp.RoleID.ToString()
						imgAdd.CommandName = "Edit"
					End If
				End If
			ElseIf item.ItemType = ListItemType.EditItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim objTemp As RoleAvatarInfo = CType(item.DataItem, RoleAvatarInfo)
				Dim RoleID As Integer = objTemp.RoleID

				'RoleName
				imgColumnControl = item.Controls(0).FindControl("lblRoleName")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblRoleName As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					lblRoleName.Text = objTemp.RoleName
				End If

				'Avatar control
				imgColumnControl = item.Controls(0).FindControl("ctlRoleAvatar")
				If TypeOf imgColumnControl Is DotNetNuke.Modules.Forum.AvatarControl Then
					Dim ctlRoleAvatar As DotNetNuke.Modules.Forum.AvatarControl = CType(imgColumnControl, DotNetNuke.Modules.Forum.AvatarControl)
					ctlRoleAvatar.Images = ";" & objTemp.Avatar & ";"
					ctlRoleAvatar.AvatarType = AvatarControlType.Role
					Dim objSecurity As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)
					ctlRoleAvatar.Security = objSecurity
					ctlRoleAvatar.LoadInitialView()
				End If

				'Update
				imgColumnControl = item.Controls(0).FindControl("imgUpdate")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgUpdate As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgUpdate.ImageUrl = objConfig.GetThemeImageURL("s_update.") & objConfig.ImageExtension
					imgUpdate.CommandArgument = objTemp.RoleID.ToString()
					imgUpdate.CommandName = "Update"
				End If

				'Cancel
				imgColumnControl = item.Controls(0).FindControl("imgCancel")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgCancel As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgCancel.ImageUrl = objConfig.GetThemeImageURL("s_cancel.") & objConfig.ImageExtension
					imgCancel.CommandArgument = objTemp.RoleID.ToString()
					imgCancel.CommandName = "Cancel"
				End If
			End If
		End Sub

		''' <summary>
		''' The edit command fired from the avatar datalist.
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dlRoleAvatar_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlRoleAvatar.EditCommand
			dlRoleAvatar.EditItemIndex = e.Item.ItemIndex
			dlRoleAvatar.SelectedIndex = -1
			BindData()
		End Sub

		''' <summary>
		''' The cancel command fired from the avatar datalist. 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dlRoleAvatar_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlRoleAvatar.CancelCommand
			dlRoleAvatar.EditItemIndex = -1
			dlRoleAvatar.SelectedIndex = -1
			BindData()
		End Sub

		''' <summary>
		''' The update command event fired from the avatar datalist,
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dlRoleAvatar_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlRoleAvatar.UpdateCommand
			Dim RoleID As Integer = -1
			RoleID = CInt(e.CommandArgument)

			'Avatar control
			Dim imgColumnControl As System.Web.UI.Control
			imgColumnControl = e.Item.Controls(0).FindControl("ctlRoleAvatar")
			If TypeOf imgColumnControl Is DotNetNuke.Modules.Forum.AvatarControl Then
				Dim ctlAvatar As New RoleAvatarController
				Dim ctlRoleAvatar As DotNetNuke.Modules.Forum.AvatarControl = CType(imgColumnControl, DotNetNuke.Modules.Forum.AvatarControl)
				Dim Avatar As String = ctlRoleAvatar.Images.Replace(";", "")
				ctlAvatar.Update(RoleID, Avatar, ModuleId)
			End If

			dlRoleAvatar.EditItemIndex = -1
			dlRoleAvatar.SelectedIndex = -1

			BindData()
		End Sub

		''' <summary>
		''' The delete command event fired from the avatar datalist,
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dlRoleAvatar_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlRoleAvatar.DeleteCommand
			Dim RoleID As Integer = -1
			RoleID = CInt(e.CommandArgument)
			Dim ctlAvatar As New RoleAvatarController
			ctlAvatar.Delete(RoleID, ModuleId)
			dlRoleAvatar.EditItemIndex = -1
			dlRoleAvatar.SelectedIndex = -1
			BindData()
		End Sub

		''' <summary>
		''' Return users back to the Forum Home page.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdHome.Click
			Response.Redirect(NavigateURL(), True)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>Binds the RoleAvatar DataList</summary>
		Private Sub BindData()
			Dim ctlRole As New RoleAvatarController
			Dim Roles As List(Of RoleAvatarInfo) = ctlRole.GetAllFromCache(ModuleId, PortalId)

			dlRoleAvatar.DataSource = Roles
			dlRoleAvatar.DataBind()
		End Sub

#End Region

	End Class

End Namespace