<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Profile.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Profile" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class=" dnnForm ucpProfile dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="Title" EnableViewState="false" /></h2>
	<div class="dnnFormItem">
		<dnn:label id="plUserID" runat="server" suffix=":" controlname="txtUserID" />
		<asp:TextBox ID="txtUserID" runat="server" ReadOnly="true" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plUserName" runat="server" suffix=":" controlname="lblUserName" />
		<asp:Label ID="lblUserName" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plDisplayName" runat="server" suffix=":" controlname="lblDisplayName" />
		<asp:Label ID="lblDisplayName" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plEmail" runat="server" suffix=":" controlname="chkDisplayEmail" />
		<asp:HyperLink ID="hlEmail" runat="server" />     
	</div>
	<div class="dnnFormItem" id="divTrust" runat="server">
		<dnn:label id="plIsTrusted" runat="server" suffix=":" controlname="chkIsTrusted" />
		<asp:CheckBox ID="chkIsTrusted" runat="server" />
	</div>
	<div class="dnnFormItem" id="divLockTrust" runat="server">
		<dnn:label id="plLockTrust" runat="server" suffix=":" controlname="chkLockTrust" />
		<asp:CheckBox ID="chkLockTrust" runat="server" />
	</div>
	<div class="dnnFormItem" id="divUserBanning" runat="server">
		<dnn:label id="plIsBanned" runat="server" suffix=":" controlname="chkIsBanned" />
		<asp:CheckBox ID="chkIsBanned" runat="server" AutoPostBack="true" />
	</div>
	<div class="dnnFormItem" id="divLiftBanDate" runat="server">
		<dnn:label id="plLiftBanDate" runat="server" suffix=":" controlname="txtLiftBanDate" />
		<dnnweb:DnnDatePicker id="rdpLiftBan" runat="server" />
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:LinkButton class="dnnPrimaryAction" ID="cmdUpdate" runat="server" resourcekey="cmdUpdate" EnableViewState="false" /></li>
	</ul>
</div>