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

#Region " StartEmailQueuedTasks"

	''' <summary>
	''' This is the Email Queue Task handler. It is designed to allow email notifications to be
	''' sent using a background thread and to not worry about the ASP.NET default limitation of 25
	''' threads since only one will be spawned to handle this.
	''' This is all methods necessary to use the core Scheduler for tasks. 
	''' </summary>
	''' <remarks>The task actually sends the emails which were queued up using the IEmailQueueable interface.
	''' </remarks>
	Public Class StartEmailQueuedTasks
		Inherits DotNetNuke.Services.Scheduling.SchedulerClient

		''' <summary>
		''' Creates a new instance of a history item (Constructor)
		''' </summary>
		''' <param name="objScheduleHistoryItem">The task which will need to have its history updated.</param>
		''' <remarks>Core part, used for auditing tasks.
		''' </remarks>
		Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
			MyBase.new()
			Me.ScheduleHistoryItem = objScheduleHistoryItem
		End Sub

		''' <summary>
		''' Logs information to the schedule history
		''' </summary>
		''' <remarks>Responsible for sending the emails which were added to the queue via the IEmailQueueable inteface.
		''' </remarks>
		Public Overrides Sub DoWork()
			'CP - COMEBACK - There needs to be some check here to see if the scheduler is running this task in another thread
			' the problem is that it is possible for two items to create the same schedule event, since we loop through. Attempting to remove looping of DoWork.

			Dim objEmailQueueTaskCnt As New EmailQueueTaskController
			Dim objEmailQueueTaskInfo As New EmailQueueTaskInfo
			Try
				'notification that the event is progressing
				Progressing()	  'OPTIONAL
				' First, we may need to check to see if there is already a queue sending (schedular may ensure this doesn't happen, check on that)

				' Get the next task (we check to make sure task did not fail in sproc)
				objEmailQueueTaskInfo = GetQueuedTask()

				If Not objEmailQueueTaskInfo Is Nothing Then
					' check to see if there are any emails in our TaskEmails table for this EmailQueueID(this would only happen if a send didn't complete while executing)
					Dim objTaskEmailsCnt As New EmailQueueTaskEmailsController
					Dim ContinueSend As Boolean = False

					ContinueSend = objTaskEmailsCnt.TaskEmailsGetIncomplete(objEmailQueueTaskInfo.EmailQueueId)

					If ContinueSend Then
						' If we need to contine, we need to finish sending this before loading another batch (add some type of error test here?) - We do not need to populate TaskEmails table

						' mark the current task as failed (we will add more here, but for now this means we will send once, if it fails we will attempt one final time to deliver reamining)
						' this is controlled in GetNextTask sproc where it does TaskFailedCount < 2
						objEmailQueueTaskCnt.EmailQueueTaskMarkFailed(objEmailQueueTaskInfo.EmailQueueId)
					Else
						' if not, we can load another batch
						' pass the info object to retrieve the list of emails and put them in the TaskEmails table
						PopulateTaskEmailsTable(objEmailQueueTaskInfo)
						' set the queuestarted datetime here
						objEmailQueueTaskCnt.EmailQueueTaskStart(objEmailQueueTaskInfo.EmailQueueId)
					End If
					' At this point, we should have tasks in TaskEmails table to send (seperation above is to determine only if we have to load the taskemails table, rest below is same)
					' Retrieve an ArrayList from TaskEmails table to pass to a send function
					Dim arrEmailsToSend As List(Of EmailQueueTaskEmailsInfo)
					arrEmailsToSend = objTaskEmailsCnt.TaskEmailsToSendGet(objEmailQueueTaskInfo.EmailQueueId)

					' there should be no reason we have no emails to send if we got this far
					If arrEmailsToSend.Count > 0 Then
						' Attempt to send
						Dim TotalSentCount As Integer = 0
						TotalSentCount = SendQueuedEmails(objEmailQueueTaskInfo, arrEmailsToSend)

						' finish up this task in schedular
						ScheduleHistoryItem.Succeeded = True	 'REQUIRED
						ScheduleHistoryItem.AddLogNote("Email Queue Task Completed. Emails Sent: " & TotalSentCount)

						' CP - COMEBACK - cycle and look for another
						'DoWork()
					Else
						' The sproc returned nothing which means no emails to send
						' This can happen for one of two reasons, there is just nobody to send to 
						' OR the stored procedure is getting a bad element for the parameter thus not getting any results. 
						' We need to mark this task as complete and insert 0 as the SentCount. We also need to check for more tasks after this.
						Dim objEmailTaskCnt As New EmailQueueTaskController
						objEmailTaskCnt.EmailQueueTaskCompleted(objEmailQueueTaskInfo.EmailQueueId)

						ScheduleHistoryItem.Succeeded = True	 'REQUIRED
						ScheduleHistoryItem.AddLogNote("Email Queue Task Completed: No Emails Returned To Send")

						' CP - COMEBACK - cycle and look for another
						'DoWork()
					End If
				Else
					' The info object is nothing, which means no emails to send
					ScheduleHistoryItem.Succeeded = True	 'REQUIRED
					ScheduleHistoryItem.AddLogNote("No Current Tasks To Schedule")


				End If
			Catch exc As Exception	 'REQUIRED
				' Mark the task as failed in our queue_tasks table
				objEmailQueueTaskCnt.EmailQueueTaskMarkFailed(objEmailQueueTaskInfo.EmailQueueId)

				ScheduleHistoryItem.Succeeded = False	 'REQUIRED
				ScheduleHistoryItem.AddLogNote("Email Queue Task Initialization Failed. " + exc.ToString)	   'OPTIONAL
				'notification that we have errored
				Errored(exc)	 'REQUIRED
				'log the exception
				LogException(exc)	 'OPTIONAL
			End Try
		End Sub

		''' <summary>
		''' Retrieves a single queued email task for sending. 
		''' The logic in the stored procedure determines which to retrieve.
		''' This is handled by WHERE ScheduleStart Date less than GetDate() ORDER BY EmailQueuePriority, DateCreated
		''' </summary>
		''' <remarks>Returns the outgoing email queue task.
		''' </remarks>
		Private Function GetQueuedTask() As EmailQueueTaskInfo
			Dim objEmailQueueTaskCnt As New EmailQueueTaskController

			Return objEmailQueueTaskCnt.EmailQueueTaskGetNext()
		End Function

		''' <summary>
		''' This gets the email addresses and all important related info and passes to populate the EmailSendQueue table.
		''' </summary>
		''' <param name="objEmailQueueInfo">The email task information we need to create a distribution list.</param>
		''' <remarks>The inline SQL sending part has not been completed.</remarks>
		Private Sub PopulateTaskEmailsTable(ByVal objEmailQueueInfo As EmailQueueTaskInfo)
			Dim sprocName As String
			Dim params As String
			If objEmailQueueInfo.DistroIsSproc Then
				' we are going to use the developers sproc and params
				sprocName = objEmailQueueInfo.DistroCall
				params = objEmailQueueInfo.DistroParams
				DataProvider.Instance().TaskEmailsSprocInsertToSend(sprocName, params, objEmailQueueInfo.EmailQueueId)
			Else
				' its raw sql
			End If
		End Sub

		''' <summary>
		''' Assigns basic SMTP settings exposed by DNN core (Host Settings)
		''' Then loops through each email, sends it and marks in db as each is sent (or failed send)
		''' At the end of this function we mark the task as completed.
		''' </summary>
		''' <param name="objTaskInfo">The email task being sent.</param>
		''' <param name="colEmailsToSend">A collection of emails to send.</param>
		''' <remarks>Uses copy of core send method, slightly enhanced.</remarks>
		Private Function SendQueuedEmails(ByVal objTaskInfo As EmailQueueTaskInfo, ByVal colEmailsToSend As List(Of EmailQueueTaskEmailsInfo)) As Integer
			' SMTP server configuration
			Dim SMTPServer As String = String.Empty
			Dim SMTPAuthentication As String = String.Empty
			Dim SMTPUsername As String = String.Empty
			Dim SMTPPassword As String = String.Empty
			Dim SMTPEnableSSL As Boolean = False

			If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPServer")) <> String.Empty Then
				SMTPServer = Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPServer"))
			End If

			If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPAuthentication")) <> String.Empty Then
				SMTPAuthentication = Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPAuthentication"))
			End If

			If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPUsername")) <> String.Empty Then
				SMTPUsername = Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPUsername"))
			End If

			If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPPassword")) <> String.Empty Then
				SMTPPassword = Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPPassword"))
			End If

			If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPEnableSSL")) <> String.Empty Then
				If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("SMTPEnableSSL")) = "Y" Then
					SMTPEnableSSL = True
				End If
			End If

			' external SMTP server alternate port
			Dim SmtpPort As Integer = Null.NullInteger
			Dim portPos As Integer = SMTPServer.IndexOf(":")
			If portPos > -1 Then
				SmtpPort = Int32.Parse(SMTPServer.Substring(portPos + 1, SMTPServer.Length - portPos - 1))
				SMTPServer = SMTPServer.Substring(0, portPos)
			End If

			Dim Client As New Net.Mail.SmtpClient()
			If SMTPServer <> String.Empty Then
				Client.Host = SMTPServer
				If SmtpPort > Null.NullInteger Then
					Client.Port = SmtpPort
				End If
				Select Case SMTPAuthentication
					Case "", "0" ' anonymous
					Case "1" ' basic
						If SMTPUsername <> String.Empty And SMTPPassword <> String.Empty Then
							Client.UseDefaultCredentials = False
							Client.Credentials = New System.Net.NetworkCredential(SMTPUsername, SMTPPassword)
						End If
					Case "2" ' NTLM
						Client.UseDefaultCredentials = True
				End Select
			End If
			Client.EnableSsl = SMTPEnableSSL

			' Set the standard task items (non user specific)
			Dim Sender As Net.Mail.MailAddress
			If objTaskInfo.EmailFromAddress <> String.Empty Then
				Sender = New Net.Mail.MailAddress(objTaskInfo.EmailFromAddress, objTaskInfo.FromFriendlyName)
			Else
				Sender = New Net.Mail.MailAddress(objTaskInfo.EmailFromAddress)
			End If

			Dim TotalSuccessCount As Integer = 0
			Dim TotalFailedCount As Integer = 0

			Dim objTaskEmailsCnt As New EmailQueueTaskEmailsController
			' loop through each entry in the arraylist and send the mail
			For Each objEmailToSend As EmailQueueTaskEmailsInfo In colEmailsToSend
				' user specific
				Dim endDelimit As String
				'Use either vbCrLF or <br><br> depending on BodyFormat.
				Dim Body As String
				If objEmailToSend.IsHTML Then
					Body = objTaskInfo.EmailHTMLBody
					endDelimit = "<br />"
				Else
					Body = objTaskInfo.EmailTextBody
					endDelimit = vbCrLf
				End If

				Dim MailTo As Net.Mail.MailAddress
				If objTaskInfo.EnableFriendlyToName Then
					MailTo = New Net.Mail.MailAddress(objEmailToSend.EmailAddress, objEmailToSend.DisplayName)
				Else
					MailTo = New Net.Mail.MailAddress(objEmailToSend.EmailAddress)
				End If
				'End user specific

				Dim mailMessage As New System.Net.Mail.MailMessage(Sender, MailTo)
				mailMessage.Priority = CType(objTaskInfo.EmailPriority, Net.Mail.MailPriority)
				mailMessage.Subject = HtmlUtils.StripWhiteSpace(objTaskInfo.EmailSubject, True)
				mailMessage.Body = Body
				mailMessage.IsBodyHtml = objEmailToSend.IsHTML
				mailMessage.BodyEncoding = System.Text.Encoding.UTF8

				Try
					' Send the mail message
					Client.Send(mailMessage)
					'SendMail = String.Empty

					' if we are here, the mail sent (mark message as sent successfull)
					TotalSuccessCount += 1
					objTaskEmailsCnt.TaskEmailsSendStatusUpdate(objEmailToSend.EmailQueueId, objEmailToSend.EmailAddress, False)
				Catch objException As Exception
					' mail configuration problem
					If Not IsNothing(objException.InnerException) Then
						'SendMail = String.Concat(objException.Message, ControlChars.CrLf, objException.InnerException.Message)
						LogException(objException.InnerException)
					Else
						LogException(objException)
					End If
					' mark the single email as failed
					TotalFailedCount += 1
					objTaskEmailsCnt.TaskEmailsSendStatusUpdate(objEmailToSend.EmailQueueId, objEmailToSend.EmailAddress, True)
				End Try

				' '' Add Base Href for any images inserted in to the email.
				''mailMessage.Body = "<Base Href='" & portalAlias & "'>"

				' kill the message
				mailMessage.Dispose()

			Next
			' Mark task complete in tasks table (assign current date in sproc as complete time, also use sproc to set failedcount, successcount, totalsentcount and if task failed)
			Dim objEmailTaskCnt As New EmailQueueTaskController
			objEmailTaskCnt.EmailQueueTaskCompleted(objTaskInfo.EmailQueueId)

			Return TotalSuccessCount + TotalFailedCount
		End Function

	End Class

#End Region

End Namespace