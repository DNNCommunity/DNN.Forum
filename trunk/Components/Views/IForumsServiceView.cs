namespace DotNetNuke.Modules.Forums.Services
{
    using System;
    using System.Collections.Generic;
    using Components.Entities;
    using Web.Mvp;

    public interface IForumsServiceView : IWebServiceView
    {
        event EventHandler<GetForumsCalledEventArgs> ListForumsCalled;
        event EventHandler<GetForumCalledEventArgs> ForumGetCalled;
    }

    public class GetForumsCalledEventArgs : EventArgs
    {
        public GetForumsCalledEventArgs(int moduleId)
        {
            this.ModuleId = moduleId;
        }

        public int ModuleId { get; private set; }
        public List<ForumInfo> Result { get; set; }
    }

    public class GetForumCalledEventArgs : EventArgs
    {
        public GetForumCalledEventArgs(int forumId)
        {
            this.ForumId = forumId;
        }

        public int ForumId { get; private set; }
        public ForumInfo Result { get; set; }
    }
}