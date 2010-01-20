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

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' This is the PM Center for a specific user.  It shows as a thread view
	''' of conversations.  
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	11/19/2006	Created
	''' </history>
	Partial Public Class PMInbox
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
		Protected Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
			Dim I, Count As Integer
			Dim cntPmThreads As New PMThreadController

			Count = dgPMThreads.Items.Count - 1

			For I = 0 To Count
				Dim chk As CheckBox = CType(dgPMThreads.Items(I).FindControl("chkThread"), CheckBox)

				'Item checked for delete.
				If chk.Checked Then
					Dim pmThreadId As Integer = CInt(dgPMThreads.DataKeys(I))
					cntPmThreads.PMThreadDelete(pmThreadId, UserId)
				End If
			Next

			BindData(BottomPager.PageSize, 1)
		End Sub

		''' <summary>
		''' Used to set properties for various sever controls used in the grid's template.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dgPMThreads_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPMThreads.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim dataItem As PMThreadInfo = CType(e.Item.DataItem, PMThreadInfo)
			Dim img As System.Web.UI.WebControls.Image
			Dim hl As HyperLink
			Dim lbl As Label

			dataItem.PortalID = PortalId

			hl = CType(e.Item.FindControl("hlSubject"), HyperLink)
			hl.Text = dataItem.PMThreadSubject
			hl.NavigateUrl = Utilities.Links.PMThreadLink(TabId, ModuleId, dataItem.PMThreadID)
			img = CType(e.Item.FindControl("imgStatus"), Image)
			' switch view status and icon depending on read/unread
			If HasNewPosts(UserId, dataItem.LastPM.CreatedDate, dataItem.PMThreadID) Then
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

			hl = CType(e.Item.FindControl("hlStatus"), HyperLink)
			hl.NavigateUrl = Utilities.Links.PMThreadLink(TabId, ModuleId, dataItem.PMThreadID)

			lbl = CType(e.Item.FindControl("lblStarter"), Label)
			lbl.Text = StartedByDetails(dataItem.PMFromUserID)

			lbl = CType(e.Item.FindControl("lblPMRecipient"), Label)
			lbl.Text = ToUserDetails(dataItem.PMToUserID)

			lbl = CType(e.Item.FindControl("lblLastPMInfo"), Label)
			lbl.Text = LastPMDetails(dataItem.LastPM.CreatedDate, dataItem.LastPM.PMFromUserID)
		End Sub

		''' <summary>
		''' Used to change the page bound to the grid (via ajax).
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub pager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BottomPager.Command
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
			Dim arrPMThreads As New List(Of PMThreadInfo)
			arrPMThreads = ctlPM.PMThreadGetAll(UserId, PageSize, CurrentPage - 1, PortalId)

			If Not arrPMThreads Is Nothing Then
				If arrPMThreads.Count > 0 Then
					Dim objPMThreadInfo As PMThreadInfo = arrPMThreads.Item(0)

					dgPMThreads.DataKeyField = "PMThreadID"
					dgPMThreads.DataSource = arrPMThreads
					dgPMThreads.DataBind()

					cmdDelete.Visible = True
					dgPMThreads.Visible = True
					BottomPager.TotalRecords = objPMThreadInfo.TotalRecords
					pnlNoItems.Visible = False
					pnlInbox.Visible = True
				Else
					pnlNoItems.Visible = True
					pnlInbox.Visible = False
				End If
			Else
				pnlNoItems.Visible = True
				pnlInbox.Visible = False
			End If
		End Sub

#End Region

#Region "Protected Function"

		''' <summary>
		''' Formats the last post info for a pm thread.
		''' </summary>
		''' <param name="LastPMDate"></param>
		''' <param name="LastPMUserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function LastPMDetails(ByVal LastPMDate As DateTime, ByVal LastPMUserID As Integer) As String
			Dim strLastPMInfo As String = String.Empty
			Dim objUser As ForumUser
			objUser = ForumUserController.GetForumUser(LastPMUserID, False, ModuleId, PortalId)

			strLastPMInfo = Utilities.ForumUtils.GetCreatedDateInfo(LastPMDate, objConfig, "Forum_LastPostText")
			strLastPMInfo += Services.Localization.Localization.GetString("by.Text", LocalResourceFile) & " " & objUser.SiteAlias

			Return strLastPMInfo
		End Function

		''' <summary>
		''' Formats the user which started the pm thread.
		''' </summary>
		''' <param name="PMStartUserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function StartedByDetails(ByVal PMStartUserID As Integer) As String
			Dim objUser As ForumUser
			objUser = ForumUserController.GetForumUser(PMStartUserID, False, ModuleId, PortalId)

			Return Services.Localization.Localization.GetString("by.Text", LocalResourceFile) & " " & objUser.SiteAlias
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

#End Region

	End Class

End Namespace
