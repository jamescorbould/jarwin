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
        public static feed CreateFeed()
        {
            feed feed = new feed();

            feed.last_download_datetime = DateTime.Now;
            feed.last_build_datetime = DateTime.Now;
            feed.update_period = "hourly";
            feed.update_frequency = 1;
            feed.status = "active";

            return feed;
        }

        public static feed_item CreateFeedItem()
        {
            feed_item feedItem = new feed_item();

            feedItem.last_download_datetime = DateTime.Now;

            return feedItem;
        }

        public static feed_history CreateFeedHistoryFromFeed(feed feed)
        {
            feed_history feedHistory = new feed_history();

            feedHistory.description = feed.description;
            feedHistory.feed_id = feed.feed_id;
            feedHistory.language = feed.language;
            feedHistory.last_build_datetime = feed.last_build_datetime;
            feedHistory.last_download_datetime = feed.last_download_datetime;
            feedHistory.site_uri = feed.site_uri;
            feedHistory.status = feed.status;
            feedHistory.title = feed.title;
            feedHistory.type = feed.type;
            feedHistory.update_frequency = feed.update_frequency;
            feedHistory.update_period = feed.update_period;
            feedHistory.feed_uri = feed.feed_uri;
            feedHistory.archived_datetime = DateTime.Now;

            return feedHistory;
        }

        public static feed_item_history CreateFeedItemHistoryFromFeedItem(feed_item feedItem)
        {
            feed_item_history feedItemHistory = new feed_item_history();

            feedItemHistory.comments = feedItem.comments;
            feedItemHistory.content = feedItem.content;
            feedItemHistory.creator = feedItem.creator;
            feedItemHistory.description = feedItem.description;
            feedItemHistory.feed_id = feedItem.feed_id;
            feedItemHistory.feed_item_id = feedItem.feed_item_id;
            feedItemHistory.item_uri = feedItem.item_uri;
            feedItemHistory.last_download_datetime = feedItem.last_download_datetime;
            feedItemHistory.published_datetime = feedItem.published_datetime;
            feedItemHistory.title = feedItem.title;
            feedItemHistory.archived_datetime = DateTime.Now;

            return feedItemHistory;
        }
    }
}
