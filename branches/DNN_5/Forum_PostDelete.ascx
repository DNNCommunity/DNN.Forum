<%@ Control language="vb" CodeBehind="Forum_PostDelete.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.PostDelete" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm forumPostDelete dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label ID="lblTitleSubject" runat="server" resourcekey="lblTitleSubject" /><asp:label id="lblSubject" runat="server" /></h2>
	<div class="dnnFormItem">
		<dnn:label id="plDeleteTemplate" runat="server" controlname="ddlDeleteTemplate" resourcekey="plDeleteTemplate" Suffix=":" />
		<asp:DropDownList id="ddlDeleteTemplate" runat="server" AutoPostBack="True" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plReason" runat="server" controlname="txtReason" Suffix=":" />
		<asp:textbox id="txtReason" runat="server" Rows="8" TextMode="MultiLine" />
		<asp:requiredfieldvalidator id="valEmailedResponse" runat="server" resourcekey="valEmailedResponse" CssClass="dnnFormMessage dnnFormError" ControlToValidate="txtReason" SetFocusOnError="true" />
	</div>
	<div class="dnnFormItem" id="divEmailUsers" runat="server">
		<dnn:label id="plEmailUser" runat="server" controlname="chkEmailUsers" Suffix=":" />
		<asp:CheckBox ID="chkEmailUsers" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plAuthor" runat="server" controlname="lblAuthor" Suffix=":" />
		<asp:label id="lblAuthor" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plBody" runat="server" controlname="lblAuthor" Suffix=":" />
		<div class="dnnLeft">
				<asp:label id="lblBody" runat="server" />
		</div>
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdDelete" runat="server" resourcekey="cmdDelete" /></li>
		<li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdCancel" runat="server" resourcekey="cmdCancel" CausesValidation="False" /></li>
	</ul>
</div>