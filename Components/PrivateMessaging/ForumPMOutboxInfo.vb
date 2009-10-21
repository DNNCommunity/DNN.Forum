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

#Region " PMOutboxInfo "

    ''' <summary>
    ''' Contains the list of Personal Messages still not read by the recipient
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PMOutboxInfo

#Region " Private Members "

        Private _PMID As Integer
        Private _Subject As String
        Private _CreatedDate As Date
        Private _PMToUserId As Integer
        Private _PMThreadId As Integer

#End Region

#Region " Public Properties "
        ''' <summary>
        ''' ID of PM 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PMID() As Integer
            Get
                Return _PMID
            End Get
            Set(ByVal Value As Integer)
                _PMID = Value
            End Set
        End Property
        ''' <summary>
        ''' PM Subject
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Subject() As String
            Get
                Return _Subject
            End Get
            Set(ByVal Value As String)
                _Subject = Value
            End Set
        End Property
        ''' <summary>
        ''' PM CreatedDate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As Date)
                _CreatedDate = Value
            End Set
        End Property
        ''' <summary>
        ''' PM Recipient UserID
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PMToUserId() As Integer
            Get
                Return _PMToUserId
            End Get
            Set(ByVal Value As Integer)
                _PMToUserId = Value
            End Set
        End Property
        ''' <summary>
        ''' PM ThreadId
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PMThreadId() As Integer
            Get
                Return _PMThreadId
            End Get
            Set(ByVal Value As Integer)
                _PMThreadId = Value
            End Set
        End Property

#End Region

    End Class
#End Region

End Namespace