<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminForumsEdit.ascx.cs" Inherits="DotNetNuke.Modules.Forums.UserControls.AdminForumsEdit" %>
<div id="dnnForumEditor" class="dnnForm dnnTabs dnnClear">
    <h2 class="dnnFormSectionHead">Edit Forum</h2>
    <ul class="dnnAdminTabNav dnnClear">
        <li><a href="#forumTab1"><span>[RESX:Details]</span></a></li>
        <li><a href="#forumTab2"><span>[RESX:Permissions]</span></a></li>
        <li><a href="#forumTab3"><span>[RESX:Features]</span></a></li>
    </ul>
    <div id="forumTab1">
        <form id="dnnForumEdit">
            <div class="dnnFormItem">
                <label for="ParentId">[RESX:ParentForum]:</label>
                <select id="ParentId" name="ParentId">
                    <option value="-1">-None-</option>
                </select>
            </div>
            <div class="dnnFormItem">
                <label for="Name">[RESX:ForumName]:</label>
                <input type="text" id="Name" name="Name" class="dnnFormRequired" value="<%=Model.forumInfo.Name %>" />
            </div>
            <div class="dnnFormItem">
                <label for="Description">[RESX:ForumDescription]:</label>
                <textarea id="Description" name="Description"><%=Model.forumInfo.Description %></textarea>
            </div>
            <div class="dnnFormItem">
                <label for="allowTopics">[RESX:AllowTopics]:</label>
                <% if (Model.forumInfo.AllowTopics) {%>
                <input type="checkbox" id="allowTopics" name="allowTopics" value="true" checked="checked" />
                <%} else { %>
                <input type="checkbox" id="allowTopics" name="allowTopics" value="true" />
                <%} %>
            </div>
            <div class="dnnFormItem">
                <label for="active">[RESX:Active]:</label>
                <% if (Model.forumInfo.Active) {%>
                <input type="checkbox" id="active" name="active" value="true" checked="checked" />
                <%} else { %>
                <input type="checkbox" id="active" name="active" value="true" />
                <%} %>
                
            </div>
            <div class="dnnFormItem">
                <label for="hidden">[RESX:Hidden]:</label>
                
                 <% if (Model.forumInfo.Hidden) {%>
                <input type="checkbox" id="hidden" name="hidden" value="true" checked="checked" />
                <%} else { %>
                <input type="checkbox" id="hidden" name="hidden" value="true" />
                <%} %>
            </div>
            <div class="dnnFormItem">
                <label for="slug">[RESX:UrlPrefix]:</label>
                <input type="text" id="slug" name="slug" value="<%=Model.forumInfo.Slug %>" />
            </div>
            <div class="dnnFormItem">
                <label for="slug">[RESX:EmailAddress]:</label>
                <input type="text" id="emailAddress" name="emailAddress"  value="<%=Model.forumInfo.EmailAddress %>" />
            </div>
            <input type="hidden" id="forumId" name="forumId" value="<%=Model.forumInfo.ForumId %>" />
            <div id="controlActions" style="display:none;">
                <ul class="dnnActions dnnClear">
                    <li><a href="#" onclick="saveForum(); return false;" class="dnnPrimaryAction">[RESX:Button:Save]</a></li>
                    <li><a href="#" onclick="dnnforums.admin.LoadView('forums',''); return false;" class="dnnSecondaryAction">[RESX:Button:Cancel]</a></li>
                </ul>   
            </div>
        </form>
    </div>
    <div id="forumTab2">
    [RESX:Permissions]
    </div>
    <div id="forumTab3">
    [RESX:Features]
    </div>
</div>
<script type="text/javascript" language="javascript">
    function saveForum() {
        var result = jQuery('#dnnForumEdit').serializeObject();
        result.action = 'save';
        dnnforums.admin.LoadView('forumsedit', document.getElementById('forumId').value, result, saveComplete);
    }
    function saveComplete() {
        dnnforums.admin.ShowMsg('[RESX:SaveComplete]', 'positive');
        setTimeout(function () {
            dnnforums.admin.HideMsg();
        }, 1600);
    }
</script>