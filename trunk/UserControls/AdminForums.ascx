<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminForums.ascx.cs" Inherits="DotNetNuke.Modules.Forums.AdminForums" %>
Forums
<a href="#" onclick="dnnforums.admin.LoadView('forumsedit', ''); return false;" class="dnnPrimaryAction">Add Forum</a>
<div class="dnnForumList">
    <asp:Literal ID="litForums" runat="server" />
</div>
<script type="text/javascript">
    (function ($) {
        $('.dnnForumList ul li').click(function () {
            var forumid = $(this).attr('id');
            dnnforums.admin.LoadView('forumsedit', forumid, null);
        });
    })(jQuery);
</script>