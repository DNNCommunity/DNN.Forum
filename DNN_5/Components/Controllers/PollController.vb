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
    ''' Communicates with the Forum_Polls_Polls table in the data store. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PollController

        ''' <summary>
		''' Retrieves a single poll instance (one row of data).
        ''' </summary>
        ''' <param name="PollID">The integer PollID instance to retrieve.</param>
        ''' <returns>A row of data representing a specific Poll.</returns>
        ''' <remarks></remarks>
        Public Function GetPoll(ByVal PollID As Integer) As PollInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetPoll(PollID), GetType(PollInfo)), PollInfo)
        End Function

        ''' <summary>
		''' Adds a poll to the data store. 
        ''' </summary>
        ''' <param name="objPoll"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function AddPoll(ByVal objPoll As PollInfo) As Integer
            Return CType(DataProvider.Instance().AddPoll(objPoll.Question, objPoll.ShowResults, objPoll.EndDate, objPoll.TakenMessage, objPoll.ModuleID), Integer)
        End Function

        ''' <summary>
		''' Updates a poll in the data store.
        ''' </summary>
        ''' <param name="objPoll"></param>
        ''' <remarks></remarks>
        Public Sub UpdatePoll(ByVal objPoll As PollInfo)
            DotNetNuke.Modules.Forum.DataProvider.Instance().UpdatePoll(objPoll.PollID, objPoll.Question, objPoll.ShowResults, objPoll.EndDate, objPoll.TakenMessage)
        End Sub

        ''' <summary>
		''' Delets a poll from the data store.
        ''' </summary>
        ''' <param name="PollID"></param>
        ''' <remarks></remarks>
		Public Sub DeletePoll(ByVal PollID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().DeletePoll(PollID)
		End Sub

        ''' <summary>
		''' Returns a list of polls that don't have an associated threadid. 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetOrphanedPolls(ByVal ModuleID As Integer) As List(Of PollInfo)
            Return CBO.FillCollection(Of PollInfo)(DataProvider.Instance().GetOrphanedPolls(ModuleID))
        End Function

    End Class

End Namespace
