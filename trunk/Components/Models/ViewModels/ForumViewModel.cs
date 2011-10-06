namespace DotNetNuke.Modules.Forums.Components.Models.ViewModels
{
    using System;
    using Templating;

    public class ForumViewModel : DnnTemplatingViewModel
    {
        public string Description { get; set; }
        public string LastPostAuthorImageUrl { get; set; }
        public string LastPostAuthorName { get; set; }
        public string LastPostAuthorUrl { get; set; }
        public DateTime LastPostDate { get; set; }
        public string LastPostUrl { get; set; }
        public string LastPostTitle { get; set; }
        public string Name { get; set; }
        public int NumberOfReplies { get; set; }
        public int NumberOfTopics { get; set; }
        public string Url { get; set; }
    }
}