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
	''' This is the users "Edit Avatar" page.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[skeel]	11/28/2008	Created
	''' </history>
	Partial Public Class Avatar
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Dim objSecurity As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)
			Dim cntForumUser As New ForumUserController

			Dim ProfileUser As ForumUser = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

			If objSecurity.IsForumAdmin = True AndAlso objConfig.EnableSystemAvatar Then
				rowSystemAvatar.Visible = True
				ctlSystemAvatar.Images = ProfileUser.SystemAvatars
				ctlSystemAvatar.LoadInitialView()
			Else
				rowSystemAvatar.Visible = False
			End If

			'[skeel] we remove the update function for users as the 
			' avatar control handles the updates for them
			If objSecurity.IsForumAdmin = True Then
				cmdUpdate.Visible = True
			Else

				cmdUpdate.Visible = False
			End If

			' Hide the avatar if we are using profile avatars. 
			If objConfig.EnableProfileAvatar Then
				rowUserAvatar.Visible = False
			End If

			ctlUserAvatar.Images = ProfileUser.Avatar
			If ProfileUser.UserAvatar = UserAvatarType.PoolAvatar Then
				ctlUserAvatar.IsPoolAvatar = True
			End If
			ctlUserAvatar.LoadInitialView()
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' There are several items we want to make sure are handled on every page load of this control. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			Dim objSecurity As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)
			Dim cntForumUser As New ForumUserController

			Dim ProfileUser As ForumUser = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

			If objSecurity.IsForumAdmin = True AndAlso objConfig.EnableSystemAvatar Then
				ctlSystemAvatar.Security = objSecurity
				ctlSystemAvatar.AvatarType = AvatarControlType.System
			End If

			ctlUserAvatar.Security = objSecurity
			ctlUserAvatar.AvatarType = AvatarControlType.User
		End Sub

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
				ForumUserController.ResetForumUser(ProfileUserID, PortalId)
				Dim cntForumUser As New ForumUserController

				Dim ProfileUser As ForumUser = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

				With ProfileUser

					'Was the avatar removed?
					If ctlUserAvatar.Images.Replace(";", "") = String.Empty Then
						.UserAvatar = UserAvatarType.None
						.Avatar = String.Empty
					Else
						'Was it a useravatar or a poolavatar?
						If ctlUserAvatar.IsPoolAvatar = True Then
							.UserAvatar = UserAvatarType.PoolAvatar
						Else
							.UserAvatar = UserAvatarType.UserAvatar
						End If
					End If
					.Avatar = ctlUserAvatar.Images
					.SystemAvatars = ctlSystemAvatar.Images

				End With

				Dim cntUser As New ForumUserController
				cntUser.Update(ProfileUser)

				ForumUserController.ResetForumUser(ProfileUser.UserID, PortalId)
				lblUpdateDone.Visible = True
			Catch Exc As System.Exception
				LogException(Exc)
				Return
			End Try
		End Sub

#End Region

	End Class

End Namespace