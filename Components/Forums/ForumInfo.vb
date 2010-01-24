'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2010
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Option Strict On
Option Explicit On

Namespace DotNetNuke.Modules.Forum

#Region "ForumInfo"

	''' <summary>
	''' The forum Info object uses caching and handles the population of all 
	''' data specific to a single forum instance.  Rendering for Group view 
	''' items is also handled here.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class ForumInfo

#Region "Private Members"

		Private Const ForumInfoCacheKeyPrefix As String = "ForumInfo"
		Private Const ForumInfoCacheTimeout As Integer = 20

		Dim _GroupID As Integer
		Dim _ModuleID As Integer
		Dim _ForumID As Integer
		Dim _IsActive As Boolean = True
		Dim _ParentID As Integer
		Dim _Name As String = Null.NullString
		Dim _Description As String = Null.NullString
		Dim _CreatedDate As DateTime = Null.NullDate
		Dim _CreatedByUser As Integer
		Dim _IsModerated As Boolean = False
		Dim _SortOrder As Integer
		Dim _ForumType As Integer
		Dim _IsIntegrated As Boolean
		Dim _IntegratedModuleID As Integer
		Dim _IntegratedObjects As String
		Dim _UpdatedDate As DateTime
		Dim _UpdatedByUser As Integer
		Dim _ForumPermissions As DotNetNuke.Modules.Forum.ForumPermissionCollection
		Dim _EnablePostStatistics As Boolean = True
		Dim _PublicPosting As Boolean = True
		Dim _EnableForumsThreadStatus As Boolean = True
		Dim _EnableForumsRating As Boolean = True
		Dim _ForumLink As String
		Dim _ForumBehavior As DotNetNuke.Modules.Forum.ForumBehavior
		Dim _EnableRSS As Boolean = True
		Dim _AllowPolls As Boolean = False
		Dim _SubForums As Integer
		Dim _DisablePostCount As Boolean = False
		'Anytime a post is added, these values can change
		Dim _MostRecentPostID As Integer
		Dim _MostRecentThreadID As Integer
		Dim _MostRecentPostAuthorID As Integer
		Dim _MostRecentPostDate As DateTime = Null.NullDate
		Dim _MostRecentThreadPinned As Boolean = False
		Dim _PostsToModerate As Integer
		Dim _TotalPosts As Integer
		Dim _TotalThreads As Integer
		Dim _PortalID As Integer
		' Email
		Dim _EmailAddress As String
		Dim _EmailFriendlyFrom As String
		Dim _NotifyByDefault As Boolean = False
		Dim _EmailStatusChange As Boolean = False
		Dim _EmailServer As String
		Dim _EmailUser As String
		Dim _EmailPass As String
		Dim _EmailEnableSSL As Boolean = False
		Dim _EmailAuth As Integer = 0	' 0 = none (for now, eventually need an enum)
		Dim _EmailPort As Integer = 110 ' 995 also used for gmail

#End Region

#Region "Constructors"

		''' <summary>
		''' Creates a new instance of the foruminfo object.
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	11/26/2005	Created
		''' </history>
		Public Sub New()
		End Sub

		''' <summary>
		''' Creates a new instance of a foruminfo object based on its ID.
		''' </summary>
		''' <param name="ForumID"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	11/26/2005	Created
		''' </history>
		Private Sub New(ByVal ForumID As Integer)
		End Sub

#End Region

