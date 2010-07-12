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

		Private mGroupID As Integer
		Private mName As String
		Private mPortalID As Integer
		Private mModuleID As Integer
		Private mSortOrder As Integer
		Private mCreatedDate As DateTime
		Private mUpdatedDate As DateTime
		Private mCreatedByUser As Integer
		Private mUpdatedByUser As Integer
		'Not part of table
		Private mForumCount As Integer
		Private mActiveForumCount As Integer

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
				Return mGroupID
			End Get
			Set(ByVal Value As Integer)
				mGroupID = Value
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
				Return mName
			End Get
			Set(ByVal Value As String)
				mName = Value
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
				Return mPortalID
			End Get
			Set(ByVal Value As Integer)
				mPortalID = Value
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
				Return mModuleID
			End Get
			Set(ByVal Value As Integer)
				mModuleID = Value
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
				Return mCreatedDate
			End Get
			Set(ByVal Value As DateTime)
				mCreatedDate = Value
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
				Return mCreatedByUser
			End Get
			Set(ByVal Value As Integer)
				mCreatedByUser = Value
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
				Return mUpdatedDate
			End Get
			Set(ByVal Value As DateTime)
				mUpdatedDate = Value
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
				Return mUpdatedByUser
			End Get
			Set(ByVal Value As Integer)
				mUpdatedByUser = Value
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
				Return mSortOrder
			End Get
			Set(ByVal Value As Integer)
				mSortOrder = Value
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
				Return mForumCount
			End Get
			Set(ByVal Value As Integer)
				mForumCount = Value
			End Set
		End Property

		''' <summary>
		''' Number of active forums in the group. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Not stored in table, retrieved in Get Sprocs</remarks>
		Public Property ActiveForumCount() As Integer
			Get
				Return mActiveForumCount
			End Get
			Set(ByVal Value As Integer)
				mActiveForumCount = Value
			End Set
		End Property

#End Region

		'#Region "Public ReadOnly Properties"

		'		''' <summary>
		'		''' The modules configuration settings. 
		'		''' </summary>
		'		''' <value></value>
		'		''' <returns></returns>
		'		''' <remarks></remarks>
		'		Public ReadOnly Property objConfig() As Forum.Configuration
		'			Get
		'				Return Forum.Configuration.GetForumConfig(ModuleID)
		'			End Get
		'		End Property

		'#End Region

	End Class

End Namespace