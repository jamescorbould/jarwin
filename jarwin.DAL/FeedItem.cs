using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.DAL
{
    [Table]
    private class FeedItem
    {
        [Column(Name="feed_id")] private int feedID { get; set; }
        [Column(Name="feed_item_id")] private int feedItemID { get; set; }
        [Column(Name="title")] private string title { get; set; }
        [Column(Name="item_uri")] private string itemURI { get; set; }
        [Column(Name="comments")] private string comments { get; set; }
        [Column(Name="published_datetime")] private DateTime publishedDateTime { get; set; }
        [Column(Name="creator")] private string creator { get; set; }
        [Column(Name="description")] private string description { get; set; }
        [Column(Name="content")] private string content { get; set; }
    }
}
