using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            catch (ConfigurationErrorsException ex)
            {
                return String.Empty;
            }
        }
    }
}
