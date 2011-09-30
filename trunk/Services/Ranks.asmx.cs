//
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2011
// by DotNetNuke Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DotNetNuke.Modules.Forums.Components.Entities;


namespace DotNetNuke.Modules.Forums.Services {
    /// <summary>
    /// Summary description for Ranks
    /// </summary>
    [WebService(Namespace = "http://dnnforums.ranks.dotnetnuke.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Ranks : System.Web.Services.WebService {
        [WebMethod]
        public List<RankInfo> RanksList(int portalId, int moduleId) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            List<RankInfo> rl = fc.GetModuleRank(moduleId);
            return rl;
        }
        [WebMethod]
        public RankInfo RankGet(int portalId, int moduleId, int rankId) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            RankInfo ri = fc.GetRank(rankId);
            return ri;
        }
        [WebMethod]
        public RankInfo RankSave(int portalId, int moduleId, int rankId, string rankName, int minPosts, int maxPosts, string display) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            RankInfo obj = new RankInfo();
            if (rankId > 0) {
                obj = fc.GetRank(rankId);
            }
            obj.PortalId = portalId;
            obj.ModuleId = moduleId;
            obj.RankId = rankId;
            obj.RankName = rankName;
            obj.MinPosts = minPosts;
            obj.MaxPosts = maxPosts;
            obj.Display = display;
            
            obj = fc.SaveRank(obj);
            return obj;

        }
        [WebMethod]
        public bool RankDelete(int portalId, int moduleId, int rankId) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            try {
                fc.DeleteRank(rankId, portalId);
                return true;
            } catch {
                return false;
            }
        }
    }
}
