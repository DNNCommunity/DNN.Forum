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

Imports DotNetNuke.Services.Sitemap
Imports DotNetNuke.Entities.Portals

Namespace DotNetNuke.Modules.Forum

#Region "ThreadController"

	''' <summary>
	''' The ThreadController class includes the option interfaces such as ISearchable, IUpgradeable
	''' in addition to the standard Get, GetAll, Update, Delete items to hook into the DAL
	''' </summary>
	''' <remarks></remarks>
	Public Class ThreadController
		Inherits SitemapProvider
		Implements DotNetNuke.Entities.Modules.ISearchable
		Implements IEmailQueueable

#Region "Private Members"

		Private Const ThreadListCacheKeyPrefix As String = "Forum_ThreadList"
		Private Const ThreadListCacheTimeout As Integer = 20

#End Region

#Region "Public Methods"

#Region "Caching Methods"

		''' <summary>
		''' Used for RSS and for Latest Post Extension. 
		''' If a forum's threads have not been cached, they will be retrieved and then added to cache. 
		''' </summary>
		''' <param name="ModuleId"></param>
		''' <param name="ForumId"></param>
		''' <param name="PageSize"></param>
		''' <param name="PageIndex"></param>
		''' <param name="Filter"></param>
		''' <param name="PortalID"></param>
		''' <param name="TotalRecords"></param>
		''' <returns>A cached list of threads.</returns>
		''' <remarks>Other aspects do not use this because of pagesize/pageindex variance. (also reason we use moduleid for cache key here)</remarks>
		Public Function ThreadListGetCached(ByVal ModuleID As Integer, ByVal ForumID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As String, ByVal PortalID As Integer, ByRef TotalRecords As Integer) As List(Of ThreadInfo)
			Dim strCacheKey As String = ThreadListCacheKeyPrefix & ForumID.ToString() & ModuleID.ToString()
			Dim objThreads As List(Of ThreadInfo) = CType(DataCache.GetCache(strCacheKey), List(Of ThreadInfo))

			If objThreads Is Nothing Then
				'thread caching settings
				Dim timeOut As Int32 = ThreadListCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				objThreads = ThreadGetAll(ModuleID, ForumID, PageSize, PageIndex, Filter, PortalID)

				'Cache Thread if timeout > 0 and Thread is not null
				If timeOut > 0 And objThreads IsNot Nothing Then
					DataCache.SetCache(strCacheKey, objThreads, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return objThreads
		End Function

		''' <summary>
		''' Clears the cache of the ThreadList (necessary for post add/edit/delete, thread move/split)
		''' </summary>
		''' <param name="ForumId"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Shared Sub ResetThreadListCached(ByVal ForumID As Integer, ByVal ModuleID As Integer)
			Dim strCacheKey As String = ThreadListCacheKeyPrefix & ForumID.ToString() & ModuleID.ToString()
			DataCache.RemoveCache(strCacheKey)
		End Sub

#End Region

		''' <summary>
		''' Gets all threads for a module (handles filtering and paging)
		''' </summary>
		''' <param name="ModuleId"></param>
		''' <param name="PageSize"></param>
		''' <param name="PageIndex"></param>
		''' <param name="LoggedOnUserID"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Function ThreadGetUnread(ByVal ModuleId As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal LoggedOnUserID As Integer) As List(Of ThreadInfo)
			Dim objThreads As New List(Of ThreadInfo)
			Dim dr As IDataReader = Nothing
			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGetUnread(ModuleId, PageSize, PageIndex, LoggedOnUserID)
				While dr.Read
					Dim objThreadInfo As ThreadInfo = FillThreadInfo(dr)
					objThreads.Add(objThreadInfo)
				End While
				dr.NextResult()
				'If dr.Read Then
				'    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
				'End If

			Catch ex As Exception
				LogException(ex)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objThreads
		End Function

		''' <summary>
		''' Gets all threads for a module (handles filtering and paging)
		''' </summary>
		''' <param name="ModuleId"></param>
		''' <param name="ForumId"></param>
		''' <param name="PageSize"></param>
		''' <param name="PageIndex"></param>
		''' <param name="Filter"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ThreadGetAll(ByVal ModuleId As Integer, ByVal ForumId As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As String, ByVal PortalID As Integer) As List(Of ThreadInfo)
			Dim objThreads As New List(Of ThreadInfo)
			Dim dr As IDataReader = Nothing
			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGetAll(ModuleId, ForumId, PageSize, PageIndex, Filter, PortalID)
				While dr.Read
					Dim objThreadInfo As ThreadInfo = FillThreadInfo(dr)
					objThreads.Add(objThreadInfo)
				End While
				dr.NextResult()
				'If dr.Read Then
				'    TotalRecords = Convert.ToInt32(dr("TotalRecords"))
				'End If

			Catch ex As Exception
				LogException(ex)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objThreads
		End Function

		''' <summary>
		''' Gets all threads for a single forum for a specific user
		''' </summary>
		''' <param name="userID"></param>
		''' <param name="ForumID"></param>
		''' <returns></returns>
		''' <remarks>This is only used for user Thread Reads, MarkAll in UserThreadController</remarks>
		Public Function GetByForum(ByVal userID As Integer, ByVal ForumID As Integer) As ArrayList
			Return CBO.FillCollection(DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGetByForum(userID, ForumID), GetType(ThreadInfo))
		End Function

		''' <summary>
		''' Gets a specific thread
		''' </summary>
		''' <param name="ThreadId"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ThreadGet(ByVal ThreadId As Integer) As ThreadInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGet(ThreadId), GetType(ThreadInfo)), ThreadInfo)
		End Function

		''' <summary>
		''' Deletes a thread (and the initial post which is the thread) but first deletes all posts in that thread individually. The Delete Thread sproc simply deletes any poll related data prior to thread deletion. 
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <remarks>Never handle email sends from here. All posts are deleted 1 by 1 so that all statistics are easily updated minimizing the potential for error. 
		''' </remarks>
		Public Sub ThreadDelete(ByVal ThreadID As Integer, ByVal PortalID As Integer, ByVal Notes As String)
			' we need to get all the posts in the thread so each can be deleted properly (and thus decrease user post counts)
			Dim cntPost As New PostController
			Dim arrPost As New List(Of PostInfo)
			Dim objThreadPost As New PostInfo

			arrPost = cntPost.PostGetAllForThread(ThreadID)

			For Each objPost As PostInfo In arrPost
				' we need to make sure we delete the threadid last (because of split and possibly move). 
				If Not objPost.PostID = ThreadID Then
					cntPost.PostDelete(objPost.PostID, objPost.ModuleId, Notes, PortalID, objPost.ParentThread.HostForum.GroupID, True, objPost.ParentThread.HostForum.ParentId)
				Else
					objThreadPost = objPost
				End If
			Next

			' we deleted all posts in the thread but the threadid one
			If Not objThreadPost Is Nothing Then
				' not sure how this would happen, but just to be safe
				cntPost.PostDelete(objThreadPost.PostID, objThreadPost.ModuleId, Notes, PortalID, objThreadPost.ParentThread.HostForum.GroupID, True, objThreadPost.ParentThread.HostForum.ParentId)
			End If
		End Sub

		''' <summary>
		''' Moves a thread from one forum to another forum (ID)
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <param name="NewForumID"></param>
		''' <param name="ModID"></param>
		''' <param name="Notes"></param>
		''' <param name="ParentID"></param>
		''' <remarks>
		''' </remarks>
		Public Sub ThreadMove(ByVal ThreadID As Integer, ByVal NewForumID As Integer, ByVal ModID As Integer, ByVal Notes As String, ByVal ParentID As Integer)
			Dim f As New ForumController
			Dim dr As IDataReader = Nothing

			Try
				Dim OldGroupID As Integer
				Dim NewGroupID As Integer
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadMove(ThreadID, NewForumID, ModID, Notes)
				While dr.Read
					OldGroupID = Convert.ToInt32(dr("OldGroupID"))
					NewGroupID = Convert.ToInt32(dr("NewGroupID"))
					f.ClearCache_ForumGetAll(ParentID, OldGroupID)
					f.ClearCache_ForumGetAll(ParentID, NewGroupID)
				End While
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try
		End Sub

		''' <summary>
		''' Splits an existing thread into two. This simply creates the new thread and removes the post from the old one. Any folliwing posts are handled by PostMove. 
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="ThreadID"></param>
		''' <param name="NewForumID"></param>
		''' <param name="ModeratorUserID">The UserID of the moderator deleting the post.</param>
		''' <param name="Subject"></param>
		''' <param name="Notes"></param>
		''' <param name="ParentID"></param>
		''' <remarks></remarks>
		Public Sub ThreadSplit(ByVal PostID As Integer, ByVal ThreadID As Integer, ByVal NewForumID As Integer, ByVal ModeratorUserID As Integer, ByVal Subject As String, ByVal Notes As String, ByVal ParentID As Integer)
			Dim f As New ForumController
			Dim dr As IDataReader = Nothing

			Try
				Dim OldGroupID As Integer
				Dim NewGroupID As Integer
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadSplit(PostID, ThreadID, NewForumID, ModeratorUserID, Subject, Notes)
				While dr.Read
					OldGroupID = Convert.ToInt32(dr("OldGroupID"))
					NewGroupID = Convert.ToInt32(dr("NewGroupID"))
					f.ClearCache_ForumGetAll(ParentID, OldGroupID)
					f.ClearCache_ForumGetAll(ParentID, NewGroupID)
				End While
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try
		End Sub

		''' <summary>
		''' Gets the number of threads in a forum
		''' </summary>
		''' <param name="ForumId"></param>
		''' <param name="Filter"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/3/2006	Created
		''' </history>
		Public Function ThreadGetCount(ByVal ForumId As Integer, ByVal Filter As String) As Integer
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGetCount(ForumId, Filter)
		End Function

		''' <summary>
		''' Increases a threads view count by one
		''' </summary>
		''' <param name="ThreadId"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/3/2006	Created
		''' </history>
		Public Sub ThreadViewsIncrement(ByVal ThreadId As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadViewsIncrement(ThreadId)
		End Sub

		''' <summary>
		''' Updates the status of a specific thread
		''' </summary>
		''' <param name="ThreadId"></param>
		''' <param name="UserID"></param>
		''' <param name="ThreadStatus"></param>
		''' <param name="AnswerPostID"></param>
		''' <param name="ModeratorUserID"></param>
		''' <param name="PortalID"></param>
		''' <remarks>We are not tracking moderator actions when setting poll for thread status type (regardless of what it was before). 
		''' </remarks>
		Public Sub ThreadStatusChange(ByVal ThreadId As Integer, ByVal UserID As Integer, ByVal ThreadStatus As Integer, ByVal AnswerPostID As Integer, ByVal ModeratorUserID As Integer, ByVal PortalID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadStatusChange(ThreadId, UserID, ThreadStatus, AnswerPostID)

			If ModeratorUserID > 0 Then
				' Log moderator action.
				Dim cntModerate As New ModerateController

				cntModerate.AddModeratorHistory(ThreadId, PortalID, UserID, "Thread Status Changed.", ModerateAction.ThreadStatusChange)
			End If

			' NOTE: CP: COMEBACK: Eventually add email notifications here. 
		End Sub

#Region "Rating"

		''' <summary>
		''' Gets a specific users rating for a specific Thread
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	9/23/2006	Created
		'''   [cpaterra] 11/15/2009 Updated
		''' </history>
		Public Function ThreadGetUserRating(ByVal ThreadID As Integer, ByVal UserID As Integer) As Double
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGetUserRating(ThreadID, UserID)
		End Function

		''' <summary>
		''' Adds a rating value for a user on a thread
		''' </summary>
		''' <param name="ThreadId"></param>
		''' <param name="UserID"></param>
		''' <param name="Rate"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/3/2006	Created
		'''   [cpaterra] 11/15/2009 Updated
		''' </history>
		Public Sub ThreadRateAdd(ByVal ThreadId As Integer, ByVal UserID As Integer, ByVal Rate As Double)
			DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadRateAdd(ThreadId, UserID, Rate)
		End Sub

#End Region

#End Region

#Region "Custom Hydrator"

		''' <summary>
		''' Hydrates the threadinfo object
		''' </summary>
		''' <param name="dr"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/3/2006	Created
		''' </history>
		Private Function FillThreadInfo(ByVal dr As IDataReader) As ThreadInfo
			Dim objThreadInfo As New ThreadInfo
			Try
				objThreadInfo.ForumID = Convert.ToInt32(Null.SetNull(dr("ForumID"), objThreadInfo.ForumID))
			Catch
			End Try
			Try
				objThreadInfo.Subject = Convert.ToString(Null.SetNull(dr("Subject"), objThreadInfo.Subject))
			Catch
			End Try
			Try
				objThreadInfo.Body = Convert.ToString(Null.SetNull(dr("Body"), objThreadInfo.Body))
			Catch
			End Try
			Try
				objThreadInfo.CreatedDate = Convert.ToDateTime(Null.SetNull(dr("CreatedDate"), objThreadInfo.CreatedDate))
			Catch
			End Try
			Try
				objThreadInfo.StartedByUserID = Convert.ToInt32(Null.SetNull(dr("StartedByUserID"), objThreadInfo.StartedByUserID))
			Catch
			End Try
			Try
				objThreadInfo.ThreadID = Convert.ToInt32(Null.SetNull(dr("ThreadID"), objThreadInfo.ThreadID))
			Catch
			End Try
			Try
				objThreadInfo.Replies = Convert.ToInt32(Null.SetNull(dr("Replies"), objThreadInfo.Replies))
			Catch
			End Try
			Try
				objThreadInfo.Views = Convert.ToInt32(Null.SetNull(dr("Views"), objThreadInfo.Views))
			Catch
			End Try
			Try
				objThreadInfo.LastApprovedPostID = Convert.ToInt32(Null.SetNull(dr("LastApprovedPostID"), objThreadInfo.LastApprovedPostID))
			Catch
			End Try
			Try
				objThreadInfo.ObjectID = Convert.ToInt32(Null.SetNull(dr("ObjectID"), objThreadInfo.ObjectID))
			Catch
			End Try
			Try
				objThreadInfo.IsPinned = Convert.ToBoolean(Null.SetNull(dr("IsPinned"), objThreadInfo.IsPinned))
			Catch
			End Try
			Try
				objThreadInfo.PinnedDate = Convert.ToDateTime(Null.SetNull(dr("PinnedDate"), objThreadInfo.PinnedDate))
			Catch
			End Try
			Try
				objThreadInfo.IsClosed = Convert.ToBoolean(Null.SetNull(dr("IsClosed"), objThreadInfo.IsClosed))
			Catch ex As Exception
			End Try
			Try
				objThreadInfo.Rating = Convert.ToDouble(Null.SetNull(dr("Rating"), objThreadInfo.Rating))
			Catch ex As Exception
			End Try
			Try
				objThreadInfo.RatingCount = Convert.ToInt32(Null.SetNull(dr("RatingCount"), objThreadInfo.RatingCount))
			Catch ex As Exception
			End Try
			Try
				objThreadInfo.NextThreadID = Convert.ToInt32(Null.SetNull(dr("NextThreadID"), objThreadInfo.NextThreadID))
			Catch ex As Exception
			End Try
			Try
				objThreadInfo.PreviousThreadID = Convert.ToInt32(Null.SetNull(dr("PreviousThreadID"), objThreadInfo.PreviousThreadID))
			Catch ex As Exception
			End Try
			Try
				objThreadInfo.ThreadStatus = CType(Null.SetNull(dr("ThreadStatus"), objThreadInfo.ThreadStatus), ThreadStatus)
			Catch ex As Exception
			End Try
			Try
				objThreadInfo.PollID = Convert.ToInt32(Null.SetNull(dr("PollID"), objThreadInfo.PollID))
			Catch ex As Exception
			End Try
			Try
				objThreadInfo.TotalRecords = Convert.ToInt32(Null.SetNull(dr("TotalRecords"), objThreadInfo.TotalRecords))
			Catch ex As Exception
			End Try

			Return objThreadInfo
		End Function

#End Region

#Region "Optional Interfaces"

		''' <summary>
		''' This is called by the framework's indexing.  This is the only time it 
		''' is called.  It gathers all posts added which are not private based on
		''' the date of the post and the last date set of indexing.  This is what
		''' exposes data to site search.
		''' </summary>
		''' <param name="ModInfo"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/3/2006	Created
		''' </history>
		Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems
			Dim objModules As New Entities.Modules.ModuleController

			' get the date of the last index operation from module settings
			Dim StartDate As DateTime = Null.NullDate
			Dim settings As Hashtable = objModules.GetModuleSettings(ModInfo.ModuleID)
			If Not settings("LastIndexDate") Is Nothing Then
				Try
					Dim tempDate As Double
					tempDate = (CType(settings("LastIndexDate"), Double))
					StartDate = Utilities.ForumUtils.NumToDate(tempDate)
				Catch exc As Exception
					LogException(exc)
				End Try
			End If

			' save the current date
			Dim LastIndexDate As DateTime = Now()

			' get all posts since the last index date
			Dim SearchItemCollection As New Services.Search.SearchItemInfoCollection
			Dim ThreadSearchCollection As ArrayList = CBO.FillCollection(DotNetNuke.Modules.Forum.DataProvider.Instance().ISearchable(ModInfo.ModuleID, StartDate), GetType(ThreadSearchInfo))
			Dim thread As ThreadSearchInfo

			' iterate through the posts and create a search item collection
			For Each thread In ThreadSearchCollection
				Dim threadBody As String = HttpUtility.HtmlDecode(thread.Body)
				Dim threadDescription As String = HtmlUtils.Shorten(threadBody, 100, "...")
				Dim SearchItem As Services.Search.SearchItemInfo = New Services.Search.SearchItemInfo(thread.Subject, threadDescription, thread.CreatedByUser, thread.CreatedDate, ModInfo.ModuleID, thread.PostID.ToString, threadBody, "forumid=" & thread.ForumID & "&postid=" & thread.PostID & "&scope=posts", 0)
				If Not SearchItem Is Nothing Then
					SearchItemCollection.Add(SearchItem)
				End If
			Next

			' update the last index date
			objModules.UpdateModuleSetting(ModInfo.ModuleID, "LastIndexDate", Utilities.ForumUtils.DateToNum(LastIndexDate).ToString)

			' return the search item collection
			Return SearchItemCollection
		End Function

		''' <summary>
		''' Calls the email queue task to queue up one email for mass distribution
		''' </summary>
		''' <param name="EmailFromAddress"></param>
		''' <param name="FromFriendlyName"></param>
		''' <param name="EmailPriority"></param>
		''' <param name="EmailHTMLBody"></param>
		''' <param name="EmailTextBody"></param>
		''' <param name="EmailSubject"></param>
		''' <param name="PortalID"></param>
		''' <param name="QueuePriority"></param>
		''' <param name="ModuleID"></param>
		''' <param name="EnableFriendlyToName"></param>
		''' <param name="DistroCall"></param>
		''' <param name="DistroIsSproc"></param>
		''' <param name="ScheduleStartDate"></param>
		''' <param name="PersonalizeEmail"></param>
		''' <param name="Attachment"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function QueueEmail(ByVal EmailFromAddress As String, ByVal FromFriendlyName As String, ByVal EmailPriority As Integer, ByVal EmailHTMLBody As String, ByVal EmailTextBody As String, ByVal EmailSubject As String, ByVal PortalID As Integer, ByVal QueuePriority As Integer, ByVal ModuleID As Integer, ByVal EnableFriendlyToName As Boolean, ByVal DistroCall As String, ByVal DistroIsSproc As Boolean, ByVal DistroParams As String, ByVal ScheduleStartDate As Date, ByVal PersonalizeEmail As Boolean, ByVal Attachment As String) As EmailQueueTaskInfo Implements IEmailQueueable.QueueEmail
			Dim EmailQueueTask As EmailQueueTaskInfo
			EmailQueueTask = New EmailQueueTaskInfo(EmailFromAddress, FromFriendlyName, EmailPriority, EmailHTMLBody, EmailTextBody, EmailSubject, PortalID, QueuePriority, ModuleID, EnableFriendlyToName, DistroCall, DistroIsSproc, DistroParams, ScheduleStartDate, PersonalizeEmail, Attachment)

			Return EmailQueueTask
		End Function

		Public Overrides Function GetUrls(ByVal portalId As Integer, ByVal ps As PortalSettings, ByVal version As String) As List(Of SitemapUrl)
			Dim blogUrl As SitemapUrl
			Dim urls As New List(Of SitemapUrl)

			'Dim blog As New EntryController
			'Dim entries As ArrayList = blog.ListAllEntriesByPortal(portalId, False, False)

			'For Each entry As EntryInfo In entries
			'	blogUrl = GetThreadUrls(entry, ps.PortalAlias.HTTPAlias)
			'	urls.Add(blogUrl)
			'Next
			Return urls
		End Function

		Private Function GetThreadUrls(ByVal objEntry As ThreadInfo, ByVal portalAlias As String) As SitemapUrl
			Dim pageUrl As New SitemapUrl
			'pageUrl.Url = objEntry.PermaLink
			pageUrl.Priority = 0.5
			'pageUrl.LastModified = objEntry.AddedDate
			pageUrl.ChangeFrequency = SitemapChangeFrequency.Weekly

			Return pageUrl
		End Function

#End Region

	End Class

#End Region

End Namespace