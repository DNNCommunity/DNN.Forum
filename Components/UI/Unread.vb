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
	''' This renders a list of threads containing unread posts
	''' </summary>
	''' <remarks>All UI is done in code, no corresponding ascx
	''' </remarks>
	Public Class Unread
		Inherits ForumObject

#Region "Private Declarations"

		Private mThreadCollection As New List(Of ThreadInfo)
		Private mThreadPage As Integer = 0
		Private mThreadsPerPage As Integer = 10
		Dim mTotalRecords As Integer = 0
		Dim Url As String
		Private hsThreadRatings As New Hashtable

#Region "Controls"

		Private cmdRead As LinkButton
		Private trcRating As Telerik.Web.UI.RadRating

#End Region

#End Region

#Region "Public Properties"

		''' <summary>
		''' The collection of threads returned. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ThreadCollection() As List(Of ThreadInfo)
			Get
				Return mThreadCollection
			End Get
		End Property

		''' <summary>
		''' Page user is on in this view.  
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ThreadPage() As Integer
			Get
				Return mThreadPage
			End Get
		End Property

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Marks all threads in the forum as unread 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub cmdRead_Clicked(ByVal sender As Object, ByVal e As System.EventArgs)
			Try
				Dim ctlThread As New ThreadController
				Dim userThreadController As New UserThreadsController

				mThreadCollection = ctlThread.ThreadGetUnread(ModuleID, ForumControl.ThreadsPerPage, ThreadPage, LoggedOnUser.UserID)
				Dim thread As ThreadInfo

				For Each thread In mThreadCollection
					userThreadController.MarkAll(LoggedOnUser.UserID, thread.ForumID, True)
				Next

			Catch ex As Exception
				LogException(ex)
			End Try

			'Well since there are no more unread threads, we'll send the user to forum home
			HttpContext.Current.Response.Redirect(Utilities.Links.ContainerForumHome(TabID), True)
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Creates a new instance of this class
		''' </summary>
		''' <param name="forum"></param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal forum As DNNForum)
			MyBase.New(forum)

			' User might access this page by typing url so better check if they are logged in
			If LoggedOnUser.UserID = -1 Then
				HttpContext.Current.Response.Redirect(Utilities.Links.ContainerForumHome(TabID), True)
				Exit Sub
			End If

			If objConfig.OverrideTitle Then
				Me.BaseControl.BasePage.Title = Me.BaseControl.PortalName
			End If

			If Not HttpContext.Current.Request.QueryString("CurrentPage") Is Nothing Then
				mThreadPage = Int32.Parse(HttpContext.Current.Request.QueryString("CurrentPage"))
			End If

			If mThreadPage > 0 Then
				mThreadPage = mThreadPage - 1
			End If

			mThreadsPerPage = LoggedOnUser.ThreadsPerPage
		End Sub

		''' <summary>
		''' Create an instance of the controls used here
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Overrides Sub CreateChildControls()
			Controls.Clear()

			' display tracking option only if user authenticated
			If ForumControl.LoggedOnUserID > 0 Then
				cmdRead = New LinkButton
				With cmdRead
					.CssClass = "Forum_Profile"
					.ID = "chkRead"
					.Text = ForumControl.LocalizedText("MarkThreadAsRead")
				End With

			End If

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
					.ItemCount = 5

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
			RenderTableBegin(wr, 0, 0, "ThreadsTable")
			RenderNavBar(wr, objConfig, ForumControl)
			RenderBreadCrumbRow(wr)
			If mThreadCollection.Count > 0 Then
				RenderThreads(wr)
				RenderFooterRow(wr)
				RenderBottomBreadCrumbRow(wr)
			Else
				RenderNoThreads(wr)
			End If
			RenderTableEnd(wr)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Loads all the handlers for the controls used in this view
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub AddControlHandlers()
			Try
				AddHandler cmdRead.Click, AddressOf cmdRead_Clicked

			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Loads the controls early on in the control's lifecycle so they can be used later on
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub AddControlsToTree()
			Try
				Controls.Add(cmdRead)
			Catch exc As Exception
				LogException(exc)
			End Try
		End Sub

		''' <summary>
		''' Binds data to control objects.
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindControls()
			' Now we get threads containing new posts to display for this user
			Dim ctlThread As New ThreadController
			mThreadCollection = ctlThread.ThreadGetUnread(ModuleID, ForumControl.ThreadsPerPage, ThreadPage, LoggedOnUser.UserID)
		End Sub

		''' <summary>
		''' No Threads
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub RenderNoThreads(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>

			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "left", "bottom", "", "")
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")
			RenderRowBegin(wr) '<tr>
			RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "NormalRed", "", "100%", "center", "", "", "") ' <td>

			wr.Write("<br />" & Localization.GetString("NoUnreadThreads", objConfig.SharedResourceFile))

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>

			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </Td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderRowEnd(wr) ' </Tr>
		End Sub

		''' <summary>
		''' breadcrumb
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
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

			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "", "", "80%", "left", "", "", "")	 ' <td>
			wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Unread, 0, objConfig, False))
			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "20%", "right", "", "", "") ' <td>

			RenderCellEnd(wr) ' </td>
			RenderCellBegin(wr, "", "", "", "", "", "", "") ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>
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
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub RenderThreads(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) ' <tr>

			' left cap 
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	   '<td><img/></td>

			'CP-** This td must not be closed at end of this function, move that and right cap to end of next function this is the spanning master column
			'middle column, contains everything seen in thread view (using table)  - This needs to span into next function
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") ' <td>

			'Create a table here that will hold everything from the top header type colum (says Thread, Replies, Views, Last Post) through each thread but stop at the footer w/ this table
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <Tr>

			' Threads column
			RenderCellBegin(wr, "", "", "57%", "left", "middle", "", "") '<td>
			'This table is made simply so we can have a height controlling image and apply left cap here
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") '<table>
			RenderRowBegin(wr) ' <Tr>

			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapLeft", "") ' <td></td>

			RenderCellBegin(wr, "Forum_Header", "", "", "", "", "", "")	' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <div>
			wr.Write("&nbsp;" & ForumControl.LocalizedText("Threads"))
			RenderDivEnd(wr) ' </div>
			RenderCellEnd(wr) ' </Td>

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			' Replies
			RenderCellBegin(wr, "Forum_Header", "", "12%", "center", "", "", "") ' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <div>
			wr.Write(ForumControl.LocalizedText("Replies"))
			RenderDivEnd(wr) ' </div>
			RenderCellEnd(wr) ' </td>

			' Views column
			RenderCellBegin(wr, "Forum_Header", "", "12%", "center", "", "", "") '<td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <div>
			wr.Write(ForumControl.LocalizedText("Views"))
			RenderDivEnd(wr) ' </div>
			RenderCellEnd(wr) ' </td>

			' Last Post column
			RenderCellBegin(wr, "", "", "19%", "center", "", "", "") ' <td>
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
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub RenderThreadInfo(ByVal wr As HtmlTextWriter)
			Try
				If ThreadCollection.Count = 0 Then
					'No unread threads inform the user and terminate rendering
					RenderRowBegin(wr) ' <tr>
					RenderCellBegin(wr, "Forum_Row", "", "center", "", "", "4", "") ' <td>
					wr.Write("You have no unread threads!")
					RenderCellEnd(wr)
					RenderRowEnd(wr) '</tr>
					RenderTableEnd(wr)
					RenderTableBegin(wr, "0", "", "", "100%", "", "", "", "", "")
					Exit Sub
				End If

				'loop through each post and make a new row within this table
				Dim Count As Integer = 1
				Dim thread As New ThreadInfo()
				For Each thread In ThreadCollection
					If Not thread Is Nothing Then
						Dim even As Boolean = ThreadIsEven(Count)
						mTotalRecords = thread.TotalRecords

						RenderRowBegin(wr) ' <Tr>

						' cell holds table for post icon/thread subject/rating
						If even Then
							RenderCellBegin(wr, "Forum_Row", "100%", "57%", "", "", "", "") ' <td>
						Else
							RenderCellBegin(wr, "Forum_Row_Alt", "100%", "57%", "", "", "", "") ' <td>
						End If

						' table holds post icon/thread subject/rating
						RenderTableBegin(wr, "", "", "100%", "100%", "2", "0", "", "", "0") ' <table>
						RenderRowBegin(wr) ' <tr>

						' cell within table for thread status icon
						RenderCellBegin(wr, "", "100%", "", "", "top", "", "")	 ' <td>
						Url = Utilities.Links.ContainerViewThreadLink(TabID, thread.ForumID, thread.ThreadID)

						'link so icon is clickable (also used below for subject)
						wr.AddAttribute(HtmlTextWriterAttribute.Href, Url)
						wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>

						' see if post is pinned, priority over other icons
						If thread.IsPinned Then
							' First see if the thread is popular
							If (thread.IsPopular) Then
								' thread IS popular and pinned
								' see if thread is locked
								If (thread.IsClosed) Then
									' thread IS popular, pinned, locked
									RenderImage(wr, objConfig.GetThemeImageURL("s_postlockedpinnedunreadplu.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotNewLockedPinnedThread"), "")
								Else
									' thread IS popular, Pinned but NOT locked
									RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedunreadplus.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewHotPinnedThread"), "")
								End If
							Else
								' thread NOT popular but IS pinned
								' see if thread is locked
								If (thread.IsClosed) Then
									' thread IS pinned, Locked but NOT popular
									RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedlockedunread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewPinnedLockedThread"), "")

								Else
									'thread IS pinned but NOT popular, Locked
									RenderImage(wr, objConfig.GetThemeImageURL("s_postpinnedunread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewPinnedThread"), "")
								End If
							End If
						Else
							' thread not pinned, determine post icon
							RenderImage(wr, GetMediaURL(thread), GetMediaText(thread), "") ' <img/>
						End If

						wr.RenderEndTag() ' </A>
						RenderCellEnd(wr) ' </td>

						' Spacing between status icon and subject
						RenderCellBegin(wr, "", "", "1px", "left", "", "", "")	 ' <td>
						RenderImage(wr, objConfig.GetThemeImageURL("row_spacer.gif"), "", "")	' <img/>
						RenderCellEnd(wr) ' </td>

						' cell for thread subject
						RenderCellBegin(wr, "", "", "100%", "left", "", "", "") ' <td>

						wr.AddAttribute(HtmlTextWriterAttribute.Href, Url)

						Dim SubjectCssClass As String = "Forum_NormalBold"

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

						'Add thread started by
						RenderDivBegin(wr, "", "Forum_NormalSmall") ' <div>
						wr.Write(String.Format("{0}&nbsp;", ForumControl.LocalizedText("by")))
						Url = Utilities.Links.UserPublicProfileLink(TabID, ModuleID, thread.StartedByUserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, LoggedOnUser.Username)
						RenderLinkButton(wr, Url, thread.StartedByUser.SiteAlias, "Forum_NormalSmall") ' <a/>

						'Add in forum (where thread belongs)
						wr.Write(String.Format("&nbsp;{0}&nbsp;", ForumControl.LocalizedText("in")))
						Url = Utilities.Links.ContainerViewForumLink(TabID, thread.ForumID, False)
						RenderLinkButton(wr, Url, thread.HostForum.Name, "Forum_NormalSmall")

						' correct logic to handle posts per page per user
						Dim userPostsPerPage As Integer
						' CapPageCount is number of pages to show as option for user in threads view.
						Dim CapPageCount As Integer = objConfig.PostPagesCount

						If LoggedOnUser.UserID > 0 Then
							userPostsPerPage = LoggedOnUser.PostsPerPage
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
									Url = Utilities.Links.ContainerViewThreadPagedLink(TabID, thread.ForumID, thread.ThreadID, ThreadPage)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, Url)
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
									Url = Utilities.Links.ContainerViewThreadPagedLink(TabID, thread.ForumID, thread.ThreadID, UserPagesCount)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, Url)
									wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalSmall")
									wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
									wr.Write(UserPagesCount.ToString())
									wr.RenderEndTag() ' </A>
								End If
							Else

								For ThreadPage As Integer = 1 To UserPagesCount
									Url = Utilities.Links.ContainerViewThreadPagedLink(TabID, thread.ForumID, thread.ThreadID, ThreadPage)
									wr.AddAttribute(HtmlTextWriterAttribute.Href, Url)
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

						'Add check for RatingsEnabled
						If objConfig.EnableRatings And thread.HostForum.EnableForumsRating Then
							' Display rating image
							RenderCellBegin(wr, "", "", "30%", "right", "", "", "") ' <td>

							''RenderImage(wr, objConfig.GetThemeImageURL(thread.RatingImage), thread.RatingText, "") ' <img/>
							If hsThreadRatings.ContainsKey(thread.ThreadID) Then
								trcRating = CType(hsThreadRatings(thread.ThreadID), Telerik.Web.UI.RadRating)
								' CP - we alter statement below if we want to enable 0 rating still showing image.
								If thread.Rating > 0 Then
									trcRating.RenderControl(wr)
								End If
							End If

							RenderCellEnd(wr) ' </td>
						End If

						'Thread status
						RenderCellBegin(wr, "", "", "5%", "right", "", "", "")	 ' <td>
						If (objConfig.EnableThreadStatus And thread.HostForum.EnableForumsThreadStatus) Or (thread.ThreadStatus = ThreadStatus.Poll And thread.HostForum.AllowPolls) Then
							RenderImage(wr, objConfig.GetThemeImageURL(thread.StatusImage), thread.StatusText, "") ' <img/>
						Else
							RenderImage(wr, objConfig.GetThemeImageURL("status_spacer.gif"), "", "")	' <img/>
						End If
						RenderCellEnd(wr) ' </td>

						RenderRowEnd(wr) ' </tr>
						RenderTableEnd(wr) ' </table>
						RenderCellEnd(wr) ' </td>

						' Replies
						If even Then
							RenderCellBegin(wr, "Forum_RowHighlight1", "", "12%", "center", "", "", "") ' <td>
						Else
							RenderCellBegin(wr, "Forum_RowHighlight1_Alt", "", "12%", "center", "", "", "")	' </td>
						End If

						RenderDivBegin(wr, "", "Forum_Posts") ' <div>
						wr.Write(thread.Replies)
						RenderDivEnd(wr) ' </div>
						RenderCellEnd(wr) ' </td>

						' Views
						If even Then
							RenderCellBegin(wr, "Forum_RowHighlight2", "", "12%", "center", "", "", "") '<td>
						Else
							RenderCellBegin(wr, "Forum_RowHighlight2_Alt", "", "12%", "center", "", "", "")	' </td>
						End If

						RenderDivBegin(wr, "", "Forum_Threads")	' <div>
						wr.Write(thread.Views)
						RenderDivEnd(wr) ' </div>
						RenderCellEnd(wr) ' </td>

						' Post date info & author
						If even Then
							RenderCellBegin(wr, "Forum_RowHighlight3", "", "19%", "right", "", "2", "") ' <td>
						Else
							RenderCellBegin(wr, "Forum_RowHighlight3_Alt", "", "19%", "right", "", "2", "")	' <td>
						End If

						' table holds last post data
						RenderTableBegin(wr, "", "", "100%", "100%", "0", "0", "right", "", "0") ' <table>
						RenderRowBegin(wr) ' <tr>

						RenderCellBegin(wr, "", "", "1px", "", "", "", "") ' <td>
						RenderImage(wr, objConfig.GetThemeImageURL("row_spacer.gif"), "", "")
						RenderCellEnd(wr) ' </td>

						RenderCellBegin(wr, "", "", "", "right", "", "", "") ' <td>
						Url = Utilities.Links.ContainerViewPostLink(TabID, thread.ForumID, thread.LastApprovedPost.PostID)

						' Skeel - This is for showing link to first unread post for logged in users. 
						If LoggedOnUser.DefaultPostsPerPage < thread.TotalPosts Then
							'Find the page on which the first unread post is located
							wr.AddAttribute(HtmlTextWriterAttribute.Href, FirstUnreadLink(thread))
						Else
							'Thread has only one page
							wr.AddAttribute(HtmlTextWriterAttribute.Href, Url + "#unread")
						End If
						wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
						RenderImage(wr, objConfig.GetThemeImageURL("thread_newest.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgThreadNewest"), "")
						wr.RenderEndTag() ' </a>

						RenderTitleLinkButton(wr, Url + "#p" + CStr(thread.LastApprovedPostID), Utilities.ForumUtils.GetCreatedDateInfo(thread.LastApprovedPost.CreatedDate, objConfig, ""), "Forum_LastPostText", thread.LastPostShortBody)	' <a/>

						RenderDivBegin(wr, "", "Forum_LastPostText")	   ' <div>
						wr.Write(ForumControl.LocalizedText("by") & " ")
						Url = Utilities.Links.UserPublicProfileLink(TabID, ModuleID, thread.LastApprovedUser.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, LoggedOnUser.Username)
						RenderLinkButton(wr, Url, thread.LastApprovedUser.SiteAlias, "Forum_LastPostText") ' <a/>
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
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	   ' <td><img/></td>
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
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub RenderFooterRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	   ' <td><img/></td>

			'Middle Column
			RenderCellBegin(wr, "", "", "100%", "", "middle", "", "") ' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "middle", "0")	  ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapLeft", "") ' <td><img/></td>
			RenderCellBegin(wr, "Forum_Footer", "", "", "", "", "", "")	' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")	' <table>
			RenderRowBegin(wr) ' <tr>

			RenderCellBegin(wr, "", "", "", "", "top", "", "") ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "")	' <img/>
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "", "", "100%", "left", "middle", "", "") ' <td>
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

			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	   ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' bottom Breadcrumb and ddl's, along w/ subscribe chkbx (last row)
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub RenderBottomBreadCrumbRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "left", "", "", "") ' top

			' start middle column table
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")

			'breadcrumb row
			RenderRowBegin(wr) '<Tr>
			RenderCellBegin(wr, "", "", "100%", "left", "top", "", "")
			Dim ChildGroupView As Boolean = False
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				ChildGroupView = True
			End If
			wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.Unread, 0, objConfig, ChildGroupView))
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>

			' Mark As Read linkbutton
			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "", "", "", "right", "", "", "")
			cmdRead.RenderControl(wr)
			RenderImage(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
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
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Sub RenderThreadsPaging(ByVal wr As HtmlTextWriter)
			' First, previous, next, last thread hyperlinks
			Dim ctlPagingControl As New DotNetNuke.Modules.Forum.WebControls.PagingControl
			ctlPagingControl.CssClass = "Forum_FooterText"
			ctlPagingControl.TotalRecords = mTotalRecords
			ctlPagingControl.PageSize = ForumControl.ThreadsPerPage
			ctlPagingControl.CurrentPage = ThreadPage + 1

			Dim Params As String = "scope=unread"

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
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
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
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Function GetMediaURL(ByVal Thread As ThreadInfo) As String
			If Thread.IsClosed Then
				' thread IS locked
				If (Thread.IsPopular) Then
					' thread IS locked, popular
					Return objConfig.GetThemeImageURL("s_postlockedunreadplus.") & objConfig.ImageExtension
				Else
					' thread IS locked NOT popular
					Return objConfig.GetThemeImageURL("s_postlockedunread.") & objConfig.ImageExtension
				End If

			Else
				' thread NOT locked
				If (Thread.IsPopular) Then
					Return objConfig.GetThemeImageURL("s_postunreadplus.") & objConfig.ImageExtension
				Else
					Return objConfig.GetThemeImageURL("s_postunread.") & objConfig.ImageExtension
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
		''' <history>
		''' 	[skeel]	12/14/2008	Created
		''' </history>
		Private Function GetMediaText(ByVal Thread As ThreadInfo) As String
			If Thread.IsClosed Then
				' thread IS locked
				If (Thread.IsPopular) Then
					' thread IS locked, popular

					Return ForumControl.LocalizedText("imgNewPopLockedThread")
				Else
					' thread IS locked NOT popular
					Return ForumControl.LocalizedText("imgNewLockedThread")
				End If
			Else
				' thread NOT locked
				If (Thread.IsPopular) Then
					' thread NOT locked IS popular
					Return ForumControl.LocalizedText("imgNewPopThread")
				Else
					' thread NOT locked, popular
					Return ForumControl.LocalizedText("imgNewThread")
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

			ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, Thread.ForumID, Thread.ThreadID) & "#unread"
			usrThread = cltUserThread.GetCachedUserThreadRead(LoggedOnUser.UserID, Thread.ThreadID)

			If usrThread Is Nothing Then
				'All new
				If LoggedOnUser.ViewDescending = True Then
					Dim PageCount As Decimal = CDec(Thread.TotalPosts / LoggedOnUser.DefaultPostsPerPage)
					PageCount = Math.Ceiling(PageCount)
					ReadLink = Utilities.Links.ContainerViewThreadPagedLink(TabID, Thread.ForumID, Thread.ThreadID, CInt(PageCount)) & "#unread"
				Else
					ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, Thread.ForumID, Thread.ThreadID) & "#unread"
				End If
			Else
				'Get the Index
				Dim PostIndex As Integer = cltUserThread.GetPostIndexFirstUnread(Thread.ThreadID, usrThread.LastVisitDate, LoggedOnUser.ViewDescending)
				Dim PageCount As Integer = CInt(Math.Ceiling(CDec(Thread.TotalPosts / LoggedOnUser.DefaultPostsPerPage)))
				Dim PageNumber As Integer = 1

				Do While PageNumber <= PageCount
					If (LoggedOnUser.DefaultPostsPerPage * PageNumber) >= PostIndex Then
						If PageNumber = 1 Then
							ReadLink = Utilities.Links.ContainerViewThreadLink(TabID, Thread.ForumID, Thread.ThreadID) & "#unread"
						Else
							ReadLink = Utilities.Links.ContainerViewThreadPagedLink(TabID, Thread.ForumID, Thread.ThreadID, PageNumber) & "#unread"
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