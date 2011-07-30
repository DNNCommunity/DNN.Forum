<%@ Control language="vb" CodeBehind="Forum_ThreadMove.ascx.vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.Modules.Forum.ThreadMove" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
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
<div class="dnnForm forumThreadMove dnnClear">
	<h2 class="dnnFormSectionHead"><asp:label id="lblSubjectTitle" runat="server" resourcekey="lblSubjectTitle" /><asp:Label ID="lblSubject" runat="server" /></h2>
	<fieldset>
		<div class="dnnFormItem">
			<dnn:label id="plOldForum" runat="server" controlname="txtOldForum" Suffix=":" />
			<asp:textbox id="txtOldForum" runat="server" Enabled="false" />
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plNewForum" runat="server" controlname="rtvForums" Suffix=":" />
			<div class="dnnLeft">
				<dnnweb:DnnTreeView ID="rtvForums" runat="server" CheckBoxes="true" OnClientNodeChecked="clientNodeChecked" />
			</div>
		</div>
		<div class="dnnFormItem">
			<dnn:label id="plEmailUsers" runat="server" controlname="chkEmailUsers" Suffix=":" />
			<asp:checkbox id="chkEmailUsers" runat="server" />
		</div>
	</fieldset>
	<ul class="dnnActions dnnClear">
		<li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdMove" runat="server" resourcekey="cmdMove" /></li>
		<li><asp:linkbutton cssclass="dnnSecondaryAction" id="cmdCancel" runat="server" resourcekey="cmdCancel" CausesValidation="false" /></li>
	</ul>
</div>