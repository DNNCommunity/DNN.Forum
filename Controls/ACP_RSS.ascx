<%@ Control language="vb" CodeBehind="ACP_RSS.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.RSS" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="ACP-RSS">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">    
				<table id="tblRSS" cellspacing="0" cellpadding="0" width="100%" runat="server">
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plRSSFeeds" runat="server" Suffix=":" controlname="chkRSSFeeds"></dnn:label>
							</span>
						</td>
						<td valign="middle" align="left">
							<asp:checkbox id="chkRSSFeeds" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
						</td>
					</tr>
					<tr id="rowThreadsPerFeed" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plRSSThreadsPerFeed" runat="server" Suffix=":" controlname="txtRSSThreadsPerFeed"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:textbox id="txtRSSThreadsPerFeed" runat="server" cssclass="Forum_NormalTextBox" width="50px" MaxLength="3" EnableViewState="false" />
							<asp:RequiredFieldValidator ID="valreqRSSThreadsFeed" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtRSSThreadsPerFeed" EnableViewState="false" />  
							<asp:regularexpressionvalidator id="valRSSThreadsPerFeed" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtRSSThreadsPerFeed" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
					<tr id="rowTTL" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plTTL" runat="server" Suffix=":" controlname="txtTTL"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:textbox id="txtTTL" runat="server" cssclass="Forum_NormalTextBox" width="50px" EnableViewState="false" />
							<asp:RequiredFieldValidator ID="valreqTTL" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtTTL" EnableViewState="false" />
							<asp:regularexpressionvalidator id="valTTLNumeric" runat="server" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" ControlToValidate="txtTTL" CssClass="NormalRed" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
				</table>
				<div align="center">
					<asp:linkbutton cssclass="CommandButton primary-action" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" EnableViewState="false" />
				</div>
				<div align="center">
					<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
				</div>
			</td>
		</tr>
	</table>
</div>