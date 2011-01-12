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

#Region "PostReportedController"

	''' <summary>
	''' Communicates with the Forum_Post_Reported table in the data store. 
	''' </summary>
	''' <remarks></remarks>
	Public Class PostReportedController

#Region "Public Methods"

		''' <summary>
		''' Flags a post as reported by the user passed in.
		''' </summary>
		''' <param name="PostID">The post being reported.</param>
		''' <param name="UserID">The user reporting the post.</param>
		''' <param name="Reason">The reason the post is being reported.</param>
		''' <remarks></remarks>
		Public Sub AddPostReport(ByVal PostID As Integer, ByVal UserID As Integer, ByVal Reason As String)
			Dim GroupID As Integer = DotNetNuke.Modules.Forum.DataProvider.Instance().AddPostReport(PostID, UserID, Reason)
		End Sub

		''' <summary>
		''' This determines if the user (attempting to report this post) has reported the same post previously. 
		''' </summary>
		''' <param name="PostID">The post the user is attempting to report.</param>
		''' <param name="UserID">The user that is attempting to report the post.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function CheckPostReport(ByVal PostID As Integer, ByVal UserID As Integer) As Boolean
			Dim HasReported As Boolean = DotNetNuke.Modules.Forum.DataProvider.Instance().CheckPostReport(PostID, UserID)
			Return HasReported
		End Function

		''' <summary>
		''' Returns a list of all posts reported that are still flagged for review by moderators.
		''' </summary>
		''' <param name="PortalID">The portal the post belongs to.</param>
		''' <param name="PageIndex">The page index being returned.</param>
		''' <param name="PageSize">The number of rows to return.</param>
        ''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetReportedPosts(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As List(Of PostInfo)
			Return CBO.FillCollection(Of PostInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GetReportedPosts(PortalID, PageIndex, PageSize))
		End Function

		''' <summary>
		''' Returns all the reports related to a single post.
		''' </summary>
		''' <param name="PostID">The post we are viewing report details of.</param>
		''' <returns>A list of reports for a specific post.</returns>
		''' <remarks>Always returned from newest to oldest.</remarks>
		Public Function GetPostReportDetails(ByVal PostID As Integer) As List(Of PostReportedInfo)
			Return CBO.FillCollection(Of PostReportedInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GetPostReportDetails(PostID))
		End Function

		''' <summary>
		''' Updates the reported post so that it is marked as 'addressed' and no longer will show in the reporting interfaces.
		''' </summary>
		''' <param name="PostReportedID">The post report we are updating.</param>
		''' <param name="UserID">The user who is addressing the post report.</param>
		''' <returns>Set to return PostID but having issues with that part.</returns>
		''' <remarks></remarks>
		Public Function AddressPostReport(ByVal PostReportedID As Integer, ByVal UserID As Integer, ByVal PortalID As Integer) As Integer
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().AddressPostReport(PostReportedID, UserID, PortalID)
		End Function

#End Region

	End Class

#End Region

#Region "ReportedUserController"

	''' <summary>
	''' Properties associated with reported forum users. This is a combination of the Forum_Users table as well as the Forum_Post_Reported table. 
	''' </summary>
	''' <remarks></remarks>
	Public Class ReportedUserController

#Region "Public Methods"

		''' <summary>
		''' Retrieves a collection of reported forum users from the data store.
		''' </summary>
		''' <param name="PortalID">The portal to retrieve reported posts for.</param>
		''' <param name="PageIndex">The current SQL page we are retrieving.</param>
		''' <param name="PageSize">The number of records to return from one request.</param>
		''' <returns>A collection of reported forum users.</returns>
		''' <remarks></remarks>
        Public Function GetReportedUsers(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As List(Of ReportedUserInfo)
            Return CBO.FillCollection(Of ReportedUserInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GetReportedUsers(PortalID, PageIndex, PageSize))
        End Function

#End Region

	End Class

#End Region

End Namespace