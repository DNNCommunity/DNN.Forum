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
	''' Used to communicate with the data store for access to Email templates.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class EmailTemplateController

		''' <summary>
		''' Gets all active email templates available to a moduleID. 
		''' </summary>
		''' <param name="ModuleID">The ModuleID to retrieve email templates for.</param>
		''' <returns>An arraylist of active email templates for the Module instance.</returns>
		''' <remarks>Email templates are customizable by ModuleID.</remarks>
		Public Function GetEmailTemplatesByModuleID(ByVal ModuleID As Integer) As ArrayList
			Return CBO.FillCollection(DataProvider.Instance().GetEmailTemplatesByModuleID(ModuleID), GetType(EmailTemplateInfo))
		End Function

		''' <summary>
		''' Retrieves details about a specific email template for a module instance.
		''' </summary>
		''' <param name="EmailTemplateID">The PK EmailTemplateID to retrieve data for.</param>
		''' <returns>The ForumEmailTemplateInfo object containing specific details about a specific email template.</returns>
		''' <remarks></remarks>
		Public Function GetEmailTemplate(ByVal EmailTemplateID As Integer) As EmailTemplateInfo
			Return CType(CBO.FillObject(DataProvider.Instance().GetEmailTemplate(EmailTemplateID), GetType(EmailTemplateInfo)), EmailTemplateInfo)
		End Function

		''' <summary>
		''' Gets a specific email template for a module. 
		''' </summary>
		''' <param name="ModuleID">The module to retrieve the template for.</param>
		''' <param name="ForumTemplateTypeID">The template type to retrieve.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetEmailTemplateForMail(ByVal ModuleID As Integer, ByVal ForumTemplateTypeID As Integer) As EmailTemplateInfo
			Return CType(CBO.FillObject(DataProvider.Instance().GetEmailTemplateForMail(ModuleID, ForumTemplateTypeID), GetType(EmailTemplateInfo)), EmailTemplateInfo)
		End Function

		''' <summary>
		''' Gets all template types from the data store. Returns a generic standard template that can then be edited and stored specific to a ModuleID.
		''' </summary>
		''' <returns>A collection of email templates.</returns>
		''' <remarks></remarks>
		Public Function GetDefaultEmailTemplates() As ArrayList
			Return CBO.FillCollection(DataProvider.Instance().GetDefaultEmailTemplates(), GetType(EmailTemplateInfo))
		End Function

		''' <summary>
		''' Adds an email template to the data store for a specific ModuleID.
		''' </summary>
		''' <param name="objEmailTemplate">The EmailTemplateInfo object to add to the data store.</param>
		''' <returns>The EmailTemplateID created by adding this record.</returns>
		''' <remarks></remarks>
		Public Function AddEmailTemplateForModuleID(ByVal objEmailTemplate As EmailTemplateInfo) As Integer
			Return CType(DataProvider.Instance().AddEmailTemplateForModuleID(objEmailTemplate.ForumTemplateTypeID, objEmailTemplate.EmailSubject, objEmailTemplate.HTMLBody, objEmailTemplate.TextBody, objEmailTemplate.ModuleID, objEmailTemplate.IsActive, objEmailTemplate.ForumContentTypeID, objEmailTemplate.EmailTemplateName, objEmailTemplate.ForumEmailTypeID), Integer)
		End Function

		''' <summary>
		''' Updateds an email template in the data store specific to a ModuleID and TemplateTypeID.
		''' </summary>
		''' <param name="objEmailTemplate">The EmailTemplateInfo object to update in the data store.</param>
		''' <remarks></remarks>
		Public Sub UpdateEmailTemplate(ByVal objEmailTemplate As EmailTemplateInfo)
			DataProvider.Instance().UpdateEmailTemplate(objEmailTemplate.EmailTemplateID, objEmailTemplate.EmailSubject, objEmailTemplate.HTMLBody, objEmailTemplate.TextBody, objEmailTemplate.IsActive, objEmailTemplate.ModuleID)
		End Sub

	End Class

End Namespace