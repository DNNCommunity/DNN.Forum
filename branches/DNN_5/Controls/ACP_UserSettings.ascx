<%@ Control language="vb" CodeBehind="ACP_UserSettings.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.UserSettings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm acpUserSettings dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label ID="plNameDisplay" runat="server" ControlName="chkUserCountry" Suffix=":" />
			<asp:DropDownList ID="rcbNameDisplay" runat="server" />
		</div>
		<div class="dnnFormItem" id="divEditWindow" runat="server">
			<dnn:label id="plPostEditWindow" runat="server" Suffix=":" controlname="txtEditWindow" />
			<asp:TextBox id="txtPostEditWindow" runat="server" />
			<asp:RangeValidator ID="valPostEditWindow" runat="server" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtPostEditWindow" Display="Dynamic" resourcekey="PostEditWindow.ErrorMessage" MaximumValue="60" MinimumValue="0" Type="Integer" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plAutoTrustEnabled" runat="server" ControlName="chkEnableAutoTrust" Suffix=":" />
			<asp:CheckBox ID="chkEnableAutoTrust" runat="server" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divAutoTrustTime" runat="server">
			<dnn:Label ID="plAutoTrustTime" runat="server" ControlName="chkTrustNewUsers" Suffix=":" />
			<asp:TextBox id="txtAutoTrustTime" runat="server" />
			<asp:RangeValidator ID="valAutoTrustTime" runat="server" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtAutoTrustTime" Display="Dynamic" resourcekey="AutoTrustTime.ErrorMessage" MaximumValue="1000" MinimumValue="0" Type="Integer" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plAutoLockTrust" runat="server" ControlName="chkAutoLockTrust" Suffix=":" />
			<asp:CheckBox ID="chkAutoLockTrust" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plUserReadManagement" runat="server" ControlName="chkUserReadManagement" Suffix=":" />
			<asp:CheckBox ID="chkUserReadManagement" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plEnableUserSignatures" runat="server" controlname="chkEnableUserSignatures" Suffix=":" />
			<asp:checkbox id="chkEnableUserSignatures" runat="server" AutoPostBack="True" />
		</div>
		<div class="dnnFormItem" id="divModSigUpdates" runat="server">
			<dnn:label id="plEnableModSigUpdates" runat="server" Suffix=":" controlname="chkEnableModSigUpdates" />
			<asp:checkbox id="chkEnableModSigUpdates" runat="server" />
		</div>
		<div class="dnnFormItem" id="divHTMLSignatures" runat="server">
			<dnn:label id="plEnableHTMLSignatures" runat="server" Suffix=":" controlname="chkEnableHTMLSignatures" />
			<asp:checkbox id="chkEnableHTMLSignatures" runat="server" />
		</div>
		<div class="dnnFormItem" id="divHideModEdit" runat="server" visible="false">
			<dnn:label id="plHideModEdit" runat="server" Suffix=":" controlname="chkHideModEdit" />
			<asp:checkbox id="chkHideModEdit" runat="server" />
		</div>
		<div class="dnnFormItem" id="divUserBanning" runat="server">
			<dnn:label id="plEnableUserBanning" runat="server" Suffix=":" controlname="chkEnableUserBanning" />
			<asp:checkbox id="chkEnableUserBanning" runat="server" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
	</ul>
</div>