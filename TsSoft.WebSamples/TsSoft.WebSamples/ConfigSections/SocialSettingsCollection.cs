namespace TsSoft.WebSamples.ConfigSections
{
    using System.Configuration;

    [ConfigurationCollection(typeof(SocialConfigElement))]
    public class SocialSettingsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SocialConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SocialConfigElement)(element)).Key;
        }

        public SocialConfigElement this[int idx]
        {
            get { return (SocialConfigElement)BaseGet(idx); }
        }

        public SocialConfigElement this[object key]
        {
            get { return (SocialConfigElement)BaseGet(key); }
        }
    }
}