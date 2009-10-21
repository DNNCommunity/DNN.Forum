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

#Region "EmailQueueTaskEmails Info/Controller"

#Region "EmailQueueTaskEmailsInfo"

    ''' <summary>
    ''' The EmailQueueTaskEmailsInfo represents an email queue task email item
    ''' </summary>
    ''' <remarks>One row of data from the data store represents a single outgoing email.
    ''' </remarks>
    ''' <history>
    ''' </history>
    Public Class EmailQueueTaskEmailsInfo

#Region "Private Members"

        Private _EmailQueueId As Integer
        Private _EmailAddress As String
        Private _IsHTML As Boolean
        Private _DisplayName As String
        Private _IsComplete As Boolean
        Private _DateAdded As Date
        Private _DateComplete As Date
        Private _Failed As Boolean

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Instantiates an instance of the class.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' The task this is linked to (FK) 
        ''' </summary>
        ''' <value></value>
        ''' <returns>The interger EmailQueueID.</returns>
        ''' <remarks>Each outgoing email in the table is matched w/ an EmailQueueID.</remarks>
        Public Property EmailQueueId() As Integer
            Get
                Return _EmailQueueId
            End Get
            Set(ByVal Value As Integer)
                _EmailQueueId = Value
            End Set
        End Property

        ''' <summary>
        ''' Send To email address
        ''' </summary>
        ''' <value></value>
        ''' <returns>The string email address to send the email to.</returns>
        ''' <remarks>This will always be joe@domain.com.</remarks>
        Public Property EmailAddress() As String
            Get
                Return _EmailAddress
            End Get
            Set(ByVal Value As String)
                _EmailAddress = Value
            End Set
        End Property

        ''' <summary>
        ''' If the email will go out as text/html
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if the outgoing email is HTML format, false if it is text.</returns>
        ''' <remarks>This is a user profile item represented by 0/1.</remarks>
        Public Property IsHTML() As Boolean
            Get
                Return _IsHTML
            End Get
            Set(ByVal Value As Boolean)
                _IsHTML = Value
            End Set
        End Property

        ''' <summary>
        ''' Send To display name
        ''' </summary>
        ''' <value></value>
        ''' <returns>The friendly name to use on the outgoing email.</returns>
        ''' <remarks>Core user profile's display name.</remarks>
        Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal Value As String)
                _DisplayName = Value
            End Set
        End Property

        ''' <summary>
        ''' If the email send was completed
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if the email send is complete, false otherwise.</returns>
        ''' <remarks>Only marked as complete if send success or failed.</remarks>
        Public Property IsComplete() As Boolean
            Get
                Return _IsComplete
            End Get
            Set(ByVal Value As Boolean)
                _IsComplete = Value
            End Set
        End Property

        ''' <summary>
        ''' Date the record was added to the TaskEmails table
        ''' </summary>
        ''' <value></value>
        ''' <returns>The date the email was added to the queue for sending.</returns>
        ''' <remarks>Stored in the task emails table.</remarks>
        Public Property DateAdded() As Date
            Get
                Return _DateAdded
            End Get
            Set(ByVal Value As Date)
                _DateAdded = Value
            End Set
        End Property

        ''' <summary>
        ''' The date the send was successfull/failed
        ''' </summary>
        ''' <value></value>
        ''' <returns>The date the send attempt occured.</returns>
        ''' <remarks>Updated regardless of success/failure.</remarks>
        Public Property DateComplete() As Date
            Get
                Return _DateComplete
            End Get
            Set(ByVal Value As Date)
                _DateComplete = Value
            End Set
        End Property

        ''' <summary>
        ''' If the email send was successfull
        ''' </summary>
        ''' <value></value>
        ''' <returns>True if the email send failed in the task, false otherwise.</returns>
        ''' <remarks>False by default.</remarks>
        Public Property Failed() As Boolean
            Get
                Return _Failed
            End Get
            Set(ByVal Value As Boolean)
                _Failed = Value
            End Set
        End Property

#End Region

    End Class

#End Region

#Region "EmailQueueTaskEmailsController"

    ''' <summary>
    ''' Interacts w/ the data store for the outgoing emails for a task.
    ''' </summary>
    ''' <remarks>Only called from the email queue task.</remarks>
    Public Class EmailQueueTaskEmailsController

#Region "Private Methods"

        ''' <summary>
        ''' Determines if a queued task is completed.
        ''' </summary>
        ''' <param name="EmailQueueID">The ID to check for in the data store.</param>
        ''' <returns>True if there are emails remaning for the specified EmailQueueID.</returns>
        ''' <remarks>Checks data store for items not marked as completed for the EmailQueueID.</remarks>
        Public Function TaskEmailsGetIncomplete(ByVal EmailQueueID As Integer) As Boolean
            Dim ContinueSend As Boolean = DotNetNuke.Modules.Forum.DataProvider.Instance().TaskEmailsGetIncomplete(EmailQueueID)
            Return ContinueSend
        End Function

        ''' <summary>
        ''' Retrieves a collection of emails to send.
        ''' </summary>
        ''' <param name="EmailQueueID">The task to look for.</param>
        ''' <returns>A collection of emails to send from the email queue task.</returns>
        ''' <remarks>Only retrieves emails not marked as complete.</remarks>
        Public Function TaskEmailsToSendGet(ByVal EmailQueueID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().TaskEmailsToSendGet(EmailQueueID), GetType(EmailQueueTaskEmailsInfo))
        End Function

        ''' <summary>
        ''' Updates the success/failure of an outgoing email that was assigned as part of a task.
        ''' </summary>
        ''' <param name="EmailQueueID">The task ID to update the status of.</param>
        ''' <param name="EmailAddress">The email address to update the success/failure of.</param>
        ''' <param name="Failed">True if the send failed, false otherwise.</param>
        ''' <remarks>The table has every outgoing user seperately for auditing purposes.</remarks>
        Public Sub TaskEmailsSendStatusUpdate(ByVal EmailQueueID As Integer, ByVal EmailAddress As String, ByVal Failed As Boolean)
            DataProvider.Instance().TaskEmailsSendStatusUpdate(EmailQueueID, EmailAddress, Failed)
        End Sub

#End Region

    End Class

#End Region

#End Region

End Namespace
