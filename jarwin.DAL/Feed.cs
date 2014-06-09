using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.DAL
{
    [Table]
    private class Feed
    {
        [Column(Name="feed_id", IsPrimaryKey=true)] private int feedID { get; set; }
        [Column(Name="feed_uri")] private string feedURI { get; set; }
        [Column(Name="title")] private string title { get; set; }
        [Column(Name="description")] private string description { get; set; }
        [Column(Name="last_build_datetime")] private DateTime lastBuildDateTime { get; set; }
        [Column(Name="last_download_datetime")] private DateTime lastDownloadDateTime { get; set; } 
        [Column(Name="language")] private string language { get; set; }
        [Column(Name="frequency_id")] private int frequencyID { get; set; }
        [Column(Name="site_uri")] private string siteURI { get; set; }
        [Column(Name="status")] private string status { get; set; }
        [Column(Name="type")] private string type { get; set; }
        [Column(Name = "folder_id")] private int folderID { get; set; }
    }
}
