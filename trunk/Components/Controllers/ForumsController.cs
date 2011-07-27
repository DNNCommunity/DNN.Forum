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

using DotNetNuke.Modules.Forums.Providers.Data;
using DotNetNuke.Modules.Forums.Providers.Data.SqlDataProvider;

namespace DotNetNuke.Modules.Forums.Components.Controllers
{

	/// <summary>
	/// 
	/// </summary>
	/// <remarks></remarks>
	public class ForumsController : IForumsController {

		private readonly IDataProvider _dataProvider;

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public ForumsController()
		{
			_dataProvider = ComponentModel.ComponentFactory.GetComponent<IDataProvider>();
			if (_dataProvider != null) return;

			// get the provider configuration based on the type
			var defaultprovider = Data.DataProvider.Instance().DefaultProviderName;
			const string dataProviderNamespace = "DotNetNuke.Modules.ExtensionForge.Providers.Data";

			if (defaultprovider == "SqlDataProvider") 
			{
				_dataProvider = new SqlDataProvider();
			}
			else 
			{
				var providerType = dataProviderNamespace + "." + defaultprovider;
				_dataProvider = (IDataProvider)Framework.Reflection.CreateObject(providerType, providerType, true);
			}

			ComponentModel.ComponentFactory.RegisterComponentInstance<IDataProvider>(_dataProvider);
		}
		  
		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataProvider"></param>
		public ForumsController(IDataProvider dataProvider)
		{
			//DotNetNuke.Common.Requires.NotNull("dataProvider", dataProvider);
			_dataProvider = dataProvider;
		}

		#endregion

		#region Public Methods

		#region Project

		///// <summary>
		///// Adds a project to the data store and returns the ProjectInfo object (updated with ProjectID and ContentItemId) that we just added to the data store.
		///// </summary>
		///// <param name="objProject"></param>
		///// <param name="tabId"></param>
		///// <returns>The ProjectInfo object we added to the data store, along w/ it's newly created ContentItemId populated.</returns>
		//public ProjectInfo AddProject(ProjectInfo objProject, int tabId)
		//{
		//    objProject.ProjectId = _dataProvider.AddProject(objProject.Title, objProject.Url, objProject.Description, objProject.ExtensionType, objProject.ExtendedDescription, objProject.DemoUrl, objProject.Authorized, objProject.GroupId, objProject.PortalId, objProject.CreatedUserId, objProject.CreatedDate);

		//    objProject.ContentItemId = CompleteProjectCreation(objProject, tabId);

		//    return objProject;
		//}

		///// <summary>
		///// Uses a PK identifier to retrieve a project from the database. This is used for all unauthorized projects so they can be reviewed/edited. 
		///// </summary>
		///// <param name="projectId"></param>
		///// <param name="avatarPropDefinitionID"></param>
		///// <returns>Returns a single Project (equivalent to a single row of data) from the data store.</returns>
		//public ProjectInfo GetProject(int projectId, int avatarPropDefinitionID)
		//{
		//    var objProject = (ProjectInfo)DataCache.GetCache(Constants.ProjectCacheKey + projectId);

		//    if (objProject == null)
		//    {
		//        var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

		//        objProject = (ProjectInfo)CBO.FillObject(_dataProvider.GetProject(projectId, avatarPropDefinitionID), typeof(ProjectInfo));

		//        if (timeOut > 0 & Constants.EnableCaching & objProject != null)
		//        {
		//            DataCache.SetCache(Constants.ProjectCacheKey + projectId, objProject, TimeSpan.FromMinutes(timeOut));
		//        }
		//    }
		//    return objProject;
		//}

		///// <summary>
		///// This is used to check if a project title is already used in the data store. We do not use a key because its data is ultimately managed @ CodePlex.
		///// </summary>
		///// <param name="title"></param>
		///// <param name="avatarPropDefinitionID"></param>
		///// <returns></returns>
		///// <remarks>Not used yet, intended for project creation. DO NOT CACHE!</remarks>
		//public ProjectInfo GetProjectByTitle(string title, int avatarPropDefinitionID)
		//{
		//    return (ProjectInfo)CBO.FillObject(_dataProvider.GetProjectByTitle(title, avatarPropDefinitionID), typeof(ProjectInfo));
		//}

		///// <summary>
		///// Uses a project's CodePlex url to retrieve it from the database. This is used for all authorized projects and is much more human friendly.
		///// </summary>
		///// <param name="codePlexUrl"></param>
		///// <param name="avatarPropDefinitionID"></param>
		///// <returns>Returns a single Project (equivalent to a single row of data) from the data store.</returns>
		//public ProjectInfo GetProjectByUrl(string codePlexUrl, int avatarPropDefinitionID)
		//{
		//    // NOTE: we need to comeback to this, we shouldn't be caching the same object multiple times (which is what we are currently doing, although cache deletion accounts for this). 
		//    var objProject = (ProjectInfo)DataCache.GetCache(Constants.ProjectCacheKey + codePlexUrl);

		//    if (objProject == null)
		//    {
		//        var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

		//        objProject = (ProjectInfo)CBO.FillObject(_dataProvider.GetProjectByUrl(codePlexUrl, avatarPropDefinitionID), typeof(ProjectInfo));

