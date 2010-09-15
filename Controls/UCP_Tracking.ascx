<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Tracking.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Tracking" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="UCP-Tracking">
	<table cellpadding="0" cellspacing="0" width="100%" class="Forum_SearchContainer">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblForumTitle" runat="server" resourcekey="Tracking" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo" align="left">
				<telerik:RadTabStrip ID="rtsNotifications" runat="server" MultiPageID="rmpNotifications" SelectedIndex="0" Skin="Vista"/>		
				<telerik:RadMultiPage ID="rmpNotifications" runat="server" SelectedIndex="0">
					<telerik:RadPageView ID="rpvForums" runat="server">
						<dnnweb:DnnGrid runat="server" ID="gridForumTracking" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None">
							<MasterTableView DataKeyNames="ForumID,MostRecentPostID" >
								<Columns>
									<telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/delete.gif" UniqueName="imgDelete" CommandName="DeleteItem" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlName" HeaderText="Name" DataTextField="Name" ItemStyle-Width="40%" />
									<telerik:GridBoundColumn UniqueName="Posts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" HeaderStyle-Width="85px" />
									<telerik:GridBoundColumn UniqueName="Threads" DataField="TotalThreads" HeaderText="TotalThreads" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="85px" HeaderStyle-Width="85px"/>
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlLastPost" HeaderText="LastPost" />
								</Columns>
							</MasterTableView>			
						</dnnweb:DnnGrid>
					</telerik:RadPageView>
					<telerik:RadPageView ID="rpvThreads" runat="server" >
						<dnnweb:DnnGrid runat="server" ID="gridThreadTracking" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None">
							<MasterTableView DataKeyNames="ThreadID,ForumID,MostRecentPostID" >
								<Columns>
									<telerik:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/delete.gif" UniqueName="imgDelete" CommandName="DeleteItem" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlSubject" HeaderText="Subject" DataTextField="Subject" ItemStyle-Width="70%" />
									<telerik:GridBoundColumn UniqueName="TotalPosts" DataField="TotalPosts" HeaderText="TotalPosts" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="85px" ItemStyle-Width="85px" />	
									<dnnweb:DnnGridHyperlinkColumn UniqueName="hlLastPost" HeaderText="LastPost" />
								</Columns>
							</MasterTableView>			
						</dnnweb:DnnGrid>
					</telerik:RadPageView>
				</telerik:RadMultiPage>
			</td>
		</tr>
	</table>
</div>