<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_BannedUsers.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.BannedUsers" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="MCP-BannedUsers">
    <dnnweb:DnnGrid runat="server" ID="dnngridBannedUsers" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None" PageSize="25" PagerStyle-AlwaysVisible="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" />
		<MasterTableView DataKeyNames="UserID" >
            <NoRecordsTemplate>
			<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
		</NoRecordsTemplate>
			<Columns>
				<wrapper:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/edit.gif" UniqueName="imgEdit" CommandName="EditUser" HeaderText="Edit" />
                <dnnweb:DnnGridHyperLinkColumn UniqueName="User" HeaderText="User" />
				<dnnweb:DnnGridBoundColumn UniqueName="StartBanDate" DataField="StartBanDate" HeaderText="StartBanDate" />
				<dnnweb:DnnGridBoundColumn UniqueName="LiftBanDate" DataField="LiftBanDate" HeaderText="LiftBanDate" />
			</Columns>
		</MasterTableView>			
	</dnnweb:DnnGrid>
</div>