<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_PopStatus.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.PopStatus" %>
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
							<asp:TextBox ID="txtPopularThreadView" runat="server" CssClass="Forum_NormalTextBox" Width="180px" MaxLength="10" EnableViewState="false" />
							<asp:RequiredFieldValidator ID="valreqView" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtPopularThreadView" EnableViewState="false" />
							<asp:RegularExpressionValidator ID="valView" runat="server" ControlToValidate="txtPopularThreadView" CssClass="NormalRed" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />
						</td>
					</tr>
					<tr>
						<td width="35%">
							<span class="Forum_Row_AdminText">
								<dnn:Label ID="plPopularPostsReply" runat="server" ControlName="txtPopularThreadReply" Suffix=":" />
							</span>
						</td>
						<td align="left">
							<asp:TextBox ID="txtPopularThreadReply" runat="server" CssClass="Forum_NormalTextBox" Width="180px" MaxLength="10" EnableViewState="false" />
							<asp:RequiredFieldValidator ID="valreqReply" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtPopularThreadReply" EnableViewState="false" />
							<asp:RegularExpressionValidator ID="valReply" runat="server" ControlToValidate="txtPopularThreadReply" CssClass="NormalRed" resourcekey="NumericValidation.ErrorMessage" ValidationExpression="[0-9]{1,}" Display="Dynamic" EnableViewState="false" />
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