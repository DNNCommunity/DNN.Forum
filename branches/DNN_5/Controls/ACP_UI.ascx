<%@ Control language="vb" CodeBehind="ACP_UI.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.UI" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm acpUI dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<div class="dnnFormItem dnnFormHelp dnnClear"><p class="dnnFormRequired"><span><%=LocalizeString("RequiredFields")%></span></p></div>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plThreadsPerPage" runat="server" Suffix=":" controlname="txtThreadsPerPage" />
			<asp:textbox id="txtTheardsPerPage" runat="server" cssclass="dnnFormRequired" MaxLength="3" />
			<asp:RequiredFieldValidator ID="valreqThreadsPerPage" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtTheardsPerPage" SetFocusOnError="true" />
			<asp:regularexpressionvalidator id="valThreadsPerPage" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtTheardsPerPage" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" SetFocusOnError="true" />   
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plPostsPerPage" runat="server" Suffix=":" controlname="txtPostsPerPage" />
			<asp:textbox id="txtPostsPerPage" runat="server" cssclass="dnnFormRequired" MaxLength="3" />
			<asp:RequiredFieldValidator ID="valreqPostsPerPage" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtPostsPerPage" SetFocusOnError="true" />
			<asp:regularexpressionvalidator id="valPostsPerPage" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtPostsPerPage" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plThreadPageCount" runat="server" Suffix=":" controlname="txtThreadPageCount" />
			<asp:textbox id="txtThreadPageCount" runat="server" cssclass="dnnFormRequired" MaxLength="2" EnableViewState="false" />
			<asp:RequiredFieldValidator ID="valreqThreadPageCount" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtThreadPageCount" SetFocusOnError="true" />
			<asp:regularexpressionvalidator id="valThreadPageCount" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtThreadPageCount" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem" id="divPostImageWidth" runat="server" visible="false">
			<dnn:label id="plMaxPostImageWidth" runat="server" Suffix=":" controlname="txtMaxPostImageWidth" />
			<asp:textbox id="txtMaxPostImageWidth" runat="server" cssclass="dnnFormRequired" MaxLength="4" EnableViewState="false" />
			<asp:RequiredFieldValidator ID="valreqMaxPostImageWidth" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtMaxPostImageWidth" SetFocusOnError="true" />
			<asp:regularexpressionvalidator id="valMaxPostImageWidth" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtMaxPostImageWidth" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plSkin" runat="server" Suffix=":" controlname="ddlSkins" />
			<asp:DropDownList ID="rcbSkins" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plImageExtension" runat="server" ControlName="txtImageExtension" Suffix=":" />
			<asp:TextBox ID="txtImageExtension" runat="server" CssClass="dnnFormRequired" MaxLength="3" />
			<asp:RequiredFieldValidator ID="valImageExt" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtImageExtension" SetFocusOnError="true" />  
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plDisplayPosterLocation" runat="server" Suffix=":" controlname="chkUserCountry" />
			<asp:DropDownList ID="rcbDisplayPosterLocation" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plDisplayPosterRegion" runat="server" ControlName="chkDisplayPosterRegion" Suffix=":" />
			<asp:checkbox id="chkDisplayPosterRegion" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plEnableQuickReply" runat="server" ControlName="chkEnableQuickReply" Suffix=":" />
			<asp:checkbox id="chkEnableQuickReply" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plEnableTagging" runat="server" ControlName="chkEnableQuickReply" Suffix=":" />
			<asp:checkbox id="chkEnableTagging" runat="server" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
	</ul>
</div>