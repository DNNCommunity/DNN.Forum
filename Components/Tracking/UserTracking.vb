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
	''' 
	''' </summary>
	''' <remarks></remarks>
	Public Class UserTrackingController

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ForumID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetForumSubscribers(ByVal ForumID As Integer) As List(Of UserTrackingInfo)
			Return CBO.FillCollection(Of UserTrackingInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GetForumSubscribers(ForumID))
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetThreadSubscribers(ByVal ThreadID As Integer) As List(Of UserTrackingInfo)
			Return CBO.FillCollection(Of UserTrackingInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GetThreadSubscribers(ThreadID))
		End Function

	End Class


	''' <summary>
	''' UserTracking info object.
	''' </summary>
	''' <remarks></remarks>
	Public Class UserTrackingInfo

#Region "Private Members"

		Dim _ForumID As Integer
		Dim _Name As String
		Dim _ThreadID As Integer
		Dim _Subject As String
		Dim _UserID As Integer
		Dim _Email As String
		Dim _Username As String
		Dim _CreatedDate As Date

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
		''' tracked forum name 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal value As String)
				_Name = value
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
		''' tracked thread subject
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
		''' The email address of the subscribed user.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Email() As String
			Get
				Return _Email
			End Get
			Set(ByVal value As String)
				_Email = value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Username() As String
			Get
				Return _Username
			End Get
			Set(ByVal value As String)
				_Username = value
			End Set
		End Property

		''' <summary>
		''' CreatedDate of ltracked item (forum/thread)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedDate() As Date
			Get
				Return _CreatedDate
			End Get
			Set(ByVal value As Date)
				_CreatedDate = value
			End Set
		End Property

#End Region

	End Class

End Namespace
