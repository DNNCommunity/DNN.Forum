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
    /// Summary description for Filters
    /// </summary>
    [WebService(Namespace = "http://dnnforums.filters.dotnetnuke.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class Filters : System.Web.Services.WebService {

        [WebMethod]
        public List<FilterInfo> FilterList(int PortalId, int ModuleId) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            List<FilterInfo> fl = fc.GetAllFilters(PortalId, ModuleId, -1);
            return fl;
        }
        [WebMethod]
        public FilterInfo FilterGet(int PortalId, int ModuleId, int FilterId) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            
            FilterInfo fi = fc.GetFilter(FilterId);
            if (fi == null) {
                fi = new FilterInfo();
                fi.PortalId = PortalId;
                fi.ModuleId = ModuleId;
                fi.FilterId = -1;
            }
            return fi;
        }
        [WebMethod]
        public FilterInfo FilterSave(int PortalId, int ModuleId, int FilterId, int ForumId, string Find, string Replace, string FilterType, bool ApplyOnSave, bool ApplyOnRender) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            FilterInfo fi = new FilterInfo();
            if (FilterId > 0) {
                fi = fc.GetFilter(FilterId);
            }
            fi.PortalId = PortalId;
            fi.ModuleId = ModuleId;
            fi.FilterId = FilterId;
            fi.ForumId = ForumId;
            fi.Find = Find;
            fi.Replace = Replace;
            fi.FilterType = FilterType;
            fi.ApplyOnRender = ApplyOnRender;
            fi.ApplyOnSave = ApplyOnSave;
            fi = fc.SaveFilter(fi);
            return fi;
            
        }
        [WebMethod]
        public bool FilterDelete(int PortalId, int ModuleId, int FilterId) {
            Modules.Forums.Components.Controllers.ForumsController fc = new Modules.Forums.Components.Controllers.ForumsController();
            try {
                fc.DeleteFilter(FilterId, PortalId);
                return true;
            } catch {
                return false;
            }
        }
    }
}
