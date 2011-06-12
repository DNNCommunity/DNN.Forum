<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Signature.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Signature" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<div class="UCP-Signature">
	<table cellpadding="0" cellspacing="0" border="0" width="100%">
		<tr>
			<td class="Forum_UCP_Header" colspan="2">
				<asp:Label id="lblTitle" runat="server" resourcekey="Title" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plSignature" runat="server" suffix=":" controlname="txtSignature"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:TextBox ID="txtSignature" runat="server" CssClass="Forum_NormalTextBox" Width="300px" TextMode="MultiLine" Rows="4" />
							<asp:Label ID="lblSignature" runat="server" CssClass="Forum_NormalTextBox" Width="300px" Visible="False" />
							<asp:LinkButton ID="cmdPreview" runat="server" CssClass="CommandButton" resourcekey="cmdPreview" />
							<asp:LinkButton ID="cmdEdit" runat="server" CssClass="CommandButton" resourcekey="cmdEdit" Visible="false" />
						</td>
					</tr>
				</table>
				<div align="center">
					<asp:LinkButton class="CommandButton primary-action" ID="cmdUpdate" runat="server" resourcekey="Update" EnableViewState="false" />
				</div>
				<div align="center">
					<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
				</div>
			</td>
		</tr>
	</table>
</div>