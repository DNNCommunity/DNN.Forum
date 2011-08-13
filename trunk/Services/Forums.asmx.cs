namespace DotNetNuke.Modules.Forums.Services
{
    using System;
    using System.Collections.Generic;
    using System.Web.Services;
    using Components.Entities;
    using Web.Mvp;
    using WebFormsMvp;

    [WebService(Namespace = "http://dnnforums.dotnetnuke.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    [PresenterBinding(typeof(ForumsServicePresenter), ViewType = typeof(IForumsServiceView))]
    public class Forums : WebServiceView, IForumsServiceView
    {
        public event EventHandler<GetForumsCalledEventArgs> ListForumsCalled;
        public event EventHandler<GetForumCalledEventArgs> ForumGetCalled;
        
        protected void OnGetForumsCalled(GetForumsCalledEventArgs args)
        {
            if (ListForumsCalled != null)
            {
                ListForumsCalled(this, args);
            }
        }

        protected void OnGetForumCalled(GetForumCalledEventArgs args)
        {
            if (ForumGetCalled != null)
            {
                ForumGetCalled(this, args);
            }
        }

        [WebMethod]
        public List<ForumInfo> GetForums(int moduleId)
        {
            var args = new GetForumsCalledEventArgs(moduleId);
            OnGetForumsCalled(args);
            return args.Result;
        }

        [WebMethod]
        public ForumInfo GetForum(int forumId)
        {
            var args = new GetForumCalledEventArgs(forumId);
            OnGetForumCalled(args);
            return args.Result;
        }
    }
}
