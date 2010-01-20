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
	''' This is the control that loads all moderator control panel sub-controls and mcp menu. (ie. it is the mcp host)
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class MCPLoader
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Dim ctlOverview As String = "MCP_Main.ascx"
		Dim ctlModQueue As String = "MCP_ModQueue.ascx"
		Dim ctlReportedPosts As String = "MCP_ReportedPost.ascx"
		Dim ctlBannedUsers As String = "MCP_BannedUsers.ascx"
		Dim ctlReportedUsers As String = "MCP_ReportedUsers.ascx"
		'Dim ctlUntrustedUsers As String = "MCP_UntrustedUsers.ascx"
		Dim _CtlToLoad As String

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
		''' Determines if we can use Ajax in this page before anything is rendered. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, False)
			End If
		End Sub

		''' <summary>
		''' Page Load of the control.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				If Not Security.IsModerator Then
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Not Page.IsPostBack Then
					litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

					' CP - Check the URL here to see if we should load a specific control (ie. view = x)
					If Not Request.QueryString("view") Is Nothing Then
						Dim strTempControl As String
						strTempControl = Request.QueryString("view").ToString.ToLower()

						Select Case strTempControl
							Case CStr(ModeratorAjaxControl.ModQueue)
								MCPmenu.ControlToLoad = ctlModQueue
							Case CStr(ModeratorAjaxControl.ReportedPosts)
								MCPmenu.ControlToLoad = ctlReportedPosts
							Case CStr(ModeratorAjaxControl.BannedUsers)
								MCPmenu.ControlToLoad = ctlBannedUsers
							Case CStr(ModeratorAjaxControl.ReportedUsers)
								MCPmenu.ControlToLoad = ctlReportedUsers
								'Case CStr(ModeratorAjaxControl.UntrustedUsers)
								'	MCPmenu.ControlToLoad = ctlUntrustedUsers
							Case Else
								MCPmenu.ControlToLoad = ctlOverview
						End Select
					End If

					' Register scripts
					Utilities.ForumUtils.RegisterPageScripts(Page, objConfig)
				End If

				MCPmenu.PortalID = PortalId
				MCPmenu.objConfig = objConfig
				MCPmenu.ModuleID = ModuleId
				' Load the appropriate Control
				_CtlToLoad = MCPmenu.ControlToLoad
				LoadForumControl(_CtlToLoad)
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' This public event is fired off from MCP_Menu but is handled here in order to load proper user control based on the clicked menu item. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>[skeel] reset viewstate, another control was requested loaded</remarks>
		Protected Sub MCPmenu_MenuClicked(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MCPmenu.MenuClicked
			ViewState("CtlToLoad") = String.Empty
			Dim objControl As Entities.Modules.PortalModuleBase
			objControl = LoadForumControl(MCPmenu.ControlToLoad)
		End Sub

		''' <summary>
		''' This method is used to load the proper user control. 
		''' It is also responsible for finding the control and firing off the interface all controls loaded by Forum_UserSettings via ajax use.
		''' </summary>
		''' <param name="control"></param>
		''' <returns></returns>
		''' <remarks>[skeel] we need viewstate to load the same subcontrol at postback</remarks>
		Protected Function LoadForumControl(ByVal control As String) As Entities.Modules.PortalModuleBase
			Dim objControl As Entities.Modules.PortalModuleBase = Nothing
			Dim ctlDirectory As String = objConfig.SourceDirectory + "/Controls/"

			objControl = TryCast(LoadControl(ctlDirectory & control), Entities.Modules.PortalModuleBase)
			If objControl IsNot Nothing Then
				phUserControl.Controls.Clear()
				objControl.ModuleConfiguration = ModuleConfiguration
				objControl.ID = System.IO.Path.GetFileNameWithoutExtension(ctlDirectory & control)
				phUserControl.Controls.Add(objControl)

				If CStr(ViewState("CtlToLoad")) <> control Then
					Dim ctlLoaded As Utilities.AjaxLoader.IPageLoad
					ctlLoaded = CType(phUserControl.FindControl(objControl.ID), Utilities.AjaxLoader.IPageLoad)
					ctlLoaded.LoadInitialView()
					ViewState("CtlToLoad") = control
				End If
			End If
			Return objControl
		End Function

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

	End Class

End Namespace