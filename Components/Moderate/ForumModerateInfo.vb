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

#Region "ModerateInfo"

    ''' <summary>
    ''' Info class is used to replicate forum info class but also has ability to render UI for part of Posts to moderate queue. 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	7/13/2005	Created
    ''' </history>
    Public Class ModerateForumInfo
        Inherits ForumInfo

#Region "Private Methods"

        ''' <summary>
        ''' Used for changing Css class in the UI, determines if the integer passed in is odd or even numbered.
        ''' </summary>
        ''' <param name="Count"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ThreadIsEven(ByVal Count As Integer) As Boolean
            If Count Mod 2 = 0 Then
                'even
                Return True
            Else
                'odd
                Return False
            End If
        End Function

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' This is a single forum instance rendered in the moderation queue view. each time this is called it builds one row in the table. 
        ''' </summary>
        ''' <param name="wr"></param>
        ''' <param name="ForumControl"></param>
        ''' <param name="Count"></param>
        ''' <remarks></remarks>
        Public Shadows Sub Render(ByVal wr As HtmlTextWriter, ByVal ForumControl As DNNForum, ByVal Count As Integer)
			Dim fPage As Page = ForumControl.DNNPage
			Dim fConfig As Forum.Config = ForumControl.objConfig
			Dim fTabID As Integer = ForumControl.TabID
			Dim fModuleID As Integer = ForumControl.ModuleID
			Dim url As String
			Dim even As Boolean = ThreadIsEven(Count)

			' New row
			wr.RenderBeginTag(HtmlTextWriterTag.Tr)	'<tr>

			' left cap
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Src, ForumControl.objConfig.GetThemeImageURL("spacer.gif"))
			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
			wr.RenderEndTag() ' </Img>

			wr.RenderEndTag() ' </td>

			'middle column
			wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
			wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

			wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
			wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>
			wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>

			If even Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Row")
			Else
				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Row_Alt")
			End If

			wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
			wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
			wr.AddAttribute(HtmlTextWriterAttribute.Width, "65%")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td>


			wr.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
			wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
			wr.RenderBeginTag(HtmlTextWriterTag.Table) ' <table>
			wr.RenderBeginTag(HtmlTextWriterTag.Tr)	' <tr>
			wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
			wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td>

			url = Utilities.Links.ContainerPostToModerateLink(ForumControl.TabID, ForumID, ModuleID)
			wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
			wr.RenderBeginTag(HtmlTextWriterTag.A) '<A>

			If PostsToModerate > 0 Then
				wr.AddAttribute(HtmlTextWriterAttribute.Src, ForumControl.objConfig.GetThemeImageURL("s_postunread.") & ForumControl.objConfig.ImageExtension)
			Else
				wr.AddAttribute(HtmlTextWriterAttribute.Src, ForumControl.objConfig.GetThemeImageURL("s_postread.") & ForumControl.objConfig.ImageExtension)
			End If
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <img>
			wr.RenderEndTag() ' </Img>
			wr.RenderEndTag() '  </A> 

			'spacer image
			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Src, ForumControl.objConfig.GetThemeImageURL("row_spacer.gif"))
			wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
			wr.RenderEndTag() ' </Img>

			wr.RenderEndTag() ' </td>

			wr.AddAttribute(HtmlTextWriterAttribute.Width, "100%")
			wr.AddAttribute(HtmlTextWriterAttribute.Valign, "top")
			wr.AddAttribute(HtmlTextWriterAttribute.Align, "left")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	'<td>

			wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>
			wr.Write("&nbsp;")
			wr.AddAttribute(HtmlTextWriterAttribute.Href, url)
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_NormalBold")
			wr.RenderBeginTag(HtmlTextWriterTag.A) '<A>
			wr.Write(Name)
			wr.RenderEndTag() '  </A> 
			wr.RenderEndTag() ' </Span>

			' Display forum description
			If Len(Description) > 0 Then
				wr.Write("<BR />")
				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Normal")
				wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <Span>
				wr.Write(Description)
				wr.RenderEndTag() '  </span>
			End If

			wr.RenderEndTag() ' </td>
			wr.RenderEndTag() ' </tr>
			wr.RenderEndTag() ' </table>

			wr.RenderEndTag() '  </td>

			If even Then
				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_RowHighlight3")
			Else
				wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_RowHighlight3_Alt")
			End If

			wr.AddAttribute(HtmlTextWriterAttribute.Width, "35%")
			wr.AddAttribute(HtmlTextWriterAttribute.Align, "center")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "Forum_Normal")
			wr.RenderBeginTag(HtmlTextWriterTag.Span) ' <span>
			wr.Write(PostsToModerate.ToString)
			wr.RenderEndTag() '  </span>
			wr.RenderEndTag() '  </td>

			wr.RenderEndTag() ' </Tr>
			wr.RenderEndTag() ' </Table>

			wr.RenderEndTag() '  </td>

			' right cap
			wr.AddAttribute(HtmlTextWriterAttribute.Class, "")
			wr.RenderBeginTag(HtmlTextWriterTag.Td)	' <td>

			wr.AddAttribute(HtmlTextWriterAttribute.Border, "0")
			wr.AddAttribute(HtmlTextWriterAttribute.Src, ForumControl.objConfig.GetThemeImageURL("spacer.gif"))
            wr.RenderBeginTag(HtmlTextWriterTag.Img) ' <Img>
            wr.RenderEndTag() ' </Img>

            wr.RenderEndTag() ' </td>

            wr.RenderEndTag() ' </tr>
        End Sub

#End Region

    End Class

#End Region

End Namespace