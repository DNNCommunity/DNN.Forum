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

namespace DotNetNuke.Modules.Forums.Components.Entities
{

	/// <summary>
	/// This is our Info class that represents columns in our data store that are associated with the Forums_Forum table. 
	/// </summary>
	public class ForumInfo
	{

		#region Public Properties

		/// <summary>
		/// Our PK field.
		/// </summary>
		public int ForumId { get; set; }

		public int PortalId { get; set; }

		public int ModuleId { get; set; }

		public int ParentId { get; set; }

		public bool AllowTopics { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>255 characters</remarks>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>nvarchar(MAX)</remarks>
		public string Description { get; set; }

		public int SortOrder { get; set; }

		public bool Active { get; set; }

		public bool Hidden { get; set; }

		public int TopicCount { get; set; }

		public int ReplyCount { get; set; }

		public int LastPostId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>100 characters</remarks>
		public string Slug { get; set; }

		public int PermissionId { get; set; }

		public int SettingId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>255 characters</remarks>
		public string EmailAddress { get; set; }

		public float SiteMapPriority { get; set; }

		public DateTime CreatedOnDate { get; set; }

		public int CreatedByUserId { get; set; }

		public DateTime LastModifiedOnDate { get; set; }

		public int LastModifiedByUserId { get; set; }

		/// <summary>
		/// A count used for paging.
		/// </summary>
		/// <remarks>Not stored in a table.</remarks>
		public int TotalRecords { get; set; }

		#endregion

	}
}