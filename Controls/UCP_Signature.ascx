<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Signature.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Signature" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td class="Forum_UCP_Header" colspan="2">
			<asp:Label id="lblTitle" runat="server" resourcekey="Title" EnableViewState="false" />
		</td>
	</tr>
	<tr>
		<td class="Forum_Row_AdminL" valign="top" width="175">
			<span class="Forum_Row_AdminText">
				<dnn:label id="plSignature" runat="server" suffix=":" controlname="txtSignature"></dnn:label>
			</span>
		</td>
		<td class="Forum_Row_AdminR" valign="top" align="left">
			<asp:TextBox ID="txtSignature" runat="server" CssClass="Forum_NormalTextBox" Width="300px" TextMode="MultiLine" Rows="4" />
			<asp:Label ID="lblSignature" runat="server" CssClass="Forum_NormalTextBox" Width="300px" Visible="False" />&nbsp;
			<asp:ImageButton ID="btnPreview" runat="server" CommandName="preview" resourcekey="Preview" />
			<asp:ImageButton ID="btnEdit" runat="server" resourcekey="Edit" Visible="False" CommandName="edit" />
		</td>
	</tr>
</table>
<div class="Forum_Row_Admin_Foot" align="center">
	<asp:LinkButton class="CommandButton" ID="cmdUpdate" runat="server" resourcekey="Update" EnableViewState="false" />
</div>
<div align="center">
	<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
</div>        