'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2009
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

#Region " PMcountInfo "

	''' <summary>
	''' Used by the UCP menu to determine the count of inbox and outbox messages 
	''' </summary>
	''' <remarks></remarks>
	Public Class PMCountInfo

#Region " Private Members "

		Private _inbox As Integer
		Private _outbox As Integer

#End Region

#Region " Public Properties "
		''' <summary>
		''' The number of unread PM of the current user 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Inbox() As Integer
			Get
				Return _inbox
			End Get
			Set(ByVal Value As Integer)
				_inbox = Value
			End Set
		End Property
		''' <summary>
		''' The number of unread PM, the current user has sent 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Outbox() As Integer
			Get
				Return _outbox
			End Get
			Set(ByVal Value As Integer)
				_outbox = Value
			End Set
		End Property

#End Region

	End Class
#End Region

End Namespace