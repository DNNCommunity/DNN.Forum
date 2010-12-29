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

Namespace DotNetNuke.Modules.Forum.UCP

	''' <summary>
	''' This is the UCP section that show the list of threads the user have bookmarked
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class Bookmark
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			BindData(CurrentForumUser.ThreadsPerPage, 0)
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
		Protected Sub gridBookmarks_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gridBookmarks.ItemDataBound
			If TypeOf e.Item Is GridDataItem Then
				Dim item As GridDataItem = CType(e.Item, GridDataItem)
				Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ForumID"))
				Dim keyMostRecentPostID As Integer = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("MostRecentPostID"))
				Dim keyThreadID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ThreadID"))

				Dim imgDelete As ImageButton = CType((item)("imgDelete").Controls(0), ImageButton)
				imgDelete.ToolTip = Localization.GetString("Delete", LocalResourceFile)
				imgDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');")
				imgDelete.CommandArgument = keyForumID.ToString()

				Dim hlForum As HyperLink = CType((item)("hlName").Controls(0), HyperLink)
				hlForum.NavigateUrl = Utilities.Links.ContainerViewThreadLink(TabId, keyForumID, keyThreadID)

				Dim hlLastPost As HyperLink = CType((item)("hlLastPost").Controls(0), HyperLink)
				Dim cntPost As New PostController
				If keyMostRecentPostID > 0 Then
					Dim objPost As PostInfo = cntPost.GetPostInfo(keyMostRecentPostID, PortalId)
					hlLastPost.Text = Utilities.ForumUtils.GetCreatedDateInfo(objPost.CreatedDate, objConfig, "")
					hlLastPost.NavigateUrl = Utilities.Links.ContainerViewPostLink(TabId, keyForumID, objPost.PostID)
				Else
					hlLastPost.Text = "-"
				End If
			End If
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds the bookmarked threads for a specific user, if none exist it will hide grid view, show notice to user.
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks>
		''' </remarks>
		Private Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim BookmarkCtl As New BookmarkController
			Dim colThreads As New List(Of TrackingInfo)

			colThreads = BookmarkCtl.BookmarkThreadGet(ProfileUserID, ModuleId, PageSize, CurrentPage)

			gridBookmarks.DataSource = colThreads
			gridBookmarks.DataBind()

			If colThreads.Count > 0 Then
				gridBookmarks.VirtualItemCount = colThreads(0).TotalRecords
			Else
				gridBookmarks.VirtualItemCount = 0
			End If
		End Sub

		''' <summary>
		''' Localizes the data grid headers for all grids on the page (that utilize Telerik).
		''' </summary>
		''' <remarks></remarks>
		Private Sub SetLocalization()
			For Each gc As GridColumn In gridBookmarks.MasterTableView.Columns
				If gc.HeaderText <> "" Then
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile)
				End If
			Next
		End Sub

#End Region

	End Class

End Namespace