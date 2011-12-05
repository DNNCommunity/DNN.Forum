namespace DotNetNuke.Modules.Forums.Components.Presenters
{
    using Common;
    using Controllers;
    using Services;
    using Web.Mvp;

    public class ForumsServicePresenter : WebServicePresenter<IForumsServiceView>
    {
        private readonly IForumsController forumController;

        public ForumsServicePresenter(IForumsServiceView view) : this(view, Utilities.GetForumsController(Utilities.GetRepository()))
        {
        }

        public ForumsServicePresenter(IForumsServiceView view, IForumsController controller) : base(view)
        {
            forumController = controller;
            View.ListForumsCalled += GetForumsCalled;
            View.ForumGetCalled += GetForumCalled;
        }

        private void GetForumCalled(object sender, GetForumCalledEventArgs e)
        {
            e.Result = forumController.GetForum(e.ForumId);
        }

        private void GetForumsCalled(object sender, GetForumsCalledEventArgs e)
        {
            e.Result = forumController.GetModuleForums(e.ModuleId);
        }
    }
}