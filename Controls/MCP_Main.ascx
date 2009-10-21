<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_Main.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.Main" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
	</tr>
      <tr>
          <td class="Forum_UCP_HeaderInfo">
              <asp:Label ID="lblInfo" runat="server" CssClass="Forum_Normal" resourcekey="lblInfo" EnableViewState="false" />
          </td>
      </tr>
</table>      