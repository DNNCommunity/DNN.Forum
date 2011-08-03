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

using System;
using System.Data;
using DotNetNuke.Common.Utilities;
using Microsoft.ApplicationBlocks.Data;

namespace DotNetNuke.Modules.Forums.Providers.Data.SqlDataProvider
{

	/// <summary>
	/// SQL Server implementation (ie. concrete provider) of the abstract DataProvider class for the Forums module.
	/// </summary>
	public class SqlDataProvider : IDataProvider
	{

		#region Private Members

		//private const string ProviderType = "data";
		private const string ModuleQualifier = "Forums_";
		private string _connectionString = String.Empty;
		private string _databaseOwner = String.Empty;
		private string _objectQualifier = String.Empty;

		#endregion

		#region Properties

		public string ConnectionString
		{
			get {
				return string.IsNullOrEmpty(_connectionString) ? DotNetNuke.Data.DataProvider.Instance().ConnectionString : _connectionString;
			}
			set { _connectionString = value; }
		}

		public string DatabaseOwner
		{
			get {
				return string.IsNullOrEmpty(_databaseOwner) ? DotNetNuke.Data.DataProvider.Instance().DatabaseOwner : _databaseOwner;
			}
			set { _databaseOwner = value; }
		}

		public string ObjectQualifier
		{
			get {
				return string.IsNullOrEmpty(_objectQualifier) ? DotNetNuke.Data.DataProvider.Instance().ObjectQualifier : _objectQualifier;
			}
			set { _objectQualifier = value; }
		}

		#endregion

		#region Private Methods

		private static object GetNull(object field)
		{
			return Null.GetNull(field, DBNull.Value);
		}

		private string GetFullyQualifiedName(string name)
		{
			return DatabaseOwner + ObjectQualifier + ModuleQualifier + name;
		}

		#endregion

		#region Public Methods

		#region Filter

		public int AddFilter(int portalId, int moduleId, int forumId, string find, string replace, string filterType, bool applyOnSave, bool applyOnRender, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Filter_Add"), portalId, GetNull(moduleId), GetNull(forumId), find, replace, GetNull(filterType), applyOnSave, applyOnRender, createdOnDate));
		}

		public IDataReader GetFilter(int filterId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Filter_Get"), filterId);
		}

		public IDataReader GetAllFilters(int portalID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Filter_GetAll"), portalID);
		}

		public void UpdateFilter(int filterId, int portalId, int moduleId, int forumId, string find, string replace, string filterType, bool applyOnSave, bool applyOnRender, DateTime lastModifiedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Filter_Update"), filterId, portalId, GetNull(moduleId), GetNull(forumId), find, replace, GetNull(filterType), applyOnSave, applyOnRender, GetNull(lastModifiedOnDate));
		}

		public void DeleteFilter(int filterId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Filter_Delete"), filterId, portalId);
		}

		#endregion

		#region Forum

		public int AddForum(int portalId, int moduleId, int parentId, int allowTopics, string name, string description, int sortOrder, bool active, bool hidden, int topicCount, int replyCount, int lastPostId, string slug, int permissionId, int settingId, string emailAddress, float siteMapPriority, DateTime createdOnDate, int createdByUserId)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Forum_Add"), portalId, moduleId, parentId, allowTopics, name, GetNull(description), sortOrder, active, hidden, topicCount, replyCount, lastPostId, GetNull(slug), permissionId, settingId, GetNull(emailAddress), GetNull(siteMapPriority), createdOnDate, createdByUserId));
		}

		public IDataReader GetForum(int forumId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Forum_Get"), forumId);
		}

		public IDataReader GetModuleForums(int moduleId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Forum_GetModule"), moduleId);
		}

		public void UpdateForum(int forumId, int portalId, int moduleId, int parentId, int allowTopics, string name, string description, int sortOrder, bool active, bool hidden, int topicCount, int replyCount, int lastPostId, string slug, int permissionId, int settingId, string emailAddress, float siteMapPriority, DateTime lastModifiedOnDate, int lastModifiedByUserId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Forum_Update"), forumId, portalId, moduleId, parentId, allowTopics, name, GetNull(description), sortOrder, active, hidden, topicCount, replyCount, lastPostId, GetNull(slug), permissionId, settingId, GetNull(emailAddress), GetNull(siteMapPriority), lastModifiedOnDate, lastModifiedByUserId);
		}

		public void DeleteForum(int forumId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Forum_Delete"), forumId, portalId);
		}

		#endregion

		#region Permission

