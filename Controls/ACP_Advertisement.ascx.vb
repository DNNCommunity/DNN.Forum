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
    ''' The control which allows forum administrators to add advertisements between posts 
    ''' Here you can add advertisement text and how densely it will be injected
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[b.waluszko]	21/10/2010	Created
    ''' </history>
    Partial Public Class Advertisement
        Inherits ForumModuleBase
        Implements Utilities.AjaxLoader.IPageLoad



        ''' <summary>
        ''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
        ''' </summary>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            cbAdsAfterFirstPost.Checked = objConfig.AdsAfterFirstPost
            tbAddAdverAfterPostNo.Text = objConfig.AddAdverAfterPostNo.ToString()
            tbAdvertisementText.Text = objConfig.AdvertisementText

        End Sub

        ''' <summary>
        ''' Updates the module's configuration (module settings)
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Saves the module settings shown in this view.</remarks>
        Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            Try
                ' Update settings in the database
                Dim ctlModule As New Entities.Modules.ModuleController
                ctlModule.UpdateModuleSetting(ModuleId, Constants.ADS_AFTER_FIRST_POST, cbAdsAfterFirstPost.Checked.ToString())
                ctlModule.UpdateModuleSetting(ModuleId, Constants.ADD_ADVER_AFTER_POST_NO, tbAddAdverAfterPostNo.Text)
                ctlModule.UpdateModuleSetting(ModuleId, Constants.ADVERTISEMENT_TEXT, tbAdvertisementText.Text.Trim())
                Configuration.ResetForumConfig(ModuleId)

                lblUpdateDone.Visible = True
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

    End Class

End Namespace