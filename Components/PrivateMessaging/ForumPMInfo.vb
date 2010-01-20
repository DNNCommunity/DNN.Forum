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

#Region "PMInfo"

    ''' <summary>
    ''' Represents a single row of data for the Forum_PMPosts table.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	1/01/2007	Created
    ''' </history>
    Public Class PMInfo

#Region "Private Members"

        Private _PMID As Integer
        Private _ParentPMId As Integer
		Private _RemoteAddr As String = String.Empty
		Private _Subject As String = String.Empty
		Private _Body As String = String.Empty
        Private _CreatedDate As DateTime
        Private _PMThreadID As Integer
        Private _Parent As PMThreadInfo
        Private _PMFromUserID As Integer
        Private _PMToUserID As Integer
        Private _TotalRecords As Integer

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Instantiates the class.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

#End Region

#Region "Public ReadOnly Properties"

        ''' <summary>
        ''' PortalID is necessary so we can load proper user information. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PortalID() As Integer
            Get
                Dim mPortalID As Integer = -1
                If HttpContext.Current.Request.IsAuthenticated Then
                    PortalID = Users.UserController.GetCurrentUserInfo.PortalID
                End If
                Return mPortalID
            End Get
        End Property

        ''' <summary>
        ''' Gets the PM Thread info object. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Parent() As PMThreadInfo
            Get
                If _Parent Is Nothing Then
                    _Parent = PMThreadInfo.GetPMThreadInfo(PMThreadID)
                End If

                Return _Parent
            End Get
        End Property

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The ID of the private Message (PK)
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
        ''' The Parent PMID of the private message.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParentPMID() As Integer
            Get
                Return _ParentPMId
            End Get
            Set(ByVal Value As Integer)
                _ParentPMId = Value
            End Set
        End Property

        ''' <summary>
        ''' The UserID who the sending user of this private message.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PMFromUserID() As Integer
            Get
                Return _PMFromUserID
            End Get
            Set(ByVal Value As Integer)
                _PMFromUserID = Value
            End Set
        End Property

        ''' <summary>
        ''' The UserID of the receiving user of this private message. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PMToUserID() As Integer
            Get
                Return _PMToUserID
            End Get
            Set(ByVal Value As Integer)
                _PMToUserID = Value
            End Set
        End Property

        ''' <summary>
        ''' The IP Address of the user who sent the private message. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property RemoteAddr() As String
            Get
                Return _RemoteAddr
            End Get
            Set(ByVal Value As String)
                _RemoteAddr = Value
            End Set
        End Property

        ''' <summary>
        ''' The subject of the private message. 
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
        ''' The HTML Body of the private message. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Body() As String
            Get
                Return _Body
            End Get
            Set(ByVal Value As String)
                _Body = Value
            End Set
        End Property

        ''' <summary>
        ''' The date the private message was created. 
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
        ''' The PMThreadID this private message belongs to. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>If it is the first PM in the thread, it should be itself. </remarks>
        Public Property PMThreadID() As Integer
            Get
                Return _PMThreadID
            End Get
            Set(ByVal Value As Integer)
                _PMThreadID = Value
            End Set
        End Property

        Public Property TotalRecords() As Integer
            Get
                Return _TotalRecords
            End Get
            Set(ByVal value As Integer)
                _TotalRecords = value
            End Set
        End Property

#End Region

    End Class

#End Region

End Namespace