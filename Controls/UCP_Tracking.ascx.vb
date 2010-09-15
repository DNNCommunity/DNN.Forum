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

Imports Telerik.Web.UI

Namespace DotNetNuke.Modules.Forum.UCP

	''' <summary>
	''' This is the UCP section that show what forums and threads the user is tracking
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class Tracking
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			BuildTabs()
			BindForumData(CurrentForumUser.ThreadsPerPage, 0)
			BindThreadData(CurrentForumUser.ThreadsPerPage, 0)
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

		Protected Sub gridForumTracking_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gridForumTracking.ItemDataBound
			If TypeOf e.Item Is GridDataItem Then
				Dim item As GridDataItem = CType(e.Item, GridDataItem)
				Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ForumID"))
				Dim keyMostRecentPostID As Integer = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("MostRecentPostID"))

				Dim imgDelete As ImageButton = CType((item)("imgDelete").Controls(0), ImageButton)
				imgDelete.ToolTip = Localization.GetString("Delete", LocalResourceFile)
				imgDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');")
				imgDelete.CommandArgument = keyForumID.ToString()

				Dim hlForum As HyperLink = CType((item)("hlName").Controls(0), HyperLink)
				hlForum.NavigateUrl = Utilities.Links.ContainerViewForumLink(TabId, keyForumID, False)

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

		Protected Sub gridForumTracking_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles gridForumTracking.ItemCommand
			If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
				Select Case e.CommandName
					Case "DeleteItem"
						Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ForumID"))
						Dim cntTracking As New TrackingController
						cntTracking.TrackingForumCreateDelete(keyForumID, ProfileUserID, False, ModuleId)
						BindForumData(CurrentForumUser.ThreadsPerPage, 0)
				End Select
			End If
		End Sub

		''' <summary>
		''' Fired when the user changes the selected page for hte forum tracking grid.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub gridForumTracking_PageIndexChanged(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles gridForumTracking.PageIndexChanged
			BindForumData(CurrentForumUser.ThreadsPerPage, e.NewPageIndex)
		End Sub

		Protected Sub gridThreadTracking_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gridThreadTracking.ItemDataBound
			If TypeOf e.Item Is GridDataItem Then
				Dim item As GridDataItem = CType(e.Item, GridDataItem)
				Dim keyThreadID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ThreadID"))
				Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ForumID"))
				Dim keyMostRecentPostID As Integer = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("MostRecentPostID"))

				Dim imgDelete As ImageButton = CType((item)("imgDelete").Controls(0), ImageButton)
				imgDelete.ToolTip = Localization.GetString("Delete", LocalResourceFile)
				imgDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');")
				imgDelete.CommandArgument = keyThreadID.ToString()

				Dim hlForum As HyperLink = CType((item)("hlName").Controls(0), HyperLink)
				hlForum.NavigateUrl = Utilities.Links.ContainerViewThreadLink(TabId, keyForumID, keyThreadID)

				Dim hlLastPost As HyperLink = CType((item)("hlLastPost").Controls(0), HyperLink)
				Dim cntPost As New PostController
				If keyMostRecentPostID > 0 Then
					Dim objPost As PostInfo = cntPost.GetPostInfo(keyMostRecentPostID, PortalId)
					hlLastPost.Text = Utilities.ForumUtils.GetCreatedDateInfo(objPost.CreatedDate, objConfig, "")
					hlLastPost.NavigateUrl = Utilities.Links.ContainerViewPostLink(TabId, keyThreadID, objPost.PostID)
				Else
					hlLastPost.Text = "-"
				End If
			End If
		End Sub

		Protected Sub gridThreadTracking_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles gridThreadTracking.ItemCommand
			If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
				Select Case e.CommandName
					Case "DeleteItem"
						Dim keyThreadID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ThreadID"))
						Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ForumID"))

						Dim cntTracking As New TrackingController
						cntTracking.TrackingThreadCreateDelete(keyForumID, keyThreadID, ProfileUserID, False, ModuleId)
						BindForumData(CurrentForumUser.ThreadsPerPage, 0)
				End Select
			End If
		End Sub

		''' <summary>
		''' Fired when the user changes the selected page for the thread tracking grid.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub gridThreadTracking_PageIndexChanged(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles gridThreadTracking.PageIndexChanged
			BindThreadData(CurrentForumUser.ThreadsPerPage, e.NewPageIndex)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Private Sub BuildTabs()
			Dim tabForums As New Telerik.Web.UI.RadTab
			tabForums.Text = Localization.GetString("TabForums", LocalResourceFile)
			tabForums.PageViewID = "rpvForums"
			rtsNotifications.Tabs.Add(tabForums)

			Dim tabThreads As New Telerik.Web.UI.RadTab
			tabThreads.Text = Localization.GetString("TabThreads", LocalResourceFile)
			tabThreads.PageViewID = "rpvThreads"
			rtsNotifications.Tabs.Add(tabThreads)
		End Sub

		''' <summary>
		''' Binds a list of tracked forums for a specific user to a datagrid. 
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks></remarks>
		Private Sub BindForumData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim TrackCtl As New TrackingController
			Dim colTrackedForums As New List(Of ForumInfo)

			colTrackedForums = TrackCtl.GetUsersForumsTracked(ProfileUserID, ModuleId, PageSize, CurrentPage)

			gridForumTracking.DataSource = colTrackedForums
			gridForumTracking.DataBind()

			If colTrackedForums.Count > 0 Then
				gridForumTracking.VirtualItemCount = colTrackedForums(0).TotalRecords
			Else
				gridForumTracking.VirtualItemCount = 0
			End If
		End Sub

		''' <summary>
		''' Binds a list of tracked threads for a specific user to a datagrid. 
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Private Sub BindThreadData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim TrackCtl As New TrackingController
			Dim colThreads As New List(Of TrackingInfo)

			colThreads = TrackCtl.TrackingThreadGetAll(ProfileUserID, ModuleId, PageSize, CurrentPage - 1)

			gridThreadTracking.DataSource = colThreads
			gridThreadTracking.DataBind()

			If colThreads.Count > 0 Then
				gridThreadTracking.VirtualItemCount = colThreads(0).TotalRecords
			Else
				gridThreadTracking.VirtualItemCount = 0
			End If
		End Sub

		''' <summary>
		''' Formats the last post info for a tracked thread.
		''' </summary>
		''' <param name="ForumID"></param>
		''' <param name="LastPostDate"></param>
		''' <param name="LastApprovedUser"></param>
		''' <param name="LastApprovedPostID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function LastPostDetails(ByVal ForumID As Integer, ByVal LastPostDate As DateTime, ByVal LastApprovedUser As ForumUserInfo, ByVal LastApprovedPostID As Integer) As String
			Dim objUser As ForumUserInfo
			Dim str As String
			Dim cntForumUser As New ForumUserController

			objUser = cntForumUser.GetForumUser(LastApprovedUser.UserID, False, ModuleId, PortalId)

			str = "<span class=""Forum_LastPostText""><a href=""" & Utilities.Links.ContainerViewPostLink(TabId, ForumID, LastApprovedPostID) & """ class=""Forum_LastPostText"">" & Utilities.ForumUtils.GetCreatedDateInfo(LastPostDate, objConfig, "Forum_LastPostText") & "</a><br />"
			str += Localization.GetString("by", LocalResourceFile) & " "
			str += "<a href="""
			If Not objConfig.EnableExternalProfile Then
				str += LastApprovedUser.UserCoreProfileLink
			Else
				str += Utilities.Links.UserExternalProfileLink(LastApprovedUser.UserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, LastApprovedUser.SiteAlias)
			End If
			str += """ class=""Forum_LastPostText"">" & LastApprovedUser.SiteAlias & "</a>"
			str += "</span>"
			Return str
		End Function

		''' <summary>
		''' If a thread has new posts or not using the UserReads methods. 
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="ThreadID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function HasNewPosts(ByVal UserID As Integer, ByVal ThreadID As Integer) As Boolean
			Dim userthreadController As New UserThreadsController
			Dim ThreadCtl As New ThreadController
			Dim userthread As New UserThreadsInfo
			Dim objThreadInfo As ThreadInfo
			Dim ctlThread As New ThreadController

			objThreadInfo = ctlThread.GetThread(ThreadID)

			If UserID > 0 Then
				If objThreadInfo Is Nothing Then
					Return True
				Else
					userthread = userthreadController.GetThreadReadsByUser(UserID, objThreadInfo.ThreadID)
					If userthread Is Nothing Then
						Return True
					Else
						If userthread.LastVisitDate < objThreadInfo.LastApprovedPost.CreatedDate Then
							Return True
						Else
							Return False
						End If
					End If
				End If
			Else
				Return True
			End If
		End Function

		''' <summary>
		''' Render forum icon
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	11/29/2008	Created
		''' </history>
		Private Function GenerateForumIcon(ByVal Forum As ForumInfo) As String
			Dim HasNewThreads As Boolean = True
			Dim userForumController As New UserForumsController
			Dim userForum As New UserForumsInfo
			Dim url As String
			Dim ForumContainerResourceFile As String = Me.Parent.TemplateSourceDirectory() & "/App_LocalResources/Forum_Container.ascx.resx"

			If Forum.GroupID = -1 Then
				' aggregated
				url = Utilities.Links.ContainerAggregatedLink(TabId, False)
			Else
				url = Utilities.Links.ContainerViewForumLink(TabId, Forum.ForumID, False)
			End If

			userForum = userForumController.GetUsersForumReads(UserId, Forum.ForumID)

			If Not userForum Is Nothing Then
				If Not userForum.LastVisitDate < Forum.MostRecentPost.CreatedDate Then
					HasNewThreads = False
				End If
			End If

			' display image depends on new post status 
			If Not Forum.PublicView Then
				' See if the forum is a Link Type forum
				If Forum.ForumType = 2 Then
					'LinkForum not possible to track
					Return ""
				Else
					' See if the forum is moderated
					If Forum.IsModerated Then
						If HasNewThreads AndAlso Forum.TotalThreads > 0 Then
							Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_private_moderated_new.") & objConfig.ImageExtension, Localization.GetString("imgNewPrivateModerated", ForumContainerResourceFile))
						Else
							Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_private_moderated.") & objConfig.ImageExtension, Localization.GetString("imgPrivateModerated", ForumContainerResourceFile))
						End If
					Else
						If HasNewThreads AndAlso Forum.TotalThreads > 0 Then
							Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_private_new.") & objConfig.ImageExtension, Localization.GetString("imgNewPrivate", ForumContainerResourceFile))
						Else
							Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_private.") & objConfig.ImageExtension, Localization.GetString("imgPrivate", ForumContainerResourceFile))
						End If
					End If
				End If
			Else
				' See if the forum is a Link Type forum
				If Forum.ForumType = 2 Then
					'LinkForum not possible to track
					Return ""
				Else
					Dim str2 As String = ForumContainerResourceFile
					If Forum.ForumID = -1 Then
						Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_aggregate.") & objConfig.ImageExtension, Localization.GetString("imgAggregated", ForumContainerResourceFile))
					Else
						' Determine if forum is moderated
						If Forum.IsModerated Then
							If HasNewThreads AndAlso Forum.TotalThreads > 0 Then
								Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_moderated_new.") & objConfig.ImageExtension, Localization.GetString("imgNewModerated", ForumContainerResourceFile))
							Else
								Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_moderated.") & objConfig.ImageExtension, Localization.GetString("imgModerated", ForumContainerResourceFile))
							End If
						Else
							If HasNewThreads AndAlso Forum.TotalThreads > 0 Then
								Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_unmoderated_new.") & objConfig.ImageExtension, Localization.GetString("imgNewUnmoderated", ForumContainerResourceFile))
							Else
								Return RenderImageButton(url, objConfig.GetThemeImageURL("forum_unmoderated.") & objConfig.ImageExtension, Localization.GetString("imgUnmoderated", ForumContainerResourceFile))
							End If
						End If
					End If
				End If
			End If
		End Function

		''' <summary>
		''' Mimics function in base (which cannot be inherited from here) so we can render image buttons (like Group icons that are clickable)
		''' </summary>
		''' <param name="Url"></param>
		''' <param name="ImageUrl"></param>
		''' <param name="Tooltip"></param>
		''' <remarks></remarks>
		Private Function RenderImageButton(ByVal Url As String, ByVal ImageUrl As String, ByVal Tooltip As String) As String
			Dim str As String = String.Empty
			str = "<a href=""" & Url & """>"
			str += "<img src=""" & ImageUrl & """ border=""0"" alt=""" & Tooltip & """ title=""" & Tooltip & """ /></a>"
			Return str
		End Function

		''' <summary>
		''' Localizes the data grid headers for all grids on the page (that utilize Telerik).
		''' </summary>
		''' <remarks></remarks>
		Private Sub SetLocalization()
			For Each gc As GridColumn In gridForumTracking.MasterTableView.Columns
				If gc.HeaderText <> "" Then
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile)
				End If
			Next

			For Each gc As GridColumn In gridThreadTracking.MasterTableView.Columns
				If gc.HeaderText <> "" Then
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile)
				End If
			Next
		End Sub

#End Region

	End Class

End Namespace