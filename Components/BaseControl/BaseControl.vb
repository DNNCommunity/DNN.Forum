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
	''' Gets/Sets all the properties we need to use in the various forum views
	''' which are loaded via the dispatch (Forum_Container) control. 
	''' </summary>
	''' <remarks>Inherits from webcontrol because it houses the various forum view controls generated programatically and injected into the Forum_Container.
	''' This is loaded by DNNForum.vb which is originally loaded by forumcontainer. This should never need to be used without current HttpContext available. (ie. an api or scheduled task). 
	''' </remarks>
	Public Class ForumBaseControl
		Inherits WebControl
		Implements INamingContainer

#Region "Private Members"

		Private _Initialized As Boolean = False
		Private _ForumBaseObject As ForumBaseObject
		Private _Config As Forum.Configuration
		Private _NavigatorActions As New ArrayList
		Private _BasePage As DotNetNuke.Framework.CDefault
		Private _PortalName As String
		Private _TreeView As Boolean = False
		Private _Descending As Boolean = False

#End Region

#Region "Constructors"

		''' <summary>
		''' Instantiates a new instance of the ForumBaseControl class.
		''' </summary>
		''' <remarks>Only called by Forum_Container.ascx.vb</remarks>
		Public Sub New()
		End Sub

#End Region

