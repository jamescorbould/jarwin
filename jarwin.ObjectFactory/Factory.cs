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
    }
}
