<%@ Register TagPrefix="DNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<%@ Control language="vb" CodeBehind="Forum_ThreadSplit.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.ThreadSplit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<script type="text/javascript" language="javascript">
var oldNode;
function clientNodeChecked(sender, eventArgs)
{
   var node = eventArgs.get_node();
   
   if(oldNode != null)
   {
       oldNode.set_checked(false);
   }
   
   node.set_checked(true);
   oldNode = node;
}

function pageLoad()
{
	var tree = $find("<%= rtvForums.ClientID  %>");
   var checkedNodes = tree.get_checkedNodes();
   if (checkedNodes)
   {
//this will ensure the correct behavior even if a node is checked server-side
       oldNode = checkedNodes[0];
   }
}
</script>
<asp:Panel ID="pnlContainer" runat="server">
	<table class="" id="tblMain" cellspacing="0" cellpadding="0" width="100%"
		align="center">
		<tr>
			<td class="Forum_Row_AdminL">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plSubject" runat="server" controlname="txttxtSubject" Suffix=":"></dnn:label>
				</span></td>
			<td class="Forum_Row_AdminR" align="left">
				<asp:textbox id="txtSubject" runat="server" width="250" cssclass="Forum_NormalTextBox"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_AdminL">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plOldForum" runat="server" controlname="txtOldForum" Suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR" align="left">
				<asp:textbox id="txtOldForum" runat="server" ReadOnly="True" width="250" cssclass="Forum_NormalTextBox"></asp:textbox>
			</td>
		</tr>
        <tr>
            <td class="Forum_Row_AdminL" valign="top">
			<span class="Forum_Row_AdminText">
				<dnn:Label ID="plPost" runat="server" ControlName="lblPost" Suffix=":" />
                </span>
            </td>
            <td align="left" class="Forum_Row_AdminR">
                <asp:Label ID="lblPost" runat="server" CssClass="Forum_Normal"></asp:Label>
			</td>
        </tr>
		<tr id="rowForum" runat="server">
			<td class="Forum_Row_AdminL" valign="top">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plNewForum" runat="server" controlname="txtctlForumLookup" Suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR">
				<telerik:RadTreeView ID="rtvForums" runat="server" Skin="Web20" CheckBoxes="true" OnClientNodeChecked="clientNodeChecked" />
				<br/>
				<asp:label id="lblErrorMsg" Runat="server" CssClass="NormalRed"></asp:label>
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_AdminL" width="25%">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plEmailUsers" runat="server" controlname="chkEmailUsers" Suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR" width="75%">
				<asp:checkbox id="chkEmailUsers" runat="server" CssClass="Forum_NormalTextBox"></asp:checkbox>
			</td>
		</tr>
		<td colspan="2" class="Forum_Row_Admin_Foot" >&nbsp;</td>
        <tr>
            <td valign="top" colspan="2">
                <asp:DataList ID="dlPostsForThread" runat="server" Width="100%" DataKeyField="PostID">
                	<ItemTemplate>
                		<table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblPostsForThread">
							<tr>
								<td width="100%">
									<table cellpadding="0" cellspacing="0" border="0" width="100%" id="">
										<tr>
											<td class="Forum_HeaderCapLeft">
												<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="imgSpacer"/>
											</td>
											<td align="left" width="100%" class="Forum_Header">
												&nbsp;<asp:Label CssClass="Forum_HeaderText" runat="server" ID="lblSubject" Text='<%# FormatCreatedDate(DataBinder.Eval(Container.DataItem, "CreatedDate")) %>' />
    										</td>
    											<td class="Forum_HeaderCapRight"><asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="imgSpacer2"/></td>
										</tr>
									</table>
								</td>
							</tr>
														<tr>
								<td class="Forum_Row_AdminBox">
									<table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblPost">
										<tr>
											<td class="Forum_AvatarBox" rowspan="4" width="25%" align="center" valign="top">
												<table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblAvatar">
													<tr>
														<td width="100%" align="center">
															<asp:HyperLink CssClass="Forum_Profile" Text='<%# UserAlias(CType(DataBinder.Eval(Container.DataItem, "UserID"), Integer)) %>' runat="server" ID="lblAlias" NavigateURL='<%# UserProfileLink(CType(DataBinder.Eval(Container.DataItem, "UserID"), Integer)) %>' target="_blank" />
														</td>
													</tr>
													<tr>
														<td align="center">
															<asp:Label CssClass="Forum_Normal" Text='<%# PostCount(CType(DataBinder.Eval(Container.DataItem, "UserID"), Integer)) %>' runat="server" ID="lblUserPosts" />
														</td>
													</tr>
													<tr>
														<td align="center">
															<asp:Label CssClass="Forum_Normal" runat="server" ID="lblMemberSince" resourcekey="lblMemberSince" /><br />
															<asp:Label CssClass="Forum_Normal" Text='<%# FormatJoinedDate(CType(DataBinder.Eval(Container.DataItem, "UserID"), Integer))  %>' runat="server" ID="lblUserJoinedDate" />
														</td>
													</tr>
												</table>
											</td>
											<td id="EmptyArea" class="Forum_PostDetailsBox" width="75%">
												<asp:Label CssClass="Forum_NormalBold" Text='<%# (CType(DataBinder.Eval(Container.DataItem, "Subject"), String))%>' runat="server" ID="lblCreatedDate" />
											</td>
										</tr>
										<tr>
											<td id="PostCreatedDetails" class="Forum_PostButtonsBox" width="75%" align="right">
											    <table>
											        <tr>
											            <td align="Left">
											                <asp:CheckBox ID="chkThreadToSplit" runat="server" resourcekey="IncludePostSplit" CssClass="Forum_Normal" Enabled='<%# EnabledSelector(CType(DataBinder.Eval(Container.DataItem, "PostID"), Integer), CType(DataBinder.Eval(Container.DataItem, "ThreadID"), Integer)) %>' TextAlign="Left" />
											            </td>
											            <td>
											                <asp:image ImageUrl='<%# VisibleNeedsModImage(DataBinder.Eval(Container.DataItem, "IsApproved")) %>' runat="server" ID="imgPostModerated" resourcekey="PostApproved"/>
											            </td>
											        </tr>
											      </table>        							    
											</td>
										</tr>
										<tr>
											<td class="Forum_PostBodyBox" width="75%">
												<asp:Label CssClass="Forum_Normal" Text='<%# FormatBody(DataBinder.Eval(Container.DataItem,"PostID")) %>' runat="server" ID="lblPostBody" />
											</td>
										</tr>
										<tr>
											<td class="Forum_PostBodyBox" width="75%" align="right">
                                                <asp:image ImageUrl='<%# VisiblePostReported(CType(DataBinder.Eval(Container.DataItem, "PostReported"), Integer)) %>' runat="server" ID="imgPostReported" resourcekey="PostReported"/>
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</ItemTemplate>
                </asp:DataList></td>
        </tr>
		<tr>
			<td align="center" width="100%" colspan="2">
                <asp:linkbutton cssclass="CommandButton" id="cmdCancel" runat="server" resourcekey="cmdCancel"></asp:linkbutton>&nbsp;
							<asp:linkbutton cssclass="CommandButton" id="cmdMove" runat="server" resourcekey="cmdMove"></asp:linkbutton></td>

		</tr>
	</table>
</asp:Panel>