<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Language="vb" AutoEventWireup="false" Codebehind="Forum_ModuleSettings.ascx.vb" Inherits="DotNetNuke.Modules.Forum.ModuleSettings" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<link href="<%= ForumConfig.Css() %>" type="text/css" rel="stylesheet" />
<script type="text/javascript" language="javascript">
var oldNode;
function clientNodeChecked(sender, eventArgs)
{
   var node = eventArgs.get_node();
   
   if(oldNode != null)
   {
       oldNode.set_checked(false);
   }
   
   node.set_checked(true);
   oldNode = node;
}

function pageLoad()
{
	var tree = $find("<%= rtvForums.ClientID  %>");
   var checkedNodes = tree.get_checkedNodes();
   if (checkedNodes)
   {
//this will ensure the correct behavior even if a node is checked server-side
       oldNode = checkedNodes[0];
   }
}
</script>
<table cellspacing="0" cellpadding="2" border="0">
	<tr>
		<td class="SubHead" width="178"><dnn:label id="plGroup" runat="server" controlname="cboGroup" suffix=":" /></td>
		<td valign="top">
			<asp:DropDownList ID="cboGroup" Runat="server" CssClass="NormalTextBox" DataTextField="Name" DataValueField="GroupID" AutoPostBack="True" />
		</td>
	</tr>
    <tr id="rowDefaultForum" runat="server">
        <td class="SubHead" width="178" valign="top">
            <dnn:label ID="plDefaultForum" runat="server" ControlName="DefaultForumTree" Suffix=":" />
        </td>
        <td valign="top">
		<telerik:RadTreeView ID="rtvForums" runat="server" Skin="Web20" CheckBoxes="true" OnClientNodeChecked="clientNodeChecked" />
        </td>
    </tr>
</table>