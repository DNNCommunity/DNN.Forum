<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_PopStatus.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.PopStatus" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm acpPopStatus dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:Label ID="plPopularPostsView" runat="server" ControlName="txtPopularThreadView" Suffix=":" />
			<dnnweb:DnnNumericTextBox ID="rntxtbxPopularThreadView" runat="server" MaxLength="4" NumberFormat-DecimalDigits="0" MinValue="1" MaxValue="1001" CssClass="dnnFormRequired"/>
			<asp:RequiredFieldValidator ID="valreqView" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="rntxtbxPopularThreadView" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plPopularPostsReply" runat="server" ControlName="txtPopularThreadReply" Suffix=":" />
			<dnnweb:DnnNumericTextBox ID="rntxtbxPopularThreadReply" runat="server" MaxLength="3" NumberFormat-DecimalDigits="0" MinValue="1" MaxValue="101" CssClass="dnnFormRequired" />
			<asp:RequiredFieldValidator ID="valreqReply" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="rntxtbxPopularThreadReply" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plDays" runat="server" ControlName="txtDays" Suffix=":" />
			<dnnweb:DnnNumericTextBox ID="rntxtbxDays" runat="server" MaxLength="3" NumberFormat-DecimalDigits="0" MinValue="1" MaxValue="366" CssClass="dnnFormRequired" />
			<asp:RequiredFieldValidator ID="valreqDays" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="rntxtbxDays" EnableViewState="false" SetFocusOnError="true" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
	</ul>
</div>