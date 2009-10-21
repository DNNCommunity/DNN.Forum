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

#Region "PostSearch"

	''' <summary>
	''' All properties associated with the Forum_Posts and Forum_Threads table that are used in ISearchable implementation.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	7/13/2005	Created
	''' </history>
	Public Class PostSearchInfo

#Region "Private Members"

		Private mForumID As Integer
		Private mPostId As Integer
		Private mSubject As String
		Private mBody As String
		Private mCreatedByUser As Integer
		Private mCreatedDate As DateTime
		Private mUpdatedByUser As Integer
		Private mUpdatedDate As DateTime

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
		''' The ForumID associated with the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
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
		''' The PostID of the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostID() As Integer
			Get
				Return mPostId
			End Get
			Set(ByVal Value As Integer)
				mPostId = Value
			End Set
		End Property

		''' <summary>
		''' The subject of the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
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
		''' The body of the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Body() As String
			Get
				Return mBody
			End Get
			Set(ByVal Value As String)
				mBody = Value
			End Set
		End Property

		''' <summary>
		''' The user who created the post. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedByUser() As Integer
			Get
				Return mCreatedByUser
			End Get
			Set(ByVal Value As Integer)
				mCreatedByUser = Value
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
				Return mCreatedDate
			End Get
			Set(ByVal Value As DateTime)
				mCreatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The user who last updated the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedByUser() As Integer
			Get
				Return mUpdatedByUser
			End Get
			Set(ByVal Value As Integer)
				mUpdatedByUser = Value
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
				Return mUpdatedDate
			End Get
			Set(ByVal Value As DateTime)
				mUpdatedDate = Value
			End Set
		End Property

#End Region

	End Class

#End Region

End Namespace