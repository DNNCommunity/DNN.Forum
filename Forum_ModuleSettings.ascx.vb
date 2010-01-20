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
Option Explicit On
Option Strict On

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' The Settings ModuleSettingsBase is used to manage the 
	''' settings for the Forum Module. You only store settings here that you want to allow
	''' to be changeable at the page level. This means you can copy existing versions of the module
	''' based on (ModuleID) and display things different based on the page settings for this ModuleID. 
	''' </summary>
	''' <remarks>[cpaterra] - As of 11/2006 this is thought to be feature complete.</remarks>
	Public MustInherit Class ModuleSettings
		Inherits Entities.Modules.ModuleSettingsBase

#Region "Enums"

		''' <summary>
		''' The image type to use to represent the various levels in the treeview. 
		''' </summary>
		''' <remarks></remarks>
		Private Enum eImageType
			Group = 0
			Forum = 1
		End Enum

#End Region

#Region "Private Members"

		Private mForumConfig As Forum.Config

#End Region

#Region "Base Method Implementations"

		''' <summary>
		''' LoadSettings loads the settings from the Databas and displays them
		''' </summary>
		''' <remarks></remarks>
		Public Overrides Sub LoadSettings()
			Try
				If (Page.IsPostBack = False) Then
					Dim objGroups As New GroupController
					cboGroup.DataSource = objGroups.GroupsGetByModuleID(ModuleId)
					cboGroup.DataBind()

					cboGroup.Items.Insert(0, New ListItem(Localization.GetString("AllGroups", ForumConfig.SharedResourceFile), "0"))
					cboGroup.Items.Insert(1, New ListItem(Localization.GetString("AggregatedGroupName", ForumConfig.SharedResourceFile), "-1"))
					rowDefaultForum.Visible = False

					' Bind the default group
					If CType(TabModuleSettings("groupid"), String) <> String.Empty Then
						If Not cboGroup.Items.FindByValue(CType(TabModuleSettings("groupid"), String)) Is Nothing Then
							cboGroup.Items.FindByValue(CType(TabModuleSettings("groupid"), String)).Selected = True
							If Not CType(cboGroup.SelectedValue, Integer) = -1 Then
								rowDefaultForum.Visible = True
							End If
						End If
					Else
						cboGroup.SelectedIndex = 0
						rowDefaultForum.Visible = True
					End If

					ForumTreeview.PopulateTelerikTree(mForumConfig, rtvForums, UserId)

					' Bind the default forum
					If CType(TabModuleSettings("defaultforumid"), String) <> String.Empty And (Not cboGroup.SelectedValue = "-1") Then
						' we need to select this in the treeview
						Dim defaultID As Integer = CType(TabModuleSettings("defaultforumid"), Integer)
						' if defaultID = 0, it means no forum value was set, so only run if the value is greater than 0
						If Not defaultID = 0 Then
							SelectDefaultForumTree(defaultID)
						End If
					End If
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' UpdateSettings saves the modified settings to the Database
		''' </summary>
		''' <remarks></remarks>
		Public Overrides Sub UpdateSettings()
			Try
				Dim objModules As New Entities.Modules.ModuleController
				Dim defaultID As Integer = 0

				objModules.UpdateTabModuleSetting(TabModuleId, "groupid", cboGroup.SelectedItem.Value)

				If Not CType(cboGroup.SelectedItem.Value, Integer) = -1 Then
					' Set the default forumid (tree) can only be a single selction here
					For Each objNode As Telerik.Web.UI.RadTreeNode In Me.rtvForums.CheckedNodes
						Dim strType As String = objNode.Value.Substring(0, 1)
						Dim iID As Integer = CInt(objNode.Value.Substring(1, objNode.Value.Length - 1))

						defaultID = iID
						Exit For
					Next
				Else
					' this means aggregated is selected, since there is only a single forum in that group, automatically bind to that forum.
					defaultID = -1
				End If

				objModules.UpdateTabModuleSetting(TabModuleId, "defaultforumid", defaultID.ToString)
			Catch exc As Exception	 'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Selects the saved defaultforumid to the forum treeview control.
		''' </summary>
		''' <param name="DefaultForumId"></param>
		''' <remarks></remarks>
		Private Sub SelectDefaultForumTree(ByVal DefaultForumId As Integer)
			Dim node As Telerik.Web.UI.RadTreeNode = rtvForums.FindNodeByValue("F" + DefaultForumId.ToString)
			node.Selected = True
			node.ParentNode.Expanded = True
			node.Checked = True
		End Sub

		''' <summary>
		''' This builds one node of the tree.  It determines if the node is at the
		''' group or forum level and builds the node accordingly.
		''' </summary>
		''' <param name="strName"></param>
		''' <param name="strType"></param>
		''' <param name="strKey"></param>
		''' <param name="eImage"></param>
		''' <param name="objNodes"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function AddNode(ByVal strName As String, ByVal strType As String, ByVal strKey As String, ByVal eImage As eImageType, ByVal objNodes As DotNetNuke.UI.WebControls.TreeNodeCollection) As DotNetNuke.UI.WebControls.TreeNode
			Dim objNode As DotNetNuke.UI.WebControls.TreeNode
			objNode = New DotNetNuke.UI.WebControls.TreeNode(strName)
			objNode.Key = strType + strKey
			objNode.ToolTip = strName
			objNode.ImageIndex = eImage
			objNode.ClickAction = DotNetNuke.UI.WebControls.eClickAction.None

			' Make it so only forums are selectable, not groups
			If strType = "G" Then
				objNode.Enabled = False
			Else
				objNode.Enabled = True
			End If

			objNode.CssClass = "Normal"
			objNode.CSSClassSelected = "NormalBold"
			objNodes.Add(objNode)
			Return objNode
		End Function

#End Region

#Region "Protected Functions"

		''' <summary>
		''' Gets an instance of the modules current configuration
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function ForumConfig() As Forum.Config
			If mForumConfig Is Nothing Then
				mForumConfig = DotNetNuke.Modules.Forum.Config.GetForumConfig(ModuleId)
			End If
			Return mForumConfig
		End Function

#End Region

#Region "Event Handlers"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			If mForumConfig Is Nothing Then
				mForumConfig = ForumConfig()
			End If
		End Sub

#End Region

	End Class

End Namespace
