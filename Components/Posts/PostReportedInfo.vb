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

#Region "PostReportedInfo"

	Public Class PostReportedInfo

#Region "Private Members"

		Dim _PostReportedID As Integer
		Dim _CreatedDate As Date
		Dim _PostID As Integer
		Dim _UserID As Integer
		Dim _Reason As String
		Dim _Addressed As Boolean
		Dim _TotalRecords As Integer

#End Region

#Region "Public Properties"

		''' <summary>
		''' The PK value of the table. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostReportedID() As Integer
			Get
				Return _PostReportedID
			End Get
			Set(ByVal Value As Integer)
				_PostReportedID = Value
			End Set
		End Property

		''' <summary>
		''' The date the post was reported. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedDate() As Date
			Get
				Return _CreatedDate
			End Get
			Set(ByVal Value As Date)
				_CreatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The PostID of the reported post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostID() As Integer
			Get
				Return _PostID
			End Get
			Set(ByVal Value As Integer)
				_PostID = Value
			End Set
		End Property

		''' <summary>
		''' The UserID who reported the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UserID() As Integer
			Get
				Return _UserID
			End Get
			Set(ByVal Value As Integer)
				_UserID = Value
			End Set
		End Property

		''' <summary>
		''' The reason the user reported the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Reason() As String
			Get
				Return _Reason
			End Get
			Set(ByVal Value As String)
				_Reason = Value
			End Set
		End Property

		''' <summary>
		''' If this report has been addressed by a moderator or admin. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Addressed() As Boolean
			Get
				Return _Addressed
			End Get
			Set(ByVal Value As Boolean)
				_Addressed = Value
			End Set
		End Property

		''' <summary>
		''' Total number of records.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This is only used for paging counts.</remarks>
		Public Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal value As Integer)
				_TotalRecords = value
			End Set
		End Property

		''' <summary>
		''' The user information about the user who reported the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Author(ByVal ModuleID As Integer, ByVal PortalID As Integer) As ForumUser
			Get
				Return ForumUserController.GetForumUser(UserID, False, ModuleID, PortalID)
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <param name="PortalID"></param>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Post(ByVal PortalID As Integer) As PostInfo
			Get
				Return PostInfo.GetPostInfo(PostID, PortalID)
			End Get
		End Property

#End Region

	End Class

#End Region

#Region "ReportedUserInfo"

	Public Class ReportedUserInfo

#Region "Private Members"

		Dim _UserID As Integer
		Dim _ReportedPostCount As Integer
		Dim _UnaddressedPostCount As Integer
		Dim _TotalRecords As Integer

#End Region

#Region "Public Properties"

		''' <summary>
		''' The UserID who posted the reported post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UserID() As Integer
			Get
				Return _UserID
			End Get
			Set(ByVal Value As Integer)
				_UserID = Value
			End Set
		End Property

		''' <summary>
		''' The number of reported posts a user has.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This should not be confused with how many times a user's post was reported. Just a total count of posts the user posted that were reported at least once.</remarks>
		Public Property ReportedPostCount() As Integer
			Get
				Return _ReportedPostCount
			End Get
			Set(ByVal Value As Integer)
				_ReportedPostCount = Value
			End Set
		End Property

		''' <summary>
		''' The number of reported posts a user has that have not be 'addressed' by a forum admin or moderator. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This should always be less or equal to reportedpostcount.</remarks>
		Public Property UnaddressedPostCount() As Integer
			Get
				Return _UnaddressedPostCount
			End Get
			Set(ByVal Value As Integer)
				_UnaddressedPostCount = Value
			End Set
		End Property

		''' <summary>
		''' Total number of records.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This is only used for paging counts.</remarks>
		Public Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal value As Integer)
				_TotalRecords = value
			End Set
		End Property

		''' <summary>
		''' The user information about the user who reported the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Author(ByVal ModuleID As Integer, ByVal PortalID As Integer) As ForumUser
			Get
				Return ForumUserController.GetForumUser(UserID, False, ModuleID, PortalID)
			End Get
		End Property

#End Region

	End Class

#End Region

End Namespace