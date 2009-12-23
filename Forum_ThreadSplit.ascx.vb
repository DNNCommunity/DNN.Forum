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

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' Allows a moderator/admin to move a thread to a new forum.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public MustInherit Class ThreadSplit
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Private _OldForumID As Integer
		Private _ThreadID As Integer
		Private _ThreadInfo As ThreadInfo
		Private _SplitMode As Boolean = False
		Private _PostID As Integer
		Private _ForumInfo As ForumInfo
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

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property SelectedForumId() As Integer
			Get
				Return CInt(ViewState("SelectedForumId"))
			End Get
			Set(ByVal Value As Integer)
				ViewState("SelectedForumId") = Value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property SplitMode() As Boolean
			Get
				Return _SplitMode
			End Get
			Set(ByVal Value As Boolean)
				_SplitMode = Value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
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
			'' Ajax
			'If DotNetNuke.Framework.AJAX.IsInstalled Then
			'    DotNetNuke.Framework.AJAX.RegisterScriptManager()
			'    DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, True)
			'    DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdCancel)
			'    DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdMove)
			'End If
		End Sub

		''' <summary>
		''' Loads page settings.
		''' </summary>
		''' <param name="sender">System.Object,</param>
		''' <param name="e">System.EventArgs</param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)
				Dim objPostInfo As New PostInfo
				Dim ctlPost As New PostController

				If Request.IsAuthenticated Then
					If Not Request.QueryString("postid") Is Nothing Then
						Dim cntForum As New ForumController

						_PostID = Int32.Parse(Request.QueryString("postid"))

						objPostInfo = ctlPost.PostGet(_PostID, PortalId)
						_ThreadID = objPostInfo.ThreadID
						_ThreadInfo = ThreadInfo.GetThreadInfo(_ThreadID)
						_OldForumID = _ThreadInfo.ForumID
						_ForumInfo = cntForum.GetForumInfoCache(_OldForumID)

						' If there is no postid, there is no reason to be here
					Else
						' they don't belong here
						HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
					End If

					'CLP
					If Not (Security.IsModerator) Then
						'If Not (Security.IsGlobalModerator = True) Or Not (Utils.IsInSingleForumModerators(mThreadInfo.ParentForum.SingleForumModerators, mLoggedOnUserID)) Then
						'    ' they don't belong here
						'    HttpContext.Current.Response.Redirect(UnAuthorizedLink())
						'End If

						' they don't belong here
						HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
					End If
				Else
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Page.IsPostBack = False Then
					litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

					txtSubject.Text = objPostInfo.Subject
					txtSubject.Enabled = True
					lblPost.Text = FormatBody(_PostID)	' Server.HtmlDecode(objPostInfo.Body)
					txtOldForum.Text = _ThreadInfo.HostForum.Name

					If Not Request.UrlReferrer Is Nothing Then
						ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
					End If

					lblErrorMsg.Visible = False

					' Treeview forum viewer
					'ForumTreeview.InitializeTree(objConfig, ForumTree)
					'ForumTreeview.SetTreeDefaults(objConfig, ForumTree, False)
					'ForumTreeview.PopulateTree(objConfig, ForumTree, UserId)
					ForumTreeview.PopulateTelerikTree(objConfig, rtvForums, UserId)
					SelectDefaultForumTree(_ThreadInfo)

					' Get all posts
					Dim arrPosts As List(Of PostInfo)
					arrPosts = ctlPost.PostGetAllForThread(_ThreadID)

					cmdMove.Attributes.Add("onClick", "javascript:return confirm('" & DotNetNuke.Services.Localization.Localization.GetString("ThreadSplitPostApprove.Text", Me.LocalResourceFile) & "');")

					dlPostsForThread.DataSource = arrPosts
					dlPostsForThread.DataBind()

					' Register scripts
					'Utils.RegisterPageScripts(Page, ForumConfig)
				End If

			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		'''' <summary>
		'''' Populates one level of the forum treeview when needed(by groupid)
		'''' </summary>
		'''' <param name="source">Object</param>
		'''' <param name="e">UI.WebControls.DNNTreeEventArgs</param>
		'''' <remarks>
		'''' </remarks>
		'Protected Sub ForumTree_PopulateOnDemand(ByVal source As Object, ByVal e As UI.WebControls.DNNTreeEventArgs) Handles ForumTree.PopulateOnDemand
		'	Dim groupController As New GroupController
		'	Dim strKey As String = e.Node.Key.Substring(1)			  'trim off type
		'	Dim objGroup As GroupInfo = groupController.GroupGet(CInt(strKey))

		'	ForumTreeview.AddForums(objGroup, e.Node, objConfig, UserId)
		'End Sub

		''' <summary>
		''' Moves the thread to a new forum, unless user is attempting to move
		''' the thread to the same forum.
		''' </summary>
		''' <param name="sender">System.Object,</param>
		''' <param name="e">System.EventArgs</param>
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

				If newForumID = -1 Then
					lblErrorMsg.Text = DotNetNuke.Services.Localization.Localization.GetString("NoForumSelected.Text", Me.LocalResourceFile)
					lblErrorMsg.Visible = True
					Exit Sub
				Else
					lblErrorMsg.Visible = False
					' build notes (old forum, new forum, say post was moved in body (basically email body text)
					Dim Notes As String = "Thread Split"

					' return to new forum page
					Dim strURL As String = Utilities.Links.ContainerViewThreadLink(TabId, newForumID, _PostID)
					Dim ctlThread As New ThreadController
					'Dim MyProfileUrl As String = Utils.MySettingsLink(TabId, ModuleId)
					Dim MyProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)

					' Split this post into a new thread
					ctlThread.ThreadSplit(_PostID, _ThreadID, newForumID, UserId, txtSubject.Text, Notes, _ForumInfo.ParentId)

					' we need to grab all the selected posts, set their parent to new threadid.
					' we know the first posts in each of the threads have sort order set to 0, we need to start w/ 1 here and increment by 1 for each post per thread
					Dim newThreadSortOrder As Integer = 1
					Dim oldThreadSortOrder As Integer = 1
					Dim ctlPosts As New PostController

					For Each item As DataListItem In dlPostsForThread.Items
						Dim chk As CheckBox = CType(item.FindControl("chkThreadToSplit"), CheckBox)
						If chk.Checked Then
							' For each selected post in datagrid
							' set its parent to the mPostID (the new threadid we are creating), see if it is lastpostid/mostrecentpostid anywhere (thread, forums), check thread status
							Dim PostIDToSplit As Integer = CInt(dlPostsForThread.DataKeys(item.ItemIndex))
							If (Not PostIDToSplit = _PostID) And (Not PostIDToSplit = _ThreadID) Then
								ctlPosts.PostMove(PostIDToSplit, _ThreadID, _PostID, newForumID, _OldForumID, UserId, newThreadSortOrder, Notes, _ForumInfo.ParentId)
							End If

							newThreadSortOrder += 1
						Else
							' make sure the parent post of this is an existing post that will remain in the thread. (Set to current threadID) 
							Dim PostIDToSplit As Integer = CInt(dlPostsForThread.DataKeys(item.ItemIndex))
							If (Not PostIDToSplit = _PostID) And (Not PostIDToSplit = _ThreadID) Then
								ctlPosts.PostMove(PostIDToSplit, _ThreadID, _ThreadID, newForumID, _OldForumID, UserId, oldThreadSortOrder, Notes, _ThreadInfo.HostForum.ParentId)
							End If

							oldThreadSortOrder += 1
						End If
					Next

					' reset the post cache
					PostInfo.ResetPostInfo(_ThreadID)
					PostInfo.ResetPostInfo(_PostID)

					' reset cache of both threads in case anyone happen to visit one during the split processing
					ThreadInfo.ResetThreadInfo(_ThreadID)
					ThreadInfo.ResetThreadInfo(_PostID)

					ThreadController.ResetThreadListCached(newForumID, ModuleId)
					ThreadController.ResetThreadListCached(_OldForumID, ModuleId)

					If objConfig.AggregatedForums Then
						ThreadController.ResetThreadListCached(-1, ModuleId)
					End If

					' the db calls handled caching at group level, make sure cache is updated at forum level(s)
					ForumController.ResetForumInfoCache(newForumID)

					If Not newForumID = _OldForumID Then
						ForumController.ResetForumInfoCache(_OldForumID)
					End If

					' Handle sending emails 
					If chkEmailUsers.Checked And objConfig.MailNotification Then
						Utilities.ForumUtils.SendForumMail(_PostID, strURL, ForumEmailType.UserThreadSplit, Notes, objConfig, MyProfileUrl, PortalId)
						' we have several scenarios: post approved - send mod emails of split and users email of split
						' Post Not Approved - send mods email of split, user email of approved
						' If the post wasn't approved, just send approval notic
					End If

					Dim objPost As PostInfo
					Dim PostCnt As New PostController
					objPost = PostCnt.PostGet(_PostID, PortalId)

					Response.Redirect(GetReturnURL(objPost), False)
				End If
			Catch ex As Exception
				LogException(ex)
			End Try
		End Sub

		''' <summary>
		''' Returns the user to where they came from
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
			Catch exc As Exception
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

			' This only needs to be handled for moderators (which are trusted by no matter what, so it only happens at this point)
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

