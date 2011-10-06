<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ForumsSettings.ascx.cs" Inherits="DotNetNuke.Modules.Forums.ForumsSettings" %>

<script src="/DesktopModules/DNNCorp/Forums/Scripts/codemirror/codemirror.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="/DesktopModules/DNNCorp/Forums/CSS/linenumbers.css" />

<asp:DropDownList runat="server" ID="TemplateGroupList" AutoPostBack="true" OnSelectedIndexChanged="TemplateGroupListSelectedIndexChanged" />
<asp:DropDownList runat="server" ID="TemplateList" AutoPostBack="true" OnSelectedIndexChanged="TemplateListSelectedIndexChanged" />
<asp:TextBox runat="server" TextMode="MultiLine" ID="TemplateTextBox" />

<script type="text/javascript">
/* globals jQuery, window, Sys */
(function ($, Sys) {
    function setUpMyModule() {
        var editor = CodeMirror.fromTextArea('<%= TemplateTextBox.ClientID %>', {
            height: 'dynamic',
            width: "700px",
            parserfile: "parsexml.js",
            stylesheet: "/DesktopModules/DNNCorp/Forums/CSS/xmlcolors.css",
            path: "/DesktopModules/DNNCorp/Forums/Scripts/codemirror/",
            lineNumbers: true
        });

        $('a.dnnPrimaryAction').click(function(e) {
            editor.save();
        });
    }

    $(document).ready(function() {
        setUpMyModule();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
            setUpMyModule();
        });
    });
}(jQuery, window.Sys))
</script>