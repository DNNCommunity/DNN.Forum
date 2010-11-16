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

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' This is the control that loads all admin control panel sub-controls and the acp menu. (ie. it is the acp host)
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class ACPLoader
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

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
		Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Init
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
			End If
		End Sub

		''' <summary>
		''' Page Load of the control.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
			Try
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				If Not (Security.IsForumAdmin = True) Then
					' they don't belong here
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
				ForumUtils.LoadCssFile(DefaultPage, objConfig)

				If Not Page.IsPostBack Then
					' CP - Check the URL here to see if we should load a specific control (ie. view = x)
					If Not Request.QueryString("view") Is Nothing Then
						Dim strTempControl As String
						strTempControl = Request.QueryString("view").ToString.ToLower()

						Select Case strTempControl
							Case CStr(AdminAjaxControl.General)
								ACPmenu.ControlToLoad = Constants.ctlGeneral
							Case CStr(AdminAjaxControl.Community)
								ACPmenu.ControlToLoad = Constants.ctlCommunity
							Case CStr(AdminAjaxControl.Attachment)
								ACPmenu.ControlToLoad = Constants.ctlAttachment
							Case CStr(AdminAjaxControl.RSS)
								ACPmenu.ControlToLoad = Constants.ctlRSS
							Case CStr(AdminAjaxControl.SEO)
								ACPmenu.ControlToLoad = Constants.ctlSEO
								' Forum Manage
							Case CStr(AdminAjaxControl.Users)
								ACPmenu.ControlToLoad = Constants.ctlUser
							Case CStr(AdminAjaxControl.Avatar)
								ACPmenu.ControlToLoad = Constants.ctlAdminAvatar
							Case CStr(AdminAjaxControl.UserSettings)
								ACPmenu.ControlToLoad = Constants.ctlUserSettings
							Case CStr(AdminAjaxControl.UserInterface)
								ACPmenu.ControlToLoad = Constants.ctlUserInterface
							Case CStr(AdminAjaxControl.FilterMain)
								ACPmenu.ControlToLoad = Constants.ctlFilterMain
							Case CStr(AdminAjaxControl.FilterWord)
								ACPmenu.ControlToLoad = Constants.ctlFilterWords
							Case CStr(AdminAjaxControl.Rating)
								ACPmenu.ControlToLoad = Constants.ctlRating
							Case CStr(AdminAjaxControl.Ranking)
								ACPmenu.ControlToLoad = Constants.ctlRanking
							Case CStr(AdminAjaxControl.PopStatus)
								ACPmenu.ControlToLoad = Constants.ctlPopStatus
							Case CStr(AdminAjaxControl.EmailSettings)
								ACPmenu.ControlToLoad = Constants.ctlEmailSettings
								' Email Template
							Case CStr(AdminAjaxControl.EmailQueue)
								ACPmenu.ControlToLoad = Constants.ctlEmailQueue
							Case CStr(AdminAjaxControl.EmailQueueTaskDetail)
								ACPmenu.ControlToLoad = Constants.ctlEmailQueueTaskDetail
							Case CStr(AdminAjaxControl.EmailSubscribers)
								ACPmenu.ControlToLoad = Constants.ctlEmailSubscribers
							Case Else
								ACPmenu.ControlToLoad = Constants.ctlACPOverview
						End Select
					End If
				End If

				ACPmenu.PortalID = PortalId
				ACPmenu.objConfig = objConfig
				ACPmenu.ModuleID = ModuleId
				ACPmenu.EnableAjax = True
				' Load the appropriate Control
				_CtlToLoad = ACPmenu.ControlToLoad
				LoadForumControl(_CtlToLoad)
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' This public event is fired off from ACP_Menu but is handled here in order to load proper user control based on the clicked menu item. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Reset viewstate, another control was requested to be loaded.</remarks>
		Protected Sub ACPmenu_MenuClicked(ByVal sender As Object, ByVal e As EventArgs) Handles ACPmenu.MenuClicked
			ViewState("CtlToLoad") = String.Empty
			Dim objControl As Entities.Modules.PortalModuleBase
			objControl = LoadForumControl(ACPmenu.ControlToLoad)
		End Sub

		''' <summary>
		''' This method is used to load the proper user control. 
		''' It is also responsible for finding the control and firing off the interface all controls loaded by Forum_UserSettings via Ajax use.
		''' </summary>
		''' <param name="control">The ascx control we are attempting to load.</param>
		''' <returns>An instance of the ascx control we loaded in the module.</returns>
		''' <remarks>We need viewstate to load the same subcontrol at postback.</remarks>
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