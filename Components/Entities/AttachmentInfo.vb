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

	''' <summary>
	''' A single instance of the AttachmentInfo Object.
	''' </summary>
	''' <remarks>Added by Skeel</remarks>
	Public Class AttachmentInfo

#Region " Private Members "

		Private _AttachmentID As Integer
		Private _FileID As Integer
		Private _PostID As Integer
		Private _UserID As Integer
		Private _LocalFileName As String
		Private _FileName As String
		Private _Extension As String
		Private _Size As Integer
		Private _Inline As Boolean
		Private _Width As Integer
		Private _Height As Integer

#End Region

#Region " Public Properties "

		''' <summary>
		''' The PK AttachmentID associated with the file.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property AttachmentID() As Integer
			Get
				Return _AttachmentID
			End Get
			Set(ByVal Value As Integer)
				_AttachmentID = Value
			End Set
		End Property

		''' <summary>
		''' The DotNetNuke FileID.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>All attachments must be part of the DNN File system.</remarks>
		Public Property FileID() As Integer
			Get
				Return _FileID
			End Get
			Set(ByVal Value As Integer)
				_FileID = Value
			End Set
		End Property

		''' <summary>
		''' The PostID the attachment is associated with.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostID() As Integer
			Get
				Return _PostID
			End Get
			Set(ByVal Value As Integer)
				_PostID = Value
			End Set
		End Property

		''' <summary>
		''' UserID - we need this to collect "lost" attachments
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UserID() As Integer
			Get
				Return _UserID
			End Get
			Set(ByVal Value As Integer)
				_UserID = Value
			End Set
		End Property

		''' <summary>
		''' LocalFileName - Used for the user to identify the file uploaded
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LocalFileName() As String
			Get
				Return _LocalFileName
			End Get
			Set(ByVal Value As String)
				_LocalFileName = Value
			End Set
		End Property

		''' <summary>
		''' FileName - Actual FileName from DNN Files (generated through the GUID model)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property FileName() As String
			Get
				Return _FileName
			End Get
			Set(ByVal Value As String)
				_FileName = Value
			End Set
		End Property

		''' <summary>
		''' Extension - We use this display either image or link depending of extension
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Extension() As String
			Get
				Return _Extension
			End Get
			Set(ByVal Value As String)
				_Extension = Value
			End Set
		End Property

		''' <summary>
		''' Size - File size in bytes
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Size() As Integer
			Get
				Return _Size
			End Get
			Set(ByVal Value As Integer)
				_Size = Value
			End Set
		End Property

		''' <summary>
		''' Inline will return true, if the attachment is to be placed inline the postbody
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Inline() As Boolean
			Get
				Return _Inline
			End Get
			Set(ByVal Value As Boolean)
				_Inline = Value
			End Set
		End Property

		''' <summary>
		''' Width (Will only apply for image files)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Width() As Integer
			Get
				Return _Width
			End Get
			Set(ByVal Value As Integer)
				_Width = Value
			End Set
		End Property

		''' <summary>
		''' Height (Will only apply for image files)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Height() As Integer
			Get
				Return _Height
			End Get
			Set(ByVal Value As Integer)
				_Height = Value
			End Set
		End Property

#End Region

#Region " Public ReadOnly Properties "

		''' <summary>
		''' FormattedSize - File size in bytes
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property FormattedSize() As String
			Get
				Return FormatBytes(_Size)
			End Get
		End Property

#End Region

#Region " Private Methods "

		''' <summary>
		''' Formats an integer value presumed as bytes to bytes/KB/MB/GB
		''' </summary>
		''' <param name="Bytes"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function FormatBytes(ByVal Bytes As Integer) As String

			Dim strBytes As String = String.Empty
			Try
				If Bytes >= 1073741824 Then
					strBytes = Format(Bytes / 1024 / 1024 / 1024, "#0.00") & " GB"
				ElseIf Bytes >= 1048576 Then
					strBytes = Format(Bytes / 1024 / 1024, "#0.00") & " MB"
				ElseIf Bytes >= 1024 Then
					strBytes = Format(Bytes / 1024, "#0.00") & " KB"
				ElseIf Bytes < 1024 Then
					strBytes = Fix(Bytes) & " Bytes"
				End If
			Catch ex As Exception
				strBytes = "0 Bytes"
			End Try

			Return strBytes

		End Function


#End Region

	End Class

End Namespace