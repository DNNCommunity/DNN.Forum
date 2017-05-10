using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetNuke.Forum.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace DotNetNuke.Forum.Library.Tests
{
    [TestClass()]
    public class UrlUtilityClassTests
    {
        [TestMethod()]
        public void UrlShortenerTest()
        {
            string ret = UrlUtilityClass.UrlShortener("<a href=\"http://iadb.azurewebsites.net/safariadblockerforfinland.safariextz\">http://iadb.azurewebsites.net/safariadblockerforfinland.safariextz</a>");

            Assert.AreEqual("<a href=\"http://iadb.azurewebsites.net/safariadblockerforfinland.safariextz\">http://iadb.azurewebsites.net/safaria...land.safariextz</a>", ret);

            Assert.AreEqual("", UrlUtilityClass.UrlShortener(""));
            Assert.AreEqual(" >http://www.juvander.fi< ", UrlUtilityClass.UrlShortener(" >http://www.juvander.fi< "));
            Assert.AreEqual(
                " >https://www.google.fi/maps/dir/31.720...31.680885?hl=fi< ",
                UrlUtilityClass.UrlShortener(" >https://www.google.fi/maps/dir/31.7201983,-5.8026824/camping+addoud+tamtatouchte/@31.6985702,-5.68036,12z/data=!4m8!4m7!1m0!1m5!1m1!1s0x0:0xf03b5e9061906335!2m2!1d-5.534105!2d31.680885?hl=fi< "));
            Assert.AreEqual(
                " https://www.google.fi/maps/dir/31.7201983,-5.8026824/camping+addoud+tamtatouchte/@31.6985702,-5.68036,12z/data=!4m8!4m7!1m0!1m5!1m1!1s0x0:0xf03b5e9061906335!2m2!1d-5.534105!2d31.680885?hl=fi ",
                UrlUtilityClass.UrlShortener(" https://www.google.fi/maps/dir/31.7201983,-5.8026824/camping+addoud+tamtatouchte/@31.6985702,-5.68036,12z/data=!4m8!4m7!1m0!1m5!1m1!1s0x0:0xf03b5e9061906335!2m2!1d-5.534105!2d31.680885?hl=fi "));
        }
    }
}
