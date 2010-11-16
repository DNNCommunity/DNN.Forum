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
	''' Communicates with the Forum_WordFilter table in the data store. 
	''' </summary>
	''' <remarks></remarks>
	Public Class WordFilterController

#Region "Private Members"

		Private Const FilterWordsCacheKeyPrefix As String = "FilterWords"
		Private Const FilterWordsCacheTimeout As Integer = 20

#End Region

#Region "Public Methods"

		''' <summary>
		''' Checks the cache for a list of filtered words, if the item doesn't exist it populates the cache
		''' </summary>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetWords(ByVal PortalID As Integer) As FilterWordCollection
			Dim strCacheKey As String = FilterWordsCacheKeyPrefix & PortalID.ToString
			Dim filterWords As FilterWordCollection = CType(DataCache.GetCache(strCacheKey), FilterWordCollection)

			If filterWords Is Nothing Then
				Dim timeOut As Int32 = FilterWordsCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				filterWords = New FilterWordCollection(PortalID)

				'Cache Thread if timeout > 0 and Thread is not null
				If timeOut > 0 And filterWords IsNot Nothing Then
					DataCache.SetCache(strCacheKey, filterWords, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return filterWords
		End Function

		''' <summary>
		''' Clears the cached object of filtered words. 
		''' </summary>
		''' <param name="PortalID"></param>
		''' <remarks></remarks>
		Public Shared Sub ResetWords(ByVal PortalID As Integer)
			Dim strCacheKey As String = FilterWordsCacheKeyPrefix & PortalID.ToString
			DataCache.RemoveCache(strCacheKey)
		End Sub

		''' <summary>
		''' Filters passed in text against list of words to filter for. 
		''' </summary>
		''' <param name="Text"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function FilterBadWord(ByVal Text As String, ByVal PortalID As Integer) As String
			Dim colFilterWord As FilterWordCollection = GetWords(PortalID)

			Try
				' This is method to use collection with cache for better performance
				If Not Text Is Nothing Then
					For Each objFilterWord As FilterWordInfo In colFilterWord
						If Text.ToLower.IndexOf(objFilterWord.BadWord.ToLower) > -1 Then
							Text = Utilities.ForumUtils.ReplaceCaseInsensitive(Text, objFilterWord.BadWord, objFilterWord.ReplacedWord)
						End If
					Next
				End If
			Catch ex As Exception
				LogException(ex)
			End Try
			Return Text
		End Function

		''' <summary>
		''' Filters passed in text against list of works to filter and replace. Includes a timestamp to compare when content was added vs. when filter replacement was added so items can be skipped if possible. 
		''' </summary>
		''' <param name="Text"></param>
		''' <param name="TimeStamp"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function FilterBadWord(ByVal Text As String, ByVal TimeStamp As DateTime, ByVal PortalID As Integer) As String
			Dim colFilterWord As FilterWordCollection = GetWords(PortalID)

			Try
				' This is method to use collection with cache for better performance
				' Also filter word which added after timestamp date for better performance
				If Not Text Is Nothing Then
					For Each objFilterWord As FilterWordInfo In colFilterWord
						If objFilterWord.CreatedOn > TimeStamp Then
							If Text.ToLower.IndexOf(objFilterWord.BadWord.ToLower) > -1 Then
								Text = Utilities.ForumUtils.ReplaceCaseInsensitive(Text, objFilterWord.BadWord, objFilterWord.ReplacedWord)
							End If
						End If
					Next
				End If
			Catch ex As Exception
				LogException(ex)
			End Try
			Return Text
		End Function

		''' <summary>
		''' Retrieves a collection of filtered words from the data store.
		''' </summary>
		''' <param name="PortalID"></param>
		''' <param name="Filter"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function FilterWordGetAll(ByVal PortalID As Integer, ByVal Filter As String) As List(Of FilterWordInfo)
			Return CBO.FillCollection(Of FilterWordInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().FilterWordGetAll(PortalID, Filter))
		End Function

		''' <summary>
		''' Returns a single filtered word.
		''' </summary>
		''' <param name="ItemId"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function FilterWordGet(ByVal ItemId As Integer) As FilterWordInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().FilterWordGet(ItemId), GetType(FilterWordInfo)), FilterWordInfo)
		End Function

		''' <summary>
		''' Updates a single filtered word in the data store.
		''' </summary>
		''' <param name="PortalID"></param>
		''' <param name="BadWord"></param>
		''' <param name="ReplacedWord"></param>
		''' <param name="CreatedBy"></param>
		''' <remarks></remarks>
		Public Sub FilterWordUpdate(ByVal PortalID As Integer, ByVal BadWord As String, ByVal ReplacedWord As String, ByVal CreatedBy As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().FilterWordUpdate(PortalID, BadWord, ReplacedWord, CreatedBy)
		End Sub

		''' <summary>
		''' Deletes a filtered word from the data store.
		''' </summary>
		''' <param name="ItemID"></param>
		''' <remarks></remarks>
		Public Sub FilterWordDelete(ByVal ItemID As Integer)
			DotNetNuke.Modules.Forum.DataProvider.Instance().FilterWordDelete(ItemID)
		End Sub

		''' <summary>
		''' Determines if a filtered word exists in the data store for the particular portal.
		''' </summary>
		''' <param name="FilteredWord"></param>
		''' <param name="PortalID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function FilterWordGetByWord(ByVal FilteredWord As String, ByVal PortalID As Integer) As Boolean
			Return DotNetNuke.Modules.Forum.DataProvider.Instance().FilterWordGetByWord(FilteredWord, PortalID)
		End Function

#End Region

	End Class

End Namespace