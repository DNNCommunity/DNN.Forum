<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_Ranking.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.Ranking" %>
<div class="dnnForm acpRanking dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblRankTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plRankings" runat="server" Suffix=":" controlname="chkRatings" />
			<asp:checkbox id="chkRankings" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plEnableRankingImage" runat="server" Suffix=":" controlname="chkEnableRankingImage" />
			<asp:checkbox id="chkEnableRankingImage" runat="server" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plFirstRank" runat="server" Suffix=":" controlname="txtFisrtRank" />
			<asp:textbox id="txtFirstRank" runat="server" cssclass="dnnFormRequired" width="50px" MaxLength="10" />
			<asp:RequiredFieldValidator ID="valFirst" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtFirstRank" EnableViewState="false" /> 
			<asp:regularexpressionvalidator id="val1st" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtFirstRank" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt1stTitle" runat="server" MaxLength="25" Width="100px" Visible="false" />
			<asp:Image ID="img1stRank" runat="server" EnableViewState="false" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plSecondRank" runat="server" Suffix=":" controlname="txtSecondRank" />
			<asp:textbox id="txtSecondRank" runat="server" cssclass="dnnFormRequired" width="50px" MaxLength="10" />
			<asp:RequiredFieldValidator ID="val2ndRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtSecondRank" EnableViewState="false" />
			<asp:regularexpressionvalidator id="val2nd" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtSecondRank" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt2ndTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img2ndRank" runat="server" EnableViewState="false" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plThirdRank" runat="server" Suffix=":" controlname="txtThirdRank" />
			<asp:textbox id="txtThirdRank" runat="server" cssclass="dnnFormRequired" width="50px" MaxLength="10" />
			<asp:RequiredFieldValidator ID="val3rdRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtThirdRank" EnableViewState="false" />
			<asp:regularexpressionvalidator id="val3rd" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtThirdRank" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt3rdTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img3rdRank" runat="server" EnableViewState="false" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plFourthRank" runat="server" Suffix=":" controlname="txtFourthRank" />
			<asp:textbox id="txtFourthRank" runat="server" cssclass="dnnFormRequired" width="50px" MaxLength="10" />
			<asp:RequiredFieldValidator ID="val4thRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtFourthRank" EnableViewState="false" />
			<asp:regularexpressionvalidator id="val4th" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtFourthRank" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt4thTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img4thRank" runat="server" EnableViewState="false" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plFifthRank" runat="server" Suffix=":" controlname="txtFifthRank" />
			<asp:textbox id="txtFifthRank" runat="server" cssclass="dnnFormRequired" width="50px" MaxLength="10" />
			<asp:RequiredFieldValidator ID="val5thRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtFifthRank" EnableViewState="false" />
			<asp:regularexpressionvalidator id="val5th" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtFifthRank" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt5thTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img5thRank" runat="server" EnableViewState="false" />           
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plSixthRank" runat="server" ControlName="txtSixthRank" Suffix=":" />
			<asp:TextBox ID="txtSixthRank" runat="server" CssClass="dnnFormRequired" Width="50px" MaxLength="10" />
			<asp:RequiredFieldValidator ID="val6thRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtSixthRank" EnableViewState="false" />
			<asp:RegularExpressionValidator ID="val6th" runat="server" ControlToValidate="txtSixthRank" CssClass="dnnFormMessage dnnFormError" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt6thTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img6thRank" runat="server" EnableViewState="false" />  
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plSeventhRank" runat="server" ControlName="txtSeventhRank" Suffix=":" />
			<asp:TextBox ID="txtSeventhRank" runat="server" CssClass="dnnFormRequired" Width="50px" MaxLength="10" />
			<asp:RequiredFieldValidator ID="val7thRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtSeventhRank" EnableViewState="false" />
			<asp:RegularExpressionValidator ID="val7th" runat="server" ControlToValidate="txtSeventhRank" CssClass="dnnFormMessage dnnFormError" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt7thTitle" runat="server"  Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img7thRank" runat="server" EnableViewState="false" />   
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plEigthRank" runat="server" ControlName="txtEigthRank" Suffix=":" />
			<asp:TextBox ID="txtEigthRank" runat="server" CssClass="dnnFormRequired" Width="50px" MaxLength="10" EnableViewState="false" />
			<asp:RequiredFieldValidator ID="val8thRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtEigthRank" EnableViewState="false" />
			<asp:RegularExpressionValidator ID="val8th" runat="server" ControlToValidate="txtEigthRank" CssClass="dnnFormMessage dnnFormError" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt8thTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img8thRank" runat="server" EnableViewState="false" />        
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plNinthRank" runat="server" ControlName="txtNinthRank" Suffix=":" />
			<asp:TextBox ID="txtNinthRank" runat="server" CssClass="dnnFormRequired" Width="50px" MaxLength="10" EnableViewState="false" />
			<asp:RequiredFieldValidator ID="val9thRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtNinthRank" EnableViewState="false" />
			<asp:RegularExpressionValidator ID="val9th" runat="server" ControlToValidate="txtNinthRank" CssClass="dnnFormMessage dnnFormError" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />
			<asp:TextBox ID="txt9thTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img9thRank" runat="server" EnableViewState="false" />   
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plTenthRank" runat="server" ControlName="txtTenthRank" Suffix=":" />
			<asp:TextBox ID="txtTenthRank" runat="server" Width="50px" MaxLength="10" CssClass="dnnFormRequired" />
			<asp:TextBox ID="txt10thTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="img10thRank" runat="server" EnableViewState="false" />
			<asp:RequiredFieldValidator ID="val10thRank" runat="server" ErrorMessage="*" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" ControlToValidate="txtTenthRank" EnableViewState="false" SetFocusOnError="true" />
			<asp:RegularExpressionValidator ID="val10th" runat="server" ControlToValidate="txtTenthRank" CssClass="dnnFormMessage dnnFormError" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" SetFocusOnError="true" />
		</div>
		<div class="dnnFormItem">
			<dnn:Label ID="plNoRanking" runat="server" ControlName="txtNoRanking" Suffix=":" />
			<asp:TextBox ID="txtNoRanking" runat="server" Width="50px" Enabled="False" />
			<asp:TextBox ID="txtNoRankTitle" runat="server" Width="100px" MaxLength="25" Visible="false" />
			<asp:Image ID="imgNoRank" runat="server" EnableViewState="false" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" /></li>
	</ul>
</div>