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

#Region "EmailQueueTaskInfo"

	''' <summary>
	''' The EmailItemInfo represents an Email Queue Task info Item
	''' </summary>
	''' <remarks>This should only be called by IEmailQueueable or its scheduled tasks.
	''' </remarks>
	''' <history>
	''' </history>
	Public Class EmailQueueTaskInfo
		Inherits System.Net.Mail.MailMessage

#Region "Enum"

		''' <summary>
		''' This determines the priority of the sending queue items. 
		''' Logic is used in the sproc to retrieve what task to queue and send next based on the combination of priority here and the date created along w/ scheduled start date. 
		''' </summary>
		''' <remarks>Currently, all sends are Normal (2)</remarks>
		Public Enum EnumEmailQueueTaskPriority
			QueueTaskPriorityHigh = 1
			QueueTaskPriorityNormal = 2
			QueueTaskPriorityLow = 3
		End Enum

#End Region

#Region "Private Members"

		Private _EmailQueueId As Integer
		Private _EmailFromAddress As String
		Private _FromFriendlyName As String
		Private _EmailPriority As Integer
		Private _EmailHTMLBody As String
		Private _EmailTextBody As String
		Private _EmailSubject As String
		Private _PortalID As Integer
		Private _QueuePriority As Integer
		Private _SendComplete As Boolean
		Private _QueueAddedDate As Date
		Private _QueueStartedDate As Date
		Private _QueueCompletedDate As Date
		Private _ModuleID As Integer
		Private _SentEmailCount As Integer
		Private _EnableFriendlyToName As Boolean
		Private _DistroCall As String
		Private _DistroIsSproc As Boolean
		Private _DistroParams As String
		Private _SuccessfullSendCount As Integer
		Private _FailedSendCount As Integer
		Private _TaskFailed As Boolean
		Private _TaskFailedCount As Integer
		Private _TotalRecords As Integer
		' reserved for later usage
		Private _ScheduleStartDate As Date
		Private _PersonalizeEmail As Boolean
		Private _Attachment As String

#End Region

#Region "Constructors"

		''' <summary>
		''' Instantiate an instance of the class.
		''' </summary>
		''' <remarks>Not used directly.</remarks>
		Public Sub New()
		End Sub

		''' <summary>
		''' Instantiates an instance of the class.
		''' </summary>
		''' <param name="EmailFromAddress">The email address seen as from.</param>
		''' <param name="FromFriendlyName">The full name of whom the email should send from.</param>
		''' <param name="EmailPriority">The priority of the outgoing email.</param>
		''' <param name="EmailHTMLBody">The HTML body of the outgoing email.</param>
		''' <param name="EmailTextBody">The text body of the outgoing email.</param>
		''' <param name="EmailSubject">The subject of the outgoing email.</param>
		''' <param name="PortalID">The portalID the email will be sent from.</param>
		''' <param name="QueuePriority"></param>
		''' <param name="ModuleID">The ModuleID which sent the email.</param>
		''' <param name="EnableFriendlyToName">If true, the full name of the receiver will be shown as the from name.</param>
		''' <param name="DistroCall">The name of the stored procedure to call, or the query to use for distribution calls.</param>
		''' <param name="DistroIsSproc">True if the DistroCall is the name of a stored procedure, otherwise it is inline SQL.</param>
		''' <param name="DistroParams">The parameters to pass to the stored procedure (if necessary)</param>
		''' <param name="ScheduleStartDate">When the email should be ready to be sent.</param>
		''' <param name="PersonalizeEmail">True if the email going out should use tokens. (not impelemented)</param>
		''' <param name="Attachment">The path to the file to attach to the outgoing email. (not implemented)</param>
		''' <remarks>Includes stubs for items not yet used.</remarks>
		Public Sub New(ByVal EmailFromAddress As String, ByVal FromFriendlyName As String, ByVal EmailPriority As Integer, ByVal EmailHTMLBody As String, ByVal EmailTextBody As String, ByVal EmailSubject As String, ByVal PortalID As Integer, ByVal QueuePriority As Integer, ByVal ModuleID As Integer, ByVal EnableFriendlyToName As Boolean, ByVal DistroCall As String, ByVal DistroIsSproc As Boolean, ByVal DistroParams As String, ByVal ScheduleStartDate As Date, ByVal PersonalizeEmail As Boolean, ByVal Attachment As String)
			Dim objEmailQueueCnt As New EmailQueueTaskController
			objEmailQueueCnt.EmailQueueTaskAdd(EmailFromAddress, FromFriendlyName, EmailPriority, EmailHTMLBody, EmailTextBody, EmailSubject, PortalID, QueuePriority, ModuleID, EnableFriendlyToName, DistroCall, DistroIsSproc, DistroParams, ScheduleStartDate, PersonalizeEmail, Attachment)
		End Sub

