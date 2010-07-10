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

Imports DotNetNuke.Modules.Forum.Utilities

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' Allows a moderator/admin to move a thread to a new forum.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	7/13/2005	Created
	''' </history>
	Public MustInherit Class ThreadMove
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Private _OldForumID As Integer
		Private _ThreadID As Integer
		Private _ThreadInfo As ThreadInfo
		Private _PostID As Integer
		Private _ModeratorReturn As Boolean = False

#End Region

#Region "Optional Interfaces"

		''' <summary>
		''' Gets a list of module actions available to the user to provide it to DNN core.
		''' </summary>
		''' <value></value>
		''' <returns>The collection of module actions available to the user</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
			Get
				Return Utilities.ForumUtils.PerUserModuleActions(objConfig, Me)
			End Get
		End Property

#End Region

#Region "Properties"

		Public Property SelectedForumId() As Integer
			Get
				Return CInt(ViewState("SelectedForumId"))
			End Get
			Set(ByVal Value As Integer)
				ViewState("SelectedForumId") = Value
			End Set
		End Property

		Public Property PostID() As Integer
			Get
				Return _PostID
			End Get
			Set(ByVal Value As Integer)
				_PostID = Value
			End Set
		End Property

#End Region

#Region "Enums"

		''' <summary>
		''' The image type to use to represent the various levels in the treeview. 
		''' </summary>
		''' <remarks></remarks>
		Private Enum eImageType
			Group = 0
			Forum = 1
		End Enum

#End Region

