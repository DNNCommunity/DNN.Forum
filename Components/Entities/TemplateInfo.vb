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

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' An instance of the ForumTemplateInfo object. This object is used for all non-email templates.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class TemplateInfo

#Region "Private Members"

		'Private Const ForumTemplateInfoCacheKeyPrefix As String = "Forum_ForumTemplateInfo"
		Private _TemplateID As Integer
		Private _TemplateName As String
		Private _TemplateValue As String
		Private _ForumTemplateTypeID As Integer
		Private _ModuleID As Integer
		Private _IsActive As Boolean

#End Region

#Region "Public Properties"

		''' <summary>
		''' The TemplateID of the item.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TemplateID() As Integer
			Get
				Return _TemplateID
			End Get
			Set(ByVal Value As Integer)
				_TemplateID = Value
			End Set
		End Property

		''' <summary>
		''' The name of the template.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TemplateName() As String
			Get
				Return _TemplateName
			End Get
			Set(ByVal Value As String)
				_TemplateName = Value
			End Set
		End Property

		''' <summary>
		''' The value of the template. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TemplateValue() As String
			Get
				Return _TemplateValue
			End Get
			Set(ByVal Value As String)
				_TemplateValue = Value
			End Set
		End Property

		''' <summary>
		''' The Type of template.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ForumTemplateTypeID() As Integer
			Get
				Return _ForumTemplateTypeID
			End Get
			Set(ByVal Value As Integer)
				_ForumTemplateTypeID = Value
			End Set
		End Property

		''' <summary>
		''' The ModuleID the template is for.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal Value As Integer)
				_ModuleID = Value
			End Set
		End Property

		''' <summary>
		''' If the template is active or not.
		''' </summary>
		''' <value></value>
		''' <returns>True if the template is active, false otherwise.</returns>
		''' <remarks></remarks>
		Public Property IsActive() As Boolean
			Get
				Return _IsActive
			End Get
			Set(ByVal Value As Boolean)
				_IsActive = Value
			End Set
		End Property

#End Region

#Region "Constructor"

		''' <summary>
		''' Instantiates the class.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			MyBase.New()
		End Sub

#End Region

	End Class

End Namespace