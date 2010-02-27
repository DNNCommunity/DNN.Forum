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
	''' A single instance of the GroupInfo Object. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class GroupInfo

#Region "Private Members"

		Private _GroupID As Integer
		Private _Name As String
		Private _PortalID As Integer
		Private _ModuleID As Integer
		Private _SortOrder As Integer
		Private _CreatedDate As DateTime
		Private _CreatedByUser As Integer
		Private _UpdatedDate As DateTime
		Private _UpdatedByUser As Integer
		'Not part of table
		Private _ForumCount As Integer

#End Region

#Region "Constructors"

		''' <summary>
		''' Instantiats the Group object. 
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Builds a collection of authorized forums for the specified Group.
		''' </summary>
		''' <param name="UserID">The user to return results for. (Permissions)</param>
		''' <param name="NoLinkForums">True if no link type forums should be added to the collection.</param>
		''' <returns>A Generics collection of ForumInfo items.</returns>
		''' <remarks></remarks>
		Public Function AuthorizedForums(ByVal UserID As Integer, ByVal NoLinkForums As Boolean) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.ForumGetAll(GroupID)
			' add Aggregated Forum option
			If GroupID = -1 Then
				objForum = New ForumInfo
				objForum.ModuleID = ModuleID
				objForum.GroupID = -1
				objForum.ForumID = -1
				objForum.ForumType = ForumType.Normal

				arrAuthForums.Add(objForum)
			End If

			For Each objForum In arrAllForums
				Dim Security As New Forum.ModuleSecurity(ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
				If Not objForum.PublicView And objForum.IsActive Then
					If Security.IsAllowedToViewPrivateForum And objForum.IsActive Then
						If NoLinkForums Then
							If Not (objForum.ForumType = ForumType.Link) Then
								arrAuthForums.Add(objForum)
							End If
						Else
							arrAuthForums.Add(objForum)
						End If
					End If
				ElseIf objForum.IsActive Then
					'We handle non-private seperately because module security (core) handles the rest
					If NoLinkForums Then
						If Not (objForum.ForumType = ForumType.Link) Then
							arrAuthForums.Add(objForum)
						End If
					Else
						arrAuthForums.Add(objForum)
					End If
				End If
			Next

			Return arrAuthForums
		End Function

		''' <summary>
		''' Builds a collection of authorized forums with no parents for the specified Group.
		''' </summary>
		''' <param name="UserID">The user to return results for. (Permissions)</param>
		''' <param name="NoLinkForums">True if no link type forums should be added to the collection.</param>
		''' <returns>A Generics collection of ForumInfo items.</returns>
		''' <remarks></remarks>
		Public Function AuthorizedNoParentForums(ByVal UserID As Integer, ByVal NoLinkForums As Boolean) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.ForumGetAllByParentID(0, GroupID, True)
			' add Aggregated Forum option
			If GroupID = -1 Then
				objForum = New ForumInfo
				objForum.ModuleID = ModuleID
				objForum.GroupID = -1
				objForum.ForumID = -1
				objForum.ForumType = ForumType.Normal

				arrAuthForums.Add(objForum)
			End If

			For Each objForum In arrAllForums
				Dim Security As New Forum.ModuleSecurity(ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
				If Not objForum.PublicView And objForum.IsActive Then
					If Security.IsAllowedToViewPrivateForum And objForum.IsActive Then
						If NoLinkForums Then
							If Not (objForum.ForumType = ForumType.Link) Then
								arrAuthForums.Add(objForum)
							End If
						Else
							arrAuthForums.Add(objForum)
						End If
					End If
				ElseIf objForum.IsActive Then
					'We handle non-private seperately because module security (core) handles the rest
					If NoLinkForums Then
						If Not (objForum.ForumType = ForumType.Link) Then
							arrAuthForums.Add(objForum)
						End If
					Else
						arrAuthForums.Add(objForum)
					End If
				End If
			Next

			Return arrAuthForums
		End Function

		''' <summary>
		''' Builds a collection of authorized subforums for the specified Group/Parrent Forum.
		''' </summary>
		''' <param name="UserID">The user to return results for. (Permissions)</param>
		''' <param name="NoLinkForums">True if no link type forums should be added to the collection.</param>
		''' <returns>A Generics collection of ForumInfo items.</returns>
		''' <remarks></remarks>
		Public Function AuthorizedSubForums(ByVal UserID As Integer, ByVal NoLinkForums As Boolean, ByVal ParentForumId As Integer) As List(Of ForumInfo)
			Dim cntForum As New ForumController
			Dim arrAuthForums As New List(Of ForumInfo)
			Dim arrAllForums As New List(Of ForumInfo)
			Dim objForum As ForumInfo

			arrAllForums = cntForum.ForumGetAllByParentID(ParentForumId, GroupID, True)
			' add Aggregated Forum option
			If GroupID = -1 Then
				objForum = New ForumInfo
				objForum.ModuleID = ModuleID
				objForum.GroupID = -1
				objForum.ForumID = -1
				objForum.ForumType = ForumType.Normal

				arrAuthForums.Add(objForum)
			End If

			For Each objForum In arrAllForums
				Dim Security As New Forum.ModuleSecurity(ModuleID, objConfig.CurrentPortalSettings.ActiveTab.TabID, objForum.ForumID, UserID)
				If Not objForum.PublicView And objForum.IsActive Then
					If Security.IsAllowedToViewPrivateForum And objForum.IsActive Then
						If NoLinkForums Then
							If Not (objForum.ForumType = ForumType.Link) Then
								arrAuthForums.Add(objForum)
							End If
						Else
							arrAuthForums.Add(objForum)
						End If
					End If
				ElseIf objForum.IsActive Then
					'We handle non-private seperately because module security (core) handles the rest
					If NoLinkForums Then
						If Not (objForum.ForumType = ForumType.Link) Then
							arrAuthForums.Add(objForum)
						End If
					Else
						arrAuthForums.Add(objForum)
					End If
				End If
			Next

			Return arrAuthForums
		End Function

#End Region

#Region "Public Properties"

		''' <summary>
		''' The GroupID (PK)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property GroupID() As Integer
			Get
				Return _GroupID
			End Get
			Set(ByVal Value As Integer)
				_GroupID = Value
			End Set
		End Property

		''' <summary>
		''' The name of the group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Name() As String
			Get
				Return _Name
			End Get
			Set(ByVal Value As String)
				_Name = Value
			End Set
		End Property

		''' <summary>
		''' The PortalID the group belongs to. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PortalID() As Integer
			Get
				Return _PortalID
			End Get
			Set(ByVal Value As Integer)
				_PortalID = Value
			End Set
		End Property

		''' <summary>
		''' The moduleID this group is part of. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal Value As Integer)
				_ModuleID = Value
			End Set
		End Property

		''' <summary>
		''' The sort order (ascending/descending) the group display in. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property SortOrder() As Integer
			Get
				Return _SortOrder
			End Get
			Set(ByVal Value As Integer)
				_SortOrder = Value
			End Set
		End Property

		''' <summary>
		''' The date the group was created. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedDate() As DateTime
			Get
				Return _CreatedDate
			End Get
			Set(ByVal Value As DateTime)
				_CreatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The UserID of the person who created the Group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedByUser() As Integer
			Get
				Return _CreatedByUser
			End Get
			Set(ByVal Value As Integer)
				_CreatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' The last UserID to update the group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedByUser() As Integer
			Get
				Return _UpdatedByUser
			End Get
			Set(ByVal Value As Integer)
				_UpdatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' The date the group was last updated. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedDate() As DateTime
			Get
				Return _UpdatedDate
			End Get
			Set(ByVal Value As DateTime)
				_UpdatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The total number of forums in the group. (includes inactive)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Not stored in table, retrieved in Get Sprocs</remarks>
		Public Property ForumCount() As Integer
			Get
				Return _ForumCount
			End Get
			Set(ByVal Value As Integer)
				_ForumCount = Value
			End Set
		End Property

#End Region

#Region "Public ReadOnly Properties"

		''' <summary>
		''' The modules configuration settings. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property objConfig() As Forum.Config
			Get
				Return Forum.Config.GetForumConfig(ModuleID)
			End Get
		End Property

#End Region

	End Class

End Namespace