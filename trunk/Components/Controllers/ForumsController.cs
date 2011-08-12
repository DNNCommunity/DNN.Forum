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

namespace DotNetNuke.Modules.Forums.Components.Controllers
{
    using System.Collections.Generic;
    using DotNetNuke.Common.Utilities;
    using Entities;
    using Providers.Data;
    using Providers.Data.SqlDataProvider;

	public class ForumsController : IForumsController {

		private readonly IDataProvider dataProvider;

		#region Constructors

		public ForumsController()
		{
			dataProvider = ComponentModel.ComponentFactory.GetComponent<IDataProvider>();
			if (dataProvider != null) return;

			// get the provider configuration based on the type
			var defaultprovider = Data.DataProvider.Instance().DefaultProviderName;
			const string dataProviderNamespace = "DotNetNuke.Modules.ExtensionForge.Providers.Data";

			if (defaultprovider == "SqlDataProvider") 
			{
				dataProvider = new SqlDataProvider();
			}
			else 
			{
				var providerType = dataProviderNamespace + "." + defaultprovider;
				dataProvider = (IDataProvider)Framework.Reflection.CreateObject(providerType, providerType, true);
			}

			ComponentModel.ComponentFactory.RegisterComponentInstance<IDataProvider>(dataProvider);
		}
		  
		public ForumsController(IDataProvider dataProvider)
		{
			this.dataProvider = dataProvider;
		}

		#endregion

		#region Public Methods

		#region Filter

		public int AddFilter(FilterInfo objFilter)
		{
			return dataProvider.AddFilter(objFilter.PortalId, objFilter.ModuleId, objFilter.ForumId, objFilter.Find, objFilter.Replace, objFilter.FilterType, objFilter.ApplyOnSave, objFilter.ApplyOnRender, objFilter.CreatedOnDate);
		}

		public FilterInfo GetFilter(int filterId)
		{
			return CBO.FillObject<FilterInfo>(dataProvider.GetFilter(filterId));
		}

		public List<FilterInfo> GetAllFilters(int portalId)
		{
			return CBO.FillCollection<FilterInfo>(dataProvider.GetAllFilters(portalId));
		}

		public void UpdateFilter(FilterInfo objFilter)
		{
			dataProvider.UpdateFilter(objFilter.FilterId, objFilter.PortalId, objFilter.ModuleId, objFilter.ForumId, objFilter.Find, objFilter.Replace, objFilter.FilterType, objFilter.ApplyOnSave, objFilter.ApplyOnRender, objFilter.CreatedOnDate);
			//Caching.ClearFilterCache(filterId, portalId);
		}

		public void DeleteFilter(int filterId, int portalId)
		{
			dataProvider.DeleteFilter(filterId, portalId);
			//Caching.ClearFilterCache(filterId, portalId);
		}

		#endregion

		#region Forum

		public int AddForum(ForumInfo objForum)
		{
			return dataProvider.AddForum(objForum.PortalId, objForum.ModuleId, objForum.ParentId, objForum.AllowTopics, objForum.Name, objForum.Description, objForum.SortOrder, objForum.Active, objForum.Hidden, objForum.TopicCount, objForum.ReplyCount, objForum.LastPostId, objForum.Slug, objForum.PermissionId, objForum.SettingId, objForum.EmailAddress, objForum.SiteMapPriority, objForum.CreatedOnDate, objForum.CreatedByUserId);
			//Caching.ClearForumCache(objForum.ModuleId, objForum.PortalId);
		}

		public ForumInfo GetForum(int forumId)
		{
			return CBO.FillObject<ForumInfo>(dataProvider.GetForum(forumId));
		}

		public List<ForumInfo> GetModuleForums(int moduleId)
		{
			return CBO.FillCollection<ForumInfo>(dataProvider.GetModuleForums(moduleId));
		}

		public void UpdateForum(ForumInfo objForum)
		{
			dataProvider.UpdateForum(objForum.ForumId, objForum.PortalId, objForum.ModuleId, objForum.ParentId, objForum.AllowTopics, objForum.Name, objForum.Description, objForum.SortOrder, objForum.Active, objForum.Hidden, objForum.TopicCount, objForum.ReplyCount, objForum.LastPostId, objForum.Slug, objForum.PermissionId, objForum.SettingId, objForum.EmailAddress, objForum.SiteMapPriority, objForum.LastModifiedOnDate, objForum.LastModifiedByUserId);
			//Caching.ClearForumCache(objForum.ModuleId, objForum.PortalId);
		}

