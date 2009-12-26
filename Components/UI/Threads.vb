'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2009
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
	''' This renders the threads view (second view in hierarchy of forum)
	''' </summary>
	''' <remarks>All UI is done in code, no corresponding ascx
	''' </remarks>
	Public Class Threads
		Inherits ForumObject

#Region "Private Declarations"

		Private _ParentForum As ForumInfo
		Private _ThreadCollection As New List(Of ThreadInfo)
		Private _ThreadPage As Integer = 0
		Private _Filter As String = String.Empty
		Private _NoReply As Boolean = False
		Dim TotalRecords As Integer = 0
		Dim url As String
		Private hsThreadRatings As New Hashtable
		'Dim Security As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, ForumControl.LoggedOnUser.UserID)

#Region "Controls"

		Private ddlDateFilter As Telerik.Web.UI.RadComboBox
		Private cmdRead As LinkButton
		Private chkEmail As CheckBox
		Private txtForumSearch As TextBox
		Private cmdForumSearch As System.Web.UI.WebControls.ImageButton
		Private trcRating As Telerik.Web.UI.RadRating

#End Region

#End Region

#Region "Private Properties"

		''' <summary>
		'''  The containing Forum Info object. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property ParentForum() As ForumInfo
			Get
				Return _ParentForum
			End Get
		End Property

		''' <summary>
		''' The ForumID the thread belongs too. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property ForumId() As Integer
			Get
				Return ForumControl.GenericObjectID
			End Get
		End Property

		''' <summary>
		''' The collection of threads returned. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property ThreadCollection() As List(Of ThreadInfo)
			Get
				Return _ThreadCollection
			End Get
		End Property

		''' <summary>
		''' Page user is on in this view.  
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property ThreadPage() As Integer
			Get
				Return _ThreadPage
			End Get
		End Property

		''' <summary>
		''' The item used to filter the returned threads. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This can be things such as date</remarks>
		Private ReadOnly Property Filter() As String
			Get
				Return _Filter
			End Get
		End Property

		''' <summary>
		''' If a thread has new posts or not using the UserReads methods. 
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="objThreadInfo"></param>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property HasNewPosts(ByVal UserID As Integer, ByVal objThreadInfo As ThreadInfo) As Boolean
			Get
				Dim userthreadController As New UserThreadsController
				Dim userthread As New UserThreadsInfo

				If UserID > 0 Then
					If objThreadInfo Is Nothing Then
						Return True
					Else
						userthread = userthreadController.GetCachedUserThreadRead(UserID, objThreadInfo.ThreadID)
						If userthread Is Nothing Then
							Return True
						Else
							If userthread.LastVisitDate < objThreadInfo.LastApprovedPost.CreatedDate Then
								Return True
							Else
								Return False
							End If
						End If
					End If
				Else
					Return True
				End If
			End Get
		End Property

		''' <summary>
		''' Used to retrieve only posts with no replies if true.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property NoReply() As Boolean
			Get
				Return _NoReply
			End Get
			Set(ByVal Value As Boolean)
				_NoReply = Value
			End Set
		End Property

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Filters the threads show by date on postback
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub ddlDateFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim dFilter As String = ddlDateFilter.SelectedItem.Value

			If Not HttpContext.Current.Request.QueryString("noreply") Is Nothing Then
				NoReply = True
			End If
			url = Utilities.Links.ContainerThreadDateFilterLink(TabID, dFilter, ForumId, NoReply)

			If ForumControl.LoggedOnUser.UserID > 0 Then
				'update the user profile
				Dim fUserCnt As New ForumUserController
				fUserCnt.UserUpdateTrackingDuration(CType(dFilter, Integer), ForumControl.LoggedOnUser.UserID, ForumControl.PortalID)
				Forum.ForumUserController.ResetForumUser(ForumControl.LoggedOnUser.UserID, PortalID)
			End If

			HttpContext.Current.Response.Redirect(url, False)
		End Sub

		''' <summary>
		''' Marks all threads in the forum as unread 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdRead_Clicked(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim userThreadController As New UserThreadsController
			userThreadController.MarkAll(ForumControl.LoggedOnUser.UserID, ForumId, True)
		End Sub

		''' <summary>
		''' Toggles user notification for thread on/off on postback
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub chkEmail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
			Dim ctlTracking As New TrackingController
			ctlTracking.TrackingForumCreateDelete(ForumId, ForumControl.LoggedOnUser.UserID, CType(sender, CheckBox).Checked, ModuleID)
		End Sub

		''' <summary>
		''' This directs the user to the search results of this particular forum. It searches this forum and the subject, body of the post. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdForumSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
			url = Utilities.Links.ContainerSingleForumSearchLink(TabID, ForumId, txtForumSearch.Text)
			MyBase.BasePage.Response.Redirect(url, False)
		End Sub

#End Region

#Region "Public Methods"

#Region "Constructors"

		''' <summary>
		''' Creates a new instance of this class
		''' </summary>
		''' <param name="forum"></param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal forum As DNNForum)
			MyBase.New(forum)

			Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, ForumControl.LoggedOnUser.UserID)

			' User might access this page by typing url so better check permissions of parent forum
			If ForumControl.GenericObjectID = -1 Then
				_ParentForum = New ForumInfo
				_ParentForum.ModuleID = ModuleID
				_ParentForum.GroupID = -1
				_ParentForum.ForumID = -1
			Else
				Dim cntForum As New ForumController
				_ParentForum = cntForum.GetForumInfoCache(ForumControl.GenericObjectID)

				' Make sure the forum is not disabled
				If Not _ParentForum.IsActive Then
					HttpContext.Current.Response.Redirect(Utilities.Links.NoContentLink(TabID, ModuleID), False)
				End If

				If objConfig.OverrideTitle Then
					Me.BaseControl.BasePage.Title = _ParentForum.Name & " - " & Me.BaseControl.PortalName
				End If

				' User might access this page by typing url so better check permission on parent forum
				If Not (_ParentForum.PublicView) Then
					' The forum is private, see if we have proper view perms here
					If Not objSecurity.IsAllowedToViewPrivateForum Then
						HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
					End If
				End If
			End If

			' We need to make sure the user's thread pagesize can handle this 
			'(problem is, a link can be posted by one user w/ page size of 5 pointing to page 2, if logged in user has pagesize set to 15, there is no page 2)
			If Not HttpContext.Current.Request.QueryString("currentpage") Is Nothing Then
				Dim urlThreadPage As Integer = Int32.Parse(HttpContext.Current.Request.QueryString("currentpage"))
				Dim TotalThreads As Integer = ParentForum.TotalThreads
				Dim userThreadsPerPage As Integer

				If ForumControl.LoggedOnUser.UserID > 0 Then
					userThreadsPerPage = ForumControl.LoggedOnUser.ThreadsPerPage
				Else
					userThreadsPerPage = objConfig.ThreadsPerPage
				End If

				Dim TotalPages As Integer = CInt(Math.Ceiling(TotalThreads / userThreadsPerPage))
				Dim ThreadPageToShow As Integer

				' We need to check if it is possible for a pagesize seen in the URL for the user browsing (happens when coming from posted link by other user)
				If TotalPages >= urlThreadPage Then
					ThreadPageToShow = urlThreadPage
				Else
					' We know for this user, total pages > user posts per page. Because of this, we know its not user using page change so show thread as normal
					ThreadPageToShow = 0
				End If

				_ThreadPage = ThreadPageToShow
			End If

			Dim Term As New SearchTerms

			If ForumControl.LoggedOnUser.UserID > -1 Then
				Dim dateFilter As Integer = ForumControl.LoggedOnUser.TrackingDuration

				If dateFilter > 999 Then
					' we are not going to add a searchTerm, this means get all threads from all days
				ElseIf dateFilter = -1 Then ' Last Activity
					Dim DateDiff As TimeSpan
					Dim DateString As String
					DateDiff = Date.Now.Subtract(ForumControl.LoggedOnUser.LastActivity)
					DateString = " DATEADD(hh, " & -DateDiff.Hours & ", GETDATE()) "
					Term.AddSearchTerm("FP.CreatedDate", CompareOperator.GreaterThan, DateString)
				ElseIf (dateFilter = 0) Then ' Today
					Dim DateDiff As TimeSpan
					Dim DateString As String
					DateDiff = Date.Now.Subtract(Date.Today())
					DateString = " DATEADD(hh, " & -DateDiff.Hours & ", GETDATE()) "
					Term.AddSearchTerm("FP.CreatedDate", CompareOperator.GreaterThanDate, DateString)
				Else
					' get threads by date interval
					Dim DateString As String
					DateString = " DATEADD(dd, " & -dateFilter & ", GETDATE()) "
					Term.AddSearchTerm("FP.CreatedDate", CompareOperator.GreaterThanDate, DateString)
				End If
			Else
				' Add date filter
				If Not HttpContext.Current.Request.QueryString("datefilter") Is Nothing Then
					Dim dateFilter As Integer = Int16.Parse(HttpContext.Current.Request.QueryString("datefilter"))

					If (dateFilter = 0) Then	' Today
						Dim DateDiff As TimeSpan
						Dim DateString As String
						DateDiff = Date.Now.Subtract(Date.Today())
						DateString = " DATEADD(hh, " & -DateDiff.Hours & ", GETDATE()) "
						Term.AddSearchTerm("FP.CreatedDate", CompareOperator.GreaterThanDate, DateString)
					ElseIf (Not dateFilter > 999) Then
						' get threads by date interval
						Dim DateString As String
						DateString = " DATEADD(dd, " & -dateFilter & ", GETDATE()) "
						Term.AddSearchTerm("FP.CreatedDate", CompareOperator.GreaterThanDate, DateString)
					End If
				End If
			End If

			If Not HttpContext.Current.Request.QueryString("noreply") Is Nothing Then
				NoReply = True
			End If

			If NoReply Then
				Term.AddSearchTerm("T.Replies", CompareOperator.EqualString, "0")
			End If

			_Filter = Term.WhereClause

			If _ThreadPage > 0 Then
				_ThreadPage = _ThreadPage - 1
			End If
		End Sub

