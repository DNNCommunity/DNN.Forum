<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.ACP.FilterWord" CodeBehind="ACP_FilterWord.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td>
			<table class="" id="tblManageFilters" runat="server" cellspacing="0" cellpadding="0" width="100%">
				<tr>
					<td valign="top" align="center" class="Forum_Row_Admin">  
						<table border="0" cellpadding="0" cellspacing="0" width="100%">
							<tr>
								<td align="left" width="70%">
									<table border="0">
										<tr>
											<td>
												<asp:Label runat="server" ID="lblNewWord" resourcekey="lblNewWord" CssClass="Forum_NormalBold" EnableViewState="false" />
											</td>
											<td>
												<asp:TextBox ID="txtNewBadWord" runat="server" CssClass="Forum_NormalTextBox" Width="200px" />
											</td>
											<td></td>
									   </tr>
									   <tr>
											<td>
												<asp:Label runat="server" ID="lblNewReplaceWord" resourcekey="lblNewReplaceWord" CssClass="Forum_NormalBold" EnableViewState="false" />
											</td>
											<td>
												<asp:TextBox ID="txtNewReplaceWord" runat="server" CssClass="Forum_NormalTextBox" Width="200px" />
											</td>
											<td>
												<asp:ImageButton ID="imgAdd" runat="server" />
											</td>
									   </tr>
								    </table>
								</td>
								<td align="right" width="30%" valign="top">
								    <table border="0">
									   <tr>
											<td>
												<asp:Label runat="server" ID="lblFilter" resourcekey="lblFilter" CssClass="Forum_NormalBold" EnableViewState="false" />
											</td>
											<td>
												<asp:dropdownlist id="ddlSearchType" Runat="server" class="Forum_NormalTextBox" AutoPostBack="True" />
											</td>
									   </tr>
								    </table>
								</td>
							</tr>
						</table>
						 <table border="0" width="100%">
						    <tr>
						        <td align="center">
						            <asp:Label ID="lblNoResults" Runat="server" CssClass="Forum_NormalBold" resourcekey="lblNoResults"/>
						            <asp:datagrid id="grdBadWords" Runat="server" GridLines="None" width="100%" cssclass="Forum_Grid" CellPadding="0" AutoGenerateColumns="false" DataKeyField="ItemID" >
										<HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Left"/>
										<ItemStyle CssClass="Forum_Grid_Row_Alt" HorizontalAlign="Left" />
										<AlternatingItemStyle CssClass="Forum_Grid_Row" HorizontalAlign="Left" />
							            <Columns>
								            <asp:BoundColumn DataField="BadWord" HeaderText="BadWord" ItemStyle-CssClass="Forum_Grid_Left" />
								            <asp:BoundColumn DataField="ReplacedWord" HeaderText="ReplaceWord" ItemStyle-CssClass="Forum_Grid_Middle" />
								            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="75px" ItemStyle-CssClass="Forum_Grid_Right" >
									            <itemtemplate>
										            <asp:imagebutton id="cmdEdit" runat="server" causesvalidation="false" commandname="Edit" resourcekey="imgEdit" EnableViewState="false" />
										            <asp:imagebutton id="cmdDelete" runat="server" causesvalidation="false" commandname="Delete" resourcekey="imgDelete" EnableViewState="false" />
									            </itemtemplate>
									            <edititemtemplate>
										            <asp:imagebutton id="cmdUpdate" runat="server" causesvalidation="false" commandname="Update" resourcekey="imgUpdate" EnableViewState="false" />
										            <asp:imagebutton id="cmdCancel" runat="server" causesvalidation="false" commandname="Cancel" resourcekey="imgCancel" EnableViewState="false" />
									            </edititemtemplate>
								            </asp:TemplateColumn>
							            </Columns>
						            </asp:datagrid>
						            <dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
						        </td>
						    </tr>
						 </table>
						
					</td>
				</tr>
			</table>
			<div class="Forum_Row_Admin_Foot" align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
			</div>
		</td>
     </tr>
</table>