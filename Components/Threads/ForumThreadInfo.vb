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

#Region "ThreadInfo"

	''' <summary>
	''' A cached instance of a thread info object.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	8/31/2006	Documented
	''' </history>
	Public Class ThreadInfo

#Region "Private Members"

		Private Const ThreadInfoCacheKeyPrefix As String = "Forum_ThreadInfo"
		Private Const ThreadInfoCacheTimeout As Integer = 20
		Private _CreatedDate As DateTime
		Private _StartedByUserID As Integer
		Private _LastApprovedPostID As Integer
		Private _ForumID As Integer
		Private _Subject As String
		Private _Body As String
		Private _ThreadID As Integer
		Private _Replies As Integer
		Private _Views As Integer
		Private _ObjectID As Integer
		Private _IsPinned As Boolean
		Private _PinnedDate As DateTime
		Private _IsClosed As Boolean
		Private _Rating As Double
		Private _RatingCount As Integer
		Private _NextThreadID As Integer
		Private _PreviousThreadID As Integer
		Private _ThreadStatus As ThreadStatus
		Private _AnswerPostID As Integer = -1
		Private _PollID As Integer = -1
		Private _TotalRecords As Integer = 0
		'CP - Not implemented
		Private _ThreadIconID As Integer = 0

#End Region

#Region "Constructors"

		Public Sub New()
		End Sub

		Private Sub New(ByVal ThreadID As Integer)
		End Sub

#End Region

#Region "Public Caching Methods"

		''' <summary>
		''' This attempts to load from cache first, if not found loads into cache
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <returns>ThreadInfo object</returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	9/1/2006	Created
		''' </history>
		Public Shared Function GetThreadInfo(ByVal ThreadID As Integer) As ThreadInfo
			Dim strCacheKey As String = ThreadInfoCacheKeyPrefix & CStr(ThreadID)
			Dim objThread As ThreadInfo = CType(DataCache.GetCache(strCacheKey), ThreadInfo)

			If objThread Is Nothing Then
				'thread caching settings
				Dim timeOut As Int32 = ThreadInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

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
		Public Shared Sub ResetThreadInfo(ByVal ThreadID As Integer)
			Dim strCacheKey As String = ThreadInfoCacheKeyPrefix & ThreadID.ToString
			DataCache.RemoveCache(strCacheKey)
		End Sub

#End Region

#Region "Public Properties"

#Region "Portal Specific ReadOnly Cached References"

		''' <summary>
		''' The user who started the thread
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property StartedByUser() As ForumUser
			Get
				Return ForumUserController.GetForumUser(StartedByUserID, False, ModuleID, PortalID)
			End Get
		End Property

		''' <summary>
		''' the user who last posted to this thread (approved)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastApprovedUser() As ForumUser
			Get
				Return ForumUserController.GetForumUser(LastApprovedPost.Author.UserID, False, ModuleID, PortalID)
			End Get
		End Property

#End Region

#Region "Module Specific ReadOnly Cached References"

		''' <summary>
		''' The forum containing this thread
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This is the actual forum, not the parent forum (when using sub-forums).</remarks>
		Public ReadOnly Property HostForum() As ForumInfo
			Get
				Dim cntForum As New ForumController
				Return cntForum.GetForumInfoCache(ForumID)
			End Get
		End Property

		''' <summary>
		''' The moduleID of the forum containing this thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ModuleID() As Integer
			Get
				Return HostForum.ModuleID
			End Get
		End Property

		''' <summary>
		''' The PortalID of the forum containing this thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PortalID() As Integer
			Get
				Dim _portalSettings As DotNetNuke.Entities.Portals.PortalSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings
				Return _portalSettings.PortalId
			End Get
		End Property

#End Region

