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
	''' This is the users "My Settings" page and also the page used for administrators of
	''' the module to change forum user settings. This is the forum users profile.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class Settings
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Dim cntForumUser As New ForumUserController
			Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)
			Dim objSecurity As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

			ddlEmailFormat.Items.Clear()

			ddlEmailFormat.Items.Insert(0, New ListItem(Localization.GetString("Text", objConfig.SharedResourceFile), "0"))
			ddlEmailFormat.Items.Insert(1, New ListItem(Localization.GetString("HTML", objConfig.SharedResourceFile), "1"))

			With ProfileUser
				txtPostsPerPage.Text = .PostsPerPage.ToString
				txtThreadsPerPage.Text = .ThreadsPerPage.ToString
				chkOnlineStatus.Checked = .EnableOnlineStatus
				chkEnableDefaultPostNotify.Checked = .EnableDefaultPostNotify
				chkEnableSelfNotifications.Checked = .EnableSelfNotifications
				chkEnableForumModNotify.Checked = .EnableModNotification
				ddlEmailFormat.SelectedValue = .EmailFormat.ToString

				If objConfig.EnableUsersOnline Then
					rowOnlineStatus.Visible = True
				Else
					rowOnlineStatus.Visible = False
				End If

				If Not objConfig.EnableUserReadManagement Then
					rowClearReads.Visible = False
				End If
			End With
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Updates the users Forum settings
		''' </summary>
		''' <param name="sender">System.Object</param>
		''' <param name="e">System.EventArgs</param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				Dim cntForumUser As New ForumUserController
				Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

				With ProfileUser
					.EnableOnlineStatus = chkOnlineStatus.Checked
					.PostsPerPage = Int32.Parse(txtPostsPerPage.Text)
					.ThreadsPerPage = Int32.Parse(txtThreadsPerPage.Text)
					.EnableModNotification = chkEnableForumModNotify.Checked
					.EmailFormat = CType(ddlEmailFormat.SelectedValue, Integer)
					.PortalID = PortalId
					.UserID = ProfileUser.UserID
					.EnableDefaultPostNotify = chkEnableDefaultPostNotify.Checked
					.EnableSelfNotifications = chkEnableSelfNotifications.Checked

					Dim cntUser As New ForumUserController
					cntUser.Update(ProfileUser)
					DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(ProfileUser.UserID, PortalId)
				End With

				lblUpdateDone.Visible = True
			Catch Exc As System.Exception
				LogException(Exc)
				Return
			End Try
		End Sub

		''' <summary>
		''' This clears all forum and thread read status items for a single user
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	12/16/2005	Created
		''' </history>
		Protected Sub cmdClearReads_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClearReads.Click
			DataProvider.Instance().UserDeleteReads(ProfileUserID)
		End Sub

#End Region

	End Class

End Namespace