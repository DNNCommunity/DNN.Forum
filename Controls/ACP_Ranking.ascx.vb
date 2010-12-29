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

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' The control which allows forum administrators to configure the module settings for ranking.
	''' behave in this module.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class Ranking
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			chkRankings.Checked = objConfig.Ranking
			chkEnableRankingImage.Checked = objConfig.EnableRankingImage
			txtFirstRank.Text = objConfig.FirstRankPosts.ToString
			txtSecondRank.Text = objConfig.SecondRankPosts.ToString
			txtThirdRank.Text = objConfig.ThirdRankPosts.ToString
			txtFourthRank.Text = objConfig.FourthRankPosts.ToString
			txtFifthRank.Text = objConfig.FifthRankPosts.ToString
			txtSixthRank.Text = objConfig.SixthRankPosts.ToString
			txtSeventhRank.Text = objConfig.SeventhRankPosts.ToString
			txtEigthRank.Text = objConfig.EigthRankPosts.ToString
			txtNinthRank.Text = objConfig.NinthRankPosts.ToString
			txtTenthRank.Text = objConfig.TenthRankPosts.ToString
			txt1stTitle.Text = objConfig.Rank_1_Title
			txt2ndTitle.Text = objConfig.Rank_2_Title
			txt3rdTitle.Text = objConfig.Rank_3_Title
			txt4thTitle.Text = objConfig.Rank_4_Title
			txt5thTitle.Text = objConfig.Rank_5_Title
			txt6thTitle.Text = objConfig.Rank_6_Title
			txt7thTitle.Text = objConfig.Rank_7_Title
			txt8thTitle.Text = objConfig.Rank_8_Title
			txt9thTitle.Text = objConfig.Rank_9_Title
			txt10thTitle.Text = objConfig.Rank_10_Title
			txtNoRankTitle.Text = objConfig.Rank_0_Title
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
			img1stRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "1"))
			img1stRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "1")
			img2ndRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "2"))
			img2ndRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "2")
			img3rdRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "3"))
			img3rdRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "3")
			img4thRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "4"))
			img4thRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "4")
			img5thRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "5"))
			img5thRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "5")
			img6thRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "6"))
			img6thRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "6")
			img7thRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "7"))
			img7thRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "7")
			img8thRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "8"))
			img8thRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "8")
			img9thRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "9"))
			img9thRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "9")
			img10thRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "10"))
			img10thRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "10")
			imgNoRank.ImageUrl = objConfig.GetThemeImageURL(String.Format("Rank_{0}." & objConfig.ImageExtension, "0"))
			imgNoRank.ToolTip = String.Format("Rank_{0}." & objConfig.ImageExtension, "0")
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
				Dim ctlModule As New Entities.Modules.ModuleController
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_RANKINGS, chkRankings.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_RANKING_IMAGES, chkEnableRankingImage.Checked.ToString)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.FIRST_RANK, txtFirstRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.SECOND_RANK, txtSecondRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.THIRD_RANK, txtThirdRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.FOURTH_RANK, txtFourthRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.FIFTH_RANK, txtFifthRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.SIXTH_RANK, txtSixthRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.SEVENTH_RANK, txtSeventhRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.EIGTH_RANK, txtEigthRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.NINTH_RANK, txtNinthRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.TENTH_RANK, txtTenthRank.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_1_TITLE, txt1stTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_2_TITLE, txt2ndTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_3_TITLE, txt3rdTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_4_TITLE, txt4thTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_5_TITLE, txt5thTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_6_TITLE, txt6thTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_7_TITLE, txt7thTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_8_TITLE, txt8thTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_9_TITLE, txt9thTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_10_TITLE, txt10thTitle.Text)
				ctlModule.UpdateModuleSetting(ModuleId, Constants.RANKING_0_TITLE, txtNoRankTitle.Text)

				Configuration.ResetForumConfig(ModuleId)

				lblUpdateDone.Visible = True
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

	End Class

End Namespace