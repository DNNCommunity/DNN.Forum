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
    using Common;
    using Controllers;
    using Models;
    using Models.ViewModels;
    using Providers.Data.SqlDataProvider;
    using Templating;
    using Views;
    using Web.Mvp;

    public class HomePresenter : ModulePresenter<IHomeView, HomeModel>
	{
        private readonly ITemplateFileManager templateFileManager;
        private readonly IModuleInstanceContext moduleContextWrapper;
        private readonly IForumsController controller;
        private readonly IDnnUserController dnnUserController;

        public HomePresenter(IHomeView view) : this(view, new ForumsController(new SqlDataProvider()), new TemplateFileManager(), new ModuleContextWrapper(), new DnnUserController())
		{
		}

        public HomePresenter(IHomeView view, IForumsController controller, ITemplateFileManager templateFileManager, IModuleInstanceContext moduleContextWrapper, IDnnUserController dnnUserController)
            : base(view)
		{
			this.controller = controller;
		    this.templateFileManager = templateFileManager;
            moduleContextWrapper.ModuleContext = this.ModuleContext;
            this.moduleContextWrapper = moduleContextWrapper;
            this.dnnUserController = dnnUserController;
            this.View.Load += ViewLoad;
		}

        private void ViewLoad(object sender, EventArgs e)
        {
            List<ForumViewModel> forums = new ForumViewModelBuilder(this.controller, this.dnnUserController).Build(this.moduleContextWrapper);
            View.Model.ForumListHtml = new TemplateProcessor(this.templateFileManager).ProcessTemplate("Default", "ListForums.template", new { forums });
            View.Refresh();
        }
	}
}