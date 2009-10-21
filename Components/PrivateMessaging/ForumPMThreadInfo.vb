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

#Region "PMThreadInfo"

	''' <summary>
	''' A single instance of the Private Message Thread Info object. 
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' 	[cpaterra]	1/21/2006	Created
	''' </history>
	Public Class PMThreadInfo

#Region "Private Members"

		Private mPMThreadID As Integer
		Private mPMThreadSubject As String
		Private mPMStartDate As DateTime
		Private mPMStartThreadUserID As Integer
		Private mPMReceiveThreadUserID As Integer
		Private mViews As Integer
		Private mLastPostedPMID As Integer
		Private mReplies As Integer
		Private mPMStartUserDeleted As Boolean
		Private mPMToUserDeleted As Boolean
		Private mPortalID As Integer
		Private mPMToUserID As Integer
		Private mPMFromUserID As Integer
		Private _TotalRecords As Integer

#End Region

#Region "Constructors"

		''' <summary>
		''' Instantiates the class.
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' This is here so if we want to change it later, we only have to do this
		''' from one spot and not throughout the application.  We could factor in
		''' caching from here but it makes no sense since this data would be cached
		''' per user and that would be a waste of resources.
		''' </summary>
		''' <param name="PMThreadID">Integer</param>
		''' <returns>PMThreadInfo</returns>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' 	[cpaterra]	1/21/2007	Created
		''' </history>
		Public Shared Function GetPMThreadInfo(ByVal PMThreadID As Integer) As PMThreadInfo
			Dim ctlThread As New PMThreadController
			Dim PMThread As PMThreadInfo = ctlThread.PMThreadGet(PMThreadID)

			Return PMThread
		End Function

#End Region

#Region "Public ReadOnly Properties"

		''' <summary>
		''' The last private message info object.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property LastPM() As PMInfo
			Get
				Dim ctlPM As New PMController
				Return ctlPM.PMGet(LastPostedPMID)
			End Get
		End Property

#End Region

#Region "Public Properties"

		''' <summary>
		''' The PortalID the PM Thread belongs to. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PortalID() As Integer
			Get
				Return mPortalID
			End Get
			Set(ByVal value As Integer)
				mPortalID = value
			End Set
		End Property

		''' <summary>
		''' The PMThreadID of the Private Message thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMThreadID() As Integer
			Get
				Return mPMThreadID
			End Get
			Set(ByVal Value As Integer)
				mPMThreadID = Value
			End Set
		End Property

		''' <summary>
		''' The first post subject of the private message thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMThreadSubject() As String
			Get
				Return mPMThreadSubject
			End Get
			Set(ByVal Value As String)
				mPMThreadSubject = Value
			End Set
		End Property

		''' <summary>
		''' The date the Private Message thread was created. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMStartDate() As DateTime
			Get
				Return mPMStartDate
			End Get
			Set(ByVal Value As DateTime)
				mPMStartDate = Value
			End Set
		End Property

		''' <summary>
		''' The UserID of the person who started the Private Message thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMStartThreadUserID() As Integer
			Get
				Return mPMStartThreadUserID
			End Get
			Set(ByVal Value As Integer)
				mPMStartThreadUserID = Value
			End Set
		End Property

		''' <summary>
		''' The UserID of the person receiving the private message thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMReceiveThreadUserID() As Integer
			Get
				Return mPMReceiveThreadUserID
			End Get
			Set(ByVal Value As Integer)
				mPMReceiveThreadUserID = Value
			End Set
		End Property

		''' <summary>
		''' The number of times this PM thread has been viewed. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Views() As Integer
			Get
				Return mViews
			End Get
			Set(ByVal Value As Integer)
				mViews = Value
			End Set
		End Property

		''' <summary>
		''' The last PMID that was posted to this PM thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property LastPostedPMID() As Integer
			Get
				Return mLastPostedPMID
			End Get
			Set(ByVal Value As Integer)
				mLastPostedPMID = Value
			End Set
		End Property

		''' <summary>
		''' The number of replies this private message thread has. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Replies() As Integer
			Get
				Return mReplies
			End Get
			Set(ByVal Value As Integer)
				mReplies = Value
			End Set
		End Property

		''' <summary>
		''' If the user who started the thread has deleted the thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMStartUserDeleted() As Boolean
			Get
				Return mPMStartUserDeleted
			End Get
			Set(ByVal value As Boolean)
				mPMStartUserDeleted = value
			End Set
		End Property

		''' <summary>
		''' If the user who received this thread has deleted the thread. 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMToUserDeleted() As Boolean
			Get
				Return mPMToUserDeleted
			End Get
			Set(ByVal value As Boolean)
				mPMToUserDeleted = value
			End Set
		End Property

		''' <summary>
		''' The UserID receiving the private message.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMToUserID() As Integer
			Get
				Return mPMToUserID
			End Get
			Set(ByVal Value As Integer)
				mPMToUserID = Value
			End Set
		End Property

		''' <summary>
		''' The UserID who created the private message.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property PMFromUserID() As Integer
			Get
				Return mPMFromUserID
			End Get
			Set(ByVal Value As Integer)
				mPMFromUserID = Value
			End Set
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TotalRecords() As Integer
			Get
				Return _TotalRecords
			End Get
			Set(ByVal value As Integer)
				_TotalRecords = value
			End Set
		End Property

#End Region

	End Class

#End Region

End Namespace