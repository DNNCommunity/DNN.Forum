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

#Region "GroupController"

    ''' <summary>
    ''' CRUD (and all db methods) Group database methods
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	7/13/2005	Created
    ''' </history>
    Public Class GroupController

#Region "Private Members"

        Private Const GroupInfoCachePrefix As String = "ForumAllGroups"
        Private Const GroupInfoCacheTimeout As Integer = 20

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Get all Groups based on moduleId. Cache this when possible.
        ''' </summary>
        ''' <param name="ModuleId">The ModuleID being used to retrieve all groups.</param>
        ''' <returns>An arraylist of all groups corresponding to the supplied ModuleID.</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	2/11/2006	Created
        ''' </history>
        Public Function GroupsGetByModuleID(ByVal ModuleId As Integer) As List(Of GroupInfo)
            Dim strCacheKey As String = GroupInfoCachePrefix & ModuleId.ToString()
            Dim arrGroups As New List(Of GroupInfo)
            arrGroups = CType(DataCache.GetCache(strCacheKey), List(Of GroupInfo))

            If arrGroups Is Nothing Then
				Dim timeOut As Int32 = GroupInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

                arrGroups = CBO.FillCollection(Of GroupInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GroupGetByModuleID(ModuleId))

                'Cache Group if timeout > 0 and Group is not null
                If timeOut > 0 And arrGroups IsNot Nothing Then
					DataCache.SetCache(strCacheKey, arrGroups, TimeSpan.FromMinutes(timeOut))
                End If
            End If

            Return arrGroups
        End Function

        ''' <summary>
        ''' Used to clear the group cache. (All groups are cached)
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <remarks></remarks>
        Public Sub ResetAllGroupsByModuleID(ByVal ModuleID As Integer)
            Dim strCacheKey As String = GroupInfoCachePrefix & ModuleID.ToString()

            DataCache.RemoveCache(strCacheKey)
        End Sub

        ''' <summary>
        ''' Returns all groups with at least one authorized forum.
        ''' </summary>
        ''' <param name="ModuleId">Integer</param>
        ''' <param name="UserID">The UserID to return results for. (Because of private permissions)</param>
        ''' <param name="NoLinkForums">True if Link Type forums should be excluded.</param>
        ''' <returns>A Generics arraylist of GroupInfo items.</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	2/11/2006	Created
        ''' </history>
        Public Function GroupGetAllAuthorized(ByVal ModuleId As Integer, ByVal UserID As Integer, ByVal NoLinkForums As Boolean) As List(Of GroupInfo)
            Dim arrAllGroups As New List(Of GroupInfo)
            Dim arrAuthGroups As New List(Of GroupInfo)
            arrAllGroups = GroupsGetByModuleID(ModuleId)

            If arrAllGroups.Count > 0 Then
                For Each objGroup As GroupInfo In arrAllGroups
                    Dim arrAuthForums As New List(Of ForumInfo)
                    arrAuthForums = objGroup.AuthorizedForums(UserID, NoLinkForums)
                    If arrAuthForums.Count > 0 Then
                        arrAuthGroups.Add(objGroup)
                    End If
                Next
            End If

            Return arrAuthGroups
        End Function

        ''' <summary>
        ''' Gets a single group
        ''' </summary>
        ''' <param name="GroupId">Integer</param>
        ''' <returns>GroupInfo</returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	2/11/2006	Created
        ''' </history>
        Public Function GroupGet(ByVal GroupId As Integer) As GroupInfo
            Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().GroupGet(GroupId), GetType(GroupInfo)), GroupInfo)
        End Function

        ''' <summary>
        ''' Adds a new group to the database. 
        ''' </summary>
        ''' <param name="Name"></param>
        ''' <param name="PortalID"></param>
        ''' <param name="ModuleId"></param>
        ''' <param name="CreatedByUser"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GroupAdd(ByVal Name As String, ByVal PortalID As Integer, ByVal ModuleId As Integer, ByVal CreatedByUser As Integer) As Integer
            Return DotNetNuke.Modules.Forum.DataProvider.Instance().GroupAdd(Name, PortalID, ModuleId, CreatedByUser)
        End Function

        ''' <summary>
        ''' Delete's a forum group 
        ''' (Should not happen when there are forums within it)
        ''' </summary>
        ''' <param name="GroupID">The PK value associated with the Group.</param>
        ''' <param name="ModuleID">The module instance the group is tied to.</param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	2/11/2006	Created
        ''' </history>
        Public Sub GroupDelete(ByVal GroupID As Integer, ByVal ModuleID As Integer)
            DotNetNuke.Modules.Forum.DataProvider.Instance().GroupDelete(GroupID, ModuleID)
        End Sub

        ''' <summary>
        ''' Updates an existing group in the database. 
        ''' </summary>
        ''' <param name="GroupID"></param>
        ''' <param name="Name"></param>
        ''' <param name="UpdatedByUser"></param>
        ''' <param name="SortOrder"></param>
        ''' <param name="ModuleID"></param>
        ''' <remarks></remarks>
        Public Sub GroupUpdate(ByVal GroupID As Integer, ByVal Name As String, ByVal UpdatedByUser As Integer, ByVal SortOrder As Integer, ByVal ModuleID As Integer)
            DotNetNuke.Modules.Forum.DataProvider.Instance().GroupUpdate(GroupID, Name, UpdatedByUser, SortOrder, ModuleID)
        End Sub

        ''' <summary>
        ''' Updates the view order of the groups (top to bottom) - Moves up
        ''' or down one level
        ''' </summary>
        ''' <param name="GroupID">Integer</param>
        ''' <param name="MoveUp">Boolean</param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	2/11/2006	Created
        ''' </history>
        Public Sub GroupSortOrderUpdate(ByVal GroupID As Integer, ByVal MoveUp As Boolean)
            DotNetNuke.Modules.Forum.DataProvider.Instance().GroupSortOrderUpdate(GroupID, MoveUp)
        End Sub

#End Region

    End Class

#End Region

End Namespace