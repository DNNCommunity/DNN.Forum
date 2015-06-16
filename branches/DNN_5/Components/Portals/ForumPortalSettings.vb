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
Imports System.IO
Imports System.Web.Hosting
Imports DotNetNuke.Forum.Library

Namespace DotNetNuke.Modules.Forum

    ''' <summary>
    ''' Simply put, this is called to populate a 'fake' HttpContext object when HttpContext is not available (ie. scheduled tasks). 
    ''' </summary>
    ''' <remarks>Although originally from the core, this is converted from a Bruce Chapman blog.</remarks>
    Public Class ForumPortalSettings
        Inherits BaseEntityInfo

        ''' <summary>
        ''' Creates a new instance of portal settings. This is used when HttpContext is not available (ie. scheduled tasks).
        ''' </summary>
        ''' <param name="portalId">The portal we are creating settings for.</param>
        ''' <returns>An instance of portal settings.</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateNewPortalSettings(ByVal portalId As Integer) As PortalSettings
            'new settings object
            Dim ps As New PortalSettings()

            'controller instances
            Dim pc As New PortalController()
            Dim tc As New TabController()
            Dim pac As New PortalAliasController()

            'get the first portal alias found to be used as the current portal alias

            Dim portalAlias As PortalAliasInfo = PortalUtilityClass.PrimaryPortalAlias(portalId)

            'get the portal and copy across the settings
            Dim portal As PortalInfo = pc.GetPortal(portalId)
            If Not portal Is Nothing Then
                ps.PortalAlias = portalAlias
                ps.PortalId = portal.PortalID
                ps.PortalName = portal.PortalName
                ps.LogoFile = portal.LogoFile
                ps.FooterText = portal.FooterText
                ps.ExpiryDate = portal.ExpiryDate
                ps.UserRegistration = portal.UserRegistration
                ps.BannerAdvertising = portal.BannerAdvertising
                ps.Currency = portal.Currency
                ps.AdministratorId = portal.AdministratorId
                ps.Email = portal.Email
                ps.HostFee = portal.HostFee
                ps.HostSpace = portal.HostSpace
                ps.PageQuota = portal.PageQuota
                ps.UserQuota = portal.UserQuota
                ps.AdministratorRoleId = portal.AdministratorRoleId
                ps.AdministratorRoleName = portal.AdministratorRoleName
                ps.RegisteredRoleId = portal.RegisteredRoleId
                ps.RegisteredRoleName = portal.RegisteredRoleName
                ps.Description = portal.Description
                ps.KeyWords = portal.KeyWords
                ps.BackgroundFile = portal.BackgroundFile
                ps.GUID = portal.GUID
                ps.SiteLogHistory = portal.SiteLogHistory
                ps.AdminTabId = portal.AdminTabId
                ps.SuperTabId = portal.SuperTabId
                ps.SplashTabId = portal.SplashTabId
                ps.HomeTabId = portal.HomeTabId
                ps.LoginTabId = portal.LoginTabId
                ps.UserTabId = portal.UserTabId
                ps.DefaultLanguage = portal.DefaultLanguage
                ps.TimeZoneOffset = portal.TimeZoneOffset
                ps.HomeDirectory = portal.HomeDirectory
                ps.Pages = portal.Pages
                ps.Users = portal.Users

                ' update properties with default values
                If Null.IsNull(ps.HostSpace) Then
                    ps.HostSpace = 0
                End If
                If Null.IsNull(ps.DefaultLanguage) Then
                    ps.DefaultLanguage = Localization.SystemLocale
                End If
                If Null.IsNull(ps.TimeZoneOffset) Then
                    ps.TimeZoneOffset = Localization.SystemTimeZoneOffset
                End If
                ps.HomeDirectory = ApplicationPath + "/" + portal.HomeDirectory + "/"
            End If

            ''At this point the DesktopTabs Collection contains all the Tabs for the current portal
            ''verify tab for portal. This assigns the Active Tab based on the Tab Id/PortalId
            'If VerifyPortalTab(portalId, tabID) Then
            '	If Not ps.ActiveTab Is Nothing Then
            '		' skin
            '		If IsAdminSkin() Then
            '			ps.ActiveTab.SkinSrc = ps.DefaultAdminSkin
            '		Else
            '			If ps.ActiveTab.SkinSrc = "" Then
            '				ps.ActiveTab.SkinSrc = ps.DefaultPortalSkin
            '			End If
            '		End If
            '		ps.ActiveTab.SkinSrc = SkinController.FormatSkinSrc(ps.ActiveTab.SkinSrc, ps)
            '		ps.ActiveTab.SkinPath = SkinController.FormatSkinPath(ps.ActiveTab.SkinSrc)

            '		' container
            '		If IsAdminSkin() Then
            '			ps.ActiveTab.ContainerSrc = ps.DefaultAdminContainer
            '		Else
            '			If ps.ActiveTab.ContainerSrc = "" Then
            '				ps.ActiveTab.ContainerSrc = ps.DefaultPortalContainer
            '			End If
            '		End If
            '		ps.ActiveTab.ContainerSrc = SkinController.FormatSkinSrc(ps.ActiveTab.ContainerSrc, ps)
            '		ps.ActiveTab.ContainerPath = SkinController.FormatSkinPath(ps.ActiveTab.ContainerSrc)

            '		' initialize collections
            '		ps.ActiveTab.BreadCrumbs = New ArrayList
            '		ps.ActiveTab.Panes = New ArrayList
            '		ps.ActiveTab.Modules = New ArrayList
            '	End If
            'End If

            ''last gasp chance in case active tab was not set
            'If ps.ActiveTab Is Nothing Then
            '	ps.ActiveTab = portalTab
            'End If

            ''Add each host Tab to DesktopTabs
            'Dim hostTab As DotNetNuke.Entities.Tabs.TabInfo = Nothing
            'For Each tabPair As KeyValuePair(Of Integer, DotNetNuke.Entities.Tabs.TabInfo) In tc.GetTabsByPortal(DotNetNuke.Common.Utilities.Null.NullInteger)
            '	' clone the tab object ( to avoid creating an object reference to the data cache )
            '	hostTab = tabPair.Value.Clone()
            '	hostTab.PortalID = ps.PortalId
            '	hostTab.StartDate = System.DateTime.MinValue
            '	hostTab.EndDate = System.DateTime.MaxValue
            '	ps.DesktopTabs.Add(hostTab)
            'Next

            'now add the portal settings to the httpContext
            If HttpContext.Current Is Nothing Then
                'if there is no HttpContext, then mock one up by creating a fake WorkerRequest
                Dim appVirtualDir As String = ApplicationPath
                Dim appPhysicalDir As String = AppDomain.CurrentDomain.BaseDirectory
                Dim page As String = ps.PortalAlias.HTTPAlias
                Dim query As String = String.Empty
                Dim output As TextWriter = Nothing
                'create a dummy simple worker request
                Dim workerRequest As New SimpleWorkerRequest(page, query, output)
                HttpContext.Current = New HttpContext(workerRequest)
            End If
            'stash the portalSettings in the Context Items, where the rest of the DNN Code expects it to be
            HttpContext.Current.Items.Add("PortalSettings", ps)

            Return ps
        End Function

    End Class

End Namespace