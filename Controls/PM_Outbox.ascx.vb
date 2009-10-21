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
	''' This is the PM Outbox for a specific user.  It shows as a thread view
	''' of conversations.  
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[skeel]	11/26/2008	Created
	''' </history>
	Partial Public Class PMOutbox
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Private Members"

		Private _PageSize As Integer

#End Region

#Region "Private ReadOnly Properties"

		''' <summary>
		''' Determines if a thread has been read by the user. 
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="LastPMDate"></param>
		''' <param name="PMThreadID"></param>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property HasNewPosts(ByVal UserID As Integer, ByVal LastPMDate As DateTime, ByVal PMThreadID As Integer) As Boolean
			Get
				Dim ctlForumPMReads As New ForumPMReadsController
				Dim objPMReadInfo As ForumPMReadsInfo = ctlForumPMReads.PMReadsGet(UserID, PMThreadID)
				If objPMReadInfo Is Nothing Then
					Return True
				Else
					If objPMReadInfo.LastVisitDate < LastPMDate Then
						Return True
					Else
						Return False
					End If
				End If
			End Get
		End Property

#End Region

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Localization.LocalizeDataGrid(dgPMThreads, Me.LocalResourceFile)
			BottomPager.PageSize = Convert.ToInt32(LoggedOnUser.ThreadsPerPage)

			If Not LoggedOnUser.EnablePM Then
				Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
			End If

			BindData(BottomPager.PageSize, 1)
			cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Deletes selected PM threads for a user. Does not delete from db unless both users have deleted thread.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
			Dim I, Count As Integer

			Count = dgPMThreads.Items.Count - 1

			For I = 0 To Count
				Dim chk As CheckBox = CType(dgPMThreads.Items(I).FindControl("chkThread"), CheckBox)

				'Item checked for delete.
				If chk.Checked Then

					Dim ctlPM As New PMController
					Dim PM As PMInfo = ctlPM.PMGet(CInt(dgPMThreads.DataKeys(I)))
					'We have to confirm that the recipient have still not read this PM
					If IsNewPost(PM.PMToUserID, PM.CreatedDate, PM.PMThreadID) Then
						'Good, delete the PM and redirect til inbox
						ctlPM.PMDelete(PM.PMID, PM.PMThreadID)
						Response.Redirect(Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Inbox, PortalSettings))
					Else
						'Too bad, let's inform the user
						Skins.Skin.AddModuleMessage(Me, Localization.GetString("DelError"), Me.LocalResourceFile, Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
					End If

				End If
			Next

			BindData(BottomPager.PageSize, 1)
		End Sub

		''' <summary>
		''' Used to set properties for various sever controls used in the item template.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub dgPMThreads_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPMThreads.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As PMOutboxInfo = CType(e.Item.DataItem, PMOutboxInfo)
			Dim img As System.Web.UI.WebControls.Image
			Dim hl As HyperLink
			Dim lbl As Label

			hl = CType(e.Item.FindControl("hlSubject"), HyperLink)
			hl.Text = dataItem.Subject
			hl.NavigateUrl = Utilities.Links.PMThreadLinkFromOutbox(TabId, ModuleId, dataItem.PMThreadId, dataItem.PMID)
			hl.CssClass = "Forum_NormalBold"

			'show new icon
			img = CType(e.Item.FindControl("imgStatus"), Image)
			img.ImageUrl = objConfig.GetThemeImageURL("s_postunread.") & objConfig.ImageExtension
			img.ToolTip = Services.Localization.Localization.GetString("UnreadPost.Text", LocalResourceFile)

			hl = CType(e.Item.FindControl("hlStatus"), HyperLink)
			hl.NavigateUrl = Utilities.Links.PMThreadLinkFromOutbox(TabId, ModuleId, dataItem.PMThreadId, dataItem.PMID)

			lbl = CType(e.Item.FindControl("lblPMRecipient"), Label)
			lbl.Text = ToUserDetails(dataItem.PMToUserId)

			lbl = CType(e.Item.FindControl("lblPMCreatedDate"), Label)
			lbl.Text = CreatedDate(dataItem.CreatedDate)
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
		''' Binds the PM threads for a specific user, if none exist it will hide grid view, show notice to user.
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Private Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim ctlPM As New PMThreadController
			Dim arrPMOutbox As New List(Of PMOutboxInfo)
			arrPMOutbox = ctlPM.PMThreadGetOutBox(UserId, PageSize, CurrentPage - 1, PortalId)

			If Not arrPMOutbox Is Nothing Then
				If arrPMOutbox.Count > 0 Then
					Dim objPMOutboxInfo As PMOutboxInfo = arrPMOutbox.Item(0)

					dgPMThreads.DataKeyField = "PMID"
					dgPMThreads.DataSource = arrPMOutbox
					dgPMThreads.DataBind()

					BottomPager.TotalRecords = arrPMOutbox.Count

					pnlOutbox.Visible = True
					pnlNoItems.Visible = False
				Else
					pnlOutbox.Visible = False
					pnlNoItems.Visible = True
				End If
			Else
				pnlOutbox.Visible = False
				pnlNoItems.Visible = True
			End If
		End Sub

#End Region

#Region "Protected Function"

		''' <summary>
		''' Formats the created date info for a pm.
		''' </summary>
		''' <param name="LastPMDate"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function CreatedDate(ByVal LastPMDate As DateTime) As String
			Dim strLastPMInfo As String = String.Empty
			strLastPMInfo = Utilities.ForumUtils.GetCreatedDateInfo(LastPMDate, objConfig, "Forum_LastPostText")
			Return strLastPMInfo
		End Function

		''' <summary>
		''' Formats whow is receiving the PM
		''' </summary>
		''' <param name="PMToUserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function ToUserDetails(ByVal PMToUserID As Integer) As String
			Dim objUser As ForumUser
			objUser = ForumUserController.GetForumUser(PMToUserID, False, ModuleId, PortalId)

			Return objUser.SiteAlias
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
