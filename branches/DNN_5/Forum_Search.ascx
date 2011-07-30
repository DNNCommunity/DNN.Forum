<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Forum_Search.ascx.vb" Inherits="DotNetNuke.Modules.Forum.SearchPage" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register TagPrefix="dnnweb" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %> 
<%@ Register TagPrefix="RDNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<div class="dnnForm forumSearch dnnClear">
	<div class="dnnFormItem">
		<dnn:label id="plPostDates" Suffix=":" runat="server" />
		<div class="dnnLeft">
			<asp:label id="lblStartDate" runat="server" resourcekey="lblStartDate" />&nbsp;&nbsp;<telerik:RadDatePicker ID="rdpFrom" runat="server" Width="100px" />
			<asp:label id="lblEndDate" runat="server" resourcekey="lblEndDate" />&nbsp;&nbsp;<telerik:RadDatePicker ID="rdpTo" runat="server" Width="100px" />
		</div>
		<asp:CompareValidator ID="valCompareDates" runat="server" ControlToValidate="rdpTo" ControlToCompare="rdpFrom" Operator="GreaterThan" Type="Date" CssClass="dnnFormMessage dnnFormValidationSummary" resourcekey="valCompareDates" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plSubject" Suffix=":" runat="server" controlname="txtSubject" />
		<asp:textbox id="txtSubject"  runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plBody" Suffix=":" runat="server" controlname="txtSearch" />
		<asp:textbox id="txtSearch" runat="server" rows="8" TextMode="MultiLine" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plThreadStatus" Suffix=":" runat="server" controlname="ddlThreadStatus" />
		<telerik:RadComboBox ID="rcbThreadStatus" runat="server" Width="332px" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plForums" Suffix=":" runat="server" controlname="txtForumID" />
		<div class="dnnLeft">
			<dnnweb:DnnTreeView ID="rtvForums" runat="server" CheckBoxes="true" />
		</div>
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton class="dnnPrimaryAction" id="cmdSearch" resourcekey="cmdSearch" runat="server" /></li>
		<li><asp:HyperLink class="dnnSecondaryAction" id="cmdCancel" resourcekey="cmdCancel" runat="server" /></li>
	</ul>
</div>