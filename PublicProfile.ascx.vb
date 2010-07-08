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
	''' This control now servers as a placeholder to redirect users to the core profile page (available in 5.3 and greater). 
	''' </summary>
	''' <remarks>This will eventually be removed.</remarks>
	Partial Class PublicProfile
		Inherits ForumModuleBase

#Region "Event Handlers"

		''' <summary>
		''' Loads a users profile and binds the data to the page
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load
			Try
				If Request.QueryString("userid") IsNot Nothing Then
					Dim profileUserID As Integer = CInt(Request.QueryString("userid").ToLower)
					Dim url As String = DotNetNuke.Common.Globals.UserProfileURL(profileUserID)

					Response.AddHeader("Location", url)
					Response.StatusCode = 301
					Response.Redirect(url, True)
				Else
					Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

	End Class

End Namespace