using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HearthstoneDeckTracker.Utilities
{
    public static class Config
    {
        public static string HearthstoneLogDirectory()
        {
            return ConfigurationManager.AppSettings["HearthstoneLogDirectory"];
        }

        public static string HearthstoneZoneLogFile()
        {
            return ConfigurationManager.AppSettings["HearthstoneZoneLogFile"];
        }

        public static int LogFileUpdateDelay()
        {
            return int.Parse(ConfigurationManager.AppSettings["LogFileUpdateDelay"]);
        }
    }
}
