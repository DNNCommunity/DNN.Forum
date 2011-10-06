namespace DotNetNuke.Modules.Forums.Templating
{
    using System.Collections.Generic;
    using DotLiquid;

    public class TemplateProcessor
    {
        private readonly ITemplateFileManager templateFileManager;

        public TemplateProcessor(ITemplateFileManager templateFileManager)
        {
            this.templateFileManager = templateFileManager;
        }

        public string ProcessTemplate(string groupName, string templateName, object templateData)
        {
            var templateContents = templateFileManager.GetTemplate(groupName, templateName);
            Template template = Template.Parse(templateContents);
            return templateData == null ? template.Render() : template.Render(Hash.FromAnonymousObject(templateData));
        }

        public string ProcessTemplate(string groupName, string templateName, Dictionary<string, object> templateData)
        {
            var templateContents = templateFileManager.GetTemplate(groupName, templateName);
            Template template = Template.Parse(templateContents);
            return templateData == null ? template.Render() : template.Render(Hash.FromDictionary(templateData));
        }
    }
}