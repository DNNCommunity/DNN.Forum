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

#Region "FilterWordCollection"

    ''' <summary>
    ''' A collection of words to filter for and their replacements. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class FilterWordCollection
        Inherits CollectionBase

#Region "Private Members"

        Private mIndexLookup As Hashtable = New Hashtable

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Instatiates the class.
        ''' </summary>
        ''' <param name="PortalID"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal PortalID As Integer)
            MyBase.New()

			Dim dr As IDataReader = Nothing
            Try
                dr = DotNetNuke.Modules.Forum.DataProvider.Instance().FilterWordGetAll(PortalID, "")
                While dr.Read
                    Dim objWordFilter As New FilterWordInfo
                    Try
                        objWordFilter.PortalID = Convert.ToInt32(Null.SetNull(dr("PortalID"), Null.NullInteger))
                    Catch
                    End Try
                    Try
                        objWordFilter.BadWord = Convert.ToString(Null.SetNull(dr("BadWord"), Null.NullString))
                    Catch
                    End Try
                    Try
                        objWordFilter.ReplacedWord = Convert.ToString(Null.SetNull(dr("ReplacedWord"), Null.NullString))
                    Catch
                    End Try
                    Try
                        objWordFilter.CreatedBy = Convert.ToInt32(Null.SetNull(dr("CreatedBy"), Null.NullInteger))
                    Catch
                    End Try
                    Try
                        objWordFilter.CreatedOn = Convert.ToDateTime(Null.SetNull(dr("CreatedOn"), Null.NullDate))
                    Catch
                    End Try

                    Me.Add(objWordFilter.BadWord, objWordFilter)

                End While
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try
        End Sub

#End Region

#Region "Methods"

        ''' <summary>
        ''' Adds an item to the collection.
        ''' </summary>
        ''' <param name="Key"></param>
        ''' <param name="Value"></param>
        ''' <remarks></remarks>
        Public Sub Add(ByVal Key As String, ByVal Value As Object)
            Dim index As Integer
            Try
                index = MyBase.List.Add(Value)
                mIndexLookup.Add(Key, index)
            Catch ex As Exception

            End Try
        End Sub

        ''' <summary>
        ''' Clears the collection.
        ''' </summary>
        ''' <remarks></remarks>
        Friend Shadows Sub Clear()
            mIndexLookup.Clear()
            MyBase.Clear()
        End Sub

        ''' <summary>
        ''' Represents one item in the collection.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function Item(ByVal index As Integer) As Object
            Dim obj As New Object
            Try
                obj = MyBase.List.Item(index)
                Return obj
            Catch Exc As System.Exception
            End Try
            Return obj
        End Function

        ''' <summary>
        ''' Represents one item in the collection.
        ''' </summary>
        ''' <param name="BadWord"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function Item(ByVal BadWord As String) As Object
            Dim index As Integer
            Dim obj As Object

            ' Do validation first
            If mIndexLookup.Item(BadWord) Is Nothing Then
                Return Nothing
            End If

            index = CInt(mIndexLookup.Item(BadWord))
            obj = MyBase.List.Item(index)

            Return obj
        End Function

#End Region

    End Class

#End Region

End Namespace