<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control language="vb" CodeBehind="ACP_Email.ascx.vb" AutoEventWireup="false" Explicit="true" Inherits="DotNetNuke.Modules.Forum.ACP.Email" %>
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
							<dnn:label id="plAutomatedAddress" runat="server" controlname="txtAutomatedAddress" suffix=":"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:textbox id="txtAutomatedAddress" runat="server" cssclass="Forum_NormalTextBox" width="250px" MaxLength="255" />
						<asp:RequiredFieldValidator ID="valAddy" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtAutomatedAddress" />
						<asp:RegularExpressionValidator ID="valEmailAddy" runat="server" ControlToValidate="txtAutomatedAddress" CssClass="NormalRed" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" resourcekey="valEmailAddy.ErrorMessage" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plEmailAddressDisplayName" runat="server" ControlName="txtEmailAddressDisplayName" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:TextBox ID="txtEmailAddressDisplayName" runat="server" CssClass="Forum_NormalTextBox" Width="250px" MaxLength="132" />
						<asp:RequiredFieldValidator ID="valDisplay" runat="server" ErrorMessage="*" CssClass="NormalRed" Display="Dynamic" ControlToValidate="txtEmailAddressDisplayName" />
					</td>
				</tr>
				<tr>
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plNofify" runat="server" controlname="chkNotify" suffix=":"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:checkbox id="chkNotify" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="true" />
					</td>
				</tr>
				<tr id="rowEnablePerForumFrom" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plEnablePerForumFrom" runat="server" ControlName="chkEnablePerForumFrom" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:CheckBox ID="chkEnablePerForumFrom" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowEnableEditEmails" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plEnableEditEmails" runat="server" ControlName="chkEnableEditEmails" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:CheckBox ID="chkEnableEditEmails" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowEmailQueueTask" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:label id="plEmailQueueTask" runat="server" Suffix=":" controlname="chkEmailQueueTask"></dnn:label>
						</span>
					</td>
					<td align="left">
						<asp:checkbox id="chkEmailQueueTask" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowEnableListServer" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plEnableListServer" runat="server" ControlName="chkEnableListServer" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:CheckBox ID="chkEnableListServer" runat="server" CssClass="Forum_NormalTextBox" />
					</td>
				</tr>
				<tr id="rowListServerFolder" runat="server">
					<td width="35%">
						<span class="Forum_Row_AdminText">
							<dnn:Label ID="plListServerFolder" runat="server" ControlName="txtListServerFolder" Suffix=":"></dnn:Label>
						</span>
					</td>
					<td align="left">
						<asp:TextBox ID="txtListServerFolder" runat="server" CssClass="Forum_NormalTextBox" MaxLength="150" Width="250px" />
					</td>
				</tr>
			</table>
			<div align="center">
				<asp:linkbutton class="CommandButton" id="cmdUpdate" runat="server" resourcekey="cmdUpdate" />
			</div>
			<div align="center">
				<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone"  />
			</div>
		</td>
	</tr>
</table>