#Region "Protected Functions"

		''' <summary>
		''' Determines if a checkbox in the datalist should be shown for a specific post
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="ThreadID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function EnabledSelector(ByVal PostID As Integer, ByVal ThreadID As Integer) As Boolean
			' if the current post is the first post in the old thread
			If PostID = ThreadID Then
				Return False
			Else
				' see if the post here is the post we are creating a new thread from
				If PostID = _PostID Then
					Return False
				Else
					Return True
				End If
			End If
		End Function

		''' <summary>
		''' Determines if the postmoderate image in the datalist should show for a specific post
		''' </summary>
		''' <param name="IsApproved"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function VisibleNeedsModImage(ByVal IsApproved As Boolean) As String
			If IsApproved = True Then
				Return objConfig.GetThemeImageURL("spacer.gif")
			Else
				Return objConfig.GetThemeImageURL("ma_moderate." & objConfig.ImageExtension)
			End If
		End Function

		''' <summary>
		''' Determines if the PostReported icon in the datalist should be displayed for a specific post
		''' </summary>
		''' <param name="PostReported"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function VisiblePostReported(ByVal PostReported As Integer) As String
			If PostReported > 0 Then
				Return objConfig.GetThemeImageURL("s_postabuse." & objConfig.ImageExtension)
			Else
				Return objConfig.GetThemeImageURL("spacer.gif")
			End If
		End Function

		''' <summary>
		''' Sets the total post count for a specific UserID
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function PostCount(ByVal UserID As Integer) As String
			Dim objUser As ForumUser
			objUser = ForumUserController.GetForumUser(UserID, False, ModuleId, PortalId)

			Return objUser.PostCount.ToString
		End Function

		''' <summary>
		''' Sets the SiteAlias for a specific UserID
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function UserAlias(ByVal UserID As Integer) As String
			Dim objUser As ForumUser
			objUser = ForumUserController.GetForumUser(UserID, False, ModuleId, PortalId)

			Return objUser.SiteAlias
		End Function

		''' <summary>
		''' Sets the height spacer image. 
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function SpacerImage() As String
			Return objConfig.GetThemeImageURL("alt_headfoot_height.gif")
		End Function

		''' <summary>
		''' Sets the litle spacer image used throughout.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function LilSpacerImage() As String
			Return objConfig.GetThemeImageURL("spacer.gif")
		End Function

		''' <summary>
		''' Formats the posts body, including emoticons and quote but doesn't include user's signature. 
		''' </summary>
		''' <param name="PostID">An integer representing which post we are going to process the body of. </param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' This is mainly pulled from Posts.vb's RenderPostBody method.
		''' </history>
		Protected Function FormatBody(ByVal PostID As Integer) As String
			Dim strFormatedBody As String = String.Empty
			Dim objPost As New PostInfo
			Dim cntPost As New PostController

			objPost = cntPost.PostGet(PostID, PortalId)


			If Not objPost Is Nothing Then
				Dim bodyForumText As Utilities.PostContent

				If objPost.ParseInfo = PostParserInfo.None Or objPost.ParseInfo = PostParserInfo.File Then
					'Nothing to Parse or just an Attachment not inline
					bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objPost.Body), objConfig)
				Else
					If objPost.ParseInfo < PostParserInfo.Inline Then
						'Something to parse, but not any inline instances
						bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objPost.Body), objConfig, objPost.ParseInfo)
					Else
						'At lease Inline to Parse
						If Users.UserController.GetCurrentUserInfo.UserID > 0 Then
							bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objPost.Body), objConfig, objPost.ParseInfo, objPost.Attachments, True)
						Else
							bodyForumText = New Utilities.PostContent(System.Web.HttpUtility.HtmlDecode(objPost.Body), objConfig, objPost.ParseInfo, objPost.Attachments, False)
						End If
					End If
				End If


				strFormatedBody = bodyForumText.ProcessHtml
			End If
			Return strFormatedBody
		End Function

		''' <summary>
		''' Formats the date a post was posted
		''' </summary>
		''' <param name="CreatedDate"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/9/2006	Created
		''' </history>
		Protected Function FormatCreatedDate(ByVal CreatedDate As Object) As String
			Dim strCreatedDate As String = String.Empty
			If Not IsDBNull(CreatedDate) Then
				Dim displayCreatedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(CType(CreatedDate, DateTime), objConfig)
				strCreatedDate = displayCreatedDate.ToString
			End If

			Return strCreatedDate

		End Function

		''' <summary>
		''' Formats the users joined date according to TimeZone
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function FormatJoinedDate(ByVal UserID As Integer) As String
			Dim strCreatedDate As String = String.Empty
			Dim objUser As ForumUser
			objUser = ForumUserController.GetForumUser(UserID, False, ModuleId, PortalId)

			Dim displayCreatedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(CType(objUser.UserJoinedDate, DateTime), objConfig)
			strCreatedDate = displayCreatedDate.ToShortDateString

			Return strCreatedDate
		End Function

		''' <summary>
		''' This generates the url string for a link to the user's profile (author)
		''' </summary>
		''' <param name="UserId"></param>
		''' <returns>String</returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/9/2006	Created
		''' </history>
		Protected Function UserProfileLink(ByVal UserId As Integer) As String
			Dim params As String()
			Dim url As String

			params = New String(1) {"mid=" & ModuleId.ToString, "userid=" & UserId.ToString}
			url = NavigateURL(TabId, "UserProfile", params)

			Return url
		End Function

#End Region

	End Class

End Namespace