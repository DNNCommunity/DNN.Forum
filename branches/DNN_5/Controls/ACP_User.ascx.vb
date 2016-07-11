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
Imports DotNetNuke.Security.Roles

Namespace DotNetNuke.Modules.Forum.ACP

    ''' <summary>
    ''' Allows forum administrators to manage all forum users. This lists all portal users to allow administrator to manage those users whom have not yet visited the module as well. 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Partial Public Class User
        Inherits ForumModuleBase
        Implements Utilities.AjaxLoader.IPageLoad

#Region "Private Members"

        Private _Users As ArrayList = New ArrayList
        Protected TotalRecords As Integer
        Shared log As Instrumentation.DnnLogger = Instrumentation.DnnLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString())

#End Region

#Region "Interfaces"

        ''' <summary>
        ''' This interface is used to replace a If Page.IsPostBack typically used in page load. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            BindRoles()
            BindSearchTypes()
            BindData(ddlRoles.SelectedItem.Text, 1)
        End Sub

#End Region

#Region "Private Properties"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Property Users() As ArrayList
            Get
                Return _Users
            End Get
            Set(ByVal Value As ArrayList)
                _Users = Value
            End Set
        End Property

#End Region

#Region "Event Handlers"

        ''' <summary>Handles when the search type drop down list is changed (autopostback).
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSearchType.SelectedIndexChanged
            If ddlSearchType.SelectedIndex = 8 Then
                ' If they want to search by role
                ddlRoles.Visible = True
                txtSearch.Visible = False
                ddlRoles.SelectedIndex = 0
            Else
                ddlRoles.Visible = False
                txtSearch.Visible = True
                BindData(txtSearch.Text, 1)
            End If
        End Sub

        ''' <summary>
        ''' Handles when the role name to search by is changed.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ddlRoles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlRoles.SelectedIndexChanged
            BindData(ddlRoles.SelectedItem.Text, 1)
        End Sub

        ''' <summary>
        ''' Fires when a command button is clicked in the users grid.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub dnngridUsers_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles dnngridUsers.ItemCommand
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
        ''' Binds user data to the grid.
        ''' </summary>
        ''' <param name="SearchText"></param>
        ''' <param name="Page"></param>
        ''' <remarks></remarks>
        Private Sub BindData(ByVal SearchText As String, ByVal Page As Integer)
            ' Get the list of registered users from the database
            If ddlSearchType.SelectedIndex = 0 Then
                ' Do Nothing
            ElseIf ddlSearchType.SelectedIndex = 1 Then ' all users
                txtSearch.Text = String.Empty
                Users = Entities.Users.UserController.GetUsers(PortalId, Page - 1, dnngridUsers.PageSize, TotalRecords)
                Dim arrTotalUsers As ArrayList

                arrTotalUsers = Entities.Users.UserController.GetUsers(PortalId, 0, 10000000, 0)

                If Not arrTotalUsers Is Nothing Then
                    TotalRecords = arrTotalUsers.Count
                End If
            ElseIf ddlSearchType.SelectedIndex = 2 Then ' online
                txtSearch.Text = String.Empty
                Users = Entities.Users.UserController.GetOnlineUsers(PortalId)

                If Not Users Is Nothing Then
                    TotalRecords = Users.Count
                End If
            ElseIf ddlSearchType.SelectedIndex = 3 Then ' username
                Users = Entities.Users.UserController.GetUsersByUserName(PortalId, SearchText + "%", Page - 1, dnngridUsers.PageSize, TotalRecords)
                Dim arrTotalUsers As ArrayList

                arrTotalUsers = Entities.Users.UserController.GetUsersByUserName(PortalId, SearchText + "%", 0, 10000000, 0)

                If Not arrTotalUsers Is Nothing Then
                    TotalRecords = arrTotalUsers.Count
                End If
            ElseIf ddlSearchType.SelectedIndex = 4 Then ' display name
                ' Since this is not part of the core, we need to add our own way to make this happen
                ' There is a bug here (in a sense) because this is only return users who have a valid forum profile. 
                Dim ctlForumUser As New ForumUserController
                Users = ctlForumUser.ManageGetAllByDisplayName(PortalId, SearchText + "%", Page - 1, dnngridUsers.PageSize, TotalRecords)
                Dim arrTotalUsers As ArrayList

                arrTotalUsers = ctlForumUser.ManageGetAllByDisplayName(PortalId, SearchText + "%", 0, 10000000, 0)

                If Not arrTotalUsers Is Nothing Then
                    TotalRecords = arrTotalUsers.Count
                End If
            ElseIf ddlSearchType.SelectedIndex = 5 Then ' email
                Users = Entities.Users.UserController.GetUsersByEmail(PortalId, SearchText + "%", Page - 1, dnngridUsers.PageSize, TotalRecords)
                Dim arrTotalUsers As ArrayList

                arrTotalUsers = Entities.Users.UserController.GetUsersByEmail(PortalId, SearchText + "%", 0, 10000000, 0)

                If Not arrTotalUsers Is Nothing Then
                    TotalRecords = arrTotalUsers.Count
                End If
            ElseIf ddlSearchType.SelectedIndex = 6 Then ' First Name
                Users = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, "FirstName", SearchText + "%", Page - 1, dnngridUsers.PageSize, TotalRecords)
                Dim arrTotalUsers As ArrayList

                arrTotalUsers = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, "FirstName", SearchText + "%", 0, 10000000, 0)

                If Not arrTotalUsers Is Nothing Then
                    TotalRecords = arrTotalUsers.Count
                End If
            ElseIf ddlSearchType.SelectedIndex = 7 Then ' Last Name
                Users = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, "LastName", SearchText + "%", Page - 1, dnngridUsers.PageSize, TotalRecords)
                Dim arrTotalUsers As ArrayList

                arrTotalUsers = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, "LastName", SearchText + "%", 0, 10000000, 0)

                If Not arrTotalUsers Is Nothing Then
                    TotalRecords = arrTotalUsers.Count
                End If
            ElseIf ddlSearchType.SelectedIndex = 8 Then ' Role
                ' Since this is not part of the core, we need to add our own way to make this happen
                Dim ctlForumUser As New ForumUserController
                Users = ctlForumUser.ManageGetUsersByRolename(PortalId, SearchText, Page - 1, dnngridUsers.PageSize, TotalRecords)
                Dim arrTotalUsers As ArrayList

                arrTotalUsers = ctlForumUser.ManageGetUsersByRolename(PortalId, SearchText, 0, 10000000, 0)

                If Not arrTotalUsers Is Nothing Then
                    TotalRecords = arrTotalUsers.Count
                End If

                'Dim cntRoles As New DotNetNuke.Security.Roles.RoleController
                'cntRoles.GetUserRolesByRoleName(PortalId, SearchText)
            Else
                ' not setup to use
                Dim propertyName As String = ddlSearchType.SelectedValue
                Users = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, propertyName, SearchText + "%", Page - 1, dnngridUsers.PageSize, TotalRecords)
                Dim arrTotalUsers As ArrayList

                arrTotalUsers = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, propertyName, SearchText + "%", 0, 10000000, 0)

                If Not arrTotalUsers Is Nothing Then
                    TotalRecords = arrTotalUsers.Count
                End If
            End If

            dnngridUsers.Visible = True
            dnngridUsers.DataSource = Users
            dnngridUsers.DataBind()
            dnngridUsers.VirtualItemCount = TotalRecords
        End Sub

        ''' <summary>
        ''' Binds a list of available portal roles when the search by is changed to role. 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindRoles()
            Dim arrRolesAs As IList(Of RoleInfo) = RoleController.Instance.GetRoles(
                   PortalId,
                   Function(role As RoleInfo) ((role.SecurityMode <> SecurityMode.SocialGroup) AndAlso (role.Status = RoleStatus.Approved)))
            ddlRoles.Items.Clear()

            If arrRolesAs.Count > 0 Then
                ddlRoles.DataTextField = "RoleName"
                ddlRoles.DataValueField = "RoleID"
                ddlRoles.DataSource = arrRolesAs
                ddlRoles.DataBind()
            End If

            ddlRoles.Items.Insert(0, New ListItem(Localization.GetString("None.Text", Me.LocalResourceFile), "0"))
        End Sub

        ''' <summary>
        ''' Binds the available search types to perform.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindSearchTypes()
            Dim filters As String = String.Empty
            filters += Localization.GetString("None.Text", Me.LocalResourceFile)
            filters += "," + Localization.GetString("All.Text", Me.LocalResourceFile)
            filters += "," + Localization.GetString("Online.Text", Me.LocalResourceFile)
            filters += "," + Localization.GetString("Username.Text", Me.LocalResourceFile)
            filters += "," + Localization.GetString("DisplayName.Text", Me.LocalResourceFile)
            filters += "," + Localization.GetString("Email.Text", Me.LocalResourceFile)
            filters += "," + Localization.GetString("FirstName.Text", Me.LocalResourceFile)
            filters += "," + Localization.GetString("LastName.Text", Me.LocalResourceFile)
            filters += "," + Localization.GetString("Roles.Text", Me.LocalResourceFile)

            Dim strAlphabet As String() = filters.Split(","c)
            ddlSearchType.Items.Clear()
            ddlSearchType.DataSource = strAlphabet
            ddlSearchType.DataBind()
        End Sub

#End Region

    End Class

End Namespace
