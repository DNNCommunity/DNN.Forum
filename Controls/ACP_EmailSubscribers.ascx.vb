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

Imports Telerik.Web.UI



Namespace DotNetNuke.Modules.Forum.ACP

    ''' <summary>
    ''' This screen is used to manage the email configuration queue cleanup
    ''' This can only be adjusted by SuperUsers
    ''' </summary>
    ''' <history>
    ''' Bart³omiej Waluszko 12/23/2010 Updated
    ''' </history>
    ''' <remarks>'TODO: Enable Grid sorting, now index out of range is thrown</remarks>
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
                If (Not String.IsNullOrEmpty(hdnRcbThreadsValue.Value)) AndAlso Integer.TryParse(hdnRcbThreadsValue.Value, _threadID) Then 'take ThreadID from hidden field

                ElseIf Request.QueryString("ThreadID") IsNot Nothing Then 'take ThreadID from QueryString
                    Integer.TryParse(Request.QueryString("ThreadID"), _threadID)
                Else
                    'take threadID from ForumID 
                    Dim trackingController As New TrackingController
                    Dim trackingInfo As List(Of TrackingInfo)
                    trackingInfo = trackingController.TrackingThreadsGetByForumID(ForumID)
                    If trackingInfo IsNot Nothing AndAlso trackingInfo.Count > 0 Then
                        _threadID = trackingInfo(0).ThreadID
                    End If
                End If
                Return _threadID
            End Get
        End Property

        Private QueryString_IsEnable As Boolean = False

#End Region

