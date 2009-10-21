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

#Region "PMThreadController"

    ''' <summary>
    ''' Ties the corresponding info object to the data layer. 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	1/21/2007	Created
    ''' </history>
    Public Class PMThreadController

        ''' <summary>
        ''' Gets a list of PM still not read by the recipient. 
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PortalId"></param>
        ''' <returns></returns>
        ''' <remarks>Added by Skeel</remarks>
        Public Function PMThreadGetOutBox(ByVal UserID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal PortalId As Integer) As List(Of PMOutboxInfo)
            Dim objOutbox As New List(Of PMOutboxInfo)
			Dim dr As IDataReader = Nothing
            Try
                dr = DotNetNuke.Modules.Forum.DataProvider.Instance().PMThreadGetOutBox(UserID, PageSize, PageIndex, PortalId)
                While dr.Read
                    Dim objOutboxInfo As PMOutboxInfo = FillPMOutbox(dr)
                    objOutbox.Add(objOutboxInfo)
                End While
                dr.NextResult()
            Catch ex As Exception
                LogException(ex)
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try

            Return objOutbox

        End Function

        ''' <summary>
        ''' Gets all PM Threads for a user. This incorporates paging. 
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PageSize"></param>
        ''' <param name="PageIndex"></param>
        ''' <param name="PortalId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function PMThreadGetAll(ByVal UserID As Integer, ByVal PageSize As Integer, ByVal PageIndex As Integer, ByVal PortalId As Integer) As List(Of PMThreadInfo)
            Dim objThreads As New List(Of PMThreadInfo)
			Dim dr As IDataReader = Nothing
            Try
                dr = DotNetNuke.Modules.Forum.DataProvider.Instance().PMThreadGetAll(UserID, PageSize, PageIndex, PortalId)
                While dr.Read
                    Dim objThreadInfo As PMThreadInfo = FillPMThreadInfo(dr)
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
        ''' Hydrates the custom business object w/out using the core CBO. (Performance reasons)
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function FillPMOutbox(ByVal dr As IDataReader) As PMOutboxInfo

            'p.PMID, p.Subject, p.CreatedDate, p.PMToUserId, p.PMThreadID

            Dim objOutboxInfo As New PMOutboxInfo

            Try
                objOutboxInfo.PMID = Convert.ToInt32(Null.SetNull(dr("PMID"), objOutboxInfo.PMID))
            Catch
            End Try
            Try
                objOutboxInfo.Subject = Convert.ToString(Null.SetNull(dr("Subject"), objOutboxInfo.Subject))
            Catch
            End Try
            Try
                objOutboxInfo.CreatedDate = Convert.ToDateTime(Null.SetNull(dr("CreatedDate"), objOutboxInfo.CreatedDate))
            Catch
            End Try
            Try
                objOutboxInfo.PMToUserId = Convert.ToInt32(Null.SetNull(dr("PMToUserId"), objOutboxInfo.PMToUserId))
            Catch
            End Try
            Try
                objOutboxInfo.PMThreadId = Convert.ToInt32(Null.SetNull(dr("PMThreadID"), objOutboxInfo.PMThreadId))
            Catch
            End Try

            Return objOutboxInfo
        End Function

        ''' <summary>
        ''' Hydrates the custom business object w/out using the core CBO. (Performance reasons)
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function FillPMThreadInfo(ByVal dr As IDataReader) As PMThreadInfo
            Dim objThreadInfo As New PMThreadInfo

            Try
                objThreadInfo.PMThreadID = Convert.ToInt32(Null.SetNull(dr("PMThreadID"), objThreadInfo.PMThreadID))
            Catch
            End Try
            Try
                objThreadInfo.PMThreadSubject = Convert.ToString(Null.SetNull(dr("PMThreadSubject"), objThreadInfo.PMThreadSubject))
            Catch
            End Try
            Try
                objThreadInfo.PMStartDate = Convert.ToDateTime(Null.SetNull(dr("PMStartDate"), objThreadInfo.PMStartDate))
            Catch
            End Try
            Try
                objThreadInfo.PMStartThreadUserID = Convert.ToInt32(Null.SetNull(dr("PMStartThreadUserID"), objThreadInfo.PMStartThreadUserID))
            Catch
            End Try
            Try
                objThreadInfo.PMReceiveThreadUserID = Convert.ToInt32(Null.SetNull(dr("PMReceiveThreadUserID"), objThreadInfo.PMReceiveThreadUserID))
            Catch
            End Try
            Try
                objThreadInfo.Views = Convert.ToInt32(Null.SetNull(dr("Views"), objThreadInfo.Views))
            Catch
            End Try
            Try
                objThreadInfo.LastPostedPMID = Convert.ToInt32(Null.SetNull(dr("LastPostedPMID"), objThreadInfo.LastPostedPMID))
            Catch
            End Try
            Try
                objThreadInfo.Replies = Convert.ToInt32(Null.SetNull(dr("Replies"), objThreadInfo.Replies))
            Catch
            End Try
            Try
                objThreadInfo.PMStartUserDeleted = Convert.ToBoolean(Null.SetNull(dr("PMStartUserDeleted"), objThreadInfo.PMStartUserDeleted))
            Catch
            End Try
            Try
                objThreadInfo.PMToUserDeleted = Convert.ToBoolean(Null.SetNull(dr("PMToUserDeleted"), objThreadInfo.PMToUserDeleted))
            Catch
            End Try
            Try
                objThreadInfo.PortalID = Convert.ToInt32(Null.SetNull(dr("PortalID"), objThreadInfo.PortalID))
            Catch
            End Try
            Try
                objThreadInfo.PMToUserID = Convert.ToInt32(Null.SetNull(dr("PMToUserID"), objThreadInfo.PMToUserID))
            Catch ex As Exception
            End Try
            Try
                objThreadInfo.PMFromUserID = Convert.ToInt32(Null.SetNull(dr("PMFromUserID"), objThreadInfo.PMFromUserID))
            Catch ex As Exception
            End Try
            Try
                objThreadInfo.TotalRecords = Convert.ToInt32(Null.SetNull(dr("TotalRecords"), objThreadInfo.TotalRecords))
            Catch
            End Try

            Return objThreadInfo
        End Function

        ''' <summary>
        ''' Gets a single instance of a private message thread, by its PMThreadID. 
        ''' </summary>
        ''' <param name="PMThreadId"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Public Function PMThreadGet(ByVal PMThreadId As Integer) As PMThreadInfo
            Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().PMThreadGet(PMThreadId), GetType(PMThreadInfo)), PMThreadInfo)
        End Function

        ''' <summary>
        ''' Deletes a private message thread, only partially marks for deletion of other user has not deleted. 
        ''' </summary>
        ''' <param name="PMThreadID"></param>
        ''' <param name="PMUserID"></param>
        ''' <remarks></remarks>
        Public Sub PMThreadDelete(ByVal PMThreadID As Integer, ByVal PMUserID As Integer)
            DotNetNuke.Modules.Forum.DataProvider.Instance().PMThreadDelete(PMThreadID, PMUserID)
        End Sub

    End Class

#End Region

End Namespace