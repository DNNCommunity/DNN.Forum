<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.MemberList" CodeBehind="Forum_MemberList.ascx.vb" language="vb" AutoEventWireup="false" Explicit="True" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<asp:Panel ID="pnlContainer" runat="server">
    <div>
        <table width="100%" border="0" class="Forum_Row_Admin">
            <tr>
	            <td align="left" width="100" valign="middle">
	                <asp:label id="lblSearch" cssclass="Forum_HeaderText" resourcekey="Search" runat="server"></asp:label></td>
	            <td align="left" width="100%" valign="middle">
	                <table>
	                    <tr>
	                        <td align="left">
	                            <asp:textbox id="txtSearch" Runat="server" class="Forum_NormalTextBox" Width="200px"></asp:textbox></td>
	                        <td align="left"><asp:dropdownlist id="ddlSearchType" Runat="server" class="Forum_NormalTextBox" AutoPostBack="True"/></td>
	                        <td align="left">
	                            </td>
	                    </tr>
	                </table>
                </td>
            </tr>
            <tr>
	            <td colspan="2" align="center">
	                <asp:Label ID="lblNoResults" runat="server" CssClass="Forum_NormalBold" resourcekey="lblNoResults"/>
		            <asp:datagrid id="grdUsers" Runat="server" GridLines="None" width="100%" cssclass="Forum_Grid" CellPadding="2" AutoGenerateColumns="false" PageSize="10">
                        <HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
                        <ItemStyle CssClass="Forum_Grid_Row_Alt" />
                        <AlternatingItemStyle CssClass="Forum_Grid_Row" />
			            <columns>
            			    <asp:TemplateColumn>
                                <ItemTemplate>
                                    <asp:HyperLink id="hlEditUser" runat="server">
                                        <asp:Image id="imgEdit" runat="Server" />
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateColumn>
				            <asp:templatecolumn ItemStyle-Width="25px">
					            <itemtemplate>
						            <asp:image id="imgOnline" runat="Server" />
					            </itemtemplate>
				            </asp:templatecolumn>
				            <asp:templatecolumn headertext="SiteAlias" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="Forum_Grid_Left">
					            <itemtemplate>
						            <asp:HyperLink runat="server" ID="hlSiteAlias" CssClass="Forum_Profile"></asp:HyperLink>
					            </itemtemplate>
				            </asp:templatecolumn>
				            <asp:BoundColumn datafield="DisplayName" headertext="DisplayName" ItemStyle-CssClass="Forum_Grid_Middle"/>
				            <asp:TemplateColumn HeaderText="CreatedDate" ItemStyle-Width="150px" ItemStyle-CssClass="Forum_Grid_Middle">
					            <itemtemplate>
						            <asp:Label ID="lblCreatedDate" Runat="server"></asp:Label>
					            </itemtemplate>
				            </asp:TemplateColumn>
				            <asp:BoundColumn headertext="PostCount" DataField="PostCount" ItemStyle-Width="100px" ItemStyle-CssClass="Forum_Grid_Middle" />
				            <asp:TemplateColumn  ItemStyle-Width="100px" ItemStyle-CssClass="Forum_Grid_Right">
					            <itemtemplate>
						            <asp:HyperLink ID="hlMessage" Runat="server" resourcekey="cmdMessage" CssClass="Forum_Profile"></asp:HyperLink>
					            </itemtemplate>
				            </asp:TemplateColumn>
			            </columns>
		            </asp:datagrid>
		            <dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
	            </td>
	        </tr>
	    </table>           
        <div align="center" class="Forum_Row_Admin_Foot">
            <asp:linkbutton id="cmdCancel" CssClass="CommandButton" runat="server" resourcekey="cmdHome"></asp:linkbutton>
        </div>
	</div>
</asp:Panel>