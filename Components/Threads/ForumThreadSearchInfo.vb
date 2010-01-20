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

#Region "ThreadSearchInfo"

	''' <summary>
	''' All properites associated with the Forum_Threads and Forum_Posts tables exposed to module's search
	''' </summary>
	''' <remarks></remarks>
	Public Class ThreadSearchInfo

#Region "Private Members"

		Private _ForumID As Integer
		Private _ThreadId As Integer
		Private _PostId As Integer
		Private _Subject As String
		Private _Body As String
		Private _CreatedByUser As Integer
		Private _CreatedDate As DateTime
		Private _UpdatedByUser As Integer
		Private _UpdatedDate As DateTime

#End Region

#Region "Constructors"

		''' <summary>
		''' Instantiates the class.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' The ForumID associated with the search result.
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
		''' The ThreadID associated with the search result.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ThreadID() As Integer
			Get
				Return _ThreadId
			End Get
			Set(ByVal Value As Integer)
				_ThreadId = Value
			End Set
		End Property

		''' <summary>
		''' The PostID associated with the search result.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostID() As Integer
			Get
				Return _PostId
			End Get
			Set(ByVal Value As Integer)
				_PostId = Value
			End Set
		End Property

		''' <summary>
		''' The subject of the post associated with the search result.
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
		''' The body of the post associated with the search result.
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
		''' The author of the post associated with the search result.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedByUser() As Integer
			Get
				Return _CreatedByUser
			End Get
			Set(ByVal Value As Integer)
				_CreatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' The date the post was created.
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
		''' The last user who updated the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedByUser() As Integer
			Get
				Return _UpdatedByUser
			End Get
			Set(ByVal Value As Integer)
				_UpdatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' The date the post was last updated.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedDate() As DateTime
			Get
				Return _UpdatedDate
			End Get
			Set(ByVal Value As DateTime)
				_UpdatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The URL to see the thread search result in threads view.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property URLParams() As String
			Get
				Return "&forumid=" & _ForumID.ToString & "&scope=posts&threadid=" & ThreadID.ToString
			End Get
		End Property

#End Region

	End Class

#End Region

End Namespace