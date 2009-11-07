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

Namespace DotNetNuke.Modules.Forum.MCP

	''' <summary>
	''' This is the MCP section that shows a list of users who have had posts reported.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class ReportedUsers
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Private Members"

		Private _PageSize As Integer

#End Region

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks>So far this is a static page</remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Localization.LocalizeDataGrid(dgReportedUsers, Me.LocalResourceFile)
			BottomPager.PageSize = Convert.ToInt32(LoggedOnUser.ThreadsPerPage)

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
		Private Sub dgReportedUsers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReportedUsers.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As ReportedUserInfo = CType(e.Item.DataItem, ReportedUserInfo)
			Dim hl As HyperLink
			Dim img As Image

			Dim objSecurity As New Forum.ModuleSecurity(ModuleId, TabId, -1, LoggedOnUser.UserID)

			img = CType(e.Item.FindControl("imgEdit"), Image)

			If objSecurity.IsForumAdmin Then
				hl = CType(e.Item.FindControl("hlEdit"), HyperLink)
				hl.NavigateUrl = Utilities.Links.UCP_AdminLinks(TabId, ModuleId, dataItem.UserID, UserAjaxControl.Profile)

				img.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
				img.ToolTip = Services.Localization.Localization.GetString("imgEdit.Text", LocalResourceFile)
			Else
				img.Visible = False
			End If

			hl = CType(e.Item.FindControl("hlUser"), HyperLink)
			hl.Text = dataItem.Author(ModuleId, PortalId).SiteAlias
			hl.NavigateUrl = Utilities.Links.UserPublicProfileLink(TabId, ModuleId, dataItem.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage)
		End Sub

		''' <summary>
		''' Used to change the page bound to the grid (via ajax).
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub pager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BottomPager.Command
			Dim CurrentPage As Int32 = CType(e.CommandArgument, Int32)
			BottomPager.CurrentPage = CurrentPage
			BindData(BottomPager.PageSize, CurrentPage)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Displays a list of users who have unaddressed reported posts. 
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Private Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim cntReportedUsers As New ReportedUserController
			Dim arrReportedUsers As New List(Of ReportedUserInfo)
			arrReportedUsers = cntReportedUsers.GetReportedUsers(PortalId, CurrentPage - 1, PageSize, BottomPager.TotalRecords)

			If Not arrReportedUsers Is Nothing Then
				If arrReportedUsers.Count > 0 Then
					Dim objReportedUserInfo As ReportedUserInfo = arrReportedUsers.Item(0)

					dgReportedUsers.DataSource = arrReportedUsers
					dgReportedUsers.DataBind()

					BottomPager.TotalRecords = arrReportedUsers.Count

					pnlReportedUsers.Visible = True
					pnlNoItems.Visible = False
				Else
					pnlReportedUsers.Visible = False
					pnlNoItems.Visible = True
				End If
			Else
				pnlReportedUsers.Visible = False
				pnlNoItems.Visible = True
			End If
		End Sub

#End Region

	End Class

End Namespace