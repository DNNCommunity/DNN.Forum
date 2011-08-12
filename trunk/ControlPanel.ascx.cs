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

    using System.Web.UI;
    using Components.Common;
    using Components.Presenters;
    using Components.Views;
    using Framework;
    using Web.Mvp;
    using WebFormsMvp;

    /// <summary>
    /// ControlPanel is the initial edit control in the Forums module. It reads the URL and determines which control should be displayed to the end user. 
    /// </summary>
    /// <remarks>The purpose of this is to avoid usage of 'ctl' in the URL and thus loading of the DotNetNuke edit skin. </remarks>
    [PresenterBinding(typeof(ControlPanelPresenter))]
    public partial class ControlPanel : ModuleView<Views.Models.ControlPanelModel>, IControlPanelView {

        /// <summary>
        /// The constructor is used here to set base properties. 
        /// </summary>
        /// <remarks>We have to disable autobinding in the ctor here so the loaded controls can also disable it, if necessary.</remarks>
        public ControlPanel() {
           AutoDataBind = false;
        }
       
         //<summary>
         //Contains control initialization logic
         //</summary>
         //<param name="e"></param>
         //<remarks>OnInit can be used here because ctl= is used in URL.</remarks>
        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            var ctlDirectory = TemplateSourceDirectory;
            var langKey = "en-US";
            if (Request.QueryString["language"] != null)
            {
                langKey = Request.QueryString["language"];
            }
            jQuery.RegisterDnnJQueryPlugins(Page);
            PageBase.RegisterStyleSheet(Page, ctlDirectory + "/ControlPanel.css");
            Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), "dnnforums.scripts", Page.ResolveUrl(ctlDirectory + "/scripts/scripts.ashx") + "?PortalId=" + ModuleContext.PortalId + "&ModuleId=" + ModuleContext.ModuleId + "&isadmin=true&language=" + langKey);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "dnnforums.history", "window.dhtmlHistory.create({ toJSON: function (o) { return JSON.stringify(o); }, fromJSON: function (s) { return JSON.parse(s); } });", true);
        }

        protected override void Render(HtmlTextWriter writer) {
            var stringWriter = new System.IO.StringWriter();
            var htmlWriter = new HtmlTextWriter(stringWriter);
            base.Render(htmlWriter);
            string html = stringWriter.ToString();
            html = Utilities.LocalizeControl(html, true);
            writer.Write(html);
        }
    }
}