#Region "Public ReadOnly Properties"

		''' <summary>
		''' Boolean information to identify if SubForums > 0
		''' </summary>
		''' <value></value>
		''' <returns>True/False</returns>
		''' <remarks>Added by Skeel</remarks>
		Public ReadOnly Property IsParentForum() As Boolean
			Get
				If SubForums > 0 Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property

		''' <summary>
		''' Determines if the forum is moderated/unmoderated. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property IsModerated() As Boolean
			Get

				If (ForumBehavior = ForumBehavior.PrivateModerated Or ForumBehavior = ForumBehavior.PrivateModeratedPostRestricted Or ForumBehavior = ForumBehavior.PublicModerated Or ForumBehavior = ForumBehavior.PublicModeratedPostRestricted) Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property

		''' <summary>
		''' If the forum is private/public viewing
		''' </summary>
		''' <value></value>
		''' <returns>True if the forum allows public viewing, false otherwise.</returns>
		''' <remarks>If public, the access limits are based on the module's view permissions. Uses Enum to match saved integer value.</remarks>
		Public ReadOnly Property PublicView() As Boolean
			Get

				If (ForumBehavior = ForumBehavior.PrivateModerated Or ForumBehavior = ForumBehavior.PrivateModeratedPostRestricted Or ForumBehavior = ForumBehavior.PrivateUnModerated Or ForumBehavior = ForumBehavior.PrivateUnModeratedPostRestricted) Then
					Return False
				Else
					Return True
				End If
			End Get
		End Property

		''' <summary>
		''' If public posting is permitted. If not, there are posting restrictions set by roles. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PublicPosting() As Boolean
			Get
				If (ForumBehavior = ForumBehavior.PrivateModeratedPostRestricted Or ForumBehavior = ForumBehavior.PrivateUnModeratedPostRestricted Or ForumBehavior = ForumBehavior.PublicModeratedPostRestricted Or ForumBehavior = ForumBehavior.PublicUnModeratedPostRestricted) Then
					Return False
				Else
					Return True
				End If
			End Get
		End Property

		''' <summary>
		''' The PortalID the forum belongs too. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PortalID() As Integer
			Get
				Return ParentGroup.PortalID
			End Get
		End Property

		''' <summary>
		''' The group information which contains this forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ParentGroup() As GroupInfo
			Get
				If GroupID <> -1 Then
					Return GroupInfo.GetGroupInfo(GroupID)
				Else
					Dim _groupInfo As GroupInfo = New GroupInfo
					_groupInfo.ModuleID = ModuleID
					_groupInfo.GroupID = GroupID
					Return _groupInfo
				End If
			End Get
		End Property

		''' <summary>
		''' Attaches an instance of a parent forum object to the current forum. 
		''' </summary>
		''' <value></value>
		''' <returns>The parent forum object if ParentID > 0, otherwise nothing is returned</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ParentForum() As ForumInfo
			Get
				Dim objForum As ForumInfo = New ForumInfo

				If ParentId > 0 Then
					Dim cntForum As New ForumController
					objForum = cntForum.GetForumInfoCache(ParentId)
				Else
					objForum.ModuleID = ModuleID
					objForum.ForumID = ForumID
				End If

				Return objForum
			End Get
		End Property

		''' <summary>
		''' The last approved post's Author's user information for this forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property MostRecentPostAuthor(ByVal RecentPostAuthorID As Integer) As ForumUser
			Get
				Return ForumUserController.GetForumUser(RecentPostAuthorID, False, ModuleID, PortalID)
			End Get
		End Property

#End Region

#Region "Public Properties"

		''' <summary>
		''' The GroupID of the Group this forum is part of.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property GroupID() As Integer
			Get
				Return _GroupID
			End Get
			Set(ByVal Value As Integer)
				_GroupID = Value
			End Set
		End Property

		''' <summary>
		''' The ModuleID this forum is related to. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal Value As Integer)
				_ModuleID = Value
			End Set
		End Property

		''' <summary>
		''' The ForumID of the current forum
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumID() As Integer
			Get
				Return _ForumID
			End Get
			Set(ByVal Value As Integer)
				_ForumID = Value
			End Set
		End Property

		''' <summary>
		''' If the forum is enabled/disabled.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsActive() As Boolean
			Get
				Return _IsActive
			End Get
			Set(ByVal Value As Boolean)
				_IsActive = Value
			End Set
		End Property

		''' <summary>
		''' The parentForumID should be 0 for now.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ParentId() As Integer
			Get
				Return _ParentID
			End Get
			Set(ByVal Value As Integer)
				_ParentID = Value
			End Set
		End Property

		''' <summary>
		''' The name the users will see for the forum.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal Value As String)
				_Name = Value
			End Set
		End Property

		''' <summary>
		''' A brief description of the forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Description() As String
			Get
				Return _Description
			End Get
			Set(ByVal Value As String)
				_Description = Value
			End Set
		End Property

		''' <summary>
		''' The UserID of the user who created the forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedByUser() As Integer
			Get
				Return _CreatedByUser
			End Get
			Set(ByVal Value As Integer)
				_CreatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' The date the forum was created. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedDate() As DateTime
			Get
				Return _CreatedDate
			End Get
			Set(ByVal Value As DateTime)
				_CreatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The order (ascending/descending) the threads should be displayed. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property SortOrder() As Integer
			Get
				Return _SortOrder
			End Get
			Set(ByVal Value As Integer)
				_SortOrder = Value
			End Set
		End Property

		''' <summary>
		''' Total number of approved posts in the forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TotalPosts() As Integer
			Get
				Return _TotalPosts
			End Get
			Set(ByVal Value As Integer)
				_TotalPosts = Value
			End Set
		End Property

		''' <summary>
		''' Total number of approved threads in the forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TotalThreads() As Integer
			Get
				Return _TotalThreads
			End Get
			Set(ByVal Value As Integer)
				_TotalThreads = Value
			End Set
		End Property

		''' <summary>
		''' If post stats are fed to a queue (non-functional)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnablePostStatistics() As Boolean
			Get
				Return _EnablePostStatistics
			End Get
			Set(ByVal Value As Boolean)
				_EnablePostStatistics = Value
			End Set
		End Property

		''' <summary>
		''' The last approved post in a forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property MostRecentPostID() As Integer
			Get
				Return _MostRecentPostID
			End Get
			Set(ByVal Value As Integer)
				_MostRecentPostID = Value
			End Set
		End Property

		''' <summary>
		''' The last approved thread in a forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property MostRecentThreadID() As Integer
			Get
				Return _MostRecentThreadID
			End Get
			Set(ByVal Value As Integer)
				_MostRecentThreadID = Value
			End Set
		End Property

		''' <summary>
		''' The last approved post author's UserID. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property MostRecentPostAuthorID() As Integer
			Get
				Return _MostRecentPostAuthorID
			End Get
			Set(ByVal Value As Integer)
				_MostRecentPostAuthorID = Value
			End Set
		End Property

		''' <summary>
		''' The last approved posts date. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property MostRecentPostDate() As DateTime
			Get
				Return _MostRecentPostDate
			End Get
			Set(ByVal Value As DateTime)
				_MostRecentPostDate = Value
			End Set
		End Property

		''' <summary>
		''' Determines if the most recent thread is pinned.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property MostRecentThreadPinned() As Boolean
			Get
				Return _MostRecentThreadPinned
			End Get
			Set(ByVal Value As Boolean)
				_MostRecentThreadPinned = Value
			End Set
		End Property

		''' <summary>
		''' Number of posts in the moderation queue for this forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostsToModerate() As Integer
			Get
				Return _PostsToModerate
			End Get
			Set(ByVal Value As Integer)
				_PostsToModerate = Value
			End Set
		End Property

		''' <summary>
		''' The type of forum determines how it will behave. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumType() As Integer
			Get
				Return _ForumType
			End Get
			Set(ByVal Value As Integer)
				_ForumType = Value
			End Set
		End Property

		''' <summary>
		''' If the forum is integrated w/ a 3rd party module
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsIntegrated() As Boolean
			Get
				Return _IsIntegrated
			End Get
			Set(ByVal Value As Boolean)
				_IsIntegrated = Value
			End Set
		End Property

		''' <summary>
		''' 3rd party integration Module's ModuleID. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IntegratedModuleID() As Integer
			Get
				Return _IntegratedModuleID
			End Get
			Set(ByVal Value As Integer)
				_IntegratedModuleID = Value
			End Set
		End Property

		''' <summary>
		''' The objects which are integrated (possibly legacy)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IntegratedObjects() As String
			Get
				Return _IntegratedObjects
			End Get
			Set(ByVal Value As String)
				_IntegratedObjects = Value
			End Set
		End Property

		''' <summary>
		''' The UserID of the person to last update the forum settings. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedByUser() As Integer
			Get
				Return _UpdatedByUser
			End Get
			Set(ByVal Value As Integer)
				_UpdatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' The date the forum settings were last updated. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedDate() As DateTime
			Get
				Return _UpdatedDate
			End Get
			Set(ByVal Value As DateTime)
				_UpdatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The url which represents the forum link.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This is used only for Link forum types.</remarks>
		Public Property ForumLink() As String
			Get
				Return _ForumLink
			End Get
			Set(ByVal Value As String)
				_ForumLink = Value
			End Set
		End Property

		''' <summary>
		''' Uses an enum to determine public/private, restricted posting/not restricted, moderated/not moderated.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumBehavior() As DotNetNuke.Modules.Forum.ForumBehavior
			Get
				Return _ForumBehavior
			End Get
			Set(ByVal Value As DotNetNuke.Modules.Forum.ForumBehavior)
				_ForumBehavior = Value
			End Set
		End Property

		''' <summary>
		''' A collection of forum permissions for a single forumID instance. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumPermissions() As DotNetNuke.Modules.Forum.ForumPermissionCollection
			Get
				Return _ForumPermissions
			End Get
			Set(ByVal Value As DotNetNuke.Modules.Forum.ForumPermissionCollection)
				_ForumPermissions = Value
			End Set
		End Property

		''' <summary>
		''' If thread status feature is enabled for this forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnableForumsThreadStatus() As Boolean
			Get
				Return _EnableForumsThreadStatus
			End Get
			Set(ByVal Value As Boolean)
				_EnableForumsThreadStatus = Value
			End Set
		End Property

		''' <summary>
		''' If rating capability is enabled for this particular forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnableForumsRating() As Boolean
			Get
				Return _EnableForumsRating
			End Get
			Set(ByVal Value As Boolean)
				_EnableForumsRating = Value
			End Set
		End Property

		''' <summary>
		''' If the forum allows polls to be created.
		''' </summary>
		''' <value></value>
		''' <returns>True if polls are enabled, false otherwise.</returns>
		Public Property AllowPolls() As Boolean
			Get
				Return _AllowPolls
			End Get
			Set(ByVal Value As Boolean)
				_AllowPolls = Value
			End Set
		End Property

		''' <summary>
		''' Permits enable/disable of RSS feeds per forum. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EnableRSS() As Boolean
			Get
				Return _EnableRSS
			End Get
			Set(ByVal Value As Boolean)
				_EnableRSS = Value
			End Set
		End Property

		''' <summary>
		''' Counts subforums
		''' </summary>
		''' <value></value>
		''' <returns>Number of subforums</returns>
		Public Property SubForums() As Integer
			Get
				Return _SubForums
			End Get
			Set(ByVal Value As Integer)
				_SubForums = Value
			End Set
		End Property

		''' <summary>
		''' The FROM email addressed used for outgoing emails. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EmailAddress() As String
			Get
				Return _EmailAddress
			End Get
			Set(ByVal Value As String)
				_EmailAddress = Value
			End Set
		End Property

		''' <summary>
		''' The friendly name to use instead of the actual address. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This helps reduce the chance of the message being flagged as SPAM.</remarks>
		Public Property EmailFriendlyFrom() As String
			Get
				Return _EmailFriendlyFrom
			End Get
			Set(ByVal Value As String)
				_EmailFriendlyFrom = Value
			End Set
		End Property

		''' <summary>
		''' If users should be subscribed to a forum by default (only happens user's first visit)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Not implemented yet.</remarks>
		Public Property NotifyByDefault() As Boolean
			Get
				Return _NotifyByDefault
			End Get
			Set(ByVal Value As Boolean)
				_NotifyByDefault = Value
			End Set
		End Property

		''' <summary>
		''' If user's can receive emails when a thread status change is made. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Not implemented yet.</remarks>
		Public Property EmailStatusChange() As Boolean
			Get
				Return _EmailStatusChange
			End Get
			Set(ByVal Value As Boolean)
				_EmailStatusChange = Value
			End Set
		End Property

		''' <summary>
		''' Not implemented.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EmailServer() As String
			Get
				Return _EmailServer
			End Get
			Set(ByVal Value As String)
				_EmailServer = Value
			End Set
		End Property

		''' <summary>
		''' Not implemented.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EmailUser() As String
			Get
				Return _EmailUser
			End Get
			Set(ByVal Value As String)
				_EmailUser = Value
			End Set
		End Property

		''' <summary>
		''' Not implemented.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EmailPass() As String
			Get
				Return _EmailPass
			End Get
			Set(ByVal Value As String)
				_EmailPass = Value
			End Set
		End Property

		''' <summary>
		''' Not implemented.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EmailEnableSSL() As Boolean
			Get
				Return _EmailEnableSSL
			End Get
			Set(ByVal Value As Boolean)
				_EmailEnableSSL = Value
			End Set
		End Property

		''' <summary>
		''' Not implemented.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EmailAuth() As Integer
			Get
				Return _EmailAuth
			End Get
			Set(ByVal Value As Integer)
				_EmailAuth = Value
			End Set
		End Property

		''' <summary>
		''' The port that will be used to check email.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EmailPort() As Integer
			Get
				Return _EmailPort
			End Get
			Set(ByVal Value As Integer)
				_EmailPort = Value
			End Set
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Renders forum group view seen on initial forum display.  This shows all expanded
		''' forum information. (ie. Forum Name, Threads, Posts, Last Post and status icons)
		''' This is ONLY called from ForumGroup.vb
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Forum"></param>
		''' <param name="Count"></param>
		''' <remarks>
		''' </remarks>
		Public Sub Render(ByVal wr As HtmlTextWriter, ByVal Forum As DNNForum, Optional ByVal Count As Integer = 0)
			Try
				Dim fLoggedOnUser As ForumUser = Forum.LoggedOnUser
				Dim fPostsPerPage As Integer = Forum.LoggedOnUser.PostsPerPage
				Dim fPage As Page = Forum.DNNPage
				Dim fTabID As Integer = Forum.TabID
				Dim fModuleID As Integer = Forum.ModuleID
				Dim url As String
				Dim objForumInfo As New ForumInfo

				If _ForumID <> -1 Then
					Dim cntForum As New ForumController

					objForumInfo = cntForum.GetForumInfoCache(_ForumID)
				Else
					'Aggregated Forum (so this is never cached, since it is per user). 
					objForumInfo.ModuleID = ModuleID
					objForumInfo.GroupID = -1
					objForumInfo.ForumID = -1
					objForumInfo.ForumType = 0
					objForumInfo.IntegratedModuleID = 0
					objForumInfo.IntegratedObjects = Nothing
					objForumInfo.IsActive = Forum.objConfig.AggregatedForums
					objForumInfo.IsIntegrated = False
					objForumInfo.TotalThreads = 0
					objForumInfo.TotalPosts = 0

					Name = Localization.GetString("AggregatedForumName", Forum.objConfig.SharedResourceFile)
					objForumInfo.Name = Name

					Description = Localization.GetString("AggregatedForumDescription", Forum.objConfig.SharedResourceFile)
					objForumInfo.Description = Description

					Dim SearchCollection As New ArrayList
					Dim cntSearch As New SearchController
					SearchCollection = cntSearch.SearchGetResults("", 0, 1, Forum.LoggedOnUser.UserID, Forum.ModuleID, DateAdd(DateInterval.Year, -1, DateTime.Today), DateAdd(DateInterval.Day, 1, DateTime.Today), -1)

					For Each objSearch As SearchInfo In SearchCollection
						If objSearch IsNot Nothing Then
							_MostRecentPostDate = objSearch.CreatedDate
							_MostRecentPostID = objSearch.LastPostedPostID
							_MostRecentPostAuthorID = objSearch.LastApprovedUser.UserID
							_MostRecentThreadID = objSearch.ThreadID
						End If
					Next

					objForumInfo.MostRecentPostDate = _MostRecentPostDate
					objForumInfo.MostRecentPostID = _MostRecentPostID
					objForumInfo.MostRecentPostAuthorID = _MostRecentPostAuthorID
					objForumInfo.MostRecentThreadID = _MostRecentThreadID
				End If

				If Not objForumInfo Is Nothing Then
					wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>   

					Dim even As Boolean = ForumIsEven(Count)
					If even Then
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Row")
					Else
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Row_Alt")
					End If

					'see threads to determine how to build table here
					' cell holds table for post icon/thread subject
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "52%")
					wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
					wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

					' table holds post icon/thread subject/number viewing
					wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
					wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
					wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>
					wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>

					'status icon column
					wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
					wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

					Dim NewWindow As Boolean = False
					If objForumInfo.ForumType = DotNetNuke.Modules.Forum.ForumType.Link Then
						Dim objCnt As New DotNetNuke.Common.Utilities.UrlController
						Dim objURLTrack As New DotNetNuke.Common.Utilities.UrlTrackingInfo
						Dim TrackClicks As Boolean = False

						objURLTrack = objCnt.GetUrlTracking(PortalID, objForumInfo.ForumLink, ModuleID)

						If Not objURLTrack Is Nothing Then
							TrackClicks = objURLTrack.TrackClicks
							NewWindow = objURLTrack.NewWindow
						End If

						url = DotNetNuke.Common.Globals.LinkClick(objForumInfo.ForumLink, ParentGroup.objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, TrackClicks)

					Else
						If objForumInfo.GroupID = -1 Then
							' aggregated
							url = Utilities.Links.ContainerAggregatedLink(Forum.TabID, False)
						Else
							'[Skeel] Check if this is a parent forum
							If objForumInfo.IsParentForum = True Then
								'Parent forum, link to group view
								url = Utilities.Links.ContainerParentForumLink(Forum.TabID, GroupID, ForumID)
								'Now let's get the most recent info
								objForumInfo = ForumGetMostRecentInfo(objForumInfo)
							Else
								'Normal Forum, link goes to Thread view
								url = Utilities.Links.ContainerViewForumLink(Forum.TabID, ForumID, False)
							End If

						End If
					End If

					' handle HasNewThreads here (because it's user specific)
					Dim HasNewThreads As Boolean = True

					' Only worry about user forum reads if the user is logged in (performance reasons)
					' [skeel] .. and not a link type forum
					If fLoggedOnUser.UserID > 0 And objForumInfo.ForumType <> 2 Then
						Dim userForumController As New UserForumsController

						'[skeel] added support for subforums
						If objForumInfo.IsParentForum = True Then
							'Parent Forum
							Dim LastVisitDate As Date = Now.AddYears(1)
							Dim dr As IDataReader = userForumController.GetSubForumIDs(objForumInfo.ForumID)
							While dr.Read
								Dim userForum As New UserForumsInfo
								userForum = userForumController.GetCachedUserForumRead(fLoggedOnUser.UserID, CInt(dr("ForumID").ToString))
								If Not userForum Is Nothing Then
									If LastVisitDate > userForum.LastVisitDate Then
										LastVisitDate = userForum.LastVisitDate
									End If
								End If
							End While
							dr.Close()
							If Not LastVisitDate < MostRecentPostDate Then
								HasNewThreads = False
							End If
						Else
							Dim userForum As New UserForumsInfo
							If objForumInfo.ForumID = -1 Then
								'Aggregated Forum
								'[skeel] at some point we should consider looping through all the threads, but it's
								'a lot of CPU cycles just to show an icon, so for now we always show it as containing new posts
							Else
								'Regular forum
								userForum = userForumController.GetCachedUserForumRead(fLoggedOnUser.UserID, ForumID)
							End If

							If Not userForum Is Nothing Then
								If Not userForum.LastVisitDate < MostRecentPostDate Then
									HasNewThreads = False
								End If
							End If

						End If

					End If

					' display image depends on new post status 
					If Not objForumInfo.PublicView Then
						' See if the forum is a Link Type forum
						If objForumInfo.ForumType = 2 Then
							RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_linktype.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgLinkType") & " " & url, "", NewWindow)
						Else
							' See if the forum is moderated
							If objForumInfo.IsModerated Then
								If HasNewThreads AndAlso TotalThreads > 0 Then
									RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_private_moderated_new.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgNewPrivateModerated"), "", False)
								Else
									RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_private_moderated.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgPrivateModerated"), "", False)
								End If
							Else
								If HasNewThreads AndAlso TotalThreads > 0 Then
									RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_private_new.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgNewPrivate"), "", False)
								Else
									RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_private.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgPrivate"), "", False)
								End If
							End If
						End If
					Else
						' See if the forum is a Link Type forum
						If objForumInfo.IsParentForum = True Then
							'[skeel] parent forum
							If HasNewThreads AndAlso TotalThreads > 0 Then
								RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_parent_new.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgNewUnmoderated"), "", False)
							Else
								RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_parent.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgUnmoderated"), "", False)
							End If
						ElseIf objForumInfo.ForumType = 2 Then
							RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_linktype.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgLinkType") & " " & url, "", NewWindow)
						Else
							If objForumInfo.ForumID = -1 Then
								RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_aggregate.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgAggregated"), "", False)
							Else
								' Determine if forum is moderated
								If objForumInfo.IsModerated Then
									If HasNewThreads AndAlso TotalThreads > 0 Then
										RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_moderated_new.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgNewModerated"), "", False)
									Else
										RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_moderated.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgModerated"), "", False)
									End If
								Else
									If HasNewThreads AndAlso TotalThreads > 0 Then
										RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_unmoderated_new.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgNewUnmoderated"), "", False)
									Else
										RenderImageButton(wr, url, Forum.objConfig.GetThemeImageURL("forum_unmoderated.") & Forum.objConfig.ImageExtension, Forum.LocalizedText("imgUnmoderated"), "", False)
									End If
								End If
							End If
						End If
					End If

					wr.RenderEndTag() '</td>

					' subject/# currently viewing column
					wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
					wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td>

					' table to hold theard subject & reserved area for number of users currently viewing            
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
					wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
					wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
					wr.RenderBeginTag(HtmlTextWriterTag.Table) '<table>
					wr.RenderBeginTag(HtmlTextWriterTag.Tr)	'<tr>

					' Spacing between status icon and subject
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "1px")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, Forum.objConfig.GetThemeImageURL("row_spacer.gif"))
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
					wr.RenderEndTag() ' </Img>

					wr.RenderEndTag() '  </td>

					wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
					wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td>

					If NewWindow Then
						wr.AddAttribute(HtmlTextWriterAttribute.Target, "_blank")
					End If
					wr.AddAttribute(HtmlTextWriterAttribute.Href, url)

					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalBold")
					wr.RenderBeginTag(HtmlTextWriterTag.A) ' <A>
					wr.Write(Name)
					wr.RenderEndTag() ' </A>             

					' Display forum description
					If Len(Description) > 0 Then
						wr.Write("<br />")
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_GroupDetails")
						wr.RenderBeginTag(HtmlTextWriterTag.Span) '<Span>
						wr.Write(objForumInfo.Description)
						wr.RenderEndTag() ' </Span>
					End If

					'[skeel] here we place subforums, if any
					If objForumInfo.IsParentForum Then
						wr.Write("<br />")
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_SubForumContainer")
						wr.RenderBeginTag(HtmlTextWriterTag.Div) '<div>
						RenderSubForums(wr, objForumInfo.ForumID, objForumInfo.GroupID, "Forum_SubForumLink", objForumInfo.SubForums, Forum)
						wr.RenderEndTag() '</div>
					End If

					wr.RenderEndTag() ' </td>

					''This is the column where currently viewing number will eventually go
					'wr.RenderBeginTag(HtmlTextWriterTag.Td) ' <td>
					'wr.RenderEndTag() ' </td>

					'end table that holds subject/# viewing 
					wr.RenderEndTag() ' </tr>
					wr.RenderEndTag() ' </table>

					'End column which holds subject/# viewing table
					wr.RenderEndTag() ' </td>

					' end table that holds post icon/thread subject/number viewing
					wr.RenderEndTag() ' </tr>
					wr.RenderEndTag() ' </table>

					' end column that holds table for post icon/thread subject
					wr.RenderEndTag() ' </td>

					' Threads column
					If even Then
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_RowHighlight1")
					Else
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_RowHighlight1_Alt")
					End If

					wr.AddAttribute(HtmlTextWriterAttribute.Align, "center")
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "11%")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Threads")
					wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>

					If objForumInfo.ForumType = 2 Or objForumInfo.ForumID = -1 Then
						wr.Write("-")
					Else
						wr.Write(objForumInfo.TotalThreads.ToString)
					End If


					wr.RenderEndTag() ' </span>
					wr.RenderEndTag() ' </td>

					If even Then
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_RowHighlight2")
					Else
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_RowHighlight2_Alt")
					End If

					'Posts column
					wr.AddAttribute(HtmlTextWriterAttribute.Align, "center")
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "11%")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Posts")
					wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>
					If objForumInfo.ForumType = 2 Or objForumInfo.ForumID = -1 Then
						wr.Write("-")
					Else
						wr.Write(objForumInfo.TotalPosts.ToString)
					End If
					wr.RenderEndTag() ' </span>
					wr.RenderEndTag() ' </td>

					' last Post date info & author
					If even Then
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_RowHighlight3")
					Else
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_RowHighlight3_Alt")
					End If

					wr.AddAttribute(HtmlTextWriterAttribute.Align, "right")
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "26%")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>
					wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Align, "right")
					wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
					wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>
					wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>

					wr.AddAttribute(HtmlTextWriterAttribute.Width, "1px")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>
					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, Forum.objConfig.GetThemeImageURL("row_spacer.gif"))
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
					wr.RenderEndTag() ' </Img>

					wr.RenderEndTag() '  </td>

					If even Then
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "")
					Else
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "")
					End If

					wr.AddAttribute(HtmlTextWriterAttribute.Align, "right")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

					If objForumInfo.ForumType = 2 Then
						'Link forum
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
						wr.RenderBeginTag(HtmlTextWriterTag.Span) '<span>
						wr.Write("-")
						wr.RenderEndTag() ' </span>
					Else
						If (objForumInfo.MostRecentPostID > 0) Then
							'Dim displayCreatedDate As DateTime = ConvertTimeZone(MostRecentPostDate, objConfig)
							Dim lastPostInfo As New PostInfo
							lastPostInfo = PostInfo.GetPostInfo(objForumInfo.MostRecentPostID, PortalID)
							Dim strLastPostInfo As String = Utilities.ForumUtils.GetCreatedDateInfo(objForumInfo.MostRecentPostDate, Forum.objConfig, "")
							' shows only first 15 letters of the post subject title
							Dim truncatedTitle As String
							If lastPostInfo.Subject.Length > 16 Then
								truncatedTitle = Left(lastPostInfo.Subject, 15) & "..."
							Else
								truncatedTitle = lastPostInfo.Subject
							End If


							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
							wr.RenderBeginTag(HtmlTextWriterTag.Div) ' <div>

							url = Utilities.Links.ContainerViewPostLink(Forum.TabID, ForumID, objForumInfo.MostRecentPostID)
							wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
							wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
							wr.Write(truncatedTitle)
							wr.RenderEndTag() '  </A>

							wr.RenderEndTag() ' </div>

							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
							wr.RenderBeginTag(HtmlTextWriterTag.Div) ' <div>

							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
							wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <a>
							wr.Write(strLastPostInfo)
							wr.RenderEndTag() '</A>

							wr.RenderEndTag() ' </div>

							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
							wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>
							wr.Write(Forum.LocalizedText("by") & " ")
							wr.RenderEndTag() ' </span>

							url = Utilities.Links.UserPublicProfileLink(Forum.TabID, ModuleID, objForumInfo.MostRecentPostAuthorID, Forum.objConfig.EnableExternalProfile, Forum.objConfig.ExternalProfileParam, Forum.objConfig.ExternalProfilePage, Forum.objConfig.ExternalProfileUsername, objForumInfo.MostRecentPostAuthor(objForumInfo.MostRecentPostAuthorID).Username)
							wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText") 'Forum_AliasLink
							wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
							wr.Write(objForumInfo.MostRecentPostAuthor(objForumInfo.MostRecentPostAuthorID).SiteAlias)
							wr.RenderEndTag() '  </A>
						Else
							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
							wr.RenderBeginTag(HtmlTextWriterTag.Span) '<span>
							wr.Write(Forum.LocalizedText("None"))
							wr.RenderEndTag() ' </span>
						End If
					End If

					wr.RenderEndTag() ' </Td>

					wr.AddAttribute(HtmlTextWriterAttribute.Width, "1px")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, Forum.objConfig.GetThemeImageURL("row_spacer.gif"))
					wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
					wr.RenderEndTag() ' </Img>

					wr.RenderEndTag() '  </td>
					wr.RenderEndTag() ' </Tr>
					wr.RenderEndTag() ' </Table>
					wr.RenderEndTag() ' </Td>
					wr.RenderEndTag() ' </Tr>

				End If
			Catch ex As Exception
				LogException(ex)
			End Try
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Updates nesseary information to display last post info for a parentforum (subforums container)
		''' </summary>
		''' <param name="objForumInfo"></param>
		''' <returns>An updated ForumInfo object</returns>
		''' <remarks>Added by Skeel</remarks>
		Private Function ForumGetMostRecentInfo(ByVal objForumInfo As ForumInfo) As ForumInfo
			Dim dr As IDataReader = DotNetNuke.Modules.Forum.DataProvider.Instance().ForumGetMostRecentInfo(_ForumID)
			While dr.Read

				objForumInfo.MostRecentPostAuthorID = CInt(dr("MostRecentPostAuthorID").ToString)
				objForumInfo.MostRecentPostDate = CDate(dr("MostRecentPostDate").ToString)
				objForumInfo.MostRecentPostID = CInt(dr("MostRecentPostID").ToString)
				objForumInfo.MostRecentThreadID = CInt(dr("MostRecentThreadID").ToString)
				'Needed to display the correct name
				MostRecentPostAuthorID = objForumInfo.MostRecentPostAuthorID

			End While
			dr.Close()
			Return objForumInfo
		End Function

		''' <summary>
		''' Renders a list of subforum links
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="ParentID"></param>
		''' <remarks>Added by Skeel</remarks>
		Private Sub RenderSubForums(ByVal wr As HtmlTextWriter, ByVal ParentID As Integer, ByVal GroupId As Integer, ByVal Css As String, ByVal SubForumCount As Integer, ByVal Forum As DNNForum)
			Dim Url As String
			Dim i As Integer = 1
			Dim SubForum As New ForumInfo
			Dim forumCtl As New ForumController
			Dim arrSubForums As List(Of ForumInfo) = forumCtl.ForumGetAllByParentID(ParentID, GroupId, True)

			wr.RenderBeginTag(HtmlTextWriterTag.B) '<b>
			wr.Write(Localization.GetString("SubForums", Forum.objConfig.SharedResourceFile) & ": ")
			wr.RenderEndTag() '</b>

			For Each SubForum In arrSubForums
				If SubForum.IsActive Then
					Dim NewWindow As Boolean = False

					If SubForum.ForumType = DotNetNuke.Modules.Forum.ForumType.Link Then
						Dim objCnt As New DotNetNuke.Common.Utilities.UrlController
						Dim objURLTrack As New DotNetNuke.Common.Utilities.UrlTrackingInfo
						Dim TrackClicks As Boolean = False

						objURLTrack = objCnt.GetUrlTracking(PortalID, SubForum.ForumLink, ModuleID)

						If Not objURLTrack Is Nothing Then
							TrackClicks = objURLTrack.TrackClicks
							NewWindow = objURLTrack.NewWindow
						End If

						Url = DotNetNuke.Common.Globals.LinkClick(SubForum.ForumLink, Forum.objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, TrackClicks)
					Else
						If SubForum.GroupID = -1 Then
							' aggregated
							Url = Utilities.Links.ContainerAggregatedLink(Forum.objConfig.CurrentPortalSettings.ActiveTab.TabID, False)
						Else
							Url = Utilities.Links.ContainerViewForumLink(Forum.objConfig.CurrentPortalSettings.ActiveTab.TabID, SubForum.ForumID, False)
						End If
					End If

					wr.AddAttribute(HtmlTextWriterAttribute.Href, Url.Replace("~/", ""))

					If Css.Length > 0 Then
						wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
					End If
					wr.RenderBeginTag(HtmlTextWriterTag.A) '<a>

					wr.Write(SubForum.Name)
					wr.RenderEndTag() ' </a>

					If i < SubForumCount Then
						wr.Write(", ")
					End If

					i = i + 1
				End If
			Next
		End Sub

		''' <summary>
		''' Mimics function in base (which cannot be inherited from here) so we can render image buttons (like Group icons that are clickable)
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Url"></param>
		''' <param name="ImageUrl"></param>
		''' <param name="Tooltip"></param>
		''' <param name="Css"></param>
		''' <remarks></remarks>
		Private Sub RenderImageButton(ByVal wr As HtmlTextWriter, ByVal Url As String, ByVal ImageUrl As String, ByVal Tooltip As String, ByVal Css As String, ByVal NewWindow As Boolean)
			wr.AddAttribute(HtmlTextWriterAttribute.Href, Url.Replace("~/", ""))

			If NewWindow Then
				wr.AddAttribute(HtmlTextWriterAttribute.Target, "_blank")
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.A)
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")

			wr.AddAttribute(HtmlTextWriterAttribute.Src, ImageUrl)

			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If

			If Tooltip.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Alt, Tooltip)
				wr.AddAttribute(HtmlTextWriterAttribute.Title, Tooltip)
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' img 
			wr.RenderEndTag() ' img 
			wr.RenderEndTag() ' A
		End Sub

		''' <summary>
		''' Determines if thread is even or odd numbered row
		''' </summary>
		''' <param name="Count"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	11/26/2005	Created
		''' </history>
		Private Function ForumIsEven(ByVal Count As Integer) As Boolean
			If Count Mod 2 = 0 Then
				'even
				Return True
			Else
				'odd
				Return False
			End If
		End Function

#End Region

	End Class

#End Region

End Namespace