		public void DeleteForum(int forumId, int moduleId, int portalId)
		{
			dataProvider.DeleteForum(forumId, moduleId);
			//Caching.ClearForumCache(moduleId, portalId);
		}

		#endregion

		#region Permission

		public int AddPermission(PermissionInfo objPermission)
		{
			return dataProvider.AddPermission(objPermission.Description, objPermission.PortalId, objPermission.CanView, objPermission.CanRead, objPermission.CanCreate, objPermission.CanEdit, objPermission.CanDelete, objPermission.CanLock, objPermission.CanPin, objPermission.CanAttach, objPermission.CanPoll, objPermission.CanBlock, objPermission.CanTrust, objPermission.CanSubscribe, objPermission.CanAnnounce, objPermission.CanTag, objPermission.CanPrioritize, objPermission.CanModApprove, objPermission.CanModMove, objPermission.CanModSplit, objPermission.CanModDelete, objPermission.CanModUser, objPermission.CanModEdit, objPermission.CanModLock, objPermission.CanModPin);
		}

		public PermissionInfo GetPermission(int permissionId)
		{
			return CBO.FillObject<PermissionInfo>(dataProvider.GetPermission(permissionId));
		}

		public List<PermissionInfo> GetPortalPermissions(int portalId)
		{
			return CBO.FillCollection<PermissionInfo>(dataProvider.GetPortalPermissions(portalId));
		}

		public void UpdatePermission(PermissionInfo objPermission)
		{
			dataProvider.UpdatePermission(objPermission.PermissionId, objPermission.Description, objPermission.PortalId, objPermission.CanView, objPermission.CanRead, objPermission.CanCreate, objPermission.CanEdit, objPermission.CanDelete, objPermission.CanLock, objPermission.CanPin, objPermission.CanAttach, objPermission.CanPoll, objPermission.CanBlock, objPermission.CanTrust, objPermission.CanSubscribe, objPermission.CanAnnounce, objPermission.CanTag, objPermission.CanPrioritize, objPermission.CanModApprove, objPermission.CanModMove, objPermission.CanModSplit, objPermission.CanModDelete, objPermission.CanModUser, objPermission.CanModEdit, objPermission.CanModLock, objPermission.CanModPin);
			//Caching.ClearPermissionCache(permissionId, portalId);
		}

		public void DeletePermission(int permissionId, int portalId)
		{
			dataProvider.DeletePermission(permissionId, portalId);
			//Caching.ClearPermissionCache(permissionId, portalId);
		}

		#endregion

		#region Poll

		public int AddPoll(PollInfo objPoll)
		{
			return dataProvider.AddPoll(objPoll.TopicId, objPoll.UserId, objPoll.Question, objPoll.PollType, objPoll.CreatedOnDate);
			//Caching.ClearPollCache(objPoll.TopicId);
		}

		//public PollInfo GetPoll(int pollId)
		//{
		//    return CBO.FillObject<PollInfo>(_dataProvider.GetPoll(pollId));
		//}

		public List<PollInfo> GetPollByTopic(int topicId)
		{
			return CBO.FillCollection<PollInfo>(dataProvider.GetPollByTopic(topicId));
		}

		public void UpdatePoll(PollInfo objPoll)
		{
			dataProvider.UpdatePoll(objPoll.PollId, objPoll.TopicId, objPoll.UserId, objPoll.Question, objPoll.PollType, objPoll.CreatedOnDate);
			//Caching.ClearPollCache(ojbPoll.Topic);
		}

		public void DeletePoll(int pollId, int topicId)
		{
			dataProvider.DeletePoll(pollId, topicId);
			//Caching.ClearPollCache(topicId);
		}

		#endregion

		#region Poll Option

		public int AddPollOption(PollOptionInfo objPollOption)
		{
			return dataProvider.AddPollOption(objPollOption.PollId, objPollOption.OptionName, objPollOption.Priority, objPollOption.CreatedOnDate);
			//Caching.ClearPollOptionCache(objPollOption.PollId);
		}

		//public PollOptionInfo GetPollOption(int pollOptionId)
		//{
		//    return CBO.FillObject<PollOptionInfo>(_dataProvider.GetPollOption(pollOptionId));
		//}

		public List<PollOptionInfo> GetPollOptions(int pollId)
		{
			return CBO.FillCollection<PollOptionInfo>(dataProvider.GetPollOptions(pollId));
		}

