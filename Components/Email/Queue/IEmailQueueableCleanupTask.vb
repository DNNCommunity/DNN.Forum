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

#Region " CleanupEmailQueuedTasks"

    ''' <summary>
    ''' This is the Email Queue Task handler. It is designed to allow email notifications to be
    ''' sent using a background thread and to not worry about the ASP.NET default limitation of 25
    ''' threads since only one will be spawned to handle this.
    ''' This is all methods necessary to use the core Scheduler for tasks. 
    ''' </summary>
    ''' <remarks>Run by the DNN scheduler only. NOT FULLY TESTED.
    ''' </remarks>
    Public Class CleanupEmailQueuedTasks
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
        ''' <remarks>This task cleans the emails which the IEmailQueueSendTask has already sent.
        ''' </remarks>
        Public Overrides Sub DoWork()
            Dim objEmailQueueTaskCnt As New EmailQueueTaskController
            Try
                'notification that the event is progressing
                Progressing()    'OPTIONAL

                Dim tasksDeleteDate As Date = Date.Now().AddDays(-30)
				If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("ForumTaskDeleteDays")) <> String.Empty Then
					Dim DaysToDelete As Integer
					DaysToDelete = Convert.ToInt32(Entities.Host.Host.GetHostSettingsDictionary("ForumTaskDeleteDays"))
					tasksDeleteDate = Date.Now.AddDays(-DaysToDelete)
				End If

				Dim emailsDeleteDate As Date = Date.Now().AddDays(-30)
				If Convert.ToString(Entities.Host.Host.GetHostSettingsDictionary("ForumEmailDeleteDays")) <> String.Empty Then
					Dim DaysToDelete As Integer
					DaysToDelete = Convert.ToInt32(Entities.Host.Host.GetHostSettingsDictionary("ForumEmailDeleteDays"))
					emailsDeleteDate = Date.Now.AddDays(-DaysToDelete)
				End If

                ' add logic to delete tasks and emails (based on hostsettings)
                objEmailQueueTaskCnt.EmailQueueTaskCleanTasks(tasksDeleteDate)
                objEmailQueueTaskCnt.EmailQueueTaskCleanEmails(emailsDeleteDate)

                ' finish up this task in schedular
                ScheduleHistoryItem.Succeeded = True     'REQUIRED
                ScheduleHistoryItem.AddLogNote("Email Queue Cleanup Completed.")

            Catch exc As Exception    'REQUIRED
                ScheduleHistoryItem.Succeeded = False    'REQUIRED
                ScheduleHistoryItem.AddLogNote("Email Queue Cleanup Initialization Failed. " + exc.ToString)    'OPTIONAL
                'notification that we have errored
                Errored(exc)    'REQUIRED
                'log the exception
                LogException(exc)    'OPTIONAL
            End Try
        End Sub

    End Class

#End Region

End Namespace