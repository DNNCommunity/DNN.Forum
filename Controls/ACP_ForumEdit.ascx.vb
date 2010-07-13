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

Imports DotNetNuke.Modules.Forum.Utilities

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' You can add/edit a forum here. (Management of an individual forum)
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class ForumEdit
		Inherits ForumModuleBase

#Region "Private Members"

		Dim _PublicPosting As Boolean
		Dim _ModeratedForum As Boolean
		Dim _PublicView As Boolean

#End Region

#Region "Private Properties"

		''' <summary>
		''' If the forum permits public posting (ie. not a private forum). 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property PublicPosting() As Boolean
			Get
				Return _PublicPosting
			End Get
			Set(ByVal Value As Boolean)
				_PublicPosting = Value
			End Set
		End Property

		''' <summary>
		''' If the forum is moderated.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property ModeratedForum() As Boolean
			Get
				Return _ModeratedForum
			End Get
			Set(ByVal Value As Boolean)
				_ModeratedForum = Value
			End Set
		End Property

		''' <summary>
		''' If the forum permits public viewing.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property PublicView() As Boolean
			Get
				Return _PublicView
			End Get
			Set(ByVal Value As Boolean)
				_PublicView = Value
			End Set
		End Property

		' ''' <summary>
		' ''' The ForumID of the forum we are editing, if it is a new one it will return a -1. 
		' ''' </summary>
		' ''' <value></value>
		' ''' <returns></returns>
		' ''' <remarks></remarks>
		'Private ReadOnly Property ForumID() As Integer
		'	Get
		'		If Not (Request.QueryString("forumid") Is Nothing) Then
		'			Return Int32.Parse(Request.QueryString("forumid"))
		'		Else
		'			Return -1
		'		End If
		'	End Get
		'End Property

		''' <summary>
		''' The forum info object that is being edited.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property objForum() As ForumInfo
			Get
				If ForumID > 0 Then
					Dim cntForum As New ForumController
					Return cntForum.GetForumItemCache(ForumID)
				Else
					Return Nothing
				End If
			End Get
		End Property

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Run to enable Ajax if the core install has it enabled. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdAdd)
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdDelete)
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdUpdate)
			End If
		End Sub

		''' <summary>
		''' The Page Load of the control.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				If Not (Request.IsAuthenticated And Security.IsForumAdmin = True) Then
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				'[skeel] enable menu
				ACPmenu.ControlToLoad = "ACP_ForumEdit.ascx"
				ACPmenu.PortalID = PortalId
				ACPmenu.objConfig = objConfig()
				ACPmenu.ModuleID = ModuleId
				ACPmenu.EnableAjax = False

				If Not Page.IsPostBack Then
					Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
					ForumUtils.LoadCssFile(DefaultPage, objConfig)

					BuildTabs()
					SetURLController()
					BindGroup()
					BindLists()
					BindForumCopyLists()

					cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
					dgPermissions.ModuleID = ModuleId

					' Make sure module allows option before allowing user to select
					If objConfig.EnableThreadStatus Then
						rowThreadStatus.Visible = True
					Else
						rowThreadStatus.Visible = False
					End If
					' Make sure module allows option before allowing user to select
					If objConfig.EnableRatings Then
						rowRating.Visible = True
					Else
						rowRating.Visible = False
					End If
					' Make sure module allows option before allowing user to select
					If objConfig.EnableRSS Then
						rowEnableRSS.Visible = True
					Else
						rowEnableRSS.Visible = False
					End If

					If Not ForumID = -1 Then
						dgPermissions.ForumID = ForumID
						PopulateForum()
						SetPageView(True)
					Else
						' This means this will be a new forum, see if there is a group for it
						If Not (Request.QueryString("groupid") Is Nothing) Then
							Dim GroupID As Integer
							GroupID = Int32.Parse(Request.QueryString("groupid"))
							ddlGroup.Items.FindByValue(GroupID.ToString).Selected = True
							'Typical new forum settings (default)
							chkActive.Checked = True
							ddlForumBehavior.SelectedValue = CStr(ForumBehavior.PublicUnModerated)
							HandleForumBehavior()
							SetupForumPermsGrid(False)
							dgPermissions.ForumID = 0
							HandleForumType()
							chkEnableForumsThreadStatus.Checked = objConfig.EnableThreadStatus
							chkEnableForumsRating.Checked = objConfig.EnableRatings
							' email
							txtEmailAddress.Text = objConfig.AutomatedEmailAddress
							txtEmailFriendlyFrom.Text = objConfig.EmailAddressDisplayName
							chkEnableRSS.Checked = objConfig.EnableRSS
							txtSitemapPriority.Text = objConfig.SitemapPriority.ToString()

							SetPageView(False)
							Dim objForum As ForumInfo = New ForumInfo
							'[skeel] load in parentforums
							BindParentForum()
						Else
							'[skeel] No GroupID means bad url right?
							Response.Redirect(Utilities.Links.ACPControlLink(TabId, ModuleId, AdminAjaxControl.Main))
							Exit Sub
						End If
					End If

					'tblGeneral.Visible = True
					'cmdGeneral.Enabled = False
					'tblBehavior.Visible = False
					'cmdBehavior.Enabled = True
					'tblEmail.Visible = False

					'If objConfig.MailNotification And objConfig.EnablePerForumFrom Then
					'	cmdEmail.Enabled = True
					'Else
					'	cmdEmail.Enabled = False
					'End If
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' This updates the specific forum.  First it handles roles if necessary, 
		''' then posts the changes to the database and redirects the user to the 
		''' forum management screen.
		''' </summary>
		''' <param name="sender">System.Object</param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Dim cntForum As New ForumController
			Dim objForum As New ForumInfo
			' Make sure we are working w/ the current forumID
			objForum = cntForum.GetForumItemCache(ForumID)

			'[skeel] store this value to clear correct cache
			Dim OriginalParentID As Integer
			OriginalParentID = objForum.ParentId

			With objForum
				.GroupID = Convert.ToInt32(ddlGroup.SelectedValue)
				.ForumID = ForumID
				.IsActive = chkActive.Checked
				.Name = txtForumName.Text
				.Description = txtForumDescription.Text
				.EnablePostStatistics = True
				.ForumType = CType(ddlForumType.SelectedIndex, Integer)
				.UpdatedByUser = UserId
				.ForumPermissions = dgPermissions.Permissions
				.EnableForumsThreadStatus = chkEnableForumsThreadStatus.Checked
				.EnableForumsRating = chkEnableForumsRating.Checked
				.ForumLink = ctlURL.Url.Trim()
				.ForumBehavior = CType((ddlForumBehavior.SelectedValue), ForumBehavior)
				'[skeel] set parent forum
				If rowParentForum.Visible = True Then
					.ParentId = Convert.ToInt32(ddlParentForum.SelectedValue)
				End If
				.ForumPermissions = dgPermissions.Permissions
				.AllowPolls = chkAllowPolls.Checked
				.EnableRSS = chkEnableRSS.Checked
				' email
				.EmailAddress = txtEmailAddress.Text
				.EmailFriendlyFrom = txtEmailFriendlyFrom.Text
				' not implemented
				.NotifyByDefault = chkNotifyByDefault.Checked
				.EmailStatusChange = chkEmailStatusChange.Checked
				.EmailServer = txtEmailServer.Text
				.EmailUser = txtEmailUser.Text
				.EmailPass = txtEmailPass.Text
				.EnableSitemap = chkEnableSitemap.Checked
				.SitemapPriority = Convert.ToDouble(txtSitemapPriority.Text)

				If Not txtEmailPort.Text.Trim = String.Empty Then
					.EmailPort = CType(txtEmailPort.Text, Integer)
				End If
				.EmailEnableSSL = chkEmailEnableSSL.Checked

				' url tracking
				Dim objUrls As New UrlController
				objUrls.UpdateUrl(PortalId, ctlURL.Url, ctlURL.UrlType, ctlURL.Log, ctlURL.Track, ModuleId, ctlURL.NewWindow)
			End With

			cntForum.ForumUpdate(objForum, OriginalParentID)

			Dim CurrentParentID As Integer = Convert.ToInt32(ddlParentForum.SelectedValue)
			'update the cached values
			Forum.Components.Utilities.Caching.UpdateForumCache(ForumID, objForum.GroupID, ModuleId)

			If Not (OriginalParentID = CurrentParentID) Then
				Forum.Components.Utilities.Caching.UpdateForumCache(CurrentParentID, objForum.GroupID, ModuleId)
			Else
				Forum.Components.Utilities.Caching.UpdateForumCache(OriginalParentID, objForum.GroupID, ModuleId)
			End If

			' Go Back to forum/group management screen
			Response.Redirect(Utilities.Links.ACPForumsManageLink(TabId, ModuleId, CType(ddlGroup.SelectedValue, Integer)), False)
		End Sub

		''' <summary>
		''' This adds a new forum to the database.  It then redirects the user 
		''' to the forum management screen when completed.
		''' </summary>
		''' <param name="sender">System.Object</param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
			Dim cntForum As New ForumController
			Dim objForum As New ForumInfo
			Dim ForumID As Integer

			With objForum
				.ForumID = ForumID
				.GroupID = Convert.ToInt32(ddlGroup.SelectedValue)
				.IsActive = chkActive.Checked
				.Name = txtForumName.Text
				.Description = txtForumDescription.Text
				.EnablePostStatistics = True
				.ForumType = CType(ddlForumType.SelectedIndex, Integer)
				.CreatedByUser = UserId
				.ForumPermissions = dgPermissions.Permissions
				.EnableForumsThreadStatus = chkEnableForumsThreadStatus.Checked
				.EnableForumsRating = chkEnableForumsRating.Checked
				.ForumLink = ctlURL.Url.Trim()
				.ForumBehavior = CType((ddlForumBehavior.SelectedValue), ForumBehavior)
				'[skeel] set parent forum
				.ParentId = Convert.ToInt32(ddlParentForum.SelectedValue)
				.ForumPermissions = dgPermissions.Permissions
				.AllowPolls = chkAllowPolls.Checked
				.EnableRSS = chkEnableRSS.Checked
				' email
				.EmailAddress = txtEmailAddress.Text
				.EmailFriendlyFrom = txtEmailFriendlyFrom.Text
				' not implemented
				.NotifyByDefault = chkNotifyByDefault.Checked
				.EmailStatusChange = chkEmailStatusChange.Checked
				.EmailServer = txtEmailServer.Text
				.EmailUser = txtEmailUser.Text
				.EmailPass = txtEmailPass.Text
				.EnableSitemap = chkEnableSitemap.Checked
				.SitemapPriority = Convert.ToDouble(txtSitemapPriority.Text)

				If Not txtEmailPort.Text.Trim = String.Empty Then
					.EmailPort = CType(txtEmailPort.Text, Integer)
				End If
				.EmailEnableSSL = chkEmailEnableSSL.Checked

				' url tracking
				Dim objUrls As New UrlController
				objUrls.UpdateUrl(PortalId, ctlURL.Url, ctlURL.UrlType, ctlURL.Log, ctlURL.Track, ModuleId, ctlURL.NewWindow)
			End With

			ForumID = cntForum.ForumAdd(objForum)

			' Update the group info so it knows there is a new forum
			Forum.Components.Utilities.Caching.UpdateGroupCache(Convert.ToInt32(ddlGroup.SelectedValue), ModuleId)

			' Go Back to forum/group management screen
			Response.Redirect(Utilities.Links.ACPForumsManageLink(TabId, ModuleId, CType(ddlGroup.SelectedValue, Integer)), False)
		End Sub

		''' <summary>
		''' Deletes the forum, then redirects the user back to the forum
		''' management screen.
		''' </summary>
		''' <param name="sender">System.Object</param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
			If Not objForum Is Nothing Then
				Dim cntForum As New ForumController
				cntForum.ForumDelete(objForum.ParentId, objForum.GroupID, ForumID, ModuleId)

				'update the cached values
				Components.Utilities.Caching.UpdateForumCache(ForumID, objForum.GroupID, ModuleId)
				If objForum.ParentId > 0 Then
					Components.Utilities.Caching.UpdateForumCache(objForum.ParentId, objForum.GroupID, ModuleId)
				End If

				' Go Back to forum/group management screen
				Response.Redirect(Utilities.Links.ACPForumsManageLink(TabId, ModuleId, CType(ddlGroup.SelectedValue, Integer)), False)
			End If
		End Sub

		''' <summary>
		''' Changes the available list of parent forums on Group selected 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub ddlGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlGroup.SelectedIndexChanged
			If rowParentForum.Visible = True Then
				BindParentForum()
			End If
		End Sub

		''' <summary>
		''' Changes the permissions grid depending on the type of ForumType selected 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub ddlForumType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlForumType.SelectedIndexChanged
			HandleForumBehavior()
			SetupForumPermsGrid(False)
			HandleForumType()
		End Sub

		''' <summary>
		''' Used to select a forum and copy permissions and permission related settings from it (done by ForumID).
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Added as an enhancement, but also to ease upgrade pains.</remarks>
		Protected Sub ddlForumPermTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlForumPermTemplate.SelectedIndexChanged
			SetupForumPermsGrid(True)
		End Sub

		''' <summary>
		''' Changes the forum behavior type.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub ddlForumBehavior_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlForumBehavior.SelectedIndexChanged
			HandleForumBehavior()
			SetupForumPermsGrid(False)
		End Sub

		''' <summary>
		''' Return users back to the Forum Home page.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdHome.Click
			Response.Redirect(NavigateURL(), True)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Sets the properties of the core's URLControl (used for link management for Link Type forums)
		''' </summary>
		''' <remarks></remarks>
		Private Sub SetURLController()
			ctlURL.Required = True
			ctlURL.ShowFiles = False
			ctlURL.ShowTabs = True
			ctlURL.ShowUpLoad = False
			ctlURL.ShowUrls = True
			ctlURL.Visible = True
			ctlURL.ShowNewWindow = True

			' CP - COMEBACK - Turn off for 4.4 release
			ctlURL.ShowTrack = False
			ctlURL.ShowLog = False
		End Sub

		''' <summary>
		''' Since different forum types operate differently, we need to hide/show options based on forum type.
		''' </summary>
		''' <remarks></remarks>
		Private Sub HandleForumType()
			Select Case CType([Enum].Parse(GetType(ForumType), ddlForumType.SelectedValue, True), ForumType)
				' May need to think about how to handle moderator rows in future after removing this and updating perms grid.
				' Right now we only have special rules for linktype forum.
				Case ForumType.Link
					rowThreadStatus.Visible = False
					rowRating.Visible = False
					rowForumLink.Visible = True
					' CP - COMEBACK - Turn off for 4.4 release
					rowLinkTracking.Visible = False
					rowPolls.Visible = False
					' As of 4.4, permissions are used on all 3 forum types
					rowPermissions.Visible = True
					'[skeel] disable ParentForum if Link type
					rowParentForum.Visible = False
					'ddlParentForum.Items.FindByValue("0").Selected = True
					rowEnableSitemap.Visible = False
					rowSitemapPriority.Visible = False
				Case Else
					rowThreadStatus.Visible = True
					rowRating.Visible = True
					rowForumLink.Visible = False
					rowLinkTracking.Visible = False
					rowPolls.Visible = True

					'[skeel] handle ParentForum here
					If Not objForum Is Nothing Then
						If objForum.SubForums = 0 Then
							rowParentForum.Visible = True
						Else
							rowParentForum.Visible = False
						End If
					End If

					' As of now, permissions are used on all 3 forum types
					rowPermissions.Visible = True
			End Select
		End Sub

		''' <summary>
		''' Gets all available forums for the module and binds to the dropdownlist to allow for copy of permissions from an existing forum.
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindForumCopyLists()
			Dim objForumCnt As New ForumController
			Dim arr As List(Of ForumInfo)
			arr = objForumCnt.GetModuleForums(ModuleId)

			If arr.Count > 0 Then
				ddlForumPermTemplate.DataSource = arr
				ddlForumPermTemplate.DataBind()
			End If

			ddlForumPermTemplate.Items.Insert(0, New ListItem(DotNetNuke.Services.Localization.Localization.GetString("None", objConfig.SharedResourceFile), "0"))
		End Sub

		''' <summary>
		''' Sets up the page depending on if this forum exists or is being created
		''' </summary>
		''' <param name="Update"></param>
		''' <remarks>
		''' </remarks>
		Private Sub SetPageView(ByVal Update As Boolean)
			If Update Then
				cmdAdd.Visible = False
				cmdUpdate.Visible = True
				cmdDelete.Visible = True
			Else
				cmdAdd.Visible = True
				cmdUpdate.Visible = False
				cmdDelete.Visible = False
			End If
		End Sub

		''' <summary>
		''' This binds the forum data to the controls.  This does not run if the 
		''' user is attempting to create a new forum. (only update an existing)
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub PopulateForum()
			With objForum
				Dim ctlForumUser As New ForumUserController

				txtForumID.Text = CType(.ForumID, String)
				txtForumName.Text = .Name
				txtForumDescription.Text = .Description
				txtForumCreatorName.Text = ctlForumUser.GetForumUser(.CreatedByUser, False, ModuleId, PortalId).Username
				txtCreatedDate.Text = CType(.CreatedDate, String)
				txtUpdatedName.Text = ctlForumUser.GetForumUser(.UpdatedByUser, False, ModuleId, PortalId).Username
				txtUpdatedDate.Text = CType(.UpdatedDate, String)
				chkActive.Checked = .IsActive
				chkEnableForumsThreadStatus.Checked = .EnableForumsThreadStatus
				chkEnableForumsRating.Checked = .EnableForumsRating
				ddlForumType.Items.FindByValue(CStr(.ForumType)).Selected = True
				ddlGroup.Items.FindByValue(.GroupID.ToString).Selected = True
				chkEnableRSS.Checked = .EnableRSS
				' email
				If .EmailAddress.Trim = String.Empty Then
					txtEmailAddress.Text = objConfig.AutomatedEmailAddress
				Else
					txtEmailAddress.Text = .EmailAddress
				End If

				If .EmailFriendlyFrom.Trim = String.Empty Then
					txtEmailFriendlyFrom.Text = objConfig.EmailAddressDisplayName
				Else
					txtEmailFriendlyFrom.Text = .EmailFriendlyFrom
				End If
				' not implemented 
				chkNotifyByDefault.Checked = .NotifyByDefault
				chkEmailStatusChange.Checked = .EmailStatusChange
				txtEmailServer.Text = .EmailServer
				txtEmailUser.Text = .EmailUser
				txtEmailPass.Text = .EmailPass

				' CP - Changes
				txtEmailPort.Text = .EmailPort.ToString()
				' End Changes

				chkEmailEnableSSL.Checked = .EmailEnableSSL

				' CP - Changes
				'ddlEmailAuth.SelectedValue = .EmailAuth.ToString()
				' End Changes

				'[skeel] Show and Set ParentForums?
				Dim i As Integer = .ParentId
				If .SubForums = 0 AndAlso .ForumType <> ForumType.Link Then
					BindParentForum()
					rowParentForum.Visible = True
				Else
					rowParentForum.Visible = False
				End If

				ddlForumBehavior.SelectedValue = CStr(.ForumBehavior)
				ModeratedForum = .IsModerated
				PublicView = .PublicView
				PublicPosting = .PublicPosting
				ctlURL.Url = .ForumLink
				ctlTracking.URL = .ForumLink
				ctlTracking.ModuleID = ModuleId
				chkAllowPolls.Checked = .AllowPolls

				HandleForumType()
				SetupForumPermsGrid(False)
			End With
		End Sub

		''' <summary>
		''' This sets up the grid for usage. It is possible this is called on a postback
		''' if triggered from click chkPrivate. 
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub SetupForumPermsGrid(ByVal usingPermsTemplate As Boolean)
			If txtForumID.Text.Trim = String.Empty Then
				txtForumID.Text = "-1"
			End If

			If Not txtForumID.Text.Trim = String.Empty Then
				Dim intForumID As Integer = CType(txtForumID.Text, Integer)
				'CP - MOD PERMS
				'Dim HasSetViewPermissions As Boolean = False
				Dim cntForumPermission As New ForumPermissionController

				' CP - NEW - Allow retrieval of other forum permissions as templates
				If usingPermsTemplate And ddlForumPermTemplate.SelectedIndex > 0 Then
					' Set intForumID to ForumID of selected template forum
					intForumID = CType(ddlForumPermTemplate.SelectedValue, Integer)

					Dim objForumCnt As New ForumController
					Dim objPerForum As ForumInfo

					' set the perms stuff from the forum template selected
					objPerForum = objForumCnt.GetForumItemCache(intForumID)
					'PublicPosting = objPerForum.PublicPosting
					'PublicView = objPerForum.PublicView
					'ModeratedForum = objPerForum.IsModerated
					ddlForumBehavior.SelectedValue = CStr(objPerForum.ForumBehavior)
					ddlForumType.SelectedValue = CStr(objPerForum.ForumType)
					HandleForumBehavior()
				End If

				Dim objForumPermissionsCollection As ForumPermissionCollection = cntForumPermission.GetForumPermissionsCollection(intForumID)

				' declare roles
				Dim arrAvailableAuthViewRoles As New ArrayList
				Dim arrAvailableAuthEditRoles As New ArrayList

				'' add an entry of All Users for the View roles
				'arrAvailableAuthViewRoles.Add(New ListItem("All Users", glbRoleAllUsers))
				'' add an entry of Unauthenticated Users for the View roles
				'arrAvailableAuthViewRoles.Add(New ListItem("Unauthenticated Users", glbRoleUnauthUser))
				'' add an entry of All Users for the Edit roles
				'arrAvailableAuthEditRoles.Add(New ListItem("All Users", glbRoleAllUsers))

				' process portal roles
				Dim objRoles As New Security.Roles.RoleController
				Dim objRole As Security.Roles.RoleInfo
				Dim arrRoleInfo As ArrayList = objRoles.GetPortalRoles(PortalId)
				For Each objRole In arrRoleInfo
					If objRole.RoleID > -1 Then
						arrAvailableAuthViewRoles.Add(New ListItem(objRole.RoleName, objRole.RoleID.ToString))
					End If
				Next

				For Each objRole In arrRoleInfo
					If objRole.RoleID > -1 Then
						arrAvailableAuthEditRoles.Add(New ListItem(objRole.RoleName, objRole.RoleID.ToString))
					End If
				Next

				' get module
				Dim objModules As New Entities.Modules.ModuleController
				Dim objModule As Entities.Modules.ModuleInfo = objModules.GetModule(ModuleId, TabId, True)
				If Not objModule Is Nothing Then
					' configure grid
					Dim objDeskMod As New Entities.Modules.DesktopModuleController
					Dim desktopModule As Entities.Modules.DesktopModuleInfo = Entities.Modules.DesktopModuleController.GetDesktopModule(objModule.DesktopModuleID, PortalId)

					dgPermissions.ResourceFile = Common.Globals.ApplicationPath + "/DesktopModules/" + desktopModule.FolderName + "/" + Services.Localization.Localization.LocalResourceDirectory + "/" + Services.Localization.Localization.LocalSharedResourceFile

					dgPermissions.EnableAttachments = objConfig.EnableAttachment
					dgPermissions.ModulePermissions = objModule.ModulePermissions
					dgPermissions.PublicView = PublicView
					dgPermissions.PublicPosting = PublicPosting
					dgPermissions.ForumModerated = ModeratedForum
					dgPermissions.TabID = TabId

					If (ddlForumType.SelectedValue) = CStr(ForumType.Notification) Then
						dgPermissions.NotificationForum = True
					Else
						dgPermissions.NotificationForum = False
					End If

					Dim strRole As String
					Dim objListItem As ListItem

					' populate view roles
					Dim arrAssignedAuthViewRoles As New ArrayList
					Dim arrAuthViewRoles As Array = Split(objModule.AuthorizedViewRoles, ";")
					For Each strRole In arrAuthViewRoles
						If strRole <> String.Empty Then
							For Each objListItem In arrAvailableAuthViewRoles
								If objListItem.Value = strRole Then
									arrAssignedAuthViewRoles.Add(objListItem)
									arrAvailableAuthViewRoles.Remove(objListItem)
									Exit For
								End If
							Next
						End If
					Next

					' populate edit roles
					Dim arrAssignedAuthEditRoles As New ArrayList
					Dim arrAuthEditRoles As Array = Split(objModule.AuthorizedEditRoles, ";")
					For Each strRole In arrAuthEditRoles
						If strRole <> String.Empty Then
							For Each objListItem In arrAvailableAuthEditRoles
								If objListItem.Value = strRole Then
									arrAssignedAuthEditRoles.Add(objListItem)
									arrAvailableAuthEditRoles.Remove(objListItem)
									Exit For
								End If
							Next
						End If
					Next
				End If
			End If
		End Sub

		''' <summary>
		''' Gets the available parent forums and binds to a drop down list.
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub BindParentForum()
			Dim cntForum As New ForumController

			ddlParentForum.Items.Clear()

			'Add None option
			Dim myItem As New ListItem
			myItem.Value = "0"
			myItem.Text = Localization.GetString("None", Me.LocalResourceFile)
			ddlParentForum.Items.Insert(0, myItem)

			Dim arrItems As List(Of ForumInfo) = cntForum.GetChildForums(0, CInt(ddlGroup.SelectedItem.Value), False)
			Dim fInfo As ForumInfo

			'Make sure we only add forums that arent link type, has posts or is another subforum
			For Each fInfo In arrItems
				If fInfo.ParentId = 0 AndAlso fInfo.ForumType <> ForumType.Link _
				AndAlso fInfo.ForumID <> ForumID AndAlso fInfo.MostRecentThreadID = 0 Then
					myItem = New ListItem
					myItem.Text = fInfo.Name
					myItem.Value = CStr(fInfo.ForumID)
					If Not objForum Is Nothing Then
						If fInfo.ForumID = objForum.ParentId Then myItem.Selected = True
					End If
					ddlParentForum.Items.Add(myItem)
				End If
			Next

			HandleForumType()
		End Sub

		''' <summary>
		''' Gets the forum groups and binds to a drop down list.
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub BindGroup()
			Dim cntGroup As New GroupController

			With ddlGroup
				.DataSource = cntGroup.GroupsGetByModuleID(ModuleId)
				.DataTextField = "Name"
				.DataValueField = "GroupID"
				.DataBind()
			End With
		End Sub

		''' <summary>
		''' Gets the forum enumerator of types and binds to a drop down list.
		''' </summary>
		''' <remarks>Entires in Lists table.
		''' </remarks>
		Private Sub BindLists()
			ddlForumType.Items.Clear()
			ddlForumType.Items.Insert(0, New ListItem(Localization.GetString("Normal", objConfig.SharedResourceFile), "0"))
			ddlForumType.Items.Insert(1, New ListItem(Localization.GetString("Notification", objConfig.SharedResourceFile), "1"))
			ddlForumType.Items.Insert(2, New ListItem(Localization.GetString("Link", objConfig.SharedResourceFile), "2"))

			ddlForumBehavior.Items.Clear()
			ddlForumBehavior.Items.Insert(0, New ListItem(Localization.GetString("PublicModerated", objConfig.SharedResourceFile), "0"))
			ddlForumBehavior.Items.Insert(1, New ListItem(Localization.GetString("PublicModeratedPostRestricted", objConfig.SharedResourceFile), "1"))
			ddlForumBehavior.Items.Insert(2, New ListItem(Localization.GetString("PublicUnModerated", objConfig.SharedResourceFile), "2"))
			ddlForumBehavior.Items.Insert(3, New ListItem(Localization.GetString("PublicUnModeratedPostRestricted", objConfig.SharedResourceFile), "3"))
			ddlForumBehavior.Items.Insert(4, New ListItem(Localization.GetString("PrivateModerated", objConfig.SharedResourceFile), "4"))
			ddlForumBehavior.Items.Insert(5, New ListItem(Localization.GetString("PrivateModeratedPostRestricted", objConfig.SharedResourceFile), "5"))
			ddlForumBehavior.Items.Insert(6, New ListItem(Localization.GetString("PrivateUnModerated", objConfig.SharedResourceFile), "6"))
			ddlForumBehavior.Items.Insert(7, New ListItem(Localization.GetString("PrivateUnModeratedPostRestricted", objConfig.SharedResourceFile), "7"))

			' CP - Changes
			' ddlForumBehavior.Items.Insert(8, New ListItem(Localization.GetString("EmailAuth", objConfig.SharedResourceFile), "8"))
			' End Changes
		End Sub

		''' <summary>
		''' Used to change the forum behavior. (ie. private, moderated, posting restrictions, etc.)
		''' </summary>
		''' <remarks></remarks>
		Private Sub HandleForumBehavior()
			Select Case CType([Enum].Parse(GetType(ForumBehavior), ddlForumBehavior.SelectedValue, True), ForumBehavior)
				Case ForumBehavior.PrivateModerated
					PublicView = False
					ModeratedForum = True
					PublicPosting = True
				Case ForumBehavior.PrivateModeratedPostRestricted
					PublicView = False
					ModeratedForum = True
					PublicPosting = False
				Case ForumBehavior.PrivateUnModerated
					PublicView = False
					ModeratedForum = False
					PublicPosting = True
				Case ForumBehavior.PrivateUnModeratedPostRestricted
					PublicView = False
					ModeratedForum = False
					PublicPosting = False
				Case ForumBehavior.PublicModerated
					PublicView = True
					ModeratedForum = True
					PublicPosting = True
				Case ForumBehavior.PublicModeratedPostRestricted
					PublicView = True
					ModeratedForum = True
					PublicPosting = False
				Case ForumBehavior.PublicUnModerated
					PublicView = True
					ModeratedForum = False
					PublicPosting = True
				Case ForumBehavior.PublicUnModeratedPostRestricted
					PublicView = True
					ModeratedForum = False
					PublicPosting = False
			End Select
		End Sub

		''' <summary>
		''' Contructs a list of user interface tabs. 
		''' </summary>
		''' <remarks></remarks>
		Private Sub BuildTabs()
			Dim tabForums As New Telerik.Web.UI.RadTab
			tabForums.Text = Localization.GetString("TabGeneral", LocalResourceFile)
			tabForums.PageViewID = "rpvGeneral"
			rtsForum.Tabs.Add(tabForums)

			Dim tabThreads As New Telerik.Web.UI.RadTab
			tabThreads.Text = Localization.GetString("TabOptions", LocalResourceFile)
			tabThreads.PageViewID = "rpvOptions"
			rtsForum.Tabs.Add(tabThreads)

			Dim tabEmail As New Telerik.Web.UI.RadTab
			tabEmail.Text = Localization.GetString("TabEmail", LocalResourceFile)
			tabEmail.PageViewID = "rpvEmail"
			rtsForum.Tabs.Add(tabEmail)
		End Sub

#End Region

	End Class

End Namespace