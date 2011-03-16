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

Imports DotNetNuke.Modules.Forum.Utilities
Imports DotNetNuke.Wrapper.UI.WebControls
Imports Telerik.Web.UI

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

				Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
				ForumUtils.LoadCssFile(DefaultPage, objConfig)

				If Not Page.IsPostBack Then
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
                            rcbGroup.Items.FindItemByValue(GroupID.ToString).Selected = True
							'Typical new forum settings (default)
							chkActive.Checked = True
                            rcbForumBehavior.SelectedValue = CStr(ForumBehavior.PublicUnModerated)
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
							textSitemapPriority.Value = objConfig.SitemapPriority

							SetPageView(False)
							Dim objForum As ForumInfo = New ForumInfo
							'[skeel] load in parentforums
							BindParentForums()
						Else
							'[skeel] No GroupID means bad url right?
							Response.Redirect(Utilities.Links.ACPControlLink(TabId, ModuleId, AdminAjaxControl.Main))
							Exit Sub
						End If
					End If
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
			Dim OriginalParentID As Integer
			Dim OriginalGroupID As Integer
			' Make sure we are working w/ the current forumID
			objForum = cntForum.GetForumItemCache(ForumID)
			OriginalParentID = objForum.ParentID
			OriginalGroupID = objForum.GroupID

			With objForum
                .GroupID = Convert.ToInt32(rcbGroup.SelectedValue)
				.ForumID = ForumID
				.IsActive = chkActive.Checked
				If txtForumName.Text <> "" Then
					.Name = txtForumName.Text
				Else
					' set validation
					Exit Sub
				End If

				.Description = txtForumDescription.Text
                .ForumType = CType(rcbForumType.SelectedIndex, Integer)
				.UpdatedByUser = UserId
				.ForumPermissions = dgPermissions.Permissions
				.EnableForumsThreadStatus = chkEnableForumsThreadStatus.Checked
				.EnableForumsRating = chkEnableForumsRating.Checked
				.ForumLink = ctlURL.Url.Trim()
                .ForumBehavior = CType((rcbForumBehavior.SelectedValue), ForumBehavior)
				'[skeel] set parent forum
				If rowParentForum.Visible = True Then
                    .ParentID = Convert.ToInt32(rcbParentForum.SelectedValue)
				End If
				.ForumPermissions = dgPermissions.Permissions
				.AllowPolls = chkAllowPolls.Checked
				.EnableRSS = chkEnableRSS.Checked
				.EmailAddress = txtEmailAddress.Text
				.EmailFriendlyFrom = txtEmailFriendlyFrom.Text
				' not implemented
				.NotifyByDefault = chkNotifyByDefault.Checked
				.EmailStatusChange = chkEmailStatusChange.Checked
				.EmailServer = txtEmailServer.Text
				.EmailUser = txtEmailUser.Text
				.EmailPass = txtEmailPass.Text
				.EnableSitemap = chkEnableSitemap.Checked
				If textSitemapPriority.Text <> "" Then
					.SitemapPriority = CType(textSitemapPriority.Value, Single)
				Else
					.SitemapPriority = 0.5
				End If

				If Not txtEmailPort.Text.Trim = String.Empty Then
					.EmailPort = CType(txtEmailPort.Text, Integer)
				End If
				.EmailEnableSSL = chkEmailEnableSSL.Checked

				' url tracking
				Dim objUrls As New UrlController
				objUrls.UpdateUrl(PortalId, ctlURL.Url, ctlURL.UrlType, ctlURL.Log, ctlURL.Track, ModuleId, ctlURL.NewWindow)
			End With

			cntForum.ForumUpdate(objForum, OriginalParentID)

			' Update the group/forum cache
			Forum.Components.Utilities.Caching.UpdateForumCache(objForum.ForumID, OriginalGroupID, ModuleId, OriginalParentID)

			If Not (OriginalParentID = objForum.ParentID) Then
				' need to update cache of new parent forum (so it sees the new child forum)
				Forum.Components.Utilities.Caching.UpdateForumCache(objForum.ForumID, objForum.GroupID, ModuleId, objForum.ParentID)
			End If

			If Not (OriginalGroupID = objForum.GroupID) Then
				' need to update cache of new group (we handled original group already)
				Forum.Components.Utilities.Caching.UpdateGroupCache(objForum.GroupID, ModuleId)
			End If

			'Forum.Components.Utilities.Caching.UpdateChildForumCache(objForum.ParentID, objForum.GroupID, ModuleId)
			' Go Back to forum/group management screen
            Response.Redirect(Utilities.Links.ACPForumsManageLink(TabId, ModuleId, CType(rcbGroup.SelectedValue, Integer)), False)
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
                .GroupID = Convert.ToInt32(rcbGroup.SelectedValue)
				.IsActive = chkActive.Checked
				.Name = txtForumName.Text
				.Description = txtForumDescription.Text
                .ForumType = CType(rcbForumType.SelectedIndex, Integer)
				.CreatedByUser = UserId
				.ForumPermissions = dgPermissions.Permissions
				.EnableForumsThreadStatus = chkEnableForumsThreadStatus.Checked
				.EnableForumsRating = chkEnableForumsRating.Checked
				.ForumLink = ctlURL.Url.Trim()
                .ForumBehavior = CType((rcbForumBehavior.SelectedValue), ForumBehavior)
				'[skeel] set parent forum
                .ParentID = Convert.ToInt32(rcbParentForum.SelectedValue)
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
				If textSitemapPriority.Text <> "" Then
					.SitemapPriority = Convert.ToSingle(textSitemapPriority.Text)
				Else
					.SitemapPriority = 0.5
				End If

				If Not txtEmailPort.Text.Trim = String.Empty Then
					.EmailPort = CType(txtEmailPort.Text, Integer)
				End If
				.EmailEnableSSL = chkEmailEnableSSL.Checked

				' url tracking
				Dim objUrls As New UrlController
				objUrls.UpdateUrl(PortalId, ctlURL.Url, ctlURL.UrlType, ctlURL.Log, ctlURL.Track, ModuleId, ctlURL.NewWindow)
			End With

			ForumID = cntForum.ForumAdd(objForum)

			' update cache
			Forum.Components.Utilities.Caching.UpdateForumCache(objForum.ForumID, objForum.GroupID, ModuleId, objForum.ParentID)

			' Go Back to forum/group management screen
            Response.Redirect(Utilities.Links.ACPForumsManageLink(TabId, ModuleId, CType(rcbGroup.SelectedValue, Integer)), False)
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
				cntForum.ForumDelete(objForum.ParentID, objForum.GroupID, ForumID, ModuleId)

				'update the cached values
				Components.Utilities.Caching.UpdateForumCache(ForumID, objForum.GroupID, ModuleId, objForum.ParentID)

				' Go Back to forum/group management screen
                Response.Redirect(Utilities.Links.ACPForumsManageLink(TabId, ModuleId, CType(rcbGroup.SelectedValue, Integer)), False)
			End If
		End Sub

		''' <summary>
		''' Changes the available list of parent forums on Group selected 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
        Protected Sub rcbGroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcbGroup.SelectedIndexChanged
            If rowParentForum.Visible = True Then
                BindParentForums()
            End If
        End Sub

		''' <summary>
		''' Changes the permissions grid depending on the type of ForumType selected 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
        Protected Sub ddlForumType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcbForumType.SelectedIndexChanged
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
        Protected Sub ddlForumPermTemplate_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcbForumPermTemplate.SelectedIndexChanged
            SetupForumPermsGrid(True)
        End Sub

		''' <summary>
		''' Changes the forum behavior type.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
        Protected Sub ddlForumBehavior_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcbForumBehavior.SelectedIndexChanged
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
            Select Case CType([Enum].Parse(GetType(ForumType), rcbForumType.SelectedValue, True), ForumType)
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
                rcbForumPermTemplate.DataSource = arr
                rcbForumPermTemplate.DataBind()
			End If

            rcbForumPermTemplate.Items.Insert(0, New RadComboBoxItem(DotNetNuke.Services.Localization.Localization.GetString("None", objConfig.SharedResourceFile), "0"))
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
                rcbForumType.Items.FindItemByValue(CStr(.ForumType)).Selected = True
                rcbGroup.Items.FindItemByValue(.GroupID.ToString).Selected = True
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
				txtEmailPort.Text = .EmailPort.ToString()
				chkEmailEnableSSL.Checked = .EmailEnableSSL
				' sitemap
				chkEnableSitemap.Checked = .EnableSitemap
				If .SitemapPriority > 1 Then
					textSitemapPriority.Value = 0.5
				Else
					textSitemapPriority.Value = .SitemapPriority
				End If

				If .SubForums = 0 AndAlso .ForumType <> ForumType.Link Then
					BindParentForums()
					If .ContainsChildForums Or (.TotalThreads = 0) Then
						rowParentForum.Visible = True
					Else
						If .ContainsChildForums = False Then
							rowParentForum.Visible = True
						Else
							rowParentForum.Visible = False
						End If
					End If
				Else
					rowParentForum.Visible = False
				End If

                rcbForumBehavior.SelectedValue = CStr(.ForumBehavior)
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
                If usingPermsTemplate And rcbForumPermTemplate.SelectedIndex > 0 Then
                    ' Set intForumID to ForumID of selected template forum
                    intForumID = CType(rcbForumPermTemplate.SelectedValue, Integer)

                    Dim objForumCnt As New ForumController
                    Dim objPerForum As ForumInfo

                    ' set the perms stuff from the forum template selected
                    objPerForum = objForumCnt.GetForumItemCache(intForumID)
                    'PublicPosting = objPerForum.PublicPosting
                    'PublicView = objPerForum.PublicView
                    'ModeratedForum = objPerForum.IsModerated
                    rcbForumBehavior.SelectedValue = CStr(objPerForum.ForumBehavior)
                    rcbForumType.SelectedValue = CStr(objPerForum.ForumType)
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

                    If (rcbForumType.SelectedValue) = CStr(ForumType.Notification) Then
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
		Private Sub BindParentForums()
			Dim cntForum As New ForumController

            rcbParentForum.Items.Clear()

			'Add None option
            Dim myItem As New Telerik.Web.UI.RadComboBoxItem
			myItem.Value = "0"
			myItem.Text = Localization.GetString("None", Me.LocalResourceFile)
            rcbParentForum.Items.Insert(0, myItem)

			'Dim arrItems As List(Of ForumInfo) = cntForum.GetChildForums(0, CInt(ddlGroup.SelectedItem.Value), False)
            Dim arrItems As List(Of ForumInfo) = cntForum.GetParentForums(CInt(rcbGroup.SelectedItem.Value))

			'Make sure we only add forums that arent link type, has posts or is another subforum
			For Each fInfo As ForumInfo In arrItems
				If fInfo.ParentID = 0 AndAlso fInfo.ForumType <> ForumType.Link _
				AndAlso fInfo.ForumID <> ForumID Then
					' we can't use lastpostid or mostrecentpostid here, not can we use total threads/posts (since a parent is inclusive of all children). Unfortunately, this means we have to get the forumn info direct from db. 
					Dim singleForum As ForumInfo = cntForum.GetForumItemCache(fInfo.ForumID)

					If singleForum.ContainsChildForums Or (singleForum.TotalThreads = 0) Then
                        myItem = New RadComboBoxItem
						myItem.Text = singleForum.Name
						myItem.Value = CStr(singleForum.ForumID)

						If Not objForum Is Nothing Then
							If singleForum.ForumID = objForum.ParentID Then myItem.Selected = True
						End If

                        rcbParentForum.Items.Add(myItem)
					End If
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

            With rcbGroup
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
            rcbForumType.Items.Clear()
            rcbForumType.Items.Insert(0, New RadComboBoxItem(Localization.GetString("Normal", objConfig.SharedResourceFile), "0"))
            rcbForumType.Items.Insert(1, New RadComboBoxItem(Localization.GetString("Notification", objConfig.SharedResourceFile), "1"))
            rcbForumType.Items.Insert(2, New RadComboBoxItem(Localization.GetString("Link", objConfig.SharedResourceFile), "2"))

            rcbForumBehavior.Items.Clear()
            rcbForumBehavior.Items.Insert(0, New RadComboBoxItem(Localization.GetString("PublicModerated", objConfig.SharedResourceFile), "0"))
            rcbForumBehavior.Items.Insert(1, New RadComboBoxItem(Localization.GetString("PublicModeratedPostRestricted", objConfig.SharedResourceFile), "1"))
            rcbForumBehavior.Items.Insert(2, New RadComboBoxItem(Localization.GetString("PublicUnModerated", objConfig.SharedResourceFile), "2"))
            rcbForumBehavior.Items.Insert(3, New RadComboBoxItem(Localization.GetString("PublicUnModeratedPostRestricted", objConfig.SharedResourceFile), "3"))
            rcbForumBehavior.Items.Insert(4, New RadComboBoxItem(Localization.GetString("PrivateModerated", objConfig.SharedResourceFile), "4"))
            rcbForumBehavior.Items.Insert(5, New RadComboBoxItem(Localization.GetString("PrivateModeratedPostRestricted", objConfig.SharedResourceFile), "5"))
            rcbForumBehavior.Items.Insert(6, New RadComboBoxItem(Localization.GetString("PrivateUnModerated", objConfig.SharedResourceFile), "6"))
            rcbForumBehavior.Items.Insert(7, New RadComboBoxItem(Localization.GetString("PrivateUnModeratedPostRestricted", objConfig.SharedResourceFile), "7"))

			' CP - Changes
			' ddlForumBehavior.Items.Insert(8, New ListItem(Localization.GetString("EmailAuth", objConfig.SharedResourceFile), "8"))
			' End Changes
		End Sub

		''' <summary>
		''' Used to change the forum behavior. (ie. private, moderated, posting restrictions, etc.)
		''' </summary>
		''' <remarks></remarks>
		Private Sub HandleForumBehavior()
            Select Case CType([Enum].Parse(GetType(ForumBehavior), rcbForumBehavior.SelectedValue, True), ForumBehavior)
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