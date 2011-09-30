<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopicEditor.ascx.cs" Inherits="DotNetNuke.Modules.Forums.TopicEditor" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForumsTopicDisplay">
    <p>TopicEditor</p>
</div>
<div class="dnnForm">
    <div class="dnnFormItem">
           <dnn:Label ControlName="txtSubject" ResourceKey="Subject" Text="Subject" runat="server" />
           <asp:textbox id="txtSubject" cssclass="dnnFormRequired" runat="server" />
    </div>
    <div class="dnnFormItem">
           <dnn:Label ControlName="txtBody" ResourceKey="Body" Text="Body" runat="server" />
           <asp:textbox id="txtBody" cssclass="dnnFormRequired" runat="server" />
    </div>
    <asp:linkbutton id="SaveButton" ResourceKey="CreateTopic" text="Create Topic" runat="server" cssclass="dnnPrimaryAction" />
</div>