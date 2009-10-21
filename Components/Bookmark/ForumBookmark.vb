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
Option Strict On
Option Explicit On

Namespace DotNetNuke.Modules.Forum

#Region " ForumBookmark "

	''' <summary>
	''' Communicates with the Forum_Bookmarks table in the data store.
	''' </summary>
	''' <history>
	''' 	[skeel]	12/1/2008	Created
	''' </history>
	Public Class BookmarkController

#Region " Public Methods "

		''' <summary>
		''' Removes/Adds forum notification subscription for a user.
		''' </summary>
		''' <param name="ThreadID"></param>
		''' <param name="UserID"></param>
		''' <param name="Add"></param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Sub BookmarkCreateDelete(ByVal ThreadID As Integer, ByVal UserID As Integer, ByVal Add As Boolean, ByVal ModuleID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().BookmarkCreateDelete(ThreadID, UserID, Add, ModuleID)
		End Sub

		''' <summary>
		''' Returns dataset needed for UCP Bookmarks
		''' </summary>
		''' <param name="ForumMemberName"></param>
		''' <param name="ModuleID"></param>
		''' <param name="PageIndex"></param>
		''' <param name="PageSize"></param>
		''' <param name="UserID"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Function BookmarkThreadGet(ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal ForumMemberName As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer) As List(Of TrackingInfo)
			Dim objThreads As New List(Of TrackingInfo)
			Dim dr As IDataReader = Nothing
			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().BookmarkThreadGet(UserID, ModuleID, ForumMemberName, PageSize, PageIndex)
				While dr.Read
					Dim objTrackingInfo As TrackingInfo = FillTrackingInfo(dr)
					objThreads.Add(objTrackingInfo)
				End While
				dr.NextResult()
			Catch ex As Exception
				LogException(ex)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objThreads
		End Function

		''' <summary>
		''' Returns boolean value if thread is bookmarked by user
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="ThreadID"></param>
		''' <param name="ModuleID"></param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Function BookmarkCheck(ByVal UserID As Integer, ByVal ThreadID As Integer, ByVal ModuleID As Integer) As Boolean
			Dim bookmarked As Boolean
			bookmarked = DotNetNuke.Modules.Forum.DataProvider.Instance().BookmarkCheck(UserID, ThreadID, ModuleID)
			Return bookmarked
		End Function

#End Region

#Region " Private Methods "

		''' <summary>
		''' Custom Hydrator to fill the TrackingInfo object.
		''' </summary>
		''' <param name="dr">The datareader to populate the TrackingInfo object.</param>
		''' <returns>A TrackingInfo object populated from a datareader.</returns>
		''' <remarks>Added by Skeel</remarks>
		Private Function FillTrackingInfo(ByVal dr As IDataReader) As TrackingInfo
			Dim objThreadInfo As New TrackingInfo

			Try
				objThreadInfo.ForumID = Convert.ToInt32(Null.SetNull(dr("ForumID"), objThreadInfo.ForumID))
			Catch
			End Try
			Try
				objThreadInfo.LastApprovedPostCreatedDate = Convert.ToDateTime(Null.SetNull(dr("LastApprovedPostCreatedDate"), objThreadInfo.LastApprovedPostCreatedDate))
			Catch
			End Try
			Try
				objThreadInfo.LastApprovedPosterID = Convert.ToInt32(Null.SetNull(dr("LastApprovedPosterID"), objThreadInfo.LastApprovedPosterID))
			Catch
			End Try
			Try
				objThreadInfo.LastApprovedPostID = Convert.ToInt32(Null.SetNull(dr("LastApprovedPostID"), objThreadInfo.LastApprovedPostID))
			Catch
			End Try
			Try
				objThreadInfo.ModuleID = Convert.ToInt32(Null.SetNull(dr("ModuleID"), objThreadInfo.ModuleID))
			Catch
			End Try
			Try
				objThreadInfo.Subject = Convert.ToString(Null.SetNull(dr("Subject"), objThreadInfo.Subject))
			Catch
			End Try
			Try
				objThreadInfo.ThreadID = Convert.ToInt32(Null.SetNull(dr("ThreadID"), objThreadInfo.ThreadID))
			Catch
			End Try
			Try
				objThreadInfo.UserID = Convert.ToInt32(Null.SetNull(dr("UserID"), objThreadInfo.UserID))
			Catch
			End Try
			Try
				objThreadInfo.TotalRecords = Convert.ToInt32(Null.SetNull(dr("TotalRecords"), objThreadInfo.TotalRecords))
			Catch
			End Try

			Return objThreadInfo
		End Function

#End Region

	End Class

#End Region

End Namespace