		public void UpdatePollOption(PollOptionInfo objPollOption, int pollId)
		{
			dataProvider.UpdatePollOption(objPollOption.PollOptionId, objPollOption.PollId, objPollOption.OptionName, objPollOption.Priority, objPollOption.LastModifiedOnDate);
			//Caching.ClearPollOptionCache(pollId);
		}

		public void DeletePollOption(int pollOptionId, int pollId)
		{
			dataProvider.DeletePollOption(pollOptionId, pollId);
			//Caching.ClearPollOptionCache(pollId);
		}

		#endregion

		#region Poll Result

		public int AddPollResult(PollResultInfo objPollResult)
		{
			return dataProvider.AddPollResult(objPollResult.PollId, objPollResult.PollOptionId, objPollResult.Response, objPollResult.IpAddress, objPollResult.UserId, objPollResult.CreatedOnDate);
			//Caching.ClearPollResultsCache(objPollResult.PollId);
		}

		//public PollResultInfo GetPollResult(int pollResultId)
		//{
		//    return CBO.FillObject<PollResultInfo>(_dataProvider.GetPollResult(pollResultId));
		//}

		public List<PollResultInfo> GetPollResults(int pollId)
		{
			return CBO.FillCollection<PollResultInfo>(dataProvider.GetPollResults(pollId));
		}

		public void UpdatePollResult(PollResultInfo objPollResult, int pollId)
		{
			dataProvider.UpdatePollResult(objPollResult.PollResultId, objPollResult.PollId, objPollResult.PollOptionId, objPollResult.Response, objPollResult.IpAddress, objPollResult.UserId, objPollResult.LastModifiedOnDate);
			//Caching.ClearPollResultsCache(pollId);
		}

		public void DeletePollResult(int pollResultId, int pollId)
		{
			dataProvider.DeletePollResult(pollResultId, pollId);
			//Caching.ClearPollResultsCache(pollId);
		}


		#endregion

		#region Post

		public int AddPost(PostInfo objPost)
		{
			return dataProvider.AddPost(objPost.TopicId, objPost.ParentPostId, objPost.Subject, objPost.Body, objPost.IsApproved, objPost.IsLocked, objPost.IsPinned, objPost.UserId, objPost.DisplayName, objPost.EmailAddress, objPost.IpAddress, objPost.PostReported, objPost.Rating, objPost.PostIcon, objPost.StatusId, objPost.Slug, objPost.PostData, objPost.ApprovedOnDate, objPost.CreatedDate);
			//Caching.ClearPostCache(topicId);
		}

		public PostInfo GetPost(int postId)
		{
			return CBO.FillObject<PostInfo>(dataProvider.GetPost(postId));
		}

		public List<PostInfo> GetTopicPosts(int topicId)
		{
			return CBO.FillCollection<PostInfo>(dataProvider.GetTopicPosts(topicId));
		}

		public void UpdatePost(PostInfo objPost)
		{
			dataProvider.UpdatePost(objPost.PostId, objPost.TopicId, objPost.ParentPostId, objPost.Subject, objPost.Body, objPost.IsApproved, objPost.IsLocked, objPost.IsPinned, objPost.DisplayName, objPost.EmailAddress, objPost.PostReported, objPost.Rating, objPost.PostIcon, objPost.StatusId, objPost.Slug, objPost.PostData, objPost.ApprovedOnDate, objPost.LastModifiedDate);
		}

		public void DeletePost(int postId, int topicId)
		{
			dataProvider.DeletePost(postId, topicId);
			//Caching.ClearPostCache(topicId);
		}

		public void HardDeletePost(int postId, int topicId)
		{
			dataProvider.HardDeletePost(postId, topicId);
			//Caching.ClearPostCache(topicId);
		}

		#endregion

		#region Post Attachment

		public int AddPostAttachment(PostAttachmentInfo objPostAttach, int topicId)
		{
			return dataProvider.AddPostAttachment(objPostAttach.PostId, objPostAttach.FileId, objPostAttach.FileUrl, objPostAttach.FileName, objPostAttach.DisplayInline);
			//Caching.ClearPostAttachmentCache(topicId);
		}

		//public PostAttachmentInfo GetPostAttachment(int attachmentId)
		//{
		//    return CBO.FillObject<PostAttachmentInfo>(_dataProvider.GetPostAttachment(attachmentId));
		//}

		public List<PostAttachmentInfo> GetTopicAttachments(int topicId)
		{
			return CBO.FillCollection<PostAttachmentInfo>(dataProvider.GetTopicAttachments(topicId));
		}

