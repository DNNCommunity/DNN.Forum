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

Imports System
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.Collections.Generic

Namespace DotNetNuke.Modules.Forum.WebControls

    <ToolboxData("<{0}:AjaxPager runat=""server""></{0}:AjaxPager>")> _
    Public Class AjaxPager
        Inherits WebControl
        Implements IPostBackEventHandler
        Implements INamingContainer

#Region "Event Handling"

        Private Shared ReadOnly EventCommand As Object = New Object()

        Public Custom Event Command As CommandEventHandler
            RaiseEvent(ByVal sender As Object, ByVal value As CommandEventArgs)
                ' RaisePostBackEvent()
            End RaiseEvent
            AddHandler(ByVal value As CommandEventHandler)
                Events.[AddHandler](EventCommand, value)
            End AddHandler
            RemoveHandler(ByVal value As CommandEventHandler)
                Events.[RemoveHandler](EventCommand, value)
            End RemoveHandler
        End Event

        Protected Overridable Sub OnCommand(ByVal e As CommandEventArgs)
            Dim clickHandler As CommandEventHandler = CType(Events(EventCommand), CommandEventHandler)
            If Not clickHandler Is Nothing Then
                clickHandler(Me, e)
            End If
        End Sub

        Sub RaisePostBackEvent(ByVal eventArgument As String) Implements IPostBackEventHandler.RaisePostBackEvent
            OnCommand(New CommandEventArgs(Me.UniqueID, Convert.ToInt32(eventArgument)))
        End Sub

#End Region

#Region "Private Members"

        Private _ShowFirstAndLastLinks As Boolean = True
        Private _CompactedPageCount As Integer = 5
        Private _NotCompactedPageCount As Integer = 10

        Private _maxSmartShortCutCount As Integer = 5
        Private _enableSSC As Boolean = False
        Private _sscRatio As Double = 3
        Private _sscThreshold As Integer = 30

        Private _AltEnabled As Boolean = True
        Private _SmartShortCutList As List(Of Integer)
        Private _RightToLeft As Boolean = False
        Private _ResourceFile As String = ApplicationPath & "/DesktopModules/Forum" & "/App_LocalResources/SharedResources.resx"

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The total number of records returned that this pager is bound to. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>This </remarks>
        <Browsable(False)> _
        Public Property TotalRecords() As Integer
            Get
                Return Convert.ToInt32(ViewState("Forum_TotalRecords"))
            End Get
            Set(ByVal value As Integer)
                ViewState("Forum_TotalRecords") = value
            End Set
        End Property

        ''' <summary>
        ''' The current page the user is viewing.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public Property CurrentPage() As Integer
            Get
                If ViewState("Forum_CurrentPage") Is Nothing Then
                    ViewState("Forum_CurrentPage") = 1
                    Return 1
                Else
                    Return Convert.ToInt32(ViewState("Forum_CurrentPage"))
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("Forum_CurrentPage") = value
            End Set
        End Property

        ''' <summary>
        ''' The number of items per page to display to the end user.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public Property PageSize() As Integer
            Get
                If ViewState("Forum_PageSize") Is Nothing Then
                    ViewState("Forum_PageSize") = 10
                    Return 10
                Else
                    Return Convert.ToInt32(ViewState("Forum_PageSize"))
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState("Forum_PageSize") = value
            End Set
        End Property

        ''' <summary>
		''' Determines if the pager should show "First" and "Last" Utilities.Links.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Category("Behavior")> _
        Public Property ShowFirstAndLastLinks() As Boolean
            Get
                Return _ShowFirstAndLastLinks
            End Get
            Set(ByVal value As Boolean)
                _ShowFirstAndLastLinks = value
            End Set
        End Property

        ' first compacted group of visible page numbers
        <Category("Behavior")> _
        Public Property CompactedPageCount() As Integer
            Get
                Return _CompactedPageCount
            End Get
            Set(ByVal value As Integer)
                _CompactedPageCount = value
            End Set
        End Property

        ' ordinary not compacted visible page numbers count
        <Category("Behavior")> _
        Public Property NotCompactedPageCount() As Integer
            Get
                Return _NotCompactedPageCount
            End Get
            Set(ByVal value As Integer)
                _NotCompactedPageCount = value
            End Set
        End Property

        ' to enable/disable smart shortcuts
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Category("Behavior")> _
        Public Property EnableSmartShortCuts() As Boolean
            Get
                Return _enableSSC
            End Get
            Set(ByVal value As Boolean)
                _enableSSC = value
            End Set
        End Property

        ' the ration to count the space whithin the smartshortcut pages
        <Category("Behavior")> _
        Public Property SmartShortCutRatio() As Double
            Get
                Return _sscRatio
            End Get
            Set(ByVal value As Double)
                _sscRatio = value
            End Set
        End Property

        ' maximum number of smart shortcuts
        <Category("Behavior")> _
        Public Property MaxSmartShortCutCount() As Integer
            Get
                Return _maxSmartShortCutCount
            End Get
            Set(ByVal value As Integer)
                _maxSmartShortCutCount = value
            End Set
        End Property

        ' the number which determines that the smart short cuts must be rendered if pagecount is morethatn specific number
        <Category("Behavior")> _
        Public Property SmartShortCutThreshold() As Integer
            Get
                Return _sscThreshold
            End Get
            Set(ByVal value As Integer)
                _sscThreshold = value
            End Set
        End Property

        ''' <summary>
        ''' Determines if pager should show alt text (tooltip) when hovering over a pager item.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Category("Behavior")> _
        Public Property AltTextEnabled() As Boolean
            Get
                Return _AltEnabled
            End Get
            Set(ByVal value As Boolean)
                _AltEnabled = value
            End Set
        End Property

