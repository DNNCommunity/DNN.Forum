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

#Region "UpgradeLogic"

    ''' <summary>
    ''' We use IUpgradeable to cleanup files from older installations and to add database entries which require ModuleDefinitionID (such as lists, module permissions). 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cpaterra]	12/3/2005	Created
    ''' </history>
    Public Class UpgradeLogic

		'      ''' <summary>
		'      ''' This adds forum module permissions to the core permission table if they don't exist. 
		'      ''' </summary>
		'      ''' <returns></returns>
		'      ''' <remarks></remarks>
		'Public Function InitPermissions() As Integer
		'	Dim HasForumAdminPermission As Boolean, HasGlobalModPermission As Boolean

		'	Dim moduleDefId As Integer
		'	Dim pc As New DotNetNuke.Security.Permissions.PermissionController
		'	Dim permissions As ArrayList = pc.GetPermissionByCodeAndKey("FORUM_MODULE", Nothing)
		'	Dim dc As New DotNetNuke.Entities.Modules.DesktopModuleController
		'	Dim desktopInfo As DotNetNuke.Entities.Modules.DesktopModuleInfo
		'	desktopInfo = dc.GetDesktopModuleByModuleName("DNN_Forum")
		'	Dim mc As New DotNetNuke.Entities.Modules.Definitions.ModuleDefinitionController
		'	Dim mInfo As DotNetNuke.Entities.Modules.Definitions.ModuleDefinitionInfo
		'	mInfo = mc.GetModuleDefinitionByName(desktopInfo.DesktopModuleID, "Forum")
		'	moduleDefId = mInfo.ModuleDefID

		'	For Each p As DotNetNuke.Security.Permissions.PermissionInfo In permissions
		'		If p.PermissionKey = "FORUMADMIN" And p.ModuleDefID = moduleDefId Then HasForumAdminPermission = True
		'		If p.PermissionKey = "FORUMGLBMOD" And p.ModuleDefID = moduleDefId Then HasGlobalModPermission = True
		'	Next

		'	If Not HasForumAdminPermission Then
		'		Dim p As New DotNetNuke.Security.Permissions.PermissionInfo
		'		p.ModuleDefID = moduleDefId
		'		p.PermissionCode = "FORUM_MODULE"
		'		p.PermissionKey = "FORUMADMIN"
		'		p.PermissionName = "Forum Administrator"
		'		pc.AddPermission(p)
		'	End If

		'	If Not HasGlobalModPermission Then
		'		Dim p As New DotNetNuke.Security.Permissions.PermissionInfo
		'		p.ModuleDefID = moduleDefId
		'		p.PermissionCode = "FORUM_MODULE"
		'		p.PermissionKey = "FORUMGLBMOD"
		'		p.PermissionName = "Global Moderator"
		'		pc.AddPermission(p)
		'	End If

		'	Return moduleDefId
		'End Function

        ''' <summary>
        ''' Adds all the lists needed for this module in version 3.30 or greater
        ''' </summary>
        ''' <param name="ModuleDefId"></param>
        ''' <remarks></remarks>
        Public Sub AddLists(ByVal ModuleDefId As Integer)
            Dim ctlLists As New DotNetNuke.Common.Lists.ListController
            'description is missing, not needed

            'ThreadStatus
            Dim objList As New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ThreadStatus"
            objList.Value = "0"
            objList.Text = "NoneSpecified"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ThreadStatus"
            objList.Value = "1"
            objList.Text = "Unanswered"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ThreadStatus"
            objList.Value = "2"
            objList.Text = "Answered"
            objList.SortOrder = 3
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ThreadStatus"
            objList.Value = "3"
            objList.Text = "Informative"
            objList.SortOrder = 4
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            'EmailFormat
            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "EmailFormat"
            objList.Value = "0"
            objList.Text = "Text"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "EmailFormat"
            objList.Value = "1"
            objList.Text = "HTML"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            'GroupView
            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "GroupView"
            objList.Value = "0"
            objList.Text = "AllExpanded"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "GroupView"
            objList.Value = "-1"
            objList.Text = "AllCollapsed"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "GroupView"
            objList.Value = "1"
            objList.Text = "AsLastViewed"
            objList.SortOrder = 3
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            'ForumMemberName
            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumMemberName"
            objList.Value = "0"
            objList.Text = "Username"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumMemberName"
            objList.Value = "1"
            objList.Text = "DisplayName"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            'ForumThreadRate
            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "0"
            objList.Text = "Rate0"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "1"
            objList.Text = "Rate1"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "2"
            objList.Text = "Rate2"
            objList.SortOrder = 3
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "3"
            objList.Text = "Rate3"
            objList.SortOrder = 4
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "4"
            objList.Text = "Rate4"
            objList.SortOrder = 5
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "5"
            objList.Text = "Rate5"
            objList.SortOrder = 6
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "6"
            objList.Text = "Rate6"
            objList.SortOrder = 7
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "7"
            objList.Text = "Rate7"
            objList.SortOrder = 8
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "8"
            objList.Text = "Rate8"
            objList.SortOrder = 9
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "9"
            objList.Text = "Rate9"
            objList.SortOrder = 10
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumThreadRate"
            objList.Value = "10"
            objList.Text = "Rate10"
            objList.SortOrder = 11
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            'TrackingDuration
            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "0"
            objList.Text = "Today"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "3"
            objList.Text = "PastThreeDays"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "7"
            objList.Text = "PastWeek"
            objList.SortOrder = 3
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "14"
            objList.Text = "PastTwoWeek"
            objList.SortOrder = 4
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "30"
            objList.Text = "PastMonth"
            objList.SortOrder = 5
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "92"
            objList.Text = "PastThreeMonth"
            objList.SortOrder = 6
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "365"
            objList.Text = "PastYear"
            objList.SortOrder = 7
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "-1"
            objList.Text = "LastVisit"
            objList.SortOrder = 8
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "TrackingDuration"
            objList.Value = "3650"
            objList.Text = "AllDays"
            objList.SortOrder = 9
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            'DisplayPosterLocation
            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "DisplayPosterLocation"
            objList.Value = "0"
            objList.Text = "None"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "DisplayPosterLocation"
            objList.Value = "1"
            objList.Text = "ToAdmin"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "DisplayPosterLocation"
            objList.Value = "2"
            objList.Text = "ToAll"
            objList.SortOrder = 3
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            ' Forum Types
            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumType"
            objList.Value = "0"
            objList.Text = "Normal"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumType"
            objList.Value = "1"
            objList.Text = "Notification"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumType"
            objList.Value = "2"
            objList.Text = "Link"
            objList.SortOrder = 3
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            ' Forum Behavior
            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumBehavior"
            objList.Value = "0"
            objList.Text = "PublicModerated"
            objList.SortOrder = 1
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumBehavior"
            objList.Value = "1"
            objList.Text = "PublicModeratedPostRestricted"
            objList.SortOrder = 2
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumBehavior"
            objList.Value = "2"
            objList.Text = "PublicUnModerated"
            objList.SortOrder = 3
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumBehavior"
            objList.Value = "3"
            objList.Text = "PublicUnModeratedPostRestricted"
            objList.SortOrder = 4
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumBehavior"
            objList.Value = "4"
            objList.Text = "PrivateModerated"
            objList.SortOrder = 5
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumBehavior"
            objList.Value = "5"
            objList.Text = "PrivateModeratedPostRestricted"
            objList.SortOrder = 6
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumBehavior"
            objList.Value = "6"
            objList.Text = "PrivateUnModerated"
            objList.SortOrder = 7
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

            objList = New DotNetNuke.Common.Lists.ListEntryInfo
            objList.ListName = "ForumBehavior"
            objList.Value = "7"
            objList.Text = "PrivateUnModeratedPostRestricted"
            objList.SortOrder = 8
            objList.ParentID = 0
            objList.Level = 0
            objList.DefinitionID = ModuleDefId
            ctlLists.AddListEntry(objList)

        End Sub

        ''' <summary>
        ''' Used to transfer permissions from older format (3.20.9) to new format (4.4.2)
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub TransferPermissions()
            Dim arrOldForumPerms As System.Collections.Generic.List(Of ForumPermInfo)
            ' Populate arraylist
            arrOldForumPerms = CBO.FillCollection(Of ForumPermInfo)(DataProvider.Instance().Upgrade_GetForumPerms())
            'arrOldForumPerms = CBO.FillCollection(DotNetNuke.Modules.Forum.DataProvider.Instance().Upgrade_GetForumPerms(), GetType(ForumPermInfo))

            If arrOldForumPerms.Count > 0 Then
                For Each item As ForumPermInfo In arrOldForumPerms
                    ' Handle Auth Edit Roles for private/non-private
					If Not item.AuthorizedEditRoles = String.Empty Then
						Const _HasRestrictedStartThreadPerms As String = "STARTTHREAD"
						Const _HasRestrictedPostReplyPerms As String = "POSTREPLY"
						Dim StartPerms As New PermissionInfo
						Dim ReplyPerms As New PermissionInfo
						StartPerms = CBO.FillObject(Of PermissionInfo)(DataProvider.Instance().Upgrade_GetPermByKey(_HasRestrictedStartThreadPerms))
						ReplyPerms = CBO.FillObject(Of PermissionInfo)(DataProvider.Instance().Upgrade_GetPermByKey(_HasRestrictedPostReplyPerms))

						For Each roleName As String In item.AuthorizedEditRoles.Split(CChar(";"))
							Dim cntRoles As New Security.Roles.RoleController
							Dim role As Security.Roles.RoleInfo
							role = cntRoles.GetRoleByName(item.PortalID, roleName)

							If Not role Is Nothing Then
								Dim cntForumPermissions As New ForumPermissionController
								Dim objStartPermission As New ForumPermissionInfo

								objStartPermission.ForumID = item.ForumID
								objStartPermission.PermissionID = StartPerms.PermissionID
								objStartPermission.RoleID = role.RoleID
								objStartPermission.AllowAccess = True
								objStartPermission.UserID = -1
								objStartPermission.DisplayName = Null.NullString
								cntForumPermissions.AddForumPermission(objStartPermission)

								Dim objReplyPermission As New ForumPermissionInfo

								objReplyPermission.ForumID = item.ForumID
								objReplyPermission.PermissionID = ReplyPerms.PermissionID
								objReplyPermission.RoleID = role.RoleID
								objReplyPermission.AllowAccess = True
								objReplyPermission.UserID = -1
								objReplyPermission.DisplayName = Null.NullString
								cntForumPermissions.AddForumPermission(objReplyPermission)
							End If
						Next
					End If

					' handle auth roles only for non-private
					If Not item.PublicView Then
						If Not item.AuthorizedRoles = String.Empty Then
							Const _HasPrivateViewPerms As String = "VIEW"
							Dim ViewPerms As New PermissionInfo
							ViewPerms = CBO.FillObject(Of PermissionInfo)(DataProvider.Instance().Upgrade_GetPermByKey(_HasPrivateViewPerms))

							For Each roleName As String In item.AuthorizedEditRoles.Split(CChar(";"))
								Dim cntRoles As New Security.Roles.RoleController
								Dim role As Security.Roles.RoleInfo
								role = cntRoles.GetRoleByName(item.PortalID, roleName)

								If Not role Is Nothing Then
									Dim cntForumPermissions As New ForumPermissionController
									Dim objViewPermission As New ForumPermissionInfo

									objViewPermission.ForumID = item.ForumID
									objViewPermission.PermissionID = ViewPerms.PermissionID
									objViewPermission.RoleID = role.RoleID
									objViewPermission.AllowAccess = True
									objViewPermission.UserID = -1
									objViewPermission.DisplayName = Null.NullString
									cntForumPermissions.AddForumPermission(objViewPermission)
								End If
							Next
						End If
					End If
                Next
                ' Handle moderator migration
                ' Get list of moderators and forums they are assigned to
                Dim arrOldForumMods As ArrayList
                ' Populate arraylist
                arrOldForumMods = CBO.FillCollection(DotNetNuke.Modules.Forum.DataProvider.Instance().Upgrade_GetForumMods(), GetType(ForumModeratorInfo))

                If arrOldForumMods.Count > 0 Then
                    Const _HasForumModeratePerms As String = "MODERATE"
                    Dim ModeratePerms As New PermissionInfo
                    ModeratePerms = CBO.FillObject(Of PermissionInfo)(DataProvider.Instance().Upgrade_GetPermByKey(_HasForumModeratePerms))

                    For Each item As ForumModeratorInfo In arrOldForumMods
                        Dim cntForumPermissions As New ForumPermissionController
						Dim objModPermission As New ForumPermissionInfo
						Dim cntUsers As New Entities.Users.UserController

						Dim displayName As String = cntUsers.GetUser(item.PortalID, item.UserID).DisplayName

                        objModPermission.ForumID = item.ForumID
                        objModPermission.PermissionID = ModeratePerms.PermissionID
                        objModPermission.RoleID = -4
                        objModPermission.AllowAccess = True
                        objModPermission.UserID = item.UserID
                        objModPermission.DisplayName = displayName
                        cntForumPermissions.AddForumPermission(objModPermission)
                    Next
                End If
            End If
        End Sub

#Region "ForumPermInfo"

        ''' <summary>
        ''' Upgrade Logic specific to Forum Permissions. 
        ''' </summary>
        ''' <remarks>Used to convert former methods to newer grid like structure for permissions control.</remarks>
        Public Class ForumPermInfo

#Region "Private Members"

            Private mForumID As Integer
            Private mPortalID As Integer
            Private mAuthorizedRoles As String
            Private mAuthorizedEditRoles As String
            Private mPublicView As Boolean

#End Region

#Region "Constructors"

            ''' <summary>
            ''' Creates a new instance of the foruminfo object.
            ''' </summary>
            ''' <remarks>
            ''' </remarks>
            Public Sub New()
            End Sub

