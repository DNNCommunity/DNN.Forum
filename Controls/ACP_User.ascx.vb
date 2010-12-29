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

#End Region

#Region "Interfaces"

		''' <summary>
		''' This interface is used to replace a If Page.IsPostBack typically used in page load. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Localization.LocalizeDataGrid(grdUsers, Me.LocalResourceFile)
			BottomPager.Visible = False
			lblNoResults.Visible = False

			BindRoles()
			BindSearchTypes()
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

		''' <summary>
		''' Formats items contained in the data grid
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub grdUsers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdUsers.ItemDataBound
			Dim item As DataGridItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.SelectedItem Then
				Dim imgColumnControl As System.Web.UI.Control = item.Controls(1).Controls(0)

				imgColumnControl = item.Controls(2).FindControl("hlEditUser")
				If TypeOf imgColumnControl Is HyperLink Then
					Dim hlEditUser As HyperLink = CType(imgColumnControl, HyperLink)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)

					hlEditUser.NavigateUrl() = Utilities.Links.UCP_AdminLinks(TabId, ModuleId, user.UserID, UserAjaxControl.Profile)
					hlEditUser.EnableViewState = False
				End If

				imgColumnControl = item.Controls(2).FindControl("imgEdit")
				If TypeOf imgColumnControl Is Image Then
					Dim EditImage As Image = CType(imgColumnControl, Image)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)

					EditImage.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
					EditImage.Visible = True	'Not (user.UserID = PortalSettings.AdministratorId)
					EditImage.AlternateText = Localization.GetString("imgEdit", LocalResourceFile)
					EditImage.ToolTip = Localization.GetString("imgEdit", LocalResourceFile)
					EditImage.EnableViewState = False
				End If

				imgColumnControl = item.Controls(2).FindControl("imgOnline")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					If objConfig.EnableUsersOnline Then
						Dim userOnlineImage As System.Web.UI.WebControls.Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)
						Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)

						userOnlineImage.EnableViewState = False

						If user.Membership.IsOnLine Then
							userOnlineImage.ImageUrl = objConfig.GetThemeImageURL("s_online.") & objConfig.ImageExtension
							userOnlineImage.AlternateText = Localization.GetString("imgOnline", LocalResourceFile)
							userOnlineImage.ToolTip = Localization.GetString("imgOnline", LocalResourceFile)
						Else
							userOnlineImage.ImageUrl = objConfig.GetThemeImageURL("s_offline.") & objConfig.ImageExtension
							userOnlineImage.AlternateText = Localization.GetString("imgOffline", LocalResourceFile)
							userOnlineImage.ToolTip = Localization.GetString("imgOffline", LocalResourceFile)
						End If
					Else
						Dim userOnlineImage As System.Web.UI.WebControls.Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)
						userOnlineImage.ImageUrl = objConfig.GetThemeImageURL("spacer.gif")
					End If
				End If

				imgColumnControl = item.Controls(2).FindControl("hlEmail")
				If TypeOf imgColumnControl Is HyperLink Then
					Dim hlEmail As HyperLink = CType(imgColumnControl, HyperLink)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)

					hlEmail.Text = user.Email
					hlEmail.NavigateUrl() = "mailto:" & user.Email
					hlEmail.EnableViewState = False
				End If
			End If
		End Sub

		''' <summary>
		''' Handles when the AjaxPager control's page index is changed.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub pager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BottomPager.Command
			Dim CurrentPage As Int32 = CType(e.CommandArgument, Int32)
			BottomPager.CurrentPage = CurrentPage
			If ddlSearchType.SelectedIndex = 8 Then
				BindData(ddlRoles.SelectedItem.Text, CurrentPage)
			Else
				BindData(txtSearch.Text, CurrentPage)
			End If
		End Sub

		''' <summary>
		''' Handles when the search type drop down list is changed (autopostback).
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub ddlSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSearchType.SelectedIndexChanged
			BottomPager.CurrentPage = 1
			If ddlSearchType.SelectedIndex = 8 Then
				' If they want to search by role
				ddlRoles.Visible = True
				txtSearch.Visible = False
				ddlRoles.SelectedIndex = 0
				' have to hide the pager manually here since we are not binding to the grid.
				BottomPager.Visible = False
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
			BottomPager.CurrentPage = 1
			BindData(ddlRoles.SelectedItem.Text, 1)
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
				Users = Entities.Users.UserController.GetUsers(PortalId, Page - 1, BottomPager.PageSize, TotalRecords)
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
				Users = Entities.Users.UserController.GetUsersByUserName(PortalId, SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords)
				Dim arrTotalUsers As ArrayList

				arrTotalUsers = Entities.Users.UserController.GetUsersByUserName(PortalId, SearchText + "%", 0, 10000000, 0)

				If Not arrTotalUsers Is Nothing Then
					TotalRecords = arrTotalUsers.Count
				End If
			ElseIf ddlSearchType.SelectedIndex = 4 Then ' display name
				' Since this is not part of the core, we need to add our own way to make this happen
				' There is a bug here (in a sense) because this is only return users who have a valid forum profile. 
				Dim ctlForumUser As New ForumUserController
				Users = ctlForumUser.ManageGetAllByDisplayName(PortalId, SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords)
				Dim arrTotalUsers As ArrayList

				arrTotalUsers = ctlForumUser.ManageGetAllByDisplayName(PortalId, SearchText + "%", 0, 10000000, 0)

				If Not arrTotalUsers Is Nothing Then
					TotalRecords = arrTotalUsers.Count
				End If
			ElseIf ddlSearchType.SelectedIndex = 5 Then ' email
				Users = Entities.Users.UserController.GetUsersByEmail(PortalId, SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords)
				Dim arrTotalUsers As ArrayList

				arrTotalUsers = Entities.Users.UserController.GetUsersByEmail(PortalId, SearchText + "%", 0, 10000000, 0)

				If Not arrTotalUsers Is Nothing Then
					TotalRecords = arrTotalUsers.Count
				End If
			ElseIf ddlSearchType.SelectedIndex = 6 Then ' First Name
				Users = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, "FirstName", SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords)
				Dim arrTotalUsers As ArrayList

				arrTotalUsers = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, "FirstName", SearchText + "%", 0, 10000000, 0)

				If Not arrTotalUsers Is Nothing Then
					TotalRecords = arrTotalUsers.Count
				End If
			ElseIf ddlSearchType.SelectedIndex = 7 Then ' Last Name
				Users = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, "LastName", SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords)
				Dim arrTotalUsers As ArrayList

				arrTotalUsers = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, "LastName", SearchText + "%", 0, 10000000, 0)

				If Not arrTotalUsers Is Nothing Then
					TotalRecords = arrTotalUsers.Count
				End If
			ElseIf ddlSearchType.SelectedIndex = 8 Then ' Role
				' Since this is not part of the core, we need to add our own way to make this happen
				Dim ctlForumUser As New ForumUserController
				Users = ctlForumUser.ManageGetUsersByRolename(PortalId, SearchText, Page - 1, BottomPager.PageSize, TotalRecords)
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
				Users = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, propertyName, SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords)
				Dim arrTotalUsers As ArrayList

				arrTotalUsers = Entities.Users.UserController.GetUsersByProfileProperty(PortalId, propertyName, SearchText + "%", 0, 10000000, 0)

				If Not arrTotalUsers Is Nothing Then
					TotalRecords = arrTotalUsers.Count
				End If
			End If

			If Users.Count > 0 Then
				grdUsers.Visible = True
				lblNoResults.Visible = False

				grdUsers.DataSource = Users
				grdUsers.DataBind()

				BottomPager.TotalRecords = TotalRecords
				BottomPager.Visible = True
			Else
				grdUsers.Visible = False
				lblNoResults.Visible = True
				BottomPager.Visible = False
			End If
		End Sub

		''' <summary>
		''' Binds a list of available portal roles when the search by is changed to role. 
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindRoles()
			Dim cntRoles As New DotNetNuke.Security.Roles.RoleController
			Dim arrRoles As ArrayList
			arrRoles = cntRoles.GetPortalRoles(PortalId)

			If arrRoles.Count > 0 Then
				ddlRoles.DataTextField = "RoleName"
				ddlRoles.DataValueField = "RoleID"
				ddlRoles.DataSource = arrRoles
				ddlRoles.DataBind()

				ddlRoles.Items.Insert(0, Localization.GetString("None.Text", Me.LocalResourceFile))
			End If
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
			ddlSearchType.DataSource = strAlphabet
			ddlSearchType.DataBind()
		End Sub

		''' <summary>
		''' DisplayEmail correctly formats an Email Address
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		Private Function DisplayEmail(ByVal Email As String) As String
			Dim _Email As String = Null.NullString
			Try
				If Not Email Is Nothing Then
					_Email = HtmlUtils.FormatEmail(Email)
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
			Return _Email
		End Function

		''' <summary>
		''' DisplayDate correctly formats the Date
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		Private Function DisplayDate(ByVal UserDate As Date) As String
			Dim strDisplayDate As String = String.Empty
			Try
				If Not Null.IsNull(UserDate) Then
					strDisplayDate = Utilities.ForumUtils.ConvertTimeZone(UserDate, objConfig).ToString
				Else
					strDisplayDate = String.Empty
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
			Return strDisplayDate
		End Function

#End Region

	End Class

End Namespace
