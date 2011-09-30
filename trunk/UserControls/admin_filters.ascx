<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="admin_filters.ascx.cs"
    Inherits="DotNetNuke.Modules.Forums.UserControls.admin_filters" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<h2 class="dnnFormSectionHead">
    Filters</h2>
<a href="#" onclick="loadFilter(-1); return false;" class="dnnPrimaryAction">[RESX:AddFilter]</a>
<div class="dnnForumList">
    <ul id="filtersList"></ul>
</div>
<div id="dnnFilterEditor" style="display: none;">
    <div class="dnnForm">
        <div class="dnnFormItem">
            <dnn:Label ControlName="Find" ResourceKey="[RESX:FilterFind]" runat="server" />
            <input type="text" id="Find" name="Find" title="[RESX:FilterFind]" class="dnnFormRequired"
                value="" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ControlName="Replace" ResourceKey="[RESX:FilterReplace]" runat="server" />
            <input type="text" id="Replace" name="Replace" title="[RESX:FilterReplace]" class="dnnFormRequired"
                value="" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ControlName="FilterType" ResourceKey="[RESX:FilterType]" runat="server" />
            <select id="FilterType" name="FilterType">
                <option value="text">[RESX:Text]</option>
                <option value="regex">[RESX:RegEx]</option>
                <option value="markup">[RESX:Markup]</option>
            </select>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ControlName="ApplyOnEdit" ResourceKey="[RESX:ApplyOnEdit]" runat="server" />
            <span class="dnnForumRadioButtons">
                <input type="radio" name="ApplyOnEdit" value="false" id="ApplyOnEditNo" />
                <label for="ApplyOnEditNo">
                    [RESX:No]</label>
                <input type="radio" name="ApplyOnEdit" value="true" id="ApplyOnEdit" isdefault="true"
                    checked="checked" />
                <label for="ApplyOnEdit">
                    [RESX:Yes]</label>
            </span>
        </div>
        <div class="dnnFormItem">
            <dnn:Label ControlName="ApplyOnRender" ResourceKey="[RESX:ApplyOnRender]" runat="server" />
            <span class="dnnForumRadioButtons">
                <input type="radio" name="ApplyOnRender" value="false" id="ApplyOnRenderNo" />
                <label for="ApplyOnRenderNo">
                    [RESX:No]</label>
                <input type="radio" name="ApplyOnRender" value="true" id="ApplyOnRenderYes" checked="checked" />
                <label for="ApplyOnRender">
                    [RESX:Yes]</label>
            </span>
        </div>
    </div>
    <ul class="dnnActions">
        <li><a href="#" onclick="saveFilter(); return false;" class="dnnPrimaryAction">[RESX:Save]</a></li>
        <li><a href="#" onclick="dialogClose('dnnFilterEditor'); return false;" class="dnnSecondaryAction">
            [RESX:Cancel]</a> </li>
    </ul>
    <input type="hidden" id="FilterId" name="FilterId" value="-1" />
</div>
