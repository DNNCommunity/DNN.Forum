using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetNuke.Forum.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetNuke.Forum.Library.Tests
{
    [TestClass()]
    public class TextUtilityClassTests
    {
        [TestMethod()]
        public void StripHTMLTest()
        {
            Assert.AreEqual("on", TextUtilityClass.StripHTML("<p>on</p>"));
        }
    }
}