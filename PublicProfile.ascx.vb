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

Imports DotNetNuke.Entities.Users

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' The profile page allows end users a way to view things about other
	''' users and is a gateway to interact w/ the user (if enabled)
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Class PublicProfile
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Private ProfileUser As ForumUser
		Private _LoggedOnUser As ForumUser
		Private _IsModerator As Boolean = False

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

#Region "Event Handlers"

		''' <summary>
		''' Loads a users profile and binds the data to the page
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
			Try
				litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

				Dim objSecurity As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				If Request.QueryString("userid") IsNot Nothing Then
					' This needs to be done on every page load (since we are not using viewstate). 
					Dim profileUserID As Integer = -1
					profileUserID = Int32.Parse(Request.QueryString("userid"))
					ProfileUser = ForumUserController.GetForumUser(profileUserID, False, ModuleId, PortalId)
					_IsModerator = False

					' See if they are admin or moderator - always have to be some type of mod or admin to edit (and be logged in)
					If LoggedOnUser.UserID > 0 And (objSecurity.IsModerator) Then
						EnableControls(True)
						_IsModerator = True
					Else
						_IsModerator = False
					End If
				End If

				rowModifyUserAvatar.Visible = False

				' See if the user is admin or moderator to update users moderator settings
				If _IsModerator Then
					' Only show the manage user to the forum admin
					If objSecurity.IsForumAdmin Then
						cmdManageUser.Visible = True
					Else
						cmdManageUser.Visible = False
					End If

					If objConfig.EnableUserAvatar Then
						rowModifyUserAvatar.Visible = True
					End If
				Else
					cmdUpdate.Visible = False
					cmdManageUser.Visible = False
					rowModeratorAdmin.Visible = False
				End If

				If Not Page.IsPostBack Then
					txtAlias.Text = ProfileUser.SiteAlias
					lblPostCount.Text = ProfileUser.PostCount.ToString()

					Dim WebSiteVisibility As UserVisibilityMode
					WebSiteVisibility = ProfileUser.Profile.ProfileProperties("Website").Visibility

					Select Case WebSiteVisibility
						Case UserVisibilityMode.AdminOnly
							If objSecurity.IsForumAdmin Then
								RenderWebsite()
							End If
						Case UserVisibilityMode.AllUsers
							RenderWebsite()
						Case UserVisibilityMode.MembersOnly
							If LoggedOnUser.UserID > 0 Then
								RenderWebsite()
							End If
					End Select

					If ProfileUser.EnablePublicEmail AndAlso (Len(ProfileUser.Email) > 0) Then
						Dim strEmailText As String = HtmlUtils.FormatEmail(ProfileUser.Email)
						strEmailText = "<span class=""Forum_Profile"">" & strEmailText & "</span>"
						litEmail.Text = strEmailText
					Else
						litEmail.Text = Localization.GetString("NotAvailable.Text", Me.LocalResourceFile)
					End If

					If ProfileUser.Profile.IM <> String.Empty Then
						txtIM.Text = ProfileUser.Profile.IM
					Else
						txtIM.Text = Localization.GetString("NotAvailable.Text", Me.LocalResourceFile)
					End If

					If ProfileUser.Biography <> String.Empty Then
						litBiography.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(ProfileUser.Biography))
					Else
						litBiography.Text = "<p>" & Localization.GetString("NotAvailable.Text", Me.LocalResourceFile) & "</p>"
					End If

					' Avatar (moderator control)
					ctlUserAvatar.Security = objSecurity
					ctlUserAvatar.AvatarType = AvatarControlType.User
					ctlUserAvatar.Images = ProfileUser.Avatar

					If ProfileUser.UserAvatar = UserAvatarType.PoolAvatar Then
						ctlUserAvatar.IsPoolAvatar = True
					End If
					ctlUserAvatar.LoadInitialView()

					'[skeel] Handle layout
					imgSpc1.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
					imgSpc2.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
					imgSpc3.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
					imgSpc4.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
					lblProfile.Text = "&nbsp;" & String.Format(Localization.GetString("LookingAtProfile", Me.LocalResourceFile), ProfileUser.SiteAlias)

					If objConfig.EnableUserBanning Then
						rowUserBanning.Visible = True
					Else
						rowUserBanning.Visible = False
					End If

					If objConfig.EnableUserSignatures Then
						rowUserSignature.Visible = True

						If objConfig.EnableModSigUpdates = True Then
							rowEditUserSig.Visible = True
						Else
							rowEditUserSig.Visible = False
						End If

						If ProfileUser.Signature.Length > 0 Then
							If objConfig.EnableHTMLSignatures Then
								lblSignature.Text = Server.HtmlDecode(ProfileUser.Signature)
							Else
								lblSignature.Text = ProfileUser.Signature
							End If

							txtSignature.Text = ProfileUser.Signature
						Else
							lblSignature.Text = "&nbsp;"
						End If
					Else
						rowEditUserSig.Visible = False
						rowUserSignature.Visible = False
					End If

					If objConfig.EnableUserAvatar Then
						If (ProfileUser.AvatarComplete.Trim() <> String.Empty) Then
							If objConfig.EnableProfileAvatar = False Then
								Dim WebVisibility As UserVisibilityMode
								WebVisibility = ProfileUser.Profile.ProfileProperties(objConfig.AvatarProfilePropName).Visibility

								Select Case WebVisibility
									Case UserVisibilityMode.AdminOnly
										If objSecurity.IsForumAdmin Then
											RenderProfileAvatar(ProfileUser)
										Else
											rowUserAvatar.Visible = False
										End If
									Case UserVisibilityMode.AllUsers
										RenderProfileAvatar(ProfileUser)
									Case UserVisibilityMode.MembersOnly
										If LoggedOnUser.UserID > 0 Then
											RenderProfileAvatar(ProfileUser)
										Else
											rowUserAvatar.Visible = False
										End If
								End Select
							Else
								' Avatars are enabled, it has a value and it is not a profile avatar, use old render stuff
								rowUserAvatar.Visible = True
								imgAvatar.ImageUrl = ProfileUser.AvatarComplete
							End If
						Else
							rowUserAvatar.Visible = False
						End If
					Else
						rowUserAvatar.Visible = False
					End If

					If objConfig.EnableSystemAvatar Then
						rowSystemAvatar.Visible = False
					Else
						rowSystemAvatar.Visible = False
					End If

					If objConfig.Ranking Then
						rowRanking.Visible = True
						Dim authorRank As PosterRank = Utilities.ForumUtils.GetRank(ProfileUser, objConfig)
						Dim rankImage As String = String.Format("Rank_{0}." & objConfig.ImageExtension, CType(authorRank, Integer).ToString)
						Dim rankURL As String = objConfig.GetThemeImageURL(rankImage)
						Dim RankTitle As String = Utilities.ForumUtils.GetRankTitle(authorRank, objConfig)

						If objConfig.EnableRankingImage Then
							imgRanking.Visible = True
							imgRanking.ImageUrl = rankURL
							imgRanking.AlternateText = RankTitle
							lblRankTitle.Text = RankTitle
							lblRankTitle.Visible = False
						Else
							imgRanking.Visible = False
							lblRankTitle.Text = RankTitle
							lblRankTitle.Visible = True
						End If
					Else
						rowRanking.Visible = False
					End If

					' Moderator setting
					chkIsTrusted.Checked = ProfileUser.IsTrusted
					' if the user's trust level is locked, only allow mod/site admin to update
					If ProfileUser.LockTrust And (Not objSecurity.IsForumAdmin) Then
						chkIsTrusted.Enabled = False
					End If

					' banned info
					chkUserBanned.Checked = ProfileUser.IsBanned
					If ProfileUser.IsBanned Then
						lblLiftBan.Visible = True
						Dim LiftBanDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(CType(ProfileUser.LiftBanDate.ToString, DateTime), objConfig)
						lblLiftBan.Text = LiftBanDate.ToString
					Else
						lblLiftBan.Visible = False
					End If

					Dim txtStats As New Text.StringBuilder
					txtStats.AppendFormat("<b>" & ProfileUser.SiteAlias & "</b>")
					txtStats.AppendFormat(" " & Localization.GetString("Contributed.Text", Me.LocalResourceFile), ProfileUser.PostCount.ToString)

					If ProfileUser.PostCount > 0 Then
						txtStats.AppendFormat("<br />")
						txtStats.AppendFormat(Localization.GetString("MostRecent.Text", Me.LocalResourceFile), ProfileUser.LastActivity.ToLongDateString)

						If Request.IsAuthenticated Then
							' if the user is logged in, use the threads/page option
							lnkUserPosts.NavigateUrl = NavigateURL(TabId, "", New String() {"pagesize=" & LoggedOnUser.ThreadsPerPage, "authors=" & ProfileUser.UserID, "scope=threadsearch"})
						Else
							' user is not logged in, use forum default for threads/page
							lnkUserPosts.NavigateUrl = NavigateURL(TabId, "", New String() {"pagesize=" & objConfig.ThreadsPerPage, "authors=" & ProfileUser.UserID, "scope=threadsearch"})
						End If

						lblStatistic.Text = txtStats.ToString
					Else
						If ProfileUser.IsDeleted Then
							rowPostLink.Visible = False
							rowStats.Visible = False
						End If
					End If

					' User Joined Date (localized, including date)
					Dim displayCreatedDate As DateTime = Utilities.ForumUtils.ConvertTimeZone(CType(ProfileUser.Membership.CreatedDate, DateTime), objConfig)
					lblJoinedDate.Text = Localization.GetString("JoinedDate.Text", Me.LocalResourceFile) & " " & displayCreatedDate.ToShortDateString

					If LoggedOnUser.UserID > 0 Then
						If objConfig.EnablePMSystem Then
							'If the user receives private messages and the logged in user does as well
							rowPMUser.Visible = True
							If ProfileUser.EnablePM And LoggedOnUser.EnablePM Then
								' No need to send a private message to yourself
								If Not (ProfileUser.UserID = LoggedOnUser.UserID) Then
									cmdPMUser.Enabled = True
								Else
									cmdPMUser.Enabled = False
								End If
							Else
								cmdPMUser.Enabled = False
							End If
						Else
							rowPMUser.Visible = False
						End If
					Else
						cmdUpdate.Visible = False
						cmdManageUser.Visible = False
						rowModeratorAdmin.Visible = False
						rowPMUser.Visible = False
					End If

					If Not Request.UrlReferrer Is Nothing Then
						ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
					End If

					' Register scripts
					Utilities.ForumUtils.RegisterPageScripts(Page, objConfig)
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ProfileUser"></param>
		''' <remarks></remarks>
		Private Sub RenderProfileAvatar(ByVal ProfileUser As ForumUser)
			imgAvatar.Width = objConfig.UserAvatarWidth
			imgAvatar.Height = objConfig.UserAvatarHeight
			imgAvatar.ImageUrl = ProfileUser.AvatarComplete
			rowUserAvatar.Visible = True
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Private Sub RenderWebsite()
			If (Not ProfileUser.Profile.Website Is Nothing) Then
				lnkWWW.Text = ProfileUser.Profile.Website
				lnkWWW.NavigateUrl = AddHTTP(ProfileUser.Profile.Website)
				If objConfig.NoFollowWeb Then
					lnkWWW.Attributes.Add("rel", "nofollow")
				End If
			Else
				rowUserWeb.Visible = False
			End If
		End Sub

		''' <summary>
		''' Does an update to this user - only moderators can change is trusted here
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' 	[hmnguyen]	    30/10/2005	Localization
		''' </history>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				With ProfileUser
					ForumUserController.GetForumUser(ProfileUser.UserID, False, ModuleId, PortalId)
					.IsTrusted = chkIsTrusted.Checked
					.Signature = String.Empty

					' only allow update here if forums have signatures enabled and moderators are permitted to update them here.
					If objConfig.EnableModSigUpdates And objConfig.EnableUserSignatures Then
						Dim objSecurity As New PortalSecurity
						Dim Signature As String = txtSignature.Text

						' run against security filter
						.Signature = objSecurity.InputFilter(Signature, PortalSecurity.FilterFlag.NoScripting)

						' check to see if HTML sigs are supported
						If Not objConfig.EnableHTMLSignatures Then
							.Signature = Utilities.ForumUtils.StripHTML(Signature)
						End If
					End If
					Dim cntUser As New ForumUserController
					cntUser.Update(ProfileUser)
				End With

			Catch Exc As System.Exception
				LogException(Exc)
				Return
			End Try

			Response.Redirect(NavigateURL(), False)
		End Sub

		''' <summary>
		''' Return the user to where they came from
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			Try
				If Not ViewState("UrlReferrer") Is Nothing Then
					Response.Redirect(ViewState("UrlReferrer").ToString, False)
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Will navigate a user to the User Settings area of this profile user
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	9/10/2006	Created
		''' </history>
		Protected Sub cmdManageUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdManageUser.Click
			Response.Redirect(Utilities.Links.UCP_AdminLinks(TabId, ModuleId, ProfileUser.UserID, UserAjaxControl.Profile), False)
		End Sub

		''' <summary>
		''' Will navigate the user to start a new private message area to send
		''' a new private message thread to the owner of this user profile
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/15/2006	Created
		''' </history>
		Protected Sub cmdPMUser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPMUser.Click
			Response.Redirect(Utilities.Links.PMUserLink(TabId, ModuleId, ProfileUser.UserID), False)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Enable/Disable profile controls
		''' </summary>
		''' <param name="Value"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Private Sub EnableControls(ByVal Value As Boolean)
			txtAlias.Enabled = Value
			txtIM.Enabled = Value
		End Sub

#End Region

	End Class

End Namespace