namespace TsSoft.WebSamples.ConfigSections
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// 
    /// </summary>
    public class SocialConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Settings")]
        private SocialSettingsCollection Settings
        {
            get { return ((SocialSettingsCollection)(base["Settings"])); }
        }

        public string GetValue(string key)
        {
            var settings = Settings.Cast<SocialConfigElement>();
            if(!settings.Any(x => x.Key == key))
            {
                throw new Exception("Key " + key + " does not exist");
            }
            return settings.Single(x => x.Key == key).Value;
        }
    }
}