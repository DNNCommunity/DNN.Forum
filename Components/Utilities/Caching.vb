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

Namespace DotNetNuke.Modules.Forum.Components.Utilities

	Friend Class Caching

		''' <summary>
		''' This is only necessary in rare situations like when a post is udpated but has no impact on thread (ie. post reporting)
		''' </summary>
		''' <param name="PostID"></param>
		''' <remarks></remarks>
		Public Shared Sub UpdatePostCache(ByVal PostID As Integer)
			UpdatePostCache(PostID, -1, -1, -1, -1, -1)
		End Sub

		''' <summary>
		''' We use this when a post edit occurs, only when we need the actual post and the thread updated for this information.
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="ThreadID"></param>
		''' <remarks></remarks>
		Public Shared Sub UpdatePostCache(ByVal PostID As Integer, ByVal ThreadID As Integer)
			UpdatePostCache(PostID, ThreadID, -1, -1, -1, -1)
		End Sub

		''' <summary>
		''' Updates the post cache and anything else that needs updated, based on the passed in parameters. 
		''' </summary>
		''' <param name="PostID">The post we want to reset in cache.</param>
		''' <param name="ThreadID">The thread we want to reset in cache.</param>
		''' <param name="ForumID">This will be -1 when we are not updating the forum cache, will have the PK value otherwise.</param>
		''' <param name="GroupID"></param>
		''' <param name="ModuleID"></param>
		''' <remarks>Forum cache can only be updated if the thread cache is updated.</remarks>
		Public Shared Sub UpdatePostCache(ByVal PostID As Integer, ByVal ThreadID As Integer, ByVal ForumID As Integer, ByVal GroupID As Integer, ByVal ModuleID As Integer, ByVal ParentID As Integer)
			PostController.ResetPostItemCache(PostID)

			If ThreadID > 0 Then
				UpdateThreadCache(ThreadID, ForumID, GroupID, ModuleID, ParentID)
			End If
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ThreadID">The thread we want to reset in cache.</param>
		''' <remarks></remarks>
		Public Shared Sub UpdateThreadCache(ByVal ThreadID As Integer)
			UpdateThreadCache(ThreadID, -1, -1, -1, -1)
		End Sub

		''' <summary>
		''' Updates the thread cache and anything else that needs updated, based on the passed in parameters.
		''' </summary>
		''' <param name="ThreadID">The thread we want to reset in cache.</param>
		''' <param name="ForumID">This will be -1 when we are not updating the forum cache, will have the PK value otherwise.</param>
		''' <param name="GroupID"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Shared Sub UpdateThreadCache(ByVal ThreadID As Integer, ByVal ForumID As Integer, ByVal GroupID As Integer, ByVal ModuleID As Integer, ByVal ParentID As Integer)
			ThreadController.ResetThreadCache(ThreadID)

			If ForumID > 0 Then
				UpdateForumCache(ForumID, GroupID, ModuleID, ParentID)
			End If
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ForumID"></param>
		''' <param name="GroupID"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Shared Sub UpdateForumCache(ByVal ForumID As Integer, ByVal GroupID As Integer, ByVal ModuleID As Integer, ByVal ParentID As Integer)
			ForumController.ResetForumItemCache(ForumID)

			If ParentID > 0 Then
				ForumController.ResetForumItemCache(ParentID)
				ForumController.ResetChildForumsCache(ParentID, GroupID)
				ForumController.ResetChildForumsCache(0, GroupID)
			Else
				ForumController.ResetChildForumsCache(0, GroupID)
			End If

			If GroupID > 0 Then
				UpdateGroupCache(GroupID, ModuleID)
			End If
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="GroupID"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Shared Sub UpdateGroupCache(ByVal GroupID As Integer, ByVal ModuleID As Integer)
			GroupController.ResetGroupCacheItem(GroupID)
			ForumController.ResetParentForumsCache(GroupID)

			If ModuleID > 0 Then
				UpdateModuleLevelCache(ModuleID)
			End If
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Private Shared Sub UpdateModuleLevelCache(ByVal ModuleID As Integer)
			ForumController.ResetModuleForumsCache(ModuleID)
			GroupController.ResetModuleGroups(ModuleID)
		End Sub

		''' <summary>
		''' This will reset the cached user data. It should happen when a user has a post added or when they change their post display order (or any other settings from "My Settings"). 
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="PortalID"></param>
		''' <remarks></remarks>
		Public Shared Sub UpdateUserCache(ByVal UserID As Integer, ByVal PortalID As Integer)
			ForumUserController.ResetForumUser(UserID, PortalID)
		End Sub

	End Class

End Namespace