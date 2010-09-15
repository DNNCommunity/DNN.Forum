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
	''' This renders the threads view (second view in hierarchy of forum)
	''' </summary>
	''' <remarks>All UI is done in code, no corresponding ascx
	''' </remarks>
	Public Class Threads
		Inherits ForumObject

#Region "Private Declarations"

		Private _Filter As String = String.Empty
		Private _TotalRecords As Integer
		Private _CurrentPage As Integer = 0
		Private _ThreadCollection As New List(Of ThreadInfo)
		Private _ThreadRatings As New Hashtable

#Region "Controls"

		Private ddlDateFilter As DotNetNuke.Web.UI.WebControls.DnnComboBox
		Private cmdRead As LinkButton
		Private chkEmail As CheckBox
		Private txtForumSearch As TextBox
		Private cmdForumSearch As ImageButton
		Private trcRating As Telerik.Web.UI.RadRating
		Private cmdForumSubscribers As LinkButton

#End Region

#End Region

#Region "Private Properties"

		''' <summary>
		''' This is used to determine the permissions for the current user/forum combination. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property objSecurity() As ModuleSecurity
			Get
				Return New ModuleSecurity(ModuleID, TabID, ForumID, CurrentForumUser.UserID)
			End Get
		End Property

		''' <summary>
		'''  The forum we are viewing the threads for.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property objForum() As ForumInfo
			Get
				Dim cntForum As New ForumController
				Return cntForum.GetForumItemCache(ForumID)
			End Get
		End Property

		''' <summary>
		''' Used to retrieve only posts with no replies if true.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property NoReply() As Boolean
			Get
				If HttpContext.Current.Request.QueryString("noreply") IsNot Nothing Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property

		''' <summary>
		''' If a thread has new posts or not using the UserReads methods. 
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="objThread"></param>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property HasNewPosts(ByVal UserID As Integer, ByVal objThread As ThreadInfo) As Boolean
			Get
				Dim userthreadController As New UserThreadsController
				Dim userthread As New UserThreadsInfo

				If UserID > 0 Then
					If objThread Is Nothing Then
						Return True
					Else
						userthread = userthreadController.GetThreadReadsByUser(UserID, objThread.ThreadID)
						If userthread Is Nothing Then
							Return True
						Else
							If userthread.LastVisitDate < objThread.LastApprovedPost.CreatedDate Then
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
		''' The item used to filter the returned threads. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This can be things such as date</remarks>
		Private Property Filter() As String
			Get
				Return _Filter
			End Get
			Set(ByVal Value As String)
				_Filter = Value
			End Set
		End Property

		''' <summary>
		''' The total number of threads available for viewing.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal Value As Integer)
				_TotalRecords = Value
			End Set
		End Property

		''' <summary>
		''' The current page the user is viewing.  
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property CurrentPage() As Integer
			Get
				Return _CurrentPage
			End Get
			Set(ByVal Value As Integer)
				_CurrentPage = Value
			End Set
		End Property

		''' <summary>
		''' The collection of threads returned. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property ThreadCollection() As List(Of ThreadInfo)
			Get
				Return _ThreadCollection
			End Get
			Set(ByVal Value As List(Of ThreadInfo))
				_ThreadCollection = Value
			End Set
		End Property

		''' <summary>
		''' A collection of ratings controls. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property ThreadRatings() As Hashtable
			Get
				Return _ThreadRatings
			End Get
			Set(ByVal Value As Hashtable)
				_ThreadRatings = Value
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
		Protected Sub ddlDateFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim dFilter As String = ddlDateFilter.SelectedItem.Value
			Dim url As String
			url = Utilities.Links.ContainerThreadDateFilterLink(TabID, dFilter, ForumId, NoReply)

			If CurrentForumUser.UserID > 0 Then
				'update the user profile
				Dim fUserCnt As New ForumUserController
				fUserCnt.UserUpdateTrackingDuration(CType(dFilter, Integer), CurrentForumUser.UserID, ForumControl.PortalID)
				DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(CurrentForumUser.UserID, PortalID)
			End If

			MyBase.BasePage.Response.Redirect(url, False)
		End Sub

		''' <summary>
		''' Marks all threads in the forum as unread 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdRead_Clicked(ByVal sender As Object, ByVal e As EventArgs)
			If objConfig.EnableUserReadManagement Then
				Dim userThreadController As New UserThreadsController
				userThreadController.MarkAll(CurrentForumUser.UserID, ForumID, True)
			End If
		End Sub

		''' <summary>
		''' Toggles user notification for thread on/off on postback
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub chkEmail_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
			Dim ctlTracking As New TrackingController
			ctlTracking.TrackingForumCreateDelete(ForumId, CurrentForumUser.UserID, CType(sender, CheckBox).Checked, ModuleID)
		End Sub

		''' <summary>
		''' This directs the user to the search results of this particular forum. It searches this forum and the subject, body of the post. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdForumSearch_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
			Dim url As String
			Url = Utilities.Links.ContainerSingleForumSearchLink(TabID, ForumID, txtForumSearch.Text)
			MyBase.BasePage.Response.Redirect(Url, False)
		End Sub

		''' <summary>
		''' Visible only to module admin, this allows them to view a list of subscribers to the current thread (via Admin Control Panel).
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdForumSubscribers_Click(ByVal sender As Object, ByVal e As EventArgs)
			Dim url As String
			Url = Utilities.Links.ForumEmailSubscribers(TabID, ModuleID, ForumID)
			MyBase.BasePage.Response.Redirect(Url, False)
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

			If ForumID = -1 Then
				' Redirect the user to the aggregated view (Prior to 4.4.4, aggregated used this view so we have to handle legacy links)
				HttpContext.Current.Response.Redirect(Utilities.Links.ContainerAggregatedLink(TabID, False), True)
			Else
				If Not objForum.IsActive Then
					' we should consider setting type of redirect here?

					MyBase.BasePage.Response.Redirect(Utilities.Links.NoContentLink(TabID, ModuleID), True)
				End If

				If Not (objForum.PublicView) Then
					' The forum is private, see if we have proper view perms here

					If Not objSecurity.IsAllowedToViewPrivateForum Then
						' we should consider setting type of redirect here?

						MyBase.BasePage.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
					End If
				End If
			End If

			'We are past knowing the user should be here, let's handle SEO oriented things
			If objConfig.OverrideTitle Then
				Me.BaseControl.BasePage.Title = objForum.Name & " - " & Me.BaseControl.PortalName
			End If

			If objConfig.OverrideDescription Then

				MyBase.BasePage.Description = "," + objForum.Name + "," + Me.BaseControl.PortalName
			End If
			' Consider add metakeywords via applied tags, when taxonomy is integrated

			' We need to make sure the user's thread pagesize can handle this 
			'(problem is, a link can be posted by one user w/ page size of 5 pointing to page 2, if logged in user has pagesize set to 15, there is no page 2)
			If Not HttpContext.Current.Request.QueryString("currentpage") Is Nothing Then
				Dim urlThreadPage As Integer = Int32.Parse(HttpContext.Current.Request.QueryString("currentpage"))
				Dim TotalThreads As Integer = objForum.TotalThreads
				Dim userThreadsPerPage As Integer

				If CurrentForumUser.UserID > 0 Then
					userThreadsPerPage = CurrentForumUser.ThreadsPerPage
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

				CurrentPage = ThreadPageToShow
			End If

			Dim Term As New SearchTerms

			If CurrentForumUser.UserID > -1 Then
				Dim dateFilter As Integer = CurrentForumUser.TrackingDuration

				If dateFilter > 999 Then
					' we are not going to add a searchTerm, this means get all threads from all days
				ElseIf dateFilter = -1 Then ' Last Activity
					Dim DateDiff As TimeSpan
					Dim DateString As String
					DateDiff = Date.Now.Subtract(CurrentForumUser.LastActivity)
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

			If NoReply Then
				Term.AddSearchTerm("T.Replies", CompareOperator.EqualString, "0")
			End If

			Filter = Term.WhereClause

			If CurrentPage > 0 Then
				CurrentPage = CurrentPage - 1
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
			If CurrentForumUser.UserID > 0 Then
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

			ddlDateFilter = New DotNetNuke.Web.UI.WebControls.DnnComboBox
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

			Me.cmdForumSubscribers = New LinkButton
			With cmdForumSubscribers
				.CssClass = "Forum_Profile"
				.ID = "cmdForumSubscribers"
				.Text = ForumControl.LocalizedText("cmdForumSubscribers")
			End With

			BindControls()
			AddControlHandlers()
			AddControlsToTree()

			For Each objThread As ThreadInfo In ThreadCollection
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

					.ID = "trcRating" + objThread.ThreadID.ToString()
					.Value = CDec(objThread.Rating)
					'AddHandler trcRating.Command, AddressOf trcRating_Rate
				End With
				ThreadRatings.Add(objThread.ThreadID, trcRating)
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
			'update the UserForum record (leave even if permitting anonymous posting)
			If HttpContext.Current.Request.IsAuthenticated And ForumID <> -1 Then
				HandleReads()
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
		''' Determines if the user reads need to be updated, if so it handles that.
		''' </summary>
		''' <remarks></remarks>
		Private Sub HandleReads()
			Dim userForumController As New UserForumsController
			Dim userForum As New UserForumsInfo

			userForum = userForumController.GetUsersForumReads(CurrentForumUser.UserID, ForumID)

			If Not userForum Is Nothing Then
				userForum.LastVisitDate = Now
				userForumController.Update(userForum)
				UserForumsController.ResetUsersForumReads(CurrentForumUser.UserID, ForumID)
			Else
				userForum = New UserForumsInfo
				With userForum
					.UserID = CurrentForumUser.UserID
					.ForumID = ForumID
					.LastVisitDate = Now
				End With
				userForumController.Add(userForum)
				UserForumsController.ResetUsersForumReads(CurrentForumUser.UserID, ForumID)
			End If
			'ForumController.ResetForumInfoCache(ForumID)
		End Sub

		''' <summary>
		''' Loads all the handlers for the controls used in this view
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub AddControlHandlers()
			Try
				If CurrentForumUser.UserID > 0 Then
					AddHandler cmdRead.Click, AddressOf cmdRead_Clicked
					AddHandler chkEmail.CheckedChanged, AddressOf chkEmail_CheckedChanged
				End If

				AddHandler ddlDateFilter.SelectedIndexChanged, AddressOf ddlDateFilter_SelectedIndexChanged
				AddHandler cmdForumSearch.Click, AddressOf cmdForumSearch_Click
				AddHandler cmdForumSubscribers.Click, AddressOf cmdForumSubscribers_Click
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
				If CurrentForumUser.UserID > 0 Then
					Controls.Add(cmdRead)
					Controls.Add(chkEmail)
					Controls.Add(cmdForumSubscribers)
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

				ddlDateFilter.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Today", objConfig.SharedResourceFile), "0"))
				ddlDateFilter.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("PastThreeDays", objConfig.SharedResourceFile), "3"))
				ddlDateFilter.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("PastWeek", objConfig.SharedResourceFile), "7"))
				ddlDateFilter.Items.Insert(3, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("PastTwoWeek", objConfig.SharedResourceFile), "14"))
				ddlDateFilter.Items.Insert(4, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("PastMonth", objConfig.SharedResourceFile), "30"))
				ddlDateFilter.Items.Insert(5, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("PastThreeMonth", objConfig.SharedResourceFile), "92"))
				ddlDateFilter.Items.Insert(6, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("PastYear", objConfig.SharedResourceFile), "365"))
				ddlDateFilter.Items.Insert(7, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("AllDays", objConfig.SharedResourceFile), "3650"))

				If CurrentForumUser.UserID > 0 Then
					ddlDateFilter.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("LastVisit", objConfig.SharedResourceFile), "-1"))
				End If

				Dim dateFilter As Integer = 1000

				' create child control date filter dropdownlist
				If Not HttpContext.Current.Request.QueryString("datefilter") Is Nothing Then
					dateFilter = Int16.Parse(HttpContext.Current.Request.QueryString("datefilter"))
				Else
					' Tracking Duration
					If CurrentForumUser.UserID > 0 Then
						dateFilter = CurrentForumUser.TrackingDuration
					End If
				End If

				ddlDateFilter.SelectedValue = dateFilter.ToString()

				If CurrentForumUser.UserID > 0 And objConfig.MailNotification Then
					' We check if user is subscribed in this forum
					Dim blnTrackedForum As Boolean = False

					For Each objTrackedForum As TrackingInfo In CurrentForumUser.TrackedForums
						If objTrackedForum.ForumID = ForumID Then
							blnTrackedForum = True
							Exit For
						End If
					Next

					chkEmail.Visible = True
					chkEmail.Checked = blnTrackedForum
				End If

				' Now we get threads to display for this user
				Dim ctlThread As New ThreadController
				ThreadCollection = ctlThread.GetForumThreads(ModuleID, ForumID, CurrentForumUser.ThreadsPerPage, CurrentPage, Filter, PortalID)

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
			wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Threads, objForum, objConfig, ChildGroupView))
			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "20%", "right", "", "", "") ' <td>

			If NoReply Then
				' show link to show all threads
				RenderLinkButton(wr, Utilities.Links.ContainerViewForumLink(TabID, ForumID, False), Localization.GetString("ShowAll", objConfig.SharedResourceFile), "Forum_BreadCrumb")
			Else
				' show link to show no reply threads
				RenderLinkButton(wr, Utilities.Links.ContainerViewForumLink(TabID, ForumID, True), Localization.GetString("ShowNoReplies", objConfig.SharedResourceFile), "Forum_BreadCrumb")
			End If

			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>

			Dim url As String

			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "100%", "left", "", "2", "") ' <td>
			'Remove LoggedOnUserID limitation if wishing to implement Anonymous Posting
			If (CurrentForumUser.UserID > 0) And (Not ForumID = -1) Then
				If Not objForum.PublicPosting Then
					If objSecurity.IsAllowedToStartRestrictedThread Then
						RenderTableBegin(wr, "", "", "", "", "4", "0", "", "", "0")	'<Table>            
						RenderRowBegin(wr) '<tr>
						Url = Utilities.Links.NewThreadLink(TabID, ForumID, ModuleID)
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

						If CurrentForumUser.IsBanned Then
							RenderLinkButton(wr, Url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
						Else
							RenderLinkButton(wr, Url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
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
					Url = Utilities.Links.NewThreadLink(TabID, ForumID, ModuleID)
					RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

					If CurrentForumUser.IsBanned Then
						RenderLinkButton(wr, Url, ForumControl.LocalizedText("NewThread"), "Forum_Link", False)
					Else
						RenderLinkButton(wr, Url, ForumControl.LocalizedText("NewThread"), "Forum_Link")
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
				Dim url As String
				For Each objThread As ThreadInfo In ThreadCollection
					If Not objThread Is Nothing Then
						Dim even As Boolean = ThreadIsEven(Count)
						TotalRecords = objThread.TotalRecords

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
						url = Utilities.Links.ContainerViewThreadLink(TabID, ForumID, objThread.ThreadID)

						'link so icon is clickable (also used below for subject)
						wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
						wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>

						' see if post is pinned, priority over other icons
						If objThread.IsPinned Then
							' First see if the thread is popular
							If (objThread.IsPopular) Then
								' thread IS popular and pinned
								' see if thread is locked
								If (objThread.IsClosed) Then
									' thread IS popular, pinned, locked
									' See if this is an unread post
									If (HasNewPosts(CurrentForumUser.UserID, objThread)) Then
										' IS read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postlockedpinnedunreadplu.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotNewLockedPinnedThread"), "")
									Else
										' not read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postlockedpinnedreadplus.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotLockedPinnedThread"), "")
									End If
								Else
									' thread IS popular, Pinned but NOT locked
									' See if this is an unread post
									If (HasNewPosts(CurrentForumUser.UserID, objThread)) Then
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
								If (objThread.IsClosed) Then
									' thread IS pinned, Locked but NOT popular
									If (HasNewPosts(CurrentForumUser.UserID, objThread)) Then
										' IS read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedlockedunread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewPinnedLockedThread"), "")
									Else
										' not read
										RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedlockedread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgPinnedLockedThread"), "")
									End If
								Else
									'thread IS pinned but NOT popular, Locked
									If (HasNewPosts(CurrentForumUser.UserID, objThread)) Then
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
							RenderImage(wr, GetMediaURL(objThread), GetMediaText(objThread), "") ' <img/>
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
						If (HasNewPosts(CurrentForumUser.UserID, objThread)) Then
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
							wr.Write(Utilities.ForumUtils.FormatProhibitedWord(objThread.Subject, objThread.CreatedDate, PortalID))
						Else
							wr.Write(objThread.Subject)
						End If

						RenderDivEnd(wr) ' </div> - CP - I am not sure why this has to be here

						RenderDivBegin(wr, "", "Forum_NormalSmall") ' <div>
						wr.Write(String.Format("{0}&nbsp;", ForumControl.LocalizedText("by")))

						If Not objConfig.EnableExternalProfile Then
							url = objThread.StartedByUser.UserCoreProfileLink
						Else
							url = Utilities.Links.UserExternalProfileLink(objThread.StartedByUserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objThread.StartedByUser.Username)
						End If

						RenderLinkButton(wr, url, objThread.StartedByUser.SiteAlias, "Forum_NormalSmall") ' <a/>

						' correct logic to handle posts per page per user
						Dim userPostsPerPage As Integer
						' CapPageCount is number of pages to show as option for user in threads view.
						Dim CapPageCount As Integer = objConfig.PostPagesCount

						If CurrentForumUser.UserID > 0 Then
							userPostsPerPage = CurrentForumUser.PostsPerPage
						Else
							userPostsPerPage = objConfig.PostsPerPage
						End If

						Dim UserPagesCount As Integer = CInt(Math.Ceiling(objThread.TotalPosts / userPostsPerPage))
						Dim ShowFinalPage As Boolean = (UserPagesCount >= CapPageCount)

						' Only show Pager if there is more than 1 page for the user
						If UserPagesCount > 1 Then
							' If thread spans several pages, then display text like (Page 1, 2, 3, ..., 5)

							wr.Write(" (" & ForumControl.LocalizedText("Page") & ": ")

							If UserPagesCount >= CapPageCount Then
								For ThreadPage As Integer = 1 To CapPageCount - 1
									url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumID, objThread.ThreadID, ThreadPage)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
									wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalSmall")
									wr.AddAttribute(HtmlTextWriterAttribute.Rel, "nofollow")
									wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
									wr.Write(ThreadPage.ToString())
									wr.RenderEndTag() ' </a>
									If (ThreadPage < CapPageCount - 1) Or (ShowFinalPage And ThreadPage = CapPageCount) Then
										wr.Write(", ")
									End If
								Next

								If ShowFinalPage Then
									If UserPagesCount > CapPageCount Then
										wr.Write("..., ")
									Else
										wr.Write(", ")
									End If
									url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumID, objThread.ThreadID, UserPagesCount)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
									wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalSmall")
									wr.AddAttribute(HtmlTextWriterAttribute.Rel, "nofollow")
									wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
									wr.Write(UserPagesCount.ToString())
									wr.RenderEndTag() ' </A>
								End If
							Else
								For ThreadPage As Integer = 1 To UserPagesCount
									url = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumID, objThread.ThreadID, ThreadPage)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
									wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalSmall")
									wr.AddAttribute(HtmlTextWriterAttribute.Rel, "nofollow")
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

						If objConfig.EnableRatings And objThread.ContainingForum.EnableForumsRating Then
							RenderCellBegin(wr, "", "", "30%", "right", "", "", "") ' <td>

							If ThreadRatings.ContainsKey(objThread.ThreadID) Then
								trcRating = CType(ThreadRatings(objThread.ThreadID), Telerik.Web.UI.RadRating)
								' CP - we alter statement below if we want to enable 0 rating still showing image.
								If objThread.Rating > 0 Then
									trcRating.RenderControl(wr)
								End If
							End If
							RenderCellEnd(wr) ' </td>
						End If

						'CP - Add for thread status
						RenderCellBegin(wr, "", "", "5%", "right", "", "", "")	 ' <td>
						If (objConfig.EnableThreadStatus And objThread.ContainingForum.EnableForumsThreadStatus) Or (objThread.ThreadStatus = ThreadStatus.Poll And objThread.ContainingForum.AllowPolls) Then
							RenderImage(wr, objConfig.GetThemeImageURL(objThread.StatusImage), objThread.StatusText, "") ' <img/>
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
						wr.Write(objThread.Replies)
						RenderDivEnd(wr) ' </div>
						RenderCellEnd(wr) ' </td>

						' Views
						If even Then
							RenderCellBegin(wr, "Forum_RowHighlight2", "", "11%", "center", "", "", "") '<td>
						Else
							RenderCellBegin(wr, "Forum_RowHighlight2_Alt", "", "11%", "center", "", "", "")	' </td>
						End If

						RenderDivBegin(wr, "", "Forum_Threads")	' <div>
						wr.Write(objThread.Views)
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
						If CurrentForumUser.UserID > 0 Then
							If HasNewPosts(CurrentForumUser.UserID, objThread) Then
								Dim params As String()

								params = New String(2) {"forumid=" & ForumID, "postid=" & objThread.LastApprovedPost.PostID, "scope=posts"}
								url = NavigateURL(TabID, "", params)

								If CurrentForumUser.PostsPerPage < objThread.TotalPosts Then
									'Find the page on which the first unread post is located
									wr.AddAttribute(HtmlTextWriterAttribute.Href, FirstUnreadLink(objThread))
								Else
									'Thread has only one page
									wr.AddAttribute(HtmlTextWriterAttribute.Href, url + "#unread")
								End If
								wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
								RenderImage(wr, objConfig.GetThemeImageURL("thread_newest.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgThreadNewest"), "")
								wr.RenderEndTag() ' </a>
							Else
								url = Utilities.Links.ContainerViewPostLink(TabID, ForumID, objThread.LastApprovedPost.PostID)
							End If
						Else
							url = Utilities.Links.ContainerViewPostLink(TabID, ForumID, objThread.LastApprovedPost.PostID)
						End If
						' End Skeel

						RenderTitleLinkButton(wr, url, Utilities.ForumUtils.GetCreatedDateInfo(objThread.LastApprovedPost.CreatedDate, objConfig, ""), "Forum_LastPostText", objThread.LastPostShortBody) ' <a/>

						RenderDivBegin(wr, "", "Forum_LastPostText")	' <div>
						wr.Write(ForumControl.LocalizedText("by") & " ")

						If Not objConfig.EnableExternalProfile Then
							url = objThread.LastApprovedUser.UserCoreProfileLink
						Else
							url = Utilities.Links.UserExternalProfileLink(objThread.LastApprovedUser.UserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, CurrentForumUser.Username)
						End If

						RenderLinkButton(wr, url, objThread.LastApprovedUser.SiteAlias, "Forum_LastPostText") ' <a/>
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
			If (ForumControl.objConfig.EnableRSS And objForum.EnableRSS) AndAlso (objForum.PublicView) Then
				RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>

				wr.AddAttribute(HtmlTextWriterAttribute.Href, objConfig.SourceDirectory & "/Forum_Rss.aspx?forumid=" & Me.ForumID.ToString & "&tabid=" & TabID & "&mid=" & ModuleID)
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

			Dim url As String

			' start middle column table
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "", "left", "", "", "") ' <td>
			'Remove LoggedOnUserID limitation if wishing to implement Anonymous Posting
			If (CurrentForumUser.UserID > 0) And (Not ForumID = -1) Then
				If Not objForum.PublicPosting Then
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
						url = Utilities.Links.NewThreadLink(TabID, ForumID, ModuleID)
						RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

						If CurrentForumUser.IsBanned Then
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
					url = Utilities.Links.NewThreadLink(TabID, ForumID, ModuleID)
					RenderCellBegin(wr, "Forum_NavBarButton", "", "", "", "middle", "", "") ' <td> 

					If CurrentForumUser.IsBanned Then
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
			wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Threads, objForum, objConfig, ChildGroupView))
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

			' Display tracking option if user is authenticated 
			If (CurrentForumUser.UserID > 0) And (objConfig.MailNotification) And (ForumID <> -1) Then
				' check email
				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, "", "", "", "right", "", "", "")

				If objSecurity.IsForumAdmin Then
					cmdForumSubscribers.RenderControl(wr)
					wr.Write("<br />")
				End If

				chkEmail.RenderControl(wr)

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
				If objConfig.EnableUserReadManagement AndAlso CurrentForumUser.UserID > 0 Then
					cmdRead.RenderControl(wr)
				End If
				RenderCellEnd(wr) ' </td>

				RenderRowEnd(wr) ' </tr>
				RenderTableEnd(wr) ' </table>

				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>
			Else
				' user is not logged on (or notification is not enabled)
			End If

			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "", "", "", "right", "", "", "")
			wr.Write("<br />")
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>


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
			ctlPagingControl.PageSize = CurrentForumUser.ThreadsPerPage
			ctlPagingControl.CurrentPage = CurrentPage + 1

			Dim Params As String = "forumid=" & ForumID.ToString & "&scope=threads"
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
					If HasNewPosts(CurrentForumUser.UserID, Thread) Then
						'new
						Return objConfig.GetThemeImageURL("s_postlockedunreadplus.") & objConfig.ImageExtension
					Else
						'read
						Return objConfig.GetThemeImageURL("s_postlockedreadplus.") & objConfig.ImageExtension
					End If
				Else
					' thread IS locked NOT popular
					If HasNewPosts(CurrentForumUser.UserID, Thread) Then
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
					If HasNewPosts(CurrentForumUser.UserID, Thread) Then
						Return objConfig.GetThemeImageURL("s_postunreadplus.") & objConfig.ImageExtension
					Else
						Return objConfig.GetThemeImageURL("s_postreadplus.") & objConfig.ImageExtension
					End If
				Else
					If HasNewPosts(CurrentForumUser.UserID, Thread) Then
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
					If HasNewPosts(CurrentForumUser.UserID, Thread) Then
						' new
						Return ForumControl.LocalizedText("imgNewPopLockedThread")
					Else
						' read
						Return ForumControl.LocalizedText("imgPopLockedThread")
					End If
				Else
					' thread IS locked NOT popular
					If HasNewPosts(CurrentForumUser.UserID, Thread) Then
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
					If HasNewPosts(CurrentForumUser.UserID, Thread) Then
						' new
						Return ForumControl.LocalizedText("imgNewPopThread")
					Else
						' read
						Return ForumControl.LocalizedText("imgPopThread")
					End If
				Else
					' thread NOT locked, popular
					If HasNewPosts(CurrentForumUser.UserID, Thread) Then
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
		Private Function FirstUnreadLink(ByVal Thread As ThreadInfo) As String
			Dim cltUserThread As New UserThreadsController
			Dim usrThread As New UserThreadsInfo
			Dim ReadLink As String

			ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, ForumID, Thread.ThreadID) & "#unread"
			usrThread = cltUserThread.GetThreadReadsByUser(CurrentForumUser.UserID, Thread.ThreadID)

			If usrThread Is Nothing Then
				'All new
				If CurrentForumUser.ViewDescending = True Then
					Dim PageCount As Decimal = CDec(Thread.TotalPosts / CurrentForumUser.PostsPerPage)
					PageCount = Math.Ceiling(PageCount)
					ReadLink = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumID, Thread.ThreadID, CInt(PageCount)) & "#unread"
				Else
					ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, ForumID, Thread.ThreadID) & "#unread"
				End If
			Else
				'Get the Index
				Dim PostIndex As Integer = cltUserThread.GetPostIndexFirstUnread(Thread.ThreadID, usrThread.LastVisitDate, CurrentForumUser.ViewDescending)
				Dim PageCount As Integer = CInt(Math.Ceiling(CDec(Thread.TotalPosts / CurrentForumUser.PostsPerPage)))
				Dim PageNumber As Integer = 1

				Do While PageNumber <= PageCount
					If (CurrentForumUser.PostsPerPage * PageNumber) >= PostIndex Then
						If PageNumber = 1 Then
							ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, ForumID, Thread.ThreadID) & "#unread"
						Else
							ReadLink = Utilities.Links.ContainerViewThreadPagedLink(TabID, ForumID, Thread.ThreadID, PageNumber) & "#unread"
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