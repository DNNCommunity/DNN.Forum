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

#Region "ForumText"

	''' <summary>
	''' This class formats the content of forum posts and the notification emails.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	5/29/2005	Created
	''' </history>
	Public Class ForumText

#Region "Private Members"

		Private mText As String
		Private mForumConfig As Forum.Config
		Private regCounter As Integer = 0
		Private Shared mRegOptions As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.Singleline
		Private Shared mQuoteAuthor As New Regex("\[quote=""(?<author>[^\]]*)\""](?<text>(.*?))\[/quote\]", mRegOptions)
		Private Shared mQuote As New Regex("\[quote\](?<text>(.*?))\[/quote\]", mRegOptions)
		Private Shared mCode As New Regex("\[code\](?<code>(.*?))\[/code\]", mRegOptions)

		'Not yet supported:
		'Private Shared mBold As New Regex("\[b\](?<text>(.*?))\[/b\]", mRegOptions)
		'Private Shared mItalic As New Regex("\[i\](?<text>(.*?))\[/i\]", mRegOptions)
		'Private Shared mUnderline As New Regex("\[u\](?<text>(.*?))\[/u\]", mRegOptions)
		'Private Shared mImg As New Regex("\[img\](?<http>(http://)|(https://)|(ftp://)|(ftps://))?(?<url>(.*?))\[/img\]", mRegOptions)
		'Private Shared mUrl As New Regex("\[url\](?<http>(http://)|(https://)|(ftp://)|(ftps://))?(?<url>(.*?))\[/url\]", mRegOptions)
		'Private Shared mUrlName As New Regex("\[url=""(?<http>(http://)|(https://)|(ftp://)|(ftps://))?(?<url>[^\]]*)\""](?<name>(.*?))\[/url\]", mRegOptions)
		'Private Shared mFont As New Regex("\[font=(?<font>([-a-z0-9, ]*))\](?<text>(.*?))\[/font\]", mRegOptions)
		'Private Shared mColor As New Regex("\[color=(?<color>(\#?[-a-z0-9]*))\](?<text>(.*?))\[/color\]", mRegOptions)
		'Private Shared mSize As New Regex("\[size=(?<size>([1-9]))\](?<text>(.*?))\[/size\]", mRegOptions)

#End Region

		''' <summary>
		''' Class used to aid the Quote post methods.
		''' </summary>
		''' <remarks></remarks>
		Private Class QuoteHelper

#Region "Private Variables"

			Private mOpen As Boolean
			Private mValid As Boolean = False
			Private mIndex As Integer
			Private mLevel As Integer

#End Region

#Region "Read Properties"

			Public ReadOnly Property Open() As Boolean
				Get
					Return mOpen
				End Get
			End Property

			Public ReadOnly Property Level() As Integer
				Get
					Return mLevel
				End Get
			End Property

			Public ReadOnly Property Index() As Integer
				Get
					Return mIndex
				End Get
			End Property

			Public Property Valid() As Boolean
				Get
					Return mValid
				End Get
				Set(ByVal Value As Boolean)
					mValid = Value
				End Set
			End Property

#End Region

			''' <summary>
			''' 
			''' </summary>
			''' <param name="open"></param>
			''' <param name="index"></param>
			''' <param name="level"></param>
			''' <remarks></remarks>
			Public Sub New(ByVal open As Boolean, ByVal index As Integer, ByVal level As Integer)
				mOpen = open
				mIndex = index
				mLevel = level
			End Sub

		End Class