		//        if (timeOut > 0 & Constants.EnableCaching & objProject != null)
		//        {
		//            DataCache.SetCache(Constants.ProjectCacheKey + codePlexUrl, objProject, TimeSpan.FromMinutes(timeOut));
		//        }
		//    }
		//    return objProject;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="portalID"></param>
		///// <param name="avatarPropDefinitionID"></param>
		///// <returns></returns>
		//public List<ProjectInfo> GetAuthorizedProjects(int portalID, int avatarPropDefinitionID)
		//{
		//    var colEntries = (List<ProjectInfo>)DataCache.GetCache(Constants.AuthorizedCacheKey);

		//    if (colEntries == null)
		//    {
		//        var timeOut = 20 * Convert.ToInt32(Host.PerformanceSetting);

		//        colEntries = CBO.FillCollection<ProjectInfo>(_dataProvider.GetAuthorizedProjects(portalID, avatarPropDefinitionID));

		//        if (timeOut > 0 & Constants.EnableCaching & colEntries != null)
		//        {
		//            DataCache.SetCache(Constants.AuthorizedCacheKey, colEntries, TimeSpan.FromMinutes(timeOut));
		//        }
		//    }
		//    return colEntries;
		//}

		///// <summary>
		///// Retrieves all projects that are not authorized from the data store. 
		///// </summary>
		///// <param name="portalID"></param>
		///// <param name="avatarPropDefinitionID"></param>
		///// <returns></returns>
		///// <remarks>Results are only meant to be seen by admin.</remarks>
		//public List<ProjectInfo> GetUnauthorizedProjects(int portalID, int avatarPropDefinitionID)
		//{
		//    return CBO.FillCollection<ProjectInfo>(_dataProvider.GetUnauthorizedProjects(portalID, avatarPropDefinitionID));
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="extensionTypeId"></param>
		///// <param name="content"></param>
		///// <param name="portalId"></param>
		///// <param name="tag"></param>
		///// <param name="avatarPropDefinitionID"></param>
		///// <returns></returns>
		///// <remarks>When parameters here change, you must also change GetTagsByTypeContentTag.</remarks>
		//public List<ProjectInfo> ProjectSearchByTypeContentTag(int extensionTypeId, string content, int portalId, string tag, int avatarPropDefinitionID)
		//{
		//    //DotNetNuke.Common.Requires.PropertyNotNegative("portalId", "", portalId);
		//    return CBO.FillCollection<ProjectInfo>(_dataProvider.ProjectSearchByTypeContentTag(extensionTypeId, content, portalId, tag, avatarPropDefinitionID));
		//}

		///// <summary>
		///// Updates an existing project  in the data store.
		///// </summary>
		///// <param name="objProject"></param>
		///// <param name="tabId"></param>
		///// <remarks>This is not meant to be used via the scheduled task (ie. updates from CodePlex directly).</remarks>
		//public void UpdateProject(ProjectInfo objProject, int tabId) {
		//    _dataProvider.UpdateProject(objProject.ProjectId, objProject.ExtensionType, objProject.ExtendedDescription, objProject.DemoUrl, objProject.Authorized, objProject.GroupId, objProject.PortalId, objProject.PrimaryMediaID, objProject.ContentItemId, objProject.WikiUrl, objProject.LastModifiedUserId, objProject.LastModifiedDate, objProject.ProjectEmail);

		//    CompleteProjectUpdate(objProject, tabId);
		//}

		///// <summary>
		///// Deletes a project from the data store.
		///// </summary>
		///// <param name="projectId"></param>
		///// <param name="portalId"></param>
		///// <param name="codePlexUrl"></param>
		//public void DeleteProject(int projectId, int portalId, string codePlexUrl) {
		//    _dataProvider.DeleteProject(projectId, portalId);

		//    Caching.ClearProjectCache(projectId, codePlexUrl);
		//}

		#endregion

		#endregion

		#region Private Methods

		///// <summary>
		///// This completes the things necessary for creating a content item in the data store.  It also adds the creator of the project as it's coordinator.
		///// </summary>
		///// <param name="objProject">The ProjectInfo entity we just created in the data store.</param>
		///// <param name="tabId">The page we will associate with our content item.</param>
		///// <returns>The ContentItemId primary key created in the Core ContentItems table.</returns>
		//private int CompleteProjectCreation(ProjectInfo objProject, int tabId)
		//{
		//    var cntTaxonomy = new Content();
		//    var objContentItem = cntTaxonomy.CreateContentItem(objProject, tabId);

		//    var objMember = new MemberInfo
		//    {
		//        UserID = objProject.CreatedUserId,
		//        MemberType = (int)Constants.MemberType.Coordinator,
		//        ProjectID = objProject.ProjectId,
		//        CreatedByUserID = objProject.CreatedUserId,
		//        CreatedOnDate = DateTime.Now
		//    };
		//    AddMember(objMember);

		//    return objContentItem.ContentItemId;
		//}

		///// <summary>
		///// This complets the things necessary for updating a content item, then clears the project cache.
		///// </summary>
		///// <param name="objProject"></param>
		///// <param name="tabId"></param>
		//private static void CompleteProjectUpdate(ProjectInfo objProject, int tabId)
		//{
		//    var cntTaxonomy = new Content();
		//    cntTaxonomy.UpdateContentItem(objProject, tabId);

		//    Caching.ClearProjectCache(objProject.ProjectId, objProject.Url);
		//}

		#endregion

	}
}