namespace DotNetNuke.Modules.Forums.Components.Models.ViewModels
{
    using System.Collections.Generic;
    using Templating;

    public class HomeViewModel : DnnTemplatingViewModel
    {
        public List<ForumViewModel> Forums;
    }
}