<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailSubscribers.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailSubscribers" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<div class="ACP-EmailSubscribers">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">
             <asp:Label ID="lblInfo" runat="server" CssClass="Forum_Normal" resourcekey="lblInfo" EnableViewState="false" />
				<div><br />
                    <asp:HiddenField runat="server" ID="hdnRcbForumsValue" />
                    <telerik:RadGrid ID="rgForums" runat="server" AllowSorting="false" AllowPaging="false" PageSize="3" ItemStyle-Height="30">
                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true">
							    <Resizing AllowColumnResize="true" />
						</ClientSettings>
                        <MasterTableView DataKeyNames="ForumID" CommandItemDisplay="Top" CommandItemStyle-Height="30">
                            <CommandItemTemplate>
                                &nbsp;&nbsp;<asp:Label runat="server" ID="lblForumDescription" EnableViewState="false" resourcekey="lblForumDescription" />
                                <telerik:RadComboBox ID="rcbForums" runat="server" Width="250" AutoPostBack="true" onselectedindexchanged="rcbForums_SelectedIndexChange" />
                                <asp:Label runat="server" ID="lblForumTitle" EnableViewState="false" />
                            </CommandItemTemplate>
							<NoRecordsTemplate>
								<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="NormalBold" />
							</NoRecordsTemplate>
							<Columns>
								<telerik:GridBoundColumn UniqueName="Username" DataField="Username" />	
								<telerik:GridBoundColumn UniqueName="DisplayName" DataField="DisplayName" />
                                <telerik:GridBoundColumn UniqueName="Email" DataField="Email" />
								<telerik:GridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" />
							</Columns>
						</MasterTableView>
					</telerik:RadGrid><br /><br />
                    
                    <asp:HiddenField runat="server" ID="hdnRcbThreadsValue" />
					<telerik:RadGrid ID="rgThreads" runat="server" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="false" PageSize="3" ItemStyle-Height="30">
						<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true">
							<Resizing AllowColumnResize="true" />
						</ClientSettings>
						<MasterTableView DataKeyNames="ThreadID" CommandItemDisplay="Top" CommandItemStyle-Height="30">
                            <CommandItemTemplate>
                                &nbsp;&nbsp;<asp:Label runat="server" ID="lblThreadDescription" EnableViewState="false" resourcekey="lblThreadDescription" />
                                <telerik:RadComboBox runat="server" ID="rcbThreads" Width="250" AutoPostBack="true" onselectedindexchanged="rcbThreads_SelectedIndexChange" />
                                <asp:Label runat="server" ID="lblThreadTitle" EnableViewState="false" />
                            </CommandItemTemplate>
							<NoRecordsTemplate>
								<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="NormalBold" />
							</NoRecordsTemplate>
							<Columns>
                                <telerik:GridBoundColumn UniqueName="Email" DataField="Email" />
								<telerik:GridBoundColumn UniqueName="Username" DataField="Username" />	
								<telerik:GridBoundColumn UniqueName="DisplayName" DataField="DisplayName" />
								<telerik:GridBoundColumn UniqueName="Subject" DataField="Subject"  />
								<telerik:GridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" />
							</Columns>
						</MasterTableView>
					</telerik:RadGrid>
                   
				</div>
                <asp:LinkButton runat="server" ID="lnkShowAll" resourcekey="lnkShowAll" Visible="false" CssClass="CommandButton" />
			</td>
		</tr>
        <tr align="center">
            <td></td>
        </tr>
	</table>
</div>