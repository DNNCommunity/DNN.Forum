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

#Region "ForumUserController"

	''' <summary>
	''' Connector to the data layer for the forum user object
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	12/4/2005	Created
	''' </history>
	Public Class ForumUserController

#Region "Members"

		Private Const FORUM_USER_CACHE_KEY_PREFIX As String = "Forum_User-"
		Private Const ForumUserInfoCacheTimeout As Integer = 20

#End Region

#Region "Public Caching Methods"

		''' <summary>
		''' This gets the forum user based on id.  The additonal userid is to determine
		''' who is getting it, if it is the same it should update the LastVisit column
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="UpdateLastVisit"></param>
		''' <returns></returns>
		''' <remarks>(UpdateLastVisit)This is only true if called from Forum_Container, BaseObject could possibly be used
		''' later on but i didn't know the impact of this without investigation
		''' </remarks>
		Public Shared Function GetForumUser(ByVal UserID As Integer, ByVal UpdateLastVisit As Boolean, ByVal ModuleID As Integer, ByVal PortalID As Integer) As ForumUser
			Dim keyID As String = FORUM_USER_CACHE_KEY_PREFIX & UserID.ToString & "-" & PortalID.ToString
			Dim fUser As ForumUser = CType(DataCache.GetCache(keyID), ForumUser)

			If fUser Is Nothing Then
				'thread caching settings
				Dim timeOut As Int32 = ForumUserInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				' if current user is authenticated
				If UserID > 0 Then
					' get the forum user based on the UserID
					Dim ctlForumUser As New ForumUserController
					fUser = ctlForumUser.UserGet(PortalID, UserID, ModuleID)

					' we could not find this forum user 
					If fUser.UserID < 1 Then
						' get the DNN user
						Dim ctlDNNUser As New Users.UserController
						Dim dnnUser As New Users.UserInfo
						dnnUser = ctlDNNUser.GetUser(PortalID, UserID)

						If Not dnnUser Is Nothing Then
							' the user is a DNN user but has never visited the forums for this portal

							' Add check to see if the user has a profile on some other portalID
							'' USER NEW UserGetMultiPortalUser(UserID)
							Dim MultiPortalUser As ForumUser = ctlForumUser.UserGetMultiPortal(UserID, ModuleID)

							If Not MultiPortalUser.UserID = -1 Then
								Try
									MultiPortalUser.PortalID = PortalID
									'' USE NEW UserAddMultiPortalUser sproc (NOT SURE WE NEED, TEST W/OUT FIRST!!!)
									ctlForumUser.UserAdd(MultiPortalUser)

									fUser = ctlForumUser.UserGet(PortalID, UserID, ModuleID)
								Catch Exc As System.Exception
									LogException(Exc) ' error adding user to forums
								End Try
							Else
								' so lets create a forum user 
								fUser = New ForumUser(ModuleID)
								With fUser
									.PortalID = PortalID
									.UserID = dnnUser.UserID
									Dim myConfig As Forum.Config
									myConfig = Config.GetForumConfig(ModuleID)

									.ThreadsPerPage = myConfig.ThreadsPerPage
									.PostsPerPage = myConfig.PostsPerPage
									.EnableDisplayInMemberList = myConfig.EnableMemberList
									.EnableOnlineStatus = myConfig.EnableUsersOnline
									.EnablePM = myConfig.EnablePMSystem
									.EnablePMNotifications = myConfig.MailNotification
									.IsTrusted = myConfig.TrustNewUsers
									.EnablePublicEmail = False
									.EnableModNotification = myConfig.MailNotification
									.EnableSelfNotifications = False
								End With
								ctlForumUser.UserAdd(fUser)
								fUser = ctlForumUser.UserGet(PortalID, UserID, ModuleID)
							End If
						Else
							' the user was deleted from the DNN membership
							' so lets return an anonymous user
							fUser = New ForumUser(ModuleID)
							With fUser
								.UserID = -1
								.Username = "anonymous"
								.DisplayName = "anonymous"
								.EnablePublicEmail = False
								.EnablePM = False
								.EnableSelfNotifications = False
								.EnableModNotification = False
								.EnablePublicEmail = False
							End With
						End If
					End If

				Else
					' return an anonymous user
					fUser = New ForumUser(ModuleID)
					With fUser
						.UserID = -1
						.Username = "anonymous"
						.DisplayName = "anonymous"
						.EnablePublicEmail = False
						.EnablePM = False
					End With
				End If
				If timeOut > 0 And fUser IsNot Nothing Then
					DataCache.SetCache(keyID, fUser, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return fUser
		End Function

		''' <summary>
		''' Resets the cached user object.
		''' </summary>
		''' <param name="UserID"></param>
		''' <remarks>
		''' </remarks>
		Public Shared Sub ResetForumUser(ByVal UserID As Integer, ByVal PortalID As Integer)
			Dim keyID As String = FORUM_USER_CACHE_KEY_PREFIX & UserID.ToString & "-" & PortalID.ToString
			DataCache.RemoveCache(keyID)
		End Sub

#End Region

#Region "Custom Hydrator"

		''' <summary>
		''' This hydrates the forum user object
		''' </summary>
		''' <param name="dr"></param>
		''' <param name="PortalID"></param>
		''' <returns>ForumUser</returns>
		''' <remarks>
		''' </remarks>
		Friend Function FillForumUserInfo(ByVal dr As IDataReader, ByVal PortalID As Integer, ByVal ModuleID As Integer) As ForumUser
			Dim objForumUser As New ForumUser(ModuleID)
			Dim UserID As Integer = -1
			' portalid and issuperuser must be set first because they are used by the progressive hydration 
			Try
				objForumUser.PortalID = PortalID
			Catch
			End Try
			Try
				objForumUser.IsSuperUser = Convert.ToBoolean(Null.SetNull(dr("IsSuperUser"), objForumUser.IsSuperUser))
			Catch
			End Try

			Try
				objForumUser.UserID = Convert.ToInt32(dr("UserID"))
				UserID = Convert.ToInt32(dr("UserID"))
			Catch
			End Try

			' If the user is nothing, they were deleted/unregistered so skip populating info and use defaults (they no longer exist as a DNN user)
			If UserID > 0 Then
				Try
					objForumUser.Username = Convert.ToString(dr("Username"))
				Catch
				End Try
				Try
					objForumUser.DisplayName = Convert.ToString(dr("DisplayName"))
				Catch
				End Try
				Try
					objForumUser.Email = Convert.ToString(dr("Email"))
				Catch
				End Try
				Try
					objForumUser.UserAvatar = Convert.ToInt32(dr("UserAvatar"))
				Catch
				End Try
				Try
					objForumUser.Avatar = Convert.ToString(dr("Avatar"))
				Catch
				End Try
				Try
					objForumUser.SystemAvatars = Convert.ToString(dr("AdditionalAvatars"))
				Catch
				End Try
				Try
					objForumUser.Signature = Convert.ToString(dr("Signature"))
				Catch
				End Try
				Try
					objForumUser.Biography = Convert.ToString(dr("Biography"))
				Catch
				End Try
				Try
					objForumUser.EnableProfileRegion = Convert.ToBoolean(dr("EnableProfileRegion"))
				Catch
				End Try
				Try
					objForumUser.IsTrusted = Convert.ToBoolean(dr("IsTrusted"))
				Catch
				End Try
				Try
					objForumUser.EnableDisplayInMemberList = Convert.ToBoolean(dr("EnableDisplayInMemberList"))
				Catch
				End Try
				Try
					objForumUser.EnableOnlineStatus = Convert.ToBoolean(dr("EnableOnlineStatus"))
				Catch
				End Try
				Try
					objForumUser.ThreadsPerPage = Convert.ToInt32(dr("ThreadsPerPage"))
				Catch
				End Try
				Try
					objForumUser.PostsPerPage = Convert.ToInt32(dr("PostsPerPage"))
				Catch
				End Try
				Try
					objForumUser.EnableModNotification = Convert.ToBoolean(dr("EnableModNotification"))
				Catch
				End Try
				Try
					objForumUser.EnablePublicEmail = Convert.ToBoolean(dr("EnablePublicEmail"))
				Catch
				End Try
				Try
					objForumUser.EnablePM = Convert.ToBoolean(dr("EnablePM"))
				Catch
				End Try
				Try
					objForumUser.EnablePMNotifications = Convert.ToBoolean(dr("EnablePMNotifications"))
				Catch
				End Try
				Try
					objForumUser.EmailFormat = Convert.ToInt32(dr("EmailFormat"))
				Catch
				End Try
				Try
					objForumUser.FlatView = Convert.ToBoolean(dr("FlatView"))
				Catch
				End Try
				Try
					objForumUser.ViewDescending = Convert.ToBoolean(dr("ViewDescending"))
				Catch
				End Try
				Try
					objForumUser.TrackingDuration = Convert.ToInt32(dr("TrackingDuration"))
				Catch
				End Try
				Try
					objForumUser.PostCount = Convert.ToInt32(dr("PostCount"))
				Catch
				End Try
				Try
					objForumUser.LastActivity = Convert.ToDateTime(dr("LastActivity"))
				Catch
				End Try
				Try
					objForumUser.LockTrust = Convert.ToBoolean(dr("LockTrust"))
				Catch
				End Try
				Try
					objForumUser.IsBanned = Convert.ToBoolean(dr("IsBanned"))
				Catch
				End Try
				Try
					objForumUser.LiftBanDate = Convert.ToDateTime(dr("LiftBanDate"))
				Catch
				End Try
				Try
					objForumUser.EnableProfileWeb = Convert.ToBoolean(dr("EnableProfileWeb"))
				Catch
				End Try
				Try
					objForumUser.EnableDefaultPostNotify = Convert.ToBoolean(dr("EnableDefaultPostNotify"))
				Catch
				End Try
				Try
					objForumUser.EnableSelfNotifications = Convert.ToBoolean(dr("EnableSelfNotifications"))
				Catch
				End Try
				Try
					objForumUser.Biography = Convert.ToString(dr("Biography"))
				Catch
				End Try
				Try
					objForumUser.StartBanDate = Convert.ToDateTime(dr("StartBanDate"))
				Catch
				End Try
				Try
					objForumUser.TotalRecords = Convert.ToInt32(dr("TotalRecords"))
				Catch
				End Try
				objForumUser.IsDeleted = False
			Else
				objForumUser.Username = "anonymous"
				objForumUser.DisplayName = "anonymous"
				objForumUser.EnablePublicEmail = False
				objForumUser.EnablePM = False
				objForumUser.PostCount = 0
				objForumUser.EnableProfileWeb = False
				objForumUser.IsDeleted = True
			End If

			Return objForumUser
		End Function

		''' <summary>
		''' This is used to hydrate the core user object, 
		''' </summary>
		''' <param name="dr">A datareader containing all users returned from a query.</param>
		''' <param name="PortalID">The portal we are retrieving users for.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend Function FillPortalUserInfo(ByVal dr As IDataReader, ByVal PortalID As Integer) As Users.UserInfo
			Dim objUser As New Users.UserInfo
			' portalid and issuperuser must be set first because they are used by the progressive hydration 
			Try
				objUser.PortalID = PortalID
			Catch
			End Try
			Try
				objUser.IsSuperUser = Convert.ToBoolean(Null.SetNull(dr("IsSuperUser"), objUser.IsSuperUser))
			Catch
			End Try

			If objUser.IsSuperUser Then
				Common.Globals.SetApplicationName(Common.Globals.glbSuperUserAppName)
			Else
				Common.Globals.SetApplicationName(PortalID)
			End If

			Try
				objUser.UserID = Convert.ToInt32(dr("UserID"))
			Catch
			End Try
			Try
				objUser.Username = Convert.ToString(dr("Username"))
			Catch
			End Try
			Try
				objUser.FirstName = Convert.ToString(dr("FirstName"))
			Catch
			End Try
			Try
				objUser.LastName = Convert.ToString(dr("LastName"))
			Catch
			End Try
			Try
				objUser.DisplayName = Convert.ToString(dr("DisplayName"))
			Catch
			End Try
			Try
				objUser.Email = Convert.ToString(dr("Email"))
			Catch
			End Try

			Return objUser
		End Function

#End Region

#Region "Public Methods"

		''' <summary>
		''' Gets all user profiles based on a portal, not specific to forum but core instead.
		''' </summary>
		''' <param name="PortalId"></param>
		''' <param name="Filter"></param>
		''' <param name="PageSize"></param>
		''' <param name="PageIndex"></param>
		''' <param name="TotalRecords"></param>
		''' <returns></returns>
		''' <remarks>This is only used in Forum_Search/Search.ascx.  This is not used in Memberlist.(Same thing, one w/ dnn tree, other standard asp.net tree)</remarks>
		Public Function UserGetAll(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As ArrayList
			Dim objUsers As New ArrayList
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().UserGetAll(PortalId, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As ForumUser = FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
				While dr.Read
					TotalRecords = Convert.ToInt32(dr("TotalRecords"))
				End While
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers

		End Function

        Public Function UserGet(ByVal PortalID As Integer, ByVal UserId As Integer, ByVal ModuleID As Integer) As ForumUser
            Dim cacheKey As String = String.Concat("froumuser_", PortalID, UserId)
            Return CBO.GetCachedObject(Of ForumUser)(New CacheItemArgs(cacheKey, 5, DataCache.PortalDesktopModuleCachePriority, PortalID, UserId, ModuleID), _
                                                                                                             AddressOf UserGetCallBack)
        End Function


        ''' <summary>
        ''' Gets a single users profile (for the forum, which is distinct per portal)
        ''' </summary>
		''' <param name="cacheItemArgs"></param>
		''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UserGetCallBack(ByVal cacheItemArgs As CacheItemArgs) As ForumUser

            Dim portalID As Integer = DirectCast(cacheItemArgs.ParamList(0), Integer)
            Dim userID As Integer = DirectCast(cacheItemArgs.ParamList(1), Integer)
            Dim moduleID As Integer = DirectCast(cacheItemArgs.ParamList(2), Integer)
			Dim objUserInfo As New ForumUser(ModuleID)
			Dim dr As IDataReader = Nothing
			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().UserGet(UserId, PortalID)
				While dr.Read
					objUserInfo = FillForumUserInfo(dr, PortalID, ModuleID)
					' For user banning, add a check here for date and if banned. If so, we need to update the db, then run this method again. 
					If objUserInfo.IsBanned = True And objUserInfo.LiftBanDate < Date.Now Then
						objUserInfo.IsBanned = False
						objUserInfo.LiftBanDate = Null.NullDate

						UserUpdate(objUserInfo)
						UserGet(PortalID, UserId, ModuleID)
					End If

				End While
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUserInfo
		End Function

		''' <summary>
		''' Gets a single users profile (for the forum, which is distinct per portal)
		''' </summary>
		''' <param name="UserId"></param>
		''' <returns></returns>
		''' <remarks>This is typically only used in upgrades. It is possible that it is used in multi-portal if a user already exists but only for minimal items.</remarks>
		Public Function UserGetMultiPortal(ByVal UserId As Integer, ByVal ModuleID As Integer) As ForumUser
			Dim objUserInfo As New ForumUser(ModuleID)
			Return objUserInfo
		End Function

		''' <summary>
		''' Adds a forum users forum profile.
		''' </summary>
		''' <param name="User"></param>
		''' <remarks></remarks>
		Public Sub UserAdd(ByVal User As ForumUser)
			DotNetNuke.Modules.Forum.DataProvider.Instance().UserAdd(User.UserID, User.UserAvatar, User.Avatar, User.SystemAvatars, User.Signature, User.IsTrusted, User.EnableDisplayInMemberList, User.EnableOnlineStatus, User.ThreadsPerPage, User.PostsPerPage, User.EnablePublicEmail, User.EnablePM, User.EnablePMNotifications, User.PortalID)
		End Sub

		''' <summary>
		''' Updates a single forum user (by portalID)
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Sub Update(ByVal objUser As ForumUser)
			UserUpdate(objUser)
		End Sub

		''' <summary>
		''' Updates a Forum User's Forum Profile
		''' </summary>
		''' <param name="objUser"></param>		
		''' <remarks></remarks>
		Public Sub UserUpdate(ByVal objUser As ForumUser)
			DotNetNuke.Modules.Forum.DataProvider.Instance().UserUpdate(objUser.UserID, objUser.UserAvatar, objUser.Avatar, objUser.SystemAvatars, objUser.Signature, objUser.IsTrusted, objUser.EnableDisplayInMemberList, objUser.EnableOnlineStatus, objUser.ThreadsPerPage, objUser.PostsPerPage, objUser.EnableModNotification, objUser.EnablePublicEmail, objUser.EnablePM, objUser.EnablePMNotifications, objUser.EmailFormat, objUser.PortalID, objUser.LockTrust, objUser.EnableProfileWeb, objUser.EnableProfileRegion, objUser.EnableDefaultPostNotify, objUser.EnableSelfNotifications, objUser.IsBanned, objUser.LiftBanDate, objUser.Biography, objUser.StartBanDate)
            DataCache.RemoveCache(String.Concat("froumuser_", objUser.PortalID, objUser.UserID))
		End Sub

		''' <summary>
		''' Not Implemented
		''' </summary>
		''' <param name="UserId"></param>
		''' <param name="FlatView"></param>
		''' <param name="ViewDescending"></param>
		''' <remarks></remarks>
		Public Sub UserViewUpdate(ByVal UserId As Integer, ByVal FlatView As Boolean, ByVal ViewDescending As Boolean)
			DotNetNuke.Modules.Forum.DataProvider.Instance().UserViewUpdate(UserId, FlatView, ViewDescending)
		End Sub

#Region "Not Implemented"

		''' <summary>
		''' This updates the users last visit time.
		''' </summary>
		''' <param name="UserId"></param>
		''' <remarks>
		''' </remarks>
		Public Sub UserUpdateLastVisit(ByVal UserId As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().UserUpdateLastVisit(UserId)
		End Sub

		''' <summary>
		''' This updates the user's tracking days for threads view.
		''' </summary>
		''' <param name="TrackingDuration"></param>
		''' <remarks></remarks>
		Public Sub UserUpdateTrackingDuration(ByVal TrackingDuration As Integer, ByVal UserID As Integer, ByVal PortalID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().UserUpdateTrackingDuration(TrackingDuration, UserID, PortalID)
		End Sub

#End Region

#Region "MemberList"

		''' <summary>
		''' Gets forum users for member directory by username (all w/ setting enabled)
		''' </summary>
		''' <param name="PortalId"></param>
		''' <param name="Filter"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="TotalRecords"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function MembersGetByUsername(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUser)
			Dim objUsers As New List(Of ForumUser)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetByUsername(PortalId, Filter, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As ForumUser = FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

		''' <summary>
		''' Gets forum users for member directory by displayname (all w/ setting enabled)
		''' </summary>
		''' <param name="PortalId"></param>
		''' <param name="Filter"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="TotalRecords"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function MembersGetByDisplayName(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUser)
			Dim objUsers As New List(Of ForumUser)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetByDisplayName(PortalId, Filter, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As ForumUser = FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

		''' <summary>
		''' Gets forum users for member directory (all w/ setting enabled)
		''' </summary>
		''' <param name="PortalId"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="TotalRecords"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function MembersGetAll(ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUser)
			Dim objUsers As New List(Of ForumUser)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetAll(PortalId, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As ForumUser = FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

		''' <summary>
		''' Gets forum users for member directory by email address (all w/ setting enabled)
		''' </summary>
		''' <param name="PortalId"></param>
		''' <param name="Filter"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="TotalRecords"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function MembersGetByEmail(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUser)
			Dim objUsers As New List(Of ForumUser)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetByEmail(PortalId, Filter, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As ForumUser = FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

		''' <summary>
		''' Gets forum users for member directory by profile property (all w/ setting enabled)
		''' </summary>
		''' <param name="PortalId"></param>
		''' <param name="PropertyName"></param>
		''' <param name="PropertyValue"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="TotalRecords"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function MembersGetByProfileProp(ByVal PortalId As Integer, ByVal PropertyName As String, ByVal PropertyValue As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUser)
			Dim objUsers As New List(Of ForumUser)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetByProfileProp(PortalId, PropertyName, PropertyValue, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As ForumUser = FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

		''' <summary>
		''' Gets all the forum users currently online for the member directory (all w/ setting enabled)
		''' </summary>
		''' <param name="PortalId"></param>
		''' <param name="Filter"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="TotalRecords"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function MembersGetOnline(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUser)
			Dim objUsers As New List(Of ForumUser)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetOnline(PortalId)
				While dr.Read
					Dim objUserInfo As ForumUser = FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

#End Region

#Region "Bannning"

		''' <summary>
		''' Gets users who are banned from posting in the forum.
		''' </summary>
		''' <param name="PortalId">The Portal the users belong to.</param>
		''' <param name="PageIndex">The page of the collection you are attempting to return.</param>
		''' <param name="PageSize">The number of records to return.</param>
		''' <param name="ModuleID">The module this collection is being retrieved for (used for links only).</param>
		''' <param name="TotalRecords"></param>
		''' <returns>A collection of banned forum users. </returns>
		''' <remarks></remarks>
		Public Function GetBannedUsers(ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal ModuleID As Integer, ByRef TotalRecords As Integer) As List(Of ForumUser)
			Dim objUsers As New List(Of ForumUser)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().GetBannedUsers(PortalId, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As ForumUser = FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
				While dr.Read
					TotalRecords = Convert.ToInt32(dr("TotalRecords"))
				End While
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

#End Region

#Region "Manage Users"

		''' <summary>
		''' Gets the core users based on the DisplayName column from the DNN Core Users table. (Using a view)
		''' </summary>
		''' <param name="PortalId">The portal to get the users of matching the filter.</param>
		''' <param name="Filter">The filter to be applied when searching for user display name's.</param>
		''' <param name="PageIndex">The page to return of users (index).</param>
		''' <param name="PageSize">The number of users to return.</param>
		''' <param name="TotalRecords">The total number of records the query returns.</param>
		''' <returns>An arraylist containing all users the query returns based on the incoming parameters.</returns>
		''' <remarks>This is only here because there is no core method to retrieve users by DisplayName and should only be called from ManageUsers.</remarks>
		Public Function ManageGetAllByDisplayName(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer) As ArrayList
			Dim objUsers As New ArrayList
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().ManageGetAllByDisplayName(PortalId, Filter, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As Users.UserInfo = FillPortalUserInfo(dr, PortalId)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
				While dr.Read
					TotalRecords = Convert.ToInt32(dr("TotalRecords"))
				End While
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

		''' <summary>
		''' Gets the core users based on a role name.
		''' </summary>
		''' <param name="PortalId">The portal the users belong to.</param>
		''' <param name="RoleName">The name of the role the users must belong to.</param>
		''' <param name="PageIndex">The page of the collection you are attempting to return.</param>
		''' <param name="PageSize">The number of records to return.</param>
		''' <param name="TotalRecords"></param>
		''' <returns>A collection of users in a specific role. </returns>
		''' <remarks></remarks>
		Public Function ManageGetUsersByRolename(ByVal PortalId As Integer, ByVal RoleName As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer) As ArrayList
			Dim objUsers As New ArrayList
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().ManageGetUsersByRolename(PortalId, RoleName, PageIndex, PageSize)
				While dr.Read
					Dim objUserInfo As Users.UserInfo = FillPortalUserInfo(dr, PortalId)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
				While dr.Read
					TotalRecords = Convert.ToInt32(dr("TotalRecords"))
				End While
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

#End Region

#End Region

	End Class

#End Region

End Namespace
