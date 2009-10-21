<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Modules.Forum" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Control language="vb" Inherits="DotNetNuke.Modules.Forum.Container" CodeBehind="Forum_Container.ascx.vb" AutoEventWireup="false" %>
<asp:Literal ID="litCSSLoad" runat="server" />
<table id="tblMain" width="100%" cellpadding="0" cellspacing="0" border="0">		
	<tr>
		<td align="center">
			<dnn:DNNFORUM id="DNNForum" Runat="server"></dnn:DNNFORUM>
		</td>
	</tr>
</table>