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
	''' This is the MCP section that shows the reported posts. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	12/21/2008	Created
	''' </history>
	Partial Public Class ReportedPost
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks>So far this is a static page</remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Localization.LocalizeDataGrid(dgReportedPost, Me.LocalResourceFile)
			Localization.LocalizeDataGrid(dgPostReportDetails, Me.LocalResourceFile)
			BottomPager.PageSize = Convert.ToInt32(CurrentForumUser.ThreadsPerPage)
			DetailPager.PageSize = Convert.ToInt32(CurrentForumUser.ThreadsPerPage)
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
		Protected Sub dgReportedPost_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgReportedPost.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As PostInfo = CType(e.Item.DataItem, PostInfo)
			Dim hl As HyperLink
			Dim cmd As LinkButton

			cmd = CType(e.Item.FindControl("cmdSubject"), LinkButton)
			cmd.CommandArgument = dataItem.PostID.ToString()
			cmd.Text = dataItem.Subject

			hl = CType(e.Item.FindControl("hlForumName"), HyperLink)
			hl.NavigateUrl = Utilities.Links.ContainerViewForumLink(TabId, dataItem.ForumID, False)
			hl.Text = dataItem.ParentThread.HostForum.Name

			hl = CType(e.Item.FindControl("hlPostAuthor"), HyperLink)
			hl.NavigateUrl = Utilities.Links.UserPublicProfileLink(TabId, ModuleId, dataItem.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, dataItem.Author.Username)
			hl.Text = dataItem.Author.SiteAlias

			hl = CType(e.Item.FindControl("lblReportedDate"), HyperLink)
			hl.NavigateUrl = Utilities.Links.ContainerViewPostLink(TabId, dataItem.ForumID, dataItem.PostID)
			hl.Text = FormatDate(dataItem.CreatedDate)
		End Sub

		''' <summary>
		''' Runs when the end user clicks a post subject link, then the command displays the appropriate post details in a separate grid.
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgReportedPost_ItemCommand(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgReportedPost.ItemCommand
			Try
				Dim cmd As String = String.Empty
				Dim argument As String = String.Empty

				If TypeOf e.CommandSource Is LinkButton Then
					cmd = CType(e.CommandSource, LinkButton).CommandName
					argument = CType(e.CommandSource, LinkButton).CommandArgument
				End If

				Select Case cmd.ToLower
					Case "details"
						pnlReportedPost.Visible = False
						pnlPostDetails.Visible = True
						BindPostDetails(CInt(argument))
				End Select
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
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

		''' <summary>
		''' Used to set properties for various sever controls used in the item template.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgPostReportDetails_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPostReportDetails.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As PostReportedInfo = CType(e.Item.DataItem, PostReportedInfo)
			Dim hl As HyperLink
			Dim imgBtn As ImageButton

			imgBtn = CType(e.Item.FindControl("imgAccept"), ImageButton)
			If dataItem.Addressed Then
				imgBtn.Enabled = False
				imgBtn.ImageUrl = objConfig.GetThemeImageURL("checked.") & objConfig.ImageExtension
				imgBtn.ToolTip = Services.Localization.Localization.GetString("Accepted.Text", LocalResourceFile)
			Else
				imgBtn.ImageUrl = objConfig.GetThemeImageURL("unchecked.") & objConfig.ImageExtension
				imgBtn.ToolTip = Services.Localization.Localization.GetString("Unaccepted.Text", LocalResourceFile)
				imgBtn.CommandArgument = dataItem.PostReportedID.ToString()
			End If

			hl = CType(e.Item.FindControl("hlUser"), HyperLink)
			hl.NavigateUrl = Utilities.Links.UserPublicProfileLink(TabId, ModuleId, dataItem.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, dataItem.Author(ModuleId, PortalId).Username)
			hl.Text = dataItem.Author(ModuleId, PortalId).SiteAlias

			hl = CType(e.Item.FindControl("lblReportedDate"), HyperLink)
			hl.NavigateUrl = Utilities.Links.ContainerViewPostLink(TabId, dataItem.Post(PortalId).ForumID, dataItem.PostID)
			hl.Text = FormatDate(dataItem.CreatedDate)
		End Sub

		''' <summary>
		''' Used to address reported posts so that moderators only see post reports that haven't been addressed.
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks>This is ideal for easier maintanence.</remarks>
		Protected Sub dgPostReportDetails_ItemCommand(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPostReportDetails.ItemCommand
			Try
				Dim cmd As String = String.Empty
				Dim argument As String = String.Empty

				If TypeOf e.CommandSource Is ImageButton Then
					cmd = CType(e.CommandSource, ImageButton).CommandName
					argument = CType(e.CommandSource, ImageButton).CommandArgument
				End If

				Select Case cmd.ToLower
					Case "accept"
						Dim cntPostReport As New PostReportedController

						cntPostReport.AddressPostReport(CInt(argument), UserId, PortalId)
						' Update the post cache.
						PostInfo.ResetPostInfo(CInt(argument))
						BindPostDetails(CInt(argument))
				End Select
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds the reported posts with the most recently reported at the top of the grid.
		''' </summary>
		''' <param name="PageSize">The number of records to return.</param>
		''' <param name="CurrentPage">The current page index the end user will be viewing.</param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Private Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim cntPostReport As New PostReportedController
			Dim arrPosts As New List(Of PostInfo)
			Dim TotalRecords As Integer

			arrPosts = cntPostReport.GetReportedPosts(PortalId, CurrentPage - 1, PageSize, TotalRecords)

			If Not arrPosts Is Nothing Then
				If arrPosts.Count > 0 Then
					dgReportedPost.DataKeyField = "UserID"
					dgReportedPost.DataSource = arrPosts
					dgReportedPost.DataBind()

					BottomPager.TotalRecords = arrPosts.Count

					pnlReportedPost.Visible = True
					pnlNoItems.Visible = False
				Else
					pnlReportedPost.Visible = False
					pnlNoItems.Visible = True
				End If
			Else
				pnlReportedPost.Visible = False
				pnlNoItems.Visible = True
			End If
			pnlPostDetails.Visible = False
		End Sub

		''' <summary>
		''' Binds details about a specific post that has been reported.
		''' </summary>
		''' <param name="PostID"></param>
		''' <remarks></remarks>
		Private Sub BindPostDetails(ByVal PostID As Integer)
			Dim cntPostReport As New PostReportedController
			Dim arrPosts As New List(Of PostReportedInfo)

			arrPosts = cntPostReport.GetPostReportDetails(PostID)

			If Not arrPosts Is Nothing Then
				If arrPosts.Count > 0 Then
					dgPostReportDetails.DataKeyField = "PostReportedID"
					dgPostReportDetails.DataSource = arrPosts
					dgPostReportDetails.DataBind()

					' hard coded so paging is never enabled.
					DetailPager.TotalRecords = 1
				Else
					' no un-addressed posts, lets go back to the initial view
					pnlPostDetails.Visible = False
					BindData(BottomPager.PageSize, 1)
				End If
			Else
				' no un-addressed posts, lets go back to the initial view
				pnlPostDetails.Visible = False
				BindData(BottomPager.PageSize, 1)
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