#Region "Thread Specific ReadOnly Cached Properties"

		''' <summary>
		''' The forum that contains the thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ParentForum() As ForumInfo
			Get
				Dim objForum As ForumInfo = New ForumInfo

				If ForumID > 0 Then
					Dim cntForum As New ForumController
					objForum = cntForum.GetForumInfoCache(ForumID)
				Else
					objForum.ModuleID = ModuleID
					objForum.ForumID = ForumID
				End If

				Return objForum
			End Get
		End Property

		''' <summary>
		''' The tooltip to load based on the threads rating
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property RatingText() As String
			Get
				If RatingCount = 0 Then
					'Return "No rating"
					Return Localization.GetString("RatingNoRating.Text", ParentForum.ParentGroup.objConfig.SharedResourceFile)
				Else
					'Return String.Format("{0} out of 10 stars - based on {1} rate(s)", Rating, RatingCount)
					Return String.Format(Localization.GetString("RatingHaveRating.Text", ParentForum.ParentGroup.objConfig.SharedResourceFile), Rating, RatingCount)
				End If
			End Get
		End Property

		''' <summary>
		''' The thread status image to load based on the threads thread status
		''' </summary>
		''' <value></value>
		''' <returns>The image including extension that represents the thread status graphically.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property StatusImage() As String
			Get
				Dim strImage As String = String.Empty
				Select Case ThreadStatus
					Case ThreadStatus.Answered
						strImage = "status_answered." & ParentForum.ParentGroup.objConfig.ImageExtension
					Case ThreadStatus.Unanswered
						strImage = "status_unanswered." & ParentForum.ParentGroup.objConfig.ImageExtension
					Case ThreadStatus.Info
						strImage = "status_info." & ParentForum.ParentGroup.objConfig.ImageExtension
					Case ThreadStatus.Poll
						strImage = "status_poll." & ParentForum.ParentGroup.objConfig.ImageExtension
					Case Else
						strImage = "status_spacer.gif"
				End Select
				Return strImage
			End Get
		End Property

		''' <summary>
		''' The thread status image's tooltip based on the threads status
		''' </summary>
		''' <value></value>
		''' <returns>The localized text used for the image alt tag and title.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property StatusText() As String
			Get
				Dim strText As String = String.Empty
				Select Case ThreadStatus
					Case ThreadStatus.Answered
						strText = Localization.GetString("StatusAnswered.Text", ParentForum.ParentGroup.objConfig.SharedResourceFile)
					Case ThreadStatus.Unanswered
						strText = Localization.GetString("StatusUnanswered.Text", ParentForum.ParentGroup.objConfig.SharedResourceFile)
					Case ThreadStatus.Info
						strText = Localization.GetString("StatusInfo.Text", ParentForum.ParentGroup.objConfig.SharedResourceFile)
					Case ThreadStatus.Poll
						strText = Localization.GetString("StatusPoll.Text", ParentForum.ParentGroup.objConfig.SharedResourceFile)
					Case Else
						strText = String.Empty
				End Select
				Return strText
			End Get
		End Property

		''' <summary>
		''' Total number of replies + 1 = Total Posts for a thread.
		''' Logic in sprocs/add/update/mod approve handles IsApproved only in counting.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property TotalPosts() As Integer
			Get
				Return _Replies + 1
			End Get
		End Property

		''' <summary>
		''' Determines if a thread is popular or not based on module configuration.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property IsPopular() As Boolean
			Get
				Return (TotalPosts > ParentForum.ParentGroup.objConfig.PopularThreadReply) OrElse (Views > ParentForum.ParentGroup.objConfig.PopularThreadView)
			End Get
		End Property

		''' <summary>
		''' The last approved post info for a single thread instance.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastApprovedPost() As PostInfo
			Get
				Return PostInfo.GetPostInfo(LastApprovedPostID, PortalID)
			End Get
		End Property

		''' <summary>
		''' Truncated last post body used for Title on links
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastPostShortBody() As String
			Get
				Dim strBody As String = LastApprovedPost.Body
				Dim strTrimedBody As String = String.Empty
				strTrimedBody = Utilities.ForumUtils.FormatToolTip(Utilities.ForumUtils.TrimString(strBody, 100))
				If ParentForum.ParentGroup.objConfig.EnableBadWordFilter Then
					strTrimedBody = Utilities.ForumUtils.FormatProhibitedWord(strTrimedBody, LastApprovedPost.CreatedDate, PortalID)
				End If
				Return strTrimedBody
			End Get
		End Property

#End Region

#Region "Populated from Threads table and Cached as part of this info object"

		' Started by info never changes and the started by info is just the original post info so this is saved at time of new thread creation in db.

		''' <summary>
		''' Date thread was created (based on original post info)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedDate() As DateTime
			Get
				Return _CreatedDate
			End Get
			Set(ByVal Value As DateTime)
				_CreatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The userID of who started the thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property StartedByUserID() As Integer
			Get
				Return _StartedByUserID
			End Get
			Set(ByVal Value As Integer)
				_StartedByUserID = Value
			End Set
		End Property

		''' <summary>
		''' The last approved PostID in this thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LastApprovedPostID() As Integer
			Get
				Return _LastApprovedPostID
			End Get
			Set(ByVal Value As Integer)
				_LastApprovedPostID = Value
			End Set
		End Property

		''' <summary>
		''' The forumID this thread belongs to.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumID() As Integer
			Get
				Return _ForumID
			End Get
			Set(ByVal Value As Integer)
				_ForumID = Value
			End Set
		End Property

		''' <summary>
		''' The subject of a thread is the subject of the original post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Subject() As String
			Get
				Return _Subject
			End Get
			Set(ByVal Value As String)
				_Subject = Value
			End Set
		End Property

		''' <summary>
		''' The body of the original post is the body of the thread, for starters. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Body() As String
			Get
				Return _Body
			End Get
			Set(ByVal Value As String)
				_Body = Value
			End Set
		End Property

		''' <summary>
		''' ThreadID = PostID of original post in thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ThreadID() As Integer
			Get
				Return _ThreadID
			End Get
			Set(ByVal Value As Integer)
				_ThreadID = Value
			End Set
		End Property

		''' <summary>
		''' Number of replies to the original post of the thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Replies() As Integer
			Get
				Return _Replies
			End Get
			Set(ByVal Value As Integer)
				_Replies = Value
			End Set
		End Property

		''' <summary>
		''' Number of times the thread has been viewed. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Views() As Integer
			Get
				Return _Views
			End Get
			Set(ByVal Value As Integer)
				_Views = Value
			End Set
		End Property

		''' <summary>
		''' Reserved for 3rd party module integration.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ObjectID() As Integer
			Get
				Return _ObjectID
			End Get
			Set(ByVal Value As Integer)
				_ObjectID = Value
			End Set
		End Property

		''' <summary>
		''' Determines if the thread is pinned or not. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsPinned() As Boolean
			Get
				Return _IsPinned
			End Get
			Set(ByVal Value As Boolean)
				_IsPinned = Value
			End Set
		End Property

		''' <summary>
		''' The date the thread was pinned (if applicable)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PinnedDate() As DateTime
			Get
				Return _PinnedDate
			End Get
			Set(ByVal Value As DateTime)
				_PinnedDate = Value
			End Set
		End Property

		''' <summary>
		''' If the thread is locked from further replies or not. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsClosed() As Boolean
			Get
				Return _IsClosed
			End Get
			Set(ByVal Value As Boolean)
				_IsClosed = Value
			End Set
		End Property

		''' <summary>
		''' The overall rating of the thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Rating() As Double
			Get
				Return _Rating
			End Get
			Set(ByVal Value As Double)
				_Rating = Value
			End Set
		End Property

		''' <summary>
		''' The number of times users have rated this thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property RatingCount() As Integer
			Get
				Return _RatingCount
			End Get
			Set(ByVal Value As Integer)
				_RatingCount = Value
			End Set
		End Property

		''' <summary>
		''' Used to determine which thread is next to be viewed (in order)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property NextThreadID() As Integer
			Get
				Return _NextThreadID
			End Get
			Set(ByVal Value As Integer)
				_NextThreadID = Value
			End Set
		End Property

		''' <summary>
		''' Used to determine which thread is previous to be viewed (in order)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PreviousThreadID() As Integer
			Get
				Return _PreviousThreadID
			End Get
			Set(ByVal Value As Integer)
				_PreviousThreadID = Value
			End Set
		End Property

		''' <summary>
		''' The thread status of the thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ThreadStatus() As ThreadStatus
			Get
				Return _ThreadStatus
			End Get
			Set(ByVal Value As ThreadStatus)
				_ThreadStatus = Value
			End Set
		End Property

		''' <summary>
		''' The postID marked as the answer for the thread (if applicable)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>0 = Not Answered</remarks>
		Public Property AnswerPostID() As Integer
			Get
				Return _AnswerPostID
			End Get
			Set(ByVal Value As Integer)
				_AnswerPostID = Value
			End Set
		End Property

		''' <summary>
		''' The Poll associated with the thread.
		''' </summary>
		''' <value></value>
		''' <returns>An integer representing the poll associated with the thread.</returns>
		''' <remarks></remarks>
		Public Property PollID() As Integer
			Get
				Return _PollID
			End Get
			Set(ByVal Value As Integer)
				_PollID = Value
			End Set
		End Property

		''' <summary>
		''' The icon associated with the thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Not Implemented</remarks>
		Public Property ThreadIconID() As Integer
			Get
				Return _ThreadIconID
			End Get
			Set(ByVal Value As Integer)
				_ThreadIconID = Value
			End Set
		End Property

		Public Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal Value As Integer)
				_TotalRecords = Value
			End Set
		End Property

#End Region

#End Region

	End Class

#End Region

End Namespace