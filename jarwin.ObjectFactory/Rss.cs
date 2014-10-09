using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarwin.DAL;
using System.Xml;
using System.Net;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using jarwin.Utility;
using System.IO;
using System.Data.Entity.Core;

namespace jarwin.ObjectFactory
{
    public class Rss
    {
        public feed feed { get; set; }
        public List<feed_item> feedItems { get; set; }
        public LoggingConfiguration loggingConfiguration { get; set; }
        public LogWriter logWriter { get; set; }

        public Rss()
        {
            feed = new feed();
            feedItems = new List<feed_item>();

            loggingConfiguration = Utility.Utility.BuildProgrammaticConfig();
            logWriter = new LogWriter(loggingConfiguration);
        }

        public Rss(feed feedIn, List<feed_item> feedItemsIn)
        {
            feed = feedIn;
            feedItems = feedItemsIn;

            loggingConfiguration = Utility.Utility.BuildProgrammaticConfig();
            logWriter = new LogWriter(loggingConfiguration);
        }

        public Rss(string inputUri)
        {
            CreateTypes(inputUri);

            loggingConfiguration = Utility.Utility.BuildProgrammaticConfig();
            logWriter = new LogWriter(loggingConfiguration);
        }

        public void CreateTypes(string inputUri)
        {
            feed = Factory.CreateFeed();
            feedItems = new List<feed_item>();
            string currentObjType = String.Empty;
            feed_item feedItem = null;

            if (inputUri.Contains("http://") || inputUri.Contains("https://"))
            {
                feed.feed_uri = inputUri;
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
                                    feed.site_uri = reader.ReadElementContentAsString();
                                }
                                else if (currentObjType == "FEEDITEM")
                                {
                                    feedItem.item_uri = reader.ReadElementContentAsString();
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
                                        feed.feed_uri = reader.GetAttribute("href");
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
                                        feed.feed_uri = reader.GetAttribute("href");
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
                                    feed.last_build_datetime = DateTime.Parse(pubDate);
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
                                    feed.update_period = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "sy:updatefrequency":
                                if (currentObjType == "FEED")
                                {
                                    feed.update_frequency = reader.ReadElementContentAsInt();
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
                                    feedItem.published_datetime = DateTime.Parse(pubDate);
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
        
        public void Delete(long feedID)
        {
            // Destroy thyself :-(

            using (jarwinEntities context = new jarwinEntities())
            {
                // Delete feed items associated with this feed:

                var deleteFeedItems =
                    from feedItem in context.feed_item
                    where feedItem.feed_id == feedID
                    select feedItem;

                foreach (var feedItem in deleteFeedItems)
                {
                    context.feed_item.Remove(feedItem);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Failed to from table FeedItem.  Source = Rss.Delete.  Exception type = {0}.  Exception msg = {1}.  feedID = {2}", ex.GetType(), ex.Message, feedID));
                    }

                    throw;
                }

                // Delete feed:

                var deleteFeed =
                    from feed in context.feeds
                    where feed.feed_id == feedID
                    select feed;

                context.feeds.Remove(deleteFeed.First<feed>());

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Failed to from table Feed.  Source = Rss.Delete.  Exception type = {0}.  Exception msg = {1}.  feedID = {2}", ex.GetType(), ex.Message, feedID));
                    }

                    // TODO: check if Feed has been deleted - if not, then need to insert FeedItems back.

                    throw;
                }
            }
        }

        public async Task<bool> Update(int feedID)
        {
            // Try and download latest rss feed and process it.

            Boolean result = new Boolean();
            result = true;
            var buffer = new byte[1];

            if (logWriter.IsLoggingEnabled())
            {
                logWriter.Write(String.Format("START :: Source = Rss.Update.  Update Rss with feedID = {0}", feedID));
            }

            // Check if can access feed URL first (i.e. are we online?).

            using(WebClient webClient = new WebClient())
            {
                try
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Starting call to web client to see if online.  feedID = {0}", feedID));
                    }

                    // The default connection limit is 2 - increase this to 100, since this code will be executed multithreaded.
                    System.Net.ServicePointManager.DefaultConnectionLimit = 100;

                    webClient.BaseAddress = "http://localhost";
                    var task = webClient.OpenReadTaskAsync("http://www.google.co.nz");

                    if (await Task.WhenAny(task, Task.Delay(10000)) == task)
                    {
                        int len = await task.Result.ReadAsync(buffer, 0, buffer.Length);

                        if (len > 0)
                        {
                            if (logWriter.IsLoggingEnabled())
                            {
                                logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Yes online.  feedID = {0}", feedID));
                            }
                        }
                        else
                        {
                            throw new Exception("No data returned.");
                        }
                    }
                    else
                    {
                        // TODO: need to cancel the task.
                        throw new TimeoutException("Timed out waiting to connect.");
                    }
                }
                catch (Exception ex)
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Source = Rss.Update.  Not online.  Exception msg = {0}  feedID = {1}", ex.Message, feedID));
                    }

