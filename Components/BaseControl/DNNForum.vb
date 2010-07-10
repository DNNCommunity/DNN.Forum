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
	''' This is initialized by the Forum_Container (dispatch).
	''' </summary>
	''' <remarks>This is the second classes loaded which in turn loads up other classes to generate a UI programatically. DNNForum serves as a placeholder.
	''' </remarks>
	Public Class DNNForum
		Inherits ForumBaseControl

#Region "Constructors"

		''' <summary>
		''' Instantiats this class (which is the basis for most UI)
		''' </summary>
		''' <remarks>Used to determine which UI to render (thread view, post view, group view, search results view)</remarks>
		Public Sub New()
			MyBase.New()

			If Not HttpContext.Current Is Nothing Then
				If Not HttpContext.Current.Request.QueryString("scope") Is Nothing Then
					ViewType = CType([Enum].Parse(GetType(ForumScope), HttpContext.Current.Request.QueryString("scope"), True), ForumScope)
				Else
					If Not HttpContext.Current.Request.QueryString("postid") Is Nothing Then
						ViewType = ForumScope.Posts
					Else
						ViewType = ForumScope.Groups
					End If
				End If
			End If
		End Sub

#End Region

#Region "Protected Methods"

		''' <summary>
		''' Initializes the proper class to render the forum UI
		''' </summary>
		''' <remarks>Creates a new instance of the base object as the type of forum view (ie. load the thread view UI).</remarks>
		Protected Overrides Sub CreateObject()
			Select Case ViewType
				Case ForumScope.Groups
					ForumBaseObject = New Groups(Me)
					'Case ForumScope.Forums
					'	ForumBaseObject = New Forums(Me)
				Case ForumScope.Threads
					ForumBaseObject = New Threads(Me)
				Case ForumScope.Posts
					ForumBaseObject = New Posts(Me)
				Case ForumScope.ThreadSearch
					ForumBaseObject = New ThreadSearch(Me)
				Case ForumScope.Unread
					ForumBaseObject = New Unread(Me)
			End Select
		End Sub

		''' <summary>
		''' Here to make certain the forum instance is completely assigned the proper variables
		''' </summary>
		''' <remarks>Used to make certain everything in the control is rendered. This actually can decrease performance (but not noticeably)</remarks>
		Protected Overrides Sub Initialize()
			MyBase.Initialize()
			Me.EnsureChildControls()
		End Sub

		''' <summary>
		''' we have to create the controls each time because they are built dynamically and loaded into Forum_Container.ascx
		''' </summary>
		''' <remarks>Creates all controls rendered in this class.</remarks>
		Protected Overrides Sub CreateChildControls()
			MyBase.CreateChildControls()
		End Sub

#End Region

	End Class

End Namespace