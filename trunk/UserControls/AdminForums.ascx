<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminForums.ascx.cs" Inherits="DotNetNuke.Modules.Forums.UserControls.AdminForums" %>

<div class="dnnForm dnnAdminForums dnnClear">
    <h2 class="dnnFormSectionHead">Forum</h2>
    <a href="#" class="dnnPrimaryAction">Add Forum</a>
    <div class="dnnForumList">
        <asp:Literal ID="litForums" runat="server" />
    </div>
</div>

<script type="text/javascript" language="javascript">
    (function ($) {
        $('.dnnForumList ul li').click(function () {
            var forumid = $(this).attr('id');
            dnnforums.admin.LoadView('forumsedit', forumid, null);
        });

        $('.dnnAdminForums a.dnnPrimaryAction').click(function (e) {
            e.preventDefault();
            dnnforums.admin.LoadView('forumsedit', '');
        });
    })(jQuery);
</script>