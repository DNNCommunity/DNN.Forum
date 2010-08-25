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

'<Assembly: WebResource("forum.js", "text/javascript")> 
Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This is the Admin Control Panel (ACP). It consists of a menu structure.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class Menu
		Inherits System.Web.UI.UserControl

#Region "Private Members"

		Dim _EnableAjax As Boolean = True
		Dim _LocalResourceFile As String = String.Empty
		Dim _DefaultControlToLoad As String = "ACP_Main.ascx"
		Dim _objConfig As Forum.Configuration
		Dim _PortalID As Integer
		Dim _ModuleID As Integer
		Dim SectionNumber As Integer = 1
		Dim ctlOverview As String = "ACP_Main.ascx"
		Dim ctlGeneral As String = "ACP_General.ascx"
		Dim ctlCommunity As String = "ACP_Community.ascx"
		Dim ctlAttachment As String = "ACP_Attachment.ascx"
		Dim ctlRSS As String = "ACP_RSS.ascx"
		Dim ctlSEO As String = "ACP_SEO.ascx"
		Dim ctlForumManager As String = "ACP_ForumManage.ascx"
		Dim ctlManageUsers As String = "ACP_User.ascx"
		Dim ctlAvatar As String = "ACP_Avatar.ascx"
		Dim ctlRoleAvatar As String = "ACP_RoleAvatar.ascx"
		Dim ctlUserSettings As String = "ACP_UserSettings.ascx"
		Dim ctlUserInterface As String = "ACP_UI.ascx"
		Dim ctlFilterMain As String = "ACP_Filter.ascx"
		Dim ctlFilterWords As String = "ACP_FilterWord.ascx"
		Dim ctlRanking As String = "ACP_Ranking.ascx"
		Dim ctlRating As String = "ACP_Rating.ascx"
		Dim ctlPopStatus As String = "ACP_PopStatus.ascx"
		Dim ctlEmailSettings As String = "ACP_Email.ascx"
		Dim ctlEmailTemplate As String = "ACP_EmailTemplate.ascx"
		Dim ctlEmailQueue As String = "ACP_EmailQueue.ascx"
		Dim ctlEmailQueueTaskDetail As String = "ACP_EmailQueueTaskDetail.ascx"
		Dim ctlEmailSubscribers As String = "ACP_EmailSubscribers.ascx"
		Dim ctlUser As String = "ACP_User.ascx"
		Dim ctlForumEdit As String = "ACP_ForumEdit.ascx"
		Dim cmdOverview As LinkButton
		Dim cmdGeneral As LinkButton
		Dim cmdCommunity As LinkButton
		Dim cmdAttachment As LinkButton
		Dim cmdRSS As LinkButton
		Dim cmdSEO As LinkButton
		Dim cmdUser As LinkButton
		Dim cmdAvatar As LinkButton
		Dim cmdUserSettings As LinkButton
		Dim cmdUserInterface As LinkButton
		Dim cmdFilterMain As LinkButton
		Dim cmdFilterWords As LinkButton
		Dim cmdRanking As LinkButton
		Dim cmdRating As LinkButton
		Dim cmdPopStatus As LinkButton
		Dim cmdEmailSettings As LinkButton
		Dim cmdEmailQueue As LinkButton
		Dim cmdEmailQueueTaskDetail As LinkButton
		Dim cmdEmailSubscribers As LinkButton

		''' <summary>
		''' The various sections of the menu that can be expanded/collapsed.
		''' </summary>
		''' <remarks></remarks>
		Private Enum ExpandMenuSection
			MainConfiguration = 1
			Forums = 2
			UserOriented = 3
			Content = 4
			Email = 5
		End Enum

		''' <summary>
		''' Local Resource file for localization. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property LocalResourceFile() As String
			Get
				Dim fileRoot As String

				If _LocalResourceFile = String.Empty Then
					fileRoot = Me.TemplateSourceDirectory & "/" & Localization.LocalResourceDirectory & "/ACP_Menu.ascx"
				Else
					fileRoot = _LocalResourceFile
				End If
				Return fileRoot
			End Get
			Set(ByVal Value As String)
				_LocalResourceFile = Value
			End Set
		End Property

