<%@ Control language="vb" CodeBehind="Forum_PostDelete.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.PostDelete" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="Post-Delete">
	<table class="Forum_SearchContainer" id="tblMain" cellspacing="0" cellpadding="0" width="100%" align="center">
		<tr>
			<td class="Forum_Row_AdminL" valign="top">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plDeleteTemplate" runat="server" controlname="ddlDeleteTemplate" resourcekey="plDeleteTemplate" Suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR" align="left">
				<asp:DropDownList id="ddlDeleteTemplate" runat="server" Width="300px" AutoPostBack="True" CssClass="Forum_NormalTextBox" />
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_AdminL" valign="top">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plReason" runat="server" controlname="txtReason" Suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR" align="left">
				<asp:textbox id="txtReason" runat="server" Width="300px" Height="150px" 
					TextMode="MultiLine" />
				<asp:requiredfieldvalidator id="valEmailedResponse" runat="server" resourcekey="valEmailedResponse" CssClass="NormalRed" ControlToValidate="txtReason" />
			</td>
		</tr>
		<tr id="rowEmailUsers" runat="server">
			<td class="Forum_Row_AdminL" valign="top">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plEmailUser" runat="server" controlname="chkEmailUsers" Suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR">
				<asp:CheckBox ID="chkEmailUsers" runat="server" CssClass="Forum_NormalTextBox" />
			</td>
		</tr>
		<tr>
			<td width="100%" colspan="2">
				<table cellspacing="0" cellpadding="0" border="0" width="100%">
					<tr>
						<td class="Forum_HeaderCapLeft">
							<asp:Image ID="imgSpacerL" runat="server" />
						</td>
						<td class="Forum_Header" width="100%">
							<asp:label id="lblSubject" runat="server" CssClass="Forum_HeaderText" />
						</td>
						<td class="Forum_HeaderCapRight">
							<asp:Image ID="imgSpacerR" runat="server" />
						</td>
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
			<td class="Forum_Row_Admin_Foot" align="center" colspan="2">
				<asp:linkbutton cssclass="CommandButton primary-action" id="cmdDelete" runat="server" resourcekey="cmdDelete" />&nbsp;
				<asp:linkbutton cssclass="CommandButton" id="cmdCancel" runat="server" resourcekey="cmdCancel" CausesValidation="False" />
			</td>
		</tr>
	</table>
</div>