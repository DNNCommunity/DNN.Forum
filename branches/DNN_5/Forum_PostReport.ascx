<%@ Control language="vb" CodeBehind="Forum_PostReport.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.PostReport" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm forumPostReport dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label ID="lblTitleSubject" runat="server" resourcekey="lblTitleSubject" /><asp:label id="lblSubject" runat="server" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plDeleteTemplate" runat="server" controlname="ddlDeleteTemplate" suffix=":" />
			<asp:DropDownList id="ddlReportTemplate" runat="server" AutoPostBack="True" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plReason" runat="server" controlname="txtReason" suffix=":" />
			<asp:textbox id="txtReason" runat="server" Rows="8" TextMode="MultiLine" />
			<asp:requiredfieldvalidator id="valEmailedResponse" runat="server" resourcekey="valEmailedResponse" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtReason" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plAuthor" runat="server" controlname="txtReason" suffix=":" />
			<asp:label id="lblAuthor" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plMessage" runat="server" controlname="txtReason" suffix=":" />
			<div class="dnnLeft">
				<asp:Literal id="lblMessage" runat="server" />
			</div>
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdReport" runat="server" resourcekey="cmdReport" /></li>
			<li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdCancel" runat="server" resourcekey="cmdCancel" CausesValidation="False" /></li>
		</ul>
	</fieldset>
</div>