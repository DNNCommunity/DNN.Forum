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

#Region "ForumPermissionInfo"

    ''' <summary>
    ''' Basically a copy of the core ModulePermissionInfo class.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	8/6/2006	Created
    ''' </history>
    Public Class ForumPermissionInfo
        Inherits PermissionInfo

#Region "Private Members"

        Dim _forumPermissionID As Integer
        Dim _forumID As Integer
        Dim _moduleID As Integer
        Dim _permissionKey As String
        Dim _roleID As Integer
        Dim _AllowAccess As Boolean
        Dim _RoleName As String
        Dim _userID As Integer
        Dim _Username As String
        Dim _DisplayName As String

#End Region

#Region "Constructor"

        ''' <summary>
        ''' Instantiates the class.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _forumPermissionID = Null.NullInteger
            _forumID = Null.NullInteger
            _moduleID = Null.NullInteger
            _permissionKey = Null.NullString
            'CP - COMEBACK - This is just until post 4.5 release (core doesn't have this var till then)
            '_roleID = Integer.Parse(DotNetNuke.Common.glbRoleNothing)
            _roleID = -4
            _AllowAccess = False
            _RoleName = Null.NullString
            _userID = Null.NullInteger
            _Username = Null.NullString
            _DisplayName = Null.NullString
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' Gets/Sets the ForumPermissionID (PK).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("forumpermissionid")> Public Property ForumPermissionID() As Integer
            Get
                Return _forumPermissionID
            End Get
            Set(ByVal Value As Integer)
                _forumPermissionID = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets/Sets the ForumID. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("forumid")> Public Property ForumID() As Integer
            Get
                Return _forumID
            End Get
            Set(ByVal Value As Integer)
                _forumID = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets/Sets the ModuleID. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("moduleid")> Public Property ModuleID() As Integer
            Get
                Return _moduleID
            End Get
            Set(ByVal Value As Integer)
                _moduleID = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets/Sets the RoleID. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("roleid")> Public Property RoleID() As Integer
            Get
                Return _roleID
            End Get
            Set(ByVal Value As Integer)
                _roleID = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets/Sets the RoleName. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("rolename")> Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal Value As String)
                _RoleName = Value
            End Set
        End Property

        ''' <summary>
        ''' Gets/Sets the allowaccess variable which determines if access is permitted or not for a role/forum combo. 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("allowaccess")> Public Property AllowAccess() As Boolean
            Get
                Return _AllowAccess
            End Get
            Set(ByVal Value As Boolean)
                _AllowAccess = Value
            End Set
        End Property

        ''' <summary>
        ''' The UserID the permissions are associated with.
        ''' </summary>
        ''' <value></value>
        ''' <returns>The UserID the permissions are associated with.</returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("userid")> Public Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal Value As Integer)
                _userID = Value
            End Set
        End Property

        ''' <summary>
        ''' The username the permissions are associated with.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("username")> Public Property Username() As String
            Get
                Return _Username
            End Get
            Set(ByVal Value As String)
                _Username = Value
            End Set
        End Property

        ''' <summary>
        ''' The displayname the permissions are associated with.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElement("displayname")> Public Property DisplayName() As String
            Get
                Return _DisplayName
            End Get
            Set(ByVal Value As String)
                _DisplayName = Value
            End Set
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Compares if two ForumPermissionInfo objects are equivalent/equal
        ''' </summary>
        ''' <param name="obj">a ForumPermissionObject</param>
        ''' <returns>true if the permissions being passed represents the same permission
        ''' in the current object
        ''' </returns>
        ''' <remarks>
        ''' This function is needed to prevent adding duplicates to the ForumPermissionCollection.
        ''' ForumPermissionCollection.Contains will use this method to check if a given permission
        ''' is already included in the collection.
        ''' </remarks>
        ''' <history>
        ''' 	[cpaterra]	08/06/2006	Created
        ''' </history>
        Public Overloads Overrides Function Equals(ByVal obj As Object) As Boolean
            If obj Is Nothing Or Not Me.GetType() Is obj.GetType() Then
                Return False
            End If
            Dim perm As ForumPermissionInfo = CType(obj, ForumPermissionInfo)
            Return (Me.AllowAccess = perm.AllowAccess) And (Me.ForumID = perm.ForumID) And _
                (Me.RoleID = perm.RoleID) And (Me.PermissionID = perm.PermissionID)
        End Function

#End Region

    End Class

#End Region

End Namespace
