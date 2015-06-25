<%@ Control Inherits="DotNetNuke.Modules.Forum.ACP.User" CodeBehind="ACP_User.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm acpUser dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="lblSearch" resourcekey="Search" runat="server" EnableViewState="false" />
			<asp:textbox id="txtSearch" Runat="server" />
			<asp:DropDownList ID="ddlRoles" Visible="false" runat="server" AutoPostBack="true" />
			<asp:DropDownList id="ddlSearchType" runat="server" AutoPostBack="true" />
		</div>
		<div>
			<dnnweb:DnnGrid runat="server" ID="dnngridUsers" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" Skin="Sitefinity" PageSize="25" >
				<ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" />
				<MasterTableView>
					<NoRecordsTemplate>
						<div class="dnnFormMessage dnnFormWarning">
							<asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" />
						</div>
					</NoRecordsTemplate>
					<Columns>
						<dnnweb:DnnGridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/edit.gif" UniqueName="imgEdit" CommandName="EditUser" HeaderText="Edit" />
						<dnnweb:DnnGridBoundColumn UniqueName="Username" DataField="Username" HeaderText="Username" />
						<dnnweb:DnnGridBoundColumn UniqueName="DisplayName" DataField="DisplayName" HeaderText="DisplayName" />
						<dnnweb:DnnGridHyperlinkColumn UniqueName="hlEmail" HeaderText="Email" DataTextField="Email" DataNavigateUrlFields="Email" DataNavigateUrlFormatString="mailto:{0}" />
					</Columns>
				</MasterTableView>			
			</dnnweb:DnnGrid>
		</div>
	</fieldset>
</div>