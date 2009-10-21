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
	''' The control which allows forum administrators to configure the module 
	''' settings which control forum ratings, rankings, and statistics and how they
	''' behave in this module.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class Rating
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			chkRatings.Checked = objConfig.EnableRatings
			txt1stRating.Text = objConfig.Rating1Title
			txt2ndRating.Text = objConfig.Rating2Title
			txt3rdRating.Text = objConfig.Rating3Title
			txt4thRating.Text = objConfig.Rating4Title
			txt5thRating.Text = objConfig.Rating5Title
			txt6thRating.Text = objConfig.Rating6Title
			txt7thRating.Text = objConfig.Rating7Title
			txt8thRating.Text = objConfig.Rating8Title
			txt9thRating.Text = objConfig.Rating9Title
			txt10thRating.Text = objConfig.Rating10Title
			'txtNoRating.Text = mForumConfig.NoRatingTitle
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Loads and binds the settings for the current control
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			img1stRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "1" & "." & objConfig.ImageExtension)
			img1stRating.ToolTip = "stars_" & "1" & "." & objConfig.ImageExtension
			img2ndRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "2" & "." & objConfig.ImageExtension)
			img2ndRating.ToolTip = "stars_" & "2" & "." & objConfig.ImageExtension
			img3rdRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "3" & "." & objConfig.ImageExtension)
			img3rdRating.ToolTip = "stars_" & "3" & "." & objConfig.ImageExtension
			img4thRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "4" & "." & objConfig.ImageExtension)
			img4thRating.ToolTip = "stars_" & "4" & "." & objConfig.ImageExtension
			img5thRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "5" & "." & objConfig.ImageExtension)
			img5thRating.ToolTip = "stars_" & "5" & "." & objConfig.ImageExtension
			img6thRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "6" & "." & objConfig.ImageExtension)
			img6thRating.ToolTip = "stars_" & "6" & "." & objConfig.ImageExtension
			img7thRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "7" & "." & objConfig.ImageExtension)
			img7thRating.ToolTip = "stars_" & "7" & "." & objConfig.ImageExtension
			img8thRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "8" & "." & objConfig.ImageExtension)
			img8thRating.ToolTip = "stars_" & "8" & "." & objConfig.ImageExtension
			img9thRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "9" & "." & objConfig.ImageExtension)
			img9thRating.ToolTip = "stars_" & "9" & "." & objConfig.ImageExtension
			img10thRating.ImageUrl = objConfig.GetThemeImageURL("stars_" & "10" & "." & objConfig.ImageExtension)
			img10thRating.ToolTip = "stars_" & "10" & "." & objConfig.ImageExtension
		End Sub

		''' <summary>
		''' Updates the module settings configurable on this page.  Then redirects
		''' the user to the Forum Administration Screen.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
			Try
				' Update settings in the database
				Dim ctlModule As New Entities.Modules.ModuleController

				ctlModule.UpdateModuleSetting(ModuleId, "EnableRatings", chkRatings.Checked.ToString())
				ctlModule.UpdateModuleSetting(ModuleId, "Rating1Title", txt1stRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating2Title", txt2ndRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating3Title", txt3rdRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating4Title", txt4thRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating5Title", txt5thRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating6Title", txt6thRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating7Title", txt7thRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating8Title", txt8thRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating9Title", txt9thRating.Text)
				ctlModule.UpdateModuleSetting(ModuleId, "Rating10Title", txt10thRating.Text)
				'ctlModule.UpdateModuleSetting(ModuleId, "NoRatingTitle", txtNoRating.Text)

				Config.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

	End Class

End Namespace