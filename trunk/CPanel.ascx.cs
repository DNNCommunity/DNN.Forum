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
using DotNetNuke.Framework;
using DotNetNuke.Modules.Forums.Components.Models;
using DotNetNuke.Modules.Forums.Components.Presenters;
using DotNetNuke.Modules.Forums.Components.Views;
using DotNetNuke.Web.Mvp;
using WebFormsMvp;
using DotNetNuke.UI.Modules;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;

namespace DotNetNuke.Modules.Forums
{

	/// <summary>
	/// Dispatch is the initial view control in the Forums module. It reads the URL and determines which control should be displayed to the end user. 
	/// </summary>
	/// <remarks>The purpose of this is to avoid usage of 'ctl' in the URL and thus loading of the DotNetNuke edit skin. </remarks>
	[PresenterBinding(typeof(CPanelPresenter))]
	public partial class CPanel : ModuleView<CPanelModel>, ICPanelView
	{

		public event EventHandler NavigationClick;

		#region Constructor

		/// <summary>
		/// The constructor is used here to set base properties. 
		/// </summary>
		/// <remarks>We have to disable autobinding in the ctor here so the loaded controls can also disable it, if necessary.</remarks>
		public CPanel()
		{
			AutoDataBind = false;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		/// <remarks>OnInit can be used here because ctl= is used in URL.</remarks>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			var ctlDirectory = TemplateSourceDirectory;
			jQuery.RequestDnnPluginsRegistration();
			PageBase.RegisterStyleSheet(Page, ctlDirectory + "/ControlPanel.css");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnNavigationClick(object sender, EventArgs e)
		{
			//ViewState["CtlToLoad"] = String.Empty;
			NavigationClick(sender, e);
			LoadControl();
		}

		/// <summary>
		/// Every time a page load occurs (initial load, postback, etc.), this method will load the proper control based on parameters in the URL.
		/// </summary>
		public void Refresh()
		{
			if (Page.IsPostBack) return;
			LoadControl();
		}

		#endregion

		public void LoadControl()
		{
			var ctlDirectory = TemplateSourceDirectory;

			var objControl = LoadControl(ctlDirectory + Model.ControlToLoad) as ModuleUserControlBase;
			if (objControl == null) return;
			phUserControl.Controls.Clear();
			objControl.ModuleContext.Configuration = ModuleContext.Configuration;
			objControl.ID = System.IO.Path.GetFileNameWithoutExtension(ctlDirectory + Model.ControlToLoad);
			phUserControl.Controls.Add(objControl);

			if ((string)ViewState["CtlToLoad"] != Model.ControlToLoad)
			{
				ViewState["CtlToLoad"] = Model.ControlToLoad;
			}
		}

	}
}