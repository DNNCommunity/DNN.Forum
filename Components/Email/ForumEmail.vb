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

#Region "ForumEmail"

	''' <summary>
	''' Constructs an outgoing email for all types of email communication. 
	''' </summary>
	''' <remarks>The is used for tasks or emails that are not being tasked.
	''' </remarks>
	''' <history>
	'''  [cpaterra] 8/27/2006 Heavily modified to use new email template system.
	''' </history>
	Public Class ForumEmail
		Inherits EmailQueueTaskInfo

#Region "Private Declarations"

		Private _DistroContentID As Integer

#End Region

#Region "Public Properties"

		''' <summary>
		''' DistroContentID represents the threadID, postID, forumID (depending on usage) needed to retrieve email details and the distribution list.
		''' </summary>
		''' <value></value>
		''' <returns>The PK used to retrieve who should get the email.</returns>
		''' <remarks>Provides flexibility by leaving as a generic property.</remarks>
		Public Property DistroContentID() As Integer
			Get
				Return _DistroContentID
			End Get
			Set(ByVal Value As Integer)
				_DistroContentID = Value
			End Set
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Instantiates a new instance of the class.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			MyBase.New()
		End Sub

		''' <summary>
		''' Builds everything neccessary for an outgoing email.
		''' </summary>
		''' <param name="Notes">String containing notes that may need to replace a token.</param>
		''' <param name="emailType">The type of email going out, based on an Enum.</param>
		''' <param name="ContentID">The PK used to retrieve email data (based on emailType) for the outgoing email.</param>
		''' <param name="objConfig">The forum module's configuration.</param>
		''' <remarks>Each active emailType requires a new variable in the case statement.
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	8/27/2006	Created
		''' </history>
		Public Sub GenerateEmail(ByVal Notes As String, ByVal emailType As ForumEmailType, ByVal ContentID As Integer, ByVal objConfig As Forum.Config, ByVal URL As String, ByVal ProfileURL As String, ByVal PortalID As Integer)
			Dim ContentType As ForumContentTypeID
			Dim ForumEmailTypeID As Integer
			Dim Keywords As New Hashtable
			Dim SetKeywords As New Hashtable

			Select Case emailType
				Case ForumEmailType.UserPostAdded
					ContentType = ForumContentTypeID.POST
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.UserPostAdded

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.ThreadID

					Dim cntForum As New ForumController
					Dim objForum As New ForumInfo
					objForum = cntForum.GetForumInfoCache(objPostInfo.ForumID)

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					' this sproc goes against Tracked Threads table and tracked forums tables
					DistroCall = "Forum_Subscriptions_NewThread"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True

					If objConfig.EnablePerForumFrom And Not (objForum.EmailAddress.Trim = String.Empty) Then
						EmailFromAddress = objForum.EmailAddress.Trim
					Else
						EmailFromAddress = objConfig.AutomatedEmailAddress
					End If

					If objConfig.EnablePerForumFrom And Not (objForum.EmailFriendlyFrom.Trim = String.Empty) Then
						FromFriendlyName = objForum.EmailFriendlyFrom.Trim
					Else
						FromFriendlyName = objConfig.EmailAddressDisplayName
					End If
				Case ForumEmailType.UserNewThread
					ContentType = ForumContentTypeID.POST
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.UserNewThread

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.ThreadID

					Dim cntForum As New ForumController
					Dim objForum As New ForumInfo
					objForum = cntForum.GetForumInfoCache(objPostInfo.ForumID)

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					' this sproc goes against Tracked Threads table and tracked forums tables
					DistroCall = "Forum_Subscriptions_NewThread"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True

					If objConfig.EnablePerForumFrom And Not (objForum.EmailAddress.Trim = String.Empty) Then
						EmailFromAddress = objForum.EmailAddress.Trim
					Else
						EmailFromAddress = objConfig.AutomatedEmailAddress
					End If

					If objConfig.EnablePerForumFrom And Not (objForum.EmailFriendlyFrom.Trim = String.Empty) Then
						FromFriendlyName = objForum.EmailFriendlyFrom.Trim
					Else
						FromFriendlyName = objConfig.EmailAddressDisplayName
					End If
				Case ForumEmailType.UserPostEdited
					ContentType = ForumContentTypeID.POST
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.UserPostEdited

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.ThreadID

					Dim cntForum As New ForumController
					Dim objForum As New ForumInfo
					objForum = cntForum.GetForumInfoCache(objPostInfo.ForumID)

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					DistroCall = "Forum_Subscriptions_NewThread"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True

					If objConfig.EnablePerForumFrom And Not (objForum.EmailAddress.Trim = String.Empty) Then
						EmailFromAddress = objForum.EmailAddress.Trim
					Else
						EmailFromAddress = objConfig.AutomatedEmailAddress
					End If

					If objConfig.EnablePerForumFrom And Not (objForum.EmailFriendlyFrom.Trim = String.Empty) Then
						FromFriendlyName = objForum.EmailFriendlyFrom.Trim
					Else
						FromFriendlyName = objConfig.EmailAddressDisplayName
					End If
				Case ForumEmailType.UserPostDeleted
					ContentType = ForumContentTypeID.DELETEPOST
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.UserPostDeleted

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.PostID

					Dim cntForum As New ForumController
					Dim objForum As New ForumInfo
					objForum = cntForum.GetForumInfoCache(objPostInfo.ForumID)

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					DistroCall = "Forum_Subscriptions_PostDeleted"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True

					If objConfig.EnablePerForumFrom And Not (objForum.EmailAddress.Trim = String.Empty) Then
						EmailFromAddress = objForum.EmailAddress.Trim
					Else
						EmailFromAddress = objConfig.AutomatedEmailAddress
					End If

					If objConfig.EnablePerForumFrom And Not (objForum.EmailFriendlyFrom.Trim = String.Empty) Then
						FromFriendlyName = objForum.EmailFriendlyFrom.Trim
					Else
						FromFriendlyName = objConfig.EmailAddressDisplayName
					End If
				Case ForumEmailType.ModeratorPostDeleted
					ContentType = ForumContentTypeID.MODERATORCOMMUNICATION
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.ModeratorPostDeleted

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.ForumID

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					DistroCall = "Forum_Subscriptions_ModPostDeleted"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True
					EmailFromAddress = objConfig.AutomatedEmailAddress
					FromFriendlyName = objConfig.EmailAddressDisplayName
				Case ForumEmailType.ModeratorPostToModerate
					ContentType = ForumContentTypeID.MODERATORCOMMUNICATION
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.ModeratorPostToModerate

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.PostID

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					' Since Post To Moderate and Post To Delete Mods are same for forum post is in, use same distro sproc to determine who receives.
					DistroCall = "Forum_Subscriptions_ModPostToModerate"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True
					EmailFromAddress = objConfig.AutomatedEmailAddress
					FromFriendlyName = objConfig.EmailAddressDisplayName
				Case ForumEmailType.ModeratorPostAbuse
					' uses notes
					ContentType = ForumContentTypeID.MODERATORCOMMUNICATION
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.ModeratorPostAbuse

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.PostID

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					DistroCall = "Forum_Subscriptions_ModPostAbuse"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True
					EmailFromAddress = objConfig.AutomatedEmailAddress
					FromFriendlyName = objConfig.EmailAddressDisplayName
				Case ForumEmailType.UserThreadMoved
					ContentType = ForumContentTypeID.MODERATORCOMMUNICATION
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.UserThreadMoved

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.ThreadID

					Dim cntForum As New ForumController
					Dim objForum As New ForumInfo
					objForum = cntForum.GetForumInfoCache(objPostInfo.ForumID)

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					DistroCall = "Forum_Subscriptions_NewThread"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True

					If objConfig.EnablePerForumFrom And Not (objForum.EmailAddress.Trim = String.Empty) Then
						EmailFromAddress = objForum.EmailAddress.Trim
					Else
						EmailFromAddress = objConfig.AutomatedEmailAddress
					End If

					If objConfig.EnablePerForumFrom And Not (objForum.EmailFriendlyFrom.Trim = String.Empty) Then
						FromFriendlyName = objForum.EmailFriendlyFrom.Trim
					Else
						FromFriendlyName = objConfig.EmailAddressDisplayName
					End If
				Case ForumEmailType.UserThreadSplit
					ContentType = ForumContentTypeID.MODERATORCOMMUNICATION
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.UserThreadSplit

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.ThreadID

					Dim cntForum As New ForumController
					Dim objForum As New ForumInfo
					objForum = cntForum.GetForumInfoCache(objPostInfo.ForumID)

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					DistroCall = "Forum_Subscriptions_NewThread"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True

					If objConfig.EnablePerForumFrom And Not (objForum.EmailAddress.Trim = String.Empty) Then
						EmailFromAddress = objForum.EmailAddress.Trim
					Else
						EmailFromAddress = objConfig.AutomatedEmailAddress
					End If

					If objConfig.EnablePerForumFrom And Not (objForum.EmailFriendlyFrom.Trim = String.Empty) Then
						FromFriendlyName = objForum.EmailFriendlyFrom.Trim
					Else
						FromFriendlyName = objConfig.EmailAddressDisplayName
					End If
				Case ForumEmailType.UserPMReceived
					ContentType = ForumContentTypeID.PRIVATEMESSAGE
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.UserPMReceived

					Dim ctlPM As New PMController
					Dim objPMInfo As New PMInfo
					objPMInfo = ctlPM.PMGet(ContentID)
					DistroContentID = objPMInfo.PMID

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take the hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPMInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					DistroCall = "Forum_Subscriptions_PrivateMessage"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True
					EmailFromAddress = objConfig.AutomatedEmailAddress
					FromFriendlyName = objConfig.EmailAddressDisplayName
				Case ForumEmailType.UserPostApproved
					ContentType = ForumContentTypeID.POST
					Priority = System.Net.Mail.MailPriority.Normal
					ForumEmailTypeID = ForumEmailType.UserPostApproved

					' Use ContentID to get post info for post content types
					Dim objPostInfo As New PostInfo
					objPostInfo = PostInfo.GetPostInfo(ContentID, PortalID)
					DistroContentID = objPostInfo.PostID

					Dim cntForum As New ForumController
					Dim objForum As New ForumInfo
					objForum = cntForum.GetForumInfoCache(objPostInfo.ForumID)

					'Grab keywords based on content type, this is stored in cache
					Keywords = ForumKeywordInfo.GetKeywordsHash(ContentType)

					' We first have to pass to the keyword rendering. This is going to take hashtable, replaces its replacementvar's based on the email content type
					If Not Keywords Is Nothing Then
						SetKeywords = RenderKeywords(Keywords, objPostInfo, ContentType, URL, ProfileURL, Notes, objConfig.CurrentPortalSettings.ActiveTab.TabID)
					End If

					DistroCall = "Forum_Subscriptions_PostApproved"
					DistroParams = DistroContentID.ToString
					DistroIsSproc = True

					If objConfig.EnablePerForumFrom And Not (objForum.EmailAddress.Trim = String.Empty) Then
						EmailFromAddress = objForum.EmailAddress.Trim
					Else
						EmailFromAddress = objConfig.AutomatedEmailAddress
					End If

					If objConfig.EnablePerForumFrom And Not (objForum.EmailFriendlyFrom.Trim = String.Empty) Then
						FromFriendlyName = objForum.EmailFriendlyFrom.Trim
					Else
						FromFriendlyName = objConfig.EmailAddressDisplayName
					End If
				Case Else
					Exit Sub
			End Select

			'Get the templates here, pass to subject and body for replacement
			Dim objETempCnt As New ForumEmailTemplateController
			Dim objETempInfo As New ForumEmailTemplateInfo
			Dim arrTemplates As ArrayList
			arrTemplates = objETempCnt.GetEmailTemplatesByModuleID(objConfig.ModuleID)

			If arrTemplates.Count > 0 Then
				' get single template
				objETempInfo = objETempCnt.GetEmailTemplateForMail(objConfig.ModuleID, ForumEmailTypeID)
			Else
				' duplicate defaults
				Dim objEmailTemplateInfo As New ForumEmailTemplateInfo
				Dim arrDefaultTemplates As ArrayList
				'Get Default templates
				arrDefaultTemplates = objETempCnt.GetDefaultEmailTemplates()

				If arrDefaultTemplates.Count > 0 Then
					' for each default template, create one specific to this module
					For Each objEmailTemplateInfo In arrDefaultTemplates
						Dim NewTemplateInfo As New ForumEmailTemplateInfo
						NewTemplateInfo.EmailSubject = objEmailTemplateInfo.EmailSubject
						NewTemplateInfo.ForumTemplateTypeID = objEmailTemplateInfo.ForumTemplateTypeID
						NewTemplateInfo.HTMLBody = objEmailTemplateInfo.HTMLBody
						NewTemplateInfo.TextBody = objEmailTemplateInfo.TextBody
						NewTemplateInfo.ModuleID = objConfig.ModuleID
						NewTemplateInfo.IsActive = objEmailTemplateInfo.IsActive
						NewTemplateInfo.EmailTemplateName = objEmailTemplateInfo.EmailTemplateName
						NewTemplateInfo.ForumContentTypeID = objEmailTemplateInfo.ForumContentTypeID
						NewTemplateInfo.ForumEmailTypeID = objEmailTemplateInfo.ForumEmailTypeID

						objETempCnt.AddEmailTemplateForModuleID(NewTemplateInfo)
					Next
					' get single template
					objETempInfo = objETempCnt.GetEmailTemplateForMail(objConfig.ModuleID, ForumEmailTypeID)
				End If
			End If

			Subject = GenerateSubject(objETempInfo.EmailSubject, SetKeywords, ContentType)
			EmailHTMLBody = GenerateBody(Notes, ContentType, SetKeywords, objETempInfo.HTMLBody, objConfig, True)
			EmailTextBody = GenerateBody(Notes, ContentType, SetKeywords, objETempInfo.TextBody, objConfig, False)
			QueuePriority = EmailQueueTaskInfo.EnumEmailQueueTaskPriority.QueueTaskPriorityNormal
			EnableFriendlyToName = False
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Parsing done to replace tokens w/ actual data for email subject.
		''' </summary>
		''' <param name="SubjectToParse">The email subject to be parsed.</param>
		''' <param name="Keywords">The keywords hashtable, replaces a token w/ an actual value.</param>
		''' <param name="ContentType">The type of email content to parse for.</param>
		''' <returns>The parsed subject ready for sending.</returns>
		''' <remarks>May need slightly revised.</remarks>
		Private Function GenerateSubject(ByVal SubjectToParse As String, ByVal Keywords As Hashtable, ByVal ContentType As ForumContentTypeID) As String
			Dim FinalSubject As String = String.Empty

			Select Case ContentType
				Case ForumContentTypeID.POST
					If Keywords.Contains("[FORUMNAME]") Then
						FinalSubject = SubjectToParse.Replace("[FORUMNAME]", Keywords.Item("[FORUMNAME]").ToString)
					Else
						FinalSubject = SubjectToParse
					End If
					If Keywords.Contains("[POSTURL]") Then
						FinalSubject = FinalSubject.Replace("[POSTURL]", Keywords.Item("[POSTURL]").ToString)
					End If
					If Keywords.Contains("[GROUPNAME]") Then
						FinalSubject = FinalSubject.Replace("[GROUPNAME]", Keywords.Item("[GROUPNAME]").ToString)
					End If
					If Keywords.Contains("[DATEPOSTED]") Then
						FinalSubject = FinalSubject.Replace("[DATEPOSTED]", Keywords.Item("[DATEPOSTED]").ToString)
					End If
					If Keywords.Contains("[POSTAUTHOR]") Then
						FinalSubject = FinalSubject.Replace("[POSTAUTHOR]", Keywords.Item("[POSTAUTHOR]").ToString)
					End If
					If Keywords.Contains("[POSTSUBJECT]") Then
						FinalSubject = FinalSubject.Replace("[POSTSUBJECT]", Keywords.Item("[POSTSUBJECT]").ToString)
					End If
				Case ForumContentTypeID.PRIVATEMESSAGE
					FinalSubject = SubjectToParse
					If Keywords.Contains("[POSTURL]") Then
						FinalSubject = FinalSubject.Replace("[POSTURL]", Keywords.Item("[POSTURL]").ToString)
					End If
					If Keywords.Contains("[POSTSUBJECT]") Then
						FinalSubject = FinalSubject.Replace("[POSTSUBJECT]", Keywords.Item("[POSTSUBJECT]").ToString)
					End If
				Case ForumContentTypeID.MODERATORCOMMUNICATION
					If Keywords.Contains("[FORUMNAME]") Then
						FinalSubject = SubjectToParse.Replace("[FORUMNAME]", Keywords.Item("[FORUMNAME]").ToString)
					Else
						FinalSubject = SubjectToParse
					End If
					If Keywords.Contains("[POSTURL]") Then
						FinalSubject = FinalSubject.Replace("[POSTURL]", Keywords.Item("[POSTURL]").ToString)
					End If
					If Keywords.Contains("[GROUPNAME]") Then
						FinalSubject = FinalSubject.Replace("[GROUPNAME]", Keywords.Item("[GROUPNAME]").ToString)
					End If
					If Keywords.Contains("[DATEPOSTED]") Then
						FinalSubject = FinalSubject.Replace("[DATEPOSTED]", Keywords.Item("[DATEPOSTED]").ToString)
					End If
					If Keywords.Contains("[POSTAUTHOR]") Then
						FinalSubject = FinalSubject.Replace("[POSTAUTHOR]", Keywords.Item("[POSTAUTHOR]").ToString)
					End If
					If Keywords.Contains("[POSTSUBJECT]") Then
						FinalSubject = FinalSubject.Replace("[POSTSUBJECT]", Keywords.Item("[POSTSUBJECT]").ToString)
					End If
				Case ForumContentTypeID.DELETEPOST
					If Keywords.Contains("[FORUMNAME]") Then
						FinalSubject = SubjectToParse.Replace("[FORUMNAME]", Keywords.Item("[FORUMNAME]").ToString)
					Else
						FinalSubject = SubjectToParse
					End If
					If Keywords.Contains("[POSTURL]") Then
						FinalSubject = FinalSubject.Replace("[POSTURL]", Keywords.Item("[POSTURL]").ToString)
					End If
					If Keywords.Contains("[GROUPNAME]") Then
						FinalSubject = FinalSubject.Replace("[GROUPNAME]", Keywords.Item("[GROUPNAME]").ToString)
					End If
					If Keywords.Contains("[DATEPOSTED]") Then
						FinalSubject = FinalSubject.Replace("[DATEPOSTED]", Keywords.Item("[DATEPOSTED]").ToString)
					End If
					If Keywords.Contains("[POSTAUTHOR]") Then
						FinalSubject = FinalSubject.Replace("[POSTAUTHOR]", Keywords.Item("[POSTAUTHOR]").ToString)
					End If
					If Keywords.Contains("[POSTSUBJECT]") Then
						FinalSubject = FinalSubject.Replace("[POSTSUBJECT]", Keywords.Item("[POSTSUBJECT]").ToString)
					End If
			End Select

			FinalSubject = Utilities.ForumUtils.TrimString(FinalSubject, 46)
			Return FinalSubject
		End Function

		''' <summary>
		''' Builds the text/HTML body of the outgoing email.
		''' </summary>
		''' <param name="Notes">The notes are used to parse a specific notes token.</param>
		''' <param name="ContentType">The type of email being generated.</param>
		''' <param name="Keywords">A list of keywords to replace.</param>
		''' <param name="BodyToParse">The email body to parse.</param>
		''' <param name="objConfig">The forum module's configuration.</param>
		''' <param name="HTML">True, if the content should be HTML formatted.</param>
		''' <remarks></remarks>
		Private Function GenerateBody(ByVal Notes As String, ByVal ContentType As ForumContentTypeID, ByVal Keywords As Hashtable, ByVal BodyToParse As String, ByVal objConfig As Forum.Config, ByVal HTML As Boolean) As String
			Dim server As HttpServerUtility = HttpContext.Current.Server
			Dim msgBody As String = String.Empty
			Dim finalBody As String = String.Empty

			Select Case ContentType
				Case ForumContentTypeID.POST
					If Keywords.Contains("[FORUMNAME]") Then
						msgBody = BodyToParse.Replace("[FORUMNAME]", Keywords.Item("[FORUMNAME]").ToString)
					Else
						msgBody = BodyToParse
					End If
					If Keywords.Contains("[POSTURL]") Then
						msgBody = msgBody.Replace("[POSTURL]", Keywords.Item("[POSTURL]").ToString)
					End If
					If Keywords.Contains("[GROUPNAME]") Then
						msgBody = msgBody.Replace("[GROUPNAME]", Keywords.Item("[GROUPNAME]").ToString)
					End If
					If Keywords.Contains("[PROFILELINK]") Then
						msgBody = msgBody.Replace("[PROFILELINK]", Keywords.Item("[PROFILELINK]").ToString)
					End If
					If Keywords.Contains("[DATEPOSTED]") Then
						msgBody = msgBody.Replace("[DATEPOSTED]", Keywords.Item("[DATEPOSTED]").ToString)
					End If
					If Keywords.Contains("[POSTAUTHOR]") Then
						msgBody = msgBody.Replace("[POSTAUTHOR]", Keywords.Item("[POSTAUTHOR]").ToString)
					End If
					If Keywords.Contains("[POSTBODY]") Then
						msgBody = msgBody.Replace("[POSTBODY]", Keywords.Item("[POSTBODY]").ToString)
					End If
					If Keywords.Contains("[POSTSUBJECT]") Then
						msgBody = msgBody.Replace("[POSTSUBJECT]", Keywords.Item("[POSTSUBJECT]").ToString)
					End If
				Case ForumContentTypeID.PRIVATEMESSAGE
					msgBody = BodyToParse
					If Keywords.Contains("[POSTURL]") Then
						msgBody = msgBody.Replace("[POSTURL]", Keywords.Item("[POSTURL]").ToString)
					End If
					If Keywords.Contains("[PROFILELINK]") Then
						msgBody = msgBody.Replace("[PROFILELINK]", Keywords.Item("[PROFILELINK]").ToString)
					End If
					If Keywords.Contains("[POSTBODY]") Then
						msgBody = msgBody.Replace("[POSTBODY]", Keywords.Item("[POSTBODY]").ToString)
					End If
					If Keywords.Contains("[POSTSUBJECT]") Then
						msgBody = msgBody.Replace("[POSTSUBJECT]", Keywords.Item("[POSTSUBJECT]").ToString)
					End If
					If Keywords.Contains("[DATEPOSTED]") Then
						msgBody = msgBody.Replace("[DATEPOSTED]", Keywords.Item("[DATEPOSTED]").ToString)
					End If
					If Keywords.Contains("[POSTAUTHOR]") Then
						msgBody = msgBody.Replace("[POSTAUTHOR]", Keywords.Item("[POSTAUTHOR]").ToString)
					End If
				Case ForumContentTypeID.MODERATORCOMMUNICATION
					If Keywords.Contains("[FORUMNAME]") Then
						msgBody = BodyToParse.Replace("[FORUMNAME]", Keywords.Item("[FORUMNAME]").ToString)
					Else
						msgBody = BodyToParse
					End If
					If Keywords.Contains("[POSTURL]") Then
						msgBody = msgBody.Replace("[POSTURL]", Keywords.Item("[POSTURL]").ToString)
					End If
					If Keywords.Contains("[NOTES]") Then
						msgBody = msgBody.Replace("[NOTES]", Keywords.Item("[NOTES]").ToString)
					End If
					If Keywords.Contains("[GROUPNAME]") Then
						msgBody = msgBody.Replace("[GROUPNAME]", Keywords.Item("[GROUPNAME]").ToString)
					End If
					If Keywords.Contains("[PROFILELINK]") Then
						msgBody = msgBody.Replace("[PROFILELINK]", Keywords.Item("[PROFILELINK]").ToString)
					End If
					If Keywords.Contains("[DATEPOSTED]") Then
						msgBody = msgBody.Replace("[DATEPOSTED]", Keywords.Item("[DATEPOSTED]").ToString)
					End If
					If Keywords.Contains("[POSTAUTHOR]") Then
						msgBody = msgBody.Replace("[POSTAUTHOR]", Keywords.Item("[POSTAUTHOR]").ToString)
					End If
					If Keywords.Contains("[POSTBODY]") Then
						msgBody = msgBody.Replace("[POSTBODY]", Keywords.Item("[POSTBODY]").ToString)
					End If
					If Keywords.Contains("[POSTSUBJECT]") Then
						msgBody = msgBody.Replace("[POSTSUBJECT]", Keywords.Item("[POSTSUBJECT]").ToString)
					End If
				Case ForumContentTypeID.DELETEPOST
					If Keywords.Contains("[FORUMNAME]") Then
						msgBody = BodyToParse.Replace("[FORUMNAME]", Keywords.Item("[FORUMNAME]").ToString)
					Else
						msgBody = BodyToParse
					End If
					If Keywords.Contains("[POSTURL]") Then
						msgBody = msgBody.Replace("[POSTURL]", Keywords.Item("[POSTURL]").ToString)
					End If
					If Keywords.Contains("[NOTES]") Then
						msgBody = msgBody.Replace("[NOTES]", Keywords.Item("[NOTES]").ToString)
					End If
					If Keywords.Contains("[GROUPNAME]") Then
						msgBody = msgBody.Replace("[GROUPNAME]", Keywords.Item("[GROUPNAME]").ToString)
					End If
					If Keywords.Contains("[PROFILELINK]") Then
						msgBody = msgBody.Replace("[PROFILELINK]", Keywords.Item("[PROFILELINK]").ToString)
					End If
					If Keywords.Contains("[DATEPOSTED]") Then
						msgBody = msgBody.Replace("[DATEPOSTED]", Keywords.Item("[DATEPOSTED]").ToString)
					End If
					If Keywords.Contains("[POSTAUTHOR]") Then
						msgBody = msgBody.Replace("[POSTAUTHOR]", Keywords.Item("[POSTAUTHOR]").ToString)
					End If
					If Keywords.Contains("[POSTBODY]") Then
						msgBody = msgBody.Replace("[POSTBODY]", Keywords.Item("[POSTBODY]").ToString)
					End If
					If Keywords.Contains("[POSTSUBJECT]") Then
						msgBody = msgBody.Replace("[POSTSUBJECT]", Keywords.Item("[POSTSUBJECT]").ToString)
					End If
			End Select
			If HTML Then
				finalBody = FormatHTMLBody(msgBody, objConfig)
			Else
				finalBody = FormatTextBody(msgBody, objConfig)
			End If
			Return finalBody
		End Function

#Region "Formatting"

		''' <summary>
		''' Formats the outgoing HTML email body for viewing in an email client. 
		''' </summary>
		''' <param name="Body">The HTML email body to format.</param>
		''' <param name="objConfig">The forum module's configuration.</param>
		''' <returns>The formated HTML body for the outgoing emails.</returns>
		''' <remarks>Could be cleaned up.
		''' </remarks>
		Protected Function FormatHTMLBody(ByVal Body As Object, ByVal objConfig As Forum.Config) As String
			Dim formatedBody As String = String.Empty
			If Not IsDBNull(Body) Then
				Dim bodyForumText As Utilities.PostContent = New Utilities.PostContent(HttpContext.Current.Server.HtmlDecode(CType(Body, String)), objConfig)
				formatedBody = bodyForumText.ProcessHtml
				formatedBody = Utilities.ForumUtils.FormatEmailImage(formatedBody, objConfig.PrimaryAlias)
			End If
			Return formatedBody
		End Function

		''' <summary>
		''' Formats the outgoing text email body for viewing in a an email client
		''' </summary>
		''' <param name="Body">The text email body to format.</param>
		''' <param name="objConfig">The forum module's configuration.</param>
		''' <returns>The formated text email body.</returns>
		''' <remarks></remarks>
		Protected Function FormatTextBody(ByVal Body As Object, ByVal objConfig As Forum.Config) As String
			Dim formatedBody As String = String.Empty
			If Not IsDBNull(Body) Then
				Dim bodyForumText As Utilities.PostContent = New Utilities.PostContent(HttpContext.Current.Server.HtmlDecode(CType(Body, String)), objConfig)
				formatedBody = bodyForumText.ProcessPlainText(objConfig)
			End If
			Return formatedBody
		End Function

