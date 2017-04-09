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

Imports Telerik.Web.UI

Namespace DotNetNuke.Modules.Forum.UCP

    ''' <summary>
    ''' This is the UCP section that show what forums and threads the user is tracking
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Partial Public Class Tracking
        Inherits ForumModuleBase
        Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

        ''' <summary>
        ''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            BuildTabs()
            BindForumData(CurrentForumUser.ThreadsPerPage, 0)
            BindThreadData(CurrentForumUser.ThreadsPerPage, 0)
        End Sub

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Alters data as it is bound to the forum grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gridForumTracking_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gridForumTracking.ItemDataBound
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = CType(e.Item, GridDataItem)
                Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ForumID"))
                Dim keyMostRecentPostID As Integer = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("MostRecentPostID"))

                Dim imgDelete As ImageButton = CType((item)("imgDelete").Controls(0), ImageButton)
                imgDelete.ToolTip = Localization.GetString("Delete", LocalResourceFile)
                imgDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');")
                imgDelete.CommandArgument = keyForumID.ToString()

                Dim cntForum As ForumController
                cntForum = New ForumController()
                Dim objForum As ForumInfo = cntForum.GetForumItemCache(keyForumID)

                Dim hlForum As HyperLink = CType((item)("hlName").Controls(0), HyperLink)
                hlForum.NavigateUrl = Utilities.Links.ContainerViewForumLink(PortalId, TabId, keyForumID, False, objForum.Name)

                Dim hlLastPost As HyperLink = CType((item)("hlLastPost").Controls(0), HyperLink)
                Dim cntPost As New PostController
                If keyMostRecentPostID > 0 Then
                    Dim objPost As PostInfo = cntPost.GetPostInfo(keyMostRecentPostID, PortalId)
                    hlLastPost.Text = Utilities.ForumUtils.GetCreatedDateInfo(objPost.CreatedDate, objConfig, "")
                    hlLastPost.NavigateUrl = Utilities.Links.ContainerViewPostLink(TabId, keyForumID, objPost.PostID)
                Else
                    hlLastPost.Text = "-"
                End If
            End If
        End Sub

        ''' <summary>
        ''' Fires when a command button is clicked in the forum grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gridForumTracking_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles gridForumTracking.ItemCommand
            If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
                Select Case e.CommandName
                    Case "DeleteItem"
                        Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ForumID"))
                        Dim cntTracking As New TrackingController
                        cntTracking.TrackingForumCreateDelete(keyForumID, ProfileUserID, False, ModuleId)
                        BindForumData(CurrentForumUser.ThreadsPerPage, 0)
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Fired when the user changes the selected page for hte forum tracking grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gridForumTracking_PageIndexChanged(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles gridForumTracking.PageIndexChanged
            BindForumData(CurrentForumUser.ThreadsPerPage, e.NewPageIndex)
        End Sub

        ''' <summary>
        ''' Alters data as it is bound to the threads grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gridThreadTracking_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles gridThreadTracking.ItemDataBound
            If TypeOf e.Item Is GridDataItem Then
                Dim item As GridDataItem = CType(e.Item, GridDataItem)
                Dim keyThreadID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ThreadID"))
                Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(item.ItemIndex)("ForumID"))
                Dim keyMostRecentPostID As Integer = CInt(item.OwnerTableView.DataKeyValues(item.ItemIndex)("MostRecentPostID"))

                Dim imgDelete As ImageButton = CType((item)("imgDelete").Controls(0), ImageButton)
                imgDelete.ToolTip = Localization.GetString("Delete", LocalResourceFile)
                imgDelete.Attributes.Add("onClick", "javascript:return confirm('" + Localization.GetString("DeleteItem") + "');")
                imgDelete.CommandArgument = keyThreadID.ToString()

                Dim tCont As ThreadController
                tCont = New ThreadController()
                Dim tInfo As ThreadInfo
                tInfo = tCont.GetThread(keyThreadID)
                Dim hlForum As HyperLink = CType((item)("hlSubject").Controls(0), HyperLink)
                hlForum.NavigateUrl = Utilities.Links.ContainerViewThreadLink(PortalId, TabId, keyForumID, keyThreadID, tInfo.Subject)

                Dim hlLastPost As HyperLink = CType((item)("hlLastPost").Controls(0), HyperLink)
                Dim cntPost As New PostController
                If keyMostRecentPostID > 0 Then
                    Dim objPost As PostInfo = cntPost.GetPostInfo(keyMostRecentPostID, PortalId)
                    hlLastPost.Text = Utilities.ForumUtils.GetCreatedDateInfo(objPost.CreatedDate, objConfig, "")
                    hlLastPost.NavigateUrl = Utilities.Links.ContainerViewPostLink(TabId, keyThreadID, objPost.PostID)
                Else
                    hlLastPost.Text = "-"
                End If
            End If
        End Sub

        ''' <summary>
        ''' Executes when a command button is clicked in the grid, currently this involves a delete button only. 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gridThreadTracking_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles gridThreadTracking.ItemCommand
            If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
                Select Case e.CommandName
                    Case "DeleteItem"
                        Dim keyThreadID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ThreadID"))
                        Dim keyForumID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("ForumID"))

                        Dim cntTracking As New TrackingController
                        cntTracking.TrackingThreadCreateDelete(keyForumID, keyThreadID, ProfileUserID, False, ModuleId)
                        BindThreadData(CurrentForumUser.ThreadsPerPage, 0)
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Fired when the user changes the selected page for the thread tracking grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gridThreadTracking_PageIndexChanged(ByVal sender As Object, ByVal e As GridPageChangedEventArgs) Handles gridThreadTracking.PageIndexChanged
            BindThreadData(CurrentForumUser.ThreadsPerPage, e.NewPageIndex)
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Uses localization to build the tabs in the user interface (for the tabstrip). 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BuildTabs()
            Dim tabForums As New Telerik.Web.UI.RadTab
            tabForums.Text = Localization.GetString("TabForums", LocalResourceFile)
            tabForums.PageViewID = "rpvForums"
            rtsNotifications.Tabs.Add(tabForums)

            Dim tabThreads As New Telerik.Web.UI.RadTab
            tabThreads.Text = Localization.GetString("TabThreads", LocalResourceFile)
            tabThreads.PageViewID = "rpvThreads"
            rtsNotifications.Tabs.Add(tabThreads)
        End Sub

        ''' <summary>
        ''' Binds a list of tracked forums for a specific user to a datagrid. 
        ''' </summary>
        ''' <param name="PageSize"></param>
        ''' <param name="CurrentPage"></param>
        ''' <remarks></remarks>
        Private Sub BindForumData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
            Dim TrackCtl As New TrackingController
            Dim colTrackedForums As New List(Of ForumInfo)

            colTrackedForums = TrackCtl.GetUsersForumsTracked(ProfileUserID, ModuleId, PageSize, CurrentPage)

            gridForumTracking.DataSource = colTrackedForums
            gridForumTracking.DataBind()

            If colTrackedForums.Count > 0 Then
                gridForumTracking.VirtualItemCount = colTrackedForums(0).TotalRecords
            Else
                gridForumTracking.VirtualItemCount = 0
            End If
        End Sub

        ''' <summary>
        ''' Binds a list of tracked threads for a specific user to a datagrid. 
        ''' </summary>
        ''' <param name="PageSize"></param>
        ''' <param name="CurrentPage"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Private Sub BindThreadData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
            Dim TrackCtl As New TrackingController
            Dim colThreads As New List(Of TrackingInfo)

            colThreads = TrackCtl.TrackingThreadGetAll(ProfileUserID, ModuleId, PageSize, CurrentPage)

            gridThreadTracking.DataSource = colThreads
            gridThreadTracking.DataBind()

            If colThreads.Count > 0 Then
                gridThreadTracking.VirtualItemCount = colThreads(0).TotalRecords
            Else
                gridThreadTracking.VirtualItemCount = 0
            End If
        End Sub

        ' ''' <summary>
        ' ''' Localizes the data grid headers for all grids on the page (that utilize Telerik).
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Private Sub SetLocalization()
        '	For Each gc As GridColumn In gridForumTracking.MasterTableView.Columns
        '		If gc.HeaderText <> "" Then
        '			gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile)
        '		End If
        '	Next

        '	For Each gc As GridColumn In gridThreadTracking.MasterTableView.Columns
        '		If gc.HeaderText <> "" Then
        '			gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", LocalResourceFile)
        '		End If
        '	Next
        'End Sub

#End Region

    End Class

End Namespace