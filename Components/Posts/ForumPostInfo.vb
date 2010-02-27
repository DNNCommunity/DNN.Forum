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

#Region "PostInfo"

	''' <summary>
	''' Creates an instance of the post info object
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	7/13/2005	Created
	''' </history>
	Public Class PostInfo

#Region "Private Members"

		Private Const PostInfoCacheKeyPrefix As String = "PostInfo"
		Private Const PostInfoCacheTimeout As Integer = 20
		Dim _PostId As Integer
		Dim _ParentPostId As Integer
		Dim _UserId As Integer
		Dim _RemoteAddr As String = String.Empty
		Dim _Notify As Boolean
		Dim _Subject As String = String.Empty
		Dim _Body As String = String.Empty
		Dim _CreatedDate As DateTime
		Dim _ThreadID As Integer
		Dim _PostLevel As Integer
		Dim _TreeSortOrder As Integer
		Dim _FlatSortOrder As Integer
		Dim _UpdatedDate As DateTime
		Dim _UpdatedByUser As Integer
		Dim _IsApproved As Boolean
		Dim _IsLocked As Boolean
		Dim _IsClosed As Boolean
		Dim _FileAttachmentURL As String = String.Empty
		Dim _FileAttachmentName As String = String.Empty
		Dim _PostReported As Integer = 0
		Dim _Addressed As Integer = 0
		Dim _Attachments As List(Of AttachmentInfo)
		Dim _ParseInfo As Integer = 0

#End Region

#Region "Public Methods"

#Region "Constructors"

		''' <summary>
		''' 
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region

		''' <summary>
		''' Gets the post info object, first checks for it in cache
		''' </summary>
		''' <param name="PostID"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function GetPostInfo(ByVal PostID As Integer, ByVal PortalID As Integer) As PostInfo
			Dim strCacheKey As String = PostInfoCacheKeyPrefix & CStr(PostID)
			Dim objPost As PostInfo = CType(DataCache.GetCache(strCacheKey), PostInfo)

			If objPost Is Nothing Then
				'post caching settings
				Dim timeOut As Int32 = PostInfoCacheTimeout * Convert.ToInt32(Entities.Host.Host.PerformanceSetting)

				Dim ctlPost As New PostController
				objPost = ctlPost.PostGet(PostID, PortalID)

				'Cache Post if timeout > 0 and Post is not null
				If timeOut > 0 And objPost IsNot Nothing Then
					DataCache.SetCache(strCacheKey, objPost, TimeSpan.FromMinutes(timeOut))
				End If
			End If

			Return objPost
		End Function

		''' <summary>
		''' Resets the post info object in cahce to nothing
		''' </summary>
		''' <param name="PostID"></param>
		''' <remarks>
		''' </remarks>
		Public Shared Sub ResetPostInfo(ByVal PostID As Integer)
			Dim strCacheKey As String = PostInfoCacheKeyPrefix & CStr(PostID)

			DataCache.RemoveCache(strCacheKey)
		End Sub

#End Region

#Region "Public Properties"

		'''' <summary>
		'''' The moduleID this post is being accessed from.
		'''' </summary>
		'''' <value></value>
		'''' <returns></returns>
		'''' <remarks></remarks>
		'Public ReadOnly Property ModuleId() As Integer
		'	Get
		'		Return ParentThread.HostForum.ModuleID
		'	End Get
		'End Property

		''' <summary>
		''' The forumID this post and thread belong to. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ForumID() As Integer
			Get
				Return ParentThread.ForumID
			End Get
		End Property

		''' <summary>
		''' Details about the thread which contains the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ParentThread() As ThreadInfo
			Get
				Return ThreadInfo.GetThreadInfo(ThreadID)
			End Get
		End Property

		''' <summary>
		''' The user information about the user who posted the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Author() As ForumUser
			Get
				Dim cntForumUser As New ForumUserController
				Return cntForumUser.GetForumUser(UserID, False, ParentThread.HostForum.ModuleID, ParentThread.HostForum.ParentGroup.PortalID)
			End Get
		End Property

		''' <summary>
		''' The user information of the user who last modified the post. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Returns anonymous user if it wasn't updated. </remarks>
		Public ReadOnly Property LastModifiedAuthor() As ForumUser
			Get
				Dim cntForumUser As New ForumUserController
				Return cntForumUser.GetForumUser(UpdatedByUser, False, ParentThread.HostForum.ModuleID, ParentThread.HostForum.ParentGroup.PortalID)
			End Get
		End Property

		''' <summary>
		''' The PostID of the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostID() As Integer
			Get
				Return _PostId
			End Get
			Set(ByVal Value As Integer)
				_PostId = Value
			End Set
		End Property

		''' <summary>
		''' The PostID of the parent post this post was in response to. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ParentPostID() As Integer
			Get
				Return _ParentPostId
			End Get
			Set(ByVal Value As Integer)
				_ParentPostId = Value
			End Set
		End Property

		''' <summary>
		''' The UserID who posted the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UserID() As Integer
			Get
				Return _UserId
			End Get
			Set(ByVal Value As Integer)
				_UserId = Value
			End Set
		End Property

		''' <summary>
		''' The IP Address of the user who posted the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property RemoteAddr() As String
			Get
				Return _RemoteAddr
			End Get
			Set(ByVal Value As String)
				_RemoteAddr = Value
			End Set
		End Property

		''' <summary>
		''' If the original posters wants to be notified of responses. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Notify() As Boolean
			Get
				Return _Notify
			End Get
			Set(ByVal Value As Boolean)
				_Notify = Value
			End Set
		End Property

		''' <summary>
		''' The subject of the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Subject() As String
			Get
				Return _Subject
			End Get
			Set(ByVal Value As String)
				_Subject = Value
			End Set
		End Property

		''' <summary>
		''' The body of the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Body() As String
			Get
				Return _Body
			End Get
			Set(ByVal Value As String)
				_Body = Value
			End Set
		End Property

		''' <summary>
		''' The date the post was created. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CreatedDate() As Date
			Get
				Return _CreatedDate
			End Get
			Set(ByVal Value As Date)
				_CreatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The ThreadID the post belongs to. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ThreadID() As Integer
			Get
				Return _ThreadID
			End Get
			Set(ByVal Value As Integer)
				_ThreadID = Value
			End Set
		End Property

		''' <summary>
		''' The post level value of the post. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostLevel() As Integer
			Get
				Return _PostLevel
			End Get
			Set(ByVal Value As Integer)
				_PostLevel = Value
			End Set
		End Property

		''' <summary>
		''' The tree sort order value of the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TreeSortOrder() As Integer
			Get
				Return _TreeSortOrder
			End Get
			Set(ByVal Value As Integer)
				_TreeSortOrder = Value
			End Set
		End Property

		''' <summary>
		''' The flat sort order value of the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property FlatSortOrder() As Integer
			Get
				Return _FlatSortOrder
			End Get
			Set(ByVal Value As Integer)
				_FlatSortOrder = Value
			End Set
		End Property

		''' <summary>
		''' The date the post was last updated.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property UpdatedDate() As Date
			Get
				Return _UpdatedDate
			End Get
			Set(ByVal Value As Date)
				_UpdatedDate = Value
			End Set
		End Property

		''' <summary>
		''' The userID of the user to last update the post.
		''' </summary>
		''' <value></value>
		''' <returns>Returns UserID of the last user to update the post or -1 if the post has never been updated.</returns>
		''' <remarks></remarks>
		Public Property UpdatedByUser() As Integer
			Get
				Return _UpdatedByUser
			End Get
			Set(ByVal Value As Integer)
				_UpdatedByUser = Value
			End Set
		End Property

		''' <summary>
		''' If the post is approved or not.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsApproved() As Boolean
			Get
				Return _IsApproved
			End Get
			Set(ByVal Value As Boolean)
				_IsApproved = Value
			End Set
		End Property

		''' <summary>
		''' If the post is locked or not.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsLocked() As Boolean
			Get
				Return _IsLocked
			End Get
			Set(ByVal Value As Boolean)
				_IsLocked = Value
			End Set
		End Property

		''' <summary>
		''' If the post is closed or not. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsClosed() As Boolean
			Get
				Return _IsClosed
			End Get
			Set(ByVal Value As Boolean)
				_IsClosed = Value
			End Set
		End Property

		''' <summary>
		''' The ParseInfo of the post as a sum of Enum PostParserInfo 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ParseInfo() As Integer
			Get
				Return _ParseInfo
			End Get
			Set(ByVal Value As Integer)
				_ParseInfo = Value
			End Set
		End Property

		''' <summary>
		''' If the thread has new posts since the user's last visit date.
		''' </summary>
		''' <param name="UserID"></param>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property NewPost(ByVal UserID As Integer) As Boolean
			Get
				Dim userThreadController As New UserThreadsController
				Dim userThread As New UserThreadsInfo
				If UserID > 0 Then
					userThread = userThreadController.GetCachedUserThreadRead(UserID, ThreadID)
					If userThread Is Nothing Then
						Return True
					Else
						'consider it a new post if made within the last minute of the most recent visit
						If userThread.LastVisitDate < CreatedDate Then
							Return True
						Else
							Return False
						End If
					End If
				Else
					Return True
				End If
			End Get
		End Property

		''' <summary>
		''' The URL to the file attachment.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property FileAttachmentURL() As String
			Get
				Return _FileAttachmentURL
			End Get
			Set(ByVal Value As String)
				_FileAttachmentURL = Value
			End Set
		End Property

		''' <summary>
		''' The name of the file attachment attached to the post.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property FileAttachmentName() As String
			Get
				Return _FileAttachmentName
			End Get
			Set(ByVal Value As String)
				_FileAttachmentName = Value
			End Set
		End Property

		''' <summary>
		''' The number of times the post has been reported. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PostReported() As Integer
			Get
				Return _PostReported
			End Get
			Set(ByVal Value As Integer)
				_PostReported = Value
			End Set
		End Property

		''' <summary>
		''' The number of post reports that have been addressed (for this post). 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>If this doesn't match PostReported, it shows up in the moderator reported posts area.</remarks>
		Public Property Addressed() As Integer
			Get
				Return _Addressed
			End Get
			Set(ByVal Value As Integer)
				_Addressed = Value
			End Set
		End Property

		''' <summary>
		''' All Attachments related to the specific post 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>Returns a list of AttachmentInfo, use Attachments.Count to see if anything is there</remarks>
		Public ReadOnly Property Attachments() As List(Of AttachmentInfo)
			Get
				If _Attachments Is Nothing Then
					Dim cntAttachment As New AttachmentController
					_Attachments = cntAttachment.GetAllByPostID(PostID)
				End If
				Return _Attachments
			End Get
		End Property

#End Region

	End Class

#End Region

End Namespace