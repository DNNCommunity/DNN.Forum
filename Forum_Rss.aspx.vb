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
	''' Used for all Forum RSS Feeds.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class Rss
		Inherits System.Web.UI.Page

#Region "Private Members"

		Private mForumID As Integer = 0
		Private mForumConfig As Forum.Configuration
		Private mThreadsPage As Integer = 1
		Private mTabId As Integer = 0
		Private mModuleId As Integer = 0

#End Region

#Region "Event Handlers"

		''' <summary>
		''' First determines what information to pull, then determines if this is authorized. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	8/17/2005	Created
		''' </history>
		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				If Not Request.QueryString("forumid") Is Nothing Then
					mForumID = Int32.Parse(Request.QueryString("forumid"))
					Dim mForumInfo As New ForumInfo

					If Not Request.QueryString("tabid") Is Nothing Then
						mTabId = Int32.Parse(Request.QueryString("tabid"))
					End If

					If Not Request.QueryString("mid") Is Nothing Then
						mModuleId = Int32.Parse(Request.QueryString("mid"))
					End If

					mForumConfig = Configuration.GetForumConfig(mModuleId)

					If Not Request.QueryString("threadspage") Is Nothing Then
						mThreadsPage = Int32.Parse(Request.QueryString("threadspage"))
					End If

					If mForumID <> -1 Then
						' We need this to make sure the forum is enabled and not private
						Dim cntForum As New ForumController
						mForumInfo = cntForum.GetForumItemCache(mForumID)
					End If

					Response.ContentType = "text/xml"
					Response.ContentEncoding = Encoding.UTF8

					' Handle aggregated Forum
					If mForumID = -1 Then
						' Have to pass in ModuleId to RSS links to work
						Dim document As RssDocument = RssDocument.GetForumRss(mForumID, mThreadsPage, mTabId, mModuleId)
						Response.Write(document.OuterXml)
					Else
						If Not mForumInfo Is Nothing Then
							' Make sure the forum is not private
							If (mForumInfo.PublicView = True) And (mForumInfo.IsActive) And (mForumInfo.EnableRSS) And (mForumConfig.EnableRSS) Then
								Try
									Dim document As RssDocument = RssDocument.GetForumRss(mForumID, mThreadsPage, mTabId, mModuleId)
									Response.Write(document.OuterXml)
								Catch exc As Exception
									LogException(exc)
								End Try
							Else
								' Its private, redirect.  (This would only happen w/ direct typed link)
								Response.Redirect(NavigateURL(Utilities.Links.UnAuthorizedLink()), True)
							End If
						Else
							' Forum Deleted or never existed (This would only happen w/ direct typed link)
							Response.Redirect(NavigateURL(Utilities.Links.UnAuthorizedLink()), True)
						End If
					End If
				Else
					' If there is no forumID, the user should not be here or something went wrong
					Response.Redirect(NavigateURL(Utilities.Links.UnAuthorizedLink()), True)
				End If
			Catch exc As Exception
				DotNetNuke.Services.Exceptions.LogException(exc)
			End Try
		End Sub

#End Region

	End Class

End Namespace