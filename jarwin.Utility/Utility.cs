using jarwin.DAL;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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

        public static LoggingConfiguration BuildProgrammaticConfig()
        {
            // Formatter
            TextFormatter briefFormatter = new TextFormatter("Timestamp: {timestamp(local)}{newline}Message: {message}{newline}");

            // Trace Listener
            var flatFileTraceListener = new FlatFileTraceListener(
                @"C:\Temp\jarwin.log", 
                "----------------------------------------", 
                "----------------------------------------", 
                briefFormatter);

            // Build Configuration
            var config = new LoggingConfiguration();

            config.AddLogSource("jarwin", SourceLevels.All, true)
                .AddTraceListener(flatFileTraceListener);

            return config;
        }
    }
}
