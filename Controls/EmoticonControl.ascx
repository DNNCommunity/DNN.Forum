<%@ Control AutoEventWireup="false" Codebehind="EmoticonControl.ascx.vb" Inherits="DotNetNuke.Modules.Forum.WebControls.EmoticonControl" Language="vb" %>
<asp:Panel ID="pnlContainer" runat="server">
    <table id="tblAdmin" runat="server" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left"><asp:Literal ID="litDisplayEmoticon" runat="server" /></td>
        </tr>
        <tr>
            <td align="left">
                <asp:DropDownList ID="cboFiles" runat="server" CssClass="NormalTextBox" DataTextField="Text"
                    DataValueField="Value" Width="260px" />
                <asp:Label ID="lblMessage" runat="server" CssClass="NormalRed" />
            </td>
        </tr>
        <tr>
            <td align="left">
		        <asp:FileUpload ID="fuFile" runat="server" CssClass="NormalTextBox" Width="260px" />
            </td>
        </tr>
        <tr>
            <td align="right"><asp:LinkButton resourcekey="Upload" runat="server" CssClass="CommandButton" ID="cmdUpload" /></td>
        </tr>
    </table>
    <table id="tblUser" runat="server" cellpadding="0" border="0" cellspacing="0">
        <tr>
            <td>
                <asp:Literal ID="litJavaScript" runat="server" />
                <asp:DataList ID="lstEmoticon" runat="server" CellPadding="2" CellSpacing="1" RepeatDirection="Vertical" RepeatColumns="10">
                    <ItemTemplate>
                        <asp:Literal ID="litEmoticon" runat="server" />
                    </ItemTemplate>
                </asp:DataList>
            </td>
            <td>&nbsp;<asp:HyperLink ID="hypShowAll" runat="server" CssClass="CommandButton" resourcekey="ShowAll" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Literal ID="litPopupStart" runat="server" />
                <asp:DataGrid ID="dgEmoticon" runat="server" GridLines="None" 
                    CellPadding="3" Cellspacing="0" Width="270px" AutoGenerateColumns="false"
                    ShowHeader="false" ShowFooter="false">
                    <Columns>
                        <asp:TemplateColumn ItemStyle-CssClass="Forum_EmoticonRowL" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Literal ID="litEmoticon" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                 </asp:DataGrid>
                <asp:Literal ID="litPopupEnd" runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel>