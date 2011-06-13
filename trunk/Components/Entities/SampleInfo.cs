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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content;

namespace DotNetNuke.Modules.Forums.Components.Entities
{

	/// <summary>
	/// This is our Info class that represents columns in our data store that are associated with the )Sample table. 
	/// </summary>
	public class SampleInfo : ContentItem
	{

		#region Public Properties

		/// <summary>
		/// Our PK to identify a project. This is used along with our affiliate ID to synchronize projects (so this is all we send CodePlex that is unique). 
		/// </summary>
		/// <remarks>Created by us, stored @ CodePlex for matching up.</remarks>
		public int ProjectId { get; set; }

		/// <summary>
		/// The title of the project. Ex: DotNetNuke® Community Edition
		/// </summary>
		/// <remarks>This comes from CodePlex.</remarks>
		public string Title { get; set; }

		/// <summary>
		/// A direct URL to view the project home page @ CodePlex. Ex: http://dotnetnuke.codeplex.com. 
		/// </summary>
		/// <remarks>This comes from CodePlex, constructed as title.codeplex.com in our synch task.</remarks>
		public string Url { get; set; }

		/// <summary>
		/// A brief summary of the project. 
		/// </summary>
		/// <remarks>This comes from CodePlex and is limited to 280 characters.</remarks>
		public string Description { get; set; }

		/// <summary>
		/// The recommended project version for download as specified by CodePlex. 
		/// </summary>
		/// <remarks>This does NOT come from CodePlex directly, but we use business logic in our synch task to determine an association.</remarks>
		public int RecProjectVersionId { get; set; }

		/// <summary>
		/// This is the total number of downloads the project has had (its entire lifetime). 
		/// </summary>
		/// <remarks>This comes from CodePlex. We use our project history to determine how many occurred any given day (this could be off if our task didn't run one day, thus resulting in 0 one day and double the count the following day). </remarks>
		public int Downloads { get; set; }

		/// <summary>
		/// The license the project uses. Ex: The MIT License (MIT)
		/// </summary>
		/// <remarks>This comes from CodePlex. Keep in mind, we do not get the actual license text from them anywhere.</remarks>
		public string License { get; set; }

		/// <summary>
		/// The total number of work items closed @ codeplex for the project. We currently do not use this anywhere. 
		/// </summary>
		/// <remarks>This comes from CodePlex.</remarks>
		public int WorkItemsClosed { get; set; }

		/// <summary>
		/// This is the date we last synchronized the project with CodePlex. 
		/// </summary>
		public DateTime LastSynchDate { get; set; }

		/// <summary>
		/// This uses the constant enumerator based on the core package types. 
		/// </summary>
		public int ExtensionType { get; set; }

		/// <summary>
		/// This is an area we allow people to store HTML (not wiki markdown). 
		/// </summary>
		public string ExtendedDescription { get; set; }

		/// <summary>
		/// A full URL to view a demo. Ex: http://www.dnnforums.com
		/// </summary>
		/// <remarks>Not sure this will be shown in initial version.</remarks>
		public string DemoUrl { get; set; }

		/// <summary>
		/// Determines if a project is authorized for viewing by people outside of the project team. 
		/// </summary>
		/// <remarks>Our requirement for this is just having the project have at least 1 release.</remarks>
		public bool Authorized { get; set; }

		/// <summary>
		/// The group PK associated with the project.
		/// </summary>
		/// <remarks>This is not currently being used and is reserved for future integration.</remarks>
		public int GroupId { get; set; }

		/// <summary>
		/// The portal the project is associated with. 
		/// </summary>
		/// <remarks>For population purposes locally, this is hard coded to 0 and would need to be udpated in the db directly 1x.</remarks>
		public int PortalId { get; set; }

		/// <summary>
		/// This is the image to display as the primary screenshot for the project. (If not set, we determine this on our own or use a default if no media exists). 
		/// </summary>
		/// <remarks>No association with CodePlex. We currently only support images but more may appear in the future.</remarks>
		public int PrimaryMediaID { get; set; }

		/// <summary>
		/// A direct link to a full size image (or media item in the future) based on the primary media id. 
		/// </summary>
		/// <remarks>Not associated with CodePlex and not stored as a column in our schema. Via a join we get foldername + filename. When displaying throughout module:
		/// ModuleContext.PortalSettings.HomeDirectory (/Portals/25/ in prod, usually /virdir/Portals/0/ locally) + this property If primarymediaid > 0. 
		/// Otherwise, we use resolveurl on the constant default project image.
		/// </remarks>
		public string PrimaryMediaLink{ get; set; }

		/// <summary>
		/// A direct link to a wiki page. Ex: http://www.dotnetnuke.com/Resources/Wiki/tabid/1409/Page/Getting-Started/Default.aspx
		/// </summary>
		public string WikiUrl { get; set; }

		/// <summary>
		/// The person who created the project.
		/// </summary>
		public int CreatedUserId { get; set; }