#End Region

		''' <summary>
		''' Create an instance of the controls used here
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Overrides Sub CreateChildControls()
			Controls.Clear()

			' display tracking option only if user authenticated
			If ForumControl.LoggedOnUser.UserID > 0 Then
				cmdRead = New LinkButton
				With cmdRead
					.CssClass = "Forum_Link"
					.ID = "chkRead"
					.Text = ForumControl.LocalizedText("MarkThreadAsRead")
				End With

				chkEmail = New CheckBox
				With chkEmail
					.CssClass = "Forum_NormalTextBox"
					.ID = "chkEmail"
					.Text = ForumControl.LocalizedText("MailWhenNewThread")
					.TextAlign = TextAlign.Left
					.AutoPostBack = True
					.Checked = False
				End With
			End If

			ddlDateFilter = New Telerik.Web.UI.RadComboBox
			With ddlDateFilter
				'.CssClass = "Forum_NormalTextBox"
				.Skin = "WebBlue"
				.ID = "lstDateFilter"
				.Width = Unit.Parse("160")
				.AutoPostBack = True
				.ClearSelection()
			End With

			txtForumSearch = New TextBox
			With txtForumSearch
				.CssClass = "Forum_NormalTextBox"
				.ID = "txtForumSearch"
			End With

			Me.cmdForumSearch = New ImageButton
			With cmdForumSearch
				.CssClass = "Forum_Profile"
				.ID = "cmdForumSearch"
				.AlternateText = ForumControl.LocalizedText("Search")
				.ToolTip = ForumControl.LocalizedText("Search")
				.ImageUrl = objConfig.GetThemeImageURL("s_lookup.") & objConfig.ImageExtension
			End With

			BindControls()
			AddControlHandlers()
			AddControlsToTree()

			For Each thread As ThreadInfo In ThreadCollection
				Me.trcRating = New Telerik.Web.UI.RadRating
				With trcRating
					.Enabled = False
					.Skin = "Office2007"
					.Width = Unit.Parse("200")
					.SelectionMode = Telerik.Web.UI.RatingSelectionMode.Continuous
					.IsDirectionReversed = False
					.Orientation = Orientation.Horizontal
					.Precision = Telerik.Web.UI.RatingPrecision.Half
					.ItemCount = objConfig.RatingScale

					.ID = "trcRating" + thread.ThreadID.ToString()
					.Value = thread.Rating
					'AddHandler trcRating.Command, AddressOf trcRating_Rate
				End With
				hsThreadRatings.Add(thread.ThreadID, trcRating)
				Controls.Add(trcRating)
			Next
		End Sub

		''' <summary>
		''' Builds the control view seen on the page * This is the final step in 
		''' in this views lifecycle (that is used)
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Public Overrides Sub Render(ByVal wr As HtmlTextWriter)
			'update the UserForum record
			If ForumControl.Page.Request.IsAuthenticated And ForumId <> -1 Then
				Dim userForumController As New UserForumsController
				Dim userForum As New UserForumsInfo

				userForum = userForumController.GetCachedUserForumRead(ForumControl.LoggedOnUser.UserID, ForumId)

				If Not userForum Is Nothing Then
					userForum.LastVisitDate = Now
					userForumController.Update(userForum)
					UserForumsController.ResetUserForumReadCache(ForumControl.LoggedOnUser.UserID, ForumId)
				Else
					userForum = New UserForumsInfo
					With userForum
						.UserID = ForumControl.LoggedOnUser.UserID
						.ForumID = ForumId
						.LastVisitDate = Now
					End With
					userForumController.Add(userForum)
					UserForumsController.ResetUserForumReadCache(ForumControl.LoggedOnUser.UserID, ForumId)
				End If
				ForumController.ResetForumInfoCache(ForumId)
			End If

			Dim AggregatedView As Boolean = False
			If ForumId = -1 Then
				'AggregatedView = True
				' Redirect the user to the aggregated view (Prior to 4.4.4, aggregated used this view so we have to handle legacy links)
				HttpContext.Current.Response.Redirect(Utilities.Links.ContainerAggregatedLink(TabID, False), True)
			End If

			RenderTableBegin(wr, 0, 0, "tblThreads")
			RenderNavBar(wr, objConfig, ForumControl)
			RenderSearchBarRow(wr)
			RenderBreadCrumbRow(wr)
			RenderThreads(wr)
			RenderFooterRow(wr)
			RenderBottomBreadCrumbRow(wr)
			RenderTableEnd(wr)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Loads all the handlers for the controls used in this view
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub AddControlHandlers()
			Try
				If ForumControl.LoggedOnUser.UserID > 0 Then
					AddHandler cmdRead.Click, AddressOf cmdRead_Clicked
					AddHandler chkEmail.CheckedChanged, AddressOf chkEmail_CheckedChanged
				End If

				AddHandler ddlDateFilter.SelectedIndexChanged, AddressOf ddlDateFilter_SelectedIndexChanged
				AddHandler cmdForumSearch.Click, AddressOf cmdForumSearch_Click
				' would add rating control handler here if we permit rating in thread view in future. (may move to per post rating, in which case this would always be no). 
			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Loads the controls early on in the control's lifecycle so they can be used later on
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub AddControlsToTree()
			Try
				If ForumControl.LoggedOnUser.UserID > 0 Then
					Controls.Add(cmdRead)
					Controls.Add(chkEmail)
				End If
				Controls.Add(ddlDateFilter)
				Controls.Add(txtForumSearch)
				Controls.Add(cmdForumSearch)
			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Binds the controls used in this view, happens each postback too
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub BindControls()
			Try
				ddlDateFilter.Items.Clear()

				For Each entry As DotNetNuke.Common.Lists.ListEntryInfo In DotNetNuke.Modules.Forum.Utilities.ForumUtils.GetTrackingDurationList
					If ForumControl.LoggedOnUser.UserID > 0 Then
						Dim dateEntry As New Telerik.Web.UI.RadComboBoxItem(Localization.GetString(entry.Text, ForumControl.objConfig.SharedResourceFile), entry.Value)
						ddlDateFilter.Items.Add(dateEntry)
					Else
						Dim dateEntry As New Telerik.Web.UI.RadComboBoxItem(Localization.GetString(entry.Text, ForumControl.objConfig.SharedResourceFile), entry.Value)
						If Not CInt(dateEntry.Value) = -1 Then
							ddlDateFilter.Items.Add(dateEntry)
						End If
					End If
				Next
				Dim dateFilter As Integer = 1000

				' create child control date filter dropdownlist
				If Not HttpContext.Current.Request.QueryString("datefilter") Is Nothing Then
					dateFilter = Int16.Parse(HttpContext.Current.Request.QueryString("datefilter"))
				Else
					' Tracking Duration
					If ForumControl.LoggedOnUser.UserID > 0 Then
						dateFilter = ForumControl.LoggedOnUser.TrackingDuration
					End If
				End If

				ddlDateFilter.SelectedValue = dateFilter.ToString()

				If ForumControl.LoggedOnUser.UserID > 0 And objConfig.MailNotification Then
					' We check if user is subscribed in this forum
					Dim blnTrackedForum As Boolean = False

					For Each objTrackedForum As TrackingInfo In ForumControl.LoggedOnUser.TrackedForums
						If objTrackedForum.ForumID = ForumId Then
							blnTrackedForum = True
							Exit For
						End If
					Next

					chkEmail.Visible = True
					chkEmail.Checked = blnTrackedForum
				End If

				' Now we get threads to display for this user
				Dim ctlThread As New ThreadController
				_ThreadCollection = ctlThread.ThreadGetAll(ModuleID, ForumId, ForumControl.ThreadsPerPage, ThreadPage, Filter, PortalID)

			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Renders the Rating selector, current rating image, search textbox and button
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderSearchBarRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>

			' left cap
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")

			RenderCellBegin(wr, "", "", "100%", "", "", "", "")
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")
			RenderRowBegin(wr) '<tr>

			RenderCellBegin(wr, "", "", "100%", "right", "", "", "")
			'Start table to hold module action buttons
			RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")
			RenderRowBegin(wr) '<tr>

			' changed from forumpost because we have no rating here
			RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td> 
			wr.Write("&nbsp;")
			RenderCellEnd(wr) ' </td>
			' end changed

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>

			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "", "right", "middle", "", "") ' <td>

			RenderTableBegin(wr, 0, 0, "InnerTable") '<table>
			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td>
			txtForumSearch.RenderControl(wr)
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td>
			cmdForumSearch.RenderControl(wr)
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			' right cap
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			wr.RenderEndTag() ' </Tr>
		End Sub

		''' <summary>
		''' breadcrumb
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderBreadCrumbRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>

			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "left", "bottom", "", "")
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "", "", "100%", "", "", "", "") ' <td>
			Dim ChildGroupView As Boolean = False
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				ChildGroupView = True
			End If

			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "", "", "80%", "left", "", "", "")	' <td>
			wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Threads, ParentForum, objConfig, ChildGroupView))
			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "20%", "right", "", "", "") ' <td>

			If NoReply Then
				' show link to show all threads
				RenderLinkButton(wr, Utilities.Links.ContainerViewForumLink(TabID, ForumId, False), Localization.GetString("ShowAll", objConfig.SharedResourceFile), "Forum_BreadCrumb")
			Else
				' show link to show no reply threads
				RenderLinkButton(wr, Utilities.Links.ContainerViewForumLink(TabID, ForumId, True), Localization.GetString("ShowNoReplies", objConfig.SharedResourceFile), "Forum_BreadCrumb")
			End If

			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>

			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "100%", "left", "", "2", "") ' <td>
			'Remove LoggedOnUserID limitation if wishing to implement Anonymous Posting
			If (ForumControl.LoggedOnUser.UserID > 0) And (Not ForumId = -1) Then
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, ForumControl.LoggedOnUser.UserID)

				If Not ParentForum.PublicPosting Then
					If objSecurity.IsAllowedToStartRestrictedThread Then
						RenderTableBegin(wr, "", "", "", "", "4", "0", "", "", "0")	'<Table>            
						RenderRowBegin(wr) '<tr>
						url = Utilities.Links.NewThreadLink(TabID, ForumId, ModuleID)
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

						If ForumControl.LoggedOnUser.IsBanned Then
							RenderLinkButton(wr, url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
						Else
							RenderLinkButton(wr, url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
						End If

						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td> 
						RenderCellEnd(wr) ' </Td>
						RenderRowEnd(wr) ' </tr>

						RenderRowBegin(wr) '<tr>
						RenderCellBegin(wr, "", "", "", "", "middle", "", "")	'<td>   
						RenderImage(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td> 
						RenderCellEnd(wr) ' </Td>
						RenderRowEnd(wr) ' </tr>

						RenderTableEnd(wr) ' </table>
					Else
						wr.Write("&nbsp;")
					End If
				Else
					RenderTableBegin(wr, "", "", "", "", "0", "0", "", "", "0")	'<Table>            
					RenderRowBegin(wr) '<tr>
					url = Utilities.Links.NewThreadLink(TabID, ForumId, ModuleID)
					RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

					If ForumControl.LoggedOnUser.IsBanned Then
						RenderLinkButton(wr, url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
					Else
						RenderLinkButton(wr, url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
					End If

					RenderCellEnd(wr) ' </Td>
					RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td> 
					RenderCellEnd(wr) ' </Td>
					RenderRowEnd(wr) ' </tr>

					RenderRowBegin(wr) '<tr>
					RenderCellBegin(wr, "", "", "", "", "middle", "", "")	'<td>   
					RenderImage(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
					RenderCellEnd(wr) ' </td>
					RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td> 
					RenderCellEnd(wr) ' </Td>
					RenderRowEnd(wr) ' </tr>

					RenderTableEnd(wr) ' </table>
				End If
			End If
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </Td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderRowEnd(wr) ' </Tr>
		End Sub

		Public Overrides Sub OnPreRender()
			' To permit ajax usage for some things, throw a script manager on the page
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
				'DotNetNuke.Framework.AJAX.RegisterPostBackControl(trcRating)
				'DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(trcRating, False)
			End If
		End Sub

		''' <summary>
		'''  top header w/ subject after bredcrumb/solpart row, before post/avatar row
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderThreads(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) ' <tr>

			' left cap 
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	'<td><img/></td>

			'CP-** This td must not be closed at end of this function, move that and right cap to end of next function this is the spanning master column
			'middle column, contains everything seen in thread view (using table)  - This needs to span into next function
			RenderCellBegin(wr, "", "", "100%", "center", "", "", "") ' <td>

			'Create a table here that will hold everything from the top header type colum (says Thread, Replies, Views, Last Post) through each thread but stop at the footer w/ this table
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "") ' <table>
			RenderRowBegin(wr) ' <Tr>

			' Threads column
			RenderCellBegin(wr, "", "", "52%", "", "middle", "", "") '<td>
			'This table is made simply so we can have a height controlling image and apply left cap here
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "center", "", "0")	'<table>
			RenderRowBegin(wr) ' <Tr>

			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapLeft", "") ' <td></td>

			RenderCellBegin(wr, "Forum_Header", "", "", "left", "", "", "")	' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <div>
			wr.Write("&nbsp;" & ForumControl.LocalizedText("Threads"))
			RenderDivEnd(wr) ' </div>
			RenderCellEnd(wr) ' </Td>

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			' Replies
			RenderCellBegin(wr, "Forum_Header", "", "11%", "center", "", "", "") ' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <div>
			wr.Write(ForumControl.LocalizedText("Replies"))
			RenderDivEnd(wr) ' </div>
			RenderCellEnd(wr) ' </td>

			' Views column
			RenderCellBegin(wr, "Forum_Header", "", "11%", "center", "", "", "") '<td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <div>
			wr.Write(ForumControl.LocalizedText("Views"))
			RenderDivEnd(wr) ' </div>
			RenderCellEnd(wr) ' </td>

			' Last Post column
			RenderCellBegin(wr, "", "", "26%", "center", "", "", "") ' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") '<table>
			RenderRowBegin(wr) ' <Tr>

			RenderCellBegin(wr, "Forum_Header", "", "", "center", "", "", "") ' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <div>
			wr.Write("&nbsp;" & ForumControl.LocalizedText("LastPost"))
			RenderDivEnd(wr) ' </div>
			RenderCellEnd(wr) ' </td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapRight", "") ' <td><img/></td>

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			' end middle table/colum
			RenderRowEnd(wr) ' </Tr>

			RenderThreadInfo(wr)
		End Sub

		''' <summary>
		''' creates the rows that holds each thread
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderThreadInfo(ByVal wr As HtmlTextWriter)
			Try
				Dim Count As Integer = 1

				'loop through each post and make a new row within this table
				Dim thread As New ThreadInfo()
				For Each thread In ThreadCollection
					If Not thread Is Nothing Then
						Dim even As Boolean = ThreadIsEven(Count)
						TotalRecords = thread.TotalRecords

						RenderRowBegin(wr) ' <Tr>

						' cell holds table for post icon/thread subject/rating
						If even Then
							RenderCellBegin(wr, "Forum_Row", "100%", "52%", "", "", "", "") ' <td>
						Else
							RenderCellBegin(wr, "Forum_Row_Alt", "100%", "52%", "", "", "", "") ' <td>
						End If

						' table holds post icon/thread subject/rating
						RenderTableBegin(wr, "", "", "100%", "100%", "0", "0", "", "", "0") ' <table>
						RenderRowBegin(wr) ' <tr>

						' cell within table for thread status icon
						RenderCellBegin(wr, "", "100%", "", "", "top", "", "")	' <td>
						url = Utilities.Links.ContainerViewThreadLink(TabID, ForumId, thread.ThreadID)

						'link so icon is clickable (also used below for subject)
						wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
						wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>

						' see if post is pinned, priority over other icons
						If thread.IsPinned Then
							' First see if the thread is popular
							If (thread.IsPopular) Then
								' thread IS popular and pinned
								' see if thread is locked
								If (thread.IsClosed) Then
									' thread IS popular, pinned, locked
									' See if this is an unread post
									If (HasNewPosts(ForumControl.LoggedOnUser.UserID, thread)) Then
										' IS read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postlockedpinnedunreadplu.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotNewLockedPinnedThread"), "")
									Else
										' not read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postlockedpinnedreadplus.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotLockedPinnedThread"), "")
									End If
								Else
									' thread IS popular, Pinned but NOT locked
									' See if this is an unread post
									If (HasNewPosts(ForumControl.LoggedOnUser.UserID, thread)) Then
										' IS read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedunreadplus.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewHotPinnedThread"), "")
									Else
										' not read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedreadplus.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotPinnedThread"), "")
									End If
								End If
							Else
								' thread NOT popular but IS pinned
								' see if thread is locked
								If (thread.IsClosed) Then
									' thread IS pinned, Locked but NOT popular
									If (HasNewPosts(ForumControl.LoggedOnUser.UserID, thread)) Then
										' IS read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedlockedunread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewPinnedLockedThread"), "")
									Else
										' not read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedlockedread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgPinnedLockedThread"), "")
									End If
								Else
									'thread IS pinned but NOT popular, Locked
									If (HasNewPosts(ForumControl.LoggedOnUser.UserID, thread)) Then
										' IS read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedunread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewPinnedThread"), "")
									Else
										' not read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgPinnedThread"), "")
									End If
								End If
							End If
						Else
							' thread not pinned, determine post icon
							RenderImage(wr, GetMediaURL(thread), GetMediaText(thread), "") ' <img/>
						End If

						wr.RenderEndTag() ' </A>
						RenderCellEnd(wr) ' </td>

						' Spacing between status icon and subject
						RenderCellBegin(wr, "", "", "1px", "left", "", "", "")	' <td>
						RenderImage(wr, objConfig.GetThemeImageURL("row_spacer.gif"), "", "")	' <img/>
						RenderCellEnd(wr) ' </td>

						' cell for thread subject
						RenderCellBegin(wr, "", "", "100%", "left", "", "", "") ' <td>

						wr.AddAttribute(HtmlTextWriterAttribute.Href, url)

						Dim SubjectCssClass As String
						If (HasNewPosts(ForumControl.LoggedOnUser.UserID, thread)) Then
							SubjectCssClass = "Forum_NormalBold"
						Else
							SubjectCssClass = "Forum_Normal"
						End If
						wr.AddAttribute(HtmlTextWriterAttribute.Class, SubjectCssClass)
						' Below would enable tooltip when over for thread link showing last post body, this adds extra page size we don't want here for now
						'wr.AddAttribute(HtmlTextWriterAttribute.Title, thread.LastPostShortBody)
						wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>

						' Format prohibited words
						If ForumControl.objConfig.FilterSubject Then
							wr.Write(Utilities.ForumUtils.FormatProhibitedWord(thread.Subject, thread.CreatedDate, PortalID))
						Else
							wr.Write(thread.Subject)
						End If

						RenderDivEnd(wr) ' </div> - CP - I am not sure why this has to be here

						RenderDivBegin(wr, "", "Forum_NormalSmall") ' <div>
						wr.Write(String.Format("{0}&nbsp;", ForumControl.LocalizedText("by")))
						url = Utilities.Links.UserPublicProfileLink(TabID, ModuleID, thread.StartedByUserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, thread.StartedByUser.Username)
						RenderLinkButton(wr, url, thread.StartedByUser.SiteAlias, "Forum_NormalSmall") ' <a/>

						' correct logic to handle posts per page per user
						Dim userPostsPerPage As Integer
						' CapPageCount is number of pages to show as option for user in threads view.
						Dim CapPageCount As Integer = objConfig.PostPagesCount

						If ForumControl.LoggedOnUser.UserID > 0 Then
							userPostsPerPage = ForumControl.LoggedOnUser.PostsPerPage
						Else
							userPostsPerPage = objConfig.PostsPerPage
						End If

						Dim UserPagesCount As Integer = CInt(Math.Ceiling((thread.TotalPosts) / userPostsPerPage))
						Dim ShowFinalPage As Boolean = (UserPagesCount > CapPageCount)

						' Only show Pager if there is more than 1 page for the user
						If UserPagesCount > 1 Then
							' If thread spans several pages, then display text like (Page 1, 2, 3, ..., 5)

							wr.Write(" (" & ForumControl.LocalizedText("Page") & ": ")

							If UserPagesCount >= CapPageCount Then
								For ThreadPage As Integer = 1 To CapPageCount - 1
									url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumId, thread.ThreadID, ThreadPage)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
									wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalSmall")
									wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
									wr.Write(ThreadPage.ToString())
									wr.RenderEndTag() ' </a>
									If (ThreadPage < CapPageCount - 1) Or (ShowFinalPage And ThreadPage = CapPageCount) Then
										wr.Write(", ")
									End If
								Next

								If ShowFinalPage Then
									wr.Write("..., ")
									url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumId, thread.ThreadID, UserPagesCount)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
									wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalSmall")
									wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
									wr.Write(UserPagesCount.ToString())
									wr.RenderEndTag() ' </A>
								End If
							Else

								For ThreadPage As Integer = 1 To UserPagesCount
									url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumId, thread.ThreadID, ThreadPage)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
									wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalSmall")
									wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
									wr.Write(ThreadPage.ToString())
									wr.RenderEndTag() ' </a>
									If (ThreadPage < UserPagesCount) Or (ShowFinalPage And ThreadPage = UserPagesCount) Then
										wr.Write(", ")
									End If
								Next
							End If

							wr.Write(")")
						End If

						RenderDivEnd(wr) ' </div>
						RenderCellEnd(wr) ' </td>

						' CP - Add check for RatingsEnabled
						If objConfig.EnableRatings And thread.HostForum.EnableForumsRating Then ' And thread.Rating > 0
							RenderCellBegin(wr, "", "", "30%", "right", "", "", "") ' <td>

							If hsThreadRatings.ContainsKey(thread.ThreadID) Then
								trcRating = CType(hsThreadRatings(thread.ThreadID), Telerik.Web.UI.RadRating)
								' CP - we alter statement below if we want to enable 0 rating still showing image.
								If thread.Rating > 0 Then
									trcRating.RenderControl(wr)
								End If
							End If
							RenderCellEnd(wr) ' </td>
						End If

						'CP - Add for thread status
						RenderCellBegin(wr, "", "", "5%", "right", "", "", "")	 ' <td>
						If (objConfig.EnableThreadStatus And thread.HostForum.EnableForumsThreadStatus) Or (thread.ThreadStatus = ThreadStatus.Poll And thread.HostForum.AllowPolls) Then
							RenderImage(wr, objConfig.GetThemeImageURL(thread.StatusImage), thread.StatusText, "") ' <img/>
						End If
						RenderCellEnd(wr) ' </td>

						RenderRowEnd(wr) ' </tr>
						RenderTableEnd(wr) ' </table>
						RenderCellEnd(wr) ' </td>

						' Replies
						If even Then
							RenderCellBegin(wr, "Forum_RowHighlight1", "", "11%", "center", "", "", "") ' <td>
						Else
							RenderCellBegin(wr, "Forum_RowHighlight1_Alt", "", "11%", "center", "", "", "")	' </td>
						End If

						RenderDivBegin(wr, "", "Forum_Posts") ' <div>
						wr.Write(thread.Replies)
						RenderDivEnd(wr) ' </div>
						RenderCellEnd(wr) ' </td>

						' Views
						If even Then
							RenderCellBegin(wr, "Forum_RowHighlight2", "", "11%", "center", "", "", "") '<td>
						Else
							RenderCellBegin(wr, "Forum_RowHighlight2_Alt", "", "11%", "center", "", "", "")	' </td>
						End If

						RenderDivBegin(wr, "", "Forum_Threads")	' <div>
						wr.Write(thread.Views)
						RenderDivEnd(wr) ' </div>
						RenderCellEnd(wr) ' </td>

						' Post date info & author
						If even Then
							RenderCellBegin(wr, "Forum_RowHighlight3", "", "26%", "right", "", "2", "") ' <td>
						Else
							RenderCellBegin(wr, "Forum_RowHighlight3_Alt", "", "26%", "right", "", "2", "")	' <td>
						End If

						' table holds last post data
						RenderTableBegin(wr, "", "", "", "100%", "0", "0", "right", "", "0") ' <table>
						RenderRowBegin(wr) ' <tr>

						RenderCellBegin(wr, "", "", "1px", "", "", "", "") ' <td>
						RenderImage(wr, objConfig.GetThemeImageURL("row_spacer.gif"), "", "")
						RenderCellEnd(wr) ' </td>

						RenderCellBegin(wr, "", "", "", "right", "", "", "") ' <td>

						' Skeel - This is for showing link to first unread post for logged in users. 
						If ForumControl.LoggedOnUser.UserID > 0 Then
							If HasNewPosts(ForumControl.LoggedOnUser.UserID, thread) Then
								Dim params As String()

								params = New String(2) {"forumid=" & ForumId, "postid=" & thread.LastApprovedPost.PostID, "scope=posts"}
								url = NavigateURL(TabID, "", params)

								If ForumControl.LoggedOnUser.DefaultPostsPerPage < thread.TotalPosts Then
									'Find the page on which the first unread post is located
									wr.AddAttribute(HtmlTextWriterAttribute.Href, FirstUnreadLink(thread))
								Else
									'Thread has only one page
									wr.AddAttribute(HtmlTextWriterAttribute.Href, url + "#unread")
								End If
								wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
								RenderImage(wr, objConfig.GetThemeImageURL("thread_newest.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgThreadNewest"), "")
								wr.RenderEndTag() ' </a>
							Else
								url = Utilities.Links.ContainerViewPostLink(TabID, ForumId, thread.LastApprovedPost.PostID)
							End If
						Else
							url = Utilities.Links.ContainerViewPostLink(TabID, ForumId, thread.LastApprovedPost.PostID)
						End If
						' End Skeel

						RenderTitleLinkButton(wr, url, Utilities.ForumUtils.GetCreatedDateInfo(thread.LastApprovedPost.CreatedDate, objConfig, ""), "Forum_LastPostText", thread.LastPostShortBody) ' <a/>

						RenderDivBegin(wr, "", "Forum_LastPostText")	' <div>
						wr.Write(ForumControl.LocalizedText("by") & " ")
						url = Utilities.Links.UserPublicProfileLink(TabID, ModuleID, thread.LastApprovedUser.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, ForumControl.LoggedOnUser.Username)
						RenderLinkButton(wr, url, thread.LastApprovedUser.SiteAlias, "Forum_LastPostText") ' <a/>
						RenderDivEnd(wr) ' </div>
						RenderCellEnd(wr) ' </td>

						RenderCellBegin(wr, "", "", "1px", "", "", "", "") ' <td>
						RenderImage(wr, objConfig.GetThemeImageURL("row_spacer.gif"), "", "")	' <img/>
						RenderCellEnd(wr) ' </td>

						RenderRowEnd(wr) ' </tr>
						RenderTableEnd(wr) ' </table>
						RenderCellEnd(wr) ' </td>
						RenderRowEnd(wr) ' </tr>

						Count = Count + 1
					End If
				Next
				RenderTableEnd(wr) ' </table>
				RenderCellEnd(wr) ' </td>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td><img/></td>
				RenderRowEnd(wr) ' </Tr>
			Catch ex As Exception
				LogException(ex)
			End Try
		End Sub

		''' <summary>
		''' Footer w/ paging 
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderFooterRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>

			'Middle Column
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") ' <td>

			RenderTableBegin(wr, "tblFooterContents", "", "", "100%", "0", "0", "", "", "0")	  ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapLeft", "") ' <td><img/></td>

			RenderCellBegin(wr, "Forum_Footer", "", "100%", "left", "", "", "") ' <td>

			RenderTableBegin(wr, 0, 0, "") ' <table>
			RenderRowBegin(wr) ' <tr>

			' xml link (for single forum syndication)
			If (ForumControl.objConfig.EnableRSS And _ParentForum.EnableRSS) AndAlso (_ParentForum.PublicView) Then
				RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>

				wr.AddAttribute(HtmlTextWriterAttribute.Href, objConfig.SourceDirectory & "/Forum_Rss.aspx?forumid=" & Me.ForumId.ToString & "&tabid=" & TabID & "&mid=" & ModuleID)
				wr.AddAttribute(HtmlTextWriterAttribute.Target, "_blank")
				wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>

				RenderImage(wr, objConfig.GetThemeImageURL("s_rss.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgRSS"), "")
				wr.RenderEndTag() ' </A>
				RenderCellEnd(wr) ' </td>
			End If

			RenderCellBegin(wr, "", "", "", "", "top", "", "") ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "")	' <img/>
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "", "", "100%", "", "middle", "", "") ' <td>
			RenderThreadsPaging(wr)
			RenderCellEnd(wr) ' </td>

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapRight", "")	   ' <td><img/></td>

			'End middle column
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' bottom Breadcrumb and ddl's, along w/ subscribe chkbx (last row)
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderBottomBreadCrumbRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "left", "", "", "") ' top

			' start middle column table
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "", "left", "", "", "") ' <td>
			'Remove LoggedOnUserID limitation if wishing to implement Anonymous Posting
			If (ForumControl.LoggedOnUser.UserID > 0) And (Not ForumId = -1) Then
				Dim objSecurity As New Forum.ModuleSecurity(ModuleID, TabID, ForumId, ForumControl.LoggedOnUser.UserID)

				If Not ParentForum.PublicPosting Then
					If objSecurity.IsAllowedToStartRestrictedThread Then
						RenderTableBegin(wr, "", "", "", "", "4", "0", "", "", "0")	'<Table>     
						RenderRowBegin(wr) '<tr>
						RenderCellBegin(wr, "", "", "", "", "middle", "", "")	'<td>   
						RenderImage(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
						RenderCellEnd(wr) ' </td>
						RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td> 
						RenderCellEnd(wr) ' </Td>
						RenderRowEnd(wr) ' </tr>

						RenderRowBegin(wr) '<tr>
						url = Utilities.Links.NewThreadLink(TabID, ForumId, ModuleID)
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

						If ForumControl.LoggedOnUser.IsBanned Then
							RenderLinkButton(wr, url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
						Else
							RenderLinkButton(wr, url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
						End If

						RenderCellEnd(wr) ' </Td>
						RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td> 
						RenderCellEnd(wr) ' </Td>
						RenderRowEnd(wr) ' </tr>
						RenderTableEnd(wr) ' </table>
					Else
						wr.Write("&nbsp;")
					End If
				Else
					RenderTableBegin(wr, "", "", "", "", "4", "0", "", "", "0")	'<Table>   
					RenderRowBegin(wr) '<tr>
					RenderCellBegin(wr, "", "", "", "", "middle", "", "")	'<td>   
					RenderImage(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "")
					RenderCellEnd(wr) ' </td>
					RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td> 
					RenderCellEnd(wr) ' </Td>
					RenderRowEnd(wr) ' </tr>

					RenderRowBegin(wr) '<tr>
					url = Utilities.Links.NewThreadLink(TabID, ForumId, ModuleID)
					RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

					If ForumControl.LoggedOnUser.IsBanned Then
						RenderLinkButton(wr, url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
					Else
						RenderLinkButton(wr, url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
					End If

					RenderCellEnd(wr) ' </Td>
					RenderCellBegin(wr, "", "", "", "", "middle", "", "") ' <td> 
					RenderCellEnd(wr) ' </Td>
					RenderRowEnd(wr) ' </tr>
					RenderTableEnd(wr) ' </table>
				End If
			End If
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>

			'breadcrumb row
			RenderRowBegin(wr) '<Tr>
			RenderCellBegin(wr, "", "", "100%", "left", "top", "", "")
			Dim ChildGroupView As Boolean = False
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				ChildGroupView = True
			End If
			wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Threads, ParentForum, objConfig, ChildGroupView))
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>

			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "100%", "right", "top", "", "")	' <td>
			RenderDivBegin(wr, "", "Forum_NormalTextBox") ' <span>
			wr.Write(ForumControl.LocalizedText("LatestThreads"))
			RenderDivEnd(wr) ' </span>

			RenderImage(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")

			ddlDateFilter.CssClass = "Forum_NormalTextBox"
			ddlDateFilter.RenderControl(wr)
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>

			' Display tracking option if user is authenticated and thread count > 0
			' CP - Removed (Total Records > 0) as why shouldn't a user be able to subscribe to a new forum
			If (ForumControl.LoggedOnUser.UserID > 0) And (objConfig.MailNotification) And (ForumId <> -1) Then
				' check email
				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, "", "", "", "right", "", "", "")
				chkEmail.RenderControl(wr)

				'' We check if user is subscribed in this forum
				'Dim blnTrackedForum As Boolean = False

				'For Each objTrackedForum As TrackingInfo In ForumControl.LoggedOnUser.TrackedForums
				'	If objTrackedForum.ForumID = ForumId Then
				'		blnTrackedForum = True
				'		Exit For
				'	End If
				'Next

				'chkEmail.Visible = True
				'chkEmail.Checked = blnTrackedForum

				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>

				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, "", "", "", "right", "", "", "")
				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>

				' Mark As Read linkbutton
				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, "", "", "", "", "", "", "")	' <td>

				' need a new table
				RenderTableBegin(wr, 0, 0, "tblReadButton") ' <table>
				RenderRowBegin(wr) '<tr>

				RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
				wr.Write("&nbsp;")
				RenderCellEnd(wr) ' </td>

				RenderCellBegin(wr, "Forum_ReplyCell", "", "85px", "right", "", "", "") ' <td>
				cmdRead.RenderControl(wr)
				RenderCellEnd(wr) ' </td>

				RenderRowEnd(wr) ' </tr>
				RenderTableEnd(wr) ' </table>

				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>
			Else
				' user is not logged on (or notification is not enabled)
			End If

			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			' right cap
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")

			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' Renders the paging shown in the footer (based on threads/page)
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderThreadsPaging(ByVal wr As HtmlTextWriter)
			' First, previous, next, last thread hyperlinks
			Dim ctlPagingControl As New DotNetNuke.Modules.Forum.WebControls.PagingControl
			ctlPagingControl.CssClass = "Forum_FooterText"
			ctlPagingControl.TotalRecords = TotalRecords
			ctlPagingControl.PageSize = ForumControl.ThreadsPerPage
			ctlPagingControl.CurrentPage = ThreadPage + 1

			Dim Params As String = "forumid=" & ForumId.ToString & "&scope=threads"
			If NoReply Then
				Params = Params & "&noreply=1"
			End If

			ctlPagingControl.QuerystringParams = Params
			ctlPagingControl.TabID = TabID
			ctlPagingControl.RenderControl(wr)
		End Sub

		''' <summary>
		''' Determins if thread is even or odd numbered row
		''' </summary>
		''' <param name="Count"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Private Function ThreadIsEven(ByVal Count As Integer) As Boolean
			If Count Mod 2 = 0 Then
				'even
				Return True
			Else
				'odd
				Return False
			End If
		End Function

		''' <summary>
		''' Determines thread icon for each thread based on its status 
		''' (Non Pinned)
		''' </summary>
		''' <param name="Thread"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Private Function GetMediaURL(ByVal Thread As ThreadInfo) As String
			If Thread.IsClosed Then
				' thread IS locked
				If (Thread.IsPopular) Then
					' thread IS locked, popular
					If HasNewPosts(ForumControl.LoggedOnUser.UserID, Thread) Then
						'new
						Return objConfig.GetThemeImageURL("s_postlockedunreadplus.") & objConfig.ImageExtension
					Else
						'read
						Return objConfig.GetThemeImageURL("s_postlockedreadplus.") & objConfig.ImageExtension
					End If
				Else
					' thread IS locked NOT popular
					If HasNewPosts(ForumControl.LoggedOnUser.UserID, Thread) Then
						'new
						Return objConfig.GetThemeImageURL("s_postlockedunread.") & objConfig.ImageExtension
					Else
						'read
						Return objConfig.GetThemeImageURL("s_postlockedread.") & objConfig.ImageExtension
					End If
				End If

			Else
				' thread NOT locked
				If (Thread.IsPopular) Then
					If HasNewPosts(ForumControl.LoggedOnUser.UserID, Thread) Then
						Return objConfig.GetThemeImageURL("s_postunreadplus.") & objConfig.ImageExtension
					Else
						Return objConfig.GetThemeImageURL("s_postreadplus.") & objConfig.ImageExtension
					End If
				Else
					If HasNewPosts(ForumControl.LoggedOnUser.UserID, Thread) Then
						Return objConfig.GetThemeImageURL("s_postunread.") & objConfig.ImageExtension
					Else
						Return objConfig.GetThemeImageURL("s_postread.") & objConfig.ImageExtension
					End If
				End If
			End If

		End Function

		''' <summary>
		''' Determines the tooltip for each thread icon based on its status for
		''' the current users (Non Pinned)
		''' </summary>
		''' <param name="Thread"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Private Function GetMediaText(ByVal Thread As ThreadInfo) As String
			If Thread.IsClosed Then
				' thread IS locked
				If (Thread.IsPopular) Then
					' thread IS locked, popular
					If HasNewPosts(ForumControl.LoggedOnUser.UserID, Thread) Then
						' new
						Return ForumControl.LocalizedText("imgNewPopLockedThread")
					Else
						' read
						Return ForumControl.LocalizedText("imgPopLockedThread")
					End If
				Else
					' thread IS locked NOT popular
					If HasNewPosts(ForumControl.LoggedOnUser.UserID, Thread) Then
						' new
						Return ForumControl.LocalizedText("imgNewLockedThread")
					Else
						' read
						Return ForumControl.LocalizedText("imgLockedThread")
					End If
				End If
			Else
				' thread NOT locked
				If (Thread.IsPopular) Then
					' thread NOT locked IS popular
					If HasNewPosts(ForumControl.LoggedOnUser.UserID, Thread) Then
						' new
						Return ForumControl.LocalizedText("imgNewPopThread")
					Else
						' read
						Return ForumControl.LocalizedText("imgPopThread")
					End If
				Else
					' thread NOT locked, popular
					If HasNewPosts(ForumControl.LoggedOnUser.UserID, Thread) Then
						' new
						Return ForumControl.LocalizedText("imgNewThread")
					Else
						' read
						Return ForumControl.LocalizedText("imgThread")
					End If
				End If
			End If

		End Function

		''' <summary>
		''' Returns a URL to the first unread post in the specific Thread
		''' </summary>
		''' <param name="Thread"></param>
		''' <returns>URL</returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	11/28/2008	Created
		''' </history>
		Private Function FirstUnreadLink(ByVal Thread As ThreadInfo) As String
			Dim cltUserThread As New UserThreadsController
			Dim usrThread As New UserThreadsInfo
			Dim ReadLink As String

			ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, ForumId, Thread.ThreadID) & "#unread"
			usrThread = cltUserThread.GetCachedUserThreadRead(ForumControl.LoggedOnUser.UserID, Thread.ThreadID)

			If usrThread Is Nothing Then
				'All new
				If ForumControl.LoggedOnUser.ViewDescending = True Then
					Dim PageCount As Decimal = CDec(Thread.TotalPosts / ForumControl.LoggedOnUser.DefaultPostsPerPage)
					PageCount = Math.Ceiling(PageCount)
					ReadLink = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumId, Thread.ThreadID, CInt(PageCount)) & "#unread"
				Else
					ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, ForumId, Thread.ThreadID) & "#unread"
				End If
			Else
				'Get the Index
				Dim PostIndex As Integer = cltUserThread.GetPostIndexFirstUnread(Thread.ThreadID, usrThread.LastVisitDate, ForumControl.LoggedOnUser.ViewDescending)
				Dim PageCount As Integer = CInt(Math.Ceiling(CDec(Thread.TotalPosts / ForumControl.LoggedOnUser.DefaultPostsPerPage)))
				Dim PageNumber As Integer = 1

				Do While PageNumber <= PageCount
					If (ForumControl.LoggedOnUser.DefaultPostsPerPage * PageNumber) >= PostIndex Then
						If PageNumber = 1 Then
							ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, ForumId, Thread.ThreadID) & "#unread"
						Else
							ReadLink = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumId, Thread.ThreadID, PageNumber) & "#unread"
						End If
						Exit Do
					End If
					PageNumber += 1
				Loop
			End If

			Return ReadLink
		End Function

#End Region

	End Class

End Namespace