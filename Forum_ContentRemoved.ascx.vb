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
	''' This is a page to handle if a thread was completely deleted.
	''' It presents the user w/ an error message and a link back home
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class ContentRemoved
		Inherits ForumModuleBase
		Implements DotNetNuke.Entities.Modules.IActionable

#Region "Optional Interfaces"

		''' <summary>
		''' Gets a list of module actions available to the user to provide it to DNN core.
		''' </summary>
		''' <value></value>
		''' <returns>The collection of module actions available to the user</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ModuleActions() As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
			Get
				Return Utilities.ForumUtils.PerUserModuleActions(objConfig, Me)
			End Get
		End Property

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Load the page/settings
		''' </summary>
		''' <param name="sender">System.Object</param>
		''' <param name="e">System.EventArgs)</param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			If Not Page.IsPostBack Then
				Dim DefaultPage As CDefault = DirectCast(Page, CDefault)
				ForumUtils.LoadCssFile(DefaultPage, objConfig)

				' Store the referrer for returning to where the user came from
				If Not Request.UrlReferrer Is Nothing Then
					ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
				End If
				'Spacer image
				imgHeadSpacer.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				imgHeadSpacer2.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				imgFootSpacer.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
				imgFootSpacer2.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
			End If
		End Sub

		''' <summary>
		''' Takes the user back where they came from
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			If Not ViewState("UrlReferrer") Is Nothing Then
				Response.Redirect(CType(ViewState("UrlReferrer"), String), False)
			Else
				Response.Redirect(Utilities.Links.ContainerForumHome(TabId), False)
			End If
		End Sub

#End Region

	End Class

End Namespace