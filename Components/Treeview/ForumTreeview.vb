'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2011
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
			''' <summary>
			''' Forum Group tree node.
			''' </summary>
			''' <remarks></remarks>
			Group = 0
			''' <summary>
			''' Forum tree node.
			''' </summary>
			''' <remarks></remarks>
			Forum = 1
		End Enum

#End Region

		''' <summary>
		''' Method used to populate a Telerik treeview control. It looks through all groups using the current user's permissions and then populates necessary nodes by calling other methods.
		''' </summary>
		''' <param name="objConfig">The module configuration.</param>
		''' <param name="ForumTree">The RadTreeView object being populated.</param>
		''' <param name="UserID">The user currently viewing the group/forum tree.</param>
		''' <param name="TabID">The page the user is viewing.</param>
		''' <remarks></remarks>
		Friend Shared Sub PopulateTelerikTree(ByVal objConfig As Forum.Configuration, ByVal ForumTree As Telerik.Web.UI.RadTreeView, ByVal UserID As Integer, ByVal TabID As Integer)
			ForumTree.Nodes.Clear()

			Dim cntGroup As New GroupController
			Dim arrAuthGroups As New List(Of GroupInfo)
			Dim objTreeNode As Telerik.Web.UI.RadTreeNode

			arrAuthGroups = cntGroup.GroupGetAllAuthorized(objConfig.ModuleID, UserID, True, TabID)

			For Each objGroup As GroupInfo In arrAuthGroups
				Dim arrForums As List(Of ForumInfo) = cntGroup.AuthorizedForums(UserID, objGroup.GroupID, True, objConfig.ModuleID, TabID)
				If arrForums.Count > 0 Then
					objTreeNode = AddTelerikNode(objGroup.Name, "G", objGroup.GroupID.ToString, eImageType.Group, ForumTree.Nodes, objGroup.GroupID, objConfig, UserID)

					'If ForumTree.PopulateNodesFromCli = False Then
					'	AddForums(objGroup, objTreeNode, objConfig, UserID)
					'Else
					'	'this should be set to true only when we have child nodes
					'	objTreeNode.HasNodes = True
					'End If
					AddTelerikForums(objGroup, objTreeNode, objConfig, UserID, TabID)
				End If
			Next
		End Sub

		''' <summary>
		''' Adds a series of child nodes for a group (depending on permissions). 
		''' </summary>
		''' <param name="objGroup">The Group object we are creating a node for.</param>
		''' <param name="objNode">A single Tree node.</param>
		''' <param name="objConfig">The module configuration.</param>
		''' <param name="UserID">The user currently viewing the group/forum tree.</param>
		''' <param name="TabID">The page the user is viewing.</param>
		''' <remarks></remarks>
		Private Shared Sub AddTelerikForums(ByVal objGroup As GroupInfo, ByVal objNode As Telerik.Web.UI.RadTreeNode, ByVal objConfig As Forum.Configuration, ByVal UserID As Integer, ByVal TabID As Integer)
			Dim cntGroup As New GroupController()
			Dim arrAuthForums As List(Of ForumInfo) = cntGroup.AuthorizedForums(UserID, objGroup.GroupID, True, objConfig.ModuleID, TabID)
			Dim forumNode As Telerik.Web.UI.RadTreeNode

			For Each objForum As Forum.ForumInfo In arrAuthForums
				If objForum.IsActive And objForum.ParentID < 1 Then ' - And (Not objForum.ForumType = ForumType.Link) 
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
		''' Adds a node to the Telerik treeview control.
		''' </summary>
		''' <param name="strName">The name of the node we are working with.</param>
		''' <param name="strType">The tree node type.</param>
		''' <param name="strKey">The key used for lookup (parent forum id).</param>
		''' <param name="eImage">The image to be used for the node (if applicable).</param>
		''' <param name="objNodes">The Tree node collection.</param>
		''' <param name="GroupID">The group we are populating.</param>
		''' <param name="objConfig">The module configuration.</param>
		''' <param name="UserID">The user currently viewing the group/forum tree.</param>
		''' <returns></returns>
		''' <remarks>We are currently not supporting images using this control.</remarks>
		Private Shared Function AddTelerikNode(ByVal strName As String, ByVal strType As String, ByVal strKey As String, ByVal eImage As eImageType, ByVal objNodes As Telerik.Web.UI.RadTreeNodeCollection, ByVal GroupID As Integer, ByVal objConfig As Forum.Configuration, ByVal UserID As Integer) As Telerik.Web.UI.RadTreeNode
			' CP - COMEBACK - we would add a check here to see if it has any children, if it does we would force an add node for each child in the list (recursion)
			' take the strKey (forumid) to see if it is a parent.
			Dim cntForum As New ForumController
			Dim arrForums As List(Of ForumInfo)

			arrForums = cntForum.GetChildForums(CInt(strKey), GroupID, True)

			Dim objNode As Telerik.Web.UI.RadTreeNode
			objNode = New Telerik.Web.UI.RadTreeNode(strName)
			objNode.Value = strType + strKey
			objNode.ToolTip = strName

			' Make it so only forums are selectable, not groups
			If strType = "G" Then
				objNode.Checkable = False
			Else
				If arrForums.Count > 0 Then
					objNode.Checkable = False
				Else
					objNode.Checkable = True
				End If
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

End Namespace