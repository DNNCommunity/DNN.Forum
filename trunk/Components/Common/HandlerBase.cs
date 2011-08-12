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
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Modules.Forums {
    public class HandlerBase : IHttpHandler {
        #region Private Members
        private int portalId = -1;
        private DotNetNuke.Entities.Portals.PortalSettings portalSettings;
        private DotNetNuke.Entities.Users.UserInfo userInfo;
        #endregion

        #region Public Properties
        public int PortalId {
            get {
                if ((HttpContext.Current.Request.QueryString["PortalId"] != null) &&
                        int.Parse(HttpContext.Current.Request.QueryString["PortalId"]) > -1) {
                    return int.Parse(HttpContext.Current.Request.QueryString["PortalId"]);
                }
                else {
                    return portalId;
                }
            }
        }
        public int ModuleId {
            get {
                if ((HttpContext.Current.Request.QueryString["ModuleId"] != null) && 
                        int.Parse(HttpContext.Current.Request.QueryString["ModuleId"]) > -1) {
                    return int.Parse(HttpContext.Current.Request.QueryString["ModuleId"]);
                }
                else {
                    return -1;
                }
            }
        }
        public int TabId {
            get {
                if ((HttpContext.Current.Request.QueryString["TabId"] != null) && 
                        int.Parse(HttpContext.Current.Request.QueryString["TabId"]) > -1) {
                    return int.Parse(HttpContext.Current.Request.QueryString["TabId"]);
                }
                else {
                    return -1;
                }
            }
        }
        public DotNetNuke.Entities.Portals.PortalSettings PortalSettings {
            get {
                return portalSettings; 
            }
        }
        public DotNetNuke.Entities.Users.UserInfo CurrentUserInfo {
            get {
                return userInfo;
            }
        }
        public int UserId {
            get {
                return userInfo.UserID;
            }
            
        }
        #endregion
        public virtual void ProcessRequest(HttpContext context) {
            if ((HttpContext.Current.Items["PortalSettings"] != null)) {
                portalSettings = (Entities.Portals.PortalSettings)HttpContext.Current.Items["PortalSettings"];
                portalId = portalSettings.PortalId;
            }
            else {

                Entities.Portals.PortalAliasInfo objPortalAliasInfo;
                //var sUrl = HttpContext.Current.Request.RawUrl.Replace("http://", string.Empty).Replace("https://", string.Empty);
                objPortalAliasInfo = Entities.Portals.PortalAliasController.GetPortalAliasInfo(HttpContext.Current.Request.Url.Host);
                portalId = objPortalAliasInfo.PortalID;
                portalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings();


            }
            Localization.SetThreadCultures(Localization.GetPageLocale(portalSettings), portalSettings);
            userInfo = HttpContext.Current.Request.IsAuthenticated ? Entities.Users.UserController.GetUserByName(PortalId, HttpContext.Current.User.Identity.Name) : new Entities.Users.UserInfo {UserID = -1};
        }

        public virtual bool IsReusable {
            get { return false; }
        }

    }
}