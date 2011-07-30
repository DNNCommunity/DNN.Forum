<%@ Control language="vb" CodeBehind="Forum_ThreadSplit.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.ThreadSplit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
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
<div class="dnnForm forumThreadSplit dnnClear">
	<h2 class="dnnFormSectionHead"><asp:Label ID="lblTitleSubject" runat="server" resourcekey="lblTitleSubject" /></h2>
	<div class="dnnFormItem">
		<dnn:label id="plSubject" runat="server" controlname="txtSubject" Suffix=":" />
		<asp:textbox id="txtSubject" runat="server" />
	</div>
	<div class="dnnFormItem">
		<dnn:Label ID="plPost" runat="server" ControlName="lblPost" Suffix=":" />
		<div class="dnnLeft"><asp:Label ID="lblPost" runat="server" /></div>
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plNewForum" runat="server" controlname="txtctlForumLookup" Suffix=":" />
		<div class="dnnLeft">
			<dnnweb:DnnTreeView ID="rtvForums" runat="server" CheckBoxes="true" OnClientNodeChecked="clientNodeChecked" />
		</div>
	</div>
	<div class="dnnFormItem">
		<dnn:label id="plEmailUsers" runat="server" controlname="chkEmailUsers" Suffix=":" />
		<asp:checkbox id="chkEmailUsers" runat="server" />
	</div>
	<div>
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
										<td class="Forum_HeaderCapRight">
											<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="imgSpacer2"/>
										</td>
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
												<td align="left">
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
		</asp:DataList>
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdMove" runat="server" resourcekey="cmdMove" /></li>
		<li><asp:HyperLink cssclass="dnnSecondaryAction" id="cmdCancel" runat="server" resourcekey="cmdCancel" /></li>
	</ul>
</div>