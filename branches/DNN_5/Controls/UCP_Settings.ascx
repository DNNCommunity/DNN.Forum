<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Settings.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Settings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm ucpSettings dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<div class="dnnFormItem">
		<dnn:label id="plEmailFormat" runat="server" controlname="ddlEmailFormat" suffix=":" />
		<dnnweb:DnnComboBox id="rcbEmailFormat" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plThreadsPerPage" runat="server" suffix=":" controlname="txtThreadsPerPage" />
		<asp:TextBox ID="txtThreadsPerPage" runat="server" EnableViewState="false" />
		<asp:RegularExpressionValidator ID="valThreadsPerPage" runat="server" resourcekey="NumericValidation.ErrorMessage" CssClass="dnnFormMessage dnnFormValidationSummary" ValidationExpression="[0-9]{1,}"  ControlToValidate="txtThreadsPerPage" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plPostsPerPage" runat="server" suffix=":" controlname="txtPostsPerPage" />
		<asp:TextBox ID="txtPostsPerPage" runat="server" EnableViewState="false" />
		<asp:RegularExpressionValidator ID="valPostsPerPage" runat="server" resourcekey="NumericValidation.ErrorMessage" CssClass="dnnFormMessage dnnFormValidationSummary" ValidationExpression="[0-9]{1,}" ControlToValidate="txtPostsPerPage" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
	</div>
	<div class="dnnFormItem" id="divOnlineStatus" runat="server">
		<dnn:label id="plOnlineStatus" runat="server" suffix=":" controlname="chkOnlineStatus" />
		<asp:CheckBox ID="chkOnlineStatus" runat="server" EnableViewState="false" />
	</div>
	<div class="dnnFormItem" id="divEnableDefaultPostNotify" runat="server">
		<dnn:label id="plEnableDefaultPostNotify" runat="server" suffix=":" controlname="chkEnableDefaultPostNotify" EnableViewState="false" />
		<asp:CheckBox ID="chkEnableDefaultPostNotify" runat="server" EnableViewState="false" />
	</div>
	<div class="dnnFormItem" id="divEnableSelfNotifications" runat="server">
		<dnn:label id="plEnableSelfNotifications" runat="server" suffix=":" controlname="chkEnableSelfNotifications" />
		<asp:CheckBox ID="chkEnableSelfNotifications" runat="server" EnableViewState="false" />
	</div>
	<div class="dnnFormItem" id="divForumModNotify" runat="server">
		<dnn:label id="plEnableForumModNotify" runat="server" suffix=":" controlname="chkEnableForumModNotify" />
		<asp:CheckBox ID="chkEnableForumModNotify" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
	</div>
	<div class="dnnFormItem" id="divClearReads" runat="server">
		<dnn:label id="plClearReads" runat="server" controlname="cmdClearReads" suffix=":" />
		<asp:LinkButton ID="cmdClearReads" runat="server" resourcekey="cmdClearReads" EnableViewState="false" />
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton class="dnnPrimaryAction" ID="cmdUpdate" runat="server" resourcekey="cmdUpdate" EnableViewState="false" /></li>
	</ul>
</div>