namespace DotNetNuke.Modules.Forums.Components.Views
{
    using System;

    public class FileSaveEventArgs : EventArgs
    {
        public string GroupName { get; set; }
        public string FileName { get; set; }
        public string FileContents { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}