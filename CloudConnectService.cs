using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace ConsoleSoftpayCloud
{
    public class CloudConnectService : ICloudConnectService
    {
        IConfigService Config;

        public Uri? BaseURL { get; private set; }

        private string? nexouri;
        public Uri? NexoUri { get; private set; }

        private string? eventsuri;
        public Uri? EventsUri { get; private set; }
        public AccessToken AccessTokenObject { get; set; }
        private string merchantReference = string.Empty;
        public string MerchantReference { get { return merchantReference; } set { merchantReference = value; } }
        public CloudConnectService(IConfigService cfg)
        {
            Config = cfg;
            if (Config != null)
            {
                string? baseurl = Config?.GetValue(ConfigService.AppSettings.BaseURL);
                if (baseurl != null) { BaseURL = new Uri(baseurl); }
            }
            AccessTokenObject = new AccessToken(Config);
            merchantReference = Config.GetValue(ConfigService.AppSettings.MerchantReference);
        }
        public bool IsAccessTokenValid
        {
            get { return AccessTokenObject.Token != string.Empty && !AccessTokenObject.IsExpired; }
        }

        public bool RenewNow
        {
            get { return true; }
        }
        public async Task<bool> RetrieveAccessToken()
        {
            string tokenUrl = Config.GetValue(ConfigService.AppSettings.GetTokenUrl);


            var clientId = Config.GetValue(ConfigService.AppSettings.ClientId);
            var clientSecret = Config.GetValue(ConfigService.AppSettings.Secret);

            // Base64 encode "client_id:client_secret"
            var credentials = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")
            );

            using var httpClient = new HttpClient();

            // Authorization: Basic <base64>
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", credentials);

            // Form data: grant_type=client_credentials
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
            });

            var response = await httpClient.PostAsync(tokenUrl, content);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();


            AccessTokenObject.UpdateAccessToken(responseString);

            return IsAccessTokenValid;
        }
    }
}
