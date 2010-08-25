<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Tracking.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Tracking" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="UCP-Tracking">
	<table cellpadding="0" cellspacing="0" width="100%" class="Forum_SearchContainer">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblForumTitle" runat="server" resourcekey="Tracking" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo" align="left">
				<telerik:RadTabStrip ID="rtsNotifications" runat="server" MultiPageID="rmpNotifications" SelectedIndex="0" Skin="Vista"/>		
				<telerik:RadMultiPage ID="rmpNotifications" runat="server" SelectedIndex="0">
					<telerik:RadPageView ID="rpvForums" runat="server">
						<dnnweb:DnnGrid runat="server" ID="gridForumTracking" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None">
							<MasterTableView DataKeyNames="ForumID,Name,MostRecentPostID" >
								<Columns>
									<telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/delete.gif" UniqueName="imgDelete" CommandName="DeleteItem" />
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlName" HeaderText="Name" DataTextField="Name" ItemStyle-Width="40%" />
									<telerik:GridBoundColumn UniqueName="Posts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" />
									<telerik:GridBoundColumn UniqueName="Threads" DataField="TotalThreads" HeaderText="TotalThreads" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" />	
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlLastPost" HeaderText="LastPost" />
								</Columns>
							</MasterTableView>			
						</dnnweb:DnnGrid>
					</telerik:RadPageView>
					<telerik:RadPageView ID="rpvThreads" runat="server" >
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
								<asp:LinkButton ID="cmdThreadRemove" CssClass="CommandButton primary-action" runat="server" resourcekey="cmdThreadRemove" />
							</td>
						</tr>
						</table>
					</telerik:RadPageView>
				</telerik:RadMultiPage>
			</td>
		</tr>
	</table>
</div>