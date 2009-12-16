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

	''' <summary>
	''' Search Info is a 'hybrid' type class used to create a custom business object that represents merged results of a search from posts/threads tables.
	''' </summary>
	''' <remarks></remarks>
	Public Class SearchInfo

#Region "Private Members"

		Private mThreadID As Integer
		Private mSubject As String
		Private mCreatedDate As DateTime
		Private mForumID As Integer
		Private mReplies As Integer
		Private mViews As Integer
		Private mForumName As String
		Private mRecordCount As Integer ' Should not be moved from this class
		Private mIsPinned As Boolean
		Private mLastPostedPostID As Integer
		Private mThreadStatus As ThreadStatus
		Private mRatingCount As Integer
		Private mRating As Double
		Private mIsUnRead As Boolean

#End Region

#Region "Constructors"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region

#Region "ReadOnly Properties"

		''' <summary>
		''' The user who posted the last approved post of a thread.
		''' </summary>
		''' <value></value>
		''' <returns>A forum user who last had an approved post in a thread.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastApprovedUser() As ForumUser
			Get
				Return ForumUserController.GetForumUser(LastApprovedPost.UserID, False, ModuleID, PortalID)
			End Get
		End Property

		''' <summary>
		''' The last approved post info for a single thread instance.
		''' </summary>
		''' <value></value>
		''' <returns>A single Instance of the last approved post Info object.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastApprovedPost() As PostInfo
			Get
				Return PostInfo.GetPostInfo(LastPostedPostID, PortalID)
			End Get
		End Property

		''' <summary>
		''' The rating image to load based on the threads rating
		''' </summary>
		''' <value></value>
		''' <returns>The name of the image to load.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property RatingImage() As String
			Get
				Dim intRate As Integer = CInt(Rating)
				Return "stars_" & intRate.ToString & "." & Parent.ParentGroup.objConfig.ImageExtension
			End Get
		End Property

		''' <summary>
		''' The tooltip to load based on the threads rating
		''' </summary>
		''' <value></value>
		''' <returns>The text used for a tooltip for rating images.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property RatingText() As String
			Get
				If RatingCount = 0 Then
					'Return "No rating"
					Return Localization.GetString("RatingNoRating.Text", Parent.ParentGroup.objConfig.SharedResourceFile)
				Else
					'Return String.Format("{0} out of 5 stars - based on {1} rate(s)", Rating, RatingCount)
					Return String.Format(Localization.GetString("RatingHaveRating.Text", Parent.ParentGroup.objConfig.SharedResourceFile), Rating, RatingCount)
				End If
			End Get
		End Property

		''' <summary>
		''' The thread status image to load based on the threads thread status
		''' </summary>
		''' <value></value>
		''' <returns>The image name of the proper status image.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property StatusImage() As String
			Get
				Dim strImage As String = String.Empty
				Select Case ThreadStatus
					Case ThreadStatus.Answered
						strImage = "status_answered." & Parent.ParentGroup.objConfig.ImageExtension
					Case ThreadStatus.Unanswered
						strImage = "status_unanswered." & Parent.ParentGroup.objConfig.ImageExtension
					Case ThreadStatus.Info
						strImage = "status_info." & Parent.ParentGroup.objConfig.ImageExtension
					Case ThreadStatus.Poll
						strImage = "status_poll." & Parent.ParentGroup.objConfig.ImageExtension
					Case ThreadStatus.NotSet
						strImage = "status_spacer.gif"
				End Select
				Return strImage
			End Get
		End Property

		''' <summary>
		''' The thread status image's tooltip based on the threads status
		''' </summary>
		''' <value></value>
		''' <returns>The text to use for the status image tooltip.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property StatusText() As String
			Get
				Dim strText As String = String.Empty
				Select Case ThreadStatus
					Case ThreadStatus.Answered
						strText = Localization.GetString("StatusAnswered.Text", Parent.ParentGroup.objConfig.SharedResourceFile)
					Case ThreadStatus.Unanswered
						strText = Localization.GetString("StatusUnanswered.Text", Parent.ParentGroup.objConfig.SharedResourceFile)
					Case ThreadStatus.Info
						strText = Localization.GetString("StatusInfo.Text", Parent.ParentGroup.objConfig.SharedResourceFile)
					Case ThreadStatus.Poll
						strText = Localization.GetString("StatusPoll.Text", Parent.ParentGroup.objConfig.SharedResourceFile)
					Case ThreadStatus.NotSet
						strText = String.Empty
				End Select
				Return strText
			End Get
		End Property

		''' <summary>
		''' Total number of posts in a thread.
		''' </summary>
		''' <value></value>
		''' <returns>Number of posts in a thread.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property PostCount() As String
			Get
				Return CStr(Replies + 1)
			End Get
		End Property

		''' <summary>
		''' Truncated last post body used for Title on links
		''' </summary>
		''' <value></value>
		''' <returns>Shortened post body.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastPostShortBody() As String
			Get
				Dim strBody As String = LastApprovedPost.Body
				Dim strTrimedBody As String = String.Empty
				strTrimedBody = Utilities.ForumUtils.FormatToolTip(Utilities.ForumUtils.TrimString(strBody, 100))
				If Parent.ParentGroup.objConfig.EnableBadWordFilter Then
					strTrimedBody = Utilities.ForumUtils.FormatProhibitedWord(strTrimedBody, LastApprovedPost.CreatedDate, PortalID)
				End If
				Return strTrimedBody
			End Get
		End Property

#End Region

