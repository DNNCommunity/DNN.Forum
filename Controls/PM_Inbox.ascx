<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.PMInbox" Codebehind="PM_Inbox.ascx.vb" Language="vb" AutoEventWireup="false" Explicit="true" %>
<asp:Panel ID="pnlInbox" runat="server">
    <asp:datagrid ID="dgPMThreads" runat="server" DataKeyField="PMThreadID" Width="100%" AutoGenerateColumns="false" CssClass="Forum_Grid" CellPadding="0" CellSpacing="0" GridLines="None" >
	    <HeaderStyle CssClass="Forum_Grid_Header" HorizontalAlign="Center"/>
	    <ItemStyle CssClass="Forum_Grid_Row_Alt" />
	    <AlternatingItemStyle CssClass="Forum_Grid_Row" />
	    <Columns>
	        <asp:TemplateColumn ItemStyle-Width="15px" >
		       <ItemTemplate>
			      <asp:CheckBox ID="chkThread" runat="server" EnableViewState="false" />
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn>
		       <ItemTemplate>
			      <asp:HyperLink ID="hlStatus" runat="server" EnableViewState="false">
				    <asp:Image ID="imgStatus" runat="server" EnableViewState="false" />
			      </asp:HyperLink> 
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn ItemStyle-CssClass="Forum_Grid_Left" HeaderStyle-HorizontalAlign="Left" HeaderText="Inbox"  ItemStyle-Width="44%">
		       <ItemTemplate>
			      <asp:HyperLink ID="hlSubject" runat="server" CssClass="Forum_NormalBold" EnableViewState="false" /><br />
			      <asp:Label ID="lblStarter" runat="server" CssClass="Forum_Normal" EnableViewState="false" />
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:TemplateColumn HeaderText="Recipient" ItemStyle-CssClass="Forum_Grid_Middle" ItemStyle-Width="17%">
		       <ItemTemplate>
			      <asp:Label ID="lblPMRecipient" runat="server" EnableViewState="false" />
		       </ItemTemplate>
	        </asp:TemplateColumn>
	        <asp:BoundColumn DataField="Replies" HeaderText="Replies" ItemStyle-CssClass="Forum_Grid_Middle" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" />
	        <asp:TemplateColumn HeaderText="LastPost" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="23%" ItemStyle-CssClass="Forum_Grid_Right">
		       <ItemTemplate>
			      <asp:Label ID="lblLastPMInfo" runat="server" CssClass="Forum_LastPostText" EnableViewState="false" />
		       </ItemTemplate>
	        </asp:TemplateColumn>       
	    </Columns>
     </asp:datagrid>
     <dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/>
     <div align="center">
	    <asp:LinkButton ID="cmdDelete" CssClass="CommandButton" runat="server" resourcekey="cmdDelete" EnableViewState="false" />
     </div>
</asp:Panel>
<asp:Panel ID="pnlNoItems" runat="server">
    <table cellpadding="0" cellspacing ="0" width="100%">
        <tr>
		    <td class="Forum_UCP_Header">
			<asp:Label id="lblInbox" runat="server" resourcekey="Inbox.Header" EnableViewState="false" />
		</td>
         </tr>
         <tr>
		    <td class="Forum_UCP_HeaderInfo">
			<asp:Label id="lblNoPM" runat="server" CssClass="Forum_NormalBold" resourcekey="NoPM" EnableViewState="false" />
		</td>
		 </tr>
    </table>
</asp:Panel>



