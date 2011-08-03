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

using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Modules.Forums.Components.Entities;
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

		#region Filter

		public int AddFilter(FilterInfo objFilter)
		{
			return _dataProvider.AddFilter(objFilter.PortalId, objFilter.ModuleId, objFilter.ForumId, objFilter.Find, objFilter.Replace, objFilter.FilterType, objFilter.ApplyOnSave, objFilter.ApplyOnRender, objFilter.CreatedOnDate);
		}

		public FilterInfo GetFilter(int filterId)
		{
			return CBO.FillObject<FilterInfo>(_dataProvider.GetFilter(filterId));
		}

		public List<FilterInfo> GetAllFilters(int portalId)
		{
			return CBO.FillCollection<FilterInfo>(_dataProvider.GetAllFilters(portalId));
		}

		public void UpdateFilter(FilterInfo objFilter)
		{
			_dataProvider.UpdateFilter(objFilter.FilterId, objFilter.PortalId, objFilter.ModuleId, objFilter.ForumId, objFilter.Find, objFilter.Replace, objFilter.FilterType, objFilter.ApplyOnSave, objFilter.ApplyOnRender, objFilter.CreatedOnDate);
			//Caching.ClearFilterCache(filterId, portalId);
		}

		public void DeleteFilter(int filterId, int portalId)
		{
			_dataProvider.DeleteFilter(filterId, portalId);
			//Caching.ClearFilterCache(filterId, portalId);
		}

		#endregion

		#region Forum
		#endregion

		#region Permission

		public int AddPermission(PermissionInfo objPermission)
		{
			return _dataProvider.AddPermission(objPermission.Description, objPermission.PortalId, objPermission.CanView, objPermission.CanRead, objPermission.CanCreate, objPermission.CanEdit, objPermission.CanDelete, objPermission.CanLock, objPermission.CanPin, objPermission.CanAttach, objPermission.CanPoll, objPermission.CanBlock, objPermission.CanTrust, objPermission.CanSubscribe, objPermission.CanAnnounce, objPermission.CanTag, objPermission.CanPrioritize, objPermission.CanModApprove, objPermission.CanModMove, objPermission.CanModSplit, objPermission.CanModDelete, objPermission.CanModUser, objPermission.CanModEdit, objPermission.CanModLock, objPermission.CanModPin);
		}

		public PermissionInfo GetPermission(int permissionId)
		{
			return CBO.FillObject<PermissionInfo>(_dataProvider.GetPermission(permissionId));
		}

		public List<PermissionInfo> GetPortalPermissions(int portalId)
		{
			return CBO.FillCollection<PermissionInfo>(_dataProvider.GetPortalPermissions(portalId));
		}

		public void UpdatePermission(PermissionInfo objPermission)
		{
			_dataProvider.UpdatePermission(objPermission.PermissionId, objPermission.Description, objPermission.PortalId, objPermission.CanView, objPermission.CanRead, objPermission.CanCreate, objPermission.CanEdit, objPermission.CanDelete, objPermission.CanLock, objPermission.CanPin, objPermission.CanAttach, objPermission.CanPoll, objPermission.CanBlock, objPermission.CanTrust, objPermission.CanSubscribe, objPermission.CanAnnounce, objPermission.CanTag, objPermission.CanPrioritize, objPermission.CanModApprove, objPermission.CanModMove, objPermission.CanModSplit, objPermission.CanModDelete, objPermission.CanModUser, objPermission.CanModEdit, objPermission.CanModLock, objPermission.CanModPin);
			//Caching.ClearPermissionCache(permissionId, portalId);
		}

		public void DeletePermission(int permissionId, int portalId)
		{
			_dataProvider.DeletePermission(permissionId, portalId);
			//Caching.ClearPermissionCache(permissionId, portalId);
		}

		#endregion

		#region Poll
		#endregion

		#region Poll_Option
		#endregion

		#region Poll_Result
		#endregion

		#region Post
		#endregion

		#region Post_Attachment
		#endregion

		#region Post_Rating
		#endregion

		#region Rank
		#endregion

		#region Setting

		public int AddSetting(SettingInfo objSetting)
		{
			return _dataProvider.AddSetting(objSetting.Description, objSetting.Attachments, objSetting.Emoticons, objSetting.Html, objSetting.PostIcon, objSetting.Rss, objSetting.Scripts, objSetting.Moderated,objSetting.AutoTrustLevel,objSetting.AttachMaxCount,objSetting.AttachMaxSize,objSetting.AttachAutoResize,objSetting.AttachMaxHeight,objSetting.AttachMaxWidth, objSetting.AttachStore, objSetting.EditorType, objSetting.EditorHeight, objSetting.EditorWidth, objSetting.Filters);
		}

		public SettingInfo GetSetting(int settingId)
		{
			return CBO.FillObject<SettingInfo>(_dataProvider.GetSetting(settingId));
		}

		public List<SettingInfo> GetAllSettings()
		{
			return CBO.FillCollection<SettingInfo>(_dataProvider.GetAllSettings());
		}

		public void UpdateSetting(SettingInfo objSetting)
		{
			_dataProvider.UpdateSetting(objSetting.SettingId, objSetting.Description, objSetting.Attachments, objSetting.Emoticons, objSetting.Html, objSetting.PostIcon, objSetting.Rss, objSetting.Scripts, objSetting.Moderated, objSetting.AutoTrustLevel, objSetting.AttachMaxCount, objSetting.AttachMaxSize, objSetting.AttachAutoResize, objSetting.AttachMaxHeight, objSetting.AttachMaxWidth, objSetting.AttachStore, objSetting.EditorType, objSetting.EditorHeight, objSetting.EditorWidth, objSetting.Filters);
			//Caching.ClearSettingsCache();
		}

		public void DeleteSetting(int settingId)
		{
			_dataProvider.DeleteSetting(settingId);
			//Caching.ClearSettingsCache();
		}

		#endregion

		#region Subscription
		#endregion

		#region Topic
		#endregion

		#region Topic_Tracking

		public int AddTopicTracking(TopicTrackingInfo objTopicTracking)
		{
			return _dataProvider.AddTracking(objTopicTracking.ForumId, objTopicTracking.TopicId, objTopicTracking.LastPostId, objTopicTracking.UserId, objTopicTracking.CreatedOnDate);
		}

		//public TopicTrackingInfo GetTopicTracking(int topicTrackingId)
		//{
	   //     return CBO.FillObject<TopicTrackingInfo>(_dataProvider.GetTopicTracking(topicTrackingId));
		//}

		public List<TopicTrackingInfo> GetTopicTrackingByForum(int forumId)
		{
			return CBO.FillCollection<TopicTrackingInfo>(_dataProvider.GetTopicTrackingByForum(forumId));
		}

		public List<TopicTrackingInfo> GetTopicTrackingByTopic(int topicId)
		{
			return CBO.FillCollection<TopicTrackingInfo>(_dataProvider.GetTopicTrackingByTopic(topicId));
		}

		public void UpdateTopicTracking(TopicTrackingInfo objTopicTracking)
		{
			_dataProvider.UpdateTopicTracking(objTopicTracking.TopicTrackingId, objTopicTracking.ForumId, objTopicTracking.TopicId, objTopicTracking.LastPostId, objTopicTracking.UserId, objTopicTracking.LastModifiedOnDate);
		}

		public void DeleteTopicTracking(int topicTrackingId)
		{
			_dataProvider.DeleteTopicTracking(topicTrackingId);
		}

		#endregion

		#region Tracking

		public int AddTracking(TrackingInfo objTracking)
		{
			return _dataProvider.AddTracking(objTracking.ForumId, objTracking.UserId, objTracking.MaxTopicRead, objTracking.MaxPostRead, objTracking.LastAccessedOnDate);
		}

		//public TrackingInfo GetTracking(int trackingId)
		//{
		//    return CBO.FillObject<TrackingInfo>(_dataProvider.GetTracking(trackingId));
		//}

		public List<TrackingInfo> GetUsersTrackedForums(int userId)
		{
			return CBO.FillCollection<TrackingInfo>(_dataProvider.GetUsersTrackedForums(userId));
		}

		public void UpdateTracking(TrackingInfo objTracking)
		{
			_dataProvider.UpdateTracking(objTracking.TrackingId, objTracking.ForumId, objTracking.UserId, objTracking.MaxTopicRead, objTracking.MaxPostRead, objTracking.LastAccessedOnDate);
		}

		public void DeleteTracking(int trackingId)
		{
			_dataProvider.DeleteTracking(trackingId);
		}

		#endregion

		#region Url

		public int AddUrl(Entities.UrlInfo objUrl)
		{
			return _dataProvider.AddUrl(objUrl.PortalId, objUrl.ForumId, objUrl.TopicId, objUrl.Url, objUrl.CreatedOnDate);
		}

		public Entities.UrlInfo GetUrl(int id)
		{
			return CBO.FillObject<Entities.UrlInfo>(_dataProvider.GetUrl(id));
		}

		public List<Entities.UrlInfo> GetAllUrls(int portalId)
		{
			return CBO.FillCollection<Entities.UrlInfo>(_dataProvider.GetAllUrls(portalId));
		}

		public void UpdateUrl(Entities.UrlInfo objUrl)
		{
			_dataProvider.UpdateUrl(objUrl.Id, objUrl.PortalId, objUrl.ForumId, objUrl.TopicId, objUrl.Url, objUrl.LastModifiedOnDate);
			//Caching.ClearUrlCache(id, portalId);
		}

		public void DeleteUrl(int id, int portalId)
		{
			_dataProvider.DeleteFilter(id, portalId);
			//Caching.ClearUrlCache(id, portalId);
		}

		#endregion

		#region User

		public int AddUser(UserInfo objUser)
		{
			return _dataProvider.AddUser(objUser.PortalId, objUser.UserId, objUser.TopicCount, objUser.ReplyCount, objUser.RewardPoints, objUser.AnswerCount, objUser.QuestionCount, objUser.TrustLevel, objUser.UserCaption, objUser.LastPostDate, objUser.LastActivityDate, objUser.AdminWatch, objUser.DisableAttach, objUser.DisableHtml, objUser.CreatedOnDate);
		}

		public UserInfo GetUser(int portalId, int userId)
		{
			return CBO.FillObject<UserInfo>(_dataProvider.GetUser(portalId, userId));
		}

		public void UpdateUser(UserInfo objUser)
		{
			_dataProvider.UpdateUser(objUser.PortalId, objUser.UserId, objUser.TopicCount, objUser.ReplyCount, objUser.RewardPoints, objUser.AnswerCount, objUser.QuestionCount, objUser.TrustLevel, objUser.UserCaption, objUser.LastPostDate, objUser.LastActivityDate, objUser.AdminWatch, objUser.DisableAttach, objUser.DisableHtml, objUser.LastModifiedOnDate);
			//Caching.ClearSettingsCache();
		}

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