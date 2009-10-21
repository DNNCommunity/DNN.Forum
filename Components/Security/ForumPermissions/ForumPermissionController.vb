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

#Region "ForumPermissionController"

	''' <summary>
	''' Basically a copy of ModulePermissionController. This allows the forum
	''' module ot implement permissions on a per module basis overriding module
	''' permissions. (Only in the sense of further restriction, not less)
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	8/6/2006	Created
	''' </history>
	Public Class ForumPermissionController

#Region "Public Shared Methods"

		''' <summary>
		''' Loops through a collection of forum permissions to find a key value for comparison.
		''' </summary>
		''' <param name="objForumPermissions">A collection of forum permissions.</param>
		''' <param name="PermissionKey">The key to look for.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function HasForumPermission(ByVal objForumPermissions As ForumPermissionCollection, ByVal PermissionKey As String, ByVal UserID As Integer, ByVal PortalID As Integer) As Boolean
			If Not objForumPermissions Is Nothing Then
				For Each objForumPermission As ForumPermissionInfo In objForumPermissions
					If objForumPermission.PermissionKey = PermissionKey Then
						If Null.IsNull(objForumPermission.UserID) Then
							If MockSecurity.IsInMockRoles(objForumPermission.RoleName, UserID, PortalID) Then	' PortalSecurity.IsInRoles
								Return True
							End If
						Else
							If MockSecurity.IsInMockRoles("[" & objForumPermission.UserID.ToString & "]", UserID, PortalID) Then
								Return True
							End If

						End If
					End If
				Next
			End If
			Return False
		End Function

#End Region

#Region "Private Methods"

		Private Function FillForumPermissionCollection(ByVal dr As IDataReader) As ForumPermissionCollection
			Dim arr As New ForumPermissionCollection()
			Try
				Dim obj As ForumPermissionInfo
				While dr.Read
					' fill business object
					obj = FillForumPermissionInfo(dr, False)
					' add to collection
					arr.Add(obj)
				End While
			Catch exc As Exception
				LogException(exc)
			Finally
				' close datareader
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try
			Return arr
		End Function

		Private Function FillForumPermissionDictionary(ByVal dr As IDataReader) As Dictionary(Of Integer, ForumPermissionCollection)
			Dim dic As New Dictionary(Of Integer, ForumPermissionCollection)
			Try
				Dim obj As ForumPermissionInfo
				While dr.Read
					' fill business object
					obj = FillForumPermissionInfo(dr, False)

					' add Module Permission to dictionary
					If dic.ContainsKey(obj.ModuleID) Then
						dic(obj.ModuleID).Add(obj)
					Else
						Dim collection As New ForumPermissionCollection

						'Add Permission to Collection
						collection.Add(obj)

						'Add Collection to Dictionary
						dic.Add(obj.ModuleID, collection)
					End If
				End While
			Catch exc As Exception
				LogException(exc)
			Finally
				' close datareader
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try
			Return dic
		End Function

		Private Function FillForumPermissionInfo(ByVal dr As IDataReader, ByVal CheckForOpenDataReader As Boolean) As ForumPermissionInfo
			Dim permissionInfo As ForumPermissionInfo

			' read datareader
			Dim canContinue As Boolean = True
			If CheckForOpenDataReader Then
				canContinue = False
				If dr.Read Then
					canContinue = True
				End If
			End If

			If canContinue Then
				permissionInfo = New ForumPermissionInfo()
				permissionInfo.ForumPermissionID = Convert.ToInt32(Null.SetNull(dr("ForumPermissionID"), permissionInfo.ForumPermissionID))
				permissionInfo.ForumID = Convert.ToInt32(Null.SetNull(dr("ForumID"), permissionInfo.ForumID))
				permissionInfo.PermissionID = Convert.ToInt32(Null.SetNull(dr("PermissionID"), permissionInfo.PermissionID))
				permissionInfo.UserID = Convert.ToInt32(Null.SetNull(dr("UserID"), permissionInfo.UserID))
				permissionInfo.Username = Convert.ToString(Null.SetNull(dr("Username"), permissionInfo.Username))
				permissionInfo.DisplayName = Convert.ToString(Null.SetNull(dr("DisplayName"), permissionInfo.DisplayName))
				permissionInfo.RoleID = Convert.ToInt32(Null.SetNull(dr("RoleID"), permissionInfo.RoleID))
				permissionInfo.RoleName = Convert.ToString(Null.SetNull(dr("RoleName"), permissionInfo.RoleName))
				permissionInfo.AllowAccess = Convert.ToBoolean(Null.SetNull(dr("AllowAccess"), permissionInfo.AllowAccess))
				permissionInfo.PermissionCode = Convert.ToString(Null.SetNull(dr("PermissionCode"), permissionInfo.PermissionCode))
				permissionInfo.PermissionKey = Convert.ToString(Null.SetNull(dr("PermissionKey"), permissionInfo.PermissionKey))
				permissionInfo.PermissionName = Convert.ToString(Null.SetNull(dr("PermissionName"), permissionInfo.PermissionName))
			Else
				permissionInfo = Nothing
			End If

			Return permissionInfo
		End Function

		Private Function GetForumPermissionsDictionary(ByVal ForumID As Integer) As Dictionary(Of Integer, ForumPermissionCollection)
			Dim dicForumPermissions As Dictionary(Of Integer, ForumPermissionCollection)
			Dim dr As IDataReader = DataProvider.Instance().GetForumPermissions(ForumID)
			dicForumPermissions = FillForumPermissionDictionary(dr)

			'Return the Dictionary
			Return dicForumPermissions
		End Function

