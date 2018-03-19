using System;
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

        public static string HearthstoneUsername()
        {
            return ConfigurationManager.AppSettings["HearthstoneUsername"];
        }

        public static bool ShowHomePage()
        {
            return Boolean.Parse(ConfigurationManager.AppSettings["ShowHomePage"]);
        }

        public static void ToggleShowHomePage()
        {
            if (ShowHomePage())
            {
                ConfigurationManager.AppSettings.Set("ShowHomePage", "false");
            }
            else
            {
                ConfigurationManager.AppSettings.Set("ShowHomePage", "true");
            }
        }

        public static string CardImagesFolder()
        {
            return ConfigurationManager.AppSettings["CardImagesFolder"];
        }

        public static string SavedDataFolder()
        {
            return ConfigurationManager.AppSettings["SavedDataFolder"];
        }

        public static string SavedDecksFile()
        {
            return ConfigurationManager.AppSettings["SavedDecksFile"];
        }

        public static string EntityXmlFile()
        {
            return ConfigurationManager.AppSettings["EntityXMLFile"];
        }

        public static string RecordedGamesXmlFile()
        {
            return ConfigurationManager.AppSettings["RecordedGamesXMLFile"];
        }

        public static string DefaultSelectedHero()
        {
            return ConfigurationManager.AppSettings["DefaultSelectedHero"];
        }

        public static void SetDefaultSelectedHero(string cardId)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                settings["DefaultSelectedHero"].Value = cardId;

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);

                Log.Info($"Successfully set '{cardId}' as default hero selection.");
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}
