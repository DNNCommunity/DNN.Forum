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

using DotNetNuke.Modules.Forums.Components.Controllers;
using DotNetNuke.Modules.Forums.Views.Models;
using DotNetNuke.Modules.Forums.Views;
using DotNetNuke.Modules.Forums.Providers.Data.SqlDataProvider;
using DotNetNuke.Web.Mvp;
using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework;
using System.Web.UI;

namespace DotNetNuke.Modules.Forums.Presenters {

    /// <summary>
    /// 
    /// </summary>
    public class ControlPanelPresenter : ModulePresenter<Views.IControlPanelView, Views.Models.ControlPanelModel> {

        #region Private Members

        protected IForumsController Controller { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private string ControlView {
            get {
                var controlView = "main";
                if (!String.IsNullOrEmpty(Request.Params["view"])) {
                    controlView = Request.Params["view"];
                }
                return controlView.ToLowerInvariant();
            }
        }

        private const string CtlHome = "main";

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        public ControlPanelPresenter(IControlPanelView view)
            : this(view, new ForumsController(new SqlDataProvider())) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ControlPanelPresenter(IControlPanelView view, IForumsController controller)
            : base(view) {
            if (view == null) {
                throw new ArgumentException(@"View is nothing.", "view");
            }

            if (controller == null) {
                throw new ArgumentException(@"Controller is nothing.", "controller");
            }

            Controller = controller;
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        protected override void OnInit() {
            base.OnInit();
            View.Model.IsEditable = ModuleContext.IsEditable;
            View.Model.ControlToLoad = "/UserControls/Admin" + ControlView + ".ascx";
            View.Refresh();       
            
            
           
            
            
           
        }

        #endregion
        #region Public Methods
        
        #endregion

    }
}