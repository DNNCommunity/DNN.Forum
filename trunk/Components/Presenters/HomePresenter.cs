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

namespace DotNetNuke.Modules.Forums.Components.Presenters
{
    using System;
    using System.Collections.Generic;
    using Controllers;
    using Entities;
    using Models;
    using Models.ViewModels;
    using Providers.Data.SqlDataProvider;
    using Templating;
    using Views;
    using Web.Mvp;

    public class HomePresenter : ModulePresenter<IHomeView, HomeModel>
	{
        private ITemplateFileManager templateFileManager;
        protected IForumsController Controller { get; private set; }

		public HomePresenter(IHomeView view) : this(view, new ForumsController(new SqlDataProvider()), new TemplateFileManager())
		{
		}

		public HomePresenter(IHomeView view, IForumsController controller, ITemplateFileManager templateFileManager) : base(view)
		{
			this.Controller = controller;
		    this.templateFileManager = templateFileManager;
            this.View.Load += ViewLoad;
		}

        private void ViewLoad(object sender, EventArgs e)
        {
            base.OnLoad();
            View.Model.TopicListLink = Common.Links.ForumTopicList(this.ModuleContext, TabId, 1);

            List<ForumViewModel> forums = new ForumViewModelBuilder(Controller).Build(this.ModuleContext);
            View.Model.ForumListHtml = new TemplateProcessor(this.templateFileManager).ProcessTemplate("Default", "ListForums.template", new { forums });
            View.Refresh();
        }
	}
}