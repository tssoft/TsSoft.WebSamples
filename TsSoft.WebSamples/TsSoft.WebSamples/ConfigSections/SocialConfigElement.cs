namespace TsSoft.WebSamples.ConfigSections
{
    using System.Configuration;

    public class SocialConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Key
        {
            get { return ((string)(base["key"])); }
            set { base["key"] = value; }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Value
        {
            get { return ((string)(base["value"])); }
            set { base["value"] = value; }
        }
    }
}