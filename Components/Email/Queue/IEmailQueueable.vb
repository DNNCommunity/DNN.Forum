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

#Region "IEmailQueueable"

    ''' <summary>
    ''' Interface used for consumption of modules wishing to schedule emails sends.
    ''' </summary>
    ''' <remarks>Implemented to mimic IPortable/ISearchable type of interfaces.</remarks>
    Public Interface IEmailQueueable

        ''' <summary>
        ''' Used to queue one email for sending to a distribution list.
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
        ''' <returns>The EmailQueueTaskInfo object that was just queued by this function.</returns>
        ''' <remarks>This is called from modules wishing to use the interface.</remarks>
        Function QueueEmail(ByVal EmailFromAddress As String, ByVal FromFriendlyName As String, ByVal EmailPriority As Integer, ByVal EmailHTMLBody As String, ByVal EmailTextBody As String, ByVal EmailSubject As String, ByVal PortalID As Integer, ByVal QueuePriority As Integer, ByVal ModuleID As Integer, ByVal EnableFriendlyToName As Boolean, ByVal DistroCall As String, ByVal DistroIsSproc As Boolean, ByVal DistroParams As String, ByVal ScheduleStartDate As Date, ByVal PersonalizeEmail As Boolean, ByVal Attachment As String) As EmailQueueTaskInfo

    End Interface

#End Region

End Namespace