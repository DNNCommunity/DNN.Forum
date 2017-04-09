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

Imports System.Xml

Namespace DotNetNuke.Modules.Forum

#Region "RssDocument"

	''' <summary>
	''' Used to create RSS Feeds for forums.
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class RssDocument
		Inherits System.Xml.XmlDocument

#Region "Private Members"

		Private Const wellformedwebUri As String = "http://wellformedweb.org/CommentAPI/"
		Private Const slashUri As String = "http://purl.org/rss/1.0/modules/slash/"
		Private Const dublinCoreUri As String = "http://purl.org/dc/elements/1.1/"
		Private Const atomUri As String = "http://www.w3.org/2005/Atom"
		'writer.WriteAttributeString("xmlns:trackback", "http://madskills.com/public/xml/rss/module/trackback/")

		Private mForumConfig As Forum.Configuration
		Private mThreadsPage As Integer
		Private mCreationTime As DateTime

#End Region

#Region "Structures"

		''' <summary>
		''' RssChannel represents a ForumID.
		''' </summary>
		''' <remarks></remarks>
		Structure RssChannel
			''' <summary>
			''' The forum title.
			''' </summary>
			''' <remarks></remarks>
			Public Title As String
			''' <summary>
			''' A link to the forum.
			''' </summary>
			''' <remarks></remarks>
			Public Link As String
			''' <summary>
			''' The forum description.
			''' </summary>
			''' <remarks></remarks>
			Public Description As String
			''' <summary>
			''' The date the feed was published.
			''' </summary>
			''' <remarks></remarks>
			Public PubDate As String
			''' <summary>
			''' The last time the feed was updated.
			''' </summary>
			''' <remarks></remarks>
			Public LastBuildDate As String
			''' <summary>
			''' The time the feed should wait until checking for updates.
			''' </summary>
			''' <remarks></remarks>
			Public TimeToLive As String
		End Structure

		''' <summary>
		''' RssItem represents a PostID.
		''' </summary>
		''' <remarks></remarks>
		Structure RssItem
			''' <summary>
			''' The title of the post.
			''' </summary>
			''' <remarks></remarks>
			Public Title As String
			''' <summary>
			''' The date the post was created.
			''' </summary>
			''' <remarks></remarks>
			Public PostedDate As String
			''' <summary>
			''' The author of the post.
			''' </summary>
			''' <remarks></remarks>
			Public Author As String
			''' <summary>
			''' A link to the thread/post.
			''' </summary>
			''' <remarks></remarks>
			Public Link As String
			''' <summary>
			''' Number of comments associated with the post
			''' </summary>
			''' <remarks></remarks>
			Public CommentCount As Integer
			''' <summary>
			''' The post body.
			''' </summary>
			''' <remarks></remarks>
			Public Description As String
		End Structure

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Constructor instantiates a new instance of the class.
        ''' </summary>
        ''' <param name="ForumID"></param>
        ''' <param name="ThreadsPage"></param>
        ''' <param name="TabId"></param>
        ''' <param name="ModuleId"></param>
        ''' <remarks></remarks>
        Sub New(ByVal ForumID As Integer, ByVal ThreadsPage As Integer, ByVal TabId As Integer, ByVal ModuleId As Integer)
            MyBase.New()

            Dim forum As New ForumInfo
            mForumConfig = Configuration.GetForumConfig(ModuleId)

            If ForumID <> -1 Then
                Dim cntForum As New ForumController
                forum = cntForum.GetForumItemCache(ForumID)
            Else
                forum.Name = Localization.GetString("AggregatedForumName", mForumConfig.SharedResourceFile)
                forum.Description = Localization.GetString("AggregatedForumDescription", mForumConfig.SharedResourceFile)
            End If

            mThreadsPage = ThreadsPage
            mCreationTime = DateTime.Now.ToUniversalTime

            Dim declaration As System.Xml.XmlDeclaration = CreateXmlDeclaration("1.0", Nothing, Nothing)
            InsertBefore(declaration, DocumentElement)

            Dim rssElement As System.Xml.XmlElement = CreateElement("rss")
            Dim rssVersion As System.Xml.XmlAttribute = CreateAttribute("version")

            rssVersion.InnerText = "2.0"
            rssElement.Attributes.Append(rssVersion)

            Dim wellformedweb As System.Xml.XmlAttribute = CreateAttribute("xmlns:wfw")
            wellformedweb.InnerText = wellformedwebUri
            rssElement.Attributes.Append(wellformedweb)

            Dim slash As System.Xml.XmlAttribute = CreateAttribute("xmlns:slash")
            slash.InnerText = slashUri
            rssElement.Attributes.Append(slash)

            Dim dublin As System.Xml.XmlAttribute = CreateAttribute("xmlns:dc")
            dublin.InnerText = dublinCoreUri
            rssElement.Attributes.Append(dublin)

            'Dim rel As System.Xml.XmlAttribute = CreateAttribute("rel")
            Dim atom As System.Xml.XmlAttribute = CreateAttribute("xmlns:atom")
            atom.InnerText = atomUri
            rssElement.Attributes.Append(atom)

            'xmlns:trackback
            AppendChild(rssElement)

            Dim channel As New RssChannel
            With channel
                .Title = forum.Name
                .Description = forum.Description
                .PubDate = System.Xml.XmlConvert.ToString(mCreationTime, "r")

                If forum.MostRecentPost IsNot Nothing Then
                    .LastBuildDate = System.Xml.XmlConvert.ToString(forum.MostRecentPost.CreatedDate.ToUniversalTime(), "r")
                Else
                    .LastBuildDate = .PubDate
                End If

                .TimeToLive = mForumConfig.RSSUpdateInterval.ToString

                If ThreadsPage > 1 Then
                    If ForumID = -1 Then
                        .Link = Utilities.Links.ContainerAggregatedLink(TabId, False)
                    Else
                        .Link = Utilities.Links.ContainerViewForumLink(mForumConfig.CurrentPortalSettings.PortalId, TabId, ForumID, False, forum.Name)
                    End If
                Else
                    If ForumID = -1 Then
                        .Link = Utilities.Links.ContainerAggregatedLink(TabId, False)
                    Else
                        .Link = Utilities.Links.ContainerViewForumLink(mForumConfig.CurrentPortalSettings.PortalId, TabId, ForumID, False, forum.Name)
                    End If
                End If
            End With

            AddChannel(channel)

            ' For rss threads to be obtained w/ anonymous perms
            Dim server As HttpServerUtility = HttpContext.Current.Server
            Dim cleanBody As String
            Dim threadItem As New RssItem

            If ForumID <> -1 Then
                Dim cntThread As New ThreadController
                Dim arrThread As List(Of ThreadInfo)
                Dim objThread As ThreadInfo

                arrThread = cntThread.GetRSSFeed(ModuleId, ForumID, mForumConfig.RSSThreadsPerFeed, (ThreadsPage - 1), "", mForumConfig.CurrentPortalSettings.PortalId, 0)

                For Each objThread In arrThread
                    Dim bodyForumText As New Utilities.PostContent(server.HtmlDecode(objThread.Body), mForumConfig)

                    If mForumConfig.EnableBadWordFilter Then
                        cleanBody = Utilities.ForumUtils.FormatProhibitedWord(bodyForumText.ProcessHtml(), objThread.CreatedDate, mForumConfig.CurrentPortalSettings.PortalId)
                    Else
                        cleanBody = bodyForumText.ProcessHtml()
                    End If

                    With threadItem
                        .Title = objThread.Subject
                        .Author = objThread.LastApprovedUser.SiteAlias
                        .Description = cleanBody
                        ' Date format r is RFC 1123, same as RFC 822 necessary for RSS
                        .PostedDate = System.Xml.XmlConvert.ToString(objThread.CreatedDate.ToUniversalTime, "r")
                        .Link = Utilities.Links.ContainerViewPostLink(TabId, ForumID, objThread.LastApprovedPostID)
                        .CommentCount = objThread.Replies
                    End With
                    AddItem(threadItem)
                Next
            Else
                Dim cntSearch As New SearchController
                Dim colThreads As List(Of ThreadInfo)
                Dim objSearch As ThreadInfo

                'Temp variables
                Dim StartDate As DateTime = DateAdd(DateInterval.Year, -1, DateTime.Today)
                Dim EndDate As DateTime = DateAdd(DateInterval.Day, 1, DateTime.Today)

                colThreads = cntSearch.SearchGetResults(" ", ThreadsPage - 1, mForumConfig.RSSThreadsPerFeed, -1, ModuleId, StartDate, EndDate, -1)

                For Each objSearch In colThreads
                    Dim bodyForumText As New Utilities.PostContent(server.HtmlDecode(objSearch.LastApprovedPost.Body), mForumConfig)

                    If mForumConfig.EnableBadWordFilter Then
                        cleanBody = Utilities.ForumUtils.FormatProhibitedWord(bodyForumText.ProcessHtml(), objSearch.CreatedDate, mForumConfig.CurrentPortalSettings.PortalId)
                    Else
                        cleanBody = bodyForumText.ProcessHtml()
                    End If

                    With threadItem
                        .Title = objSearch.Subject
                        .Author = objSearch.LastApprovedUser.SiteAlias
                        .Description = cleanBody
                        ' Date format r is RFC 1123, same as RFC 822 necessary for RSS
                        .PostedDate = System.Xml.XmlConvert.ToString(objSearch.CreatedDate.ToUniversalTime, "r")
                        .Link = Utilities.Links.ContainerViewPostLink(TabId, objSearch.ForumID, objSearch.LastApprovedPostID)
                        .CommentCount = objSearch.Replies
                    End With
                    AddItem(threadItem)
                Next
            End If

            If forum.TotalThreads > mForumConfig.ThreadsPerPage * ThreadsPage Then
                AddEndItem()
            End If
        End Sub

        ''' <summary>
        ''' Creates a new forum for RSS consumption.
        ''' </summary>
        ''' <param name="ForumChannel"></param>
        ''' <remarks></remarks>
        Private Sub AddChannel(ByVal ForumChannel As RssChannel)
			Dim channel As System.Xml.XmlElement = CreateElement("channel")
			Dim rssElement As System.Xml.XmlNode = SelectSingleNode("rss")

			rssElement.AppendChild(channel)

			Dim titleElement As System.Xml.XmlElement = CreateElement("title")
			titleElement.InnerText = ForumChannel.Title
			channel.AppendChild(titleElement)

			Dim linkElement As System.Xml.XmlElement = CreateElement("link")
			linkElement.InnerText = ForumChannel.Link
			channel.AppendChild(linkElement)

			Dim descriptionElement As System.Xml.XmlElement = CreateElement("description")
			descriptionElement.InnerText = ForumChannel.Description
			channel.AppendChild(descriptionElement)

			Dim pubDateElement As System.Xml.XmlElement = CreateElement("pubDate")
			pubDateElement.InnerText = ForumChannel.PubDate
			channel.AppendChild(pubDateElement)

			Dim lastBuildDateElement As System.Xml.XmlElement = CreateElement("lastBuildDate")
			lastBuildDateElement.InnerText = ForumChannel.LastBuildDate
			channel.AppendChild(lastBuildDateElement)

			Dim ttlElement As System.Xml.XmlElement = CreateElement("ttl")
			ttlElement.InnerText = ForumChannel.TimeToLive
			channel.AppendChild(ttlElement)
		End Sub

		''' <summary>
		''' Creates a new post for RSS consumption.
		''' </summary>
		''' <param name="ThreadItem"></param>
		''' <remarks></remarks>
		Private Sub AddItem(ByVal ThreadItem As RssItem)
			Dim itemElement As System.Xml.XmlElement = CreateElement("item")
			Dim channelElement As System.Xml.XmlNode = SelectSingleNode("rss/channel")

			Dim titleElement As System.Xml.XmlElement = CreateElement("title")
			' need to strip html from title here
			titleElement.InnerText = HttpUtility.HtmlDecode(ThreadItem.Title)
			itemElement.AppendChild(titleElement)

			Dim dateElement As System.Xml.XmlElement = CreateElement("pubDate")
			dateElement.InnerText = ThreadItem.PostedDate
			itemElement.AppendChild(dateElement)

			' We aren't using actual author element because it wants email, nothing else.
			'Dim authorElement As System.Xml.XmlElement = CreateElement("author")
			'authorElement.InnerText = ThreadItem.Author
			'itemElement.AppendChild(authorElement)

			Dim authorElement As System.Xml.XmlElement = CreateElement("dc", "creator", dublinCoreUri)
			authorElement.InnerText = ThreadItem.Author
			itemElement.AppendChild(authorElement)

			Dim linkElement As System.Xml.XmlElement = CreateElement("link")
			linkElement.InnerText = ThreadItem.Link
			itemElement.AppendChild(linkElement)

			Dim guidElement As System.Xml.XmlElement = CreateElement("guid")
			guidElement.InnerText = ThreadItem.Link
			itemElement.AppendChild(guidElement)

			'Dim atomlinkElement As System.Xml.XmlElement = CreateElement("atom", "link", atomUri)
			'atomlinkElement.SetAttributeNode("rel", "self")
			'itemElement.AppendChild(atomlinkElement)

			Dim descriptionElement As System.Xml.XmlElement = CreateElement("description")
			descriptionElement.InnerText = ThreadItem.Description
			itemElement.AppendChild(descriptionElement)

			If ThreadItem.CommentCount > 0 Then
				Dim commentsElement As System.Xml.XmlElement = CreateElement("comments")
				commentsElement.InnerText = ThreadItem.Link
				itemElement.AppendChild(commentsElement)
			End If

			'     Dim 
			'      writer.WriteElementString("slash:comments", "0")
			'   Dim commentsElement As System.Xml.XmlElement = CreateElement("slash", "comments", slashUri)

			channelElement.AppendChild(itemElement)
		End Sub

		''' <summary>
		''' Creates the last post item for RSS consumption.
		''' </summary>
		''' <remarks></remarks>
		Private Sub AddEndItem()
			Dim itemElement As System.Xml.XmlElement = CreateElement("item")
			Dim channelElement As System.Xml.XmlNode = SelectSingleNode("rss/channel")
			Dim strPage As String = "&threadspage=" & mThreadsPage.ToString
			Dim url As String = String.Empty

			If Not HttpContext.Current.Request.QueryString("threadspage") Is Nothing Then
				url = HttpContext.Current.Request.Url.ToString.Replace(mThreadsPage.ToString, (mThreadsPage + 1).ToString)
			Else
				url = HttpContext.Current.Request.Url.ToString & "&threadspage=" & (mThreadsPage + 1).ToString
			End If

			Dim titleElement As System.Xml.XmlElement = CreateElement("title")
			titleElement.InnerText = "More..."
			itemElement.AppendChild(titleElement)

			Dim linkElement As System.Xml.XmlElement = CreateElement("wfw", "link", wellformedwebUri)
			linkElement.InnerText = url
			itemElement.AppendChild(linkElement)

			channelElement.AppendChild(itemElement)
		End Sub

		''' <summary>
		''' Gets the RSS feed for a forumID.
		''' </summary>
		''' <param name="ForumID"></param>
		''' <param name="ThreadsPage"></param>
		''' <param name="TabId"></param>
		''' <param name="ModuleId"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetForumRss(ByVal ForumID As Integer, ByVal ThreadsPage As Integer, ByVal TabId As Integer, ByVal ModuleId As Integer) As RssDocument
			Dim app As HttpApplicationState = HttpContext.Current.Application
			Dim rss As RssDocument
			rss = New RssDocument(ForumID, ThreadsPage, TabId, ModuleId)

			Return rss
		End Function

#End Region

#Region "Public Properties"

		''' <summary>
		''' The time the RSS feed was created.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property CreationTime() As DateTime
			Get
				Return mCreationTime
			End Get
		End Property

#End Region

	End Class

#End Region

End Namespace

