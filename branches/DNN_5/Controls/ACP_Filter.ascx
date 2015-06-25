<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Control Inherits="DotNetNuke.Modules.Forum.ACP.Filter" CodeBehind="ACP_Filter.ascx.vb" language="vb" AutoEventWireup="false" Explicit="true" %>
<div class="dnnForm acpFilter dnnClear">
    <h2 class="dnnFormSectionHead"><asp:Label id="lblTitle" runat="server" resourcekey="lblTitle" EnableViewState="false"/></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="plBadWord" runat="server" Suffix=":" controlname="chkBadWord" />
            <asp:checkbox id="chkBadWord" runat="server" AutoPostBack="True" />
        </div>
        <div class="dnnFormItem" id="divSubjectFilter" runat="server">
            <dnn:label id="plFilterSubject" runat="server" Suffix=":" controlname="chkFilterSubject" />
            <asp:checkbox id="chkFilterSubject" runat="server" />
        </div>
    </fieldset>
    <ul class="dnnActions dnnClear">
        <li><asp:linkbutton id="cmdUpdate" runat="server" resourcekey="cmdUpdate" cssclass="dnnPrimaryAction" /></li>
    </ul>
</div>