using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using DotNetNuke.Framework;

namespace DotNetNuke.Modules.Forums {
    /// <summary>
    /// Summary description for scripts
    /// </summary>
    public class scripts : HandlerBase  {

        public override void ProcessRequest(HttpContext context) {
            base.ProcessRequest(context);
            bool isAdmin = false;
            context.Response.ContentType = "text/javascript";
            string basePath = VirtualPathUtility.ToAbsolute("~/DesktopModules/dnncorp/forums/");
            string scriptPath = VirtualPathUtility.ToAbsolute("~/DesktopModules/dnncorp/forums/scripts/");
            StringBuilder sb = new StringBuilder();

            sb.Append("var rshScriptPath = '" + scriptPath + "';");
            sb.AppendLine(Common.Utilities.GetFile(context.Server.MapPath(scriptPath + "json2009.min.js")));
            string langKey = "en-US";
            if (context.Request.QueryString["language"] != null) {
                langKey = context.Request.QueryString["language"];
            }
            sb.AppendLine(Common.Utilities.GetFile(context.Server.MapPath(scriptPath + "dnnforums.js")));
            if (context.Request.QueryString["isadmin"] != null) {
                isAdmin = true;
                sb.AppendLine(Common.Utilities.GetFile(context.Server.MapPath(scriptPath + "rsh.min.js")));
                sb.AppendLine(Common.Utilities.GetFile(context.Server.MapPath(scriptPath + "dnnforums.admin.js")));
            }

            sb.Replace("[SERVICESPATH]", basePath);
            sb.Replace("[PORTALID]", PortalId.ToString());
            sb.Replace("[MODULEID]", ModuleId.ToString());
            sb.Replace("[LANGUAGE]", langKey);
            context.Response.Write(Common.Utilities.LocalizeControl(sb.ToString(), isAdmin));
        }

        public override bool IsReusable {
            get {
                return false;
            }
        }
    }
}