#End Region

#Region "Public Properties"

            ''' <summary>
            ''' The ForumID (PK)
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property ForumID() As Integer
                Get
                    Return mForumID
                End Get
                Set(ByVal Value As Integer)
                    mForumID = Value
                End Set
            End Property

            ''' <summary>
            ''' The PortalID the forum belongs to.
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property PortalID() As Integer
                Get
                    Return mPortalID
                End Get
                Set(ByVal Value As Integer)
                    mPortalID = Value
                End Set
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property AuthorizedRoles() As String
                Get
                    Return mAuthorizedRoles
                End Get
                Set(ByVal Value As String)
                    mAuthorizedRoles = Value
                End Set
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property AuthorizedEditRoles() As String
                Get
                    Return mAuthorizedEditRoles
                End Get
                Set(ByVal Value As String)
                    mAuthorizedEditRoles = Value
                End Set
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property PublicView() As Boolean
                Get
                    Return mPublicView
                End Get
                Set(ByVal Value As Boolean)
                    mPublicView = Value
                End Set
            End Property

#End Region

        End Class

#End Region

#Region "ForumModerators"

        ''' <summary>
        ''' Upgrade logic specific to ForumModerators. 
        ''' </summary>
        ''' <remarks>Used to convert former methods to newer moderator logic.</remarks>
        Public Class ForumModeratorInfo

#Region "Private Members"

            Private _UserID As Integer
            Private _ForumID As Integer
            Private _PortalID As Integer

#End Region

#Region "Constructors"

            ''' <summary>
            ''' Creates a new instance of the foruminfo object.
            ''' </summary>
            ''' <remarks>
            ''' </remarks>
            Public Sub New()
            End Sub

#End Region

#Region "Public Properties"

            ''' <summary>
            ''' 
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
            ''' 
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property ForumID() As Integer
                Get
                    Return _ForumID
                End Get
                Set(ByVal Value As Integer)
                    _ForumID = Value
                End Set
            End Property

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property PortalID() As Integer
                Get
                    Return _PortalID
                End Get
                Set(ByVal Value As Integer)
                    _PortalID = Value
                End Set
            End Property

#End Region

        End Class

#End Region

    End Class

#End Region

End Namespace
