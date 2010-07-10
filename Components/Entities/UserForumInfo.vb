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
	''' Represents single row of data for a userid/forumid read combination.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class UserForumsInfo

#Region "Private Members"

		Dim _userID As Integer
		Dim _forumID As Integer
		Dim _lastVisitDate As DateTime

#End Region

#Region "Constructors"

		''' <summary>
		''' Used to instantiate the class.
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[jmathis]	12/3/2005	Created
		''' </history>
		Public Sub New()
		End Sub

		''' <summary>
		''' Instantiates a new instance of the class.
		''' </summary>
		''' <param name="userID">The UserID for Forum Read results.</param>
		''' <param name="forumID">The ForumID for Forum Read results.</param>
		''' <param name="lastVisitDate">The last visit time for Forum Read Results for the forum user and forum.</param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[jmathis]	12/3/2005	Created
		''' </history>
		Public Sub New(ByVal userID As Integer, ByVal forumID As Integer, ByVal lastVisitDate As DateTime)
			Me.UserID = userID
			Me.ForumID = forumID
			Me.LastVisitDate = lastVisitDate
		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' The UserID of the user being checked.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UserID() As Integer
			Get
				Return _userID
			End Get
			Set(ByVal Value As Integer)
				_userID = Value
			End Set
		End Property

		''' <summary>
		''' The ForumID being checked for.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumID() As Integer
			Get
				Return _forumID
			End Get
			Set(ByVal Value As Integer)
				_forumID = Value
			End Set
		End Property

		''' <summary>
		''' The last time the userid/forumid combo visited the forum.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LastVisitDate() As DateTime
			Get
				Return _lastVisitDate
			End Get
			Set(ByVal Value As DateTime)
				_lastVisitDate = Value
			End Set
		End Property

#End Region

	End Class

End Namespace

