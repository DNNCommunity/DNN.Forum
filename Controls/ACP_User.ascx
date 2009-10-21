<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.ACP.User" CodeBehind="ACP_User.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td class="Forum_UCP_HeaderInfo">
			<table width="100%" border="0" cellspacing="0" cellpadding="0">
				<tr>
					<td align="left" width="75" valign="middle">
						<asp:label id="lblSearch" cssclass="Forum_NormalBold" resourcekey="Search" runat="server" EnableViewState="false" />
					</td>
					<td align="left" valign="middle">
						<table>
							<tr>
								<td align="left">
									<asp:textbox id="txtSearch" Runat="server" class="Forum_NormalTextBox" Width="200px" />
									<asp:DropDownList ID="ddlRoles" CssClass="Forum_NormalTextBox" Width="200px" Visible="false" runat="server" AutoPostBack="true" />
								</td>
								<td align="left">
									<asp:dropdownlist id="ddlSearchType" Runat="server" class="Forum_NormalTextBox" Width="200px" AutoPostBack="True" />
								</td>
								<td align="left">
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="2"></td>
				</tr>
		          <tr>
					<td colspan="2" width="100%" align="center">
						<asp:Label ID="lblNoResults" runat="server" CssClass="Forum_NormalBold" resourcekey="lblNoResults" EnableViewState="false" />
		                    <asp:datagrid id="grdUsers" Runat="server" GridLines="None" width="100%" cssclass="Forum_Grid"
			                        CellPadding="0" cellspacing="0" AutoGenerateColumns="false" EnableViewState="false" >
							<HeaderStyle CssClass="Forum_Grid_Header" />
                                   <ItemStyle CssClass="Forum_Grid_Row_Alt" />
                                   <AlternatingItemStyle CssClass="Forum_Grid_Row" />
							<Columns>
								<asp:TemplateColumn>
									<ItemTemplate>
										<asp:HyperLink id="hlEditUser" runat="server" EnableViewState="false" >
											<asp:Image id="imgEdit" runat="Server" EnableViewState="false" />
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:templatecolumn>
									<itemtemplate>
										<asp:image id="imgOnline" runat="Server" EnableViewState="false" />
									</itemtemplate>
								</asp:templatecolumn>
								<asp:BoundColumn DataField="UserName" headertext="Username" ItemStyle-CssClass="Forum_Grid_Left" HeaderStyle-HorizontalAlign="Left" />
								<asp:BoundColumn DataField="DisplayName" headertext="DisplayName" ItemStyle-CssClass="Forum_Grid_Middle" />
								<asp:TemplateColumn HeaderText="Email" ItemStyle-CssClass="Forum_Grid_Right" ItemStyle-Width="25%">
									<ItemTemplate>
										<asp:HyperLink id="hlEmail" runat="server" CssClass="Forum_Profile" EnableViewState="false" />
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
		                    </asp:datagrid>
                              <dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
		               </td>
		          </tr>
		     </table>
		</td>
     </tr>
</table>