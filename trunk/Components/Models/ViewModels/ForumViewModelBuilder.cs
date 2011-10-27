namespace DotNetNuke.Modules.Forums.Components.Models.ViewModels
{
    using System.Collections.Generic;
    using Common;
    using Controllers;
    using Entities;

    public class ForumViewModelBuilder
    {
        private readonly IForumsController controller;
        private readonly IDnnUserController userController;

        public ForumViewModelBuilder(IForumsController controller, IDnnUserController userController)
        {
            this.controller = controller;
            this.userController = userController;
        }

        public List<ForumViewModel> Build(IModuleInstanceContext moduleContext)
        {
            var forumInfos = controller.GetModuleForums(moduleContext.ModuleId);
            return BuildForums(forumInfos, moduleContext);
        }

        public List<ForumViewModel> Build(List<ForumInfo> forumInfos, IModuleInstanceContext moduleContext)
        {
            return BuildForums(forumInfos, moduleContext);
        }

        private List<ForumViewModel> BuildForums(IEnumerable<ForumInfo> forumInfos, IModuleInstanceContext moduleContext)
        {
            var list = new List<ForumViewModel>();
            if (forumInfos == null)
            {
                return list;
            }
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
                    forum.LastPostAuthorImageUrl = this.userController.UserProfileImageUrl(moduleContext.PortalId, lastPost.UserId);
                    forum.LastPostAuthorName = lastPost.DisplayName;
                    forum.LastPostAuthorUrl = this.userController.UserProfileURL(lastPost.UserId);
                    forum.LastPostDate = lastPost.CreatedDate;
                    forum.LastPostUrl = null; //todo: get url of the last post (not topic, but post within the topic?)
                    forum.LastPostTitle = lastPost.Subject;
                }

                list.Add(forum);
            }
            return list;
        }
    }
}