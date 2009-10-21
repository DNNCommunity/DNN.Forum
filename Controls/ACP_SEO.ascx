<%@ Control language="vb" CodeBehind="ACP_SEO.ascx.vb" Explicit="true" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.SEO" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td>    
               <table id="tblSpider" cellspacing="0" cellpadding="0" width="100%" runat="server">
		                <tr>
			                <td class="Forum_Row_AdminL" width="35%">
			                    <span class="Forum_Row_AdminText">
			                        <dnn:label id="plNoFollowWeb" runat="server" Suffix=":" controlname="chkNoFollowWeb"></dnn:label>
				                </span>
				            </td>
			                <td class="Forum_Row_AdminR" valign="middle" align="left">
			                    <asp:checkbox id="chkNoFollowWeb" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
			                </td>
		                </tr>
                        <tr>
                            <td class="Forum_Row_AdminL" width="35%">
                                <dnn:label id="plOverrideTitle" runat="server" Suffix=":" controlname="chkOverrideTitle"></dnn:label>
                            </td>
                            <td align="left" class="Forum_Row_AdminR" valign="middle">
                                <asp:checkbox id="chkOverrideTitle" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
                            </td>
                        </tr>
                         <tr>
                            <td class="Forum_Row_AdminL" width="35%">
                                <dnn:label id="plNoFollowLatestThreads" runat="server" Suffix=":" controlname="chkNoFollowLatestThreads"></dnn:label>
                            </td>
                            <td align="left" class="Forum_Row_AdminR" valign="middle">
                                <asp:checkbox id="chkNoFollowLatestThreads" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
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