#Region "Public Methods"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="text"></param>
		''' <param name="ForumConfig"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal [text] As String, ByVal ForumConfig As Forum.Config)
			mText = [text]
			mForumConfig = ForumConfig
		End Sub

		''' <summary>
		''' Creates a new instance of ForumText including Parse Information
		''' </summary>
		''' <param name="text"></param>
		''' <param name="ForumConfig"></param>
		''' <param name="ParserInfo"></param>
		''' <remarks>[skeel] 8/1/2009 created</remarks>
		Public Sub New(ByVal [text] As String, ByVal ForumConfig As Forum.Config, ByVal ParserInfo As Integer)
			mText = [text]
			mForumConfig = ForumConfig

			Select Case ParserInfo

				Case 1
					'Emoticon
					FormatEmoticons()
				Case 2
					'BBCode
					FormatBBCode()
				Case 3
					'Emoticons + BBCode
					FormatEmoticons()
					FormatBBCode()
				Case 5
					FormatEmoticons()
				Case 6
					'BBCode
					FormatBBCode()
				Case 7
					'Emoticons + BBCode
					FormatEmoticons()
					FormatBBCode()

				Case Else
					'Do nothing
			End Select
		End Sub

		''' <summary>
		''' Creates a new instance of ForumText including Parse Information, also for Attachments
		''' </summary>
		''' <param name="text"></param>
		''' <param name="ForumConfig"></param>
		''' <param name="ParserInfo"></param>
		''' <param name="lstAttachments"></param>
		''' <param name="IsAuthenticated"></param>
		''' <remarks>[skeel] 8/1/2009 created</remarks>
		Public Sub New(ByVal [text] As String, ByVal ForumConfig As Forum.Config, ByVal ParserInfo As Integer, ByVal lstAttachments As List(Of AttachmentInfo), ByVal IsAuthenticated As Boolean)
			mText = [text]
			mForumConfig = ForumConfig

			Select Case ParserInfo

				Case 8, 12 'Inline
					FormatInlineAttachment(lstAttachments, ForumConfig, IsAuthenticated)

				Case 9 'Emoticons + Inline
					FormatEmoticons()
					FormatInlineAttachment(lstAttachments, ForumConfig, IsAuthenticated)

				Case 10 'BBCode + Inline
					FormatBBCode()
					FormatInlineAttachment(lstAttachments, ForumConfig, IsAuthenticated)

				Case 11, 15 'Emoticons + BBCode + Inline
					FormatEmoticons()
					FormatBBCode()
					FormatInlineAttachment(lstAttachments, ForumConfig, IsAuthenticated)

				Case Else
					'Do nothing

			End Select
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Public Sub FormatMultiLine()
			mText = Utils.FormatMultiLine(mText)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Public Sub FormatPlainText()
			mText = Utils.FormatPlainText(mText)
		End Sub

		''' <summary>
		''' Get a string based on specified length, protect the image/thumb string
		''' </summary>
		''' <param name="OpenTag"></param>
		''' <param name="CloseTag"></param>
		''' <param name="ContentLength"></param>
		''' <remarks></remarks>
		Private Sub TrimText(ByVal OpenTag As String, ByVal CloseTag As String, ByVal ContentLength As Integer)
			Dim startIndex As Integer = 0
			Dim indexMediaOpen As Integer = 0
			Dim indexMediaClose As Integer = 0
			Dim sb As New StringBuilder

			While indexMediaOpen >= 0 AndAlso startIndex < ContentLength
				indexMediaOpen = mText.IndexOf(OpenTag, startIndex)
				If indexMediaOpen >= 0 Then
					indexMediaClose = mText.IndexOf(CloseTag, indexMediaOpen)
				Else
					indexMediaClose = mText.IndexOf(CloseTag, startIndex)
				End If

				If (indexMediaOpen >= 0) AndAlso (indexMediaClose >= 0) Then
					sb.Append(mText.Substring(startIndex, (indexMediaClose + CloseTag.Length) - startIndex))
					startIndex = indexMediaClose + CloseTag.Length
				End If
			End While

			Dim iLength As Integer = Math.Min(ContentLength, mText.Length)

			If iLength - startIndex > 0 Then
				sb.Append(mText.Substring(startIndex, iLength - startIndex))
			End If

			mText = sb.ToString()
		End Sub

		''' <summary>
		''' Trim text and keep thumb tag
		''' </summary>
		''' <param name="Length"></param>
		''' <remarks></remarks>
		Private Sub FormatTrimText(ByVal Length As Integer)
			Dim iLength As Integer

			' Get end index to return 
			iLength = Math.Max(iLength, InStrRev(mText, "."))
			iLength = Math.Max(iLength, InStrRev(mText, " "))
			mText = mText.Substring(0, iLength)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="array"></param>
		''' <remarks></remarks>
		Private Sub ProcessOpenClose(ByVal array As ArrayList)
			Dim index As Integer = 0
			Dim level As Integer = 0
			Dim indexQuoteOpen As Integer = 0
			Dim indexQuoteClose As Integer = 0
			Dim previousQuoteOpen As Boolean = False

			While indexQuoteOpen >= 0 OrElse indexQuoteClose >= 0
				indexQuoteOpen = mText.IndexOf(Config.QUOTE_OPEN, index)
				indexQuoteClose = mText.IndexOf(Config.QUOTE_CLOSE, index)

				If indexQuoteOpen >= 0 OrElse indexQuoteClose >= 0 Then
					If indexQuoteOpen >= 0 AndAlso (indexQuoteOpen < indexQuoteClose OrElse indexQuoteClose = -1) Then
						If index > 0 AndAlso previousQuoteOpen Then
							level += 1
						End If
						array.Add(New QuoteHelper(True, indexQuoteOpen, level))
						index = indexQuoteOpen + Config.QUOTE_OPEN.Length
						previousQuoteOpen = True
					ElseIf indexQuoteClose >= 0 AndAlso (indexQuoteClose < indexQuoteOpen OrElse indexQuoteOpen = -1) Then
						If index > 0 AndAlso Not previousQuoteOpen Then
							level -= 1
						End If
						array.Add(New QuoteHelper(False, indexQuoteClose, level))
						index = indexQuoteClose + Config.QUOTE_CLOSE.Length
						previousQuoteOpen = False
					End If
				End If
			End While
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="array"></param>
		''' <param name="index"></param>
		''' <remarks></remarks>
		Private Sub ValidateFindClose(ByVal array As ArrayList, ByVal index As Integer)
			Dim helperOpen As QuoteHelper = CType(array(index), QuoteHelper)

			index += 1
			Dim foundClose As Boolean = False

			While Not foundClose AndAlso index < array.Count
				Dim helperClose As QuoteHelper = CType(array(index), QuoteHelper)

				If helperClose.Level = helperOpen.Level AndAlso Not helperClose.Valid Then
					helperOpen.Valid = True
					helperClose.Valid = True
					foundClose = True
				End If
				index += 1
			End While
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="array"></param>
		''' <remarks></remarks>
		Private Sub ValidateOpenClose(ByVal array As ArrayList)
			Dim index As Integer
			For index = 0 To array.Count - 1
				Dim helper As QuoteHelper = CType(array(index), QuoteHelper)
				If helper.Open Then
					ValidateFindClose(array, index)
				End If
			Next index
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sb"></param>
		''' <remarks></remarks>
		Private Sub FormatQuoteOpen(ByVal sb As StringBuilder)
			sb.Append("<div class=""Quote"">")
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <param name="sb"></param>
		''' <remarks></remarks>
		Private Sub FormatQuoteClose(ByVal sb As StringBuilder)
			sb.Append("</div>")
		End Sub

		''' <summary>
		''' removes quote layout from new version quotes
		''' </summary>
		''' <remarks>Added by Skeel</remarks>
		Private Sub ProcessStripQuotes()
			'Replace &#160; with <space>, only need to do this once..
			If regCounter = 0 Then
				mText = mText.Replace("&#160;", " ")
			End If

			'Handle Quotes with names
			Dim strExpresson As String = "\<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", mForumConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+)(\<\/div>)"
			Dim regExp As Regex = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'Handle Quotes without names
			strExpresson = "\<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", mForumConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+)<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote]$1[/quote]")

			'Now we need to check if there were quotes beside each other
			strExpresson = "\<\/div>([^\]]+)<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", mForumConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[/quote]$1[quote=""$2""]")

			'And the same for quotes without names..
			strExpresson = "\<\/div>([^\]]+)<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", mForumConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[/quote]$1[quote]")

			'And the final scenario
			strExpresson = "\<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", mForumConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+]+[^\]]+)\<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'And the final scenario without name
			strExpresson = "\<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", mForumConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+]+[^\]]+)\<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'Add to the counter
			regCounter = regCounter + 1

			'Check for more instances..
			If mText.IndexOf("<div class=""Quote""><em>") > -1 And regCounter < 10 Then
				ProcessStripQuotes()
			End If

		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Private Sub FormatStripQuotes()
			'[skeel] run the new style quotes first!
			regCounter = 0
			ProcessStripQuotes()

			Dim array As New ArrayList
			ProcessOpenClose(array)
			ValidateOpenClose(array)

			Dim sb As New StringBuilder
			Dim startIndex As Integer = 0
			Dim index As Integer = 0

			While index < array.Count
				Dim helper As QuoteHelper = CType(array(index), QuoteHelper)

				If helper.Valid Then
					' helper.Open will be true, meaning there must be a corresponding
					' closing helper item at the same level as helper.Open
					Dim found As Boolean = False
					Dim level As Integer = helper.Level
					index += 1
					Dim helperClose As QuoteHelper = Nothing
					While index < array.Count AndAlso Not found
						helperClose = CType(array(index), QuoteHelper)
						If helperClose.Valid AndAlso Not helperClose.Open AndAlso helperClose.Level = level Then
							found = True
						End If
						index += 1
					End While
					If found Then
						Dim length As Integer = helper.Index - startIndex
						If length > 0 Then
							sb.Append(mText.Substring(startIndex, length))
						End If
						startIndex = helperClose.Index + Config.QUOTE_CLOSE.Length
					End If
				Else
					index += 1
				End If
			End While

			If mText.Length - startIndex > 0 Then
				sb.Append(mText.Substring(startIndex, mText.Length - startIndex))
			End If

			mText = sb.ToString()

		End Sub

		''' <summary>
		''' Replaces quote/code tags with equilevant html code
		''' </summary>
		''' <remarks>[skeel] 1/8/2009 created</remarks>
		Private Sub FormatBBCode()

			Dim strTmpName As String = String.Format("<div class=""Quote""><em>{0} " & Localization.GetString("ForumTextWrote.Text", mForumConfig.SharedResourceFile).Trim() & ":</em><br />{1}</div>", "${author}", "${text}")
			While mQuoteAuthor.IsMatch(mText)
				mText = mQuoteAuthor.Replace(mText, strTmpName)
			End While

			Dim strTmp As String = String.Format("<div class=""Quote""><em>{0}:</em><br />{1}</div>", Localization.GetString("ForumTextQuote.Text", mForumConfig.SharedResourceFile).Trim(), "${text}")
			While mQuote.IsMatch(mText)
				mText = mQuote.Replace(mText, strTmp)
			End While

			Dim strTmpCode As String = String.Format("<div class=""Code""><em>{0}:</em><br />{1}</div>", Localization.GetString("ForumTextCode.Text", mForumConfig.SharedResourceFile).Trim(), "${code}")
			While mCode.IsMatch(mText)
				mText = mCode.Replace(mText, strTmpCode)
			End While

		End Sub

		''' <summary>
		''' Replaces emoticon codes with emoticon images
		''' </summary>
		''' <remarks>[skeel] 1/8/2009 created</remarks>
		Private Sub FormatEmoticons()
			Dim ctlEmoticon As New EmoticonController
			mText = ctlEmoticon.ProcessEmoticons(mText, mForumConfig.ModuleID)
		End Sub

		''' <summary>
		''' Replaces inline attachments with html code
		''' </summary>
		''' <param name="lstAttachments"></param>
		''' <param name="ForumConfig"></param>
		''' <remarks>[skeel] 1/8/2009 created</remarks>
		Private Sub FormatInlineAttachment(ByVal lstAttachments As List(Of AttachmentInfo), ByVal ForumConfig As Forum.Config, ByVal IsAuthenticated As Boolean)
			Dim strBasePath As String = mForumConfig.CompleteAttachmentURI
			For Each objFile As AttachmentInfo In lstAttachments

				If objFile.Inline = True Then
					Dim regInline As New Regex("\[attachment\]" & objFile.LocalFileName & "\[/attachment\]", mRegOptions)
					Dim strTmp As String = ""

					' Is this an image?
					If InStr("," & Common.glbImageFileTypes.ToLower, "," & objFile.Extension.ToLower) > 0 Then
						If objFile.Width > ForumConfig.MaxPostImageWidth Then
							' This is most likely a preview scenario, so let's show the user
							' what the post will look like, once the image has been resized
							Dim w As Integer = objFile.Width
							Dim h As Integer = objFile.Height
							Dim f As Decimal = CDec(ForumConfig.MaxPostImageWidth / w)
							h = CInt(Math.Ceiling(h * f))
							strTmp = "<img src=""" & strBasePath & objFile.FileName & """ height=""" & CStr(h) & """ width=""" & CStr(ForumConfig.MaxPostImageWidth) & """ border=""0"" />"
						Else
							' Write the HTML image
							strTmp = "<img src=""" & strBasePath & objFile.FileName & """ height=""" & CStr(objFile.Height) & """ width=""" & CStr(objFile.Width) & """ border=""0"" />"
						End If
					Else
						' Not an image
						strTmp = RenderNonImageInlineAttachment(objFile, ForumConfig, IsAuthenticated)

					End If
					' Perform the replacement
					mText = regInline.Replace(mText, strTmp)
				End If

			Next
		End Sub

		''' <summary>
		''' Will render an inline attachment
		''' </summary>
		''' <remarks>[skeel] 1/9/2009 created</remarks>
		Private Function RenderNonImageInlineAttachment(ByVal objFile As AttachmentInfo, ByVal ForumConfig As Forum.Config, ByVal IsAuthenticated As Boolean) As String
			Dim sb As New StringBuilder

			'sb.Append("<div class=""Forum_InlineAttachment"">")

			If ForumConfig.AnonDownloads = False And IsAuthenticated = False Then
				sb.Append("<span class=""Forum_Link"">[" & Localization.GetString("NoAnonDownloads", ForumConfig.SharedResourceFile) & "]</span>")
			Else
				Dim Url As String = Common.Globals.LinkClick(CStr("FileID=" & objFile.FileID), ForumConfig.TabID, ForumConfig.ModuleID, False, True)
				sb.Append("<a href=""" & Url.Replace("~/", "") & """ class=""Forum_Link"" title=""" & Localization.GetString("DownloadAttachment", ForumConfig.SharedResourceFile) & """>" & objFile.LocalFileName & " (" & objFile.FormattedSize & ")</a>")
			End If

			'sb.Append("</div>")

			Return sb.ToString
		End Function

		''' <summary>
		''' Used to be Format old Quotes, kept for backward compability
		''' </summary>
		''' <remarks>[skeel] 1/8/2009 created</remarks>
		Private Sub FormatOldQuotes()
			Dim array As New ArrayList
			ProcessOpenClose(array)
			ValidateOpenClose(array)

			Dim sb As New StringBuilder

			Dim startIndex As Integer = 0
			Dim index As Integer
			For index = 0 To array.Count - 1
				Dim helper As QuoteHelper = CType(array(index), QuoteHelper)

				If helper.Valid Then
					Dim length As Integer = helper.Index - startIndex
					If length > 0 Then
						sb.Append(mText.Substring(startIndex, length))
					End If
					If helper.Open Then
						FormatQuoteOpen(sb)
						startIndex = helper.Index + Config.QUOTE_OPEN.Length
					Else
						FormatQuoteClose(sb)
						startIndex = helper.Index + Config.QUOTE_CLOSE.Length
					End If
				End If
			Next index

			If mText.Length - startIndex > 0 Then
				sb.Append(mText.Substring(startIndex, mText.Length - startIndex))
			End If
			mText = sb.ToString()
		End Sub

		''' <summary>
		''' Processes an html body to prepare for an email send. 
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ProcessHtml() As String
			FormatMultiLine()
			'Only process the old style quotes if present
			If mText.IndexOf("[QUOTE]") > -1 Then
				FormatOldQuotes()
			End If
			'<tam:note Depends on which decision on PostSave or on PostDisplay>
			'FormatProhibitedWord()
			'</tam:note>
			Return mText
		End Function

		''' <summary>
		''' Processes a text email body to prepare for an email send.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ProcessPlainText() As String
			FormatStripQuotes()
			FormatMultiLine()
			FormatPlainText()
			'<tam:note Depends on which decision on PostSave or on PostDisplay>
			'FormatProhibitedWord()
			'</tam:note>
			Return mText
		End Function

		''' <summary>
		''' Process Text for What's New by trimming it
		''' </summary>
		''' <param name="Length"></param>
		''' <param name="ImageToThumb"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ProcessTrimText(Optional ByVal Length As Integer = 100, Optional ByVal ImageToThumb As Boolean = False) As String
			FormatStripQuotes()
			FormatTrimText(Length)
			FormatMultiLine()
			Return mText
		End Function

		''' <summary>
		''' Process text for What's New by removing media from post.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ProcessNoMedia() As String
			FormatMultiLine()
			Return mText
		End Function

		''' <summary>
		''' Will generate a quote to PostEdit or PM_Edit
		''' </summary>
		''' <param name="parentPoster"></param>
		''' <param name="DisableHTMLPosting"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ProcessQuoteBody(ByVal parentPoster As String, ByVal DisableHTMLPosting As Boolean) As String
			Dim sb As New StringBuilder

			If DisableHTMLPosting = True Then
				sb.Append("[quote=""" & parentPoster & """]" & mText & "[/quote]")
			Else
				sb.Append("<div class=""Quote""><em>" & parentPoster & " " & Localization.GetString("ForumTextWrote.Text", mForumConfig.SharedResourceFile).Trim() & ":</em><br/>" & mText & "</div>")
			End If

			mText = sb.ToString()
			Return mText

		End Function

#End Region

#Region "Public Properties"

		''' <summary>
		''' The text to be processed.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property [Text]() As String
			Get
				Return mText
			End Get
			Set(ByVal Value As String)
				mText = Value
			End Set
		End Property

#End Region

	End Class

#End Region

End Namespace