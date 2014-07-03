﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarwin.DAL;
using System.Xml;
using System.Net;

namespace jarwin.ObjectFactory
{
    public class Rss
    {
        public Feed feed { get; set; }
        public List<FeedItem> feedItems { get; set; }

        public Rss()
        {
            feed = new Feed();
            feedItems = new List<FeedItem>();
        }

        public Rss(Feed feedIn, List<FeedItem> feedItemsIn)
        {
            feed = feedIn;
            feedItems = feedItemsIn;
        }

        public Rss(string inputUri)
        {
            feed = Factory.CreateFeed();
            feedItems = new List<FeedItem>();
            string currentObjType = String.Empty;
            FeedItem feedItem = null;

            if (inputUri.Contains("http://") || inputUri.Contains("https://"))
            {
                using (WebClient webClient = new WebClient())
                {
                    var textFromFile = webClient.DownloadString(inputUri);
                }
            }

            using (XmlReader reader = XmlReader.Create(inputUri))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
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
                                    feed.siteURI = reader.ReadElementContentAsString();
                                }
                                else if (currentObjType == "FEEDITEM")
                                {
                                    feedItem.itemURI = reader.ReadElementContentAsString();
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
                                        feed.feedURI = reader.GetAttribute("href");
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
                                    feed.lastBuildDateTime = DateTime.Parse(pubDate);
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
                            case "sy:updatePeriod":
                                if (currentObjType == "FEED")
                                {
                                    feed.updatePeriod = reader.ReadElementContentAsString();
                                }
                                else
                                {
                                    reader.Skip();
                                }
                                break;
                            case "sy:updateFrequency":
                                if (currentObjType == "FEED")
                                {
                                    feed.updateFrequency = reader.ReadElementContentAsInt();
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
                                    feedItem.publishedDateTime = DateTime.Parse(pubDate);
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
    }
}
