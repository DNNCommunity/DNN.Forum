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

Imports DotNetNuke.Entities.Modules

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' A re-usable control for handling the various avatar types in the forum module. 
	''' </summary>
	''' <remarks></remarks>
	Partial Public Class AvatarControl
		Inherits PortalModuleBase


#Region "Private Members"

		Private Const FileFilter As String = "jpg,gif,png"
		Private imageList() As String
		Private _BaseFolder As String = String.Empty
		Private _PoolFolder As String = String.Empty
		Private _MaxUploadSize As Integer = 0
		Private _ImageWidth As Integer = Null.NullInteger
		Private _ImageHeight As Integer = Null.NullInteger
		Private _localResourceFile As String = String.Empty
		Private _AvatarType As AvatarControlType = AvatarControlType.User
		Private _Security As Forum.ModuleSecurity

#End Region

#Region "Public Properties"

		''' <summary>
		''' We need the ModuleID set so we can get configuration settings for the avatar control. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>This is required to be set for proper integration.</remarks>
		Public Overloads Property ModuleId() As Integer
			Get
				If ViewState("ModuleID") IsNot Nothing Then
					Return CType(ViewState("ModuleID").ToString(), Integer)
				Else
					Return -1
				End If
			End Get
			Set(ByVal value As Integer)
				ViewState("ModuleID") = CInt(value)
			End Set
		End Property

		''' <summary>
		''' Local Resource file for localization. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Shadows Property LocalResourceFile() As String
			Get
				Dim fileRoot As String

				If _localResourceFile = String.Empty Then
					fileRoot = Me.TemplateSourceDirectory & "/" & Localization.LocalResourceDirectory & "/AvatarControl.ascx"
				Else
					fileRoot = _localResourceFile
				End If
				Return fileRoot
			End Get
			Set(ByVal Value As String)
				_localResourceFile = Value
			End Set
		End Property

		''' <summary>
		''' Type of avatar this will be displayed (configured) for. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property AvatarType() As AvatarControlType
			Get
				If ViewState("AvatarType") Is Nothing Then Return AvatarControlType.Role

				Return CType(ViewState("AvatarType").ToString(), AvatarControlType)
			End Get
			Set(ByVal value As AvatarControlType)
				ViewState("AvatarType") = CInt(value)
			End Set
		End Property

		''' <summary>
		''' The current list of images to bind to the control's datalist. Viewstate is used here because of Ajax and dynamically binding. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Images() As String
			Get
				If ViewState("ImageList") Is Nothing Then Return ""

				Return ViewState("ImageList").ToString()
			End Get
			Set(ByVal value As String)
				ViewState("ImageList") = value
			End Set
		End Property

		''' <summary>
		''' Determines if selected Image is from the Avatar Pool. Viewstate is used here because of Ajax and dynamically binding. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsPoolAvatar() As Boolean
			Get
				If ViewState("IsPoolAvatar") Is Nothing Then Return False

				Return CBool(ViewState("IsPoolAvatar").ToString())
			End Get
			Set(ByVal value As Boolean)
				ViewState("IsPoolAvatar") = value
			End Set
		End Property

		''' <summary>
		''' The security object is used to determine what avatar types the user can see. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Security() As Forum.ModuleSecurity
			Get
				Return _Security
			End Get
			Set(ByVal value As Forum.ModuleSecurity)
				_Security = value
			End Set
		End Property

		''' <summary>
		''' The user who's profile is being altered.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ProfileUserID() As Integer
			Get
				If ViewState("ProfileUserID") IsNot Nothing Then
					Return CType(ViewState("ProfileUserID").ToString(), Integer)
				Else
					Return -1
				End If
			End Get
			Set(ByVal value As Integer)
				ViewState("ProfileUserID") = CInt(value)
			End Set
		End Property

		''' <summary>
		''' Post portal root folder path setting.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Added by skeel</remarks>
		Private ReadOnly Property PoolFolder() As String
			Get
				If _PoolFolder = String.Empty Then
					_PoolFolder = objConfig.UserAvatarPoolPath
				End If

				If _PoolFolder.EndsWith("/") = False Then _PoolFolder += "/"

				Return _PoolFolder
			End Get
		End Property

#End Region

