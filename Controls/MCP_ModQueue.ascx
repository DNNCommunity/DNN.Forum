<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_ModQueue.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.ModQueue" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<div class="MCP-ModQueue">
	<asp:Panel ID="pnlModQueue" runat="server" EnableViewState="false">
	    <asp:datagrid ID="dgModQueue" runat="server" DataKeyField="ForumID" Width="100%" AutoGenerateColumns="false" CssClass="Forum_Grid" CellPadding="0" CellSpacing="0" GridLines="None" EnableViewState="false" >
		    <HeaderStyle CssClass="Forum_Grid_Header" />
		    <ItemStyle CssClass="Forum_Grid_Row_Alt" />
		    <AlternatingItemStyle CssClass="Forum_Grid_Row" />
		    <Columns>
			   <asp:TemplateColumn>
				  <ItemTemplate>
					 <asp:HyperLink ID="hlStatus" runat="server" EnableViewState="false" >
						<asp:Image ID="imgStatus" runat="server" EnableViewState="false" />
					 </asp:HyperLink> 
				  </ItemTemplate>
			   </asp:TemplateColumn>
			   <asp:TemplateColumn ItemStyle-CssClass="Forum_Grid_Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Forum"  ItemStyle-Width="80%">
				  <ItemTemplate>
					 <asp:HyperLink ID="hlSubject" runat="server" CssClass="Forum_NormalBold" EnableViewState="false" />
				  </ItemTemplate>
			   </asp:TemplateColumn>
			   <asp:BoundColumn DataField="PostsToModerate" HeaderText="PostsToModerate" ItemStyle-CssClass="Forum_Grid_Right" ItemStyle-Width="15%" />
		    </Columns>
		</asp:datagrid>
		<dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
	</asp:Panel>
	<asp:Panel ID="pnlNoItems" runat="server">
		<table cellpadding="0" cellspacing ="0" width="100%">
			<tr>
				<td class="Forum_UCP_Header">
					<asp:Label id="lblInbox" runat="server" resourcekey="ModQueue.Header" EnableViewState="false" />
				</td>
			</tr>
			<tr>
				<td class="Forum_UCP_HeaderInfo" align="center">
					<asp:Label id="lblNoResults" runat="server" CssClass="Forum_NormalBold" resourcekey="lblNoResults" EnableViewState="false" />
				</td>
			 </tr>
	    </table>
	</asp:Panel>
</div>