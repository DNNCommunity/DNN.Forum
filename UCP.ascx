<%@ Control Language="vb" AutoEventWireup="false" Codebehind="UCP.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCPLoader" %>
<%@ Register src="~/DesktopModules/Forum/Controls/UCP_Menu.ascx" tagname="UCPmenu" tagprefix="forum" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<asp:Panel ID="pnlContainer" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0">
        <tr valign="top">
            <td class="Forum_UCP_Left"><forum:UCPmenu ID="UCPmenu" runat="server" /></td>
            <td class="Forum_UCP_Right"><asp:PlaceHolder ID="phUserControl" runat="server" /></td>
        </tr>
        <tr>
		<td align="center" colspan="2"><asp:LinkButton ID="cmdHome" runat="server" CssClass="CommandButton" resourcekey="cmdHome"></asp:LinkButton></td>
        </tr>
    </table>
</asp:Panel>          