'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2011
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
	''' Creates an instance of the forum object and reads properties critical to its operation.
	''' </summary>
	''' <remarks>All dynamically generated UI's inherit from this class. (Threads, Posts, Forums, Search) This is essentially our PortalModuleBase for those controls.
	''' </remarks>
    Public Class ForumObject
        Inherits ForumBaseObject

#Region "Contructors"

        ''' <summary>
        ''' Instantiates a new instance of this class.
        ''' </summary>
        ''' <param name="forum">The housing forum placeholder control to inject the UI into.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal forum As DNNForum)
            MyBase.New(forum)
        End Sub

#End Region

#Region "Protected Properties"

        ''' <summary>
        ''' Gets the DNNForum object being rendered. 
        ''' </summary>
        ''' <value></value>
        ''' <returns>What type of UI is bening rendered</returns>
        ''' <remarks>This is the thread, posts, group, search UI types.</remarks>
        Protected ReadOnly Property ForumControl() As DNNForum
            Get
                Return CType(BaseControl, DNNForum)
            End Get
        End Property

        ''' <summary>
        ''' Gets the ModuleID being accessed. 
        ''' </summary>
        ''' <value></value>
        ''' <returns>The moduleID being accessed by the user.</returns>
        ''' <remarks>PortalModulebase attribute picked up from the Basecontrol object.</remarks>
        Protected Overloads ReadOnly Property ModuleID() As Integer
            Get
                Return ForumControl.ModuleID
            End Get
        End Property

        ''' <summary>
        ''' Gets the PortalID the module is running in. 
        ''' </summary>
        ''' <value></value>
        ''' <returns>The PortalID being accessed by the user.</returns>
        ''' <remarks>PortalModulebase attribute picked up from the Basecontrol object.</remarks>
        Protected Overloads ReadOnly Property PortalID() As Integer
            Get
                Return MyBase.BaseControl.PortalID
            End Get
        End Property

        ''' <summary>
        ''' Gets the configuration settings for a forum module. 
        ''' </summary>
        ''' <value></value>
        ''' <returns>An object containing all configuration options for a forum module instance.</returns>
        ''' <remarks>This is all the forum configuration settings needed for the various dynamically generated UI's.</remarks>
        Protected Overloads ReadOnly Property objConfig() As Configuration
            Get
                Return DotNetNuke.Modules.Forum.Configuration.GetForumConfig(ForumControl.ModuleID)
            End Get
        End Property

        ''' <summary>
        ''' Gets the TabID being accessed. 
        ''' </summary>
        ''' <value></value>
        ''' <returns>The TabID being accessed by the user.</returns>
        ''' <remarks>PortalModulebase attribute picked up from the Basecontrol object.</remarks>
        Protected Overloads ReadOnly Property TabID() As Integer
            Get
                Return MyBase.BaseControl.TabID
            End Get
        End Property

        ''' <summary>
        ''' Gets the BasePage object being accessed. 
        ''' </summary>
        ''' <value></value>
        ''' <returns>The DotNetNuke.Framework.CDefault being accessed.</returns>
        ''' <remarks>PortalModulebase attribute picked up from the Basecontrol object.</remarks>
        Public ReadOnly Property BasePage() As DotNetNuke.Framework.CDefault
            Get
                Return MyBase.BaseControl.BasePage
            End Get
        End Property

#End Region

        ''' <summary>
        ''' This is the user who is viewing the forum.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrentForumUser() As ForumUserInfo
            Get
                Dim cntForumUser As New ForumUserController
                Return cntForumUser.GetForumUser(Users.UserController.Instance.GetCurrentUserInfo.UserID, False, ForumControl.ModuleID, PortalID)
            End Get
        End Property

        ''' <summary>
        ''' This is the user who's profile we are altering (typically, the logged in user unless a forum admin).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ProfileUserID() As Integer
            Get
                If HttpContext.Current.Request.QueryString("userid") IsNot Nothing Then
                    Dim _ProfileUserID As Integer = Int32.Parse(HttpContext.Current.Request.QueryString("userid"))
                    Return _ProfileUserID
                Else
                    Return CurrentForumUser.UserID
                End If
            End Get
        End Property

        ''' <summary>
        ''' The current forum the user is viewing. 
        ''' </summary>
        ''' <value></value>
        ''' <returns>An integer representing the current forum the usre is viewing. It will return -1 if there is no forumid parameter in the URL.</returns>
        ''' <remarks>The module depends heavily on this always being available in the URL. However, this is typically not used for security reasons.</remarks>
        Public Overridable ReadOnly Property ForumID() As Integer
            Get
                Dim _forumID As Integer = -1
                If (HttpContext.Current.Request.QueryString("forumid") IsNot Nothing) AndAlso Integer.TryParse(HttpContext.Current.Request.QueryString("forumid"), _forumID) Then
                    Return _ForumID
                Else
                    ' maybe something else is in the url
                    Return -1
                End If
            End Get
        End Property

    End Class

End Namespace