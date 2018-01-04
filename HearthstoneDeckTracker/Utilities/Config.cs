using System.Configuration;

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

	    public static string LogFolder()
	    {
		    return ConfigurationManager.AppSettings["LogFolder"];
	    }

	}
}
