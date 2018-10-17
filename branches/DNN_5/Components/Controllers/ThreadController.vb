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

Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Forum.Library

Namespace DotNetNuke.Modules.Forum

    ''' <summary>
    ''' The ThreadController class includes the option interfaces such as ISearchable, IUpgradeable
    ''' in addition to the standard Get, GetAll, Update, Delete items to hook into the DAL
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ThreadController
        Inherits ModuleSearchBase
        Implements IEmailQueueable

#Region "Private Members"

        Shared log As Instrumentation.DnnLogger = Instrumentation.DnnLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString())

        Private Const THREAD_KEY As String = Constants.CACHE_KEY_PREFIX + "THREAD_"
        Private Const RSS_KEY As String = Constants.CACHE_KEY_PREFIX + "RSS_"

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' This is used to return an eligible list of threads for the seo sitemap provider. It is not cached because SEO Sitemap generation should only occur once a day and caching would avoid instance updates if pushed from admin UI module. 
        ''' </summary>
        ''' <param name="PortalID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSitemapThreads(ByVal PortalID As Integer) As List(Of ThreadInfo)
            Return CBO.FillCollection(Of ThreadInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GetSitemapThreads(PortalID))
        End Function

#End Region

#Region "Friend Methods"

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
        Friend Function GetRSSFeed(ByVal ModuleID As Integer, ByVal ForumID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As String, ByVal PortalID As Integer, ByRef TotalRecords As Integer) As List(Of ThreadInfo)
            Dim strCacheKey As String = RSS_KEY & ForumID.ToString() & ModuleID.ToString()
            Dim objThreads As List(Of ThreadInfo) = CType(DataCache.GetCache(strCacheKey), List(Of ThreadInfo))

            If objThreads Is Nothing Then
                'thread caching settings
                Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

                objThreads = GetForumThreads(ModuleID, ForumID, PageSize, PageIndex, Filter, PortalID)

                'Cache Thread if timeout > 0 and Thread is not null
                If timeOut > 0 And objThreads IsNot Nothing Then
                    DataCache.SetCache(strCacheKey, objThreads, TimeSpan.FromMinutes(timeOut))
                End If
            End If

            Return objThreads
        End Function

        ''' <summary>
        ''' This attempts to load from cache first, if not found loads into cache
        ''' </summary>
        ''' <param name="ThreadID"></param>
        ''' <returns>ThreadInfo object</returns>
        ''' <remarks>
        ''' </remarks>
        Friend Function GetThread(ByVal ThreadID As Integer) As ThreadInfo
            Dim strCacheKey As String = THREAD_KEY & CStr(ThreadID)
            Dim objThread As ThreadInfo = CType(DataCache.GetCache(strCacheKey), ThreadInfo)

            If objThread Is Nothing Then
                'thread caching settings
                Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

                Dim ctlThread As New ThreadController
                objThread = ctlThread.ThreadGet(ThreadID)

                'Cache Thread if timeout > 0 and Thread is not null
                If timeOut > 0 And objThread IsNot Nothing Then
                    DataCache.SetCache(strCacheKey, objThread, TimeSpan.FromMinutes(timeOut))
                End If
            End If

            Return objThread
        End Function

        ''' <summary>
        ''' Resets the cached thread to nothing
        ''' </summary>
        ''' <param name="ThreadID"></param>
        ''' <remarks></remarks>
        Friend Shared Sub ResetThreadCache(ByVal ThreadID As Integer)
            Dim strCacheKey As String = THREAD_KEY & ThreadID.ToString
            DataCache.RemoveCache(strCacheKey)
        End Sub

        ''' <summary>
        ''' Gets all threads for a module (handles filtering and paging)
        ''' </summary>
        ''' <param name="ModuleId"></param>
        ''' <param name="PageSize"></param>
        ''' <param name="PageIndex"></param>
        ''' <param name="LoggedOnUserID"></param>
        ''' <returns></returns>
        ''' <remarks>Added by Skeel</remarks>
        Friend Function GetUnreadThreads(ByVal ModuleId As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal LoggedOnUserID As Integer) As List(Of ThreadInfo)
            Return CBO.FillCollection(Of ThreadInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGetUnread(ModuleId, PageSize, PageIndex, LoggedOnUserID))
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
        ''' <remarks>This is not meant to be cached because of filtering and pagesize.</remarks>
        Friend Function GetForumThreads(ByVal ModuleId As Integer, ByVal ForumID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal Filter As String, ByVal PortalID As Integer) As List(Of ThreadInfo)
            Return CBO.FillCollection(Of ThreadInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGetAll(ModuleId, ForumID, PageSize, PageIndex, Filter, PortalID))
        End Function

        ''' <summary>
        ''' Gets all threads for a single forum for a specific user in order to 'mark all as read'. 
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="ForumID"></param>
        ''' <returns></returns>
        ''' <remarks>This is only used for user Thread Reads, MarkAll in UserThreadController</remarks>
        Friend Function GetByForum(ByVal userID As Integer, ByVal ForumID As Integer) As List(Of ThreadInfo)
            Return CBO.FillCollection(Of ThreadInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGetByForum(userID, ForumID))
        End Function

        ''' <summary>
        ''' Deletes a thread (and the initial post which is the thread) but first deletes all posts in that thread individually. The Delete Thread sproc simply deletes any poll related data prior to thread deletion. 
        ''' </summary>
        ''' <param name="objThread"></param>
        ''' <param name="PortalID"></param>
        ''' <param name="Notes"></param>
        ''' <remarks>Never handle email sends from here. All posts are deleted 1 by 1 so that all statistics are easily updated minimizing the potential for error. 
        ''' </remarks>
        Friend Sub DeleteThread(ByVal objThread As ThreadInfo, ByVal PortalID As Integer, ByVal Notes As String)
            ' we need to get all the posts in the thread so each can be deleted properly (and thus decrease user post counts)
            Dim cntPost As New PostController
            Dim arrPost As New List(Of PostInfo)
            Dim objThreadPost As New PostInfo

            arrPost = cntPost.PostGetAllForThread(objThread.ThreadID)

            For Each objPost As PostInfo In arrPost
                ' we need to make sure we delete the threadid last (because of split and possibly move). 
                If Not objPost.PostID = objThread.ThreadID Then
                    cntPost.PostDelete(objPost.PostID, objPost.ModuleId, Notes, PortalID, objThread.ForumID, objThread.ModuleID, objThread.ThreadID, objPost.Author.UserID)
                Else
                    objThreadPost = objPost
                End If
            Next

            ' we deleted all posts in the thread but the threadid one
            If Not objThreadPost Is Nothing Then
                cntPost.PostDelete(objThreadPost.PostID, objThreadPost.ModuleId, Notes, PortalID, objThread.ForumID, objThread.ModuleID, objThread.ThreadID, objThreadPost.Author.UserID)
            End If

            ' We need to delete the Content Item here
            Forum.Content.DeleteContentItem(objThread)
            Forum.Components.Utilities.Caching.UpdatePostCache(objThread.ThreadID, objThread.ThreadID, objThread.ForumID, objThread.ContainingForum.GroupID, objThread.ModuleID, objThread.ContainingForum.ParentID)
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
        Friend Sub MoveThread(ByVal ThreadID As Integer, ByVal NewForumID As Integer, ByVal ModID As Integer, ByVal Notes As String, ByVal ParentID As Integer)
            DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadMove(ThreadID, NewForumID, ModID, Notes)
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
        ''' <param name="ModuleID"></param>
        ''' <remarks></remarks>
        Friend Sub SplitThread(ByVal PostID As Integer, ByVal ThreadID As Integer, ByVal NewForumID As Integer, ByVal ModeratorUserID As Integer, ByVal Subject As String, ByVal Notes As String, ByVal ParentID As Integer, ByVal ModuleID As Integer)
            DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadSplit(PostID, ThreadID, NewForumID, ModeratorUserID, Subject, Notes)
        End Sub

        ''' <summary>
        ''' Increases a threads view count by one
        ''' </summary>
        ''' <param name="ThreadId"></param>
        ''' <remarks>
        ''' </remarks>
        Friend Sub IncrementThreadViewCount(ByVal ThreadId As Integer)
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
        Friend Sub ChangeThreadStatus(ByVal ThreadId As Integer, ByVal UserID As Integer, ByVal ThreadStatus As Integer, ByVal AnswerPostID As Integer, ByVal ModeratorUserID As Integer, ByVal PortalID As Integer)
            DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadStatusChange(ThreadId, UserID, ThreadStatus, AnswerPostID)

            If ModeratorUserID > 0 Then
                ' Log moderator action.
                Dim cntModerate As New PostModerationController

                cntModerate.AddModeratorHistory(ThreadId, PortalID, UserID, "Thread Status Changed.", ModerateAction.ThreadStatusChange)
            End If

            ' NOTE: CP: COMEBACK: Eventually add email notifications here. 
        End Sub

        ''' <summary>
        ''' Updates a thread's contentitemid. 
        ''' </summary>
        ''' <param name="ThreadID">The thread we are going to update.</param>
        ''' <param name="ContentItemID">The content item id we are assigning to the thread.</param>
        ''' <remarks></remarks>
        Friend Sub UpdateThread(ByVal ThreadID As Integer, ByVal ContentItemID As Integer, ByVal SitemapInclude As Boolean)
            DotNetNuke.Modules.Forum.DataProvider.Instance().UpdateThread(ThreadID, ContentItemID, SitemapInclude)
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
        Friend Function ThreadGetUserRating(ByVal ThreadID As Integer, ByVal UserID As Integer) As Double
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
        Friend Sub ThreadRateAdd(ByVal ThreadId As Integer, ByVal UserID As Integer, ByVal Rate As Double)
            DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadRateAdd(ThreadId, UserID, Rate)
        End Sub

#End Region

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Gets a single thread from the data store.
        ''' </summary>
        ''' <param name="ThreadId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ThreadGet(ByVal ThreadId As Integer) As ThreadInfo
            Return CBO.FillObject(Of ThreadInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ThreadGet(ThreadId))
        End Function

#End Region

#Region "Optional Interfaces"

        ''' <summary>
        ''' This is called by the framework's indexing.  This is the only time it 
        ''' is called.  It gathers all posts added which are not private based on
        ''' the date of the post and the last date set of indexing.  This is what
        ''' exposes data to site search.
        ''' </summary>
        ''' <param name="moduleInfo"></param>
        ''' <param name="beginDate"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	2/3/2006	Created
        ''' </history>
        Public Overrides Function GetModifiedSearchDocuments(moduleInfo As ModuleInfo, beginDate As Date) As IList(Of Search.Entities.SearchDocument)
            log.Debug("Search Portal: " + moduleInfo.PortalID.ToString)
            log.Debug("Search from: " + beginDate.ToString)
            Dim retval As List(Of Search.Entities.SearchDocument) = New List(Of Search.Entities.SearchDocument)()
            Try

                Dim ThreadSearchCollection As List(Of ThreadSearchInfo) = CBO.FillCollection(Of ThreadSearchInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ISearchable(moduleInfo.ModuleID, beginDate))
                Dim thread As ThreadSearchInfo

                If ThreadSearchCollection IsNot Nothing Then
                    ' iterate through the posts and create a search item collection
                    For Each thread In ThreadSearchCollection
                        'log.Debug("Search found: " + thread.Subject)
                        Dim sitem As Search.Entities.SearchDocument = New Search.Entities.SearchDocument()
                        sitem.Description = HtmlUtils.ConvertToText(HttpUtility.HtmlDecode(thread.Body))
                        sitem.AuthorUserId = thread.CreatedByUser
                        sitem.Body = HtmlUtils.Shorten(HtmlUtils.ConvertToText(HttpUtility.HtmlDecode(thread.Body)), 100, "...")
                        sitem.IsActive = True
                        'sitem.Keywords = Nothing
                        'log.Debug("UpdateDate: " + thread.UpdatedDate.ToString)
                        sitem.ModifiedTimeUtc = thread.UpdatedDate.ToUniversalTime()
                        sitem.ModuleDefId = moduleInfo.ModuleDefID
                        sitem.ModuleId = moduleInfo.ModuleID
                        'sitem.NumericKeys
                        'sitem.Permissions 
                        sitem.PortalId = moduleInfo.PortalID
                        'sitem.QueryString = 
                        'sitem.RoleId 
                        'sitem.SearchTypeId 
                        sitem.TabId = moduleInfo.TabID
                        Dim cntThread As New ThreadController()
                        Dim objThread As ThreadInfo = cntThread.GetThread(thread.ThreadID)
                        Dim ters As List(Of String) = GetTags(moduleInfo.Terms)

                        For Each Term As Entities.Content.Taxonomy.Term In objThread.Terms
                            ters.Add(Term.Name)
                            'log.Debug("Added Term: " + Term.Name)
                        Next

                        sitem.Tags = ters
                        'log.Debug("TabPath: " + tinfo.TabPath)
                        sitem.Title = HttpUtility.HtmlDecode(thread.Subject)
                        sitem.UniqueKey = thread.PostID.ToString

                        sitem.Url = New Uri(Links.ContainerViewQueryLink(moduleInfo.PortalID, moduleInfo.TabID, thread.ForumID, thread.PostID, thread.Subject)).PathAndQuery
                        retval.Add(sitem)
                    Next
                End If
            Catch ex As System.InvalidCastException
                'There are cases when isearch does not return correctly so just catch & log
                log.Error("Search Portal: " + moduleInfo.PortalID.ToString, ex)
            End Try
            Return retval
        End Function

        Function GetTags(ByVal terms As List(Of Entities.Content.Taxonomy.Term)) As List(Of String)
            Dim tags As List(Of String) = New List(Of String)
            If terms IsNot Nothing AndAlso tags.Count > 0 Then
                For Each ter As Entities.Content.Taxonomy.Term In terms
                    tags.Add(ter.Name)
                    tags.AddRange(GetTags(ter.ChildTerms))
                Next
            End If
            Return tags
        End Function

        'Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems
        '    Dim objModules As New Entities.Modules.ModuleController

        '    ' get the date of the last index operation from module settings
        '    Dim StartDate As DateTime = Null.NullDate
        '    Dim settings As Hashtable = ModInfo.ModuleSettings
        '    If Not settings("LastIndexDate") Is Nothing Then
        '        Try
        '            Dim tempDate As Double
        '            tempDate = (CType(settings("LastIndexDate"), Double))
        '            StartDate = Utilities.ForumUtils.NumToDate(tempDate)
        '        Catch exc As Exception
        '            LogException(exc)
        '        End Try
        '    End If

        '    ' save the current date
        '    Dim LastIndexDate As DateTime = Now()

        '    ' get all posts since the last index date
        '    Dim SearchItemCollection As New Services.Search.SearchItemInfoCollection
        '    Dim ThreadSearchCollection As ArrayList = CBO.FillCollection(DotNetNuke.Modules.Forum.DataProvider.Instance().ISearchable(ModInfo.ModuleID, StartDate), GetType(ThreadSearchInfo))
        '    Dim thread As ThreadSearchInfo

        '    ' iterate through the posts and create a search item collection
        '    For Each thread In ThreadSearchCollection
        '        Dim threadBody As String = HttpUtility.HtmlDecode(thread.Body)
        '        Dim threadDescription As String = HtmlUtils.Shorten(threadBody, 100, "...")
        '        Dim SearchItem As Services.Search.SearchItemInfo = New Services.Search.SearchItemInfo(thread.Subject, threadDescription, thread.CreatedByUser, thread.CreatedDate, ModInfo.ModuleID, thread.PostID.ToString, threadBody, "forumid=" & thread.ForumID & "&postid=" & thread.PostID & "&scope=posts", 0)
        '        If Not SearchItem Is Nothing Then
        '            SearchItemCollection.Add(SearchItem)
        '        End If
        '    Next

        '    ' update the last index date
        '    objModules.UpdateModuleSetting(ModInfo.ModuleID, "LastIndexDate", Utilities.ForumUtils.DateToNum(LastIndexDate).ToString)

        '    ' return the search item collection
        '    Return SearchItemCollection
        'End Function

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
#End Region

    End Class

End Namespace