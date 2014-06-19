﻿using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.DAL
{
    [Table]
    public class Feed
    {
        [Column(Name="feed_id", IsPrimaryKey=true)] public int feedID { get; set; }
        [Column(Name="feed_uri")] public string feedURI { get; set; }
        [Column(Name="title")] public string title { get; set; }
        [Column(Name="description")] public string description { get; set; }
        [Column(Name="last_build_datetime")] public DateTime lastBuildDateTime { get; set; }
        [Column(Name="last_download_datetime")] public DateTime lastDownloadDateTime { get; set; } 
        [Column(Name="language")] public string language { get; set; }
        [Column(Name="frequency_id")] public int? frequencyID { get; set; }
        [Column(Name="site_uri")] public string siteURI { get; set; }
        [Column(Name="status")] public string status { get; set; }
        [Column(Name="type")] public string type { get; set; }
        [Column(Name = "folder_id")] public int? folderID { get; set; }
    }
}
