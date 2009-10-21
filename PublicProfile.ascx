<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="PublicProfile.ascx.vb" Inherits="DotNetNuke.Modules.Forum.PublicProfile" %>
<%@ Register Src="~/DesktopModules/Forum/controls/AvatarControl.ascx" TagName="AvatarControl" TagPrefix="forum" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="Forum_HeaderCapLeft">
					<asp:Image ID="imgSpc1" runat="server" />
                    </td>
                    <td class="Forum_Header">
					<asp:Label ID="lblProfile" runat="server" CssClass="Forum_HeaderText" />
                    </td>
                    <td class="Forum_HeaderCapRight">
					<asp:Image ID="imgSpc2" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="Forum_Row_Admin">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr valign="top">
                                <td width="60%">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr valign="top">
				                        <td width="40%">
										<asp:label id="lblAlias" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblAlias" />
				                        </td>
				                        <td width="60%">
										<asp:label id="txtAlias" runat="server" CssClass="Forum_Normal" />
				                        </td>
				                    </tr>
                                        <tr id="rowPMUser" runat="server" valign="top">
                                            <td>
										<asp:Label ID="lblPMUser" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblPMUser" />
                                            </td>
                                            <td>
						                    <asp:linkbutton id="cmdPMUser" runat="server" CssClass="Forum_Profile" resourcekey="cmdPMUser" />
						               </td>
								</tr>
								<tr valign="top">
								    <td>
									  <asp:label id="lblEmail" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblEmail" />
								    </td>
								    <td>
									    <asp:Literal ID="litEmail" runat="server" />
								    </td>
							    </tr>
								<tr id="rowUserWeb" runat="server" valign="top">
								    <td>
									  <asp:label id="lblURL" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblURL" />
								    </td>
								    <td>
									    <asp:hyperlink id="lnkWWW" runat="server" CssClass="Forum_Profile" Target="_blank" />
								    </td>
							    </tr>
								<tr valign="top">
				                        <td>
			                                <asp:label id="lblIM" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblIM" />
					                    </td>
				                        <td>
				                            <asp:label id="txtIM" runat="server" CssClass="Forum_Normal" />
				                         </td>
			                        </tr>
								<tr id="rowStats" runat="server" valign="top">
				                        <td>
			                                <asp:label id="lblStat" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblStat" />
					                    </td>
				                        <td class="Forum_Normal">
					                        <p>
					                            <asp:label id="lblStatistic" Runat="server" CssClass="Forum_Normal" /><br/>
						                       <asp:label id="lblJoinedDate" CssClass="Forum_Normal" Runat="server" />
					                        </p>
				                        </td>
			                        </tr>
                                        <tr valign="top">
                                            <td>
                                                <asp:Label ID="lblPCount" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblPCount" />
                                            </td>
                                            <td align="left">
                                                <asp:label id="lblPostCount" CssClass="Forum_Normal" Runat="server" />
                                            </td>
                                        </tr>
				                    <tr id="rowPostLink" runat="server" valign="top">
					                        <td>
				                                <asp:Label ID="lblViewPost" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblViewPost" />
					                        </td>
					                        <td>
						                        <p>
							                        <asp:hyperlink id="lnkUserPosts" runat="server" CssClass="Forum_Profile" resourcekey="lnkUserPosts">[lnkUserPosts]</asp:hyperlink>
							                   </p>
					                        </td>
				                        </tr>
                                        <tr id="rowUserSignature" runat="server" valign="top">
                                            <td>
										<asp:Label ID="lblSig" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblSig" />
                                            </td>
                                            <td>
										<asp:Label ID="lblSignature" runat="server" CssClass="Forum_NormalBold" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="1%"></td>
                                <td width="39%" align="center">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr id="rowRanking" runat="server" valign="top">
                                            <td width="50%" align="left">
                                                    <asp:Label ID="lblRanking" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblRanking" />
                                             </td>
                                            <td width="50%">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
												<asp:Image ID="imgRanking" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
												<asp:Label ID="lblRankTitle" runat="server" CssClass="Forum_Normal" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="rowUserAvatar" runat="server" valign="top">
                                            <td align="left">
                                                <asp:Label ID="lblAvatar" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblAvatar" />
                                             </td>
                                            <td>
                                            <asp:Image ID="imgAvatar" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="rowSystemAvatar" runat="server" valign="top">
                                            <td align="left">
                                                <asp:Label ID="lblSystemAvatar" runat="server" CssClass="Forum_Row_AdminText" resourcekey="lblSystemAvatar" />
                                            </td>
                                            <td>
									<asp:Image ID="imgSystemAvatar" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="Forum_HeaderText" align="left" width="100%">
									<dnn:sectionhead id="dshBiography" runat="server" CssClass="Forum_HeaderText" resourcekey="Biography" isExpanded="False" section="tblBiography"></dnn:sectionhead>
				                                <table id="tblBiography" runat="server" cellpadding="3" cellspacing="1" width="100%">
				                                    <tr>
				                                        <td width="76%" align="left" class="Forum_Normal">
													<asp:Literal ID="litBiography" runat="server" />
												</td>
				                                    </tr>
				                                </table>
				                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <tr id="rowModeratorAdmin" valign="top" runat="server">
		                                    <td class="Forum_HeaderText" align="left" width="100%">
		                                        <dnn:sectionhead id="dshModeratorSettings" runat="server" cssclass="Forum_HeaderText" resourcekey="ModeratorSettings" isExpanded="False" section="tblModerator"></dnn:sectionhead>
			                                    <br />
			                                    <table id="tblModerator" cellspacing="0" cellpadding="0" width="100%" runat="server">
				                                    <tr>
					                                    <td width="160px">
					                                    <span class="Forum_Row_AdminText">
					                                    <dnn:label id="plIsTrusted" runat="server" controlname="chkIsTrusted" Suffix=":"></dnn:label>
						                                    </span></td>
					                                    <td align="left">
					                                    <asp:checkbox id="chkIsTrusted" runat="server" CssClass="Forum_NormalTextBox" />
					                                    </td>
				                                    </tr>
                                                    <tr id="rowEditUserSig" runat="server" valign="top">
                                                           <td>
                                                        <span class="Forum_Row_AdminText">
												<dnn:label id="plEditUserSig" runat="server" controlname="txtSignature" Suffix=":"></dnn:label>
                                                        </span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtSignature" runat="server" CssClass="Forum_NormalTextBox" Rows="4" TextMode="MultiLine" Width="250px" />
                                                       </td>
                                                    </tr>
                                                    	<tr id="rowModifyUserAvatar" runat="server">
												<td width="175" valign="top">
													<span class="Forum_Row_AdminText">
														<dnn:label id="plAvatar" runat="server" suffix=":" controlname="ctlUserAvatar"></dnn:label>
													</span>
												</td>
												<td align="left" valign="top">
													<forum:avatarcontrol id="ctlUserAvatar" runat="server"></forum:avatarcontrol>
												</td>
											</tr>
				                                    <tr id="rowUserBanning" runat="server" valign="top">
					                                    <td><span class="Forum_Row_AdminText">
													<dnn:label id="plUserBanned" runat="server" controlname="chkUserBanned" Suffix=":"></dnn:label>
						                                    </span>
						                              </td>
					                                    <td>
					                                    <asp:checkbox id="chkUserBanned" runat="server" CssClass="Forum_NormalTextBox" Enabled="False" />&nbsp;
					                                    <asp:Label ID="lblLiftBan" runat="server" CssClass="Forum_Normal" />
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
        </td>
     </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td class="Forum_FooterCapLeft">
					<asp:Image ID="imgSpc3" runat="server" />
				</td>
                    <td class="Forum_Footer">&nbsp;</td>
                    <td class="Forum_FooterCapRight">
					<asp:Image ID="imgSpc4" runat="server" />
				</td>
                </tr>
            </table>
        </td>
     </tr>
     <tr id="rowFooter" runat="server">
        <td align="center">
	       <asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" CssClass="CommandButton" />&nbsp;
            <asp:linkbutton id="cmdCancel" runat="server" resourcekey="cmdCancel" CssClass="CommandButton" />&nbsp;
            <asp:linkbutton cssclass="CommandButton" id="cmdManageUser" runat="server" resourcekey="cmdManageUser" />
        </td>
     </tr>
</table>

	
		

