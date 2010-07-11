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

#Region "ModuleSecurity"

	''' <summary>
	''' The central class where all non-core security related checks should be done.
	''' </summary>
	''' <remarks>This base allows one level lower than core permissions (ie. forum level).
	''' </remarks>
	Public Class ModuleSecurity

#Region "Private Members"

		'keep all changes to be made later in this class 

		'PER MODULE
		Private HasForumAdminPermission As Boolean
		Private HasGlobalModPermission As Boolean
		Private HasEditModPermission As Boolean
		Private HasModeratePermisson As Boolean = False

		' PER FORUM
		Private HasPrivateViewPerms As Boolean
		Private HasRestrictedStartThreadPerms As Boolean
		Private HasRestrictedPostReplyPerms As Boolean
		Private HasAddAttachmentPerms As Boolean
		Private HasPinThreadPerms As Boolean
		Private HasLockThreadPerms As Boolean
		Private HasForumModeratePerms As Boolean
		Private HasUnmoderatedPerms As Boolean

		' PER FORUM
		Private Const _HasPrivateViewPerms As String = "VIEW"
		Private Const _HasRestrictedStartThreadPerms As String = "STARTTHREAD"
		Private Const _HasRestrictedPostReplyPerms As String = "POSTREPLY"
		Private Const _HasAddAttachmentPerms As String = "ADDATTACHMENT"
		Private Const _HasPinThreadPerms As String = "PINTHREAD"
		Private Const _HasLockThreadPerms As String = "LOCKTHREAD"
		Private Const _HasForumModeratePerms As String = "MODERATE"
		Private Const _HasUnmoderatedPermission As String = "UNMODERATED"

#End Region

#Region "Constructors"

		''' <summary>
		''' Instantiates the class to create the security object. 
		''' </summary>
		''' <param name="moduleId"></param>
		''' <param name="tabId"></param>
		''' <param name="ForumId"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal moduleId As Integer, ByVal tabId As Integer, ByVal ForumId As Integer, ByVal UserID As Integer)
			Dim mc As New DotNetNuke.Entities.Modules.ModuleController
			Dim objMod As New DotNetNuke.Entities.Modules.ModuleInfo
			Dim mp As New DotNetNuke.Security.Permissions.ModulePermissionCollection

			objMod = mc.GetModule(moduleId, tabId, False)

			If objMod IsNot Nothing Then
				mp = objMod.ModulePermissions

				' Next 3 items only apply for currently logged in user accessing the class (CP - Modified 7/8)
				HasForumAdminPermission = MockSecurity.HasMockModulePermission(mp, DotNetNuke.Modules.Forum.PermissionKeys.FORUMADMIN.ToString, UserID, objMod.PortalID, moduleId)

				If Not HasForumAdminPermission Then
					HasGlobalModPermission = MockSecurity.HasMockModulePermission(mp, DotNetNuke.Modules.Forum.PermissionKeys.FORUMGLBMOD.ToString, UserID, objMod.PortalID, moduleId)
				End If
				'HasEditModPermission = PortalSecurity.HasEditPermissions(moduleId, tabId)

				If ForumId > -1 Then
					' For forum specific perms, we need to hook into the forum permission controller
					Dim cntForum As New ForumController

					Dim fp As DotNetNuke.Modules.Forum.ForumPermissionCollection = cntForum.GetForumInfoCache(ForumId).ForumPermissions

					If fp IsNot Nothing Then
						HasPrivateViewPerms = ForumPermissionController.HasForumPermission(fp, _HasPrivateViewPerms, UserID, objMod.PortalID, moduleId)
						HasRestrictedStartThreadPerms = ForumPermissionController.HasForumPermission(fp, _HasRestrictedStartThreadPerms, UserID, objMod.PortalID, moduleId)
						HasRestrictedPostReplyPerms = ForumPermissionController.HasForumPermission(fp, _HasRestrictedPostReplyPerms, UserID, objMod.PortalID, moduleId)
						HasForumModeratePerms = ForumPermissionController.HasForumPermission(fp, _HasForumModeratePerms, UserID, objMod.PortalID, moduleId)
						HasAddAttachmentPerms = ForumPermissionController.HasForumPermission(fp, _HasAddAttachmentPerms, UserID, objMod.PortalID, moduleId)
						HasPinThreadPerms = ForumPermissionController.HasForumPermission(fp, _HasPinThreadPerms, UserID, objMod.PortalID, moduleId)
						HasLockThreadPerms = ForumPermissionController.HasForumPermission(fp, _HasLockThreadPerms, UserID, objMod.PortalID, moduleId)
						HasUnmoderatedPerms = ForumPermissionController.HasForumPermission(fp, _HasUnmoderatedPermission, UserID, objMod.PortalID, moduleId)

						If HasForumAdminPermission Then
							' make sure the user has view perms (we only have to worry about this on the specific forum level?)
							' we could potentially tie attach/pin/lock to mods in general here if desired
							If HasPrivateViewPerms And (cntForum.GetForumInfoCache(ForumId).PublicView = False) Then
								HasForumModeratePerms = True
								HasUnmoderatedPerms = True
								'Else
								'	' the user is not an admin or they don't have view perms. We should make sure that (as backup) this isn't a p
							End If
						Else
							If HasGlobalModPermission Then
								' This needs to be here so we ignore any user trust settings (this trumps those)
								If HasPrivateViewPerms And (cntForum.GetForumInfoCache(ForumId).PublicView = False) Then
									HasForumModeratePerms = True
									HasUnmoderatedPerms = True
									'Else
									'	' the user is not an admin or they don't have view perms. We should make sure that (as backup) this isn't a p
								End If
							Else
								' we know the user is not forum admin or global mod
							End If
						End If
					End If
				Else
					If HasForumAdminPermission Then
						HasModeratePermisson = True
						HasUnmoderatedPerms = True
					Else
						If HasGlobalModPermission Then
							HasModeratePermisson = True
							HasUnmoderatedPerms = True
						Else
							' here is where we check per forum
							Dim objForumCnt As New ForumController
							Dim arrAllForums As List(Of ForumInfo)
							arrAllForums = objForumCnt.GetModuleForums(moduleId)

							If arrAllForums IsNot Nothing Then
								For Each objForum As ForumInfo In arrAllForums
									'CP - Use to see if user is a moderator in any forum. 
									If objForum IsNot Nothing Then
										Dim Security As New Forum.ModuleSecurity(moduleId, tabId, objForum.ForumID, UserID)
										If Security.IsForumModerator Then
											HasModeratePermisson = True
											HasUnmoderatedPerms = True
											Exit For
										End If
									End If
								Next
							End If
						End If
					End If
				End If
			Else
				' The tabmodule doesn't exist.
				Exit Sub
			End If
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' If the user is allowed to post a new thread.
		''' </summary>
		''' <returns>True if the user is permitted to start a thread, false otherwise.</returns>
		''' <remarks>Restricted forums only.</remarks>
		Public Function IsAllowedToStartRestrictedThread() As Boolean
			If HasRestrictedStartThreadPerms Or HasForumModeratePerms Or HasForumModeratePerms Or HasGlobalModPermission Or HasForumAdminPermission Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is allowed to post a reply to an existing thread.
		''' </summary>
		''' <returns>True if the user is permitted to reply to an existing thread, false otherwise.</returns>
		''' <remarks>Restricted forums only.</remarks>
		Public Function IsAllowedToPostRestrictedReply() As Boolean
			If HasGlobalModPermission Or HasForumAdminPermission Or HasEditModPermission Or HasForumModeratePerms Or HasRestrictedPostReplyPerms Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is an administrator for the forum.
		''' </summary>
		''' <returns>True if the user is a forum administrator, false otherwise.</returns>
		''' <remarks></remarks>
		Public Function IsForumAdmin() As Boolean
			If HasForumAdminPermission Or HasEditModPermission Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is a global moderator of all forums.
		''' </summary>
		''' <returns>True if the user is a global moderator, false otherwise.</returns>
		''' <remarks></remarks>
		Public Function IsGlobalModerator() As Boolean
			If HasForumAdminPermission Or HasEditModPermission Or HasGlobalModPermission Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is permitted to view the forum.
		''' </summary>
		''' <returns>True if the user is permitted to view the forum, false otherwise.</returns>
		''' <remarks>Private forums only.</remarks>
		Public Function IsAllowedToViewPrivateForum() As Boolean
			If HasPrivateViewPerms Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user can add an attachment to a post.
		''' </summary>
		''' <returns>True if the user is permitted to add attachments, false otherwise.</returns>
		''' <remarks></remarks>
		Public Function CanAddAttachments() As Boolean
			If HasForumAdminPermission Or HasGlobalModPermission Or HasEditModPermission Or HasForumModeratePerms Or HasAddAttachmentPerms Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is permitted to pin a thread.
		''' </summary>
		''' <returns>True if the user is permitted to pin a thread, false otherwise.</returns>
		''' <remarks></remarks>
		Public Function CanPinThread() As Boolean
			If HasForumAdminPermission Or HasGlobalModPermission Or HasEditModPermission Or HasForumModeratePerms Or HasPinThreadPerms Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is permitted to lock a thread from further replies.
		''' </summary>
		''' <returns>True if the user is permitted to lock a thread, false otherwise.</returns>
		''' <remarks></remarks>
		Public Function CanLockThread() As Boolean
			If HasForumAdminPermission Or HasGlobalModPermission Or HasEditModPermission Or HasForumModeratePerms Or HasLockThreadPerms Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is a moderator of at least one forum in the module instance.
		''' </summary>
		''' <returns>True if the user is a moderator in the forum instance, false otherwise.</returns>
		''' <remarks>This should be used when determining if the user is a moderator anywhere in this module instance (ie. not a specific forum). This has a flaw as it only works if -1 is passed in.</remarks>
		Public Function IsModerator() As Boolean
			If HasGlobalModPermission Or HasForumAdminPermission Or HasEditModPermission Or HasModeratePermisson Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is a moderator of the specified forum.
		''' </summary>
		''' <returns>True if the user is a moderator of the specified forum, false otherwise.</returns>
		''' <remarks></remarks>
		Public Function IsForumModerator() As Boolean
			If HasGlobalModPermission Or HasForumAdminPermission Or HasEditModPermission Or HasForumModeratePerms Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' If the user is unmoderated based on forum permission 
		''' </summary>
		''' <returns>True if the user is unmoderated, flase otherwise.</returns>
		''' <remarks></remarks>
		Public Function IsUnmoderated() As Boolean
			If HasGlobalModPermission Or HasForumModeratePerms Or HasForumAdminPermission Or HasEditModPermission Or HasUnmoderatedPerms Then
				Return True
			Else
				Return False
			End If
		End Function

#End Region

	End Class

#End Region

End Namespace