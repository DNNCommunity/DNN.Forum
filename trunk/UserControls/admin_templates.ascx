<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="admin_templates.ascx.cs" Inherits="DotNetNuke.Modules.Forums.admin_templates" %>

<div class="forumTemplateEditor dnnForm">
    <h2>Manage Templates</h2>
    <p class="dnnFormMessage" style="display:none;"></p>
    <p>Each module view has a template. Pick the group that you'd like to edit, then pick the particular file you'd like to edit. Once you've made your changes click save.</p>
    <fieldset>
        <div class="dnnFormItem">
            <label for="groupList">Template Group</label>
            <select id="groupList"></select>
        </div>

        <div class="dnnFormItem">
            <label for="groupFileList">Template</label>
            <select id="groupFileList"></select>
        </div>
        <div class="dnnFormItem">
            <label for="fileContents">Editor</label>
            <textarea id="fileContents"></textarea>
        </div>
    </fieldset>
    <ul class="dnnActions dnnClear">
        <li><a href="#" class="dnnPrimaryAction save">Save</a></li>
        <li><a href="#" class="dnnSecondaryAction clear">Start over</a></li>
    </ul>
</div>

<script src="/DesktopModules/DNNCorp/Forums/Scripts/codemirror/codemirror.js" type="text/javascript"></script>
<script src="/Resources/Shared/Scripts/json2.js" type="text/javascript"></script>
<script src="/DesktopModules/DNNCorp/Forums/Scripts/serviceProxy.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="/DesktopModules/DNNCorp/Forums/CSS/linenumbers.css" />

<script type="text/javascript">
/* globals jQuery, window, Sys */
    (function ($, window) {

        function getTemplateGroups() {
            var proxy = new ServiceProxy("/DesktopModules/DNNCorp/Forums/Services/Templates.asmx/");
            proxy.invoke("GetTemplateGroups", {}, function (data) {
                $("select#groupList").html(buildOptions(data));
            });
        }

        getTemplateGroups();

        $('select#groupList').change(function () {
            var selectedGroupName = $(this).val();

            if (selectedGroupName === "-1") {
                $('select#groupFileList').html('');
                return;
            }

            var proxy = new ServiceProxy("/DesktopModules/DNNCorp/Forums/Services/Templates.asmx/");
            proxy.invoke("GetGroupFiles", { groupName: selectedGroupName }, function (data) {
                $("select#groupFileList").html(buildOptions(data));
            });
        });

        $('select#groupFileList').change(function () {
            var selectedGroupName = $('select#groupList').val();
            var selectedFileName = $(this).val();

            var invalid = false;

            if (selectedGroupName === "-1") {
                $('select#groupFileList').html('');
                invalid = true;
            }

            if (selectedFileName === "-1") {
                invalid = true;
            }

            if (invalid) {
                return;
            }

            var proxy = new ServiceProxy("/DesktopModules/DNNCorp/Forums/Services/Templates.asmx/");
            proxy.invoke("GetFile", { groupName: selectedGroupName, fileName: selectedFileName }, function (data) {
                $("textarea#fileContents").html(data);
                if (window['CodeMirrorEditor'] != undefined) {
                    CodeMirrorEditor.setCode(data);
                }
                else {
                    setUpEditor();
                }
            });
        });

        function buildOptions(data) {
            var options = '<option value="-1"> - Please select one - </option>';
            for (var i = 0; i < data.length; i++) {
                options += '<option value="' + data[i] + '">' + data[i] + '</option>';
            }
            return options;
        }

        function setUpEditor() {

            // wire up syntax highlighting
            window.CodeMirrorEditor = CodeMirror.fromTextArea('fileContents', {
                height: 'dynamic',
                parserfile: "parsexml.js",
                stylesheet: "/DesktopModules/DNNCorp/Forums/CSS/xmlcolors.css",
                path: "/DesktopModules/DNNCorp/Forums/Scripts/codemirror/",
                lineNumbers: true
            });
        }

        // wire up save event
        // this pushes the editor content back down into the text area.
        $('.forumTemplateEditor a.dnnPrimaryAction').click(function (e) {
            e.preventDefault();

            var selectedGroupName = $('select#groupList').val();
            var selectedFileName = $(this).val();
            
            var invalid = false;

            if (selectedGroupName === "-1") {
                $('select#groupFileList').html('');
                invalid = true;
            }

            if (selectedFileName === "-1") {
                invalid = true;
            }

            if (invalid) {
                return;
            }
            
            CodeMirrorEditor.save();

            var groupName = $('select#groupList').val();
            var fileName = $('select#groupFileList').val();
            var fileContents = $('textarea#fileContents').val();

            var proxy = new ServiceProxy("/DesktopModules/DNNCorp/Forums/Services/Templates.asmx/");
            proxy.invoke("SaveFile", { groupName: groupName, fileName: fileName, fileContents: fileContents }, function (data) {
                var $message = $('.forumTemplateEditor .dnnFormMessage');
                if (data.Success) {
                    $message.addClass('dnnFormSuccess');
                    $message.removeClass('dnnFormError');
                }
                else {
                    $message.removeClass('dnnFormSuccess');
                    $message.addClass('dnnFormError');
                }
                $message.text(data.Message).show();
            });
        });

        // clear out the editor on group file change
        $('select#groupList, select#groupFileList').change(function () {
            CodeMirrorEditor.setCode('');
            CodeMirrorEditor.focus();
        });

        $('.dnnSecondaryAction.clear').click(function (e) {
            e.preventDefault();

            CodeMirrorEditor.setCode('');

            $(':input', '.forumTemplateEditor')
                    .not(':button, :submit, :reset, :hidden')
                    .val('')
                    .removeAttr('checked')
                    .removeAttr('selected');
        });

    } (jQuery, window))
</script>