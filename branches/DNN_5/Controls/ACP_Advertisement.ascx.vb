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

Imports DotNetNuke.Services.Vendors

Namespace DotNetNuke.Modules.Forum.ACP

    ''' <summary>
    ''' The control which allows forum administrators to add advertisements between posts.
    ''' Here you can add advertisement text and set how densely it will be injected
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[b.waluszko]	21/10/2010	Created
    ''' </history>
    Partial Public Class Advertisement
        Inherits ForumModuleBase
        Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

        ''' <summary>
        ''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
        ''' </summary>
        Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
            cbAdsAfterFirstPost.Checked = objConfig.AdsAfterFirstPost
            tbAddAdverAfterPostNo.Text = objConfig.AddAdverAfterPostNo.ToString()
            tbAdvertisementText.Text = objConfig.AdvertisementText
        End Sub

#End Region

#Region "Event Handlers"

        ''' <summary>Updates the module's configuration (module settings)
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

                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, DotNetNuke.Services.Localization.Localization.GetString("lblUpdateDone.Text", Me.LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess)

                VendorsGridUpdate()
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Protected Sub rgVendors_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgVendors.NeedDataSource
            Dim advertController As New AdvertController
            Dim adverts As New List(Of AdvertInfo)
            Try
                adverts = advertController.VendorsGet(Me.ModuleId)
                For Each item As AdvertInfo In adverts
                    'vendor logo
                    If String.IsNullOrEmpty(item.LogoFile) = False Then
                        item.LogoFile = DotNetNuke.Common.Globals.LinkClick(item.LogoFile, Me.TabId, Me.ModuleId)
                    End If

                    'banners
                    Dim bannersList As List(Of BannerInfo) = advertController.BannersGet(item.VendorId)
                    If (bannersList IsNot Nothing) AndAlso bannersList.Count > 0 Then
                        item.BannerUrl = ""
                        For Each banner As BannerInfo In bannersList
                            item.BannerUrl += "<img src=""" & DotNetNuke.Common.Globals.LinkClick(banner.ImageFile, Me.TabId, Me.ModuleId) & """ />&nbsp;"
                        Next
                    End If

                    If String.IsNullOrEmpty(item.BannerUrl) Then
                        item.BannerUrl = "No banners"
                    End If

                Next
                rgVendors.DataSource = adverts
            Catch ex As Exception
                Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub rgVendors_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles rgVendors.PreRender
            'select rows that have enabled vendors
            Dim advertController As New AdvertController
            Dim vendors As List(Of AdvertInfo) = advertController.VendorsGet(ModuleId)

            If (vendors IsNot Nothing) AndAlso rgVendors.Items.Count > 0 Then
                For Each item As Telerik.Web.UI.GridDataItem In rgVendors.Items ' DotNetNuke.Wrapper.UI.WebControls.DnnGridDataItem
                    Dim i As Integer = CInt(item("VendorID").Text())
                    item.Selected = vendors.Where(Function(v) v.VendorId = i).FirstOrDefault().IsEnabled
                Next
            End If
        End Sub

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Save to DB enabled/disabled vendors
        ''' </summary>
        Private Sub VendorsGridUpdate()
            Dim advertController As New AdvertController
            For Each item As Telerik.Web.UI.GridDataItem In rgVendors.Items ' DotNetNuke.Wrapper.UI.WebControls.DnnGridDataItem
                advertController.VendorUpdate(CInt(item("VendorID").Text), item.Selected, ModuleId)
            Next
        End Sub

        ''' <summary>
        ''' Bind data to the Vendors list grid
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub VendorsGridBind()
            Dim advertController As New AdvertController
            Dim bannersController As New BannerController
            Dim adverts As New List(Of AdvertInfo)
            Try
                adverts = advertController.VendorsGet(Me.ModuleId)
                For Each item As AdvertInfo In adverts
                    'vendor logo
                    If String.IsNullOrEmpty(item.LogoFile) = False Then
                        item.LogoFile = DotNetNuke.Common.Globals.LinkClick(item.LogoFile, Me.TabId, Me.ModuleId)
                    End If

                    'banners
                    Dim bannersList As ArrayList = bannersController.GetBanners(item.VendorId)
                    If (bannersList IsNot Nothing) AndAlso bannersList.Count > 0 Then
                        item.BannerUrl = ""
                        For Each banner As BannerInfo In bannersList
                            item.BannerUrl += "<img src=""" & DotNetNuke.Common.Globals.LinkClick(banner.ImageFile, Me.TabId, Me.ModuleId) & """ />&nbsp;"
                        Next
                    End If

                    If String.IsNullOrEmpty(item.BannerUrl) Then
                        item.BannerUrl = "No banners"
                    End If

                Next
                rgVendors.DataSource = adverts
                rgVendors.DataBind()
            Catch ex As Exception
                Exceptions.ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace