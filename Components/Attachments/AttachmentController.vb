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

#Region " AttachmentController "

    ''' <summary>
    ''' CRUD (and all db methods) Attachment Methods
    ''' </summary>
    ''' <remarks>Added by Skeel</remarks>
    Public Class AttachmentController

#Region " Constructors "

        ''' <summary>
        ''' Instantiats the AttachmentContoller. 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

#End Region

#Region " Private Properties "

		Private mCompleteAttachmentPath As String = String.Empty

        Private Property CompleteAttachmentPath() As String
            Get
                Return mCompleteAttachmentPath
            End Get
            Set(ByVal value As String)
                mCompleteAttachmentPath = value
            End Set
        End Property

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Gets a list of AttachmentInfo related to a Post
        ''' </summary>
        ''' <param name="PostID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAllByPostID(ByVal PostID As Integer) As List(Of AttachmentInfo)

            Dim objAttachments As New List(Of AttachmentInfo)
			Dim dr As IDataReader = Nothing
            Try
                dr = DotNetNuke.Modules.Forum.DataProvider.Instance().Attachment_GetAllByPostID(PostID)
                While dr.Read
                    Dim objAttachment As AttachmentInfo = FillAttachmentInfo(dr)
                    objAttachments.Add(objAttachment)
                End While
                dr.NextResult()

            Catch ex As Exception
                LogException(ex)
            Finally
                If Not dr Is Nothing Then
                    dr.Close()
                End If
            End Try

            Return objAttachments

        End Function

        ''' <summary>
        ''' Gets a list of AttachmentInfo related to a UserId and NOT related to any posts
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Function GetAllByUserID(ByVal UserID As Integer) As List(Of AttachmentInfo)
			Dim objAttachments As New List(Of AttachmentInfo)
			Dim dr As IDataReader = Nothing

			Try
				dr = DotNetNuke.Modules.Forum.DataProvider.Instance().Attachment_GetAllByUserID(UserID)
				While dr.Read
					Dim objAttachment As AttachmentInfo = FillAttachmentInfo(dr)
					objAttachments.Add(objAttachment)
				End While
				dr.NextResult()

			Catch ex As Exception
				LogException(ex)
			Finally
				If Not dr Is Nothing Then
					dr.Close()
				End If
			End Try

			Return objAttachments

		End Function

        ''' <summary>
        ''' Adds an AttachmentInfo object
        ''' </summary>
        ''' <param name="objAttachment"></param>
        ''' <remarks></remarks>
        Public Sub Update(ByVal objAttachment As AttachmentInfo)
            DotNetNuke.Modules.Forum.DataProvider.Instance().Attachment_Update(objAttachment)
        End Sub

#End Region

#Region " Custom Hydrator "

        ''' <summary>
        ''' Hydrates the AttachmentInfo object
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[skeel]	12/22/2008	Created
        ''' </history>
        Private Function FillAttachmentInfo(ByVal dr As IDataReader) As AttachmentInfo
            Dim objAttachment As New AttachmentInfo
            Try
                objAttachment.FileID = Convert.ToInt32(Null.SetNull(dr("FileID"), objAttachment.FileID))
            Catch
            End Try
            Try
                objAttachment.PostID = Convert.ToInt32(Null.SetNull(dr("PostID"), objAttachment.PostID))
            Catch
            End Try
            Try
                objAttachment.UserID = Convert.ToInt32(Null.SetNull(dr("UserID"), objAttachment.UserID))
            Catch
            End Try
            Try
                objAttachment.LocalFileName = Convert.ToString(Null.SetNull(dr("LocalFileName"), objAttachment.LocalFileName))
            Catch
            End Try
            Try
                objAttachment.FileName = Convert.ToString(Null.SetNull(dr("FileName"), objAttachment.FileName))
            Catch
            End Try
            Try
                objAttachment.Extension = Convert.ToString(Null.SetNull(dr("Extension"), objAttachment.Extension))
            Catch
            End Try
            Try
                objAttachment.Size = Convert.ToInt32(Null.SetNull(dr("Size"), objAttachment.Size))
            Catch
            End Try
            Try
                objAttachment.Inline = Convert.ToBoolean(Null.SetNull(dr("Inline"), objAttachment.Inline))
            Catch
            End Try
            Try
                objAttachment.Width = Convert.ToInt32(Null.SetNull(dr("Width"), objAttachment.Width))
            Catch
            End Try
            Try
                objAttachment.Height = Convert.ToInt32(Null.SetNull(dr("Height"), objAttachment.Height))
            Catch
            End Try

            Return objAttachment
        End Function

#End Region

    End Class

#End Region

End Namespace