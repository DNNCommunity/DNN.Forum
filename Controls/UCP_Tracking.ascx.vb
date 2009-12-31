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

Namespace DotNetNuke.Modules.Forum.UCP

	''' <summary>
	''' This is the UCP section that show what forums and threads the user is tracking
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[skeel]	11/29/2008	Created
	''' </history>
	Partial Public Class Tracking
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
			Localization.LocalizeDataGrid(dgForums, Me.LocalResourceFile)
			Localization.LocalizeDataGrid(dgThreads, Me.LocalResourceFile)
			BuildTabs()

			ForumPager.PageSize = Convert.ToInt32(LoggedOnUser.ThreadsPerPage)
			ThreadPager.PageSize = Convert.ToInt32(LoggedOnUser.ThreadsPerPage)

			cmdForumRemove.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("RemoveItem", Me.LocalResourceFile) & "');")
			cmdThreadRemove.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("RemoveItem", Me.LocalResourceFile) & "');")

			BindForumData(ForumPager.PageSize, 1)
			BindThreadData(ThreadPager.PageSize, 1)
		End Sub

#End Region

#Region "Event Handlers"

		'Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		'	If DotNetNuke.Framework.AJAX.IsInstalled Then
		'		DotNetNuke.Framework.AJAX.RegisterScriptManager()
		'		'DotNetNuke.Framework.AJAX.RegisterPostBackControl(urlLogo)
		'	End If
		'End Sub

		''' <summary>
		''' Deletes selected tracked forums for a user. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdForumRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdForumRemove.Click
			Dim I, Count As Integer
			Dim TrackCtl As New TrackingController

			Count = dgForums.Items.Count - 1

			For I = 0 To Count
				Dim chk As CheckBox = CType(dgForums.Items(I).FindControl("chkForum"), CheckBox)

				'Item checked for delete.
				If chk.Checked Then
					Dim ForumId As Integer = CInt(dgForums.DataKeys(I))
					TrackCtl.TrackingForumCreateDelete(ForumId, UserId, False, ModuleId)
				End If
			Next

			BindForumData(ForumPager.PageSize, 1)
		End Sub

		''' <summary>
		''' Deletes selected tracked threads for a user. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdThreadRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdThreadRemove.Click
			Dim I, Count As Integer
			Dim TrackCtl As New TrackingController

			Count = dgThreads.Items.Count - 1

			For I = 0 To Count
				Dim chk As CheckBox = CType(dgThreads.Items(I).FindControl("chkThread"), CheckBox)

				'Item checked for delete.
				If chk.Checked Then
					Dim ThreadId As Integer = CInt(dgThreads.DataKeys(I))
					TrackCtl.TrackingThreadCreateDelete(-1, ThreadId, UserId, False, ModuleId)
				End If
			Next

			BindThreadData(ThreadPager.PageSize, 1)
		End Sub

		''' <summary>
		''' Used to set properties for various sever controls used in &gt;ItemTempate&lt;.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgForums_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgForums.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As TrackingInfo = CType(e.Item.DataItem, TrackingInfo)
			Dim cntForum As New ForumController
			Dim objForum As ForumInfo
			Dim hl As HyperLink
			Dim lbl As Label
			Dim lit As Literal

			objForum = cntForum.GetForumInfoCache(dataItem.ForumID)

			hl = CType(e.Item.FindControl("hlName"), HyperLink)
			hl.Text = dataItem.Subject
			hl.NavigateUrl = Utilities.Links.ContainerViewForumLink(TabId, dataItem.ForumID, False)

			lit = CType(e.Item.FindControl("imgStatus"), Literal)
			lit.Text = GenerateForumIcon(objForum)

			lbl = CType(e.Item.FindControl("lblTotalPosts"), Label)
			lbl.Text = CStr(objForum.TotalPosts)

			lbl = CType(e.Item.FindControl("lblTotalThreads"), Label)
			lbl.Text = CStr(objForum.TotalThreads)

			lbl = CType(e.Item.FindControl("lblLastPostInfo"), Label)
			Dim objUser As ForumUser = ForumUserController.GetForumUser(dataItem.LastApprovedPosterID, False, ModuleId, PortalId)
			lbl.Text = LastPostDetails(dataItem.ForumID, dataItem.LastApprovedPostCreatedDate, dataItem.LastApprovedPosterID, dataItem.LastApprovedPostID, objUser.Username)
		End Sub

		''' <summary>
		''' Used to set properties for various sever controls used in &gt;ItemTempate&lt;.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgThreads_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgThreads.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As TrackingInfo = CType(e.Item.DataItem, TrackingInfo)
			Dim cntThread As New ThreadController
			Dim objThread As ThreadInfo
			Dim hl As HyperLink
			Dim lbl As Label
			Dim img As System.Web.UI.WebControls.Image

			objThread = cntThread.ThreadGet(dataItem.ThreadID)

			hl = CType(e.Item.FindControl("hlSubject"), HyperLink)
			' Format prohibited words
			If objConfig.FilterSubject Then
				hl.Text = Utilities.ForumUtils.FormatProhibitedWord(dataItem.Subject, PortalId)
			Else
				hl.Text = dataItem.Subject
			End If

			hl.NavigateUrl = Utilities.Links.ContainerViewThreadLink(TabId, ModuleId, dataItem.ThreadID)
			img = CType(e.Item.FindControl("imgStatus"), Image)
			' switch view status and icon depending on read/unread
			If HasNewPosts(UserId, dataItem.ThreadID) Then
				'show new icon
				hl.CssClass = "Forum_NormalBold"
				img.ImageUrl = objConfig.GetThemeImageURL("s_postunread.") & objConfig.ImageExtension
				img.ToolTip = Localization.GetString("UnreadPost.Text", LocalResourceFile)
			Else
				'show read icon
				hl.CssClass = "Forum_Normal"
				img.ImageUrl = objConfig.GetThemeImageURL("s_postread.") & objConfig.ImageExtension
				img.ToolTip = Localization.GetString("ReadPost.Text", LocalResourceFile)
			End If

			lbl = CType(e.Item.FindControl("lblTotalPosts"), Label)
			lbl.Text = CStr(objThread.TotalPosts)

			lbl = CType(e.Item.FindControl("lblLastPostInfo"), Label)

			Dim LAUser As ForumUser = ForumUserController.GetForumUser(dataItem.LastApprovedPosterID, False, ModuleId, PortalId)
			lbl.Text = LastPostDetails(dataItem.ForumID, dataItem.LastApprovedPostCreatedDate, dataItem.LastApprovedPosterID, dataItem.LastApprovedPostID, LAUser.Username)
		End Sub

		''' <summary>
		''' Changes the selected page for the tracked forums datagrid. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub ForumPager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ForumPager.Command
			Dim CurrentPage As Int32 = CType(e.CommandArgument, Int32)
			ForumPager.CurrentPage = CurrentPage
			BindForumData(ForumPager.PageSize, CurrentPage)
		End Sub

		''' <summary>
		''' Changes the selected page of the tracked threads datagrid. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub ThreadPager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles ThreadPager.Command
			Dim CurrentPage As Int32 = CType(e.CommandArgument, Int32)
			ThreadPager.CurrentPage = CurrentPage
			BindThreadData(ThreadPager.PageSize, CurrentPage)
		End Sub

#End Region

#Region "Private Methods"

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
			Dim arrForums As New List(Of TrackingInfo)
			arrForums = TrackCtl.TrackingForumGetAll(UserId, ModuleId, PageSize, CurrentPage - 1)

			If Not arrForums Is Nothing Then
				If arrForums.Count > 0 Then
					Dim objBookmarks As TrackingInfo = arrForums.Item(0)

					dgForums.DataKeyField = "ForumID"
					dgForums.DataSource = arrForums
					dgForums.DataBind()

					cmdForumRemove.Visible = True
					dgForums.Visible = True
					ForumPager.TotalRecords = objBookmarks.TotalRecords
					ForumPager.Visible = True
					lblForums.Visible = False
				Else
					cmdForumRemove.Visible = False
					dgForums.Visible = False
					ForumPager.Visible = False
					lblForums.Text = "<br />" & Localization.GetString("NoForums", Me.LocalResourceFile)
					lblForums.Visible = True
				End If
			Else
				cmdForumRemove.Visible = False
				dgForums.Visible = False
				ForumPager.Visible = False
				lblForums.Text = "<br />" & Localization.GetString("NoForums", Me.LocalResourceFile)
				lblForums.Visible = True
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
			Dim arrThreads As New List(Of TrackingInfo)
			arrThreads = TrackCtl.TrackingThreadGetAll(UserId, ModuleId, PageSize, CurrentPage - 1)

			If Not arrThreads Is Nothing Then
				If arrThreads.Count > 0 Then
					Dim objBookmarks As TrackingInfo = arrThreads.Item(0)

					dgThreads.DataKeyField = "ThreadID"
					dgThreads.DataSource = arrThreads
					dgThreads.DataBind()

					cmdThreadRemove.Visible = True
					dgThreads.Visible = True
					ThreadPager.TotalRecords = objBookmarks.TotalRecords
					ThreadPager.Visible = True
					lblThreads.Visible = False
				Else
					cmdThreadRemove.Visible = False
					dgThreads.Visible = False
					ThreadPager.Visible = False
					lblThreads.Text = "<br />" & Localization.GetString("NoThreads", Me.LocalResourceFile)
					lblThreads.Visible = True
				End If
			Else
				cmdThreadRemove.Visible = False
				dgThreads.Visible = False
				ThreadPager.Visible = False
				lblThreads.Text = "<br />" & Localization.GetString("NoThreads", Me.LocalResourceFile)
				lblThreads.Visible = True
			End If
		End Sub

		''' <summary>
		''' Formats the last post info for a tracked thread.
		''' </summary>
		''' <param name="ForumID"></param>
		''' <param name="LastPostDate"></param>
		''' <param name="LastPostUserID"></param>
		''' <param name="LastApprovedPostID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function LastPostDetails(ByVal ForumID As Integer, ByVal LastPostDate As DateTime, ByVal LastPostUserID As Integer, ByVal LastApprovedPostID As Integer, ByVal LastApprovedUsername As String) As String
			Dim objUser As ForumUser
			Dim str As String
			objUser = ForumUserController.GetForumUser(LastPostUserID, False, ModuleId, PortalId)

			str = "<span class=""Forum_LastPostText""><a href=""" & Utilities.Links.ContainerViewPostLink(TabId, ForumID, LastApprovedPostID) & """ class=""Forum_LastPostText"">" & Utilities.ForumUtils.GetCreatedDateInfo(LastPostDate, objConfig, "Forum_LastPostText") & "</a><br />"
			str += Localization.GetString("by", LocalResourceFile) & " "
			str += "<a href=""" & Utilities.Links.UserPublicProfileLink(TabId, ModuleId, LastPostUserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, LastApprovedUsername) & """ class=""Forum_LastPostText"">" & objUser.DisplayName & "</a>"
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

			objThreadInfo = ctlThread.ThreadGet(ThreadID)

			If UserID > 0 Then
				If objThreadInfo Is Nothing Then
					Return True
				Else
					userthread = userthreadController.GetCachedUserThreadRead(UserID, objThreadInfo.ThreadID)
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

			userForum = userForumController.GetCachedUserForumRead(UserId, Forum.ForumID)

			If Not userForum Is Nothing Then
				If Not userForum.LastVisitDate < Forum.MostRecentPostDate Then
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

#End Region

	End Class

End Namespace