'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2011
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
    ''' Connector to the data layer for the forum user object
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
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
        Public Function GetForumUser(ByVal UserID As Integer, ByVal UpdateLastVisit As Boolean, ByVal ModuleID As Integer, ByVal PortalID As Integer) As ForumUserInfo
            If UserID > 0 Then
                Dim cacheKey As String = FORUM_USER_CACHE_KEY_PREFIX & UserID.ToString & "-" & PortalID.ToString
                Dim timeOut As Int32 = ForumUserInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)
                Dim objForumUser As ForumUserInfo   'As New ForumUser(ModuleID)

                objForumUser = TryCast(DataCache.GetCache(cacheKey), ForumUserInfo)

                If objForumUser Is Nothing Then
                    Return UserCheck(UserID, PortalID, ModuleID)
                Else
                    ' Forum User Object is something, make sure fUser.UserID > 0
                    If objForumUser.UserID > 0 Then
                        Return objForumUser
                    Else
                        Return UserCheck(UserID, PortalID, ModuleID)
                    End If
                End If
            Else
                ' We know the user is simply not logged in.
                Return SetAnonymousUser(ModuleID)
            End If
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
        Friend Function FillForumUserInfo(ByVal dr As IDataReader, ByVal PortalID As Integer, ByVal ModuleID As Integer) As ForumUserInfo
            Dim objForumUser As New ForumUserInfo(ModuleID)
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
                    objForumUser.IsTrusted = Convert.ToBoolean(dr("IsTrusted"))
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
                    objForumUser.EmailFormat = Convert.ToInt32(dr("EmailFormat"))
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
                    objForumUser.EnableDefaultPostNotify = Convert.ToBoolean(dr("EnableDefaultPostNotify"))
                Catch
                End Try
                Try
                    objForumUser.EnableSelfNotifications = Convert.ToBoolean(dr("EnableSelfNotifications"))
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
                objForumUser.PostCount = 0
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
                    Dim objUserInfo As ForumUserInfo = FillForumUserInfo(dr, PortalId, ModuleID)
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
        ''' Adds a forum users forum profile.
        ''' </summary>
        ''' <param name="User"></param>
        ''' <remarks></remarks>
        Public Sub UserAdd(ByVal User As ForumUserInfo)
            DotNetNuke.Modules.Forum.DataProvider.Instance().UserAdd(User.UserID, User.UserAvatar, User.Avatar, User.SystemAvatars, User.Signature, User.IsTrusted, User.EnableOnlineStatus, User.ThreadsPerPage, User.PostsPerPage, False, User.PortalID)
        End Sub

        ''' <summary>
        ''' Updates a single forum user (by portalID)
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Public Sub Update(ByVal objUser As ForumUserInfo)
            UserUpdate(objUser)
        End Sub

        ''' <summary>
        ''' Updates a Forum User's Forum Profile
        ''' </summary>
        ''' <param name="objUser"></param>		
        ''' <remarks></remarks>
        Public Sub UserUpdate(ByVal objUser As ForumUserInfo)
            DotNetNuke.Modules.Forum.DataProvider.Instance().UserUpdate(objUser.UserID, objUser.UserAvatar, objUser.Avatar, objUser.SystemAvatars, objUser.Signature, objUser.IsTrusted, objUser.EnableOnlineStatus, objUser.ThreadsPerPage, objUser.PostsPerPage, objUser.EnableModNotification, False, objUser.EmailFormat, objUser.PortalID, objUser.LockTrust, False, objUser.EnableDefaultPostNotify, objUser.EnableSelfNotifications, objUser.IsBanned, objUser.LiftBanDate, objUser.StartBanDate)
            DataCache.RemoveCache(String.Concat(FORUM_USER_CACHE_KEY_PREFIX & objUser.UserID.ToString & "-" & objUser.PortalID.ToString))
        End Sub

        ''' <summary>
        ''' Not Implemented
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PortalID"></param>
        ''' <param name="ViewDescending"></param>
        ''' <remarks></remarks>
        Public Sub UpdateUsersView(ByVal UserID As Integer, ByVal PortalID As Integer, ByVal ViewDescending As Boolean)
            DotNetNuke.Modules.Forum.DataProvider.Instance().UpdateUsersView(UserID, PortalID, ViewDescending)
            DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(UserID, PortalID)
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

        ' ''' <summary>
        ' ''' Gets forum users for member directory by username (all w/ setting enabled)
        ' ''' </summary>
        ' ''' <param name="PortalId"></param>
        ' ''' <param name="Filter"></param>
        ' ''' <param name="PageIndex"></param>
        ' ''' <param name="PageSize"></param>
        ' ''' <param name="TotalRecords"></param>
        ' ''' <param name="ModuleID"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function MembersGetByUsername(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUserInfo)
        '	Dim objUsers As New List(Of ForumUserInfo)
        '	Dim dr As IDataReader = Nothing

        '	Try
        '		dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetByUsername(PortalId, Filter, PageIndex, PageSize)
        '		While dr.Read
        '			Dim objUserInfo As ForumUserInfo = FillForumUserInfo(dr, PortalId, ModuleID)
        '			objUsers.Add(objUserInfo)
        '		End While
        '		dr.NextResult()
        '	Catch exc As Exception
        '		LogException(exc)
        '	Finally
        '		If Not dr Is Nothing Then
        '			dr.Close()
        '		End If
        '	End Try

        '	Return objUsers
        'End Function

        ' ''' <summary>
        ' ''' Gets forum users for member directory by displayname (all w/ setting enabled)
        ' ''' </summary>
        ' ''' <param name="PortalId"></param>
        ' ''' <param name="Filter"></param>
        ' ''' <param name="PageIndex"></param>
        ' ''' <param name="PageSize"></param>
        ' ''' <param name="TotalRecords"></param>
        ' ''' <param name="ModuleID"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function MembersGetByDisplayName(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUserInfo)
        '	Dim objUsers As New List(Of ForumUserInfo)
        '	Dim dr As IDataReader = Nothing

        '	Try
        '		dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetByDisplayName(PortalId, Filter, PageIndex, PageSize)
        '		While dr.Read
        '			Dim objUserInfo As ForumUserInfo = FillForumUserInfo(dr, PortalId, ModuleID)
        '			objUsers.Add(objUserInfo)
        '		End While
        '		dr.NextResult()
        '	Catch exc As Exception
        '		LogException(exc)
        '	Finally
        '		If Not dr Is Nothing Then
        '			dr.Close()
        '		End If
        '	End Try

        '	Return objUsers
        'End Function

        ' ''' <summary>
        ' ''' Gets forum users for member directory (all w/ setting enabled)
        ' ''' </summary>
        ' ''' <param name="PortalId"></param>
        ' ''' <param name="PageIndex"></param>
        ' ''' <param name="PageSize"></param>
        ' ''' <param name="TotalRecords"></param>
        ' ''' <param name="ModuleID"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function MembersGetAll(ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUserInfo)
        '	Dim objUsers As New List(Of ForumUserInfo)
        '	Dim dr As IDataReader = Nothing

        '	Try
        '		dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetAll(PortalId, PageIndex, PageSize)
        '		While dr.Read
        '			Dim objUserInfo As ForumUserInfo = FillForumUserInfo(dr, PortalId, ModuleID)
        '			objUsers.Add(objUserInfo)
        '		End While
        '		dr.NextResult()
        '	Catch exc As Exception
        '		LogException(exc)
        '	Finally
        '		If Not dr Is Nothing Then
        '			dr.Close()
        '		End If
        '	End Try

        '	Return objUsers
        'End Function

        ' ''' <summary>
        ' ''' Gets forum users for member directory by email address (all w/ setting enabled)
        ' ''' </summary>
        ' ''' <param name="PortalId"></param>
        ' ''' <param name="Filter"></param>
        ' ''' <param name="PageIndex"></param>
        ' ''' <param name="PageSize"></param>
        ' ''' <param name="TotalRecords"></param>
        ' ''' <param name="ModuleID"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function MembersGetByEmail(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUserInfo)
        '	Dim objUsers As New List(Of ForumUserInfo)
        '	Dim dr As IDataReader = Nothing

        '	Try
        '		dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetByEmail(PortalId, Filter, PageIndex, PageSize)
        '		While dr.Read
        '			Dim objUserInfo As ForumUserInfo = FillForumUserInfo(dr, PortalId, ModuleID)
        '			objUsers.Add(objUserInfo)
        '		End While
        '		dr.NextResult()
        '	Catch exc As Exception
        '		LogException(exc)
        '	Finally
        '		If Not dr Is Nothing Then
        '			dr.Close()
        '		End If
        '	End Try

        '	Return objUsers
        'End Function

        ' ''' <summary>
        ' ''' Gets forum users for member directory by profile property (all w/ setting enabled)
        ' ''' </summary>
        ' ''' <param name="PortalId"></param>
        ' ''' <param name="PropertyName"></param>
        ' ''' <param name="PropertyValue"></param>
        ' ''' <param name="PageIndex"></param>
        ' ''' <param name="PageSize"></param>
        ' ''' <param name="TotalRecords"></param>
        ' ''' <param name="ModuleID"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function MembersGetByProfileProp(ByVal PortalId As Integer, ByVal PropertyName As String, ByVal PropertyValue As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUserInfo)
        '	Dim objUsers As New List(Of ForumUserInfo)
        '	Dim dr As IDataReader = Nothing

        '	Try
        '		dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetByProfileProp(PortalId, PropertyName, PropertyValue, PageIndex, PageSize)
        '		While dr.Read
        '			Dim objUserInfo As ForumUserInfo = FillForumUserInfo(dr, PortalId, ModuleID)
        '			objUsers.Add(objUserInfo)
        '		End While
        '		dr.NextResult()
        '	Catch exc As Exception
        '		LogException(exc)
        '	Finally
        '		If Not dr Is Nothing Then
        '			dr.Close()
        '		End If
        '	End Try

        '	Return objUsers
        'End Function

        ' ''' <summary>
        ' ''' Gets all the forum users currently online for the member directory (all w/ setting enabled)
        ' ''' </summary>
        ' ''' <param name="PortalId"></param>
        ' ''' <param name="Filter"></param>
        ' ''' <param name="PageIndex"></param>
        ' ''' <param name="PageSize"></param>
        ' ''' <param name="TotalRecords"></param>
        ' ''' <param name="ModuleID"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function MembersGetOnline(ByVal PortalId As Integer, ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer, ByVal ModuleID As Integer) As List(Of ForumUserInfo)
        '	Dim objUsers As New List(Of ForumUserInfo)
        '	Dim dr As IDataReader = Nothing

        '	Try
        '		dr = DotNetNuke.Modules.Forum.DataProvider.Instance().MembersGetOnline(PortalId)
        '		While dr.Read
        '			Dim objUserInfo As ForumUserInfo = FillForumUserInfo(dr, PortalId, ModuleID)
        '			objUsers.Add(objUserInfo)
        '		End While
        '		dr.NextResult()
        '	Catch exc As Exception
        '		LogException(exc)
        '	Finally
        '		If Not dr Is Nothing Then
        '			dr.Close()
        '		End If
        '	End Try

        '	Return objUsers
        'End Function

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
        Public Function GetBannedUsers(ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal ModuleID As Integer, ByRef TotalRecords As Integer) As List(Of ForumUserInfo)
            Dim objUsers As New List(Of ForumUserInfo)
            Dim dr As IDataReader = Nothing

            Try
                dr = DotNetNuke.Modules.Forum.DataProvider.Instance().GetBannedUsers(PortalId, PageIndex, PageSize)
                While dr.Read
                    Dim objUserInfo As ForumUserInfo = FillForumUserInfo(dr, PortalId, ModuleID)
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

