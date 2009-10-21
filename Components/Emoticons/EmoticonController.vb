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

#Region " EmoticonController "

    ''' <summary>
    ''' CRUD (and all db methods) Emoticon Methods
    ''' </summary>
    ''' <remarks>Added by Skeel</remarks>
    Public Class EmoticonController

#Region " Private Members "

        Private Const EmoticonCacheKeyPrefix As String = "Emoticon"
        Private Const ForumInfoCacheTimeout As Integer = 120 ' Emoticons shouldn't be changed that often...

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Instantiats the RoleAvatar Contoller. 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Get Emoticons
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <returns>RoleAvatarInfo</returns>
        ''' <history>
        ''' 	[skeel]	12/15/2008	Created
        ''' </history>
        Private Function GetAllFromDB(ByVal ModuleID As Integer, ByVal IsDefault As Boolean) As List(Of EmoticonInfo)

            Dim objEmoticons As New List(Of EmoticonInfo)
			Dim dr As IDataReader = Nothing
            Try
                dr = DotNetNuke.Modules.Forum.DataProvider.Instance().Emoticon_GetAll(ModuleID, IsDefault)
                While dr.Read
                    Dim objInfo As EmoticonInfo = FillEmoticonInfo(dr)
                    objEmoticons.Add(objInfo)
                End While
                dr.NextResult()

            Catch ex As Exception
                LogException(ex)
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try

            Return objEmoticons

        End Function

        ''' <summary>
        ''' Resets the Emoticon list in cache. 
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <remarks></remarks>
        Private Sub ResetEmoticonCache(ByVal ModuleID As Integer, ByVal IsDefault As Boolean)
            Dim strCacheKey As String = EmoticonCacheKeyPrefix & CStr(ModuleID & IsDefault)
            DataCache.RemoveCache(strCacheKey)
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Attempts to load the Emoticons from cache, if not available it retrieves it and places it in cache. 
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <param name="DefaultsOnly"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAll(ByVal ModuleID As Integer, ByVal DefaultsOnly As Boolean) As List(Of EmoticonInfo)

            Dim strCacheKey As String = EmoticonCacheKeyPrefix & CStr(ModuleID & DefaultsOnly)
            Dim objEmoticon As List(Of EmoticonInfo) = CType(DataCache.GetCache(strCacheKey), List(Of EmoticonInfo))

            If objEmoticon Is Nothing Then
                'Statistics caching settings
				Dim timeOut As Int32 = ForumInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

                objEmoticon = GetAllFromDB(ModuleID, DefaultsOnly)
                'Cache Statistics if timeout > 0 and Statistics is not null
                If timeOut > 0 And objEmoticon IsNot Nothing Then
					DataCache.SetCache(strCacheKey, objEmoticon, TimeSpan.FromMinutes(timeOut))
                End If
            End If


            Return objEmoticon
        End Function

        ''' <summary>
        ''' Updates or adds the Emoticon
        ''' </summary>
        ''' <param name="objEmoticon"></param>
        ''' <remarks></remarks>
        Public Sub Update(ByVal objEmoticon As EmoticonInfo)
            DotNetNuke.Modules.Forum.DataProvider.Instance().Emoticon_Update(objEmoticon)
            ResetEmoticonCache(objEmoticon.ModuleID, True)
            ResetEmoticonCache(objEmoticon.ModuleID, False)
        End Sub

        ''' <summary>
        ''' Deletes the selected Emoticon
        ''' </summary>
        ''' <param name="ID"></param>
        ''' <param name="ModuleId"></param>
        ''' <remarks></remarks>
        Public Sub Delete(ByVal ID As Integer, ByVal ModuleId As Integer)
            DotNetNuke.Modules.Forum.DataProvider.Instance().Emoticon_Delete(ID)
            ResetEmoticonCache(ModuleId, True)
            ResetEmoticonCache(ModuleId, False)
        End Sub

        ''' <summary>
        ''' Updates the sort order for an emoticon in the data store.
        ''' </summary>
        ''' <param name="ID"></param>
        ''' <param name="ModuleId"></param>
        ''' <param name="MoveUp"></param>
        ''' <remarks></remarks>
        Public Sub SortOrderUpdate(ByVal ID As Integer, ByVal ModuleId As Integer, ByVal MoveUp As Boolean)
            DotNetNuke.Modules.Forum.DataProvider.Instance().Emoticon_SetOrder(ID, ModuleId, MoveUp)
            ResetEmoticonCache(ModuleId, False)
            ResetEmoticonCache(ModuleId, True)
        End Sub

        ''' <summary>
        ''' Filters passed in text against list of emoticons to filter for. 
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ProcessEmoticons(ByVal Text As String, ByVal ModuleID As Integer) As String
            Dim colEmoticons As New EmoticonCollection(ModuleID)

            Try
                ' This is method to use collection with cache for better performance
                If Not Text Is Nothing Then
                    For Each objEmoticon As EmoticonInfo In colEmoticons
                        If Text.ToLower.IndexOf(objEmoticon.Code.ToLower) > -1 Then
                            ' We will replace regardless the case
                            Text = Strings.Replace(Text, objEmoticon.Code, objEmoticon.EmoticonHTML, 1, , CompareMethod.Text)
                        End If
                    Next
                End If
            Catch ex As Exception
                LogException(ex)
            End Try
            Return Text
        End Function

        ''' <summary>
        ''' Will identify if Text contains Emoticons 
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IdentifyEmoticons(ByVal Text As String, ByVal ModuleID As Integer) As Boolean
            Dim HasEmoticons As Boolean = False
            Dim colEmoticons As New EmoticonCollection(ModuleID)

            Try
                ' This is method to use collection with cache for better performance
                If Not Text Is Nothing Then
                    For Each objEmoticon As EmoticonInfo In colEmoticons
                        If Text.ToLower.IndexOf(objEmoticon.Code.ToLower) > -1 Then
                            HasEmoticons = True
                            Exit For
                        End If
                    Next
                End If
            Catch ex As Exception
                LogException(ex)
            End Try
            Return HasEmoticons
        End Function

#End Region

#Region " Custom Hydrator "

        ''' <summary>
        ''' Hydrates the EmoticonInfo object
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[skeel]	12/22/2008	Created
        ''' </history>
        Private Function FillEmoticonInfo(ByVal dr As IDataReader) As EmoticonInfo
            Dim objEmoticon As New EmoticonInfo
            Try
                objEmoticon.ID = Convert.ToInt32(Null.SetNull(dr("ID"), objEmoticon.ID))
            Catch
            End Try
            Try
                objEmoticon.Code = Convert.ToString(Null.SetNull(dr("Code"), objEmoticon.Code))
            Catch
            End Try
            Try
                objEmoticon.Emoticon = Convert.ToString(Null.SetNull(dr("Emoticon"), objEmoticon.Emoticon))
            Catch
            End Try
            Try
                objEmoticon.IsDefault = Convert.ToBoolean(Null.SetNull(dr("IsDefault"), objEmoticon.IsDefault))
            Catch
            End Try
            Try
                objEmoticon.ModuleID = Convert.ToInt32(Null.SetNull(dr("ModuleID"), objEmoticon.ModuleID))
            Catch
            End Try
            Try
                objEmoticon.SortOrder = Convert.ToInt32(Null.SetNull(dr("SortOrder"), objEmoticon.SortOrder))
            Catch
            End Try
            Try
                objEmoticon.ToolTip = Convert.ToString(Null.SetNull(dr("ToolTip"), objEmoticon.ToolTip))
            Catch
            End Try

            Return objEmoticon
        End Function

#End Region

    End Class

#End Region

End Namespace