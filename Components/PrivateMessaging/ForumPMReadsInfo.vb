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

#Region "ForumPMReadsInfo"

    ''' <summary>
    ''' A Forum Private Message Read Info object is used to determine if a user has read a PM or not. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ForumPMReadsInfo

#Region "Private Members"

        Dim mUserID As Integer
        Dim mPMThreadID As Integer
        Dim mLastVisitDate As Date
        Dim mPortalID As Integer

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Instantiates the class
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Instantiates the class
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="PMThreadID"></param>
        ''' <param name="LastVisitDate"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        Public Sub New(ByVal UserID As Integer, ByVal PMThreadID As Integer, ByVal LastVisitDate As Date)
            Me.UserID = UserID
            Me.PMThreadID = PMThreadID
            Me.LastVisitDate = LastVisitDate
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The userID of whom we are tracking. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserID() As Integer
            Get
                Return mUserID
            End Get
            Set(ByVal Value As Integer)
                mUserID = Value
            End Set
        End Property

        ''' <summary>
        ''' The ThreadID we are tracking. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PMThreadID() As Integer
            Get
                Return mPMThreadID
            End Get
            Set(ByVal Value As Integer)
                mPMThreadID = Value
            End Set
        End Property

        ''' <summary>
        ''' The last visit date to the thread for the user. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastVisitDate() As Date
            Get
                Return mLastVisitDate
            End Get
            Set(ByVal Value As Date)
                mLastVisitDate = Value
            End Set
        End Property

        ''' <summary>
        ''' The PM system is Portal specific. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PortalID() As Integer
            Get
                Return mPortalID
            End Get
            Set(ByVal Value As Integer)
                mPortalID = Value
            End Set
        End Property

#End Region

    End Class

#End Region

End Namespace

