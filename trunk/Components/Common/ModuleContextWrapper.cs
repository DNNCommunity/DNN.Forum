namespace DotNetNuke.Modules.Forums.Components.Common
{
    using UI.Modules;

    public interface IModuleInstanceContext
    {
        int ModuleId { get; }
        int PortalId { get; }
        int TabId { get; }
        int TabModuleId { get; }
        ModuleInstanceContext ModuleContext { get; set; }
        string EditUrl();
        string EditUrl(string controlKey);
        string EditUrl(string keyName, string keyValue);
        string EditUrl(string keyName, string keyValue, string controlKey);
        string EditUrl(string keyName, string keyValue, string controlKey, params string[] additionalParameters);
        string NavigateUrl(int tabID, string controlKey, bool pageRedirect, params string[] additionalParameters);
    }

    public class ModuleContextWrapper : IModuleInstanceContext
    {
        public ModuleInstanceContext ModuleContext { get; set; }
        
        public int ModuleId
        {
            get { return this.ModuleContext.ModuleId; }
        }

        public int PortalId
        {
            get { return this.ModuleContext.PortalId; }
        }

        public int TabId
        {
            get { return this.ModuleContext.TabId; }
        }

        public int TabModuleId
        {
            get { return this.ModuleContext.TabModuleId; }
        }

        public string EditUrl()
        {
            return this.ModuleContext.EditUrl();
        }

        public string EditUrl(string controlKey)
        {
            return this.ModuleContext.EditUrl(controlKey);
        }

        public string EditUrl(string keyName, string keyValue)
        {
            return this.ModuleContext.EditUrl(keyName, keyValue);
        }

        public string EditUrl(string keyName, string keyValue, string controlKey)
        {
            return this.ModuleContext.EditUrl(keyName, keyValue, controlKey);
        }

        public string EditUrl(string keyName, string keyValue, string controlKey, params string[] additionalParameters)
        {
            return this.ModuleContext.EditUrl(keyName, keyValue, controlKey, additionalParameters);
        }

        public string NavigateUrl(int tabId, string controlKey, bool pageRedirect, params string[] additionalParameters)
        {
            return this.ModuleContext.NavigateUrl(tabId, controlKey, pageRedirect, additionalParameters);
        }
    }
}