using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleSoftpayCloud
{
    public class AccessToken
    {
   
        public AccessToken(IConfigService config) 
        { 
            Config = config; 
//            Token = Config.GetValue(ConfigService.AppSettings.AccessToken).ToString();
//            if (Config.GetValue(ConfigService.AppSettings.ExpiresOn).ToString() != string.Empty)
//            {
//                ExpiresOn = DateTime.Parse(Config.GetValue(ConfigService.AppSettings.ExpiresOn).ToString());
//                RenewAt = DateTime.Parse(Config.GetValue(ConfigService.AppSettings.RenewAt).ToString());
//            }
        }
        public string Token { get; protected set; } = string.Empty;
        public DateTime ExpiresOn { get; protected set; }
        public DateTime RenewAt { get; protected set; }
        public bool IsExpired =>  DateTime.Now > ExpiresOn;
        public bool RenewNow => DateTime.Now > RenewAt;

        private readonly IConfigService Config;

        public void UpdateAccessToken(string jsonResponse)
        {
            JsonDocument j = JsonDocument.Parse(jsonResponse);
            if (j != null)
            {
                if (j.RootElement.TryGetProperty("access_token", out JsonElement token)) {
                    Token = token.ToString();
                    j.RootElement.TryGetProperty("expires_in", out JsonElement exp);

                    double iexp = double.Parse(exp.ToString());
                    DateTime orgDateTime = DateTime.Now;
                    ExpiresOn = orgDateTime.AddSeconds(iexp).ToLocalTime();
                    RenewAt = orgDateTime.AddSeconds(iexp - 600d).ToLocalTime();
/*                    Config.StoreValue(ConfigService.AppSettings.AccessToken, Token);
                    Config.StoreValue(ConfigService.AppSettings.ExpiresOn, ExpiresOn.ToString());
                    Config.StoreValue(ConfigService.AppSettings.RenewAt, RenewAt.ToString());
*/                }
            }
        }
    }
}
