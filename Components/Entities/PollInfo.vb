'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2011
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
    ''' All properties associated with the Forum_Polls_Polls table in the data store. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PollInfo

#Region "Private Members"

        Private _PollID As Integer
        Private _Question As String
        Private _ShowResults As Boolean
        Private _CreatedDate As DateTime
        Private _EndDate As DateTime
        Private _TakenMessage As String
        Private _ThreadID As Integer
        Private _ModuleID As Integer
        Private _TotalVotes As Integer

#End Region

#Region "Constructors"

        Public Sub New()
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The primary key value for the Polls table.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
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
        ''' The question the poll is answering.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Question() As String
            Get
                Return _Question
            End Get
            Set(ByVal Value As String)
                _Question = Value
            End Set
        End Property

        ''' <summary>
        ''' If the results should be shown to users.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ShowResults() As Boolean
            Get
                Return _ShowResults
            End Get
            Set(ByVal Value As Boolean)
                _ShowResults = Value
            End Set
        End Property

        ''' <summary>
        ''' The date the poll was created.
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
        ''' The date the poll ends/ended.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EndDate() As DateTime
            Get
                Return _EndDate
            End Get
            Set(ByVal Value As DateTime)
                _EndDate = Value
            End Set
        End Property

        ''' <summary>
        ''' The message to show users after they have taken the poll.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Also shown if users cannot see results.</remarks>
        Public Property TakenMessage() As String
            Get
                Return _TakenMessage
            End Get
            Set(ByVal Value As String)
                _TakenMessage = Value
            End Set
        End Property

        ''' <summary>
        ''' The Thread the poll is attached to.
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
        ''' The ModuleID the poll is associated with.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Exists for FK delete reasons, since no FK associated w/ ThreadID.</remarks>
        Public Property ModuleID() As Integer
            Get
                Return _ModuleID
            End Get
            Set(ByVal Value As Integer)
                _ModuleID = Value
            End Set
        End Property

        ''' <summary>
        ''' The total number of users who answered the poll.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TotalVotes() As Integer
            Get
                Return _TotalVotes
            End Get
            Set(ByVal Value As Integer)
                _TotalVotes = Value
            End Set
        End Property

#End Region

#Region "Read Only Properties"

        ''' <summary>
        ''' A collection of answers available for a specific poll.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Answers() As List(Of AnswerInfo)
            Get
                If PollID > 0 Then
                    Dim cntAnswerController As New AnswerController
                    Return cntAnswerController.GetPollAnswers(PollID)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' A collection of user's answers for a specific poll.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property UserAnswers() As List(Of UserAnswerInfo)
            Get
                If PollID > 0 Then
                    Dim cntUserAnswer As New UserAnswerController
                    Return cntUserAnswer.GetUserAnswers(PollID)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' If the poll is closed to voting or not.
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if the poll is open for voting.</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PollClosed() As Boolean
            Get
                If EndDate > Date.Now() Or EndDate = Null.NullDate Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

#End Region

    End Class

End Namespace
