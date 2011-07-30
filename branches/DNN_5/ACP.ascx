<%@ Control Inherits="DotNetNuke.Modules.Forum.ACPLoader" CodeBehind="ACP.ascx.vb" language="vb" AutoEventWireup="false" %>
<%@ Register src="~/DesktopModules/Forum/Controls/ACP_Menu.ascx" tagname="ACPmenu" tagprefix="forum" %>
<div class="ACP">
	<table cellpadding="0" cellspacing="0" width="100%" border="0" class="Forum_SearchContainer">
		<tr valign="top">
			<td class="Forum_UCP_Left"><forum:ACPmenu ID="ACPmenu" runat="server" /></td>
			<td class="Forum_UCP_Right"><asp:PlaceHolder ID="phUserControl" runat="server" /></td>
		</tr>
		<tr>
		<td align="center" colspan="2">
			<asp:HyperLink ID="cmdHome" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdHome" />
		</td>
		</tr>
	</table>
</div>