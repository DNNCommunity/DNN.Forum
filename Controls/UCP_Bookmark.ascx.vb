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

Namespace DotNetNuke.Modules.Forum.UCP

	''' <summary>
	''' This is the UCP section that show the list of threads the user have bookmarked
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[skeel]	12/1/2008	Created
	''' </history>
	Partial Public Class Bookmark
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
			Localization.LocalizeDataGrid(dgBookmarks, Me.LocalResourceFile)
			BottomPager.PageSize = Convert.ToInt32(CurrentForumUser.ThreadsPerPage)

			BindData(BottomPager.PageSize, 1)
			cmdRemove.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("RemoveItem", Me.LocalResourceFile) & "');")
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Deletes selected bookmark for a user. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub cmdRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRemove.Click
			Dim I, Count As Integer
			Dim BookmarkCtl As New BookmarkController

			Count = dgBookmarks.Items.Count - 1

			For I = 0 To Count
				Dim chk As CheckBox = CType(dgBookmarks.Items(I).FindControl("chkBookmark"), CheckBox)

				'Item checked for delete.
				If chk.Checked Then
					Dim ThreadId As Integer = CInt(dgBookmarks.DataKeys(I))
					BookmarkCtl.BookmarkCreateDelete(ThreadId, UserId, False, ModuleId)
				End If
			Next

			BindData(BottomPager.PageSize, 1)
		End Sub

		''' <summary>
		''' Used to set properties for various sever controls used in &gt;ItemTempate&lt;.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub dgBookmarks_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgBookmarks.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As TrackingInfo = CType(e.Item.DataItem, TrackingInfo)
			Dim cntThread As New ThreadController
			Dim objThread As ThreadInfo
			Dim hl As HyperLink
			Dim lbl As Label
			Dim img As System.Web.UI.WebControls.Image

			objThread = cntThread.GetThread(dataItem.ThreadID)

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
				img.ToolTip = Services.Localization.Localization.GetString("UnreadPost.Text", LocalResourceFile)
			Else
				'show read icon
				hl.CssClass = "Forum_Normal"
				img.ImageUrl = objConfig.GetThemeImageURL("s_postread.") & objConfig.ImageExtension
				img.ToolTip = Services.Localization.Localization.GetString("ReadPost.Text", LocalResourceFile)
			End If

			' CP - COMEBACK - For release, not permitting ratings to show here (it was a late suggestion)
			img = CType(e.Item.FindControl("imgRating"), Image)
			img.Visible = False
			If objConfig.EnableRatings Then
				'img.ImageUrl = dataItem.RatingImage
				'img.ToolTip = dataItem.RatingText
			Else
				' img.Visible = False
			End If
			' status here too

			lbl = CType(e.Item.FindControl("lblTotalPosts"), Label)
			lbl.Text = CStr(objThread.TotalPosts)

			lbl = CType(e.Item.FindControl("lblLastPostInfo"), Label)

			Dim cntForumUser As New ForumUserController
			Dim objUser As ForumUserInfo = cntForumUser.GetForumUser(dataItem.LastApprovedPosterID, False, ModuleId, PortalId)
			lbl.Text = LastPostDetails(dataItem.ForumID, dataItem.LastApprovedPostCreatedDate, objUser, dataItem.LastApprovedPostID)
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
		''' Formats the last post info for a bookmarked thread.
		''' </summary>
		''' <param name="ForumID"></param>
		''' <param name="LastPostDate"></param>
		''' <param name="LastApprovedUser"></param>
		''' <param name="LastApprovedPostID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function LastPostDetails(ByVal ForumID As Integer, ByVal LastPostDate As DateTime, ByVal LastApprovedUser As ForumUserInfo, ByVal LastApprovedPostID As Integer) As String
			Dim str As String

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
		Protected Function HasNewPosts(ByVal UserID As Integer, ByVal ThreadID As Integer) As Boolean
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
		''' Binds the bookmarked threads for a specific user, if none exist it will hide grid view, show notice to user.
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Protected Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim BookmarkCtl As New BookmarkController
			Dim arrThreads As New List(Of TrackingInfo)

			arrThreads = BookmarkCtl.BookmarkThreadGet(ProfileUserID, ModuleId, objConfig.ForumMemberName, PageSize, CurrentPage - 1)

			If Not arrThreads Is Nothing Then
				If arrThreads.Count > 0 Then
					Dim objBookmarks As TrackingInfo = arrThreads.Item(0)

					dgBookmarks.DataKeyField = "ThreadID"
					dgBookmarks.DataSource = arrThreads
					dgBookmarks.DataBind()

					cmdRemove.Visible = True
					dgBookmarks.Visible = True
					BottomPager.TotalRecords = objBookmarks.TotalRecords
					BottomPager.Visible = True
					lblInfo.Visible = False
				Else
					cmdRemove.Visible = False
					dgBookmarks.Visible = False
					BottomPager.Visible = False
					lblInfo.Text = "<br />" & Localization.GetString("NoThreads", Me.LocalResourceFile)
					lblInfo.Visible = True
				End If
			Else
				cmdRemove.Visible = False
				dgBookmarks.Visible = False
				BottomPager.Visible = False
				lblInfo.Text = "<br />" & Localization.GetString("NoThreads", Me.LocalResourceFile)
				lblInfo.Visible = True
			End If
		End Sub

		''' <summary>
		''' Render forum icon
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	11/29/2008	Created
		''' </history>
		Protected Function GenerateForumIcon(ByVal Forum As ForumInfo) As String
			Dim HasNewThreads As Boolean = True
			Dim userForumController As New UserForumsController
			Dim userForum As New UserForumsInfo
			Dim url As String
			Dim ForumContainerResourceFile As String = TemplateSourceDirectory() & "/App_LocalResources/Forum_Container.ascx.resx"

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
		Protected Function RenderImageButton(ByVal Url As String, ByVal ImageUrl As String, ByVal Tooltip As String) As String
			Dim str As String = String.Empty
			str = "<a href=""" & Url & """>"
			str += "<img src=""" & ImageUrl & """ border=""0"" alt=""" & Tooltip & """ title=""" & Tooltip & """ /></a>"
			Return str
		End Function

#End Region

	End Class

End Namespace