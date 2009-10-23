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

Namespace DotNetNuke.Modules.Forum.Utilities
	''' <summary>
	''' This class formats the content of forum posts and the notification emails.
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	5/29/2005	Created
	''' </history>
	Friend Class PostContent

#Region "Private Members"

		Private mText As String
		Private regCounter As Integer = 0

		Private Shared _
		 mRegOptions As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.Singleline

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

#Region "Public Methods"

		''' <summary>
		''' 
		''' </summary>
		''' <param name="text"></param>
		''' <param name="objConfig"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal [text] As String, ByVal objConfig As Config)
			mText = [text]
			objConfig = objConfig
		End Sub

		''' <summary>
		''' Creates a new instance of ForumText including Parse Information
		''' </summary>
		''' <param name="text"></param>
		''' <param name="objConfig"></param>
		''' <param name="ParserInfo"></param>
		''' <remarks>[skeel] 8/1/2009 created</remarks>
		Public Sub New(ByVal [text] As String, ByVal objConfig As Config, ByVal ParserInfo As Integer)
			mText = [text]
			Select Case ParserInfo
				Case 1
					'Emoticon
					FormatEmoticons(objConfig)
				Case 2
					'BBCode
					FormatBBCode(objConfig)
				Case 3
					'Emoticons + BBCode
					FormatEmoticons(objConfig)
					FormatBBCode(objConfig)
				Case 5
					FormatEmoticons(objConfig)
				Case 6
					'BBCode
					FormatBBCode(objConfig)
				Case 7
					'Emoticons + BBCode
					FormatEmoticons(objConfig)
					FormatBBCode(objConfig)
				Case Else
					'Do nothing
			End Select
		End Sub

		''' <summary>
		''' Creates a new instance of ForumText including Parse Information, also for Attachments
		''' </summary>
		''' <param name="text"></param>
		''' <param name="objConfig"></param>
		''' <param name="ParserInfo"></param>
		''' <param name="lstAttachments"></param>
		''' <param name="IsAuthenticated"></param>
		''' <remarks>[skeel] 8/1/2009 created</remarks>
		Public Sub New(ByVal [text] As String, ByVal objConfig As Config, ByVal ParserInfo As Integer, _
					 ByVal lstAttachments As List(Of AttachmentInfo), ByVal IsAuthenticated As Boolean)
			mText = [text]

			Select Case ParserInfo
				Case 8, 12 'Inline
					FormatInlineAttachment(lstAttachments, objConfig, IsAuthenticated)
				Case 9 'Emoticons + Inline
					FormatEmoticons(objConfig)
					FormatInlineAttachment(lstAttachments, objConfig, IsAuthenticated)
				Case 10 'BBCode + Inline
					FormatBBCode(objConfig)
					FormatInlineAttachment(lstAttachments, objConfig, IsAuthenticated)
				Case 11, 15 'Emoticons + BBCode + Inline
					FormatEmoticons(objConfig)
					FormatBBCode(objConfig)
					FormatInlineAttachment(lstAttachments, objConfig, IsAuthenticated)
				Case Else
					'Do nothing
			End Select
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Public Sub FormatMultiLine()
			mText = ForumUtils.FormatMultiLine(mText)
		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Public Sub FormatPlainText()
			mText = ForumUtils.FormatPlainText(mText)
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
		Private Sub ProcessStripQuotes(ByVal objConfig As Config)
			'Replace &#160; with <space>, only need to do this once..
			If regCounter = 0 Then
				mText = mText.Replace("&#160;", " ")
			End If

			'Handle Quotes with names
			Dim _
			 strExpresson As String = "\<div class=[""]Quote[""]><em>(\w+)\ " & _
								 Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim & _
								 "(?::|)\<\/em>\<br \/>([^\]]+)(\<\/div>)"
			Dim regExp As Regex = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'Handle Quotes without names
			strExpresson = "\<div class=[""]Quote[""]><em>" & _
						Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & _
						"(?::|)\<\/em>\<br \/>([^\]]+)<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote]$1[/quote]")

			'Now we need to check if there were quotes beside each other
			strExpresson = "\<\/div>([^\]]+)<div class=[""]Quote[""]><em>(\w+)\ " & _
						Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim & _
						"(?::|)\<\/em>\<br \/>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[/quote]$1[quote=""$2""]")

			'And the same for quotes without names..
			strExpresson = "\<\/div>([^\]]+)<div class=[""]Quote[""]><em>" & _
						Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[/quote]$1[quote]")

			'And the final scenario
			strExpresson = "\<div class=[""]Quote[""]><em>(\w+)\ " & _
						Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim & _
						"(?::|)\<\/em>\<br \/>([^\]]+]+[^\]]+)\<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'And the final scenario without name
			strExpresson = "\<div class=[""]Quote[""]><em>" & _
						Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & _
						"(?::|)\<\/em>\<br \/>([^\]]+]+[^\]]+)\<\/div>"
			regExp = New Regex(strExpresson)
			mText = regExp.Replace(mText, "[quote=""$1""]$2[/quote]")

			'Add to the counter
			regCounter = regCounter + 1

			'Check for more instances..
			If mText.IndexOf("<div class=""Quote""><em>") > -1 And regCounter < 10 Then
				ProcessStripQuotes(objConfig)
			End If

		End Sub

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Private Sub FormatStripQuotes(ByVal objConfig As Config)
			'[skeel] run the new style quotes first!
			regCounter = 0
			ProcessStripQuotes(objConfig)

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
		Private Sub FormatBBCode(ByVal objConfig As Config)

			Dim _
			 strTmpName As String = _
			  String.Format( _
						  "<div class=""Quote""><em>{0} " & _
						  Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim() & _
						  ":</em><br />{1}</div>", "${author}", "${text}")
			While mQuoteAuthor.IsMatch(mText)
				mText = mQuoteAuthor.Replace(mText, strTmpName)
			End While

			Dim _
			 strTmp As String = _
			  String.Format("<div class=""Quote""><em>{0}:</em><br />{1}</div>", _
						  Localization.GetString("ForumTextQuote.Text", objConfig.SharedResourceFile).Trim(), "${text}")
			While mQuote.IsMatch(mText)
				mText = mQuote.Replace(mText, strTmp)
			End While

			Dim _
			 strTmpCode As String = _
			  String.Format("<div class=""Code""><em>{0}:</em><br />{1}</div>", _
						  Localization.GetString("ForumTextCode.Text", objConfig.SharedResourceFile).Trim(), "${code}")
			While mCode.IsMatch(mText)
				mText = mCode.Replace(mText, strTmpCode)
			End While

		End Sub

		''' <summary>
		''' Replaces emoticon codes with emoticon images
		''' </summary>
		''' <remarks>[skeel] 1/8/2009 created</remarks>
		Private Sub FormatEmoticons(ByVal objConfig As Config)
			Dim ctlEmoticon As New EmoticonController
			mText = ctlEmoticon.ProcessEmoticons(mText, objConfig.ModuleID)
		End Sub

		''' <summary>
		''' Replaces inline attachments with html code
		''' </summary>
		''' <param name="lstAttachments"></param>
		''' <param name="objConfig"></param>
		''' <remarks>[skeel] 1/8/2009 created</remarks>
		Private Sub FormatInlineAttachment(ByVal lstAttachments As List(Of AttachmentInfo), ByVal objConfig As Config, _
									 ByVal IsAuthenticated As Boolean)
			Dim strBasePath As String = objConfig.CompleteAttachmentURI
			For Each objFile As AttachmentInfo In lstAttachments

				If objFile.Inline = True Then
					Dim regInline As New Regex("\[attachment\]" & objFile.LocalFileName & "\[/attachment\]", mRegOptions)
					Dim strTmp As String = String.Empty

					' Is this an image?
					If InStr("," & glbImageFileTypes.ToLower, "," & objFile.Extension.ToLower) > 0 Then
						If objFile.Width > objConfig.MaxPostImageWidth Then
							' This is most likely a preview scenario, so let's show the user
							' what the post will look like, once the image has been resized
							Dim w As Integer = objFile.Width
							Dim h As Integer = objFile.Height
							Dim f As Decimal = CDec(objConfig.MaxPostImageWidth / w)
							h = CInt(Math.Ceiling(h * f))
							strTmp = "<img src=""" & strBasePath & objFile.FileName & """ height=""" & CStr(h) & """ width=""" & _
								    CStr(objConfig.MaxPostImageWidth) & """ border=""0"" />"
						Else
							' Write the HTML image
							strTmp = "<img src=""" & strBasePath & objFile.FileName & """ height=""" & CStr(objFile.Height) & """ width=""" & _
								    CStr(objFile.Width) & """ border=""0"" />"
						End If
					Else
						' Not an image
						strTmp = RenderNonImageInlineAttachment(objFile, objConfig, IsAuthenticated)

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
		Private Function RenderNonImageInlineAttachment(ByVal objFile As AttachmentInfo, ByVal objConfig As Config, _
											    ByVal IsAuthenticated As Boolean) As String
			Dim sb As New StringBuilder

			'sb.Append("<div class=""Forum_InlineAttachment"">")

			If objConfig.AnonDownloads = False And IsAuthenticated = False Then
				sb.Append( _
						 "<span class=""Forum_Link"">[" & Localization.GetString("NoAnonDownloads", objConfig.SharedResourceFile) & _
						 "]</span>")
			Else
				Dim _
				 Url As String = _
				  LinkClick(CStr("FileID=" & objFile.FileID), objConfig.CurrentPortalSettings.ActiveTab.TabID, objConfig.ModuleID, _
						   False, True)
				sb.Append( _
						 "<a href=""" & Url.Replace("~/", "") & """ class=""Forum_Link"" title=""" & _
						 Localization.GetString("DownloadAttachment", objConfig.SharedResourceFile) & """>" & _
						 objFile.LocalFileName & " (" & objFile.FormattedSize & ")</a>")
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
		Public Function ProcessPlainText(ByVal objConfig As Config) As String
			FormatStripQuotes(objConfig)
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
		''' <param name="objConfig"></param>
		''' <param name="Length"></param>
		''' <param name="ImageToThumb"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ProcessTrimText(ByVal objConfig As Config, Optional ByVal Length As Integer = 100, _
								   Optional ByVal ImageToThumb As Boolean = False) As String
			FormatStripQuotes(objConfig)
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
		''' <param name="objConfig"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ProcessQuoteBody(ByVal parentPoster As String, ByVal objConfig As Config) As String
			Dim sb As New StringBuilder

			If objConfig.DisableHTMLPosting = True Then
				sb.Append("[quote=""" & parentPoster & """]" & mText & "[/quote]")
			Else
				sb.Append("<div class=""Quote""><em>" & parentPoster & " " & Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim() & ":</em><br/>" & mText & "</div>")
				sb.Append("<div></div>")
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

#Region "QuoteHelper"

		''' <summary>
		''' Class used to aid the Quote post methods.
		''' </summary>
		''' <remarks></remarks>
		Private Class QuoteHelper

#Region "Private Members"

			Dim _Open As Boolean
			Dim _Valid As Boolean = False
			Dim _Index As Integer
			Dim _Level As Integer

#End Region

#Region "ReadOnly Properties"

			''' <summary>
			''' 
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			ReadOnly Property Open() As Boolean
				Get
					Return _Open
				End Get
			End Property

			''' <summary>
			''' 
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			ReadOnly Property Level() As Integer
				Get
					Return _Level
				End Get
			End Property

			''' <summary>
			''' 
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			ReadOnly Property Index() As Integer
				Get
					Return _Index
				End Get
			End Property

			''' <summary>
			''' 
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Property Valid() As Boolean
				Get
					Return _Valid
				End Get
				Set(ByVal Value As Boolean)
					_Valid = Value
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
				_Open = open
				_Index = index
				_Level = level
			End Sub
		End Class

#End Region

	End Class

End Namespace