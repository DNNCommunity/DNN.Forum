<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Forum_ContentRemoved.ascx.vb" Inherits="DotNetNuke.Modules.Forum.ContentRemoved" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<table class="Forum_Container" id="tblMain" cellspacing="0" cellpadding="0" width="100%"
	align="center">
	<tr>
		<td id="celHeader" width="100%" class="Forum_Header" valign="middle">
			<table cellspacing="0" cellpadding="0" border="0" width="100%">
				<tr>
					<td width="1" class="Forum_HeaderCapLeft"><asp:image id="imgHeadSpacer" runat="server" /></td>
					<td width="100%">&nbsp;<asp:Label ID="lblTitleContentRemoved" Runat="server" resourcekey="lblTitleContentRemoved"
							Text="Content Removed" CssClass="Forum_HeaderText"></asp:Label></td>
					<td width="1" class="Forum_HeaderCapRight"><asp:image id="imgHeadSpacer2" runat="server" /></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td valign="top" align="center" width="100%" class="Forum_Row_Admin">
			<asp:Label id="lblContentRemoved" runat="server" resourcekey="lblContentRemoved" CssClass="Forum_Normal"></asp:Label>
		</td>
	</tr>
	<tr>
		<td class="Forum_Footer" valign="middle" align="center">
			<table cellpadding="0" cellspacing="0" border="0" width="100%">
				<tr>
					<td width="1" class="Forum_FooterCapLeft"><asp:image id="imgFootSpacer" runat="server" /></td>
					<td width="100%" align="center">
						<asp:linkbutton class="CommandButton" id="cmdCancel" runat="server" resourcekey="cmdCancel"></asp:linkbutton></td>
					<td width="1" class="Forum_FooterCapRight"><asp:image id="imgFootSpacer2" runat="server" /></td>
				</tr>
			</table>
		</td>
	</tr>
</table>