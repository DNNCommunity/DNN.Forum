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

Imports Telerik.Web.UI

Namespace DotNetNuke.Modules.Forum.MCP

	''' <summary>
	''' This is the MCP section that shows a list of forums that have posts to moderate.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class ModQueue
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
			BindData()
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Runs when the control is initialized, even before anything in LoadInitialView runs. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>All controls containing grids should localize the grid headers here. </remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			SetLocalization()
		End Sub

		''' <summary>
		''' Alters data as it is bound to the grid.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub gridPostsToModerate_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gridPostsToModerate.ItemDataBound
			If TypeOf e.Item Is GridDataItem Then
				Dim item As GridDataItem = CType(e.Item, GridDataItem)
				Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ForumID"))

				Dim hlForum As HyperLink = CType((item)("hlName").Controls(0), HyperLink)
				hlForum.NavigateUrl = Utilities.Links.ContainerPostToModerateLink(TabId, keyForumID, ModuleId)
			End If
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds a list of forums with posts to moderate that the logged in user has permissions to approve.
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindData()
			Dim ctlModerate As New PostModerationController
			Dim colForums As List(Of ForumInfo)

			colForums = ctlModerate.ModerateForumGetByModeratorThreads(CurrentForumUser.UserID, ModuleId, PortalId)

			gridPostsToModerate.DataSource = colForums
			gridPostsToModerate.DataBind()

			If colForums.Count > 0 Then
				gridPostsToModerate.VirtualItemCount = colForums(0).TotalRecords
			Else
				gridPostsToModerate.VirtualItemCount = 0
			End If
		End Sub

		''' <summary>
		''' Localizes the data grid headers for all grids on the page (that utilize Telerik).
		''' </summary>
		''' <remarks></remarks>
		Private Sub SetLocalization()
			For Each gc As GridColumn In gridPostsToModerate.MasterTableView.Columns
				If gc.HeaderText <> "" Then
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile)
				End If
			Next
		End Sub

#End Region

	End Class

End Namespace