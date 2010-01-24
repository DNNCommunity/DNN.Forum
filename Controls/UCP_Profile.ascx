<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Profile.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Profile" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<table cellpadding="0" cellspacing="0" border="0" width="100%">
	<tr>
		<td class="Forum_UCP_Header">
			<asp:Label id="lblTitle" runat="server" resourcekey="Title" EnableViewState="false" />
		</td>
     </tr>
     <tr>
		<td>
			<table cellspacing="0" cellpadding="2" width="100%" runat="server">
				<tr>
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plUserID" runat="server" suffix=":" controlname="txtUserID"></dnn:label>
						</span>
					</td>
					<td colspan="2" class="Forum_Row_AdminR" align="left">
						<asp:TextBox ID="txtUserID" runat="server" CssClass="Forum_NormalTextBox" Width="50px" ReadOnly="true" />
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plUserName" runat="server" suffix=":" controlname="lblUserName"></dnn:label>
						</span>
					</td>
					<td colspan="2" class="Forum_Row_AdminR" align="left">
						<asp:Label ID="lblUserName" runat="server" CssClass="Forum_Normal" Width="250px" />
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plDisplayName" runat="server" suffix=":" controlname="lblDisplayName"></dnn:label>
						</span>
					</td>
					<td colspan="2" class="Forum_Row_AdminR" align="left">
						<asp:Label ID="lblDisplayName" runat="server" CssClass="Forum_Normal" Width="250px" />
					</td>
				</tr>
				<tr id="rowEmail" runat="server">
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEmail" runat="server" suffix=":" controlname="chkDisplayEmail"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminM" align="left">
						<asp:HyperLink ID="hlEmail" runat="server" CssClass="Forum_Profile" />        
					</td>
					<td class="Forum_Row_AdminR" align="left">
						<asp:Label ID="visEmail" runat="server" CssClass="Forum_Normal" />
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEnableProfileWeb" runat="server" suffix=":" controlname="chkEnableProfileWeb"></dnn:label>
						</span>    
					</td>
					<td align="left" class="Forum_Row_AdminM">
						<asp:CheckBox ID="chkEnableProfileWeb" runat="server" CssClass="Forum_NormalTextBox" />
						<asp:Label ID="lblWebsite" runat="server" CssClass="Forum_Normal" />
					</td>
					<td class="Forum_Row_AdminR" align="left">
						<asp:Label ID="visWebsite" runat="server" CssClass="Forum_Normal" />
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEnableProfileRegion" runat="server" suffix=":" controlname="chkEnableProfileRegion"></dnn:label>
						</span>    
					</td>
					<td align="left" class="Forum_Row_AdminM">
						<asp:CheckBox ID="chkEnableProfileRegion" runat="server" CssClass="Forum_NormalTextBox" />
						<asp:Label ID="lblRegion" runat="server" CssClass="Forum_Normal" />
					</td>
					<td class="Forum_Row_AdminR" align="left">
						<asp:Label ID="visRegion" runat="server" CssClass="Forum_Normal" />
					</td>
				</tr>
				<tr id="rowBio" runat="server" visible="false">
					<td class="Forum_Row_AdminL" width="175" valign="top">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plBiography" runat="server" suffix=":" controlname="txtBiography"></dnn:label>
						</span>    
					</td>
					<td class="Forum_Row_AdminR" align="left" colspan="2">
						<asp:TextBox ID="txtBiography" runat="server" CssClass="Forum_NormalTextBox" Height="100px" Width="250px" MaxLength="1024" />
					</td>
				</tr>
			</table>
			<table id="tblAdmin" cellspacing="0" cellpadding="2" width="100%" runat="server">
				<tr>
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plIsTrusted" runat="server" suffix=":" controlname="chkIsTrusted"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" align="left">
						<asp:CheckBox ID="chkIsTrusted" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr>
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plLockTrust" runat="server" suffix=":" controlname="chkLockTrust"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" align="left">
						<asp:CheckBox ID="chkLockTrust" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowUserBanning" runat="server">
					<td class="Forum_Row_AdminL" width="175">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plIsBanned" runat="server" suffix=":" controlname="chkIsBanned"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" align="left">
						<asp:CheckBox ID="chkIsBanned" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
					</td>
				</tr>
				<tr id="rowLiftBanDate" runat="server">
					<td class="Forum_Row_AdminL" width="175" valign="top">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plLiftBanDate" runat="server" suffix=":" controlname="txtLiftBanDate"></dnn:label>
						</span>
					</td>
					<td class="Forum_Row_AdminR" align="left" valign="top">
						<table collspacing="0" cellpadding="0" border="0">
							<tr>
								<td>
									<asp:TextBox ID="txtLiftBanDate" runat="server" CssClass="Forum_NormalTextBox"  Width="100px" />
								</td>
								<td>
									<asp:hyperlink id="cmdCalBanDate" resourcekey="cmdCalBanDate" runat="server" />
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<div class="Forum_Row_Admin_Foot" align="center">
				<asp:LinkButton class="CommandButton" ID="cmdUpdate" runat="server" resourcekey="cmdUpdate" EnableViewState="false" />
			</div>
			<div align="center">
			    <asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" EnableViewState="false" />
			</div>
		</td>
	</tr>
</table>       