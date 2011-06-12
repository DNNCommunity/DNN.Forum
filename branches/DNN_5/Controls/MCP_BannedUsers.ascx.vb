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

Namespace DotNetNuke.Modules.Forum.MCP

	''' <summary>
	''' This is the MCP section that shows an overview about the mcp.
	''' </summary>
	''' <remarks>
	''' </remarks>
    Partial Public Class BannedUsers
        Inherits ForumModuleBase
        Implements Utilities.AjaxLoader.IPageLoad

#Region "Private Members"

        Private _PageSize As Integer

#End Region

#Region "Interfaces"

        ''' <summary>
        ''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            dnngridBannedUsers.PageSize = Convert.ToInt32(CurrentForumUser.ThreadsPerPage)
            BindData(dnngridBannedUsers.PageSize, 1)
        End Sub

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Used to set properties for various sever controls used in the item template.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub dnngridBannedUsers_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles dnngridBannedUsers.ItemDataBound
            If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
                Dim keyID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("UserID"))
                Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
                Dim objUser As ForumUserInfo = CType(e.Item.DataItem, ForumUserInfo)

                Dim hl As HyperLink

                hl = DirectCast((dataItem)("User").Controls(0), HyperLink)
                hl.Text = objUser.SiteAlias
                If Not objConfig.EnableExternalProfile Then
                    hl.NavigateUrl = objUser.UserCoreProfileLink
                Else
                    hl.NavigateUrl = Utilities.Links.UserExternalProfileLink(keyID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objUser.Username)
                End If

            End If
            'Dim lbl As Label
            'lbl = CType(e.Item.FindControl("lblStartBanDate"), Label)
            'lbl.Text = FormatDate(dataItem.StartBanDate)
            'lbl = CType(e.Item.FindControl("lblLiftBanDate"), Label)
            'lbl.Text = FormatDate(dataItem.LiftBanDate)
        End Sub

        ''' <summary>
        ''' Fires when a command button is clicked in the users grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub dnngridBannedUsers_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles dnngridBannedUsers.ItemCommand
            If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then
                Select Case e.CommandName
                    Case "EditUser"
                        Dim keyID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("UserID"))
                        Response.Redirect(Utilities.Links.UCP_AdminLinks(TabId, ModuleId, keyID, UserAjaxControl.Profile), True)
                End Select
            End If
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Binds the PM threads for a specific user, if none exist it will hide grid view, show notice to user.
        ''' </summary>
        ''' <param name="PageSize"></param>
        ''' <param name="CurrentPage"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Private Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
            Dim cntUser As New ForumUserController
            Dim arrUsers As New List(Of ForumUserInfo)
            Dim TotalRecords As Integer

            arrUsers = cntUser.GetBannedUsers(PortalId, CurrentPage - 1, PageSize, ModuleId, TotalRecords)

            dnngridBannedUsers.DataSource = arrUsers
            dnngridBannedUsers.DataBind()

            If Not arrUsers Is Nothing Then
                dnngridBannedUsers.VirtualItemCount = arrUsers.Count
            End If
        End Sub

        ''' <summary>
        ''' Formats the start/end dates for banning.
        ''' </summary>
        ''' <param name="objDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function FormatDate(ByVal objDate As Date) As String
            Dim srtDate As String = String.Empty
            srtDate = Utilities.ForumUtils.GetCreatedDateInfo(objDate, objConfig, "Forum_LastPostText")
            Return srtDate
        End Function

#End Region

    End Class

End Namespace