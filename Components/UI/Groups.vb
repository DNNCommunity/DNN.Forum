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

	''' <summary>
	''' This is the initial view seen by the forums module. (Group View)
	''' All rendering is done in code to create UI or code is called from here (in utilities, for example). 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class Groups
		Inherits ForumObject

#Region "Private Declarations"

		Dim _AuthorizedGroups As List(Of GroupInfo)
		Dim _AuthForumsCount As Integer = 0

#End Region

#Region "Properties"

		''' <summary>
		''' This is the selected group. When a group is selected, only its forums will be displayed (and not other groups). 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property SelectedGroupID() As Integer
			Get
				If Not HttpContext.Current.Request.QueryString("groupid") Is Nothing Then
					Return CType(HttpContext.Current.Request.QueryString("groupid"), Integer)
				Else
					Return 0
				End If
			End Get
		End Property

		''' <summary>
		''' This is the selected parent forum (since we render child forums, like forums, within this group view). 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Results in similar view as selected group.</remarks>
		Private ReadOnly Property SelectedForumID() As Integer
			Get
				If Not HttpContext.Current.Request.QueryString("forumid") Is Nothing Then
					Return CType(HttpContext.Current.Request.QueryString("forumid"), Integer)
				Else
					Return 0
				End If
			End Get
		End Property

		''' <summary>
		''' Collection of groups that contain at least one authorized forum for the current user.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property AuthorizedGroups() As List(Of GroupInfo)
			Get
				Return _AuthorizedGroups
			End Get
			Set(ByVal Value As List(Of GroupInfo))
				_AuthorizedGroups = Value
			End Set
		End Property

		''' <summary>
		''' This is the total number of forums the end user is authorized to view (used for total # count).
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property AuthForumsCount() As Integer
			Get
				Return _AuthForumsCount
			End Get
			Set(ByVal Value As Integer)
				_AuthForumsCount = Value
			End Set
		End Property

#End Region

#Region "Public Methods"

#Region "Contructors"

		''' <summary>
		''' Creates a new instance of this class.
		''' </summary>
		''' <param name="forum"></param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal forum As DNNForum)
			MyBase.New(forum)
		End Sub

#End Region

		''' <summary>
		''' Creates all controls like drop down lists, image buttons, etc.
		''' This also uses defacto standards from the module's settings
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Overrides Sub CreateChildControls()
			Controls.Clear()

			' Get groups
			Dim cntGroup As New GroupController
			AuthorizedGroups = cntGroup.GroupGetAllAuthorized(ModuleID, CurrentForumUser.UserID, False, TabID)
			AuthForumsCount = 0

			If AuthorizedGroups.Count > 0 Then
				For Each objGroup As GroupInfo In AuthorizedGroups
					Dim arrAuthForums As New List(Of ForumInfo)
					arrAuthForums = cntGroup.AuthorizedForums(CurrentForumUser.UserID, objGroup.GroupID, False, ModuleID, TabID)
					If arrAuthForums.Count > 0 Then
						AuthForumsCount += arrAuthForums.Count
					End If
				Next
			End If
		End Sub

		''' <summary>
		''' This renders the initial UI seen in the module (by calling other private 
		''' methods.  This is the group view.  It also checks first to see if only a 
		''' specific group should be shown based
		''' on TabModuleSettings.
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Public Overrides Sub Render(ByVal wr As HtmlTextWriter)
			RenderTableBegin(wr, 0, 0, "tblGroup") ' <table>
			Dim objGroupCnt As New GroupController
			Dim arrGroups As New List(Of GroupInfo)

			arrGroups = objGroupCnt.GroupsGetByModuleID(ModuleID)

			If arrGroups.Count > 0 Then
				RenderNavBar(wr, objConfig, ForumControl)
				RenderBreadCrumbRow(wr)
				RenderForums(wr)
				RenderFooter(wr)
			Else
				'No Groups are configured for this module
				RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "", "", "") ' <td> 
				RenderDivBegin(wr, "Config", "NormalRed") ' <div>
				wr.Write(ForumControl.LocalizedText("ForumContainsNothing"))
				RenderDivEnd(wr) ' </div>
				RenderCellEnd(wr) ' </Td>
			End If

			RenderTableEnd(wr) '</table>
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Breadcrumb row.
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderBreadCrumbRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td><img/></td>

			RenderCellBegin(wr, "", "", "100%", "left", "bottom", "", "") ' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "") ' <table>
			RenderRowBegin(wr) '<tr>

			RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td> 
			Dim ChildGroupView As Boolean = False
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				ChildGroupView = True
			End If

			'[Skeel] add support for full breadcrumb on groupview and parentforum view
			If SelectedForumID > 0 And SelectedGroupID > 0 Then
				'Parent Forum view
				Dim cltForum As New ForumController
				Dim objForumInfo As ForumInfo
				objForumInfo = cltForum.GetForumItemCache(SelectedForumID)
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Groups, objForumInfo, objConfig, ChildGroupView))
			ElseIf SelectedGroupID > 0 Then
				'Group view
				Dim cltGroups As New GroupController
				Dim objGroupInfo As GroupInfo
				objGroupInfo = cltGroups.GroupGet(SelectedGroupID)
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Groups, objGroupInfo, objConfig, ChildGroupView))
				'ElseIf SelectedGroupID = -1 Then
				'	' Aggregated Group View
				'	Dim cltGroups As New GroupController
				'	Dim objGroupInfo As GroupInfo
				'	objGroupInfo = cltGroups.GroupGet(SelectedGroupID)
				'	wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Groups, objGroupInfo, objConfig, ChildGroupView))
			Else
				'Forum Home view
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Groups, Nothing, objConfig, ChildGroupView))
				RenderCellEnd(wr) ' </td>
				RenderCellBegin(wr, "Forum_LastPostText", "", "", "right", "", "", "") ' <td>

				'View latest x hours
				Dim url As String
				wr.Write(Localization.GetString("ViewLatest", objConfig.SharedResourceFile) & " ")
				url = Utilities.Links.ContainerViewLatestHoursLink(TabID, 6)
				RenderLinkButton(wr, url, Localization.GetString("6", objConfig.SharedResourceFile), "Forum_LastPostText", "", False, objConfig.NoFollowLatestThreads)
				wr.Write(", ")
				url = Utilities.Links.ContainerViewLatestHoursLink(TabID, 12)
				RenderLinkButton(wr, url, Localization.GetString("12", objConfig.SharedResourceFile), "Forum_LastPostText", "", False, objConfig.NoFollowLatestThreads)
				wr.Write(", ")
				url = Utilities.Links.ContainerViewLatestHoursLink(TabID, 24)
				RenderLinkButton(wr, url, Localization.GetString("24", objConfig.SharedResourceFile), "Forum_LastPostText", "", False, objConfig.NoFollowLatestThreads)
				wr.Write(", ")
				url = Utilities.Links.ContainerViewLatestHoursLink(TabID, 48)
				RenderLinkButton(wr, url, Localization.GetString("48", objConfig.SharedResourceFile), "Forum_LastPostText", "", False, objConfig.NoFollowLatestThreads)
				wr.Write("&nbsp;" & Localization.GetString("Hours", objConfig.SharedResourceFile) & "&nbsp;")

				'View unread threads link
				If CurrentForumUser.UserID > 0 Then
					wr.Write("|&nbsp;")
					url = Utilities.Links.ContainerViewUnreadThreadsLink(TabID)
					RenderLinkButton(wr, url, Localization.GetString("ViewUnreadThreads", objConfig.SharedResourceFile), "Forum_LastPostText", "", False, objConfig.NoFollowLatestThreads)
					wr.Write("&nbsp;")
				End If
			End If

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </Td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td><img/></td>
			RenderRowEnd(wr) ' </Tr>
		End Sub

		''' <summary>
		''' Area which has the +/- similar to section head.  This displays group name
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Group"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderGroupInfo(ByVal wr As HtmlTextWriter, ByVal Group As GroupInfo)
			' Start row
			RenderRowBegin(wr) '<tr>

			' This is middle column start - set padding for 4, had Forum_AltHeader as class
			RenderCellBegin(wr, "", "", "", "left", "middle", "4", "")	'<td>

			' Start table
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "left", "", "0") '<Table>            
			RenderRowBegin(wr) '<tr>

			' space to keep subject from being too close and still allow it to be evenly spaced if it wraps to a new row because it is long
			wr.AddAttribute(HtmlTextWriterAttribute.Width, "1px")
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_AltHeaderCapLeft")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>
			'wr.Write("&nbsp;")
			RenderImage(wr, objConfig.GetThemeImageURL("alt_headfoot_height.gif"), "", "")
			RenderCellEnd(wr) ' </td>

			' group name  with link
			RenderCellBegin(wr, "Forum_AltHeader", "", "100%", "left", "middle", "", "") '<td>
			Dim GroupLinkURL As String
			wr.Write("&nbsp;")
			GroupLinkURL = Utilities.Links.ContainerSingleGroupLink(TabID, Group.GroupID)

			RenderLinkButton(wr, GroupLinkURL, Group.Name, "Forum_AltHeaderText")

			'wr.Write(Group.Name)
			RenderCellEnd(wr) ' </td>

			wr.AddAttribute(HtmlTextWriterAttribute.Width, "1px")
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_AltHeaderCapRight")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("alt_headfoot_height.gif"), "", "")

			RenderCellEnd(wr) ' </td>

			' Finish middle table
			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>

			'end middle column
			RenderCellEnd(wr) ' </td>

			' End row
			RenderRowEnd(wr) ' </Tr>
		End Sub

		''' <summary>
		''' Area which has the +/- similar to section head.  This displays group name + Parent Forum name
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="Group"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderGroupForumInfo(ByVal wr As HtmlTextWriter, ByVal Group As GroupInfo)
			' Start row
			RenderRowBegin(wr) '<tr>

			' This is middle column start - set padding for 4, had Forum_AltHeader as class
			RenderCellBegin(wr, "", "", "", "left", "middle", "4", "")	'<td>

			' Start table
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "left", "", "0") '<Table>            
			RenderRowBegin(wr) '<tr>

			' space to keep subject from being too close and still allow it to be evenly spaced if it wraps to a new row because it is long
			wr.AddAttribute(HtmlTextWriterAttribute.Width, "1px")
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_AltHeaderCapLeft")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("alt_headfoot_height.gif"), "", "")
			RenderCellEnd(wr) ' </td>

			' group name with link
			RenderCellBegin(wr, "Forum_AltHeader", "", "100%", "left", "middle", "", "") '<td>
			Dim GroupLinkURL As String
			wr.Write("&nbsp;")
			GroupLinkURL = Utilities.Links.ContainerSingleGroupLink(TabID, Group.GroupID)

			Dim forum As ForumInfo
			Dim cntForum As New ForumController
			forum = cntForum.GetForumItemCache(SelectedForumID)


			RenderLinkButton(wr, GroupLinkURL, Group.Name + " > " & forum.Name, "Forum_AltHeaderText")

			'wr.Write(Group.Name)
			RenderCellEnd(wr) ' </td>

			wr.AddAttribute(HtmlTextWriterAttribute.Width, "1px")
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_AltHeaderCapRight")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("alt_headfoot_height.gif"), "", "")

			RenderCellEnd(wr) ' </td>

			' Finish middle table
			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>

			'end middle column
			RenderCellEnd(wr) ' </td>

			' End row
			RenderRowEnd(wr) ' </Tr>
		End Sub

		''' <summary>
		''' Same as RenderThreads in ForumThread.vb
		''' This renders the groups then all available forums for the user to view
		''' If no forums available to user, the group is not rendered at all and moves to the next.
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderForums(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td><img/></td>
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") '<td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "left", "", "0") ' <table>
			RenderRowBegin(wr) '<tr>
			' Status icon/Subject/# Currently Viewing  column
			RenderCellBegin(wr, "", "", "52%", "left", "middle", "", "")  '<td>
			' Table to hold status and subject
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "left", "", "0") ' <table>
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapLeft", "") ' <td><img/></td>

			RenderCellBegin(wr, "Forum_Header", "", "", "left", "middle", "", "")	 '<td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
			wr.Write("&nbsp;")
			If AuthorizedGroups.Count > 0 Then
				wr.Write(ForumControl.LocalizedText("Forums"))
			Else
				wr.Write(ForumControl.LocalizedText("ForumContainsNothing"))
			End If
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			' Threads column 
			RenderCellBegin(wr, "Forum_Header", "", "11%", "center", "middle", "", "")	 '<td>
			If AuthorizedGroups.Count > 0 Then
				RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
				wr.Write(ForumControl.LocalizedText("Threads"))
				RenderDivEnd(wr) ' </span>
			End If
			RenderCellEnd(wr) ' </td>

			' Posts column 
			RenderCellBegin(wr, "Forum_Header", "", "11%", "center", "middle", "", "")	 '<td>
			If AuthorizedGroups.Count > 0 Then
				RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
				wr.Write(ForumControl.LocalizedText("Posts"))
				RenderDivEnd(wr) ' </span>
			End If
			RenderCellEnd(wr) ' </td>

			' Last Post column 
			RenderCellBegin(wr, "", "", "26%", "center", "middle", "", "")  '<td>
			RenderTableBegin(wr, "", "", "100%", "100%", "0", "0", "", "middle", "0") ' <table>
			RenderRowBegin(wr) '<tr>

			RenderCellBegin(wr, "Forum_Header", "", "", "center", "middle", "", "") ' <td>
			If AuthorizedGroups.Count > 0 Then
				RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
				wr.Write(ForumControl.LocalizedText("LastPost"))
				RenderDivEnd(wr) ' </span>
			End If
			RenderCellEnd(wr) ' </td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapRight", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>

			' Loop through each forum group visible to the user
			If AuthorizedGroups.Count > 0 Then
				' get the group specification ( if it exists )
				Dim GroupID As Integer = 0
				If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
					If CType(ForumControl.TabModuleSettings("groupid"), Integer) = 0 Then
						' We know here group feature (parent/child) is not being used, check for url set item to show single group
						If SelectedGroupID > 0 Then
							' assign a specific groupid so we only show a single group
							GroupID = SelectedGroupID
						End If
					Else
						GroupID = CType(ForumControl.TabModuleSettings("groupid"), Integer)
					End If
				Else	' from else to end of if is correct
					' We know here group feature (parent/child) is not being used, check for url set item to show single group
					If SelectedGroupID > 0 Then
						' assign a specific groupid so we only show a single group
						GroupID = SelectedGroupID
					End If
				End If
				Dim objGroup As New GroupInfo

				If ForumControl.objConfig.AggregatedForums Then
					objGroup = New GroupInfo
					objGroup.PortalID = PortalID
					objGroup.ModuleID = ModuleID
					objGroup.GroupID = -1
					objGroup.Name = Localization.GetString("AggregatedGroupName", ForumControl.objConfig.SharedResourceFile)
					AuthorizedGroups.Insert(0, objGroup)
				End If

				' group is expand or collapse depends on user settings handled a few lines below
				For Each objGroup In AuthorizedGroups
					' filter based on group:  - 1 means show all, matching the groupid to the current groupid in the colleciton means show a single one
					If GroupID = 0 Or GroupID = objGroup.GroupID Then
						'[skeel] Subforums
						Dim arrForums As List(Of ForumInfo)
						Dim cntGroup As New GroupController()

						If SelectedForumID > 0 Then
							arrForums = cntGroup.AuthorizedSubForums(CurrentForumUser.UserID, objGroup.GroupID, False, SelectedForumID, ModuleID, TabID)
						Else
							arrForums = cntGroup.AuthorizedTopLevelForums(CurrentForumUser.UserID, objGroup.GroupID, False, ModuleID, TabID)
						End If

						' display group only if group contains atleast one authorized forum
						If arrForums.Count > 0 Then
							'[skeel] Subforums
							If SelectedForumID > 0 Then
								RenderGroupForumInfo(wr, objGroup)
							Else
								RenderGroupInfo(wr, objGroup)
							End If

							'If the group is not collapsed
							'If (Not LoggedOnUser.GroupIsCollapsed(_groupInfo.GroupID)) Then
							Dim Count As Integer = 1

							' Render a row for each forum in this group exposed to this user
							For Each objForum As ForumInfo In arrForums
								RenderForum(wr, objForum, CurrentForumUser, Count)
								Count += 1
							Next
						End If
					End If
				Next
				'CP-** This end td (ends middle column started in previous function and right cap moved to end of RenderThreadInfo)
				RenderTableEnd(wr) ' </table>
				RenderCellEnd(wr) ' </td>

				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td><img/></td>
				RenderRowEnd(wr) ' </Tr>
			End If
		End Sub

		''' <summary>
		''' Renders forum group view seen on initial forum display.  This shows all expanded
		''' forum information. (ie. Forum Name, Threads, Posts, Last Post and status icons)
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="objForum"></param>
		''' <param name="Count"></param>
		''' <remarks>
		''' </remarks>
		Public Sub RenderForum(ByVal wr As HtmlTextWriter, ByVal objForum As ForumInfo, ByVal CurrentForumUser As ForumUserInfo, Optional ByVal Count As Integer = 0)
			Try
				Dim fPostsPerPage As Integer = CurrentForumUser.PostsPerPage
				Dim fPage As Page = Me.ForumControl.DNNPage
				Dim fTabID As Integer = Me.ForumControl.TabID
				Dim fModuleID As Integer = Me.ForumControl.ModuleID

				If objForum.ForumID = -1 Then
					'Aggregated Forum (so this is never cached, since it is per user). 
					objForum.ModuleID = ModuleID
					objForum.GroupID = -1
					objForum.ForumID = -1
					objForum.ForumType = 0
					objForum.IsActive = objConfig.AggregatedForums
					objForum.TotalThreads = 0
					objForum.TotalPosts = 0
					objForum.ParentID = 0
					objForum.SubForums = 0
					objForum.NotifyByDefault = False
					objForum.Name = Localization.GetString("AggregatedForumName", objConfig.SharedResourceFile)
					objForum.Description = Localization.GetString("AggregatedForumDescription", objConfig.SharedResourceFile)

					Dim SearchCollection As New List(Of ThreadInfo)
					Dim cntSearch As New SearchController
					SearchCollection = cntSearch.SearchGetResults("", 0, 1, CurrentForumUser.UserID, ModuleID, DateAdd(DateInterval.Year, -1, DateTime.Today), DateAdd(DateInterval.Day, 1, DateTime.Today), -1)

					For Each objSearch As ThreadInfo In SearchCollection
						If objSearch IsNot Nothing Then
							objForum.MostRecentPostID = objSearch.LastApprovedPostID
							objForum.ForumID = objSearch.ForumID
						End If
					Next
				End If

				If Not objForum Is Nothing Then
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
					Dim url As String

					If objForum.ForumType = DotNetNuke.Modules.Forum.ForumType.Link Then
						Dim objCnt As New DotNetNuke.Common.Utilities.UrlController
						Dim objURLTrack As New DotNetNuke.Common.Utilities.UrlTrackingInfo
						Dim TrackClicks As Boolean = False

						objURLTrack = objCnt.GetUrlTracking(PortalID, objForum.ForumLink, ModuleID)

						If Not objURLTrack Is Nothing Then
							TrackClicks = objURLTrack.TrackClicks
							NewWindow = objURLTrack.NewWindow
						End If

						url = DotNetNuke.Common.Globals.LinkClick(objForum.ForumLink, TabID, ModuleID, TrackClicks)
					Else
						If objForum.GroupID = -1 Then
							' aggregated
							url = Utilities.Links.ContainerAggregatedLink(TabID, False)
						Else
							'[Skeel] Check if this is a parent forum
							If objForum.ContainsChildForums = True Then
								'Parent forum, link to group view
								url = Utilities.Links.ContainerParentForumLink(TabID, objForum.GroupID, objForum.ForumID)
							Else
								'Normal Forum, link goes to Thread view
								url = Utilities.Links.ContainerViewForumLink(TabID, objForum.ForumID, False)
							End If
						End If
					End If

					' handle HasNewThreads here (because it's user specific)
					Dim HasNewThreads As Boolean = True

					' Only worry about user forum reads if the user is logged in (performance reasons)
					' [skeel] .. and not a link type forum
					If CurrentForumUser.UserID > 0 And objForum.ForumType <> 2 Then
						Dim userForumController As New UserForumsController

						'[skeel] added support for subforums
						If objForum.ContainsChildForums = True Then
							'Parent Forum
							Dim LastVisitDate As Date = Now.AddYears(1)
							Dim cntForum As New ForumController()
							Dim colChildForums As New List(Of ForumInfo)
							colChildForums = cntForum.GetChildForums(objForum.ForumID, objForum.GroupID, True)

							For Each objChildForum As ForumInfo In colChildForums
								Dim userForum As New UserForumsInfo
								userForum = userForumController.GetUsersForumReads(CurrentForumUser.UserID, objChildForum.ForumID)
								If Not userForum Is Nothing Then
									If LastVisitDate > userForum.LastVisitDate Then
										LastVisitDate = userForum.LastVisitDate
									End If
								End If
							Next

							If objForum.MostRecentPost Is Nothing Then
								HasNewThreads = False
							Else
								If Not LastVisitDate < objForum.MostRecentPost.CreatedDate Then
									HasNewThreads = False
								End If
							End If
						Else
							Dim userForum As New UserForumsInfo
							If Not (objForum.ForumID = -1) Then
								userForum = userForumController.GetUsersForumReads(CurrentForumUser.UserID, objForum.ForumID)
							End If

							If Not userForum Is Nothing Then
								If objForum.MostRecentPost Is Nothing Then
									HasNewThreads = False
								Else
									If Not userForum.LastVisitDate < objForum.MostRecentPost.CreatedDate Then
										HasNewThreads = False
									End If
								End If
							End If
						End If
					End If

					' display image depends on new post status 
					If Not objForum.PublicView Then
						' See if the forum is a Link Type forum
						If objForum.ForumType = 2 Then
							RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_linktype.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgLinkType") & " " & url, "", NewWindow)
						Else
							' See if the forum is moderated
							If objForum.IsModerated Then
								If HasNewThreads AndAlso objForum.TotalThreads > 0 Then
									RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_private_moderated_new.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgNewPrivateModerated"), "", False)
								Else
									RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_private_moderated.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgPrivateModerated"), "", False)
								End If
							Else
								If HasNewThreads AndAlso objForum.TotalThreads > 0 Then
									RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_private_new.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgNewPrivate"), "", False)
								Else
									RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_private.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgPrivate"), "", False)
								End If
							End If
						End If
					Else
						' See if the forum is a Link Type forum
						If objForum.ContainsChildForums = True Then
							'[skeel] parent forum
							If HasNewThreads AndAlso objForum.TotalThreads > 0 Then
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_parent_new.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgNewUnmoderated"), "", False)
							Else
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_parent.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgUnmoderated"), "", False)
							End If
						ElseIf objForum.ForumType = 2 Then
							RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_linktype.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgLinkType") & " " & url, "", NewWindow)
						Else
							If objForum.ForumID = -1 Then
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_aggregate.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgAggregated"), "", False)
							Else
								' Determine if forum is moderated
								If objForum.IsModerated Then
									If HasNewThreads AndAlso objForum.TotalThreads > 0 Then
										RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_moderated_new.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgNewModerated"), "", False)
									Else
										RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_moderated.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgModerated"), "", False)
									End If
								Else
									If HasNewThreads AndAlso objForum.TotalThreads > 0 Then
										RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_unmoderated_new.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgNewUnmoderated"), "", False)
									Else
										RenderImageButton(wr, url, objConfig.GetThemeImageURL("forum_unmoderated.") & objConfig.ImageExtension, Me.BaseControl.LocalizedText("imgUnmoderated"), "", False)
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
					wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("row_spacer.gif"))
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
					wr.Write(objForum.Name)
					wr.RenderEndTag() ' </A>             

					' Display forum description
					If Len(objForum.Description) > 0 Then
						wr.Write("<br />")
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_GroupDetails")
						wr.RenderBeginTag(HtmlTextWriterTag.Span) '<Span>
						wr.Write(objForum.Description)
						wr.RenderEndTag() ' </Span>
					End If

					'[skeel] here we place subforums, if any
					If objForum.ContainsChildForums Then
						wr.Write("<br />")
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_SubForumContainer")
						wr.RenderBeginTag(HtmlTextWriterTag.Div) '<div>
						RenderSubForums(wr, objForum.ForumID, objForum.GroupID, "Forum_SubForumLink")
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

					If objForum.ForumType = 2 Or objForum.ForumID = -1 Then
						wr.Write("-")
					Else
						wr.Write(objForum.TotalThreads.ToString)
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
					If objForum.ForumType = 2 Or objForum.ForumID = -1 Then
						wr.Write("-")
					Else
						wr.Write(objForum.TotalPosts.ToString)
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
					wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("row_spacer.gif"))
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

					If objForum.ForumType = 2 Then
						'Link forum
						wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
						wr.RenderBeginTag(HtmlTextWriterTag.Span) '<span>
						wr.Write("-")
						wr.RenderEndTag() ' </span>
					Else
						If (objForum.MostRecentPostID > 0) Then
							'Dim displayCreatedDate As DateTime = ConvertTimeZone(MostRecentPostDate, objConfig)
							Dim lastPostInfo As New PostInfo
							Dim cntPost As New PostController()
							lastPostInfo = cntPost.GetPostInfo(objForum.MostRecentPostID, PortalID)

							Dim strLastPostInfo As String = Utilities.ForumUtils.GetCreatedDateInfo(objForum.MostRecentPost.CreatedDate, objConfig, "")
							' shows only first 15 letters of the post subject title
							Dim truncatedTitle As String
							If lastPostInfo.Subject.Length > 16 Then
								truncatedTitle = Left(lastPostInfo.Subject, 15) & "..."
							Else
								truncatedTitle = lastPostInfo.Subject
							End If

							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
							wr.RenderBeginTag(HtmlTextWriterTag.Div) ' <div>

							url = Utilities.Links.ContainerViewPostLink(TabID, objForum.ForumID, objForum.MostRecentPostID)
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
							wr.Write(Me.BaseControl.LocalizedText("by") & " ")
							wr.RenderEndTag() ' </span>

							url = String.Empty

							If objForum.MostRecentPost IsNot Nothing Then
								If Not objConfig.EnableExternalProfile Then
									If objForum.MostRecentPost.Author IsNot Nothing Then
										url = objForum.MostRecentPost.Author.UserCoreProfileLink
									End If
								Else
									If objForum.MostRecentPost.Author IsNot Nothing Then
										url = Utilities.Links.UserExternalProfileLink(objForum.MostRecentPost.Author.UserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objForum.MostRecentPost.Author.Username)
									End If
								End If
							End If

							wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText") 'Forum_AliasLink
							wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
							If objForum.MostRecentPost IsNot Nothing Then
								If objForum.MostRecentPost.Author IsNot Nothing Then
									wr.Write(objForum.MostRecentPost.Author.SiteAlias)
								End If
							End If

							wr.RenderEndTag() '  </A>
						Else
							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_LastPostText")
							wr.RenderBeginTag(HtmlTextWriterTag.Span) '<span>
							wr.Write(Me.BaseControl.LocalizedText("None"))
							wr.RenderEndTag() ' </span>
						End If
					End If

					wr.RenderEndTag() ' </Td>

					wr.AddAttribute(HtmlTextWriterAttribute.Width, "1px")
					wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

					wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
					wr.AddAttribute(HtmlTextWriterAttribute.Src, objConfig.GetThemeImageURL("row_spacer.gif"))
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

		''' <summary>
		''' Renders a list of subforum links
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="ParentID"></param>
		''' <remarks>Added by Skeel</remarks>
		Private Sub RenderSubForums(ByVal wr As HtmlTextWriter, ByVal ParentID As Integer, ByVal GroupId As Integer, ByVal Css As String)
			Dim Url As String
			Dim i As Integer = 1
			Dim SubForum As New ForumInfo
			Dim forumCtl As New ForumController
			Dim arrSubForums As List(Of ForumInfo) = forumCtl.GetChildForums(ParentID, GroupId, True)

			wr.RenderBeginTag(HtmlTextWriterTag.B) '<b>
			wr.Write(Localization.GetString("SubForums", objConfig.SharedResourceFile) & ": ")
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

						Url = DotNetNuke.Common.Globals.LinkClick(SubForum.ForumLink, objConfig.CurrentPortalSettings.ActiveTab.TabID, ModuleID, TrackClicks)
					Else
						If SubForum.GroupID = -1 Then
							' aggregated
							Url = Utilities.Links.ContainerAggregatedLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, False)
						Else
							Url = Utilities.Links.ContainerViewForumLink(objConfig.CurrentPortalSettings.ActiveTab.TabID, SubForum.ForumID, False)
						End If
					End If

					wr.AddAttribute(HtmlTextWriterAttribute.Href, Url.Replace("~/", ""))

					If Css.Length > 0 Then
						wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
					End If
					wr.RenderBeginTag(HtmlTextWriterTag.A) '<a>

					wr.Write(SubForum.Name)
					wr.RenderEndTag() ' </a>

					If i < arrSubForums.Count Then
						wr.Write(", ")
					End If

					i = i + 1
				End If
			Next
		End Sub

		''' <summary>
		''' Footer area of group section
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderFooter(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") '<td>

			RenderTableBegin(wr, "tblFooterContents", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapLeft", "") ' <td><img/></td>
			RenderCellBegin(wr, "Forum_Footer", "", "", "left", "", "", "") ' <td>

			Dim strFooter As String = " "
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				' There is a groupID, if its 0 its a parent
				' so see if it is a child (no group stats will be show if so)
				If CType(ForumControl.TabModuleSettings("groupid"), Integer) = 0 Then
					' CP - ADD 1/1/06 - See if a specific group is being shown
					If HttpContext.Current.Request.QueryString("groupid") Is Nothing Then
						Dim sb As New System.Text.StringBuilder
						strFooter = FooterStats(sb)
					End If
				End If
			Else
				' No setting so it is a parent
				' CP - ADD 1/1/06 - See if a specific group is being shown
				If HttpContext.Current.Request.QueryString("groupid") Is Nothing Then
					Dim sb As New System.Text.StringBuilder
					strFooter = FooterStats(sb)
				End If
			End If

			RenderDivBegin(wr, "spCounting", "Forum_FooterText") ' <span>
			wr.Write("&nbsp;")
			wr.Write(strFooter)
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapRight", "") ' <td><img/></td>

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </Tr>

			' This is the last row displayed in the UI, create some space below.
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "", "", "", "", "") '<td>
			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "", "", "", "", "") '<td>
			wr.Write("<br />")
			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "", "", "", "", "") '<td>
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>
		End Sub

		''' <summary>
		''' Replaces tokens for stats w/ actual numbers
		''' </summary>
		''' <param name="sb"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function FooterStats(ByVal sb As StringBuilder) As String
			sb.Append(ForumControl.LocalizedText("ForumsCountInfoPosts"))
			sb.Replace("[ForumCount]", AuthForumsCount.ToString)

			If objConfig.AggregatedForums Then
				sb.Replace("[GroupCount]", (AuthorizedGroups.Count - 1).ToString())
			Else
				sb.Replace("[GroupCount]", AuthorizedGroups.Count.ToString())
			End If

			Return sb.ToString
		End Function

		''' <summary>
		''' Determines if thread is even or odd numbered row
		''' </summary>
		''' <param name="Count"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
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

End Namespace