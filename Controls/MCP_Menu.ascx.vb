'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2009
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

Namespace DotNetNuke.Modules.Forum.MCP

	''' <summary>
	''' This is the Moderator Control Panel (MCP). It consists of a menu structure.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[skeel]	11/11/2008	Created
	''' </history>
	Partial Public Class Menu
		Inherits System.Web.UI.UserControl

#Region "Private Members"

		Dim _LocalResourceFile As String = String.Empty
		Dim _DefaultControlToLoad As String = "MCP_Main.ascx"
		Dim _objConfig As Forum.Config
		Dim _PortalID As Integer
		Dim _ModuleID As Integer
		Dim SectionNumber As Integer = 1
		Dim ctlOverview As String = "MCP_Main.ascx"
		Dim ctlModQueue As String = "MCP_ModQueue.ascx"
		Dim ctlReportedPosts As String = "MCP_ReportedPost.ascx"
		Dim ctlBannedUsers As String = "MCP_BannedUsers.ascx"
		Dim ctlReportedUsers As String = "MCP_ReportedUsers.ascx"
		'Dim ctlUntrustedUsers As String = "MCP_UntrustedUsers.ascx"

		Dim cmdOverview As LinkButton
		Dim cmdModQueue As LinkButton
		Dim cmdReportedPosts As LinkButton
		Dim cmdBannedUsers As LinkButton
		Dim cmdReportedUsers As LinkButton
		'Dim cmdUntrustedUsers As LinkButton

		''' <summary>
		''' The various sections of the menu that can be expanded/collapsed.
		''' </summary>
		''' <remarks></remarks>
		Private Enum ExpandMenuSection
			ControlPanel = 1
			Users = 2
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
					fileRoot = Me.TemplateSourceDirectory & "/" & Localization.LocalResourceDirectory & "/MCP_Menu.ascx"
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
		''' The forum configuration for the module this control is loaded into. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property objConfig() As Forum.Config
			Get
				Return _objConfig
			End Get
			Set(ByVal value As Forum.Config)
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
		''' This is the control we want to load in UCP.ascx via Ajax.
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
		Private Function IdentifyControl(ByVal CurrentPage As ModeratorAjaxControl, ByVal sender As Object, ByVal e As EventArgs) As String
			Select Case CurrentPage
				Case ModeratorAjaxControl.ModQueue
					ControlToLoad = ctlModQueue
				Case ModeratorAjaxControl.ReportedPosts
					ControlToLoad = ctlReportedPosts
				Case ModeratorAjaxControl.BannedUsers
					ControlToLoad = ctlBannedUsers
				Case ModeratorAjaxControl.ReportedUsers
					ControlToLoad = ctlReportedUsers
					'Case ModeratorAjaxControl.UntrustedUsers
					'	ControlToLoad = ctlUntrustedUsers
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
				Dim ExpandSection As Integer = -1
				Dim url As String = String.Empty

				Select Case ControlToLoad
					Case ctlOverview
						ExpandSection = ExpandMenuSection.ControlPanel
					Case ctlModQueue
						ExpandSection = ExpandMenuSection.ControlPanel
					Case ctlReportedPosts
						ExpandSection = ExpandMenuSection.ControlPanel
					Case ctlBannedUsers
						ExpandSection = ExpandMenuSection.Users
					Case ctlReportedUsers
						ExpandSection = ExpandMenuSection.Users
						'Case ctlUntrustedUsers
						'	ExpandSection = ExpandMenuSection.Users
				End Select

				'Lets build the menu structure
				Dim imgSelectedURL As String = objConfig.GetThemeImageURL("curpage.") & objConfig.ImageExtension
				Dim imgSelectedToolTip As String = Localization.GetString("Selected", Me.LocalResourceFile)

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Menu")
				wr.RenderBeginTag(HtmlTextWriterTag.Div) ' <div>	

				'First Menu section, contains overview, tracking, bookmarks and a link to the forum home
				GenerateMenuSection(wr, Localization.GetString("GeneralSettings", Me.LocalResourceFile), IsExpanded(1, ExpandSection), 100)
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
				cmdOverview.RenderControl(wr)
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' Moderation Queue
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlModQueue Then
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
				cmdModQueue.RenderControl(wr)
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' Reported Posts
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlReportedPosts Then
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
				cmdReportedPosts.RenderControl(wr)
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'Close section
				wr.RenderEndTag() ' </table>
				wr.RenderEndTag() ' </div>

				'Second Menu section, contains user related moderation items
				GenerateMenuSection(wr, Localization.GetString("UserSettings", Me.LocalResourceFile), IsExpanded(2, ExpandSection), 40)
				wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
				wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
				wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>

				' Banned Users
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlBannedUsers Then
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
				cmdBannedUsers.RenderControl(wr)
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				' Reported Users
				wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				If ControlToLoad = ctlReportedUsers Then
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
				cmdReportedUsers.RenderControl(wr)
				wr.RenderEndTag() ' </td>
				wr.RenderEndTag() ' </tr>

				'' Untrusted Users
				'wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
				'wr.AddAttribute(HtmlTextWriterAttribute.Width, "15")
				'wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

				'If ControlToLoad = ctlUntrustedUsers Then
				'	wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
				'	wr.AddAttribute(HtmlTextWriterAttribute.Src, imgSelectedURL)
				'	wr.AddAttribute(HtmlTextWriterAttribute.Alt, imgSelectedToolTip)
				'	wr.AddAttribute(HtmlTextWriterAttribute.Title, imgSelectedToolTip)
				'	wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img> 
				'	wr.RenderEndTag() ' </img>
				'End If
				'wr.RenderEndTag() ' </td>

				'wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_UCP_Item")
				'wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 
				'cmdUntrustedUsers.RenderControl(wr)
				'wr.RenderEndTag() ' </td>
				'wr.RenderEndTag() ' </tr>

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
				Dim CurrentPage As ModeratorAjaxControl
				If Not sender Is Nothing Then
					CurrentPage = CType(CType(sender, LinkButton).CommandArgument, ModeratorAjaxControl)
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
		''' <history>
		''' 	[cpaterra]	11/29/2008	Created
		''' </history>
		Protected Overrides Sub CreateChildControls()
			Controls.Clear()

			Me.cmdOverview = New LinkButton
			With cmdOverview
				.CssClass = "Forum_Link"
				.ID = "cmdOverview"
				.Text = Localization.GetString("cmdOverview", Me.LocalResourceFile)
				.CommandArgument = (CInt(ModeratorAjaxControl.Main)).ToString()
				.CausesValidation = False
				AddHandler cmdOverview.Click, AddressOf PageButton_Click
				.EnableViewState = False
			End With
			Controls.Add(cmdOverview)

			Me.cmdModQueue = New LinkButton
			With cmdModQueue
				.CssClass = "Forum_Link"
				.ID = "cmdModQueue"
				.Text = Localization.GetString("cmdModQueue", Me.LocalResourceFile)
				.CommandArgument = (CInt(ModeratorAjaxControl.ModQueue)).ToString()
				.CausesValidation = False
				AddHandler cmdModQueue.Click, AddressOf PageButton_Click
				.EnableViewState = False
			End With
			Controls.Add(cmdModQueue)

			Me.cmdReportedPosts = New LinkButton
			With cmdReportedPosts
				.CssClass = "Forum_Link"
				.ID = "cmdReportedPosts"
				.Text = Localization.GetString("cmdReportedPosts", Me.LocalResourceFile)
				.CommandArgument = (CInt(ModeratorAjaxControl.ReportedPosts)).ToString()
				.CausesValidation = False
				AddHandler cmdReportedPosts.Click, AddressOf PageButton_Click
				.EnableViewState = False
			End With
			Controls.Add(cmdReportedPosts)

			Me.cmdBannedUsers = New LinkButton
			With cmdBannedUsers
				.CssClass = "Forum_Link"
				.ID = "cmdBannedUsers"
				.Text = Localization.GetString("cmdBannedUsers", Me.LocalResourceFile)
				.CommandArgument = (CInt(ModeratorAjaxControl.BannedUsers)).ToString()
				.CausesValidation = False
				AddHandler cmdBannedUsers.Click, AddressOf PageButton_Click
				.EnableViewState = False
			End With
			Controls.Add(cmdBannedUsers)

			Me.cmdReportedUsers = New LinkButton
			With cmdReportedUsers
				.CssClass = "Forum_Link"
				.ID = "cmdReportedUsers"
				.Text = Localization.GetString("cmdReportedUsers", Me.LocalResourceFile)
				.CommandArgument = (CInt(ModeratorAjaxControl.ReportedUsers)).ToString()
				.CausesValidation = False
				AddHandler cmdReportedUsers.Click, AddressOf PageButton_Click
				.EnableViewState = False
			End With
			Controls.Add(cmdReportedUsers)

			'Me.cmdUntrustedUsers = New LinkButton
			'With cmdUntrustedUsers
			'	.CssClass = "Forum_Link"
			'	.ID = "cmdUntrustedUsers"
			'	.Text = Localization.GetString("cmdUntrustedUsers", Me.LocalResourceFile)
			'	.CommandArgument = (CInt(ModeratorAjaxControl.UntrustedUsers)).ToString()
			'	.CausesValidation = False
			'	AddHandler cmdUntrustedUsers.Click, AddressOf PageButton_Click
			'	.EnableViewState = False
			'End With
			'Controls.Add(cmdUntrustedUsers)
		End Sub

#End Region

	End Class

End Namespace