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
	''' Management screen which includes a grid list of current words to filter for
	''' and the ability to enable/disable this throughout the instance of the module.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Public Class FilterWord
		Inherits ForumModuleBase
		Implements Utilities.AjaxLoader.IPageLoad

#Region "Private Members"

		Private _PageSize As Integer

#End Region

#Region "Interfaces"

		''' <summary>
		''' This is required to replace If Page.IsPostBack = False because controls are dynamically loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Protected Sub LoadInitialView() Implements Utilities.AjaxLoader.IPageLoad.LoadInitialView
			Localization.LocalizeDataGrid(grdBadWords, Me.LocalResourceFile)
			' change ME implementing paging.
			'BottomPager.PageSize = Convert.ToInt32(LoggedOnUser.ThreadsPerPage)
			BottomPager.PageSize = 10000000

			imgAdd.ImageUrl = objConfig.GetThemeImageURL("s_add.") & objConfig.ImageExtension
			imgAdd.ToolTip = Localization.GetString("imgAdd", LocalResourceFile)
			CreateLetterSearch()
			BindData(ddlSearchType.SelectedValue, BottomPager.PageSize)
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' Formats items displayed in the grid as data is bound to the grid. 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub grdBadWords_ItemDataBound(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles grdBadWords.ItemDataBound
			Dim item As DataGridItem = e.Item

			If item.ItemType = ListItemType.Item Or item.ItemType = ListItemType.AlternatingItem Or item.ItemType = ListItemType.EditItem Then
				Dim imgColumnControl As System.Web.UI.Control


				imgColumnControl = item.Controls(0).FindControl("cmdEdit")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgBadWord As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgBadWord.ImageUrl = objConfig.GetThemeImageURL("s_edit.") & objConfig.ImageExtension
				End If

				imgColumnControl = item.Controls(0).FindControl("cmdDelete")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgBadWord As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgBadWord.ImageUrl = objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
					imgBadWord.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem") & "');")
				End If

				imgColumnControl = item.Controls(0).FindControl("cmdCancel")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgBadWord As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgBadWord.ImageUrl = objConfig.GetThemeImageURL("s_cancel.") & objConfig.ImageExtension
				End If

				imgColumnControl = item.Controls(0).FindControl("cmdUpdate")
				If TypeOf imgColumnControl Is System.Web.UI.WebControls.ImageButton Then
					Dim imgBadWord As ImageButton = CType(imgColumnControl, System.Web.UI.WebControls.ImageButton)

					imgBadWord.ImageUrl = objConfig.GetThemeImageURL("s_update.") & objConfig.ImageExtension
				End If
			End If
		End Sub

		Protected Sub grdBadWords_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdBadWords.EditCommand
			grdBadWords.EditItemIndex = e.Item.ItemIndex
			grdBadWords.SelectedIndex = -1
			imgAdd.Enabled = False
			BindData(ddlSearchType.SelectedValue, BottomPager.PageSize)
		End Sub

		''' <summary>
		''' Deletes the selected word from the data store.
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub grdBadWords_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdBadWords.DeleteCommand
			Dim cntWordFilter As New WordFilterController
			cntWordFilter.FilterWordDelete(CInt(grdBadWords.DataKeys(e.Item.ItemIndex)))
			grdBadWords.EditItemIndex = Null.NullInteger

			BindData(ddlSearchType.SelectedValue, BottomPager.PageSize)
		End Sub

		''' <summary>
		''' Runs when a word is updated from within the grid. 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub grdBadWords_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdBadWords.UpdateCommand
			Dim txtBadWord As TextBox = CType(e.Item.Cells(0).Controls(0), TextBox)
			Dim txtReplacedWord As TextBox = CType(e.Item.Cells(1).Controls(0), TextBox)
			Dim cntWordFilter As New WordFilterController
			cntWordFilter.FilterWordUpdate(PortalId, txtBadWord.Text, txtReplacedWord.Text, CurrentForumUser.UserID)

			grdBadWords.EditItemIndex = Null.NullInteger
			imgAdd.Enabled = True
			BindData(ddlSearchType.SelectedValue, BottomPager.PageSize)

		End Sub

		''' <summary>
		''' Runs when an update (edit action) is canceled within the grid.
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub grdBadWords_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles grdBadWords.CancelCommand
			grdBadWords.EditItemIndex = Null.NullInteger
			imgAdd.Enabled = True
			BindData(ddlSearchType.SelectedValue, BottomPager.PageSize)
		End Sub

		''' <summary>
		''' Adds a word and it's replacement to the grid. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub imgAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgAdd.Click
			Dim ctlFilterWords As New WordFilterController
			ctlFilterWords.FilterWordUpdate(PortalId, txtNewBadWord.Text, txtNewReplaceWord.Text, CurrentForumUser.UserID)

			grdBadWords.EditItemIndex = Null.NullInteger
			txtNewBadWord.Text = String.Empty
			txtNewReplaceWord.Text = String.Empty
			BindData(ddlSearchType.SelectedValue, BottomPager.PageSize)
		End Sub

		''' <summary>
		''' Changes the items bound to the grid, based on a letter the word starts with or displays all words. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub ddlSearchType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlSearchType.SelectedIndexChanged
			'CurrentPage = 1
			BindData(ddlSearchType.SelectedValue, BottomPager.PageSize)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Binds data to the grid based on search filter
		''' </summary>
		''' <param name="SearchText"></param>
		''' <param name="PageSize"></param>
		''' <remarks>Page Size Not Implemented Yet!</remarks>
		Private Sub BindData(ByVal SearchText As String, ByVal PageSize As Integer)
			Dim ctlFilterWords As New WordFilterController
			Dim lstFilterWord As List(Of Forum.FilterWordInfo)

			If SearchText = Localization.GetString("All", Me.LocalResourceFile) Then
				lstFilterWord = ctlFilterWords.FilterWordGetAll(PortalId, "")
			Else
				lstFilterWord = ctlFilterWords.FilterWordGetAll(PortalId, ddlSearchType.SelectedValue)
			End If

			If lstFilterWord.Count > 0 Then
				grdBadWords.DataSource = lstFilterWord
				grdBadWords.DataBind()

				BottomPager.Visible = True
				grdBadWords.Visible = True
				lblNoResults.Visible = False
				' change ME implementing paging.
				'BottomPager.TotalRecords = lstFilterWord.TotalRecords
				BottomPager.TotalRecords = 10000000
			Else
				BottomPager.Visible = False
				grdBadWords.Visible = False
				lblNoResults.Visible = True
			End If
		End Sub

		''' <summary>
		''' Builds the letter filter
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cnurse]	9/10/2004	Updated to reflect design changes for Help, 508 support
		'''                       and localisation
		''' </history>
		Private Sub CreateLetterSearch()
			Dim filters As String = String.Empty

			filters += Localization.GetString("All", Me.LocalResourceFile)
			filters += "," + Localization.GetString("Filter", Me.LocalResourceFile)

			Dim strAlphabet As String() = filters.Split(","c)
			ddlSearchType.DataSource = strAlphabet
			ddlSearchType.DataBind()
		End Sub

		''' <summary>
		''' formats the user created information to the grid
		''' </summary>
		''' <param name="DataItem"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function DisplayAuthor(ByVal DataItem As FilterWordInfo) As String
			Dim strAuthor As String = String.Empty
			Dim cntForumUser As New ForumUserController
			strAuthor = cntForumUser.GetForumUser(DataItem.CreatedBy, False, ModuleId, PortalId).SiteAlias
			Return strAuthor
		End Function

		''' <summary>
		''' Formats the date for display in the grid
		''' </summary>
		''' <param name="DataItem"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function DisplayDate(ByVal DataItem As FilterWordInfo) As String
			Dim strDate As String = String.Empty
			strDate = Utilities.ForumUtils.ConvertTimeZone(DataItem.CreatedOn, objConfig).ToString
			Return strDate
		End Function

#End Region

	End Class

End Namespace
