namespace DotNetNuke.Modules.Forums.Services
{
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Services;
    using System.Web.Services;
    using Components.Presenters;
    using Components.Views;
    using Web.Mvp;
    using WebFormsMvp;

    // sample ajax post to web method
//    $.ajax({
//      type: "POST",
//      contentType: "application/json; charset=utf-8",
//      url: "/DesktopModules/DNNCorp/Forums/Services/Templates.asmx/HelloWorld",
//      data: "{}",
//      dataType: "json",
//      success: function (data) {
//          console.log(data.d);
//      },
//      error: function (xhr, status, error) {
//          console.log(error);
//      }
//  });

    [WebService]
    [ScriptService]
    [PresenterBinding(typeof(TemplatesServicePresenter), ViewType = typeof(ITemplatesServiceView))]
    public class Templates : WebServiceView, ITemplatesServiceView
    {
        public event EventHandler<HelloWorldEventArgs> HelloWorldCalled;
        public event EventHandler<GroupsEventArgs> GetGroupsCalled;
        public event EventHandler<GroupFilesEventArgs> GetGroupFilesCalled;
        public event EventHandler<FileEventArgs> GetFileCalled;
        public event EventHandler<FileSaveEventArgs> FileSaveCalled;

        [WebMethod]
        public string HelloWorld()
        {
            var helloWorldEventArgs = new HelloWorldEventArgs();
            HelloWorldCalled(this, helloWorldEventArgs);
            return helloWorldEventArgs.Message;
        }

        [WebMethod]
        public List<String> GetTemplateGroups()
        {
            var groupsEventArgs = new GroupsEventArgs();
            GetGroupsCalled(this, groupsEventArgs);
            return groupsEventArgs.TemplateGroups;
        }

        [WebMethod]
        public List<String> GetGroupFiles(string groupName)
        {
            var groupFilesEventArgs = new GroupFilesEventArgs { GroupName = groupName };
            GetGroupFilesCalled(this, groupFilesEventArgs);
            return groupFilesEventArgs.GroupFiles;
        }

        [WebMethod]
        public string GetFile(string groupName, string fileName)
        {
            var fileEventArgs = new FileEventArgs { GroupName = groupName, FileName = fileName };
            GetFileCalled(this, fileEventArgs);
            return fileEventArgs.Contents;
        }

        [WebMethod]
        public FileSaveEventArgs SaveFile(string groupName, string fileName, string fileContents)
        {
            var fileSaveEventArgs = new FileSaveEventArgs { GroupName = groupName, FileName = fileName, FileContents = fileContents };
            FileSaveCalled(this, fileSaveEventArgs);
            return fileSaveEventArgs;
        }
    }
}