#End Region

#Region "Public Properties"

		''' <summary>
		''' Enable Ajax tells the menu if menu links should be rendered as links for all items(if false).
		''' </summary>
		''' <value></value>
		''' <returns>True, if we are on a control that supports Ajax.</returns>
		''' <remarks>Currently, Forum Manager, Email Template Manager do not support Ajax.</remarks>
		Public Property EnableAjax() As Boolean
			Get
				Return _EnableAjax
			End Get
			Set(ByVal value As Boolean)
				_EnableAjax = value
			End Set
		End Property

		''' <summary>
		''' The forum configuration for the module this control is loaded into. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property objConfig() As Forum.Configuration
			Get
				Return _objConfig
			End Get
			Set(ByVal value As Forum.Configuration)
				_objConfig = value
			End Set
		End Property

		''' <summary>
		''' The PortalID this menu is loaded into. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PortalID() As Integer
			Get
				Return _PortalID
			End Get
			Set(ByVal value As Integer)
				_PortalID = value
			End Set
		End Property

		''' <summary>
		''' The ModuleID this menu is loaded into. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal value As Integer)
				_ModuleID = value
			End Set
		End Property

		''' <summary>
		''' This is the control we want to load in ACP.ascx via Ajax.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>We need viewstate to remember current on postback</remarks>
		Public Property ControlToLoad() As String
			Get
				If CStr(ViewState("CtlToLoad")) <> String.Empty Then
					_DefaultControlToLoad = CStr(ViewState("CtlToLoad"))
				End If
				Return _DefaultControlToLoad
			End Get
			Set(ByVal value As String)
				_DefaultControlToLoad = value
				ViewState("CtlToLoad") = value
			End Set
		End Property

#End Region

#Region "Public Events"

		''' <summary>
		''' This event is used so the host control can load the appropriate user control when Ajax is enabled. 
		''' </summary>
		''' <remarks></remarks>
		Public Event MenuClicked As EventHandler

#End Region

