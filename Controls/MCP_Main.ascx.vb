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

Namespace DotNetNuke.Modules.Forum.MCP

	''' <summary>
	''' This is the MCP section that shows an overview about the mcp.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class Main
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks>So far this is a static page</remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			BuildOverview()
		End Sub

#End Region

#Region "Private Methods"

		Private Sub BuildOverview()
			Dim cntPostReport As New PostReportedController
			Dim arrPosts As New List(Of PostInfo)
			Dim ctlModerate As New PostModerationController
			Dim colForums As List(Of ForumInfo)
			Dim PostsToModerate As Integer = 0
			Dim ReportedPosts As Integer = 0

			arrPosts = cntPostReport.GetReportedPosts(PortalId, 0, 10)
			colForums = ctlModerate.ModerateForumGetByModeratorThreads(CurrentForumUser.UserID, ModuleId, PortalId)

			If arrPosts.Count > 0 Then
				ReportedPosts = arrPosts(0).TotalRecords
			End If

			If colForums.Count > 0 Then
				PostsToModerate = colForums(0).TotalRecords
			End If

			lblPostQueue.Text = Localization.GetString("lblPostQueue", LocalResourceFile) + PostsToModerate.ToString()
			lblReportedPosts.Text = Localization.GetString("lblReportedPosts", LocalResourceFile) + ReportedPosts.ToString()
		End Sub

#End Region

	End Class

End Namespace