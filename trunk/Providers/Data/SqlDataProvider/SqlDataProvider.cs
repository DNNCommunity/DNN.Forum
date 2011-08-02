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
		#endregion

		#region Subscription
		#endregion

		#region Topic
		#endregion

		#region Topic_Tracking
		#endregion

		#region Tracking
		#endregion

		#region Url
		#endregion

		#region User
		#endregion
		
		#endregion

	}
}