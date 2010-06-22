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
	''' Gathers search criteria from the end user then uses this to navigate
	''' to the search results page.
	''' </summary>
	''' <remarks> 2/11/2006 - CP - Added In ClientAPI
	''' </remarks>
	Public Class SearchPage
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Optional Interfaces"

		''' <summary>
		''' Gets a list of module actions available to the user to provide it to DNN core.
		''' </summary>
		''' <value></value>
		''' <returns>The collection of module actions available to the user</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
			Get
				Return Utilities.ForumUtils.PerUserModuleActions(objConfig, Me)
			End Get
		End Property

#End Region

#Region "Properties"

		Public Property SelectedForumId() As Integer
			Get
				Return CType(ViewState("SelectedForumId"), Integer)
			End Get
			Set(ByVal Value As Integer)
				ViewState("SelectedForumId") = Value
			End Set
		End Property

		Public Property SelectedForumIds() As String
			Get
				Return CType(ViewState("SelectedForumIds"), String)
			End Get
			Set(ByVal Value As String)
				ViewState("SelectedForumIds") = Value
			End Set
		End Property

		Public Property SelectedGroupId() As Integer
			Get
				Return CType(ViewState("SelectedGroupId"), Integer)
			End Get
			Set(ByVal Value As Integer)
				ViewState("SelectedGroupId") = Value
			End Set
		End Property

#End Region

