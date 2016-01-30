<%@ Control Language="vb" Codebehind="ACP_Avatar.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Avatar" Explicit="true" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm acpAvatar dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plEnableUserAvatar" runat="server" controlname="chkEnableUserAvatar" Suffix=":" />
			<asp:CheckBox ID="chkEnableUserAvatar" runat="server" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divProfileAvatarPropertyName" runat="server">
			<dnn:label id="plProfileAvatarPropertyName" runat="server" controlname="ddlProfileAvatarPropertyName" Suffix=":" />
			<dnnweb:DnnComboBox ID="rcbProfileAvatarPropertyName" runat="server" DataTextField="PropertyName" DataValueField="PropertyName" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plEnableSystemAvatar" runat="server" controlname="chkEnableSystemAvatar" Suffix=":" />
			<asp:CheckBox ID="chkEnableSystemAvatar" runat="server" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divSystemAvatarPath" runat="server">
			<dnn:label id="plSystemAvatarPath" runat="server" Suffix=":" />
			<asp:TextBox ID="txtSystemAvatarPath" runat="server" CssClass="dnnFormRequired" MaxLength="255" />
			<asp:RequiredFieldValidator ID="valSysPath" runat="server" ErrorMessage="*" ControlToValidate="txtSystemAvatarPath" CssClass="dnnFormMessage dnnFormError" Display="Dynamic"  EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plEnableRoleAvatar" runat="server" controlname="chkEnableRoleAvatar" Suffix=":" />
			<asp:CheckBox ID="chkEnableRoleAvatar" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divRoleAvatarPath" runat="server">
			<dnn:label id="plRoleAvatarPath" runat="server" Suffix=":" controlname="txtRoleAvatarPath" />
			<asp:TextBox ID="txtRoleAvatarPath" runat="server" CssClass="dnnFormRequired" MaxLength="255" />
			<asp:RequiredFieldValidator ID="valRoleAvatarPath" runat="server" ErrorMessage="*" ControlToValidate="txtRoleAvatarPath" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" causesvalidation="False" cssclass="dnnPrimaryAction" EnableViewState="false" /></li>
	</ul>
</div>