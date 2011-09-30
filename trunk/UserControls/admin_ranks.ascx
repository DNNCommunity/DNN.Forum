<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="admin_ranks.ascx.cs" Inherits="DotNetNuke.Modules.Forums.UserControls.admin_ranks" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<h2 class="dnnFormSectionHead">
                        Ranking</h2>
                    <a href="#" onclick="openRank(); return false;"
                        class="dnnPrimaryAction">[RESX:AddRank]</a>
                    <ul>
                    </ul>
                    <div id="dnnRankEditor" style="display: none;">
                        <div class="dnnForm">
                            <div class="dnnFormItem">
                                <dnn:Label ControlName="RankName" ResourceKey="[RESX:RankName]" runat="server" />
                                <input type="text" id="RankName" title="[RESX:RankName]" class="dnnFormRequired"
                                    value="" />
                            </div>
                            <div class="dnnFormItem">
                                <dnn:Label ControlName="MinPosts" ResourceKey="[RESX:MinPosts]" runat="server" />
                                <input type="text" id="MinPosts" title="[RESX:MinPosts]" class="dnnFormRequired"
                                    value="" />
                            </div>
                            <div class="dnnFormItem">
                                <dnn:Label ControlName="MaxPosts" ResourceKey="[RESX:MaxPosts]" runat="server" />
                                <input type="text" id="MaxPosts" title="[RESX:MaxPosts]" class="dnnFormRequired"
                                    value="" />
                            </div>
                            <div class="dnnFormItem">
                                <dnn:Label ControlName="RankDisplay" ResourceKey="[RESX:RankDisplay]" runat="server" />
                                <input type="text" id="RankDisplay" title="[RESX:RankDisplay]" class="dnnFormRequired"
                                    value="" />
                            </div>
                        </div>
                        <ul class="dnnActions">
                            <li><a href="#" onclick="rankSave(); return false;" class="dnnPrimaryAction">
                                [RESX:Save]</a></li>
                            <li><a href="#" onclick="dialogClose('dnnRankEditor'); return false;" class="dnnSecondaryAction">
                                [RESX:Cancel]</a> </li>
                        </ul>
                        <input type="hidden" id="RankId" value="-1" />
                    </div>