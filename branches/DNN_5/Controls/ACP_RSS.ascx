<%@ Control language="vb" CodeBehind="ACP_RSS.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.RSS" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm acpRSS dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plRSSFeeds" runat="server" Suffix=":" controlname="chkRSSFeeds" />
			<asp:checkbox id="chkRSSFeeds" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
		</div>
		<div class="dnnFormItem" id="divThreadsPerFeed" runat="server">
			<dnn:label id="plRSSThreadsPerFeed" runat="server" Suffix=":" controlname="txtRSSThreadsPerFeed" />
			<asp:textbox id="txtRSSThreadsPerFeed" runat="server" MaxLength="3" />
			<asp:RequiredFieldValidator ID="valreqRSSThreadsFeed" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormerror" Display="Dynamic" ControlToValidate="txtRSSThreadsPerFeed" EnableViewState="false" SetFocusOnError="true" />  
			<asp:regularexpressionvalidator id="valRSSThreadsPerFeed" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtRSSThreadsPerFeed" CssClass="dnnFormMessage dnnFormValidationSummary" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem" id="divTTL" runat="server">
			<dnn:label id="plTTL" runat="server" Suffix=":" controlname="txtTTL" />
			<asp:textbox id="txtTTL" runat="server" />
			<asp:RequiredFieldValidator ID="valreqTTL" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtTTL" EnableViewState="false" SetFocusOnError="true" />
			<asp:regularexpressionvalidator id="valTTLNumeric" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtTTL" CssClass="dnnFormMessage dnnFormValidationSummary" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" /></li>
		</ul>
	</fieldset>
</div>