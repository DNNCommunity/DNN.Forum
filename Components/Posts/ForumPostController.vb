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

#Region "PostController"

	''' <summary>
	''' Communicates with the Forum_Posts table in the data store. Note that some items in the data store also interact with the Forum_Threads table as well. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	7/13/2005	Created
	''' </history>
	Public Class PostController

#Region "Public Methods"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <param name="ThreadPage"></param>
		''' <param name="PostsPerPage"></param>
		''' <param name="TreeView"></param>
		''' <param name="Descending"></param>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function PostGetAll(ByVal ThreadID As Integer, ByVal ThreadPage As Integer, ByVal PostsPerPage As Integer, ByVal TreeView As Boolean, ByVal Descending As Boolean, ByVal PortalID As Integer) As List(Of PostInfo)
			Return CBO.FillCollection(Of PostInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().PostGetAll(ThreadID, ThreadPage, PostsPerPage, TreeView, Descending, PortalID))
		End Function

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
			Dim cntPost As New PostController
			Dim cntForum As New ForumController
			Dim objPost As New PostInfo

			' Get the post info now so we can clear cache later.
			objPost = cntPost.PostGet(PostID, PortalID)

			DotNetNuke.Modules.Forum.DataProvider.Instance().PostDelete(PostID, ModeratorID, Notes, PortalID)

			' Clear cache items affected by this delete
			ForumUserController.ResetForumUser(objPost.Author.UserID, PortalID)
			' We use the groupid passed in since it is possible the thread is gone if a lookup were to occur
			cntForum.ClearCache_ForumGetAll(ParentID, GroupID)
			' [Skeel] 8/1/2009
			' As can not have a relation between posts and files, as postid -1 is used for attachments
			' still not related to a specific post, so at this point, we need to delete all attachments 
			' related to this post. We do this by updating the AttachmentInfo and set the PostID to -2
			' The actual deletion process will then be handled later in AttachmentControl.ascx while the
			' file is unlocked!
			' CP - NOTE: I commented below out, we are not actually deleting files so who cares about a file lock? 
			'Dim cntAttachment As New AttachmentController
			'Dim lstAttachments As List(Of AttachmentInfo) = cntAttachment.GetAllByPostID(PostID)
			'If lstAttachments.Count > 0 Then
			'    For Each objFile As AttachmentInfo In lstAttachments
			'        objFile.PostID = -2
			'        cntAttachment.Update(objFile)
			'    Next
			'End If
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
			PostInfo.ResetPostInfo(PostID)
			Dim f As New ForumController
			f.ClearCache_ForumGetAll(PostID, GroupID)
		End Sub

		'#Region "Post Reporting"

		'		Public Sub PostReport(ByVal PostID As Integer, ByVal UserID As Integer, ByVal Reason As String)
		'			Dim GroupID As Integer = DotNetNuke.Modules.Forum.DataProvider.Instance().PostReport(PostID, UserID, Reason)
		'		End Sub

		'		Public Function PostGetReported(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByRef TotalRecords As Integer) As List(Of PostInfo)
		'			Return CBO.FillCollection(Of PostInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().PostGetReported(PortalID, PageIndex, PageSize))
		'		End Function

		'#End Region

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend Function PostReportCheck(ByVal PostID As Integer, ByVal UserID As Integer) As Boolean
			Dim HasReported As Boolean = DotNetNuke.Modules.Forum.DataProvider.Instance().PostReportCheck(PostID, UserID)
			Return HasReported
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
			Dim f As New ForumController
			Dim dr As IDataReader = Nothing
			Try
				Dim OldGroupID As Integer
				Dim NewGroupID As Integer
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().PostMove(PostID, oldThreadID, newThreadID, newForumID, oldForumID, ModID, SortOrder, Notes)
				While dr.Read
					OldGroupID = Convert.ToInt32(dr("OldGroupID"))
					NewGroupID = Convert.ToInt32(dr("NewGroupID"))
					f.ClearCache_ForumGetAll(ParentID, OldGroupID)
					f.ClearCache_ForumGetAll(ParentID, NewGroupID)
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
		''' <param name="Notify"></param>
		''' <param name="Subject"></param>
		''' <param name="Body"></param>
		''' <param name="IsPinned"></param>
		''' <param name="PinnedDate"></param>
		''' <param name="IsClosed"></param>
		''' <param name="ObjectID"></param>
		''' <param name="FileAttachmentURL"></param>
		''' <param name="PortalID"></param>
		''' <param name="ThreadIconID"></param>
		''' <param name="PollID"></param>
		''' <param name="IsModerated"></param>
		''' <param name="GroupID"></param>
		''' <param name="ParentID"></param>
		''' <param name="ParseInfo"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend Function PostAdd(ByVal ParentPostID As Integer, ByVal ForumID As Integer, ByVal UserID As Integer, ByVal RemoteAddr As String, ByVal Notify As Boolean, ByVal Subject As String, ByVal Body As String, ByVal IsPinned As Boolean, ByVal PinnedDate As DateTime, ByVal IsClosed As Boolean, ByVal ObjectID As Integer, ByVal FileAttachmentURL As String, ByVal PortalID As Integer, ByVal ThreadIconID As Integer, ByVal PollID As Integer, ByVal IsModerated As Boolean, ByVal GroupID As Integer, ByVal ParentID As Integer, ByVal ParseInfo As Integer) As Integer
			Dim PostID As Integer
			PostID = DotNetNuke.Modules.Forum.DataProvider.Instance().PostAdd(ParentPostID, ForumID, UserID, RemoteAddr, Notify, Subject, Body, IsPinned, PinnedDate, IsClosed, ObjectID, FileAttachmentURL, PortalID, ThreadIconID, PollID, IsModerated, ParseInfo)

			Dim f As New ForumController
			f.ClearCache_ForumGetAll(ParentID, GroupID)

			Return PostID
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <param name="PostID"></param>
		''' <param name="Notify"></param>
		''' <param name="Subject"></param>
		''' <param name="Body"></param>
		''' <param name="IsPinned"></param>
		''' <param name="PinnedDate"></param>
		''' <param name="IsClosed"></param>
		''' <param name="UpdatedBy"></param>
		''' <param name="FileAttachmentURL"></param>
		''' <param name="PortalID"></param>
		''' <param name="ThreadIconID"></param>
		''' <param name="PollID"></param>
		''' <param name="ParentID"></param>
		''' <param name="ParseInfo"></param>
		''' <remarks></remarks>
		Friend Sub PostUpdate(ByVal ThreadID As Integer, ByVal PostID As Integer, ByVal Notify As Boolean, ByVal Subject As String, ByVal Body As String, ByVal IsPinned As Boolean, ByVal PinnedDate As DateTime, ByVal IsClosed As Boolean, ByVal UpdatedBy As Integer, ByVal FileAttachmentURL As String, ByVal PortalID As Integer, ByVal ThreadIconID As Integer, ByVal PollID As Integer, ByVal ParentID As Integer, ByVal ParseInfo As Integer)
			Dim GroupID As Integer = DotNetNuke.Modules.Forum.DataProvider.Instance().PostUpdate(ThreadID, PostID, Notify, Subject, Body, IsPinned, PinnedDate, IsClosed, UpdatedBy, FileAttachmentURL, PortalID, ThreadIconID, PollID, ParseInfo)
			Dim f As New ForumController
			f.ClearCache_ForumGetAll(ParentID, GroupID)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="FlatView"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend Function PostSortOrderGet(ByVal PostID As Integer, ByVal FlatView As Boolean) As Integer
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().PostSortOrderGet(PostID, FlatView)
		End Function

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

#End Region

End Namespace