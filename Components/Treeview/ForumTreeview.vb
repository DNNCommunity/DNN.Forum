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

#Region "ForumTreeview"

	''' <summary>
	''' Used to build the DNNTree in User Settings, Search views. 
	''' </summary>
	''' <remarks></remarks>
	Public Class ForumTreeview

#Region "Enums"

		''' <summary>
		''' The image type to use to represent the various levels in the treeview. 
		''' </summary>
		''' <remarks></remarks>
		Public Enum eImageType
			Group = 0
			Forum = 1
		End Enum

#End Region

		''' <summary>
		''' Sets the images for the treeview
		''' </summary>
		''' <param name="objConfig"></param>
		''' <param name="ForumTree"></param>
		''' <remarks></remarks>
		Public Shared Sub InitializeTree(ByVal objConfig As Forum.Config, ByVal ForumTree As DotNetNuke.UI.WebControls.DnnTree)
			' Handle expand/collapse images
			ForumTree.CollapsedNodeImage = objConfig.GetThemeImageURL("s_folder.") & objConfig.ImageExtension
			ForumTree.ExpandedNodeImage = objConfig.GetThemeImageURL("s_folderopen.") & objConfig.ImageExtension

			' Handle various level in tree images
			ForumTree.ImageList.Add(objConfig.GetThemeImageURL("spacer.gif"))
			ForumTree.ImageList.Add(objConfig.GetThemeImageURL("s_forum.") & objConfig.ImageExtension)
		End Sub

		''' <summary>
		''' Sets defaults for the ForumTree. (multi selection)
		''' </summary>
		''' <param name="objConfig"></param>
		''' <param name="ForumTree"></param>
		''' <param name="MultiSelect"></param>
		''' <remarks></remarks>
		Public Shared Sub SetTreeDefaults(ByVal objConfig As Forum.Config, ByVal ForumTree As DotNetNuke.UI.WebControls.DnnTree, ByVal MultiSelect As Boolean)
			ForumTree.PopulateNodesFromClient = True
			ForumTree.ForceDownLevel = False

			If MultiSelect Then
				ForumTree.CheckBoxes = True
			Else
				ForumTree.CheckBoxes = False
			End If

			'ForumTree.Target = "MyTarget"
			ForumTree.WorkImage = objConfig.GetThemeImageURL("s_dnnanim.") & objConfig.ImageExtension
		End Sub

		''' <summary>
		''' Starts the process of populating a Forum Treeview
		''' </summary>
		''' <param name="objConfig"></param>
		''' <param name="ForumTree"></param>
		''' <remarks></remarks>
		Public Shared Sub PopulateTree(ByVal objConfig As Forum.Config, ByVal ForumTree As DotNetNuke.UI.WebControls.DnnTree, ByVal UserID As Integer)
			ForumTree.TreeNodes.Clear()

			Dim cntGroup As New GroupController
			Dim arrAuthGroups As New List(Of GroupInfo)
			Dim objGroup As GroupInfo
			Dim objTreeNode As DotNetNuke.UI.WebControls.TreeNode

			arrAuthGroups = cntGroup.GroupGetAllAuthorized(objConfig.ModuleID, UserID, True)

			For Each objGroup In arrAuthGroups
				Dim arrForums As List(Of ForumInfo) = objGroup.AuthorizedForums(UserID, True)
				If arrForums.Count > 0 Then
					objTreeNode = AddNode(objGroup.Name, "G", objGroup.GroupID.ToString, eImageType.Group, ForumTree.TreeNodes, objGroup.GroupID, objConfig, UserID)

					If ForumTree.PopulateNodesFromClient = False Then
						AddForums(objGroup, objTreeNode, objConfig, UserID)
					Else
						'this should be set to true only when we have child nodes
						objTreeNode.HasNodes = True
					End If
				End If
			Next
		End Sub

		Public Shared Sub PopulateTelerikTree(ByVal objConfig As Forum.Config, ByVal ForumTree As Telerik.Web.UI.RadTreeView, ByVal UserID As Integer)
			ForumTree.Nodes.Clear()

			Dim cntGroup As New GroupController
			Dim arrAuthGroups As New List(Of GroupInfo)
			Dim objGroup As GroupInfo
			Dim objTreeNode As Telerik.Web.UI.RadTreeNode

			arrAuthGroups = cntGroup.GroupGetAllAuthorized(objConfig.ModuleID, UserID, True)

			For Each objGroup In arrAuthGroups
				Dim arrForums As List(Of ForumInfo) = objGroup.AuthorizedForums(UserID, True)
				If arrForums.Count > 0 Then
					objTreeNode = AddTelerikNode(objGroup.Name, "G", objGroup.GroupID.ToString, eImageType.Group, ForumTree.Nodes, objGroup.GroupID, objConfig, UserID)

					'If ForumTree.PopulateNodesFromCli = False Then
					'	AddForums(objGroup, objTreeNode, objConfig, UserID)
					'Else
					'	'this should be set to true only when we have child nodes
					'	objTreeNode.HasNodes = True
					'End If
					AddTelerikForums(objGroup, objTreeNode, objConfig, UserID)
				End If
			Next
		End Sub

		''' <summary>
		''' This renders one child node of a group in the tree.  This builds the list
		''' of forums and binds each active one to the tree.
		''' </summary>
		''' <param name="objGroup"></param>
		''' <param name="objNode"></param>
		''' <param name="objConfig"></param>
		''' <param name="UserID"></param>
		''' <remarks></remarks>
		Public Shared Sub AddForums(ByVal objGroup As GroupInfo, ByVal objNode As DotNetNuke.UI.WebControls.TreeNode, ByVal objConfig As Forum.Config, ByVal UserID As Integer)
			Dim cntForum As New ForumController
			Dim arrAuthForums As List(Of ForumInfo) = objGroup.AuthorizedForums(UserID, True)
			Dim forumNode As DotNetNuke.UI.WebControls.TreeNode

			For Each objForum As Forum.ForumInfo In arrAuthForums
				If objForum.IsActive And objForum.ParentId < 1 Then ' - And (Not objForum.ForumType = ForumType.Link) 
					Dim Security As New Forum.ModuleSecurity(objConfig.ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
					If Not objForum.PublicView Then
						If Security.IsAllowedToViewPrivateForum Then
							forumNode = AddNode(objForum.Name, "F", objForum.ForumID.ToString, eImageType.Forum, objNode.TreeNodes, objGroup.GroupID, objConfig, UserID)
						End If
					Else
						'We handle non-private seperately because module security (core) handles the rest
						forumNode = AddNode(objForum.Name, "F", objForum.ForumID.ToString, eImageType.Forum, objNode.TreeNodes, objGroup.GroupID, objConfig, UserID)
					End If
				End If
			Next
		End Sub

		Public Shared Sub AddTelerikForums(ByVal objGroup As GroupInfo, ByVal objNode As Telerik.Web.UI.RadTreeNode, ByVal objConfig As Forum.Config, ByVal UserID As Integer)
			Dim cntForum As New ForumController
			Dim arrAuthForums As List(Of ForumInfo) = objGroup.AuthorizedForums(UserID, True)
			Dim forumNode As Telerik.Web.UI.RadTreeNode

			For Each objForum As Forum.ForumInfo In arrAuthForums
				If objForum.IsActive And objForum.ParentId < 1 Then ' - And (Not objForum.ForumType = ForumType.Link) 
					Dim Security As New Forum.ModuleSecurity(objConfig.ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
					If Not objForum.PublicView Then
						If Security.IsAllowedToViewPrivateForum Then
							forumNode = AddTelerikNode(objForum.Name, "F", objForum.ForumID.ToString, eImageType.Forum, objNode.Nodes, objGroup.GroupID, objConfig, UserID)
						End If
					Else
						'We handle non-private seperately because module security (core) handles the rest
						forumNode = AddTelerikNode(objForum.Name, "F", objForum.ForumID.ToString, eImageType.Forum, objNode.Nodes, objGroup.GroupID, objConfig, UserID)
					End If
				End If
			Next
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
		''' <remarks>This only uses active forums.</remarks>
		Public Shared Function AddNode(ByVal strName As String, ByVal strType As String, ByVal strKey As String, ByVal eImage As eImageType, ByVal objNodes As DotNetNuke.UI.WebControls.TreeNodeCollection, ByVal GroupID As Integer, ByVal objConfig As Forum.Config, ByVal UserID As Integer) As DotNetNuke.UI.WebControls.TreeNode
			' CP - COMEBACK - we would add a check here to see if it has any children, if it does we would force an add node for each child in the list (recursion)
			' take the strKey (forumid) to see if it is a parent.
			Dim cntForum As New ForumController
			Dim arrForums As List(Of ForumInfo)

			arrForums = cntForum.ForumGetAllByParentID(CInt(strKey), GroupID, True)

			Dim objNode As DotNetNuke.UI.WebControls.TreeNode
			objNode = New DotNetNuke.UI.WebControls.TreeNode(strName)
			objNode.Key = strType + strKey
			objNode.ToolTip = strName

			If arrForums.Count > 0 Then
				' Parent Forum
				objNode.ClickAction = DotNetNuke.UI.WebControls.eClickAction.Expand
			Else
				objNode.ClickAction = DotNetNuke.UI.WebControls.eClickAction.None
				'	objNode.ImageIndex = eImage
			End If

			' Make it so only forums are selectable, not groups
			If strType = "G" Then
				'objNode.JSFunction = "getSelectedTreeNode();"
				objNode.Enabled = False
			Else
				objNode.Enabled = True
				'objNode.JSFunction = "getSelectedTreeNode();"
			End If

			objNode.CssClass = "Forum_TreeNormal"
			objNode.CSSClassSelected = "Forum_TreeNormalBold"
			objNodes.Add(objNode)

			If arrForums.Count > 0 Then
				' This is a parent - get children and run this method. Also, make sure it is not selectable. 
				Dim forumNode As DotNetNuke.UI.WebControls.TreeNode

				For Each objForum As Forum.ForumInfo In arrForums
					If objForum.IsActive Then
						Dim Security As New Forum.ModuleSecurity(objConfig.ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
						If Not objForum.PublicView Then
							If Security.IsAllowedToViewPrivateForum Then
								forumNode = AddNode(objForum.Name, "F", objForum.ForumID.ToString, eImageType.Forum, objNode.TreeNodes, GroupID, objConfig, UserID)
							End If
						Else
							'We handle non-private seperately because module security (core) handles the rest
							forumNode = AddNode(objForum.Name, "F", objForum.ForumID.ToString, eImageType.Forum, objNode.TreeNodes, GroupID, objConfig, UserID)
						End If
					End If
				Next
			End If

			Return objNode
		End Function

		Public Shared Function AddTelerikNode(ByVal strName As String, ByVal strType As String, ByVal strKey As String, ByVal eImage As eImageType, ByVal objNodes As Telerik.Web.UI.RadTreeNodeCollection, ByVal GroupID As Integer, ByVal objConfig As Forum.Config, ByVal UserID As Integer) As Telerik.Web.UI.RadTreeNode
			' CP - COMEBACK - we would add a check here to see if it has any children, if it does we would force an add node for each child in the list (recursion)
			' take the strKey (forumid) to see if it is a parent.
			Dim cntForum As New ForumController
			Dim arrForums As List(Of ForumInfo)

			arrForums = cntForum.ForumGetAllByParentID(CInt(strKey), GroupID, True)

			Dim objNode As Telerik.Web.UI.RadTreeNode
			objNode = New Telerik.Web.UI.RadTreeNode(strName)
			objNode.Value = strType + strKey
			objNode.ToolTip = strName

			' Make it so only forums are selectable, not groups
			If strType = "G" Then
				objNode.Checkable = False
			Else
				'If arrForums.Count > 0 Then
				'	' Parent Forum
				'	objNode.ClickActio = DotNetNuke.UI.WebControls.eClickAction.Expand
				'Else
				'	objNode.ClickAction = DotNetNuke.UI.WebControls.eClickAction.None
				'	'	objNode.ImageIndex = eImage
				'End If
				'objNode.Enabled = True
				objNode.Checkable = True

			End If

			objNodes.Add(objNode)

			If arrForums.Count > 0 Then
				' This is a parent - get children and run this method. Also, make sure it is not selectable. 
				Dim forumNode As Telerik.Web.UI.RadTreeNode

				For Each objForum As Forum.ForumInfo In arrForums
					If objForum.IsActive Then
						Dim Security As New Forum.ModuleSecurity(objConfig.ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
						If Not objForum.PublicView Then
							If Security.IsAllowedToViewPrivateForum Then
								forumNode = AddTelerikNode(objForum.Name, "F", objForum.ForumID.ToString, eImageType.Forum, objNode.Nodes, GroupID, objConfig, UserID)
							End If
						Else
							'We handle non-private seperately because module security (core) handles the rest
							forumNode = AddTelerikNode(objForum.Name, "F", objForum.ForumID.ToString, eImageType.Forum, objNode.Nodes, GroupID, objConfig, UserID)
						End If
					End If
				Next
			End If
			Return objNode
		End Function

	End Class

#End Region

End Namespace