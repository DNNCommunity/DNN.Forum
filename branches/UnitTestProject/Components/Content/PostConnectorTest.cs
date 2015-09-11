using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace UnitTestProject.Components.Content
{
    [TestClass]
    public class PostConnectorTest
    {
        [TestMethod]
        public void TestReplaceUrls()
        {
            Regex regExp = new Regex(@"<a[^>]+>[^>]*<\/a>");
            string body = "<td ><p>jep&nbsp;https://www.juvander.fi/site/ilpo/juvander.aspx</p>" +
                        "<p><a href=\"https://www.juvander.fi/site/ilpo/juvander.aspx\" target=\"_blank\" rel=\"nofollow\">https://www.juvander.fi/site/ilpsadfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfasdfo/juvander.aspx</a></p></div></td>";
            string ret = regExp.Replace(body, new MatchEvaluator(DotNetNuke.Modules.Forum.PostConnector.ReplaceUrls));
            string fixedBody = "<td ><p>jep&nbsp;https://www.juvander.fi/site/ilpo/juvander.aspx</p>" +
                       "<p><a href=\"https://www.juvander.fi/site/ilpo/juvander.aspx\" target=\"_blank\" rel=\"nofollow\">https://www.juvander.fi/site/ilpsadfa...o/juvander.aspx</a></p></div></td>";

            Assert.AreEqual(fixedBody, ret);

            body = "<td ><p>jep&nbsp;https://www.juvander.fi/site/ilpo/juvander.aspx</p>" +
                         "<p><a href=\"https://www.juvander.fi/site/ilpo/juvander.aspx\" target=\"_blank\" rel=\"nofollow\">testiä vaan</a></p></div></td>";
            ret = regExp.Replace(body, new MatchEvaluator(DotNetNuke.Modules.Forum.PostConnector.ReplaceUrls));

            Assert.AreEqual(body, ret);
        }
    }
}
