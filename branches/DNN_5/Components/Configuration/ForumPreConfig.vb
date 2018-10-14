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

Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Controllers

Namespace DotNetNuke.Modules.Forum

#Region "ForumPreConfig"

    ''' <summary>
    ''' Runs only when a forum module is first placed on a page to set 
    ''' configuration defaults and to create a new forum group and a new 
    ''' default forum so user can use immediately
    ''' </summary>
    ''' <remarks>Only fires off when a new module instance is placed on a page.
    ''' </remarks>
    Public Class ForumPreConfig

#Region "Public Shared Methods"

        ''' <summary>
        ''' Creates the default forum configuration (module settings) for a new
        ''' instance of the module.
        ''' </summary>
        ''' <param name="ModuleId">The moduleID of the module instance we are configuring.</param>
        ''' <param name="PortalId">The portalID of the module isntance we are configuring.</param>
        ''' <param name="UserId">The UserID of who has placed the module on a page.</param>
        ''' <remarks>This adds module settings and creates a froum group and forum to allow users to start using module immediately after placing on a page. 
        ''' It has later been useful for upgrade situations.
        ''' </remarks>
        ''' <history>
        ''' </history>
        Public Shared Sub PreConfig(ByVal ModuleId As Integer, ByVal PortalId As Integer, ByVal UserId As Integer)
            Dim ctlModule As New DotNetNuke.Entities.Modules.ModuleController
            Dim _portalSettings As PortalSettings = PortalController.Instance.GetCurrentPortalSettings
            Dim mSourceDirectory As String = ApplicationPath & "/DesktopModules/Forum"

            If Not _portalSettings.Email Is Nothing Then
                ctlModule.UpdateModuleSetting(ModuleId, Constants.EMAIL_AUTO_FROM_ADDRESS, _portalSettings.Email)
            End If

            ' Users online module integration
            Dim Enabled As Boolean
            If HostController.Instance.GetSettingsDictionary.ContainsKey(Constants.HOST_SETTING_USERS_ONLINE) Then
                If HostController.Instance.GetSettingsDictionary(Constants.HOST_SETTING_USERS_ONLINE).ToString = "Y" Then
                    Enabled = False
                Else
                    Enabled = True
                End If
            Else
                Enabled = False
            End If

            'Community
            ctlModule.UpdateModuleSetting(ModuleId, Constants.ENABLE_USERS_ONLINE, Enabled.ToString)
            ctlModule.UpdateModuleSetting(ModuleId, Constants.EMAIL_ADDRESS_DISPLAY_NAME, _portalSettings.PortalName & " " & Localization.GetString("Forum", mSourceDirectory & "/App_LocalResources/SharedResources.resx"))

            SetupDefaultGroup(ModuleId, PortalId, UserId, mSourceDirectory)
        End Sub

        ''' <summary>
        ''' Adds a default forum to the default group created here as well
        ''' </summary>
        ''' <param name="ModuleId">The ModuleID of the module being configured.</param>
        ''' <param name="PortalId">The PortalID of the module being configured.</param>
        ''' <param name="UserId">The UserID of the person placing the module on a page.</param>
        ''' <param name="mSourceDirectory">The module's path to it's files.</param>
        ''' <remarks>Used to create a default group for a new module instance.
        ''' </remarks>
        ''' <history>
        ''' </history>
        Public Shared Sub SetupDefaultGroup(ByVal ModuleId As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal mSourceDirectory As String)
            Dim cntGroup As New GroupController
            Dim cntForum As New ForumController
            Dim newGroupName As String = Localization.GetString("DefaultForumGroupName", mSourceDirectory & "/App_LocalResources/SharedResources.resx")
            Dim newForumName As String = Localization.GetString("DefaultForumName", mSourceDirectory & "/App_LocalResources/SharedResources.resx")
            Dim GroupID As Integer
            Dim objUser As New Users.UserInfo
            Dim cntUser As New Entities.Users.UserController

            objUser = cntUser.GetUser(PortalId, UserId)
            Try
                GroupID = cntGroup.GroupAdd(newGroupName, PortalId, ModuleId, objUser.UserID)
                Dim objForum As New ForumInfo

                objForum.GroupID = GroupID
                objForum.Name = newForumName
                objForum.ForumType = ForumType.Normal
                objForum.ForumBehavior = ForumBehavior.PublicUnModerated
                objForum.IsActive = True
                objForum.CreatedByUser = UserId
                objForum.ModuleID = ModuleId
                objForum.ParentID = 0
                ' Email
                objForum.NotifyByDefault = False
                objForum.EmailStatusChange = False
                objForum.EmailEnableSSL = False
                objForum.EmailAuth = 0
                objForum.EnableSitemap = True
                objForum.SitemapPriority = 0.5

                cntForum.ForumAdd(objForum)
            Catch ex As Exception
                LogException(ex)
            End Try

        End Sub

#End Region

    End Class

#End Region

End Namespace
