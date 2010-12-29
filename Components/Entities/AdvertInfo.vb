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

Imports DotNetNuke.Services.Vendors

Namespace DotNetNuke.Modules.Forum

	''' <summary>
    ''' All properties associated with the Advertisement option
	''' </summary>
	''' <remarks></remarks>
	''' <history> 
	''' 	[b.waluszko]	21/10/2010	Created
	''' </history>
	Public Class AdvertInfo
		Inherits VendorInfo

#Region "Private Members"

		Private _IsEnabled As Boolean
		Private _ModuleID As Integer
		Private _BannersUrl As String

#End Region

#Region "Public Properties"

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsEnabled As Boolean
			Get
				Return _IsEnabled
			End Get
			Set(ByVal value As Boolean)
				_IsEnabled = value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModuleId As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal value As Integer)
				_ModuleID = value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property BannerUrl As String
			Get
				Return _BannersUrl
			End Get
			Set(ByVal value As String)
				_BannersUrl = value
			End Set
		End Property

#End Region

	End Class

End Namespace
