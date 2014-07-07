using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jarwin.ObjectFactory;

namespace jarwin.UnitTests
{
    [TestClass]
    public class ObjectFactoryUnitTests
    {
        [TestMethod]
        public void CreateRssFromUriFileSystem()
        {
            string inputUri = @"..\..\..\SolutionArtefacts\jc_blog_rss.xml";
            Rss rss = new Rss(inputUri);

            Assert.IsTrue(rss != null);
        }

        [TestMethod]
        public void CreateRssFromUriHttp()
        {
            string inputUri = @"http://adventuresinsidethemessagebox.wordpress.com/feed/";
            Rss rss = new Rss(inputUri);

            Assert.IsTrue(rss != null);
        }
    }
}
