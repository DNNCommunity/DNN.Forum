namespace DotNetNuke.Modules.Forums.Components.Views
{
    using System;
    using Web.Mvp;
    using Services;

    public interface ITemplatesServiceView : IWebServiceView
    {
        event EventHandler<HelloWorldEventArgs> HelloWorldCalled;
        event EventHandler<GroupsEventArgs> GetGroupsCalled;
        event EventHandler<GroupFilesEventArgs> GetGroupFilesCalled;
        event EventHandler<FileEventArgs> GetFileCalled;
        event EventHandler<FileSaveEventArgs> FileSaveCalled;
    }
}