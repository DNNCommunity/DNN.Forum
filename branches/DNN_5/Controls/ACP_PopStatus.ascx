<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_PopStatus.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.PopStatus" %>
<%@ Register TagPrefix="wrapper" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<div class="ACP-PopStatus">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
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
								<dnn:Label ID="plPopularPostsView" runat="server" ControlName="txtPopularThreadView" Suffix=":" />
							</span>
						</td>
						<td align="left">
                            <wrapper:RadNumericTextBox ID="rntxtbxPopularThreadView" runat="server" MaxLength="4" NumberFormat-DecimalDigits="0" ShowSpinButtons="true" MinValue="1" MaxValue="1001"/>
							<asp:RequiredFieldValidator ID="valreqView" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="rntxtbxPopularThreadView" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:Label ID="plPopularPostsReply" runat="server" ControlName="txtPopularThreadReply" Suffix=":" />
							</span>
						</td>
						<td align="left">
                            <wrapper:RadNumericTextBox ID="rntxtbxPopularThreadReply" runat="server" MaxLength="3" NumberFormat-DecimalDigits="0" ShowSpinButtons="true" MinValue="1" MaxValue="101"/>
							<asp:RequiredFieldValidator ID="valreqReply" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="rntxtbxPopularThreadReply" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:Label ID="plDays" runat="server" ControlName="txtDays" Suffix=":" />
							</span>
						</td>
						<td align="left">
                            <wrapper:RadNumericTextBox ID="rntxtbxDays" runat="server" MaxLength="3" NumberFormat-DecimalDigits="0" ShowSpinButtons="true" MinValue="1" MaxValue="366"/>
							<asp:RequiredFieldValidator ID="valreqDays" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="rntxtbxDays" EnableViewState="false" />
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