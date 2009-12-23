<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Tracking.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Tracking" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblForumTitle" runat="server" resourcekey="Tracking" EnableViewState="false" />
		</td>
	</tr>
	<tr>
		<td class="Forum_UCP_HeaderInfo" align="left">
			<telerik:RadTabStrip ID="rtsNotifications" runat="server" Skin="Vista" MultiPageID="rmpNotifications" SelectedIndex="0" />		
			<telerik:RadMultiPage ID="rmpNotifications" runat="server" SelectedIndex="0">
				<telerik:RadPageView ID="rpvForums" runat="server">
					<table cellpadding="0" cellspacing="0" width="100%">
						<tr>
							 <td>
								<asp:Label id="lblForums" runat="server" CssClass="Forum_NormalBold" />
								<asp:datagrid ID="dgForums" runat="server" DataKeyField="ForumID" Width="100%" AutoGenerateColumns="false" CssClass="Forum_Grid" CellPadding="0" CellSpacing="0" GridLines="None" >
								    <HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
								    <ItemStyle CssClass="Forum_Grid_Row_Alt" />
								    <AlternatingItemStyle CssClass="Forum_Grid_Row" />
								    <Columns>
									   <asp:TemplateColumn ItemStyle-Width="15px" >
										  <ItemTemplate>
											 <asp:CheckBox ID="chkForum" runat="server" />
										  </ItemTemplate>
									   </asp:TemplateColumn>
									   <asp:TemplateColumn ItemStyle-Width="40px" >
										  <ItemTemplate>
										    <asp:Literal ID="imgStatus" runat="server" />
										  </ItemTemplate>
									   </asp:TemplateColumn>
									   <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Forum" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="Forum_Grid_Left" >
										  <ItemTemplate>
											 <asp:HyperLink ID="hlName" runat="server" CssClass="Forum_NormalBold" />
										  </ItemTemplate>
									   </asp:TemplateColumn>
									   <asp:TemplateColumn HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-CssClass="Forum_Grid_Middle" >
										  <ItemTemplate>
											 <asp:Label ID="lblTotalPosts" runat="server" />
										  </ItemTemplate>
									   </asp:TemplateColumn>
									   <asp:TemplateColumn HeaderText="TotalThreads" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-CssClass="Forum_Grid_Middle" >
										  <ItemTemplate>
											 <asp:Label ID="lblTotalThreads" runat="server" />
										  </ItemTemplate>
									   </asp:TemplateColumn>
									   <asp:TemplateColumn HeaderText="LastPost" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="23%" ItemStyle-CssClass="Forum_Grid_Right" >
										  <ItemTemplate>
											 <asp:Label ID="lblLastPostInfo" runat="server" />
										  </ItemTemplate>
									   </asp:TemplateColumn> 
								    </Columns>
								</asp:datagrid>
								<dnnforum:AjaxPager ID="ForumPager" runat="server" Width="100%"/>
							 </td>
						  </tr>
						<tr>
							<td align="center">
								<asp:LinkButton ID="cmdForumRemove" CssClass="CommandButton" runat="server" resourcekey="cmdForumRemove" Width="200" />
							</td>
						</tr>
					</table>
				</telerik:RadPageView>
				<telerik:RadPageView ID="rpvThreads" runat="server">
					<table cellspacing="0" cellpadding="0" width="100%">
						<tr>
						<td>
							<asp:Label ID="lblThreads" runat="server" CssClass="Forum_NormalBold" />
							<asp:datagrid ID="dgThreads" runat="server" DataKeyField="ThreadID" Width="100%" AutoGenerateColumns="false" CssClass="Forum_Grid" CellPadding="0" CellSpacing="0" GridLines="None">
							    <HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
							    <ItemStyle CssClass="Forum_Grid_Row_Alt" />
							    <AlternatingItemStyle CssClass="Forum_Grid_Row" />
							    <Columns>
									<asp:TemplateColumn ItemStyle-Width="15px" >
										<ItemTemplate>
											<asp:CheckBox ID="chkThread" runat="server" />
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn ItemStyle-Width="40px" >
										<ItemTemplate>
											<asp:Image ID="imgStatus" runat="server" />
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Subject" ItemStyle-CssClass="Forum_Grid_Left">
										<ItemTemplate>
											<asp:HyperLink ID="hlSubject" runat="server" CssClass="Forum_NormalBold" />
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-CssClass="Forum_Grid_Middle" >
									  <ItemTemplate>
										 <asp:Label ID="lblTotalPosts" runat="server" />
									  </ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="LastPost" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="23%" ItemStyle-CssClass="Forum_Grid_Right" >
										<ItemTemplate>
											<asp:Label ID="lblLastPostInfo" runat="server" />
										</ItemTemplate>
									</asp:TemplateColumn> 
									<asp:BoundColumn DataField="ForumID" Visible="false" />      
								</Columns>
							</asp:datagrid>
							<dnnforum:AjaxPager ID="ThreadPager" runat="server" Width="100%"/>
						</td>
					</tr>
						<tr>
						<td align="center">
							<asp:LinkButton ID="cmdThreadRemove" CssClass="CommandButton" runat="server" resourcekey="cmdThreadRemove" Width="200" />
						</td>
					</tr>
					</table>
				</telerik:RadPageView>
			</telerik:RadMultiPage>
		</td>
	</tr>
</table>

            