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

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This is the General Settings area from Forum Administration.
	''' Set basic configuration items for this forum module. (mod settings)
	''' </summary>
	''' <remarks>The majority of how the module operates is set here.
	''' </remarks>
	Partial Public Class UI
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This interface is used to replace a If Page.IsPostBack typically used in page load. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			txtTheardsPerPage.Text = objConfig.ThreadsPerPage.ToString()
			txtPostsPerPage.Text = objConfig.PostsPerPage.ToString()
			txtThreadPageCount.Text = objConfig.PostPagesCount.ToString()
			txtImageExtension.Text = objConfig.ImageExtension
			txtMaxPostImageWidth.Text = objConfig.MaxPostImageWidth.ToString
			chkEnableScripts.Checked = objConfig.LoadScripts
			chkEnableIconBarAsImages.Checked = objConfig.IconBarAsImages
			chkDisplayPosterRegion.Checked = objConfig.DisplayPosterRegion
			chkEnableQuickReply.Checked = objConfig.EnableQuickReply

			Dim themesPath As String = System.IO.Path.Combine(Server.MapPath(objConfig.SourceDirectory), "Themes")
			Dim forumThemePath As String = System.IO.Path.Combine(themesPath, objConfig.ForumTheme)

			If System.IO.Directory.Exists(forumThemePath) Then
				ddlSkins.Items.FindByValue(objConfig.ForumTheme).Selected = True
			Else
				ddlSkins.Items(0).Selected = True
			End If
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Page Load is run in this control, which is loaded via Ajax.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			BindThemes()
			BindPosterLocation()
		End Sub

		''' <summary>
		''' Updates the module's configuration (module settings)
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Saves the module settings shown in this view.
		''' </remarks>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				' Update settings in the database
				Dim ctlModule As New Entities.Modules.ModuleController
				ctlModule.UpdateModuleSetting(ModuleId, "ThreadsPerPage", txtTheardsPerPage.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "PostsPerPage", txtPostsPerPage.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "PostPagesCount", txtThreadPageCount.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "ForumSkin", ddlSkins.SelectedItem.Value)
				ctlModule.UpdateModuleSetting(ModuleId, "ImageExtension", txtImageExtension.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "LoadScripts", chkEnableScripts.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "IconBarAsImages", chkEnableIconBarAsImages.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "DisplayPosterLocation", ddlDisplayPosterLocation.SelectedItem.Value)
				ctlModule.UpdateModuleSetting(ModuleId, "DisplayPosterRegion", chkDisplayPosterRegion.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "DisplayPosterRegion", chkDisplayPosterRegion.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, "EnableQuickReply", chkEnableQuickReply.Text)

				Config.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds the available themes (skins) for the module
		''' </summary>
		''' <remarks>Binds a list of themes in the module's directory.</remarks>
		Private Sub BindThemes()
			Try
				Dim themesPath As String = System.IO.Path.Combine(Server.MapPath(objConfig.SourceDirectory), "Themes")

				With ddlSkins
					.ClearSelection()

					For Each themeDir As String In System.IO.Directory.GetDirectories(themesPath)
						Dim currentDir As New System.IO.DirectoryInfo(themeDir)
						ddlSkins.Items.Add(New ListItem(currentDir.Name, currentDir.Name))
					Next
					.DataBind()
				End With
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Binds the available poster location display options, pulled from core Lists table.
		''' </summary>
		''' <remarks>Uses lists localized items to determine options.</remarks>
		Private Sub BindPosterLocation()
			' Use new Lists feature to provide DisplayPosterLocation entries (localization support)
			Dim ctlLists As New DotNetNuke.Common.Lists.ListController
			Dim LocationDisplayTypes As DotNetNuke.Common.Lists.ListEntryInfoCollection = ctlLists.GetListEntryInfoCollection("DisplayPosterLocation")
			ddlDisplayPosterLocation.ClearSelection()

			For Each entry As DotNetNuke.Common.Lists.ListEntryInfo In LocationDisplayTypes
				Dim LocationEntryType As New ListItem(Localization.GetString(entry.Text, objConfig.SharedResourceFile), entry.Value)
				ddlDisplayPosterLocation.Items.Add(LocationEntryType)
			Next
			' Now Bind the items
			ddlDisplayPosterLocation.Items.FindByValue(objConfig.DisplayPosterLocation.ToString).Selected = True
		End Sub

#End Region

	End Class

End Namespace