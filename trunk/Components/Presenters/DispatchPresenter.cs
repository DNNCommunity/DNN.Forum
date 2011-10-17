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
    using Controllers;
    using DotNetNuke.Common.Utilities;
    using Models;
    using Providers.Data.SqlDataProvider;
    using Views;
    using Web.Mvp;

	public class DispatchPresenter : ModulePresenter<IDispatchView, DispatchModel>
	{

		protected IForumsController Controller { get; private set; }

		private string ControlView
		{
			get
			{
				var controlView = Null.NullString;
				if (!String.IsNullOrEmpty(Request.Params["view"]))
				{
					controlView = Request.Params["view"];
				}
				return controlView;
			}
		}

		private const string CtlHome = "/Home.ascx";
        private const string CtlTopicList = "/TopicList.ascx";
        private const string CtlTopicDisplay = "/TopicDisplay.ascx";
        private const string CtlTopicEditor = "/TopicEditor.ascx";

		public DispatchPresenter(IDispatchView view) : this(view, new ForumsController(new SqlDataProvider()))
		{

		}

		public DispatchPresenter(IDispatchView view, IForumsController controller) : base(view)
		{
			if (view == null)
			{
				throw new ArgumentException(@"View is nothing.", "view");
			}

			if (controller == null)
			{
				throw new ArgumentException(@"Controller is nothing.", "controller");
			}

			Controller = controller;
		}

	    protected override void OnInit()
		{
            base.OnInit();

            View.Model.IsEditable = ModuleContext.IsEditable;
			switch (ControlView.ToLowerInvariant())
			{
				case "home" :
					View.Model.ControlToLoad = CtlHome;
					break;
                case "topics" :
                    View.Model.ControlToLoad = CtlTopicList;
                    break;
                case "topic" :
                    View.Model.ControlToLoad = CtlTopicDisplay;
                    break;
                case "editor" :
                    View.Model.ControlToLoad = CtlTopicEditor;
                    break;
				default:
					View.Model.ControlToLoad = CtlHome;
					break;
			}
			this.View.Refresh();
		}
	}
}