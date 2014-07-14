using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jarwin.ObjectFactory;
using jarwin.DAL;
using System.Linq;

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

        [TestMethod]
        public void CreateRssFromUriFileSystemStephenson()
        {
            string inputUri = @"..\..\..\SolutionArtefacts\stephenson_blog_rss.xml";
            Rss rss = new Rss(inputUri);

            Assert.IsTrue(rss != null);
            Assert.IsTrue(rss.feed.feedURI == null);  // Not provided in the file, hence cannot be assigned.
        }

        [TestMethod]
        public void CreateRssFromWebUriStephenson()
        {
            string inputUri = @"http://geekswithblogs.net/michaelstephenson/Rss.aspx";
            Rss rss = new Rss(inputUri);

            Assert.IsTrue(rss != null);
            Assert.IsTrue(rss.feed.feedURI == null);  // Not provided in the file, hence cannot be assigned.
        }

        [TestMethod]
        public void CreateAndDeleteRssFromUriFileSystem()
        {
            // Create and delete Rss in succession.  Any db locking issues?

            string inputUri = @"..\..\..\SolutionArtefacts\jc_blog_rss.xml";
            Rss rss = new Rss(inputUri);

            Assert.IsTrue(rss != null);

            Utility.Utility utility = new Utility.Utility();
            JarwinDataContext dataContext = new JarwinDataContext(utility.GetAppSetting("connectionString2"));

            var feedHdr =
                from feed in dataContext.Feed
                where feed.feedID == rss.feed.feedID
                select feed;

            Assert.IsTrue(feedHdr != null);



        }
    }
}
