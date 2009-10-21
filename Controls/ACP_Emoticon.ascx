<%@ Control language="vb" CodeBehind="ACP_Emoticon.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Emoticon" %>
<%@ Register TagPrefix="forum" TagName="Emoticon" Src="~/DesktopModules/Forum/controls/EmoticonControl.ascx" %>
<%@ Register TagPrefix="forum" TagName="ACPmenu" src="~/DesktopModules/Forum/Controls/ACP_Menu.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<asp:Panel ID="pnlContainer" runat="server">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr valign="top">
			<td class="Forum_UCP_Left">
				<forum:ACPmenu ID="ACPmenu" runat="server" />
			</td>
			<td class="Forum_UCP_Right">
				<table cellpadding="0" cellspacing="0" width="100%" border="0">
					<tr>
						<td class="Forum_UCP_Header">
							<asp:Label id="lblTitle" runat="server" resourcekey="Title" />
						</td>
					</tr>
					<tr>
						<td class="Forum_UCP_HeaderInfo">
							<table cellpadding="0" cellspacing="0" border="0" width="100%">
								<tr>
									<td>
										<table>
											<tr>
												<td rowspan="2"><forum:Emoticon id="ctlEmoticon" runat="server" DisplayType="Admin" /></td>
												<td><asp:Label ID="lblCode" runat="server" CssClass="Forum_NormalBold" resourcekey="lblCode" /></td>
												<td><asp:TextBox ID="txtCode" runat="server" Width="100px" />
												<asp:RequiredFieldValidator ID="valCode" ControlToValidate="txtCode" CssClass="NormalRed" runat="server" Text="*" /></td>
												<td rowspan="2" valign="top"><asp:ImageButton ID="imgAdd" runat="server" resourcekey="Add" /></td>
											</tr>
											<tr>
												<td>
													<asp:Label ID="lblTooltip" runat="server" CssClass="Forum_NormalBold" resourcekey="lblTooltip" />
												</td>
												<td>
													<asp:TextBox ID="txtToolTip" runat="server" Width="100px" />
												</td>
											</tr>
										</table>						
									</td>
								</tr>
								<tr>
									<td>
										<asp:DataGrid ID="dgEmoticon" GridLines="None" CellPadding="0" CellSpacing="0" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyField="ID" CssClass="Forum_Grid" >
											<HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
											<ItemStyle CssClass="Forum_Grid_Row_Alt" />
											<AlternatingItemStyle CssClass="Forum_Grid_Row" />
											<Columns>
												<asp:BoundColumn DataField="ID" Visible="false" />
												<asp:TemplateColumn HeaderText="Image" ItemStyle-CssClass="Forum_Grid_Left" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
													<ItemTemplate>
														<asp:Literal ID="litEmoticon" runat="server" />
													</ItemTemplate>
													<EditItemTemplate>
													<forum:Emoticon ID="ctlEmoticon" runat="server" DisplayType="Admin" />
												</EditItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn ItemStyle-Width="80px" HeaderText="Code" ItemStyle-CssClass="Forum_Grid_Middle" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
													<ItemTemplate>
													<asp:Label ID="lblCode" runat="server" />
												</ItemTemplate>
													<EditItemTemplate>
													<asp:TextBox ID="txtCode" runat="server" Width="80px" CssClass="NormalTextBox" />
												</EditItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn ItemStyle-Width="100px" HeaderText="Tooltip" ItemStyle-CssClass="Forum_Grid_Middle">
													<ItemTemplate>
														<asp:Label ID="lblToolTip" runat="server" />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:TextBox ID="txtToolTip" runat="server" Width="100px" CssClass="NormalTextBox" />
													</EditItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn ItemStyle-Width="60px" HeaderText="Default" ItemStyle-CssClass="Forum_Grid_Middle" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" >
													<ItemTemplate>
														<asp:CheckBox ID="chkDefault" runat="server" Enabled="false" />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:CheckBox ID="chkDefault" runat="server" />
													</EditItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn ItemStyle-Width="125px" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="Forum_Grid_Right" >
													<ItemTemplate>
														<asp:ImageButton ID="imgEdit" runat="server" CommandName="edit" resourcekey="Edit" CausesValidation="false" />
														<asp:ImageButton ID="imgDelete" runat="server" CommandName="delete" resourcekey="Delete" CausesValidation="false" />
														<asp:ImageButton ID="imgUp" runat="server" CommandName="up" resourcekey="Up" CausesValidation="false" />
														<asp:ImageButton ID="imgDown" runat="server" CommandName="down" resourcekey="Down" CausesValidation="false" />
													</ItemTemplate>
													<EditItemTemplate>
														<asp:ImageButton ID="imgCancel" runat="server" CommandName="cancel" resourcekey="Cancel" CausesValidation="false" />
														<asp:ImageButton ID="imgUpdate" runat="server" CommandName="update" resourcekey="Update" CausesValidation="false" />
													</EditItemTemplate>
												</asp:TemplateColumn>
											</Columns>
 										</asp:DataGrid>
									</td>
								</tr>
								<tr>
									<td class="Forum_Footer">&nbsp;</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Panel>