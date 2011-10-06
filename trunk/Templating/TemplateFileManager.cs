namespace DotNetNuke.Modules.Forums.Templating
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;

    public class TemplateFileManager : ITemplateFileManager
    {
        public string BaseTemplatePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("/DesktopModules/DNNCorp/Forums/Templates/");
            }
        }

        public string TemplateExtension
        {
            get { return ".template"; }
        }

        public string GetTemplate(string groupName, string templateName)
        {
            string filePath = GetFilePath(groupName, templateName);
            return File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
        }

        public List<DirectoryInfo> GetTemplateGroups()
        {
            var templateDirectory = new DirectoryInfo(this.BaseTemplatePath);
            return templateDirectory.GetDirectories().ToList();
        }

        public void SaveTemplate(string groupName, string templateFileName, string templateContents)
        {
            File.WriteAllText(GetFilePath(groupName, templateFileName), templateContents);
        }

        public string GetFilePath(string groupName, string templateFileName)
        {
            return Path.Combine(this.BaseTemplatePath + groupName, templateFileName);
        }

        public string GetDirectoryPath(string groupName)
        {
            return (this.BaseTemplatePath.EndsWith("/") ? this.BaseTemplatePath : this.BaseTemplatePath + "/") + groupName;
        }

        public List<FileInfo> GetGroupTemplates(string groupName)
        {
            var groupDirectory = new DirectoryInfo(GetDirectoryPath(groupName));
            return groupDirectory.GetFiles().ToList();
        }
    }
}