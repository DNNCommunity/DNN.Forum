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

#Region "ForumPermissionCollection"

	''' <summary>
	''' Basically a copy of the core ModulePermissionCollection.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class ForumPermissionCollection
		Inherits CollectionBase

#Region "Constructors"

		''' <summary>
		''' Instantiates the class.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			MyBase.New()
		End Sub

		''' <summary>
		''' Instantiates the class.
		''' </summary>
		''' <param name="ForumPermissions">A collection of forum permissions.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal ForumPermissions As ArrayList)
			Dim i As Integer
			For i = 0 To ForumPermissions.Count - 1
				Dim objForumPerm As ForumPermissionInfo = CType(ForumPermissions(i), ForumPermissionInfo)
				Add(objForumPerm)
			Next
		End Sub

		''' <summary>
		''' Instantiates the class.
		''' </summary>
		''' <param name="ForumPermissions">A collection of forum permissions.</param>
		''' <param name="ForumID">The specific forum.</param>
		''' <remarks></remarks>
		Public Sub New(ByVal ForumPermissions As ArrayList, ByVal ForumID As Integer)
			Dim i As Integer
			For i = 0 To ForumPermissions.Count - 1
				Dim objForumPerm As ForumPermissionInfo = CType(ForumPermissions(i), ForumPermissionInfo)
				If objForumPerm.ForumID = ForumID Then
					Add(objForumPerm)
				End If
			Next
		End Sub

#End Region

#Region "Properties"

		''' <summary>
		''' A single ForumPermissionInfo object.
		''' </summary>
		''' <param name="index"></param>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Default Public Property Item(ByVal index As Integer) As ForumPermissionInfo
			Get
				Return CType(List(index), ForumPermissionInfo)
			End Get
			Set(ByVal Value As ForumPermissionInfo)
				List(index) = Value
			End Set
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Add an item to the collection.
		''' </summary>
		''' <param name="value"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Add(ByVal value As ForumPermissionInfo) As Integer
			Return List.Add(value)
		End Function

		''' <summary>
		''' Finds the index value of an item in the collection.
		''' </summary>
		''' <param name="value"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function IndexOf(ByVal value As ForumPermissionInfo) As Integer
			Return List.IndexOf(value)
		End Function

		''' <summary>
		''' Inserts an item into the collection.
		''' </summary>
		''' <param name="index"></param>
		''' <param name="value"></param>
		''' <remarks></remarks>
		Public Sub Insert(ByVal index As Integer, ByVal value As ForumPermissionInfo)
			List.Insert(index, value)
		End Sub

		''' <summary>
		''' Removes an item from the collection.
		''' </summary>
		''' <param name="value"></param>
		''' <remarks></remarks>
		Public Sub Remove(ByVal value As ForumPermissionInfo)
			List.Remove(value)
		End Sub

		''' <summary>
		''' Determines if an item already exists in the collection.
		''' </summary>
		''' <param name="value"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Contains(ByVal value As ForumPermissionInfo) As Boolean
			Return List.Contains(value)
		End Function

		''' <summary>
		''' Compares two items.
		''' </summary>
		''' <param name="objForumPermissionCollection"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function CompareTo(ByVal objForumPermissionCollection As ForumPermissionCollection) As Boolean
			If objForumPermissionCollection.Count <> Me.Count Then
				Return False
			End If
			InnerList.Sort(New CompareForumPermissions)
			objForumPermissionCollection.InnerList.Sort(New CompareForumPermissions)

			Dim objForumPermission As ForumPermissionInfo
			Dim i As Integer
			For Each objForumPermission In objForumPermissionCollection
				If objForumPermissionCollection(i).PermissionID <> Me(i).PermissionID _
				Or objForumPermissionCollection(i).AllowAccess <> Me(i).AllowAccess Then
					Return False
				End If
				i += 1
			Next
			Return True
		End Function

#End Region

	End Class

#End Region

#Region "CompareForumPermissions"

	''' <summary>
	''' Compares two sets of forum permission collections.
	''' </summary>
	''' <remarks></remarks>
	Public Class CompareForumPermissions
		Implements System.Collections.IComparer

#Region "IComparer Interface"

		''' <summary>
		''' Compares two sets of forum permissions.
		''' </summary>
		''' <param name="x">A forum permissions object.</param>
		''' <param name="y">A forum permissions object.</param>
		''' <returns>An integer indicating whether one is less than, equal to or greater than the other.</returns>
		''' <remarks></remarks>
		Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
			Return CType(x, ForumPermissionInfo).PermissionID.CompareTo(CType(y, ForumPermissionInfo).PermissionID)
		End Function

#End Region

	End Class

#End Region

End Namespace
