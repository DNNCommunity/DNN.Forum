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

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' This is a member listing directory. Only users who permit their listing are shown here. This is a central spot for PM as well as searching to find other forum users. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' </history>
	Public MustInherit Class MemberList
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Private _Users As List(Of ForumUser)

#End Region

#Region "Protected Members"

		''' <summary>
		''' Gets whether we are dealing with SuperUsers
		''' </summary>
		''' <history>
		''' </history>
		Protected ReadOnly Property IsSuperUser() As Boolean
			Get
				If PortalSettings.ActiveTab.ParentId = PortalSettings.SuperTabId Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property

		Protected Property Users() As List(Of ForumUser)
			Get
				Return _Users
			End Get
			Set(ByVal Value As List(Of ForumUser))
				_Users = Value
			End Set
		End Property

#End Region

#Region "Optional Interfaces"

		''' <summary>
		''' Gets a list of module actions available to the user to provide it to DNN core.
		''' </summary>
		''' <value></value>
		''' <returns>The collection of module actions available to the user</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
			Get
				Return Utilities.ForumUtils.PerUserModuleActions(objConfig, Me)
			End Get
		End Property

#End Region

#Region "Event Handlers"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			' Ajax
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, True)
			End If
		End Sub

		''' <summary>
		''' Loads the initial settings
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	11/13/2006	Created
		''' </history>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try

				Dim LoggedOnUserID As Integer = -1
				Dim objForumUser As ForumUser
				Dim cntForumUser As New ForumUserController

				If Request.IsAuthenticated And (objConfig.EnablePMSystem = True) Then
					LoggedOnUserID = Entities.Users.UserController.GetCurrentUserInfo.UserID
				Else
					Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				objForumUser = cntForumUser.GetForumUser(LoggedOnUserID, False, ModuleId, PortalId)
				BottomPager.PageSize = Convert.ToInt32(objForumUser.ThreadsPerPage)

				If Not Page.IsPostBack Then
					'Localize the Headers
					Localization.LocalizeDataGrid(grdUsers, Me.LocalResourceFile)
					litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

					'Load the Search Combo
					BindSearchTypes()
					lblNoResults.Visible = False
					BottomPager.Visible = False
					BottomPager.CurrentPage = 1

					If Not Request.UrlReferrer Is Nothing Then
						ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
					End If

					' Register scripts
					Utilities.ForumUtils.RegisterPageScripts(Page, objConfig)
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Takes the user back to the forum admin page. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			Response.Redirect(Utilities.Links.ContainerForumHome(TabId), False)
		End Sub

		''' <summary>
		''' Formats items contained in the data grid
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[Administrator]	8/5/2006	Created
		''' </history>
		Protected Sub grdUsers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdUsers.ItemDataBound
			Dim item As DataGridItem = e.Item

			If item.ItemType = ListItemType.Item Or _
			    item.ItemType = ListItemType.AlternatingItem Or _
			    item.ItemType = ListItemType.SelectedItem Then
				Dim imgColumnControl As System.Web.UI.Control = item.Controls(1).Controls(0)

				Dim objSecurity As New ModuleSecurity(ModuleId, TabId, -1, UserId)

				imgColumnControl = item.Controls(2).FindControl("hlEditUser")
				If TypeOf imgColumnControl Is HyperLink Then
					Dim hlEditUser As HyperLink = CType(imgColumnControl, HyperLink)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)

					hlEditUser.NavigateUrl() = Utilities.Links.UCP_AdminLinks(TabId, ModuleId, user.UserID, UserAjaxControl.Profile)
				End If

				imgColumnControl = item.Controls(2).FindControl("imgEdit")
				If TypeOf imgColumnControl Is Image Then
					Dim EditImage As Image = CType(imgColumnControl, Image)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)

					EditImage.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
					EditImage.Visible = objSecurity.IsForumAdmin
				End If

				imgColumnControl = item.Controls(2).FindControl("imgOnline")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Image Then
					Dim userOnlineImage As System.Web.UI.WebControls.Image = CType(imgColumnControl, System.Web.UI.WebControls.Image)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)
					Dim objForumUser As ForumUser
					Dim cntForumUser As New ForumUserController

					objForumUser = cntForumUser.GetForumUser(user.UserID, False, ModuleId, PortalId)

					If objConfig.EnableUsersOnline Then
						If objForumUser.EnableOnlineStatus Then
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
							userOnlineImage.Visible = False
						End If
					Else
						userOnlineImage.Visible = False
					End If
				End If

				imgColumnControl = item.Controls(2).FindControl("hlSiteAlias")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.HyperLink Then
					Dim userProfileLink As System.Web.UI.WebControls.HyperLink = CType(imgColumnControl, System.Web.UI.WebControls.HyperLink)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)
					Dim objForumUser As ForumUser
					Dim cntForumUser As New ForumUserController

					objForumUser = cntForumUser.GetForumUser(user.UserID, False, ModuleId, PortalId)

					userProfileLink.NavigateUrl = Utilities.Links.UserPublicProfileLink(TabId, ModuleId, objForumUser.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objForumUser.Username)
					userProfileLink.Target = "_self"
					userProfileLink.Text = objForumUser.SiteAlias
				End If

				imgColumnControl = item.Controls(2).FindControl("lblCreatedDate")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblCreatedDate As System.Web.UI.WebControls.Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)
					Dim objForumUser As ForumUser
					Dim cntForumUser As New ForumUserController

					objForumUser = cntForumUser.GetForumUser(user.UserID, False, ModuleId, PortalId)

					lblCreatedDate.Text = Utilities.ForumUtils.ConvertTimeZone(objForumUser.Membership.CreatedDate, objConfig).ToShortDateString
				End If

				imgColumnControl = item.Controls(2).FindControl("lblPostCount")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.Label Then
					Dim lblPostCount As System.Web.UI.WebControls.Label = CType(imgColumnControl, System.Web.UI.WebControls.Label)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)
					Dim objForumUser As ForumUser
					Dim cntForumUser As New ForumUserController

					objForumUser = cntForumUser.GetForumUser(user.UserID, False, ModuleId, PortalId)

					lblPostCount.Text = objForumUser.PostCount.ToString
				End If

				imgColumnControl = item.Controls(2).FindControl("hlMessage")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.HyperLink Then
					Dim hlMessageUser As System.Web.UI.WebControls.HyperLink = CType(imgColumnControl, System.Web.UI.WebControls.HyperLink)
					Dim user As Users.UserInfo = CType(item.DataItem, Users.UserInfo)
					Dim objForumUser As ForumUser
					Dim cntForumUser As New ForumUserController

					objForumUser = cntForumUser.GetForumUser(user.UserID, False, ModuleId, PortalId)

					If Request.IsAuthenticated Then
						Dim LoggedOnUser As ForumUser
						Dim ctlForumUser As New ForumUserController

						LoggedOnUser = ctlForumUser.GetForumUser(UserId, False, ModuleId, PortalId)

						If objConfig.EnablePMSystem And objForumUser.EnablePM And LoggedOnUser.EnablePM Then
							hlMessageUser.Visible = True
							hlMessageUser.NavigateUrl = Utilities.Links.PMUserLink(TabId, ModuleId, objForumUser.UserID)
						Else
							hlMessageUser.Visible = False
						End If
					Else
						hlMessageUser.Visible = False
					End If
				End If
			End If
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub pager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BottomPager.Command
			Dim CurrentPage As Int32 = CType(e.CommandArgument, Int32)
			BottomPager.CurrentPage = CurrentPage
			BindData(txtSearch.Text, CurrentPage)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub ddlSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSearchType.SelectedIndexChanged
			BottomPager.CurrentPage = 1
			BindData(txtSearch.Text, 1)
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
			Dim ctlForumUser As New ForumUserController
			Dim TotalRecords As Integer

			' Get the list of registered users from the database
			If ddlSearchType.SelectedIndex = 0 Then
				' Do Nothing
			ElseIf ddlSearchType.SelectedIndex = 1 Then
				' All
				txtSearch.Text = String.Empty
				Users = ctlForumUser.MembersGetAll(PortalId, Page - 1, BottomPager.PageSize, TotalRecords, ModuleId)
			ElseIf ddlSearchType.SelectedIndex = 2 Then
				' Online
				txtSearch.Text = String.Empty
				Users = ctlForumUser.MembersGetOnline(PortalId, "", Page - 1, BottomPager.PageSize, TotalRecords, ModuleId)
			ElseIf ddlSearchType.SelectedIndex = 3 Then
				' Site Alias
				If objConfig.ForumMemberName = 0 Then
					' Username
					Users = ctlForumUser.MembersGetByUsername(PortalId, SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords, ModuleId)
				Else
					' DisplayName
					Users = ctlForumUser.MembersGetByDisplayName(PortalId, SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords, ModuleId)
				End If
			ElseIf ddlSearchType.SelectedIndex = 4 Then
				' Email
				Users = ctlForumUser.MembersGetByEmail(PortalId, SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords, ModuleId)
			ElseIf ddlSearchType.SelectedIndex = 5 Then
				' First Name
				Users = ctlForumUser.MembersGetByProfileProp(PortalId, "FirstName", SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords, ModuleId)
			ElseIf ddlSearchType.SelectedIndex = 6 Then
				' Last Name
				Users = ctlForumUser.MembersGetByProfileProp(PortalId, "LastName", SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords, ModuleId)
			Else
				' Not Active
				Dim propertyName As String = ddlSearchType.SelectedValue
				Users = ctlForumUser.MembersGetByProfileProp(PortalId, propertyName, SearchText + "%", Page - 1, BottomPager.PageSize, TotalRecords, ModuleId)
			End If

			If Not Users Is Nothing Then
				If Users.Count > 0 Then
					Dim objForumUser As ForumUser = Users.Item(0)

					grdUsers.Visible = True
					lblNoResults.Visible = False

					grdUsers.DataSource = Users
					grdUsers.DataBind()
					grdUsers.DataKeyField = "UserID"

					BottomPager.TotalRecords = objForumUser.TotalRecords
					BottomPager.Visible = True
				Else
					grdUsers.Visible = False
					lblNoResults.Visible = True
					BottomPager.Visible = False
				End If
			Else
				grdUsers.Visible = False
				lblNoResults.Visible = True
				BottomPager.Visible = False
			End If
		End Sub

		Private Sub BindSearchTypes()
			Dim filters As String = String.Empty
			filters += Localization.GetString("None.Text", Me.LocalResourceFile)
			filters += "," + Localization.GetString("All.Text", Me.LocalResourceFile)
			filters += "," + Localization.GetString("Online.Text", Me.LocalResourceFile)
			filters += "," + Localization.GetString("SiteAlias.Text", Me.LocalResourceFile)
			filters += "," + Localization.GetString("Email.Text", Me.LocalResourceFile)
			filters += "," + Localization.GetString("FirstName.Text", Me.LocalResourceFile)
			filters += "," + Localization.GetString("LastName.Text", Me.LocalResourceFile)

			Dim strAlphabet As String() = filters.Split(","c)
			ddlSearchType.DataSource = strAlphabet
			ddlSearchType.DataBind()
		End Sub

#End Region

	End Class

End Namespace
