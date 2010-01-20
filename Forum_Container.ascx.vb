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
	''' Loads the proper control to display to the end user based on various parameters and settings.
	''' </summary>
	''' <remarks>This is a 'Dispatch' control, only the UI folder classes are rendered here.</remarks>
	Public MustInherit Class Container
		Inherits ForumModuleBase
		Implements DotNetNuke.Entities.Modules.IActionable

#Region "Private Members"

		Private _GroupID As Integer = Null.NullInteger
		Private _ForumID As Integer = Null.NullInteger
		Private _ThreadID As Integer = Null.NullInteger
		Private _PostID As Integer = Null.NullInteger

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
		''' Since this is a dispatch page, we need to set base properties and 
		''' then load the proper view (Based on scope variable). This currently 
		''' loads all non ascx views used throughout this module. It is important 
		''' the base object be tied to the current moduleid, tabid. (Navigation)
		''' </summary>
		''' <param name="sender">The Object</param>
		''' <param name="e">The event arguement.</param>
		''' <remarks>The event arguement and the object are not used. ObjectID needs to be reconsidered.
		''' </remarks>
		''' <history>
		''' 	[Administrator]	9/9/2006	Created
		''' </history>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			'' load the css file
			'litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

			'If DotNetNuke.Framework.AJAX.IsInstalled Then
			'	DotNetNuke.Framework.AJAX.RegisterScriptManager()
			'	'DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, False)
			'End If
			''
			If Not (Request.QueryString("groupid") Is Nothing) Then
				_GroupID = Int32.Parse(Request.QueryString("groupid"))
			End If

			If Not (Request.QueryString("forumid") Is Nothing) Then
				_ForumID = Int32.Parse(Request.QueryString("forumid"))
			End If

			If Not (Request.QueryString("threadid") Is Nothing) Then
				_ThreadID = Int32.Parse(Request.QueryString("threadid"))
			End If

			If Not (Request.QueryString("postid") Is Nothing) Then
				_PostID = Int32.Parse(Request.QueryString("postid"))
			End If

			With DNNForum
				.PortalID = PortalSettings.PortalId
				.TabID = PortalSettings.ActiveTab.TabID
				.ModuleID = ModuleId
				.objConfig = objConfig
				.BasePage = CType(Me.Page, DotNetNuke.Framework.CDefault)
				.PortalName = PortalSettings.PortalName
				.LocalResourceFile = TemplateSourceDirectory & "/" & Localization.LocalResourceDirectory & "/" & Me.ID
				.TabModuleSettings = Settings

				Select Case DNNForum.ViewType
					Case ForumScope.Groups
						.GenericObjectID = ModuleId
					Case ForumScope.Forums
						.GenericObjectID = _GroupID
					Case ForumScope.Threads
						.GenericObjectID = _ForumID
					Case ForumScope.Posts
						.GenericObjectID = _ThreadID
					Case ForumScope.ThreadSearch
						.GenericObjectID = _ForumID
					Case ForumScope.Unread
						.GenericObjectID = ModuleId
				End Select
			End With
		End Sub

		''' <summary>
		''' Determines if we need to redirect to this page again to change the scope, as well as sets the module actions 
		''' </summary>
		''' <param name="sender">The object.</param>
		''' <param name="e">The event arguement being passed in.</param>
		''' <remarks>We have to load the javascript files on every load of this control.</remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				' load the css file
				litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

				' Redirect to user's default forum if user access forum via menu
				Dim objModules As Entities.Modules.ModuleController = New Entities.Modules.ModuleController

				If (Not CType(Settings("defaultforumid"), String) Is Nothing) AndAlso (DNNForum.ViewType = ForumScope.Groups) AndAlso (Request.UrlReferrer Is Nothing) Then
					If Not CType(Settings("defaultforumid"), Integer) = 0 Then
						Response.Redirect(Utilities.Links.ContainerViewForumLink(TabId, CType(Settings("defaultforumid"), Integer), False), False)
					End If
				End If

				'Set the Navigator Actions Collection
				For Each action As Entities.Modules.Actions.ModuleAction In Actions
					If action.CommandName = Entities.Modules.Actions.ModuleActionType.ContentOptions Then
						If DotNetNuke.Security.Permissions.ModulePermissionController.HasModuleAccess(action.Secure, "Edit", ModuleConfiguration) Then
							DNNForum.NavigatorActions.Add(action)
						End If
					End If
				Next

				' Register the javascript files used by this module. 
				Utilities.ForumUtils.RegisterPageScripts(Me.Page, objConfig)
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try

		End Sub

#End Region

	End Class

End Namespace