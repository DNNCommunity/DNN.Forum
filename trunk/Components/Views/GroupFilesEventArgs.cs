namespace DotNetNuke.Modules.Forums.Components.Views
{
    using System;
    using System.Collections.Generic;

    public class GroupFilesEventArgs : EventArgs
    {
        public string GroupName { get; set; }

        public List<String> GroupFiles;
    }
}