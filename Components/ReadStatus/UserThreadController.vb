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

#Region "UserThreadsController"

    ''' <summary>
    ''' Hanldes the Thread read status for a single user instance.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[jmathis]	12/3/2005	Created
    ''' </history>
    Public Class UserThreadsController

#Region "Private Members"

        Private Const FORUM_USERTHREADREADS_CACHE_KEY_PREFIX As String = "Forum_UserThreadReads-"
        Private Const UserThreadReadsCacheTimeout As Integer = 20

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Gets the thread read status for a single thread for a single user
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="threadID"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[jmathis]	12/3/2005	Created
        ''' </history>
        Private Function GetUserThreadRead(ByVal UserID As Integer, ByVal ThreadID As Integer) As UserThreadsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetUserThreads(UserID, ThreadID), GetType(UserThreadsInfo)), UserThreadsInfo)
        End Function

#End Region

#Region "Public Methods"

#Region "Caching"

        ''' <summary>
        ''' Gets a single row of data for a threadid/userid combination.
        ''' </summary>
        ''' <param name="UserID">The UserID being checked in the db.</param>
        ''' <param name="ThreadID">The ForumID being checked in the db.</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Public Function GetCachedUserThreadRead(ByVal UserID As Integer, ByVal ThreadID As Integer) As UserThreadsInfo
            Dim keyID As String = FORUM_USERTHREADREADS_CACHE_KEY_PREFIX & UserID.ToString & "-" & ThreadID
			Dim timeOut As Int32 = UserThreadReadsCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

            Dim objUserThread As New UserThreadsInfo
            objUserThread = CType(DataCache.GetCache(keyID), UserThreadsInfo)

            If objUserThread Is Nothing Then
                objUserThread = GetUserThreadRead(UserID, ThreadID)

                If timeOut > 0 And objUserThread IsNot Nothing Then
					DataCache.SetCache(keyID, objUserThread, TimeSpan.FromMinutes(timeOut))
                End If
            End If

            Return objUserThread
        End Function

        ''' <summary>
        ''' Resets the cached user thread 
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	12/4/2005	Created
        ''' </history>
        Public Shared Sub ResetUserThreadReadCache(ByVal UserID As Integer, ByVal ThreadID As Integer)
            Dim keyID As String = FORUM_USERTHREADREADS_CACHE_KEY_PREFIX & UserID.ToString & "-" & ThreadID
            DataCache.RemoveCache(keyID)
        End Sub

#End Region

        ''' <summary>
        ''' Adds a read status
        ''' </summary>
        ''' <param name="objUserThreads"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[jmathis]	12/3/2005	Created
        ''' </history>
        Public Sub Add(ByVal objUserThreads As UserThreadsInfo)
            DataProvider.Instance().AddUserThreads(objUserThreads.UserID, objUserThreads.ThreadID, objUserThreads.LastVisitDate)
        End Sub

        ''' <summary>
        ''' Updates a read status
        ''' </summary>
        ''' <param name="objUserThreads"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[jmathis]	12/3/2005	Created
        ''' </history>
        Public Sub Update(ByVal objUserThreads As UserThreadsInfo)
            DataProvider.Instance().UpdateUserThreads(objUserThreads.UserID, objUserThreads.ThreadID, objUserThreads.LastVisitDate)
        End Sub

        ''' <summary>
        ''' Deletes all threads from db table for thread read status for a single user
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="forumID"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[jmathis]	12/3/2005	Created
        ''' </history>
        Public Sub DeleteAllByForum(ByVal userID As Integer, ByVal forumID As Integer)
            DataProvider.Instance().DeleteUserThreadsByForum(userID, forumID)
        End Sub

        ''' <summary>
        ''' Marks all threads in a forum as read for a single user
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="forumID"></param>
        ''' <param name="read"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[jmathis]	12/3/2005	Created
        ''' </history>
        Public Sub MarkAll(ByVal userID As Integer, ByVal forumID As Integer, ByVal read As Boolean)
            DeleteAllByForum(userID, forumID)

            If read Then
                Dim threadController As New ThreadController
                Dim forumThreads As ArrayList = threadController.GetByForum(userID, forumID)
                For Each forumThread As ThreadInfo In forumThreads
                    Dim userThread As New UserThreadsInfo
                    With userThread
                        .UserID = userID
                        .ThreadID = forumThread.ThreadID
                        .LastVisitDate = Now
                    End With
                    Add(userThread)
                    ResetUserThreadReadCache(userID, forumThread.ThreadID)
                Next

                Dim userForum As New UserForumsInfo
                With userForum
                    .UserID = userID
                    .ForumID = forumID
                    .LastVisitDate = Now
                End With
            End If
        End Sub

        ''' <summary>
        ''' Gets the Post index number for the first unread Post  
        ''' </summary>
        ''' <param name="ThreadID"></param>
        ''' <param name="LastVisitDate"></param>
        ''' <param name="ViewDecending"></param>
        ''' <returns>Integer</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[skeel]	11/28/2008	Created
        ''' </history>
        Public Function GetPostIndexFirstUnread(ByVal ThreadID As Integer, ByVal LastVisitDate As Date, ByVal ViewDecending As Boolean) As Integer
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().ReadsGetFirstUnread(ThreadID, LastVisitDate, ViewDecending)
		End Function

#End Region

    End Class

#End Region

End Namespace