#End Region

#Region "Properties"

		''' <summary>
		''' The primary key
		''' </summary>
		''' <value></value>
		''' <returns>An integer representing the primary key.</returns>
		''' <remarks>Necessary for every outgoing email as well.</remarks>
		Public Property EmailQueueId() As Integer
			Get
				Return _EmailQueueId
			End Get
			Set(ByVal Value As Integer)
				_EmailQueueId = Value
			End Set
		End Property

		''' <summary>
		''' Send from email address 
		''' </summary>
		''' <value></value>
		''' <returns>A string that is an email address.</returns>
		''' <remarks>ie. jdoe@domain.com</remarks>
		Public Property EmailFromAddress() As String
			Get
				Return _EmailFromAddress
			End Get
			Set(ByVal Value As String)
				_EmailFromAddress = Value
			End Set
		End Property

		''' <summary>
		''' Send from friendly name
		''' </summary>
		''' <value></value>
		''' <returns>A string representing a full name</returns>
		''' <remarks>ie. John Doe</remarks>
		Public Property FromFriendlyName() As String
			Get
				Return _FromFriendlyName
			End Get
			Set(ByVal Value As String)
				_FromFriendlyName = Value
			End Set
		End Property

		''' <summary>
		''' The email priority of outgoing emails. 
		''' </summary>
		''' <value></value>
		''' <returns>An integer representing the priority of the email.</returns>
		''' <remarks>Normal, High, Low. Uses an Enum</remarks>
		Public Property EmailPriority() As Integer
			Get
				Return _EmailPriority
			End Get
			Set(ByVal Value As Integer)
				_EmailPriority = Value
			End Set
		End Property

		''' <summary>
		''' The HTML value for the email body.
		''' </summary>
		''' <value></value>
		''' <returns>The string containing the HTML body of the outgoing email.</returns>
		''' <remarks>Already formated properly.</remarks>
		Public Property EmailHTMLBody() As String
			Get
				Return _EmailHTMLBody
			End Get
			Set(ByVal Value As String)
				_EmailHTMLBody = Value
			End Set
		End Property

		''' <summary>
		''' The Text value for the email body.
		''' </summary>
		''' <value></value>
		''' <returns>The text formatted body for the outgoing email.</returns>
		''' <remarks>Already parsed for HTML items.</remarks>
		Public Property EmailTextBody() As String
			Get
				Return _EmailTextBody
			End Get
			Set(ByVal Value As String)
				_EmailTextBody = Value
			End Set
		End Property

		''' <summary>
		''' Subject of outgoing emails
		''' </summary>
		''' <value></value>
		''' <returns>The subject of the outgoing email.</returns>
		''' <remarks>Already parsed for tokens.</remarks>
		Public Property EmailSubject() As String
			Get
				Return _EmailSubject
			End Get
			Set(ByVal Value As String)
				_EmailSubject = Value
			End Set
		End Property

		''' <summary>
		''' The portalID that created this task.
		''' </summary>
		''' <value></value>
		''' <returns>The PortalID for the outgoing email.</returns>
		''' <remarks>This is necessary for obtaining user profile data.</remarks>
		Public Property PortalID() As Integer
			Get
				Return _PortalID
			End Get
			Set(ByVal Value As Integer)
				_PortalID = Value
			End Set
		End Property

		''' <summary>
		''' Determines how this will be placed in the queue for sending order.
		''' </summary>
		''' <value></value>
		''' <returns>The integer value representing the Queue priority.</returns>
		''' <remarks>Based on Enumerator: EnumEmailQueueTaskPriority</remarks>
		Public Property QueuePriority() As Integer
			Get
				Return _QueuePriority
			End Get
			Set(ByVal Value As Integer)
				_QueuePriority = Value
			End Set
		End Property

		''' <summary>
		''' If the task completed sending or not.
		''' </summary>
		''' <value></value>
		''' <returns>True if the send has been completed, false otherwise.</returns>
		''' <remarks>This is only true once the queue has completed a task.</remarks>
		Public Property SendComplete() As Boolean
			Get
				Return _SendComplete
			End Get
			Set(ByVal Value As Boolean)
				_SendComplete = Value
			End Set
		End Property

		''' <summary>
		''' Date a task was added to the queue
		''' </summary>
		''' <value></value>
		''' <returns>The date the task was added to the queue.</returns>
		''' <remarks>This is when it was added to be queued.</remarks>
		Public Property QueueAddedDate() As Date
			Get
				Return _QueueAddedDate
			End Get
			Set(ByVal Value As Date)
				_QueueAddedDate = Value
			End Set
		End Property

		''' <summary>
		''' Date the queue started sending emails.
		''' </summary>
		''' <value></value>
		''' <returns>The date the queue item should start.</returns>
		''' <remarks>Currently this is not used in logic.</remarks>
		Public Property QueueStartedDate() As Date
			Get
				Return _QueueStartedDate
			End Get
			Set(ByVal Value As Date)
				_QueueStartedDate = Value
			End Set
		End Property

		''' <summary>
		''' Date the task completed sending all emails
		''' </summary>
		''' <value></value>
		''' <returns>The date the queue task was completed.</returns>
		''' <remarks>Null if the task is not complete.</remarks>
		Public Property QueueCompletedDate() As Date
			Get
				Return _QueueCompletedDate
			End Get
			Set(ByVal Value As Date)
				_QueueCompletedDate = Value
			End Set
		End Property

		''' <summary>
		''' The moduleID that assigned this to the queue
		''' </summary>
		''' <value></value>
		''' <returns>The ModuleID the outgoing email was sent from.</returns>
		''' <remarks>This is needed for obtaining data about what is sending the email.</remarks>
		Public Property ModuleID() As Integer
			Get
				Return _ModuleID
			End Get
			Set(ByVal Value As Integer)
				_ModuleID = Value
			End Set
		End Property

		''' <summary>
		''' Number of emails sent for a task
		''' </summary>
		''' <value></value>
		''' <returns>The number of emails sent.</returns>
		''' <remarks>This is the actual number of who was sent to in total.(Can increase if task is not complete)</remarks>
		Public Property SentEmailCount() As Integer
			Get
				Return _SentEmailCount
			End Get
			Set(ByVal Value As Integer)
				_SentEmailCount = Value
			End Set
		End Property

		''' <summary>
		''' If the outgoing emails should be in format of John Doe vs. simply jdoe@domain.com.
		''' </summary>
		''' <value></value>
		''' <returns>True if the outgoing email should use a friendly name.</returns>
		''' <remarks>This is like John Doe</remarks>
		Public Property EnableFriendlyToName() As Boolean
			Get
				Return _EnableFriendlyToName
			End Get
			Set(ByVal Value As Boolean)
				_EnableFriendlyToName = Value
			End Set
		End Property

		''' <summary>
		''' The sproc w/ parameters or the full queury to populate the distribution list
		''' </summary>
		''' <value></value>
		''' <returns>Returns the stored procedure name for the outgoing email, otherwise it is inline SQL</returns>
		''' <remarks>Currently only using sprocs.</remarks>
		Public Property DistroCall() As String
			Get
				Return _DistroCall
			End Get
			Set(ByVal Value As String)
				_DistroCall = Value
			End Set
		End Property

		''' <summary>
		''' If the DistroCall is a query or a stored procedure
		''' </summary>
		''' <value></value>
		''' <returns>True if the DistroCall is a stored procedure name, false if it is inline SQL.</returns>
		''' <remarks>Only using sprocs at this time.</remarks>
		Public Property DistroIsSproc() As Boolean
			Get
				Return _DistroIsSproc
			End Get
			Set(ByVal Value As Boolean)
				_DistroIsSproc = Value
			End Set
		End Property

		''' <summary>
		''' The parameters to pass to the stored procedure (DistroCall) if DistroIsSproc is True
		''' </summary>
		''' <value></value>
		''' <returns>The parameters for the stored procedure.</returns>
		''' <remarks>Empty string if using inline SQL.</remarks>
		Public Property DistroParams() As String
			Get
				Return _DistroParams
			End Get
			Set(ByVal Value As String)
				_DistroParams = Value
			End Set
		End Property

		''' <summary>
		''' Date to start place email in Queue for sending.
		''' </summary>
		''' <value></value>
		''' <returns>The date the queued item should start sending.</returns>
		''' <remarks>Not Implemented</remarks>
		Public Property ScheduleStartDate() As Date
			Get
				Return _ScheduleStartDate
			End Get
			Set(ByVal Value As Date)
				_ScheduleStartDate = Value
			End Set
		End Property

		''' <summary>
		''' Determines if an email should be parsed per outgoing user
		''' and have keywords rendered.
		''' </summary>
		''' <value></value>
		''' <returns>True if the email should be personalized via tokens.</returns>
		''' <remarks>Not Implemented</remarks>
		Public Property PersonalizeEmail() As Boolean
			Get
				Return _PersonalizeEmail
			End Get
			Set(ByVal Value As Boolean)
				_PersonalizeEmail = Value
			End Set
		End Property

		''' <summary>
		''' Number of emails that successfully sent for a task
		''' </summary>
		''' <value></value>
		''' <returns>Total number of emails sent for the task w/out failure.</returns>
		''' <remarks>Can increase till send is complete.</remarks>
		Public Property SuccessfullSendCount() As Integer
			Get
				Return _SuccessfullSendCount
			End Get
			Set(ByVal Value As Integer)
				_SuccessfullSendCount = Value
			End Set
		End Property

		''' <summary>
		''' Number of failed sends for a specific task
		''' </summary>
		''' <value></value>
		''' <returns>The number of failed sends of the task outgoing emails.</returns>
		''' <remarks>Can increase until Send is complete.</remarks>
		Public Property FailedSendCount() As Integer
			Get
				Return _FailedSendCount
			End Get
			Set(ByVal Value As Integer)
				_FailedSendCount = Value
			End Set
		End Property

		''' <summary>
		''' Attachment to be distributed w/ email
		''' </summary>
		''' <value></value>
		''' <returns>The path to the file to attach to the outgoing email.</returns>
		''' <remarks>Not Implemented</remarks>
		Public Property Attachment() As String
			Get
				Return _Attachment
			End Get
			Set(ByVal Value As String)
				_Attachment = Value
			End Set
		End Property

		''' <summary>
		''' If a send fails and it never 'completes' it needs to be marked as such to avoid attempting to send again.
		''' </summary>
		''' <value></value>
		''' <returns>True if the task failed, false otherwise.</returns>
		''' <remarks>Remains false if task is not complete.</remarks>
		Public Property TaskFailed() As Boolean
			Get
				Return _TaskFailed
			End Get
			Set(ByVal Value As Boolean)
				_TaskFailed = Value
			End Set
		End Property

		''' <summary>
		''' The number of times a task has failed
		''' </summary>
		''' <value></value>
		''' <returns>The number of times the task has failed.</returns>
		''' <remarks>This gets increased from 0 only if a task was loaded and never finished, so think of it like an index int value</remarks>
		Public Property TaskFailedCount() As Integer
			Get
				Return _TaskFailedCount
			End Get
			Set(ByVal Value As Integer)
				_TaskFailedCount = Value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal Value As Integer)
				_TotalRecords = Value
			End Set
		End Property

#End Region

	End Class

#End Region

#Region "EmailQueueTaskController"

	''' <summary>
	''' Permits access to the data store for EmailQueueTask items.
	''' </summary>
	''' <remarks>Should only be necessary via IEmailQueueable.</remarks>
	Public Class EmailQueueTaskController

#Region "Public Methods"

		''' <summary>
		''' Used to add an email queue task to the data store.
		''' </summary>
		''' <param name="EmailFromAddress">The email address seen as from.</param>
		''' <param name="FromFriendlyName">The full name of whom the email should send from.</param>
		''' <param name="EmailPriority">The priority of the outgoing email.</param>
		''' <param name="EmailHTMLBody">The HTML body of the outgoing email.</param>
		''' <param name="EmailTextBody">The text body of the outgoing email.</param>
		''' <param name="EmailSubject">The subject of the outgoing email.</param>
		''' <param name="PortalID">The portalID the email will be sent from.</param>
		''' <param name="QueuePriority"></param>
		''' <param name="ModuleID">The ModuleID which sent the email.</param>
		''' <param name="EnableFriendlyToName">If true, the full name of the receiver will be shown as the from name.</param>
		''' <param name="DistroCall">The name of the stored procedure to call, or the query to use for distribution calls.</param>
		''' <param name="DistroIsSproc">True if the DistroCall is the name of a stored procedure, otherwise it is inline SQL.</param>
		''' <param name="DistroParams">The parameters to pass to the stored procedure (if necessary)</param>
		''' <param name="ScheduleStartDate">When the email should be ready to be sent.</param>
		''' <param name="PersonalizeEmail">True if the email going out should use tokens. (not impelemented)</param>
		''' <param name="Attachment">The path to the file to attach to the outgoing email. (not implemented)</param>
		''' <returns>An integer representing the primary key of the Queue Task.</returns>
		''' <remarks></remarks>
		Public Function EmailQueueTaskAdd(ByVal EmailFromAddress As String, ByVal FromFriendlyName As String, ByVal EmailPriority As Integer, ByVal EmailHTMLBody As String, ByVal EmailTextBody As String, ByVal EmailSubject As String, ByVal PortalID As Integer, ByVal QueuePriority As Integer, ByVal ModuleID As Integer, ByVal EnableFriendlyToName As Boolean, ByVal DistroCall As String, ByVal DistroIsSproc As Boolean, ByVal DistroParams As String, ByVal ScheduleStartDate As Date, ByVal PersonalizeEmail As Boolean, ByVal Attachment As String) As Integer
			Return CType(DataProvider.Instance().EmailQueueTaskAdd(EmailFromAddress, FromFriendlyName, EmailPriority, EmailHTMLBody, EmailTextBody, EmailSubject, PortalID, QueuePriority, ModuleID, EnableFriendlyToName, DistroCall, DistroIsSproc, DistroParams, ScheduleStartDate, PersonalizeEmail, Attachment), Integer)
		End Function

		''' <summary>
		''' Gets a single email queue task from the data store.
		''' </summary>
		''' <param name="EmailQueueID">The primary key value to look for in the data store.</param>
		''' <returns>The populated info object w/ one row of data.</returns>
		''' <remarks>Only called from the email queue task.</remarks>
		Public Function EmailQueueTaskGet(ByVal EmailQueueID As Integer) As EmailQueueTaskInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().EmailQueueTaskGet(EmailQueueID), GetType(EmailQueueTaskInfo)), EmailQueueTaskInfo)
		End Function

		''' <summary>
		''' Gets the next email queue task to send.
		''' </summary>
		''' <returns>The info object of the next email queued task to send.</returns>
		''' <remarks>Only called from the email queue task.</remarks>
		Public Function EmailQueueTaskGetNext() As EmailQueueTaskInfo
			Return CType(CBO.FillObject(DotNetNuke.Modules.Forum.DataProvider.Instance().EmailQueueTaskGetNext(), GetType(EmailQueueTaskInfo)), EmailQueueTaskInfo)
		End Function

		''' <summary>
		''' Marks an email queue task as complete.
		''' </summary>
		''' <param name="EmailQueueID">The primary key to lookup in the table.</param>
		''' <remarks>Only called from the email queue task.</remarks>
		Public Sub EmailQueueTaskCompleted(ByVal EmailQueueID As Integer)
			DataProvider.Instance().EmailQueueTaskCompleted(EmailQueueID)
		End Sub

		''' <summary>
		''' Marks an email queue task as failed, by increasing an integer value.
		''' </summary>
		''' <param name="EmailQueueID">The primiary key to matchup w/ in the data store.</param>
		''' <remarks>This is called form the email queue task. Will not send after 2 failures.</remarks>
		Public Sub EmailQueueTaskMarkFailed(ByVal EmailQueueID As Integer)
			DataProvider.Instance().EmailQueueTaskMarkFailed(EmailQueueID)
		End Sub

		''' <summary>
		''' Starts the email queued task.
		''' </summary>
		''' <param name="EmailQueueID">The primarly key to matchup in the data store.</param>
		''' <remarks>Start populates the emails to send to table.</remarks>
		Public Sub EmailQueueTaskStart(ByVal EmailQueueID As Integer)
			DataProvider.Instance().EmailQueueTaskStart(EmailQueueID)
		End Sub

		''' <summary>
		''' Deletes all send to emails from the email queue emails table.
		''' </summary>
		''' <param name="DeleteDate">The date to look for to delete all previous task emails.</param>
		''' <remarks>It is easier to use a scheduled task to delete emails by date rather than PK. This is called from the clean email queue task.</remarks>
		Public Sub EmailQueueTaskCleanEmails(ByVal DeleteDate As Date)
			DataProvider.Instance().EmailQueueTaskCleanEmails(DeleteDate)
		End Sub

		''' <summary>
		''' Deletes all previous email tasks from the data store. 
		''' </summary>
		''' <param name="DeleteDate">The date to delete the scheduled tasks in the queue.</param>
		''' <remarks>Only called from the clean scheduled task for the queue.</remarks>
		Public Sub EmailQueueTaskCleanTasks(ByVal DeleteDate As Date)
			DataProvider.Instance().EmailQueueTaskCleanTasks(DeleteDate)
		End Sub

		''' <summary>
		''' Retrieves all email tasks associated with a portal.
		''' </summary>
		''' <param name="PortalID">The Portal to retrieve email tasks for.</param>
		''' <returns>A colleciton of email tasks associated with the specified portal.</returns>
		''' <remarks></remarks>
		Public Function GetPortalEmailSendTasks(ByVal PortalID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As List(Of EmailQueueTaskInfo)
			Return CBO.FillCollection(Of EmailQueueTaskInfo)(DotNetNuke.Modules.Forum.DataProvider.Instance().GetPortalEmailSendTasks(PortalID, PageIndex, PageSize))
		End Function

#End Region

	End Class

#End Region

End Namespace