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

namespace DotNetNuke.Modules.Forums {

    using System.Web;
    using DotNetNuke.Services.Localization;
    using Entities.Portals;

    public class HandlerBase : IHttpHandler {
        
        private int portalId = -1;
        private PortalSettings portalSettings;
        private Entities.Users.UserInfo userInfo;
        
        public int PortalId {
            get
            {
                if ((HttpContext.Current.Request.QueryString["PortalId"] != null) &&
                        int.Parse(HttpContext.Current.Request.QueryString["PortalId"]) > -1) {
                    return int.Parse(HttpContext.Current.Request.QueryString["PortalId"]);
                }
                return portalId;
            }
        }
        public int ModuleId {
            get
            {
                if ((HttpContext.Current.Request.QueryString["ModuleId"] != null) && 
                        int.Parse(HttpContext.Current.Request.QueryString["ModuleId"]) > -1) {
                    return int.Parse(HttpContext.Current.Request.QueryString["ModuleId"]);
                }
                return -1;
            }
        }
        public int TabId {
            get
            {
                if ((HttpContext.Current.Request.QueryString["TabId"] != null) && 
                        int.Parse(HttpContext.Current.Request.QueryString["TabId"]) > -1) {
                    return int.Parse(HttpContext.Current.Request.QueryString["TabId"]);
                }
                return -1;
            }
        }
        public PortalSettings PortalSettings
        {
            get
            {
                return portalSettings; 
            }
        }
        public Entities.Users.UserInfo CurrentUserInfo
        {
            get
            {
                return userInfo;
            }
        }
        public int UserId
        {
            get
            {
                return userInfo.UserID;
            }
        }
        
        public virtual void ProcessRequest(HttpContext context)
        {
            if ((HttpContext.Current.Items["PortalSettings"] != null))
            {
                portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
                portalId = portalSettings.PortalId;
            }
            else
            {
                PortalAliasInfo objPortalAliasInfo = PortalAliasController.GetPortalAliasInfo(HttpContext.Current.Request.Url.Host);
                portalId = objPortalAliasInfo.PortalID;
                portalSettings = PortalController.GetCurrentPortalSettings();
            }
            Localization.SetThreadCultures(Localization.GetPageLocale(portalSettings), portalSettings);
            userInfo = HttpContext.Current.Request.IsAuthenticated ? Entities.Users.UserController.GetUserByName(PortalId, HttpContext.Current.User.Identity.Name) : new Entities.Users.UserInfo {UserID = -1};
        }

        public virtual bool IsReusable {
            get { return false; }
        }
    }
}