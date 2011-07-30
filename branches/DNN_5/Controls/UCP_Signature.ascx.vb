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

Namespace DotNetNuke.Modules.Forum.UCP

    ''' <summary>
    ''' This is the users "Edit Profile" page and also the page used for administrators of
    ''' the module to change forum user settings. This is the forum users profile.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[skeel]	11/28/2008	Created
    ''' </history>
    Partial Public Class Signature
        Inherits ForumModuleBase
        Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

        ''' <summary>
        ''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            Dim cntForumUser As New ForumUserController
            Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

            With ProfileUser
                txtSignature.Text = Server.HtmlDecode(.Signature)
            End With
        End Sub

#End Region

#Region "Event Handlers"

        ''' <summary>
        ''' Updates the users Forum settings
        ''' </summary>
        ''' <param name="sender">System.Object</param>
        ''' <param name="e">System.EventArgs</param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	7/13/2005	Created
        ''' </history>
        Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            Try
                Dim cntForumUser As New ForumUserController
                Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, PortalId)

                With ProfileUser
                    Dim Signature As String = txtSignature.Text
                    Dim objSecurity As New PortalSecurity

                    If objConfig.EnableUserSignatures Then
                        ' run against security filter
                        Signature = objSecurity.InputFilter(Signature, PortalSecurity.FilterFlag.NoScripting)
                        ' check to see if HTML sigs are supported
                        If Not objConfig.EnableHTMLSignatures Then
                            Signature = Utilities.ForumUtils.StripHTML(Signature)
                        End If
                    Else
                        Signature = String.Empty
                    End If

                    .Signature = Signature

                    Dim cntUser As New ForumUserController
                    cntUser.Update(ProfileUser)

                    DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(ProfileUser.UserID, PortalId)

                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me.Parent, Localization.GetString("lblUpdateDone.Text", LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)
                End With
            Catch Exc As System.Exception
                LogException(Exc)
                Return
            End Try
        End Sub

        ''' <summary>
        ''' Shows a preview of the user's signature, hides the edit area for it
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	7/13/2005	Created
        ''' </history>
        Protected Sub cmdPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
            Dim Signature As Utilities.PostContent = New Utilities.PostContent(Server.HtmlDecode(txtSignature.Text), objConfig)
            lblSignature.Text = "<HR><br />" & Signature.ProcessHtml()
            EnablePreview(True)
        End Sub

        ''' <summary>
        ''' Turns off signature preview, makes it editable 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	7/13/2005	Created
        ''' </history>
        Protected Sub cmdEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
            EnablePreview(False)
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Enables/Disable user signature preview mode
        ''' </summary>
        ''' <param name="IsPreview"></param>
        ''' <remarks>
        ''' </remarks>
        Private Sub EnablePreview(ByVal IsPreview As Boolean)
            lblSignature.Visible = IsPreview
            cmdEdit.Visible = IsPreview
            txtSignature.Visible = Not IsPreview
            cmdPreview.Visible = Not IsPreview
        End Sub

#End Region

#Region "Protected Functions"

        ''' <summary>
        ''' Binds the remove icon based on theme.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function imgClearSrc() As String
            Return objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
        End Function

        ''' <summary>
        ''' Binds the edit icon based on theme.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function imgEditSrc() As String
            Return objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
        End Function

        ''' <summary>
        ''' Binds the preview icon based on theme.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function imgPreviewSrc() As String
            Return objConfig.GetThemeImageURL("s_preview.") & objConfig.ImageExtension
        End Function

#End Region

    End Class

End Namespace