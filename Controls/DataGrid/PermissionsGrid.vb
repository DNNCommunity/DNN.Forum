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

Namespace DotNetNuke.Modules.Forum.Controls

	''' <summary>
	''' A base classed used to contruct permissions grids.
	''' </summary>
	''' <remarks></remarks>
	Public MustInherit Class PermissionsGrid
		Inherits Control
		Implements INamingContainer

#Region "Controls"

		Private dgRolePermissions As DataGrid
		Private pnlPermissions As Panel
		Private lblGroups As Label
		Private WithEvents cboRoleGroups As DropDownList
		Private lblUser As Label
		Private txtUser As TextBox
		Private WithEvents cmdUser As LinkButton
		Private dgUserPermissions As DataGrid

#End Region

#Region "Private Members"

		Private _dtForumPermissions As New DataTable
		Private _dtUserPermissions As New DataTable
		Private _roles As ArrayList
		Private _users As ArrayList
		Private _permissions As ArrayList
		Private _resourceFile As String

#End Region

#Region "Public Properties"

#Region "DataGrid Properties"

		Public ReadOnly Property AlternatingItemStyle() As TableItemStyle
			Get
				Return dgRolePermissions.AlternatingItemStyle
			End Get
		End Property

		Public Property AutoGenerateColumns() As Boolean
			Get
				Return dgRolePermissions.AutoGenerateColumns
			End Get
			Set(ByVal Value As Boolean)
				dgRolePermissions.AutoGenerateColumns = Value
				dgUserPermissions.AutoGenerateColumns = Value
			End Set
		End Property

		Public Property CellSpacing() As Integer
			Get
				Return dgRolePermissions.CellSpacing
			End Get
			Set(ByVal Value As Integer)
				dgRolePermissions.CellSpacing = Value
				dgUserPermissions.CellSpacing = Value
			End Set
		End Property

		Public ReadOnly Property Columns() As DataGridColumnCollection
			Get
				Return dgRolePermissions.Columns()
			End Get
		End Property

		Public ReadOnly Property FooterStyle() As TableItemStyle
			Get
				Return dgRolePermissions.FooterStyle
			End Get
		End Property

		Public Property GridLines() As GridLines
			Get
				Return dgRolePermissions.GridLines
			End Get
			Set(ByVal Value As GridLines)
				dgRolePermissions.GridLines = Value
				dgUserPermissions.GridLines = Value
			End Set
		End Property

		Public ReadOnly Property HeaderStyle() As TableItemStyle
			Get
				Return dgRolePermissions.HeaderStyle
			End Get
		End Property

		Public ReadOnly Property ItemStyle() As TableItemStyle
			Get
				Return dgRolePermissions.ItemStyle
			End Get
		End Property

		Public ReadOnly Property Items() As DataGridItemCollection
			Get
				Return dgRolePermissions.Items()
			End Get
		End Property

		Public ReadOnly Property SelectedItemStyle() As TableItemStyle
			Get
				Return dgRolePermissions.SelectedItemStyle
			End Get
		End Property

