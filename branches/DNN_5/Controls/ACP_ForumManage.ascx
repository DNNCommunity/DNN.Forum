<%@ Control Inherits="DotNetNuke.Modules.Forum.ACP.ForumManage" CodeBehind="ACP_ForumManage.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="forum" TagName="ACPmenu" src="~/DesktopModules/Forum/Controls/ACP_Menu.ascx" %>
<div class="ACP-ForumManage">
	<table cellpadding="0" cellspacing="0" width="100%" border="0" class="Forum_SearchContainer" >
		<tr valign="top">
			<td class="Forum_UCP_Left">
			<forum:ACPmenu ID="ACPmenu" runat="server" />
		  </td>
			<td class="Forum_UCP_Right">
				<div class="dnnForm dnnClear">
					<h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" /></h2>
					<fieldset>
						<div class="dnnFormItem">
							<dnn:Label runat="server" ID="lblNewGroup" Suffix=":" />
							<div class="dnnLeft">
								<asp:TextBox ID="txtAddGroup" runat="server" />
								<asp:ImageButton ID="imgAddGroup" runat="server" />
								<asp:Label CssClass="dnnFormMessage dnnFormValidationSummary" runat="server" ID="lblvalAddGroup" resourcekey="lblvalAddGroup" Visible="False" />
							</div>
						</div>
						<div>
							<asp:DataList id="lstGroup" cellspacing="0" cellpadding="0" gridlines="None" borderwidth="0" datakeyfield="GroupID" width="100%" runat="server">
							<FooterTemplate>
								<div class="Forum_Row_Admin_Foot">&nbsp;</div>
							</FooterTemplate>
							<ItemStyle CssClass="Forum_Grid_Row_Alt" />
							<AlternatingItemStyle CssClass="Forum_Grid_Row" />
								<ItemTemplate>
									<table width="100%" cellpadding="0" cellspacing="0" border="0" runat="server" id="tblGroup" >
										<tr>
											 <td class="Forum_HeaderCapLeft">
											<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="imgSpacer"/>
										 </td>
											<td class="Forum_Header" width="100%">
												<table cellspacing="0" cellpadding="0" width="100%" runat="server" id="tblGroupEditHeaderClosed">
													<tr>
														<td width="20px" align="center" valign="middle">
															<asp:ImageButton id="imgExpand" resourcekey="imgExpand" runat="server" />
														</td>
														<td align="left" valign="middle">
															<asp:Label CssClass="Forum_HeaderText" runat="server" ID="lblGroupName" />&nbsp;
															<asp:Label CssClass="Forum_HeaderText" runat="server" ID="lblForumCount" />
														</td>
														<td align="right" width="75px">
															<asp:Label CssClass="Forum_HeaderText" runat="server" ID="lblCreatedDate" />
														</td>
														<td align="right" width="125px">
															<asp:ImageButton id="imgGroupDelete" resourcekey="imgGroupDelete" runat="server" CommandName="delete" />
															<asp:ImageButton id="imgGroupEdit" resourcekey="imgGroupEdit" runat="server" />
															<asp:ImageButton id="imgGroupUp" resourcekey="imgGroupUp" runat="server" CommandName="up" />
															<asp:ImageButton id="imgGroupDown" resourcekey="imgGroupDown" runat="server" CommandName="down" />
														</td>
													</tr>
												</table>
											</td>
										 <td class="Forum_HeaderCapRight">
											<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="Image1"/>
										 </td>
										</tr>
									</table>
								</ItemTemplate>
								<EditItemTemplate>
									<table width="100%" cellpadding="0" cellspacing="0" border="0" runat="server" id="tblGroup">
										<tr>
											 <td class="Forum_HeaderCapLeft">
											<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="imgSpacer"/>
										 </td>
											<td class="Forum_Header" width="100%">
												<table cellspacing="0" cellpadding="0" width="100%">
													<tr>
														<td align="left">
															<asp:TextBox CssClass="Forum_NormalTextBox" runat="server" ID="txtGroupName" />
														</td>
														<td align="right">
															<asp:ImageButton id="imgGroupSave" resourcekey="imgGroupSave" runat="server" CommandName="save" />
															<asp:ImageButton id="imgGroupCancel" resourcekey="imgGroupCancel" runat="server" CommandName="cancel" />
														</td>
													</tr>
												</table>
											</td>
											<td class="Forum_HeaderCapRight">
												<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="Image2"/>
										 </td>
										</tr>
									</table>
								</EditItemTemplate>
								<SelectedItemTemplate>
									<table id="tblGroupSelected" cellspacing="0" cellpadding="0" width="100%">
										<tr>
											<td class="Forum_HeaderCapLeft">
											<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="imgSpacer"/>
										 </td>
											<td width="100%" class="Forum_Header">
												<table cellspacing="0" cellpadding="0" width="100%" runat="server" id="tblGroupEditHeaderOpen">
													<tr>
														<td width="20px" align="center">
															<asp:ImageButton id="imgCloseGroup" resourcekey="imgCloseGroup" runat="server" />
														</td>
														<td align="left">
															<asp:Label id="lblGroupName" CssClass="Forum_HeaderText" Width="100%" runat="server" />
														</td>
														<td align="right">
															<asp:ImageButton id="imgForumAdd" resourcekey="imgForumAdd" runat="server" OnClick="AddForum_Click" />
														</td>
													</tr>
												</table>
											</td>
											<td class="Forum_HeaderCapRight">
											<asp:image ImageUrl='<%# SpacerImage() %>' runat="server" ID="Image3"/>
										 </td>
										</tr>
										<tr>
											  <td width="100%" colspan="3">
												<table id="tblGroupEditDetails" cellspacing="0" cellpadding="0" width="100%">
													  <tr>
														<td width="100%">
															  <table id="tblGroupDetails" cellspacing="0" cellpadding="0" width="100%">
																  <tr>
																	  <td align="left" valign="top" class="Forum_Row_Admin">
																		<asp:DataList ID="lstForum" runat="server" OnItemDataBound="lstForum_ItemDataBound" Width="100%">
																			<ItemTemplate>
																				<table width="100%" cellpadding="0" cellspacing="0">
																					  <tr>
																						  <td width="80%">
																								<asp:Label CssClass="Forum_NormalBold" runat="server" ID="lblForumName" />
																						  </td>
																						  <td width="60px" align="right" nowrap="nowrap">
																						<asp:Label CssClass="Forum_NormalBold" ID="lblTotalPosts" runat="server" />
																						</td>
																						  <td width="60px" align="right" nowrap="nowrap">
																						<asp:Image id="imgEnabled" runat="server"/>
																					</td>
																						  <td width="130px" align="right" nowrap="nowrap">
																							   <asp:ImageButton id="imgForumEdit" resourcekey="imgForumEdit" runat="server" CommandName="edit" OnClick="EditForum_Click" />
																							  <asp:ImageButton id="imgForumDelete" resourcekey="imgForumDelete" runat="server" CommandName="delete" OnClick="deleteForum_Click" />
																							  <asp:ImageButton id="imgForumUp" resourcekey="imgForumUp" runat="server" CommandName="up" OnClick="ForumUp_Click"  />
																							  <asp:ImageButton id="imgForumDown" resourcekey="imgForumDown" runat="server" CommandName="down" OnClick="ForumDown_Click" />
																						  </td>
																					</tr>
																					</table>
																					<asp:DataList ID="lstSubForum" runat="server" OnItemDataBound="lstSubForum_ItemDataBound" Width="100%">
																					<ItemTemplate>
																							<table width="100%" cellpadding="0" cellspacing="0">
																								<tr>
																									<td valign="top" width="1px" nowrap="nowrap">
																										<asp:image id="imgSubSpacer" runat="server" />
																								   </td>
																									<td width="15px" align="center" nowrap="nowrap">
																									</td>
																									<td width="80%">
																										<asp:Label CssClass="Forum_NormalBold" runat="server" ID="lblSubForumName" />
																									</td>
																									<td width="60px" align="right" nowrap="nowrap">
																									<asp:Label CssClass="Forum_NormalBold" ID="lblSubTotalPosts" runat="server" />
																									</td>
																									<td width="60px" align="right" nowrap="nowrap">
																									<asp:Image id="imgSubEnabled" runat="server"/>
																								</td>
																									<td width="130px" align="right" nowrap="nowrap">
																										<asp:ImageButton id="imgSubForumEdit" resourcekey="imgForumEdit" runat="server" CommandName="edit" OnClick="EditForum_Click" />
																										<asp:ImageButton id="imgSubForumDelete" resourcekey="imgForumDelete" runat="server" CommandName="delete" OnClick="deleteForum_Click" />
																										<asp:ImageButton id="imgSubForumUp" resourcekey="imgForumUp" runat="server" CommandName="up" OnClick="ForumUp_Click"  />
																										<asp:ImageButton id="imgSubForumDown" resourcekey="imgForumDown" runat="server" CommandName="down" OnClick="ForumDown_Click" />
																									</td>
																								</tr>
																							</table>
																						</ItemTemplate>
																					</asp:DataList>
																				</ItemTemplate>
																			</asp:DataList>
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
													</table>
												</td>
										  </tr>
									</table>
								</SelectedItemTemplate>
							</asp:DataList>
						</div>
					</fieldset>
				</div>
			</td>
		</tr>
		<tr>
			<td align="center" colspan="2"><asp:HyperLink ID="cmdHome" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdHome" /></td>
		</tr>
	</table>
</div>