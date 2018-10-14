<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_ModQueue.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.ModQueue" %>
<%@ Register Assembly="DotNetNuke.Web.Deprecated" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm mcpModQueue dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<dnnweb:DnnGrid runat="server" ID="gridPostsToModerate" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" Skin="Sitefinity">
		<MasterTableView DataKeyNames="ForumID" >
			<Columns>
				<dnnweb:DnnGridHyperlinkColumn UniqueName="hlName" HeaderText="Name" DataTextField="Name" />
				<dnnweb:DnnGridBoundColumn UniqueName="PostsToModerate" DataField="PostsToModerate" HeaderText="PostsToModerate" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
			</Columns>
			<NoRecordsTemplate>
					<div class="dnnFormMessage dnnFormWarning">
						<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" />
					</div>
				</NoRecordsTemplate>
		</MasterTableView>			
	</dnnweb:DnnGrid>
</div>