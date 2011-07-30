<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UCP.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCPLoader" %>
<%@ Register src="~/DesktopModules/Forum/Controls/UCP_Menu.ascx" tagname="UCPmenu" tagprefix="forum" %>
<div class="UCP">
	<table cellpadding="0" cellspacing="0" width="100%" border="0" class="Forum_SearchContainer" >
		<tr valign="top">
			<td class="Forum_UCP_Left"><forum:UCPmenu ID="UCPmenu" runat="server" /></td>
			<td class="Forum_UCP_Right"><asp:PlaceHolder ID="phUserControl" runat="server" /></td>
		</tr>
		<tr>
		<td align="center" colspan="2"><asp:HyperLink ID="cmdHome" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdHome" /></td>
		</tr>
	</table>
</div>   