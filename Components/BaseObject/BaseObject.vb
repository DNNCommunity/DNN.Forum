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

#Region "ForumBaseObject"

    ''' <summary>
    ''' The base object is responsible for common methods used to build the forum UI in vb code including:
    ''' ForumPost, ForumThread, ForumGroup, ForumThreadSearch, ForumPortalSearch, ForumModerate
    ''' </summary>
    ''' <remarks>This is loaded by BaseControl.vb which is loaded by DNNForum.vb, which is loaded by forum container.
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	7/13/2005	Created
    ''' </history>
	Public Class ForumBaseObject
		Inherits ForumModuleBase

#Region "Private Members"

		Private _BaseControl As ForumBaseControl

#End Region

#Region "Protected Methods"

		''' <summary>
		''' Starts a table built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The htmlTextWriter being used</param>
		''' <param name="cellspacing">The cellspacing size the table should have</param>
		''' <param name="cellpadding">The cellpadding size the table should have</param>
		''' <remarks>Similar to a stringbuilder</remarks>
		Protected Sub RenderTableBegin(ByVal wr As HtmlTextWriter, ByVal cellspacing As Integer, ByVal cellpadding As Integer, ByVal id As String)
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Container")
			wr.AddAttribute(HtmlTextWriterAttribute.Align, "center")
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, cellspacing.ToString())
			wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, cellpadding.ToString())
			wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
			wr.AddAttribute(HtmlTextWriterAttribute.Id, id)
			wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>
		End Sub

		''' <summary>
		''' Starts a table built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">THe HtmlTextWriter being used.</param>
		''' <param name="Id">The id to apply to the table.</param>
		''' <param name="CssClass">The css class to apply to the table.</param>
		''' <param name="Height">The height of the table.</param>
		''' <param name="Width">The width of the table.</param>
		''' <param name="Cellspacing">The cellspacing size being applied to the table.</param>
		''' <param name="Cellpadding">The cellpading size being applied to the table.</param>
		''' <param name="Align">The horizontal alignment to apply to the table.</param>
		''' <param name="Valign">The vertical alignment to apply to the table.</param>
		''' <param name="Border">The broder thickness to apply to the table.</param>
		''' <remarks>Similar to a stringbuilder.</remarks>
		Protected Sub RenderTableBegin(ByVal wr As HtmlTextWriter, ByVal Id As String, ByVal CssClass As String, ByVal Height As String, ByVal Width As String, ByVal Cellspacing As String, ByVal Cellpadding As String, ByVal Align As String, ByVal Valign As String, ByVal Border As String)
			If Id.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Id, Id)
			End If
			If CssClass.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, CssClass)
			End If
			'If Height.Length > 0 Then
			'    wr.AddAttribute(HtmlTextWriterAttribute.Height, Height)
			'End If
			If Width.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Width, Width)
			End If
			If Cellspacing.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, Cellspacing)
			End If
			If Cellpadding.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, Cellpadding)
			End If
			If Align.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Align, Align)
			End If
			If Valign.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Valign, Valign)
			End If
			If Border.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Border, Border)
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>

		End Sub

		''' <summary>
		''' Ends a table built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <remarks>Just a table end(Similar to a stringbuilder)</remarks>
		Protected Sub RenderTableEnd(ByVal wr As HtmlTextWriter)
			wr.RenderEndTag() ' </table>
		End Sub

		''' <summary>
		''' Starts a table row built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <remarks>Just a table row begin.(Similar to a stringbuilder)</remarks>
		Protected Sub RenderRowBegin(ByVal wr As HtmlTextWriter)
			wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
		End Sub

		''' <summary>
		''' Starts a table row built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="ID">The ID to apply to the table row.</param>
		''' <remarks>Just a table row begin</remarks>
		Protected Sub RenderRowBegin(ByVal wr As HtmlTextWriter, ByVal ID As String)
			wr.AddAttribute(HtmlTextWriterAttribute.Id, ID)
			wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
		End Sub

		''' <summary>
		''' Ends a table row built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <remarks>Just ends a table row.</remarks>
		Protected Sub RenderRowEnd(ByVal wr As HtmlTextWriter)
			wr.RenderEndTag() ' </tr>
		End Sub

		''' <summary>
		''' Starts a table cell built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="CssClass">The css class to apply to the table cell.</param>
		''' <param name="Height">The height to apply to the table cell.</param>
		''' <param name="Width">The width to apply to the table cell.</param>
		''' <param name="Align">The horizontal alignment to apply to the table cell.</param>
		''' <param name="Valign">The vertical alignment to apply to the table cell.</param>
		''' <param name="Colspan">The column span value to apply to the table cell.</param>
		''' <param name="Rowspan">The row span value to apply to the table cell.</param>
		''' <remarks>Just a td begin. (Similar to a stringbuilder)</remarks>
		Protected Sub RenderCellBegin(ByVal wr As HtmlTextWriter, ByVal CssClass As String, ByVal Height As String, ByVal Width As String, ByVal Align As String, ByVal Valign As String, ByVal Colspan As String, ByVal Rowspan As String)
			If CssClass.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, CssClass)
			End If
			'If Height.Length > 0 Then
			'    wr.AddAttribute(HtmlTextWriterAttribute.Height, Height)
			'End If
			If Width.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Width, Width)
			End If
			If Align.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Align, Align)
			End If
			If Valign.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Valign, Valign)
			End If
			If Colspan.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Colspan, Colspan)
			End If
			If Rowspan.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Rowspan, Rowspan)
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

		End Sub

		''' <summary>
		''' Ends a table cell built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <remarks>Just a td end. (Similar to a stringbuilder)</remarks>
		Protected Sub RenderCellEnd(ByVal wr As HtmlTextWriter)
			wr.RenderEndTag() ' </td>
		End Sub

		''' <summary>
		''' Starts a div tag built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="Id">The ID of the div.</param>
		''' <param name="CssClass">The css class to apply to the div.</param>
		''' <remarks>Just a div begin tag. (Similar to a stringbuilder)</remarks>
		Protected Sub RenderDivBegin(ByVal wr As HtmlTextWriter, ByVal Id As String, ByVal CssClass As String)
			If Id.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Id, Id)
			End If
			If CssClass.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, CssClass)
			End If
			wr.RenderBeginTag(HtmlTextWriterTag.Div) ' <div>

		End Sub

		''' <summary>
		''' Ends a div tag built w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <remarks>The div end tag. (Similar to a stringbuilder)</remarks>
		Protected Sub RenderDivEnd(ByVal wr As HtmlTextWriter)
			wr.RenderEndTag() ' </div>
		End Sub

		''' <summary>
		''' Creates a complete table cell used for space holders w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="Image">The image path to inject into the table cell.</param>
		''' <param name="CssClass">The css class to apply to the table cell.</param>
		''' <param name="Align">The horizontal alignment to apply to the table cell.</param>
		''' <remarks>Used to build a table cell and enclose an image in it for most of programatically generated UI. (Similar to a stringbuilder)</remarks>
		Protected Sub RenderCapCell(ByVal wr As HtmlTextWriter, ByVal Image As String, ByVal CssClass As String, ByVal Align As String)
			If CssClass.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, CssClass)
			End If

			If Align.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Align, Align)
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td>

			'wr.AddAttribute(HtmlTextWriterAttribute.Valign, "middle")
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Src, Image)
			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
			wr.RenderEndTag() ' </Img>

			wr.RenderEndTag() ' </Td>
		End Sub

		''' <summary>
		''' Creates an Image Button used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="Url">The url used for navigation.</param>
		''' <param name="ImageUrl">The image url to apply.</param>
		''' <param name="Tooltip">The text to be shown on hover.</param>
		''' <param name="Css">The css class to apply to the image button.</param>
		''' <remarks>I don't think we apply a css class. (Similar to a stringbuilder)</remarks>
		Protected Sub RenderImageButton(ByVal wr As HtmlTextWriter, ByVal Url As String, ByVal ImageUrl As String, ByVal Tooltip As String, ByVal Css As String)
			wr.AddAttribute(HtmlTextWriterAttribute.Href, Url.Replace("~/", ""))

			wr.RenderBeginTag(HtmlTextWriterTag.A)
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")

			wr.AddAttribute(HtmlTextWriterAttribute.Src, ImageUrl)

			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If

			If Tooltip.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Alt, Tooltip)
				wr.AddAttribute(HtmlTextWriterAttribute.Title, Tooltip)
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' img 
			wr.RenderEndTag() ' img 
			wr.RenderEndTag() ' A
		End Sub

		''' <summary>
		''' Creates an Image Button used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The TextHtmlWriter being used.</param>
		''' <param name="Url">The url the link should navigate to.</param>
		''' <param name="ImageUrl">The image url to the image.</param>
		''' <param name="Tooltip">The text to display on hover.</param>
		''' <param name="Css">The css class to apply to the image button.</param>
		''' <param name="TargetBlank">True if the link should open in a new browser window.</param>
		''' <param name="Enabled">If the link should be clickable.</param>
		''' <remarks>If disabled, it should be for feature not completed or a permissions thing, or not logically correct (ie. an action they should not be allowed to perform)</remarks>
		Protected Sub RenderImageButton(ByVal wr As HtmlTextWriter, ByVal Url As String, ByVal ImageUrl As String, ByVal Tooltip As String, ByVal Css As String, ByVal TargetBlank As Boolean, ByVal Enabled As Boolean)

			If Enabled Then
				wr.AddAttribute(HtmlTextWriterAttribute.Href, Url.Replace("~/", ""))
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.A)
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Src, ImageUrl)

			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If

			If Tooltip.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Alt, Tooltip)
				wr.AddAttribute(HtmlTextWriterAttribute.Title, Tooltip)
			End If

			If TargetBlank Then
				wr.AddAttribute(HtmlTextWriterAttribute.Target, "_blank")
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' img 
			wr.RenderEndTag() ' img 
			wr.RenderEndTag() ' A
		End Sub

		''' <summary>
		''' Creates an Image Button used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="Url">The url to navigate to.</param>
		''' <param name="ImageUrl">The url to the image.</param>
		''' <param name="Tooltip">The text to show on hover.</param>
		''' <param name="Css">The css class to apply to the image butotn.</param>
		''' <param name="TargetBlank">If a new browser window should be opened when the link is clicked.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Protected Sub RenderImageButton(ByVal wr As HtmlTextWriter, ByVal Url As String, ByVal ImageUrl As String, ByVal Tooltip As String, ByVal Css As String, ByVal TargetBlank As Boolean)
			wr.AddAttribute(HtmlTextWriterAttribute.Href, Url.Replace("~/", ""))

			wr.RenderBeginTag(HtmlTextWriterTag.A)
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Src, ImageUrl)

			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If

			If Tooltip.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Alt, Tooltip)
				wr.AddAttribute(HtmlTextWriterAttribute.Title, Tooltip)
			End If

			If TargetBlank Then
				wr.AddAttribute(HtmlTextWriterAttribute.Target, "_blank")
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' img 
			wr.RenderEndTag() ' img 
			wr.RenderEndTag() ' A
		End Sub

		''' <summary>
		''' Creates an Image used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="ImageUrl">The url to the image.</param>
		''' <param name="Tooltip">The text to display on hover.</param>
		''' <param name="Css">The css class to apply to the image.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Protected Sub RenderImage(ByVal wr As HtmlTextWriter, ByVal ImageUrl As String, ByVal Tooltip As String, ByVal Css As String)
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Src, ImageUrl)

			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If

			If Tooltip.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Alt, Tooltip)
				wr.AddAttribute(HtmlTextWriterAttribute.Title, Tooltip)
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' img 
			wr.RenderEndTag() ' img thumb
		End Sub

		''' <summary>
		''' Creates an Image used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="ImageUrl">The url to the image.</param>
		''' <param name="Tooltip">The text to display on hover.</param>
		''' <param name="Css">The css class to apply to the image.</param>
		''' <param name="Width">The width we should set the image to.</param>
		''' <param name="Height">The height we should set the image to.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Protected Sub RenderImage(ByVal wr As HtmlTextWriter, ByVal ImageUrl As String, ByVal Tooltip As String, ByVal Css As String, ByVal Width As String, ByVal Height As String)
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Src, ImageUrl)

			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If

			If Width.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Width, Width)
			End If

			If Height.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Height, Height)
			End If

			If Tooltip.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Alt, Tooltip)
				wr.AddAttribute(HtmlTextWriterAttribute.Title, Tooltip)
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' img 
			wr.RenderEndTag() ' img thumb
		End Sub

		''' <summary>
		''' Creates a link w/ css rollover capability, assigns title tag 
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="URL">The url to navigate to when clicked.</param>
		''' <param name="Text">The string to show as the link.</param>
		''' <param name="Css">The css class to apply to the link button.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Protected Overloads Sub RenderCssLinkButton(ByVal wr As HtmlTextWriter, ByVal URL As String, ByVal Text As String, ByVal Css As String, ByVal objConfig As Configuration)
			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If
			wr.AddAttribute(HtmlTextWriterAttribute.Title, Text)
			wr.AddAttribute(HtmlTextWriterAttribute.Href, URL)
			wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
			RenderImage(wr, objConfig.GetThemeImageURL("ib_spacer.gif"), "", "")
			wr.RenderEndTag() ' </A>
		End Sub

		''' <summary>
		''' Creates a Link Button used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="URL">The url to navigate to when clicked.</param>
		''' <param name="Text">The string of text to show to the end user.</param>
		''' <param name="Css">The css class being applied to the link button.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Protected Overloads Sub RenderLinkButton(ByVal wr As HtmlTextWriter, ByVal URL As String, ByVal Text As String, ByVal Css As String)
			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If
			wr.AddAttribute(HtmlTextWriterAttribute.Href, URL)
			wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
			wr.Write(Text)
			wr.RenderEndTag() ' </A>
		End Sub

		''' <summary>
		''' Renders a link button w/ an HtmlTextWriter that has a title.
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="URL">The url to navigate to when the link butotn is clicked.</param>
		''' <param name="Text">The string of text to dispaly to the user.</param>
		''' <param name="Css">The css class to apply to the link button.</param>
		''' <param name="Title">The hover over text to display to the user.</param>
		''' <remarks>Title was used to work across all browsers.</remarks>
		Protected Overloads Sub RenderTitleLinkButton(ByVal wr As HtmlTextWriter, ByVal URL As String, ByVal Text As String, ByVal Css As String, ByVal Title As String)
			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If

			wr.AddAttribute(HtmlTextWriterAttribute.Title, Title)
			wr.AddAttribute(HtmlTextWriterAttribute.Href, URL)
			wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
			wr.Write(Text)
			wr.RenderEndTag() ' </A>
		End Sub

		''' <summary>
		''' Creates a Link Button used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="URL">The url to navigate to when clicked.</param>
		''' <param name="Text">The string of text to display to the user.</param>
		''' <param name="Css">The css class to apply to the link button.</param>
		''' <param name="Enabled">If the link button should be clickable.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Protected Overloads Sub RenderLinkButton(ByVal wr As HtmlTextWriter, ByVal URL As String, ByVal Text As String, ByVal Css As String, ByVal Enabled As Boolean)
			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If

			If Enabled Then
				wr.AddAttribute(HtmlTextWriterAttribute.Href, URL)
			End If

			wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
			wr.Write(Text)
			wr.RenderEndTag() ' </A>
		End Sub

		''' <summary>
		''' Creates a Link Button used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="URL">The url to navigate to when the link is clicked.</param>
		''' <param name="Text">The string of text to show to the end user.</param>
		''' <param name="Css">The css class to apply to the link button.</param>
		''' <param name="Width">The width of the link button.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Protected Overloads Sub RenderLinkButton(ByVal wr As HtmlTextWriter, ByVal URL As String, ByVal Text As String, ByVal Css As String, ByVal Width As String)
			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If
			If Width.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Width, Width)
			End If
			wr.AddAttribute(HtmlTextWriterAttribute.Href, URL)
			wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
			wr.Write(Text)
			wr.RenderEndTag() ' </a>
		End Sub

		''' <summary>
		''' Creates a Link Button used w/ an HtmlTextWriter
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="URL">The url to navigate to when clicked.</param>
		''' <param name="Text">The string of text to show to the user.</param>
		''' <param name="Css">The css class to apply to the link button.</param>
		''' <param name="Width">The width of the link button.</param>
		''' <param name="TargetBlank">If the link should open a new browser window when clicked.</param>
		''' <param name="NoFollow">True if the link should not be followed by search engines.</param>
		''' <remarks>(Similar to a stringbuilder)</remarks>
		Protected Overloads Sub RenderLinkButton(ByVal wr As HtmlTextWriter, ByVal URL As String, ByVal Text As String, ByVal Css As String, ByVal Width As String, ByVal TargetBlank As Boolean, ByVal NoFollow As Boolean)
			If Css.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, Css)
			End If
			If Width.Length > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Width, Width)
			End If
			If TargetBlank Then
				wr.AddAttribute(HtmlTextWriterAttribute.Target, "_blank")
			End If
			If NoFollow Then
				wr.AddAttribute(HtmlTextWriterAttribute.Rel, "nofollow")
			End If

			wr.AddAttribute(HtmlTextWriterAttribute.Href, URL)
			wr.RenderBeginTag(HtmlTextWriterTag.A) ' <a>
			wr.Write(Text)
			wr.RenderEndTag() ' </a>
		End Sub

		''' <summary>
		''' Navigation bar (forum iconbar w/ images or text links) used in all views hosted in Forum_Container.ascx.
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <param name="objConfig">The forum's configuration settings.</param>
		''' <param name="ForumControl">The forum control loaded by the Forum_Container class that houses the UI being built here.</param>
		''' <remarks>This is somewhat centralized here as part of refactoring.</remarks>
		Protected Sub RenderNavBar(ByVal wr As HtmlTextWriter, ByVal objConfig As Forum.Configuration, ByVal ForumControl As Forum.DNNForum)
			RenderRowBegin(wr, "NavigationToolbar")	'<tr>

			' left cap
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td><img/></td>

			wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_IconTd")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td> 

			RenderDivBegin(wr, "divForumIcons", "Forum_Icons") ' <div>
			RenderDivBegin(wr, "divForumIconsCol", "Forum_IconsCol") ' <div>

			For Each Action As Entities.Modules.Actions.ModuleAction In ForumControl.NavigatorActions
				RenderLinkButton(wr, Action.Url, Action.Title, "Forum_ToolbarLink")
			Next

			RenderDivEnd(wr) ' </div> divForumIconsCol
			RenderDivEnd(wr) ' </div> divForumIcons

			RenderCellEnd(wr) ' </td>
			' right cap
			RenderCapCell(wr, objConfig.GetThemeImageURL("spacer.gif"), "", "")	' <td><img/></td>
			wr.RenderEndTag() ' </Tr>
			'wr.Write("<div style='clear:both;' />")
		End Sub

#End Region

#Region "Public Methods"

#Region "Constructors"

		''' <summary>
		''' Instantiates an instance of the class. 
		''' </summary>
		''' <param name="BaseControl">This is the forum view type being rendered.</param>
		''' <remarks>The allows this class to know what UI is being rendered.</remarks>
		Public Sub New(ByVal BaseControl As ForumBaseControl)
			_BaseControl = BaseControl
		End Sub

