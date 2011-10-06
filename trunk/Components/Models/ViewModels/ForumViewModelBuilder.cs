namespace DotNetNuke.Modules.Forums.Components.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Controllers;
    using Entities;

    public class ForumViewModelBuilder
    {
        private readonly IForumsController controller;

        public ForumViewModelBuilder(IForumsController controller)
        {
            this.controller = controller;
        }

        public List<ForumViewModel> Build(int moduleId)
        {
            var forumInfos = controller.GetModuleForums(moduleId);
            return BuildForums(forumInfos);
        }

        public List<ForumViewModel> Build(List<ForumInfo> forumInfos)
        {
            return BuildForums(forumInfos);
        }

        private List<ForumViewModel> BuildForums(IEnumerable<ForumInfo> forumInfos)
        {
            return (from forumInfo in forumInfos
                    let lastPost = controller.GetPost(forumInfo.LastPostId)
                    select new ForumViewModel
                               {
                                   Description = forumInfo.Description,
                                   //LastPostAuthorImageUrl = string.Empty,
                                   LastPostAuthorName = lastPost.DisplayName,
                                   //LastPostAuthorUrl = string.Empty,
                                   LastPostDate = lastPost.CreatedDate,
                                   //LastPostUrl = ,
                                   LastPostTitle = lastPost.Subject,
                                   Name = forumInfo.Name,
                                   NumberOfReplies = forumInfo.ReplyCount,
                                   NumberOfTopics = forumInfo.TopicCount,
                                   //Url = ,
                               }).ToList();
        }
    }
}