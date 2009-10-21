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

#Region "ForumKeywordInfo"

    ''' <summary>
    ''' Keyword Info Object is used for retrieving what keywords to parse for based on content type 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	8/27/2006	Created
    ''' </history>
    Public Class ForumKeywordInfo

#Region "Private Declarations"

        Private Const ForumKeywordInfoCacheKeyPrefix As String = "Forum_KeywordInfo"
        Private _KeywordID As Integer
        Private _Token As String
        Private _Description As String
        Private _ContentTypeID As Integer

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The Keyword ID (PK).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KeywordID() As Integer
            Get
                Return _KeywordID
            End Get
            Set(ByVal Value As Integer)
                _KeywordID = Value
            End Set
        End Property

        ''' <summary>
        ''' The token represents the keyword.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Token() As String
            Get
                Return _Token
            End Get
            Set(ByVal Value As String)
                _Token = Value
            End Set
        End Property

        ''' <summary>
        ''' Description about what the keyword is replaced with.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        ''' <summary>
        ''' The Content type that this keyword is for.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContentTypeID() As Integer
            Get
                Return _ContentTypeID
            End Get
            Set(ByVal Value As Integer)
                _ContentTypeID = Value
            End Set
        End Property

#End Region

#Region "Public Methods"

#Region "Constructors"

        ''' <summary>
        ''' Instatiates the class.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            MyBase.New()
        End Sub

#End Region

        ''' <summary>
        ''' This is where we get a list of keywords from the db based on a keyword type. In this case it will be forum email keywords
        ''' </summary>
        ''' <param name="ContentTypeID"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	8/27/2006	Created
        ''' </history>
        Public Shared Function GetKeywordsHash(ByVal ContentTypeID As ForumContentTypeID) As Hashtable
            Dim strCacheKey As String = ForumKeywordInfoCacheKeyPrefix & ContentTypeID

            'If DataCache.GetCache(strCacheKey) Is Nothing Then
            Dim ctlKeywords As New ForumKeywordController
            Dim arrKeywords As ArrayList
            Dim KeywordHash As New Hashtable

            arrKeywords = ctlKeywords.GetKeywordsByType(ContentTypeID)

            If arrKeywords.Count > 0 Then
                For Each objKeywordInfo As ForumKeywordInfo In arrKeywords
                    KeywordHash.Add(objKeywordInfo.Token, objKeywordInfo.Description)
                Next

                'DataCache.SetCache(strCacheKey, KeywordHash)
                Return KeywordHash
                'Return CType(DataCache.GetCache(strCacheKey), Hashtable)
            Else
                Return Nothing
            End If
            'End If
        End Function

#End Region

    End Class

#End Region

End Namespace