#End Region

		''' <summary>
		''' Gets the Id of the Administrator Role
		''' </summary>
		Public ReadOnly Property AdministratorRoleId() As Integer
			Get
				Return Portals.PortalController.GetCurrentPortalSettings.AdministratorRoleId
			End Get
		End Property

		''' <summary>
		''' Gets the Id of the Registered Users Role
		''' </summary>
		Public ReadOnly Property RegisteredUsersRoleId() As Integer
			Get
				Return Portals.PortalController.GetCurrentPortalSettings.RegisteredRoleId
			End Get
		End Property

		''' <summary>
		''' Gets and Sets whether a Dynamic Column has been added
		''' </summary>
		Public Property DynamicColumnAdded() As Boolean
			Get
				If ViewState("ColumnAdded") Is Nothing Then
					Return False
				Else
					Return True
				End If
			End Get
			Set(ByVal Value As Boolean)
				ViewState("ColumnAdded") = Value
			End Set
		End Property

		''' <summary>
		''' Gets the underlying Permissions Data Table
		''' </summary>
		Public ReadOnly Property dtForumPermissions() As DataTable
			Get
				Return _dtForumPermissions
			End Get
		End Property

		''' <summary>
		''' Gets the underlying Permissions Data Table
		''' </summary>
		Public ReadOnly Property dtUserPermissions() As DataTable
			Get
				Return _dtUserPermissions
			End Get
		End Property

		''' <summary>
		''' Gets the Id of the Portal
		''' </summary>
		Public ReadOnly Property PortalId() As Integer
			Get
				' Obtain PortalSettings from Current Context
				Dim _portalSettings As Portals.PortalSettings = Portals.PortalController.GetCurrentPortalSettings
				Dim intPortalID As Integer

				If _portalSettings.ActiveTab.ParentId = _portalSettings.SuperTabId Then 'if we are in host filemanager then we need to pass a null portal id
					intPortalID = Null.NullInteger
				Else
					intPortalID = _portalSettings.PortalId
				End If

				Return intPortalID
			End Get
		End Property

		''' <summary>
		''' Gets and Sets the collection of Roles to display
		''' </summary>
		Public Property Roles() As ArrayList
			Get
				Return _roles
			End Get
			Set(ByVal Value As ArrayList)
				_roles = Value
			End Set
		End Property

		''' <summary>
		''' Gets and Sets the ResourceFile to localize permissions
		''' </summary>
		Public Property ResourceFile() As String
			Get
				Return _resourceFile
			End Get
			Set(ByVal Value As String)
				_resourceFile = Value
			End Set
		End Property

#End Region

