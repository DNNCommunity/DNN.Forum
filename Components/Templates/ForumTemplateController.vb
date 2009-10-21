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

#Region "ForumTemplateController"

    ''' <summary>
    ''' Forum Template Controller - These are DeletePost, MoveThread, etc.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	11/20/2006	Created
    ''' </history>
    Public Class ForumTemplateController

        ''' <summary>
        ''' Retrieves a collection of templates from the Forum_Templates table in the data store based on template type.
        ''' </summary>
        ''' <param name="ModuleID">The Module to retrieve templates for.</param>
        ''' <param name="ForumTemplateTypeID">The type of template to retieve.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TemplatesGetByType(ByVal ModuleID As Integer, ByVal ForumTemplateTypeID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().TemplatesGetByType(ModuleID, ForumTemplateTypeID), GetType(ForumTemplateInfo))
        End Function

        ''' <summary>
        ''' Retrieves a collection of forum templates from the Forum_Templates table in the data store based on the template type.
        ''' </summary>
        ''' <param name="ForumTemplateTypeID">The template type to retrieve default templates for.</param>
        ''' <returns>A collection of templates.</returns>
        ''' <remarks></remarks>
        Public Function TemplatesGetDefaults(ByVal ForumTemplateTypeID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().TemplatesGetDefaults(ForumTemplateTypeID), GetType(ForumTemplateInfo))
        End Function

        ''' <summary>
        ''' Adds a row to the Forum_Templates table in the data store. 
        ''' </summary>
        ''' <param name="objForumTemplate">The template object to add to the data store.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TemplatesAddForModuleID(ByVal objForumTemplate As ForumTemplateInfo) As Integer
            Return CType(DataProvider.Instance().TemplatesAddForModuleID(objForumTemplate.TemplateName, objForumTemplate.TemplateValue, objForumTemplate.ForumTemplateTypeID, objForumTemplate.ModuleID, objForumTemplate.IsActive), Integer)
        End Function

        ''' <summary>
        ''' Updates a template in the data store.
        ''' </summary>
        ''' <param name="objForumTemplate">The ForumTemplateInfo object to update in the data store.</param>
        ''' <remarks></remarks>
        Public Sub TemplatesUpdate(ByVal objForumTemplate As ForumTemplateInfo)
            DataProvider.Instance().TemplatesUpdate(objForumTemplate.TemplateID, objForumTemplate.TemplateName, objForumTemplate.TemplateValue, objForumTemplate.ForumTemplateTypeID, objForumTemplate.ModuleID, objForumTemplate.IsActive)
        End Sub

        ''' <summary>
        ''' Retrieves a single ForumTemplateInfo object from the data store.
        ''' </summary>
        ''' <param name="TemplateID">The template PK value to retrieve from the data store.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TemplatesGetSingle(ByVal TemplateID As Integer) As ForumTemplateInfo
            Return CType(CBO.FillObject(DataProvider.Instance().TemplatesGetSingle(TemplateID), GetType(ForumTemplateInfo)), ForumTemplateInfo)
        End Function

    End Class

#End Region

End Namespace