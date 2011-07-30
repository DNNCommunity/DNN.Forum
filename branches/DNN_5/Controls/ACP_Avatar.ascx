<%@ Control Language="vb" Codebehind="ACP_Avatar.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Avatar" Explicit="true" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm acpAvatar dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plEnableUserAvatar" runat="server" controlname="chkEnableUserAvatar" Suffix=":" />
			<asp:CheckBox ID="chkEnableUserAvatar" runat="server" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divEnableProfileAvatar" runat="server">
			<dnn:label id="plEnableProfileAvatar" runat="server" controlname="chkEnableProfileAvatar" Suffix=":" />
			<asp:CheckBox ID="chkEnableProfileAvatar" runat="server" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divEnableProfileUserFolders" runat="server">
			<dnn:label id="plEnableProfileUserFolders" runat="server" controlname="chkEnableProfileAvatar" Suffix=":" />
			<asp:CheckBox ID="chkEnableProfileUserFolders" runat="server" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divProfileAvatarPropertyName" runat="server">
			<dnn:label id="plProfileAvatarPropertyName" runat="server" controlname="ddlProfileAvatarPropertyName" Suffix=":" />
			<dnnweb:DnnComboBox ID="rcbProfileAvatarPropertyName" runat="server" DataTextField="PropertyName" DataValueField="PropertyName" />
		</div>
		<div class="dnnFormItem" id="divUserAvatarPath" runat="server">
			<dnn:label id="plUserAvatarPath" runat="server" controlname="txtUserAvatarPath" Suffix=":" />
			<asp:TextBox ID="txtUserAvatarPath" runat="server" CssClass="dnnFormRequired" MaxLength="255" />
			<asp:RequiredFieldValidator ID="valUsrPath" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarPath" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem" id="divUserAvatarDimensions" runat="server">
			<dnn:label id="plUserAvatarDimensions" runat="server" controlname="txtUserAvatarWidth" Suffix=":" />
			<asp:TextBox ID="txtUserAvatarWidth" runat="server" Width="50" MaxLength="3" CssClass="dnnFormRequired" />
			<asp:RequiredFieldValidator ID="valUsrAvW" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarWidth" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
			<asp:TextBox ID="txtUserAvatarHeight" runat="server" Width="50" MaxLength="3" CssClass="dnnFormRequired" />
			<asp:RequiredFieldValidator ID="valUsrH" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarHeight" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem" id="divUserAvatarSizeLimit" runat="server">
			<dnn:label id="plUserAvatarSizeLimit" runat="server" Suffix=":" />
			<asp:TextBox ID="txtUserAvatarSizeLimit" runat="server" CssClass="dnnFormRequired" MaxLength="4" />
			<asp:RequiredFieldValidator ID="valUsrSize" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarSizeLimit" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem" id="divUserAvatarPoolEnable" runat="server">
			<dnn:label id="plEnableUserAvatarPool" runat="server" controlname="chkEnableUserAvatarPool" Suffix=":" />
			<asp:CheckBox ID="chkEnableUserAvatarPool" runat="server" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divUserAvatarPoolPath" runat="server">
			<dnn:label id="plUserAvatarPoolPath" runat="server" controlname="txtUserAvatarPoolPath" Suffix=":" />
			<asp:TextBox ID="txtUserAvatarPoolPath" runat="server" CssClass="dnnFormRequired" MaxLength="255" />
			<asp:RequiredFieldValidator ID="valUserAvatarPoolPath" runat="server" ErrorMessage="*" ControlToValidate="txtUserAvatarPoolPath" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
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
		<div>
			<asp:CompareValidator ID="validUserDimWidth" runat="server" ControlToValidate="txtUserAvatarWidth" CssClass="dnnFormMessage dnnFormValidationSummary" Display="Dynamic" resourcekey="validUserDimWidth" Operator="DataTypeCheck" Type="Integer" EnableViewState="false" />
			<asp:CompareValidator ID="validUserDimHeight" runat="server" ControlToValidate="txtUserAvatarHeight" CssClass="dnnFormMessage dnnFormValidationSummary" Display="Dynamic" resourcekey="validUserDimHeight" Operator="DataTypeCheck" Type="Integer" EnableViewState="false" />
			<asp:CompareValidator ID="validUserSize" runat="server" ControlToValidate="txtUserAvatarSizeLimit" CssClass="dnnFormMessage dnnFormValidationSummary" Display="Dynamic" resourcekey="validUserSize" Operator="DataTypeCheck" Type="Integer" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" causesvalidation="False" cssclass="dnnPrimaryAction" EnableViewState="false" /></li>
	</ul>
</div>