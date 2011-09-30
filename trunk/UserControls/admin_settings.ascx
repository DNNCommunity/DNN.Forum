<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="admin_settings.ascx.cs" Inherits="DotNetNuke.Modules.Forums.UserControls.admin_settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
  <h2 class="dnnFormSectionHead">
                        Settings</h2>
                    <h2 id="Panel-Main" class="dnnFormSectionHead">
                        <a href="" class="">[RESX:MainFeatures]</a></h2>
                    <fieldset>
                        <div class="dnnFormItem">
                            <dnn:Label ControlName="ms_Subscriptions" ResourceKey="[RESX:Theme]" runat="server" />
                            <span class="dnnForumRadioButtons">
                                <input type="radio" id="ms_Subscriptions_True" name="ms_Subscriptions" value="true" /><label
                                    for="ms_Subscriptions_True">[RESX:Yes]</label>
                                <input type="radio" id="ms_Subscriptions_False" name="ms_Subscriptions" value="false" /><label
                                    for="ms_Subscriptions_False">[RESX:No]</label>
                            </span>
                        </div>
                        <div class="dnnFormItem">
                            <dnn:Label ControlName="ms_Theme" ResourceKey="[RESX:Theme]" runat="server" />
                            <select id="ms_Theme">
                            </select>
                        </div>
                        <div class="dnnFormItem">
                            <dnn:Label ControlName="ms_MainTemplate" ResourceKey="[RESX:MainTemplate]" runat="server" />
                            <select id="ms_MainTemplate">
                            </select>
                        </div>
                        <div class="dnnFormItem">
                            <dnn:Label ControlName="ms_DefaultPageSize" ResourceKey="[RESX:DefaultPageSize]"
                                runat="server" />
                            <select id="ms_DefaultPageSize">
                            </select>
                        </div>
                        <div class="dnnFormItem">
                            <dnn:Label ControlName="ms_DateFormat" ResourceKey="[RESX:DateFormat]" runat="server" />
                            <select id="ms_DateFormat">
                            </select>
                        </div>
                        <div class="dnnFormItem">
                            <dnn:Label ControlName="ms_TimeFormat" ResourceKey="[RESX:TimeFormat]" runat="server" />
                            <select id="ms_TimeFormat">
                            </select>
                        </div>
                        <div class="dnnFormItem">
                            <dnn:Label ControlName="ms_FloodInterval" ResourceKey="[RESX:FloodInterval]" runat="server" />
                            <select id="ms_FloodInterval">
                            </select>
                        </div>
                        <div class="dnnFormItem">
                            <dnn:Label ControlName="ms_FloodInterval" ResourceKey="[RESX:FloodInterval]" runat="server" />
                            <select id="Select1">
                            </select>
                        </div>
                    </fieldset>
                