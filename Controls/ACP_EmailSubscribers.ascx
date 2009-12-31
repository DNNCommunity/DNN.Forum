<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailSubscribers.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailSubscribers" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td class="Forum_UCP_HeaderInfo">
			<div>
				<div align="center">
					<telerik:RadComboBox ID="rcbForums" runat="server" Skin="Web20" Width="250" DataTextField="Name" DataValueField="ForumID" AutoPostBack="true" /> <br /><br />
					<asp:TextBox ID="txtThreadID" runat="server" Width="50" Visible="false" />
				</div>
				<telerik:RadGrid ID="rgForums" runat="server" AutoGenerateColumns="false" AllowSorting="true" Skin="Web20" AllowPaging="true" PageSize="20" >
					<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true">
						<Resizing AllowColumnResize="true" />
					</ClientSettings>
					<MasterTableView DataKeyNames="ForumID">
						<NoRecordsTemplate>
							<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="NormalBold" />
						</NoRecordsTemplate>
						<Columns>
							<telerik:GridBoundColumn UniqueName="Email" HeaderText="Email" DataField="Email" />
							<telerik:GridBoundColumn UniqueName="Username" HeaderText="Username" DataField="Username" />	
							<telerik:GridBoundColumn UniqueName="Name" HeaderText="Name" DataField="Name" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
							<telerik:GridBoundColumn UniqueName="CreatedDate" HeaderText="CreatedDate" DataField="CreatedDate" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
						</Columns>
					</MasterTableView>
				</telerik:RadGrid>
				<telerik:RadGrid ID="rgThreads" runat="server" AutoGenerateColumns="false" AllowSorting="true" Skin="Web20" AllowPaging="true" PageSize="20" >
					<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true">
						<Resizing AllowColumnResize="true" />
					</ClientSettings>
					<MasterTableView DataKeyNames="ThreadID">
						<NoRecordsTemplate>
							<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="NormalBold" />
						</NoRecordsTemplate>
						<Columns>
							<telerik:GridBoundColumn UniqueName="Email" HeaderText="Email" DataField="Email" />
							<telerik:GridBoundColumn UniqueName="Username" HeaderText="Username" DataField="Username" />	
							<telerik:GridBoundColumn UniqueName="Subject" HeaderText="Subject" DataField="Subject" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
							<telerik:GridBoundColumn UniqueName="CreatedDate" HeaderText="CreatedDate" DataField="CreatedDate" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
						</Columns>
					</MasterTableView>
				</telerik:RadGrid>
			</div>
		</td>
     </tr>
</table>