namespace DotNetNuke.Modules.Forums.Components.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Controllers;
    using Entities;
    using UI.Modules;

    public class ForumViewModelBuilder
    {
        private readonly IForumsController controller;

        public ForumViewModelBuilder(IForumsController controller)
        {
            this.controller = controller;
        }

        public List<ForumViewModel> Build(ModuleInstanceContext moduleContext)
        {
            var forumInfos = controller.GetModuleForums(moduleContext.ModuleId);
            return BuildForums(forumInfos, moduleContext);
        }

        public List<ForumViewModel> Build(List<ForumInfo> forumInfos, ModuleInstanceContext moduleContext)
        {
            return BuildForums(forumInfos, moduleContext);
        }

        private List<ForumViewModel> BuildForums(IEnumerable<ForumInfo> forumInfos, ModuleInstanceContext moduleContext)
        {
            var list = new List<ForumViewModel>();
            foreach (ForumInfo forumInfo in forumInfos)
            {
                var forum = new ForumViewModel
                                         {
                                             Description = forumInfo.Description,
                                             Name = forumInfo.Name,
                                             NumberOfReplies = forumInfo.ReplyCount,
                                             NumberOfTopics = forumInfo.TopicCount,
                                             Url = Links.ForumTopicList(moduleContext, moduleContext.TabId, forumInfo.ForumId),
                                         };
                PostInfo lastPost = controller.GetPost(forumInfo.LastPostId);
                if (lastPost != null)
                {
                    forum.LastPostAuthorImageUrl = string.Empty; //todo: get author image url
                    forum.LastPostAuthorName = lastPost.DisplayName;
                    forum.LastPostAuthorUrl = DotNetNuke.Common.Globals.UserProfileURL(lastPost.UserId);
                    forum.LastPostDate = lastPost.CreatedDate;
                    forum.LastPostUrl = string.Empty; //todo: get url of the last post (not topic, but post within the topic?)
                    forum.LastPostTitle = lastPost.Subject;
                }

                list.Add(forum);
            }
            return list;
        }
    }
}