<%@ Control language="vb" CodeBehind="ACP_Attachment.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Attachment" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="ACP-Attachment">
	<table cellpadding="0" cellspacing="0" width="100%" border="0">
		<tr>
			<td class="Forum_UCP_Header">
				<asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" />
			</td>
		</tr>
		<tr>
			<td class="Forum_UCP_HeaderInfo">    
				<table id="tblAttachments" cellspacing="0" cellpadding="0" width="100%" runat="server">
						  <tr id="rowAttachment" runat="server">
							  <td width="35%">
								 <span class="Forum_Row_AdminText">
									<dnn:label id="plAttachment" runat="server" Suffix=":" controlname="chkAttachment"></dnn:label>
								  </span>
							   </td>
							  <td valign="middle" align="left">
								 <asp:checkbox id="chkAttachment" runat="server" CssClass="Forum_NormalTextBox" AutoPostBack="True" />
							  </td>
						  </tr>
						  <tr id="rowAnonDownloads" runat="server">
							  <td width="35%">
								 <span class="Forum_Row_AdminText">
									  <dnn:label id="plAnonDownloads" runat="server" Suffix=":" controlname="chkAnonDownloads"></dnn:label>
								  </span>
							   </td>
							  <td valign="middle" align="left">
								  <asp:checkbox id="chkAnonDownloads" runat="server" CssClass="Forum_NormalTextBox" />
							   </td>
						  </tr>
						  <tr id="rowAttachmentPath" runat="server">
							  <td width="35%">
								 <span class="Forum_Row_AdminText">
									  <dnn:label id="plAttachmentPath" runat="server" Suffix=":" controlname="txtAttachmentPath"></dnn:label>
								  </span>
							   </td>
							  <td valign="middle" align="left">
								  <asp:TextBox id="txtAttachmentPath" runat="server" CssClass="Forum_NormalTextBox" Width="250px" />
								  <asp:RequiredFieldValidator ID="valAttachmentPath" runat="server" ErrorMessage="*" ControlToValidate="txtAttachmentPath" CssClass="NormalRed" Display="Dynamic"></asp:RequiredFieldValidator>
							   </td>
						  </tr>
						  <tr id="rowMaxAttachmentSize" runat="server">
							  <td width="35%">
								 <span class="Forum_Row_AdminText">
									  <dnn:label id="plMaxAttachmentSize" runat="server" Suffix=":" controlname="txtMaxAttachmentSize"></dnn:label>
								  </span>
							   </td>
							  <td valign="middle" align="left">
								  <asp:TextBox id="txtMaxAttachmentSize" runat="server" CssClass="Forum_NormalTextBox" Width="50px" MaxLength="5" />
								  <asp:RequiredFieldValidator ID="valMaxAttachSize" runat="server" ErrorMessage="*" ControlToValidate="txtMaxAttachmentSize" CssClass="NormalRed" Display="Dynamic"></asp:RequiredFieldValidator>
							   </td>
						  </tr>
					  </table>
				<div align="center">
					<asp:linkbutton cssclass="CommandButton primary-action" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" />
				</div>
				<div align="center">
					<asp:Label ID="lblUpdateDone" runat="server" CssClass="NormalRed" Visible="false" resourcekey="lblUpdateDone" />
				</div>
			</td>
		</tr>
	</table>
</div>