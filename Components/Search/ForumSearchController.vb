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

#Region "SearchController"

    ''' <summary>
    ''' This class connects the search custom business object to the data layer.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SearchController

        ''' <summary>
        ''' Gets the paged results for a search query used by the module's self contained search control as threads.
        ''' </summary>
        ''' <param name="Filter">The search conditions, where clause, etc.</param>
        ''' <param name="PageIndex">The page number to return search results for.</param>
        ''' <param name="PageSize">The number of threads to return for the search at once.</param>
        ''' <param name="UserID">The UserID of who is performing the search.</param>
        ''' <param name="ModuleID">The moduleID being searched.</param>
        ''' <param name="FromDate">The start date to search with.</param>
        ''' <param name="ToDate">The end date to search with.</param>
        ''' <param name="ThreadStatusID">The threadStatus to search for.</param>
        ''' <returns>A collection of search results.</returns>
        ''' <remarks></remarks>
        Public Function SearchGetResults(ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal FromDate As DateTime, ByVal ToDate As DateTime, ByVal ThreadStatusID As Integer) As ArrayList
            Return CBO.FillCollection(DotNetNuke.Modules.Forum.DataProvider.Instance().SearchGetResults(Filter, PageIndex, PageSize, UserID, ModuleID, FromDate, ToDate, ThreadStatusID), GetType(SearchInfo))
        End Function

        ''' <summary>
        ''' Gets the paged results for a search query used by the module's self contained search control as posts.
        ''' </summary>
        ''' <param name="Filter">The search conditions, where clause, etc.</param>
        ''' <param name="PageIndex">The page number to return search results for.</param>
        ''' <param name="PageSize">The number of threads to return for the search at once.</param>
        ''' <param name="UserID">The UserID of who is performing the search.</param>
        ''' <param name="ModuleID">The moduleID being searched.</param>
        ''' <param name="FromDate">The start date to search with.</param>
        ''' <param name="ToDate">The end date to search with.</param>
        ''' <param name="ThreadStatusID">The threadStatus to search for.</param>
        ''' <returns>A collection of search results.</returns>
        ''' <remarks>Added by Skeel</remarks>
        Public Function Search(ByVal Filter As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal UserID As Integer, ByVal ModuleID As Integer, ByVal FromDate As DateTime, ByVal ToDate As DateTime, ByVal ThreadStatusID As Integer) As ArrayList
            Return CBO.FillCollection(DotNetNuke.Modules.Forum.DataProvider.Instance().Search(Filter, PageIndex, PageSize, UserID, ModuleID, FromDate, ToDate, ThreadStatusID), GetType(SearchResult))
        End Function

    End Class

#End Region

End Namespace