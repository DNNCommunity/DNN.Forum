<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Tracking.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Tracking" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="dnnForm ucpTracking dnnClear" id="ucpTracking">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblForumTitle" runat="server" resourcekey="Tracking" EnableViewState="false" /></h2>
	<wrapper:RadTabStrip ID="rtsNotifications" runat="server" MultiPageID="rmpNotifications" SelectedIndex="0" Skin="Sitefinity" AutoPostBack="false" />		
	<dnnweb:DnnMultiPage ID="rmpNotifications" runat="server" SelectedIndex="0">
		<dnnweb:DnnPageView ID="rpvForums" runat="server">
			<div class="trackForums dnnClear" id="trackForums">
				<dnnweb:DnnGrid runat="server" ID="gridForumTracking" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None" Skin="Sitefinity">
					<MasterTableView DataKeyNames="ForumID,MostRecentPostID" >
						<Columns>
							<dnnweb:DnnGridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/delete.gif" UniqueName="imgDelete" CommandName="DeleteItem" HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderText="Delete" />
							<dnnweb:DnnGridHyperlinkColumn UniqueName="hlName" HeaderText="Name" DataTextField="Name" ItemStyle-Width="45%" />
							<dnnweb:DnnGridBoundColumn UniqueName="Posts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" HeaderStyle-Width="85px" />
							<dnnweb:DnnGridBoundColumn UniqueName="Threads" DataField="TotalThreads" HeaderText="TotalThreads" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" HeaderStyle-Width="85px"/>
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
		</dnnweb:DnnPageView>
		<dnnweb:DnnPageView ID="rpvThreads" runat="server" >
			<div class="trackThreads dnnClear" id="trackThreads">		
				<dnnweb:DnnGrid runat="server" ID="gridThreadTracking" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None" Skin="Sitefinity">
					<MasterTableView DataKeyNames="ThreadID,ForumID,MostRecentPostID" >
						<Columns>
							<dnnweb:DnnGridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/delete.gif" UniqueName="imgDelete" CommandName="DeleteItem" HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderText="Delete" />
							<dnnweb:DnnGridHyperlinkColumn UniqueName="hlSubject" HeaderText="Subject" DataTextField="Subject" ItemStyle-Width="55%" />
							<dnnweb:DnnGridBoundColumn UniqueName="Posts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="85px" ItemStyle-Width="85px" />	
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
		</dnnweb:DnnPageView>
	</dnnweb:DnnMultiPage>
</div>