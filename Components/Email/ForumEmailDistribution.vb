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

#Region "ForumEmailDistribution"

#Region "ForumEmailDistributionInfo"

    ''' <summary>
    ''' Used for determing who to send an email to.
    ''' </summary>
    ''' <remarks>This is used by IEmailQueueable and non-queued sends.
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	8/27/2006	Created
    ''' </history>
    Public Class ForumEmailDistributionInfo

#Region "Private Declarations"

        Private _Email As String
        Private _EmailFormat As Boolean
        Private _DisplayName As String

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Email Address of recipient
        ''' </summary>
        ''' <value></value>
        ''' <returns>The string email address of the email recipient.</returns>
        ''' <remarks></remarks>
        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal Value As String)
                _Email = Value
            End Set
        End Property

        ''' <summary>
        ''' Email format to send recipient's email (HTML or text).
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if HTML, false otherwise.</returns>
        ''' <remarks>Tied to Enumerator.</remarks>
        Public Property EmailFormat() As Boolean
            Get
                Return _EmailFormat
            End Get
            Set(ByVal Value As Boolean)
                _EmailFormat = Value
            End Set
        End Property

        ''' <summary>
        ''' DisplayName of recipient of the outgoing email.
        ''' </summary>
        ''' <value></value>
        ''' <returns>The string display name of the email recipient.</returns>
        ''' <remarks></remarks>
        Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal Value As String)
                _DisplayName = Value
            End Set
        End Property

#End Region

    End Class

#End Region

#Region "ForumEmailDistributionController"

    ''' <summary>
    ''' Communicates w/ the data store to retrieve a distribution arraylist of recipients for an outgoing email.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ForumEmailDistributionController

        ''' <summary>
        ''' Retrieves a list of email recipients from the data store.
        ''' </summary>
        ''' <param name="SprocName">The stored procedure name used to retrieve the email recipients.</param>
        ''' <param name="params">The parameters to be used in the query to determine who should receive the outgoing email.</param>
        ''' <param name="EmailQueueID"></param>
        ''' <returns>An arraylist of email addresses and other details for an outgoing email send.</returns>
        ''' <remarks></remarks>
        Public Function GetEmailsToSend(ByVal SprocName As String, ByVal params As String, ByVal EmailQueueID As Integer) As List(Of ForumEmailDistributionInfo)
            Return CBO.FillCollection(Of ForumEmailDistributionInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().TaskEmailsSprocInsertToSend(SprocName, params, EmailQueueID))
        End Function

    End Class

#End Region

#End Region

End Namespace