#End Region

		''' <summary>
		''' Replaces the values of the keyword hashtable based on content type. 
		''' </summary>
		''' <param name="Keywords">A hashtable of keywords to get values for.</param>
		''' <param name="objInfo">The info object representing the email's data object.</param>
		''' <param name="ContentType">The type of email content to get the tokens for.</param>
		''' <param name="PostUrl">A direct full url to a post, or a place the user can navigate to about the post.</param>
		''' <param name="ProfileUrl">A full url path to a user's profile. This is used for a specific token.</param>
		''' <returns>A hashtable of keywords and the values to replace them with.</returns>
		''' <remarks>Keywords are based on an enumerator of email content type.</remarks>
		Private Function RenderKeywords(ByVal Keywords As Hashtable, ByVal objInfo As Object, ByVal ContentType As ForumContentTypeID, ByVal PostUrl As String, ByVal ProfileUrl As String, ByVal Notes As String, ByVal TabID As Integer) As Hashtable

			Select Case ContentType
				Case ForumContentTypeID.POST
					Dim cntForum As New ForumController
					Dim objPost As PostInfo = CType(objInfo, PostInfo)
					'user -1 for userid, so we never get a value of IsTracked (important to not rely on userid specifically here)
					Dim objThread As ThreadInfo = ThreadInfo.GetThreadInfo(objPost.ThreadID)
					Dim objForum As ForumInfo = cntForum.GetForumInfoCache(objThread.ForumID)

					If Not Keywords Is Nothing Then
						If Keywords.ContainsKey("[FORUMNAME]") Then
							Keywords.Item("[FORUMNAME]") = objForum.Name
						End If

						If Keywords.ContainsKey("[POSTSUBJECT]") Then
							Keywords.Item("[POSTSUBJECT]") = objPost.Subject
						End If

						If Keywords.ContainsKey("[DATEPOSTED]") Then
							Keywords.Item("[DATEPOSTED]") = objPost.CreatedDate
						End If

						If Keywords.ContainsKey("[POSTAUTHOR]") Then
							Keywords.Item("[POSTAUTHOR]") = objPost.Author.SiteAlias
						End If

						If Keywords.ContainsKey("[POSTBODY]") Then
							Keywords.Item("[POSTBODY]") = objPost.Body
						End If

						If Keywords.ContainsKey("[POSTURL]") Then
							Keywords.Item("[POSTURL]") = PostUrl
						End If

						If Keywords.ContainsKey("[GROUPNAME]") Then
							Keywords.Item("[GROUPNAME]") = objForum.ParentGroup.Name
						End If

						If Keywords.ContainsKey("[PROFILELINK]") Then
							Keywords.Item("[PROFILELINK]") = ProfileUrl
						End If

					End If

				Case ForumContentTypeID.MODERATORCOMMUNICATION
					Dim cntForum As New ForumController
					Dim objPost As PostInfo = CType(objInfo, PostInfo)
					'user -1 for userid, so we never get a value of IsTracked (important to not rely on userid specifically here)
					Dim objThread As ThreadInfo = ThreadInfo.GetThreadInfo(objPost.ThreadID)
					Dim objForum As ForumInfo = cntForum.GetForumInfoCache(objThread.ForumID)

					If Not Keywords Is Nothing Then
						If Keywords.ContainsKey("[FORUMNAME]") Then
							Keywords.Item("[FORUMNAME]") = objForum.Name
						End If

						If Keywords.ContainsKey("[POSTSUBJECT]") Then
							Keywords.Item("[POSTSUBJECT]") = objPost.Subject
						End If

						If Keywords.ContainsKey("[DATEPOSTED]") Then
							Keywords.Item("[DATEPOSTED]") = objPost.CreatedDate
						End If

						If Keywords.ContainsKey("[POSTAUTHOR]") Then
							Keywords.Item("[POSTAUTHOR]") = objPost.Author.SiteAlias
						End If

						If Keywords.ContainsKey("[POSTBODY]") Then
							Keywords.Item("[POSTBODY]") = objPost.Body
						End If

						If Keywords.ContainsKey("[POSTBODY]") Then
							Keywords.Item("[POSTBODY]") = objPost.Body
						End If

						If Keywords.ContainsKey("[POSTURL]") Then
							Keywords.Item("[POSTURL]") = PostUrl
						End If

						If Keywords.ContainsKey("[GROUPNAME]") Then
							Keywords.Item("[GROUPNAME]") = objForum.ParentGroup.Name
						End If

						If Keywords.ContainsKey("[PROFILELINK]") Then
							Keywords.Item("[PROFILELINK]") = ProfileUrl
						End If

						If Keywords.ContainsKey("[NOTES]") Then
							Keywords.Item("[NOTES]") = Notes
						End If
					End If

				Case ForumContentTypeID.PRIVATEMESSAGE
					Dim objPMInfo As PMInfo = CType(objInfo, PMInfo)

					If Not Keywords Is Nothing Then
						If Keywords.ContainsKey("[POSTURL]") Then
							Keywords.Item("[POSTURL]") = PostUrl
						End If

						If Keywords.ContainsKey("[PROFILELINK]") Then
							Keywords.Item("[PROFILELINK]") = ProfileUrl
						End If

						If Keywords.ContainsKey("[POSTSUBJECT]") Then
							Keywords.Item("[POSTSUBJECT]") = objPMInfo.Subject
						End If

						If Keywords.ContainsKey("[DATEPOSTED]") Then
							Keywords.Item("[DATEPOSTED]") = objPMInfo.CreatedDate
						End If

						If Keywords.ContainsKey("[POSTAUTHOR]") Then
							Keywords.Item("[POSTAUTHOR]") = ForumUserController.GetForumUser(objPMInfo.PMFromUserID, False, ModuleID, PortalID).SiteAlias
						End If

						If Keywords.ContainsKey("[POSTBODY]") Then
							Keywords.Item("[POSTBODY]") = objPMInfo.Body
						End If
					End If

				Case ForumContentTypeID.DELETEPOST
					Dim cntForum As New ForumController
					Dim objPost As PostInfo = CType(objInfo, PostInfo)
					'user -1 for userid, so we never get a value of IsTracked (important to not rely on userid specifically here)
					Dim objThread As ThreadInfo = ThreadInfo.GetThreadInfo(objPost.ThreadID)
					Dim objForum As ForumInfo = cntForum.GetForumInfoCache(objThread.ForumID)

					If Not Keywords Is Nothing Then
						If Keywords.ContainsKey("[FORUMNAME]") Then
							Keywords.Item("[FORUMNAME]") = objForum.Name
						End If

						If Keywords.ContainsKey("[POSTSUBJECT]") Then
							Keywords.Item("[POSTSUBJECT]") = objPost.Subject
						End If

						If Keywords.ContainsKey("[DATEPOSTED]") Then
							Keywords.Item("[DATEPOSTED]") = objPost.CreatedDate
						End If

						If Keywords.ContainsKey("[POSTAUTHOR]") Then
							Keywords.Item("[POSTAUTHOR]") = objPost.Author.SiteAlias
						End If

						If Keywords.ContainsKey("[POSTBODY]") Then
							Keywords.Item("[POSTBODY]") = objPost.Body
						End If

						If Keywords.ContainsKey("[POSTURL]") Then
							Keywords.Item("[POSTURL]") = PostUrl
						End If

						If Keywords.ContainsKey("[GROUPNAME]") Then
							Keywords.Item("[GROUPNAME]") = objForum.ParentGroup.Name
						End If

						If Keywords.ContainsKey("[PROFILELINK]") Then
							Keywords.Item("[PROFILELINK]") = ProfileUrl
						End If

						If Keywords.ContainsKey("[NOTES]") Then
							Keywords.Item("[NOTES]") = Notes
						End If
					End If
			End Select

			Return Keywords
		End Function

#End Region

	End Class

#End Region

End Namespace