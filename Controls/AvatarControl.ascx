<%@ Control AutoEventWireup="false" Codebehind="AvatarControl.ascx.vb" Inherits="DotNetNuke.Modules.Forum.AvatarControl" Language="vb" %>
<%@ Register TagPrefix="dnnforum" Namespace="DotNetNuke.Modules.Forum.WebControls" Assembly="DotNetNuke.Modules.Forum" %>
<table id="tblMain" runat="server" cellpadding="5" border="0" cellspacing="0">
    <tr>
        <td>
            <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed"></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:DataList ID="dlAvatars" runat="server" RepeatDirection="horizontal" RepeatColumns="3" ItemStyle-VerticalAlign="Bottom">
                <ItemTemplate>
                    <asp:Image ID="imgAvatar" runat="server" />
                    <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/images/delete.gif" CommandName="Delete" />
                    <asp:Label ID="lblImageName" runat="server" Visible="false" />
                </ItemTemplate>
            </asp:DataList>
        </td>
    </tr>
    <tr id="trAvatarPool" runat="server">
        <td>
            <asp:LinkButton ID="cmdBrowsePool" runat="server" CssClass="CommandButton" CausesValidation="false" resourcekey="cmdViewPool" />
        </td>
    
    </tr>
    <tr>
        <td id="trFileUpload" runat="server">
		<asp:FileUpload ID="fuFile" runat="server" CssClass="NormalTextBox" Width="300px" />
		<asp:LinkButton resourcekey="Upload" runat="server" CssClass="CommandButton" ID="cmdUpload" />
		</td>
    </tr>
</table>
<table id="tblAvatarPool" runat="server" visible="false" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Forum_Row_AdminText" style="padding-top:5px"><asp:Label ID="lblPoolHeader" runat="server" />:</td>
    </tr>
    <tr valign="top">
        <td class="Forum_Row_AdminBox">
            <asp:DataList ID="dlAvatarPool" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" CellSpacing="1" CellPadding="2" >
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="Forum_Row_AdminBox" />
                <ItemTemplate>
                    <asp:Image ID="imgBlank" runat="server" />
                    <asp:ImageButton ID="imgAvatar" runat="server" CommandName="Update" />
                    <asp:Label ID="lblImageName" runat="server" Visible="false" />
                 </ItemTemplate>
            </asp:DataList>
        </td>
    </tr>
    <tr>
        <td><dnnforum:AjaxPager ID="BottomPager" runat="server" Width="100%"/></td>
    </tr>
    <tr>
        <td align="center"><asp:LinkButton ID="cmdCancel" runat="server" recourcekey="cmdCancel" CssClass="CommandButton" /><br /><br /></td>
    </tr>
</table>