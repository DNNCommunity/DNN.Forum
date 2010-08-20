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
	''' This is what gets/sets the forum/thread notification options for subscriptions.
	''' </summary>
	''' <remarks></remarks>
	Public Class TrackingController

		''' <summary>
		''' Loads up the forums a user is tracking
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function TrackingForumGet(ByVal UserID As Integer, ByVal ModuleID As Integer) As List(Of TrackingInfo)
			Return CBO.FillCollection(Of TrackingInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().TrackingForumGet(UserID, ModuleID, 10000, 0))
		End Function

		''' <summary>
		''' Loads up the threads a user is tracking
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function TrackingThreadGet(ByVal UserID As Integer, ByVal ModuleID As Integer) As List(Of TrackingInfo)
			Return CBO.FillCollection(Of TrackingInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().TrackingThreadGet(UserID, ModuleID, 10000, 0))
		End Function

		''' <summary>
		''' Returns dataset needed for UCP Tracking
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Function TrackingForumGetAll(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As List(Of TrackingInfo)
			Return CBO.FillCollection(Of TrackingInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().TrackingForumGet(UserID, ModuleID, PageSize, PageIndex))
		End Function

		''' <summary>
		''' Returns dataset needed for UCP Tracking
		''' </summary>
		''' <param name="ModuleID"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Function TrackingThreadGetAll(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As List(Of TrackingInfo)
			Return CBO.FillCollection(Of TrackingInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().TrackingThreadGet(UserID, ModuleID, PageSize, PageIndex))
		End Function

		''' <summary>
		''' Removes/Adds forum notification subscription for a user.
		''' </summary>
		''' <param name="ForumID"></param>
		''' <param name="UserID"></param>
		''' <param name="Add"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Sub TrackingForumCreateDelete(ByVal ForumID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().TrackingForumCreateDelete(ForumID, UserID, Add, ModuleID)
		End Sub

		''' <summary>
		''' This will always delete post notifications. If adding, thread tracking will be added and post notify deleted. If deleting, everything associated with the thread and the user will be deleted.
		''' </summary>
		''' <param name="ForumID"></param>
		''' <param name="ThreadID"></param>
		''' <param name="UserID"></param>
		''' <param name="Add"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Sub TrackingThreadCreateDelete(ByVal ForumID As Integer, ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().TrackingThreadCreateDelete(ForumID, ThreadID, UserID, Add, ModuleID)
		End Sub

	End Class

	''' <summary>
	''' Tracking info object.
	''' </summary>
	''' <remarks></remarks>
	Public Class TrackingInfo

#Region "Private Members"

		Dim _ForumID As Integer
		Dim _ThreadID As Integer
		Dim _UserID As Integer
		Dim _ModuleID As Integer
		Dim _Subject As String
		Dim _LastApprovedPostID As Integer
		Dim _LastApprovedPostCreatedDate As Date
		Dim _LastApprovedPosterID As Integer
		Dim _TotalRecords As Integer
		Dim _RatingImage As String
		Dim _RatingText As String

#End Region

#Region "Public Properties"

		''' <summary>
		''' The forumID of the thread being tracked.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumID() As Integer
			Get
				Return _ForumID
			End Get
			Set(ByVal value As Integer)
				_ForumID = value
			End Set
		End Property

		''' <summary>
		''' Thread threadID of the thread being tracked.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ThreadID() As Integer
			Get
				Return _ThreadID
			End Get
			Set(ByVal value As Integer)
				_ThreadID = value
			End Set
		End Property

		''' <summary>
		''' The userID of the user tracking the thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UserID() As Integer
			Get
				Return _UserID
			End Get
			Set(ByVal value As Integer)
				_UserID = value
			End Set
		End Property

		''' <summary>
		''' The module associated with the tracked thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal value As Integer)
				_ModuleID = value
			End Set
		End Property

		''' <summary>
		''' tracked forum name or thread subject
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Subject() As String
			Get
				Return _Subject
			End Get
			Set(ByVal value As String)
				_Subject = value
			End Set
		End Property

		''' <summary>
		''' PostId of last post in tracked thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LastApprovedPostID() As Integer
			Get
				Return _LastApprovedPostID
			End Get
			Set(ByVal value As Integer)
				_LastApprovedPostID = value
			End Set
		End Property

		''' <summary>
		''' CreatedDate of last post in tracked thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LastApprovedPostCreatedDate() As Date
			Get
				Return _LastApprovedPostCreatedDate
			End Get
			Set(ByVal value As Date)
				_LastApprovedPostCreatedDate = value
			End Set
		End Property

		''' <summary>
		''' PosterID of last post in tracked thread.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LastApprovedPosterID() As Integer
			Get
				Return _LastApprovedPosterID
			End Get
			Set(ByVal value As Integer)
				_LastApprovedPosterID = value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal value As Integer)
				_TotalRecords = value
			End Set
		End Property

#End Region

	End Class

End Namespace
