<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Tracking.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Tracking" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblForumTitle" runat="server" resourcekey="Tracking" EnableViewState="false" />
		</td>
	</tr>
	<tr>
		<td class="Forum_UCP_HeaderInfo" align="left">
			<table cellpadding="0" cellspacing="0">
                 <tr>
                     <td class="Forum_Tab_Left">
                         <asp:LinkButton ID="cmdForum" runat="server" CssClass="Forum_Link" resourcekey="cmdForum" CausesValidation="false" />
                     </td>
                     <td class="Forum_Tab_Right">
                         <asp:LinkButton ID="cmdThread" runat="server" CssClass="Forum_Link" resourcekey="cmdThread" CausesValidation="false" />
                     </td>
                 </tr>
			</table>
			<table cellpadding="0" cellspacing="0" width="100%" id="tblForum" runat="server">
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
						<asp:LinkButton ID="cmdForumRemove" CssClass="CommandButton" runat="server" resourcekey="cmdForumRemove" />
					</td>
				</tr>
			</table>
			<table cellspacing="0" cellpadding="0" width="100%" runat="server" id="tblThread">
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
						<asp:LinkButton ID="cmdThreadRemove" CssClass="CommandButton" runat="server" resourcekey="cmdThreadRemove" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

            