<%@ Control language="vb" CodeBehind="ACP_SEO.ascx.vb" Explicit="true" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.SEO" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm acpSEO dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plNoFollowWeb" runat="server" Suffix=":" controlname="chkNoFollowWeb" />
			<asp:checkbox id="chkNoFollowWeb" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plOverrideTitle" runat="server" Suffix=":" controlname="chkOverrideTitle" />
			<asp:checkbox id="chkOverrideTitle" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plOverrideDescription" runat="server" Suffix=":" controlname="chkOverrideDescription" />
			<asp:checkbox id="chkOverrideDescription" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plOverrideKeyWords" runat="server" Suffix=":" controlname="chkOverrideKeyWords" />
			<asp:checkbox id="chkOverrideKeyWords" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plNoFollowLatestThreads" runat="server" Suffix=":" controlname="chkNoFollowLatestThreads" />
			<asp:checkbox id="chkNoFollowLatestThreads" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plSitemapPriority" runat="server" Suffix=":" controlname="txtSitemapPriority" />
			<dnnweb:DnnNumericTextBox ID="textSitemapPriority" runat="server" MinValue="0" MaxValue="1" />
		</div>
		<ul class="dnnActions dnnClear">
			<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
		</ul>
	</fieldset>
</div>