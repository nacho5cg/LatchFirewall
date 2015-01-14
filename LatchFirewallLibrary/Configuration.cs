using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace LatchFirewallLibrary
{
    [Serializable]
    [XmlRoot("Configuration", IsNullable = false)]
    public class Configuration
    {
        public string AppId { get; set; }
        public string Secret { get; set; }
        public string AccountId { get; set; }
        public Int32 Timeout { get; set; }

        [XmlArray()]
        public List<FirewallRule> Rules { get; set; }

        public static readonly string ConfigFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Config.xml");

        private static Configuration configInstance = null;

        public static Configuration GetConfig()
        {
            if (configInstance == null)
                InitializeConfigurationFromFile(ConfigFile);

            if (configInstance == null)
            {
                configInstance = new Configuration();
            }
            if (configInstance.Rules == null)
            {
                configInstance.Rules = new List<FirewallRule>();
            }

            return configInstance;
        }

        public static void InvalidateCachedData()
        {
            configInstance = null;
        }

        private static void InitializeConfigurationFromFile(string configurationFile)
        {
            try
            {
                if (!string.IsNullOrEmpty(configurationFile) && File.Exists(configurationFile))
                {
                    using (FileStream stream = new FileStream(configurationFile, FileMode.Open))
                        configInstance = new XmlSerializer(typeof(Configuration)).Deserialize(stream) as Configuration;
                }
            }
            catch (Exception ex)
            {
                UserLog.LogMessage(ex);
            }
        }

        private static void SaveConfigurationToFile(string configurationFile)
        {
            try
            {
                Configuration config = GetConfig();
                using (FileStream stream = new FileStream(configurationFile, FileMode.Create))
                    new XmlSerializer(typeof(Configuration)).Serialize(stream, config);
            }
            catch (Exception ex)
            {      
                UserLog.LogMessage(ex);
            }

        }

        public static void Save()
        {
            SaveConfigurationToFile(ConfigFile);
        }

    }
}
