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
Option Explicit On
Option Strict On

Namespace DotNetNuke.Modules.Forum

#Region "UserThreadsInfo"

    ''' <summary>
    ''' Creates a custom business object that represents one row of data from the Forum_UserThreads table.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[jmathis]	12/3/2005	Created
    ''' </history>
    Public Class UserThreadsInfo

#Region "Private Members"

        Dim _userID As Integer
        Dim _threadID As Integer
        Dim _lastVisitDate As Date

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Instantiates the class
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[jmathis]	12/3/2005	Created
        ''' </history>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Instantiates the class
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="threadID"></param>
        ''' <param name="lastVisitDate"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[jmathis]	12/3/2005	Created
        ''' </history>
        Public Sub New(ByVal userID As Integer, ByVal threadID As Integer, ByVal lastVisitDate As Date)
            Me.UserID = userID
            Me.ThreadID = threadID
            Me.LastVisitDate = lastVisitDate
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The userID of whom we are tracking. 
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
        ''' The ThreadID we are tracking. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ThreadID() As Integer
            Get
                Return _threadID
            End Get
            Set(ByVal Value As Integer)
                _threadID = Value
            End Set
        End Property

        ''' <summary>
        ''' The last visit date to the thread for the user. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastVisitDate() As Date
            Get
                Return _lastVisitDate
            End Get
            Set(ByVal Value As Date)
                _lastVisitDate = Value
            End Set
        End Property

#End Region

    End Class

#End Region

End Namespace
