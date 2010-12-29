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

Namespace DotNetNuke.Modules.Forum.MCP

	''' <summary>
	''' This is the MCP section that shows an overview about the mcp.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	12/21/2008	Created
	''' </history>
	Partial Public Class BannedUsers
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Private Members"

		Private _PageSize As Integer

#End Region

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Localization.LocalizeDataGrid(dgBannedUsers, Me.LocalResourceFile)
			BottomPager.PageSize = Convert.ToInt32(CurrentForumUser.ThreadsPerPage)

			BindData(BottomPager.PageSize, 1)
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Used to set properties for various sever controls used in the item template.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgBannedUsers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgBannedUsers.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As ForumUserInfo = CType(e.Item.DataItem, ForumUserInfo)
			Dim hl As HyperLink
			Dim lbl As Label
			Dim img As Image

			Dim objSecurity As New Forum.ModuleSecurity(ModuleId, TabId, -1, CurrentForumUser.UserID)

			If objSecurity.IsForumAdmin Then
				hl = CType(e.Item.FindControl("hlEdit"), HyperLink)
				hl.NavigateUrl = Utilities.Links.UCP_AdminLinks(TabId, ModuleId, dataItem.UserID, UserAjaxControl.Profile)


				img = CType(e.Item.FindControl("imgEdit"), Image)
				img.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
				img.ToolTip = Services.Localization.Localization.GetString("imgEdit.Text", LocalResourceFile)
			End If

			hl = CType(e.Item.FindControl("hlUser"), HyperLink)
			If Not objConfig.EnableExternalProfile Then
				hl.NavigateUrl = dataItem.UserCoreProfileLink
			Else
				hl.NavigateUrl = Utilities.Links.UserExternalProfileLink(dataItem.UserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, dataItem.Username)
			End If

			hl.Target = "_blank"
			hl.Text = dataItem.SiteAlias

			lbl = CType(e.Item.FindControl("lblStartBanDate"), Label)
			lbl.Text = FormatDate(dataItem.StartBanDate)

			lbl = CType(e.Item.FindControl("lblLiftBanDate"), Label)
			lbl.Text = FormatDate(dataItem.LiftBanDate)
		End Sub

		''' <summary>
		''' Used to change the page bound to the grid (via ajax).</summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub pager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BottomPager.Command
			Dim CurrentPage As Int32 = CType(e.CommandArgument, Int32)
			BottomPager.CurrentPage = CurrentPage
			BindData(BottomPager.PageSize, CurrentPage)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds the PM threads for a specific user, if none exist it will hide grid view, show notice to user.
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Private Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim cntUser As New ForumUserController
			Dim arrUsers As New List(Of ForumUserInfo)
			Dim TotalRecords As Integer

			arrUsers = cntUser.GetBannedUsers(PortalId, CurrentPage - 1, PageSize, ModuleId, TotalRecords)

			If Not arrUsers Is Nothing Then
				If arrUsers.Count > 0 Then
					dgBannedUsers.DataKeyField = "UserID"
					dgBannedUsers.DataSource = arrUsers
					dgBannedUsers.DataBind()

					BottomPager.TotalRecords = arrUsers.Count

					pnlBannedUsers.Visible = True
					pnlNoItems.Visible = False
				Else
					pnlBannedUsers.Visible = False
					pnlNoItems.Visible = True
				End If
			Else
				pnlBannedUsers.Visible = False
				pnlNoItems.Visible = True
			End If
		End Sub

		''' <summary>
		''' Formats the start/end dates for banning.
		''' </summary>
		''' <param name="objDate"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function FormatDate(ByVal objDate As Date) As String
			Dim srtDate As String = String.Empty
			srtDate = Utilities.ForumUtils.GetCreatedDateInfo(objDate, objConfig, "Forum_LastPostText")
			Return srtDate
		End Function

#End Region

	End Class

End Namespace