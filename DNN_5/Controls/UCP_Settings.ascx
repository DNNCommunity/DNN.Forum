<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Settings.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Settings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="UCP-Settings">
	<table cellpadding="0" cellspacing="0" border="0" width="100%">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
							    <dnn:label id="plEmailFormat" runat="server" controlname="ddlEmailFormat" suffix=":"></dnn:label>
							</span>
						 </td>
						 <td align="left">
                            <dnnweb:DnnComboBox id="rcbEmailFormat" runat="server" Width="198px" />
						 </td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plThreadsPerPage" runat="server" suffix=":" controlname="txtThreadsPerPage"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:TextBox ID="txtThreadsPerPage" runat="server" CssClass="Forum_NormalTextBox" Width="100px" EnableViewState="false" />&nbsp;
							<asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" resourcekey="NumericValidation.ErrorMessage" CssClass="NormalRed" ValidationExpression="[0-9]{1,}" ErrorMessage="Needs to be numeric!" ControlToValidate="txtThreadsPerPage" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plPostsPerPage" runat="server" suffix=":" controlname="txtPostsPerPage"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:TextBox ID="txtPostsPerPage" runat="server" CssClass="Forum_NormalTextBox" Width="100px" EnableViewState="false" />&nbsp;
							<asp:RegularExpressionValidator ID="valPostsPerPage" runat="server" resourcekey="NumericValidation.ErrorMessage" CssClass="NormalRed" ValidationExpression="[0-9]{1,}" ControlToValidate="txtPostsPerPage" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
					<tr id="rowOnlineStatus" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plOnlineStatus" runat="server" suffix=":" controlname="chkOnlineStatus"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:CheckBox ID="chkOnlineStatus" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
						</td>
					</tr>
					<tr runat="server" id="rowEnableDefaultPostNotify">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableDefaultPostNotify" runat="server" suffix=":" controlname="chkEnableDefaultPostNotify" EnableViewState="false"></dnn:label>
							</span>
						</td>
						<td align="left" >
							<asp:CheckBox ID="chkEnableDefaultPostNotify" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
						</td>
					</tr>
					<tr id="rowEnableSelfNotifications" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableSelfNotifications" runat="server" suffix=":" controlname="chkEnableSelfNotifications"></dnn:label>
							</span>
						</td>
						<td align="left" >
							<asp:CheckBox ID="chkEnableSelfNotifications" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
						</td>
					</tr>
					<tr id="rowForumModNotify" runat="server">
						<td valign="middle" width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableForumModNotify" runat="server" suffix=":" controlname="chkEnableForumModNotify"></dnn:label>
							</span>
						</td>
						<td align="left">
						<asp:CheckBox ID="chkEnableForumModNotify" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
						</td>
					</tr>
					<tr id="rowClearReads" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plClearReads" runat="server" controlname="cmdClearReads" suffix=":"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:LinkButton class="Forum_Profile" ID="cmdClearReads" runat="server" resourcekey="cmdClearReads" EnableViewState="false" />
						</td>
					</tr>
				</table>   
				<div align="center">
					<asp:LinkButton class="CommandButton primary-action" ID="cmdUpdate" runat="server" resourcekey="cmdUpdate" EnableViewState="false" />
				</div>
				<div align="center">
					<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
				</div>
			</td>
		</tr>
	</table>
</div>