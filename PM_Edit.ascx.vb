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

	''' <summary>
	''' This is where all posts are added and/or edited from.  It also fires off 
	''' email notification and factors in moderation.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Partial Class PMAdd
		Inherits ForumModuleBase
		Implements Entities.Modules.IActionable

#Region "Private Members"

		Private mParentPMThreadID As Integer
		Private mParentPMThread As PMThreadInfo
		Private mParentPMID As Integer
		Private mParentPMInfo As PMInfo
		Private mParentPMAuthorID As Integer
		Private mParentPMAuthor As ForumUser
		Private mPMThreadPage As Integer
		Private mPMAction As PMAction = PMAction.[New]
		Private mPMRecipientUser As ForumUser
		Private mPMRecipientID As Integer
		Private mPMToEnabled As Boolean
		Private regCounter As Integer = 0

#End Region

#Region "Optional Interfaces"

		''' <summary>
		''' Gets a list of module actions available to the user to provide it to DNN core.
		''' </summary>
		''' <value></value>
		''' <returns>The collection of module actions available to the user</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
			Get
				Return Utilities.ForumUtils.PerUserModuleActions(objConfig, Me)
			End Get
		End Property

#End Region

#Region "Event Handlers"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
			'' Ajax
			'If DotNetNuke.Framework.AJAX.IsInstalled Then
			'	DotNetNuke.Framework.AJAX.RegisterScriptManager()
			'	DotNetNuke.Framework.AJAX.WrapUpdatePanelControl(pnlContainer, True)
			'	DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdSubmit)
			'	DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdPreview)
			'	DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdBackToEdit)
			'	DotNetNuke.Framework.AJAX.RegisterPostBackControl(teContent)
			'	'CP - test for lookup
			'	DotNetNuke.Framework.AJAX.RegisterPostBackControl(txtForumUserSuggest)
			'	DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdCancel)
			'End If
		End Sub

		''' <summary>
		''' The Page Load request of this control.  Make sure user is ok to be 
		''' here then handle accordingly.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				' Before we do anything, see if the user is logged in and PM's are enabled
				Dim Security As New Forum.ModuleSecurity(ModuleId, TabId, -1, UserId)

				'consider split up to direct to denied or user does not accept PM page. Also consider PM blocking person to person
				If Not (Request.IsAuthenticated And (objConfig.EnablePMSystem = True)) Then
					HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
				End If

				If Page.IsPostBack = False Then
					litCSSLoad.Text = "<link href='" & objConfig.Css & "' type='text/css' rel='stylesheet' />"

					' Get info of who is to receive this private message
					If Not Request.QueryString("userid") Is Nothing Then
						mPMRecipientID = Int32.Parse(Request.QueryString("userid"))
						mPMRecipientUser = ForumUserController.GetForumUser(mPMRecipientID, False, ModuleId, PortalId)
						If Not mPMRecipientUser Is Nothing Then
							txtForumUserSuggest.Text = mPMRecipientUser.SiteAlias
							mPMToEnabled = mPMRecipientUser.EnablePM
						Else
							' they don't belong here
							HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
						End If
					Else
						' If this is not a new PM thread, obtain original post/thread
						If (mParentPMInfo Is Nothing) AndAlso (Not Request.QueryString("pmid") Is Nothing) Then
							mParentPMID = Int32.Parse(Request.QueryString("pmid"))
							Dim ctlPM As New PMController
							mParentPMInfo = ctlPM.PMGet(mParentPMID)

							mParentPMThreadID = mParentPMInfo.PMThreadID
							mParentPMThread = PMThreadInfo.GetPMThreadInfo(mParentPMThreadID)
							mParentPMThread.PortalID = Me.PortalId

							Dim ToUserID As Integer = mParentPMThread.PMReceiveThreadUserID
							Dim FromUserID As Integer = mParentPMThread.PMStartThreadUserID
							'Dim NewToUserID As Integer

							If ToUserID = LoggedOnUser.UserID Then
								mPMRecipientID = FromUserID
							Else
								mPMRecipientID = ToUserID
							End If
							mPMRecipientUser = ForumUserController.GetForumUser(mPMRecipientID, False, ModuleId, PortalId)

							If Not mPMRecipientUser Is Nothing Then
								txtForumUserSuggest.Text = mPMRecipientUser.SiteAlias
								mPMToEnabled = mPMRecipientUser.EnablePM
							Else
								' they don't belong here
								HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
							End If
						Else
							If mParentPMInfo Is Nothing Then
								If Not Request.QueryString("pmaction") Is Nothing Then
									mPMAction = CType([Enum].Parse(GetType(PMAction), Request.QueryString("pmaction"), True), PMAction)
									If mPMAction = PMAction.New Then
										'New PM no recipient selected yet
										mPMToEnabled = True
									End If
								Else
									' If there is no userid it was a bad link and should be considered a security issue
									' they don't belong here
									HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), False)
								End If
							End If
						End If
					End If

					'See if both recipient and sender have pm enabled
					'Add check to make sure user is not blocked from sending this particular user a PM
					If (Not LoggedOnUser.EnablePM) Or (Not mPMToEnabled) Then
						' they don't belong here
						' if there is not a userid, there better be some type of pmid
						HttpContext.Current.Response.Redirect(Utilities.Links.UnAuthorizedLink(), True)
					End If

					' Obtain pm action
					If (Not Request.QueryString("pmaction") Is Nothing) Then
						mPMAction = CType([Enum].Parse(GetType(PMAction), Request.QueryString("pmaction"), True), PMAction)
						'localize
						cmdSubmit.CommandName = mPMAction.ToString
					End If

					' Store the referrer for returning to where the user came from
					If Not Request.UrlReferrer Is Nothing Then
						ViewState("UrlReferrer") = Request.UrlReferrer.ToString()
					End If

					'Spacer images
					imgHeader.ImageUrl = objConfig.GetThemeImageURL("headfoot_height.gif")
					imgLeftCap.ImageUrl = objConfig.GetThemeImageURL("header_cap_left.gif")
					imgRightCap.ImageUrl = objConfig.GetThemeImageURL("header_cap_right.gif")

					lblHeader.Text = Localization.GetString("PM", Me.LocalResourceFile)

					' Register scripts
					'
					'Utils.RegisterPageScripts(Page, ForumConfig)
					InitializeTextSuggest()
					EnableControls()
					GeneratePM()

					' Set the form focus to the subject textbox(avoid users missing subject)
					SetFormFocus(txtSubject)
				End If

			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' This submits the Private Message from either initial view or preview view. 
		''' This calls to PostToDataBase function which then takes the necessary action
		''' and then navigates the user to the appropriate screen.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>It seems in 2.0, you cannot have two items handled by the same event</remarks>
		Protected Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
			Try
				If Len(teContent.Text) = 0 Then
					lblInfo.Text = Localization.GetString("BodyRequired.Text", LocalResourceFile)
					lblInfo.Visible = True
				Else
					lblInfo.Visible = False
					PostToDatabase()
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Takes the user back to where they were.
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			If Not ViewState("UrlReferrer") Is Nothing Then
				Response.Redirect(CType(ViewState("UrlReferrer"), String), False)
			Else
				Response.Redirect(Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Inbox, PortalSettings), False)
			End If
		End Sub

		''' <summary>
		''' Hides areas on the screen and shows user what their post will look like
		''' once it is submitted.  (This does rendering)
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPreview.Click
			Dim fText As Utilities.PostContent = New Utilities.PostContent(Server.HtmlDecode(teContent.Text), objConfig)
			lblHeader.Text = Localization.GetString("lblPreviewHead", Me.LocalResourceFile)
			lblPreview.Text = fText.ProcessHtml()
			rowPreview.Visible = True
			rowNewPost.Visible = False
			cmdPreview.Visible = False
			cmdCancel.Visible = False
			cmdBackToEdit.Visible = True
		End Sub

		''' <summary>
		''' Takes the user back to edit post mode when clicked.  
		''' (This is only visible in preview mode)
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub cmdBackToEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBackToEdit.Click
			lblHeader.Text = Localization.GetString("PM", Me.LocalResourceFile)
			rowPreview.Visible = False
			rowNewPost.Visible = True
			cmdPreview.Visible = True
			cmdCancel.Visible = True
			cmdBackToEdit.Visible = False
		End Sub

		''' <summary>
		''' POD For Client API Text Suggest
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks>
		''' </remarks>
		Protected Sub txtForumUserSuggest_PopulateOnDemand(ByVal source As Object, ByVal e As UI.WebControls.DNNTextSuggestEventArgs) Handles txtForumUserSuggest.PopulateOnDemand
			' Here is where we would count e.Text (if we had minimum character requirements - needed for huge sites)
			PopulateList(e.Nodes, e.Text)
		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Show/Hides replied to post if it exists.  Also sets the intial visiblity for
		''' this screen.
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub EnableControls()
			If (mPMAction = PMAction.Reply) Or (mPMAction = PMAction.Quote) Then
				rowOldPost.Visible = True
			Else
				rowOldPost.Visible = False
			End If

			cmdPreview.Visible = True
			cmdCancel.Visible = True
			cmdBackToEdit.Visible = False
		End Sub

		''' <summary>
		''' This gets the data for the post if in edit post mode.
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub GeneratePM()
			' generate pm content
			If (Not mPMAction = PMAction.[New]) Then
				Dim fTextDecode As Utilities.PostContent = New Utilities.PostContent(Server.HtmlDecode(mParentPMInfo.Body), objConfig)

				lblMessage.Text = fTextDecode.ProcessHtml()
				Dim objFromUser As ForumUser = ForumUserController.GetForumUser(mParentPMInfo.PMFromUserID, False, ModuleId, PortalId)
				hlAuthor.Text = objFromUser.SiteAlias
				hlAuthor.NavigateUrl = Utilities.Links.UserPublicProfileLink(TabId, ModuleId, objFromUser.UserID, objConfig.EnableExternalProfile, objConfig.ExternalProfileParam, objConfig.ExternalProfilePage, objConfig.ExternalProfileUsername, objFromUser.Username)

				Select Case mPMAction
					Case PMAction.Reply
						txtSubject.Text = ReplySubject()
					Case PMAction.Quote
						txtSubject.Text = ReplySubject()
						teContent.Text = fTextDecode.ProcessQuoteBody(objFromUser.SiteAlias, objConfig)
					Case PMAction.Edit
						txtSubject.Text = mParentPMInfo.Subject
						teContent.Text = fTextDecode.Text
				End Select
			End If
		End Sub

		''' <summary>
		''' This submits the post to the database.  It also calls email notification 
		''' if enabled.  It takes moderation into consideration and fires off the 
		''' necessary actions if needed.  Note that it also filters the post from here
		''' for security reasons as well as bad words (if enabled), smiley's.
		''' </summary>
		''' <remarks>
		''' </remarks>
		Private Sub PostToDatabase()
			Try
				' Validate
				lblInfo.Text = String.Empty
				If Len(txtSubject.Text) = 0 Then
					lblInfo.Text = Localization.GetString("SubjectRequired.Text", LocalResourceFile)
					lblInfo.Visible = True
					Return
				End If

				Dim _newPMID As Integer
				Dim _newPMInfo As New PMInfo
				Dim objSecurity As New PortalSecurity

				Dim _RemoteAddr As String = "0.0.0.0"
				If Not Request.ServerVariables("REMOTE_ADDR") Is Nothing Then
					_RemoteAddr = Request.ServerVariables("REMOTE_ADDR")
				End If
				_RemoteAddr = objSecurity.InputFilter(_RemoteAddr, PortalSecurity.FilterFlag.NoMarkup)

				Dim _Subject As String = txtSubject.Text
				Dim _Body As String = teContent.Text

				'[skeel] check images
				_Body = HttpUtility.HtmlDecode(_Body)
				Dim regExp As Regex = New Regex("<img([^>]+)>")
				_Body = regExp.Replace(_Body, New MatchEvaluator(AddressOf ReplaceImageUrl))
				regCounter = 0
				_Body = ProcessStripQuotes(_Body)

				_Body = HttpUtility.HtmlEncode(_Body)

				Dim _ParentPMID As Integer = -1
				If Not (Request.QueryString("pmid")) Is Nothing Then
					_ParentPMID = Int32.Parse(Request.QueryString("pmid"))
				End If

				_Body = objSecurity.InputFilter(_Body, PortalSecurity.FilterFlag.NoScripting)
				'CP - Move to NoHTML post 4.4.1
				_Subject = objSecurity.InputFilter(_Subject, PortalSecurity.FilterFlag.NoScripting)

				' Filter bad word
				Dim ctlWordFilter As New WordFilterController
				If objConfig.EnableBadWordFilter Then
					_Body = ctlWordFilter.FilterBadWord(_Body, PortalId)
				End If
				If objConfig.FilterSubject Then
					_Subject = ctlWordFilter.FilterBadWord(_Subject, PortalId)
				End If

				Dim ctlPost As New PMController
				Dim _emailType As ForumEmailType
				'Dim ProfileUrl As String = LinkUtils.MySettingsLink(TabId, ModuleId)
				Dim ProfileUrl As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Tracking, PortalSettings)

				If txtForumUserSuggest.SelectedNodes.Count > 0 Then
					For Each Node As DotNetNuke.UI.WebControls.DNNNode In Me.txtForumUserSuggest.SelectedNodes

						If Node.Key.Trim() <> String.Empty Then
							mPMRecipientID = CInt(Node.Key)
						End If
					Next
				End If

				' the end user potentially typed a name in but never selected it from the list
				If mPMRecipientID = 0 Then

					Dim cntPM As New PMController
					Dim i As Integer = -1

					Try
						If objConfig.ForumMemberName = 0 Then
							'Username
							i = cntPM.PMUserGetSpecific(PortalId, txtForumUserSuggest.Text.Trim, True)
						Else
							'Displayname
							i = cntPM.PMUserGetSpecific(PortalId, txtForumUserSuggest.Text.Trim, False)
						End If
					Catch ex As Exception
						i = -1
					End Try

					If i > 0 Then
						mPMRecipientID = i
					Else
						'No dice...
						Skins.Skin.AddModuleMessage(Me, String.Format(Localization.GetString("NoUserFound", Me.LocalResourceFile), txtForumUserSuggest.Text), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
						Exit Sub
					End If
				End If

				' Make sure we have the recipient
				mPMRecipientUser = ForumUserController.GetForumUser(mPMRecipientID, False, ModuleId, PortalId)

				' Obtain pm action
				If (Not Request.QueryString("pmaction") Is Nothing) Then
					mPMAction = CType([Enum].Parse(GetType(PMAction), Request.QueryString("pmaction"), True), PMAction)
					'localize
					cmdSubmit.CommandName = mPMAction.ToString
				End If

				' Add new private message
				Select Case mPMAction
					Case PMAction.[New]
						_newPMID = ctlPost.PMAdd(0, LoggedOnUser.UserID, _RemoteAddr, _Subject, _Body, mPMRecipientID, PortalId)
						_emailType = ForumEmailType.UserPMReceived
					Case PMAction.Edit
						'Check if it's still an unread PM
						_newPMID = Int32.Parse(Request.QueryString("pmid"))
						_newPMInfo = ctlPost.PMGet(_newPMID)
						If IsNewPost(_newPMInfo.PMToUserID, _newPMInfo.CreatedDate, _newPMInfo.PMThreadID) Then
							ctlPost.PMUpdate(_newPMInfo.PMID, _Subject, _Body)
							Response.Redirect(Utilities.Links.PMThreadLinkFromOutbox(TabId, ModuleId, _newPMInfo.PMThreadID, _newPMInfo.PMID))
						Else
							Skins.Skin.AddModuleMessage(Me, Localization.GetString("EditError"), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
							Exit Sub
						End If
					Case Else	' Reply or Quote
						_newPMID = ctlPost.PMAdd(_ParentPMID, LoggedOnUser.UserID, _RemoteAddr, _Subject, _Body, mPMRecipientID, PortalId)
						_emailType = ForumEmailType.UserPMReceived
				End Select
				Dim ctlPM As New PMController
				' Get the private message which was just added
				_newPMInfo = ctlPM.PMGet(_newPMID)

				' Send notification email & update forum post added info
				Dim _mailURL As String = Utilities.Links.UCP_UserLinks(TabId, ModuleId, UserAjaxControl.Inbox, PortalSettings)

				If objConfig.MailNotification Then
					If mPMRecipientUser.EnablePMNotifications Then
						Utilities.ForumUtils.SendForumMail(_newPMID, _mailURL, ForumEmailType.UserPMReceived, "Notes", objConfig, ProfileUrl, PortalId)
					End If
				End If

				' Reset the user cached objects here
				ForumUserController.ResetForumUser(LoggedOnUser.UserID, PortalId)
				ForumUserController.ResetForumUser(mPMRecipientID, PortalId)
				ForumPMReadsController.ResetUserNewPMCount(mPMRecipientID)

				Response.Redirect(_mailURL, False)
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		''' <summary>
		''' Gets the URL used in notification emails for users to comeback and see
		''' this post being submitted.
		''' </summary>
		''' <param name="PMInfo"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function GetEmailURL(ByVal PMInfo As PMInfo) As String
			Dim params As String()
			Dim url As String
			params = New String(1) {"mid=" & ModuleId.ToString, "pmthreadid=" & PMInfo.PMThreadID.ToString}
			url = NavigateURL(TabId, "PMThread", params)

			Return url
		End Function

		''' <summary>
		''' Gets the subject if this is a quote or reply and pre-populates that
		''' text box.
		''' </summary>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Private Function ReplySubject() As String
			Dim strSubject As String = mParentPMInfo.Subject

			If (strSubject.Length >= 3) Then
				If Not (strSubject.Substring(0, 3) = "Re:") Then
					strSubject = "Re: " + strSubject
				End If
			Else
				strSubject = "Re: " + strSubject
			End If
			Return strSubject
		End Function

		''' <summary>
		''' This function will test an image 
		''' </summary>
		''' <param name="m"></param>
		''' <remarks>Added by Skeel</remarks>
		Private Function ReplaceImageUrl(ByVal m As Match) As String
			Dim strImage As String = String.Empty
			Try
				Dim GetImage As Boolean = True

				'Does the image have a width set already?
				Dim regExp As New Regex("(?i)(\s*width=)[\""]?([^""\s]*)[\""]?")
				Dim match As Match
				match = regExp.Match(m.Value)

				If match.Success = True Then
					Dim w As Integer = CInt(match.Groups(2).Value)
					If w <= objConfig.MaxPostImageWidth Then
						'image okay, return original html string
						strImage = m.Value
						GetImage = False
					End If
				End If

				If GetImage = True Then
					'Extract the image source
					regExp = New Regex("src=[\""]?([^""\s]*)[\""]?")
					match = regExp.Match(m.Value)
					Dim imageUrl As String = match.Groups(1).Value

					'Get the image from URI
					Dim request As System.Net.HttpWebRequest = TryCast(System.Net.WebRequest.Create(imageUrl), System.Net.HttpWebRequest)
					Dim response As System.Net.HttpWebResponse = TryCast(request.GetResponse(), System.Net.HttpWebResponse)
					Dim stream As System.IO.Stream = response.GetResponseStream()
					Dim ms As New System.IO.MemoryStream()
					Dim bw As New System.IO.BinaryWriter(ms)
					Dim br As New System.IO.BinaryReader(stream)
					bw.Write(br.ReadBytes(CInt(response.ContentLength)))
					ms.Position = 0
					Dim srcImage As System.Drawing.Bitmap = TryCast(System.Drawing.Bitmap.FromStream(ms), System.Drawing.Bitmap)

					'Clean up, we got the Image!
					br.Close()
					bw.Close()
					ms.Close()
					stream.Close()
					response.Close()

					'Check the width
					If srcImage.Width > objConfig.MaxPostImageWidth Then

						'Calculate new image size
						Dim w As Integer = srcImage.Width
						Dim h As Integer = srcImage.Height
						Dim f As Decimal = CDec(objConfig.MaxPostImageWidth / w)
						h = CInt(Math.Ceiling(h * f))
						strImage = "<img src=""" & imageUrl & """ height=""" & CStr(h) & """ width=""" & CStr(objConfig.MaxPostImageWidth) & """ border=""0"" />"
					Else
						'Width is smaller than allowed, return the original html string
						strImage = m.Value
					End If
				End If

			Catch ex As Exception
				'Something's wrong with the image...return original html string
				strImage = m.Value
			End Try

			Return strImage
		End Function

		''' <summary>
		''' removes quote layout from new version quotes
		''' </summary>
		''' <remarks>Added by Skeel</remarks>
		Private Function ProcessStripQuotes(ByVal mText As String) As String
			'Replace &#160; with <space>, only need to do this once..
			If regCounter = 0 Then
				mText = mText.Replace("&#160;", " ")
			End If

			'Handle Quotes with names
			Dim strExpresson As String = "\<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+)(\<\/div>)"
			Dim regExp As Regex = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'Handle Quotes without names
			strExpresson = "\<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+)<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote]$1[/quote]")

			'Now we need to check if there were quotes beside each other
			strExpresson = "\<\/div>([^\]]+)<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[/quote]$1[quote=""$2""]")

			'And the same for quotes without names..
			strExpresson = "\<\/div>([^\]]+)<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[/quote]$1[quote]")

			'And the final scenario
			strExpresson = "\<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+]+[^\]]+)\<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'And the final scenario without name
			strExpresson = "\<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+]+[^\]]+)\<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'Add to the counter
			regCounter = regCounter + 1

			'Check for more instances..
			If mText.IndexOf("<div class=""Quote""><em>") > -1 And regCounter < 10 Then
				ProcessStripQuotes(mText)
			End If

			Return mText

		End Function

#Region "DNNTextSuggest"

		''' <summary>
		''' Setup the DNNTextSuggest for use here
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		Private Sub InitializeTextSuggest()
			txtForumUserSuggest.ForceDownLevel = False
			txtForumUserSuggest.Target = "MyTarget"
			' can't use min char lookup because a user could potentially have a single character username or display name. 
			'txtForumUserSuggest.MinCharacterLookup = 2
			txtForumUserSuggest.IDToken = UI.WebControls.DNNTextSuggest.eIDTokenChar.Brackets
			' comment out delimiter when not wanting multi-select
			'txtForumUserSuggest.Delimiter = ";"c
			txtForumUserSuggest.TextSuggestCssClass = "Forum_Suggest"
			txtForumUserSuggest.DefaultChildNodeCssClass = "Forum_Suggest_DefaultChildNode"
			txtForumUserSuggest.DefaultNodeCssClass = "Forum_Suggest_DefaultNode"
			txtForumUserSuggest.DefaultNodeCssClassOver = "Forum_Suggest_DefaultNodeOver"
			txtForumUserSuggest.DefaultNodeCssClassSelected = "Forum_Suggest_DefaultNodeSelected"
		End Sub

		''' <summary>
		''' Used to populate the DNNTextSuggest w/ results as user types.
		''' </summary>
		''' <param name="objNodes"></param>
		''' <param name="strText"></param>
		''' <remarks>
		''' It is important to note that the filtering is actually handled by the control so no need to pass in a parameter. 
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	8/6/2006	Created
		''' </history>
		Private Sub PopulateList(ByVal objNodes As UI.WebControls.DNNNodeCollection, ByVal strText As String)
			Dim o As UI.WebControls.DNNNode
			Dim arrUsers As List(Of ForumUser)
			Dim cntPM As New PMController
			Dim ByUsername As Boolean = False

			If objConfig.ForumMemberName = 0 Then
				ByUsername = True
			End If

			arrUsers = cntPM.PMUsersGet(PortalId, "", 0, 100, ByUsername, ModuleId)

			For Each objForumUser As ForumUser In arrUsers
				If Me.txtForumUserSuggest.MaxSuggestRows = 0 OrElse objNodes.Count < (Me.txtForumUserSuggest.MaxSuggestRows + 1) Then	 '+1 to let control know there is more "pending"
					o = New UI.WebControls.DNNNode(objForumUser.SiteAlias)
					o.ID = objForumUser.UserID.ToString
					o.Key = (objForumUser.UserID.ToString)
					o.CSSClass = "SpecialNode"
					objNodes.Add(o)
				End If
			Next
		End Sub

#End Region

#End Region

#Region "Protected Functions"

		''' <summary>
		''' Gets the proper image for the UI.
		''' </summary>
		''' <param name="IsLookup"></param>
		''' <returns>String</returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/15/2006	Created
		''' </history>
		Protected Function GetImage(ByVal IsLookup As Boolean) As String
			Dim path As String

			If IsLookup Then
				path = objConfig.GetThemeImageURL("s_lookup.") & objConfig.ImageExtension
			Else
				path = objConfig.GetThemeImageURL("s_delete.") & objConfig.ImageExtension
			End If
			Return path
		End Function

		''' <summary>
		''' Determines if a post has been read by the user. 
		''' </summary>
		''' <param name="UserID"></param>
		''' <param name="PostedDate"></param>
		''' <param name="PMThreadID"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function IsNewPost(ByVal UserID As Integer, ByVal PostedDate As DateTime, ByVal PMThreadID As Integer) As Boolean
			Dim ctlForumPMReads As New ForumPMReadsController
			Dim objPMReadInfo As ForumPMReadsInfo = ctlForumPMReads.PMReadsGet(UserID, PMThreadID)
			If objPMReadInfo Is Nothing Then
				Return True
			Else
				If objPMReadInfo.LastVisitDate < PostedDate Then
					Return True
				Else
					Return False
				End If
			End If
		End Function

#End Region

	End Class

End Namespace
