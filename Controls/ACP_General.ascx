<%@ Control language="vb" CodeBehind="ACP_General.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.General" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="ACP-General">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">    
				<table id="tblGeneral" cellspacing="0" cellpadding="0" width="100%" runat="server">
					<tr id="rowPrimaryAlias" runat="server" visible="false">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plPrimaryAlias" runat="server" Suffix=":" controlname="ddlPrimaryAlias"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:dropdownlist id="ddlPrimaryAlias" runat="server" cssclass="Forum_NormalTextBox" Columns="26" width="250px" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plAggregatedForums" runat="server" Suffix=":" controlname="chkAggregatedForums"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:checkbox id="chkAggregatedForums" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnableThreadStatus" runat="server" Suffix=":" controlname="chkEnableThreadStatus"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:checkbox id="chkEnableThreadStatus" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plEnablePostAbuse" runat="server" Suffix=":" controlname="chkEnablePostAbuse"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:checkbox id="chkEnablePostAbuse" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:Label ID="plDisableHTMLPosting" runat="server" ControlName="chkDisableHTMLPosting" Suffix=":"></dnn:Label>
							</span>
						</td>
						<td align="left" >
							  <asp:CheckBox ID="chkDisableHTMLPosting" runat="server" CssClass="Forum_NormalTextBox" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plSearchIndexDate" runat="server" Suffix=":" controlname="chkShowNavigator"></dnn:label>
							</span>
						</td>
						<td align="left">
							<asp:Label id="lblDateIndexed" runat="server" CssClass="Forum_Normal" />&nbsp;
							<asp:linkbutton id="cmdResetDate" runat="server" CssClass="Forum_Profile" resourcekey="cmdResetDate" EnableViewState="false" />
						</td>
					</tr>
				</table>
				<div align="center">
					<asp:linkbutton cssclass="CommandButton primary-action" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" EnableViewState="false" />
				</div>
				<div align="center">
					<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
				</div>
			</td>
		</tr>
	</table>
</div>