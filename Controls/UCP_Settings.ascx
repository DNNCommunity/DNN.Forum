<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Settings.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Settings" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td class="Forum_UCP_Header" colspan="2">
			<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" />
		</td>
	</tr>
	<tr>
	 <td class="Forum_Row_AdminL" valign="top" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plEmailFormat" runat="server" controlname="ddlEmailFormat" suffix=":"></dnn:label>
		</span>
	 </td>
	 <td class="Forum_Row_AdminR" align="left">
		<asp:DropDownList ID="ddlEmailFormat" runat="server" Width="250px" CssClass="Forum_NormalTextBox" />
	 </td>
	</tr>
	<tr>
		<td class="Forum_Row_AdminL" width="200">
			<span class="Forum_Row_AdminText">
				<dnn:label id="plThreadsPerPage" runat="server" suffix=":" controlname="txtThreadsPerPage"></dnn:label>
			</span>
		</td>
		<td class="Forum_Row_AdminR" align="left">
			<asp:TextBox ID="txtThreadsPerPage" runat="server" CssClass="Forum_NormalTextBox" Width="100px" EnableViewState="false" />&nbsp;
			<asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" resourcekey="NumericValidation.ErrorMessage" CssClass="NormalRed" ValidationExpression="[0-9]{1,}" ErrorMessage="Needs to be numeric!" ControlToValidate="txtThreadsPerPage" Display="Dynamic" EnableViewState="false" />
		</td>
	</tr>
	<tr>
		<td class="Forum_Row_AdminL" width="200">
			<span class="Forum_Row_AdminText">
			    <dnn:label id="plPostsPerPage" runat="server" suffix=":" controlname="txtPostsPerPage"></dnn:label>
			</span>
		</td>
		 <td class="Forum_Row_AdminR" align="left">
			<asp:TextBox ID="txtPostsPerPage" runat="server" CssClass="Forum_NormalTextBox" Width="100px" EnableViewState="false" />&nbsp;
			<asp:RegularExpressionValidator ID="valPostsPerPage" runat="server" resourcekey="NumericValidation.ErrorMessage" CssClass="NormalRed" ValidationExpression="[0-9]{1,}" ControlToValidate="txtPostsPerPage" Display="Dynamic" EnableViewState="false" />
		 </td>
	</tr>
	<tr id="rowOnlineStatus" runat="server">
	 <td class="Forum_Row_AdminL" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plOnlineStatus" runat="server" suffix=":" controlname="chkOnlineStatus"></dnn:label>
		</span>
	 </td>
	 <td class="Forum_Row_AdminR" align="left">
		<asp:CheckBox ID="chkOnlineStatus" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
	 </td>
	</tr>
	<tr id="rowMemberList" runat="server">
	 <td class="Forum_Row_AdminL" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plEnableMemberList" runat="server" ControlName="chkEnableMemberList" Suffix=":" />
		</span>
	 </td>
	 <td align="left" class="Forum_Row_AdminR">
		<asp:CheckBox ID="chkEnableMemberList" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
	 </td>
	</tr>
	<tr id="rowEnablePM" runat="server">
	 <td class="Forum_Row_AdminL" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plEnablePM" runat="server" suffix=":" controlname="chkEnablePM"></dnn:label>
		</span>
	 </td>
	 <td class="Forum_Row_AdminR" align="left">
		<asp:CheckBox ID="chkEnablePM" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="True" EnableViewState="false" />
	 </td>
	</tr>
	<tr runat="server" id="rowEnableDefaultPostNotify">
	 <td class="Forum_Row_AdminL" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plEnableDefaultPostNotify" runat="server" suffix=":" controlname="chkEnableDefaultPostNotify" EnableViewState="false"></dnn:label>
		</span>
	 </td>
	 <td align="left" class="Forum_Row_AdminR">
		<asp:CheckBox ID="chkEnableDefaultPostNotify" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
	 </td>
	</tr>
	<tr id="rowEnableSelfNotifications" runat="server">
	 <td class="Forum_Row_AdminL" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plEnableSelfNotifications" runat="server" suffix=":" controlname="chkEnableSelfNotifications"></dnn:label>
		</span>
	 </td>
	 <td align="left" class="Forum_Row_AdminR">
		<asp:CheckBox ID="chkEnableSelfNotifications" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
	 </td>
	</tr>
	<tr id="rowPMNotification" runat="server">
	 <td class="Forum_Row_AdminL" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plEnablePMNotifications" runat="server" suffix=":" controlname="chkEnablePMNotifications"></dnn:label>
		</span>
	 </td>
	 <td class="Forum_Row_AdminR" align="left">
		<asp:CheckBox ID="chkEnablePMNotifications" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
	 </td>
	</tr>
	<tr id="rowForumModNotify" runat="server">
	 <td class="Forum_Row_AdminL" valign="middle" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plEnableForumModNotify" runat="server" suffix=":" controlname="chkEnableForumModNotify"></dnn:label>
		</span>
	 </td>
	 <td class="Forum_Row_AdminR" align="left">
		<asp:CheckBox ID="chkEnableForumModNotify" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
	 </td>
	</tr>
	<tr>
	 <td class="Forum_Row_AdminL" width="200">
		<span class="Forum_Row_AdminText">
		    <dnn:label id="plClearReads" runat="server" controlname="cmdClearReads" suffix=":"></dnn:label>
		</span>
	 </td>
	 <td class="Forum_Row_AdminR" align="left">
		<asp:LinkButton class="Forum_Profile" ID="cmdClearReads" runat="server" resourcekey="cmdClearReads" EnableViewState="false" />
	 </td>
	</tr>
</table>   
<div class="Forum_Row_Admin_Foot" align="center">
	<asp:LinkButton class="CommandButton" ID="cmdUpdate" runat="server" resourcekey="cmdUpdate" EnableViewState="false" />
</div>
<div align="center">
	<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
</div>         