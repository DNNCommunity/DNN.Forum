<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_ModQueue.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.ModQueue" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<div class="MCP-ModQueue">
	<dnnweb:DnnGrid runat="server" ID="gridPostsToModerate" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None">
		<MasterTableView DataKeyNames="ForumID" >
			<Columns>
				<dnnweb:DnnGridHyperlinkColumn UniqueName="hlName" HeaderText="Name" DataTextField="Name" />
				<telerik:GridBoundColumn UniqueName="Posts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
			</Columns>
		</MasterTableView>			
	</dnnweb:DnnGrid>
</div>