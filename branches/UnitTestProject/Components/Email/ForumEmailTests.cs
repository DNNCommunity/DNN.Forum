using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetNuke.Modules.Forum;

namespace UnitTestProject
{
    [TestClass]
    public class ForumEmailTests
    {
        /// <summary>
        /// To verify issue https://github.com/juvander/DotNetNuke-Forum/issues/1
        /// </summary>
        [TestMethod]
        public void GenerateSubjectTest()
        {
            ForumEmail mail = new ForumEmail();
            string actual = mail.GenerateSubject("Re: &quot;http://", new System.Collections.Hashtable(), ForumContentTypeID.POST);
            string expected = "Re: \"http://";
            Assert.AreEqual(expected, actual);
        }
    }
}
