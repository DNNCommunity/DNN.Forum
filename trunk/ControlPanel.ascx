<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ControlPanel.ascx.cs"
    Inherits="DotNetNuke.Modules.Forums.ControlPanel" %>
<%@ Register TagPrefix="dnnforum" TagName="filters" Src="~/DesktopModules/DNNCorp/Forums/UserControls/admin_filters.ascx" %>
<%@ Register TagPrefix="dnnforum" TagName="ranks" Src="~/DesktopModules/DNNCorp/Forums/UserControls/admin_ranks.ascx" %>
<%@ Register TagPrefix="dnnforum" TagName="forumeditor" Src="~/DesktopModules/DNNCorp/Forums/UserControls/admin_forumeditor.ascx" %>
<%@ Register TagPrefix="dnnforum" TagName="forumlist" Src="~/DesktopModules/DNNCorp/Forums/UserControls/admin_forumlist.ascx" %>
<%@ Register TagPrefix="dnnforum" TagName="dashboard" Src="~/DesktopModules/DNNCorp/Forums/UserControls/admin_dashboard.ascx" %>
<%@ Register TagPrefix="dnnforum" TagName="settings" Src="~/DesktopModules/DNNCorp/Forums/UserControls/admin_settings.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<script type="text/javascript">
    var forums = [];

    function loadFilters() {
        dnnforums.admin.LoadView('dnnForumFilters', '');
        var data = {};
        data.PortalId = 0;
        data.ModuleId = 421;
        doCallback(data, 'filters.asmx/FilterList', bindFilters);
    }
    function bindFilters(rows) {
        $('#filtersList').empty();
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            $('#filtersList').append('<li id=\'' + row.FilterId + '\'>' + row.Find + '</li>');
            $('#filtersList li').click(function () {
                var fid = $(this).attr('id');
                loadFilter(fid);
            });
        }
    }
    function loadFilter(filterId) {
        dialogOpen('dnnFilterEditor', filterEditModalOptions);
        var data = {};
        currFilter = null;
        data.PortalId = 0;
        data.ModuleId = 421;
        data.FilterId = filterId;
        doCallback(data, 'filters.asmx/FilterGet', bindFilter);

    }
    var currFilter = {};
    function bindFilter(row) {
        currFilter = row;
        for (var obj in row) {
            $('#' + obj).val(row[obj]);
        }
    }
    function saveFilter() {
        currFilter = $.extend(currFilter, $('#dnnFilterEditor input').serializeObject());
        currFilter = $.extend(currFilter, $('#dnnFilterEditor select').serializeObject());
        delete currFilter.LastModifiedOnDate;
        delete currFilter.CreatedOnDate;
        delete currFilter.__type;
        doCallback(currFilter, 'filters.asmx/FilterSave', closeFilter);
    }
    function closeFilter(data) {
        currFilter = null;
        loadFilters();
        dialogClose('dnnFilterEditor'); 
    }
    function loadForums() {
        dnnforums.admin.LoadView('dnnForumList', '');
        var data = {};
        data.ModuleId = 421;
        doCallback(data, 'forums.asmx/ForumsList', bindForums);
        
    };
    function bindForums(rows) {
        forums = rows;
        $('#forumrows').empty();
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            $('#forumrows').append('<li id=\'' + row.ForumId + '\'>' + row.Name + '</li>');
            $('#forumrows li').click(function () {
                var fid = $(this).attr('id');
                loadForum(fid);
            });
        }
    };
    function doCallback(data, action, callback) {
        $.ajax({
            type: "POST",
            url: "/desktopmodules/dnncorp/forums/Services/" + action,
            data: JSON.stringify(data),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (typeof(callback) != "undefined") {
                    callback(data.d);
                } 
                
            },
            error: function (xhr, status, error) {
                alert(error);
            }
        });
    }
    function loadForum(forumId) {
        dnnforums.admin.LoadView('dnnForumEditor', '');
        currForum = null;
        var data = {};
        data.ForumId = forumId;
        doCallback(data,'forums.asmx/ForumGet',bindForum);
    };
    var currForum = {};
    function bindForum(row) {
        currForum = row;
        $('#ParentId').empty();
        $('#ParentId').append($("<option></option>").attr("value", -1).text('--None--'));
        for (var i = 0; i < forums.length; i++) {
            var forum = forums[i];
            if (currForum.ForumId != forum.ForumId) {
                $('#ParentId').append($("<option></option>").attr("value", forum.ForumId).text(forum.Name));
            }
        }
        for (var obj in row) {
            $('#' + obj).val(row[obj]);
        }

    };
    var rankEditModalOptions = {
        "height": 350,
        "width": 450,
        "title": "[RESX:RankEditor]"
    };
    var filterEditModalOptions = {
        "height": 350,
        "width": 550,
        "title": "[RESX:FilterEditor]"
    };
    function dialogOpen(id, options) {
        var $modal = $('#' + id);
        var width = $modal.width();
        var height = $modal.height();
        var modtitle = document.getElementById(id).getAttribute('title');
        if (typeof (options) != 'undefined') {
            width = options.width;
            height = options.height;
            modtitle = options.title;
        }
        $modal.dialog({
            modal: true,
            autoOpen: true,
            dialogClass: "dnnFormPopup",
            position: "center",
            minWidth: width,
            minHeight: height,
            maxWidth: width,
            maxHeight: height,
            resizable: false,
            closeOnEscape: true,
            title: modtitle,
            close: function (event, ui) {
                $(this).dialog("destroy");
            }
        });
        //.width(width - 11)
        //.height(height - 11);
    };
    function dialogClose(id) {
        $("#" + id).dialog("close");
        //$("#" + id).dialog("destroy");
    };
   
    function openRank() {
        dialogOpen('dnnRankEditor', rankEditModalOptions);
    }
    function rankSave() {
        alert('needs work');
    }

