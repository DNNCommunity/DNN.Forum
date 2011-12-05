namespace DotNetNuke.Modules.Forums.Components.Views
{
    using System;

    public class FileEventArgs : EventArgs
    {
        public string Contents { get; set; }

        public string GroupName { get; set; }

        public string FileName { get; set; }
    }
}