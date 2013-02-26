namespace TsSoft.WebSamples.Helpers
{
    using System;
    using System.Configuration;
    using System.Web.Configuration;

    public class EncryptHelper
    {
        private Configuration config;
        private string pathWebConfig;

        public EncryptHelper(string pathWebConfig = "~/")
        {
            this.pathWebConfig = pathWebConfig;
            config = WebConfigurationManager.OpenWebConfiguration(pathWebConfig);
        }

        public string PathWebConfig
        {
            get
            {
                return pathWebConfig;
            }
            set
            {
                pathWebConfig = value;
                config = WebConfigurationManager.OpenWebConfiguration(pathWebConfig);
            }
        }

        public void Encrypt(string encryptSection, string providerName = "RsaProtectedConfigurationProvider")
        {
            var section = GetSection(encryptSection);
            if (section.SectionInformation.IsProtected == true)
            {
                throw new Exception("Section " + encryptSection + " already encrypted.");
            }
            section.SectionInformation.ProtectSection(providerName);
            config.Save();
        }

        public void Decrypt(string decryptSection)
        {
            var section = GetSection(decryptSection);
            if (section.SectionInformation.IsProtected == false)
            {
                throw new Exception("Section " + decryptSection + " already decrypted.");
            }
            section.SectionInformation.UnprotectSection();
            config.Save();
        }

        private ConfigurationSection GetSection(string sectionName)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                throw new Exception("Section name is null");
            }
            var section = config.GetSection(sectionName);
            if (section == null)
            {
                throw new Exception("Section " + sectionName + " is not defined. Check your Web.config");
            }
            if (section.ElementInformation.IsLocked)
            {
                throw new Exception("Section " + sectionName + " is locked.");
            }
            return section;
        }
    }
}