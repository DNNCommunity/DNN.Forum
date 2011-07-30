<%@ Control language="vb" CodeBehind="ACP_General.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.General" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm acpGeneral dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" /></h2>
	<div class="dnnFormItem" id="divPrimaryAlias" runat="server" visible="false">
		<dnn:label id="plPrimaryAlias" runat="server" Suffix=":" controlname="ddlPrimaryAlias" />
		<asp:dropdownlist id="ddlPrimaryAlias" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plAggregatedForums" runat="server" Suffix=":" controlname="chkAggregatedForums" />
		<asp:checkbox id="chkAggregatedForums" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plEnableThreadStatus" runat="server" Suffix=":" controlname="chkEnableThreadStatus" />
		<asp:checkbox id="chkEnableThreadStatus" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plEnablePostAbuse" runat="server" Suffix=":" controlname="chkEnablePostAbuse" />
		<asp:checkbox id="chkEnablePostAbuse" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:Label ID="plDisableHTMLPosting" runat="server" ControlName="chkDisableHTMLPosting" Suffix=":" />
		<asp:CheckBox ID="chkDisableHTMLPosting" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plSearchIndexDate" runat="server" Suffix=":" controlname="chkShowNavigator" />
		<asp:Label id="lblDateIndexed" runat="server" />&nbsp;
		<asp:linkbutton id="cmdResetDate" runat="server" resourcekey="cmdResetDate" EnableViewState="false" CausesValidation="false" />
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
	</ul>
</div>