		/// <summary>
		/// The date the project was created.
		/// </summary>
		public DateTime CreatedDate { get; set; }
		
		/// <summary>
		/// The last person to modify a project.
		/// </summary>
		/// <remarks>The synch task does not update this.</remarks>
		public int LastModifiedUserId { get; set; }

		/// <summary>
		/// The last person to modify the project. 
		/// </summary>
		/// <remarks>The synch task does not update this.</remarks>
		public DateTime LastModifiedDate { get; set; }

		/// <summary>
		/// The email address for the project.
		/// </summary>
		/// <remarks>Added for appgallery (March 24th, 2011) due to user profile privacy concerns.</remarks>
		public string ProjectEmail { get; set; }

		/// <summary>
		/// The profile property definition ID associated with the 'photo' user profile property. This is set via module settings and is necessary here for lookups for project members (internally). 
		/// If this is not available to you, -1 is fine.
		/// </summary>
		/// <remarks>Added April 6th, 2011</remarks>
		public int AvatarPropDefinitionID { get; set; }

		#region via Versions join

		public string RecMinDnnVersion { get; set; }

		public int RecReleaseId { get; set; }

		public string RecReleaseName { get; set; }

		public string RecReleaseDevStatus { get; set; }

		public string RecReleaseNotes { get; set; }

		public DateTime RecReleaseDate { get; set; }

		public int RecReleaseFileId { get; set; }

		public string RecReleaseFileLink { get; set; }

		public string RecReleaseFileName { get; set; }

		public string RecExtensionVersion { get; set; }

		#endregion

		/// <summary>
		/// A count used for paging.
		/// </summary>
		/// <remarks>Not stored in a table.</remarks>
		public int TotalRecords { get; set; }

		#endregion

		#region IHydratable Implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dr"></param>
		public override void Fill(System.Data.IDataReader dr) {
			base.FillInternal(dr);

			ProjectId = Null.SetNullInteger(dr["ProjectID"]);
			Title = Null.SetNullString(dr["Title"]);
			Url = Null.SetNullString(dr["URL"]);
			Description = Null.SetNullString(dr["Description"]);
			RecProjectVersionId = Null.SetNullInteger(dr["RecProjectVersionID"]);
			Downloads = Null.SetNullInteger(dr["Downloads"]);
			License = Null.SetNullString(dr["License"]);
			WorkItemsClosed = Null.SetNullInteger(dr["WorkItemsClosed"]);
			LastSynchDate = Null.SetNullDateTime(dr["LastSynchDate"]);
			ExtensionType = Null.SetNullInteger(dr["ExtensionType"]);
			ExtendedDescription = Null.SetNullString(dr["ExtendedDescription"]);
			DemoUrl = Null.SetNullString(dr["DemoURL"]);
			Authorized = Null.SetNullBoolean(dr["Authorized"]);
			GroupId = Null.SetNullInteger(dr["GroupID"]);
			PortalId = Null.SetNullInteger(dr["PortalID"]);
			PrimaryMediaID = Null.SetNullInteger(dr["PrimaryMediaID"]);
			PrimaryMediaLink = Null.SetNullString(dr["PrimaryMediaLink"]);
			WikiUrl = Null.SetNullString(dr["WikiUrl"]);
			CreatedUserId = Null.SetNullInteger(dr["CreatedUserID"]);
			CreatedDate = Null.SetNullDateTime(dr["CreatedDate"]);
			LastModifiedUserId = Null.SetNullInteger(dr["LastModifiedUserID"]);
			LastModifiedDate = Null.SetNullDateTime(dr["LastModifiedDate"]);
			ProjectEmail = Null.SetNullString(dr["ProjectEmail"]);
			AvatarPropDefinitionID = Null.SetNullInteger(dr["AvatarPropDefinitionID"]);
			// version related
			RecMinDnnVersion = Null.SetNullString(dr["RecMinDnnVersion"]);
			RecReleaseId = Null.SetNullInteger(dr["RecReleaseID"]);
			RecReleaseName = Null.SetNullString(dr["RecReleaseName"]);
			RecReleaseDevStatus = Null.SetNullString(dr["RecReleaseDevStatus"]);
			RecReleaseNotes = Null.SetNullString(dr["RecReleaseNotes"]);
			RecReleaseDate = Null.SetNullDateTime(dr["RecReleaseDate"]);
			RecReleaseFileId = Null.SetNullInteger(dr["RecReleaseFileID"]);
			RecReleaseFileName = Null.SetNullString(dr["RecReleaseFileName"]);
			RecReleaseFileLink = Null.SetNullString(dr["RecReleaseFileLink"]);
			RecExtensionVersion = Null.SetNullString(dr["RecExtensionVersion"]);
			TotalRecords = Null.SetNullInteger(dr["TotalRecords"]);
		}

		/// <summary>
		/// Gets and sets the Key ID
		/// </summary>
		/// <returns>An Integer</returns>
		public override int KeyID {
			get { return ProjectId; }
			set { ProjectId = value; }
		}

		#endregion

	}
}