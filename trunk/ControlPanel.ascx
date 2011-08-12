<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ControlPanel.ascx.cs" Inherits="DotNetNuke.Modules.Forums.ControlPanel" %>
<div class="dnnForumShell dnnClear">
    <div class="dnnForumNav dnnLeft">
        <ul>
            <li id="nav-main"><a href="#" onclick="dnnforums.admin.LoadView('main', ''); return false;"><div>Dashboard</div></a></li>
            <li id="nav-forums"><a href="#" onclick="dnnforums.admin.LoadView('forums', ''); return false;"><div>Forums</div></a></li>
            <li id="nav-filters"><a href="#" onclick="dnnforums.admin.LoadView('filters', ''); return false;"><div>Filters</div></a></li>
            <li id="nav-ranks"><a href="#" onclick="dnnforums.admin.LoadView('ranks', ''); return false;"><div>Ranks</div></a></li>
            <li id="nav-templates"><a href="#" onclick="dnnforums.admin.LoadView('templates', ''); return false;"><div>Templates</div></a></li>
            <li id="nav-settings"><a href="#" onclick="dnnforums.admin.LoadView('settings', ''); return false;"><div>Settings</div></a></li>
       </ul>
    </div>
    <div class="dnnForumContent dnnLeft">
        <div id="dnnForumMsgArea">
            <div></div>
        </div>
        <div id="dnnForumContent">
            <div id="dnnForumControl">
                <asp:PlaceHolder ID="phUserControl" runat="server" />
            </div>
            <div class="dnnForumActionArea"></div>
        </div>
    </div>
</div>
<script type="text/javascript">
    dnnforums.history.Init();
    dnnforums.admin.Init();
</script>