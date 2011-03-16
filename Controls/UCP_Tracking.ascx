<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Tracking.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Tracking" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="UCP-Tracking">
	<table cellpadding="0" cellspacing="0" width="100%" class="Forum_SearchContainer">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblForumTitle" runat="server" resourcekey="Tracking" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo" align="left">
				<dnnweb:DnnTabStrip ID="rtsNotifications" runat="server" MultiPageID="rmpNotifications" SelectedIndex="0" Skin="Vista" AutoPostBack="true" />		
				<dnnweb:DnnMultiPage ID="rmpNotifications" runat="server" SelectedIndex="0">
					<wrapper:RadPageView ID="rpvForums" runat="server">
						<dnnweb:DnnGrid runat="server" ID="gridForumTracking" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None">
							<MasterTableView DataKeyNames="ForumID,MostRecentPostID" >
								<Columns>
									<dnnweb:DnnGridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/delete.gif" UniqueName="imgDelete" CommandName="DeleteItem" HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderText="Delete" />
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlName" HeaderText="Name" DataTextField="Name" ItemStyle-Width="45%" />
									<dnnweb:DnnGridBoundColumn UniqueName="Posts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" HeaderStyle-Width="85px" />
									<dnnweb:DnnGridBoundColumn UniqueName="Threads" DataField="TotalThreads" HeaderText="TotalThreads" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" HeaderStyle-Width="85px"/>
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlLastPost" HeaderText="LastPost" />
								</Columns>
							</MasterTableView>			
						</dnnweb:DnnGrid>
					</wrapper:RadPageView>
					<wrapper:RadPageView ID="rpvThreads" runat="server" >
						<dnnweb:DnnGrid runat="server" ID="gridThreadTracking" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None">
							<MasterTableView DataKeyNames="ThreadID,ForumID,MostRecentPostID" >
								<Columns>
									<dnnweb:DnnGridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/delete.gif" UniqueName="imgDelete" CommandName="DeleteItem" HeaderStyle-Width="20px" ItemStyle-Width="20px" HeaderText="Delete" />
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlSubject" HeaderText="Subject" DataTextField="Subject" ItemStyle-Width="55%" />
									<dnnweb:DnnGridBoundColumn UniqueName="Posts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="85px" ItemStyle-Width="85px" />	
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlLastPost" HeaderText="LastPost" />
								</Columns>
							</MasterTableView>			
						</dnnweb:DnnGrid>
					</wrapper:RadPageView>
				</dnnweb:DnnMultiPage>
			</td>
		</tr>
	</table>
</div>