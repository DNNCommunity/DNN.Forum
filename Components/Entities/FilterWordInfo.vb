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
	''' All properties associated with the Forum_FilterWords Word items. 
	''' </summary>
	''' <remarks>Portal specific.</remarks>
	Public Class FilterWordInfo

#Region "Private Members"

		Private _ItemID As Integer
		Private _PortalID As Integer
		Private _BadWord As String = String.Empty
		Private _ReplacedWord As String = String.Empty
		Private _CreatedBy As Integer
		Private _CreatedOn As DateTime

#End Region

#Region "Public Properties"

		''' <summary>
		''' The PK of the filter item.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ItemID() As Integer
			Get
				Return _ItemID
			End Get
			Set(ByVal Value As Integer)
				_ItemID = Value
			End Set
		End Property

		''' <summary>
		''' The PortalID the filter item belongs to.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PortalID() As Integer
			Get
				Return _PortalID
			End Get
			Set(ByVal Value As Integer)
				_PortalID = Value
			End Set
		End Property

		''' <summary>
		''' The word to filter.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property BadWord() As String
			Get
				Return _BadWord
			End Get
			Set(ByVal Value As String)
				_BadWord = Value
			End Set
		End Property

		''' <summary>
		''' String to replace the filtered word with.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ReplacedWord() As String
			Get
				Return _ReplacedWord
			End Get
			Set(ByVal Value As String)
				_ReplacedWord = Value
			End Set
		End Property

		''' <summary>
		''' Who created the filter item.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedBy() As Integer
			Get
				Return _CreatedBy
			End Get
			Set(ByVal Value As Integer)
				_CreatedBy = Value
			End Set
		End Property

		''' <summary>
		''' Date the item was created on.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedOn() As DateTime
			Get
				Return _CreatedOn
			End Get
			Set(ByVal Value As DateTime)
				_CreatedOn = Value
			End Set
		End Property

#End Region

	End Class

End Namespace