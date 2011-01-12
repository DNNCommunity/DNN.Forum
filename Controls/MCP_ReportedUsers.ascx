<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_ReportedUsers.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.ReportedUsers" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="MCP-ReportedUsers">
    <dnnweb:DnnGrid runat="server" ID="dnngridReportedUsers" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None" PageSize="25" PagerStyle-AlwaysVisible="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" />
		<MasterTableView DataKeyNames="UserID" >
            <NoRecordsTemplate>
			<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
		</NoRecordsTemplate>
			<Columns>
				<dnnweb:DnnGridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/edit.gif" UniqueName="imgEdit" CommandName="EditUser" HeaderText="Edit" />
                <dnnweb:DnnGridHyperLinkColumn UniqueName="User" HeaderText="User" />
				<dnnweb:DnnGridBoundColumn UniqueName="ReportedPostCount" DataField="ReportedPostCount" HeaderText="ReportedPostCount" />
				<dnnweb:DnnGridBoundColumn UniqueName="UnaddressedPostCount" DataField="UnaddressedPostCount" HeaderText="UnaddressedPostCount" />
			</Columns>
		</MasterTableView>			
	</dnnweb:DnnGrid>
</div>