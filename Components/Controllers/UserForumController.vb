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
	''' Communicates with the Forum_UserForums table in the data store. 
	''' </summary>
	''' <remarks>This tracks if a forum has new posts for a specific user. 
	''' </remarks>
	Public Class UserForumsController

#Region "Private Members"

		Private Const FORUM_USERFORUMREADS_CACHE_KEY_PREFIX As String = Constants.CACHE_KEY_PREFIX + "USER_FORUM_READS_"

#End Region

#Region "Friend Methods"

		''' <summary>
		''' Gets a single row of data for a forumid/userid combination.
		''' </summary>
		''' <param name="userID">The UserID being checked in the db.</param>
		''' <param name="forumID">The ForumID being checked in the db.</param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Friend Function GetUsersForumReads(ByVal UserID As Integer, ByVal ForumID As Integer) As UserForumsInfo
			Dim keyID As String = FORUM_USERFORUMREADS_CACHE_KEY_PREFIX & UserID.ToString & "-" & ForumID
			Dim timeOut As Int32 = Constants.CACHE_TIMEOUT * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

			Dim objUserForum As New UserForumsInfo
			objUserForum = CType(DataCache.GetCache(keyID), UserForumsInfo)

			If objUserForum Is Nothing Then
				objUserForum = GetUserForumRead(UserID, ForumID)

				If timeOut > 0 And objUserForum IsNot Nothing Then
					DataCache.SetCache(keyID, objUserForum, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return objUserForum
		End Function

		''' <summary>
		''' Resets the cached user PM Count object
		''' </summary>
		''' <param name="UserID"></param>
		''' <remarks>
		''' </remarks>
		Friend Shared Sub ResetUsersForumReads(ByVal UserID As Integer, ByVal ForumID As Integer)
			Dim keyID As String = FORUM_USERFORUMREADS_CACHE_KEY_PREFIX & UserID.ToString & "-" & ForumID
			DataCache.RemoveCache(keyID)
		End Sub

		''' <summary>
		''' Adds an instance of a forumid/userid combination to the db.
		''' </summary>
		''' <param name="objUserForums"></param>
		''' <remarks>
		''' </remarks>
		Friend Sub Add(ByVal objUserForums As UserForumsInfo)
			DataProvider.Instance().AddUserForums(objUserForums.UserID, objUserForums.ForumID, objUserForums.LastVisitDate)
		End Sub

		''' <summary>
		''' Updates an instance of a forumid/userid combination in the db.
		''' </summary>
		''' <param name="objUserForums"></param>
		''' <remarks>
		''' </remarks>
		Friend Sub Update(ByVal objUserForums As UserForumsInfo)
			DataProvider.Instance().UpdateUserForums(objUserForums.UserID, objUserForums.ForumID, objUserForums.LastVisitDate)
		End Sub

		''' <summary>
		''' Deletes an instance of read data for a forumid/userid combination.
		''' </summary>
		''' <param name="userID"></param>
		''' <param name="forumID"></param>
		''' <remarks>
		''' </remarks>
		Friend Sub Delete(ByVal userID As Integer, ByVal forumID As Integer)
			DataProvider.Instance().DeleteUserForums(userID, forumID)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Gets a single row of data for a forumid/userid combination.
		''' </summary>
		''' <param name="userID">The UserID being checked in the db.</param>
		''' <param name="forumID">The ForumID being checked in the db.</param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Private Function GetUserForumRead(ByVal UserID As Integer, ByVal ForumID As Integer) As UserForumsInfo
			Return CType(CBO.FillObject(DataProvider.Instance().GetUserForums(UserID, ForumID), GetType(UserForumsInfo)), UserForumsInfo)
		End Function

#End Region

	End Class

End Namespace