#Region "Private ReadOnly Properties"

		''' <summary>
		''' The user viewing this control. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property CurrentForumUser() As ForumUserInfo
			Get
				Dim cntForumUser As New ForumUserController
				Return cntForumUser.GetForumUser(Users.UserController.GetCurrentUserInfo.UserID, False, ModuleId, objConfig.CurrentPortalSettings.PortalId)
			End Get
		End Property

		''' <summary>
		''' This is the forum's configuration so it can be used by loaded controls.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property objConfig() As Forum.Configuration
			Get
				Return Configuration.GetForumConfig(ModuleId)
			End Get
		End Property

		''' <summary>
		''' Post portal root folder path setting.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property BaseFolder() As String
			Get
				If _BaseFolder = String.Empty Then
					Select Case AvatarType
						Case AvatarControlType.Role
							_BaseFolder = objConfig.RoleAvatarPath
						Case AvatarControlType.System
							_BaseFolder = objConfig.SystemAvatarPath
						Case AvatarControlType.User
							' may need to handle pool folder here
							If IsPoolAvatar And objConfig.EnableUserAvatarPool Then
								' We set to avatar pool folder here
								_BaseFolder = objConfig.UserAvatarPoolPath
							Else
								_BaseFolder = objConfig.UserAvatarPath
							End If
					End Select
					If _BaseFolder.EndsWith("/") = False Then _BaseFolder += "/"
				End If

				Return _BaseFolder
			End Get
		End Property

		''' <summary>
		''' Maximum size for an image to be uploaded. 
		''' </summary>
		''' <value></value>
		''' <returns>Integer representing file size in Kb.</returns>
		''' <remarks>Only used for user avatars.</remarks>
		Private ReadOnly Property MaxUploadSize() As Integer
			Get
				If AvatarType = AvatarControlType.User Then
					_MaxUploadSize = objConfig.UserAvatarMaxSize
				End If

				Return _MaxUploadSize
			End Get
		End Property

		''' <summary>
		''' Avatar width setting.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Only used for user avatars.</remarks>
		Private ReadOnly Property ImageWidth() As Integer
			Get
				If AvatarType = AvatarControlType.User Then
					_ImageWidth = objConfig.UserAvatarWidth
				End If

				Return _ImageWidth
			End Get
		End Property

		''' <summary>
		''' Avatar height setting.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Only used for user avatars.</remarks>
		Private ReadOnly Property ImageHeight() As Integer
			Get
				If AvatarType = AvatarControlType.User Then
					_ImageHeight = objConfig.UserAvatarHeight
				End If

				Return _ImageHeight
			End Get
		End Property

#End Region

