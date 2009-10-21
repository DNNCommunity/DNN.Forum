<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.PMOutbox" Codebehind="PM_Outbox.ascx.vb" Language="vb" AutoEventWireup="false" Explicit="true" %>
<asp:Panel ID="pnlOutbox" runat="server">
    <asp:datagrid ID="dgPMThreads" runat="server" DataKeyField="PMID" Width="100%" AutoGenerateColumns="false" CssClass="Forum_Grid" CellPadding="0" CellSpacing="0" GridLines="None" EnableViewState="false" >
	    <HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
	    <ItemStyle CssClass="Forum_Grid_Row_Alt" />
	    <AlternatingItemStyle CssClass="Forum_Grid_Row" />
	    <Columns>
	        <asp:TemplateColumn ItemStyle-Width="15px" Visible="false" >
		       <ItemTemplate>
			      <asp:CheckBox ID="chkThread" runat="server" EnableViewState="false" />
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn ItemStyle-Width="15px" >
		       <ItemTemplate>
			      <asp:HyperLink ID="hlStatus" runat="server" EnableViewState="false">
				    <asp:Image ID="imgStatus" runat="server" EnableViewState="false" />
			      </asp:HyperLink> 
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn HeaderText="Outbox" HeaderStyle-HorizontalAlign="Left" ItemStyle-CssClass="Forum_Grid_Left" ItemStyle-Width="55%">
		       <ItemTemplate>
			      <asp:HyperLink ID="hlSubject" runat="server" CssClass="Forum_NormalBold" EnableViewState="false" /><br />
			      <asp:Label ID="lblStarter" runat="server" EnableViewState="false"/>
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn HeaderText="Recipient" ItemStyle-CssClass="Forum_Grid_Middle" ItemStyle-Width="17%">
		       <ItemTemplate>
			      <asp:Label ID="lblPMRecipient" runat="server" EnableViewState="false" />
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn HeaderText="SentDate" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="23%" ItemStyle-CssClass="Forum_Grid_Right" >
		       <ItemTemplate>
			      <asp:Label ID="lblPMCreatedDate" runat="server" CssClass="Forum_LastPostText" EnableViewState="false" />
		       </ItemTemplate>
	        </asp:TemplateColumn>       
	    </Columns>
    </asp:datagrid>
    <dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
    <div align="center">
	    <asp:LinkButton ID="cmdDelete" CssClass="CommandButton" runat="server" resourcekey="cmdDelete" EnableViewState="false" Visible="false" />
    </div>
</asp:Panel>
<asp:Panel ID="pnlNoItems" runat="server" EnableViewState="false">
    <table cellpadding="0" cellspacing ="0" width="100%">
        <tr>
		    <td class="Forum_UCP_Header">
			<asp:Label id="lblOutbox" runat="server" resourcekey="Outbox.Header" EnableViewState="false" />
		    </td>
         </tr>
         <tr>
		    <td class="Forum_UCP_HeaderInfo">
			<asp:Label id="lblNoPM" runat="server" CssClass="Forum_NormalBold" resourcekey="NoPM" EnableViewState="false" />
		    </td>
		 </tr>
    </table>
</asp:Panel>