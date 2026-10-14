using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Configuration;
using System.Reflection;
using System.Reflection.Emit;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;


namespace ConsoleSoftpayCloud
{
    public class ConfigService : IConfigService
    {

        public enum AppSettings { Secret, ClientId, AccessToken, ExpiresOn, RenewAt, Scope, GetTokenUrl, ChannelID, MerchantReference, BaseURL,
            MerchantUri, NexoUri, EventsUri }
        private ExeConfigurationFileMap? configFileMap;
        private ConfigService() { }

        private static ConfigService? instance = null;
        //public static IConfigService Create() { if (instance == null) { instance = new ConfigService(); } return instance; }
        public static IConfigService Create(string configPath="") 
        { 
            if (instance == null) { 
                if (string.IsNullOrEmpty(configPath))
                {
                    configPath = Path.Combine(AppContext.BaseDirectory, "ConsoleSoftpayCloud.dll.config");
                }
                else {
                    configPath = Path.Combine(configPath, "ConsoleSoftpayCloud.dll.config");
                }
                ExeConfigurationFileMap _configFileMap = new ExeConfigurationFileMap() { ExeConfigFilename = configPath };
              
                instance = new ConfigService();
                instance.configFileMap = _configFileMap; 
            } 
            return instance; 
        }

        public bool StoreValue(AppSettings name, string value)
        {
            string value2Store = value;

            if (new AppSettings[] { 
                AppSettings.Secret, 
                AppSettings.ClientId, 
                AppSettings.AccessToken,
                AppSettings.MerchantReference }
                .Contains(name))
            {
                byte[] secretBytes = Encoding.UTF8.GetBytes(value);
                byte[] encryptedSecret = ProtectedData.Protect(secretBytes, null, DataProtectionScope.CurrentUser);
                value2Store = Convert.ToBase64String(encryptedSecret);
            }
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            config.AppSettings.Settings[name.ToString()].Value = value2Store;
            config.Save();
            return true;
        }
        public string GetValue(AppSettings name)
        {
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            byte[]? decryptedSecret = null;

            string value = config.AppSettings.Settings[name.ToString()].Value;

            if (new AppSettings[] { 
                AppSettings.Secret, 
                AppSettings.ClientId, 
                AppSettings.AccessToken,
                AppSettings.MerchantReference }
                .Contains(name))
            {
                if (value != string.Empty)
                {
                    byte[]? encryptedSecret = Convert.FromBase64String(value);
                    if (encryptedSecret != null && encryptedSecret.Length != 0)
                    {
                        decryptedSecret = ProtectedData.Unprotect(encryptedSecret, null, DataProtectionScope.CurrentUser);
                        value = Encoding.UTF8.GetString(decryptedSecret);
                    }
                }
            }
            return value ?? string.Empty;
        }
    }
}