#Region "Protected Methods"

		''' <summary>
		''' Initializes control. 
		''' </summary>
		''' <remarks>Life cycle</remarks>
		Protected Overridable Sub Initialize()
			_Initialized = True
		End Sub

		''' <summary>
		''' OnLoad method which calls the initalize method. 
		''' </summary>
		''' <param name="e">The event arguement.</param>
		''' <remarks>We don't use the event arguement. Life cycle</remarks>
		Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
			Initialize()
		End Sub

		''' <summary>
		''' Needed to make sure things are all rendered. 
		''' </summary>
		''' <remarks>If this is not here there is nothing to give us piece of mind that the controls which are needed are definitely there. (life cycle issues)</remarks>
		Protected Overrides Sub EnsureChildControls()
			If Not _Initialized Then
				Initialize()
			End If
			MyBase.EnsureChildControls()
		End Sub

		''' <summary>
		''' Used to create a new instance of the ForumObject. 
		''' </summary>
		''' <remarks>Life cycle.</remarks>
		Protected Overridable Sub CreateObject()
		End Sub

		''' <summary>
		''' Creates the child controls 
		''' </summary>
		''' <remarks>Life cycle</remarks>
		Protected Overrides Sub CreateChildControls()
			CreateObject()

			If Not (_ForumBaseObject Is Nothing) Then
				_ForumBaseObject.CreateChildControls()
			End If
		End Sub

		''' <summary>
		''' OnPreRender method. 
		''' </summary>
		''' <param name="e">The Event arguement being passed in.</param>
		''' <remarks>Event arguement is not used here. Life cycle</remarks>
		Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
			If Not (_ForumBaseObject Is Nothing) Then
				_ForumBaseObject.OnPreRender()
			End If
		End Sub

		''' <summary>
		''' Render method. 
		''' </summary>
		''' <param name="wr">The HtmlTextWriter used to generate the loaded controls user interface.</param>
		''' <remarks>Life cycle</remarks>
		Protected Overrides Sub Render(ByVal wr As System.Web.UI.HtmlTextWriter)
			If Not (_ForumBaseObject Is Nothing) Then
				_ForumBaseObject.Render(wr)
			End If
		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' The BasePage is used to set the Title bar Title of the browser. 
		''' </summary>
		''' <value></value>
		''' <returns>The string to place in the browser's title bar.</returns>
		''' <remarks>This is one of the items affected by EnsureChildControls. (we are recreating PortalModuleBase)</remarks>
		Friend Property BasePage() As DotNetNuke.Framework.CDefault
			Get
				Return _BasePage
			End Get
			Set(ByVal Value As DotNetNuke.Framework.CDefault)
				_BasePage = Value
			End Set
		End Property

		''' <summary>
		''' Name of the portal the module is running on. 
		''' </summary>
		''' <value></value>
		''' <returns>The string name of the portal the module is being loaded into.</returns>
		''' <remarks>Used for the base page naming. (we are recreating PortalModuleBase)</remarks>
		Friend Property PortalName() As String
			Get
				Return _PortalName
			End Get
			Set(ByVal Value As String)
				_PortalName = Value
			End Set
		End Property

		''' <summary>
		''' List of available module actions for the module.
		''' </summary>
		''' <value></value>
		''' <returns>An arraylist of module actions</returns>
		''' <remarks>This does not check the user's permissions, so all are part of the arraylist. (we are recreating PortalModuleBase)</remarks>
		Friend Property NavigatorActions() As ArrayList
			Get
				Return _NavigatorActions
			End Get
			Set(ByVal Value As ArrayList)
				_NavigatorActions = Value
			End Set
		End Property

		''' <summary>
		''' The PortalID the module is running in.
		''' </summary>
		''' <value></value>
		''' <returns>The integer portalID the module is running in.</returns>
		''' <remarks>This is needed throughout the module to keep users profile type data portal specific. (we are recreating PortalModuleBase)</remarks>
		Friend Property PortalID() As Integer
			Get
				Dim savedState As New Object
				If Not ViewState("ForumPortalID") Is Nothing Then
					savedState = ViewState("ForumPortalID")
				End If
				If savedState Is Nothing Then
					Return 0
				Else
					Return CType(savedState, Integer)
				End If
			End Get
			Set(ByVal Value As Integer)
				ViewState("ForumPortalID") = Value
			End Set
		End Property

		''' <summary>
		''' The TabID user is accessing the module from. 
		''' </summary>
		''' <value></value>
		''' <returns>The TabID the user is accessing.</returns>
		''' <remarks>This is required to ensure all links are relative to the page which the user is browsing. (we are recreating PortalModuleBase)</remarks>
		Friend Property TabID() As Integer
			Get
				Dim savedState As New Object
				If Not ViewState("ForumTabID") Is Nothing Then
					savedState = ViewState("ForumTabID")
				End If
				If savedState Is Nothing Then
					Return 0
				Else
					Return CType(savedState, Integer)
				End If
			End Get
			Set(ByVal Value As Integer)
				ViewState("ForumTabID") = Value
			End Set
		End Property

		''' <summary>
		''' The ModuleID being accessed. 
		''' </summary>
		''' <value></value>
		''' <returns>The integer ModuleID the user is browsing.</returns>
		''' <remarks>This is necessary to ensure all dynamically generated controls know which moduleID to use for navigation (we are recreating PortalModuleBase)</remarks>
		Friend Property ModuleID() As Integer
			Get
				Dim savedState As New Object
				If Not ViewState("ForumModuleID") Is Nothing Then
					savedState = ViewState("ForumModuleID")
				End If
				If savedState Is Nothing Then
					Return 0
				Else
					Return CType(savedState, Integer)
				End If
			End Get
			Set(ByVal Value As Integer)
				ViewState("ForumModuleID") = Value
			End Set
		End Property

		''' <summary>
		''' The Module's configuration settings. 
		''' </summary>
		''' <value></value>
		''' <returns>The configuration settings pertaining to the module instance.</returns>
		''' <remarks>This is necessary for every control being loaded in the module.</remarks>
		Friend Property objConfig() As Forum.Configuration
			Get
				If _Config Is Nothing Then
					_Config = Forum.Configuration.GetForumConfig(ModuleID)
				End If
				Return _Config
			End Get
			Set(ByVal Value As Forum.Configuration)
				_Config = Value
			End Set

		End Property

		''' <summary>
		''' Type of action user is performing, such as new post, edit post, etc. 
		''' </summary>
		''' <value></value>
		''' <returns>The action the user is performing</returns>
		''' <remarks>Usually aids in Add vs. Update type situations. (Also consider that it is base for some notification items loading a template.)</remarks>
		Friend Property Action() As PostAction
			Get
				Dim savedState As New Object
				If Not ViewState("ForumPostAction") Is Nothing Then
					savedState = ViewState("ForumPostAction")
				End If
				If savedState Is Nothing Then
					Return Nothing
				Else
					Return CType(savedState, PostAction)
				End If
			End Get
			Set(ByVal Value As PostAction)
				ViewState("ForumPostAction") = Value
			End Set
		End Property

		''' <summary>
		''' If Posts are being viewed in ascending/descending mode. 
		''' </summary>
		''' <value></value>
		''' <returns>True if the posts are being shown descending by date, false if ascending.</returns>
		''' <remarks>This is only really used on the post view (ForumPost.vb)</remarks>
		Friend Property Descending() As Boolean
			Get
				Return _Descending
			End Get
			Set(ByVal Value As Boolean)
				_Descending = Value
			End Set
		End Property

		''' <summary>
		''' ViewType is used to determine which control to load for the forum module. 
		''' </summary>
		''' <value></value>
		''' <returns>The integer of the Scope enumerator.</returns>
		''' <remarks>This is the Container items: Group, Threads, Posts, Post Mod Queue</remarks>
		Friend Property ViewType() As ForumScope
			Get
				Dim savedState As New Object
				If Not ViewState("ViewType") Is Nothing Then
					savedState = ViewState("ViewType")
				End If
				If savedState Is Nothing Then
					Return ForumScope.Groups
				Else
					Return CType(savedState, ForumScope)
				End If
			End Get
			Set(ByVal Value As ForumScope)
				ViewState("ViewType") = Value
			End Set
		End Property

		''' <summary>
		''' The GenericObjectID represents the PK item we are attempting to view, such as ForumID or GroupID. 
		''' </summary>
		''' <value></value>
		''' <returns>An integer that represents a primary key, depending on which UI view the actual object type will vary.</returns>
		''' <remarks>This has nothing to do with any of the former or current integration methods.</remarks>
		Friend Property GenericObjectID() As Integer
			Get
				Dim savedState As New Object
				If Not ViewState("GenericObjectID") Is Nothing Then
					savedState = ViewState("GenericObjectID")
				End If
				If savedState Is Nothing Then
					Return 0
				Else
					Return CType(savedState, Integer)
				End If
			End Get
			Set(ByVal Value As Integer)
				ViewState("GenericObjectID") = Value
			End Set
		End Property

		''' <summary>
		''' Localization resource file's path used for Forum_Container.ascx. 
		''' </summary>
		''' <value></value>
		''' <returns>The path to the localization resource file to load.</returns>
		''' <remarks>This resource file contains all the localized values used in all controls loaded in forum container.</remarks>
		Friend Property LocalResourceFile() As String
			Get
				Dim savedState As New Object
				If Not ViewState("ForumResourceFile") Is Nothing Then
					savedState = ViewState("ForumResourceFile")
				End If
				If savedState Is Nothing Then
					ViewState("ForumResourceFile") = TemplateSourceDirectory & "/" & Localization.LocalResourceDirectory & "/Forum_Container.ascx"
				End If
				Return CType(ViewState("ForumResourceFile"), String)
			End Get
			Set(ByVal Value As String)
				ViewState("ForumResourceFile") = Value
			End Set
		End Property

		''' <summary>
		''' The Page object is used so we can override some elements. 
		''' </summary>
		''' <value></value>
		''' <returns>The page object being rendered.</returns>
		''' <remarks>(we are recreating PortalModuleBase)</remarks>
		Friend ReadOnly Property DNNPage() As Page
			Get
				Return MyBase.Page
			End Get
		End Property

		''' <summary>
		''' Creates the base forum object. 
		''' </summary>
		''' <value></value>
		''' <returns>The UI being rendered based on view type.</returns>
		''' <remarks>The base forum object is the view being rendered.</remarks>
		Protected Friend Property ForumBaseObject() As ForumBaseObject
			Get
				Return _ForumBaseObject
			End Get
			Set(ByVal Value As ForumBaseObject)
				_ForumBaseObject = Value
			End Set
		End Property

		''' <summary>
		''' Settings are the core's TabModuleSettings. We need these to control behaviour. 
		''' </summary>
		''' <value></value>
		''' <returns>The hashtable of tab module settings.</returns>
		''' <remarks>We are recreating PortalModuleBase.</remarks>
		Friend Property TabModuleSettings() As Hashtable
			Get
				Dim savedState As New Object
				If Not ViewState("TabModuleSettings") Is Nothing Then
					savedState = ViewState("TabModuleSettings")
				End If
				If savedState Is Nothing Then
					Return Nothing
				Else
					Return CType(savedState, Hashtable)
				End If
			End Get
			Set(ByVal Value As Hashtable)
				ViewState("TabModuleSettings") = Value
			End Set
		End Property

#End Region

#Region "Public Methods"

		''' <summary>
		''' Returns localized text items
		''' </summary>
		''' <param name="key">The string to look for a match for.</param>
		''' <returns>The string variable to display to the end user.</returns>
		''' <remarks>This is used to consilidate where it is called from. (we are recreating PortalModuleBase)</remarks>
		Friend Function LocalizedText(ByVal key As String) As String
			If Not Localization.GetString(key, LocalResourceFile) Is Nothing Then
				Return Localization.GetString(key, LocalResourceFile)
			Else
				Return key
			End If
		End Function

#End Region

	End Class

End Namespace