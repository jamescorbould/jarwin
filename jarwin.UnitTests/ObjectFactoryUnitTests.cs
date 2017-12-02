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
            Assert.IsTrue(rss.feed.feed_uri == null);  // Not provided in the file, hence cannot be assigned.
        }

        [TestMethod]
        public void CreateRssFromWebUriStephenson()
        {
            string inputUri = @"http://geekswithblogs.net/michaelstephenson/Rss.aspx";
            Rss rss = new Rss(inputUri);

            Assert.IsTrue(rss != null);
            Assert.IsTrue(rss.feed.feed_uri == null);  // Not provided in the file, hence cannot be assigned.
        }

        [TestMethod]
        public void CreateAndDeleteRssFromUriFileSystem()
        {
            // Create and delete Rss in succession.  Any db locking issues?

            string inputUri = @"..\..\..\SolutionArtefacts\jc_blog_rss.xml";
            Rss rss = new Rss(inputUri);

            Assert.IsTrue(rss != null);

            using (mainEntities context = new mainEntities())
            {

                context.feeds.Add(rss.feed);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Assert.Fail("Error - failed to insert Feed record: " + ex.Message);
                }

                int feedID = (int)rss.feed.feed_id; // Value provided by SQL Server identity column.
                int feedItemID = 0;

                foreach (var feedItem in rss.feedItems)
                {
                    feedItem.feed_id = feedID;
                    feedItem.feed_item_id = feedItemID;
                    context.feed_item.Add(feedItem);

                    feedItemID += 1;
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Assert.Fail("Error - failed to insert FeedItem record: " + ex.Message);
                }

                try
                {
                    rss.Delete((int)rss.feed.feed_id);
                }
                catch (Exception ex)
                {
                    Assert.Fail("Error - failed to delete Feed or FeedItem record: " + ex.Message);
                }
            }
        }

        [TestMethod]
        public async void CreateAndUpdateRssFromUriFileSystem()
        {
            // Create and then update.

            string inputUri = @"..\..\..\SolutionArtefacts\jc_blog_rss.xml";
            Rss rss = new Rss(inputUri);

            Assert.IsTrue(rss != null);

            using (mainEntities context = new mainEntities())
            {
                context.feeds.Add(rss.feed);

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Assert.Fail("Error - failed to insert Feed record: " + ex.Message);
                }

                int feedID = (int)rss.feed.feed_id; // Value provided by identity column.
                int feedItemID = 0;

                foreach (var feedItem in rss.feedItems)
                {
                    feedItem.feed_id = feedID;
                    feedItem.feed_item_id = feedItemID;
                    context.feed_item.Add(feedItem);

                    feedItemID += 1;
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Assert.Fail("Error - failed to insert FeedItem record: " + ex.Message);
                }

                //try
                //{
                //    bool success = await rss.Update((int)rss.feed.feed_id);
                //    Assert.IsTrue(success);
                //}
                //catch (Exception ex)
                //{
                //    Assert.Fail("Error - failed to update Feed or FeedItem record: " + ex.Message);
                //}
            }
        }
    }
}
