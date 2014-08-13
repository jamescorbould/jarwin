using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarwin.DAL;
using System.Xml;
using System.Net;

namespace jarwin.ObjectFactory
{
    public class Rss
    {
        public Feed feed { get; set; }
        public List<FeedItem> feedItems { get; set; }

        public Rss()
        {
            feed = new Feed();
            feedItems = new List<FeedItem>();
        }

        public Rss(Feed feedIn, List<FeedItem> feedItemsIn)
        {
            feed = feedIn;
            feedItems = feedItemsIn;
        }

        public Rss(string inputUri)
        {
            CreateTypes(inputUri);
        }

        public void CreateTypes(string inputUri)
        {
            feed = Factory.CreateFeed();
            feedItems = new List<FeedItem>();
            string currentObjType = String.Empty;
            FeedItem feedItem = null;

            if (inputUri.Contains("http://") || inputUri.Contains("https://"))
            {
                feed.feedURI = inputUri;
            }

            using (XmlReader reader = XmlReader.Create(inputUri))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        // JRC 14/07/2014:  TODO: rewrite this logic using a state machine...
                        switch (reader.Name.ToLower())
                        {
                            case "channel":  // This corresponds to a Feed object - should only be one per Rss XML.
                                currentObjType = "FEED";
                                break;
                            case "item":  // This corresponds to a FeedItem object - 0..n per Rss XML.
                                currentObjType = "FEEDITEM";
                                feedItem = Factory.CreateFeedItem();
                                feedItems.Add(feedItem);
                                break;
                            case "title":
                                if (currentObjType == "FEED")
                                {
                                    feed.title = reader.ReadElementContentAsString();
                                }
                                else if (currentObjType == "FEEDITEM")
                                {
                                    feedItem.title = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "link":
                                if (currentObjType == "FEED")
                                {
                                    feed.siteURI = reader.ReadElementContentAsString();
                                }
                                else if (currentObjType == "FEEDITEM")
                                {
                                    feedItem.itemURI = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "atom:link":
                                if (currentObjType == "FEED")
                                {
                                    string linkType = String.Empty;

                                    try
                                    {
                                        linkType = reader.GetAttribute("type").ToLower();
                                    }
                                    catch (NullReferenceException)
                                    {
                                        linkType = String.Empty;
                                    }

                                    if (linkType == "application/rss+xml")
                                    {
                                        feed.feedURI = reader.GetAttribute("href");
                                    }
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "atom10:link":
                                if (currentObjType == "FEED")
                                {
                                    string linkType = String.Empty;

                                    try
                                    {
                                        linkType = reader.GetAttribute("type").ToLower();
                                    }
                                    catch (NullReferenceException)
                                    {
                                        linkType = String.Empty;
                                    }

                                    if (linkType == "application/rss+xml")
                                    {
                                        feed.feedURI = reader.GetAttribute("href");
                                    }
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "description":
                                if (currentObjType == "FEED")
                                {
                                    feed.description = reader.ReadElementContentAsString();
                                }
                                else if (currentObjType == "FEEDITEM")
                                {
                                    feedItem.description = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "lastbuilddate":
                                if (currentObjType == "FEED")
                                {
                                    string pubDate = reader.ReadElementContentAsString();
                                    feed.lastBuildDateTime = DateTime.Parse(pubDate);
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "language":
                                if (currentObjType == "FEED")
                                {
                                    feed.language = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "sy:updateperiod":
                                if (currentObjType == "FEED")
                                {
                                    feed.updatePeriod = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "sy:updatefrequency":
                                if (currentObjType == "FEED")
                                {
                                    feed.updateFrequency = reader.ReadElementContentAsInt();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "comments":
                                if (currentObjType == "FEEDITEM")
                                {
                                    feedItem.comments = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "pubdate":
                                if (currentObjType == "FEEDITEM")
                                {
                                    string pubDate = reader.ReadElementContentAsString();
                                    feedItem.publishedDateTime = DateTime.Parse(pubDate);
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "content:encoded":
                                if (currentObjType == "FEEDITEM")
                                {
                                    feedItem.content = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            default:
                                reader.Skip();
                                break;
                        }
                    }
                }
            }
        }
        
        public void Delete(int feedID, JarwinDataContext dataContext)
        {
            // Destroy thyself :-(

            var deleteFeedItems =
                from feedItem in dataContext.FeedItem
                where feedItem.feedID == feedID
                select feedItem;

            foreach (var feedItem in deleteFeedItems)
            {
                dataContext.FeedItem.DeleteOnSubmit(feedItem);
            }

            try
            {
                dataContext.SubmitChanges();
            }
            catch (Exception)
            {
                // TODO: need to log.
                throw;
            }

            var deleteFeed =
                from feed in dataContext.Feed
                where feed.feedID == feedID
                select feed;

            dataContext.Feed.DeleteOnSubmit(deleteFeed.First<Feed>());

            try
            {
                dataContext.SubmitChanges();
            }
            catch
            {
                // TODO: need to log.
                throw;
            }
        }

        public void Update(int feedID, JarwinDataContext dataContext)
        {
            // Update thyself.

            // Check if can access feed URL first (i.e. are we online?).
            
            // TODO: implement error logging to specified log.

            using(WebClient webClient = new WebClient())
            {
                try
                {
                    webClient.BaseAddress = "http://localhost";
                    webClient.OpenRead("http://www.google.co.nz");
                }
                catch
                {
                    // Not online for whatever reason.
                    // TODO: Need to log reason.
                    throw;
                }
            }

            // We are online so we can sync.
            // Copy existing Feed and FeedItems to a history table first.

            var deleteFeedItems =
                from feedItem in dataContext.FeedItem
                where feedItem.feedID == feedID
                select feedItem;

            foreach (var feedItem in deleteFeedItems)
            {
                dataContext.FeedItemHistory.InsertOnSubmit(Factory.CreateFeedItemHistoryFromFeedItem(feedItem));
            }

            try
            {
                dataContext.SubmitChanges();
            }
            catch (Exception)
            {
                // TODO: need to log.
                throw;
            }

            foreach (var feedItem in deleteFeedItems)
            {
                dataContext.FeedItem.DeleteOnSubmit(feedItem);
            }

            try
            {
                dataContext.SubmitChanges();
            }
            catch (Exception)
            {
                // TODO: need to log.
                throw;
            }

            var updateFeed =
                from feed in dataContext.Feed
                where feed.feedID == feedID
                select feed;

            dataContext.FeedHistory.InsertOnSubmit(Factory.CreateFeedHistoryFromFeed(updateFeed.First<Feed>()));

            try
            {
                dataContext.SubmitChanges();
            }
            catch
            {
                // TODO: need to log.
                throw;
            }
            
            // Feed and FeedItem now backed up in history tables.
            // Update Feed and insert latest FeedItem records.

            // Create local Feed and FeedItem types using the provided URL to the Rss feed.
            CreateTypes(updateFeed.First<Feed>().feedURI);

            // Update Feed.

            // Update Feed in the local data context with the new locally downloaded Feed details.
            updateFeed.First<Feed>().description = this.feed.description;
            updateFeed.First<Feed>().language = this.feed.language;
            updateFeed.First<Feed>().lastBuildDateTime = this.feed.lastBuildDateTime;
            updateFeed.First<Feed>().lastDownloadDateTime = DateTime.Now;
            updateFeed.First<Feed>().title = this.feed.title;
            updateFeed.First<Feed>().type = this.feed.type;
            updateFeed.First<Feed>().updateFrequency = this.feed.updateFrequency;
            updateFeed.First<Feed>().updatePeriod = this.feed.updatePeriod;

            try
            {
                dataContext.SubmitChanges();
            }
            catch
            {
                // TODO: need to log.
                throw;
            }

            // Insert new FeedItem(s).
            int feedItemID = 0;

            foreach (var feedItem in feedItems)
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
            catch
            {
                // TODO: need to log.
                throw;
            }

            // TODO: log errors and cleanup database on failures (rollback steps).
        }
    }
}
