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

		#region Project

		//public int AddProject(string title, string url, string description, int extensionType, string extendedDescription, string demoUrl, bool authorized, int groupId, int portalId, int createdByUserId, DateTime createdOnDate)
		//{
		//    return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("Project_Add"), title, GetNull(url), description, extensionType, extendedDescription, demoUrl, authorized, groupId, portalId, createdByUserId, createdOnDate));
		//}

		//public IDataReader GetProject(int projectId, int avatarPropDefinitionID)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Project_Get"), projectId, GetNull(avatarPropDefinitionID));
		//}

		//public IDataReader GetProjectByTitle(string title, int avatarPropDefinitionID)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Project_GetByTitle"), title, GetNull(avatarPropDefinitionID));
		//}

		//public IDataReader GetProjectByUrl(string codeplexUrl, int avatarPropDefinitionID)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Project_GetByUrl"), codeplexUrl, GetNull(avatarPropDefinitionID));
		//}

		//public IDataReader GetAuthorizedProjects(int portalID, int avatarPropDefinitionID)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Project_GetAuthorized"), portalID, GetNull(avatarPropDefinitionID));
		//}

		//public IDataReader GetUnauthorizedProjects(int portalID, int avatarPropDefinitionID)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Project_GetUnauthorized"), portalID, GetNull(avatarPropDefinitionID));
		//}

		//public IDataReader ProjectSearchByTypeContentTag(int extensionTypeID, string content, int portalId, string tag, int avatarPropDefinitionID)
		//{
		//    return SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("Project_SearchTypeContentTag"), GetNull(extensionTypeID), GetNull(content), portalId, GetNull(tag), GetNull(avatarPropDefinitionID));
		//}

		//public void UpdateProject(int projectId, int extensionType, string extendedDescription, string demoUrl, bool authorized, int groupId, int portalId, int primaryMediaId, int contentItemId, string wikiUrl, int lastModifiedUserId, DateTime lastModifiedDate, string projectEmail)
		//{
		//    SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Project_Update"), projectId, extensionType, extendedDescription, demoUrl, authorized, groupId, portalId, GetNull(primaryMediaId), contentItemId, wikiUrl, lastModifiedUserId, GetNull(lastModifiedDate), GetNull(projectEmail));
		//}

		//public void DeleteProject(int projectId, int portalId)
		//{
		//    SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("Project_Delete"), projectId, portalId);
		//}

		#endregion

		#endregion

	}
}