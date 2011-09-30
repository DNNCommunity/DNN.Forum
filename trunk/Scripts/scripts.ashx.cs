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

using System.Web;
using System.Text;
using DotNetNuke.Modules.Forums.Components.Common;

namespace DotNetNuke.Modules.Forums {

    /// <summary>
    /// Summary description for scripts
    /// </summary>
    public class scripts : HandlerBase  {

        public override void ProcessRequest(HttpContext context) {
            base.ProcessRequest(context);
           // var isAdmin = false;
            context.Response.ContentType = "text/javascript";
            var basePath = VirtualPathUtility.ToAbsolute("~/DesktopModules/dnncorp/forums/");
            var scriptPath = VirtualPathUtility.ToAbsolute("~/DesktopModules/dnncorp/forums/scripts/");
            var sb = new StringBuilder();

            sb.Append("var rshScriptPath = '" + scriptPath + "';");
            sb.AppendLine(Utilities.GetFile(context.Server.MapPath(scriptPath + "json2009.min.js")));
            string langKey = "en-US";
            if (context.Request.QueryString["language"] != null) {
                langKey = context.Request.QueryString["language"];
            }
            if (context.Request.QueryString["isadmin"] != null) {
               // isAdmin = true;
                sb.AppendLine(Utilities.GetFile(context.Server.MapPath(scriptPath + "rsh.min.js")));
                sb.AppendLine(Utilities.GetFile(context.Server.MapPath(scriptPath + "dnnforums.admin.js")));
            }
           //sb.AppendLine(Utilities.GetFile(context.Server.MapPath(scriptPath + "dnnforums.js")));
           

            sb.Replace("[SERVICESPATH]", basePath);
            sb.Replace("[PORTALID]", PortalId.ToString());
            sb.Replace("[MODULEID]", ModuleId.ToString());
            sb.Replace("[LANGUAGE]", langKey);
           // context.Response.Write(Utilities.LocalizeControl(sb.ToString(), isAdmin));
            context.Response.Write(sb.ToString());
        }

        public override bool IsReusable {
            get {
                return false;
            }
        }

    }
}