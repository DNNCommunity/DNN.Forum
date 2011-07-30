<%@ Control language="vb" CodeBehind="ACP_Community.ascx.vb" AutoEventWireup="false" Inherits="DotNetNuke.Modules.Forum.ACP.Community" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" TagPrefix="dnnweb" %>
<div class="dnnForm acpCommunity dnnClear">
    <h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" /></h2>
    <fieldset>
        <div class="dnnFormItem" id="divUserOnline" runat="server" visible="false">
            <dnn:label id="plUserOnline" runat="server" controlname="chkUserOnline" />
            <asp:checkbox id="chkUserOnline" runat="server" />
        </div>
        <div class="dnnFormItem" id="divEnableExtProfile" runat="server">
            <dnn:label id="plEnableExtProfilePage" runat="server" Suffix=":" controlname="chkEnableExtProfilePage" />
            <asp:checkbox id="chkEnableExtProfilePage" runat="server" AutoPostBack="True" />
        </div>
        <div class="dnnFormItem" id="divExtProfilePageID" runat="server">
            <dnn:label id="plExtProfilePageID" runat="server" Suffix=":" controlname="ddlExtProfilePageID" />
            <asp:DropDownList ID="rcbExtProfilePageID" runat="server" DataTextField="TabName" DataValueField="TabId" />
        </div>
        <div class="dnnFormItem" id="divExtProfileUserParam" runat="server">
            <dnn:label id="plExtProfileUserParam" runat="server" Suffix=":" controlname="txtExtProfileUserParam" />
            <asp:TextBox id="txtExtProfileUserParam" runat="server" />
        </div>
        <div class="dnnFormItem" id="divExtProfileParamName" runat="server">
            <dnn:label id="plExtProfileParamName" runat="server" Suffix=":" controlname="txtExtProfileParamName" />
            <asp:TextBox id="txtExtProfileParamName" runat="server" />
        </div>
        <div class="dnnFormItem" id="divExtProfileParamValue" runat="server">
            <dnn:label id="plExtProfileParamValue" runat="server" Suffix=":" controlname="txtExtProfileParamValue" />
            <asp:TextBox id="txtExtProfileParamValue" runat="server" />
        </div>
        <ul class="dnnActions dnnClear">
            <li><asp:linkbutton cssclass="dnnPrimaryAction" id="cmdUpdate" runat="server" text="Update" resourcekey="cmdUpdate" /></li>
        </ul>
    </fieldset>
</div>