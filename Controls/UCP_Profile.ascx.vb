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
	''' This is the users "Edit Profile" page and also the page used for administrators of
	''' the module to change forum user settings. This is the forum users profile.
	''' </summary>
	''' <remarks></remarks>
	Partial Public Class Profile
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, CurrentForumUser.UserID)
			Dim cntForumUser As New ForumUserController
			Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

			rdpLiftBan.MinDate = Date.Now()

			With ProfileUser
				txtUserID.Text = .UserID.ToString
				lblUserName.Text = .Username
				lblDisplayName.Text = .SiteAlias
				hlEmail.Text = .Email
				hlEmail.NavigateUrl = "mailto:" & .Email
				' Impersontation would go here.
				chkIsTrusted.Checked = .IsTrusted
				chkLockTrust.Checked = .LockTrust
				chkIsBanned.Checked = .IsBanned
				If .LiftBanDate >= rdpLiftBan.MinDate Then
					rdpLiftBan.SelectedDate = .LiftBanDate
				End If
			End With

			If objConfig.EnableUserBanning And Security.IsForumAdmin Then
				rowUserBanning.Visible = True
				'rdpLiftBan.SelectedDate = DateAdd(DateInterval.Day, 7, Date.Today)
			Else
				rowUserBanning.Visible = False
			End If

			If chkIsBanned.Checked And Security.IsForumAdmin Then
				rowLiftBanDate.Visible = True
			Else
				rowLiftBanDate.Visible = False
			End If

			rowTrust.Visible = Security.IsModerator
			rowLockTrust.Visible = Security.IsForumAdmin

			ViewState("Alias") = ProfileUser.SiteAlias
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
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				Dim cntForumUser As New ForumUserController
				Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

				With ProfileUser
					Dim PreviouslyBanned As Boolean
					.UserID = .UserID
					.PortalID = PortalId
					.IsTrusted = chkIsTrusted.Checked
					.LockTrust = chkLockTrust.Checked

					PreviouslyBanned = .IsBanned

					.IsBanned = chkIsBanned.Checked
					If chkIsBanned.Checked Then
						If rdpLiftBan.SelectedDate < Date.Now() Then
							.LiftBanDate = CDate(rdpLiftBan.SelectedDate)
						End If
						If Not PreviouslyBanned Then
							.StartBanDate = Date.Now()
						End If
					Else
						.LiftBanDate = Null.NullDate
						.StartBanDate = Null.NullDate
					End If

					Dim ctlForumUser As New ForumUserController
					ctlForumUser.Update(ProfileUser)

					DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(.UserID, PortalId)
					' We need an audit trail for banning users, check to see if the user was banned before this visit
					If (Not PreviouslyBanned) And chkIsBanned.Checked Then
						' Audit the user as being banned in moderation audit table
					End If
				End With

				lblUpdateDone.Visible = True
			Catch Exc As System.Exception
				LogException(Exc)
				Return
			End Try
		End Sub

		''' <summary>
		''' Used to show/hide banned lift date area when the banned checkbox is changed.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub chkIsBanned_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkIsBanned.CheckedChanged
			If chkIsBanned.Checked Then
				rowLiftBanDate.Visible = True
			Else
				rowLiftBanDate.Visible = False
			End If
		End Sub

#End Region

	End Class

End Namespace