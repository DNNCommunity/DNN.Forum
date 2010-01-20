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

Imports DotNetNuke.Security.Permissions

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' This class is utilized by the ModuleSecurity class to obtain permissions (because they are dependent on HttpContext and the current logged in user and provide no flexibility for scheduled tasks).
	''' </summary>
	''' <remarks>Because context is removed, we also have the ability to determine permissions of users beyond the one currently logged in. This permits us, as an example, to avoid a moderator removing permissions from an admin. BE CAREFULL!</remarks>
	Friend Class MockSecurity

		''' <summary>
		''' This is a replacement for the core HasModulePermissions method, because the core method requires the currently logged in user.
		''' </summary>
		''' <param name="objModulePermissions">A collection of module permissions.</param>
		''' <param name="PermissionKey">The permission key we want to validate against.</param>
		''' <param name="UserID">The user we want to check the permissions of.</param>
		''' <param name="PortalID">The portal the user is in.</param>
		''' <returns>True if the user has permission.</returns>
		''' <remarks>The currently logged in user is not useful to us in specific use cases, such as scheduled tasks.</remarks>
		Friend Shared Function HasMockModulePermission(ByVal objModulePermissions As ModulePermissionCollection, ByVal PermissionKey As String, ByVal UserID As Integer, ByVal PortalID As Integer, ByVal ModuleID As Integer) As Boolean
			If Not objModulePermissions Is Nothing Then
				For Each objModulePermission As ModulePermissionInfo In objModulePermissions
					If objModulePermission.PermissionKey = PermissionKey Then


						' test (add or for global mod)
						If objModulePermission.PermissionKey = (DotNetNuke.Modules.Forum.PermissionKeys.FORUMADMIN.ToString) Then
							' if we are looking for admin, lets just take care of hosts here (since they aren't in a role)
							Dim cntUsers As New ForumUserController

							Dim objUser As ForumUser = cntUsers.UserGet(PortalID, UserID, ModuleID)
							If objUser.IsSuperUser Then
								Return True
							Else
								If Null.IsNull(objModulePermission.UserID) Then
									If IsInMockRoles(objModulePermission.RoleName, UserID, PortalID, ModuleID) Then
										Return True
									End If
								Else
									If IsInMockRoles("[" & objModulePermission.UserID.ToString & "]", UserID, PortalID, ModuleID) Then
										Return True
									End If
								End If
							End If
						End If
					End If
				Next
			End If
			Return False
		End Function

		''' <summary>
		''' This is a replacement for the core IsInRoles method, because the core method requires the currently logged in user. 
		''' </summary>
		''' <param name="roles">The roles we want to check against.</param>
		''' <param name="UserID">The user we want to check the permissions of.</param>
		''' <param name="PortalID">The portal the user is in.</param>
		''' <returns>True if the user has permission.</returns>
		''' <remarks>The currently logged in user is not useful to us in specific use cases, such as scheduled tasks.</remarks>
		Friend Shared Function IsInMockRoles(ByVal roles As String, ByVal UserID As Integer, ByVal PortalID As Integer, ByVal ModuleID As Integer) As Boolean
			If Not roles Is Nothing Then
				Dim IsSuperUser As Boolean = False
				Dim IsInRole As Boolean = False
				Dim cntUsers As New ForumUserController

				Dim objUser As ForumUser = cntUsers.UserGet(PortalID, UserID, ModuleID)
				Dim role As String

				For Each role In roles.Split(New Char() {";"c})
					If Not objUser Is Nothing Then
						IsSuperUser = objUser.IsSuperUser
						IsInRole = objUser.IsInRole(role)
					End If

					If IsSuperUser Or (role <> String.Empty AndAlso Not role Is Nothing AndAlso ((UserID = -1 And role = glbRoleUnauthUserName) Or role = glbRoleAllUsersName Or IsInRole = True)) Then
						Return True
					End If
				Next role
			End If

			Return False
		End Function

	End Class

End Namespace