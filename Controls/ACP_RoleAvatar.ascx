<%@ Control Language="vb" Codebehind="ACP_RoleAvatar.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.RoleAvatar" %>
<%@ Register TagPrefix="forum" TagName="AvatarControl" Src="~/DesktopModules/Forum/Controls/AvatarControl.ascx" %>
<%@ Register TagPrefix="forum" TagName="ACPmenu" src="~/DesktopModules/Forum/Controls/ACP_Menu.ascx" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<asp:Panel ID="pnlContainer" runat="server" />
	<table cellpadding="0" cellspacing="0" width="100%" border="0" class="Forum_SearchContainer">
		<tr valign="top">
			<td class="Forum_UCP_Left"><forum:ACPmenu ID="ACPmenu" runat="server" /></td>
			<td class="Forum_UCP_Right">
					<table cellspacing="0" cellpadding="0" width="100%">
				 <tr>
				    <td>
					    <asp:DataList id="dlRoleAvatar" cellspacing="0" cellpadding="0" gridlines="None" datakeyfield="RoleID" width="100%" runat="server" RepeatDirection="Vertical" RepeatLayout="Table">
						   <HeaderStyle CssClass="Forum_UCP_Header" />
						   <FooterStyle CssClass="Forum_Row_Admin_Foot" />
						   <HeaderTemplate>
							  <table cellpadding="0" cellspacing="0" width="100%">
								 <tr>
									<td width="150px" class="Forum_HeaderText"><asp:label ID="lblRoleName" runat="server" resourcekey="RoleName" /></td>
									<td class="Forum_HeaderText"><asp:label ID="lblAvatar" runat="server" resourcekey="Avatar" /></td>
								  </tr>
							  </table>
						   </HeaderTemplate>
						   <ItemTemplate>
							  <table cellpadding="0" cellspacing="0" width="100%">
								 <tr valign="top">
									<td width="150px" class="Forum_Row_AdminL"><asp:Label ID="lblRoleName" runat="server" /></td>
									<td class="Forum_Row_AdminL"><asp:Label ID="lblAvatar" runat="server" /></td>
									<td class="Forum_Row_AdminR" align="right">
									    <asp:ImageButton id="imgDelete" resourcekey="Delete" runat="server" CommandName="delete" />
											<asp:ImageButton id="imgEdit" resourcekey="Edit" runat="server" />
											<asp:ImageButton id="imgAdd" resourcekey="Add" runat="server" />
									</td>
								 </tr>
							  </table>
						   </ItemTemplate>
						   <EditItemTemplate>
							  <table cellpadding="0" cellspacing="0" width="100%">
								 <tr valign="top">
									<td width="150px" class="Forum_Row_AdminL"><asp:Label ID="lblRoleName" runat="server" /></td>
									<td class="Forum_Row_AdminL"><forum:avatarcontrol id="ctlRoleAvatar" runat="server"></forum:avatarcontrol></td>
									<td class="Forum_Row_AdminR" align="right">
									    <asp:ImageButton id="imgUpdate" resourcekey="Update" runat="server" CommandName="delete" />
											<asp:ImageButton id="imgCancel" resourcekey="Cancel" runat="server" />
									</td>
								 </tr>
							  </table>
						   </EditItemTemplate>
						   <FooterTemplate>
							  &nbsp;
						   </FooterTemplate>
					    </asp:DataList>
				    </td>
				 </tr>
				 <tr>
					<td align="center"><asp:LinkButton ID="cmdHome" runat="server" CssClass="CommandButton" resourcekey="cmdHome"></asp:LinkButton></td>
				 </tr>
			  </table>
			</td>
		</tr>
	</table>
</asp:Panel>