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

		public int AddPoll(PollInfo objPoll)
		{
			return _dataProvider.AddPoll(objPoll.TopicId, objPoll.UserId, objPoll.Question, objPoll.PollType, objPoll.CreatedOnDate);
			//Caching.ClearPollCache(objPoll.TopicId);
		}

		//public PollInfo GetPoll(int pollId)
		//{
		//    return CBO.FillObject<PollInfo>(_dataProvider.GetPoll(pollId));
		//}

		public List<PollInfo> GetPollByTopic(int topicId)
		{
			return CBO.FillCollection<PollInfo>(_dataProvider.GetPollByTopic(topicId));
		}

		public void UpdatePoll(PollInfo objPoll)
		{
			_dataProvider.UpdatePoll(objPoll.PollId, objPoll.TopicId, objPoll.UserId, objPoll.Question, objPoll.PollType, objPoll.CreatedOnDate);
			//Caching.ClearPollCache(ojbPoll.Topic);
		}

		public void DeletePoll(int pollId, int topicId)
		{
			_dataProvider.DeletePoll(pollId, topicId);
			//Caching.ClearPollCache(topicId);
		}

		#endregion

		#region Poll Option

		public int AddPollOption(PollOptionInfo objPollOption)
		{
			return _dataProvider.AddPollOption(objPollOption.PollId, objPollOption.OptionName, objPollOption.Priority, objPollOption.CreatedOnDate);
			//Caching.ClearPollOptionCache(objPollOption.PollId);
		}

		//public PollOptionInfo GetPollOption(int pollOptionId)
		//{
		//    return CBO.FillObject<PollOptionInfo>(_dataProvider.GetPollOption(pollOptionId));
		//}

		public List<PollOptionInfo> GetPollOptions(int pollId)
		{
			return CBO.FillCollection<PollOptionInfo>(_dataProvider.GetPollOptions(pollId));
		}

		public void UpdatePollOption(PollOptionInfo objPollOption, int pollId)
		{
			_dataProvider.UpdatePollOption(objPollOption.PollOptionId, objPollOption.PollId, objPollOption.OptionName, objPollOption.Priority, objPollOption.LastModifiedOnDate);
			//Caching.ClearPollOptionCache(pollId);
		}

		public void DeletePollOption(int pollOptionId, int pollId)
		{
			_dataProvider.DeletePollOption(pollOptionId, pollId);
			//Caching.ClearPollOptionCache(pollId);
		}

		#endregion

		#region Poll Result

		public int AddPollResult(PollResultInfo objPollResult)
		{
			return _dataProvider.AddPollResult(objPollResult.PollId, objPollResult.PollOptionId, objPollResult.Response, objPollResult.IpAddress, objPollResult.UserId, objPollResult.CreatedOnDate);
			//Caching.ClearPollResultsCache(objPollResult.PollId);
		}

		//public PollResultInfo GetPollResult(int pollResultId)
		//{
		//    return CBO.FillObject<PollResultInfo>(_dataProvider.GetPollResult(pollResultId));
		//}

		public List<PollResultInfo> GetPollResults(int pollId)
		{
			return CBO.FillCollection<PollResultInfo>(_dataProvider.GetPollResults(pollId));
		}

		public void UpdatePollResult(PollResultInfo objPollResult, int pollId)
		{
			_dataProvider.UpdatePollResult(objPollResult.PollResultId, objPollResult.PollId, objPollResult.PollOptionId, objPollResult.Response, objPollResult.IpAddress, objPollResult.UserId, objPollResult.LastModifiedOnDate);
			//Caching.ClearPollResultsCache(pollId);
		}

		public void DeletePollResult(int pollResultId, int pollId)
		{
			_dataProvider.DeletePollResult(pollResultId, pollId);
			//Caching.ClearPollResultsCache(pollId);
		}


		#endregion

		#region Post
		#endregion

		#region Post Attachment

		public int AddPostAttachment(PostAttachmentInfo objPostAttach, int topicId)
		{
			return _dataProvider.AddPostAttachment(objPostAttach.PostId, objPostAttach.FileId, objPostAttach.FileUrl, objPostAttach.FileName, objPostAttach.DisplayInline);
			//Caching.ClearPostAttachmentCache(topicId);
		}

		//public PostAttachmentInfo GetPostAttachment(int attachmentId)
		//{
		//    return CBO.FillObject<PostAttachmentInfo>(_dataProvider.GetPostAttachment(attachmentId));
		//}

		public List<PostAttachmentInfo> GetTopicAttachments(int topicId)
		{
			return CBO.FillCollection<PostAttachmentInfo>(_dataProvider.GetTopicAttachments(topicId));
		}

		public void UpdatePostAttachment(PostAttachmentInfo objPostAttach, int topicId)
		{
			_dataProvider.UpdatePostAttachment(objPostAttach.AttachmentId, objPostAttach.PostId, objPostAttach.FileId, objPostAttach.FileUrl, objPostAttach.FileName, objPostAttach.DisplayInline);
			//Caching.ClearPostAttachmentCache(topicId);
		}

		public void DeletePostAttachment(int attachmentId, int postId, int topicId)
		{
			_dataProvider.DeletePostAttachment(attachmentId, postId);
			//Caching.ClearPostAttachmentCache(topicId);
		}

		#endregion

		#region Post Rating

		public int AddPostRating(PostRatingInfo objPostRating, int topicId)
		{
			return _dataProvider.AddPostRating(objPostRating.PostId, objPostRating.UserId, objPostRating.Rating, objPostRating.Helpful, objPostRating.Comments, objPostRating.IpAddress, objPostRating.CreatedOnDate);
			//Caching.ClearPostRatingCache(topicId);
		}

		//public PostRatingInfo GetPostRating(int ratingId)
		//{
		//    return CBO.FillObject<PostRatingInfo>(_dataProvider.GetPostRating(ratingId));
		//}

		public List<PostRatingInfo> GetTopicRatings(int topicId)
		{
			return CBO.FillCollection<PostRatingInfo>(_dataProvider.GetTopicRatings(topicId));
		}

		public void UpdatePostRating(PostRatingInfo objPostRating, int topicId)
		{
			_dataProvider.UpdatePostRating(objPostRating.RatingId, objPostRating.PostId, objPostRating.UserId, objPostRating.Rating, objPostRating.Helpful, objPostRating.Comments, objPostRating.IpAddress);
			//Caching.ClearPostRatingCache(topicId);
		}

		public void DeletePostRating(int ratingId, int portalId, int topicId)
		{
			_dataProvider.DeletePostRating(ratingId, portalId);
			//Caching.ClearPostRatingCache(topicId);
		}

		#endregion

		#region Rank

		public int AddRank(RankInfo objRank)
		{
			return _dataProvider.AddRank(objRank.PortalId, objRank.ModuleId, objRank.RankName, objRank.MinPosts, objRank.MaxPosts, objRank.Display, objRank.CreatedOnDate);
		}

		public RankInfo GetRank(int rankId)
		{
			return CBO.FillObject<RankInfo>(_dataProvider.GetRank(rankId));
		}

		public List<RankInfo> GetModuleRank(int moduleId)
		{
			return CBO.FillCollection<RankInfo>(_dataProvider.GetModuleRank(moduleId));
		}

		public void UpdateRank(RankInfo objRank)
		{
			_dataProvider.UpdateRank(objRank.RankId, objRank.PortalId, objRank.ModuleId, objRank.RankName, objRank.MinPosts, objRank.MaxPosts, objRank.Display, objRank.LastModifiedOnDate);
			//Caching.ClearRankingCache(objRank.portalId);
		}

		public void DeleteRank(int rankId, int portalId)
		{
			_dataProvider.DeleteRank(rankId, portalId);
			//Caching.ClearRankingCache(portalId);
		}

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

		public int AddSubscription(SubscriptionInfo objSubscription)
		{
			return _dataProvider.AddSubscription(objSubscription.PortalId, objSubscription.ModuleId, objSubscription.ForumId, objSubscription.TopicId, objSubscription.SubscriptionType, objSubscription.UserId);
		}

		public SubscriptionInfo GetSubscription(int subscriptionId)
		{
			return CBO.FillObject<SubscriptionInfo>(_dataProvider.GetSubscription(subscriptionId));
		}

		public List<SubscriptionInfo> GetTopicsSubscribers(int portalId, int moduleId, int forumId, int topicId)
		{
			return CBO.FillCollection<SubscriptionInfo>(_dataProvider.GetTopicsSubscribers(portalId, moduleId, forumId, topicId));
		}

		public List<SubscriptionInfo> GetUsersSubscriptions(int portalId, int userId)
		{
			return CBO.FillCollection<SubscriptionInfo>(_dataProvider.GetUsersSubscriptions(portalId, userId));
		}

		public void UpdateSubscription(SubscriptionInfo objSubscription)
		{
			_dataProvider.UpdateSubscription(objSubscription.SubscriptionId, objSubscription.PortalId, objSubscription.ModuleId, objSubscription.ForumId, objSubscription.TopicId, objSubscription.SubscriptionType, objSubscription.UserId);
		}

		public void DeleteSubscription(int subscriptionId, int portalId)
		{
			_dataProvider.DeleteSubscription(subscriptionId, portalId);
		}

		#endregion

		#region Topic
		#endregion

		#region Topic Tracking

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
			//Caching.ClearUserCache(portalId, userId);
		}

		public void UpdateUser(UserInfo objUser)
		{
			_dataProvider.UpdateUser(objUser.PortalId, objUser.UserId, objUser.TopicCount, objUser.ReplyCount, objUser.RewardPoints, objUser.AnswerCount, objUser.QuestionCount, objUser.TrustLevel, objUser.UserCaption, objUser.LastPostDate, objUser.LastActivityDate, objUser.AdminWatch, objUser.DisableAttach, objUser.DisableHtml, objUser.LastModifiedOnDate);
			//Caching.ClearUserCache(portalId, userId);
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