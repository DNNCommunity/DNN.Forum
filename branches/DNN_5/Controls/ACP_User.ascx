<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.ACP.User" CodeBehind="ACP_User.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="ACP-User">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td align="left" width="75" valign="middle">
							<asp:label id="lblSearch" cssclass="Forum_NormalBold" resourcekey="Search" runat="server" EnableViewState="false" />
						</td>
						<td align="left" valign="middle">
							<table>
								<tr>
									<td align="left">
										<asp:textbox id="txtSearch" Runat="server" class="Forum_NormalTextBox" Width="200px" />
                                        <dnnweb:DnnComboBox ID="ddlRoles" Width="200px" Visible="false" runat="server" AutoPostBack="true" />
									</td>
									<td align="left">
                                        <dnnweb:DnnComboBox id="ddlSearchType" runat="server" Width="200px" AutoPostBack="true" />
									</td>
									<td align="left">
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td colspan="2"></td>
					</tr>
					<tr>
						<td colspan="2" width="100%" align="center">
                            <dnnweb:DnnGrid runat="server" ID="dnngridUsers" AllowPaging="true" AllowSorting="false" AutoGenerateColumns="false" AllowCustomPaging="true" GridLines="None" PageSize="25" PagerStyle-AlwaysVisible="true" PagerStyle-Mode="NextPrevNumericAndAdvanced">
                                <ClientSettings AllowColumnsReorder="false" EnableRowHoverStyle="true" />
							    <MasterTableView DataKeyNames="UserID" >
                                    <NoRecordsTemplate>
								    <asp:Label ID="lblNoRecords" runat="server" resourcekey="lblNoRecords" CssClass="Normal" />
							    </NoRecordsTemplate>
								    <Columns>
									    <wrapper:GridButtonColumn ButtonType="ImageButton" ImageUrl="~/images/edit.gif" UniqueName="imgEdit" CommandName="EditUser" HeaderText="Edit" />
									    <dnnweb:DnnGridBoundColumn UniqueName="Username" DataField="Username" HeaderText="Username" />
									    <dnnweb:DnnGridBoundColumn UniqueName="DisplayName" DataField="DisplayName" HeaderText="DisplayName" />
									    <dnnweb:DnnGridHyperlinkColumn UniqueName="hlEmail" HeaderText="Email" DataTextField="Email" DataNavigateUrlFields="Email" DataNavigateUrlFormatString="mailto:{0}" />
								    </Columns>
							    </MasterTableView>			
						    </dnnweb:DnnGrid>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>