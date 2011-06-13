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

using System.Linq;
using DotNetNuke.Entities.Content.Common;
using DotNetNuke.Entities.Content;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Forums.Components.Common;
using DotNetNuke.Modules.Forums.Components.Entities;

namespace DotNetNuke.Modules.Forums.Components.Taxonomy
{

	public class Content
	{

		/// <summary>
		/// This should only run after the Project exists in the data store. 
		/// </summary>
		/// <returns>The newly created ContentItemID from the data store.</returns>
		internal ContentItem CreateContentItem(SampleInfo objProject, int tabId)
		{
			var objContent = new ContentItem
								{
									Content = objProject.Description,
									ContentTypeId = GetContentTypeID(),
									Indexed = false,
									//ContentKey = "view=" + Constants.PageScope.ProjectDetail + "&project=" + objProject.Title,
									ModuleID = objProject.ModuleID,
									TabID = tabId
								};

			objContent.ContentItemId = Util.GetContentController().AddContentItem(objContent);

			// Add Terms
			var cntTerm = new Terms();
			cntTerm.ManageThreadTerms(objProject, objContent);

			return objContent;
		}

		/// <summary>
		/// This is used to update the content in the ContentItems table. Should be called when a project is updated.
		/// </summary>
		internal void UpdateContentItem(SampleInfo objProject, int tabId)
		{
			var objContent = Util.GetContentController().GetContentItem(objProject.ContentItemId);

			if (objContent == null) return;
			objContent.Content = objProject.Description;
			//objContent.ContentKey = "view=" + Constants.PageScope.ProjectDetail + "&project=" + objProject.Title;
			objContent.ModuleID = objProject.ModuleID;
			objContent.TabID = tabId;
			Util.GetContentController().UpdateContentItem(objContent);

			// Update Terms
			var cntTerm = new Terms();
			cntTerm.ManageThreadTerms(objProject, objContent);
		}

		/// <summary>
		/// This removes a content item associated with a project from the data store. Should run every time a project is deleted.
		/// </summary>
		/// <param name="objProject">The Content Item we wish to remove from the data store.</param>
		internal void DeleteContentItem(SampleInfo objProject)
		{
			if (objProject.ContentItemId <= Null.NullInteger) return;
			var objContent = Util.GetContentController().GetContentItem(objProject.ContentItemId);
			if (objContent == null) return;

			// remove any metadata/terms associated first (perhaps we should just rely on ContentItem cascade delete here?)
			var cntTerms = new Terms();
			cntTerms.RemoveThreadTerms(objProject);

			Util.GetContentController().DeleteContentItem(objContent);
		}

		/// <summary>
		/// This is used to determine the ContentTypeID (part of the Core API) based on this module's content type. If the content type doesn't exist yet for the module, it is created.
		/// </summary>
		/// <returns>The primary key value (ContentTypeID) from the core API's Content Types table.</returns>
		internal static int GetContentTypeID()
		{
			var typeController = new ContentTypeController();
			var colContentTypes = (from t in typeController.GetContentTypes() where t.ContentType == Constants.ContentTypeName select t);
			int contentTypeId;

			if (colContentTypes.Count() > 0)
			{
				var contentType = colContentTypes.Single();
				contentTypeId = contentType == null ? CreateContentType() : contentType.ContentTypeId;
			}
			else
			{
				contentTypeId = CreateContentType();
			}

			return contentTypeId;
		}

		#region Private Methods

		/// <summary>
		/// Creates a Content Type (for taxonomy) in the data store.
		/// </summary>
		/// <returns>The primary key value of the new ContentType.</returns>
		private static int CreateContentType()
		{
			var typeController = new ContentTypeController();
			var objContentType = new ContentType { ContentType = Constants.ContentTypeName };

			return typeController.AddContentType(objContentType);
		}

		#endregion

	}
}