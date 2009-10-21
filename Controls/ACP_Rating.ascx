<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_Rating.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.Rating" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td>
	          <table id="tblRatings" cellspacing="0" cellpadding="0" width="100%" runat="server">
		          <tr>
			            <td class="Forum_Row_AdminL" width="25%">
		                    <span class="Forum_Row_AdminText">
					            <dnn:label id="plRatings" runat="server" Suffix=":" controlname="chkRatings"></dnn:label>
				            </span>
			            </td>
			            <td class="Forum_Row_AdminR" valign="middle" align="left" width="75%">
			                <asp:checkbox id="chkRatings" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
			            </td>
		            </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl1stRating" runat="server" ControlName="txt1stRating" Suffix=":" ></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt1stRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img1stRating" runat="server" EnableViewState="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl2ndRating" runat="server" ControlName="txt2ndRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt2ndRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img2ndRating" runat="server" EnableViewState="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl3rdRating" runat="server" ControlName="txt3rdRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt3rdRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img3rdRating" runat="server" EnableViewState="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl4thRating" runat="server" ControlName="txt4thRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt4thRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img4thRating" runat="server" EnableViewState="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl5thRating" runat="server" ControlName="txt5thRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt5thRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img5thRating" runat="server" EnableViewState="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl6thRating" runat="server" ControlName="txt6thRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt6thRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img6thRating" runat="server" EnableViewState="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl7thRating" runat="server" ControlName="txt7thRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                              <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt7thRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img7thRating" runat="server" EnableViewState="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl8thRating" runat="server" ControlName="txt8thRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt8thRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img8thRating" runat="server" EnableViewState="false" />
                                   </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl9thRating" runat="server" ControlName="txt9thRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt9thRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img9thRating" runat="server" EnableViewState="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="Forum_Row_AdminL" width="25%">
                            <span class="Forum_Row_AdminText">
                                <dnn:Label ID="pl10thRating" runat="server" ControlName="txt10thRating" Suffix=":"></dnn:Label>
                            </span>
                        </td>
                        <td align="left" class="Forum_Row_AdminR" valign="middle" width="75%">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txt10thRating" runat="server" CssClass="Forum_NormalTextBox" MaxLength="50" Width="180px" EnableViewState="false" />&nbsp;
                                    </td>
                                    <td>
                                        <asp:Image ID="img10thRating" runat="server" EnableViewState="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
	            </table>
	          <div class="Forum_Row_Admin_Foot" align="center">
	                <asp:linkbutton cssclass="CommandButton" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" EnableViewState="false" />
	            </div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
			</div>
		</td>
	</tr>
</table>