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
	''' CRUD (and all db methods) RoleAvatar Methods
	''' </summary>
	''' <remarks>Added by Skeel</remarks>
	Public Class RoleAvatarController

#Region " Private Members "

		Private Const RoleAvatarInfoCacheKeyPrefix As String = "RoleAvatarInfo"
		Private Const RoleCacheKeyPrefix As String = "RoleAvatarRoleID"
		Private Const ForumInfoCacheTimeout As Integer = 120 ' RoleAvatars shouldn't be changed that often...

#End Region

#Region " Constructors "

		''' <summary>
		''' Instantiats the RoleAvatar Contoller. 
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region

#Region " Private Methods "

		''' <summary>
		''' Get Forum roles and defined avatars
		''' </summary>
		''' <param name="PortalID"></param>
		''' <returns>RoleAvatarInfo</returns>
		Private Function GetAll(ByVal PortalID As Integer) As List(Of RoleAvatarInfo)
			Dim objAvatars As New List(Of RoleAvatarInfo)
			Dim dr As IDataReader = Nothing
			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().RoleAvatar_GetAll(PortalID)
				While dr.Read
					Dim objAvatarInfo As RoleAvatarInfo = FillRoleAvatarInfo(dr)
					objAvatars.Add(objAvatarInfo)
				End While
				dr.NextResult()

			Catch ex As Exception
				LogException(ex)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objAvatars

		End Function

		''' <summary>
		''' This gets role avatars for a specific user. 
		''' </summary>
		''' <param name="PortalID">The portal the user belongs to (and we are viewing).</param>
		''' <param name="UserID">The user who we want the role avatars for.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetUsersRoleAvatars(ByVal PortalID As Integer, ByVal UserID As Integer) As List(Of RoleAvatarInfo)
			Dim objAvatars As New List(Of RoleAvatarInfo)
			Dim dr As IDataReader = Nothing
			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().RoleAvatar_GetUsers(PortalID, UserID)
				While dr.Read
					Dim objAvatarInfo As RoleAvatarInfo = FillRoleAvatarInfo(dr)
					objAvatars.Add(objAvatarInfo)
				End While
				dr.NextResult()

			Catch ex As Exception
				LogException(ex)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objAvatars
		End Function

		''' <summary>
		''' Attempts to load the stats from cache, if not available it retrieves it and places it in cache. 
		''' </summary>
		''' <param name="RoleID"></param>
		''' <param name="ModuleID"></param>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetFromCache(ByVal RoleID As Integer, ByVal ModuleID As Integer, ByVal PortalID As Integer) As String

			Dim strAvatar As String = String.Empty
			' moving this 
			Dim list As List(Of RoleAvatarInfo) = GetAllFromCache(ModuleID, PortalID)
			Dim role As RoleAvatarInfo

			For Each role In list
				If role.RoleID = RoleID Then
					strAvatar = role.Avatar
					Exit For
				End If
			Next

			Return strAvatar
		End Function

		''' <summary>
		''' Resets the RoleAvatar info in cache. 
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Private Sub ResetRoleAvatarInfo(ByVal ModuleID As Integer)
			Dim strCacheKey As String = RoleAvatarInfoCacheKeyPrefix & CStr(ModuleID)
			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' Attempts to load the list of  from cache, if not available it retrieves it and places it in cache. 
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetUserRoles(ByVal UserID As Integer) As List(Of RoleIdInfo)
			Dim strCacheKey As String = RoleCacheKeyPrefix & CStr(UserID)
			Dim objRoles As List(Of RoleIdInfo) = CType(DataCache.GetCache(strCacheKey), List(Of RoleIdInfo))

			If objRoles Is Nothing Then
				Dim dr As IDataReader = Nothing
				objRoles = New List(Of RoleIdInfo)
				Try
					'RoleId caching settings
					Dim timeOut As Int32 = ForumInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

					dr = DotNetNuke.Modules.Forum.DataProvider.Instance().RoleAvatar_GetUserRoles(UserID)
					While dr.Read
						Dim objRoleInfo As RoleIdInfo = FillRoleIdInfo(dr)
						objRoles.Add(objRoleInfo)
					End While
					dr.NextResult()

					'Cache Statistics if timeout > 0 and Statistics is not null
					If timeOut > 0 Then
						DataCache.SetCache(strCacheKey, objRoles, TimeSpan.FromMinutes(timeOut))
					End If

				Catch ex As Exception
					LogException(ex)
				Finally
					If Not dr Is Nothing Then
						dr.Close()
					End If
				End Try
			End If

			Return objRoles
		End Function

