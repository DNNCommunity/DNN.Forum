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
    ''' Communicates with the data store via the DAL for the Forum_Polls_Answers Answer items. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AnswerController

        ''' <summary>
        ''' Retrieves a single Answer instance (one row of data).
        ''' </summary>
        ''' <param name="AnswerID">The integer AnswerID instance to retrieve.</param>
        ''' <returns>A row of data representing a specific Answer.</returns>
        ''' <remarks></remarks>
        Public Function GetAnswer(ByVal AnswerID As Integer) As AnswerInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetAnswer(AnswerID), GetType(AnswerInfo)), AnswerInfo)
        End Function

        ''' <summary>
        ''' Retrieves a collection of User Incidents.
        ''' </summary>
        ''' <param name="PollID">The Poll to retrieve Answers for.</param>
        ''' <returns>A collection of Answers assigned to a Poll.</returns>
        ''' <remarks></remarks>
        Public Function GetPollAnswers(ByVal PollID As Integer) As List(Of AnswerInfo)
            Return CBO.FillCollection(Of AnswerInfo)(DataProvider.Instance().GetPollAnswers(PollID))
        End Function

        ''' <summary>
        ''' Adds a single Answer record.
        ''' </summary>
        ''' <param name="objAnswer">The AnswerInfo object to add.</param>
        ''' <returns>An integer representing the AnswerID which was just added.</returns>
        ''' <remarks></remarks>
        Public Function AddAnswer(ByVal objAnswer As AnswerInfo) As Integer
            Return CType(DataProvider.Instance().AddAnswer(objAnswer.PollID, objAnswer.Answer, objAnswer.SortOrder), Integer)
        End Function

        ''' <summary>
        ''' Updates a single Answer record.
        ''' </summary>
        ''' <param name="objAnswer">The AnswerInfo object to update.</param>
        ''' <remarks></remarks>
        Public Sub UpdateAnswer(ByVal objAnswer As AnswerInfo)
            DataProvider.Instance().UpdateAnswer(objAnswer.AnswerID, objAnswer.PollID, objAnswer.Answer, objAnswer.SortOrder)
        End Sub

        ''' <summary>
        ''' Permanently removes an Answer record.
        ''' </summary>
        ''' <param name="AnswerID">The integer AnswerID value to remove.</param>
        ''' <remarks></remarks>
        Public Sub DeleteAnswer(ByVal AnswerID As Integer)
            DataProvider.Instance().DeleteAnswer(AnswerID)
        End Sub

    End Class

End Namespace
