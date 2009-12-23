<%@ Register TagPrefix="DNN" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke.WebControls" %>
<%@ Control language="vb" CodeBehind="Forum_ThreadMove.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.ThreadMove" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<asp:Literal ID="litCSSLoad" runat="server" />
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
<asp:Panel ID="pnlContainer" runat="server">
	<table class="" id="tblMain" cellspacing="0" cellpadding="0" width="100%" align="center">
		<tr>
			<td class="Forum_Row_AdminL">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plSubject" runat="server" controlname="txttxtSubject" Suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR" align="left">
				<asp:textbox id="txtSubject" runat="server" ReadOnly="True" width="250" cssclass="Forum_NormalTextBox"></asp:textbox>
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_AdminL">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plOldForum" runat="server" controlname="txtOldForum" Suffix=":"></dnn:label>
				</span></td>
			<td class="Forum_Row_AdminR" align="left">
				<asp:textbox id="txtOldForum" runat="server" ReadOnly="True" width="250" cssclass="Forum_NormalTextBox"></asp:textbox>
			</td>
		</tr>
		<tr id="rowForum" runat="server">
			<td class="Forum_Row_AdminL" valign="top">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plNewForum" runat="server" controlname="txtctlForumLookup" Suffix=":"></dnn:label>
				</span>
			</td>
			<td class="Forum_Row_AdminR">
				<telerik:RadTreeView ID="rtvForums" runat="server" Skin="Web20" CheckBoxes="true" OnClientNodeChecked="clientNodeChecked" />
				<br/>
				<asp:label id="lblErrorMsg" Runat="server" CssClass="NormalRed"></asp:label></td>
		</tr>
		<tr>
			<td class="Forum_Row_AdminL">
				<span class="Forum_Row_AdminText">
					<dnn:label id="plEmailUsers" runat="server" controlname="chkEmailUsers" Suffix=":"></dnn:label>
				</span></td>
			<td class="Forum_Row_AdminR">
				<asp:checkbox id="chkEmailUsers" runat="server" CssClass="Forum_NormalTextBox"></asp:checkbox>
			</td>
		</tr>
		<tr>
			<td class="Forum_Row_Admin_Foot" align="center" colspan="2">
				<asp:linkbutton cssclass="CommandButton" id="cmdCancel" runat="server" resourcekey="cmdCancel"></asp:linkbutton>&nbsp;
				<asp:linkbutton cssclass="CommandButton" id="cmdMove" runat="server" resourcekey="cmdMove"></asp:linkbutton>
			</td>
		</tr>
	</table>
</asp:Panel>