#Region "Private Methods"

		''' <summary>
		''' Determines which control to load dynamically, typically via Ajax. 
		''' </summary>
		''' <param name="CurrentPage"></param>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function IdentifyControl(ByVal CurrentPage As AdminAjaxControl, ByVal sender As Object, ByVal e As EventArgs) As String
			Select Case CurrentPage
				Case AdminAjaxControl.General
					ControlToLoad = ctlGeneral
				Case AdminAjaxControl.Community
					ControlToLoad = ctlCommunity
				Case AdminAjaxControl.Attachment
					ControlToLoad = ctlAttachment
				Case AdminAjaxControl.RSS
					ControlToLoad = ctlRSS
				Case AdminAjaxControl.SEO
					ControlToLoad = ctlSEO
				Case AdminAjaxControl.ForumManage
					ControlToLoad = ctlForumManager
				Case AdminAjaxControl.ForumEdit
					ControlToLoad = ctlForumEdit
				Case AdminAjaxControl.Users
					ControlToLoad = ctlManageUsers
				Case AdminAjaxControl.Avatar
					ControlToLoad = ctlAvatar
				Case AdminAjaxControl.RoleAvatar
					ControlToLoad = ctlRoleAvatar
				Case AdminAjaxControl.UserSettings
					ControlToLoad = ctlUserSettings
				Case AdminAjaxControl.UserInterface
					ControlToLoad = ctlUserInterface
				Case AdminAjaxControl.FilterMain
					ControlToLoad = ctlFilterMain
				Case AdminAjaxControl.FilterWord
					ControlToLoad = ctlFilterWords
				Case AdminAjaxControl.Ranking
					ControlToLoad = ctlRanking
				Case AdminAjaxControl.Rating
					ControlToLoad = ctlRating
				Case AdminAjaxControl.PopStatus
					ControlToLoad = ctlPopStatus
				Case AdminAjaxControl.EmailSettings
					ControlToLoad = ctlEmailSettings
				Case AdminAjaxControl.EmailTemplate
					ControlToLoad = ctlEmailTemplate
				Case AdminAjaxControl.EmailQueue
					ControlToLoad = ctlEmailQueue
				Case AdminAjaxControl.EmailQueueTaskDetail
					ControlToLoad = ctlEmailQueueTaskDetail
				Case AdminAjaxControl.EmailSubscribers
					ControlToLoad = ctlEmailSubscribers
				Case Else
					ControlToLoad = ctlOverview
			End Select

			Return ControlToLoad
		End Function

		''' <summary>
		''' Used to generate a menu section head (root item). 
		''' </summary>
		''' <param name="wr">The HtmlTextWriter we are creating the UI with.</param>
		''' <param name="SectionString">The text that will be rendered as the title of this particular menu section.</param>
		''' <param name="IsExpanded">True if this particular section of the menu should be expanded, false otherwise.</param>
		''' <param name="Height">The display height this section of the menu requires for rendering properly.</param>
		''' <remarks></remarks>
		Private Sub GenerateMenuSection(ByVal wr As HtmlTextWriter, ByVal SectionString As String, ByVal IsExpanded As Boolean, ByVal Height As Integer)
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_MenuItem")
			wr.AddAttribute("onmousedown", "togglemenu('ucp" & SectionNumber.ToString() & "');")
			wr.RenderBeginTag(HtmlTextWriterTag.Div) ' <div>	

			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_HeaderText")
			wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>	
			wr.Write(SectionString)
			wr.RenderEndTag() ' </span>
			wr.RenderEndTag() ' </div>

			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_MenuGroup")
			wr.AddAttribute(HtmlTextWriterAttribute.Id, "ucp" & SectionNumber.ToString())
			If IsExpanded Then
				wr.AddAttribute(HtmlTextWriterAttribute.Style, "display:block;height:" & CStr(Height) & "px")
			Else
				wr.AddAttribute(HtmlTextWriterAttribute.Style, "display:none;height:" & CStr(Height) & "px")
			End If
			wr.RenderBeginTag(HtmlTextWriterTag.Div) ' <div>

			'Add to section count
			SectionNumber = SectionNumber + 1
		End Sub

		''' <summary>
		''' Used to determine if the current menu section is to be expanded or not, 
		''' this depends on the current control generating the menu structure.
		''' </summary>
		''' <param name="SectionNumber">The menu section number we need to know if it should be expanded or not.</param>
		''' <param name="ExpandSection">The menu section number that is set to be expanded.</param>
		''' <returns>True if this section of the menu is expanded, false otherwise.</returns>
		''' <remarks></remarks>
		Private Function IsExpanded(ByVal SectionNumber As Integer, ByVal ExpandSection As Integer) As Boolean
			If SectionNumber = ExpandSection Then
				Return True
			Else
				Return False
			End If
		End Function

		''' <summary>
		''' Creates a Link Button used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="URL">The url to navigate to when clicked.</param>
		''' <param name="Text">The string of text to show to the end user.</param>
		''' <param name="Css">The css class being applied to the link button.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Private Sub RenderLinkButton(ByVal wr As HtmlTextWriter, ByVal URL As String, ByVal Text As String, ByVal Css As String)
			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If
			wr.AddAttribute(HtmlTextWriterAttribute.Href, URL)
			wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
			wr.Write(Text)
			wr.RenderEndTag() ' </A>
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Loads the menu javascript file (forum.js) onto the page.
		''' </summary>
		''' <remarks></remarks>
		Protected Sub Control_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
			Me.Page.ClientScript.RegisterClientScriptResource(GetType(Menu), "forum.js")
		End Sub

		''' <summary>
		''' Builds the navigation menu user interface. 
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Protected Overrides Sub Render(ByVal wr As HtmlTextWriter)
			Try
				'Now we need to figure out what section to expand
				Dim ExpandSection As Integer = -1
				Dim url As String = String.Empty

				Select Case ControlToLoad
					Case ctlOverview
						ExpandSection = ExpandMenuSection.MainConfiguration
					Case ctlGeneral
						ExpandSection = ExpandMenuSection.MainConfiguration
					Case ctlCommunity
						ExpandSection = ExpandMenuSection.MainConfiguration
					Case ctlAttachment
						ExpandSection = ExpandMenuSection.MainConfiguration
					Case ctlRSS
						ExpandSection = ExpandMenuSection.MainConfiguration
					Case ctlSEO
						ExpandSection = ExpandMenuSection.MainConfiguration
					Case ctlForumManager
						ExpandSection = ExpandMenuSection.Forums
					Case ctlForumEdit
						ExpandSection = ExpandMenuSection.Forums
					Case ctlManageUsers
						ExpandSection = ExpandMenuSection.UserOriented
					Case ctlAvatar
						ExpandSection = ExpandMenuSection.UserOriented
					Case ctlRoleAvatar
						ExpandSection = ExpandMenuSection.UserOriented
					Case ctlUserSettings
						ExpandSection = ExpandMenuSection.UserOriented
					Case ctlUserInterface
						ExpandSection = ExpandMenuSection.UserOriented
					Case ctlFilterMain
						ExpandSection = ExpandMenuSection.Content
					Case ctlFilterWords
						ExpandSection = ExpandMenuSection.Content
					Case ctlRanking
						ExpandSection = ExpandMenuSection.Content
					Case ctlRating
						ExpandSection = ExpandMenuSection.Content
					Case ctlPopStatus
						ExpandSection = ExpandMenuSection.Content
					Case ctlEmailSettings
						ExpandSection = ExpandMenuSection.Email
					Case ctlEmailTemplate
						ExpandSection = ExpandMenuSection.Email
					Case ctlEmailQueue
						ExpandSection = ExpandMenuSection.Email
					Case ctlEmailQueueTaskDetail
						ExpandSection = ExpandMenuSection.Email
					Case ctlEmailSubscribers
						ExpandSection = ExpandMenuSection.Email
				End Select

				'Lets build the menu structure
				Dim imgSelectedURL As String = objConfig.GetThemeImageURL("curpage.") & objConfig.ImageExtension
				Dim imgSubLevelURL As String = objConfig.GetThemeImageURL("sublevel.") & objConfig.ImageExtension
				Dim imgSelectedToolTip As String = Localization.GetString("imgSelected", Me.LocalResourceFile)

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Menu")
				wr.RenderBeginTag(HtmlTextWriterTag.Div) ' <div>	

				'First Menu section, contains overview, tracking, bookmarks and a link to the forum home
				GenerateMenuSection(wr, Localization.GetString("ControlPanel", Me.LocalResourceFile), IsExpanded(1, ExpandSection), 150)
				wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "90%")
				wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>

				'Overview
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlOverview Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				'Add control or link
				If EnableAjax Then
					cmdOverview.RenderControl(wr)
				Else
					url = Utilities.Links.ForumAdminLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID)
					RenderLinkButton(wr, url, Localization.GetString("cmdOverview", Me.LocalResourceFile), "Forum_Link")
				End If
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'General
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlGeneral Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdGeneral.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.General)
					RenderLinkButton(wr, url, Localization.GetString("cmdGeneral", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Community
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlCommunity Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdCommunity.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.Community)
					RenderLinkButton(wr, url, Localization.GetString("cmdCommunity", Me.LocalResourceFile), "Forum_Link")
				End If
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' Attachment
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlAttachment Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdAttachment.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.Attachment)
					RenderLinkButton(wr, url, Localization.GetString("cmdAttachment", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' RSS
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlRSS Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax = True Then
					cmdRSS.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.RSS)
					RenderLinkButton(wr, url, Localization.GetString("cmdRSS", Me.LocalResourceFile), "Forum_Link")
				End If
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' SEO
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlSEO Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdSEO.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.SEO)
					RenderLinkButton(wr, url, Localization.GetString("cmdSEO", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Close section
				wr.RenderEndTag() ' </table>
				wr.RenderEndTag() ' </div>

				'Second Menu section, contains forum manage
				GenerateMenuSection(wr, Localization.GetString("Forums", Me.LocalResourceFile), IsExpanded(2, ExpandSection), 40)
				wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
				wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>

				' Forum Manager
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlForumManager Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.AddAttribute(HtmlTextWriterAttribute.Colspan, "2")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				url = Utilities.Links.ACPForumsManageLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, -1)
				RenderLinkButton(wr, url, Localization.GetString("cmdForumManager", Me.LocalResourceFile), "Forum_Link")

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' Forum Edit
				If ControlToLoad = ctlForumEdit Then
					wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>

					wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
					wr.RenderEndTag() '</td>

					wr.AddAttribute(HtmlTextWriterAttribute.Width, "10")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSubLevelURL)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>

					wr.RenderEndTag() ' </td>

					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

					wr.Write("<span class=""Forum_Link"">" & Localization.GetString("cmdForumEdit", Me.LocalResourceFile) & "</span>")

					wr.RenderEndTag() ' </td>
					wr.RenderEndTag() ' </tr>
				End If

				'Close section
				wr.RenderEndTag() ' </table>
				wr.RenderEndTag() ' </div>

				'Third Menu section, user oriented
				If objConfig.EnableRoleAvatar = True Then
					GenerateMenuSection(wr, Localization.GetString("UserSection", Me.LocalResourceFile), IsExpanded(3, ExpandSection), 100)
				Else
					GenerateMenuSection(wr, Localization.GetString("UserSection", Me.LocalResourceFile), IsExpanded(3, ExpandSection), 80)
				End If
				wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
				wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>

				'Manage Users
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlManageUsers Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdUser.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.Users)
					RenderLinkButton(wr, url, Localization.GetString("cmdUser", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' Avatar
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlAvatar Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If EnableAjax Then
					cmdAvatar.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.Avatar)
					RenderLinkButton(wr, url, Localization.GetString("cmdAvatar", Me.LocalResourceFile), "Forum_Link")
				End If
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'RoleAvatar
				If objConfig.EnableRoleAvatar = True Then

					wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

					If ControlToLoad = ctlRoleAvatar Then
						wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
						wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
						wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
						wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
						wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
						wr.RenderEndTag() ' </img>
					End If
					wr.RenderEndTag() ' </td>

					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

					url = Utilities.Links.ACPRoleAvatarManagerLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID)
					RenderLinkButton(wr, url, Localization.GetString("cmdRoleAvatar", Me.LocalResourceFile), "Forum_Link")

					wr.RenderEndTag() ' </td>
					wr.RenderEndTag() ' </tr>
				End If

				' User Settings
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlUserSettings Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdUserSettings.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.UserSettings)
					RenderLinkButton(wr, url, Localization.GetString("cmdUserSettings", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' User Interface
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlUserInterface Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdUserInterface.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.UserSettings)
					RenderLinkButton(wr, url, Localization.GetString("cmdUserInterface", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Close Section
				wr.RenderEndTag() ' </table>
				wr.RenderEndTag() ' </div>

				'Fourth Menu section, Content
				GenerateMenuSection(wr, Localization.GetString("ContentSection", Me.LocalResourceFile), IsExpanded(4, ExpandSection), 100)

				wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
				wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>

				'Filter Main
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlFilterMain Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdFilterMain.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.FilterMain)
					RenderLinkButton(wr, url, Localization.GetString("cmdFilterMain", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Filter Words
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlFilterWords Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdFilterWords.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.FilterWord)
					RenderLinkButton(wr, url, Localization.GetString("cmdFilterWords", Me.LocalResourceFile), "Forum_Link")
				End If
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Rating
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlRating Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdRating.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.Rating)
					RenderLinkButton(wr, url, Localization.GetString("cmdRating", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Ranking
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlRanking Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdRanking.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.Ranking)
					RenderLinkButton(wr, url, Localization.GetString("cmdRanking", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Pop Stat
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlPopStatus Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdPopStatus.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.PopStatus)
					RenderLinkButton(wr, url, Localization.GetString("cmdPopStatus", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Close Section
				wr.RenderEndTag() ' </table>
				wr.RenderEndTag() ' </div>

				' Fifth Menu Section, Email
				GenerateMenuSection(wr, Localization.GetString("EmailSection", Me.LocalResourceFile), IsExpanded(5, ExpandSection), 100)
				wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
				wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>

				' Email Settings
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlEmailSettings Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdEmailSettings.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.EmailSettings)
					RenderLinkButton(wr, url, Localization.GetString("cmdEmailSettings", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Email Template 
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlEmailTemplate Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				url = Utilities.Links.ACPEmailTemplateManagerLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID)
				RenderLinkButton(wr, url, Localization.GetString("cmdEmailTemplate", Me.LocalResourceFile), "Forum_Link")
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				If objConfig.EnableEmailQueueTask Then
					'Email Queue
					If Users.UserController.GetCurrentUserInfo.IsSuperUser Then
						wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
						wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
						wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

						If ControlToLoad = ctlEmailQueue Then
							wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
							wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
							wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
							wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
							wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
							wr.RenderEndTag() ' </img>
						End If
						wr.RenderEndTag() ' </td>

						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
						wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
						If EnableAjax Then
							cmdEmailQueue.RenderControl(wr)
						Else
							url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.EmailQueue)
							RenderLinkButton(wr, url, Localization.GetString("cmdEmailQueue", Me.LocalResourceFile), "Forum_Link")
						End If

						wr.RenderEndTag() ' </td>
						wr.RenderEndTag() ' </tr>
					End If

					' Queue Task Detail
					wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

					If ControlToLoad = ctlEmailQueueTaskDetail Then
						wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
						wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
						wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
						wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
						wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
						wr.RenderEndTag() ' </img>
					End If
					wr.RenderEndTag() ' </td>

					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
					If EnableAjax Then
						cmdEmailQueueTaskDetail.RenderControl(wr)
					Else
						url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.EmailQueueTaskDetail)
						RenderLinkButton(wr, url, Localization.GetString("cmdEmailQueueTaskDetail", Me.LocalResourceFile), "Forum_Link")
					End If

					wr.RenderEndTag() ' </td>
					wr.RenderEndTag() ' </tr>
				End If

				' Email Subscribers
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlEmailSubscribers Then
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
					wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
					wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
					wr.RenderEndTag() ' </img>
				End If
				wr.RenderEndTag() ' </td>

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				If EnableAjax Then
					cmdEmailSubscribers.RenderControl(wr)
				Else
					url = Utilities.Links.ACPControlLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, AdminAjaxControl.EmailSubscribers)
					RenderLinkButton(wr, url, Localization.GetString("cmdEmailSubscribers", Me.LocalResourceFile), "Forum_Link")
				End If

				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Close Section
				wr.RenderEndTag() ' </table>
				wr.RenderEndTag() ' </div>

				'Section end, close the div.
				wr.RenderEndTag() ' </div>
			Catch ex As Exception
				'Do nothing, main control will handle errors
			End Try
		End Sub

		''' <summary>
		''' This event fires when one of the navigation link buttons are clicked. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub PageButton_Click(ByVal sender As Object, ByVal e As EventArgs)
			If TypeOf sender Is LinkButton Then
				Dim CurrentPage As AdminAjaxControl
				If Not sender Is Nothing Then
					CurrentPage = CType(CType(sender, LinkButton).CommandArgument, AdminAjaxControl)
				End If
				Me.ControlToLoad = IdentifyControl(CurrentPage, sender, e)
				RaiseEvent MenuClicked(sender, e)
			End If
		End Sub

		''' <summary>
		''' This is used to create our controls that will be placed in the menu. 
		''' </summary>
		''' <remarks>
		''' </remarks>
		Protected Overrides Sub CreateChildControls()
			Controls.Clear()

			Me.cmdOverview = New LinkButton
			With cmdOverview
				.CssClass = "Forum_Link"
				.ID = "cmdOverview"
				.Text = Localization.GetString("cmdOverview", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.Main)).ToString()
				.CausesValidation = False
				AddHandler cmdOverview.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdOverview)

			Me.cmdGeneral = New LinkButton
			With cmdGeneral
				.CssClass = "Forum_Link"
				.ID = "cmdGeneral"
				.Text = Localization.GetString("cmdGeneral", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.General)).ToString()
				.CausesValidation = False
				AddHandler cmdGeneral.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdGeneral)

			Me.cmdCommunity = New LinkButton
			With cmdCommunity
				.CssClass = "Forum_Link"
				.ID = "cmdCommunity"
				.Text = Localization.GetString("cmdCommunity", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.Community)).ToString()
				.CausesValidation = False
				AddHandler cmdCommunity.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdCommunity)

			Me.cmdAttachment = New LinkButton
			With cmdAttachment
				.CssClass = "Forum_Link"
				.ID = "cmdAttachment"
				.Text = Localization.GetString("cmdAttachment", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.Attachment)).ToString()
				.CausesValidation = False
				AddHandler cmdAttachment.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdAttachment)

			Me.cmdRSS = New LinkButton
			With cmdRSS
				.CssClass = "Forum_Link"
				.ID = "cmdRSS"
				.Text = Localization.GetString("cmdRSS", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.RSS)).ToString()
				.CausesValidation = False
				AddHandler cmdRSS.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdRSS)

			Me.cmdSEO = New LinkButton
			With cmdSEO
				.CssClass = "Forum_Link"
				.ID = "cmdSEO"
				.Text = Localization.GetString("cmdSEO", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.SEO)).ToString()
				.CausesValidation = False
				AddHandler cmdSEO.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdSEO)

			'Me.cmdForumManager = New LinkButton
			'With cmdForumManager
			'	.CssClass = "Forum_Link"
			'	.ID = "cmdForumManager"
			'	.Text = Localization.GetString("cmdForumManager", Me.LocalResourceFile)
			'	.CommandArgument = (CInt(AdminAjaxControl.ForumManager)).ToString()
			'   .CausesValidation = False
			'	AddHandler cmdForumManager.Click, AddressOf PageButton_Click
			'End With
			'Controls.Add(cmdForumManager)

			Me.cmdUser = New LinkButton
			With cmdUser
				.CssClass = "Forum_Link"
				.ID = "cmdUser"
				.Text = Localization.GetString("cmdUser", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.Users)).ToString()
				.CausesValidation = False
				AddHandler cmdUser.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdUser)

			Me.cmdAvatar = New LinkButton
			With cmdAvatar
				.CssClass = "Forum_Link"
				.ID = "cmdAvatar"
				.Text = Localization.GetString("cmdAvatar", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.Avatar)).ToString()
				.CausesValidation = False
				AddHandler cmdAvatar.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdAvatar)

			Me.cmdUserSettings = New LinkButton
			With cmdUserSettings
				.CssClass = "Forum_Link"
				.ID = "cmdUserSettings"
				.Text = Localization.GetString("cmdUserSettings", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.UserSettings)).ToString()
				.CausesValidation = False
				AddHandler cmdUserSettings.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdUserSettings)

			Me.cmdUserInterface = New LinkButton
			With cmdUserInterface
				.CssClass = "Forum_Link"
				.ID = "cmdUserInterface"
				.Text = Localization.GetString("cmdUserInterface", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.UserInterface)).ToString()
				.CausesValidation = False
				AddHandler cmdUserInterface.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdUserInterface)

			Me.cmdFilterMain = New LinkButton
			With cmdFilterMain
				.CssClass = "Forum_Link"
				.ID = "cmdFilterMain"
				.Text = Localization.GetString("cmdFilterMain", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.FilterMain)).ToString()
				.CausesValidation = False
				AddHandler cmdFilterMain.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdFilterMain)

			Me.cmdFilterWords = New LinkButton
			With cmdFilterWords
				.CssClass = "Forum_Link"
				.ID = "cmdFilterWords"
				.Text = Localization.GetString("cmdFilterWords", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.FilterWord)).ToString()
				.CausesValidation = False
				AddHandler cmdFilterWords.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdFilterWords)

			Me.cmdRanking = New LinkButton
			With cmdRanking
				.CssClass = "Forum_Link"
				.ID = "cmdRanking"
				.Text = Localization.GetString("cmdRanking", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.Ranking)).ToString()
				.CausesValidation = False
				AddHandler cmdRanking.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdRanking)

			Me.cmdRating = New LinkButton
			With cmdRating
				.CssClass = "Forum_Link"
				.ID = "cmdRating"
				.Text = Localization.GetString("cmdRating", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.Rating)).ToString()
				.CausesValidation = False
				AddHandler cmdRating.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdRating)

			Me.cmdPopStatus = New LinkButton
			With cmdPopStatus
				.CssClass = "Forum_Link"
				.ID = "cmdPopStatus"
				.Text = Localization.GetString("cmdPopStatus", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.PopStatus)).ToString()
				.CausesValidation = False
				AddHandler cmdPopStatus.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdPopStatus)

			Me.cmdEmailSettings = New LinkButton
			With cmdEmailSettings
				.CssClass = "Forum_Link"
				.ID = "cmdEmailSettings"
				.Text = Localization.GetString("cmdEmailSettings", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.EmailSettings)).ToString()
				.CausesValidation = False
				AddHandler cmdEmailSettings.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdEmailSettings)

			'Me.cmdEmailTemplate = New LinkButton
			'With cmdEmailTemplate
			'	.CssClass = "Forum_Link"
			'	.ID = "cmdEmailTemplate"
			'	.Text = Localization.GetString("cmdEmailTemplate", Me.LocalResourceFile)
			'	.CommandArgument = (CInt(AdminAjaxControl.EmailTemplate)).ToString()
			'   .CausesValidation = False
			'	AddHandler cmdEmailTemplate.Click, AddressOf PageButton_Click
			'End With
			'Controls.Add(cmdEmailTemplate)

			Me.cmdEmailQueue = New LinkButton
			With cmdEmailQueue
				.CssClass = "Forum_Link"
				.ID = "cmdEmailQueue"
				.Text = Localization.GetString("cmdEmailQueue", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.EmailQueue)).ToString()
				.CausesValidation = False
				AddHandler cmdEmailQueue.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdEmailQueue)

			Me.cmdEmailQueueTaskDetail = New LinkButton
			With cmdEmailQueueTaskDetail
				.CssClass = "Forum_Link"
				.ID = "cmdEmailQueueTaskDetail"
				.Text = Localization.GetString("cmdEmailQueueTaskDetail", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.EmailQueueTaskDetail)).ToString()
				.CausesValidation = False
				AddHandler cmdEmailQueueTaskDetail.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdEmailQueueTaskDetail)

			Me.cmdEmailSubscribers = New LinkButton
			With cmdEmailSubscribers
				.CssClass = "Forum_Link"
				.ID = "cmdEmailSubscribers"
				.Text = Localization.GetString("cmdEmailSubscribers", Me.LocalResourceFile)
				.CommandArgument = (CInt(AdminAjaxControl.EmailSubscribers)).ToString()
				.CausesValidation = False
				AddHandler cmdEmailSubscribers.Click, AddressOf PageButton_Click
			End With
			Controls.Add(cmdEmailSubscribers)
		End Sub

#End Region

	End Class

End Namespace