#Region "Private Methods"

		''' <summary>
		''' This generates the thumbnail from the uploaded image based on parameters set in configuration. 
		''' </summary>
		''' <param name="SourceFileName"></param>
		''' <param name="DestFileName"></param>
		''' <remarks></remarks>
		Private Sub CreateThumbnail(ByVal SourceFileName As String, ByVal DestFileName As String)
			Try
				Dim srcImage As New System.Drawing.Bitmap(SourceFileName)

				If srcImage.Width > ImageWidth Or srcImage.Height > ImageHeight Then
					Dim newSize As New System.Drawing.Size(ImageWidth, ImageHeight)
					Dim aspectRatio As Double = srcImage.Width / srcImage.Height

					If aspectRatio < 1 Then
						newSize.Width = CInt(newSize.Width * aspectRatio)
					Else
						newSize.Height = CInt(newSize.Height / aspectRatio)
					End If

					Dim myPixelFormat As Drawing.Imaging.PixelFormat
					myPixelFormat = srcImage.PixelFormat

					Dim myImageFormat As Drawing.Imaging.ImageFormat
					myImageFormat = srcImage.RawFormat

					Dim newImg As New System.Drawing.Bitmap(newSize.Width, newSize.Height)
					Dim recDest As New Drawing.Rectangle(0, 0, newSize.Width, newSize.Height)
					Dim mGraphics As System.Drawing.Graphics = System.Drawing.Graphics.FromImage(newImg)

					mGraphics.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighQuality
					mGraphics.CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
					mGraphics.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
					mGraphics.PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality

					mGraphics.DrawImage(srcImage, recDest)
					'Releasing file
					srcImage.Dispose()
					mGraphics.Dispose()

					Try
						newImg.Save(DestFileName, myImageFormat)
					Catch ex As Exception
						Exceptions.LogException(ex)
					End Try
					'End If

					'Releasing resource
					newImg.Dispose()
				Else
					If Not SourceFileName = DestFileName Then
						' Just need to rename the image here, it is smaller than the size limitations
						FileSystemUtils.MoveFile(SourceFileName, DestFileName, objConfig.CurrentPortalSettings)
					End If
					srcImage.Dispose()
				End If

				Try
					If Not SourceFileName = DestFileName Then
						DotNetNuke.Common.Utilities.FileSystemUtils.DeleteFile(SourceFileName, objConfig.CurrentPortalSettings)
					End If
				Catch ex As Exception
					DotNetNuke.Services.Exceptions.LogException(ex)
				End Try
			Catch ex As Exception
				DotNetNuke.Services.Exceptions.LogException(ex)
			End Try
		End Sub

		''' <summary>
		''' Binds a list of available images to a datalist. 
		''' </summary>
		''' <remarks></remarks>
		Private Sub BindImages()
			imageList = Images.Trim(";"c).Split(";"c)
			If imageList.Length = 1 AndAlso imageList(0) = String.Empty Then
				dlAvatars.Visible = False
			Else
				dlAvatars.Visible = True
				dlAvatars.DataSource = imageList
				dlAvatars.DataBind()
			End If
		End Sub

		''' <summary>
		''' Deletes a previously uploaded avatar and saves the profile
		''' </summary>
		''' <remarks>Added by Skeel</remarks>
		Private Sub DeleteUploadedAvatarAndSaveProfile()
			Dim cntForumUser As New ForumUserController
			Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, objConfig.CurrentPortalSettings.PortalId)

			'Get the parent folder
			Dim ParentFolderName As String = objConfig.CurrentPortalSettings.HomeDirectoryMapPath
			ParentFolderName += objConfig.UserAvatarPath
			ParentFolderName = ParentFolderName.Replace("/", "\")
			If ParentFolderName.EndsWith("\") = False Then ParentFolderName += "\"

			'Get the file to delete
			Dim FileName As String = ProfileUser.Avatar
			FileName = FileName.Replace(";", "")

			Try
				DotNetNuke.Common.Utilities.FileSystemUtils.DeleteFile(ParentFolderName + FileName, objConfig.CurrentPortalSettings)
			Catch ex As Exception
				DotNetNuke.Services.Exceptions.LogException(ex)
			End Try

			'[skeel] Let's update the profile now, 
			With ProfileUser
				.UserAvatar = UserAvatarType.PoolAvatar
				.Avatar = Images
			End With

			Dim ctlForumUser As New ForumUserController
			ctlForumUser.Update(ProfileUser)

			DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(ProfileUserID, objConfig.CurrentPortalSettings.PortalId)
		End Sub

#End Region

#Region "Event Handlers"

		''' <summary>
		''' When the control is loaded, we want to make sure the cmdUpload is registered for postback because of the nature of the file upload control and security. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
			If DotNetNuke.Framework.AJAX.IsInstalled Then
				DotNetNuke.Framework.AJAX.RegisterPostBackControl(cmdUpload)
			End If
		End Sub

		''' <summary>
		''' Removes an avatar file from the disk and file system of core. 
		''' </summary>
		''' <param name="source"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dlAvatars_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlAvatars.DeleteCommand
			Dim lbl As Label = CType(e.Item.FindControl("lblImageName"), Label)
			ViewState("ImageList") = Images.Replace(lbl.Text + ";", "")
			BindImages()

			If AvatarType = AvatarControlType.User AndAlso IsPoolAvatar = False Then
				DeleteUploadedAvatarAndSaveProfile()
			End If

		End Sub

		''' <summary>
		''' Binds existing avatars to a datalist. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub dlAvatars_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlAvatars.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim lbl As Label = CType(e.Item.FindControl("lblImageName"), Label)
			Dim img As Image = CType(e.Item.FindControl("imgAvatar"), Image)
			Dim imgb As ImageButton = CType(e.Item.FindControl("imgDelete"), ImageButton)

			imgb.ToolTip = Localization.GetString("imgDelete", Me.LocalResourceFile)

			lbl.Text = imageList(e.Item.ItemIndex)
			If IsPoolAvatar Then
				img.ImageUrl = objConfig.CurrentPortalSettings.HomeDirectory + PoolFolder + lbl.Text
			Else
				img.ImageUrl = objConfig.CurrentPortalSettings.HomeDirectory + BaseFolder + lbl.Text
			End If
		End Sub

		''' <summary>
		''' This uploads a file which generates a GUID name, uses original image extension as save type. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Protected Sub cmdUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
			Try
				' if no file is selected exit
				If fuFile.PostedFile.FileName = String.Empty Then
					Exit Sub
				End If

				lblMessage.Text = String.Empty

				Dim ParentFolderName As String = objConfig.CurrentPortalSettings.HomeDirectoryMapPath
				Dim FileName As String = System.IO.Path.GetFileName(fuFile.PostedFile.FileName)
				Dim destFileName As String

				ParentFolderName += BaseFolder
				ParentFolderName = ParentFolderName.Replace("/", "\")
				If ParentFolderName.EndsWith("\") = False Then ParentFolderName += "\"

				Dim strExtension As String = Replace(IO.Path.GetExtension(fuFile.PostedFile.FileName), ".", "")
				If FileFilter <> String.Empty And InStr("," & FileFilter.ToLower, "," & strExtension.ToLower) = 0 Then
					' trying to upload a file not allowed for current filter
					lblMessage.Text = String.Format(Localization.GetString("UploadError", Me.LocalResourceFile), FileFilter, strExtension)
				End If

				If (fuFile.PostedFile.ContentLength / 1024) > MaxUploadSize AndAlso MaxUploadSize <> 0 Then
					lblMessage.Text += String.Format(Localization.GetString("MaxUploadSizeError", Me.LocalResourceFile), (fuFile.PostedFile.ContentLength / 1024), MaxUploadSize)
				End If

				If lblMessage.Text = String.Empty Then
					lblMessage.Text += FileSystemUtils.UploadFile(ParentFolderName, fuFile.PostedFile, False)
				End If

				If lblMessage.Text <> String.Empty Then Exit Sub

				Dim msgs(1)() As String
				If AvatarType = AvatarControlType.User Then
					destFileName = Guid.NewGuid().ToString().Replace("-", "") + "." + strExtension
				Else
					destFileName = FileName
				End If

				If ImageWidth > Null.NullInteger AndAlso ImageHeight > Null.NullInteger Then
					CreateThumbnail(ParentFolderName + FileName, ParentFolderName + destFileName)
				End If

				If AvatarType = AvatarControlType.User And IsPoolAvatar = False Then

					Dim oldFileName As String = Images.Trim(";"c)
					If oldFileName <> String.Empty Then
						Try
							DotNetNuke.Common.Utilities.FileSystemUtils.DeleteFile(ParentFolderName + oldFileName, objConfig.CurrentPortalSettings)
						Catch ex As Exception
							DotNetNuke.Services.Exceptions.LogException(ex)
						End Try
					End If
				End If

				'Make sure this is set for uploads, as u can't upload to the pool
				IsPoolAvatar = False

				If AvatarType = AvatarControlType.System Then
					'[skeel] Only SystemAvatars will allow multiple Avatars
					Images = Images + destFileName + ";"
				Else
					'[skeel] User or Role Avatar - Just one...
					Images = ";" + destFileName + ";"
				End If

				BindImages()

				'[skeel] In case of a User upload, we want to update the profile now, 
				'to ensure uploaded avatars are always deleted when not in use
				If AvatarType = AvatarControlType.User Then
					Dim cntForumUser As New ForumUserController
					Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, objConfig.CurrentPortalSettings.PortalId)

					With ProfileUser
						.UserAvatar = UserAvatarType.UserAvatar
						.Avatar = Images
					End With

					Dim ctlForumUser As New ForumUserController
					ctlForumUser.Update(ProfileUser)

					DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(ProfileUserID, objConfig.CurrentPortalSettings.PortalId)
				End If
			Catch exc As Exception
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

		'''' <summary>
		'''' This places the uploaded file into the avatar datalist
		'''' </summary>
		'''' <param name="sender"></param>
		'''' <param name="e"></param>
		'''' <remarks></remarks>
		'Protected Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
		'	If Images.IndexOf(";" + cboFiles.SelectedItem.Text + ";") <> -1 Then Exit Sub

		'	If Not AvatarType = AvatarControlType.User Then
		'		'Images += cboFiles.SelectedItem.Text + ";"
		'	Else
		'		'Images = ";" + cboFiles.SelectedItem.Text + ";"
		'		IsPoolAvatar = False
		'	End If

		'	BindImages()
		'End Sub

		''' <summary>
		''' Loads up the pool gallery
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Added by skeel</remarks>
		Protected Sub cmdBrowsePool_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdBrowsePool.Click
			tblMain.Visible = False
			tblAvatarPool.Visible = True

			'Figure out the size of the datalist
			dlAvatarPool.Width = (3 * ImageWidth) + 2 + 6 + 10 ' cellspacing + cellpadding for 3 columns
			dlAvatarPool.Height = (2 * ImageHeight) + 1 + 4 + 10 ' cellspacing + cellpadding for 2 columns
			dlAvatarPool.ItemStyle.Width = New Unit(ImageWidth)
			dlAvatarPool.ItemStyle.Height = New Unit(ImageHeight)

			'Add strings
			lblPoolHeader.Text = Localization.GetString("AvatarPoolHeader", Me.LocalResourceFile)
			cmdCancel.Text = Localization.GetString("cmdCancel", Me.LocalResourceFile)

			BottomPager.PageSize = 6
			If AvatarType = AvatarControlType.System Then
				BindData(BottomPager.PageSize, 1, BaseFolder)
			Else
				BindData(BottomPager.PageSize, 1, PoolFolder)
			End If
		End Sub

#End Region

#Region "Interfaces"

		''' <summary>
		''' This runs only the first time the control is loaded via Ajax. 
		''' </summary>
		''' <remarks></remarks>
		Public Sub LoadInitialView()
			'The current imagelist is already set by the parent
			BindImages()

			Select Case AvatarType
				Case AvatarControlType.Role
					trAvatarPool.Visible = False
					trFileUpload.Visible = True
				Case AvatarControlType.System
					trAvatarPool.Visible = False
					trFileUpload.Visible = True
				Case AvatarControlType.User
					' Allow user avatar upload?
					trFileUpload.Visible = objConfig.EnableUserAvatar

					'Display Pool for Users?
					If objConfig.EnableUserAvatarPool Then
						'Check if any avatars are available from the pool 
						Dim tmpArray As ArrayList = Common.Globals.GetFileList(objConfig.CurrentPortalSettings.PortalId, FileFilter, False, PoolFolder, False)
						If tmpArray.Count > 0 Then
							trAvatarPool.Visible = True
						Else
							trAvatarPool.Visible = False
						End If
					Else
						' we know it is a user avatar AND pool is disabled
						trAvatarPool.Visible = False
					End If
			End Select

			'Localization
			cmdUpload.Text = Localization.GetString("Upload", Me.LocalResourceFile)
			cmdBrowsePool.Text = Localization.GetString("cmdBrowsePool", Me.LocalResourceFile)
		End Sub

#End Region

#Region " Avatar Pool "

		''' <summary>
		''' Binds existing avatars to a datalist. 
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Added by skeel</remarks>
		Private Sub dlAvatarPool_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlAvatarPool.ItemDataBound
			If e.Item.ItemType <> ListItemType.AlternatingItem AndAlso e.Item.ItemType <> ListItemType.Item Then Exit Sub

			Dim ImageName As String = CType((CType(e.Item.DataItem, DataRowView)).Row.ItemArray(0), String)
			Dim imgb As ImageButton = CType(e.Item.FindControl("imgAvatar"), ImageButton)
			Dim img As Image = CType(e.Item.FindControl("imgBlank"), Image)
			Dim lbl As Label = CType(e.Item.FindControl("lblImageName"), Label)
			lbl.Text = ImageName

			If ImageName <> "spacer.gif" Then
				'If AvatarType = AvatarControlType.System Then
				imgb.ImageUrl = objConfig.CurrentPortalSettings.HomeDirectory + BaseFolder + ImageName
				'Else
				'	imgb.ImageUrl = PortalSettings.HomeDirectory + PoolFolder + ImageName
				'End If

				imgb.ToolTip = Localization.GetString("SelectAvatar", Me.LocalResourceFile)
				img.Visible = False
			Else
				img.ImageUrl = objConfig.GetThemeImageURL("spacer.gif", False)
				img.Height = New Unit(50)
				img.Width = New Unit(50)
				imgb.Visible = False
			End If
		End Sub

		''' <summary>
		''' Set the selected avatars to the dlAvatars view 
		''' </summary>
		''' <remarks>Added by skeel</remarks>
		Private Sub dlAvatarPool_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlAvatarPool.UpdateCommand
			Dim lbl As Label = CType(e.Item.FindControl("lblImageName"), Label)

			If AvatarType = UserAvatarType.UserAvatar Then
				IsPoolAvatar = True
				Images = ";" + lbl.Text + ";"
			Else
				IsPoolAvatar = False
				Images += lbl.Text + ";"
			End If
			BindImages()

			tblMain.Visible = True
			tblAvatarPool.Visible = False

			If AvatarType = AvatarControlType.User Then
				'[skeel] At this point, we need to check if the current avatar needs to be deleted
				Dim cntForumUser As New ForumUserController
				Dim ProfileUser As ForumUserInfo = cntForumUser.GetForumUser(ProfileUserID, False, ModuleId, objConfig.CurrentPortalSettings.PortalId)

				If ProfileUser.UserAvatar = UserAvatarType.UserAvatar Then
					DeleteUploadedAvatarAndSaveProfile()
				Else
					'[skeel] Let's update the profile now, 
					With ProfileUser
						.UserAvatar = UserAvatarType.PoolAvatar
						.Avatar = Images
					End With

					Dim ctlUser As New ForumUserController
					ctlUser.Update(ProfileUser)
					DotNetNuke.Modules.Forum.Components.Utilities.Caching.UpdateUserCache(ProfileUserID, objConfig.CurrentPortalSettings.PortalId)
				End If
			End If
		End Sub

		''' <summary>
		''' Handle the Ajax pager
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks>Added by skeel</remarks>
		Private Sub pager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles BottomPager.Command
			Dim CurrentPage As Int32 = CType(e.CommandArgument, Int32)
			BottomPager.CurrentPage = CurrentPage
			'If AvatarType = AvatarControlType.System Then
			BindData(BottomPager.PageSize, CurrentPage, BaseFolder)
			'Else
			'	BindData(BottomPager.PageSize, CurrentPage, PoolFolder)
			'End If
		End Sub

		''' <summary>
		''' Binds the Avatar Pool Gallery 
		''' </summary>
		''' <param name="CurrentPage"></param>
		''' <param name="PageSize"></param>
		''' <remarks>Added by skeel</remarks>
		Private Sub BindData(ByVal PageSize As Integer, ByVal CurrentPage As Integer, ByVal Folder As String)
			Dim NoneSpecified As Boolean = False

			Dim arr As ArrayList = Common.Globals.GetFileList(objConfig.CurrentPortalSettings.PortalId, FileFilter, NoneSpecified, Folder, False)
			BottomPager.TotalRecords = arr.Count

			Dim dsTemp As DataSet = New DataSet()
			Dim Tables As DataTable = New DataTable()
			dsTemp.Tables.Add(Tables)

			dsTemp.Tables(0).Columns.Add("Text", System.Type.GetType("System.String"))
			dsTemp.Tables(0).Columns.Add("Value", System.Type.GetType("System.String"))

			Dim str As Common.FileItem
			Dim i As Integer = 0
			Dim intFrom As Integer = (CurrentPage - 1) * PageSize
			Dim intTo As Integer = (intFrom + PageSize) - 1

			For Each str In arr
				If i >= intFrom And i <= intTo Then
					Dim myRow As DataRow = dsTemp.Tables(0).NewRow
					myRow(0) = str.Text
					myRow(1) = str.Value
					dsTemp.Tables(0).Rows.Add(myRow)
				End If
				i = i + 1
			Next

			'Fill up with blank images for the sake of layout
			If dsTemp.Tables(0).Rows.Count < PageSize Then
				Dim add As Integer = (PageSize - dsTemp.Tables(0).Rows.Count)
				i = 1
				Do While i <= add
					Dim myRow As DataRow = dsTemp.Tables(0).NewRow
					myRow(0) = "spacer.gif"
					myRow(1) = "none"
					dsTemp.Tables(0).Rows.Add(myRow)
					i = i + 1
				Loop
			End If

			dlAvatarPool.DataSource = dsTemp
			dlAvatarPool.DataBind()
		End Sub

		''' <summary>
		''' Cancel the avatar pool browsing 
		''' </summary>
		''' <remarks>Added by skeel</remarks>
		Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
			tblMain.Visible = True
			tblAvatarPool.Visible = False
		End Sub

#End Region

	End Class

End Namespace