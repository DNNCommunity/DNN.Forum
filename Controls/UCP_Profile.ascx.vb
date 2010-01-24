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

Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Entities.Users

Namespace DotNetNuke.Modules.Forum.UCP

	''' <summary>
	''' This is the users "Edit Profile" page and also the page used for administrators of
	''' the module to change forum user settings. This is the forum users profile.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[skeel]	11/28/2008	Created
	''' </history>
	Partial Public Class Profile
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)
			Dim ProfileUser As ForumUser = ForumUserController.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

			'[skeel] get visibility settings
			Dim ProfileProp As ProfilePropertyDefinition
			Dim WebVisibility As UserVisibilityMode
			Dim RegionVisibility As UserVisibilityMode
			'Dim BiographyVisibility As UserVisibilityMode
			Dim EmailVisibility As UserVisibilityMode
			Dim IMVisibility As UserVisibilityMode

			For Each ProfileProp In ProfileUser.Profile.ProfileProperties
				Select Case ProfileProp.PropertyName
					Case "Website"
						WebVisibility = ProfileProp.Visibility
					Case "Region"
						RegionVisibility = ProfileProp.Visibility
					Case "Biography"
						'BiographyVisibility = ProfileProp.Visibility
					Case "Email"
						EmailVisibility = ProfileProp.Visibility
					Case "IM"
						IMVisibility = ProfileProp.Visibility
				End Select
			Next

			With ProfileUser
				txtUserID.Text = .UserID.ToString
				lblUserName.Text = .Username
				lblDisplayName.Text = .SiteAlias
				lblWebsite.Text = .Profile.Website
				visWebsite.Text = Localization.GetString("Visibility", Me.LocalResourceFile) & ": " & WebVisibility.ToString
				hlEmail.Text = .Email
				hlEmail.NavigateUrl = "mailto:" & .Email
				visEmail.Text = Localization.GetString("Visibility", Me.LocalResourceFile) & ": " & EmailVisibility.ToString
				chkEnableProfileWeb.Checked = .EnableProfileWeb
				chkEnableProfileRegion.Checked = .EnableProfileRegion
				visRegion.Text = Localization.GetString("Visibility", Me.LocalResourceFile) & ": " & RegionVisibility.ToString
				lblRegion.Text = .Profile.Region

				'txtBiography.Text = HttpUtility.HtmlDecode(.Biography)
				'visBiography.Text = Localization.GetString("Visibility", Me.LocalResourceFile) & ": " & BiographyVisibility.ToString
				'visBiography.Visible = False

				' Impersontation would go here.
				chkIsTrusted.Checked = .IsTrusted
				chkLockTrust.Checked = .LockTrust
				chkIsBanned.Checked = .IsBanned
				txtLiftBanDate.Text = .LiftBanDate.ToShortDateString()
			End With

			If objConfig.EnableUserBanning And Security.IsForumAdmin Then
				rowUserBanning.Visible = True
				With Me.cmdCalBanDate
					.ImageUrl = objConfig.GetThemeImageURL("s_calendar.") & objConfig.ImageExtension
					.NavigateUrl = CType(DotNetNuke.Common.Utilities.Calendar.InvokePopupCal(txtLiftBanDate), String)
					.ToolTip = Localization.GetString("cmdCalBanDate", LocalResourceFile)
				End With

				txtLiftBanDate.Text = DateAdd(DateInterval.Day, 7, Date.Today).ToShortDateString
			Else
				rowUserBanning.Visible = False
			End If

			If chkIsBanned.Checked And Security.IsForumAdmin Then
				rowLiftBanDate.Visible = True
			Else
				rowLiftBanDate.Visible = False
			End If

			ViewState("Alias") = ProfileUser.SiteAlias
			EnableControls(Security)
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
				Dim ProfileUser As ForumUser = ForumUserController.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

				With ProfileUser
					Dim PreviouslyBanned As Boolean
					.UserID = .UserID
					.PortalID = PortalId
					.EnableProfileWeb = chkEnableProfileWeb.Checked
					.EnableProfileRegion = chkEnableProfileRegion.Checked
					.IsTrusted = chkIsTrusted.Checked
					.LockTrust = chkLockTrust.Checked
					.Biography = HttpUtility.HtmlEncode(txtBiography.Text)

					PreviouslyBanned = .IsBanned

					.IsBanned = chkIsBanned.Checked
					If chkIsBanned.Checked Then
						If txtLiftBanDate.Text.Trim() <> String.Empty Then
							.LiftBanDate = CDate(txtLiftBanDate.Text)
						End If
						If Not PreviouslyBanned Then
							.StartBanDate = Date.Now()
						End If
					Else
						.LiftBanDate = Null.NullDate
						.StartBanDate = Null.NullDate
					End If

					Dim cntForumUser As New ForumUserController
					cntForumUser.Update(ProfileUser)

					ForumUserController.ResetForumUser(.UserID, PortalId)
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

#Region "Private Methods"

		''' <summary>
		''' Shows/Hides controls depending on module settings and user configuration.
		''' </summary>
		''' <remarks></remarks>
		Private Sub EnableControls(ByVal objSecurity As Forum.ModuleSecurity)
			tblAdmin.Visible = objSecurity.IsForumAdmin
		End Sub

#End Region

	End Class

End Namespace