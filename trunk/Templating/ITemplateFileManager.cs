namespace DotNetNuke.Modules.Forums.Templating
{
    using System.Collections.Generic;
    using System.IO;

    public interface ITemplateFileManager
    {
        string BaseTemplatePath { get; }
        string TemplateExtension { get; }
        string GetTemplate(string groupName, string templateName);
        List<DirectoryInfo> GetTemplateGroups();
        void SaveTemplate(string groupName, string templateFileName, string templateContents);
        string GetFilePath(string groupName, string templateFileName);
        string GetDirectoryPath(string groupName);
        List<FileInfo> GetGroupTemplates(string groupName);
        
    }
}