#End Region

#Region " Public Methods "

		''' <summary>
		''' Attempts to load the stats from cache, if not available it retrieves it and places it in cache. 
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetAllFromCache(ByVal ModuleID As Integer, ByVal PortalID As Integer) As List(Of RoleAvatarInfo)
			Dim strCacheKey As String = RoleAvatarInfoCacheKeyPrefix & CStr(ModuleID)

			Dim objAvatar As List(Of RoleAvatarInfo) = CType(DataCache.GetCache(strCacheKey), List(Of RoleAvatarInfo))

			If objAvatar Is Nothing Then
				'Statistics caching settings
				Dim timeOut As Int32 = ForumInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				objAvatar = GetAll(PortalID)
				'Cache Statistics if timeout > 0 and Statistics is not null
				If timeOut > 0 And objAvatar IsNot Nothing Then
					DataCache.SetCache(strCacheKey, objAvatar, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return objAvatar
		End Function

		''' <summary>
		''' Updates the RoleAvatar for the selecter RoleID
		''' </summary>
		''' <param name="RoleID"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Sub Update(ByVal RoleID As Integer, ByVal Avatar As String, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().RoleAvatar_Update(RoleID, Avatar)
			ResetRoleAvatarInfo(ModuleID)
		End Sub

		''' <summary>
		''' Deletes the RoleAvatar for the selecter RoleID
		''' </summary>
		''' <param name="RoleID"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Sub Delete(ByVal RoleID As Integer, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().RoleAvatar_Delete(RoleID)
			ResetRoleAvatarInfo(ModuleID)
		End Sub

		''' <summary>
		''' This is used to get role avatars for the user, it is sorted by file name ascending. (Top items would be 0 or A).
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="ModuleID"></param>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetUsersRoleAvatars(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PortalID As Integer) As String
			Dim strAvatars As String = String.Empty
			Dim colRoleAvatars As List(Of RoleAvatarInfo)

			colRoleAvatars = GetUsersRoleAvatars(PortalID, UserID)

			If Not colRoleAvatars Is Nothing Then
				For Each objAvatar As RoleAvatarInfo In colRoleAvatars
					If Len(objAvatar.Avatar) > 0 And (Not strAvatars.Contains(objAvatar.Avatar)) Then
						strAvatars += objAvatar.Avatar & ";"
					End If
				Next
			End If

			'We need to add something here to ensure this function isn't called again right away
			If strAvatars = String.Empty Then
				strAvatars = ";"
			End If

			Return strAvatars
		End Function

#End Region

#Region " Custom Hydrator "

		''' <summary>
		''' Hydrates the RoleAvatarinfo object
		''' </summary>
		''' <param name="dr"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/22/2008	Created
		''' </history>
		Private Function FillRoleAvatarInfo(ByVal dr As IDataReader) As RoleAvatarInfo
			Dim objAvatar As New RoleAvatarInfo
			Try
				objAvatar.RoleID = Convert.ToInt32(Null.SetNull(dr("RoleID"), objAvatar.RoleID))
			Catch
			End Try
			Try
				objAvatar.RoleName = Convert.ToString(Null.SetNull(dr("RoleName"), objAvatar.RoleName))
			Catch
			End Try
			Try
				objAvatar.Avatar = Convert.ToString(Null.SetNull(dr("Avatar"), objAvatar.Avatar))
			Catch
			End Try

			Return objAvatar
		End Function

		''' <summary>
		''' Hydrates the RoleIdInfo object
		''' </summary>
		''' <param name="dr"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/23/2008	Created
		''' </history>
		Private Function FillRoleIdInfo(ByVal dr As IDataReader) As RoleIdInfo
			Dim objRoles As New RoleIdInfo
			Try
				objRoles.RoleID = Convert.ToInt32(Null.SetNull(dr(0), objRoles.RoleID))
			Catch
			End Try
			Return objRoles
		End Function

#End Region

#Region " RoleIDInfo Class "

		''' <summary>A private info object to store RoleID</summary>
		''' <remarks></remarks>
		Private Class RoleIdInfo

#Region " Private Properties "

			Private mRoleID As Integer

#End Region

#Region " Public Properties "

			''' <summary>
			''' 
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Property RoleID() As Integer
				Get
					Return mRoleID
				End Get
				Set(ByVal value As Integer)
					mRoleID = value
				End Set
			End Property

#End Region

		End Class

#End Region

	End Class

End Namespace