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

using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Content;
using DotNetNuke.Modules.Forums.Components.Entities;

namespace DotNetNuke.Modules.Forums.Components.Taxonomy {

	public class Terms {

		/// <summary>
		/// This should run only after a thread has been added/updated in data store and the ContentItem exists.
		/// </summary>
		/// <param name="objProject">The content item we are associating categories with. In this module, it will always be a thread.</param>
		/// <param name="objContent"></param>
		internal void ManageThreadTerms(SampleInfo objProject, ContentItem objContent)
		{
			RemoveThreadTerms(objContent);

			foreach (var term in objProject.Terms) {
				Util.GetTermController().AddTermToContent(term, objContent);
			}
		}

		/// <summary>
		/// Removes terms associated w/ a specific ContentItem.
		/// </summary>
		/// <param name="objContent"></param>
		internal void RemoveThreadTerms(ContentItem objContent)
		{
			Util.GetTermController().RemoveTermsFromContent(objContent);
		}

	}
}