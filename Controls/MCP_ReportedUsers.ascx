<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_ReportedUsers.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.ReportedUsers" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<div class="MCP-ReportedUsers">
	<asp:Panel ID="pnlReportedUsers" runat="server">
    <asp:datagrid ID="dgReportedUsers" runat="server" DataKeyField="UserID" Width="100%" AutoGenerateColumns="false" CssClass="Forum_Grid" CellPadding="0" CellSpacing="0" GridLines="None" >
	    <HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
	    <ItemStyle CssClass="Forum_Grid_Row_Alt" />
	    <AlternatingItemStyle CssClass="Forum_Grid_Row" />
	    <Columns>
	    	<asp:TemplateColumn ItemStyle-Width="15px">
		       <ItemTemplate>
			      <asp:HyperLink ID="hlEdit" runat="server">
				    <asp:Image ID="imgEdit" runat="server" />
			      </asp:HyperLink> 
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn ItemStyle-CssClass="Forum_Grid_Left" HeaderStyle-HorizontalAlign="Left" HeaderText="User" ItemStyle-Width="55%">
		       <ItemTemplate>
			      <asp:HyperLink ID="hlUser" runat="server" CssClass="Forum_NormalBold" EnableViewState="false" Target="_blank" />
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:BoundColumn DataField="ReportedPostCount" HeaderText="ReportedPostCount" ItemStyle-CssClass="Forum_Grid_Middle" ItemStyle-Width="17%" ItemStyle-HorizontalAlign="Center" />
	        <asp:BoundColumn DataField="UnaddressedPostCount" HeaderText="UnaddressedPostCount" ItemStyle-CssClass="Forum_Grid_Right" ItemStyle-Width="23%" ItemStyle-HorizontalAlign="Center" />
	    </Columns>
     </asp:datagrid>
     <dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
</asp:Panel>
	<asp:Panel ID="pnlNoItems" runat="server">
    <table cellpadding="0" cellspacing ="0" width="100%">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle.Header" EnableViewState="false" />
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