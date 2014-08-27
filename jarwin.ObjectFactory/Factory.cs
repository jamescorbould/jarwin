using jarwin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.ObjectFactory
{
    public class Factory
    {
        public static Feed CreateFeed()
        {
            Feed feed = new Feed();

            feed.lastDownloadDateTime = DateTime.Now;
            feed.lastBuildDateTime = DateTime.Now;
            feed.updatePeriod = "hourly";
            feed.updateFrequency = 1;
            feed.status = "active";

            return feed;
        }

        public static FeedItem CreateFeedItem()
        {
            FeedItem feedItem = new FeedItem();

            feedItem.lastDownloadDateTime = DateTime.Now;

            return feedItem;
        }

        public static FeedHistory CreateFeedHistoryFromFeed(Feed feed)
        {
            FeedHistory feedHistory = new FeedHistory();

            feedHistory.description = feed.description;
            feedHistory.feedID = feed.feedID;
            feedHistory.folderID = feed.folderID;
            feedHistory.language = feed.language;
            feedHistory.lastBuildDateTime = feed.lastBuildDateTime;
            feedHistory.lastDownloadDateTime = feed.lastDownloadDateTime;
            feedHistory.siteURI = feed.siteURI;
            feedHistory.status = feed.status;
            feedHistory.title = feed.title;
            feedHistory.type = feed.type;
            feedHistory.updateFrequency = feed.updateFrequency;
            feedHistory.updatePeriod = feed.updatePeriod;
            feedHistory.feedURI = feed.feedURI;

            return feedHistory;
        }

        public static FeedItemHistory CreateFeedItemHistoryFromFeedItem(FeedItem feedItem)
        {
            FeedItemHistory feedItemHistory = new FeedItemHistory();

            feedItemHistory.comments = feedItem.comments;
            feedItemHistory.content = feedItem.content;
            feedItemHistory.creator = feedItem.creator;
            feedItemHistory.description = feedItem.description;
            feedItemHistory.feedID = feedItem.feedID;
            feedItemHistory.feedItemID = feedItem.feedItemID;
            feedItemHistory.itemURI = feedItem.itemURI;
            feedItemHistory.lastDownloadDateTime = feedItem.lastDownloadDateTime;
            feedItemHistory.publishedDateTime = feedItem.publishedDateTime;
            feedItemHistory.title = feedItem.title;
            feedItemHistory.archivedDateTime = DateTime.Now;

            return feedItemHistory;
        }
    }
}
