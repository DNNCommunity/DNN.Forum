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

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This is the SEO section for the module. Items related to SEO that are configured per module are set here. 
	''' </summary>
	''' <remarks></remarks>
	Partial Public Class SEO
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			chkNoFollowWeb.Checked = objConfig.NoFollowWeb
			chkOverrideTitle.Checked = objConfig.OverrideTitle
			chkOverrideDescription.Checked = objConfig.OverrideDescription
			chkOverrideKeyWords.Checked = objConfig.OverrideKeyWords
			chkNoFollowLatestThreads.Checked = objConfig.NoFollowLatestThreads
			txtSitemapPriority.Text = objConfig.SitemapPriority.ToString()
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Updates the module's configuration (module settings)
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Saves the module settings shown in this view.
		''' </remarks>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				Dim ctlModule As New Entities.Modules.ModuleController
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_NO_FOLLOW_WEBSITE_LINK, chkNoFollowWeb.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_OVERRIDE_PAGE_TITLE, chkOverrideTitle.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_OVERRIDE_PAGE_DESCRIPTION, chkOverrideDescription.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_OVERRIDE_PAGE_KEYWORDS, chkOverrideKeyWords.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_NO_FOLLOW_LATEST_THREAD_LINKS, chkNoFollowLatestThreads.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.SITEMAP_PRIORITY, txtSitemapPriority.Text)

				Configuration.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				Dim s As String = exc.ToString
				s = s & " "
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

	End Class

End Namespace