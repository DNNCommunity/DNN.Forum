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
	''' From this page you can re-order forums and groups, navigate to edit/add forums/groups.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class ForumManage
		Inherits ForumModuleBase

#Region "Event Handlers"

		''' <summary>
		''' Determines if we can use Ajax in this page before anything is rendered. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, True)
			End If
		End Sub

		'''' <summary>
		'''' Sets the page up for the user to view. 
		'''' </summary>
		'''' <param name="sender"></param>
		'''' <param name="e"></param>
		'''' <remarks>
		'''' </remarks>
		'''' <history>
		'''' 	[cpaterra]	7/13/2005	Created
		'''' </history>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				If Not Security.IsForumAdmin Then
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Page.IsPostBack = False Then
					litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

					' Register scripts
					Utilities.ForumUtils.RegisterPageScripts(Page, objConfig)

					imgAddGroup.ImageUrl = objConfig.GetThemeImageURL("s_add.") & objConfig.ImageExtension
					imgAddGroup.ToolTip = Localization.GetString("AddGroup", LocalResourceFile)
					BindGroupList()
				End If

				'[skeel] enable menu
				ACPmenu.ControlToLoad = "ACP_ForumManage.ascx"
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
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub lstGroup_ItemCommand(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles lstGroup.ItemCommand
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

				Dim groupID As Integer = Int32.Parse(argument)
				Dim cntGroup As New GroupController
				Dim objGroup As GroupInfo = cntGroup.GetCachedGroup(groupID)

				Select Case cmd.ToLower
					Case "close"
						lstGroup.SelectedIndex = -1
					Case "select"
						lstGroup.SelectedIndex = e.Item.ItemIndex
						lstGroup.EditItemIndex = -1
					Case "delete"
						cntGroup.GroupDelete(groupID, ModuleId)
					Case "up"
						cntGroup.GroupSortOrderUpdate(groupID, True)
					Case "down"
						cntGroup.GroupSortOrderUpdate(groupID, False)
					Case "add"
						Utilities.Links.ForumEditLink(TabId, ModuleId, -1, groupID)
				End Select
				cntGroup.ResetAllGroupsByModuleID(ModuleId)
				BindGroupList()
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
		Protected Sub lstGroup_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles lstGroup.EditCommand
			lstGroup.EditItemIndex = e.Item.ItemIndex
			lstGroup.SelectedIndex = -1

			BindGroupList()
		End Sub

		''' <summary>
		''' The cancel command fired from the group datalist. 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub lstGroup_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles lstGroup.CancelCommand
			lstGroup.EditItemIndex = -1
			lstGroup.SelectedIndex = -1

			BindGroupList()
		End Sub

		''' <summary>
		''' The update command event fired from the group datalist,
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub lstGroup_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles lstGroup.UpdateCommand
			Dim GroupID As Integer = -1
			GroupID = CInt(e.CommandArgument)

			Dim cntGroup As New GroupController
			Dim objGroup As GroupInfo
			objGroup = cntGroup.GetCachedGroup(GroupID)

			Dim txtGroupName As TextBox = CType(e.Item.Controls(0).FindControl("txtGroupName"), TextBox)

			cntGroup.GroupUpdate(CType(GroupID, Integer), txtGroupName.Text, UserId, objGroup.SortOrder, ModuleId)

			' Reset the module groups
			Dim objGrpCnt As New GroupController
			objGrpCnt.ResetAllGroupsByModuleID(ModuleId)

			' Remove the updated group from cache
			cntGroup.ResetGroupCache(CType(GroupID, Integer))

			lstGroup.EditItemIndex = -1
			lstGroup.SelectedIndex = -1

			BindGroupList()
		End Sub

		''' <summary>
		''' Formats items bound to the group datalist. 
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub lstGroup_ItemDataBound(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles lstGroup.ItemDataBound
			Dim item As DataListItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim objTempGroup As GroupInfo = CType(item.DataItem, GroupInfo)
				Dim GroupID As Integer = objTempGroup.GroupID
				Dim cntGroup As New GroupController
				Dim GroupCount As Integer = cntGroup.GroupsGetByModuleID(ModuleId).Count

				Dim objGroup As New GroupInfo
				objGroup = cntGroup.GetCachedGroup(GroupID)

				imgColumnControl = item.Controls(0).FindControl("imgExpand")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgExpand As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgExpand.ImageUrl = objConfig.GetThemeImageURL("s_folder.") & objConfig.ImageExtension
					imgExpand.CommandArgument = objGroup.GroupID.ToString()
					imgExpand.CommandName = "select"
				End If

				imgColumnControl = item.Controls(0).FindControl("lblGroupName")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblGroupName As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)

					lblGroupName.Text = objGroup.Name
				End If

				imgColumnControl = item.Controls(0).FindControl("lblForumCount")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblForumCount As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)

					lblForumCount.Text = "(" + objGroup.ForumCount.ToString() + ")"
				End If

				imgColumnControl = item.Controls(0).FindControl("lblCreatedDate")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblCreatedDate As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)

					lblCreatedDate.Text = objGroup.CreatedDate.ToShortDateString()
				End If

				imgColumnControl = item.Controls(0).FindControl("imgGroupDelete")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgDelete As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
					imgDelete.ImageUrl = objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
					imgDelete.CommandArgument = objGroup.GroupID.ToString()
					If objGroup.ForumCount > 0 Then
						imgDelete.Visible = False
					Else
						imgDelete.Visible = True
					End If
				End If

				imgColumnControl = item.Controls(0).FindControl("imgGroupEdit")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objGroup.GroupID.ToString()
					imgEdit.CommandName = "Edit"
				End If

				imgColumnControl = item.Controls(0).FindControl("imgGroupUp")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_up.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objGroup.GroupID.ToString()

					If objGroup.SortOrder < 1 Then
						imgEdit.Enabled = False
					End If
				End If

				imgColumnControl = item.Controls(0).FindControl("imgGroupDown")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_down.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objGroup.GroupID.ToString()

					If GroupCount = objGroup.SortOrder + 1 Then
						imgEdit.Enabled = False
					End If
				End If
			ElseIf item.ItemType = ListItemType.EditItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim objTempGroup As GroupInfo = CType(item.DataItem, GroupInfo)
				Dim GroupID As Integer = objTempGroup.GroupID
				Dim cntGroup As New GroupController

				Dim objGroup As New GroupInfo
				objGroup = cntGroup.GetCachedGroup(GroupID)

				imgColumnControl = item.Controls(0).FindControl("imgHeadSpacerL")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgHeadSpacer As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)

					imgHeadSpacer.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				End If

				imgColumnControl = item.Controls(0).FindControl("imgHeadSpacerR")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgHeadSpacer As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)

					imgHeadSpacer.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				End If

				imgColumnControl = item.Controls(0).FindControl("txtGroupName")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.TextBox Then
					Dim lblGroupName As TextBox = CType(imgColumnControl, System.Web.UI.WebControls.TextBox)

					lblGroupName.Text = objGroup.Name
				End If

				imgColumnControl = item.Controls(0).FindControl("imgGroupSave")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_update.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objGroup.GroupID.ToString()
					imgEdit.CommandName = "Update"
				End If

				imgColumnControl = item.Controls(0).FindControl("imgGroupCancel")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_cancel.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objGroup.GroupID.ToString()
					imgEdit.CommandName = "Cancel"
				End If
			End If

			If item.ItemType = ListItemType.SelectedItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim dataListControl As System.Web.UI.Control
				Dim objTempGroup As GroupInfo = CType(item.DataItem, GroupInfo)
				Dim GroupID As Integer = objTempGroup.GroupID
				Dim cntGroup As New GroupController

				Dim objGroup As New GroupInfo
				objGroup = cntGroup.GetCachedGroup(GroupID)

				imgColumnControl = item.Controls(0).FindControl("imgCloseGroup")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgExpand As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgExpand.ImageUrl = objConfig.GetThemeImageURL("s_folderopen.") & objConfig.ImageExtension
					imgExpand.CommandArgument = objGroup.GroupID.ToString()
					imgExpand.CommandName = "close"
				End If

				imgColumnControl = item.Controls(0).FindControl("imgHeadSpacerL")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgHeadSpacer As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)

					imgHeadSpacer.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				End If

				imgColumnControl = item.Controls(0).FindControl("imgHeadSpacerR")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgHeadSpacer As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)

					imgHeadSpacer.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				End If

				imgColumnControl = item.Controls(0).FindControl("lblGroupName")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblGroupName As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)

					lblGroupName.Text = objGroup.Name
				End If

				imgColumnControl = item.Controls(0).FindControl("imgForumAdd")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgAddForum As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgAddForum.ImageUrl = objConfig.GetThemeImageURL("s_add.") & objConfig.ImageExtension
					imgAddForum.AlternateText = Localization.GetString("AddForum", LocalResourceFile)
					imgAddForum.ToolTip = Localization.GetString("AddForum", LocalResourceFile)
					imgAddForum.CommandArgument = objGroup.GroupID.ToString()
				End If

				dataListControl = e.Item.Controls(0).FindControl("lstForum")
				If TypeOf dataListControl Is System.Web.UI.WebControls.DataList Then
					Dim arrForums As List(Of ForumInfo)
					arrForums = BindForums(0, objGroup.GroupID)

					Dim dlForums As DataList = CType(dataListControl, System.Web.UI.WebControls.DataList)

					dlForums.DataSource = arrForums
					dlForums.DataBind()
				End If
			End If
		End Sub

		''' <summary>
		''' Formats the items bound to the forum datalist. 
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub lstForum_ItemDataBound(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
			Dim item As DataListItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.EditItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim objForum As ForumInfo = CType(item.DataItem, ForumInfo)

				Dim GroupID As Integer = objForum.GroupID
				Dim cntGroup As New GroupController
				Dim objGroup As New GroupInfo
				objGroup = cntGroup.GetCachedGroup(GroupID)

				imgColumnControl = item.Controls(0).FindControl("imgSpacer")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgSpacer As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)

					imgSpacer.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				End If

				imgColumnControl = item.Controls(0).FindControl("lblForumName")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblForumName As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					lblForumName.Text = objForum.Name
				End If

				imgColumnControl = item.Controls(0).FindControl("lblTotalPosts")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblForumName As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)

					lblForumName.Text = objForum.TotalPosts.ToString()
				End If

				imgColumnControl = item.Controls(0).FindControl("imgEnabled")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgForumEnable As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)

					imgForumEnable.ImageUrl = EnabledImage(objForum.ForumID)
					imgForumEnable.ToolTip = EnabledImageText(objForum.ForumID)
					imgForumEnable.AlternateText = EnabledImageText(objForum.ForumID)
				End If

				imgColumnControl = item.Controls(0).FindControl("imgForumEdit")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objForum.ForumID.ToString()
				End If

				imgColumnControl = item.Controls(0).FindControl("imgForumDelete")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgDelete As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
					imgDelete.ImageUrl = objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
					imgDelete.CommandArgument = objForum.ParentId.ToString() + "|" + objForum.GroupID.ToString() + "|" + objForum.ForumID.ToString()
				End If

				imgColumnControl = item.Controls(0).FindControl("imgForumUp")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_up.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objForum.ForumID.ToString()

					If objForum.SortOrder < 1 Then
						imgEdit.Enabled = False
					End If
				End If

				imgColumnControl = item.Controls(0).FindControl("imgForumDown")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_down.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objForum.ForumID.ToString()

					If (objGroup.ForumCount - objForum.SubForums) = objForum.SortOrder + 1 Then
						imgEdit.Enabled = False
					End If
				End If

				'[Skeel] load subforums
				Dim dataListControl As System.Web.UI.Control
				dataListControl = e.Item.Controls(0).FindControl("lstSubForum")
				If TypeOf dataListControl Is System.Web.UI.WebControls.DataList Then
					Dim arrSubForums As List(Of ForumInfo)
					arrSubForums = BindForums(objForum.ForumID, objGroup.GroupID)

					Dim dlSubForums As DataList = CType(dataListControl, System.Web.UI.WebControls.DataList)
					dlSubForums.DataSource = arrSubForums
					dlSubForums.DataBind()
				End If

			End If
		End Sub

		''' <summary>
		''' Formats the items bound to the subforum datalist. 
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub lstSubForum_ItemDataBound(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs)
			Dim item As DataListItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.EditItem Then
				Dim imgColumnControl As System.Web.UI.Control
				Dim objForum As ForumInfo = CType(item.DataItem, ForumInfo)

				imgColumnControl = item.Controls(0).FindControl("imgSubSpacer")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgSpacer As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)
					imgSpacer.ImageUrl = objConfig.GetThemeImageURL("alt_headfoot_height.gif")
				End If

				imgColumnControl = item.Controls(0).FindControl("lblSubForumName")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblForumName As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					lblForumName.Text = objForum.Name
				End If

				imgColumnControl = item.Controls(0).FindControl("lblSubTotalPosts")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblForumName As Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					lblForumName.Text = objForum.TotalPosts.ToString()
				End If

				imgColumnControl = item.Controls(0).FindControl("imgSubEnabled")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgForumEnable As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)
					imgForumEnable.ImageUrl = EnabledImage(objForum.ForumID)
					imgForumEnable.ToolTip = EnabledImageText(objForum.ForumID)
					imgForumEnable.AlternateText = EnabledImageText(objForum.ForumID)
				End If

				imgColumnControl = item.Controls(0).FindControl("imgSubForumEdit")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objForum.ForumID.ToString()
				End If

				imgColumnControl = item.Controls(0).FindControl("imgSubForumDelete")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgDelete As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
					imgDelete.ImageUrl = objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
					imgDelete.CommandArgument = objForum.ParentId.ToString() + "|" + objForum.GroupID.ToString() + "|" + objForum.ForumID.ToString()
				End If

				imgColumnControl = item.Controls(0).FindControl("imgSubForumUp")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_up.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objForum.ForumID.ToString()

					If objForum.SortOrder < 1 Then
						imgEdit.Enabled = False
					End If
				End If

				imgColumnControl = item.Controls(0).FindControl("imgSubForumDown")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgEdit As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)
					imgEdit.ImageUrl = objConfig.GetThemeImageURL("s_down.") & objConfig.ImageExtension
					imgEdit.CommandArgument = objForum.ForumID.ToString()

					If objForum.ParentForum.SubForums = objForum.SortOrder + 1 Then
						imgEdit.Enabled = False
					End If
				End If

				'[skeel] handle level images here
				imgColumnControl = item.Controls(0).FindControl("imgLevel")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim imgLevel As Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)
					imgLevel.ImageUrl = objConfig.GetThemeImageURL("sublevel.") & objConfig.ImageExtension
				End If

			End If
		End Sub

		''' <summary>
		''' Redirects the User to Edit a forum
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub EditForum_Click(ByVal Sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
			Try
				Dim ForumID As Integer = Int32.Parse(CType(Sender, ImageButton).CommandArgument)
				Response.Redirect(Utilities.Links.ForumEditLink(TabId, ModuleId, ForumID, -1), False)
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Deletes a forum from the database
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub DeleteForum_Click(ByVal Sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
			Try
				Dim str As String = CType(Sender, ImageButton).CommandArgument
				Dim ParentID As Integer = Convert.ToInt32(Split(str, "|")(0))
				Dim GroupID As Integer = Convert.ToInt32(Split(str, "|")(1))
				Dim ForumID As Integer = Convert.ToInt32(Split(str, "|")(2))
				Dim ctlForum As New ForumController
				ctlForum.ForumDelete(ParentID, GroupID, ForumID, ModuleId)

				BindGroupList()
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Move a forum up one in order within a group
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub ForumUp_Click(ByVal Sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
			Try
				Dim forumID As Integer = Int32.Parse(CType(Sender, ImageButton).CommandArgument)
				Dim cntForum As New ForumController
				Dim forum As ForumInfo = cntForum.GetForumInfoCache(forumID)

				Dim ctlForum As New ForumController
				ctlForum.ForumSortOrderUpdate(forum.ParentId, forum.GroupID, forumID, True)

				BindGroupList()
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Move a forum down one in order within a group
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub ForumDown_Click(ByVal Sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
			Try
				Dim forumID As Integer = Int32.Parse(CType(Sender, ImageButton).CommandArgument)
				Dim cntForum As New ForumController
				Dim forum As ForumInfo = cntForum.GetForumInfoCache(forumID)
				Dim GroupID As Integer = forum.GroupID
				Dim ctlForum As New ForumController
				ctlForum.ForumSortOrderUpdate(forum.ParentId, forum.GroupID, forumID, False)

				BindGroupList()
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Navigates the user to add a new forum
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub AddForum_Click(ByVal Sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
			Try
				Dim GroupID As Integer = Int32.Parse(lstGroup.DataKeys.Item(lstGroup.SelectedIndex).ToString)

				Response.Redirect(Utilities.Links.ForumEditLink(TabId, ModuleId, -1, GroupID), False)
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Adds a group (If valid) and rebinds the datalist to display the newly added group. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub imgAddGroup_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAddGroup.Click
			If txtAddGroup.Text <> String.Empty Then
				Dim ctlGroup As New GroupController

				Dim GroupID As Integer = -1
				GroupID = ctlGroup.GroupAdd(txtAddGroup.Text, PortalId, ModuleId, UserId)

				' Reset the module groups
				Dim objGrpCnt As New GroupController
				objGrpCnt.ResetAllGroupsByModuleID(ModuleId)

				' Re-bind
				lblvalAddGroup.Visible = False
				txtAddGroup.Text = String.Empty

				lstGroup.SelectedIndex = -1
				lstGroup.EditItemIndex = -1

				BindGroupList()
			Else
				lblvalAddGroup.Visible = True
			End If
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Sets the enabled/disabled image for a theme
		''' </summary>
		''' <param name="ForumID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function EnabledImage(ByVal ForumID As Integer) As String
			Dim cntForum As New ForumController
			Dim forum As ForumInfo = cntForum.GetForumInfoCache(ForumID)
			Dim GroupID As Integer = forum.GroupID
			Dim ctlForum As New ForumController
			Dim imagepath As String

			If forum.IsActive Then
				imagepath = objConfig.GetThemeImageURL("checked.") & objConfig.ImageExtension
			Else
				imagepath = objConfig.GetThemeImageURL("unchecked.") & objConfig.ImageExtension
			End If

			Return imagepath
		End Function

		''' <summary>
		''' Sets the enabled/disabled text for a theme.
		''' </summary>
		''' <param name="ForumID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function EnabledImageText(ByVal ForumID As Integer) As String
			Dim cntForum As New ForumController
			Dim forum As ForumInfo = cntForum.GetForumInfoCache(ForumID)
			Dim GroupID As Integer = forum.GroupID
			Dim ctlForum As New ForumController
			Dim imagetext As String

			If forum.IsActive Then
				imagetext = Localization.GetString("imgEnabled", LocalResourceFile)
			Else
				imagetext = Localization.GetString("imgDisabled", LocalResourceFile)
			End If

			Return imagetext
		End Function

		''' <summary>
		''' Binds the list of groups and the list of forums if any are available
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindGroupList()
			Dim ctlGroup As New GroupController
			Dim Group As List(Of GroupInfo) = ctlGroup.GroupsGetByModuleID(ModuleId)

			lstGroup.DataSource = Group
			lstGroup.DataBind()
		End Sub

		''' <summary>
		''' Binds a single forum instance
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function BindForums(ByVal ParentID As Integer, ByVal GroupID As Integer) As List(Of ForumInfo)
			Dim ctlForum As New ForumController
			Dim forumCollection As List(Of ForumInfo)

			forumCollection = ctlForum.ForumGetAllByParentID(ParentID, GroupID, False)
			Dim test As Integer = forumCollection.Count

			Return forumCollection
		End Function

		''' <summary>
		''' Formats the user alias for who edit/added the forum
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function FormatUser(ByVal UserID As Object) As String
			If Not IsDBNull(UserID) Then
				Dim cntForumUser As New ForumUserController
				Dim user As ForumUser = cntForumUser.GetForumUser(CType(UserID, Integer), False, ModuleId, PortalId)
				Return user.Username
			Else
				Return "Anonymous"
			End If
		End Function

		''' <summary>
		''' Sets the height spacer image. 
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function SpacerImage() As String
			Return objConfig.GetThemeImageURL("headfoot_height.gif")
		End Function

#End Region

	End Class

End Namespace