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

#Region "ThreadRateInfo"

    ''' <summary>
    ''' All properties associated with the Forum_ThreadRating table in the data store. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ThreadRateInfo

#Region "Private Members"

        Private _ThreadID As Integer
        Private _UserID As Integer
        Private _Rate As Integer
        Private _Comment As String
        Private _RatedByAlias As String

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The ThreadID associated with the thread rating.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ThreadID() As Integer
            Get
                Return _ThreadID
            End Get
            Set(ByVal Value As Integer)
                _ThreadID = Value
            End Set
        End Property

        ''' <summary>
        ''' The UserID associated with the thread rating.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal Value As Integer)
                _UserID = Value
            End Set
        End Property

        ''' <summary>
        ''' The rating integer value associated with the thread rating.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Rate() As Integer
            Get
                Return _Rate
            End Get
            Set(ByVal Value As Integer)
                _Rate = Value
            End Set
        End Property

        ''' <summary>
        ''' The image associated with the thread rating.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property RatingImage() As String
            Get
                Return "stars_" & (_Rate * 10).ToString & ".gif"
            End Get
        End Property

#End Region

    End Class

#End Region

End Namespace