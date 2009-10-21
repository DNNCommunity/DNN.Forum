<%@ Control language="vb" CodeBehind="Forum_PostReport.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.PostReport" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<asp:Panel ID="pnlContainer" runat="server">
	<table class="" id="tblMain" cellspacing="0" cellpadding="0" width="100%"
		align="center">
		<tr>
			<td align="left" colspan="2">
				<table cellspacing="0" cellpadding="0" border="0" width="100%">
					<tr>
						<td class="Forum_HeaderCapLeft"><asp:Image ID="imgHeadSpacerL" runat="server" /></td>
						<td class="Forum_Header" width="100%"><asp:label id="lblSubject" runat="server" CssClass="Forum_HeaderText" /></td>
						<td class="Forum_HeaderCapRight"><asp:Image ID="imgHeadSpacerR" runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_Admin" align="left" colspan="2">
				<asp:label id="lblAuthor" runat="server" CssClass="Forum_Normal" />
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_Admin" align="left" colspan="2">
				<asp:label id="lblMessage" runat="server" CssClass="Forum_Normal" />
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_AdminL" valign="top">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plDeleteTemplate" runat="server" controlname="ddlDeleteTemplate" resourcekey="plDeleteTemplate" suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR" align="left">
				<asp:DropDownList id="ddlReportTemplate" runat="server" Width="250px" AutoPostBack="True" CssClass="Forum_NormalTextBox" />
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_AdminL" valign="top">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plReason" runat="server" controlname="txtReason" suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR" align="left">
				<asp:textbox id="txtReason" runat="server" Width="350px" Height="150px" TextMode="MultiLine" CssClass="Forum_NormalTextBox" />
				<asp:requiredfieldvalidator id="valEmailedResponse" runat="server" resourcekey="valEmailedResponse" CssClass="NormalRed" ControlToValidate="txtReason" />
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_Admin_Foot" align="center" colspan="2">
				<asp:linkbutton cssclass="CommandButton" id="cmdCancel" runat="server" resourcekey="cmdCancel" CausesValidation="False" />&nbsp;
				<asp:linkbutton cssclass="CommandButton" id="cmdReport" runat="server" resourcekey="cmdReport" />
			</td>
		</tr>
				<tr>
			<td align="center" colspan="2">
				<asp:Label ID="lblAlreadyReported" runat="server" CssClass="NormalRed" Visible="False" resourcekey="lblAlreadyReported"></asp:Label>
			</td>
		</tr>
	</table>
</asp:Panel>