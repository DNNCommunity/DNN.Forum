<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Signature.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Signature" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm ucpSignature dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="Title" EnableViewState="false" /></h2>
	<div class="dnnFormItem">
		<dnn:label id="plSignature" runat="server" suffix=":" controlname="txtSignature" />
		<asp:TextBox ID="txtSignature" runat="server" CssClass="Forum_NormalTextBox" Width="350px" TextMode="MultiLine" Rows="8" />
		<asp:Label ID="lblSignature" runat="server" CssClass="Forum_NormalTextBox" Width="300px" Visible="False" />
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton class="dnnPrimaryAction" ID="cmdUpdate" runat="server" resourcekey="Update" EnableViewState="false" /></li>
		<li><asp:LinkButton ID="cmdPreview" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdPreview" /></li>
		<li><asp:LinkButton ID="cmdEdit" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdEdit" Visible="false" /></li>
	</ul>
</div>