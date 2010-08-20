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

#Region "Private Properties"

		''' <summary>
		''' If this was linked to from the posts view, this variable is populated to also display a list of subscribers at the thread level. 
		''' </summary>
		''' <value></value>
		''' <returns>A the threadid we are attempting subscribers of.</returns>
		''' <remarks></remarks>
		Private ReadOnly Property ThreadID As Integer
			Get
				Dim _threadID As Integer = -1
				If Request.QueryString("ThreadID") IsNot Nothing Then
					_threadID = CInt(Request.QueryString("ThreadID"))
				End If
				Return _threadID
			End Get
		End Property
		
#End Region

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			BindForums()

			rcbForums.SelectedValue = ForumID.ToString()

			If ThreadID > 0 Then
				rgForums.Visible = True
				rgThreads.Visible = True
				BindThreadGrid(True)
				If ForumID > 0 Then
					rcbForums.SelectedValue = ForumID.ToString()
				End If
			Else
				rgForums.Visible = True
				rgThreads.Visible = False
			End If

			BindForumGrid(True)
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Runs when the control is initialized, even before anything in LoadInitialView runs. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>All controls containing grids should localize the grid headers here. </remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			SetLocalization()
		End Sub

		''' <summary>
		''' Fires when a user changes the selected forum combobox. 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub rcbForums_SelectedIndexChange(ByVal source As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbForums.SelectedIndexChanged
			rgForums.Visible = True
			rgThreads.Visible = False
			BindForumGrid(True)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds a list of forums to the combobox. 
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
		''' Used to bind data to the forum subscriptions grid. 
		''' </summary>
		''' <param name="BindIt"></param>
		''' <remarks></remarks>
		Private Sub BindForumGrid(ByVal BindIt As Boolean)
			Dim cntUserTracking As New UserTrackingController
			Dim colUserTracking As List(Of UserTrackingInfo)

			colUserTracking = cntUserTracking.GetForumSubscribers(ForumID)

			If Not colUserTracking Is Nothing Then
				rgForums.DataSource = colUserTracking
				If BindIt Then
					rgForums.DataBind()
				End If
			End If
		End Sub

		''' <summary>
		''' Used to bind data to the thread subscriptions grid. 
		''' </summary>
		''' <param name="BindIt"></param>
		''' <remarks></remarks>
		Private Sub BindThreadGrid(ByVal BindIt As Boolean)
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
		''' Used to localize the grid headers for the forums grid (a replacement for core method). 
		''' </summary>
		''' <remarks></remarks>
		Private Sub SetLocalization()
			For Each gc As Telerik.Web.UI.GridColumn In rgForums.MasterTableView.Columns
				If gc.HeaderText <> "" Then
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", Me.LocalResourceFile)
				End If
			Next
		End Sub

		''' <summary>
		''' Used to localize the grid headers for the threads grid (a replacement for core method). 
		''' </summary>
		''' <remarks></remarks>
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