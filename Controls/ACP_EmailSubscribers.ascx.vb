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

Imports Telerik.Web.UI

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This screen is used to manage the email configuration queue cleanup
	''' This can only be adjusted by SuperUsers
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class EmailSubscribers
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			' need a check here for querystring for forum or thread id (we need this here too because we are doing it more than on initial page load). 
			BindForums()

			Dim ForumID As Integer = -1
			Dim ThreadID As Integer = -1

			If Request.QueryString("ForumID") IsNot Nothing Then
				ForumID = CInt(Request.QueryString("ForumID"))
				rcbForums.SelectedValue = ForumID.ToString()
			End If

			If Request.QueryString("ThreadID") IsNot Nothing Then
				ThreadID = CInt(Request.QueryString("ThreadID"))
				txtThreadID.Text = ThreadID.ToString()
			End If

			If ThreadID > 0 Then
				rgForums.Visible = True
				rgThreads.Visible = True
				BindThreadGrid(False)

				If ForumID > 0 Then
					rcbForums.SelectedValue = ForumID.ToString()
				End If
			Else
				txtThreadID.Text = "-1"
				rgForums.Visible = True
				rgThreads.Visible = False
				BindForumGrid(False)
			End If
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Used to populate the grid with a datasource when one is not found (and is needed).  
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub rgForums_NeedsDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgForums.NeedDataSource
			'If Not e.IsFromDetailTable Then
			BindForumGrid(False)
			'End If
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub rcbForums_SelectedIndexChange(ByVal source As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbForums.SelectedIndexChanged
			txtThreadID.Text = "-1"
			rgForums.Visible = True
			rgThreads.Visible = False
			BindForumGrid(True)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindForums()
			Dim cntForum As New ForumController
			Dim arrForums As List(Of ForumInfo)

			arrForums = cntForum.GetModuleForums(ModuleId)

			If arrForums IsNot Nothing Then
				rcbForums.DataSource = arrForums
				rcbForums.DataBind()
			End If
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="BindIt"></param>
		''' <remarks></remarks>
		Private Sub BindForumGrid(ByVal BindIt As Boolean)
			' need a check here for querystring for forum or thread id (we need this here too because we are doing it more than on initial page load). 
			Dim ForumID As Integer = -1
			Dim ThreadID As Integer = -1

			If rcbForums.SelectedValue <> "" Then
				ForumID = CInt(rcbForums.SelectedValue)
			End If

			Dim cntUserTracking As New UserTrackingController
			Dim colUserTracking As List(Of UserTrackingInfo)

			colUserTracking = cntUserTracking.GetForumSubscribers(ForumID)

			If Not colUserTracking Is Nothing Then
				rgForums.DataSource = colUserTracking
				If BindIt Then
					rgForums.DataBind()
				End If
			End If

			SetLocalization()
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="BindIt"></param>
		''' <remarks></remarks>
		Private Sub BindThreadGrid(ByVal BindIt As Boolean)
			' need a check here for querystring for forum or thread id (we need this here too because we are doing it more than on initial page load). 
			Dim ThreadID As Integer = -1

			Dim cntUserTracking As New UserTrackingController
			Dim colUserTracking As List(Of UserTrackingInfo)

			colUserTracking = cntUserTracking.GetThreadSubscribers(ThreadID)

			If Not colUserTracking Is Nothing Then
				rgThreads.DataSource = colUserTracking
				If BindIt Then
					rgThreads.DataBind()
				End If
			End If

			SetThreadLocalization()
		End Sub

		''' <summary>
		''' Used to localized the grid headers (a replacement for core method). 
		''' </summary>
		''' <remarks></remarks>
		Private Sub SetLocalization()
			For Each gc As Telerik.Web.UI.GridColumn In rgForums.MasterTableView.Columns
				If gc.HeaderText <> "" Then
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", Me.LocalResourceFile)
				End If
			Next
		End Sub

		Private Sub SetThreadLocalization()
			For Each gc As Telerik.Web.UI.GridColumn In rgThreads.MasterTableView.Columns
				If gc.HeaderText <> "" Then
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", Me.LocalResourceFile)
				End If
			Next
		End Sub

#End Region

	End Class

End Namespace