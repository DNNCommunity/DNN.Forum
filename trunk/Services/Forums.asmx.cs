using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using DotNetNuke.Modules.Forums.Components.Entities;

namespace DotNetNuke.Modules.Forums.Services {
    /// <summary>
    /// Summary description for Forums
    /// </summary>
    [WebService(Namespace = "http://dnnforums.dotnetnuke.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Forums : System.Web.Services.WebService {

        [WebMethod]
        public List<ForumInfo> ForumList(int ModuleId) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            List<ForumInfo> fl = fc.GetModuleForums(ModuleId);
            return fl;
            
        }
        [WebMethod]
        public ForumInfo ForumGet(int ForumId) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            ForumInfo fi = fc.GetForum(ForumId); 
            return fi;

        }
    }
}
