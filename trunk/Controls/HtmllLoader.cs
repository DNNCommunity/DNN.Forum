using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Modules.Forums.Components.Common;

namespace DotNetNuke.Modules.Forums.Controls {
    [ToolboxData("<{0}:HtmlLoader runat=server></{0}:HtmlLoader>")]
    public class HtmlControlLoader : Control {

        public string ControlId { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }

        protected override void Render(HtmlTextWriter output) {
            EnableViewState = false;
            FilePath = HttpContext.Current.Server.MapPath(FilePath);
            string ControlText = Utilities.GetFile(FilePath);
            ControlText = ControlText.Replace("{id}", ControlId);
            ControlText = ControlText.Replace("{height}", Height);
            ControlText = ControlText.Replace("{width}", Width);
            ControlText = ControlText.Replace("{name}", Name);
            output.Write(ControlText);

        }
    }
}
