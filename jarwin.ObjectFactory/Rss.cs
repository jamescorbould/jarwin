using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarwin.DAL;

namespace jarwin.ObjectFactory
{
    public class Rss : Feed, FeedItem
    {
        public Feed feed { get; set; }
        public List<FeedItem>? feedItems { get; set; }

        public Rss()
        {
            feed = new Feed();
            feedItems = new List<FeedItem>();
        }

        public Rss(Feed feedIn, List<FeedItem>? feedItemsIn)
        {
            feed = feedIn;
            feedItems = feedItemsIn;
        }
    }
}
