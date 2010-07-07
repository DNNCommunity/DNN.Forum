<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailQueueTaskDetail.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailQueueTaskDetail" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
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
					<telerik:RadGrid ID="rgTaskDetails" runat="server" AutoGenerateColumns="false" AllowSorting="false" AllowPaging="true" PageSize="20" >
						<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true">
							<Resizing AllowColumnResize="true" />
						</ClientSettings>
						<MasterTableView DataKeyNames="EmailQueueID">
							<NoRecordsTemplate>
								<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="NormalBold" />
							</NoRecordsTemplate>
							<DetailTables>
								<telerik:GridTableView DataKeyNames="EmailQueueID" Name="TaskEmails" Width="100%">
									<Columns>
										<telerik:GridBoundColumn UniqueName="EmailAddress" HeaderText="EmailAddress" DataField="EmailAddress" />	
										<telerik:GridCheckBoxColumn UniqueName="IsComplete" HeaderText="IsComplete" DataField="IsComplete" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
										<telerik:GridCheckBoxColumn UniqueName="Failed" HeaderText="Failed" DataField="Failed" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
										<telerik:GridBoundColumn UniqueName="DateAdded" HeaderText="DateAdded" DataField="DateAdded" />	
										<telerik:GridBoundColumn UniqueName="DateComplete" HeaderText="DateComplete" DataField="DateComplete" />	
									</Columns>
								</telerik:GridTableView>
							</DetailTables>
							<Columns>
								<telerik:GridBoundColumn UniqueName="SuccessfullSendCount" HeaderText="SuccessfullSendCount" DataField="SuccessfullSendCount" HeaderStyle-Width="50px" ItemStyle-Width="50px" />
								<telerik:GridBoundColumn UniqueName="EmailSubject" HeaderText="EmailSubject" DataField="EmailSubject" />	
								<telerik:GridBoundColumn UniqueName="QueueAddedDate" HeaderText="QueueAddedDate" DataField="QueueAddedDate" />	
								<telerik:GridBoundColumn UniqueName="QueueStartedDate" HeaderText="QueueStartedDate" DataField="QueueStartedDate" />	
								<telerik:GridBoundColumn UniqueName="QueueCompletedDate" HeaderText="QueueCompletedDate" DataField="QueueCompletedDate" />	
							</Columns>
						</MasterTableView>
					</telerik:RadGrid>
				</div>
			</td>
		</tr>
	</table>
</div>