#Region "Private Methods"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="PortalID"></param>
        ''' <param name="UserId"></param>
        ''' <param name="ModuleID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function UserGet(ByVal PortalID As Integer, ByVal UserId As Integer, ByVal ModuleID As Integer) As ForumUserInfo
            Dim cacheKey As String = FORUM_USER_CACHE_KEY_PREFIX & UserId.ToString & "-" & PortalID.ToString
            Return CBO.GetCachedObject(Of ForumUserInfo)(New CacheItemArgs(cacheKey, 5, DataCache.PortalDesktopModuleCachePriority, PortalID, UserId, ModuleID), AddressOf UserGetCallBack)
        End Function

        ''' <summary>
        ''' Gets a single users profile (for the forum, which is distinct per portal)
        ''' </summary>
        ''' <param name="cacheItemArgs"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function UserGetCallBack(ByVal cacheItemArgs As CacheItemArgs) As ForumUserInfo
            Dim portalID As Integer = DirectCast(cacheItemArgs.ParamList(0), Integer)
            Dim userID As Integer = DirectCast(cacheItemArgs.ParamList(1), Integer)
            Dim moduleID As Integer = DirectCast(cacheItemArgs.ParamList(2), Integer)
            Dim objUserInfo As ForumUserInfo = Nothing
            Dim dr As IDataReader = Nothing
            Try
                dr = DotNetNuke.Modules.Forum.DataProvider.Instance().UserGet(userID, portalID)
                If dr.Read Then
                    objUserInfo = FillForumUserInfo(dr, portalID, moduleID)
                    ' For user banning, add a check here for date and if banned. If so, we need to update the db, then run this method again. 
                    If objUserInfo.IsBanned = True And objUserInfo.LiftBanDate < Date.Now Then
                        objUserInfo.IsBanned = False
                        objUserInfo.LiftBanDate = Null.NullDate

                        UserUpdate(objUserInfo)
                        'UserGet(portalID, userID, moduleID)
                    End If
                Else
                    objUserInfo = SetForumUser(userID, portalID, moduleID)
                End If
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
        ''' This returns standard forum user information for anonymous or deleted users. 
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function SetAnonymousUser(ByVal ModuleID As Integer) As ForumUserInfo
            Dim fUser As New ForumUserInfo(ModuleID)

            With fUser
                .UserID = -1
                .Username = "anonymous"
                .DisplayName = "anonymous"
                .IsTrusted = False
                .EnableSelfNotifications = False
                .EnableModNotification = False
            End With

            Return fUser
        End Function

        ''' <summary>
        ''' Used to retrieve a core user (ie. membership by the UserID).
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PortalID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetCoreUser(ByVal UserID As Integer, ByVal PortalID As Integer) As Entities.Users.UserInfo
            Dim cntUser As New Users.UserController
            Return cntUser.GetUser(PortalID, UserID)
        End Function

        ''' <summary>
        ''' This will create a new user for the forum. 
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PortalID"></param>
        ''' <param name="ModuleID"></param>
        ''' <remarks></remarks>
        Private Function SetForumUser(ByVal UserID As Integer, ByVal PortalID As Integer, ByVal ModuleID As Integer) As ForumUserInfo
            Dim cntForumUser As New ForumUserController
            Dim fUser As New ForumUserInfo(ModuleID)

            fUser.PortalID = PortalID
            fUser.UserID = UserID
            Dim myConfig As Forum.Configuration
            myConfig = Configuration.GetForumConfig(ModuleID)

            fUser.ThreadsPerPage = myConfig.ThreadsPerPage
            fUser.PostsPerPage = myConfig.PostsPerPage
            fUser.EnableOnlineStatus = myConfig.EnableUsersOnline
            fUser.IsTrusted = myConfig.EnableAutoTrust And myConfig.AutoTrustTime = 0
            fUser.EnableModNotification = myConfig.MailNotification
            fUser.EnableSelfNotifications = False

            cntForumUser.UserAdd(fUser)
            DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(UserID, PortalID)

            Return fUser
        End Function

        ''' <summary>
        ''' This checks to see if a user exists for the portal's forum. If a valid user exists here, the cache is set. If not, user is created so long as they exist as a core user. 
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PortalID"></param>
        ''' <param name="ModuleID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function UserCheck(ByVal UserID As Integer, ByVal PortalID As Integer, ByVal ModuleID As Integer) As ForumUserInfo
            Dim cacheKey As String = FORUM_USER_CACHE_KEY_PREFIX & UserID.ToString & "-" & PortalID.ToString
            Dim timeOut As Int32 = ForumUserInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

            Dim cntForumUser As New ForumUserController
            Dim fUser As New ForumUserInfo(ModuleID)
            fUser = cntForumUser.UserGet(PortalID, UserID, ModuleID)

            '' we could not find this forum user 
            'If fUser Is Nothing Then
            '    Dim dnnUser As New Users.UserInfo
            '    dnnUser = GetCoreUser(UserID, PortalID)

            '    If Not dnnUser Is Nothing Then
            '        ' the user is a DNN user but has never visited the forums for this portal
            '        SetForumUser(UserID, PortalID, ModuleID)

            '        fUser = New ForumUser(ModuleID)
            '        fUser = cntForumUser.UserGet(PortalID, UserID, ModuleID)
            '    End If
            'End If

            'If timeOut > 0 And fUser IsNot Nothing Then
            '    DataCache.SetCache(cacheKey, fUser, TimeSpan.FromMinutes(timeOut))
            'End If

            Return fUser
        End Function

#End Region

    End Class

End Namespace