</script>
<div class="dnnForumShell dnnClear">
    <div class="dnnForumNav dnnLeft">
        <ul>
            <li id="nav-dnnForumHome"><a href="#" onclick="dnnforums.admin.LoadView('dnnForumHome', ''); return false;">
                <div>
                    [RESX:Dashboard]</div>
            </a></li>
            <li id="nav-dnnForumList"><a href="#" onclick="loadForums(); return false;">
                <div>
                    [RESX:Forums]</div>
            </a></li>
            <li id="nav-dnnForumFilters"><a href="#" onclick="loadFilters(); return false;">
                <div>
                    [RESX:Filters]</div>
            </a></li>
            <li id="nav-dnnForumRanks"><a href="#" onclick="dnnforums.admin.LoadView('dnnForumRanks', ''); return false;">
                <div>
                    [RESX:Ranks]</div>
            </a></li>
            <li id="nav-dnnForumTemplates"><a href="#" onclick="dnnforums.admin.LoadView('dnnForumTemplates', ''); return false;">
                <div>
                    [RESX:Templates]</div>
            </a></li>
            <li id="nav-dnnForumSettings"><a href="#" onclick="dnnforums.admin.LoadView('dnnForumSettings', ''); return false;">
                <div>
                    [RESX:Settings]</div>
            </a></li>
        </ul>
    </div>
    <div class="dnnForumContent dnnLeft">
        <div id="dnnForumMsgArea">
            <div>
            </div>
        </div>
        <div id="dnnForumContent">
            <div id="dnnForumControl" class="dnnForm">
                <div id="dnnForumHome" class="dnnClear dnnForumArea">
                    <dnnforum:dashboard runat="server" />
                </div>
                <div class="dnnClear dnnForumArea" id="dnnForumList">
                    <dnnforum:forumlist runat="server" />
                </div>
                <div id="dnnForumEditor" class="dnnTabs dnnClear dnnForumArea">
                    <dnnforum:forumeditor runat="server" />
                </div>
                <div class="dnnForumArea" id="dnnForumFilters">
                    <dnnforum:filters runat="server" />
                </div>
                <div class="dnnForumArea" id="dnnForumRanks">
                    <dnnforum:ranks runat="server" />
                </div>
                <div class="dnnForumArea" id="dnnForumSettings">
                    <dnnforum:settings runat="server" />
                </div>
                <div class="dnnForumArea" id="dnnForumTemplates">
                    <h2 class="dnnFormSectionHead">
                        Templates</h2>
                </div>
            </div>
            <div class="dnnForumActionArea">
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    jQuery(document).ready(function () {
        dnnforums.history.Init();
        dnnforums.admin.Init();
    });
  
</script>
