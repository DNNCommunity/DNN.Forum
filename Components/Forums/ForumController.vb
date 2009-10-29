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

#Region "ForumController"

	''' <summary>
	''' Used to communicate with the data store about Forum specific items. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	7/13/2005	Created
	''' </history>
	Public Class ForumController

#Region "Private Members"

		Private Const ForumInfoCacheKeyPrefix As String = "ForumGetAll"
		Private Const ForumInfoCacheTimeout As Integer = 20

#End Region

#Region "Public Methods"

		''' <summary>
		''' Checks cache to get all forums for a specified GroupID, if not in cache a collection is 
		''' retrieved from the database then hydrated one by one and placed into cache.
		''' </summary>
		''' <param name="GroupId">The GroupID of forums to populate.</param>
		''' <returns>An arraylist of forums belonging to a specific GroupID.</returns>
		''' <remarks></remarks>
		Public Function ForumGetAllByParentID(ByVal ParentID As Integer, ByVal GroupId As Integer, ByVal EnabledOnly As Boolean) As List(Of ForumInfo)
			Dim arrForums As List(Of ForumInfo)
			Dim strCacheKey As String = ForumInfoCacheKeyPrefix & CStr(ParentID) & CStr(GroupId)
			arrForums = CType(DataCache.GetCache(strCacheKey), List(Of ForumInfo))

			If arrForums Is Nothing Then
				'forum caching settings
				Dim timeOut As Int32 = ForumInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				arrForums = CBO.FillCollection(Of ForumInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ForumGetAllByParentID(ParentID, GroupId, EnabledOnly))

				'Cache Forum if timeout > 0 and Forum is not null
				If timeOut > 0 And arrForums IsNot Nothing Then
					DataCache.SetCache(strCacheKey, arrForums, TimeSpan.FromMinutes(timeOut))
				End If
			End If
			Return arrForums
		End Function

		''' <summary>
		''' Checks cache to get all forums for a specified GroupID, if not in cache a collection is 
		''' retrieved from the database then hydrated one by one and placed into cache.
		''' </summary>
		''' <param name="GroupId">The GroupID of forums to populate.</param>
		''' <returns>An arraylist of forums belonging to a specific GroupID.</returns>
		''' <remarks></remarks>
		Public Function ForumGetAll(ByVal GroupId As Integer) As List(Of ForumInfo)
			Dim arrForums As List(Of ForumInfo)
			Dim strCacheKey As String = ForumInfoCacheKeyPrefix & CStr(GroupId)
			arrForums = CType(DataCache.GetCache(strCacheKey), List(Of ForumInfo))

			If arrForums Is Nothing Then
				'forum caching settings
				Dim timeOut As Int32 = ForumInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				arrForums = CBO.FillCollection(Of ForumInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ForumGetAll(GroupId))

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
		Public Sub ClearCache_ForumGetAll(ByVal ParentID As Integer, ByVal GroupID As Integer)
			Dim strCacheKey As String = ForumInfoCacheKeyPrefix & CStr(ParentID) & CStr(GroupID)
			Dim strCacheKey2 As String = ForumInfoCacheKeyPrefix & CStr(GroupID)
			DataCache.RemoveCache(strCacheKey)
			DataCache.RemoveCache(strCacheKey2)
		End Sub

		''' <summary>
		''' Calls the Custom Hydrator to hydrate a specific ForumID instance.
		''' </summary>
		''' <param name="ForumId">The ForumID of the forum to hydrate.</param>
		''' <returns>A single instance of the forum info object for the specified ForumID.</returns>
		''' <remarks></remarks>
		Public Function GetForum(ByVal ForumId As Integer) As ForumInfo
			Dim dr As IDataReader = DotNetNuke.Modules.Forum.DataProvider.Instance().ForumGet(ForumId)
			Try
				Return FillForumInfo(dr, True, True)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try
		End Function

		''' <summary>
		''' Used to get all forums for a moduleID. Permissions are not considered here, meaning all forums are returned regardless of the users ability to see them or not. 
		''' </summary>
		''' <param name="ModuleID">The ModuleID of the module for which to return all forums.</param>
		''' <returns>Arraylist of all forums available for a ModuleID instance.</returns>
		''' <remarks>ONLY USE IN ADMIN SETTINGS!!!</remarks>
		Public Function GetForumByModuleID(ByVal ModuleID As Integer) As List(Of ForumInfo)
			Return CBO.FillCollection(Of ForumInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ForumsGetByModuleID(ModuleID))
		End Function

		''' <summary>
		''' Adds a new forum to the data store. 
		''' </summary>
		''' <param name="objForum">The ForumInfo object to add to the data store.</param>
		''' <returns></returns>
		''' <remarks>If anything is changed here, consider chaging ForumPreConfig.vb method for adding default "General" forum.</remarks>
		Public Function ForumAdd(ByVal objForum As ForumInfo) As Integer
			Dim ForumId As Integer = DotNetNuke.Modules.Forum.DataProvider.Instance().ForumAdd(objForum.GroupID, objForum.IsActive, objForum.ParentId, objForum.Name, objForum.Description, objForum.IsModerated, objForum.EnablePostStatistics, objForum.ForumType, objForum.IsIntegrated, objForum.IntegratedModuleID, objForum.PublicView, objForum.CreatedByUser, objForum.PublicPosting, objForum.EnableForumsThreadStatus, objForum.EnableForumsRating, objForum.ForumLink, objForum.ForumBehavior, objForum.AllowPolls, objForum.EnableRSS, objForum.EmailAddress, objForum.EmailFriendlyFrom, objForum.NotifyByDefault, objForum.EmailStatusChange, objForum.EmailServer, objForum.EmailUser, objForum.EmailPass, objForum.EmailEnableSSL, objForum.EmailAuth, objForum.EmailPort)

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

			ClearCache_ForumGetAll(objForum.ParentId, objForum.GroupID)

			Return ForumId
		End Function

		''' <summary>
		''' Updates an existing forum in the data store. 
		''' </summary>
		''' <param name="objForum">The ForumInfo object being updated in the data store.</param>
		''' <remarks></remarks>
		Public Sub ForumUpdate(ByVal objForum As ForumInfo, ByVal PreviousParentID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ForumUpdate(objForum.GroupID, objForum.ForumID, objForum.IsActive, objForum.ParentId, objForum.Name, objForum.Description, objForum.IsModerated, objForum.EnablePostStatistics, objForum.ForumType, objForum.IsIntegrated, objForum.IntegratedModuleID, objForum.PublicView, objForum.UpdatedByUser, objForum.PublicPosting, objForum.EnableForumsThreadStatus, objForum.EnableForumsRating, objForum.ForumLink, objForum.ForumBehavior, objForum.AllowPolls, objForum.EnableRSS, objForum.EmailAddress, objForum.EmailFriendlyFrom, objForum.NotifyByDefault, objForum.EmailStatusChange, objForum.EmailServer, objForum.EmailUser, objForum.EmailPass, objForum.EmailEnableSSL, objForum.EmailAuth, objForum.EmailPort)

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

			ClearCache_ForumGetAll(PreviousParentID, objForum.GroupID)
			If PreviousParentID <> objForum.ParentId Then
				ClearCache_ForumGetAll(objForum.ParentId, objForum.GroupID)
			End If
		End Sub

		''' <summary>
		''' Deletes a forum from the data store.
		''' </summary>
		''' <param name="GroupID"></param>
		''' <param name="ForumID"></param>
		''' <remarks></remarks>
		Public Sub ForumDelete(ByVal ParentID As Integer, ByVal GroupID As Integer, ByVal ForumID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ForumDelete(ForumID, GroupID)
			ClearCache_ForumGetAll(ParentID, GroupID)
		End Sub

		''' <summary>
		''' Updates the sort order for a forum in the data store.
		''' </summary>
		''' <param name="GroupID"></param>
		''' <param name="ForumId"></param>
		''' <param name="MoveUp"></param>
		''' <remarks></remarks>
		Public Sub ForumSortOrderUpdate(ByVal ParentID As Integer, ByVal GroupID As Integer, ByVal ForumId As Integer, ByVal MoveUp As Boolean)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ForumSortOrderUpdate(GroupID, ForumId, MoveUp)
			ClearCache_ForumGetAll(ParentID, GroupID)
		End Sub

#End Region

#Region "Private Hydration Methods"

		''' <summary>
		''' The Custom Hydrator function which is used for performance reasons. (This is faster than core CBO)
		''' </summary>
		''' <param name="dr">The datareader populated by the ForumID we are about to hydrate.</param>
		''' <param name="CheckForOpenDataReader">Determines if the function should check for open datareaders.</param>
		''' <param name="IncludePermissions">Determines if the permissions collection should be populated.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function FillForumInfo(ByVal dr As IDataReader, ByVal CheckForOpenDataReader As Boolean, ByVal IncludePermissions As Boolean) As ForumInfo
			Dim objForumInfo As New ForumInfo
			Dim objForumPermissionController As New DotNetNuke.Modules.Forum.ForumPermissionController
			' read datareader
			Dim [Continue] As Boolean = True

			If CheckForOpenDataReader Then
				[Continue] = False
				If dr.Read Then
					[Continue] = True
				End If
			End If
			If [Continue] Then
				Try
					objForumInfo.CreatedByUser = Convert.ToInt32(Null.SetNull(dr("CreatedByUser"), objForumInfo.CreatedByUser))
				Catch
				End Try
				Try
					objForumInfo.CreatedDate = Convert.ToDateTime(Null.SetNull(dr("CreatedDate"), objForumInfo.CreatedDate))
				Catch
				End Try
				Try
					objForumInfo.Description = Convert.ToString(Null.SetNull(dr("Description"), objForumInfo.Description))
				Catch
				End Try
				Try
					objForumInfo.EnablePostStatistics = Convert.ToBoolean(Null.SetNull(dr("EnablePostStatistics"), objForumInfo.EnablePostStatistics))
				Catch
				End Try
				Try
					objForumInfo.ForumID = Convert.ToInt32(Null.SetNull(dr("ForumID"), objForumInfo.ForumID))
				Catch
				End Try

				Try
					objForumInfo.ForumType = Convert.ToInt32(Null.SetNull(dr("ForumType"), objForumInfo.ForumType))
				Catch
				End Try
				Try
					objForumInfo.GroupID = Convert.ToInt32(Null.SetNull(dr("GroupID"), objForumInfo.GroupID))
				Catch
				End Try
				Try
					objForumInfo.IsIntegrated = Convert.ToBoolean(Null.SetNull(dr("IsIntegrated"), objForumInfo.IsIntegrated))
				Catch
				End Try
				Try
					objForumInfo.IntegratedModuleID = Convert.ToInt32(Null.SetNull(dr("IntegratedModuleID"), objForumInfo.IntegratedModuleID))
				Catch
				End Try
				Try
					objForumInfo.IsActive = Convert.ToBoolean(Null.SetNull(dr("IsActive"), objForumInfo.IsActive))
				Catch
				End Try
				Try
					objForumInfo.ModuleID = Convert.ToInt32(Null.SetNull(dr("ModuleID"), objForumInfo.ModuleID))
				Catch
				End Try
				Try
					objForumInfo.Name = Convert.ToString(Null.SetNull(dr("Name"), objForumInfo.Name))
				Catch
				End Try
				Try
					objForumInfo.MostRecentPostAuthorID = Convert.ToInt32(Null.SetNull(dr("MostRecentPostAuthorID"), objForumInfo.MostRecentPostAuthorID))
				Catch
				End Try
				Try
					objForumInfo.MostRecentPostDate = Convert.ToDateTime(Null.SetNull(dr("MostRecentPostDate"), objForumInfo.MostRecentPostDate))
				Catch
				End Try
				Try
					objForumInfo.MostRecentPostID = Convert.ToInt32(Null.SetNull(dr("MostRecentPostID"), objForumInfo.MostRecentPostID))
				Catch
				End Try
				Try
					objForumInfo.MostRecentThreadID = Convert.ToInt32(Null.SetNull(dr("MostRecentThreadID"), objForumInfo.MostRecentThreadID))
				Catch
				End Try
				Try
					objForumInfo.PostsToModerate = Convert.ToInt32(Null.SetNull(dr("PostsToModerate"), objForumInfo.PostsToModerate))
				Catch
				End Try
				Try
					objForumInfo.SortOrder = Convert.ToInt32(Null.SetNull(dr("SortOrder"), objForumInfo.SortOrder))
				Catch
				End Try
				Try
					objForumInfo.TotalPosts = Convert.ToInt32(Null.SetNull(dr("TotalPosts"), objForumInfo.TotalPosts))
				Catch
				End Try
				Try
					objForumInfo.TotalThreads = Convert.ToInt32(Null.SetNull(dr("TotalThreads"), objForumInfo.TotalThreads))
				Catch
				End Try
				Try
					objForumInfo.UpdatedByUser = Convert.ToInt32(Null.SetNull(dr("UpdatedByUser"), objForumInfo.UpdatedByUser))
				Catch
				End Try
				Try
					objForumInfo.UpdatedDate = Convert.ToDateTime(Null.SetNull(dr("UpdatedDate"), objForumInfo.UpdatedDate))
				Catch
				End Try
				Try
					objForumInfo.EnableForumsThreadStatus = Convert.ToBoolean(Null.SetNull(dr("EnableForumsThreadStatus"), objForumInfo.EnableForumsThreadStatus))
				Catch
				End Try
				Try
					objForumInfo.EnableForumsRating = Convert.ToBoolean(Null.SetNull(dr("EnableForumsRating"), objForumInfo.EnableForumsRating))
				Catch
				End Try
				Try
					objForumInfo.ForumLink = Convert.ToString(Null.SetNull(dr("ForumLink"), objForumInfo.ForumLink))
				Catch
				End Try
				Try
					objForumInfo.ForumBehavior = CType(Null.SetNull(dr("ForumBehavior"), objForumInfo.ForumBehavior), DotNetNuke.Modules.Forum.ForumBehavior)
				Catch
				End Try
				Try
					objForumInfo.MostRecentThreadPinned = Convert.ToBoolean(Null.SetNull(dr("MostRecentThreadPinned"), objForumInfo.MostRecentThreadPinned))
				Catch
				End Try
				Try
					objForumInfo.AllowPolls = Convert.ToBoolean(Null.SetNull(dr("AllowPolls"), objForumInfo.AllowPolls))
				Catch
				End Try
				Try
					objForumInfo.EnableRSS = Convert.ToBoolean(Null.SetNull(dr("EnableRSS"), objForumInfo.EnableRSS))
				Catch
				End Try
				'[skeel] subforums support
				Try
					objForumInfo.SubForums = Convert.ToInt32(Null.SetNull(dr("SubForums"), objForumInfo.SubForums))
				Catch
				End Try
				Try
					objForumInfo.ParentId = Convert.ToInt32(Null.SetNull(dr("ParentID"), objForumInfo.ParentId))
				Catch
				End Try
				' Email support
				Try
					objForumInfo.EmailAddress = Convert.ToString(Null.SetNull(dr("EmailAddress"), objForumInfo.EmailAddress))
				Catch
				End Try
				Try
					objForumInfo.EmailFriendlyFrom = Convert.ToString(Null.SetNull(dr("EmailFriendlyFrom"), objForumInfo.EmailFriendlyFrom))
				Catch
				End Try
				Try
					objForumInfo.NotifyByDefault = Convert.ToBoolean(Null.SetNull(dr("NotifyByDefault"), objForumInfo.NotifyByDefault))
				Catch
				End Try
				Try
					objForumInfo.EmailStatusChange = Convert.ToBoolean(Null.SetNull(dr("EmailStatusChange"), objForumInfo.EmailStatusChange))
				Catch
				End Try

				Try
					objForumInfo.EmailServer = Convert.ToString(Null.SetNull(dr("EmailServer"), objForumInfo.EmailServer))
				Catch
				End Try
				Try
					objForumInfo.EmailUser = Convert.ToString(Null.SetNull(dr("EmailUser"), objForumInfo.EmailUser))
				Catch
				End Try
				Try
					objForumInfo.EmailPass = Convert.ToString(Null.SetNull(dr("EmailPass"), objForumInfo.EmailPass))
				Catch
				End Try
				Try
					objForumInfo.EmailEnableSSL = Convert.ToBoolean(Null.SetNull(dr("EmailEnableSSL"), objForumInfo.EmailEnableSSL))
				Catch
				End Try
				Try
					objForumInfo.EmailAuth = Convert.ToInt32(Null.SetNull(dr("EmailAuth"), objForumInfo.EmailAuth))
				Catch
				End Try
				Try
					objForumInfo.EmailPort = Convert.ToInt32(Null.SetNull(dr("EmailPort"), objForumInfo.EmailPort))
				Catch
				End Try

				If IncludePermissions Then
					If Not objForumInfo Is Nothing Then
						' per forum perms
						Try
							objForumInfo.ForumPermissions = objForumPermissionController.GetForumPermissionsCollection(objForumInfo.ForumID)
						Catch
						End Try
					End If
				End If
			Else
				objForumInfo = Nothing
			End If
			Return objForumInfo
		End Function

#End Region

	End Class

#End Region

End Namespace