<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailQueueTaskDetail.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailQueueTaskDetail" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="ACP-EmailQueueTaskDetails">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">
				<div>
					<dnnweb:DnnGrid ID="rgTaskDetails" runat="server" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="True" PageSize="20">
                        <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true">
							    <Resizing AllowColumnResize="true" />
						</ClientSettings>
						<MasterTableView DataKeyNames="EmailQueueID">
							<NoRecordsTemplate>
								<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
							</NoRecordsTemplate>
							<DetailTables>
								<telerik:GridTableView DataKeyNames="EmailQueueID" Name="TaskEmails" Width="99%">
                                    <NoRecordsTemplate>
								        <asp:Label ID="lblNoDetailRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
							        </NoRecordsTemplate>
									<Columns>
										<telerik:GridBoundColumn UniqueName="EmailAddress" DataField="EmailAddress" />	
										<telerik:GridCheckBoxColumn UniqueName="IsComplete" DataField="IsComplete" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
										<telerik:GridCheckBoxColumn UniqueName="Failed" DataField="Failed" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
										<telerik:GridBoundColumn UniqueName="DateAdded" DataField="DateAdded" />	
										<telerik:GridBoundColumn UniqueName="DateComplete" DataField="DateComplete" />	
									</Columns>
								</telerik:GridTableView>
							</DetailTables>
							<Columns>
								<telerik:GridBoundColumn UniqueName="SuccessfullSendCount" DataField="SuccessfullSendCount" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
								<telerik:GridBoundColumn UniqueName="EmailSubject" DataField="EmailSubject" />	
								<telerik:GridBoundColumn UniqueName="QueueAddedDate" DataField="QueueAddedDate" />	
								<telerik:GridBoundColumn UniqueName="QueueStartedDate" DataField="QueueStartedDate" />	
								<telerik:GridBoundColumn UniqueName="QueueCompletedDate" DataField="QueueCompletedDate" />	
							</Columns>
						</MasterTableView>
					</dnnweb:DnnGrid>
				</div>
			</td>
		</tr>
	</table>
</div>