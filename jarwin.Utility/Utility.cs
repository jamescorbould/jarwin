using jarwin.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace jarwin.Utility
{
    public class Utility
    {
        public string GetAppSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch
            {
                return String.Empty;
            }
        }

        public static bool ProcessRssFeed(string inputUri)
        {
            Feed feed = new Feed();
            List<FeedItem> feedItems = new List<FeedItem>();
            string objType = String.Empty;
            FeedItem feedItem = null;

            using (XmlReader reader = XmlReader.Create(inputUri))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "channel":  // This corresponds to Feed object - should only be one per Rss XML.
                                objType = "FEED";
                                break;
                            case "item":
                                objType = "FEEDITEM";
                                feedItem = new FeedItem();
                                feedItems.Add(feedItem);
                                break;
                            case "title":
                                if (objType == "FEED")
                                {
                                    feed.title = reader.ReadContentAsString();
                                }
                                else if (objType == "FEEDITEM")
                                {
                                    feedItem.title = reader.ReadContentAsString();
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

            return true;
        }
    }
}
