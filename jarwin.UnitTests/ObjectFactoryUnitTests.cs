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

            dataContext.Feed.InsertOnSubmit(rss.feed);

            try
            {
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                Assert.Fail("Error - failed to insert Feed record: " + ex.Message);
            }

            int feedID = rss.feed.feedID; // Value provided by SQL Server identity column.
            int feedItemID = 0;

            foreach (var feedItem in rss.feedItems)
            {
                feedItem.feedID = feedID;
                feedItem.feedItemID = feedItemID;
                dataContext.FeedItem.InsertOnSubmit(feedItem);

                feedItemID += 1;
            }

            try
            {
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                Assert.Fail("Error - failed to insert FeedItem record: " + ex.Message);
            }

            // Delete Feed and FeedItems.
            bool success = true;

            try
            {
                success = rss.Delete(rss.feed.feedID, dataContext);
            }
            catch (Exception ex)
            {
                Assert.Fail("Error - failed to delete Feed or FeedItem record: " + ex.Message);
            }

            if (!success)
            {
                Assert.Fail("Error - failed to delete Feed or FeedItems record.");
            }
        }
    }
}
