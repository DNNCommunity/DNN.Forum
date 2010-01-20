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
	''' All rendering is done in code to create UI and done or called from
	''' this class.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class Groups
		Inherits ForumObject

#Region "Private Declarations"

		Private mForumsCount As Integer = 0
		Private mAuthorizedForumsCount As Integer = 0
		Private a As Integer = 0
		Dim url As String
		Dim mGroupId As Integer = 0

		'[skeel] Subforums
		Dim mForumId As Integer = 0

		Dim _arrAuthGroups As List(Of GroupInfo)
		Dim _arrAuthForumsCount As Integer = 0

#End Region

#Region "Public Properties"

		''' <summary>
		''' Collection of groups
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property arrAuthGroups() As List(Of GroupInfo)
			Get
				Return _arrAuthGroups
			End Get
			Set(ByVal Value As List(Of GroupInfo))
				_arrAuthGroups = Value
			End Set
		End Property

		''' <summary>
		''' Number of active forums in the group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property arrAuthForumsCount() As Integer
			Get
				Return _arrAuthForumsCount
			End Get
			Set(ByVal Value As Integer)
				_arrAuthForumsCount = Value
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
			arrAuthGroups = cntGroup.GroupGetAllAuthorized(ModuleID, ForumControl.LoggedOnUser.UserID, False)
			mAuthorizedForumsCount = 0

			If arrAuthGroups.Count > 0 Then
				Dim objGroup As New GroupInfo

				For Each objGroup In arrAuthGroups
					Dim arrAuthForums As New List(Of ForumInfo)
					arrAuthForums = objGroup.AuthorizedForums(ForumControl.LoggedOnUser.UserID, False)
					If arrAuthForums.Count > 0 Then
						arrAuthForumsCount += arrAuthForums.Count
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
			'[skeel] load in the url requests, if any
			If Not HttpContext.Current.Request.QueryString("groupid") Is Nothing Then
				' assign a specific groupid so we only show a single group
				mGroupId = CType(HttpContext.Current.Request.QueryString("groupid"), Integer)
			End If
			If Not HttpContext.Current.Request.QueryString("forumid") Is Nothing Then
				' assign a specific groupid so we only show a single group
				mForumId = CType(HttpContext.Current.Request.QueryString("forumid"), Integer)
			End If

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
			If mForumId > 0 And mGroupId > 0 Then
				'Parent Forum view
				Dim cltForum As New ForumController
				Dim objForumInfo As ForumInfo
				objForumInfo = cltForum.GetForumInfoCache(mForumId)
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Groups, objForumInfo, objConfig, ChildGroupView))
			ElseIf mGroupId > 0 Then
				'Group view
				Dim cltGroups As New GroupController
				Dim objGroupInfo As GroupInfo
				objGroupInfo = cltGroups.GroupGet(mGroupId)
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Groups, objGroupInfo, objConfig, ChildGroupView))
			Else
				'Forum Home view
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Groups, Nothing, objConfig, ChildGroupView))
				RenderCellEnd(wr) ' </td>
				RenderCellBegin(wr, "Forum_LastPostText", "", "", "right", "", "", "") ' <td>

				'View latest x hours
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
				If ForumControl.LoggedOnUser.UserID > 0 Then
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
			If arrAuthGroups.Count > 0 Then
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
			If arrAuthGroups.Count > 0 Then
				RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
				wr.Write(ForumControl.LocalizedText("Threads"))
				RenderDivEnd(wr) ' </span>
			End If
			RenderCellEnd(wr) ' </td>

			' Posts column 
			RenderCellBegin(wr, "Forum_Header", "", "11%", "center", "middle", "", "")	 '<td>
			If arrAuthGroups.Count > 0 Then
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
			If arrAuthGroups.Count > 0 Then
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
			If arrAuthGroups.Count > 0 Then
				' get the group specification ( if it exists )
				Dim GroupID As Integer = 0
				If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
					If CType(ForumControl.TabModuleSettings("groupid"), Integer) = 0 Then
						' We know here group feature (parent/child) is not being used, check for url set item to show single group
						If mGroupId > 0 Then
							' assign a specific groupid so we only show a single group
							GroupID = mGroupId
						End If
					Else
						GroupID = CType(ForumControl.TabModuleSettings("groupid"), Integer)
					End If
				Else	' from else to end of if is correct
					' We know here group feature (parent/child) is not being used, check for url set item to show single group
					If mGroupId > 0 Then
						' assign a specific groupid so we only show a single group
						GroupID = mGroupId
					End If
				End If
				Dim objGroup As New GroupInfo
				'Dim objForum As New ForumInfo

				If ForumControl.objConfig.AggregatedForums Then
					objGroup = New GroupInfo
					objGroup.PortalID = PortalID
					objGroup.ModuleID = ModuleID
					objGroup.GroupID = -1
					objGroup.Name = Localization.GetString("AggregatedGroupName", ForumControl.objConfig.SharedResourceFile)
					arrAuthGroups.Insert(0, objGroup)
				End If
				' group is expand or collapse depends on user settings handled a few lines below
				For Each objGroup In arrAuthGroups
					' filter based on group:  - 1 means show all, matching the groupid to the current groupid in the colleciton means show a single one
					If GroupID = 0 Or GroupID = objGroup.GroupID Then
						'[skeel] Subforums
						Dim arrForums As List(Of ForumInfo)

						If mForumId > 0 Then
							arrForums = objGroup.AuthorizedSubForums(ForumControl.LoggedOnUser.UserID, False, mForumId)
						Else
							arrForums = objGroup.AuthorizedNoParentForums(ForumControl.LoggedOnUser.UserID, False)
						End If

						' display group only if group contains atleast one authorized forum
						If arrForums.Count > 0 Then
							'[skeel] Subforums
							If mForumId > 0 Then
								RenderGroupForumInfo(wr, objGroup)
							Else
								RenderGroupInfo(wr, objGroup)
							End If

							'If the group is not collapsed
							'If (Not LoggedOnUser.GroupIsCollapsed(_groupInfo.GroupID)) Then
							Dim Count As Integer = 1

							' Render a row for each forum in this group exposed to this user
							For Each objForum As ForumInfo In arrForums
								objForum.Render(wr, Me.ForumControl, Count)
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
		''' <history>
		''' 	[skeel]	12/11/2008	Created
		''' </history>
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
			forum = cntForum.GetForumInfoCache(mForumId)


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
		End Sub

		''' <summary>
		''' Replaces tokens for stats w/ actual numbers
		''' </summary>
		''' <param name="sb"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function FooterStats(ByVal sb As StringBuilder) As String
			sb.Append(ForumControl.LocalizedText("ForumsCountInfoPosts"))
			sb.Replace("[ForumCount]", arrAuthForumsCount.ToString)

			If objConfig.AggregatedForums Then
				sb.Replace("[GroupCount]", (arrAuthGroups.Count - 1).ToString())
			Else
				sb.Replace("[GroupCount]", arrAuthGroups.Count.ToString())
			End If

			Return sb.ToString
		End Function

#End Region

	End Class

End Namespace