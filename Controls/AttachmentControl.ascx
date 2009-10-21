<%@ Control AutoEventWireup="false" Codebehind="AttachmentControl.ascx.vb" Inherits="DotNetNuke.Modules.Forum.WebControls.AttachmentControl" Language="vb" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<asp:Panel ID="pnlContainer" runat="server">
	<table id="tblAttachment" runat="server" cellpadding="0" cellspacing="0">
	    <tr id="rowInlineHelp" runat="server">
		   <td colspan="2"><dnn:label id="plInline" runat="server" CssClass="Forum_Row_AdminText" Suffix=":" controlname="ddlInline"></dnn:label></td>
	    </tr>
	    <tr id="rowInline" runat="server">
		   <td><asp:DropDownList ID="ddlInline" runat="server" CssClass="Forum_NormalTextBox" Width="300px" /></td>
		   <td>&nbsp;<asp:HyperLink ID="hypInline" runat="server" CssClass="CommandButton" /><asp:Literal ID="litScript" runat="server" /></td>
	    </tr>
	    <tr>
		   <td colspan="2"><dnn:label id="plAttachments" runat="server" CssClass="Forum_Row_AdminText" Suffix=":" controlname="lstAttachments"></dnn:label></td>
	    </tr>
	    <tr valign="top">
		   <td><asp:ListBox ID="lstAttachments" runat="server" CssClass="Forum_NormalTextBox" Width="300px" /></td>
		   <td>&nbsp;<asp:ImageButton ID="cmdDelete" runat="server" ImageUrl="~/images/delete.gif" CausesValidation="false" /></td>
	    </tr>
	    <tr>
		   <td colspan="2"><dnn:label id="plUpload" runat="server" CssClass="Forum_Row_AdminText" Suffix=":" controlname="fuFile"></dnn:label></td>
	    </tr>
	    <tr>
		   <td><asp:FileUpload ID="fuFile" runat="server" width="300px" CssClass="Forum_NormalTextBox" /></td>
		   <td>&nbsp;<asp:LinkButton ID="cmdUpload" runat="server" CssClass="CommandButton" CausesValidation="false" /></td>
	    </tr>
	</table>
	<asp:Label id="lblMessage" runat="server" CssClass="NormalRed" />
</asp:Panel>