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

Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' A cached instance of a thread info object.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class ThreadInfo
		Inherits DotNetNuke.Entities.Content.ContentItem
		Implements IHydratable

#Region "Private Members"

		Private _Subject As String
		Private _Body As String
		Private _CreatedDate As DateTime
		Private _StartedByUserID As Integer
		Private _ThreadID As Integer
		Private _ForumID As Integer
		Private _Views As Integer
		Private _LastApprovedPostID As Integer
		Private _Replies As Integer
		Private _IsPinned As Boolean
		Private _PinnedDate As DateTime
		Private _IsClosed As Boolean
		Private _ThreadStatus As ThreadStatus
		Private _AnswerPostID As Integer
		Private _AnswerUserID As Integer
		Private _PollID As Integer
		Private _AnswerDate As DateTime
		Private _SitemapInclude As Boolean
		Private _PreviousThreadID As Integer
		Private _NextThreadID As Integer
		Private _RatingCount As Integer
		Private _Rating As Double
		Private _TotalRecords As Integer

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property objConfig() As Configuration
			Get
				Return Configuration.GetForumConfig(ContainingForum.ModuleID)
			End Get
		End Property

#End Region

#Region "Constructors"

		Public Sub New()
			'initialize the properties that
			'can be null in the database

			_RatingCount = -1
			_TotalRecords = 0
			_NextThreadID = -1
			_PreviousThreadID = -1
			_PollID = -1
		End Sub

#End Region

#Region "Public ReadOnly Properties"

		''' <summary>
		''' The user who started the thread
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property StartedByUser() As ForumUserInfo
			Get
				Dim cntForumUser As New ForumUserController
				Return cntForumUser.GetForumUser(StartedByUserID, False, ModuleID, PortalID)
			End Get
		End Property

		''' <summary>
		''' the user who last posted to this thread (approved)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastApprovedUser() As ForumUserInfo
			Get
				If LastApprovedPost IsNot Nothing Then
					Dim cntForumUser As New ForumUserController
					Return cntForumUser.GetForumUser(LastApprovedPost.Author.UserID, False, ContainingForum.ModuleID, PortalID)
				Else
					Return Nothing
				End If			
			End Get
		End Property

		''' <summary>
		''' The forum containing this thread
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This is the actual forum, not the parent forum (when using sub-forums).</remarks>
		Public ReadOnly Property ContainingForum() As ForumInfo
			Get
				Dim cntForum As New ForumController
				Return cntForum.GetForumItemCache(ForumID)
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

		''' <summary>
		''' The tooltip to load based on the threads rating
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property RatingText() As String
			Get
				If RatingCount < 1 Then
					'Return "No rating"
					Return Localization.GetString("RatingNoRating.Text", objConfig.SharedResourceFile)
				Else
					'Return String.Format("{0} out of 10 stars - based on {1} rate(s)", Rating, RatingCount)
					Return String.Format(Localization.GetString("RatingHaveRating.Text", objConfig.SharedResourceFile), Rating, RatingCount)
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
						strImage = "status_answered." & objConfig.ImageExtension
					Case ThreadStatus.Unanswered
						strImage = "status_unanswered." & objConfig.ImageExtension
					Case ThreadStatus.Info
						strImage = "status_info." & objConfig.ImageExtension
					Case ThreadStatus.Poll
						strImage = "status_poll." & objConfig.ImageExtension
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
						strText = Localization.GetString("StatusAnswered.Text", objConfig.SharedResourceFile)
					Case ThreadStatus.Unanswered
						strText = Localization.GetString("StatusUnanswered.Text", objConfig.SharedResourceFile)
					Case ThreadStatus.Info
						strText = Localization.GetString("StatusInfo.Text", objConfig.SharedResourceFile)
					Case ThreadStatus.Poll
						strText = Localization.GetString("StatusPoll.Text", objConfig.SharedResourceFile)
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
				If LastApprovedPost.CreatedDate > DateAdd(DateInterval.Day, -objConfig.PopularThreadDays, Date.Now) Then
					Return (TotalPosts > objConfig.PopularThreadReply) OrElse (Views > objConfig.PopularThreadView)
				Else
					Return False
				End If
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
				Dim cntPost As New PostController()
				Return cntPost.GetPostInfo(LastApprovedPostID, PortalID)
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
				If objConfig.EnableBadWordFilter Then
					strTrimedBody = Utilities.ForumUtils.FormatProhibitedWord(strTrimedBody, LastApprovedPost.CreatedDate, PortalID)
				End If
				Return strTrimedBody
			End Get
		End Property

