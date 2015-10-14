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
        }
    }
}
