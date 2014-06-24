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

        public static IEnumerable<XElement> GetXMLElementAtUri(string inputUri, string elementName)
        {
            // Based on: http://stackoverflow.com/questions/2441673/reading-xml-with-xmlreader-in-c-sharp

            using (XmlReader reader = XmlReader.Create(inputUri))
            {
                reader.MoveToContent();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == elementName)
                        {
                            XElement el = XNode.ReadFrom(reader) as XElement;
                            if (el != null)
                            {
                                yield return el;
                            }
                        }
                    }
                }
            }
        }
    }
}