#End Region

#Region "Public Methods"

		''' <summary>
		''' Adds a forum permission item to the Forum_ForumPermission table in the data store.
		''' </summary>
		''' <param name="objForumPermission"></param>
		''' <returns>The integer PK of the item just added to the data store.</returns>
		''' <remarks></remarks>
		Public Function AddForumPermission(ByVal objForumPermission As ForumPermissionInfo) As Integer
			Dim Id As Integer = CType(DataProvider.Instance().AddForumPermission(objForumPermission.ForumID, objForumPermission.PermissionID, objForumPermission.RoleID, objForumPermission.AllowAccess, objForumPermission.UserID), Integer)
			Return Id
		End Function

		''' <summary>
		''' Deletes a forum permissions item permanently from the Forum_ForumPermission table in the data store.
		''' </summary>
		''' <param name="ForumPermissionID">The PK to delete.</param>
		''' <remarks></remarks>
		Public Sub DeleteForumPermission(ByVal ForumPermissionID As Integer)
			DataProvider.Instance().DeleteForumPermission(ForumPermissionID)
		End Sub

		''' <summary>
		''' Deletes all permissions in the Forum_ForumPermission table in the data store that have the provided ForumID.
		''' </summary>
		''' <param name="ForumID">The ForumID to delete permissions for in the data store.</param>
		''' <remarks></remarks>
		Public Sub DeleteForumPermissionsByForumID(ByVal ForumID As Integer)
			DataProvider.Instance().DeleteForumPermissionsByForumID(ForumID)
		End Sub

		''' <summary>
		''' Gets a single row of data from the Forum_ForumPermission table based on a permissionID.
		''' </summary>
		''' <param name="ForumPermissionID">The permission</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetForumPermission(ByVal ForumPermissionID As Integer) As ForumPermissionInfo
			Dim permission As ForumPermissionInfo

			'Get permission from Database
			Dim dr As IDataReader = DataProvider.Instance().GetForumPermission(ForumPermissionID)
			Try
				permission = FillForumPermissionInfo(dr, True)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return permission
		End Function

		''' <summary>
		''' Loops through a collection of forum permissions.
		''' </summary>
		''' <param name="forumPermissions"></param>
		''' <param name="PermissionKey"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetForumPermissions(ByVal forumPermissions As ForumPermissionCollection, ByVal PermissionKey As String) As String
			Dim strRoles As String = String.Empty
			Dim strUsers As String = String.Empty
			For Each objForumPermission As ForumPermissionInfo In forumPermissions
				If objForumPermission.AllowAccess = True AndAlso objForumPermission.PermissionKey = PermissionKey Then
					If Null.IsNull(objForumPermission.UserID) Then
						strRoles += objForumPermission.RoleName + ";"
					Else
						strUsers += "[" + objForumPermission.UserID.ToString + "];"
					End If
				End If
			Next
			Return ";" & strRoles & strUsers
		End Function

		''' <summary>
		''' Retrieves a collection of forum permission from the data store.
		''' </summary>
		''' <param name="ForumID">The ForumID to retrieve a collection of forum permissions for.</param>
		''' <returns></returns>
		''' <remarks>Used to build the forum permissions grid.</remarks>
		Public Function GetPermissionsByForumID(ByVal ForumID As Integer) As ArrayList
			Return CBO.FillCollection(DataProvider.Instance().GetPermissionsByForumID(ForumID), GetType(PermissionInfo))
		End Function

		''' <summary>
		''' Retrieves a collection of forum permssions from the Forum_ForumPermission table in the data store.
		''' </summary>
		''' <param name="ForumID">The ForumID to retrieve a collection of forum permissions for.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetForumPermissionsCollection(ByVal ForumID As Integer) As ForumPermissionCollection
			Dim bFound As Boolean = False

			'Get the Tab ModulePermission Dictionary
			Dim dicModulePermissions As Dictionary(Of Integer, ForumPermissionCollection) = GetForumPermissionsDictionary(ForumID)

			'Get the Collection from the Dictionary
			Dim forumPermissions As ForumPermissionCollection = Nothing
			bFound = dicModulePermissions.TryGetValue(ForumID, forumPermissions)

			If Not bFound Then
				'try the database
				forumPermissions = FillForumPermissionCollection(DataProvider.Instance().GetForumPermissionsCollection(ForumID, -1))
			End If

			Return forumPermissions
		End Function

		''' <summary>
		''' Updates a forum permission item in the Forum_ForumPermission table in the data store.
		''' </summary>
		''' <param name="objForumPermission">The ForumPermissionInfo object to update in the data store.</param>
		''' <remarks></remarks>
		Public Sub UpdateForumPermission(ByVal objForumPermission As ForumPermissionInfo)
			DataProvider.Instance().UpdateForumPermission(objForumPermission.ForumPermissionID, objForumPermission.ForumID, objForumPermission.PermissionID, objForumPermission.RoleID, objForumPermission.AllowAccess, objForumPermission.UserID)
		End Sub

#End Region

	End Class

#End Region

End Namespace
