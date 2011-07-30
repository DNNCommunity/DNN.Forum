<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_Rating.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.Rating" %>
<div class="dnnForm acpRating dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plRatings" runat="server" Suffix=":" controlname="chkRatings" />
			<asp:checkbox id="chkRatings" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plRatingScale" runat="server" ControlName="txtRatingScale" Suffix=":" />
			<asp:TextBox ID="txtRatingScale" runat="server" CssClass="dnnFormRequired" MaxLength="50" />
			<asp:RequiredFieldValidator ID="valRatingReq" runat="server" ErrorMessage="*" ControlToValidate="txtRatingScale" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
	</ul>
</div>