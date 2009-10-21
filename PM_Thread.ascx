<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Control Language="vb" Codebehind="PM_Thread.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.PMThread" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<asp:Panel ID="pnlContainer" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" border="0">
        <tr>
            <td>
                <asp:DataList ID="lstPost" runat="server" DataKeyField="PMID" CellPadding="0" width="100%">
                    <ItemTemplate>
                        <table cellpadding="0" cellspacing="0" width="100%" runat="server" id="tblPostList">
                            <tr>
                                <td class="Forum_HeaderCapLeft">
							<asp:Image ID="imgHeadSpacerL" runat="server" />
                                </td>
                                <td class="Forum_Header" width="100%">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblPostHeader">
                                        <tr>
                                            <td width="100%">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td valign="middle">
                                                            <asp:Image runat="server" ID="imgPostRead" />
                                                        </td>
                                                        <td align="left" width="70%" valign="middle">
                                                            &nbsp;<asp:Label CssClass="Forum_HeaderText" runat="server" ID="lblCreatedDate" />
                                                        </td>
                                                        <td align="right" width="30%">
                                                            <asp:Label CssClass="Forum_HeaderText" runat="server" ID="Label1" resourcekey="To"/>&nbsp;
                                                            <asp:HyperLink CssClass="Forum_Link" runat="server" ID="hlToUser" Target="_blank" />&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <td class="Forum_HeaderCapRight">
								<asp:Image ID="imgHeadSpacerR" runat="server" />
							 </td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%" id="tblPost">
                                        <tr>
                                            <td class="Forum_Avatar" width="25%" align="center" valign="top">
                                                <table cellpadding="0" cellspacing="0" border="0" class="Forum_PostAuthorTable" width="100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="1">
                                                                <tr>
                                                                    <td><asp:Image ID="imgOnline" runat="server" /></td>
                                                                    <td><asp:HyperLink CssClass="Forum_Profile" runat="server" ID="lblAlias" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td><asp:Image id="imgRank" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td><br /><asp:Image id="userAvatar" runat="server" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br /><asp:Label CssClass="Forum_NormalSmall" Text='Member Since' runat="server" ID="lblMemberSince"
                                                                resourcekey="lblMemberSince" />
                                                            <asp:Label CssClass="Forum_NormalSmall" runat="server" ID="lblUserJoinedDate" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td class="Forum_PostBody_Container" width="75%" align="left" valign="top">
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td id="cellSubject" class="Forum_PostDetails">
                                                            <asp:Label CssClass="Forum_NormalBold" runat="server" ID="lblPMSubject" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="PostCreatedDetails" class="Forum_PostButtons" align="right">
                                                            <table cellpadding="0" cellspacing="4" border="0" id="tblButtons">
                                                                <tr>
                                                                    <td class="Forum_ReplyCell">
                                                                        <asp:LinkButton ID="cmdReply" runat="server" CssClass="Forum_Link" /></td>
                                                                    <td class="Forum_ReplyCell">
                                                                        <asp:LinkButton ID="cmdQuote" runat="server" CssClass="Forum_Link" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="Forum_PostBody">
                                                            <asp:Label CssClass="Forum_Normal" runat="server" ID="lblPostBody" />
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
                    <SeparatorTemplate>
					<table cellspacing="0" cellpadding="0" width="100%">
						<tr>
							<td class="Forum_SpacerRow">&nbsp;</td>
						</tr>
					</table>
                    </SeparatorTemplate>
                </asp:DataList>
            </td>
        </tr>
        <tr>
            <td class="Forum_SpacerRow"><dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/></td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label CssClass="NormalRed" runat="server" ID="lblDeleted" />
                <br /><br />
                <asp:LinkButton ID="cmdCancel" runat="server" CssClass="CommandButton" resourcekey="cmdCancel" />
            </td>
        </tr>
    </table>
</asp:Panel>
