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
    ''' This object is used in the same way as Groups, Threads, etc w/ the exception that it has no UI to render this logic in. 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
	Public Class Forums
		Inherits ForumObject

#Region "Private Declarations"

		Private mForumCollection As New ArrayList
		Private mThreadsCount As Integer
		Private mPostsCount As Integer

#End Region

#Region "Public Properties"

		''' <summary>
		''' A collection of forum info objects.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ForumCollection() As ArrayList
			Get
				Return mForumCollection
			End Get
		End Property

		''' <summary>
		''' The GroupID of the forum info object.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property GroupID() As Integer
			Get
				Return ForumControl.GenericObjectID
			End Get
		End Property

		''' <summary>
		''' The number of threads in the forum.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ThreadsCount() As Integer
			Get
				Return mThreadsCount
			End Get
		End Property

		''' <summary>
		''' The number of posts in the forum.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PostsCount() As Integer
			Get
				Return mPostsCount
			End Get
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Instantiates the class.
		''' </summary>
		''' <param name="forum"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal forum As DNNForum)
			MyBase.New(forum)
		End Sub

#End Region

	End Class

End Namespace