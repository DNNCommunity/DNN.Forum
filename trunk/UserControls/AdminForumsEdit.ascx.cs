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

using DotNetNuke.Modules.Forums.Components.Common;
using DotNetNuke.Modules.Forums.Components.Models.UserControls;
using DotNetNuke.Modules.Forums.Components.Presenters.UserControls;
using DotNetNuke.Modules.Forums.Components.Views;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using DotNetNuke.Modules.Forums.Components.Entities;

namespace DotNetNuke.Modules.Forums.UserControls {

    /// <summary>
    /// This is the initial view seen within the Forums module, loaded by Dispatch.ascx. 
    /// </summary>
    [PresenterBinding(typeof(AdminForumsEditPresenter))]
    public partial class AdminForumsEdit : ModuleView<AdminForumsEditModel>, IAdminForumsEditView {
        
        #region Constructor

        /// <summary>
        /// The constructor is used here to set base properties. 
        /// </summary>
        /// <remarks>We disable AutoDataBind in the ctor so we can utlize Telerik and other 'Ajax' controls, otherwise we will get a PreRender script registration error.</remarks>
        public AdminForumsEdit() {
            this.LocalResourceFile = "~/DesktopModules/DNNCorp/Forums/App_LocalResources/ControlPanel.ascx.resx";
            AutoDataBind = false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void Refresh() {
            var fc = new Components.Controllers.ForumsController();
            int ForumId = 0;
            if (int.TryParse(Request.Form["param"].ToString(), out ForumId)) {
                Model.ForumId = int.Parse(Request.Form["param"]);
            }
            if (Model.ForumId > 0) {
                Model.forumInfo = fc.GetForum(ForumId);
            }
            if (Request.Form["action"] == "save") {
              
                Model.forumInfo = (ForumInfo)Utilities.FillObjectFromRequest(Model.forumInfo, Request.Form);
                Model.forumInfo.PortalId = ModuleContext.PortalId;
                Model.forumInfo.ModuleId = ModuleContext.ModuleId;
                Model.forumInfo.TopicCount = 0;
                Model.forumInfo.ReplyCount = 0;
                Model.forumInfo.PermissionId = 1;
                Model.forumInfo.SettingId = 1;
                Model.forumInfo.CreatedByUserId = -1;
                if (Model.ForumId > 0) {
                    Model.forumInfo.LastModifiedByUserId = Model.CurrentUserID;
                    fc.UpdateForum(Model.forumInfo);
                }
                else {
                    Model.ForumId = fc.AddForum(Model.forumInfo);
                    Model.forumInfo = fc.GetForum(Model.ForumId);
                }
                
                
            }
            //if (Request.Form["action"] == "callback") {
            //    if (Request.Form["param"] != null) {
                   
                    
            //        if (ForumId > 0) {
            //            Model.forumInfo = fc.GetForum(ForumId);
            //        }
            //    }
            //}

            
        }
     
        #endregion

    }
}