		public int AddPermission(string description, int portalId, string canView, string canRead, string canCreate, string canEdit, string canDelete, string canLock, string canPin, string canAttach, string canPoll, string canBlock, string canTrust, string canSubscribe, string canAnnounce, string canTag, string canPrioritize, string canModApprove, string canModMove, string canModSplit, string canModDelete, string canModUser, string canModEdit, string canModLock, string canModPin)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Permission_Add"), GetNull(description), portalId, canView, canRead, canCreate, canEdit, canDelete, canLock, canPin, canAttach, canPoll, canBlock, canTrust, canSubscribe, canAnnounce, canTag, canPrioritize, canModApprove, canModMove, canModSplit, canModDelete, canModUser, canModEdit, canModLock, canModPin));
		}

		public IDataReader GetPermission(int permissionId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Permission_Get"), permissionId);
		}

		public IDataReader GetPortalPermissions(int portalID)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Permission_GetPortal"), portalID);
		}

		public void UpdatePermission(int permissionId, string description, int portalId, string canView, string canRead, string canCreate, string canEdit, string canDelete, string canLock, string canPin, string canAttach, string canPoll, string canBlock, string canTrust, string canSubscribe, string canAnnounce, string canTag, string canPrioritize, string canModApprove, string canModMove, string canModSplit, string canModDelete, string canModUser, string canModEdit, string canModLock, string canModPin)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Permission_Update"), GetNull(description), portalId, canView, canRead, canCreate, canEdit, canDelete, canLock, canPin, canAttach, canPoll, canBlock, canTrust, canSubscribe, canAnnounce, canTag, canPrioritize, canModApprove, canModMove, canModSplit, canModDelete, canModUser, canModEdit, canModLock, canModPin);
		}

		public void DeletePermission(int permissionId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Permission_Delete"), permissionId, portalId);
		}

		#endregion

		#region Poll

		public int AddPoll(int topicId, int userId, string question, string pollType, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Poll_Add"), topicId, userId, GetNull(question), GetNull(pollType), createdOnDate));
		}

		// I don't think we need this
		//public IDataReader GetPoll(int pollId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Poll_Get"), pollId);
		//}

		public IDataReader GetPollByTopic(int topicId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Poll_GetByTopic"), topicId);
		}

		public void UpdatePoll(int pollId, int topicId, int userId, string question, string pollType, DateTime lastModifiedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Poll_Update"), pollId, topicId, userId, GetNull(question), GetNull(pollType), lastModifiedOnDate);
		}

		public void DeletePoll(int pollId, int topicId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Poll_Delete"), pollId, topicId);
		}

		#endregion

		#region Poll Option

		public int AddPollOption(int pollId, string optionName, int priority, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Poll_Option_Add"), pollId, optionName, priority, createdOnDate));
		}

		// I don't think we need this
		//public IDataReader GetPollOption(int pollOptionId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Poll_Option_Get"), pollOptionId);
		//}

		public IDataReader GetPollOptions(int pollId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Poll_Option_GetPoll"), pollId);
		}

		public void UpdatePollOption(int pollOptionId, int pollId, string optionName, int priority, DateTime lastModifiedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Poll_Option_Update"), pollOptionId, pollId, optionName, priority,  lastModifiedOnDate);
		}

		public void DeletePollOption(int pollOptionId, int pollId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Poll_Option_Delete"), pollOptionId, pollId);
		}

		#endregion

		#region Poll Result

		public int AddPollResult(int pollId, int pollOptionId, string response, string ipaddress, int userId, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Poll_Result_Add"), pollId, pollOptionId, GetNull(response), GetNull(ipaddress), GetNull(userId), createdOnDate));
		}

		// I don't think we need this
		//public IDataReader GetPollResult(int pollResultId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Poll_Result_Get"), pollResultId);
		//}

		public IDataReader GetPollResults(int pollId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Poll_Result_GetPoll"), pollId);
		}

		public void UpdatePollResult(int pollResultId, int pollId, int pollOptionId, string response, string ipaddress, int userId, DateTime lastModifiedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Poll_Result_Update"), pollResultId, pollId, pollOptionId, GetNull(response), GetNull(ipaddress), GetNull(userId), lastModifiedOnDate);
		}

		public void DeletePollResult(int pollResultId, int pollId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Poll_Result_Delete"), pollResultId, pollId);
		}

		#endregion

		#region Post

		public int AddPost(int topicId, int parentPostId, string subject, string body, bool isApproved, bool isLocked, bool isPinned, int userId, string displayName, string emailAddress, string ipAddress, bool postReported, float rating, string postIcon, int statusId, string slug, DateTime approvedOnDate, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Post_Add"), topicId, parentPostId, subject, body, isApproved, isLocked, isPinned, userId, GetNull(displayName), GetNull(emailAddress), GetNull(ipAddress), postReported, rating, GetNull(postIcon), GetNull(statusId), GetNull(slug), GetNull(approvedOnDate), createdOnDate));
		}

		public IDataReader GetPost(int postId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_Get"), postId);
		}

		public IDataReader GetTopicPosts(int topicId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_GetTopic"), topicId);
		}

		public void UpdatePost(int postId, int topicId, int parentPostId, string subject, string body, bool isApproved, bool isLocked, bool isPinned, string displayName, string emailAddress, bool postReported, float rating, string postIcon, int statusId, string slug, DateTime approvedOnDate, DateTime lastModifiedDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_Update"), postId, topicId, parentPostId, subject, body, isApproved, isLocked, isPinned, GetNull(displayName), GetNull(emailAddress), postReported, rating, GetNull(postIcon), GetNull(statusId), GetNull(slug), GetNull(approvedOnDate), lastModifiedDate);

		}

		public void DeletePost(int postId, int topicId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_Delete"), postId, topicId);
		}

		public void HardDeletePost(int postId, int topicId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_HardDelete"), postId, topicId);
		}

		#endregion

		#region Post Attachment

		public int AddPostAttachment(int postId, int fileId, string fileUrl, string fileName, bool displayInline)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Post_Attachment_Add"), postId, fileId, GetNull(fileUrl), GetNull(fileName), displayInline));
		}

		// I don't think we need this
		//public IDataReader GetPostAttachment(int attachmentId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_Attachment_Get"), attachmentId);
		//}

		/// <summary>
		/// Retrieves a collection of post attachments assigned to a topic from the data store. 
		/// </summary>
		/// <param name="topicId"></param>
		/// <returns></returns>
		/// <remarks>It seems like it would be more efficient to get all post attachments for posts in a single topic. You can then cache this and query via linq (handle in sproc.).</remarks>
		public IDataReader GetTopicAttachments(int topicId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Post_Attachment_GetTopic"), topicId);
		}

		public void UpdatePostAttachment(int attachmentId, int postId, int fileId, string fileUrl, string fileName, bool displayInline)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_Attachment_Update"), attachmentId, postId, fileId, GetNull(fileUrl), GetNull(fileName), displayInline);
		}

		public void DeletePostAttachment(int attachmentId, int postId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Post_Attachment_Delete"), attachmentId, postId);
		}

		#endregion

		#region Post Rating

		/// <summary>
		/// Adds a post rating (for a specific user and post) row to the data store.
		/// </summary>
		/// <param name="postId"></param>
		/// <param name="userId"></param>
		/// <param name="rating"></param>
		/// <param name="helpful"></param>
		/// <param name="comments"></param>
		/// <param name="ipAddress"></param>
		/// <param name="createdOnDate"></param>
		/// <returns></returns>
		public int AddPostRating(int postId, int userId, int rating, bool helpful, string comments, string ipAddress, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Rating_Add"), postId, userId, rating, GetNull(helpful), GetNull(comments), GetNull(ipAddress), createdOnDate));
		}

		// I don't think we need this
		//public IDataReader GetPostRating(int ratingId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Rating_GetPost"), ratingId);
		//}

		/// <summary>
		/// Retrieves a collection of post ratings assigned to a topic from the data store. 
		/// </summary>
		/// <param name="topicId"></param>
		/// <returns></returns>
		/// <remarks>It seems like it would be more efficient to get all post ratings for posts in a single topic. You could then cache this and query via linq (handle in sproc.).</remarks>
		public IDataReader GetTopicRatings(int topicId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Rating_GetTopic"), topicId);
		}

		/// <summary>
		/// Updates a post rating row in the data store. 
		/// </summary>
		/// <param name="ratingId"></param>
		/// <param name="postId"></param>
		/// <param name="userId"></param>
		/// <param name="rating"></param>
		/// <param name="helpful"></param>
		/// <param name="comments"></param>
		/// <param name="ipAddress"></param>
		public void UpdatePostRating(int ratingId, int postId, int userId, int rating, bool helpful, string comments, string ipAddress)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Rating_Update"), ratingId, postId, userId, rating, GetNull(helpful), GetNull(comments), GetNull(ipAddress));
		}

		/// <summary>
		/// Removes a post rating row from the data store. 
		/// </summary>
		/// <param name="ratingId"></param>
		/// <param name="portalId"></param>
		public void DeletePostRating(int ratingId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Rating_Delete"), ratingId, portalId);
		}

		#endregion

		#region Rank

		public int AddRank(int portalId, int moduleId, string rankName, int minPosts, int maxPosts, string display, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Rank_Add"), portalId, moduleId, rankName, minPosts, maxPosts, GetNull(display), createdOnDate));
		}

		public IDataReader GetRank(int rankId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Rank_Get"), rankId);
		}

		public IDataReader GetModuleRank(int moduleId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Rank_GetModule"), moduleId);
		}

		public void UpdateRank(int rankId, int portalId, int moduleId, string rankName, int minPosts, int maxPosts, string display, DateTime lastModifiedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Rank_Update"), rankId, portalId, moduleId, rankName, minPosts, maxPosts, GetNull(display), lastModifiedOnDate);
		}

		public void DeleteRank(int rankId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Rank_Delete"), rankId, portalId);
		}

		#endregion

		#region Setting

		public int AddSetting(string description, bool attachments, bool emoticons, bool html, bool postIcon, bool rss, bool scripts, bool moderated, int autoTrustLevel, int attachMaxCount, int attachMaxSize, bool attachAutoResize, int attachMaxHeight, int attachMaxWidth, int attachStore, string editorType, string editorHeight, string editorWidth, bool filters)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Setting_Add"), GetNull(description), attachments, emoticons, html, postIcon, rss, scripts, moderated, autoTrustLevel, attachMaxCount, attachMaxSize, attachAutoResize, attachMaxHeight, attachMaxWidth, attachStore, GetNull(editorType), GetNull(editorHeight), GetNull(editorWidth), GetNull(filters)));
		}

		public IDataReader GetSetting(int settingId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Setting_Get"), settingId);
		}

		public IDataReader GetAllSettings()
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Setting_GetAll"));
		}

		public void UpdateSetting(int settingId, string description, bool attachments, bool emoticons, bool html, bool postIcon, bool rss, bool scripts, bool moderated, int autoTrustLevel, int attachMaxCount, int attachMaxSize, bool attachAutoResize, int attachMaxHeight, int attachMaxWidth, int attachStore, string editorType, string editorHeight, string editorWidth, bool filters)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Setting_Update"), settingId, GetNull(description), attachments, emoticons, html, postIcon, rss, scripts, moderated, autoTrustLevel, attachMaxCount, attachMaxSize, attachAutoResize, attachMaxHeight, attachMaxWidth, attachStore, GetNull(editorType), GetNull(editorHeight), GetNull(editorWidth), GetNull(filters));
		}

		public void DeleteSetting(int settingId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Setting_Delete"), settingId);
		}

		#endregion

		#region Subscription

		public int AddSubscription(int portalId, int moduleId, int forumId, int topicId, int subscriptionType, int userId)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Subscription_Add"), portalId, moduleId, forumId, topicId, subscriptionType, userId));
		}

		public IDataReader GetSubscription(int subscriptionId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Subscription_Get"), subscriptionId);
		}

		public IDataReader GetTopicsSubscribers(int portalId, int moduleId, int forumId, int topicId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Subscription_GetTopicSubscribers"), portalId, moduleId, forumId, topicId);
		}

		public IDataReader GetUsersSubscriptions(int portalId, int userId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Subscription_GetUsers"), portalId, userId);
		}

		public void UpdateSubscription(int subscriptionId, int portalId, int moduleId, int forumId, int topicId, int subscriptionType, int userId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Subscription_Update"), subscriptionId, portalId, moduleId, forumId, topicId, subscriptionType, userId);
		}

		public void DeleteSubscription(int subscriptionId, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Subscription_Delete"), subscriptionId, portalId);
		}

		#endregion

		#region Topic

		public int AddTopic(int forumId, int viewCount, int replyCount, int topicTypeId, int lastPostId, string slug)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Topic_Add"), forumId, viewCount, replyCount, topicTypeId, lastPostId, GetNull(slug)));
		}

		public IDataReader GetTopic(int topicId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Topic_Get"), topicId);
		}

		public IDataReader GetForumTopics(int forumId, int pageIndex, int pageSize)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Topic_GetForum"), forumId, pageIndex, pageSize);
		}

		////We need a way to get all topics in a module that a user can see, for the aggregated view. We should only cache this for anonymous users.
		//public IDataReader GetAllModuleTopics(int moduleId, int pageIndex, int pageSize)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Topic_GetAllModule"), moduleId, pageIndex, pageSize);
		//}

		public void UpdateTopic(int topicId, int forumId, int viewCount, int replyCount, int topicTypeId, int lastPostId, string slug, int contentItemId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Topic_Update"), topicId, forumId, viewCount, replyCount, topicTypeId, lastPostId, GetNull(slug), contentItemId);
		}

		public void DeleteTopic(int topicId, int forumId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Topic_Delete"), topicId, forumId);
		}

		#endregion

		#region Topic Tracking

		public int AddTopicTracking(int forumId, int topicId, int lastPostId, int userId, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Topic_Tracking_Add"), forumId, topicId, lastPostId, userId, createdOnDate));
		}

		//public IDataReader GetTopicTracking(int topicTrackingId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Topic_Tracking_Get"), topicTrackingId);
		//}


		public IDataReader GetTopicTrackingByForum(int forumId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Topic_Tracking_GetByForum"), forumId);
		}

		public IDataReader GetTopicTrackingByTopic(int topicId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Topic_Tracking_GetByTopic"), topicId);
		}

		public void UpdateTopicTracking(int topicTrackingId, int forumId, int topicId, int lastPostId, int userId, DateTime lastModifiedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Topic_Tracking_Update"), topicTrackingId, forumId, topicId, lastPostId, userId, lastModifiedOnDate);
		}

		public void DeleteTopicTracking(int topicTrackingId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Topic_Tracking_Delete"), topicTrackingId);
		}

		#endregion

		#region Tracking
		// this is the same as user threads/user forums from the core forum

		public int AddTracking(int forumId, int userId, int maxTopicRead, int maxPostRead, DateTime lastAccessedOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Tracking_Add"), forumId, userId, maxTopicRead, maxPostRead, lastAccessedOnDate));
		}

		// I don't think we need this
		//public IDataReader GetTracking(int trackingId)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Tracking_Get"), trackingId);
		//}

		public IDataReader GetUsersTrackedForums(int userId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Tracking_GetUsersForums"), userId);
		}

		public void UpdateTracking(int trackingId, int forumId, int userId, int maxTopicRead, int maxPostRead, DateTime lastAccessedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Tracking_Update"), trackingId, forumId, userId, maxTopicRead, maxPostRead, lastAccessedOnDate);
		}

		public void DeleteTracking(int trackingId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Tracking_Delete"), trackingId);
		}

		#endregion

		#region Url

		public int AddUrl(int portalId, int forumId, int topicId, string url, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Url_Add"), portalId, forumId, topicId, url, createdOnDate));
		}

		public IDataReader GetUrl(int id)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Url_Get"), id);
		}

		public IDataReader GetAllUrls(int portalId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Url_GetAll"), portalId);
		}

		public void UpdateUrl(int id, int portalId, int forumId, int topicId, string url, DateTime lastModifiedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Url_Update"), id, portalId, forumId, topicId, url, lastModifiedOnDate);
		}

		public void DeleteUrl(int id, int portalId)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Url_Delete"), id, portalId);
		}

		#endregion

		#region User

		public int AddUser(int portalId, int userId, int topicCount, int replyCount, int rewardPoints, int answerCount, int questionCount, int trustLevel, string userCaption, DateTime lastPostDate, DateTime lastActivityDate, bool adminWatch, bool disableAttach, bool disableHtml, DateTime createdOnDate)
		{
			return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("User_Add"), portalId, userId, topicCount, replyCount, rewardPoints, answerCount, questionCount, trustLevel, userCaption, lastPostDate, lastActivityDate, adminWatch, disableAttach, disableHtml, createdOnDate));
		}

		public IDataReader GetUser(int portalId, int userId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("User_Get"), portalId, userId);
		}

		public void UpdateUser(int portalId, int userId, int topicCount, int replyCount, int rewardPoints, int answerCount, int questionCount, int trustLevel, string userCaption, DateTime lastPostDate, DateTime lastActivityDate, bool adminWatch, bool disableAttach, bool disableHtml, DateTime lastModifiedOnDate)
		{
			SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("User_Update"), portalId, userId, topicCount, replyCount, rewardPoints, answerCount, questionCount, trustLevel, userCaption, lastPostDate, lastActivityDate, adminWatch, disableAttach, disableHtml, lastModifiedOnDate);
		}

		#endregion
		
		#endregion

	}
}