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

Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.UI.UserControls
Imports DotNetNuke.Web.Client.ClientResourceManagement
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Forum.Library
Imports DotNetNuke.Framework.JavaScriptLibraries

Namespace DotNetNuke.Modules.Forum.Utilities

    ''' <summary>
    ''' Various utility methods used throughout the forum module. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ForumUtils
        Shared log As Instrumentation.DnnLogger = Instrumentation.DnnLogger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString())

#Region "General Methods"

        ''' <summary>
        ''' This method is necessary to use the dnn label control on other controls which are loaded dynamically via Ajax.
        ''' </summary>
        ''' <param name="Page"></param>
        ''' <remarks>If a control is initially loaded with the dnn label control on it, this is not necessary but usage should be done just in case.</remarks>
        Public Shared Sub RegisterClientDependencies(ByVal Page As CDefault)
            ClientAPI.RegisterClientReference(Page, ClientAPI.ClientNamespaceReferences.dnn)
            Page.ClientScript.RegisterClientScriptInclude("hoverintent", ResolveUrl("~/Resources/Shared/Scripts/jquery/jquery.hoverIntent.min.js"))
            JavaScript.RequestRegistration(CommonJs.DnnPlugins)
            Page.ClientScript.RegisterClientScriptBlock(GetType(LabelControl), "dnnTooltip", "jQuery(document).ready(function($){ $('.dnnTooltip').dnnTooltip();Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function(){$('.dnnTooltip').dnnTooltip();}); });", True)
        End Sub

        ''' <summary>
        ''' Used to add the host domain info for relative URL's.
        ''' </summary>
        ''' <param name="URL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function AddHost(ByVal URL As String, ByVal PrimaryHost As String) As String
            Dim strReturn As String
            'Dim strHost As String = HttpContext.Current.Request.ServerVariables("HTTP_HOST")

            ' Check if this URL has already included host
            If Not URL.ToLower.IndexOf(PrimaryHost.ToLower) >= 0 Then
                If PrimaryHost.EndsWith("/") Then
                    PrimaryHost = PrimaryHost.TrimEnd("/"c)
                End If
                If Not URL.StartsWith("/") Then
                    URL = "/" & URL
                End If
                strReturn = AddHTTP(PrimaryHost & URL)
            Else
                strReturn = URL
            End If
            Dim test As String = IO.Path.Combine(PrimaryHost, URL)
            Return strReturn
        End Function

        ''' <summary>
        ''' Determines a user's post ranking.
        ''' </summary>
        ''' <param name="User">The Forum User.</param>
        ''' <param name="objConfig">The forum module configuratoin.</param>
        ''' <returns>The poster ranking value.</returns>
        ''' <remarks></remarks>
        Public Shared Function GetRank(ByVal User As ForumUserInfo, ByVal objConfig As Forum.Configuration) As PosterRank
            If Not objConfig.Ranking Then
            Else
                If User.PostCount >= objConfig.FirstRankPosts Then Return PosterRank.First
                If User.PostCount >= objConfig.SecondRankPosts Then Return PosterRank.Second
                If User.PostCount >= objConfig.ThirdRankPosts Then Return PosterRank.Third
                If User.PostCount >= objConfig.FourthRankPosts Then Return PosterRank.Fourth
                If User.PostCount >= objConfig.FifthRankPosts Then Return PosterRank.Fifth
                If User.PostCount >= objConfig.SixthRankPosts Then Return PosterRank.Sixth
                If User.PostCount >= objConfig.SeventhRankPosts Then Return PosterRank.Seventh
                If User.PostCount >= objConfig.EigthRankPosts Then Return PosterRank.Eigth
                If User.PostCount >= objConfig.NinthRankPosts Then Return PosterRank.Ninth
                If User.PostCount >= objConfig.TenthRankPosts Then Return PosterRank.Tenth

            End If
            Return PosterRank.None
        End Function

        ''' <summary>
        ''' Determines the post ranking title to display to users.
        ''' </summary>
        ''' <param name="Rank">The ranking value</param>
        ''' <param name="objConfig">The forum module configuration.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRankTitle(ByVal Rank As PosterRank, ByVal objConfig As Forum.Configuration) As String
            Dim RankTitle As String = String.Empty

            Select Case Rank
                Case PosterRank.First
                    If objConfig.Rank_1_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_1_Title
                    Else
                        RankTitle = Localization.GetString("Rank_1", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Second
                    If objConfig.Rank_2_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_2_Title
                    Else
                        RankTitle = Localization.GetString("Rank_2", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Third
                    If objConfig.Rank_3_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_3_Title
                    Else
                        RankTitle = Localization.GetString("Rank_3", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Fourth
                    If objConfig.Rank_4_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_4_Title
                    Else
                        RankTitle = Localization.GetString("Rank_4", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Fifth
                    If objConfig.Rank_5_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_5_Title
                    Else
                        RankTitle = Localization.GetString("Rank_5", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Sixth
                    If objConfig.Rank_6_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_6_Title
                    Else
                        RankTitle = Localization.GetString("Rank_6", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Seventh
                    If objConfig.Rank_7_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_7_Title
                    Else
                        RankTitle = Localization.GetString("Rank_7", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Eigth
                    If objConfig.Rank_8_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_8_Title
                    Else
                        RankTitle = Localization.GetString("Rank_8", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Ninth
                    If objConfig.Rank_9_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_9_Title
                    Else
                        RankTitle = Localization.GetString("Rank_9", objConfig.SharedResourceFile)
                    End If
                Case PosterRank.Tenth
                    If objConfig.Rank_10_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_10_Title
                    Else
                        RankTitle = Localization.GetString("Rank_10", objConfig.SharedResourceFile)
                    End If
                Case Else
                    If objConfig.Rank_0_Title <> String.Empty Then
                        RankTitle = objConfig.Rank_0_Title
                    Else
                        RankTitle = Localization.GetString("Rank_0", objConfig.SharedResourceFile)
                    End If
            End Select
            Return RankTitle
        End Function

        ''' <summary>
        ''' Uses cores GeoIP data file to determine a user's country.
        ''' </summary>
        ''' <param name="IP">The IP Address of the user being looked up.</param>
        ''' <returns>The country name or IP if country not found.</returns>
        ''' <remarks>Lookup only done if </remarks>
        Public Shared Function LookupCountry(ByVal IP As String) As String
            Dim _GeoIPFile As String = "controls/CountryListBox/Data/GeoIP.dat"
            Dim _CountryLookup As UI.WebControls.CountryLookup
            Dim _UserCountry As String = String.Empty
            Dim Context As HttpContext
            Context = HttpContext.Current

            Try
                'Check to see if we need to generate the Cache for the GeoIPData file
                If Context.Cache.Get("GeoIPData") Is Nothing Then
                    'Store it as	well as	setting	a dependency on	the	file
                    Context.Cache.Insert("GeoIPData", UI.WebControls.CountryLookup.FileToMemory(Context.Server.MapPath(_GeoIPFile)), New CacheDependency(Context.Server.MapPath(_GeoIPFile)))
                End If

                'Yes, get it from cache
                _CountryLookup = New UI.WebControls.CountryLookup(CType(Context.Cache.Get("GeoIPData"), IO.MemoryStream))

                'Get the country code based on the IP address
                _UserCountry = _CountryLookup.LookupCountryName(IP)

            Catch ex As Exception
                LogException(ex)
            End Try

            Return _UserCountry
        End Function

        ''' <summary>
        ''' Attaches the db object qualifier for all inline SQL
        ''' </summary>
        ''' <returns>The objectqualifier set in the web.config</returns>
        ''' <remarks></remarks>
        Public Shared Function DatabaseObjectQualifier() As String
            Dim _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration("data")
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)
            Dim _objectQualifier As String = String.Empty

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> String.Empty And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            Return _objectQualifier
        End Function

        ''' <summary>
        ''' This is used to load the stylesheet associated with the set forum theme. 
        ''' </summary>
        ''' <remarks>This loads into the head area of the page, instead of inline as part of the module (Proper format, necessary for Ajax). Mimics how containers load stylesheets.</remarks>
        Public Shared Sub LoadCssFile(ByVal DefaultPage As CDefault, ByVal objConfig As Forum.Configuration)
            ' load the css file
            Dim blnUpdateCache As Boolean = False

            Dim ID As String
            Dim objCSSCache As Hashtable = Nothing

            If DotNetNuke.Entities.Host.Host.PerformanceSetting <> Common.Globals.PerformanceSettings.NoCaching Then
                objCSSCache = CType(DotNetNuke.Common.Utilities.DataCache.GetCache("CSS"), Hashtable)
            End If
            If objCSSCache Is Nothing Then
                objCSSCache = New Hashtable
            End If

            ' module theme style sheet
            ID = CreateValidID(objConfig.ThemeImageFolder)
            If objCSSCache.ContainsKey(ID) = False Then
                objCSSCache(ID) = objConfig.Css
                blnUpdateCache = True
            End If
            If objCSSCache(ID).ToString <> "" Then
                ClientResourceManager.RegisterStyleSheet(DefaultPage, objCSSCache(ID).ToString)
            End If

            If DotNetNuke.Entities.Host.Host.PerformanceSetting <> Common.Globals.PerformanceSettings.NoCaching Then
                If blnUpdateCache Then
                    DotNetNuke.Common.Utilities.DataCache.SetCache("CSS", objCSSCache)
                End If
            End If
        End Sub

