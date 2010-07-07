<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="UCP_Avatar.ascx.vb" Inherits="DotNetNuke.Modules.Forum.UCP.Avatar" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Src="~/DesktopModules/Forum/controls/AvatarControl.ascx" TagName="AvatarControl" TagPrefix="forum" %>
<div class="UCP-Avatar">
	<table cellpadding="0" cellspacing="0" border="0" width="100%">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="Title" EnableViewState="false" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">
				<table border="0" cellpadding="0" cellspacing="0" width="100%">
					<tr id="rowUserAvatar" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plAvatar" runat="server" suffix=":" controlname="ctlUserAvatar"></dnn:label>
							</span>
						</td>
						<td align="left">
							<forum:avatarcontrol id="ctlUserAvatar" runat="server"></forum:avatarcontrol>
						</td>
					</tr>
					<tr id="rowSystemAvatar" runat="server">
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:label id="plSystemAvatarsLookup" runat="server" controlname="ctlSystemAvatar" suffix=":"></dnn:label>
							</span>
						</td>
						<td align="left">
							<forum:avatarcontrol id="ctlSystemAvatar" runat="server"></forum:avatarcontrol>
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