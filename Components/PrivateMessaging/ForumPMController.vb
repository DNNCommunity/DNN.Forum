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

#Region "PMController"

	''' <summary>
	''' Communicates with the Forum_PM_Posts table in the data store. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class PMController

#Region "Public Methods"

		''' <summary>
		''' Returns an inbox/outbox PM count for a specific portal user. 
		''' </summary>
		''' <param name="UserId">The user to return the Message Status of.</param>
		''' <param name="PortalId">The Portal the user belongs to where we should look for PM message status.</param>
		''' <returns></returns>
		''' <remarks>Added by Skeel</remarks>
		Public Function PMGetMessageStatus(ByVal UserId As Integer, ByVal PortalId As Integer) As PMCountInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().PMGetMessageStatus(UserId, PortalId), GetType(PMCountInfo)), PMCountInfo)
		End Function

		''' <summary>
		''' This returns a list of PMs for a specific PM Thread.
		''' </summary>
		''' <param name="PMThreadID">The PM Thread we want to return posts of.</param>
		''' <param name="PageIndex">The page we are returning the PM post list for.</param>
		''' <param name="PageSize">The max number of items to return. </param>
		''' <param name="Descending">The order we should return the PM list. True = Descending.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function PMGetAll(ByVal PMThreadID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal Descending As Boolean) As List(Of PMInfo)
			Dim objThreads As New List(Of PMInfo)
			Dim dr As IDataReader = Nothing
			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().PMGetAll(PMThreadID, PageIndex, PageSize, Descending)
				While dr.Read
					Dim objThreadInfo As PMInfo = FillPMInfo(dr)
					objThreads.Add(objThreadInfo)
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
		''' Custom Hydrator to fill the PMInfo object.
		''' </summary>
		''' <param name="dr">The datareader to populate the PMInfo object.</param>
		''' <returns>A PMInfo object populated from a datareader.</returns>
		''' <remarks></remarks>
		Private Function FillPMInfo(ByVal dr As IDataReader) As PMInfo
			Dim objThreadInfo As New PMInfo

			Try
				objThreadInfo.PMID = Convert.ToInt32(Null.SetNull(dr("PMID"), objThreadInfo.PMID))
			Catch
			End Try
			Try
				objThreadInfo.ParentPMID = Convert.ToInt32(Null.SetNull(dr("ParentPMId"), objThreadInfo.ParentPMID))
			Catch
			End Try
			Try
				objThreadInfo.RemoteAddr = Convert.ToString(Null.SetNull(dr("RemoteAddr"), objThreadInfo.RemoteAddr))
			Catch
			End Try
			Try
				objThreadInfo.Subject = Convert.ToString(Null.SetNull(dr("Subject"), objThreadInfo.Subject))
			Catch
			End Try
			Try
				objThreadInfo.Body = Convert.ToString(Null.SetNull(dr("Body"), objThreadInfo.Body))
			Catch
			End Try
			Try
				objThreadInfo.CreatedDate = Convert.ToDateTime(Null.SetNull(dr("CreatedDate"), objThreadInfo.CreatedDate))
			Catch
			End Try
			Try
				objThreadInfo.PMThreadID = Convert.ToInt32(Null.SetNull(dr("PMThreadID"), objThreadInfo.PMThreadID))
			Catch
			End Try
			Try
				objThreadInfo.PMFromUserID = Convert.ToInt32(Null.SetNull(dr("PMFromUserID"), objThreadInfo.PMFromUserID))
			Catch
			End Try
			Try
				objThreadInfo.PMToUserID = Convert.ToInt32(Null.SetNull(dr("PMToUserID"), objThreadInfo.PMToUserID))
			Catch
			End Try
			Try
				objThreadInfo.TotalRecords = Convert.ToInt32(Null.SetNull(dr("TotalRecords"), objThreadInfo.PMToUserID))
			Catch
			End Try

			Return objThreadInfo
		End Function

		''' <summary>
		''' Retrieves a private message from the Forum_PM_Posts table in the data store.
		''' </summary>
		''' <param name="PMID">The private message ID to retieve from the data store.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function PMGet(ByVal PMID As Integer) As PMInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().PMGet(PMID), GetType(PMInfo)), PMInfo)
		End Function

		''' <summary>
		''' Adds a private message to the data store.
		''' </summary>
		''' <param name="ParentPMID"></param>
		''' <param name="PMFromUserID"></param>
		''' <param name="RemoteAddr"></param>
		''' <param name="Subject"></param>
		''' <param name="Body"></param>
		''' <param name="PMToUserID"></param>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function PMAdd(ByVal ParentPMID As Integer, ByVal PMFromUserID As Integer, ByVal RemoteAddr As String, ByVal Subject As String, ByVal Body As String, ByVal PMToUserID As Integer, ByVal PortalID As Integer) As Integer
			Dim PMID As Integer
			Try
				PMID = DotNetNuke.Modules.Forum.DataProvider.Instance().PMAdd(ParentPMID, PMFromUserID, RemoteAddr, Subject, Body, PMToUserID, PortalID)
				Return PMID
			Catch ex As Exception
				DotNetNuke.Services.Exceptions.LogException(ex)
				Return -1
			End Try
		End Function

		''' <summary>
		''' Updates a private message to the data store.
		''' </summary>
		''' <param name="PMID"></param>
		''' <param name="Subject"></param>
		''' <param name="Body"></param>
		''' <remarks>Added by Skeel</remarks>
		Public Sub PMUpdate(ByVal PMID As Integer, ByVal Subject As String, ByVal Body As String)
			DotNetNuke.Modules.Forum.DataProvider.Instance().PMUpdate(PMID, Subject, Body)
		End Sub

		''' <summary>
		''' Deletes a private message not read by recipient.
		''' </summary>
		''' <param name="PMID"></param>
		''' <param name="PMThreadId"></param>
		''' <remarks>Added by Skeel</remarks>
		Public Sub PMDelete(ByVal PMID As Integer, ByVal PMThreadId As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().PMDelete(PMID, PMThreadId)
		End Sub

		''' <summary>
		''' Returns a list of users who have private messaging enabled for a specific portal, searched by username or display name depending on settings.
		''' </summary>
		''' <param name="PortalId">The Portal to return PM users for.</param>
		''' <param name="NameToMatch">The username or display name to match in the database.</param>
		''' <param name="PageIndex">The starting point index (page).</param>
		''' <param name="PageSize">The number of items to return.</param>
		''' <param name="ByUsername">Boolean to determine if we should search by DisplayName or Username. True if by username.</param>
		''' <param name="ModuleID"></param>
		''' <remarks></remarks>
		Public Function PMUsersGet(ByVal PortalId As Integer, ByVal NameToMatch As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal ByUsername As Boolean, ByVal ModuleID As Integer) As List(Of ForumUser)
			Dim objUsers As New List(Of ForumUser)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().PMUsersGet(PortalId, NameToMatch, PageIndex, PageSize, ByUsername, False)
				While dr.Read
					Dim cntUser As New ForumUserController
					Dim objUserInfo As ForumUser = cntUser.FillForumUserInfo(dr, PortalId, ModuleID)
					objUsers.Add(objUserInfo)
				End While
				dr.NextResult()
			Catch exc As Exception
				LogException(exc)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objUsers
		End Function

		''' <summary>
		''' Returns a single ForumUser.
		''' </summary>
		''' <param name="PortalId">The Portal to return the PM user for.</param>
		''' <param name="UserName">The username or display name to find in the database.</param>
		''' <param name="ByUsername">Boolean to determine if we should search by DisplayName or Username. True if by username.</param>
		''' <remarks>Added by Skeel</remarks>
		Public Function PMUserGetSpecific(ByVal PortalId As Integer, ByVal UserName As String, ByVal ByUsername As Boolean) As Integer

			Dim dr As IDataReader = DotNetNuke.Modules.Forum.DataProvider.Instance().PMUsersGet(PortalId, UserName, 0, 0, ByUsername, True)
			While dr.Read
				Return CInt(dr(0).ToString)
			End While
			dr.Close()
		End Function

#End Region

	End Class

#End Region

End Namespace