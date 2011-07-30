<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="Forum_PostModerate.ascx.vb" AutoEventWireup="true" Inherits="DotNetNuke.Modules.Forum.PostModerate" %>
<div class="dnnForm forumPostModerate dnnClear">
	<h2 class="dnnFormSectionHead"><asp:hyperlink id="hlForum" Runat="server" Target="_blank" /></h2>
	<div>
		<asp:datalist id="lstPost" runat="server" DataKeyField="PostID" CellPadding="0" Width="100%">
			<ItemTemplate>
				<table width="100%" cellpadding="0" cellspacing="0" border="0" runat="server" id="tblPostList">
					<tr>
						<td width="100%" colspan="2">
							<table cellpadding="0" cellspacing="0" border="0" width="100%" id="">
								<tr>
									<td class="Forum_HeaderCapLeft">
										<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="Image1"/>
									</td>
									<td align="left" width="100%" class="Forum_Header">
										<asp:hyperlink CssClass="Forum_HeaderText" NavigateUrl='<%# ThreadLink(CType(DataBinder.Eval(Container.DataItem, "ThreadID"), Integer), CType(DataBinder.Eval(Container.DataItem, "ForumID"), Integer), CType(DataBinder.Eval(Container.DataItem, "ParentPostID"), Integer))  %>' runat="server" ID="lblSubject" Target="_blank" Text='<%# FormatCreatedDate(DataBinder.Eval(Container.DataItem, "CreatedDate")) %>' />
									</td>
									<td class="Forum_HeaderCapRight">
										<asp:Image ImageUrl='<%# SpacerImage() %>' runat="server" ID="Image2" />
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td width="100%">
							<table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblPost">
								<tr>
									<td class="Forum_Avatar" width="25%" align="center" valign="top">
										<table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblAvatar">
											<tr>
												<td width="100%" align="center">
													<asp:HyperLink CssClass="Forum_Profile" Text='<%# FormatUserAlias(CType(DataBinder.Eval(Container.DataItem, "UserID"), Integer)) %>' runat="server" ID="lblAlias" NavigateURL='<%# UserProfileLink(CType(DataBinder.Eval(Container.DataItem, "UserID"), Integer)) %>' target="_blank" />
													<asp:HyperLink ID="hlEdit" runat="server" NavigateUrl='<%# EditProfileLink(DataBinder.Eval(Container.DataItem, "UserID")) %>'>
														<asp:Image ID="imgEdit" runat="server" ImageUrl='<%# EditImage() %>' />
													</asp:HyperLink>
												</td>
											</tr>
											<tr>
												<td align="center">
													<asp:Label CssClass="Forum_Normal" Text='<%# FormatUserPostCount(CType(DataBinder.Eval(Container.DataItem, "UserID"), Integer))  %>' runat="server" ID="lblUserPosts" />
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
									<td id="EmptyArea" class="Forum_PostBody_Container" width="75%">
										<table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblPostBody">
											<tr>
												<td class="Forum_PostDetails">
													<asp:Label CssClass="Forum_NormalBold" Text='<%# (HttpUtility.HtmlEncode(CType(DataBinder.Eval(Container.DataItem, "Subject"), String))) %>' runat="server" ID="lblCreatedDate" />
												</td>
											</tr>
											<tr>
												<td id="PostCreatedDetails" class="Forum_PostButtons" width="75%" align="right">
													<table cellpadding="0" cellspacing="4" border="0" id="tblButtons">
														<tr>
															<td class="Forum_ReplyCell">
																<asp:LinkButton id="cmdSplit" runat="server" CommandName="Split" CommandArgument='<%# CType(DataBinder.Eval(Container.DataItem, "PostID"), String) %>' resourcekey="cmdSplit" CssClass="Forum_Link" Enabled='<%# ThreadCanSplit(DataBinder.Eval(Container.DataItem, "PostID")) %>'/></td>		
															<td class="Forum_ReplyCell">								          
																<asp:LinkButton id="cmdMove" runat="server" CommandName="Move" CommandArgument='<%# CType(DataBinder.Eval(Container.DataItem, "PostID"), String) %>' resourcekey="cmdMove" CssClass="Forum_Link" Enabled='<%# ThreadCanMove(DataBinder.Eval(Container.DataItem, "PostID")) %>'/></td>								          
															<td class="Forum_ReplyCell">
																<asp:LinkButton id="cmdDelete" runat="server" CommandName="Delete" CommandArgument='<%# CType(DataBinder.Eval(Container.DataItem, "PostID"), String) %>' resourcekey="cmdDelete" CssClass="Forum_Link"/></td>
															<td class="Forum_ReplyCell">
																<asp:LinkButton id="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# CType(DataBinder.Eval(Container.DataItem, "PostID"), String) %>' resourcekey="cmdApproveEdit"	CssClass="Forum_Link"/></td>
															<td class="Forum_ReplyCell">
																<asp:LinkButton id="btnApproveRespond" runat="server" CommandName="ApproveRespond" CommandArgument='<%# CType(DataBinder.Eval(Container.DataItem, "PostID"), String) %>' resourcekey="cmdApproveReply" CssClass="Forum_Link" /></td>								            								                                                                <td class="Forum_ReplyCell">
																<asp:LinkButton id="cmdApprove" runat="server" CommandName="Approve" CommandArgument='<%# CType(DataBinder.Eval(Container.DataItem, "PostID"), String) %>' resourcekey="cmdApprove" CssClass="Forum_Link"/></td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td class="Forum_PostBody" width="75%">
													<table>
														<tr>
															<td width="100%">
																<asp:Label CssClass="Forum_Normal" Text='<%# FormatBody(DataBinder.Eval(Container.DataItem,"Body")) %>' runat="server" ID="lblPostBody" />
															</td>
														</tr>
														<tr>
															<td align="center">
																<br />
																<asp:Label ID="lblQuestion" runat="server" CssClass="Forum_Normal" /><br />
																<asp:RadioButtonList ID="rblstAnswers" runat="server" CssClass="Forum_NormalTextBox" /><br />
																<asp:Label ID="lblEndDate" runat="server" CssClass="Forum_Normal" /><br />
																<asp:Label ID="lblTakenMessage" runat="server" CssClass="Forum_Normal" />
															</td>
														</tr>
													</table>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</ItemTemplate>
		</asp:datalist>
	</div>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton id="cmdBack" runat="server" CssClass="dnnPrimaryAction" resourcekey="cmdCancel" /></li>
	</ul>
</div>