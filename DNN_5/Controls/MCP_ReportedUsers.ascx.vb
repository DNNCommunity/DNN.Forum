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
	''' This is the MCP section that shows a list of users who have had posts reported.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class ReportedUsers
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Private Members"

		Private _PageSize As Integer

#End Region

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks>So far this is a static page</remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            dnngridReportedUsers.PageSize = Convert.ToInt32(CurrentForumUser.ThreadsPerPage)
            BindData(dnngridReportedUsers.PageSize, 1)
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Used to set properties for various sever controls used in the item template.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
        Protected Sub dgReportedUsers_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles dnngridReportedUsers.ItemDataBound
            If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
                Dim keyID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("UserID"))
                Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
                Dim objUser As ReportedUserInfo = CType(e.Item.DataItem, ReportedUserInfo)

                Dim hl As HyperLink
                hl = DirectCast((dataItem)("User").Controls(0), HyperLink)
                hl.Text = objUser.Author(ModuleId, PortalId).SiteAlias

                If Not objConfig.EnableExternalProfile Then
                    hl.NavigateUrl = objUser.Author(ModuleId, PortalId).UserCoreProfileLink
                Else
                    hl.NavigateUrl = Utilities.Links.UserExternalProfileLink(keyID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objUser.Author(ModuleId, PortalId).Username)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Fires when a command button is clicked in the users grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub dnngridReportedUsers_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles dnngridReportedUsers.ItemCommand
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
		''' Displays a list of users who have unaddressed reported posts. 
		''' </summary>
		''' <param name="PageSize"></param>
		''' <param name="CurrentPage"></param>
		''' <remarks>
		''' </remarks>
		Private Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer)
			Dim cntReportedUsers As New ReportedUserController
			Dim arrReportedUsers As New List(Of ReportedUserInfo)
            arrReportedUsers = cntReportedUsers.GetReportedUsers(PortalId, CurrentPage - 1, PageSize)

            If arrReportedUsers.Count > 0 Then
                Dim objReportedUserInfo As ReportedUserInfo = arrReportedUsers.Item(0)

                dnngridReportedUsers.DataSource = arrReportedUsers
                dnngridReportedUsers.DataBind()

                If arrReportedUsers.Count > 0 Then
                    dnngridReportedUsers.VirtualItemCount = arrReportedUsers(0).TotalRecords
                Else
                    dnngridReportedUsers.VirtualItemCount = 0
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace