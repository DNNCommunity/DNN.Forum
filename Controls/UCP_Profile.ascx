<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Profile.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Profile" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="Title" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td class="Forum_UCP_HeaderInfo">
			<table border="0" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plUserID" runat="server" suffix=":" controlname="txtUserID"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:TextBox ID="txtUserID" runat="server" CssClass="Forum_NormalTextBox" Width="50px" ReadOnly="true" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plUserName" runat="server" suffix=":" controlname="lblUserName"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:Label ID="lblUserName" runat="server" CssClass="Forum_Normal" Width="250px" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plDisplayName" runat="server" suffix=":" controlname="lblDisplayName"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:Label ID="lblDisplayName" runat="server" CssClass="Forum_Normal" Width="250px" />
					</td>
				</tr>
				<tr id="rowEmail" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEmail" runat="server" suffix=":" controlname="chkDisplayEmail"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:HyperLink ID="hlEmail" runat="server" CssClass="Forum_Profile" />        
					</td>
				</tr>
				<tr id="rowTrust" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plIsTrusted" runat="server" suffix=":" controlname="chkIsTrusted"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:CheckBox ID="chkIsTrusted" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowLockTrust" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plLockTrust" runat="server" suffix=":" controlname="chkLockTrust"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:CheckBox ID="chkLockTrust" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowUserBanning" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plIsBanned" runat="server" suffix=":" controlname="chkIsBanned"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:CheckBox ID="chkIsBanned" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
					</td>
				</tr>
				<tr id="rowLiftBanDate" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plLiftBanDate" runat="server" suffix=":" controlname="txtLiftBanDate"></dnn:label>
						</span>
					</td>
					<td align="left">
						<telerik:RadDatePicker id="rdpLiftBan" runat="server" />
					</td>
				</tr>
			</table>   
			<div align="center">
				<asp:LinkButton class="CommandButton" ID="cmdUpdate" runat="server" resourcekey="cmdUpdate" EnableViewState="false" />
			</div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
			</div>
		</td>
	</tr>
</table>