#Region "Event Handlers"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			' Ajax
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdCancel)
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdMove)
			End If
		End Sub

		''' <summary>
		''' Loads page settings.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)
				Dim cntThread As New ThreadController()
				Dim objPostInfo As New PostInfo
				Dim ctlPost As New PostController

				If Request.IsAuthenticated Then
					If Not Request.QueryString("threadid") Is Nothing Then
						_ThreadID = Int32.Parse(Request.QueryString("threadid"))
						_ThreadInfo = cntThread.GetThreadInfo(_ThreadID)
						_OldForumID = _ThreadInfo.ForumID
					Else
						If Not Request.QueryString("postid") Is Nothing Then
							_PostID = Int32.Parse(Request.QueryString("postid"))

							objPostInfo = ctlPost.PostGet(_PostID, PortalId)
							_ThreadID = objPostInfo.ThreadID
							_ThreadInfo = cntThread.GetThreadInfo(_ThreadID)
							_OldForumID = _ThreadInfo.ForumID
						End If
					End If

					If Not (Security.IsModerator) Then
						' they don't belong here
						HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
					End If
				Else
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
				ForumUtils.LoadCssFile(DefaultPage, objConfig)

				If Page.IsPostBack = False Then
					txtSubject.Text = _ThreadInfo.Subject
					txtOldForum.Text = _ThreadInfo.HostForum.Name

					If Not Request.UrlReferrer Is Nothing Then
						ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
					End If

					' Treeview forum viewer
					ForumTreeview.PopulateTelerikTree(objConfig, rtvForums, UserId)
					SelectDefaultForumTree(_ThreadInfo)
					lblErrorMsg.Visible = False
				End If

			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Moves the thread to a new forum, unless user is attempting to move
		''' the thread to the same forum.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdMove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdMove.Click
			Try
				' See if user selected a forum to move thread to.
				Dim newForumID As Integer = -1

				For Each objNode As Telerik.Web.UI.RadTreeNode In Me.rtvForums.CheckedNodes
					Dim strType As String = objNode.Value.Substring(0, 1)
					Dim iID As Integer = CInt(objNode.Value.Substring(1, objNode.Value.Length - 1))

					newForumID = iID
					Exit For
				Next
				Dim cntThread As New ThreadController()

				If Not Request.QueryString("threadid") Is Nothing Then
					_ThreadID = Int32.Parse(Request.QueryString("threadid"))
					_ThreadInfo = cntThread.GetThreadInfo(_ThreadID)
					_OldForumID = _ThreadInfo.ForumID
					_PostID = _ThreadInfo.LastApprovedPostID
				Else
					If Not Request.QueryString("postid") Is Nothing Then
						_PostID = Int32.Parse(Request.QueryString("postid"))

						Dim objPostInfo As New PostInfo
						Dim ctlPost As New PostController

						objPostInfo = ctlPost.PostGet(_PostID, PortalId)
						_ThreadID = objPostInfo.ThreadID
						_ThreadInfo = cntThread.GetThreadInfo(_ThreadID)
						_OldForumID = _ThreadInfo.ForumID
					End If
				End If

				' If the new forumID is the same as the old ForumID
				If (newForumID = _OldForumID) Then
					' Show user error message
					lblErrorMsg.Text = DotNetNuke.Services.Localization.Localization.GetString("OldForumSeleted.Text", Me.LocalResourceFile)
					lblErrorMsg.Visible = True
					Exit Sub
				Else
					If newForumID = -1 Then
						'PopulateTree(mForumConfig)
						lblErrorMsg.Text = DotNetNuke.Services.Localization.Localization.GetString("NoForumSelected.Text", Me.LocalResourceFile)
						lblErrorMsg.Visible = True
						Exit Sub
					Else
						lblErrorMsg.Visible = False
						' build notes (old forum, new forum, say post was moved in body (basically email body text)
						Dim Notes As String = "Thread Move"

						' return to new forum page
						Dim strURL As String = Utilities.Links.ContainerViewThreadLink(TabId, newForumID, _PostID)
						Dim ctlThread As New ThreadController
						'Dim MyProfileUrl As String = Utils.MySettingsLink(TabId, ModuleId)
						Dim MyProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)

						ctlThread.ThreadMove(_ThreadID, newForumID, UserId, Notes, _ThreadInfo.HostForum.ParentId)

						ForumController.ResetForumInfoCache(newForumID)
						ForumController.ResetForumInfoCache(_OldForumID)
						ThreadController.ResetThreadInfo(_ThreadID)

						ThreadController.ResetThreadListCached(newForumID, ModuleId)
						ThreadController.ResetThreadListCached(_OldForumID, ModuleId)
						If objConfig.AggregatedForums Then
							ThreadController.ResetThreadListCached(-1, ModuleId)
						End If

						' Handle sending emails 
						If chkEmailUsers.Checked And objConfig.MailNotification Then
							Utilities.ForumUtils.SendForumMail(_ThreadID, strURL, ForumEmailType.UserThreadMoved, Notes, objConfig, MyProfileUrl, PortalId)
						End If

						Dim objPost As PostInfo
						Dim PostCnt As New PostController
						objPost = PostCnt.PostGet(_PostID, PortalId)

						Response.Redirect(GetReturnURL(objPost), False)
					End If
				End If
			Catch ex As Exception
				LogException(ex)
			End Try
		End Sub

		''' <summary>
		''' Returns the user back to where they came from
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			Try
				If Not ViewState("UrlReferrer") Is Nothing Then
					Response.Redirect(CType(ViewState("UrlReferrer"), String), False)
				Else
					Response.Redirect(NavigateURL(), False)
				End If
			Catch exc As Exception	 'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Used to select the user's default forum in the tree (if applicable)
		''' </summary>
		''' <param name="objThreadInfo">ThreadInfo</param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	9/25/2006	Created
		''' </history>
		Private Sub SelectDefaultForumTree(ByVal objThreadInfo As ThreadInfo)
			Dim SelectedForumID As Integer
			SelectedForumID = objThreadInfo.ForumID

			Dim node As Telerik.Web.UI.RadTreeNode = rtvForums.FindNodeByValue("F" + SelectedForumID.ToString)
			node.Selected = True
			node.ParentNode.Expanded = True
			node.Checked = True
		End Sub

		''' <summary>
		''' Gets the URL to return the user too.
		''' </summary>
		''' <param name="Post"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetReturnURL(ByVal Post As PostInfo) As String
			Dim url As String

			' This only needs to be handled for moderators (which are trusted no matter what, so it only happens at this point)
			Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, Post.ForumID, UserId)

			If Security.IsForumModerator Then
				' Check for querystring parameter to make sure this is where they came from.  Even if a direct link was typed in (should not happen) no damage will be done
				If (Not Request.QueryString("moderatorreturn") Is Nothing) Then
					If Request.QueryString("moderatorreturn") = "1" Then
						_ModeratorReturn = True
					End If
				End If
			End If

			If _ModeratorReturn Then
				If Not ViewState("UrlReferrer") Is Nothing Then
					url = (CType(ViewState("UrlReferrer"), String))
				Else
					' behave as before (normal usage)
					url = Utilities.Links.ContainerViewPostLink(TabId, Post.ForumID, Post.PostID)
				End If
			Else
				' behave as before (normal usage)
				url = Utilities.Links.ContainerViewPostLink(TabId, Post.ForumID, Post.PostID)
			End If

			Return url
		End Function

#End Region

	End Class

End Namespace