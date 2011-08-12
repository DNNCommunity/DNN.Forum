<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="CPanel.ascx.cs" Inherits="DotNetNuke.Modules.Forums.CPanel" %>
<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<dnnweb:DnnCodeBlock ID="rcbProjectDetail" runat="server"></dnnweb:DnnCodeBlock>
<div class="dnnForumShell dnnClear">
    <div class="dnnForumNav dnnLeft">
        <ul>
            <li><asp:LinkButton ID="cmdMain" runat="server" Text="Dashboard" CommandArgument="0" CausesValidation="false" OnClick="OnNavigationClick" CommandName="Control" /></li>
            <li><asp:LinkButton ID="cmdForums" runat="server" Text="Forums" CommandArgument="1" CausesValidation="false" OnClick="OnNavigationClick" CommandName="Control" /></li>
        </ul>
    </div>
    <div class="dnnForumContent dnnLeft">
        <asp:PlaceHolder ID="phUserControl" runat="server" />
    </div>
</div>