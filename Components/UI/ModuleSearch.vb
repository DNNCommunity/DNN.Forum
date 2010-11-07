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
	''' This renders the search results view (similar to thread view - only reachable from modules search, user profile, or my settings)
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class ThreadSearch
		Inherits ForumObject

#Region "Private Declarations"

		Private _TotalRecords As Integer = 0
		Private _CurrentPage As Integer
		Private _ThreadCollection As New List(Of ThreadInfo)
		Private _PostCollection As New List(Of PostInfo)
		Private _ThreadRatings As New Hashtable
		Private _NoResults As Boolean
		Private _SearchTerms As String
		Private _StartDate As DateTime
		Private _EndDate As DateTime

#Region "Controls"

		Private trcRating As Telerik.Web.UI.RadRating

#End Region

		''' <summary>
		''' Used to retrieve only posts with no replies if true.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property NoReply() As Boolean
			Get
				If Not HttpContext.Current.Request.QueryString("noreply") Is Nothing Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property

		''' <summary>
		''' Determines if the user is using aggregated view. This is a similar look to threads view.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property Aggregated() As Boolean
			Get
				If HttpContext.Current.Request.QueryString("aggregated") IsNot Nothing Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property

		''' <summary>
		''' Determines if the user is in 'my threads' view (like aggregated).
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property MyThreads() As Boolean
			Get
				If HttpContext.Current.Request.QueryString("mythreads") IsNot Nothing Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property

		''' <summary>
		''' Determines if the user is in 'latest hours' view (like aggregated).
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property LatestHours() As Boolean
			Get
				If HttpContext.Current.Request.QueryString("latesthours") IsNot Nothing Then
					Return True
				Else
					Return False
				End If
			End Get
		End Property

		''' <summary>
		''' Determines if we are applying a threadstatus as part of our search criteria. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property ThreadStatusID() As Integer
			Get
				If HttpContext.Current.Request.QueryString("threadstatusid") IsNot Nothing Then
					Return Convert.ToInt32(HttpContext.Current.Request.QueryString("threadstatusid"))
				Else
					Return -1
				End If
			End Get
		End Property

		''' <summary>
		''' The total number of threads/posts available for viewing.
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
		''' The collection of threads returned when in my threads/aggregated views. 
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
		''' The collection of posts returned, when doing a search or 'my posts' view.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property PostCollection() As List(Of PostInfo)
			Get
				Return _PostCollection
			End Get
			Set(ByVal Value As List(Of PostInfo))
				_PostCollection = Value
			End Set
		End Property

		''' <summary>
		''' A collection of ratings controls. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This is only used in 'my threads'/aggregated view.</remarks>
		Private Property ThreadRatings() As Hashtable
			Get
				Return _ThreadRatings
			End Get
			Set(ByVal Value As Hashtable)
				_ThreadRatings = Value
			End Set
		End Property

		''' <summary>
		''' Determines if the performed search returned any results. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property NoResults() As Boolean
			Get
				Return _NoResults
			End Get
			Set(ByVal Value As Boolean)
				_NoResults = Value
			End Set
		End Property

		''' <summary>
		''' The search terms that are sent to the query for post/thread retrieval.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property SearchTerms() As String
			Get
				Return _SearchTerms
			End Get
			Set(ByVal Value As String)
				_SearchTerms = Value
			End Set
		End Property

		''' <summary>
		''' The start date/time of our search period.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property StartDate() As DateTime
			Get
				Return _StartDate
			End Get
			Set(ByVal Value As DateTime)
				_StartDate = Value
			End Set
		End Property

		''' <summary>
		''' The end date/time of our search period. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property EndDate() As DateTime
			Get
				Return _EndDate
			End Get
			Set(ByVal Value As DateTime)
				_EndDate = Value
			End Set
		End Property

#End Region

#Region "Public Methods"

#Region "Constructors"

		''' <summary>
		''' Instantiates the class and sets up the initial properties
		''' </summary>
		''' <param name="forum"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal forum As DNNForum)
			MyBase.New(forum)

			If Not HttpContext.Current.Request.QueryString("CurrentPage") Is Nothing Then
				CurrentPage = Int32.Parse(HttpContext.Current.Request.QueryString("CurrentPage"))
			End If

			If CurrentPage > 0 Then
				CurrentPage = CurrentPage - 1
			End If

			Dim Term As New SearchTerms
			Dim subject As String
			Dim body As String
			Dim forums As String
			Dim authors As String
			Dim HasForums As Boolean = False
			Dim HasAuthors As Boolean = False
			Dim HasSubject As Boolean = False
			Dim HasBody As Boolean = False

			'CP - KNOWN ISSUE: By default, we are limiting searches to the past year (unless the user specifies otherwise)
			If Not HttpContext.Current.Request.QueryString("fromdate") Is Nothing Then
				Dim strStartDate As String = HttpContext.Current.Request.QueryString("fromdate")
				StartDate = Utilities.ForumUtils.NumToDate(Double.Parse(strStartDate))
			Else
				StartDate = DateAdd(DateInterval.Year, -1, DateTime.Today)
			End If

			If Not HttpContext.Current.Request.QueryString("todate") Is Nothing Then
				EndDate = Utilities.ForumUtils.NumToDate(Double.Parse(HttpContext.Current.Request.QueryString("todate")))
			Else
				EndDate = DateAdd(DateInterval.Day, 1, DateTime.Today)
			End If

			If Not HttpContext.Current.Request.Params("forums") Is Nothing Then
				forums = HttpContext.Current.Request.Params("forums")
				If forums.Length > 0 Then
					Dim str As String = String.Empty
					For Each forumid As String In forums.Split(";"c)
						If Trim(forumid).Length > 0 Then
							str += forumid.ToString & ","
						End If
					Next

					If str.Length > 0 Then
						HasForums = True
						str = Left(str, str.Length - 1)
						Term.AddSearchTerm("ForumID", CompareOperator.HaveValueIn, Trim(str))
					End If

				End If
			End If

			If Not HttpContext.Current.Request.Params("authors") Is Nothing Then
				authors = HttpContext.Current.Request.Params("authors")
				If Len(authors) > 0 Then
					Dim str As String = String.Empty
					For Each userid As String In authors.Split(";"c)
						If Trim(userid).Length > 0 Then
							str += userid.ToString + ","
						End If
					Next

					If str.Length > 0 Then
						HasAuthors = True
						str = Left(str, str.Length - 1)
						Term.AddSearchTerm("UserID", CompareOperator.HaveValueIn, Trim(str))
					End If
				End If
			End If

			' [skeel] 9/1/2009 (Complex language support): 
			' Some languages uses special letters, these are HTML Encoded by the TextEditor before being sent to the
			' database. In order to support seach including these letters, we need to HTML Encode
			' the search string for both Body and Subject to get a macth from the database, as they are stored
			' there as &#[ascii number]; As the search terms utilize the DNN 'PortalSecurity.FilterFlag.NoSQL'
			' all semicolons will be removed. The solution is to replace ; with :semi: before the search
			' term is added, and then convert it back befire the final TSQL statement is sent to the DB.
			' The changes are tagged as: Complex language support
			If Not HttpContext.Current.Request.Params("subject") Is Nothing AndAlso Not HttpContext.Current.Request.Params("body") Is Nothing Then
				subject = HttpContext.Current.Request.Params("subject")
				If subject.Length > 0 AndAlso subject <> " " Then
					subject = HttpUtility.UrlDecode(subject.Trim())
					Dim ExactMatch As Boolean = False

					' Complex language support
					subject = HttpUtility.HtmlEncode(subject)
					subject = subject.Replace(";", ":semi:")

					HasSubject = True

					If ExactMatch Then
						Term.AddSearchTerm("(Subject", CompareOperator.EqualString, subject)
					Else
						Term.AddSearchTerm("(Subject", CompareOperator.Contains, subject)
					End If
				End If
				body = HttpContext.Current.Request.Params("body")
				If body.Length > 0 AndAlso body <> " " Then
					HasBody = True
					body = HttpUtility.UrlDecode(body.Trim())
					' Complex language support
					body = HttpUtility.HtmlEncode(body)
					body = body.Replace(";", ":semi:")

					Term.AddSearchTerm("Body)", CompareOperator.Contains, body, " OR ")
				End If
			Else
				If Not HttpContext.Current.Request.Params("subject") Is Nothing Then
					subject = HttpContext.Current.Request.Params("subject")
					If subject.Length > 0 AndAlso subject <> " " Then
						subject = HttpUtility.UrlDecode(subject.Trim())
						Dim ExactMatch As Boolean = False

						'If subject.StartsWith("""") Then
						'	ExactMatch = True
						'	subject = subject.Replace("""", "")
						'End If

						' Complex language support
						subject = HttpUtility.HtmlEncode(subject)
						subject = subject.Replace(";", ":semi:")

						HasSubject = True

						If ExactMatch Then
							Term.AddSearchTerm("Subject", CompareOperator.EqualString, subject)
						Else
							Term.AddSearchTerm("Subject", CompareOperator.Contains, subject)
						End If
					End If
				End If

				If Not HttpContext.Current.Request.Params("body") Is Nothing Then
					'[skeel] added support for words out of context
					body = HttpContext.Current.Request.Params("body")
					If body.Length > 0 AndAlso body <> " " Then
						body = HttpUtility.UrlDecode(body.Trim())
						' Complex language support
						body = HttpUtility.HtmlEncode(body)
						body = body.Replace(";", ":semi:")

						HasBody = True
						If Aggregated = False Then
							'Post search
							If body.IndexOf(",") > 0 Then
								'search contains several words
								Dim SearchArray() As String = SplitSearchString(body)
								Dim strWord As String
								Dim i As Integer = 1
								For Each strWord In SearchArray
									If i = 1 Then
										Term.AddSearchTerm("", CompareOperator.And, "", "")
										Term.AddSearchTerm("", CompareOperator.LeftParentes, "", "")
										Term.AddSearchTerm("Body", CompareOperator.Contains, strWord.Trim(), "")
									Else
										Term.AddSearchTerm("Body", CompareOperator.Contains, strWord.Trim(), " OR ")
									End If
									i = i + 1
								Next
								Term.AddSearchTerm("", CompareOperator.RightParentes, "", "")
							Else
								'search contains only one word
								Term.AddSearchTerm("Body", CompareOperator.Contains, body)
							End If
						Else
							'Thread search
							Term.AddSearchTerm("Body", CompareOperator.Contains, body)
						End If

					End If
				End If
			End If

			If NoReply Then
				Term.AddSearchTerm("Replies", CompareOperator.EqualString, "0")
			Else
				Term.AddSearchTerm("Replies", CompareOperator.GreaterThanOrEqualTo, "0")
			End If

			If LatestHours Then
				subject = HttpContext.Current.Request.Params("latesthours")
				If subject.Length > 0 AndAlso subject <> " " Then

					Try
						Dim hours As Integer = CInt(subject)
						EndDate = Now
						StartDate = Now.AddHours((hours * -1))
					Catch ex As Exception
						'Do nothing
					End Try
				End If
			End If

			SearchTerms = Term.WhereClause

			If (Not SearchTerms = Null.NullString) Then
				SearchTerms = SearchTerms.Replace(":amp:", "&")
				SearchTerms = SearchTerms.Replace(":perc:", "%")
				' Complex language support
				SearchTerms = SearchTerms.Replace(":semi:", ";")
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
			BindControls()

			Dim ctlSearch As New SearchController
			Dim InThreadView As Boolean = False

			If Aggregated Or MyThreads Or LatestHours Then
				InThreadView = True
			End If

			If InThreadView Then
				ThreadCollection = ctlSearch.SearchGetResults(SearchTerms, CurrentPage, CurrentForumUser.ThreadsPerPage, CurrentForumUser.UserID, ForumControl.ModuleID, StartDate, EndDate, ThreadStatusID)

				If ThreadCollection.Count > 0 Then
					TotalRecords = CType(ThreadCollection(0), ThreadInfo).TotalRecords
				End If

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
						.Value = CDec(thread.Rating)
						'AddHandler trcRating.Command, AddressOf trcRating_Rate
					End With
					ThreadRatings.Add(thread.ThreadID, trcRating)
					Controls.Add(trcRating)
				Next
			Else
				PostCollection = ctlSearch.Search(SearchTerms, CurrentPage, CurrentForumUser.PostsPerPage, CurrentForumUser.UserID, ForumControl.ModuleID, StartDate, EndDate, ThreadStatusID)

				If PostCollection.Count > 0 Then
					TotalRecords = PostCollection(0).TotalRecords
				End If
			End If
		End Sub

		''' <summary>
		''' Builds the UI
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Public Overrides Sub Render(ByVal wr As HtmlTextWriter)
			If Not ForumControl.Page.IsPostBack Then
				RenderTableBegin(wr, 0, 0, "SearchResultsTable")
				RenderNavBar(wr, objConfig, ForumControl)
				RenderBreadCrumbRow(wr)
				If Aggregated Or myThreads Or LatestHours Then
					RenderThreadSearchResults(wr)
				Else
					RenderPostSearchResults(wr)
				End If
				RenderFooter(wr)
				RenderBottomBreadCrumb(wr)
				RenderTableEnd(wr)
			End If
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Determines what the page size is, gets the search results collection
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindControls()
			If CurrentForumUser.UserID > 0 Then
				Dim cntForumUser As New ForumUserController
				Dim forumUser As ForumUserInfo
				forumUser = cntForumUser.GetForumUser(CurrentForumUser.UserID, False, ForumControl.ModuleID, PortalID)
			End If
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

			RenderCellBegin(wr, "", "", "80%", "", "", "", "") ' <td> 
			Dim ChildGroupView As Boolean = False
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				ChildGroupView = True
			End If

			If Aggregated Then
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.ThreadSearch, ThreadCollection, objConfig, ChildGroupView))
			Else
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.ThreadSearch, Nothing, objConfig, ChildGroupView))
			End If

			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "", "", "20%", "right", "", "", "") ' <td>

			If Aggregated Then
				If NoReply Then
					' show link to show all threads
					RenderLinkButton(wr, Utilities.Links.ContainerAggregatedLink(TabID, False), Localization.GetString("ShowAll", objConfig.SharedResourceFile), "Forum_BreadCrumb")
				Else
					' show link to show no reply threads
					RenderLinkButton(wr, Utilities.Links.ContainerAggregatedLink(TabID, True), Localization.GetString("ShowNoReplies", objConfig.SharedResourceFile), "Forum_BreadCrumb")
				End If
			Else
				wr.Write("&nbsp;")
			End If
			RenderCellEnd(wr) ' </td>

			RenderRowEnd(wr) ' </Tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </Td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderRowEnd(wr) ' </Tr>
		End Sub

		''' <summary>
		''' Renders each row of results in the UI as posts
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderPostSearchResults(ByVal wr As HtmlTextWriter)
			'Check if we have any hits
			If PostCollection.Count = 0 Then
				'Start Cell
				RenderRowBegin(wr) '<tr>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderCellBegin(wr, "NormalRed", "", "100%", "center", "", "", "") '<td>
				wr.Write("<br />" & Localization.GetString("SearchNoResult", objConfig.SharedResourceFile))

				'End Cell 
				RenderCellEnd(wr) ' </Td>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderRowEnd(wr) ' </Tr>
				NoResults = True
				Exit Sub
			Else
				'Ok we found something, let's display a summery
				'Start Cell
				RenderRowBegin(wr) '<tr>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderCellBegin(wr, "Forum_NormalBold", "", "100%", "right", "", "", "") '<td>
				wr.Write(String.Format(Localization.GetString("SearchResult", objConfig.SharedResourceFile), TotalRecords) & ":")

				'End Cell 
				RenderCellEnd(wr) ' </Td>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderRowEnd(wr) ' </Tr>
			End If

			'Display the results
			Dim url As String
			Dim Count As Integer = 1
			For Each objPost As PostInfo In PostCollection
				'Handle Css
				Dim authorCellClass As String = String.Empty
				Dim bodyCellClass As String = String.Empty
				Dim detailCellClass As String = String.Empty
				Dim postBodyClass As String = String.Empty
				Dim buttonCellClass As String = String.Empty
				Dim postCountIsEven As Boolean = ThreadIsEven(Count)

				' these classes to set bg color of cells
				If postCountIsEven Then
					authorCellClass = "Forum_Avatar"
					bodyCellClass = "Forum_PostBody_Container"
					detailCellClass = "Forum_PostDetails"
					postBodyClass = "Forum_PostBody"
					buttonCellClass = "Forum_PostButtons"
				Else
					authorCellClass = "Forum_Avatar_Alt"
					bodyCellClass = "Forum_PostBody_Container_Alt"
					detailCellClass = "Forum_PostDetails_Alt"
					postBodyClass = "Forum_PostBody_Alt"
					buttonCellClass = "Forum_PostButtons_Alt"
				End If

				'Start Cell
				RenderRowBegin(wr) '<tr>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderCellBegin(wr, "", "", "100%", "left", "", "", "") '<td>

				'Middle Cell
				RenderTableBegin(wr, "", "", "", "100%", "0", "0", "center", "middle", "0")  ' <table> 
				RenderRowBegin(wr) ' <tr>
				RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapLeft", "") ' <td><img /></td>

				' start post status image
				RenderCellBegin(wr, "Forum_Header", "", "1%", "left", "middle", "", "") '<td>
				' display "new" image if this post is new since last time user visited the thread (leave even if permitting anonymous posting)
				If HttpContext.Current.Request.IsAuthenticated Then
					If NewPost(objPost.ThreadID, objPost.CreatedDate) Then
						RenderImage(wr, objConfig.GetThemeImageURL("s_new.") & objConfig.ImageExtension, ForumControl.LocalizedText("UnreadPost"), "")
					Else
						RenderImage(wr, objConfig.GetThemeImageURL("s_old.") & objConfig.ImageExtension, ForumControl.LocalizedText("ReadPost"), "")
					End If
				Else
					RenderImage(wr, objConfig.GetThemeImageURL("s_new.") & objConfig.ImageExtension, ForumControl.LocalizedText("UnreadPost"), "")
				End If
				RenderCellEnd(wr) ' </td> 

				'Forum Link
				RenderCellBegin(wr, "Forum_Header", "", "", "", "middle", "", "")    '<td>
				RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
				wr.Write("&nbsp;" & Localization.GetString("Forum", objConfig.SharedResourceFile) & ": ")
				url = Utilities.Links.ContainerViewForumLink(TabID, objPost.ForumID, False)
				RenderLinkButton(wr, url, objPost.ParentThread.ContainingForum.Name, "Forum_HeaderText", "")
				RenderDivEnd(wr) ' </span>
				RenderCellEnd(wr) ' </td>

				'Post Date
				RenderCellBegin(wr, "Forum_Header", "", "", "right", "middle", "", "")   '<td>
				RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
				wr.Write("&nbsp;" & Replace(Utilities.ForumUtils.GetCreatedDateInfo(objPost.CreatedDate, objConfig, ""), "<br />", "") & "&nbsp;&nbsp;")
				RenderDivEnd(wr) ' </span>
				RenderCellEnd(wr) ' </td> 
				RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapRight", "")
				RenderRowEnd(wr) ' </tr>
				RenderTableEnd(wr) ' </table>

				'End Cell 
				RenderCellEnd(wr) ' </Td>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderRowEnd(wr) ' </Tr>

				'Start Cell
				RenderRowBegin(wr) '<tr>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderCellBegin(wr, "", "", "100%", "left", "", "", "") '<td>

				'Middle Cell
				RenderTableBegin(wr, "", "", "", "100%", "0", "0", "center", "middle", "0")  ' <table> 
				RenderRowBegin(wr) ' <tr>

				'Author Cell
				RenderCellBegin(wr, authorCellClass, "", "20%", "center", "top", "1", "1")	  ' <td>
				'link to user profile, always display in both view
				RenderAuthor(wr, objPost, postCountIsEven)
				RenderCellEnd(wr) ' </td>

				'Post Cell Start
				RenderCellBegin(wr, bodyCellClass, "100%", "80%", "left", "top", "", "")	   '<td>

				'Search Cell
				RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "")  ' <table> 
				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, detailCellClass, "", "", "", "top", "", "") ' <td>

				'Subject
				RenderDivBegin(wr, "", "Forum_Profile")	' <span>
				url = Utilities.Links.ContainerViewPostLink(TabID, objPost.ForumID, objPost.PostID)
				RenderLinkButton(wr, url, objPost.Subject, "Forum_Profile", "")
				RenderDivEnd(wr) ' </span>
				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>

				'This is to support layout classes and draw a missing border...
				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, buttonCellClass, "1", "", "", "", "", "") ' <td>
				wr.Write("<span style=""font-size:1px"">&nbsp;</span>")
				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>

				'Body
				RenderRowBegin(wr) ' <tr>
				RenderCellBegin(wr, postBodyClass, "", "", "left", "top", "", "") ' <td>
				RenderDivBegin(wr, "", "Forum_Normal")
				wr.Write(ProcessSearchBody(objPost.Body, objPost.CreatedDate))
				RenderDivEnd(wr) ' </span>
				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>
				RenderTableEnd(wr) ' </table>

				'Postr Cell End
				RenderCellEnd(wr) ' </Td>

				'End middle cell
				RenderRowEnd(wr) ' </Tr>
				RenderTableEnd(wr) ' </table>

				'End Cell 
				RenderCellEnd(wr) ' </td>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderRowEnd(wr) ' </tr>

				'Add spacer
				If Count < PostCollection.Count Then
					RenderSpacerRow(wr)
				End If

				'Add to count
				Count = Count + 1
			Next
		End Sub

		''' <summary>
		''' Renders this views post header columns
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderThreadSearchResults(ByVal wr As HtmlTextWriter)
			'[skeel] Check if we have any hits
			If ThreadCollection.Count = 0 Then
				'Start Cell
				RenderRowBegin(wr) '<tr>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderCellBegin(wr, "NormalRed", "", "100%", "center", "", "", "") '<td>

				wr.Write("<br />" & Localization.GetString("SearchNoResult", objConfig.SharedResourceFile))

				'End Cell 
				RenderCellEnd(wr) ' </td>
				RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
				RenderRowEnd(wr) ' </tr>
				NoResults = True
				Exit Sub
			Else
				'Ok we found something, let's display a summary
				If myThreads Or LatestHours Then
					'Start Cell
					RenderRowBegin(wr) '<tr>
					RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
					RenderCellBegin(wr, "Forum_NormalBold", "", "100%", "right", "", "", "") '<td>
					If myThreads Then
						wr.Write("&nbsp;")
					Else
						'LatestHours
						wr.Write(String.Format(Localization.GetString("SearchResult", objConfig.SharedResourceFile), TotalRecords) & ":")
					End If

					'End Cell 
					RenderCellEnd(wr) ' </td>
					RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
					RenderRowEnd(wr) ' </tr>
				End If
			End If

			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")		 '<td><img/></td>
			RenderCellBegin(wr, "", "", "100%", "", "", "", "") ' <td>

			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") ' <table>
			RenderRowBegin(wr) ' <tr>
			' Threads column
			RenderCellBegin(wr, "", "", "52%", "left", "middle", "", "") '<td>
			'This table is made simply so we can have a height controlling image and apply left cap here
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") '<table>
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapLeft", "") ' <td></td>
			RenderCellBegin(wr, "Forum_Header", "", "", "", "", "", "")	' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
			wr.Write("&nbsp;" & ForumControl.LocalizedText("Threads"))
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td>

			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			' Replies
			RenderCellBegin(wr, "Forum_Header", "", "11%", "center", "", "", "") ' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
			wr.Write(ForumControl.LocalizedText("Replies"))
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td>

			' Views column
			RenderCellBegin(wr, "Forum_Header", "", "11%", "center", "", "", "") '<td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
			wr.Write(ForumControl.LocalizedText("Views"))
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td>

			' Last Post column
			RenderCellBegin(wr, "", "", "26%", "center", "", "", "") ' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0") '<table>
			RenderRowBegin(wr) ' <tr>
			RenderCellBegin(wr, "Forum_Header", "", "", "center", "", "", "") ' <td>
			RenderDivBegin(wr, "", "Forum_HeaderText") ' <span>
			wr.Write("&nbsp;" & ForumControl.LocalizedText("LastPost"))
			RenderDivEnd(wr) ' </span>
			RenderCellEnd(wr) ' </td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_HeaderCapRight", "") ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			RenderRowEnd(wr) ' </tr>
			RenderSearchInfo(wr)
		End Sub

		''' <summary>
		''' Renders each row of results in the UI
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderSearchInfo(ByVal wr As HtmlTextWriter)
			Dim Count As Integer = 1
			Dim url As String

			For Each objThread As ThreadInfo In ThreadCollection
				Dim even As Boolean = ThreadIsEven(Count)

				RenderRowBegin(wr) ' <tr>
				' cell holds table for post icon/thread subject/rating
				If even Then
					RenderCellBegin(wr, "Forum_Row", "", "52%", "left", "", "", "") ' <td>
				Else
					RenderCellBegin(wr, "Forum_Row_Alt", "", "52%", "left", "", "", "") ' <td>
				End If

				' table holds post icon/thread subject/rating
				RenderTableBegin(wr, "", "", "100%", "100%", "0", "0", "", "", "0") ' <table>
				RenderRowBegin(wr) ' <tr>

				' cell within table for thread status icon
				RenderCellBegin(wr, "", "100%", "", "left", "", "", "") ' <td>

				' Determine url here so we can use it for thread name and icon
				url = Utilities.Links.ContainerViewThreadLink(TabID, objThread.ForumID, objThread.ThreadID)

				' see if post is pinned, priority over other icons
				If objThread.LastApprovedPost.ParentThread.IsPinned Then
					' First see if the thread is popular
					If (objThread.LastApprovedPost.ParentThread.IsPopular) Then
						' thread IS popular and pinned
						' see if thread is locked
						If (objThread.LastApprovedPost.ParentThread.IsClosed) Then
							' thread IS popular, pinned, locked
							' See if this is an unread post
							If (HasNewPosts(CurrentForumUser.UserID, objThread.LastApprovedPost.ParentThread)) Then
								' IS read
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("s_postlockedpinnedunreadplu.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotNewLockedPinnedThread"), "")
							Else
								' not read
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("s_postlockedpinnedreadplus.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotLockedPinnedThread"), "")
							End If
						Else
							' thread IS popular, Pinned but NOT locked
							' See if this is an unread post
							If (HasNewPosts(CurrentForumUser.UserID, objThread.LastApprovedPost.ParentThread)) Then
								' IS read
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("s_postpinnedunreadplus.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewHotPinnedThread"), "")
							Else
								' not read
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("s_postpinnedreadplus.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgHotPinnedThread"), "")
							End If
						End If
					Else
						' thread NOT popular but IS pinned
						' see if thread is locked
						If (objThread.LastApprovedPost.ParentThread.IsClosed) Then
							' thread IS pinned, Locked but NOT popular
							If (HasNewPosts(CurrentForumUser.UserID, objThread.LastApprovedPost.ParentThread)) Then
								' IS read
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("s_postpinnedlockedunread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewPinnedLockedThread"), "")
							Else
								' not read
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("s_postpinnedlockedread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgPinnedLockedThread"), "")
							End If
						Else
							'thread IS pinned but NOT popular, Locked
							If (HasNewPosts(CurrentForumUser.UserID, objThread.LastApprovedPost.ParentThread)) Then
								' IS read
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("s_postpinnedunread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgNewPinnedThread"), "")
							Else
								' not read
								RenderImageButton(wr, url, objConfig.GetThemeImageURL("s_postpinnedread.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgPinnedThread"), "")
							End If
						End If
					End If
				Else
					' thread not pinned, determine post icon
					RenderImageButton(wr, url, GetMediaURL(objThread.LastApprovedPost.ParentThread), GetMediaText(objThread.LastApprovedPost.ParentThread), "")
				End If

				' sloppy code, but quick fix
				RenderImage(wr, objConfig.GetThemeImageURL("row_spacer.gif"), "", "")

				RenderCellEnd(wr) ' </td>
				' cell for thread subject
				RenderCellBegin(wr, "", "", "100%", "left", "", "", "") ' <td>

				wr.AddAttribute(HtmlTextWriterAttribute.Href, url)

				If HasNewPosts(CurrentForumUser.UserID, objThread) Then
					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalBold")
					wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
				Else
					wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Normal")
					wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
				End If

				If objConfig.FilterSubject Then
					wr.Write(Utilities.ForumUtils.FormatProhibitedWord(objThread.Subject, objThread.CreatedDate, PortalID))
				Else
					wr.Write(objThread.Subject)
				End If
				wr.RenderEndTag() ' </A>

				wr.Write("<br />")

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
					RenderDivBegin(wr, "", "Forum_NormalSmall") ' <div>

					wr.Write(" (" & ForumControl.LocalizedText("Page") & ": ")

					If UserPagesCount >= CapPageCount Then
						For ThreadPage As Integer = 1 To CapPageCount - 1
							url = Utilities.Links.ContainerViewThreadPagedLink(TabID, objThread.ForumID, objThread.ThreadID, ThreadPage)
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
							url = Utilities.Links.ContainerViewThreadPagedLink(TabID, objThread.ForumID, objThread.ThreadID, UserPagesCount)
							wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
							wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalSmall")
							wr.AddAttribute(HtmlTextWriterAttribute.Rel, "nofollow")
							wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
							wr.Write(UserPagesCount.ToString())
							wr.RenderEndTag() ' </A>
						End If
					Else
						For ThreadPage As Integer = 1 To UserPagesCount
							url = Utilities.Links.ContainerViewThreadPagedLink(TabID, objThread.ForumID, objThread.ThreadID, ThreadPage)
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
					RenderDivEnd(wr) ' </div>
				End If

				RenderDivBegin(wr, "", "Forum_NormalSmall")
				url = Utilities.Links.ContainerViewForumLink(TabID, objThread.ForumID, False)
				wr.Write(ForumControl.LocalizedText("in") + "&nbsp;")
				RenderLinkButton(wr, url, objThread.ContainingForum.Name, "Forum_NormalSmall")
				RenderDivEnd(wr) ' </span>
				RenderCellEnd(wr) ' </td>

				' start third cell for ratings held within nested table
				' CP - Add check for RatingsEnabled
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
				If (objConfig.EnableThreadStatus And objThread.ContainingForum.EnableForumsThreadStatus) Or (objThread.ThreadStatus = ThreadStatus.Poll And objThread.ContainingForum.AllowPolls) Then
					' Display rating image
					RenderCellBegin(wr, "", "", "5%", "right", "", "", "")
					RenderImage(wr, objConfig.GetThemeImageURL(objThread.StatusImage), objThread.StatusText, "")
					RenderCellEnd(wr) ' </td>
				End If

				RenderRowEnd(wr) ' </Tr>
				RenderTableEnd(wr) ' </table>
				RenderCellEnd(wr) ' </td>

				' Replies
				If even Then
					RenderCellBegin(wr, "Forum_RowHighlight1", "", "11%", "center", "", "", "") ' <td>
				Else
					RenderCellBegin(wr, "Forum_RowHighlight1_Alt", "", "11%", "center", "", "", "")	' <td>
				End If

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Normal")
				wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>
				wr.Write(objThread.Replies)
				RenderDivEnd(wr) ' </span>
				RenderCellEnd(wr) ' </td>

				' Views
				If even Then
					RenderCellBegin(wr, "Forum_RowHighlight2", "", "11%", "center", "", "", "") ' <td>
				Else
					RenderCellBegin(wr, "Forum_RowHighlight2_Alt", "", "11%", "center", "", "", "")	' <td>
				End If

				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Normal")
				wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>
				wr.Write(objThread.Views)
				RenderDivEnd(wr) ' </span>
				RenderCellEnd(wr) ' </td>

				' Post date info & author
				If even Then
					RenderCellBegin(wr, "Forum_RowHighlight3", "", "26%", "right", "", "2", "") ' <td>
				Else
					RenderCellBegin(wr, "Forum_RowHighlight3_Alt", "", "26%", "right", "", "2", "")	' <td>
				End If

				' table holds last post data
				RenderTableBegin(wr, "", "", "100%", "100%", "0", "0", "right", "", "0") ' <table>
				RenderRowBegin(wr) ' <tr>

				RenderCellBegin(wr, "", "", "1px", "", "", "", "") ' <td>
				RenderImage(wr, objConfig.GetThemeImageURL("row_spacer.gif"), "", "")
				RenderCellEnd(wr) ' </td>

				RenderCellBegin(wr, "", "", "", "right", "", "", "") ' <td>
				url = Utilities.Links.ContainerViewPostLink(TabID, objThread.ForumID, objThread.LastApprovedPost.PostID)
				'' Skeel - This is for showing link to first unread post for logged in users. 
				'If LoggedOnUserID > 0 Then
				'	If HasNewPosts(LoggedOnUserID, SearchItem.LastApprovedP) Then
				'		If LoggedOnUser.DefaultPostsPerPage < SearchItem.TotalPost Then
				'			'Find the page on which the first unread post is located
				'			wr.AddAttribute(HtmlTextWriterAttribute.Href, FirstUnreadLink(thread))
				'		Else
				'			'Thread has only one page
				'			wr.AddAttribute(HtmlTextWriterAttribute.Href, url + "#unread")
				'		End If
				'		wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
				'		RenderImage(wr, objConfig.GetImageURL("thread_newest.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgThreadNewest"), "")
				'		wr.RenderEndTag() ' </a>
				'	End If
				'End If
				'' End Skeel

				RenderTitleLinkButton(wr, url, Utilities.ForumUtils.GetCreatedDateInfo(objThread.LastApprovedPost.CreatedDate, objConfig, ""), "Forum_LastPostText", objThread.LastPostShortBody)	  ' <a/>

				RenderDivBegin(wr, "", "Forum_LastPostText")	   ' <div>
				wr.Write(ForumControl.LocalizedText("by") & " ")

				If Not objConfig.EnableExternalProfile Then
					url = objThread.LastApprovedUser.UserCoreProfileLink
				Else
					url = Utilities.Links.UserExternalProfileLink(objThread.LastApprovedUser.UserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objThread.LastApprovedUser.Username)
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
			Next
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' Builds the left cell for RenderPost (author, rank, avatar area)
		''' </summary>
		''' <param name="wr"></param>
		''' <param name="objPost"></param>
		''' <param name="PostCountIsEven"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderAuthor(ByVal wr As HtmlTextWriter, ByVal objPost As PostInfo, ByVal PostCountIsEven As Boolean)
			' table to display integrated media, user alias, poster rank, avatar, homepage, and number of posts.
			RenderTableBegin(wr, "", "Forum_PostAuthorTable", "", "100%", "0", "0", "", "top", "")

			' row to display user alias and online status
			RenderRowBegin(wr) '<tr> 

			' display user online status
			If objConfig.EnableUsersOnline Then
				RenderCellBegin(wr, "", "", "5%", "", "middle", "", "")  ' <td> 
				If objPost.Author.IsOnline Then
					Dim imgURL As String = objConfig.GetThemeImageURL("s_online.") & objConfig.ImageExtension
					RenderImage(wr, imgURL, ForumControl.LocalizedText("imgOnline"), "")
				Else
					Dim imgURL As String = objConfig.GetThemeImageURL("s_offline.") & objConfig.ImageExtension
					RenderImage(wr, imgURL, ForumControl.LocalizedText("imgOffline"), "")
				End If
				RenderCellEnd(wr) ' </td>

				RenderCellBegin(wr, "", "", "95%", "", "middle", "", "")   ' <td>
				wr.Write("&nbsp;")
			Else
				RenderCellBegin(wr, "", "", "", "", "middle", "2", "")	  ' <td> 
			End If

			'link to user profile, always display in both view
			Dim url As String
			If Not objConfig.EnableExternalProfile Then
				url = objPost.Author.UserCoreProfileLink
			Else
				url = Utilities.Links.UserExternalProfileLink(objPost.Author.UserID, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objPost.Author.Username)
			End If

			RenderTitleLinkButton(wr, url, objPost.Author.SiteAlias, "Forum_Profile", ForumControl.LocalizedText("ViewProfile"))

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr> (end user alias/online)  

			' display user ranking 
			If (objConfig.Ranking) Then
				Dim authorRank As PosterRank = Utilities.ForumUtils.GetRank(objPost.Author, ForumControl.objConfig)
				Dim rankImage As String = String.Format("Rank_{0}." & objConfig.ImageExtension, CType(authorRank, Integer).ToString)
				Dim rankURL As String = objConfig.GetThemeImageURL(rankImage)
				Dim RankTitle As String = Utilities.ForumUtils.GetRankTitle(authorRank, objConfig)

				RenderRowBegin(wr) ' <tr> (start ranking row)
				RenderCellBegin(wr, "", "", "", "", "top", "2", "") ' <td>
				If objConfig.EnableRankingImage Then
					RenderImage(wr, rankURL, RankTitle, "")
				Else
					RenderDivBegin(wr, "", "Forum_NormalSmall")
					wr.Write(RankTitle)
					RenderDivEnd(wr)
				End If
				RenderCellEnd(wr) ' </td>
				RenderRowEnd(wr) ' </tr>
			End If

			'Author information
			RenderRowBegin(wr) ' <tr> 
			RenderCellBegin(wr, "Forum_NormalSmall", "", "", "", "top", "2", "")	 ' <td>

			'Post count
			RenderDivBegin(wr, "", "Forum_NormalSmall")
			wr.Write(ForumControl.LocalizedText("PostCount").Replace("[PostCount]", objPost.Author.PostCount.ToString))
			RenderDivEnd(wr)

			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>

			RenderTableEnd(wr) ' </table>  (End of user avatar/alias table, close td next)
		End Sub

		''' <summary>
		''' This allows for spacing between posts
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderSpacerRow(ByVal wr As HtmlTextWriter)
			RenderRowBegin(wr) '<tr> 
			RenderCellBegin(wr, "Forum_SpacerRow", "", "", "", "", "", "")  ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "") ' <img />
			RenderCellEnd(wr) ' </td>

			RenderCellBegin(wr, "Forum_SpacerRow", "", "", "", "", "", "")  ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("height_spacer.gif"), "", "") ' <img />
			RenderCellEnd(wr) '</td>
			RenderRowEnd(wr) ' </tr>
		End Sub

		'''
		'''  <summary>
		''' Renders the paging shown in the footer (based on threads/page)
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderSearchPaging(ByVal wr As HtmlTextWriter) ' Start the new column
			Dim ctlPagingControl As New DotNetNuke.Modules.Forum.WebControls.PagingControl
			ctlPagingControl.CssClass = "Forum_FooterText"
			ctlPagingControl.TotalRecords = TotalRecords

			If Aggregated Or myThreads Or LatestHours Then
				ctlPagingControl.PageSize = CurrentForumUser.ThreadsPerPage
			Else
				ctlPagingControl.PageSize = CurrentForumUser.PostsPerPage
			End If


			ctlPagingControl.CurrentPage = CurrentPage + 1

			Dim Params As String
			Params = GetQuerystring()

			ctlPagingControl.QuerystringParams = Params
			ctlPagingControl.TabID = TabID
			ctlPagingControl.RenderControl(wr)
		End Sub

		''' <summary>
		''' Footer w/ paging 
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks>
		''' </remarks>
		Private Sub RenderFooter(ByVal wr As HtmlTextWriter)
			'[skeel] If no results, then don't display the footer
			If NoResults = True Then Exit Sub

			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")		 ' <td><img/></td>

			RenderCellBegin(wr, "", "", "100%", "", "middle", "", "") ' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "middle", "0")	    ' <table>
			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapLeft", "") ' <td><img/></td>
			RenderCellBegin(wr, "Forum_Footer", "", "", "", "", "", "")	' <td>
			RenderTableBegin(wr, "", "", "", "100%", "0", "0", "", "", "0")	' <table>
			RenderRowBegin(wr) ' <tr>

			RenderCellBegin(wr, "", "", "", "", "top", "", "") ' <td>
			RenderImage(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "", "")	' <img/>
			RenderCellEnd(wr) ' </td>

			' xml link (for single forum syndication)
			If (objConfig.EnableRSS) AndAlso (Aggregated = True) Then
				RenderCellBegin(wr, "", "", "", "left", "middle", "", "") ' <td>

				wr.AddAttribute(HtmlTextWriterAttribute.Href, objConfig.SourceDirectory & "/Forum_Rss.aspx?forumid=" & -1 & "&tabid=" & TabID & "&mid=" & ModuleID)
				wr.AddAttribute(HtmlTextWriterAttribute.Target, "_blank")
				wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>

				RenderImage(wr, objConfig.GetThemeImageURL("s_rss.") & objConfig.ImageExtension, ForumControl.LocalizedText("imgRSS"), "")
				wr.RenderEndTag() ' </A>
				RenderCellEnd(wr) ' </td>
			End If

			RenderCellBegin(wr, "", "", "100%", "right", "middle", "", "") ' <td>
			RenderSearchPaging(wr)
			RenderCellEnd(wr) ' </td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("headfoot_height.gif"), "Forum_FooterCapRight", "")		 ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
			RenderTableEnd(wr) ' </table>
			RenderCellEnd(wr) ' </td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")		 ' <td><img/></td>
			RenderRowEnd(wr) ' </tr>
		End Sub

		''' <summary>
		''' Contains breadcrumb below threads footer
		''' </summary>
		''' <param name="wr"></param>
		''' <remarks></remarks>
		Private Sub RenderBottomBreadCrumb(ByVal wr As HtmlTextWriter)
			'[skeel] If no results, then don't display the footer
			If NoResults = True Then Exit Sub

			RenderRowBegin(wr) ' <tr>
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderCellBegin(wr, "", "", "100%", "left", "", "", "") ' <td>

			Dim ChildGroupView As Boolean = False
			If CType(ForumControl.TabModuleSettings("groupid"), String) <> String.Empty Then
				ChildGroupView = True
			End If

			If Aggregated Then
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.ThreadSearch, ThreadCollection, objConfig, ChildGroupView))
			Else
				wr.Write(Utilities.ForumUtils.BreadCrumbs(TabID, ModuleID, ForumScope.ThreadSearch, Nothing, objConfig, ChildGroupView))
			End If

			RenderCellEnd(wr) ' </td>

			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")
			RenderRowEnd(wr) ' </Tr>
		End Sub

		''' <summary>
		''' Obtains search parameters from the querystring.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetQuerystring() As String
			Dim str As New System.Text.StringBuilder
			Dim h As HttpRequest = HttpContext.Current.Request
			If h.QueryString("scope") <> String.Empty Then
				str.Append("&scope=" & h.QueryString("scope"))
			End If
			If h.QueryString("fromdate") <> String.Empty Then
				str.Append("&fromdate=" & h.QueryString("fromdate"))
			End If
			If h.QueryString("todate") <> String.Empty Then
				str.Append("&todate=" & h.QueryString("todate"))
			End If
			If h.QueryString("pagesize") <> String.Empty Then
				str.Append("&pagesize=" & h.QueryString("pagesize"))
			End If
			If h.QueryString("forums") <> String.Empty Then
				str.Append("&forums=" & h.QueryString("forums"))
			End If
			If h.QueryString("authors") <> String.Empty Then
				str.Append("&authors=" & h.QueryString("authors"))
			End If
			If h.QueryString("subject") <> String.Empty Then
				str.Append("&subject=" & h.QueryString("subject"))
			End If
			If h.QueryString("body") <> String.Empty Then
				str.Append("&body=" & h.QueryString("body"))
			End If
			If h.QueryString("threadstatusid") <> String.Empty Then
				str.Append("&threadstatusid=" & h.QueryString("threadstatusid"))
			End If
			If h.QueryString("noreply") <> String.Empty Then
				str.Append("&noreply=1")
			End If
			If h.QueryString("aggregated") <> String.Empty Then
				str.Append("&aggregated=1")
			End If
			If h.QueryString("mythreads") <> String.Empty Then
				str.Append("&mythreads=1")
			End If
			If h.QueryString("latesthours") <> String.Empty Then
				str.Append("&latesthours=" & h.QueryString("latesthours"))
			End If

			Return str.ToString
		End Function

		''' <summary>
		''' If the thread has new posts since the user's last visit date.
		''' </summary>
		''' <param name="PostThreadID"></param>
		''' <param name="PostCreatedDate"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function NewPost(ByVal PostThreadID As Integer, ByVal PostCreatedDate As Date) As Boolean
			Dim userThreadController As New UserThreadsController
			Dim userThread As New UserThreadsInfo
			If CurrentForumUser.UserID > 0 Then
				userThread = userThreadController.GetThreadReadsByUser(CurrentForumUser.UserID, PostThreadID)
				If userThread Is Nothing Then
					Return True
				Else
					'consider it a new post if made within the last minute of the most recent visit
					If userThread.LastVisitDate < PostCreatedDate Then
						Return True
					Else
						Return False
					End If
				End If
			Else
				Return True
			End If
		End Function

		''' <summary>
		''' Determins if post is even or odd numbered row
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
		''' Formats the body text to display as search result
		''' </summary>
		''' <param name="strBody"></param>
		''' <param name="CreatedDate"></param>
		''' <returns>A string with markup of search words, max 300 caracters</returns>
		''' <remarks></remarks>
		Private Function ProcessSearchBody(ByVal strBody As String, ByVal CreatedDate As Date) As String
			Dim str As String = HttpUtility.HtmlDecode(strBody)
			Try

				If strBody IsNot Nothing Then
					' Here we handle any instances of Quotes and Code tags
					Dim mRegOptions As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.Singleline
					Dim mQuoteAuthor As New Regex("\[quote=""(?<author>[^\]]*)\""](?<text>(.*?))\[/quote\]", mRegOptions)
					Dim mQuote As New Regex("\[quote\](?<text>(.*?))\[/quote\]", mRegOptions)
					Dim mCode As New Regex("\[code\](?<code>(.*?))\[/code\]", mRegOptions)
					Dim mImage As New Regex("<img[^>]*>", mRegOptions)

					Dim strTmpName As String = String.Format("{0} " & Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim() & ": ""{1}""</div>", "${author}", "${text}")
					While mQuoteAuthor.IsMatch(str)
						str = mQuoteAuthor.Replace(str, strTmpName)
					End While

					Dim strTmp As String = String.Format("{0}: ""{1}""", Localization.GetString("ForumTextQuote.Text", objConfig.SharedResourceFile).Trim(), "${text}")
					While mQuote.IsMatch(str)
						str = mQuote.Replace(str, strTmp)
					End While

					Dim strTmpCode As String = String.Format("{0}: ""{1}""", Localization.GetString("ForumTextCode.Text", objConfig.SharedResourceFile).Trim(), "${code}")
					While mCode.IsMatch(str)
						str = mCode.Replace(str, strTmpCode)
					End While

					' Any inline attachments?
					If objConfig.EnableAttachment = True And str.ToLower.IndexOf("[attachment") >= 0 Then
						Dim mAttach As New Regex("\[attachment\](?<file>(.*?))\[/attachment\]", mRegOptions)
						While mAttach.IsMatch(str)

							For Each mMatch As Match In mAttach.Matches(str)
								Dim strExtension As String = Replace(IO.Path.GetExtension(mMatch.Groups(1).ToString), ".", "")
								If InStr("," & Common.glbImageFileTypes.ToLower, "," & strExtension.ToLower) > 0 Then
									'Image -> tag it!
									str = str.Replace(mMatch.Groups(0).ToString, Localization.GetString("ImageTag", objConfig.SharedResourceFile))
								Else
									'File -> display the filename with quotes
									str = str.Replace(mMatch.Groups(0).ToString, """" & mMatch.Groups(1).ToString & """")
								End If
							Next
						End While
					End If

					' Replace all instances of <img> with the image tag
					While mImage.IsMatch(str)
						str = mImage.Replace(str, Localization.GetString("ImageTag", objConfig.SharedResourceFile))
					End While

					' Then we need to decode the text and then strip all the remaining html
					str = Utilities.ForumUtils.StripHTML(str)

					'Now let's handle old type quotes
					If str.IndexOf(Configuration.QUOTE_OPEN) > 0 Then
						str = str.Replace(Configuration.QUOTE_OPEN, " "" ")
						str = str.Replace(Configuration.QUOTE_CLOSE, " "" ")
					End If

					'..and naughty words...
					If objConfig.EnableBadWordFilter Then
						str = Utilities.ForumUtils.FormatProhibitedWord(str, CreatedDate, PortalID)
					End If

					'Let's find out if the search contained the body
					If Not HttpContext.Current.Request.QueryString("body") Is Nothing Then

						Dim SearchArray() As String = SplitSearchString(CStr(HttpContext.Current.Request.QueryString("body")))
						Dim strWord As String
						Dim Count As Integer = 0
						Dim FoundWord As Boolean = False

						If Len(str) > 300 Then

							'So what part of the body do we display?
							For Each strWord In SearchArray

								If FoundWord = False Then
									strWord = strWord.Trim()

									Dim pos As Integer = str.IndexOf(strWord, StringComparison.OrdinalIgnoreCase)
									If pos >= 0 Then
										'Start?
										If pos < 150 Then
											str = Left(str, 297) & "..."
										Else
											'What's the length?
											Dim posEnd As Integer = Len(strBody)
											'middle?
											If pos + 147 < posEnd Then
												str = "..." & str.Substring((pos - 147), 297) & "..."
											Else
												'Right
												str = "..." & str.Substring((posEnd - 297), 297)
											End If

										End If
										'Ok - We found at least one word
										FoundWord = True
									End If
								End If

							Next

						End If

						'Markup the words and count instances
						For Each strWord In SearchArray

							strWord = strWord.Trim()
							'Count the instances of this specific word in the reduced body
							Dim iWord As Long = UBound(Split(str.ToLower, strWord.ToLower))

							If iWord > 0 Then

								Dim i As Integer = 0
								Dim LastPos As Integer = 0

								Do While i < iWord

									Dim pos As Integer = str.IndexOf(strWord, LastPos, StringComparison.OrdinalIgnoreCase)

									If pos >= 0 Then
										'This is nessary to keep case sensitivity
										Dim origStr As String = str.Substring(pos, strWord.Length)
										str = str.Remove(pos, strWord.Length)
										str = str.Insert(pos, "<span class=""Forum_Markup"">" & origStr & "</span>")
										LastPos = pos + strWord.Length + 27 'length of span string 
									End If
									i = i + 1
								Loop
							End If

							'Add to the hit counter, based on the full body 
							Dim iCount As Long = UBound(Split(strBody.ToLower, strWord.ToLower))
							Count = Count + CInt(iCount)

						Next

						'Add the hit count
						str = "<b>[" & Localization.GetString("Hits", objConfig.SharedResourceFile) & ": " & CStr(Count) & "]</b> " & str
					Else
						'Search was on author, subject or date range only
						If Len(str) > 300 Then
							str = Left(str, 297) & "..."
						End If
					End If
				End If

				Return str
			Catch ex As Exception
				LogException(ex)
				If Len(str) > 300 Then
					str = Left(str, 297) & "..."
				End If
				Return str
			End Try
		End Function

		''' <summary>
		''' Splits up a string separated by + caracter
		''' </summary>
		''' <param name="str"></param>
		''' <returns>An array of strings</returns>
		''' <remarks>Added by Skeel</remarks>
		Private Function SplitSearchString(ByVal str As String) As String()
			Dim splitter As Char() = {","c}
			Dim SearchArray() As String = str.Split(splitter)
			Return SearchArray
		End Function

		''' <summary>
		''' If the thread being displayed has new posts since the users last visit to that thread.
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="objThreadInfo"></param>
		''' <returns>Boolean value</returns>
		''' <remarks></remarks>
		Private Function HasNewPosts(ByVal UserID As Integer, ByVal objThreadInfo As ThreadInfo) As Boolean
			Dim userThreadController As New UserThreadsController
			Dim userThread As New UserThreadsInfo

			If UserID > 0 Then
				userThread = userThreadController.GetThreadReadsByUser(UserID, objThreadInfo.ThreadID)

				If userThread Is Nothing Then
					Return True
				Else
					If userThread.LastVisitDate < objThreadInfo.LastApprovedPost.CreatedDate Then
						Return True
					Else
						Return False
					End If
				End If
			Else
				Return True
			End If
		End Function

#End Region

	End Class

End Namespace