#Region "Event Handlers"

		''' <summary>
		''' POD For Client API Text Suggest
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub txtForumUserSuggest_PopulateOnDemand(ByVal source As Object, ByVal e As UI.WebControls.DNNTextSuggestEventArgs) Handles txtForumUserSuggest.PopulateOnDemand
			' Here is where we would count e.Text (if we had minimum character requirements - needed for huge sites)
			PopulateList(e.Nodes, e.Text)
		End Sub

		''' <summary>
		''' Page Load/Re-Load handler
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				If Not Page.IsPostBack Then
					litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

					''[skeel]
					'If ForumConfig.AggregatedForums = False Then
					'    'SearchPosts
					'    lblBodyHelp.Visible = True
					'    lblBodyHelp.Text = "<br /><em>" & Localization.GetString("SearchPosts", LocalResourceFile) & "</em>"
					'End If

					' Store the referrer for returning to where the user came from
					If Not Request.UrlReferrer Is Nothing Then
						ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
					End If

					With Me.rdpFrom
						.ToolTip = Localization.GetString("cmdCalFrom", LocalResourceFile)
					End With

					With Me.rdpTo
						.ToolTip = Localization.GetString("cmdCalFrom", LocalResourceFile)
					End With

					rdpFrom.SelectedDate = DateAdd(DateInterval.Month, -1, Date.Today)
					rdpTo.SelectedDate = Date.Today

					With Me.rcbThreadStatus
						rcbThreadStatus.Items.Clear()

						rcbThreadStatus.Items.Insert(0, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("AnyStatus", LocalResourceFile), "-1"))
						rcbThreadStatus.Items.Insert(1, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Unanswered", objConfig.SharedResourceFile), "1"))
						rcbThreadStatus.Items.Insert(2, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Answered", objConfig.SharedResourceFile), "2"))
						rcbThreadStatus.Items.Insert(3, New Telerik.Web.UI.RadComboBoxItem(Localization.GetString("Informative", objConfig.SharedResourceFile), "3"))
					End With

					' Treeview forum viewer
					InitializeTextSuggest()
					'ForumTreeview.InitializeTree(objConfig, ForumTree)
					'ForumTreeview.SetTreeDefaults(objConfig, ForumTree, True)
					'ForumTreeview.PopulateTree(objConfig, ForumTree, UserId)
					ForumTreeview.PopulateTelerikTree(objConfig, rtvForums, UserId)
					' Register scripts (broke DNNTree if loaded)
					'Utils.RegisterPageScripts(Page, ForumConfig)
				End If

				SelectedForumIds = String.Empty
				For Each objNode As Telerik.Web.UI.RadTreeNode In Me.rtvForums.Nodes
					Dim strType As String = objNode.Value.Substring(0, 1)
					Dim iID As Integer = CInt(objNode.Value.Substring(1, objNode.Value.Length - 1))

					If strType = "F" Then
						SelectedForumId = iID

						Dim newForumId As String = String.Empty
						newForumId = SelectedForumId & ";" & SelectedForumIds
						SelectedForumIds = newForumId
					Else
						SelectedGroupId = iID
					End If
				Next

			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Builds a string to append the url we are navigation too with.  This will
		''' build search criteria the screen we are going to needs.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	7/13/2005	Created
		''' </history>
		Protected Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
			Try
				Dim sb As New System.Text.StringBuilder
				Dim startDate As DateTime
				Dim endDate As DateTime

				' Default calendar entry is 00:00 AM so add 1 more day to cover "today" search date
				If Len(rdpFrom.SelectedDate) > 0 AndAlso Len(rdpTo.SelectedDate) > 0 Then
					startDate = CDate(rdpFrom.SelectedDate)
					endDate = DateAdd(DateInterval.Day, 1, CDate(rdpTo.SelectedDate))
				Else
					startDate = DateAdd(DateInterval.Month, -1, Date.Today)
					endDate = Date.Today
				End If

				sb.Append("&scope=threadsearch")
				sb.Append("&fromdate=")
				sb.Append(Utilities.ForumUtils.DateToNum(startDate).ToString)
				sb.Append("&todate=")
				sb.Append(Utilities.ForumUtils.DateToNum(endDate).ToString)
				sb.Append("&pagesize=")
				sb.Append(CurrentForumUser.ThreadsPerPage.ToString)

				SelectedForumIds = String.Empty
				For Each objNode As Telerik.Web.UI.RadTreeNode In Me.rtvForums.CheckedNodes
					Dim strType As String = objNode.Value.Substring(0, 1)
					Dim iID As Integer = CInt(objNode.Value.Substring(1, objNode.Value.Length - 1))

					If strType = "F" Then
						SelectedForumId = iID

						Dim newForumId As String = String.Empty
						newForumId = SelectedForumId & ";" & SelectedForumIds
						SelectedForumIds = newForumId
					Else
						SelectedGroupId = iID
					End If
				Next

				If SelectedForumIds.Length > 0 Then
					SelectedForumIds = Left(SelectedForumIds, Len(SelectedForumIds) - 1)
					sb.Append("&forums=")
					sb.Append(Trim(SelectedForumIds))
				End If

				If txtForumUserSuggest.Text.Trim() <> String.Empty Then
					' we know sometihng is there, do a lookup

					If txtForumUserSuggest.SelectedNodes.Count > 0 Then
						Dim loopCount As Integer = 0

						For Each Node As DotNetNuke.UI.WebControls.DNNNode In Me.txtForumUserSuggest.SelectedNodes
							If Node.Key.Trim() <> String.Empty Then
								If loopCount < 1 Then
									sb.Append("&authors=")
								End If
								sb.Append(Node.Key)
								sb.Append(";")
								loopCount += 1
							End If
						Next
					End If
				End If

				If rcbThreadStatus.SelectedIndex > 0 Then
					sb.Append("&threadstatusid=")
					sb.Append(rcbThreadStatus.SelectedValue)
				End If

				If txtSubject.Text.Length > 0 Then
					sb.Append("&subject=")
					Dim subject As String = txtSubject.Text.Replace("..", ".")
					sb.Append(HttpUtility.UrlEncode(subject))
				End If

				If txtSearch.Text.Length > 0 Then
					sb.Append("&body=")
					Dim body As String = txtSearch.Text.Replace("..", ".")
					sb.Append(HttpUtility.UrlEncode(body))
				End If

				Dim strURL As String = sb.ToString
				Response.Redirect(NavigateURL(TabId, "", strURL), False)

			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Send the user back to where they came from.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	2/11/2006	Created
		''' </history>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			If Not ViewState("UrlReferrer") Is Nothing Then
				Response.Redirect(CType(ViewState("UrlReferrer"), String), False)
			Else
				Response.Redirect(Utilities.Links.ContainerForumHome(TabId), False)
			End If
		End Sub

#End Region

#Region "Private Methods"

#Region "DNNTextSuggest"

		''' <summary>
		''' Setup the DNNTextSuggest for use here
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Private Sub InitializeTextSuggest()
			txtForumUserSuggest.ForceDownLevel = False
			txtForumUserSuggest.Target = "MyTarget"
			' can't use min char lookup because a user could potentially have a single character username or display name. 
			'txtForumUserSuggest.MinCharacterLookup = 2
			txtForumUserSuggest.IDToken = UI.WebControls.DNNTextSuggest.eIDTokenChar.Brackets
			' comment out delimiter when not wanting multi-select
			txtForumUserSuggest.Delimiter = ";"c
			txtForumUserSuggest.TextSuggestCssClass = "Forum_Suggest"
			txtForumUserSuggest.DefaultChildNodeCssClass = "Forum_Suggest_DefaultChildNode"
			txtForumUserSuggest.DefaultNodeCssClass = "Forum_Suggest_DefaultNode"
			txtForumUserSuggest.DefaultNodeCssClassOver = "Forum_Suggest_DefaultNodeOver"
			txtForumUserSuggest.DefaultNodeCssClassSelected = "Forum_Suggest_DefaultNodeSelected"
		End Sub

		''' <summary>
		''' Used to populate the DNNTextSuggest w/ results as user types
		''' </summary>
		''' <param name="objNodes"></param>
		''' <param name="strText"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	8/6/2006	Created
		''' </history>
		Private Sub PopulateList(ByVal objNodes As UI.WebControls.DNNNodeCollection, ByVal strText As String)
			Dim o As UI.WebControls.DNNNode
			Dim arrUsers As ArrayList
			Dim objForumUser As ForumUser
			Dim objUserCnt As New ForumUserController

			arrUsers = objUserCnt.UserGetAll(PortalId, strText, 10, 0, 100, ModuleId)

			For Each objForumUser In arrUsers
				If Me.txtForumUserSuggest.MaxSuggestRows = 0 OrElse objNodes.Count < (Me.txtForumUserSuggest.MaxSuggestRows + 1) Then	 '+1 to let control know there is more "pending"
					o = New UI.WebControls.DNNNode(objForumUser.SiteAlias)
					o.ID = objForumUser.UserID.ToString
					o.Key = (objForumUser.UserID.ToString)
					o.CSSClass = "SpecialNode"
					objNodes.Add(o)
				End If
			Next
		End Sub

#End Region

#End Region

	End Class

End Namespace