#Region "Abstract Methods"

		''' <summary>
		''' Generate the Data Grid
		''' </summary>
		''' <history>
		''' </history>
		Public MustOverride Sub GenerateDataGrid()

#End Region

#Region "Private Methods"

		''' <summary>
		''' Bind the data to the controls
		''' </summary>
		''' <history>
		''' </history>
		Private Sub BindData()
			Me.EnsureChildControls()

			BindRolesGrid()
			BindUsersGrid()
		End Sub

		''' <summary>
		''' Bind the Roles data to the Grid
		''' </summary>
		Private Sub BindRolesGrid()
			dtForumPermissions.Columns.Clear()
			dtForumPermissions.Rows.Clear()

			Dim col As DataColumn

			'Add Roles Column
			col = New DataColumn("RoleId")
			dtForumPermissions.Columns.Add(col)

			'Add Roles Column
			col = New DataColumn("RoleName")
			dtForumPermissions.Columns.Add(col)

			Dim i As Integer
			For i = 0 To _permissions.Count - 1
				Dim objPerm As PermissionInfo
				objPerm = CType(_permissions(i), PermissionInfo)

				'Add Enabled Column
				col = New DataColumn(objPerm.PermissionName & "_Enabled")
				dtForumPermissions.Columns.Add(col)

				'Add Permission Column
				col = New DataColumn(objPerm.PermissionName)
				dtForumPermissions.Columns.Add(col)
			Next

			GetRoles()

			UpdateRolePermissions()
			Dim row As DataRow
			For i = 0 To Roles.Count - 1
				Dim role As Security.Roles.RoleInfo = DirectCast(Roles(i), Security.Roles.RoleInfo)
				row = dtForumPermissions.NewRow
				row("RoleId") = role.RoleID
				row("RoleName") = Localization.LocalizeRole(role.RoleName)

				Dim j As Integer
				For j = 0 To _permissions.Count - 1
					Dim objPerm As PermissionInfo
					objPerm = CType(_permissions(j), PermissionInfo)

					row(objPerm.PermissionName & "_Enabled") = GetEnabled(objPerm, role, j + 1)
					row(objPerm.PermissionName) = GetPermission(objPerm, role, j + 1)
				Next
				dtForumPermissions.Rows.Add(row)
			Next

			dgRolePermissions.DataSource = dtForumPermissions
			dgRolePermissions.DataBind()
		End Sub

		''' <summary>
		''' Bind the Roles data to the Grid
		''' </summary>
		Private Sub BindUsersGrid()
			dtUserPermissions.Columns.Clear()
			dtUserPermissions.Rows.Clear()

			Dim col As DataColumn

			'Add Roles Column
			col = New DataColumn("UserId")
			dtUserPermissions.Columns.Add(col)

			'Add Roles Column
			col = New DataColumn("DisplayName")
			dtUserPermissions.Columns.Add(col)

			Dim i As Integer
			For i = 0 To _permissions.Count - 1
				Dim objPerm As PermissionInfo
				objPerm = CType(_permissions(i), PermissionInfo)

				'Add Enabled Column
				col = New DataColumn(objPerm.PermissionName & "_Enabled")
				dtUserPermissions.Columns.Add(col)

				'Add Permission Column
				col = New DataColumn(objPerm.PermissionName)
				dtUserPermissions.Columns.Add(col)
			Next

			If Not dgUserPermissions Is Nothing Then
				_users = GetUsers()

				If _users.Count <> 0 Then
					dgUserPermissions.Visible = True

					UpdateUserPermissions()

					Dim row As DataRow
					For i = 0 To _users.Count - 1
						Dim user As Users.UserInfo = DirectCast(_users(i), Users.UserInfo)
						row = dtUserPermissions.NewRow
						row("UserId") = user.UserID
						row("DisplayName") = user.DisplayName

						Dim j As Integer
						For j = 0 To _permissions.Count - 1
							Dim objPerm As PermissionInfo
							objPerm = CType(_permissions(j), PermissionInfo)

							row(objPerm.PermissionName & "_Enabled") = GetEnabled(objPerm, user, j + 1)
							row(objPerm.PermissionName) = GetPermission(objPerm, user, j + 1)
						Next
						dtUserPermissions.Rows.Add(row)
					Next

					dgUserPermissions.DataSource = dtUserPermissions
					dgUserPermissions.DataBind()
				Else
					dgUserPermissions.Visible = False
				End If
			End If
		End Sub

		''' <summary>
		''' Gets the roles from the Database and loads them into the Roles property
		''' </summary>
		''' <history>
		''' </history>
		Private Sub GetRoles()
			Dim objRoleController As New Security.Roles.RoleController
			Dim RoleGroupId As Integer = -2
			If (Not cboRoleGroups Is Nothing) AndAlso (Not cboRoleGroups.SelectedValue Is Nothing) Then
				RoleGroupId = Integer.Parse(cboRoleGroups.SelectedValue)
			End If

			If RoleGroupId > -2 Then
				_roles = objRoleController.GetRolesByGroup(Portals.PortalController.GetCurrentPortalSettings.PortalId, RoleGroupId)
			Else
				_roles = objRoleController.GetPortalRoles(Portals.PortalController.GetCurrentPortalSettings.PortalId)
			End If

			'CP - This is commented out to avoid showing all users and unauthenticated user rows in permissions grid
			' We will need to enable All Users if allowing anonymous posting (we don't need unauth as all users are unauth too, don't need to have permission ability for unauth to be only users to view/post/etc)
			'If RoleGroupId < 0 Then
			'    Dim r As New RoleInfo
			'    r.RoleID = Integer.Parse(glbRoleUnauthUser)
			'    r.RoleName = glbRoleUnauthUserName
			'    _roles.Add(r)
			'    r = New RoleInfo
			'    r.RoleID = Integer.Parse(glbRoleAllUsers)
			'    r.RoleName = glbRoleAllUsersName
			'    _roles.Add(r)
			'End If
			_roles.Reverse()
			_roles.Sort(New Security.Roles.RoleComparer)
		End Sub

		''' <summary>
		''' Sets up the columns for the Grid
		''' </summary>
		Private Sub SetUpRolesGrid()
			dgRolePermissions.Columns.Clear()

			Dim textCol As New BoundColumn
			textCol.HeaderText = "&nbsp;"
			textCol.DataField = "RoleName"
			textCol.ItemStyle.Width = Unit.Parse("100px")
			dgRolePermissions.Columns.Add(textCol)

			Dim idCol As New BoundColumn
			idCol.HeaderText = String.Empty
			idCol.DataField = "roleid"
			idCol.Visible = False
			dgRolePermissions.Columns.Add(idCol)

			Dim checkCol As TemplateColumn

			Dim objPermission As PermissionInfo
			For Each objPermission In _permissions

				checkCol = New TemplateColumn
				Dim columnTemplate As New DotNetNuke.UI.WebControls.CheckBoxColumnTemplate
				columnTemplate.DataField = objPermission.PermissionName
				columnTemplate.EnabledField = objPermission.PermissionName & "_Enabled"
				checkCol.ItemTemplate = columnTemplate
				Dim locName As String = String.Empty
				'If objPermission.ModuleDefID > 0 Then
				If ResourceFile <> String.Empty Then
					' custom permission
					locName = Services.Localization.Localization.GetString(objPermission.PermissionName + ".Permission", ResourceFile)
				End If
				'Else
				'    ' system permission
				'    locName = Services.Localization.Localization.GetString(objPermission.PermissionName + ".Permission", Services.Localization.Localization.GlobalResourceFile)
				'End If
				checkCol.HeaderText = IIf(locName <> String.Empty, locName, objPermission.PermissionName).ToString
				checkCol.ItemStyle.HorizontalAlign = HorizontalAlign.Center
				checkCol.HeaderStyle.Wrap = True
				dgRolePermissions.Columns.Add(checkCol)
			Next
		End Sub

		''' <summary>
		''' Sets up the columns for the Grid
		''' </summary>
		''' <remarks></remarks>
		Private Sub SetUpUsersGrid()
			If Not dgUserPermissions Is Nothing Then
				dgUserPermissions.Columns.Clear()

				Dim textCol As New BoundColumn
				textCol.HeaderText = "&nbsp;"
				textCol.DataField = "DisplayName"
				textCol.ItemStyle.Width = Unit.Parse("100px")
				dgUserPermissions.Columns.Add(textCol)

				Dim idCol As New BoundColumn
				idCol.HeaderText = String.Empty
				idCol.DataField = "userid"
				idCol.Visible = False
				dgUserPermissions.Columns.Add(idCol)

				Dim checkCol As TemplateColumn

				Dim objPermission As PermissionInfo
				For Each objPermission In _permissions
					checkCol = New TemplateColumn
					Dim columnTemplate As New UI.WebControls.CheckBoxColumnTemplate
					columnTemplate.DataField = objPermission.PermissionName
					columnTemplate.EnabledField = objPermission.PermissionName & "_Enabled"
					checkCol.ItemTemplate = columnTemplate
					Dim locName As String = String.Empty
					'If objPermission.ModuleDefID > 0 Then
					If ResourceFile <> String.Empty Then
						' custom permission
						locName = Services.Localization.Localization.GetString(objPermission.PermissionName + ".Permission", ResourceFile)
					End If
					'Else
					' system permission
					'locName = Services.Localization.Localization.GetString(objPermission.PermissionName + ".Permission", Services.Localization.Localization.GlobalResourceFile)
					'End If
					checkCol.HeaderText = IIf(locName <> String.Empty, locName, objPermission.PermissionName).ToString
					checkCol.ItemStyle.HorizontalAlign = HorizontalAlign.Center
					checkCol.HeaderStyle.Wrap = True
					dgUserPermissions.Columns.Add(checkCol)
				Next
			End If

		End Sub

