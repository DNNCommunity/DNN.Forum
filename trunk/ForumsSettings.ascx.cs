namespace DotNetNuke.Modules.Forums
{
    using Entities.Modules;
    using Templating;

    public partial class ForumsSettings : ModuleSettingsBase
    {
        public override void LoadSettings()
        {
            base.LoadSettings();

            this.BindTemplateGroupList();
            this.BindTemplateList();
            this.BindTemplateContents();
        }

        public override void UpdateSettings()
        {
            base.UpdateSettings();
            new TemplateFileManager().SaveTemplate(this.GroupName, this.TemplateFileName, this.TemplateContents);
        }
        
        protected void TemplateGroupListSelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.BindTemplateList();
            this.BindTemplateContents();
        }

        protected void TemplateListSelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.BindTemplateContents();
        }

        private void BindTemplateContents()
        {
            this.TemplateTextBox.Text = new TemplateFileManager().GetTemplate(this.GroupName, this.TemplateFileName);
        }

        private void BindTemplateList()
        {
            this.TemplateList.DataSource = new TemplateFileManager().GetGroupTemplates(this.GroupName);
            this.TemplateList.DataBind();
        }

        private void BindTemplateGroupList()
        {
            this.TemplateGroupList.DataSource = new TemplateFileManager().GetTemplateGroups();
            this.TemplateGroupList.DataBind();
        }

        private string GroupName
        {
            get { return this.TemplateGroupList.SelectedItem.Text; }
        }

        private string TemplateFileName
        {
            get { return this.TemplateList.SelectedItem.Text; }
        }

        private string TemplateContents
        {
            get { return this.TemplateTextBox.Text; }
        }
    }
}