#End Region

#Region "File System"

        ''' <summary>
        ''' Checks to see if a folder exists in the DNN file system, if not it will be created. 
        ''' </summary>
        ''' <param name="Path">The folder to check for.</param>
        ''' <remarks>This is based on the root of the current portal. Requires context.</remarks>
        Public Shared Sub CheckFolder(ByVal Path As String)
            If Path <> String.Empty Then
                Dim _portalSettings As Portals.PortalSettings = Portals.PortalController.Instance.GetCurrentPortalSettings()
                Dim folderNames() As String = Path.Trim("/"c).Split("/"c)
                Dim completeFolderPath As String = _portalSettings.HomeDirectoryMapPath

                For Each folderName As String In folderNames
                    If folderName <> String.Empty Then
                        completeFolderPath = System.IO.Path.Combine(completeFolderPath, folderName)
                        'DotNetNuke.Instrumentation.DnnLogger.GetLogger("ForumUtils").Debug("completeFolderPath: " + completeFolderPath)
                        If FileUtilityClass.GetFolder(completeFolderPath, _portalSettings.PortalId) Is Nothing Then
                            FileUtilityClass.AddFolder(completeFolderPath, _portalSettings.PortalId)
                        End If
                    End If
                Next
            End If
        End Sub

#End Region

#Region "Email Utils"

        ''' <summary>
        ''' All email sends for this module should go through this function
        ''' </summary>
        ''' <param name="ContentID">Generic variable used for token parsing.</param>
        ''' <param name="URL">Used for parsing</param>
        ''' <param name="EmailType">Used to determine what email template to start with and how to populate the distro list.</param>
        ''' <param name="Notes">Used for parsing</param>
        ''' <param name="objConfig">Used for parsing and ModuleID (needed for task)</param>
        ''' <param name="ProfileURL">Used for parsing</param>
        ''' <param name="PortalID">Needed for distro list creation and task</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SendForumMail(ByVal ContentID As Integer, ByVal URL As String, ByVal EmailType As ForumEmailType, ByVal Notes As String, ByVal objConfig As Forum.Configuration, ByVal ProfileURL As String, ByVal PortalID As Integer) As String
            ' Cannot inherit from DotNEtNuke.Services.Mail.Mail because it is too limited
            Dim forumMail As New ForumEmail
            'handle converting URL for FURLS/NON FURLS
            URL = Utilities.ForumUtils.EmailFullURL(URL)
            ProfileURL = Utilities.ForumUtils.EmailFullURL(ProfileURL)

            forumMail.GenerateEmail(Notes, EmailType, ContentID, objConfig, URL, ProfileURL, PortalID)
            'Idealy this would run in its own thread in the bg. There is an asp.net thread limit of 25, so we need a constant 'service' (scheduled task) to do this
            ' This would need to check setting to see if its available first. WHich should be abstracted w/ the provider so modules all don't have to check
            ' Once the body is parsed w/ specific changes and everything is set, send mailformat, distribution list (which maybe is sproc name used to retrieve list?), subject, body
            ' This abstract provider should handle core token replacement (like system messages, with portalinfo and userinfo specific information) THIS SHOULD BE CONTROLLED VIA boolean to avoid doing this processing when its not necessary which increases performance)

            If objConfig.EnableEmailQueueTask Then
                ' Handle sending post deleted to user separate (because post will be removed and there is no way to get the recipient using queue since post will be gone from db)
                If EmailType = ForumEmailType.UserPostDeleted Then
                    Dim sprocName As String = forumMail.DistroCall
                    Dim params As String = forumMail.DistroParams
                    Dim DistroCnt As New ForumEmailDistributionController
                    Dim arrEmails As List(Of ForumEmailDistributionInfo)

                    arrEmails = DistroCnt.GetEmailsToSend(sprocName, params, 0)

                    ' Used to maintain list of who received (NOT FINISHED)
                    Dim arrSentEmails As New List(Of ForumEmailDistributionInfo)

                    For Each objEmail As ForumEmailDistributionInfo In arrEmails
                        ' do not use the scheduled task, go ahead and send emails now (one at a time)
                        Dim emailFormat As String = String.Empty
                        Dim emailBody As String = String.Empty

                        If objEmail.EmailFormat = True Then
                            emailFormat = "html"
                            emailBody = forumMail.EmailHTMLBody
                        Else
                            emailFormat = "text"
                            emailBody = forumMail.EmailTextBody
                        End If
                        DotNetNuke.Services.Mail.Mail.SendMail(forumMail.EmailFromAddress, objEmail.Email, "", forumMail.Subject, emailBody, "", emailFormat, "", "", "", "")
                        arrSentEmails.Add(objEmail)
                        'CP - COMEBACK This is part not finished, should log every outgoing email
                        arrSentEmails.Clear()
                    Next
                Else
                    ' Make sure we set all forumMail items above
                    Dim objThreadCnt As New ThreadController
                    objThreadCnt.QueueEmail(forumMail.EmailFromAddress, forumMail.FromFriendlyName, forumMail.Priority, forumMail.EmailHTMLBody, forumMail.EmailTextBody, forumMail.Subject, PortalID, forumMail.QueuePriority, objConfig.ModuleID, forumMail.EnableFriendlyToName, forumMail.DistroCall, forumMail.DistroIsSproc, forumMail.DistroParams, Date.Now(), False, "")
                End If
            Else
                Dim sprocName As String = forumMail.DistroCall
                Dim params As String = forumMail.DistroParams
                Dim DistroCnt As New ForumEmailDistributionController
                Dim arrEmails As List(Of ForumEmailDistributionInfo)

                arrEmails = removeDuplicates(DistroCnt.GetEmailsToSend(sprocName, params, 0))

                ' Used to maintain list of who received (NOT FINISHED)
                Dim arrSentEmails As New List(Of ForumEmailDistributionInfo)

                For Each objEmail As ForumEmailDistributionInfo In arrEmails
                    ' do not use the scheduled task, go ahead and send emails now (one at a time)
                    Dim emailFormat As String = String.Empty
                    Dim emailBody As String = String.Empty

                    If objEmail.EmailFormat = True Then
                        emailFormat = "html"
                        emailBody = forumMail.EmailHTMLBody
                    Else
                        emailFormat = "text"
                        emailBody = forumMail.EmailTextBody
                    End If
                    ' CP - COMEBACK: This is not respecting friendly name because it is using core send method (possibly extract my .NET methods to a re-usable class).
                    DotNetNuke.Services.Mail.Mail.SendMail(forumMail.EmailFromAddress, objEmail.Email, "", forumMail.Subject, emailBody, "", emailFormat, "", "", "", "")
                    arrSentEmails.Add(objEmail)
                    'CP COMEBACK - This is part not finished, should log every outgoing email
                    arrSentEmails.Clear()
                Next
            End If

            Return ""
        End Function

        ''' <summary>
        ''' This is used to make sure links are proper if FURL's are off
        ''' </summary>
        ''' <param name="mailURL"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Private Shared Function EmailFullURL(ByVal mailURL As String) As String
            'this may be possible if FURL's are off
            If mailURL.StartsWith("http") = False Then
                'Removing virtual directory from portal alias.
                Dim _portalSettings As Portals.PortalSettings = Portals.PortalController.Instance.GetCurrentPortalSettings
                Dim domain As String = _portalSettings.PortalAlias.HTTPAlias.Replace(HttpContext.Current.Request.ApplicationPath, "")

                If domain.EndsWith("/") Then
                    mailURL = AddHTTP(domain + mailURL)
                Else
                    mailURL = AddHTTP(domain + "/" + mailURL)
                End If
            End If

            Return mailURL
        End Function

        ''' <summary>
        ''' Removes duplicate email address from the collection. 
        ''' </summary>
        ''' <param name="inputList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function removeDuplicates(ByVal inputList As List(Of ForumEmailDistributionInfo)) As List(Of ForumEmailDistributionInfo)
            Dim uniqueStore As New Dictionary(Of ForumEmailDistributionInfo, Integer)
            Dim finalList As New List(Of ForumEmailDistributionInfo)
            For Each currValue As ForumEmailDistributionInfo In inputList
                If Not uniqueStore.ContainsKey(currValue) Then
                    uniqueStore.Add(currValue, 0)
                    finalList.Add(currValue)
                End If
            Next
            Return finalList
        End Function

#End Region

#Region "String Replacement"

        ''' <summary>
        ''' This is used to used to set the reply subject of posts.
        ''' </summary>
        ''' <param name="PostSubject"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SetReplySubject(ByVal PostSubject As String) As String
            If (PostSubject.Length >= 3) Then
                If Not (PostSubject.Substring(0, 3) = "Re:") Then
                    PostSubject = "Re: " + PostSubject
                End If
            Else
                PostSubject = "Re: " + PostSubject
            End If
            Return PostSubject
        End Function

        ''' <summary>
        ''' Replaces text regardless of case of the string
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="oldValue"></param>
        ''' <param name="newValue"></param>
        ''' <returns></returns>
        ''' <remarks>See http://www.aspalliance.com/bbilbro/viewarticle.aspx?paged_article_id=4</remarks>
        Shared Function ReplaceCaseInsensitive(ByVal text As String, ByVal oldValue As String, ByVal newValue As String) As String
            If text <> String.Empty Then
                oldValue = GetCaseInsensitiveSearch(oldValue)
                Return Regex.Replace(text, oldValue, newValue)
            Else
                Return ""
            End If
        End Function

        ''' <summary>
        ''' Helpler function for ReplaceCaseInsensitive
        ''' </summary>
        ''' <param name="search"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetCaseInsensitiveSearch(ByVal search As String) As String
            Dim result As String = String.Empty

            Dim index As Integer

            For index = 0 To search.Length - 1
                Dim character As Char = search.Chars(index)
                Dim characterLower As Char = Char.ToLower(character)
                Dim characterUpper As Char = Char.ToUpper(character)

                If characterUpper = characterLower Then
                    result = result + character
                Else
                    result = result + "[" + characterLower + characterUpper + "]"
                End If

            Next index
            Return result
        End Function

        ''' <summary>
        ''' Used to trim strings based on word count passed in.
        ''' This is similar to a My Subject Here ...
        ''' </summary>
        ''' <param name="Content"></param>
        ''' <param name="CharCount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function TrimString(ByVal Content As String, ByVal CharCount As Integer) As String
            Dim descString As String = String.Empty
            If Content IsNot Nothing Then
                If Content.Length > 0 Then
                    If Len(Content) > CharCount Then
                        descString = Left(Content, CharCount) & "..."
                    Else
                        descString = Content
                    End If
                End If
            End If

            Return descString
        End Function

        ''' <summary>
        ''' Takes a filtered word and replaces it.
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function FormatProhibitedWord(ByVal Text As String, ByVal PortalID As Integer) As String
            Dim ctlWordFilter As New WordFilterController
            Dim cleanText As String = ctlWordFilter.FilterBadWord(Text, PortalID)

            Return cleanText
        End Function

        ''' <summary>
        ''' Filters and replaces a bad word based on date.
        ''' </summary>
        ''' <param name="Text"></param>
        ''' <param name="TimeStamp"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function FormatProhibitedWord(ByVal Text As String, ByVal TimeStamp As DateTime, ByVal PortalID As Integer) As String
            Dim ctlWordFilter As New WordFilterController
            Dim cleanText As String = (ctlWordFilter.FilterBadWord(Text, TimeStamp, PortalID))

            Return cleanText
        End Function

        ''' <summary>
        ''' Converts the HTML tooltips to plain text
        ''' </summary>
        ''' <param name="strText"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function FormatToolTip(ByVal strText As String) As String
            strText = FormatMultiLine(strText)
            strText = FormatPlainText(strText)
            Return strText
        End Function

        ''' <summary>
        ''' Used to parse HTML code and make it text readable (no HTML formatting)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>String of plain text.</remarks>
        Shared Function FormatPlainText(ByVal strText As String) As String
            strText = ReplaceCaseInsensitive(strText, "<br>", vbCrLf & vbCrLf)
            strText = ReplaceCaseInsensitive(strText, "<br />", vbCrLf & vbCrLf)
            strText = ReplaceCaseInsensitive(strText, "<br/>", vbCrLf & vbCrLf)
            strText = ReplaceCaseInsensitive(strText, "<a href=", "")
            strText = ReplaceCaseInsensitive(strText, "</a>", "")

            strText = ReplaceCaseInsensitive(strText, "<b>", "")
            strText = ReplaceCaseInsensitive(strText, "</b>", "")
            strText = ReplaceCaseInsensitive(strText, "&lt;b&gt;", "")
            strText = ReplaceCaseInsensitive(strText, "&lt;/b&gt;", "")

            strText = ReplaceCaseInsensitive(strText, "<i>", "")
            strText = ReplaceCaseInsensitive(strText, "</i>", "")
            strText = ReplaceCaseInsensitive(strText, """", "'")
            strText = ReplaceCaseInsensitive(strText, "/>", "")
            strText = ReplaceCaseInsensitive(strText, "&lt;", "<")
            strText = ReplaceCaseInsensitive(strText, "&gt;", ">")
            strText = ReplaceCaseInsensitive(strText, "<p>", vbCrLf)
            strText = ReplaceCaseInsensitive(strText, "</p>", vbCrLf)

            strText = ReplaceCaseInsensitive(strText, "<blockquote>", "-----------------------------------------" & vbCrLf)
            strText = ReplaceCaseInsensitive(strText, "</blockquote>", vbCrLf & "-----------------------------------------" & vbCrLf)

            strText = ReplaceCaseInsensitive(strText, "<ol>", vbCrLf)
            strText = ReplaceCaseInsensitive(strText, "</ol>", vbCrLf)

            strText = ReplaceCaseInsensitive(strText, "<ul>", vbCrLf)
            strText = ReplaceCaseInsensitive(strText, "</ul>", vbCrLf)

            strText = ReplaceCaseInsensitive(strText, "<li>", "")
            strText = ReplaceCaseInsensitive(strText, "</li>", vbCrLf)

            strText = ReplaceCaseInsensitive(strText, "<strong>", "")
            strText = ReplaceCaseInsensitive(strText, "</strong>", "")

            'strText = ReplaceCaseInsensitive(strText, "<span", "")
            strText = ReplaceCaseInsensitive(strText, "</span>", "")

            strText = ReplaceCaseInsensitive(strText, "<strike>", "")
            strText = ReplaceCaseInsensitive(strText, "</strike>", "")

            strText = ReplaceCaseInsensitive(strText, "&nbsp;", " ")
            strText = ReplaceCaseInsensitive(strText, "&amp;&nbsp;", " ")
            strText = ReplaceCaseInsensitive(strText, "&amp;", " ")

            strText = ReplaceCaseInsensitive(strText, "&amp;quot;", "'")
            strText = ReplaceCaseInsensitive(strText, "&quot", "'")

            Return strText
        End Function

        ''' <summary>
        ''' Used to format multiline HTML elements so it can be parsed for plain text usage.
        ''' </summary>
        ''' <param name="strText"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function FormatMultiLine(ByVal strText As String) As String
            strText = strText.Replace(ControlChars.Cr + ControlChars.Lf, ControlChars.Lf)
            strText = strText.Replace(ControlChars.Cr, ControlChars.Lf)
            strText = strText.Replace("<br><P>", "<P>")
            strText = strText.Replace("<P><br>", "<P>")
            strText = strText.Replace("<table><br>", "<table>")
            strText = strText.Replace("<br></table>", "</table>")
            strText = strText.Replace("<td><br>", "<td>")
            strText = strText.Replace("<tr><br>", "<tr>")
            strText = strText.Replace("</td><br>", "</td>")
            strText = strText.Replace("</tr><br>", "</tr>")

            Return strText
        End Function

        ''' <summary>
        ''' Addes the full url for outging HTML emails for images.
        ''' </summary>
        ''' <param name="mText">The email body to parse.</param>
        ''' <returns>The HTML email body with the full image paths adjusted.</returns>
        ''' <remarks></remarks>
        Shared Function FormatEmailImage(ByVal mText As String, ByVal PrimaryAlias As String) As String
            Dim fullURL As String = AddHost("/", PrimaryAlias)
            mText = ReplaceCaseInsensitive(mText, "src=""/", "src=""" & fullURL)
            Return mText
        End Function

        ''' <summary>
        ''' This function will remove all quotes formatting prior to saving
        ''' the post to the database 
        ''' </summary>
        ''' <param name="PostBody"></param>>
        ''' <remarks>Added by Skeel. This should only be done at display time, so it properly accounts for localization. For no context (and thus no culture/language) we should use the user's profile.</remarks>
        Shared Function ProcessStripQuotesRender(ByVal PostBody As String, ByVal objConfig As Forum.Configuration) As String
            'Replace &#160; with <space>, only need to do this once..
            'If regCounter = 0 Then
            '	PostBody = PostBody.Replace("&#160;", " ")
            'End If

            'Handle Quotes with names
            Dim strExpresson As String = "\<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile, objConfig.CurrentPortalSettings, "").Trim & "(?::|)\<\/em>\<br \/>([^\]]+)(\<\/div>)"
            Dim regExp As Regex = New Regex(strExpresson)
            PostBody = regExp.Replace(PostBody, "[quote=""$1""]$2[/quote]")

            'Handle Quotes without names
            strExpresson = "\<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+)<\/div>"
            regExp = New Regex(strExpresson)
            PostBody = regExp.Replace(PostBody, "[quote]$1[/quote]")

            ''Now we need to check if there were quotes beside each other
            'strExpresson = "\<\/div>([^\]]+)<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>"
            'regExp = New Regex(strExpresson)
            'PostBody = regExp.Replace(PostBody, "[/quote]$1[quote=""$2""]")

            ''And the same for quotes without names..
            'strExpresson = "\<\/div>([^\]]+)<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>"
            'regExp = New Regex(strExpresson)
            'PostBody = regExp.Replace(PostBody, "[/quote]$1[quote]")

            ''And the final scenario
            'strExpresson = "\<div class=[""]Quote[""]><em>(\w+)\ " & Localization.GetString("ForumTextWrote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+]+[^\]]+)\<\/div>"
            'regExp = New Regex(strExpresson)
            'PostBody = regExp.Replace(PostBody, "[quote=""$1""]$2[/quote]")

            ''And the final scenario without name
            'strExpresson = "\<div class=[""]Quote[""]><em>" & Localization.GetString("Quote.Text", objConfig.SharedResourceFile).Trim & "(?::|)\<\/em>\<br \/>([^\]]+]+[^\]]+)\<\/div>"
            'regExp = New Regex(strExpresson)
            'PostBody = regExp.Replace(PostBody, "[quote=""$1""]$2[/quote]")

            ''Add to the counter
            'regCounter = regCounter + 1

            ''Check for more instances..
            'If PostBody.IndexOf("<div class=""Quote""><em>") > -1 And regCounter < 10 Then
            '	'PostBody = ProcessStripQuotes(PostBody, objConfig)
            'End If

            Return PostBody

        End Function

        ''' <summary>
        ''' Replaces inline attachments with the filename.
        ''' Used when quoting a post that contains inline attachments 
        ''' </summary>
        ''' <remarks></remarks>
        Shared Function RemoveInlineAttachments(ByVal strTextToRemove As String) As String
            Dim mRegOptions As RegexOptions = RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.Singleline
            Dim regInline As New Regex("\[attachment\](?<filename>(.*?))\[/attachment\]", mRegOptions)
            Dim strTmp As String

            strTmp = String.Format("'{0}'", "${filename}")
            strTextToRemove = regInline.Replace(strTextToRemove, strTmp)

            Return strTextToRemove
        End Function

