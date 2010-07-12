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
	''' An instance of the ForumEmailTemplateInfo object. This object is used for all email templates.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class EmailTemplateInfo

#Region "Private Declarations"

		Private _EmailTemplateID As Integer
		Private _EmailSubject As String
		Private _HTMLBody As String
		Private _TextBody As String
		Private _ModuleID As Integer
		Private _IsActive As Boolean
		Private _ForumTemplateTypeID As Integer
		Private _ForumContentTypeID As ForumContentTypeID
		Private _EmailTemplateName As String
		Private _ForumEmailTypeID As Integer

#End Region

#Region "Public Properties"

		''' <summary>
		''' The PK value for EmailTemplates.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property EmailTemplateID() As Integer
			Get
				Return _EmailTemplateID
			End Get
			Set(ByVal Value As Integer)
				_EmailTemplateID = Value
			End Set
		End Property

		''' <summary>
		''' The subject of the email template.
		''' </summary>
		''' <value></value>
		''' <returns>The subject of the outgoing email.</returns>
		''' <remarks></remarks>
		Public Property EmailSubject() As String
			Get
				Return _EmailSubject
			End Get
			Set(ByVal Value As String)
				_EmailSubject = Value
			End Set
		End Property

		''' <summary>
		''' The HTML Body of the email template.
		''' </summary>
		''' <value></value>
		''' <returns>The HTML used for an email's body if the send format is HTML.</returns>
		''' <remarks></remarks>
		Public Property HTMLBody() As String
			Get
				Return _HTMLBody
			End Get
			Set(ByVal Value As String)
				_HTMLBody = Value
			End Set
		End Property

		''' <summary>
		''' The Text Body of the email template.
		''' </summary>
		''' <value></value>
		''' <returns>The text used for an email's body if the send format is text.</returns>
		''' <remarks></remarks>
		Public Property TextBody() As String
			Get
				Return _TextBody
			End Get
			Set(ByVal Value As String)
				_TextBody = Value
			End Set
		End Property

		''' <summary>
		''' The ModuleID the email template is associated with.
		''' </summary>
		''' <value></value>
		''' <returns>The ModuleID the email template is associated with.</returns>
		''' <remarks>-1 if a default email template.</remarks>
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal Value As Integer)
				_ModuleID = Value
			End Set
		End Property

		''' <summary>
		''' If the email template is active or not.
		''' </summary>
		''' <value></value>
		''' <returns>True if the email template is active, false otherwise.</returns>
		''' <remarks></remarks>
		Public Property IsActive() As Boolean
			Get
				Return _IsActive
			End Get
			Set(ByVal Value As Boolean)
				_IsActive = Value
			End Set
		End Property

		''' <summary>
		''' The type of template this email template is associated with.
		''' </summary>
		''' <value></value>
		''' <returns>The email template type.</returns>
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
		''' The type of content associated with the email template.
		''' </summary>
		''' <value></value>
		''' <returns>The content type associated with the email template.</returns>
		''' <remarks></remarks>
		Public Property ForumContentTypeID() As ForumContentTypeID
			Get
				Return _ForumContentTypeID
			End Get
			Set(ByVal Value As ForumContentTypeID)
				_ForumContentTypeID = Value
			End Set
		End Property

		''' <summary>
		''' The name of the email template.
		''' </summary>
		''' <value></value>
		''' <returns>The name of the email template.</returns>
		''' <remarks></remarks>
		Public Property EmailTemplateName() As String
			Get
				Return _EmailTemplateName
			End Get
			Set(ByVal Value As String)
				_EmailTemplateName = Value
			End Set
		End Property

		''' <summary>
		''' The type of forum email the template is associated with.
		''' </summary>
		''' <value></value>
		''' <returns>The integer type of forum email the template is associated with.</returns>
		''' <remarks></remarks>
		Public Property ForumEmailTypeID() As Integer
			Get
				Return _ForumEmailTypeID
			End Get
			Set(ByVal Value As Integer)
				_ForumEmailTypeID = Value
			End Set
		End Property

#End Region

	End Class

End Namespace