		public void UpdatePostAttachment(PostAttachmentInfo objPostAttach, int topicId)
		{
			dataProvider.UpdatePostAttachment(objPostAttach.AttachmentId, objPostAttach.PostId, objPostAttach.FileId, objPostAttach.FileUrl, objPostAttach.FileName, objPostAttach.DisplayInline);
			//Caching.ClearPostAttachmentCache(topicId);
		}

		public void DeletePostAttachment(int attachmentId, int postId, int topicId)
		{
			dataProvider.DeletePostAttachment(attachmentId, postId);
			//Caching.ClearPostAttachmentCache(topicId);
		}

		#endregion

		#region Post Rating

		public int AddPostRating(PostRatingInfo objPostRating, int topicId)
		{
			return dataProvider.AddPostRating(objPostRating.PostId, objPostRating.UserId, objPostRating.Rating, objPostRating.Helpful, objPostRating.Comments, objPostRating.IpAddress, objPostRating.CreatedOnDate);
			//Caching.ClearPostRatingCache(topicId);
		}

		//public PostRatingInfo GetPostRating(int ratingId)
		//{
		//    return CBO.FillObject<PostRatingInfo>(_dataProvider.GetPostRating(ratingId));
		//}

		public List<PostRatingInfo> GetTopicRatings(int topicId)
		{
			return CBO.FillCollection<PostRatingInfo>(dataProvider.GetTopicRatings(topicId));
		}

		public void UpdatePostRating(PostRatingInfo objPostRating, int topicId)
		{
			dataProvider.UpdatePostRating(objPostRating.RatingId, objPostRating.PostId, objPostRating.UserId, objPostRating.Rating, objPostRating.Helpful, objPostRating.Comments, objPostRating.IpAddress);
			//Caching.ClearPostRatingCache(topicId);
		}

		public void DeletePostRating(int ratingId, int portalId, int topicId)
		{
			dataProvider.DeletePostRating(ratingId, portalId);
			//Caching.ClearPostRatingCache(topicId);
		}

		#endregion

		#region Rank

		public int AddRank(RankInfo objRank)
		{
			return dataProvider.AddRank(objRank.PortalId, objRank.ModuleId, objRank.RankName, objRank.MinPosts, objRank.MaxPosts, objRank.Display, objRank.CreatedOnDate);
		}

		public RankInfo GetRank(int rankId)
		{
			return CBO.FillObject<RankInfo>(dataProvider.GetRank(rankId));
		}

		public List<RankInfo> GetModuleRank(int moduleId)
		{
			return CBO.FillCollection<RankInfo>(dataProvider.GetModuleRank(moduleId));
		}

		public void UpdateRank(RankInfo objRank)
		{
			dataProvider.UpdateRank(objRank.RankId, objRank.PortalId, objRank.ModuleId, objRank.RankName, objRank.MinPosts, objRank.MaxPosts, objRank.Display, objRank.LastModifiedOnDate);
			//Caching.ClearRankingCache(objRank.portalId);
		}

		public void DeleteRank(int rankId, int portalId)
		{
			dataProvider.DeleteRank(rankId, portalId);
			//Caching.ClearRankingCache(portalId);
		}

		#endregion

		#region Setting

		public int AddSetting(SettingInfo objSetting)
		{
			return dataProvider.AddSetting(objSetting.Description, objSetting.Attachments, objSetting.Emoticons, objSetting.Html, objSetting.PostIcon, objSetting.Rss, objSetting.Scripts, objSetting.Moderated,objSetting.AutoTrustLevel,objSetting.AttachMaxCount,objSetting.AttachMaxSize,objSetting.AttachAutoResize,objSetting.AttachMaxHeight,objSetting.AttachMaxWidth, objSetting.AttachStore, objSetting.EditorType, objSetting.EditorHeight, objSetting.EditorWidth, objSetting.Filters);
		}

		public SettingInfo GetSetting(int settingId)
		{
			return CBO.FillObject<SettingInfo>(dataProvider.GetSetting(settingId));
		}

		public List<SettingInfo> GetAllSettings()
		{
			return CBO.FillCollection<SettingInfo>(dataProvider.GetAllSettings());
		}