#End Region

#Region "BreadCrumbs"

        ''' <summary>
        ''' Builds the breadcrumb hierarchy shown in many forum areas (Forum_Container)
        ''' </summary>
        ''' <param name="TabID"></param>
        ''' <param name="ModuleID"></param>
        ''' <param name="Scope"></param>
        ''' <param name="InfoObject"></param>
        ''' <param name="objConfig"></param>
        ''' <param name="ChildGroupView"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function BreadCrumbs(ByVal TabID As Integer, ByVal ModuleID As Integer, ByVal Scope As ForumScope, ByVal InfoObject As Object, ByVal objConfig As Configuration, ByVal ChildGroupView As Boolean) As String
            Dim sb As New StringBuilder
            sb.Append("<table cellpadding=""1"" cellspacing=""1""><tr>")
            'Only Scope of Threads, Posts need aggregatedview to worry about
            Try
                Dim imageURL As String = objConfig.GetThemeImageURL("breadcrumb." & objConfig.ImageExtension)

                Select Case Scope

                    Case ForumScope.Groups
                        '[skeel] Support for full breadcrumb in group and parent forum view
                        Dim Completed As Boolean = False

                        'Forum Home 
                        sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerForumHome(TabID), Localization.GetString("Home", objConfig.SharedResourceFile), imageURL))

                        'Are we displaying a Group?
                        Try
                            Dim objGroupInfo As GroupInfo
                            objGroupInfo = CType(InfoObject, GroupInfo)
                            If Not objGroupInfo Is Nothing Then
                                ' Render Group Name
                                sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerSingleGroupLink(TabID, objGroupInfo.GroupID), objGroupInfo.Name, imageURL))
                            End If
                            Completed = True
                        Catch ex As Exception
                        End Try

                        'Are we are displaying a parentforum?
                        If Completed = False Then
                            Try
                                Dim objForumInfo As ForumInfo
                                objForumInfo = CType(InfoObject, ForumInfo)
                                If Not objForumInfo Is Nothing Then
                                    ' Render Group Name
                                    sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerSingleGroupLink(TabID, objForumInfo.GroupID), objForumInfo.ParentGroup.Name, imageURL))
                                    ' Render ParentForum Name
                                    sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerParentForumLink(TabID, objForumInfo.GroupID, objForumInfo.ForumID), objForumInfo.Name, imageURL))
                                    Utilities.Links.ContainerParentForumLink(TabID, objForumInfo.GroupID, objForumInfo.ForumID)
                                End If
                            Catch ex As Exception
                            End Try
                        End If

                    Case ForumScope.Threads
                        Dim objForumInfo As ForumInfo
                        objForumInfo = CType(InfoObject, ForumInfo)
                        'Forum Home
                        sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerForumHome(TabID), Localization.GetString("Home", objConfig.SharedResourceFile), imageURL))
                        If objForumInfo.ForumID = -1 And objConfig.AggregatedForums Then
                            ' Render Group Name
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerViewForumLink(TabID, -1, False), Localization.GetString("Aggregate", objConfig.SharedResourceFile), imageURL))
                        Else
                            ' Render Group Name
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerSingleGroupLink(TabID, objForumInfo.GroupID), TrimString(objForumInfo.ParentGroup.Name, 15), imageURL))
                            '[skeel] check for subforum
                            If objForumInfo.ParentID > 0 Then
                                'Render Parent Forum Name
                                sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerParentForumLink(TabID, objForumInfo.GroupID, objForumInfo.ParentID), objForumInfo.ParentForum.Name, imageURL))
                            End If
                            ' Render Forum Name
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerViewForumLink(TabID, objForumInfo.ForumID, False), objForumInfo.Name, imageURL))
                        End If

                    Case ForumScope.Unread
                        'Forum Home
                        sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerForumHome(TabID), Localization.GetString("Home", objConfig.SharedResourceFile), imageURL))
                        'Unread Threads
                        sb = sb.Append(GetBreadCrumb("", Localization.GetString("UnreadThreads", objConfig.SharedResourceFile), imageURL))

                    Case ForumScope.Posts
                        Dim objThreadInfo As ThreadInfo
                        objThreadInfo = CType(InfoObject, ThreadInfo)
                        'Forum Home
                        sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerForumHome(TabID), Localization.GetString("Home", objConfig.SharedResourceFile), imageURL))
                        ' Render Group Name
                        sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerSingleGroupLink(TabID, objThreadInfo.ContainingForum.GroupID), TrimString(objThreadInfo.ContainingForum.ParentGroup.Name, 15), imageURL))

                        'Check if this is a sub forum
                        If objThreadInfo.ContainingForum.ParentID > 0 Then
                            'Render Parent Forum Name
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerParentForumLink(TabID, objThreadInfo.ContainingForum.GroupID, objThreadInfo.ContainingForum.ParentID), objThreadInfo.ContainingForum.ParentForum.Name, imageURL))
                        End If

                        sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerViewForumLink(TabID, objThreadInfo.ForumID, False), TrimString(objThreadInfo.ContainingForum.Name, 15), imageURL))
                        ' Render Thread Name
                        If objConfig.FilterSubject Then
                            Dim strFilteredSubject As String = objThreadInfo.Subject
                            strFilteredSubject = FormatProhibitedWord(strFilteredSubject, objConfig.CurrentPortalSettings.PortalId)
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerViewThreadLink(TabID, objThreadInfo.ForumID, objThreadInfo.ThreadID), strFilteredSubject, imageURL))
                        Else
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerViewThreadLink(TabID, objThreadInfo.ForumID, objThreadInfo.ThreadID), objThreadInfo.Subject, imageURL))
                        End If
                    Case ForumScope.ThreadSearch
                        If (Not InfoObject Is Nothing) And objConfig.AggregatedForums Then
                            'Forum Home
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerForumHome(TabID), Localization.GetString("Home", objConfig.SharedResourceFile), imageURL))
                            ' Render Aggregated Group Name
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerViewForumLink(TabID, -1, False), Localization.GetString("Aggregate", objConfig.SharedResourceFile), imageURL))
                        Else
                            'Forum Home
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerForumHome(TabID), Localization.GetString("Home", objConfig.SharedResourceFile), imageURL))
                            'Search
                            sb = sb.Append(GetBreadCrumb(Utilities.Links.SearchPageLink(TabID, ModuleID), Localization.GetString("Search", objConfig.SharedResourceFile), imageURL))
                            'Results
                            sb = sb.Append(GetBreadCrumb("", Localization.GetString("Results", objConfig.SharedResourceFile), imageURL))
                        End If

                    Case Else
                        'Forum Home
                        sb = sb.Append(GetBreadCrumb(Utilities.Links.ContainerForumHome(TabID), Localization.GetString("Home", objConfig.SharedResourceFile), imageURL))
                End Select
            Catch ex As Exception
                LogException(ex)
            End Try
            sb.Append("</tr></table>")

            Return sb.ToString
        End Function

        ''' <summary>
        ''' Helper function for the breadcrumb function. Places the breadcrumb gif in place when needed.
        ''' </summary>
        ''' <param name="Link"></param>
        ''' <param name="Name"></param>
        ''' <param name="ImageURL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetBreadCrumb(ByVal Link As String, ByVal Name As String, ByVal ImageURL As String) As String
            Dim sb As New StringBuilder

            If ImageURL.Length > 0 Then
                sb.Append("<td><img src=""")
                sb.Append(ImageURL)
                sb.Append(""" alt='" & Name & "' /></td>")
            End If

            '[skeel] Support of non linkable breadcrumbs, like search results..
            If Link = String.Empty Then
                sb.Append("<td class=""Forum_BreadCrumb"">")
                sb.Append(Name)
                sb.Append("</td>")
            Else
                sb.Append("<td><a class='Forum_BreadCrumb' href=""")
                sb.Append(Link)
                sb.Append(""">")
                sb.Append(Name)
                sb.Append("</a></td>")
            End If

            Return sb.ToString
        End Function

#End Region

#Region "Date/Time Handling"

        ''' <summary>
        ''' All time displayed to all users should come through this fuctnion. It converts it to the users preferred timezone and uses "Today/Yesterday" where applicable
        ''' </summary>
        ''' <param name="serverPostedDate"></param>
        ''' <param name="objConfig"></param>
        ''' <param name="CssClass"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetCreatedDateInfo(ByVal serverPostedDate As DateTime, ByVal objConfig As Forum.Configuration, ByVal CssClass As String) As String
            Dim strInfo As String
            Dim userPostedDate As DateTime
            Dim serverPostDay As DateTime
            Dim userPostedDay As DateTime
            Dim userCurrentDate As DateTime
            Dim serverCurrentDate As DateTime
            Dim userCurrentDay As DateTime
            Dim serverCurrentDay As DateTime
            Dim userYesterdayDate As DateTime
            Dim userYesterdayDay As DateTime

            userPostedDate = ConvertTimeZone(serverPostedDate, objConfig)
            userPostedDay = userPostedDate.Date
            serverPostDay = serverPostedDate.Date
            serverCurrentDate = Date.Now()
            userCurrentDate = ConvertTimeZone(serverCurrentDate, objConfig)
            serverCurrentDay = serverCurrentDay.Date
            userCurrentDay = userCurrentDate.Date
            userYesterdayDate = userCurrentDate.Date.Subtract(New TimeSpan(1, 0, 0, 0))
            userYesterdayDay = userYesterdayDate.Date

            If userPostedDay = userCurrentDay Then
                strInfo = "<b>" & Localization.GetString("Today.Text", objConfig.SharedResourceFile) + " @ " + " " + userPostedDate.ToShortTimeString & "</b>"
            ElseIf userPostedDay = userYesterdayDay Then
                strInfo = "<b>" & Localization.GetString("Yesterday.Text", objConfig.SharedResourceFile) + " @ " + " " + userPostedDate.ToShortTimeString & "</b>"
            Else
                strInfo = userPostedDate.ToShortDateString + " " + userPostedDate.ToShortTimeString
            End If

            Return strInfo
        End Function

        ''' <summary>
        ''' Altered to handle DST as well as various other issues w/ timezone conversion
        ''' </summary>
        ''' <param name="value">DateTime</param>
        ''' <param name="objConfig">Forum.Config</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Shared Function ConvertTimeZone(ByVal value As DateTime, ByVal objConfig As Forum.Configuration) As DateTime
            Dim displayCreatedDate As DateTime = value

            Try
                If HttpContext.Current.Request.IsAuthenticated Then
                    Dim GMTTime As DateTime = displayCreatedDate.ToUniversalTime()
                    Dim objUser As Users.UserInfo = Users.UserController.Instance.GetCurrentUserInfo
                    displayCreatedDate = objUser.LocalTime(GMTTime)
                Else
                    displayCreatedDate = TimeZoneInfo.ConvertTime(displayCreatedDate.ToUniversalTime(), TimeZoneInfo.Utc, GetPortalSettings().TimeZone)
                End If
            Catch
                ' The added or subtracted value results in an un-representable DateTime. Possibly a problem with TimeZone values.
                displayCreatedDate = value
            End Try

            Return displayCreatedDate
        End Function

        ''' <summary>
        ''' Old method cause error when user culture has different date format
        ''' </summary>
        ''' <param name="dt"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function DateToNum(ByVal dt As Date) As Double
            Return dt.ToOADate
        End Function

        ''' <summary>
        ''' Converts a Double to a date
        ''' </summary>
        ''' <param name="n"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function NumToDate(ByVal n As Double) As Date
            Return Date.FromOADate(n)
        End Function