#End Region

#Region "ReadOnly Properties"

        ''' <summary>
        ''' The number of pages available to the user. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Browsable(False)> _
        Public ReadOnly Property PageCount() As Integer
            Get
                Return CInt(System.Math.Ceiling(TotalRecords / PageSize))
            End Get
        End Property

#End Region

#Region "Private Properties"

        Private Property SmartShortCutList() As List(Of Integer)
            Get
                Return _SmartShortCutList
            End Get
            Set(ByVal value As List(Of Integer))
                _SmartShortCutList = value
            End Set
        End Property

#End Region

#Region "Localization"

        Private _GO As String = "go"
        Private _OF As String = Services.Localization.Localization.GetString("of", _ResourceFile)
        Private _FROM As String = "From"
        Private _PAGE As String = Services.Localization.Localization.GetString("Page", _ResourceFile)
        Private _TO As String = Services.Localization.Localization.GetString("to", _ResourceFile)
        Private _SHOWING_RESULT As String = "Showing Results"
        Private _SHOW_RESULT As String = "Show Result"
        Private _BACK_TO_FIRST As String = "to First Page"
        Private _GO_TO_LAST As String = "to Last Page"
        Private _BACK_TO_PAGE As String = "Back to Page"
        Private _NEXT_TO_PAGE As String = "Next to Page"
        Private _LAST As String = Services.Localization.Localization.GetString("Last", DotNetNuke.Services.Localization.Localization.SharedResourceFile)
        Private _FIRST As String = Services.Localization.Localization.GetString("First", DotNetNuke.Services.Localization.Localization.SharedResourceFile)
        Private _previous As String = Services.Localization.Localization.GetString("Previous", DotNetNuke.Services.Localization.Localization.SharedResourceFile)
        Private _next As String = Services.Localization.Localization.GetString("Next", DotNetNuke.Services.Localization.Localization.SharedResourceFile)

        <Category("GlobalizaionSettings")> _
        Public Property GoClause() As String
            Get
                Return _GO
            End Get
            Set(ByVal value As String)
                _GO = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property OfClause() As String
            Get
                Return _OF
            End Get
            Set(ByVal value As String)
                _OF = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property FromClause() As String
            Get
                Return _FROM
            End Get
            Set(ByVal value As String)
                _FROM = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property PageClause() As String
            Get
                Return _PAGE
            End Get
            Set(ByVal value As String)
                _PAGE = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property ToClause() As String
            Get
                Return _TO
            End Get
            Set(ByVal value As String)
                _TO = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property ShowingResultClause() As String
            Get
                Return _SHOWING_RESULT
            End Get
            Set(ByVal value As String)
                _SHOWING_RESULT = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property ShowResultClause() As String
            Get
                Return _SHOW_RESULT
            End Get
            Set(ByVal value As String)
                _SHOW_RESULT = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property BackToFirstClause() As String
            Get
                Return _BACK_TO_FIRST
            End Get
            Set(ByVal value As String)
                _BACK_TO_FIRST = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property GoToLastClause() As String
            Get
                Return _GO_TO_LAST
            End Get
            Set(ByVal value As String)
                _GO_TO_LAST = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property BackToPageClause() As String
            Get
                Return _BACK_TO_PAGE
            End Get
            Set(ByVal value As String)
                _BACK_TO_PAGE = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property NextToPageClause() As String
            Get
                Return _NEXT_TO_PAGE
            End Get
            Set(ByVal value As String)
                _NEXT_TO_PAGE = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property LastClause() As String
            Get
                Return _LAST
            End Get
            Set(ByVal value As String)
                _LAST = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property FirstClause() As String
            Get
                Return _FIRST
            End Get
            Set(ByVal value As String)
                _FIRST = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property PreviousClause() As String
            Get
                Return _previous
            End Get
            Set(ByVal value As String)
                _previous = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property NextClause() As String
            Get
                Return _next
            End Get
            Set(ByVal value As String)
                _next = value
            End Set
        End Property

        <Category("GlobalizaionSettings")> _
        Public Property RTL() As Boolean
            Get
                Return _RightToLeft
            End Get
            Set(ByVal value As Boolean)
                _RightToLeft = value
            End Set
        End Property

