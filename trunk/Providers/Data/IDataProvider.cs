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

		#region Project

		//int AddProject(string title, string url, string description, int extensionType, string extendedDescription, string demoUrl, bool authorized, int groupId, int portalId, int createdByUserId, DateTime createdOnDate);   
   
		//IDataReader GetProject(int projectId, int avatarPropDefinitionID);

		//IDataReader GetProjectByTitle(string title, int avatarPropDefinitionID);

		//IDataReader GetProjectByUrl(string codeplexUrl, int avatarPropDefinitionID);    

		//IDataReader GetAuthorizedProjects(int portalId, int avatarPropDefinitionID);

		//IDataReader GetUnauthorizedProjects(int portalID, int avatarPropDefinitionID);

		//IDataReader ProjectSearchByTypeContentTag(int extensionTypeID, string content, int portalId, string tag, int avatarPropDefinitionID);

		//void UpdateProject(int projectId, int extensionType, string extendedDescription, string demoUrl, bool authorized, int groupId, int portalId, int primaryMediaId, int contentItemId, string wikiUrl, int lastModifiedUserId, DateTime lastModifiedDate, string projectEmail);	

		//void DeleteProject(int projectID, int portalID);

		#endregion

		#endregion

	}
}