#End Region

#Region "Public Properties"

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
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property AnswerUserID() As Integer
			Get
				Return _AnswerUserID
			End Get
			Set(ByVal Value As Integer)
				_AnswerUserID = Value
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
		''' The date the thread was marked as answered. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Currently we are not populating this in any of the get sprocs. This was added quickly just for DNN.COM moderation purposes. Thread_Get, Thread_GetAll, Thread_GetByForum, Thread_GetUnread = All need updated.</remarks>
		Public Property AnswerDate() As DateTime
			Get
				Return _AnswerDate
			End Get
			Set(ByVal Value As DateTime)
				_AnswerDate = Value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property SitemapInclude() As Boolean
			Get
				Return _SitemapInclude
			End Get
			Set(ByVal Value As Boolean)
				_SitemapInclude = Value
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
		''' Total number of records retrieved with the query. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal Value As Integer)
				_TotalRecords = Value
			End Set
		End Property

#End Region

#Region "IHydratable Implementation"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="dr"></param>
		''' <remarks></remarks>
		Public Overrides Sub Fill(ByVal dr As System.Data.IDataReader)
			'Call the base classes fill method to populate base class proeprties
			MyBase.FillInternal(dr)

			Subject = Null.SetNullString(dr("Subject"))
			Body = Null.SetNullString(dr("Body"))
			CreatedDate = Null.SetNullDateTime(dr("CreatedDate"))
			StartedByUserID = Null.SetNullInteger(dr("StartedByUserID"))
			ThreadID = Null.SetNullInteger(dr("ThreadID"))
			ForumID = Null.SetNullInteger(dr("ForumID"))
			Views = Null.SetNullInteger(dr("Views"))
			LastApprovedPostID = Null.SetNullInteger(dr("LastApprovedPostID"))
			Replies = Null.SetNullInteger(dr("Replies"))
			IsPinned = Null.SetNullBoolean(dr("IsPinned"))
			PinnedDate = Null.SetNullDateTime(dr("PinnedDate"))
			IsClosed = Null.SetNullBoolean(dr("IsClosed"))
			ThreadStatus = CType(Null.SetNull(dr("ThreadStatus"), ThreadStatus), ThreadStatus)
			AnswerPostID = Null.SetNullInteger(dr("AnswerPostID"))
			AnswerUserID = Null.SetNullInteger(dr("AnswerUserID"))
			AnswerDate = Null.SetNullDateTime(dr("AnswerDate"))
			PollID = Null.SetNullInteger(dr("PollID"))
			SitemapInclude = Null.SetNullBoolean(dr("SitemapInclude"))
			PreviousThreadID = Null.SetNullInteger(dr("PreviousThreadID"))
			NextThreadID = Null.SetNullInteger(dr("NextThreadID"))
			RatingCount = Null.SetNullInteger(dr("RatingCount"))
			Rating = Convert.ToDouble(Null.SetNull(dr("Rating"), Rating))
			TotalRecords = Null.SetNullInteger(dr("TotalRecords"))
		End Sub

		''' <summary>
		''' Gets/Sets the KeyID, which is the ThreadID.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Property KeyID() As Integer
			Get
				Return ThreadID
			End Get
			Set(ByVal value As Integer)
				ThreadID = value
			End Set
		End Property

#End Region

	End Class

End Namespace