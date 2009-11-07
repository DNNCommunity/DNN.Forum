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
	''' This is a single private message thread.  It looks like a forum post
	''' screen by design.  It was designed to show an entire thread as a 
	''' single conversation and allow the user to reply like posts view.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	1/22/2006	Created
	''' </history>
	Partial Public MustInherit Class PMThread
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Dim _PageSize As Integer
		Dim _PMID As Integer
		Dim _PMThreadID As Integer
		Dim _PMThreadDeleted As Boolean = False
		Dim _CurrentForumUser As ForumUser
		Dim _IsOutboxItem As Boolean

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

#Region "Private Properties"

		''' <summary>
		''' Loads the forum module configuration
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property ForumConfig() As Forum.Config
			Get
				Return Config.GetForumConfig(ModuleId)
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property IsOutboxItem() As Boolean
			Get
				Return _IsOutboxItem
			End Get
			Set(ByVal value As Boolean)
				_IsOutboxItem = value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property PMID() As Integer
			Get
				Return _PMID
			End Get
			Set(ByVal value As Integer)
				_PMID = value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property PMThreadID() As Integer
			Get
				Return _PMThreadID
			End Get
			Set(ByVal value As Integer)
				_PMThreadID = value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property PMThreadDeleted() As Boolean
			Get
				Return _PMThreadDeleted
			End Get
			Set(ByVal Value As Boolean)
				_PMThreadDeleted = Value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property CurrentForumUser() As ForumUser
			Get
				Return _CurrentForumUser
			End Get
			Set(ByVal value As ForumUser)
				_CurrentForumUser = value
			End Set
		End Property

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
				DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, False)
			End If
		End Sub

		''' <summary>
		''' Make sure user belongs on this page, set defaults and necessary
		''' variables.  
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/22/2006	Created
		''' </history>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
			Try
				' Before we do anything, see if the user is logged in and PM's are enabled
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)
				Dim LoggedOnUserID As Integer = -1

				If Request.IsAuthenticated And (ForumConfig.EnablePMSystem = True) Then
					LoggedOnUserID = Users.UserController.GetCurrentUserInfo.UserID
				Else
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				CurrentForumUser = ForumUserController.GetForumUser(LoggedOnUserID, False, ModuleId, PortalId)
				BottomPager.PageSize = Convert.ToInt32(CurrentForumUser.PostsPerPage)

				' Make sure there is a pmthreadid, if not redirect the user
				If Not Request.QueryString("pmthreadid") Is Nothing Then
					PMThreadID = Int32.Parse(Request.QueryString("pmthreadid"))

					Dim PThreadInfo As New PMThreadInfo
					PThreadInfo = Forum.PMThreadInfo.GetPMThreadInfo(PMThreadID)

					'[skeel] 
					If Not PThreadInfo Is Nothing Then
						PMThreadDeleted = (PThreadInfo.PMStartUserDeleted OrElse PThreadInfo.PMToUserDeleted)

						' Make sure user is recipient or sender
						If (PThreadInfo.PMStartThreadUserID = LoggedOnUserID) Or (PThreadInfo.PMReceiveThreadUserID = LoggedOnUserID) Then
							If PMThreadDeleted Then
								lblDeleted.Visible = True
								If PThreadInfo.PMToUserDeleted Then
									lblDeleted.Text = "<br/>" + Localization.GetString("ReciverDeletedThread", Me.LocalResourceFile)
								Else
									lblDeleted.Text = "<br/>" + Localization.GetString("SenderDeletedThread", Me.LocalResourceFile)
								End If
							Else
								lblDeleted.Visible = False
							End If
						Else
							' they don't belong here
							Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
						End If
					Else
						'No such threadid, redirect the user
						Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
					End If

				Else
					' they don't belong here
					Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Not Page.IsPostBack Then

					litCSSLoad.Text = "<link href='" & ForumConfig.Css & "' type='text/css' rel='stylesheet' />"

					'[skeel] check if we were refered from the outbox
					If Not Request.QueryString("pmid") Is Nothing Then
						PMID = Int32.Parse(Request.QueryString("pmid"))
						'Now we have to verify the PMID exist
						Dim ctlPM As New PMController
						Dim PMInfo As PMInfo
						PMInfo = ctlPM.PMGet(PMID)
						If Not PMInfo Is Nothing Then
							If PMInfo.PMThreadID = PMThreadID Then
								'Was it sent by the current user?
								If PMInfo.PMFromUserID = CurrentForumUser.UserID Then
									'Verify that recipient have not read the PM yet
									IsOutboxItem = IsNewPost(PMInfo.PMToUserID, PMInfo.CreatedDate, PMInfo.PMThreadID)
								End If
							End If
						End If
					End If

					' Register scripts
					Utilities.ForumUtils.RegisterPageScripts(Page, ForumConfig)

					' Store the referrer for returning to where the user came from
					If Not Request.UrlReferrer Is Nothing Then
						ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
					End If

					'Possibly add pmid to go directly to pm
					BindList(BottomPager.PageSize, 1)
					' handle reads status
					HandleUserPMReads(CurrentForumUser.UserID, PMThreadID, CurrentForumUser)
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Actions based on button clicked (these are reply, quote, etc)
		''' </summary>
		''' <param name="Sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/22/2006	Created
		''' </history>
		Protected Sub lstPost_ItemCommand(ByVal Sender As System.Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles lstPost.ItemCommand
			Try
				' Determine the command of the button             
				Dim cmdButton As LinkButton = DirectCast(e.CommandSource, LinkButton)
				Dim PMID As Integer = Int32.Parse(cmdButton.CommandArgument)
				Dim selListItem As DataListItem = DirectCast(e.Item, DataListItem)
				Dim _nextURL As String = String.Empty
				Dim ctlPM As New PMController
				Dim _post As PMInfo = ctlPM.PMGet(PMID)

				Select Case cmdButton.CommandName.ToLower

					Case "reply"
						_nextURL = Utilities.Links.NewPMReplyLink(TabId, ModuleId, _post.PMID, "reply")
					Case "quote"
						_nextURL = Utilities.Links.NewPMReplyLink(TabId, ModuleId, _post.PMID, "quote")
					Case "edit"
						'redirect to PM post edit
						Response.Redirect(Utilities.Links.PMEditLink(_post.PMID, TabId, ModuleId))
					Case "delete"
						'We have to confirm that the recipient have still not read this PM
						If IsNewPost(_post.PMToUserID, _post.CreatedDate, _post.PMThreadID) Then
							'Good, delete the PM and redirect til inbox
							Dim ctl As New PMController
							ctl.PMDelete(_post.PMID, _post.PMThreadID)
							Response.Redirect(Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Inbox, PortalSettings), False)
						Else
							'Too bad, let's inform the user
							Skins.Skin.AddModuleMessage(Me, Localization.GetString("DelError"), Me.LocalResourceFile, Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
						End If

				End Select
				Response.Redirect(_nextURL, False)
			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Binds/Formats the datalist items
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub lstPost_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles lstPost.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As PMInfo = CType(e.Item.DataItem, PMInfo)

			Dim hl As HyperLink
			Dim lbl As Label
			Dim cmd As LinkButton
			Dim img As Image

			Dim objFromUser As ForumUser = ForumUserController.GetForumUser(dataItem.PMFromUserID, False, ModuleId, PortalId)
			Dim objToUser As ForumUser = ForumUserController.GetForumUser(dataItem.PMToUserID, False, ModuleId, PortalId)

			img = CType(e.Item.FindControl("imgHeadSpacerL"), Image)
			img.ImageUrl = ForumConfig.GetThemeImageURL("header_cap_left.gif")

			img = CType(e.Item.FindControl("imgHeadSpacerR"), Image)
			img.ImageUrl = ForumConfig.GetThemeImageURL("header_cap_right.gif")

			img = CType(e.Item.FindControl("imgPostRead"), Image)

			'Is this an Outbox unread Item?
			If IsOutboxItem = True And PMID = dataItem.PMID Then
				img.ImageUrl = ForumConfig.GetThemeImageURL("s_new.") & ForumConfig.ImageExtension
				img.ToolTip = Services.Localization.Localization.GetString("UnreadPost.Text", LocalResourceFile)
			Else
				Dim PostIsNew As Boolean = False
				PostIsNew = IsNewPost(UserId, dataItem.CreatedDate, dataItem.PMThreadID)
				If PostIsNew Then
					img.ImageUrl = ForumConfig.GetThemeImageURL("s_new.") & ForumConfig.ImageExtension
					img.ToolTip = Services.Localization.Localization.GetString("UnreadPost.Text", LocalResourceFile)
				Else
					img.ImageUrl = ForumConfig.GetThemeImageURL("s_old.") & ForumConfig.ImageExtension
					img.ToolTip = Services.Localization.Localization.GetString("ReadPost.Text", LocalResourceFile)
				End If
			End If

			'Online/offline image
			Dim authorOnline As Boolean = (objFromUser.EnableOnlineStatus AndAlso objFromUser.IsOnline AndAlso (ForumConfig.EnableUsersOnline))
			img = CType(e.Item.FindControl("imgOnline"), Image)
			If ForumConfig.EnableUsersOnline Then
				If authorOnline = True Then
					img.ImageUrl = ForumConfig.GetThemeImageURL("s_online.") & ForumConfig.ImageExtension
					img.AlternateText = Localization.GetString("imgOnline", Me.LocalResourceFile)
					img.ToolTip = Localization.GetString("imgOnline", Me.LocalResourceFile)
				Else
					img.ImageUrl = ForumConfig.GetThemeImageURL("s_offline.") & ForumConfig.ImageExtension
					img.AlternateText = Localization.GetString("imgOffline", Me.LocalResourceFile)
					img.ToolTip = Localization.GetString("imgOffline", Me.LocalResourceFile)
				End If
			Else
				img.Visible = False
			End If

			'Ranking
			img = CType(e.Item.FindControl("imgRank"), Image)
			If ForumConfig.Ranking = True Then
				Dim authorRank As PosterRank = Utilities.ForumUtils.GetRank(objFromUser, ForumConfig)
				Dim rankImage As String = String.Format("Rank_{0}." & ForumConfig.ImageExtension, CType(authorRank, Integer).ToString)
				Dim rankURL As String = ForumConfig.GetThemeImageURL(rankImage)
				Dim RankTitle As String = Utilities.ForumUtils.GetRankTitle(authorRank, ForumConfig)
				img.ImageUrl = rankURL
				img.AlternateText = RankTitle
				img.ToolTip = RankTitle
			Else
				img.Visible = False
			End If

			hl = CType(e.Item.FindControl("lblAlias"), HyperLink)
			hl.Text = objFromUser.SiteAlias
			hl.NavigateUrl = Utilities.Links.UserPublicProfileLink(TabId, ModuleId, objFromUser.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage)

			lbl = CType(e.Item.FindControl("lblPMSubject"), Label)
			lbl.Text = dataItem.Subject

			hl = CType(e.Item.FindControl("hlToUser"), HyperLink)
			hl.Text = objToUser.SiteAlias
			hl.NavigateUrl = Utilities.Links.UserPublicProfileLink(TabId, ModuleId, objToUser.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage)

			lbl = CType(e.Item.FindControl("lblUserJoinedDate"), Label)
			Dim JoinedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(objFromUser.UserJoinedDate, ForumConfig)
			lbl.Text = JoinedDate.ToShortDateString

			img = CType(e.Item.FindControl("userAvatar"), Image)
			If objFromUser.UserAvatar > 0 Then
				img.ImageUrl = objFromUser.AvatarComplete
				img.Visible = True
			Else
				img.Visible = False
			End If

			lbl = CType(e.Item.FindControl("lblCreatedDate"), Label)
			lbl.Text = Utilities.ForumUtils.ConvertTimeZone(CType(dataItem.CreatedDate, DateTime), ForumConfig).ToString()

			cmd = CType(e.Item.FindControl("cmdReply"), LinkButton)
			cmd.CommandArgument = dataItem.PMID.ToString()

			'Is this an Outbox unread Item?
			If IsOutboxItem = True And PMID = dataItem.PMID Then
				'It is :-)
				cmd.Visible = True
				cmd.CommandName = "Edit"
				cmd.Text = Localization.GetString("cmdEdit", Me.LocalResourceFile)
			ElseIf IsOutboxItem = True And PMID <> dataItem.PMID Then
				'Outbox History Item, lets disable reply and qoute
				cmd.Visible = False
			Else
				'Normal Post
				cmd.CommandName = "Reply"
				cmd.Visible = Not PMThreadDeleted
				cmd.Text = Localization.GetString("cmdReply", Me.LocalResourceFile)
			End If

			cmd = CType(e.Item.FindControl("cmdQuote"), LinkButton)
			cmd.CommandArgument = dataItem.PMID.ToString()

			'Is this an Outbox Item?
			If IsOutboxItem = True And PMID = dataItem.PMID Then
				'It is :-)
				cmd.Visible = True
				cmd.CommandName = "Delete"
				cmd.Text = Localization.GetString("cmdDelete", Me.LocalResourceFile)
				cmd.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
			ElseIf IsOutboxItem = True And PMID <> dataItem.PMID Then
				'Outbox History Item, lets disable reply and qoute
				cmd.Visible = False
			Else
				'Normal Post
				cmd.Visible = Not PMThreadDeleted
				cmd.CommandName = "Quote"
				cmd.Text = Localization.GetString("cmdQuote", Me.LocalResourceFile)
			End If

			lbl = CType(e.Item.FindControl("lblPostBody"), Label)
			lbl.Text = FormatBody(dataItem.Body)

		End Sub

		''' <summary>
		''' Executed when the pager is clicked (to change the page). 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub pager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BottomPager.Command
			Dim CurrentPage As Int32 = CType(e.CommandArgument, Int32)
			BottomPager.CurrentPage = CurrentPage
			BindList(BottomPager.PageSize, CurrentPage)
		End Sub

		''' <summary>
		''' Takes the user back to where they were.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/15/2006	Created
		''' </history>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			If Not ViewState("UrlReferrer") Is Nothing Then
				Response.Redirect(CType(ViewState("UrlReferrer"), String), False)
			Else
				Response.Redirect(Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Inbox, PortalSettings), False)
			End If
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Checks the read status of the current thread and user combination, updates or adds the lastvisitdate.
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="PMThreadID"></param>
		''' <remarks></remarks>
		Private Sub HandleUserPMReads(ByVal UserID As Integer, ByVal PMThreadID As Integer, ByVal mUser As ForumUser)
			Dim ctlForumPMReads As New ForumPMReadsController
			Dim objPMReadInfo As ForumPMReadsInfo = ctlForumPMReads.PMReadsGet(UserID, PMThreadID)
			If Not objPMReadInfo Is Nothing Then
				' Add error handling Just in case because of constraints and data integrity - This is highly unlikely to occur so do it here instead of the database(performance reasons)
				Try
					' Only update if the LastvisitDate is more than five seconds older
					If objPMReadInfo.LastVisitDate < DateAdd(DateInterval.Second, -5, Now) Then
						objPMReadInfo.UserID = UserID
						objPMReadInfo.LastVisitDate = Now
						ctlForumPMReads.PMReadsUpdate(objPMReadInfo)
					End If
				Catch exc As Exception
					LogException(exc)
				End Try
			Else
				objPMReadInfo = New ForumPMReadsInfo
				With objPMReadInfo
					.UserID = UserID
					.PMThreadID = PMThreadID
					.LastVisitDate = Now
					.PortalID = PortalId
				End With
				ctlForumPMReads.PMReadsAdd(objPMReadInfo)
			End If

			ForumPMReadsController.ResetUserNewPMCount(UserID)
		End Sub

		''' <summary>
		''' Binds a list of non-deleted private messages for a user (single thread)
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks></remarks>
		Private Sub BindList(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			If PMThreadID > 0 Then
				Dim ctlPM As New PMController
				Dim arrPMThreads As New List(Of PMInfo)
				'[skeel] PM Thread should always be newest PM at top
				'arrPMThreads = ctlPM.PMGetAll(PMThreadID, CurrentPage - 1, PageSize, CurrentForumUser.ViewDescending)
				arrPMThreads = ctlPM.PMGetAll(PMThreadID, CurrentPage - 1, PageSize, True)

				If Not arrPMThreads Is Nothing Then
					If arrPMThreads.Count > 0 Then
						Dim objPMThreadInfo As PMInfo = arrPMThreads.Item(0)
						lstPost.DataSource = arrPMThreads
						lstPost.DataBind()

						BottomPager.TotalRecords = objPMThreadInfo.TotalRecords
						BottomPager.Visible = True
					Else
						lstPost.Visible = False
						BottomPager.Visible = False
					End If
				Else
					lstPost.Visible = False
					BottomPager.Visible = False
				End If
			Else
				' Houston, we have a problem
				Response.Redirect(Utilities.Links.NoContentLink(TabId, ModuleId), False)
			End If
		End Sub

		''' <summary>
		''' Formats the posts boyd
		''' </summary>
		''' <param name="Body"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/22/2006	Created
		''' </history>
		Private Function FormatBody(ByVal Body As Object) As String
			Dim formatedBody As String = String.Empty
			If Not IsDBNull(Body) Then
				Dim bodyForumText As Utilities.PostContent = New Utilities.PostContent(Server.HtmlDecode(CType(Body, String)), ForumConfig)
				formatedBody = bodyForumText.ProcessHtml
			End If
			Return formatedBody
		End Function

		''' <summary>
		''' Determines if a post has been read by the user. 
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="PostedDate"></param>
		''' <param name="PMThreadID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function IsNewPost(ByVal UserID As Integer, ByVal PostedDate As DateTime, ByVal PMThreadID As Integer) As Boolean
			Dim ctlForumPMReads As New ForumPMReadsController
			Dim objPMReadInfo As ForumPMReadsInfo = ctlForumPMReads.PMReadsGet(UserID, PMThreadID)
			If objPMReadInfo Is Nothing Then
				Return True
			Else
				If objPMReadInfo.LastVisitDate < PostedDate Then
					Return True
				Else
					Return False
				End If
			End If
		End Function

#End Region

	End Class

End Namespace