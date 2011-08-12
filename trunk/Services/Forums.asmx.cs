namespace DotNetNuke.Modules.Forums.Services
{
    using System.Collections.Generic;
    using System.Web.Services;
    using Components.Entities;

    [WebService(Namespace = "http://dnnforums.dotnetnuke.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Forums : WebService {

        [WebMethod]
        public List<ForumInfo> ForumList(int moduleId)
        {
            return new Components.Controllers.ForumsController().GetModuleForums(moduleId);
        }

        [WebMethod]
        public ForumInfo ForumGet(int forumId)
        {
            return new Components.Controllers.ForumsController().GetForum(forumId); 
        }
    }
}
