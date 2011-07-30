<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Bookmark.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Bookmark" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm ucpBookmark dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="Bookmark" EnableViewState="false" /></h2>
	<dnnweb:DnnGrid runat="server" ID="gridBookmarks" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" Skin="Sitefinity">
		<MasterTableView DataKeyNames="ForumID,ThreadID,MostRecentPostID" >
			<Columns>
				<dnnweb:DnnGridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/delete.gif" UniqueName="imgDelete" CommandName="DeleteItem" HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderText="Delete" />
				<dnnweb:DnnGridHyperlinkColumn UniqueName="hlName" HeaderText="Subject" DataTextField="Subject" ItemStyle-Width="45%" />
				<dnnweb:DnnGridBoundColumn UniqueName="Posts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" HeaderStyle-Width="85px" />
				<dnnweb:DnnGridHyperlinkColumn UniqueName="hlLastPost" HeaderText="LastPost" />
			</Columns>
			<NoRecordsTemplate>
				<div class="dnnFormMessage dnnFormWarning">
					<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" />
				</div>
			</NoRecordsTemplate>
		</MasterTableView>
	</dnnweb:DnnGrid>
</div>