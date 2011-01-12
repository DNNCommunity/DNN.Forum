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

Imports DotNetNuke.Wrapper.UI.WebControls

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
            BindThemes()
            BindPosterLocation()

            txtTheardsPerPage.Text = objConfig.ThreadsPerPage.ToString()
            txtPostsPerPage.Text = objConfig.PostsPerPage.ToString()
            txtThreadPageCount.Text = objConfig.PostPagesCount.ToString()
            txtImageExtension.Text = objConfig.ImageExtension
            txtMaxPostImageWidth.Text = objConfig.MaxPostImageWidth.ToString
            chkDisplayPosterRegion.Checked = objConfig.DisplayPosterRegion
            chkEnableQuickReply.Checked = objConfig.EnableQuickReply
            rcbDisplayPosterLocation.Items.FindItemByValue(objConfig.DisplayPosterLocation.ToString).Selected = True
            chkEnableTagging.Checked = objConfig.EnableTagging

            If System.IO.Directory.Exists(System.IO.Path.Combine(Server.MapPath(objConfig.SourceDirectory), "Themes/" + objConfig.ForumTheme)) Then
                rcbSkins.Items.FindItemByValue(objConfig.ForumTheme).Selected = True
            Else
                rcbSkins.Items(0).Selected = True
            End If
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
				ctlModule.UpdateModuleSetting(ModuleId, Constants.THREADS_PER_PAGE, txtTheardsPerPage.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.POSTS_PER_PAGE, txtPostsPerPage.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.POST_PAGE_COUNT_LIMIT, txtThreadPageCount.Text)
                ctlModule.UpdateModuleSetting(ModuleId, Constants.FORUM_THEME, rcbSkins.SelectedItem.Value)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.IMAGE_EXTENSIONS, txtImageExtension.Text)
                ctlModule.UpdateModuleSetting(ModuleId, Constants.DISPLAY_POSTER_LOCATION, rcbDisplayPosterLocation.SelectedItem.Value)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.DISPLAY_POSTER_REGION, chkDisplayPosterRegion.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_QUICK_REPLY, chkEnableQuickReply.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_TAGGING, chkEnableTagging.Checked.ToString)

				Configuration.ResetForumConfig(ModuleId)

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

                With rcbSkins
                    .Items.Clear()

                    For Each themeDir As String In System.IO.Directory.GetDirectories(themesPath)
                        Dim currentDir As New System.IO.DirectoryInfo(themeDir)

                        If Not currentDir.Name.StartsWith(".") Then
                            rcbSkins.Items.Add(New DnnComboBoxItem(currentDir.Name, currentDir.Name))
                        End If
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
            rcbDisplayPosterLocation.Items.Clear()

            rcbDisplayPosterLocation.Items.Insert(0, New DnnComboBoxItem(Localization.GetString("None", objConfig.SharedResourceFile), "0"))
            rcbDisplayPosterLocation.Items.Insert(1, New DnnComboBoxItem(Localization.GetString("ToAdmin", objConfig.SharedResourceFile), "1"))
            rcbDisplayPosterLocation.Items.Insert(2, New DnnComboBoxItem(Localization.GetString("ToAll", objConfig.SharedResourceFile), "2"))
		End Sub

#End Region

	End Class

End Namespace