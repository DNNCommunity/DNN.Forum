<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" Codebehind="MCP_Main.ascx.vb" Inherits="DotNetNuke.Modules.Forum.MCP.Main" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm mcpMain dnnClear">
    <h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false" /></h2>
    <div>
        <p><asp:Label ID="lblInfo" runat="server" resourcekey="lblInfo" EnableViewState="false" /></p>
        <h4>Moderation Overview</h4>
        <asp:Label ID="lblPostQueue" runat="server" EnableViewState="false" /><br />
        <asp:Label ID="lblReportedPosts" runat="server" EnableViewState="false" />
    </div>
</div>