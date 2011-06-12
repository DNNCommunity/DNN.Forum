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
	''' Used to communicate with the data store about Forum specific items. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class ForumController

#Region "Private Members"

		Private Const ForumCacheKeyPrefix As String = Constants.CACHE_KEY_PREFIX + "FORUM_"
		Private Const ForumModuleColCacheKeyPrefix As String = Constants.CACHE_KEY_PREFIX + "FORUM_MODULE_COL_"
		Private Const ForumGroupColCacheKeyPrefix As String = Constants.CACHE_KEY_PREFIX + "FORUM_GROUP_COL_"
		Private Const ForumChildColCacheKeyPrefix As String = Constants.CACHE_KEY_PREFIX + "FORUM_CHILD_COL_"

#End Region

#Region "Friend Methods"

		''' <summary>
		''' Checks cache to get all forums for a specified GroupID, if not in cache a collection is 
		''' retrieved from the database then hydrated one by one and placed into cache.
		''' </summary>
		''' <param name="GroupId">The GroupID of forums to populate.</param>
		''' <returns>An arraylist of forums belonging to a specific GroupID.</returns>
		''' <remarks></remarks>
		Friend Function GetChildForums(ByVal ParentID As Integer, ByVal GroupId As Integer, ByVal EnabledOnly As Boolean) As List(Of ForumInfo)
			Dim arrForums As List(Of ForumInfo)
			Dim strCacheKey As String

			If EnabledOnly Then
				strCacheKey = ForumChildColCacheKeyPrefix + "-" + CStr(ParentID) + "-" + CStr(GroupId) + "-TRUE"
			Else
				strCacheKey = ForumChildColCacheKeyPrefix + "-" + CStr(ParentID) + "-" + CStr(GroupId) + "-FALSE"
			End If

			arrForums = CType(DataCache.GetCache(strCacheKey), List(Of ForumInfo))

			If arrForums Is Nothing Then
				'forum caching settings
				Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				arrForums = CBO.FillCollection(Of ForumInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ForumGetAllByParentID(ParentID, GroupId, EnabledOnly))

				'Cache Forum if timeout > 0 and Forum is not null
				If timeOut > 0 And arrForums IsNot Nothing Then
					DataCache.SetCache(strCacheKey, arrForums, TimeSpan.FromMinutes(timeOut))
				End If
			End If
			Return arrForums
		End Function

		''' <summary>
		''' Checks the cache first, if it is not populated it then loads the foruminfo
		''' </summary>
		''' <param name="ForumID">The ForumID (integer) that we want to retrieve information about (and store in cache).</param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Friend Function GetForumItemCache(ByVal ForumID As Integer) As ForumInfo
			Dim strCacheKey As String = ForumCacheKeyPrefix + CStr(ForumID)
			Dim objForum As ForumInfo = CType(DataCache.GetCache(strCacheKey), ForumInfo)

			If objForum Is Nothing Then
				If ForumID > 0 Then
					'forum caching settings
					Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)
					objForum = New ForumInfo
					objForum = GetForum(ForumID)

					'Cache Forum if timeout > 0 and Forum is not null
					If timeOut > 0 And objForum IsNot Nothing Then
						DataCache.SetCache(strCacheKey, objForum, TimeSpan.FromMinutes(timeOut))
					End If
				Else
					objForum = New ForumInfo
				End If
			End If

			Return objForum
		End Function

		''' <summary>
		''' Checks cache to get all forums for a specified GroupID, if not in cache a collection is 
		''' retrieved from the database then hydrated one by one and placed into cache.
		''' </summary>
		''' <param name="GroupId">The GroupID of forums to populate.</param>
		''' <returns>An arraylist of forums belonging to a specific GroupID.</returns>
		''' <remarks></remarks>
		Friend Function GetParentForums(ByVal GroupId As Integer) As List(Of ForumInfo)
			Dim arrForums As List(Of ForumInfo)
			Dim strCacheKey As String = ForumGroupColCacheKeyPrefix + "-" + CStr(GroupId)
			arrForums = CType(DataCache.GetCache(strCacheKey), List(Of ForumInfo))

			If arrForums Is Nothing Then
				'forum caching settings
				Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				arrForums = CBO.FillCollection(Of ForumInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ForumGetAll(GroupId))

				'Cache Forum if timeout > 0 and Forum is not null
				If timeOut > 0 And arrForums IsNot Nothing Then
					DataCache.SetCache(strCacheKey, arrForums, TimeSpan.FromMinutes(timeOut))
				End If
			End If
			Return arrForums
		End Function

		''' <summary>
		''' Retreives all forums associated with a single module. 
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend Function GetModuleForums(ByVal ModuleID As Integer) As List(Of ForumInfo)
			Dim arrForums As List(Of ForumInfo)
			Dim strCacheKey As String = ForumModuleColCacheKeyPrefix + CStr(ModuleID)
			arrForums = CType(DataCache.GetCache(strCacheKey), List(Of ForumInfo))

			If arrForums Is Nothing Then
				'forum caching settings
				Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				arrForums = GetForumByModuleID(ModuleID)

				'Cache Forum if timeout > 0 and Forum is not null
				If timeOut > 0 And arrForums IsNot Nothing Then
					DataCache.SetCache(strCacheKey, arrForums, TimeSpan.FromMinutes(timeOut))
				End If
			End If
			Return arrForums
		End Function

		''' <summary>
		''' Clears the cached intance of forums for the specified GroupID. 
		''' </summary>
		''' <param name="GroupID">The GroupID of forums to clear the cached items for.</param>
		''' <remarks></remarks>
		Friend Shared Sub ResetChildForumsCache(ByVal ParentID As Integer, ByVal GroupID As Integer)
			Dim strCacheKeyT As String = ForumChildColCacheKeyPrefix + "-" + CStr(ParentID) + "-" + CStr(GroupID) + "-TRUE"
			Dim strCacheKeyF As String = ForumChildColCacheKeyPrefix + "-" + CStr(ParentID) + "-" + CStr(GroupID) + "-FALSE"

			DataCache.RemoveCache(strCacheKeyT)
			DataCache.RemoveCache(strCacheKeyF)
		End Sub

		''' <summary>
		''' Resets the cache for the forumuser Info.
		''' </summary>
		''' <param name="ForumID">The ForumID (integer) that we want to remove from cache.</param>
		''' <remarks>
		''' </remarks>
		Friend Shared Sub ResetForumItemCache(ByVal ForumID As Integer)
			Dim strCacheKey As String = ForumCacheKeyPrefix + CStr(ForumID)

			ForumPermissionController.ClearCache_GetForumPermissionsDictionary(ForumID)
			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' Clears the cache of parent forums within a group.
		''' </summary>
		''' <param name="GroupId"></param>
		''' <remarks></remarks>
		Friend Shared Sub ResetParentForumsCache(ByVal GroupId As Integer)
			Dim strCacheKey As String = ForumGroupColCacheKeyPrefix + "-" + CStr(GroupId)
			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' Clears the cache of forums within a single module.
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Friend Shared Sub ResetModuleForumsCache(ByVal ModuleID As Integer)
			Dim strCacheKey As String = ForumModuleColCacheKeyPrefix + CStr(ModuleID)
			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' Adds a new forum to the data store. 
		''' </summary>
		''' <param name="objForum">The ForumInfo object to add to the data store.</param>
		''' <returns></returns>
		''' <remarks>If anything is changed here, consider chaging ForumPreConfig.vb method for adding default "General" forum.</remarks>
		Friend Function ForumAdd(ByVal objForum As ForumInfo) As Integer
			Dim ForumId As Integer = DotNetNuke.Modules.Forum.DataProvider.Instance().ForumAdd(objForum.GroupID, objForum.IsActive, objForum.ParentID, objForum.Name, objForum.Description, objForum.IsModerated, objForum.ForumType, objForum.PublicView, objForum.CreatedByUser, objForum.PublicPosting, objForum.EnableForumsThreadStatus, objForum.EnableForumsRating, objForum.ForumLink, objForum.ForumBehavior, objForum.AllowPolls, objForum.EnableRSS, objForum.EmailAddress, objForum.EmailFriendlyFrom, objForum.NotifyByDefault, objForum.EmailStatusChange, objForum.EmailServer, objForum.EmailUser, objForum.EmailPass, objForum.EmailEnableSSL, objForum.EmailAuth, objForum.EmailPort, objForum.EnableSitemap, objForum.SitemapPriority)

			' update forum permissions
			If Not objForum.ForumPermissions Is Nothing Then
				Dim objForumPermissionController As New ForumPermissionController
				Dim objCurrentForumPermissions As ForumPermissionCollection
				objCurrentForumPermissions = objForumPermissionController.GetForumPermissionsCollection(ForumId)
				If Not objCurrentForumPermissions.CompareTo(objForum.ForumPermissions) Then
					objForumPermissionController.DeleteForumPermissionsByForumID(ForumId)
					Dim objForumPermission As ForumPermissionInfo
					For Each objForumPermission In objForum.ForumPermissions
						objForumPermission.ForumID = ForumId
						If objForumPermission.AllowAccess Then
							objForumPermissionController.AddForumPermission(objForumPermission)
						End If
					Next
				End If
			End If

			Return ForumId
		End Function

		''' <summary>
		''' Updates an existing forum in the data store. 
		''' </summary>
		''' <param name="objForum">The ForumInfo object being updated in the data store.</param>
		''' <remarks></remarks>
		Friend Sub ForumUpdate(ByVal objForum As ForumInfo, ByVal PreviousParentID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ForumUpdate(objForum.GroupID, objForum.ForumID, objForum.IsActive, objForum.ParentID, objForum.Name, objForum.Description, objForum.IsModerated, objForum.ForumType, objForum.PublicView, objForum.UpdatedByUser, objForum.PublicPosting, objForum.EnableForumsThreadStatus, objForum.EnableForumsRating, objForum.ForumLink, objForum.ForumBehavior, objForum.AllowPolls, objForum.EnableRSS, objForum.EmailAddress, objForum.EmailFriendlyFrom, objForum.NotifyByDefault, objForum.EmailStatusChange, objForum.EmailServer, objForum.EmailUser, objForum.EmailPass, objForum.EmailEnableSSL, objForum.EmailAuth, objForum.EmailPort, objForum.EnableSitemap, objForum.SitemapPriority)

			' update forum permissions
			If Not objForum.ForumPermissions Is Nothing Then
				Dim objForumPermissionController As New ForumPermissionController
				Dim objCurrentForumPermissions As ForumPermissionCollection
				objCurrentForumPermissions = objForumPermissionController.GetForumPermissionsCollection(objForum.ForumID)
				If Not objCurrentForumPermissions.CompareTo(objForum.ForumPermissions) Then
					objForumPermissionController.DeleteForumPermissionsByForumID(objForum.ForumID)
					Dim objForumPermission As ForumPermissionInfo
					For Each objForumPermission In objForum.ForumPermissions
						objForumPermission.ForumID = objForum.ForumID
						If objForumPermission.AllowAccess Then
							objForumPermissionController.AddForumPermission(objForumPermission)
						End If
					Next
				End If
			End If
		End Sub

		''' <summary>
		''' Deletes a forum from the data store.
		''' </summary>
		''' <param name="GroupID"></param>
		''' <param name="ForumID"></param>
		''' <remarks></remarks>
		Friend Sub ForumDelete(ByVal ParentID As Integer, ByVal GroupID As Integer, ByVal ForumID As Integer, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ForumDelete(ForumID, GroupID)
		End Sub

		''' <summary>
		''' Updates the sort order for a forum in the data store.
		''' </summary>
		''' <param name="ParentID"></param>
		''' <param name="GroupID"></param>
		''' <param name="ForumId"></param>
		''' <param name="MoveUp"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Friend Sub ForumSortOrderUpdate(ByVal ParentID As Integer, ByVal GroupID As Integer, ByVal ForumID As Integer, ByVal MoveUp As Boolean, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ForumSortOrderUpdate(GroupID, ForumID, MoveUp)
			' need to update cache here
			Forum.Components.Utilities.Caching.UpdateForumCache(ForumID, GroupID, ModuleID, ParentID)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Retrieves a single forum entity from the data store. 
		''' </summary>
		''' <param name="ForumId">The ForumID, pk, we are retrieving.</param>
		''' <returns>A single instance of the forum info object for the specified ForumID.</returns>
		''' <remarks></remarks>
		Private Function GetForum(ByVal ForumId As Integer) As ForumInfo
			Return CBO.FillObject(Of ForumInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ForumGet(ForumId))
		End Function

		''' <summary>
		''' Used to get all forums for a moduleID. Permissions are not considered here, meaning all forums are returned regardless of the users ability to see them or not. 
		''' </summary>
		''' <param name="ModuleID">The ModuleID of the module for which to return all forums.</param>
		''' <returns>Arraylist of all forums available for a ModuleID instance.</returns>
		''' <remarks>ONLY USE IN ADMIN SETTINGS!!!</remarks>
		Private Function GetForumByModuleID(ByVal ModuleID As Integer) As List(Of ForumInfo)
			Return CBO.FillCollection(Of ForumInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ForumsGetByModuleID(ModuleID))
		End Function

#End Region

	End Class

End Namespace