		public void UpdateSetting(SettingInfo objSetting)
		{
			dataProvider.UpdateSetting(objSetting.SettingId, objSetting.Description, objSetting.Attachments, objSetting.Emoticons, objSetting.Html, objSetting.PostIcon, objSetting.Rss, objSetting.Scripts, objSetting.Moderated, objSetting.AutoTrustLevel, objSetting.AttachMaxCount, objSetting.AttachMaxSize, objSetting.AttachAutoResize, objSetting.AttachMaxHeight, objSetting.AttachMaxWidth, objSetting.AttachStore, objSetting.EditorType, objSetting.EditorHeight, objSetting.EditorWidth, objSetting.Filters);
			//Caching.ClearSettingsCache();
		}

		public void DeleteSetting(int settingId)
		{
			dataProvider.DeleteSetting(settingId);
			//Caching.ClearSettingsCache();
		}

		#endregion

		#region Subscription

		public int AddSubscription(SubscriptionInfo objSubscription)
		{
			return dataProvider.AddSubscription(objSubscription.PortalId, objSubscription.ModuleId, objSubscription.ForumId, objSubscription.TopicId, objSubscription.SubscriptionType, objSubscription.UserId);
		}

		public SubscriptionInfo GetSubscription(int subscriptionId)
		{
			return CBO.FillObject<SubscriptionInfo>(dataProvider.GetSubscription(subscriptionId));
		}

		public List<SubscriptionInfo> GetTopicsSubscribers(int portalId, int moduleId, int forumId, int topicId)
		{
			return CBO.FillCollection<SubscriptionInfo>(dataProvider.GetTopicsSubscribers(portalId, moduleId, forumId, topicId));
		}

		public List<SubscriptionInfo> GetUsersSubscriptions(int portalId, int userId)
		{
			return CBO.FillCollection<SubscriptionInfo>(dataProvider.GetUsersSubscriptions(portalId, userId));
		}

		public void UpdateSubscription(SubscriptionInfo objSubscription)
		{
			dataProvider.UpdateSubscription(objSubscription.SubscriptionId, objSubscription.PortalId, objSubscription.ModuleId, objSubscription.ForumId, objSubscription.TopicId, objSubscription.SubscriptionType, objSubscription.UserId);
		}

		public void DeleteSubscription(int subscriptionId, int portalId)
		{
			dataProvider.DeleteSubscription(subscriptionId, portalId);
		}

		#endregion

		#region Topic

		public int AddTopic(TopicInfo objTopic, int moduleId, int portalId)
		{
			return dataProvider.AddTopic(objTopic.ForumId, objTopic.ViewCount, objTopic.ReplyCount, objTopic.TopicTypeId, objTopic.LastPostId, objTopic.Slug);
			//Caching.ClearTopicCache(topicId, forumId, moduleId, portalId); 
		}

		public TopicInfo GetTopic(int topicId)
		{
			return CBO.FillObject<TopicInfo>(dataProvider.GetTopic(topicId));
		}

		public List<TopicInfo> GetForumTopics(int forumId, int pageIndex, int pageSize)
		{
			return CBO.FillCollection<TopicInfo>(dataProvider.GetForumTopics(forumId, pageIndex, pageSize));
		}

		//public List<TopicInfo> GetAllModuleTopics(int moduleId, int pageIndex, int pageSize)
		//{
		//    return CBO.FillCollection<TopicInfo>(_dataProvider.GetAllModuleTopics(moduleId, pageIndex, pageSize));
		//}

		public void UpdateTopic(TopicInfo objTopic, int moduleId, int portalId)
		{
			dataProvider.UpdateTopic(objTopic.TopicId, objTopic.ForumId, objTopic.ViewCount, objTopic.ReplyCount, objTopic.TopicTypeId, objTopic.LastPostId, objTopic.Slug, objTopic.ContentItemId);
			//Caching.ClearTopicCache(topicId, forumId, moduleId, portalId); 
		}

		public void DeleteTopic(int topicId, int forumId, int moduleId, int portalId)
		{
			dataProvider.DeleteTopic(topicId, forumId);
			//Caching.ClearTopicCache(topicId, forumId, moduleId, portalId); 
		}

		#endregion

		#region Topic Tracking

		public int AddTopicTracking(TopicTrackingInfo objTopicTracking)
		{
			return dataProvider.AddTracking(objTopicTracking.ForumId, objTopicTracking.TopicId, objTopicTracking.LastPostId, objTopicTracking.UserId, objTopicTracking.CreatedOnDate);
		}

		//public TopicTrackingInfo GetTopicTracking(int topicTrackingId)
		//{
	   //     return CBO.FillObject<TopicTrackingInfo>(_dataProvider.GetTopicTracking(topicTrackingId));
		//}

