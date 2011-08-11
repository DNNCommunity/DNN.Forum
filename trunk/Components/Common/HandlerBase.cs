using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Framework;
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
        public virtual void ProcessRequest(System.Web.HttpContext context) {
            if ((HttpContext.Current.Items["PortalSettings"] != null)) {
                portalSettings = (DotNetNuke.Entities.Portals.PortalSettings)HttpContext.Current.Items["PortalSettings"];
                portalId = portalSettings.PortalId;
            }
            else {

                DotNetNuke.Entities.Portals.PortalAliasInfo objPortalAliasInfo = null;
                string sUrl = HttpContext.Current.Request.RawUrl.Replace("http://", string.Empty).Replace("https://", string.Empty);
                objPortalAliasInfo = DotNetNuke.Entities.Portals.PortalAliasController.GetPortalAliasInfo(HttpContext.Current.Request.Url.Host);
                portalId = objPortalAliasInfo.PortalID;
                portalSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings();


            }
            Localization.SetThreadCultures(Localization.GetPageLocale(portalSettings), portalSettings);
            if (HttpContext.Current.Request.IsAuthenticated) {
                userInfo = DotNetNuke.Entities.Users.UserController.GetUserByName(PortalId, HttpContext.Current.User.Identity.Name);
            } else {
                userInfo = new DotNetNuke.Entities.Users.UserInfo();
                userInfo.UserID = -1;
            }


        }
        public virtual bool IsReusable {
            get { return false; }
        }
    }
}