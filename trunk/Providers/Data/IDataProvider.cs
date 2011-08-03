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

using System.Data;
using System;

namespace DotNetNuke.Modules.Forums.Providers.Data
{

	/// <summary>
	/// An abstract class for the data access layer (Thus, this is our abstract data provider).
	/// </summary>
	public interface IDataProvider
	{

		#region Abstract methods

		#region Filter

		int AddFilter(int portalId, int moduleId, int forumId, string find, string replace, string filterType, bool applyOnSave, bool applyOnRender, DateTime createdOnDate);

		IDataReader GetFilter(int filterId);

		IDataReader GetAllFilters(int portalId);

		void UpdateFilter(int filterId, int portalId, int moduleId, int forumId, string find, string replace, string filterType, bool applyOnSave, bool applyOnRender, DateTime lastModifiedOnDate);

		void DeleteFilter(int filterId, int portalId);

		#endregion

		#region Forum
		#endregion

		#region Permission

		int AddPermission(string description, int portalId, string canView, string canRead, string caCreate, string canEdit, string canDelete, string canLock, string canPin, string canAttach, string canPoll, string canBlock, string canTrust, string canSubscribe, string canAnnounce, string canTag, string canPrioritize, string canModApprove, string canModMove, string canModSplit, string canModDelete, string canModUser, string canModModEdit, string canModLock, string canModPin);

		IDataReader GetPermission(int permissionId);

		IDataReader GetPortalPermissions(int portalId);

		void UpdatePermission(int permissionId, string description, int portalId, string canView, string canRead, string caCreate, string canEdit, string canDelete, string canLock, string canPin, string canAttach, string canPoll, string canBlock, string canTrust, string canSubscribe, string canAnnounce, string canTag, string canPrioritize, string canModApprove, string canModMove, string canModSplit, string canModDelete, string canModUser, string canModModEdit, string canModLock, string canModPin);

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

	    int AddPostAttachment(int postId, int fileId, string fileUrl, string fileName, bool displayInline);

	    IDataReader GetPostAttachment(int attachmentId);

	    IDataReader GetTopicAttachments(int topicId);

	    void UpdatePostAttachment(int attachmentId, int postId, int fileId, string fileUrl, string fileName, bool displayInline);

	    void DeletePostAttachment(int attachmentId, int postId);

		#endregion

		#region Post Rating

	    int AddPostRating(int postId, int userId, int rating, bool helpful, string comments, string ipAddress, DateTime createdOnDate);

	    IDataReader GetPostRating(int ratingId);

	    IDataReader GetTopicRatings(int topicId);

	    void UpdatePostRating(int ratingId, int postId, int userId, int rating, bool helpful, string comments, string ipAddress);

	    void DeletePostRating(int ratingId, int portalId);

		#endregion

		#region Rank

	    int AddRank(int portalId, int moduleId, string rankName, int minPosts, int maxPosts, string display, DateTime createdOnDate);

	    IDataReader GetRank(int rankId);

	    IDataReader GetModuleRank(int moduleId);

	    void UpdateRank(int rankId, int portalId, int moduleId, string rankName, int minPosts, int maxPosts, string display, DateTime lastModifiedOnDate);

	    void DeleteRank(int rankId, int portalId);

		#endregion

		#region Setting

		int AddSetting(string description, bool attachments, bool emoticons, bool html, bool postIcon, bool rss, bool scripts, bool moderated, int autoTrustLevel, int attachMaxCount, int attachMaxSize, bool attachAutoResize, int attachMaxHeight, int attachMaxWidth, int attachStore, string editorType, string editorHeight, string editorWidth, bool filters);

		IDataReader GetSetting(int permissionId);

		IDataReader GetAllSettings();

		void UpdateSetting(int settingId, string description, bool attachments, bool emoticons, bool html, bool postIcon, bool rss, bool scripts, bool moderated, int autoTrustLevel, int attachMaxCount, int attachMaxSize, bool attachAutoResize, int attachMaxHeight, int attachMaxWidth, int attachStore, string editorType, string editorHeight, string editorWidth, bool filters);

		void DeleteSetting(int settingId);

		#endregion

		#region Subscription

		int AddSubscription(int portalId, int moduleId, int forumId, int topicId, int subscriptionType, int userId);

		IDataReader GetSubscription(int subscriptionId);

		IDataReader GetTopicsSubscribers(int portalId, int moduleId, int forumId, int topicId);

		IDataReader GetUsersSubscriptions(int portalId, int userId);

		void UpdateSubscription(int subscriptionId, int portalId, int moduleId, int forumId, int topicId, int subscriptionType, int userId);

		void DeleteSubscription(int subscriptionId, int portalId);

		#endregion

		#region Topic
		#endregion

		#region Topic Tracking

		int AddTopicTracking(int forumId, int topicId, int lastPostId, int userId, DateTime createdOnDate);

		//IDataReader GetTopicTracking(int topicTrackingId);

		IDataReader GetTopicTrackingByForum(int forumId);

		IDataReader GetTopicTrackingByTopic(int topicId);

		void UpdateTopicTracking(int topicTrackingId, int forumId, int topicId, int lastPostId, int userId, DateTime lastModifiedOnDate);

		void DeleteTopicTracking(int topicTrackingId);

		#endregion

		#region Tracking
		// this is the same as user threads/user forums from the core forum

		int AddTracking(int forumId, int userId, int maxTopicRead, int maxPostRead, DateTime lastAccessedOnDate);

		//IDataReader GetTracking(int trackingId);

		IDataReader GetUsersTrackedForums(int userId);

		void UpdateTracking(int trackingId, int forumId, int userId, int maxTopicRead, int maxPostRead, DateTime lastAccessedOnDate);

		void DeleteTracking(int trackingId);

		#endregion

		#region Url

		int AddUrl(int portalId, int forumId, int topicId, string url, DateTime createdOnDate);

		IDataReader GetUrl(int id);

		IDataReader GetAllUrls(int portalId);

		void UpdateUrl(int id, int portalId, int forumId, int topicId, string url, DateTime lastModifiedOnDate);

		void DeleteUrl(int id, int portalId);

		#endregion

		#region User

		int AddUser(int portalId, int userId, int topicCount, int replyCount, int rewardPoints, int answerCount, int questionCount, int trustLevel, string userCaption, DateTime lastPostDate, DateTime lastActivityDate, bool adminWatch, bool disableAttach, bool disableHtml, DateTime createdOnDate);

		IDataReader GetUser(int portalId, int userId);

		void UpdateUser(int portalId, int userId, int topicCount, int replyCount, int rewardPoints, int answerCount, int questionCount, int trustLevel, string userCaption, DateTime lastPostDate, DateTime lastActivityDate, bool adminWatch, bool disableAttach, bool disableHtml, DateTime lastModifiedOnDate);

		#endregion

		#endregion

	}
}