                    result = false;
                    throw;
                }
            }

            // We are online so we can sync.
            // Copy existing Feed and FeedItems to a history table first.
            // TODO: make this an atomic transaction.

            if (logWriter.IsLoggingEnabled())
            {
                logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Insert into FeedItemHistory.  feedID = {0}", feedID));
            }

            using (jarwinEntities context = new jarwinEntities())
            {
                var deleteFeedItems =
                    from feedItem in context.feed_item
                    where feedItem.feed_id == feedID
                    select feedItem;

                foreach (var feedItem in deleteFeedItems)
                {
                    context.feed_item_history.Add(Factory.CreateFeedItemHistoryFromFeedItem(feedItem));
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Source = Rss.Update.  Failed to insert into FeedItemHistory.  Exception type = {0}.  Exception msg = {1}.  feedID = {2}", ex.GetType(), ex.Message, feedID));
                    }

                    result = false;
                    throw;
                }

                if (logWriter.IsLoggingEnabled())
                {
                    logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Delete from FeedItem.  feedID = {0}", feedID));
                }

                foreach (var feedItem in deleteFeedItems)
                {
                    context.feed_item.Remove(feedItem);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Source = Rss.Update.  Failed to delete from FeedItem.  Exception msg = {0}.  feedID = {1}", ex.Message, feedID));
                    }

                    result = false;
                    throw;
                }

                if (logWriter.IsLoggingEnabled())
                {
                    logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Insert into FeedHistory.  feedID = {0}", feedID));
                }

                var updateFeed =
                    from feed in context.feeds
                    where feed.feed_id == feedID
                    select feed;

                context.feed_history.Add(Factory.CreateFeedHistoryFromFeed(updateFeed.First<feed>()));

                try
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Insert into FeedHistory.  feedID = {0}", feedID));
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Source = Rss.Update.  Failed to insert into from FeedHistory.  Exception msg = {0}.  feedID = {1}", ex.Message, feedID));
                    }

                    result = false;
                    throw;
                }

                // Feed and FeedItem now backed up in history tables.
                // Update Feed and insert latest FeedItem records.

                // Create local Feed and FeedItem types using the provided URL to the Rss feed.
                if (logWriter.IsLoggingEnabled())
                {
                    logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Call CreateTypes to hydrate local types.  feedID = {0}", feedID));
                }

                CreateTypes(updateFeed.First<feed>().feed_uri);

                // Update Feed.
                if (logWriter.IsLoggingEnabled())
                {
                    logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Update Feed.  feedID = {0}", feedID));
                }

                // Update Feed in the local data context with the new locally downloaded Feed details.
                updateFeed.First<feed>().description = this.feed.description;
                updateFeed.First<feed>().language = this.feed.language;
                updateFeed.First<feed>().last_build_datetime = this.feed.last_build_datetime;
                updateFeed.First<feed>().last_download_datetime = DateTime.Now;
                updateFeed.First<feed>().title = this.feed.title;
                updateFeed.First<feed>().type = this.feed.type;
                updateFeed.First<feed>().update_frequency = this.feed.update_frequency;
                updateFeed.First<feed>().update_period = this.feed.update_period;

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Source = Rss.Update.  Failed to update Feed.  Exception msg = {0}.  feedID = {1}", ex.Message, feedID));
                    }

                    result = false;
                    throw;
                }

                // Insert new FeedItem(s).

                if (logWriter.IsLoggingEnabled())
                {
                    logWriter.Write(String.Format("INFO :: Source = Rss.Update.  Insert into FeedItem.  feedID = {0}", feedID));
                }

                int feedItemID = 0;

                foreach (var feedItem in feedItems)
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
                    if (logWriter.IsLoggingEnabled())
                    {
                        logWriter.Write(String.Format("ERROR :: Source = Rss.Update.  Failed to insert into FeedItem.  Exception msg = {0}.  feedID = {1}", ex.Message, feedID));
                    }

                    // TODO: compensate by inserting feedItems back from feedItemHistory table.
                    result = false;
                    throw;
                }
            }

            // TODO: need to make this atomic and rollback on failures i.e. cleanup database.

            if (logWriter.IsLoggingEnabled())
            {
                logWriter.Write(String.Format("END :: Source = Rss.Update.  Update Rss with feedID = {0}", feedID));
            }

            return result;
        }
    }
}
