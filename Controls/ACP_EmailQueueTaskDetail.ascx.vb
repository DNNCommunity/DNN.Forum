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

Imports Telerik.Web.UI

Namespace DotNetNuke.Modules.Forum.ACP

	''' <summary>
	''' This screen is used to manage the email configuration queue cleanup
	''' This can only be adjusted by SuperUsers
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class EmailQueueTaskDetail
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			rgTaskDetails.PageSize = PageSize
			PageIndex = 0
		End Sub

#End Region

#Region "Private Members"

		'Dim _SortExpression As String = "CurrentPoints"
		Dim _Ascending As Boolean = False
		Dim _PageIndex As Integer = 0

#End Region

#Region "Private Properties"

		''' <summary>
		''' The total number of users we will show in the grid at a given time, this is pulled from the settings.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property PageSize() As Integer
			Get
				'If CType(Settings("PageSize"), String) <> "" Then
				'	Return CInt(Settings("PageSize"))
				'Else
				'	Return 10
				'End If
				Return 20
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property PageIndex() As Integer
			Get
				If CStr(ViewState("PageIndex")) <> String.Empty Then
					_PageIndex = CInt(ViewState("PageIndex"))
				End If
				Return _PageIndex
			End Get
			Set(ByVal value As Integer)
				_PageIndex = value
				ViewState("PageIndex") = value
			End Set
		End Property

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
		''' Runs when a Command event is raised in the Grid 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub rgTaskDetails_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgTaskDetails.ItemCommand
			Try
				If (e.Item.ItemType <> Telerik.Web.UI.GridItemType.Item Or e.Item.ItemType <> Telerik.Web.UI.GridItemType.AlternatingItem) Then
					'Select Case e.CommandName
					'	Case "ViewItem"
					'		'Dim keyID As Integer = CInt(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex)("EmailQueueID"))
					'		'e.Item.Selected = True
					'		'BindImpact(keyID)
					'End Select
				End If
			Catch exc As System.Exception
				'Module failed to load
				DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Binds the appropriate image items to the datagrid based on the results stored in the db.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub rgTaskDetails_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgTaskDetails.ItemDataBound
			'If TypeOf e.Item Is Telerik.Web.UI.GridDataItem Then
			'	Dim dataItem As Telerik.Web.UI.GridDataItem = CType(e.Item, Telerik.Web.UI.GridDataItem)
			'	Dim imgBtn As ImageButton

			'	imgBtn = CType(dataItem("ViewItem").Controls(0), ImageButton)
			'	imgBtn.ToolTip = Localization.GetString("View", Me.LocalResourceFile)
			'End If
		End Sub

		''' <summary>
		''' Used to populate the grid with a datasource when one is not found (and is needed).  
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub rgTaskDetails_NeedsDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgTaskDetails.NeedDataSource
			If Not e.IsFromDetailTable Then
				BindGrid(False)
			End If
		End Sub

		Protected Sub rgTaskDetails_PageChange(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles rgTaskDetails.PageIndexChanged
			PageIndex = e.NewPageIndex

			BindGrid(False)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub rgTaskDetails_DetailTableDataBind(ByVal source As Object, ByVal e As GridDetailTableDataBindEventArgs) Handles rgTaskDetails.DetailTableDataBind
			Dim dataItem As GridDataItem = CType(e.DetailTableView.ParentItem, GridDataItem)
			Select Case e.DetailTableView.Name
				Case "TaskEmails"
					Dim EmailQueueID As Integer = CInt(dataItem.GetDataKeyValue("EmailQueueID"))
					Dim cntEmailTask As New EmailQueueTaskEmailsController
					Dim colEmails As List(Of EmailQueueTaskEmailsInfo)

					colEmails = cntEmailTask.TaskEmailsGet(EmailQueueID)
					e.DetailTableView.DataSource = colEmails
			End Select
		End Sub

		'''' <summary>
		'''' Updates the email settings in the database
		'''' </summary>
		'''' <param name="sender"></param>
		'''' <param name="e"></param>
		'''' <remarks>
		'''' </remarks>
		'Protected Sub cmdUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
		'	Try

		'	Catch exc As Exception
		'		ProcessModuleLoadException(Me, exc)
		'	End Try
		'End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="BindIt"></param>
		''' <remarks></remarks>
		Private Sub BindGrid(ByVal BindIt As Boolean)
			Dim cntEmailTask As New EmailQueueTaskController
			Dim colEmailTasks As List(Of EmailQueueTaskInfo)

			colEmailTasks = cntEmailTask.GetPortalEmailSendTasks(PortalId, PageIndex, PageSize)

			Dim cntEmailTask2 As New EmailQueueTaskController

			rgTaskDetails.VirtualItemCount = cntEmailTask2.GetPortalEmailTaskCount(PortalId)

			If Not colEmailTasks Is Nothing Then
				rgTaskDetails.DataSource = colEmailTasks
				If BindIt Then
					rgTaskDetails.DataBind()
				End If
			End If
		End Sub

		''' <summary>
		''' Used to localized the grid headers (a replacement for core method). 
		''' </summary>
		''' <remarks></remarks>
		Private Sub SetLocalization()
			For Each gc As Telerik.Web.UI.GridColumn In rgTaskDetails.MasterTableView.Columns
				If gc.HeaderText <> "" Then
					gc.HeaderText = Localization.GetString(gc.HeaderText + ".Header", Me.LocalResourceFile)
				End If
			Next
		End Sub

#End Region

	End Class

End Namespace