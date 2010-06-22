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

#Region "ModerateController"

	''' <summary>
	''' Handles all database calls for moderator auditing and for tasks specific to a moderator
	''' This does not include post moderation actions where posts are actually approved/rejected, etc. 
	''' just the auditing of those functions (database tracking)
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	7/13/2005	Created
	''' </history>
	Public Class ModerateController

#Region "Public Methods"

		''' <summary>
		''' Gets all posts to moderate based on the forum 
		''' </summary>
		''' <param name="ForumID"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	12/3/2005	Created
		''' </history>
		Public Function ModeratePostGet(ByVal ForumID As Integer) As List(Of PostInfo)
			Return CBO.FillCollection(Of PostInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ModeratePostGet(ForumID))
		End Function

		''' <summary>
		''' Gets all the forums the user is a moderator of
		''' </summary>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	12/3/2005	Created
		''' </history>
		Public Function ModerateForumGetByModeratorThreads(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal PortalID As Integer) As List(Of ModerateForumInfo)
			Return CBO.FillCollection(Of ModerateForumInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().ModerateForumGetByModeratorThreads(UserID, ModuleID, PortalID))
		End Function

		''' <summary>
		''' Updates database with which user approved which post (generic notes)
		''' This is for moderator tracking (auditing)
		''' </summary>
		''' <param name="PostID"></param>
		''' <param name="ApprovedBy"></param>
		''' <param name="Notes"></param>
		''' <param name="ForumID"></param>
		''' <param name="ParentID"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	12/3/2005	Created
		''' </history>
		Public Sub ModeratePostApprove(ByVal PostID As Integer, ByVal ApprovedBy As Integer, ByVal Notes As String, ByVal ForumID As Integer, ByVal ParentID As Integer)
			Dim GroupID As Integer
			GroupID = DotNetNuke.Modules.Forum.DataProvider.Instance().ModeratePostApprove(PostID, ApprovedBy, Notes)
			' Reset Group Info
			GroupInfo.ResetGroupInfo(GroupID)
			' Reset Forum Info

			ForumController.ResetForumInfoCache(ForumID)
			Dim f As New ForumController
			f.ClearCache_ForumGetAll(ParentID, GroupID)
		End Sub

		''' <summary>
		''' This is used to add moderator history. Currently, this is only used for thread status changes by a moderator. Other items are handled via other stored procedures located in this class. 
		''' </summary>
		''' <param name="ObjectID"></param>
		''' <param name="PortalID"></param>
		''' <param name="ModeratorID"></param>
		''' <param name="Notes"></param>
		''' <param name="ActionID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function AddModeratorHistory(ByVal ObjectID As Integer, ByVal PortalID As Integer, ByVal ModeratorID As Integer, ByVal Notes As String, ByVal ActionID As Integer) As Integer
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().AddModeratorHistory(ObjectID, PortalID, ModeratorID, Notes, ActionID)
		End Function

#End Region

	End Class

#End Region

End Namespace