#End Region

		''' <summary>
		''' The create child controls part of the page life cycle.
		''' </summary>
		''' <remarks>Life cycle</remarks>
		Public Overridable Sub CreateChildControls()
			Controls.Clear()
		End Sub

		''' <summary>
		''' The On Pre Render part of the page life cycle.
		''' </summary>
		''' <remarks>Life cycle</remarks>
		Public Overridable Sub OnPreRender()
			' To permit ajax usage for some things, throw a script manager on the page
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterScriptManager()
			End If
		End Sub

		''' <summary>
		''' The Render part of the page life cycle.
		''' </summary>
		''' <param name="wr">The HtmlTextWriter being used.</param>
		''' <remarks>Life cycle</remarks>
		Public Overridable Sub Render(ByVal wr As HtmlTextWriter)
		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' Gets the ForumBaseControl object that represents the dynamically built user control. 
		''' </summary>
		''' <value></value>
		''' <returns>Returns the ForumBaseControl object which is a dynamically built user control.</returns>
		''' <remarks>This is the UI being rendered (ie. Posts, Threads, Posts, Search views)</remarks>
		Public ReadOnly Property BaseControl() As ForumBaseControl
			Get
				Return _BaseControl
			End Get
		End Property

		''' <summary>
		''' Gets a System.Web.UI.ControlCollection object that represents the child controls for a specified server control in the UI hierarchy.
		''' </summary>
		''' <value></value>
		''' <returns>A collection of controls.</returns>
		''' <remarks>The collection of child controls for the specified server control.</remarks>
		Public Overloads ReadOnly Property Controls() As ControlCollection
			Get
				Return _BaseControl.Controls
			End Get
		End Property

#End Region

	End Class

#End Region

End Namespace