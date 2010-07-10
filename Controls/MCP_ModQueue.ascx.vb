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
			Localization.LocalizeDataGrid(dgModQueue, Me.LocalResourceFile)
			BottomPager.PageSize = Convert.ToInt32(CurrentForumUser.ThreadsPerPage)
			BindData()
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Used to set properties for various sever controls used in the grid's template.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgModQueue_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgModQueue.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As ForumInfo = CType(e.Item.DataItem, ForumInfo)

			Dim img As System.Web.UI.WebControls.Image
			Dim hl As HyperLink
			Dim url As String

			url = Utilities.Links.ContainerPostToModerateLink(TabId, dataItem.ForumID, ModuleId)

			hl = CType(e.Item.FindControl("hlStatus"), HyperLink)
			hl.NavigateUrl = url

			img = CType(e.Item.FindControl("imgStatus"), Image)
			img.ToolTip = Services.Localization.Localization.GetString("imgStatus.Text", LocalResourceFile)
			img.ImageUrl = objConfig.GetThemeImageURL("s_postunread.") & objConfig.ImageExtension

			hl = CType(e.Item.FindControl("hlSubject"), HyperLink)
			hl.Text = dataItem.Name
			hl.NavigateUrl = url
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds a list of forums with posts to moderate that the logged in user has permissions to approve.
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindData()
			Dim ctlModerate As New PostModerationController
			Dim arrPostsToModerate As List(Of ForumInfo)

			arrPostsToModerate = ctlModerate.ModerateForumGetByModeratorThreads(CurrentForumUser.UserID, ModuleId, PortalId)

			If Not arrPostsToModerate Is Nothing Then
				If arrPostsToModerate.Count > 0 Then
					dgModQueue.DataKeyField = "ForumID"
					dgModQueue.DataSource = arrPostsToModerate
					dgModQueue.DataBind()

					BottomPager.TotalRecords = 1
					dgModQueue.Visible = True
					pnlNoItems.Visible = False
					pnlModQueue.Visible = True
				Else
					pnlNoItems.Visible = True
					pnlModQueue.Visible = False
				End If
			Else
				pnlNoItems.Visible = True
				pnlModQueue.Visible = False
			End If
		End Sub

#End Region

	End Class

End Namespace