#Region "Interfaces"

        ''' <summary>
        ''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            'bind grids
            GridForumBind()
            GridThreadBind()
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
            'if it runs from Forum or Post list, disable ComboBoxes
            If Request.QueryString("ThreadID") IsNot Nothing Or Request.QueryString("ForumID") IsNot Nothing Then
                QueryString_IsEnable = True
                lnkShowAll.Visible = True
            End If
        End Sub

        ''' <summary>
        ''' Show Grids and relaed ComboBox's
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub lnkShowAll_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkShowAll.Click
            Dim params As String()
            params = New String(1) {"mid=" & ModuleId.ToString(), "view=" & CStr(AdminAjaxControl.EmailSubscribers)}
            Response.Redirect(NavigateURL(TabId, ForumPage.ACP.ToString(), params))
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
                    hdnRcbThreadsValue.Value = ""
                    GridForumBind()
                    GridThreadBind()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Bind rcbForums ComboBox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub rgForums_ItemDataBound(ByVal sender As System.Object, ByVal e As GridItemEventArgs) Handles rgForums.ItemDataBound
            If TypeOf e.Item Is GridCommandItem Then
                Dim commandItem As GridCommandItem = CType(e.Item, GridCommandItem)
                Dim rcbForums As RadComboBox = CType(commandItem.FindControl("rcbForums"), RadComboBox)
                Dim forumController As New ForumController
                Dim forumList As List(Of ForumInfo)
                forumList = forumController.GetModuleForums(ModuleId)

                If QueryString_IsEnable Then
                    rcbForums.Visible = False
                Else
                    rcbForums.DataTextField = "Name"
                    rcbForums.DataValueField = "ForumID"
                    rcbForums.DataSource = forumList
                    rcbForums.DataBind()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Update SelectedValue in rcbForums ComboBox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub rgForums_PreRender(ByVal sender As System.Object, ByVal e As EventArgs) Handles rgForums.PreRender
            If Not String.IsNullOrEmpty(hdnRcbForumsValue.Value) Then
                Dim gridCommandItem As GridCommandItem = CType(rgForums.MasterTableView.GetItems(GridItemType.CommandItem)(0), GridCommandItem)
                Dim rcbForums As RadComboBox = CType(gridCommandItem.FindControl("rcbForums"), RadComboBox)
                rcbForums.SelectedValue = hdnRcbForumsValue.Value
            End If
        End Sub

        ''' <summary>
        ''' Used to bind data to the forum subscriptions grid. 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GridForumBind()
            Dim userTrackingController As New UserTrackingController
            rgForums.AutoGenerateColumns = False 'AutoGenerateColumns didn't works in *.ascx
            If ForumID <> -1 Then
                rgForums.DataSource = userTrackingController.GetForumSubscribers(ForumID)
            Else 'get first forum belong to this module
                Dim forumController As New ForumController
                Dim forumInfo As List(Of ForumInfo) = forumController.GetModuleForums(ModuleId)
                If (forumInfo IsNot Nothing) AndAlso forumInfo.Count > 0 Then
                    hdnRcbForumsValue.Value = forumInfo(0).ForumID.ToString()
                    rgForums.DataSource = userTrackingController.GetForumSubscribers(ForumID)

                End If
            End If
            rgForums.DataBind()
        End Sub

        ''' <summary>
        ''' If QueryString exist make forum name visible
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rgForums_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles rgForums.ItemCreated
            If TypeOf e.Item Is GridCommandItem Then
                Dim commandItem As GridCommandItem = CType(e.Item, GridCommandItem)
                Dim lnkHeader As Label = CType(commandItem.FindControl("lblForumTitle"), Label)

                If QueryString_IsEnable Then
                    Dim forumController As New ForumController
                    Dim forumInfo As ForumInfo = forumController.GetModuleForums(ModuleId).Where(Function(f) f.ForumID = ForumID).FirstOrDefault()
                    If forumInfo IsNot Nothing Then
                        lnkHeader.Text = """" & forumInfo.Name & """"
                    End If
                    lnkHeader.Visible = True
                Else
                    lnkHeader.Visible = False
                End If


                'localize Forum grid headers
            ElseIf TypeOf e.Item Is GridHeaderItem Then
                Dim headerItem As GridHeaderItem = CType(e.Item, GridHeaderItem)

                Dim label As LiteralControl = CType(headerItem("Username").Controls(0), LiteralControl)
                label.Text = Localization.GetString("Username.Header", Me.LocalResourceFile)

                label = CType(headerItem("DisplayName").Controls(0), LiteralControl)
                label.Text = Localization.GetString("DisplayName.Header", Me.LocalResourceFile)

                label = CType(headerItem("Email").Controls(0), LiteralControl)
                label.Text = Localization.GetString("Email.Header", Me.LocalResourceFile)

                label = CType(headerItem("CreatedDate").Controls(0), LiteralControl)
                label.Text = Localization.GetString("CreatedDate.Header", Me.LocalResourceFile)
            End If
        End Sub


#End Region

#Region "rgThreads"

        ''' <summary>
        ''' Fires when a user changes the rcbThreads RadComboBox
        ''' Save current threadID in hidden field
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rcbThreads_SelectedIndexChange(ByVal source As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
            If TypeOf source Is RadComboBox Then
                Dim rcbThreads As RadComboBox = CType(source, RadComboBox)
                Dim _threadID As Integer
                If Integer.TryParse(rcbThreads.SelectedValue, _threadID) Then
                    hdnRcbThreadsValue.Value = _threadID.ToString()
                    GridThreadBind()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Bind rcbThreads ComboBox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub rgThreads_ItemDataBound(ByVal sender As System.Object, ByVal e As GridItemEventArgs) Handles rgThreads.ItemDataBound
            If (TypeOf e.Item Is GridCommandItem) Then
                Dim commandItem As GridCommandItem = CType(e.Item, GridCommandItem)
                Dim rcbThreads As RadComboBox = CType(commandItem.FindControl("rcbThreads"), RadComboBox)
                If QueryString_IsEnable Then
                    rcbThreads.Visible = False
                Else
                    rcbThreads.DataTextField = "Subject"
                    rcbThreads.DataValueField = "ThreadID"
                    Dim trackingController As New TrackingController
                    rcbThreads.DataSource = trackingController.TrackingThreadsGetByForumID(ForumID)
                    rcbThreads.DataBind()
                End If

            End If
        End Sub

        ''' <summary>
        ''' Update SelectedValue in rcbThreads ComboBox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub rgThreads_PreRender(ByVal sender As System.Object, ByVal e As EventArgs) Handles rgThreads.PreRender
            If Not String.IsNullOrEmpty(hdnRcbThreadsValue.Value) Then
                Dim gridCommandItem As GridCommandItem = CType(rgThreads.MasterTableView.GetItems(GridItemType.CommandItem)(0), GridCommandItem)
                Dim rcbThreads As RadComboBox = CType(gridCommandItem.FindControl("rcbThreads"), RadComboBox)
                rcbThreads.SelectedValue = hdnRcbThreadsValue.Value()
            End If
        End Sub

        ''' <summary>
        ''' Used to bind data to the thread subscriptions grid. 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GridThreadBind()
            Dim userTrackingController As New UserTrackingController
            rgThreads.DataSource = userTrackingController.GetThreadSubscribers(ThreadID)
            rgThreads.DataBind()
        End Sub

        ''' <summary>
        ''' If QueryString exist make thread subject visible
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rgThreads_ItemCreated(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles rgThreads.ItemCreated
            If TypeOf e.Item Is GridCommandItem Then
                Dim commandItem As GridCommandItem = CType(e.Item, GridCommandItem)
                Dim lnkHeader As Label = CType(commandItem.FindControl("lblThreadTitle"), Label)

                If QueryString_IsEnable Then
                    Dim threadController As New ThreadController
                    Dim threadInfo As ThreadInfo = threadController.GetThread(ThreadID)
                    If (threadInfo IsNot Nothing) Then
                        lnkHeader.Text = """" & threadInfo.Subject & """"
                    End If
                    lnkHeader.Visible = True
                Else
                    lnkHeader.Visible = False
                End If

                'localize Thread grid headers
            ElseIf TypeOf e.Item Is GridHeaderItem Then
                Dim headerItem As GridHeaderItem = CType(e.Item, GridHeaderItem)

                Dim label As LiteralControl = CType(headerItem("Email").Controls(0), LiteralControl)
                label.Text = Localization.GetString("Email.Header", Me.LocalResourceFile)

                label = CType(headerItem("Username").Controls(0), LiteralControl)
                label.Text = Localization.GetString("Username.Header", Me.LocalResourceFile)

                label = CType(headerItem("DisplayName").Controls(0), LiteralControl)
                label.Text = Localization.GetString("DisplayName.Header", Me.LocalResourceFile)

                label = CType(headerItem("Subject").Controls(0), LiteralControl)
                label.Text = Localization.GetString("Subject.Header", Me.LocalResourceFile)

                label = CType(headerItem("CreatedDate").Controls(0), LiteralControl)
                label.Text = Localization.GetString("CreatedDate.Header", Me.LocalResourceFile)
            End If
        End Sub
#End Region

#End Region

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

    End Class

End Namespace