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
	''' A single instance of the RoleAvatarInfo Object.
	''' </summary>
	''' <remarks></remarks>
	Public Class RoleAvatarInfo

#Region " Private Members "

		Private mRoleID As Integer
		Private mRoleName As String
		Private mAvatar As String
		Private mForumConfig As Config

#End Region

#Region " Public Properties "

		''' <summary>
		''' RoleID
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property RoleID() As Integer
			Get
				Return mRoleID
			End Get
			Set(ByVal Value As Integer)
				mRoleID = Value
			End Set
		End Property

		''' <summary>
		''' RoleName
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property RoleName() As String
			Get
				Return mRoleName
			End Get
			Set(ByVal Value As String)
				mRoleName = Value
			End Set
		End Property

		''' <summary>
		''' Avatar as file name, is always places in the SystemAvatar FoolderPath
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Avatar() As String
			Get
				Return mAvatar
			End Get
			Set(ByVal Value As String)
				mAvatar = Value
			End Set
		End Property

#End Region

	End Class

End Namespace