		public List<TopicTrackingInfo> GetTopicTrackingByForum(int forumId)
		{
			return CBO.FillCollection<TopicTrackingInfo>(dataProvider.GetTopicTrackingByForum(forumId));
		}

		public List<TopicTrackingInfo> GetTopicTrackingByTopic(int topicId)
		{
			return CBO.FillCollection<TopicTrackingInfo>(dataProvider.GetTopicTrackingByTopic(topicId));
		}

		public void UpdateTopicTracking(TopicTrackingInfo objTopicTracking)
		{
			dataProvider.UpdateTopicTracking(objTopicTracking.TopicTrackingId, objTopicTracking.ForumId, objTopicTracking.TopicId, objTopicTracking.LastPostId, objTopicTracking.UserId, objTopicTracking.LastModifiedOnDate);
		}

		public void DeleteTopicTracking(int topicTrackingId)
		{
			dataProvider.DeleteTopicTracking(topicTrackingId);
		}

		#endregion

		#region Tracking

		public int AddTracking(TrackingInfo objTracking)
		{
			return dataProvider.AddTracking(objTracking.ForumId, objTracking.UserId, objTracking.MaxTopicRead, objTracking.MaxPostRead, objTracking.LastAccessedOnDate);
		}

		//public TrackingInfo GetTracking(int trackingId)
		//{
		//    return CBO.FillObject<TrackingInfo>(_dataProvider.GetTracking(trackingId));
		//}

		public List<TrackingInfo> GetUsersTrackedForums(int userId)
		{
			return CBO.FillCollection<TrackingInfo>(dataProvider.GetUsersTrackedForums(userId));
		}

		public void UpdateTracking(TrackingInfo objTracking)
		{
			dataProvider.UpdateTracking(objTracking.TrackingId, objTracking.ForumId, objTracking.UserId, objTracking.MaxTopicRead, objTracking.MaxPostRead, objTracking.LastAccessedOnDate);
		}

		public void DeleteTracking(int trackingId)
		{
			dataProvider.DeleteTracking(trackingId);
		}

		#endregion

		#region Url

		public int AddUrl(Entities.UrlInfo objUrl)
		{
			return dataProvider.AddUrl(objUrl.PortalId, objUrl.ForumId, objUrl.TopicId, objUrl.Url, objUrl.CreatedOnDate);
		}

		public Entities.UrlInfo GetUrl(int id)
		{
			return CBO.FillObject<Entities.UrlInfo>(dataProvider.GetUrl(id));
		}

		public List<Entities.UrlInfo> GetAllUrls(int portalId)
		{
			return CBO.FillCollection<Entities.UrlInfo>(dataProvider.GetAllUrls(portalId));
		}

		public void UpdateUrl(Entities.UrlInfo objUrl)
		{
			dataProvider.UpdateUrl(objUrl.Id, objUrl.PortalId, objUrl.ForumId, objUrl.TopicId, objUrl.Url, objUrl.LastModifiedOnDate);
			//Caching.ClearUrlCache(id, portalId);
		}

		public void DeleteUrl(int id, int portalId)
		{
			dataProvider.DeleteFilter(id, portalId);
			//Caching.ClearUrlCache(id, portalId);
		}

		#endregion

		#region User

		public int AddUser(UserInfo objUser)
		{
            //Personalization.SetProfile()
			return dataProvider.AddUser(objUser.PortalId, objUser.UserId, objUser.TopicCount, objUser.ReplyCount, objUser.RewardPoints, objUser.AnswerCount, objUser.QuestionCount, objUser.TrustLevel, objUser.UserCaption, objUser.LastPostDate, objUser.LastActivityDate, objUser.AdminWatch, objUser.DisableAttach, objUser.DisableHtml, objUser.CreatedOnDate);
		}

		public UserInfo GetUser(int portalId, int userId)
		{
			return CBO.FillObject<UserInfo>(dataProvider.GetUser(portalId, userId));
			//Caching.ClearUserCache(portalId, userId);
		}

		public void UpdateUser(UserInfo objUser)
		{
			dataProvider.UpdateUser(objUser.PortalId, objUser.UserId, objUser.TopicCount, objUser.ReplyCount, objUser.RewardPoints, objUser.AnswerCount, objUser.QuestionCount, objUser.TrustLevel, objUser.UserCaption, objUser.LastPostDate, objUser.LastActivityDate, objUser.AdminWatch, objUser.DisableAttach, objUser.DisableHtml, objUser.LastModifiedOnDate);
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