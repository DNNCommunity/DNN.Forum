<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_EmailSubscribers.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.EmailSubscribers" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm acpEmailSubscribers dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<asp:HiddenField runat="server" ID="hdnRcbForumsValue" />
		<dnnweb:DnnGrid ID="rgForums" runat="server" AllowSorting="true" AllowPaging="true" PageSize="10" ItemStyle-Height="30" AutoGenerateColumns="false" Skin="Sitefinity">
			<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="false">
					<Resizing AllowColumnResize="true" />
			</ClientSettings>
			<MasterTableView DataKeyNames="ForumID" CommandItemDisplay="Top" CommandItemStyle-Height="30">
				<CommandItemTemplate>
					&nbsp;&nbsp;<asp:Label runat="server" ID="lblForumDescription" EnableViewState="false" resourcekey="lblForumDescription" />&nbsp;&nbsp;
					<dnnweb:DnnComboBox ID="rcbForums" runat="server" Width="250" AutoPostBack="true" onselectedindexchanged="rcbForums_SelectedIndexChange" />
					<asp:Label runat="server" ID="lblForumTitle" EnableViewState="false" />
				</CommandItemTemplate>
				<NoRecordsTemplate>
					<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
				</NoRecordsTemplate>
				<Columns>
					<dnnweb:DnnGridBoundColumn UniqueName="Email" DataField="Email" HeaderText="Email" />
					<dnnweb:DnnGridBoundColumn UniqueName="Username" DataField="Username" HeaderText="Username" />	
					<dnnweb:DnnGridBoundColumn UniqueName="DisplayName" DataField="DisplayName" HeaderText="DisplayName" />
					<dnnweb:DnnGridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" HeaderText="CreatedDate" />
				</Columns>
			</MasterTableView>
		</dnnweb:DnnGrid>
		<br /><br />          
		<dnnweb:DnnGrid ID="rgThreads" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PageSize="10" ItemStyle-Height="30" Skin="Sitefinity">
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
					<dnnweb:DnnGridBoundColumn UniqueName="Email" DataField="Email" HeaderText="Email" />
					<dnnweb:DnnGridBoundColumn UniqueName="Username" DataField="Username" HeaderText="Username" />	
					<dnnweb:DnnGridBoundColumn UniqueName="DisplayName" DataField="DisplayName" HeaderText="DisplayName" />
					<dnnweb:DnnGridBoundColumn UniqueName="CreatedDate" DataField="CreatedDate" HeaderText="CreatedDate" />
				</Columns>
			</MasterTableView>
		</dnnweb:DnnGrid>
	</fieldset>
</div>