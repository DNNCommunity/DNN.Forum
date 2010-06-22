<%@ Control language="vb" CodeBehind="ACP_UserSettings.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.UserSettings" %>
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
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plNameDisplay" runat="server" ControlName="chkUserCountry" Suffix=":" ></dnn:Label>
						</span>
					</td>
					<td align="left" >
						<asp:dropdownlist id="ddlNameDisplay" runat="server" cssclass="Forum_NormalTextBox" width="250px" />
					</td>
				</tr>
				<tr id="rowEditWindow" runat="server">
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plPostEditWindow" runat="server" Suffix=":" controlname="txtEditWindow"></dnn:label>
						</span>
					</td>
					<td  valign="middle" align="left">
						<asp:TextBox id="txtPostEditWindow" runat="server" CssClass="Forum_NormalTextBox" />
						<asp:RangeValidator ID="valPostEditWindow" runat="server" CssClass="NormalRed" ControlToValidate="txtPostEditWindow" Display="Dynamic" resourcekey="PostEditWindow.ErrorMessage" MaximumValue="60" MinimumValue="0" Type="Integer" />
					</td>
				</tr>
				<tr>
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plTrustNewUsers" runat="server" ControlName="chkTrustNewUsers" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left" >
						<asp:CheckBox ID="chkTrustNewUsers" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr>
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plAutoLockTrust" runat="server" ControlName="chkAutoLockTrust" Suffix=":" ></dnn:Label>
						</span>
					</td>
					<td align="left" >
						<asp:CheckBox ID="chkAutoLockTrust" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr>
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEnableUserSignatures" runat="server" controlname="chkEnableUserSignatures" Suffix=":"></dnn:label>
						</span>
					</td>
					<td  valign="middle" align="left">
						<asp:checkbox id="chkEnableUserSignatures" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="True" />
					</td>
				</tr>
				<tr id="rowModSigUpdates" runat="server">
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEnableModSigUpdates" runat="server" Suffix=":" controlname="chkEnableModSigUpdates"></dnn:label>
						</span>
					</td>
					<td  valign="middle" align="left">
						<asp:checkbox id="chkEnableModSigUpdates" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowHTMLSignatures" runat="server">
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEnableHTMLSignatures" runat="server" Suffix=":" controlname="chkEnableHTMLSignatures"></dnn:label>
						</span>
					</td>
					<td  valign="middle" align="left">
						<asp:checkbox id="chkEnableHTMLSignatures" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowHideModEdit" runat="server" visible="False">
					<td class="style1" width="35%">
						<dnn:label id="plHideModEdit" runat="server" Suffix=":" controlname="chkHideModEdit"></dnn:label>
					</td>
					<td align="left" valign="middle">
						<asp:checkbox id="chkHideModEdit" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowUserBanning" runat="server">
					<td  width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEnableUserBanning" runat="server" Suffix=":" controlname="chkEnableUserBanning"></dnn:label>
						</span>
					</td>
					<td  valign="middle" align="left">
						<asp:checkbox id="chkEnableUserBanning" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
			</table>
			<div align="center">
				<asp:linkbutton cssclass="CommandButton" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" />
			</div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" />
			</div>
		</td>
     </tr>
</table>