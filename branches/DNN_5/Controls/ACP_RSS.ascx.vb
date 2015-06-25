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
    ''' This is the control panel item for controll RSS module settings.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public MustInherit Class RSS
        Inherits ForumModuleBase
        Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

        ''' <summary>
        ''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            chkRSSFeeds.Checked = objConfig.EnableRSS
            txtRSSThreadsPerFeed.Text = Convert.ToString(objConfig.RSSThreadsPerFeed)
            txtTTL.Text = objConfig.RSSUpdateInterval.ToString
            divThreadsPerFeed.Visible = objConfig.EnableRSS
            divTTL.Visible = objConfig.EnableRSS
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
                ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_RSS_FEEDS, chkRSSFeeds.Checked.ToString)
                ctlModule.UpdateModuleSetting(ModuleId, Constants.RSS_FEEDS_PER_PAGE, txtRSSThreadsPerFeed.Text)
                ctlModule.UpdateModuleSetting(ModuleId, Constants.RSS_UPDATE_INTERVAL, txtTTL.Text)

                Configuration.ResetForumConfig(ModuleId)

                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, DotNetNuke.Services.Localization.Localization.GetString("lblUpdateDone.Text", Me.LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)
            Catch exc As Exception
                Dim s As String = exc.ToString
                s = s & " "
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' <summary>
        ''' Enables/Disables RSS config rows depending on primary setting
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>Changes viewable/editable items when checked/unchecked.
        ''' </remarks>
        Protected Sub chkRSSFeeds_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRSSFeeds.CheckedChanged
            divThreadsPerFeed.Visible = chkRSSFeeds.Checked
            divTTL.Visible = chkRSSFeeds.Checked

            'Set default values if empty
            If chkRSSFeeds.Checked Then
                If objConfig.RSSThreadsPerFeed > 0 Then
                    txtRSSThreadsPerFeed.Text = CStr(objConfig.RSSThreadsPerFeed)
                Else
                    Try
                        txtRSSThreadsPerFeed.Text = CStr(objConfig.RSSThreadsPerFeed)
                    Catch ex As Exception
                        txtRSSThreadsPerFeed.Text = "20"
                    End Try
                End If

                If objConfig.RSSUpdateInterval > 0 Then
                    txtTTL.Text = CStr(objConfig.RSSUpdateInterval)
                Else
                    Try
                        txtTTL.Text = CStr(objConfig.RSSUpdateInterval)
                    Catch ex As Exception
                        txtTTL.Text = "30"
                    End Try
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace