<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Bookmark.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Bookmark" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<table cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="Bookmark" EnableViewState="false" />
		</td>
	</tr>
	<tr>
		<td class="Forum_UCP_HeaderInfo">
			<asp:Label ID="lblInfo" runat="server" CssClass="Forum_NormalBold" />
			<table cellspacing="0" cellpadding="0" width="100%">
				<tr>
					<td>
						<asp:datagrid ID="dgBookmarks" runat="server" DataKeyField="ThreadID" Width="100%" AutoGenerateColumns="false" CssClass="Forum_Grid" CellPadding="0" CellSpacing="0" GridLines="None" >
							<HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
							<ItemStyle CssClass="Forum_Grid_Row_Alt" />
							<AlternatingItemStyle CssClass="Forum_Grid_Row" />
							<Columns>
								<asp:TemplateColumn ItemStyle-Width="15px" >
									<ItemTemplate>
										<asp:CheckBox ID="chkBookmark" runat="server" EnableViewState="false" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn ItemStyle-Width="40px" >
									<ItemTemplate>
										<asp:Image ID="imgStatus" runat="server" EnableViewState="false" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn ItemStyle-CssClass="Forum_Grid_Left" HeaderText="Subject" HeaderStyle-HorizontalAlign="Left">
									<ItemTemplate>
										<asp:HyperLink ID="hlSubject" runat="server" CssClass="Forum_NormalBold" EnableViewState="false" />
										<asp:Image ID="imgRating" runat="server" EnableViewState="false" />
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%" ItemStyle-CssClass="Forum_Grid_Middle" >
								  <ItemTemplate>
									 <asp:Label ID="lblTotalPosts" runat="server" />
								  </ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn HeaderText="LastPost" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="23%" ItemStyle-CssClass="Forum_Grid_Right">
									<ItemTemplate>
										<asp:Label ID="lblLastPostInfo" runat="server" CssClass="Forum_LastPostText" EnableViewState="false" />
									</ItemTemplate>
								</asp:TemplateColumn> 
								<asp:BoundColumn DataField="ForumID" Visible="false" />      
							</Columns>
						</asp:datagrid>
						<dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
					</td>
				</tr>
				<tr>
					<td align="center" width="100%">
						<asp:LinkButton ID="cmdRemove" CssClass="CommandButton" runat="server" resourcekey="cmdRemove" EnableViewState="false" Width="200" />
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>           