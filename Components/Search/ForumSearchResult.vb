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

#Region "SearchResult"

	''' <summary>
	''' Search Info is a 'hybrid' type class used to create a custom business object that represents merged results of a search from posts/threads tables.
	''' </summary>
	''' <remarks>Added by Skeel</remarks>
	Public Class SearchResult

#Region " Private Properties "

		Private mPostID As Integer
		Private mThreadID As Integer
		Private mForumID As Integer
		Private mAuthorID As Integer
		Private mRecordCount As Integer
		Private mModuleId As Integer
		Private mSubject As String
		Private mBody As String
		Private mCreatedDate As Date
		Private mForumName As String
		Private mTabID As Integer

#End Region

#Region " Constructors "

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()

		End Sub

		''' <summary>
		''' Constructor to be used in majority of the module.
		''' </summary>
		''' <remarks>Only instantiated from ModuleSearch.vb - meaning a UI is required for it.</remarks>
		Public Sub New(ByVal TabID As Integer)
			mTabID = TabID
		End Sub

#End Region

#Region " Public Properties "

		''' <summary>
		''' The PortalID being used when the search is being conducted.
		''' </summary>
		''' <value></value>
		''' <returns>The current PortalID.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property PortalID() As Integer
			Get
				Dim _portalSettings As Portals.PortalSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings
				Return _portalSettings.PortalId
			End Get
		End Property

		''' <summary>
		''' The PostID of the search result item
		''' </summary>
		''' <value></value>
		''' <returns>The PostID.</returns>
		''' <remarks></remarks>
		Public Property PostID() As Integer
			Get
				Return mPostID
			End Get
			Set(ByVal Value As Integer)
				mPostID = Value
			End Set
		End Property

		''' <summary>
		''' The date the post was created. 
		''' </summary>
		''' <value></value>
		''' <returns>Date the post was created.</returns>
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
		''' The ThreadID which the search result belongs to. 
		''' </summary>
		''' <value></value>
		''' <returns>ThreadID of the search result.</returns>
		''' <remarks></remarks>
		Public Property ThreadID() As Integer
			Get
				Return mThreadID
			End Get
			Set(ByVal Value As Integer)
				mThreadID = Value
			End Set
		End Property

		''' <summary>
		''' Total number of search results returned.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property RecordCount() As Integer
			Get
				Return mRecordCount
			End Get
			Set(ByVal Value As Integer)
				mRecordCount = Value
			End Set
		End Property

		''' <summary>
		''' Post author ID from the search results. 
		''' </summary>
		''' <value></value>
		''' <returns>Last approved PostID of the thread.</returns>
		''' <remarks></remarks>
		Public Property AuthorID() As Integer
			Get
				Return mAuthorID
			End Get
			Set(ByVal Value As Integer)
				mAuthorID = Value
			End Set
		End Property

		''' <summary>
		''' The subject of the post from the search results returned.
		''' </summary>
		''' <value></value>
		''' <returns>Subject of the post.</returns>
		''' <remarks></remarks>
		Public Property Subject() As String
			Get
				Return mSubject
			End Get
			Set(ByVal Value As String)
				mSubject = Value
			End Set
		End Property

		''' <summary>
		''' Nmae of the forums which the result post belongs
		''' </summary>
		''' <value></value>
		''' <returns>Subject of the post.</returns>
		''' <remarks></remarks>
		Public Property ForumName() As String
			Get
				Return mForumName
			End Get
			Set(ByVal Value As String)
				mForumName = Value
			End Set
		End Property

		''' <summary>
		''' The body of the post from the search results returned.
		''' </summary>
		''' <value></value>
		''' <returns>Body of the post.</returns>
		''' <remarks></remarks>
		Public Property Body() As String
			Get
				Return mBody
			End Get
			Set(ByVal Value As String)
				mBody = Value
			End Set
		End Property

		''' <summary>
		''' The ForumID the search result belongs to.
		''' </summary>
		''' <value></value>
		''' <returns>The forumID the post belongs to.</returns>
		''' <remarks></remarks>
		Public Property ForumID() As Integer
			Get
				Return mForumID
			End Get
			Set(ByVal Value As Integer)
				mForumID = Value
			End Set
		End Property

		''' <summary>
		''' The ModuleID the search result belongs to.
		''' </summary>
		''' <value></value>
		''' <returns>The forumID the post belongs to.</returns>
		''' <remarks></remarks>
		Public Property ModuleId() As Integer
			Get
				Return mModuleId
			End Get
			Set(ByVal Value As Integer)
				mModuleId = Value
			End Set
		End Property

		''' <summary>
		''' The user information about the user who posted the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Author() As ForumUser
			Get
				Dim cntForumUser As New ForumUserController
				Return cntForumUser.GetForumUser(mAuthorID, False, ModuleId, PortalID)
			End Get
		End Property



#End Region


	End Class

#End Region

End Namespace