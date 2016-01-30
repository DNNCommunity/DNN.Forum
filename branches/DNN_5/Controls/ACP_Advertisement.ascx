<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ACP_Advertisement.ascx.vb" Inherits="DotNetNuke.Modules.Forum.ACP.Advertisement" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="dnnForm acpAdvertisement dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label ID="plAdsAfterFirstPost" runat="server" ControlName="cbAdsAfterFirstPost" Suffix=":" />
			<asp:CheckBox runat="server" ID="cbAdsAfterFirstPost" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plAddAdverAfterPostNo" runat="server" ControlName="txtAddAdverAfterPostNo" Suffix=":" />
			<asp:TextBox runat="server" ID="tbAddAdverAfterPostNo" CssClass="dnnFormRequired" Width="30px" MaxLength="2" EnableViewState="false" />
			<asp:RequiredFieldValidator ID="valreqView" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="tbAddAdverAfterPostNo" EnableViewState="false" SetFocusOnError="true" />
			<asp:RegularExpressionValidator ID="valView" runat="server" ControlToValidate="tbAddAdverAfterPostNo" CssClass="dnnFormMessage dnnFormError" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plAdvertisementText" runat="server" ControlName="txtAdvertisementText" Suffix=":" />
			<asp:TextBox runat="server" ID="tbAdvertisementText" TextMode="MultiLine" Rows="8" />
		</div>
		<div>
			<h3><asp:Label runat="server" ID="plVendorsList" resourcekey="plVendorsList" /></h3>
			<dnnweb:DnnGrid ID="rgVendors" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PageSize="10" AllowMultiRowSelection="true" Skin="Sitefinity">
				<MasterTableView DataKeyNames="VendorID">
					<NoRecordsTemplate>
						<asp:Label runat="server" ID="lblNoAdvertisements" resourcekey="lblNoAdvertisements" CssClass="Normal" />
					</NoRecordsTemplate>
					<Columns>
						<dnnweb:DnnGridClientSelectColumn  UniqueName="IsEnabled" HeaderText="IsEnabled" />
						<wrapper:GridNumericColumn UniqueName="VendorID" DataField="VendorID"  Visible="false" HeaderText="VendorID" />
						<dnnweb:DnnGridBoundColumn UniqueName="VendorName" DataField="VendorName" HeaderText="VendorName" />
						<wrapper:GridImageColumn UniqueName="Logo" DataType="System.String" DataImageUrlFields="LogoFile" AlternateText="No logo" HeaderText="Logo" />
						<dnnweb:DnnGridBoundColumn UniqueName="BannerUrl" DataField="BannerUrl" HeaderText="BannerUrl" />
					</Columns>
				</MasterTableView>
				<ClientSettings  EnableRowHoverStyle="true">
					<Selecting AllowRowSelect="True" />
				</ClientSettings>
			</dnnweb:DnnGrid>
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
	</ul>
</div>