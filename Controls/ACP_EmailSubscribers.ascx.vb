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

Namespace DotNetNuke.Modules.Forum.ACP

    ''' <summary>
    ''' This screen is used to manage the email configuration queue cleanup
    ''' This can only be adjusted by SuperUsers
    ''' </summary>
    ''' <history>
    ''' Bart³omiej Waluszko 12/23/2010 Updated
    ''' </history>
    ''' <remarks></remarks>
    Partial Public Class EmailSubscribers
        Inherits ForumModuleBase
        Implements Utilities.AjaxLoader.IPageLoad

#Region "Private Properties"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides ReadOnly Property ForumID As Integer
            Get
                Dim _forumID As Integer
                If (Not String.IsNullOrEmpty(hdnRcbForumsValue.Value)) AndAlso Integer.TryParse(hdnRcbForumsValue.Value, _forumID) Then
                    Return _forumID
                Else
                    Return MyBase.ForumID
                End If
            End Get
        End Property

        ''' <summary>
        ''' If this was linked to from the posts view, this variable is populated to also display a list of subscribers at the thread level.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property ThreadID As Integer
            Get
                Dim _threadID As Integer = -1
                If Request.QueryString("ThreadID") IsNot Nothing Then 'take ThreadID from QueryString
                    Integer.TryParse(Request.QueryString("ThreadID"), _threadID)
                End If
                Return _threadID
            End Get
        End Property

#End Region

#Region "Interfaces"

        ''' <summary>
        ''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView

        End Sub

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Runs when the control is initialized, even before anything in LoadInitialView runs. 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>All controls containing grids should localize the grid headers here. </remarks>
        Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            Dim _forumID As Integer
            If (Request.QueryString("ForumID") IsNot Nothing) AndAlso (Integer.TryParse(Request.QueryString("ForumID"), _forumID)) Then 'take ThreadID from QueryString
                hdnRcbForumsValue.Value = _forumID.ToString()
            End If

            'ForumID & ThreadID is in QueryString
            If ForumID <> -1 AndAlso ThreadID <> -1 Then
                rgForums.Visible = True
                rgThreads.Visible = True

            Else 'show only rgForums
                rgForums.Visible = True
                rgThreads.Visible = False
            End If
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

#Region "rgForums"

        ''' <summary>
        ''' Fires when a user changes the rcbForums RadComboBox
        ''' Save current forumID in hidden field 
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rcbForums_SelectedIndexChange(ByVal source As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
            If TypeOf source Is RadComboBox Then
                Dim rcbForums As RadComboBox = CType(source, RadComboBox)
                Dim forumID As Integer
                If Integer.TryParse(rcbForums.SelectedValue, forumID) Then
                    hdnRcbForumsValue.Value = forumID.ToString()
                    rgForums.Rebind()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Bind rcbForums ComboBox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rgForums_ItemDataBound(ByVal sender As System.Object, ByVal e As GridItemEventArgs) Handles rgForums.ItemDataBound
            If TypeOf e.Item Is GridCommandItem Then
                Dim commandItem As GridCommandItem = CType(e.Item, GridCommandItem)
                Dim rcbForums As RadComboBox = CType(commandItem.FindControl("rcbForums"), RadComboBox)
                Dim forumController As New ForumController
                Dim forumList As List(Of ForumInfo)
                forumList = forumController.GetModuleForums(ModuleId)

                rcbForums.DataTextField = "Name"
                rcbForums.DataValueField = "ForumID"
                rcbForums.DataSource = forumList
                rcbForums.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Update SelectedValue in rcbForums ComboBox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rgForums_PreRender(ByVal sender As System.Object, ByVal e As EventArgs) Handles rgForums.PreRender
            If Not String.IsNullOrEmpty(hdnRcbForumsValue.Value) Then
                Dim gridCommandItem As GridCommandItem = CType(rgForums.MasterTableView.GetItems(GridItemType.CommandItem)(0), GridCommandItem)
                Dim rcbForums As RadComboBox = CType(gridCommandItem.FindControl("rcbForums"), RadComboBox)
                rcbForums.SelectedValue = ForumID.ToString()
            End If
        End Sub

        ''' <summary>
        ''' rgForums - Advanced data binding
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rgForums_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgForums.NeedDataSource
            Dim userTrackingController As New UserTrackingController
            If ForumID <> -1 Then
                rgForums.DataSource = userTrackingController.GetForumSubscribers(ForumID)
            Else
                'get first forum belong to this module
                Dim forumController As New ForumController
                Dim forumInfo As List(Of ForumInfo) = forumController.GetModuleForums(ModuleId)

                If (forumInfo IsNot Nothing) AndAlso forumInfo.Count > 0 Then
                    hdnRcbForumsValue.Value = forumInfo(0).ForumID.ToString()
                    rgForums.DataSource = userTrackingController.GetForumSubscribers(ForumID)
                End If
            End If
        End Sub

#End Region

#Region "rgThreads"

        ''' <summary>
        ''' Make thread subject visible
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rgThreads_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles rgThreads.ItemCreated
            If TypeOf e.Item Is GridCommandItem Then
                Dim commandItem As GridCommandItem = CType(e.Item, GridCommandItem)
                Dim lnkHeader As Label = CType(commandItem.FindControl("lblThreadTitle"), Label)

                Dim threadController As New ThreadController
                Dim threadInfo As ThreadInfo = threadController.GetThread(ThreadID)
                If (threadInfo IsNot Nothing) Then
                    lnkHeader.Text = """" & threadInfo.Subject & """"
                    lnkHeader.Visible = True
                End If
            End If
        End Sub

        ''' <summary>
        ''' rgThreads - Advanced data binding
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rgThreads_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgThreads.NeedDataSource
            Dim userTrackingController As New UserTrackingController
            rgThreads.DataSource = userTrackingController.GetThreadSubscribers(ThreadID)
        End Sub
#End Region

#End Region

    End Class

End Namespace