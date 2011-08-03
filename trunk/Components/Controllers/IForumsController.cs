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
using DotNetNuke.Modules.Forums.Components.Entities;

namespace DotNetNuke.Modules.Forums.Components.Controllers 
{

	/// <summary>
	/// 
	/// </summary>
	public interface IForumsController
	{
		
		#region Filter

		int AddFilter(FilterInfo objFilter);

		FilterInfo GetFilter(int filterId);

		List<FilterInfo> GetAllFilters(int portalId);

		void UpdateFilter(FilterInfo objFilter);

		void DeleteFilter(int filterId, int portalId);

		#endregion

		#region Forum
		#endregion

		#region Permission

		int AddPermission(PermissionInfo objPermission);

		PermissionInfo GetPermission(int permissionId);

		List<PermissionInfo> GetPortalPermissions(int portalId);

		void UpdatePermission(PermissionInfo objPermission);

		void DeletePermission(int permissionId, int portalId);

		#endregion

		#region Poll
		#endregion

		#region Poll Option
		#endregion

		#region Poll Result
		#endregion

		#region Post
		#endregion

		#region Post Attachment
		#endregion

		#region Post Rating

		int AddPostRating(PostRatingInfo objPostRating);

		PostRatingInfo GetPostRating(int ratingId);

		List<PostRatingInfo> GetTopicRatings(int topicId);

		void UpdatePostRating(PostRatingInfo objPostRating);

		void DeletePostRating(int ratingId, int portalId, int topicId);

		#endregion

		#region Rank

		int AddRank(RankInfo objRank);

		RankInfo GetRank(int rankId);

		List<RankInfo> GetModuleRank(int moduleId);

		void UpdateRank(RankInfo objRank);

		void DeleteRank(int rankId, int portalId);

		#endregion

		#region Setting

		int AddSetting(SettingInfo objSetting);

		SettingInfo GetSetting(int settingId);

		List<SettingInfo> GetAllSettings();

		void UpdateSetting(SettingInfo objSetting);

		void DeleteSetting(int settingId);

		#endregion

		#region Subscription

		int AddSubscription(SubscriptionInfo objSubscription);

		SubscriptionInfo GetSubscription(int subscriptionId);

		List<SubscriptionInfo> GetTopicsSubscribers(int portalId, int moduleId, int forumId, int topicId);

		List<SubscriptionInfo> GetUsersSubscriptions(int portalId, int userId);

		void UpdateSubscription(SubscriptionInfo objSubscription);

		void DeleteSubscription(int subscriptionId, int portalId);

		#endregion

		#region Topic
		#endregion

		#region Topic Tracking

		int AddTopicTracking(TopicTrackingInfo objTopicTracking);

		//TopicTrackingInfo GetTopicTracking(int topicTrackingId);

		List<TopicTrackingInfo> GetTopicTrackingByForum(int forumId);

		List<TopicTrackingInfo> GetTopicTrackingByTopic(int topicId);

		void UpdateTopicTracking(TopicTrackingInfo objTopicTracking);

		void DeleteTopicTracking(int topicTrackingId);

		#endregion

		#region Tracking

		int AddTracking(TrackingInfo objTracking);

		//TrackingInfo GetTracking(int trackingId);

		List<TrackingInfo> GetUsersTrackedForums(int userId);

		void UpdateTracking(TrackingInfo objTracking);

		void DeleteTracking(int trackingId);

		#endregion

		#region Url

		int AddUrl(UrlInfo objUrl);

		UrlInfo GetUrl(int id);

		List<UrlInfo> GetAllUrls(int portalId);

		void UpdateUrl(UrlInfo objUrl);

		void DeleteUrl(int id, int portalId);

		#endregion

		#region User

		int AddUser(UserInfo objUser);

		UserInfo GetUser(int portalId, int userId);

		void UpdateUser(UserInfo objUser);

		#endregion
	
	}
}