#End Region

#Region "Module Actions"

        ''' <summary>
        ''' Determines which css class to assign based on which module action case
        ''' </summary>
        ''' <param name="ActionTitle"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function GetActionCss(ByVal ActionTitle As String, ByVal ForumControl As Forum.DNNForum, ByVal CurrentForumUser As ForumUserInfo) As String
            Dim CssClass As String = String.Empty
            Select Case ActionTitle
                Case ForumControl.LocalizedText("Administration")
                    CssClass = ("Forum_ib_admin")
                Case ForumControl.LocalizedText("Moderate")
                    CssClass = ("Forum_ib_moderate")
                Case ForumControl.LocalizedText("MemberList")
                    CssClass = ("Forum_ib_memberlist")
                Case ForumControl.LocalizedText("MySettings")
                    CssClass = ("Forum_ib_mysettings")
                Case ForumControl.LocalizedText("MyPosts")
                    CssClass = ("Forum_ib_myposts")
                Case ForumControl.LocalizedText("MyThreads")
                    CssClass = ("Forum_ib_mythreads")
                Case ForumControl.LocalizedText("Search")
                    CssClass = ("Forum_ib_search")
                Case ForumControl.LocalizedText("ForumHome")
                    CssClass = ("Forum_ib_forumhome")
                Case ForumControl.LocalizedText("Aggregate")
                    CssClass = ("Forum_ib_aggregate")
            End Select

            Return CssClass
        End Function

        ''' <summary>
        ''' Renders the module actions shown on all ascx controls used by this module which inherit from PortalModuleBase.
        ''' This is specific to each user based on their permissions which determines which are available to them.
        ''' </summary>
        ''' <param name="objConfig"></param>
        ''' <param name="ModBase"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Shared Function PerUserModuleActions(ByVal objConfig As Forum.Configuration, ByVal ModBase As DotNetNuke.Entities.Modules.PortalModuleBase) As DotNetNuke.Entities.Modules.Actions.ModuleActionCollection
            Dim mLoggedOnUserID As Integer = -1
            mLoggedOnUserID = Users.UserController.Instance.GetCurrentUserInfo.UserID
            Dim Security As New Forum.ModuleSecurity(ModBase.ModuleId, ModBase.TabId, -1, mLoggedOnUserID)
            Dim Actions As New DotNetNuke.Entities.Modules.Actions.ModuleActionCollection

            If (mLoggedOnUserID > 0) And (Security.IsModerator) Then
                Actions.Add(ModBase.GetNextActionID, Services.Localization.Localization.GetString("Moderate.Text", ModBase.LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", Utilities.Links.MCPControlLink(ModBase.TabId, ModBase.ModuleId, ModeratorAjaxControl.Main), False, SecurityAccessLevel.View, True, False)
            End If

            If (Security.IsForumAdmin) Then
                Actions.Add(ModBase.GetNextActionID, Services.Localization.Localization.GetString("Administration.Text", ModBase.LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", Utilities.Links.ForumAdminLink(ModBase.TabId, ModBase.ModuleId), False, SecurityAccessLevel.View, True, False)
            End If

            If mLoggedOnUserID > 0 Then
                Actions.Add(ModBase.GetNextActionID, Services.Localization.Localization.GetString("MySettings.Text", ModBase.LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", Utilities.Links.UCP_UserLinks(ModBase.TabId, ModBase.ModuleId, UserAjaxControl.Main, objConfig.CurrentPortalSettings), False, SecurityAccessLevel.View, True, False)
                Actions.Add(ModBase.GetNextActionID, Services.Localization.Localization.GetString("MyPosts.Text", ModBase.LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", Utilities.Links.ContainerMyPostsLink(ModBase.TabId, mLoggedOnUserID), False, SecurityAccessLevel.View, True, False)
                Actions.Add(ModBase.GetNextActionID, Services.Localization.Localization.GetString("MyThreads.Text", ModBase.LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", Utilities.Links.ContainerMyThreadsLink(ModBase.TabId, mLoggedOnUserID), False, SecurityAccessLevel.View, True, False)
            End If

            If objConfig.HideSearchButton = False Then
                Actions.Add(ModBase.GetNextActionID, Services.Localization.Localization.GetString("Search.Text", ModBase.LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", Utilities.Links.SearchPageLink(ModBase.TabId, ModBase.ModuleId), False, SecurityAccessLevel.View, True, False)
            End If

            If objConfig.HideHomeButton = False Then
                Actions.Add(ModBase.GetNextActionID, Services.Localization.Localization.GetString("ForumHome.Text", ModBase.LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", Utilities.Links.ContainerForumHome(ModBase.TabId), False, SecurityAccessLevel.View, True, False)
            End If

            If objConfig.AggregatedForums Then
                Actions.Add(ModBase.GetNextActionID, Services.Localization.Localization.GetString("Aggregate.Text", ModBase.LocalResourceFile), Entities.Modules.Actions.ModuleActionType.ContentOptions, "", "", Utilities.Links.ContainerAggregatedLink(ModBase.TabId, False), False, SecurityAccessLevel.View, True, False)
            End If

            Return Actions
        End Function

#End Region

#Region "JavaScripts"

        ''' <summary>
        ''' Will generate some of the Javascript nessecary to "communicate" with the DNN
        ''' HTMLEditor Provider while objConfig DisableHTMLPosting is set to True.
        ''' This javascript is used by the EmoticonControl and/or the AttachmentControl
        ''' </summary>
        ''' <param name="ModuleID"></param>
        ''' <param name="strControl"></param>
        ''' <returns>Javascript string</returns>
        ''' <remarks>[skeel] 1/8/2009 created</remarks>
        Shared Function GenerateEditorJavascript(ByVal ModuleID As Integer, ByVal strControl As String) As String
            Dim sb As New StringBuilder

            sb.Append("var pc = navigator.userAgent.toLowerCase(); ")
            sb.Append("var ver = parseInt(navigator.appVersion); ")
            sb.Append("var ie = ((pc.indexOf('msie') != -1) && (pc.indexOf('opera') == -1)); ")
            sb.Append("var bh; ")
            sb.Append("window.onload = editorinit; ")
            sb.Append("function editorinit() { ")
            sb.Append("var doc; ")
            sb.Append("if (document.forms['Form']) { doc = document; } ")
            sb.Append("else { doc = opener.document; } ")
            sb.Append("var txt = doc.forms['Form'].elements['dnn$ctr" & CStr(ModuleID) & "$" & strControl & "$teContent$txtDesktopHTML']; ")
            sb.Append("if (ie && typeof(bh) != 'number') ")
            sb.Append("{ txt.focus(); bh = doc.selection.createRange().duplicate().boundingHeight; document.body.focus(); } } ")

            sb.Append("function storepos(txt) { if (txt.createTextRange) { txt.caretPos = document.selection.createRange().duplicate(); } } ")

            sb.Append("function mozsel(txt, op, cl) { ")
            sb.Append("var sl = txt.textLength; ")
            sb.Append("var ss = txt.selectionStart; ")
            sb.Append("var se = txt.selectionEnd; ")
            sb.Append("var st = txt.scrollTop; ")
            sb.Append("if (se == 1 || se == 2) { se = sl; } ")
            sb.Append("var s1 = (txt.value).substring(0,ss); ")
            sb.Append("var s2 = (txt.value).substring(ss, se); ")
            sb.Append("var s3 = (txt.value).substring(se, sl); ")
            sb.Append("txt.value = s1 + op + s2 + cl + s3; ")
            sb.Append("txt.selectionStart = se + op.length + cl.length; ")
            sb.Append("txt.selectionEnd = txt.selectionStart; ")
            sb.Append("txt.focus(); ")
            sb.Append("txt.scrollTop = st; ")
            sb.Append("return; } ")

            sb.Append("function AddText(code) { ")
            sb.Append("var txt; txt = document.forms['Form'].elements['dnn$ctr" & CStr(ModuleID) & "$" & strControl & "$teContent$txtDesktopHTML']; ")
            sb.Append("if (!isNaN(txt.selectionStart)) { ")
            sb.Append("var ss = txt.selectionStart; ")
            sb.Append("var se = txt.selectionEnd; ")
            sb.Append("mozsel(txt, code, ''); ")
            sb.Append("txt.selectionStart = ss + code.length; ")
            sb.Append("txt.selectionEnd = se + code.length; } ")
            sb.Append("else if (txt.createTextRange && txt.caretPos) { ")
            sb.Append("if (bh != txt.caretPos.boundingHeight) { txt.focus(); storepos(txt); } ")
            sb.Append("var pos = txt.caretPos; ")
            sb.Append("pos.text = pos.text.charAt(pos.text.length - 1) == ' ' ? pos.text + code + ' ' : pos.text + code; } ")
            sb.Append("else { txt.value = txt.value + code; } } ")

            Return sb.ToString
        End Function

#End Region

    End Class

End Namespace
