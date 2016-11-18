<%@ Control language="vb" CodeBehind="ACP_Attachment.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Attachment" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm acpAttachment dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" /></h2>
	<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
	<fieldset>
		<div class="dnnFormItem" id="divAttachment" runat="server">
			<dnn:label id="plAttachment" runat="server" Suffix=":" controlname="chkAttachment" />
			<asp:checkbox id="chkAttachment" runat="server" AutoPostBack="True" />
		</div>
		<div class="dnnFormItem" id="divAnonDownloads" runat="server">
			<dnn:label id="plAnonDownloads" runat="server" Suffix=":" controlname="chkAnonDownloads" />
			<asp:checkbox id="chkAnonDownloads" runat="server" />
		</div>
		<div class="dnnFormItem" id="divAttachmentPath" runat="server">
			<dnn:label id="plAttachmentPath" runat="server" Suffix=":" controlname="txtAttachmentPath" />
			<asp:TextBox id="txtAttachmentPath" runat="server" CssClass="dnnFormRequired" />
			<asp:RequiredFieldValidator ID="valAttachmentPath" runat="server" ErrorMessage="*" ControlToValidate="txtAttachmentPath" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" SetFocusOnError="true" />
            </div>
		<div class="dnnFormItem" id="divMaxAttachmentSize" runat="server">
			<dnn:label id="plMaxAttachmentSize" runat="server" Suffix=":" controlname="txtMaxAttachmentSize" />
			<asp:TextBox id="txtMaxAttachmentSize" runat="server" MaxLength="5" CssClass="dnnFormRequired" />
			<asp:RequiredFieldValidator ID="valMaxAttachSize" runat="server" ErrorMessage="*" ControlToValidate="txtMaxAttachmentSize" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" SetFocusOnError="true" />
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" /></li>
		</ul>
	</fieldset>
</div>