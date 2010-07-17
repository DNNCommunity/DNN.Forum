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

	''' <summary>
	''' Communicates with the Forum_Posts table in the data store. Note that some items in the data store also interact with the Forum_Threads table as well. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class PostController

#Region "Private Members"

		Private Const PostCacheKeyPrefix As String = Constants.CACHE_KEY_PREFIX + "POST_"

#End Region

#Region "Public Methods"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="Descending"></param>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function PostGetAll(ByVal ThreadID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal Descending As Boolean, ByVal PortalID As Integer) As List(Of PostInfo)
			Return CBO.FillCollection(Of PostInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().PostGetAll(ThreadID, PageIndex, PageSize, Descending, PortalID))
		End Function

		''' <summary>
		''' Gets the post info object, first checks for it in cache
		''' </summary>
		''' <param name="PostID"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Public Function GetPostInfo(ByVal PostID As Integer, ByVal PortalID As Integer) As PostInfo
			Dim strCacheKey As String = PostCacheKeyPrefix & CStr(PostID)
			Dim objPost As PostInfo = CType(DataCache.GetCache(strCacheKey), PostInfo)

			If objPost Is Nothing Then
				'post caching settings
				Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)
				objPost = PostGet(PostID, PortalID)

				'Cache Post if timeout > 0 and Post is not null
				If timeOut > 0 And objPost IsNot Nothing Then
					DataCache.SetCache(strCacheKey, objPost, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return objPost
		End Function

		''' <summary>
		''' Resets the post info object in cahce to nothing
		''' </summary>
		''' <param name="PostID"></param>
		''' <remarks>
		''' </remarks>
		Friend Shared Sub ResetPostItemCache(ByVal PostID As Integer)
			Dim strCacheKey As String = PostCacheKeyPrefix & CStr(PostID)
			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' This is used in splitting and deleting threads. Returns a list of all posts in a thread ordering by time - descending. 
		''' </summary>
		''' <param name="ThreadID">The ThreadID to retrieve all the posts for.</param>
		''' <returns></returns>
		''' <remarks>This must always sort by CreatedDate DESC (in the sproc) so we are delete posts with newest ones being deleted first.</remarks>
		Public Function PostGetAllForThread(ByVal ThreadID As Integer) As List(Of PostInfo)
			Return CBO.FillCollection(Of PostInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().PostGetEntireThread(ThreadID))
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function PostGet(ByVal PostID As Integer, ByVal PortalID As Integer) As PostInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().PostGet(PostID, PortalID), GetType(PostInfo)), PostInfo)
		End Function

		''' <summary>
		''' Post delete removes a single post from the database and updates the cache. It also updates user post count as well as the forum/thread related statistics (ie. last post). 
		''' </summary>
		''' <param name="PostID">The post PK that is going to be deleted.</param>
		''' <param name="ModeratorID">The ModuleID that contains the post that will be deleted.</param>
		''' <param name="Notes">Notes that will be written to the data store for auditing purposes.</param>
		''' <param name="PortalID">The PortalID of the post that is going to be deleted, necessary for user post count updates.</param>
		''' <param name="GroupID">The GroupID of the post that is going to be deleted, necessary for updating group statistics and for data verification reasons.</param>
		''' <remarks>Never handle email sends from here. Also, the post delete sproc handles related attachment deletes in the data store.</remarks>
		Friend Sub PostDelete(ByVal PostID As Integer, ByVal ModeratorID As Integer, ByVal Notes As String, ByVal PortalID As Integer, ByVal GroupID As Integer, ByVal DeleteThread As Boolean, ByVal ParentID As Integer)
			Dim objPost As New PostInfo
			Dim objThread As New ThreadInfo
			Dim cntThread As New ThreadController

			objPost = PostGet(PostID, PortalID)
			objThread = cntThread.GetThread(objPost.PostID)

			DotNetNuke.Modules.Forum.DataProvider.Instance().PostDelete(PostID, ModeratorID, Notes, PortalID)

			' We need to delete the Content Item here
			Forum.Content.DeleteContentItem(objThread)
			ForumUserController.ResetForumUser(objPost.Author.UserID, PortalID)
		End Sub

		''' <summary>
		''' This sub will update the ParseInfo field for a specific post. ParseInfo should be the sum of
		''' all Enum PostParserInfo, that apply to the specific Post
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="ParseInfo"></param>
		''' <remarks></remarks>
		Friend Sub PostUpdateParseInfo(ByVal PostID As Integer, ByVal GroupID As Integer, ByVal ParseInfo As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().PostUpdateParseInfo(PostID, ParseInfo)
			Forum.Components.Utilities.Caching.UpdatePostCache(PostID)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend Function PostReportCheck(ByVal PostID As Integer, ByVal UserID As Integer) As Boolean
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().PostReportCheck(PostID, UserID)
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="oldThreadID"></param>
		''' <param name="newThreadID"></param>
		''' <param name="newForumID"></param>
		''' <param name="oldForumID"></param>
		''' <param name="ModID"></param>
		''' <param name="SortOrder"></param>
		''' <param name="Notes"></param>
		''' <param name="ParentID"></param>
		''' <remarks></remarks>
		Friend Sub PostMove(ByVal PostID As Integer, ByVal oldThreadID As Integer, ByVal newThreadID As Integer, ByVal newForumID As Integer, ByVal oldForumID As Integer, ByVal ModID As Integer, ByVal SortOrder As Integer, ByVal Notes As String, ByVal ParentID As Integer)
			Dim dr As IDataReader = Nothing
			Try
				Dim OldGroupID As Integer
				Dim NewGroupID As Integer
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().PostMove(PostID, oldThreadID, newThreadID, newForumID, oldForumID, ModID, SortOrder, Notes)
				While dr.Read
					OldGroupID = Convert.ToInt32(dr("OldGroupID"))
					NewGroupID = Convert.ToInt32(dr("NewGroupID"))
				End While
			Finally
				If dr IsNot Nothing Then
					dr.Close()
				End If
			End Try
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ParentPostID"></param>
		''' <param name="ForumID"></param>
		''' <param name="UserID"></param>
		''' <param name="RemoteAddr"></param>
		''' <param name="Subject"></param>
		''' <param name="Body"></param>
		''' <param name="IsPinned"></param>
		''' <param name="PinnedDate"></param>
		''' <param name="IsClosed"></param>
		''' <param name="PortalID"></param>
		''' <param name="PollID"></param>
		''' <param name="IsModerated"></param>
		''' <param name="GroupID"></param>
		''' <param name="ParentID"></param>
		''' <param name="ParseInfo"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend Function PostAdd(ByVal ParentPostID As Integer, ByVal ForumID As Integer, ByVal UserID As Integer, ByVal RemoteAddr As String, ByVal Subject As String, ByVal Body As String, ByVal IsPinned As Boolean, ByVal PinnedDate As DateTime, ByVal IsClosed As Boolean, ByVal PortalID As Integer, ByVal PollID As Integer, ByVal IsModerated As Boolean, ByVal GroupID As Integer, ByVal ParentID As Integer, ByVal ParseInfo As Integer) As Integer
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().PostAdd(ParentPostID, ForumID, UserID, RemoteAddr, Subject, Body, IsPinned, PinnedDate, IsClosed, PortalID, PollID, IsModerated, ParseInfo)
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <param name="PostID"></param>
		''' <param name="Subject"></param>
		''' <param name="Body"></param>
		''' <param name="IsPinned"></param>
		''' <param name="PinnedDate"></param>
		''' <param name="IsClosed"></param>
		''' <param name="UpdatedBy"></param>
		''' <param name="PortalID"></param>
		''' <param name="PollID"></param>
		''' <param name="ParentID"></param>
		''' <param name="ParseInfo"></param>
		''' <remarks></remarks>
		Friend Sub PostUpdate(ByVal ThreadID As Integer, ByVal PostID As Integer, ByVal Subject As String, ByVal Body As String, ByVal IsPinned As Boolean, ByVal PinnedDate As DateTime, ByVal IsClosed As Boolean, ByVal UpdatedBy As Integer, ByVal PortalID As Integer, ByVal PollID As Integer, ByVal ParentID As Integer, ByVal ParseInfo As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().PostUpdate(ThreadID, PostID, Subject, Body, IsPinned, PinnedDate, IsClosed, UpdatedBy, PortalID, PollID, ParseInfo)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PostID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend Function PostGetChildren(ByVal PostID As Integer) As ArrayList
			Return CBO.FillCollection(DotNetNuke.Modules.Forum.DataProvider.Instance().PostGetChildren(PostID), GetType(PostInfo))
		End Function

#End Region

	End Class

End Namespace