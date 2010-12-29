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
                    <dnnweb:DnnGrid ID="rgForums" runat="server" AllowSorting="true" AllowPaging="true" PageSize="10" ItemStyle-Height="30" AutoGenerateColumns="false">
                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false">
							    <Resizing AllowColumnResize="true" />
						</ClientSettings>
                        <MasterTableView DataKeyNames="ForumID" CommandItemDisplay="Top" CommandItemStyle-Height="30">
                            <CommandItemTemplate>
                                &nbsp;&nbsp;<asp:Label runat="server" ID="lblForumDescription" EnableViewState="false" resourcekey="lblForumDescription" />&nbsp;&nbsp;
                                <telerik:RadComboBox ID="rcbForums" runat="server" Width="250" AutoPostBack="true" onselectedindexchanged="rcbForums_SelectedIndexChange" />
                                <asp:Label runat="server" ID="lblForumTitle" EnableViewState="false" />
                            </CommandItemTemplate>
							<NoRecordsTemplate>
								<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
							</NoRecordsTemplate>
							<Columns>
                                <telerik:GridBoundColumn UniqueName="Email" DataField="Email" />
								<telerik:GridBoundColumn UniqueName="Username" DataField="Username" />	
								<telerik:GridBoundColumn UniqueName="DisplayName" DataField="DisplayName" />
								<telerik:GridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" />
							</Columns>
						</MasterTableView>
					</dnnweb:DnnGrid><br /><br />
                    
					<dnnweb:DnnGrid ID="rgThreads" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PageSize="10" ItemStyle-Height="30">
						<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false">
							<Resizing AllowColumnResize="true" />
						</ClientSettings>
						<MasterTableView DataKeyNames="ThreadID" CommandItemDisplay="Top" CommandItemStyle-Height="30">
                            <CommandItemTemplate>
                                &nbsp;&nbsp;<asp:Label runat="server" ID="lblThreadDescription" EnableViewState="false" resourcekey="lblThreadDescription" />
                                <asp:Label runat="server" ID="lblThreadTitle" EnableViewState="false" />
                            </CommandItemTemplate>
							<NoRecordsTemplate>
								<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
							</NoRecordsTemplate>
							<Columns>
                                <telerik:GridBoundColumn UniqueName="Email" DataField="Email" />
								<telerik:GridBoundColumn UniqueName="Username" DataField="Username" />	
								<telerik:GridBoundColumn UniqueName="DisplayName" DataField="DisplayName" />
								<telerik:GridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" />
							</Columns>
						</MasterTableView>
					</dnnweb:DnnGrid>
				</div>
			</td>
		</tr>
	</table>
</div>