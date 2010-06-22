<%@ Control language="vb" CodeBehind="ACP_UI.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.UI" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td class="Forum_UCP_HeaderInfo">    
			<table id="tblGeneral" cellspacing="0" cellpadding="0" width="100%" runat="server">
			     <tr>
				     <td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plThreadsPerPage" runat="server" Suffix=":" controlname="txtThreadsPerPage"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:textbox id="txtTheardsPerPage" runat="server" cssclass="Forum_NormalTextBox" width="250px" MaxLength="3" />
						<asp:RequiredFieldValidator ID="valreqThreadsPerPage" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtTheardsPerPage"  />
						<asp:regularexpressionvalidator id="valThreadsPerPage" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtTheardsPerPage" CssClass="NormalRed" Display="Dynamic"  />        
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plPostsPerPage" runat="server" Suffix=":" controlname="txtPostsPerPage"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:textbox id="txtPostsPerPage" runat="server" cssclass="Forum_NormalTextBox" width="250px" MaxLength="3" EnableViewState="false" />
						<asp:RequiredFieldValidator ID="valreqPostsPerPage" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtPostsPerPage" />
						<asp:regularexpressionvalidator id="valPostsPerPage" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtPostsPerPage" CssClass="NormalRed" Display="Dynamic" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plThreadPageCount" runat="server" Suffix=":" controlname="txtThreadPageCount"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:textbox id="txtThreadPageCount" runat="server" cssclass="Forum_NormalTextBox" width="250px" MaxLength="2" EnableViewState="false" />
						<asp:RequiredFieldValidator ID="valreqThreadPageCount" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtThreadPageCount" />
						<asp:regularexpressionvalidator id="valThreadPageCount" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtThreadPageCount" CssClass="NormalRed" Display="Dynamic" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plMaxPostImageWidth" runat="server" Suffix=":" controlname="txtMaxPostImageWidth"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:textbox id="txtMaxPostImageWidth" runat="server" cssclass="Forum_NormalTextBox" width="250px" MaxLength="4" EnableViewState="false" />
						<asp:RequiredFieldValidator ID="valreqMaxPostImageWidth" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtMaxPostImageWidth" />
						<asp:regularexpressionvalidator id="valMaxPostImageWidth" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtMaxPostImageWidth" CssClass="NormalRed" Display="Dynamic" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plSkin" runat="server" Suffix=":" controlname="ddlSkins"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:dropdownlist id="ddlSkins" runat="server" CssClass="Forum_NormalTextBox" width="250px" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plImageExtension" runat="server" ControlName="txtImageExtension" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:TextBox ID="txtImageExtension" runat="server" CssClass="Forum_NormalTextBox" Width="250px" MaxLength="3" />
						<asp:RequiredFieldValidator ID="valImageExt" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtImageExtension" />  
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plDisplayPosterLocation" runat="server" Suffix=":" controlname="chkUserCountry"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:dropdownlist id="ddlDisplayPosterLocation" tabIndex="32" runat="server" cssclass="Forum_NormalTextBox" width="250px" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plEnableIconBarAsImages" runat="server" ControlName="chkEnableIconBarAsImages" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:checkbox id="chkEnableIconBarAsImages" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plDisplayPosterRegion" runat="server" ControlName="chkDisplayPosterRegion" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:checkbox id="chkDisplayPosterRegion" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plEnableQuickReply" runat="server" ControlName="chkEnableQuickReply" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:checkbox id="chkEnableQuickReply" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
			</table>
			<div align="center">
				<asp:linkbutton cssclass="CommandButton" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" EnableViewState="false" />
			</div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
			</div>
		</td>
     </tr>
</table>