#End Region

#Region "Protected Methods"

		''' <summary>
		''' Builds the key used to store the "permission" information in the ViewState
		''' </summary>
		''' <param name="checked">Is the checkbox checked</param>
		''' <param name="permissionId">The Id of the permission</param>
		''' <param name="objectPermissionId">The Id of the object permission</param>
		''' <param name="roleId">The role id</param>
		''' <param name="roleName">The role name</param>
		''' <remarks>This is here to handle support for core (pre 4.5) for module permissions (doesn't include per user mod perms this release)</remarks>
		Protected Function BuildKey(ByVal checked As Boolean, ByVal permissionId As Integer, ByVal objectPermissionId As Integer, ByVal roleId As Integer, ByVal roleName As String) As String
			Return BuildKey(checked, permissionId, objectPermissionId, roleId, roleName, Null.NullInteger, Null.NullString)
		End Function

		''' <summary>
		''' Builds the key used to store the "permission" information in the ViewState
		''' </summary>
		''' <param name="checked">Is the checkbox checked</param>
		''' <param name="permissionId">The Id of the permission</param>
		''' <param name="objectPermissionId">The Id of the object permission</param>
		''' <param name="roleId">The role id</param>
		''' <param name="roleName">The role name</param>
		''' <param name="userId">The user id</param>
		''' <param name="displayName">The user display name</param>
		Protected Function BuildKey(ByVal checked As Boolean, ByVal permissionId As Integer, ByVal objectPermissionId As Integer, ByVal roleId As Integer, ByVal roleName As String, ByVal userID As Integer, ByVal displayName As String) As String
			Dim key As String

			If checked Then
				key = "True"
			Else
				key = "False"
			End If

			key += "|" + Convert.ToString(permissionId)

			'Add objectPermissionId
			key += "|"
			If objectPermissionId > -1 Then
				key += Convert.ToString(objectPermissionId)
			End If

			key += "|" + roleName
			key += "|" + roleId.ToString
			key += "|" + userID.ToString
			key += "|" + displayName.ToString

			Return key
		End Function

		''' <summary>
		''' Creates the Child Controls
		''' </summary>
		''' <history>
		''' </history>
		Protected Overrides Sub CreateChildControls()
			' get data
			_permissions = GetPermissions()

			pnlPermissions = New Panel
			pnlPermissions.CssClass = "DataGrid_Container"

			'Optionally Add Role Group Filter
			Dim _portalSettings As Portals.PortalSettings = Portals.PortalController.GetCurrentPortalSettings
			Dim arrGroups As ArrayList = Security.Roles.RoleController.GetRoleGroups(_portalSettings.PortalId)
			If arrGroups.Count > 0 Then
				lblGroups = New Label
				lblGroups.Text = Localization.GetString("RoleGroupFilter")
				lblGroups.CssClass = "SubHead"
				pnlPermissions.Controls.Add(lblGroups)

				pnlPermissions.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))

				cboRoleGroups = New DropDownList
				cboRoleGroups.AutoPostBack = True

				cboRoleGroups.Items.Add(New ListItem(Localization.GetString("AllRoles"), "-2"))
				Dim liItem As ListItem = New ListItem(Localization.GetString("GlobalRoles"), "-1")
				liItem.Selected = True
				cboRoleGroups.Items.Add(liItem)
				For Each roleGroup As Security.Roles.RoleGroupInfo In arrGroups
					cboRoleGroups.Items.Add(New ListItem(roleGroup.RoleGroupName, roleGroup.RoleGroupID.ToString))
				Next
				pnlPermissions.Controls.Add(cboRoleGroups)
				pnlPermissions.Controls.Add(New LiteralControl("<br/><br/>"))
			End If

			dgRolePermissions = New DataGrid
			dgRolePermissions.AutoGenerateColumns = False
			dgRolePermissions.CellSpacing = 0
			dgRolePermissions.GridLines = GridLines.None
			dgRolePermissions.FooterStyle.CssClass = "DataGrid_Footer"
			dgRolePermissions.HeaderStyle.CssClass = "DataGrid_Header"
			dgRolePermissions.ItemStyle.CssClass = "DataGrid_Item"
			dgRolePermissions.AlternatingItemStyle.CssClass = "DataGrid_AlternatingItem"
			SetUpRolesGrid()
			pnlPermissions.Controls.Add(dgRolePermissions)

			_users = GetUsers()

			If Not _users Is Nothing Then
				dgUserPermissions = New DataGrid
				dgUserPermissions.AutoGenerateColumns = False
				dgUserPermissions.CellSpacing = 0
				dgUserPermissions.GridLines = GridLines.None
				dgUserPermissions.FooterStyle.CssClass = "DataGrid_Footer"
				dgUserPermissions.HeaderStyle.CssClass = "DataGrid_Header"
				dgUserPermissions.ItemStyle.CssClass = "DataGrid_Item"
				dgUserPermissions.AlternatingItemStyle.CssClass = "DataGrid_AlternatingItem"
				SetUpUsersGrid()
				pnlPermissions.Controls.Add(dgUserPermissions)

				pnlPermissions.Controls.Add(New LiteralControl("<br/>"))

				lblUser = New Label
				lblUser.Text = DotNetNuke.Services.Localization.Localization.GetString("User", ResourceFile)
				lblUser.CssClass = "SubHead"
				pnlPermissions.Controls.Add(lblUser)

				pnlPermissions.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))

				txtUser = New TextBox
				txtUser.CssClass = "NormalTextBox"
				pnlPermissions.Controls.Add(txtUser)

				pnlPermissions.Controls.Add(New LiteralControl("&nbsp;&nbsp;"))

				cmdUser = New LinkButton
				cmdUser.Text = Localization.GetString("Add")
				cmdUser.CssClass = "CommandButton"
				pnlPermissions.Controls.Add(cmdUser)
			End If

			Me.Controls.Add(pnlPermissions)
		End Sub

		''' <summary>
		''' Gets the Enabled status of the permission
		''' </summary>
		''' <param name="objPerm">The permission being loaded</param>
		''' <param name="role">The role</param>
		''' <param name="column">The column of the Grid</param>
		''' <history>
		''' </history>
		Protected Overridable Function GetEnabled(ByVal objPerm As PermissionInfo, ByVal role As Security.Roles.RoleInfo, ByVal column As Integer) As Boolean
			Return False
		End Function

		''' <summary>
		''' Gets the Enabled status of the permission
		''' </summary>
		''' <param name="objPerm">The permission being loaded</param>
		''' <param name="user">The user</param>
		''' <param name="column">The column of the Grid</param>
		Protected Overridable Function GetEnabled(ByVal objPerm As PermissionInfo, ByVal user As Users.UserInfo, ByVal column As Integer) As Boolean
			Return False
		End Function

		''' <summary>
		''' Gets the Value of the permission
		''' </summary>
		''' <param name="objPerm">The permission being loaded</param>
		''' <param name="role">The role</param>
		''' <param name="column">The column of the Grid</param>
		''' <history>
		''' </history>
		Protected Overridable Function GetPermission(ByVal objPerm As PermissionInfo, ByVal role As Security.Roles.RoleInfo, ByVal column As Integer) As Boolean
			Return False
		End Function

		''' <summary>
		''' Gets the Value of the permission
		''' </summary>
		''' <param name="objPerm">The permission being loaded</param>
		''' <param name="user">The user</param>
		''' <param name="column">The column of the Grid</param>
		Protected Overridable Function GetPermission(ByVal objPerm As PermissionInfo, ByVal user As Users.UserInfo, ByVal column As Integer) As Boolean
			Return False
		End Function

		''' <summary>
		''' Gets the permissions from the Database
		''' </summary>
		''' <history>
		''' </history>
		Protected Overridable Function GetPermissions() As ArrayList
			Return Nothing
		End Function

		''' <summary>
		''' Gets the users from the Database
		''' </summary>
		Protected Overridable Function GetUsers() As ArrayList
			Return Nothing
		End Function

		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

		End Sub

		''' <summary>
		''' Overrides the base OnPreRender method to Bind the Grid to the Permissions
		''' </summary>
		''' <history>
		''' </history>
		Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
			BindData()
		End Sub

		''' <summary>
		''' Updates a Permission
		''' </summary>
		''' <param name="permission">The permission being updated</param>
		''' <param name="roleName">The name of the role</param>
		''' <param name="allowAccess">The value of the permission</param>
		''' <history>
		''' </history>
		Protected Overridable Sub UpdatePermission(ByVal permission As PermissionInfo, ByVal roleId As Integer, ByVal roleName As String, ByVal allowAccess As Boolean)

		End Sub

		''' <summary>
		''' Updates a Permission
		''' </summary>
		''' <param name="permission">The permission being updated</param>
		''' <param name="displayName">The user's displayname</param>
		''' <param name="userId">The user's id</param>
		''' <param name="allowAccess">The value of the permission</param>
		Protected Overridable Sub UpdatePermission(ByVal permission As PermissionInfo, ByVal displayName As String, ByVal userId As Integer, ByVal allowAccess As Boolean)

		End Sub

		''' <summary>
		''' Updates a Permission
		''' </summary>
		''' <param name="permissions">The permissions collection</param>
		''' <param name="user">The user to add</param>
		Protected Overridable Sub AddPermission(ByVal permissions As ArrayList, ByVal user As Users.UserInfo)

		End Sub

		''' <summary>
		''' Updates the permissions
		''' </summary>
		''' <history>
		''' </history>
		Protected Sub UpdatePermissions()
			Me.EnsureChildControls()

			UpdateRolePermissions()
			UpdateUserPermissions()
		End Sub

		''' <summary>
		''' Updates the permissions
		''' </summary>
		Protected Sub UpdateRolePermissions()
			If Not dgRolePermissions Is Nothing Then
				Dim dgi As DataGridItem
				For Each dgi In dgRolePermissions.Items
					Dim i As Integer
					For i = 2 To dgi.Cells.Count - 1
						'all except first two cells which is role names and role ids
						If dgi.Cells(i).Controls.Count > 0 Then
							Dim cb As CheckBox = CType(dgi.Cells(i).Controls(0), CheckBox)
							UpdatePermission(CType(_permissions(i - 2), PermissionInfo), Integer.Parse(dgi.Cells(1).Text), dgi.Cells(0).Text, cb.Checked)
						End If
					Next
				Next
			End If
		End Sub

		''' <summary>
		''' Updates the permissions
		''' </summary>
		''' <history>
		'''     [cnurse]    01/09/2006  Created
		''' </history>
		Protected Sub UpdateUserPermissions()
			If Not dgUserPermissions Is Nothing Then
				Dim dgi As DataGridItem
				For Each dgi In dgUserPermissions.Items
					Dim i As Integer
					For i = 2 To dgi.Cells.Count - 1
						'all except first two cells which is displayname and userid
						If dgi.Cells(i).Controls.Count > 0 Then
							Dim cb As CheckBox = CType(dgi.Cells(i).Controls(0), CheckBox)
							UpdatePermission(CType(_permissions(i - 2), PermissionInfo), dgi.Cells(0).Text, Integer.Parse(dgi.Cells(1).Text), cb.Checked)
						End If
					Next
				Next
			End If
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' RoleGroupsSelectedIndexChanged runs when the Role Group is changed
		''' </summary>
		''' <history>
		''' </history>
		Private Sub RoleGroupsSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoleGroups.SelectedIndexChanged
			UpdatePermissions()
		End Sub

		''' <summary>
		''' AddUser runs when the Add user linkbutton is clicked
		''' </summary>
		''' <history>
		''' </history>
		Private Sub AddUser(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUser.Click
			UpdatePermissions()
			' at this point, we don't have the new user added to the grid, so no update is happening for the user we are trying to create

			If txtUser.Text <> String.Empty Then
				' verify username
				Dim objUser As Users.UserInfo = Users.UserController.GetUserByName(PortalId, txtUser.Text)
				If Not objUser Is Nothing Then
					AddPermission(_permissions, objUser)
					txtUser.Text = String.Empty
					BindData()
				Else
					' user does not exist
					lblUser = New Label
					lblUser.Text = "<br />" & Localization.GetString("InvalidUserName")
					lblUser.CssClass = "NormalRed"
					pnlPermissions.Controls.Add(lblUser)
				End If
			End If
		End Sub

#End Region

	End Class

End Namespace