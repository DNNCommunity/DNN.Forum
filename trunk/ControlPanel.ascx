<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ControlPanel.ascx.cs"
    Inherits="DotNetNuke.Modules.Forums.ControlPanel" %>
    <script type="text/javascript">
        function loadForums() {
            dnnforums.admin.LoadView('dnnForumList', '');
            $.ajax({
                type: "POST",
                url: "/desktopmodules/dnncorp/forums/Services/forums.asmx/GetForums",
                data: "{'moduleId' : '421' }",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    bindForums(data.d);  
                },
                error: function (xhr, status, error) {
                    alert(error);
                }
            });
        };
        function bindForums(rows) {
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
        function loadForum(forumId) {
            dnnforums.admin.LoadView('dnnForumEditor', '');
            $.ajax({
                type: "POST",
                url: "/desktopmodules/dnncorp/forums/Services/forums.asmx/GetForum",
                data: "{'forumId' : '" + forumId + "' }",
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
        function bindForum(row) {
            for (var obj in row) {
                $('#' + obj).val(row[obj]);
            }

        };
    </script>
<div class="dnnForumShell dnnClear">
    <div class="dnnForumNav dnnLeft">
        <ul>
            <li id="nav-dnnForumHome">
                <a href="#" onclick="dnnforums.admin.LoadView('dnnForumHome', ''); return false;">
                    <div>[RESX:Dashboard]</div>
                </a>
            </li>
            <li id="nav-dnnForumList"><a href="#" onclick="loadForums(); return false;">
                <div>
                    Forums</div>
            </a></li>
            <li id="nav-dnnForumFilters"><a href="#" onclick="dnnforums.admin.LoadView('dnnForumFilters', ''); return false;">
                <div>
                    Filters</div>
            </a></li>
            <li id="nav-dnnForumRanks"><a href="#" onclick="dnnforums.admin.LoadView('dnnForumRanks', ''); return false;">
                <div>
                    Ranks</div>
            </a></li>
            <li id="nav-dnnForumTemplates"><a href="#" onclick="dnnforums.admin.LoadView('dnnForumTemplates', ''); return false;">
                <div>
                    Templates</div>
            </a></li>
            <li id="nav-dnnForumSettings"><a href="#" onclick="dnnforums.admin.LoadView('dnnForumSettings', ''); return false;">
                <div>
                    Settings</div>
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
                    <div class="dashOuter dnnRight" style="width: 29%; margin-right: 4px;">
                        <div class="dashOuterHeader">
                            Header</div>
                        <div class="dashInner">
                            <div class="dashHD">
                                Sub Header</div>
                            <ul>
                                <li>Item 1</li>
                                <li>Item 2</li>
                                <li>Item 3</li>
                            </ul>
                        </div>
                    </div>
                    <div class="dashOuter dnnLeft" style="width: 69%; margin-left: 4px;">
                        <div class="dashOuterHeader">
                            Header</div>
                        <div class="dashInner">
                            <div class="dashHD">
                                Sub Header</div>
                            <ul>
                                <li>Item 1</li>
                                <li>Item 2</li>
                                <li>Item 3</li>
                            </ul>
                        </div>
                    </div>
                    <div class="dashOuter dnnRight" style="width: 49%; margin-right: 4px;">
                        <div class="dashOuterHeader">
                            Header</div>
                        <div class="dashInner">
                            <div class="dashHD">
                                Sub Header</div>
                            <ul>
                                <li>Item 1</li>
                                <li>Item 2</li>
                                <li>Item 3</li>
                            </ul>
                        </div>
                    </div>
                    <div class="dashOuter dnnLeft" style="width: 49%; margin-left: 4px;">
                        <div class="dashOuterHeader">
                            Header</div>
                        <div class="dashInner">
                            <div class="dashHD">
                                Sub Header</div>
                            <ul>
                                <li>Item 1</li>
                                <li>Item 2</li>
                                <li>Item 3</li>
                            </ul>
                        </div>
                    </div>
                    <div class="dnnClear">
                    </div>
                    <div class="dashOuter" style="margin-left: 4px; margin-right: 4px;">
                        <div class="dashOuterHeader">
                            Header</div>
                        <div class="dashInner">
                            <div class="dashHD">
                                Sub Header</div>
                            <ul>
                                <li>Item 1</li>
                                <li>Item 2</li>
                                <li>Item 3</li>
                            </ul>
                        </div>
                    </div>
                </div>
          
                <div class="dnnClear dnnForumArea" id="dnnForumList">
                    <h2 class="dnnFormSectionHead">Forum</h2>
                    <a href="#" onclick="dnnforums.admin.LoadView('dnnForumEditor', ''); return false;" class="dnnPrimaryAction">Add Forum</a>
                    <div class="dnnForumList">
                      <ul id="forumrows">
                        <li>Test</li>
                      </ul>
                    </div>
                </div>
                <div id="dnnForumEditor" class="dnnTabs dnnClear dnnForumArea">
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
                                <input type="text" id="Name" name="Name" class="dnnFormRequired" value="" />
                            </div>
                            <div class="dnnFormItem">
                                <label for="Description">[RESX:ForumDescription]:</label>
                                <textarea id="Description" name="Description"></textarea>
                            </div>
                            <div class="dnnFormItem">
                                <label for="allowTopics">[RESX:AllowTopics]:</label>
                                <input type="checkbox" id="allowTopics" name="allowTopics" value="true" />
                            </div>
                            <div class="dnnFormItem">
                                <label for="active">[RESX:Active]:</label>
                                <input type="checkbox" id="Active" name="Active" value="true" checked="checked" />
                            </div>
                            <div class="dnnFormItem">
                                <label for="hidden">[RESX:Hidden]:</label>
                                <input type="checkbox" id="Hidden" name="Hidden" value="true" />
                            </div>
                            <div class="dnnFormItem">
                                <label for="slug">[RESX:UrlPrefix]:</label>
                                <input type="text" id="Slug" name="Slug" value="" />
                            </div>
                            <div class="dnnFormItem">
                                <label for="slug">[RESX:EmailAddress]:</label>
                                <input type="text" id="EmailAddress" name="EmailAddress"  value="" />
                            </div>
                            <input type="hidden" id="ForumId" name="ForumId" value="" />
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
                <div class="dnnForumArea" id="dnnForumFilters">
                   <h2 class="dnnFormSectionHead">Filters</h2>
                </div>
                <div class="dnnForumArea" id="dnnForumRanks">
                    <h2 class="dnnFormSectionHead">Ranking</h2>
                </div>
                <div class="dnnForumArea" id="dnnForumSettings">
                    <h2 class="dnnFormSectionHead">Settings</h2>
                </div>
                <div class="dnnForumArea" id="dnnForumTemplates">
                    <h2 class="dnnFormSectionHead">Templates</h2>
                </div>
            </div>
            <div class="dnnForumActionArea">
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    dnnforums.history.Init();
    dnnforums.admin.Init();
</script>
