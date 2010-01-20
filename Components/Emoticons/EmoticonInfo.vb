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

#Region " EmoticonInfo "

	''' <summary>
	''' A single instance of the EmoticonInfo Object.
	''' </summary>
	''' <remarks>Added by Skeel</remarks>
	Public Class EmoticonInfo

#Region "Constructors"

		Public Sub New()
		End Sub

#End Region

#Region " Private Members "

		Private mID As Integer
		Private mCode As String
		Private mEmoticon As String
		Private mSortOrder As Integer
		Private mToolTip As String
		Private mIsDefault As Boolean
		Private mModuleID As Integer
		Private _objConfig As Forum.Config

#End Region

#Region " Public Properties "

		''' <summary>
		''' ID
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ID() As Integer
			Get
				Return mID
			End Get
			Set(ByVal Value As Integer)
				mID = Value
			End Set
		End Property

		''' <summary>
		''' Code
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Code() As String
			Get
				Return mCode
			End Get
			Set(ByVal Value As String)
				mCode = Value
			End Set
		End Property

		''' <summary>
		''' Emoticon as file name, is always placed in the EmoticonFolderPath
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Emoticon() As String
			Get
				Return mEmoticon
			End Get
			Set(ByVal Value As String)
				mEmoticon = Value
			End Set
		End Property

		''' <summary>
		''' SortOrder
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property SortOrder() As Integer
			Get
				Return mSortOrder
			End Get
			Set(ByVal Value As Integer)
				mSortOrder = Value
			End Set
		End Property

		''' <summary>
		''' Tooltip (should describe a feeling for the emoticon)
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ToolTip() As String
			Get
				Return mToolTip
			End Get
			Set(ByVal Value As String)
				mToolTip = Value
			End Set
		End Property

		''' <summary>
		''' IsDefault = true, will display the emoticon on the postedit page
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property IsDefault() As Boolean
			Get
				Return mIsDefault
			End Get
			Set(ByVal Value As Boolean)
				mIsDefault = Value
			End Set
		End Property

		''' <summary>
		''' ModuleID
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property ModuleID() As Integer
			Get
				Return mModuleID
			End Get
			Set(ByVal Value As Integer)
				mModuleID = Value
			End Set
		End Property

		''' <summary>
		''' HTML Markup that is injected in post content to represent an emoticon. 
		''' </summary>
		''' <value></value>
		''' <returns>Returns a full HTML string that represents the emoticon, for use in post rendering.</returns>
		''' <remarks></remarks>
		Public ReadOnly Property EmoticonHTML() As String
			Get
				Return "<img src=""" & objConfig.CurrentPortalSettings.HomeDirectory & objConfig.EmoticonPath & Emoticon & """ border=""0"" alt=""" & mToolTip & """ title=""" & mToolTip & """ />"
			End Get
		End Property

		''' <summary>
		''' The forum configuration settings.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private ReadOnly Property objConfig() As Forum.Config
			Get
				If _objConfig Is Nothing Then
					_objConfig = Forum.Config.GetForumConfig(ModuleID)
				End If
				Return _objConfig
			End Get
		End Property

#End Region

	End Class

#End Region

End Namespace