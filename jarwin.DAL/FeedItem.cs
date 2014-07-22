using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.DAL
{
    [Table(Name="feed_item")]
    public class FeedItem
    {
        [Column(Name="feed_id", IsPrimaryKey=true)] public int feedID { get; set; }
        [Column(Name="feed_item_id", IsPrimaryKey=true)] public int feedItemID { get; set; }
        [Column(Name="title")] public string title { get; set; }
        [Column(Name="item_uri")] public string itemURI { get; set; }
        [Column(Name="comments")] public string comments { get; set; }
        [Column(Name="published_datetime")] public DateTime publishedDateTime { get; set; }
        [Column(Name="creator")] public string creator { get; set; }
        [Column(Name="description")] public string description { get; set; }
        [Column(Name="content")] public string content { get; set; }
        [Column(Name="last_download_datetime")] public DateTime lastDownloadDateTime { get; set; }
    }
}