#Region "Properties"

		''' <summary>
		''' The ModuleID the search is being run from.
		''' </summary>
		''' <value></value>
		''' <returns>The moduleID the search is being run from.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ModuleID() As Integer
			Get
				Return Parent.ModuleID
			End Get
		End Property

		''' <summary>
		''' The PortalID being used when the search is being conducted.
		''' </summary>
		''' <value></value>
		''' <returns>The current PortalID.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property PortalID() As Integer
			Get
				Return Parent.ParentGroup.PortalID
			End Get
		End Property

		''' <summary>
		''' The Parent Forum info object for the ForumID which the search results returned.
		''' </summary>
		''' <value></value>
		''' <returns>The Forum's Parent Info object.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property Parent() As ForumInfo
			Get
				Dim cntForum As New ForumController
				Return cntForum.GetForumInfoCache(ForumID)
			End Get
		End Property

		''' <summary>
		''' The name of the forum the search results belongs to.
		''' </summary>
		''' <value></value>
		''' <returns>Name of the forum.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ForumName() As String
			Get
				Return Parent.Name
			End Get
		End Property

		''' <summary>
		''' The date the post was created. 
		''' </summary>
		''' <value></value>
		''' <returns>Date the post was created.</returns>
		''' <remarks></remarks>
		Public Property CreatedDate() As DateTime
			Get
				Return mCreatedDate
			End Get
			Set(ByVal Value As DateTime)
				mCreatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The ThreadID which the search result belongs to. 
		''' </summary>
		''' <value></value>
		''' <returns>ThreadID of the search result.</returns>
		''' <remarks></remarks>
		Public Property ThreadID() As Integer
			Get
				Return mThreadID
			End Get
			Set(ByVal Value As Integer)
				mThreadID = Value
			End Set
		End Property

		''' <summary>
		''' Total number of search results returned.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property RecordCount() As Integer
			Get
				Return mRecordCount
			End Get
			Set(ByVal Value As Integer)
				mRecordCount = Value
			End Set
		End Property

		''' <summary>
		''' The last approved PostID of the returned thread from the search results. 
		''' </summary>
		''' <value></value>
		''' <returns>Last approved PostID of the thread.</returns>
		''' <remarks></remarks>
		Public Property LastPostedPostID() As Integer
			Get
				Return mLastPostedPostID
			End Get
			Set(ByVal Value As Integer)
				mLastPostedPostID = Value
			End Set
		End Property

		''' <summary>
		''' The subject of the post from the search results returned.
		''' </summary>
		''' <value></value>
		''' <returns>Subject of the post.</returns>
		''' <remarks></remarks>
		Public Property Subject() As String
			Get
				Return mSubject
			End Get
			Set(ByVal Value As String)
				mSubject = Value
			End Set
		End Property

		''' <summary>
		''' The ForumID the search result belongs to.
		''' </summary>
		''' <value></value>
		''' <returns>The forumID the post belongs to.</returns>
		''' <remarks></remarks>
		Public Property ForumID() As Integer
			Get
				Return mForumID
			End Get
			Set(ByVal Value As Integer)
				mForumID = Value
			End Set
		End Property

		''' <summary>
		''' If the thead has been read by the user or not for the returned search item. 
		''' </summary>
		''' <value></value>
		''' <returns>True if the user has read the post, false otherwise.</returns>
		''' <remarks></remarks>
		Public Property IsUnRead() As Boolean
			Get
				Return mIsUnRead
			End Get
			Set(ByVal Value As Boolean)
				mIsUnRead = Value
			End Set
		End Property

		''' <summary>
		''' The thread rating of the returned search result.
		''' </summary>
		''' <value></value>
		''' <returns>Thread rating.</returns>
		''' <remarks></remarks>
		Public Property Rating() As Double
			Get
				Return mRating
			End Get
			Set(ByVal Value As Double)
				mRating = Value
			End Set
		End Property

		''' <summary>
		''' The number of times the thread has been rated, for the returned search result.
		''' </summary>
		''' <value></value>
		''' <returns>Number of times the thread has been rated.</returns>
		''' <remarks></remarks>
		Public Property RatingCount() As Integer
			Get
				Return mRatingCount
			End Get
			Set(ByVal Value As Integer)
				mRatingCount = Value
			End Set
		End Property

		''' <summary>
		''' The number of times the thread has been viewed by users for the returned search result.
		''' </summary>
		''' <value></value>
		''' <returns>Number of times the thread has been viewed.</returns>
		''' <remarks></remarks>
		Public Property Views() As Integer
			Get
				Return mViews
			End Get
			Set(ByVal Value As Integer)
				mViews = Value
			End Set
		End Property

		''' <summary>
		''' The number of replies for the thread returned by the search.
		''' </summary>
		''' <value></value>
		''' <returns>The number of replies to a thread.</returns>
		''' <remarks></remarks>
		Public Property Replies() As Integer
			Get
				Return mReplies
			End Get
			Set(ByVal Value As Integer)
				mReplies = Value
			End Set
		End Property

		''' <summary>
		''' The status of the thread returned from the search. 
		''' </summary>
		''' <value></value>
		''' <returns>The status of the thread. (ie. Unanswered, Answered, etc.)</returns>
		''' <remarks></remarks>
		Public Property ThreadStatus() As ThreadStatus
			Get
				Return mThreadStatus
			End Get
			Set(ByVal Value As ThreadStatus)
				mThreadStatus = Value
			End Set
		End Property

#End Region

	End Class

End Namespace