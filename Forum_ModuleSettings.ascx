<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="DNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Forum_ModuleSettings.ascx.vb" Inherits="DotNetNuke.Modules.Forum.ModuleSettings" %>
<link href="<%= ForumConfig.Css() %>" type="text/css" rel="stylesheet" />
<table cellspacing="0" cellpadding="2" border="0">
	<tr>
		<td class="SubHead" width="178"><dnn:label id="plGroup" runat="server" controlname="cboGroup" suffix=":"></dnn:label></td>
		<td valign="top">
			<asp:DropDownList ID="cboGroup" Runat="server" CssClass="NormalTextBox" DataTextField="Name" DataValueField="GroupID" AutoPostBack="True"></asp:DropDownList>
		</td>
	</tr>
    <tr id="rowDefaultForum" runat="server">
        <td class="SubHead" width="178" valign="top">
            <dnn:label ID="plDefaultForum" runat="server" ControlName="DefaultForumTree" Suffix=":" />
        </td>
        <td valign="top"><DNN:DNNTREE id="DefaultForumTree" runat="server"></DNN:DNNTREE>
        </td>
    </tr>
</table>
