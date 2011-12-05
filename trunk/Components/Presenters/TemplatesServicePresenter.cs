namespace DotNetNuke.Modules.Forums.Components.Presenters
{
    using System;
    using System.Linq;
    using DotNetNuke.Services.Localization;
    using Templating;
    using Views;
    using Web.Mvp;

    public class TemplatesServicePresenter : WebServicePresenter<ITemplatesServiceView>
    {
        private readonly ITemplateFileManager fileManager;

        public TemplatesServicePresenter(ITemplatesServiceView view) : this(view, new TemplateFileManager())
        {
            
        }

        public TemplatesServicePresenter(ITemplatesServiceView view, ITemplateFileManager fileManager) : base(view)
        {
            this.fileManager = fileManager;
            View.HelloWorldCalled += HelloWorldCalled;
            View.GetGroupsCalled += GetGroupsCalled;
            View.GetGroupFilesCalled += GetGroupFilesCalled;
            View.GetFileCalled += GetFileCalled;
            View.FileSaveCalled += SaveFileCalled;
        }

        private void SaveFileCalled(object sender, FileSaveEventArgs e)
        {
            try
            {
                this.fileManager.SaveTemplate(e.GroupName, e.FileName, e.FileContents);
                e.Success = true;
                e.Message = "Success";
            }
            catch(Exception exception)
            {
                e.Success = false;
                e.Message = String.Format("Error: {0}", exception.Message);
            }
        }

        private void GetFileCalled(object sender, FileEventArgs e)
        {
            e.Contents = this.fileManager.GetTemplate(e.GroupName, e.FileName);
        }

        private void GetGroupsCalled(object sender, GroupsEventArgs e)
        {
            e.TemplateGroups = this.fileManager.GetTemplateGroups().Select(directory => directory.Name).ToList();
        }

        private void GetGroupFilesCalled(object sender, GroupFilesEventArgs e)
        {
            e.GroupFiles = this.fileManager.GetGroupTemplates(e.GroupName).Select(file => file.Name).ToList();
        }

        private void HelloWorldCalled(object sender, HelloWorldEventArgs e)
        {
            e.Message = "Hello World";
        }
    }
}