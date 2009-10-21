<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.ACP.Filter" CodeBehind="ACP_Filter.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<table cellpadding="0" cellspacing="0" width="100%" border="0">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false"/>
		</td>
     </tr>
     <tr>
		<td>
			<table class="" id="tblGeneral" runat="server" cellspacing="0" cellpadding="0" width="100%">
				<tr>
					<td class="Forum_Row_AdminL" width="30%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plBadWord" runat="server" Suffix=":" controlname="chkBadWord"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" valign="middle" align="left" width="70%">
						<asp:checkbox id="chkBadWord" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="True" EnableViewState="false" />
					</td>
				</tr>
				<tr id="rowSubjectFilter" runat="server">
					<td class="Forum_Row_AdminL" width="30%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plFilterSubject" runat="server" Suffix=":" controlname="chkFilterSubject"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" valign="middle" align="left" width="70%">
						<asp:checkbox id="chkFilterSubject" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
					</td>
				</tr>
			</table>
			<div class="Forum_Row_Admin_Foot" align="center">
				<asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" borderstyle="none" causesvalidation="False" cssclass="CommandButton" EnableViewState="false" />
			</div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
			</div>
		</td>
     </tr>
</table>