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

#Region "ForumPMReadsController"

    ''' <summary>
    ''' A Forum PM Read Controller populates the corresponding Info object. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ForumPMReadsController

#Region "Private Members"

        Private Const FORUM_USERNEWPM_CACHE_KEY_PREFIX As String = "Forum_UserNewPMCount-"
        Private Const UserNewPMCountCacheTimeout As Integer = 20

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Gets the number of private messages a specific user has not read yet.
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PortalID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function PMGetUserNewMessageCount(ByVal UserID As Integer, ByVal PortalID As Integer) As Integer
            Return CType(DataProvider.Instance().PMGetUserNewMessageCount(UserID, PortalID), Integer)
        End Function

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Gets the thread read status for a single thread for a single user
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PMThreadID"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[jmathis]	12/3/2005	Created
        ''' </history>
        Public Function PMReadsGet(ByVal UserID As Integer, ByVal PMThreadID As Integer) As ForumPMReadsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().PMReadsGet(UserID, PMThreadID), GetType(ForumPMReadsInfo)), ForumPMReadsInfo)
        End Function

        ''' <summary>
        ''' Adds a read status
        ''' </summary>
        ''' <param name="objPMUserReads"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Public Sub PMReadsAdd(ByVal objPMUserReads As ForumPMReadsInfo)
            DataProvider.Instance().PMReadsAdd(objPMUserReads.UserID, objPMUserReads.PMThreadID, objPMUserReads.LastVisitDate, objPMUserReads.PortalID)
        End Sub

        ''' <summary>
        ''' Updates a read status
        ''' </summary>
        ''' <param name="objPMUserReads"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Public Sub PMReadsUpdate(ByVal objPMUserReads As ForumPMReadsInfo)
            DataProvider.Instance().PMReadsUpdate(objPMUserReads.UserID, objPMUserReads.PMThreadID, objPMUserReads.LastVisitDate)
        End Sub

#Region "Caching"

        ''' <summary>
        ''' Gets the number of private messages a specific user has not read yet and caches it.
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUserNewPMCount(ByVal UserID As Integer) As Integer
            Dim _portalSettings As Portals.PortalSettings = Portals.PortalController.GetCurrentPortalSettings
            Dim keyID As String = FORUM_USERNEWPM_CACHE_KEY_PREFIX & UserID.ToString & "-" & _portalSettings.PortalId.ToString
			Dim timeOut As Int32 = UserNewPMCountCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

            Dim NewPMCount As Integer = -1
            If Not DataCache.GetCache(keyID) Is Nothing Then
                NewPMCount = CType(DataCache.GetCache(keyID), Integer)
            End If

            If NewPMCount = -1 Then
                NewPMCount = PMGetUserNewMessageCount(UserID, _portalSettings.PortalId)
                If timeOut > 0 And NewPMCount <> -1 Then
					DataCache.SetCache(keyID, NewPMCount, TimeSpan.FromMinutes(timeOut))
                End If
            End If

            Return NewPMCount
        End Function

        ''' <summary>
        ''' Resets the cached user PM Count object
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	12/4/2005	Created
        ''' </history>
        Public Shared Sub ResetUserNewPMCount(ByVal UserID As Integer)
            Dim _portalSettings As Portals.PortalSettings = Portals.PortalController.GetCurrentPortalSettings
            Dim keyID As String = FORUM_USERNEWPM_CACHE_KEY_PREFIX & UserID.ToString & "-" & _portalSettings.PortalId.ToString
            DataCache.RemoveCache(keyID)
        End Sub

#End Region

#End Region

    End Class

#End Region

End Namespace
