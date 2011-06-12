<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailQueueTaskDetail.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailQueueTaskDetail" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
								<wrapper:GridTableView DataKeyNames="EmailQueueID" Name="TaskEmails" Width="99%">
                                    <NoRecordsTemplate>
								        <asp:Label ID="lblNoDetailRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
							        </NoRecordsTemplate>
									<Columns>
										<dnnweb:DnnGridBoundColumn UniqueName="EmailAddress" DataField="EmailAddress" HeaderText="EmailAddress" />	
										<dnnweb:DnnGridCheckBoxColumn UniqueName="IsComplete" DataField="IsComplete" HeaderStyle-Width="50px" ItemStyle-Width="50px" HeaderText="IsComplete" />
										<dnnweb:DnnGridCheckBoxColumn UniqueName="Failed" DataField="Failed" HeaderStyle-Width="50px" ItemStyle-Width="50px" HeaderText="Failed" />
										<dnnweb:DnnGridBoundColumn UniqueName="DateAdded" DataField="DateAdded" HeaderText="DateAdded" />	
										<dnnweb:DnnGridBoundColumn UniqueName="DateComplete" DataField="DateComplete" HeaderText="DateComplete" />	
									</Columns>
								</wrapper:GridTableView>
							</DetailTables>
							<Columns>
								<dnnweb:DnnGridBoundColumn UniqueName="SuccessfullSendCount" DataField="SuccessfullSendCount" HeaderStyle-Width="50px" ItemStyle-Width="50px" HeaderText="SuccessfullSendCount" />
								<dnnweb:DnnGridBoundColumn UniqueName="EmailSubject" DataField="EmailSubject" HeaderText="EmailSubject" />	
								<dnnweb:DnnGridBoundColumn UniqueName="QueueAddedDate" DataField="QueueAddedDate" HeaderText="QueueAddedDate" />	
								<dnnweb:DnnGridBoundColumn UniqueName="QueueStartedDate" DataField="QueueStartedDate" HeaderText="QueueStartedDate" />	
								<dnnweb:DnnGridBoundColumn UniqueName="QueueCompletedDate" DataField="QueueCompletedDate" HeaderText="QueueCompletedDate" />	
							</Columns>
						</MasterTableView>
					</dnnweb:DnnGrid>
				</div>
			</td>
		</tr>
	</table>
</div>