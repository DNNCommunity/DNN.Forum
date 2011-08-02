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

namespace DotNetNuke.Modules.Forums.Components.Entities
{

	/// <summary>
	/// This is our Info class that represents columns in our data store that are associated with the Forums_Permission table. 
	/// </summary>\
	/// <remarks>All strings have a character count of 1000 in the data store, unless noted otherwise.</remarks>
	public class PermissionInfo
	{

		#region Public Properties

		/// <summary>
		/// Our PK field.
		/// </summary>
		public int PermissionId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <rename>50 characters</rename>
		public string Description { get; set; }

		public int PortalId { get; set; }

		public string CanView { get; set; }

		public string CanRead { get; set; }

		public string CanCreate { get; set; }

		public string CanEdit { get; set; }

		public string CanDelete { get; set; }

		public string CanLock { get; set; }

		public string CanPin { get; set; }

		public string CanAttach { get; set; }

		public string CanPoll { get; set; }

		public string CanBlock { get; set; }

		public string CanTrust { get; set; }

		public string CanSubscribe { get; set; }

		public string CanAnnounce { get; set; }

		public string CanTag { get; set; }

		public string CanPrioritize { get; set; }

		public string CanModApprove { get; set; }

		public string CanModMove { get; set; }

		public string CanModSplit { get; set; }

		public string CanModDelete { get; set; }

		public string CanModUser { get; set; }

		public string CanModEdit { get; set; }

		public string CanModLock { get; set; }

		public string CanModPin { get; set; }

		#endregion

	}
}