#End Region

#Region "Render Utils"

        Private Function GenerateAltMessage(ByVal desiredPage As Integer) As String
            Dim altGen As New StringBuilder()
            altGen.Append(IIf(desiredPage = CurrentPage, ShowingResultClause, ShowResultClause))
            altGen.Append(" ")
            altGen.Append(((desiredPage - 1) * PageSize) + 1)
            altGen.Append(" ")
            altGen.Append(ToClause)
            altGen.Append(" ")
            altGen.Append(IIf(desiredPage = PageCount, TotalRecords, desiredPage * PageSize))
            altGen.Append(" ")
            altGen.Append(OfClause)
            altGen.Append(" ")
            altGen.Append(TotalRecords)

            Return altGen.ToString()
        End Function

        ''' <summary>
        ''' The localized text to display as a tooltip for AltText.
        ''' </summary>
        ''' <param name="index"></param>
        ''' <returns>The localized string.</returns>
        ''' <remarks></remarks>
        Private Function GetAlternativeText(ByVal index As Integer) As String
            If AltTextEnabled Then
                Return String.Format(" title=""{0}""", GenerateAltMessage(index))
            Else
                Return ""
            End If
        End Function

        Private Function RenderFirst() As String
            Dim templateCell As String = "<td class=""PagerOtherPageCells""><a class=""PagerHyperlinkStyle"" href=""{0}"" title=""" + " " + BackToFirstClause + " " + """> " + FirstClause + " </a></td>"
            ' string templateURL = String.Format(PageURLFormat, "1");
            Return [String].Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(Me, "1"))
        End Function

        Private Function RenderLast() As String
            Dim templateCell As String = "<td class=""PagerOtherPageCells""><a class=""PagerHyperlinkStyle"" href=""{0}"" title=""" + " " + GoToLastClause + " " + """> " + LastClause + " </a></td>"
            ' string templateURL = String.Format(PageURLFormat, PageCount.ToString());
            Return [String].Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(Me, PageCount.ToString()))
        End Function

        Private Function RenderBack() As String
            Dim templateCell As String = "<td class=""PagerOtherPageCells""><a class=""PagerHyperlinkStyle"" href=""{0}"" title=""" + " " + BackToPageClause + " " + (CurrentPage - 1).ToString() + """> " + PreviousClause + " </a></td>"
            ' string templateURL = String.Format(PageURLFormat, (CurrentIndex - 1).ToString());
            Return [String].Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(Me, (CurrentPage - 1).ToString()))
        End Function

        Private Function RenderNext() As String
            Dim templateCell As String = "<td class=""PagerOtherPageCells""><a class=""PagerHyperlinkStyle"" href=""{0}"" title=""" + " " + NextToPageClause + " " + (CurrentPage + 1).ToString() + """> " + NextClause + " </a></td>"
            ' string templateURL = String.Format(PageURLFormat, (CurrentIndex + 1).ToString());
            Return [String].Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(Me, (CurrentPage + 1).ToString()))
        End Function

        Private Function RenderCurrent() As String
            ' string altMessage = AlternativeTextEnabled ? string.Format(" title=\"{0}\"", GenerateAltMessage(CurrentIndex)) : "";
            Return "<td class=""PagerCurrentPageCell""><span class=""PagerHyperlinkStyle"" " + GetAlternativeText(CurrentPage) + " ><strong> " + CurrentPage.ToString() + " </strong></span></td>"
        End Function

        Private Function RenderOther(ByVal index As Integer) As String
            Dim templateCell As String = "<td class=""PagerOtherPageCells""><a class=""PagerHyperlinkStyle"" href=""{0}"" " + GetAlternativeText(index) + " > " + index.ToString() + " </a></td>"
            ' string templateURL = String.Format(PageURLFormat, index.ToString());
            Return [String].Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(Me, index.ToString()))
        End Function

        Private Function RenderSSC(ByVal index As Integer) As String
            Dim templateCell As String = "<td class=""PagerSSCCells""><a class=""PagerHyperlinkStyle"" href=""{0}"" " + GetAlternativeText(index) + " > " + index.ToString() + " </a></td>"
            ' string templateURL = String.Format(PageURLFormat, index.ToString());
            Return [String].Format(templateCell, Page.ClientScript.GetPostBackClientHyperlink(Me, index.ToString()))
        End Function

#End Region

#Region "Shortcuts"

        Private Sub CalculateSmartShortcutAndFillList()
            _SmartShortCutList = New List(Of Integer)()
            Dim shortCutCount As Double = Me.PageCount * SmartShortCutRatio / 100
            Dim shortCutCountRounded As Double = System.Math.Round(shortCutCount, 0)
            If shortCutCountRounded > MaxSmartShortCutCount Then
                shortCutCountRounded = MaxSmartShortCutCount
            End If
            'Possible Issue
            If shortCutCountRounded = 1 Then
                shortCutCountRounded += shortCutCountRounded
            End If

            Dim i As Integer = 1
            While i < shortCutCountRounded + 1
                'Possible Issue
                Dim calculatedValue As Integer = CInt((System.Math.Round((Me.PageCount * (100 / shortCutCountRounded) * i / 100) * 0.1, 0) * 10))
                If calculatedValue >= Me.PageCount Then
                    Exit While
                End If
                SmartShortCutList.Add(calculatedValue)
                System.Math.Max(System.Threading.Interlocked.Increment(i), i - 1)
            End While
        End Sub

        ' smart shortcut list calculator and list 
        Private Sub RenderSmartShortCutByCriteria(ByVal basePageNumber As Integer, ByVal getRightBand As Boolean, ByVal writer As HtmlTextWriter)
            If IsSmartShortCutAvailable() Then
                Dim lstSSC As List(Of Integer) = Me.SmartShortCutList
                Dim rVal As Integer = -1
                Dim i As Integer = 0

                If getRightBand Then
                    i = 0
                    While i < lstSSC.Count
                        If lstSSC(i) > basePageNumber Then
                            rVal = i
                            ' sometimes we dont reach here and inappropriate ssc show's up
                            ' allowRender = true;
                            Exit While
                        End If
                        i += 1
                    End While

                    If rVal >= 0 Then
                        i = rVal
                        While i < lstSSC.Count
                            If lstSSC(i) <> basePageNumber Then
                                writer.Write(RenderSSC(lstSSC(i)))
                            End If
                            i += 1
                        End While
                    End If
                ElseIf Not getRightBand Then
                    i = 0
                    While i < lstSSC.Count
                        If basePageNumber > lstSSC(i) Then
                            rVal = i
                        End If
                        i += 1
                    End While

                    If rVal >= 0 Then
                        i = 0
                        While i < rVal + 1
                            If lstSSC(i) <> basePageNumber Then
                                writer.Write(RenderSSC(lstSSC(i)))
                            End If
                            i += 1
                        End While
                    End If
                End If
            End If
        End Sub

        'Potential
        Function IsSmartShortCutAvailable() As Boolean
            If Not Me.SmartShortCutList Is Nothing Then
                If Me.SmartShortCutList.Count > 0 Then
                    Return Me.EnableSmartShortCuts
                End If
            End If
        End Function

#End Region

        ' Potential For issues
        Protected Overloads Overrides Sub Render(ByVal writer As HtmlTextWriter)
            If Page Is Nothing Then
                Page.VerifyRenderingInServerForm(Me)
            End If

            If Me.PageCount > Me.SmartShortCutThreshold Then
                If EnableSmartShortCuts Then
                    CalculateSmartShortcutAndFillList()
                End If
            End If

            writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0")
            writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0")
            writer.AddAttribute(HtmlTextWriterAttribute.Border, "0")
            writer.AddAttribute(HtmlTextWriterAttribute.[Class], "PagerContainerTable")
            If RTL Then
                writer.AddAttribute(HtmlTextWriterAttribute.Dir, "rtl")
            End If
            writer.RenderBeginTag(HtmlTextWriterTag.Table)

            writer.RenderBeginTag(HtmlTextWriterTag.Tr)
            writer.AddAttribute(HtmlTextWriterAttribute.[Class], "PagerInfoCell")
            writer.RenderBeginTag(HtmlTextWriterTag.Td)
            writer.Write(PageClause + " " + CurrentPage.ToString() + " " + OfClause + " " + PageCount.ToString())
            writer.RenderEndTag()

            If ShowFirstAndLastLinks AndAlso CurrentPage <> 1 Then
                writer.Write(RenderFirst())
            End If

            If CurrentPage <> 1 Then
                writer.Write(RenderBack())
            End If

            If CurrentPage < CompactedPageCount Then
                If CompactedPageCount > PageCount Then
                    CompactedPageCount = PageCount
                End If

                Dim i As Integer
                For i = 1 To (CompactedPageCount + 1) - 1
                    If i = CurrentPage Then
                        writer.Write(RenderCurrent())
                    Else
                        writer.Write(RenderOther(i))
                    End If
                Next i

                RenderSmartShortCutByCriteria(CompactedPageCount, True, writer)
            ElseIf CurrentPage >= CompactedPageCount And CurrentPage < NotCompactedPageCount Then

                If NotCompactedPageCount > PageCount Then
                    NotCompactedPageCount = PageCount
                End If
                Dim i As Integer
                For i = 1 To (NotCompactedPageCount + 1) - 1
                    If i = CurrentPage Then
                        writer.Write(RenderCurrent())
                    Else
                        writer.Write(RenderOther(i))
                    End If
                Next i

                RenderSmartShortCutByCriteria(NotCompactedPageCount, True, writer)
            ElseIf CurrentPage >= NotCompactedPageCount Then
                Dim gapValue As Integer = CInt(NotCompactedPageCount / 2)
                Dim leftBand As Integer = CurrentPage - gapValue
                Dim rightBand As Integer = CurrentPage + gapValue

                RenderSmartShortCutByCriteria(leftBand, False, writer)

                Dim i As Integer
                For i = leftBand To (PageCount + 1)
                    If i = CurrentPage Then
                        writer.Write(RenderCurrent())
                    Else
                        writer.Write(RenderOther(i))
                    End If
                Next i

                If rightBand < Me.PageCount Then
                    RenderSmartShortCutByCriteria(rightBand, True, writer)
                End If
            End If

            If CurrentPage <> PageCount Then
                writer.Write(RenderNext())
            End If

            If ShowFirstAndLastLinks AndAlso CurrentPage <> PageCount Then
                writer.Write(RenderLast())
            End If

            writer.RenderEndTag()
            writer.RenderEndTag()
            MyBase.Render(writer)
        End Sub

    End Class

End Namespace
