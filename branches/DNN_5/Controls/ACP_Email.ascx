<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_Email.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.Email" %>
<div class="dnnForm acpEmail dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
		<div class="dnnFormItem">
			<dnn:label id="plAutomatedAddress" runat="server" controlname="txtAutomatedAddress" suffix=":" />
			<asp:textbox id="txtAutomatedAddress" runat="server" cssclass="dnnFormRequired" MaxLength="255" />
			<asp:RequiredFieldValidator ID="valAddy" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtAutomatedAddress" SetFocusOnError="true" />
			<asp:RegularExpressionValidator ID="valEmailAddy" runat="server" ControlToValidate="txtAutomatedAddress" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" resourcekey="valEmailAddy.ErrorMessage" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plEmailAddressDisplayName" runat="server" ControlName="txtEmailAddressDisplayName" Suffix=":" />
			<asp:TextBox ID="txtEmailAddressDisplayName" runat="server" MaxLength="132" CssClass="dnnFormRequired" />
			<asp:RequiredFieldValidator ID="valDisplay" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtEmailAddressDisplayName" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plNofify" runat="server" controlname="chkNotify" suffix=":" />
			<asp:checkbox id="chkNotify" runat="server" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divEnablePerForumFrom" runat="server">
			<dnn:Label ID="plEnablePerForumFrom" runat="server" ControlName="chkEnablePerForumFrom" Suffix=":" />
			<asp:CheckBox ID="chkEnablePerForumFrom" runat="server" />
		</div>
		<div class="dnnFormItem" id="divEnableEditEmails" runat="server">
			<dnn:Label ID="plEnableEditEmails" runat="server" ControlName="chkEnableEditEmails" Suffix=":" />
			<asp:CheckBox ID="chkEnableEditEmails" runat="server" />
		</div>
		<div class="dnnFormItem" id="divEmailQueueTask" runat="server">
			<dnn:label id="plEmailQueueTask" runat="server" Suffix=":" controlname="chkEmailQueueTask" />
			<asp:checkbox id="chkEmailQueueTask" runat="server" />
		</div>
		<div class="dnnFormItem" id="divEnableListServer" runat="server" visible="false">
			<dnn:Label ID="plEnableListServer" runat="server" ControlName="chkEnableListServer" Suffix=":" />
			<asp:CheckBox ID="chkEnableListServer" runat="server" />
		</div>
		<div class="dnnFormItem" id="divListServerFolder" runat="server" visible="false">
			<dnn:Label ID="plListServerFolder" runat="server" ControlName="txtListServerFolder" Suffix=":" />
			<asp:TextBox ID="txtListServerFolder" runat="server" MaxLength="150" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton CssClass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
	</ul>
</div>