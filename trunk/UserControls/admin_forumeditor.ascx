<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="admin_forumeditor.ascx.cs"
    Inherits="DotNetNuke.Modules.Forums.UserControls.admin_forumedit" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<h2 class="dnnFormSectionHead">
    Edit Forum</h2>
<div id="forumTabs" class="dnnForm dnnTabs">
    <ul class="dnnAdminTabNav dnnClear">
        <li><a href="#forumTab1">[RESX:Details]</a></li>
        <li><a href="#forumTab2">[RESX:Permissions]</a></li>
        <li><a href="#forumTab3">[RESX:Features]</a></li>
    </ul>
    <div id="forumTab1">
        
        <div class="dnnFormItem">
            <label for="ParentId">
                [RESX:ParentForum]:</label>
            <select id="ParentId" name="ParentId">
                <option value="-1">-None-</option>
            </select>
        </div>
        <div class="dnnFormItem">
            <label for="Name">
                [RESX:ForumName]:</label>
            <input type="text" id="Name" name="Name" class="dnnFormRequired" value="" />
        </div>
        <div class="dnnFormItem">
            <label for="Description">
                [RESX:ForumDescription]:</label>
            <textarea id="Description" name="Description"></textarea>
        </div>
        <div class="dnnFormItem">
            <label for="allowTopics">
                [RESX:AllowTopics]:</label>
            <input type="checkbox" id="allowTopics" name="allowTopics" value="true" />
        </div>
        <div class="dnnFormItem">
            <label for="active">
                [RESX:Active]:</label>
            <input type="checkbox" id="Active" name="Active" value="true" checked="checked" />
        </div>
        <div class="dnnFormItem">
            <label for="hidden">
                [RESX:Hidden]:</label>
            <input type="checkbox" id="Hidden" name="Hidden" value="true" />
        </div>
        <div class="dnnFormItem">
            <label for="slug">
                [RESX:UrlPrefix]:</label>
            <input type="text" id="Slug" name="Slug" value="" />
        </div>
        <div class="dnnFormItem">
            <label for="slug">
                [RESX:EmailAddress]:</label>
            <input type="text" id="EmailAddress" name="EmailAddress" value="" />
        </div>
        <input type="hidden" id="ForumId" name="ForumId" value="" />
        <div id="controlActions">
            <ul class="dnnActions dnnClear">
                <li><a href="#" onclick="saveForum(); return false;" class="dnnPrimaryAction">[RESX:Button:Save]</a></li>
                <li><a href="#" onclick="dnnforums.admin.LoadView('forums',''); return false;" class="dnnSecondaryAction">
                    [RESX:Button:Cancel]</a></li>
            </ul>
        </div>
    
    </div>
    <div id="forumTab2">
        [RESX:Permissions]
        <div id="forumPermGrid">
            <table>
                <tr>
                    <td>
                        <div class="forumPermObjects">
                            <table id="forumPermObject">
                                <thead>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                    <td style="width:95%">
                        <div class="forumPermActions">
                            <table id="forumPermActions">
                                <thead>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
            
            
        
        </div>
    </div>
    <div id="forumTab3">
        [RESX:Features]
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {

        var options = { disabled: [] };
        $('#forumTabs').dnnTabs(options);
        var regUsers = {};
        regUsers.Name = 'Registered Users';
        regUsers.Id = 2;
        regUsers.Type = 'r';
        var adminUsers = {};
        adminUsers.Name = 'Administrators';
        adminUsers.Id = 1;
        adminUsers.Type = 'r';
        var objects = [regUsers, adminUsers];
        var permissions = ['CanView', 'CanRead', 'CanCreate', 'CanEdit', 'CanDelete', 'CanLock', 'CanPin', 'CanAttach', 'CanPoll', 'CanBlock', 'CanTrust', 'CanSubscribe', 'CanAnnounce', 'CanTag'];
        $('#forumPermObject tbody').empty();
        $('#forumPermActions thead tr').empty();
        $('#forumPermActions tbody').empty();
        for (var i = 0; i < permissions.length; i++) {
            $('#forumPermActions thead tr').append('<td>' + permissions[i] + '</td>');
        }
        for (var i = 0; i < objects.length; i++) {
            var obj = objects[i];
            $('#forumPermObject tbody').append('<tr><td>' + obj.Name + '</td></tr>');
            var key = obj.Type + '-' + obj.Id;
            $('#forumPermActions tbody').append($('<tr></tr>').attr('id', key));
            for (var x = 0; x < permissions.length; x++) {
                $('#' + key).append('<td> x </td>');
            }

        }

    });
    function saveForum() {
        currForum = $.extend(currForum, $('#forumTab1 input').serializeObject());
        currForum = $.extend(currForum, $('#forumTab1 select').serializeObject());
        currForum = $.extend(currForum, $('#forumTab1 textarea').serializeObject());
        delete currForum.LastModifiedOnDate;
        delete currForum.CreatedOnDate;
        delete currForum.__type;
        alert(JSON.stringify(currForum));
        $.ajax({
            type: "POST",
            url: "/desktopmodules/dnncorp/forums/Services/forums.asmx/ForumSave",
            data: JSON.stringify(currForum),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                bindForum(data.d);
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    };
   
</script>
