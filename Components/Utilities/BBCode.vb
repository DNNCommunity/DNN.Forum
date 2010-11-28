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

Imports System.Text.RegularExpressions

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' BBCode Helper allows formatting of text
	''' without the need to use html
	''' </summary>
	Public Class BBCodeHelper

#Region "Helper Classes"

		Private Interface IHtmlFormatter
			Function Format(ByVal data As String) As String
		End Interface

		Protected Class RegexFormatter
			Implements IHtmlFormatter

			Private _replace As String
			Private _regex As Regex

			Public Sub New(ByVal pattern As String, ByVal replace As String)
				Me.New(pattern, replace, True)
			End Sub

			Public Sub New(ByVal pattern As String, ByVal replace As String, ByVal ignoreCase As Boolean)
				Dim options As RegexOptions = RegexOptions.Compiled

				If ignoreCase Then
					options = options Or RegexOptions.IgnoreCase
				End If

				_replace = replace
				_regex = New Regex(pattern, options)
			End Sub

			Public Function Format(ByVal data As String) As String
				Return _regex.Replace(data, _replace)
			End Function

			Public Function IHtmlFormatter_Format(ByVal data As String) As String Implements IHtmlFormatter.Format
				Throw New NotImplementedException()
			End Function

  End Class

		Protected Class SearchReplaceFormatter
			Implements IHtmlFormatter
			Private _pattern As String
			Private _replace As String

			Public Sub New(ByVal pattern As String, ByVal replace As String)
				_pattern = pattern
				_replace = replace
			End Sub

			Public Function Format(ByVal data As String) As String
				Return data.Replace(_pattern, _replace)
			End Function

			Public Function IHtmlFormatter_Format(ByVal data As String) As String Implements IHtmlFormatter.Format
				Throw New NotImplementedException()
			End Function

  End Class

#End Region

#Region "BBCode"

		Shared _formatters As List(Of IHtmlFormatter)

		Shared Sub New()
			_formatters = New List(Of IHtmlFormatter)()

			_formatters.Add(New RegexFormatter("<(.|\n)*?>", String.Empty))

			_formatters.Add(New SearchReplaceFormatter(vbCr, ""))
			_formatters.Add(New SearchReplaceFormatter(vbLf & vbLf, "</p><p>"))
			_formatters.Add(New SearchReplaceFormatter(vbLf, "<br />"))

			_formatters.Add(New RegexFormatter("\[b(?:\s*)\]((.|\n)*?)\[/b(?:\s*)\]", "<b>$1</b>"))
			_formatters.Add(New RegexFormatter("\[i(?:\s*)\]((.|\n)*?)\[/i(?:\s*)\]", "<i>$1</i>"))
			_formatters.Add(New RegexFormatter("\[s(?:\s*)\]((.|\n)*?)\[/s(?:\s*)\]", "<strike>$1</strike>"))

			_formatters.Add(New RegexFormatter("\[left(?:\s*)\]((.|\n)*?)\[/left(?:\s*)]", "<div style=""text-align:left"">$1</div>"))
			_formatters.Add(New RegexFormatter("\[center(?:\s*)\]((.|\n)*?)\[/center(?:\s*)]", "<div style=""text-align:center"">$1</div>"))
			_formatters.Add(New RegexFormatter("\[right(?:\s*)\]((.|\n)*?)\[/right(?:\s*)]", "<div style=""text-align:right"">$1</div>"))

			Dim quoteStart As String = "<blockquote><b>$1 said:</b></p><p>"
			Dim quoteEmptyStart As String = "<blockquote>"
			Dim quoteEnd As String = "</blockquote>"

			_formatters.Add(New RegexFormatter("\[quote=((.|\n)*?)(?:\s*)\]", quoteStart))
			_formatters.Add(New RegexFormatter("\[quote(?:\s*)\]", quoteEmptyStart))
			_formatters.Add(New RegexFormatter("\[/quote(?:\s*)\]", quoteEnd))

			_formatters.Add(New RegexFormatter("\[url(?:\s*)\]www\.(.*?)\[/url(?:\s*)\]", "<a class=""bbcode-link"" href=""http://www.$1"" target=""_blank"" title=""$1"">$1</a>"))
			_formatters.Add(New RegexFormatter("\[url(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$1</a>"))
			_formatters.Add(New RegexFormatter("\[url=""((.|\n)*?)(?:\s*)""\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$3</a>"))
			_formatters.Add(New RegexFormatter("\[url=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$3</a>"))
			_formatters.Add(New RegexFormatter("\[link(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$1</a>"))
			_formatters.Add(New RegexFormatter("\[link=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=""bbcode-link"" href=""$1"" target=""_blank"" title=""$1"">$3</a>"))

			_formatters.Add(New RegexFormatter("\[img(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img src=""$1"" border=""0"" alt="""" />"))
			_formatters.Add(New RegexFormatter("\[img align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img src=""$3"" border=""0"" align=""$1"" alt="""" />"))
			_formatters.Add(New RegexFormatter("\[img=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img width=""$1"" height=""$3"" src=""$5"" border=""0"" alt="""" />"))

			_formatters.Add(New RegexFormatter("\[color=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/color(?:\s*)\]", "<span style=""color:$1;"">$3</span>"))

			_formatters.Add(New RegexFormatter("\[hr(?:\s*)\]", "<hr />"))

			_formatters.Add(New RegexFormatter("\[email(?:\s*)\]((.|\n)*?)\[/email(?:\s*)\]", "<a href=""mailto:$1"">$1</a>"))

			_formatters.Add(New RegexFormatter("\[size=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/size(?:\s*)\]", "<span style=""font-size:$1"">$3</span>"))
			_formatters.Add(New RegexFormatter("\[font=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/font(?:\s*)\]", "<span style=""font-family:$1;"">$3</span>"))
			_formatters.Add(New RegexFormatter("\[align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/align(?:\s*)\]", "<span style=""text-align:$1;"">$3</span>"))
			_formatters.Add(New RegexFormatter("\[float=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/float(?:\s*)\]", "<span style=""float:$1;"">$3</div>"))

			Dim sListFormat As String = "<ol class=""bbcode-list"" style=""list-style:{0};"">$1</ol>"

			_formatters.Add(New RegexFormatter("\[\*(?:\s*)]\s*([^\[]*)", "<li>$1</li>"))
			_formatters.Add(New RegexFormatter("\[list(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", "<ul class=""bbcode-list"">$1</ul>"))
			_formatters.Add(New RegexFormatter("\[list=1(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "decimal"), False))
			_formatters.Add(New RegexFormatter("\[list=i(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "lower-roman"), False))
			_formatters.Add(New RegexFormatter("\[list=I(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "upper-roman"), False))
			_formatters.Add(New RegexFormatter("\[list=a(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "lower-alpha"), False))
			_formatters.Add(New RegexFormatter("\[list=A(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", String.Format(sListFormat, "upper-alpha"), False))
		End Sub

#End Region

#Region "Format"

		Public Shared Function Format(ByVal data As String) As String
			For Each formatter As IHtmlFormatter In _formatters
				data = formatter.Format(data)
			Next

			Return data
		End Function

#End Region

	End Class

End Namespace