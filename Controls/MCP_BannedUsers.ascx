<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_BannedUsers.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.BannedUsers" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<div class="MCP-BannedUsers">
	<asp:Panel ID="pnlBannedUsers" runat="server">
	    <asp:datagrid ID="dgBannedUsers" runat="server" DataKeyField="UserID" Width="100%" AutoGenerateColumns="false" CssClass="Forum_Grid" CellPadding="0" CellSpacing="0" GridLines="None" EnableViewState="false" >
		    <HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
		    <ItemStyle CssClass="Forum_Grid_Row_Alt" />
		    <AlternatingItemStyle CssClass="Forum_Grid_Row" />
		    <Columns>
	    		   <asp:TemplateColumn>
				  <ItemTemplate>
					 <asp:HyperLink ID="hlEdit" runat="server">
					    <asp:Image ID="imgEdit" runat="server" />
					 </asp:HyperLink> 
				  </ItemTemplate>
			   </asp:TemplateColumn>
			   <asp:TemplateColumn ItemStyle-CssClass="Forum_Grid_Left" HeaderStyle-HorizontalAlign="Left" HeaderText="User" ItemStyle-Width="49%">
				  <ItemTemplate>
					 <asp:HyperLink ID="hlUser" runat="server" CssClass="Forum_NormalBold" EnableViewState="false" />
				  </ItemTemplate>
			   </asp:TemplateColumn>
	        		<asp:TemplateColumn HeaderText="StartBanDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="23%" ItemStyle-CssClass="Forum_Grid_Middle"  >
				  <ItemTemplate>
					 <asp:Label ID="lblStartBanDate" runat="server" CssClass="Forum_LastPostText" EnableViewState="false" />
				  </ItemTemplate>
			   </asp:TemplateColumn>   
	        		<asp:TemplateColumn HeaderText="LiftBanDate" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="23%" ItemStyle-CssClass="Forum_Grid_Right" >
				  <ItemTemplate>
					 <asp:Label ID="lblLiftBanDate" runat="server" CssClass="Forum_LastPostText" EnableViewState="false" />
				  </ItemTemplate>
			   </asp:TemplateColumn>   
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