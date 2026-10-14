using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Web;

namespace ConsoleSoftpayCloud
{
    public class SoftpayImplementation
    {
        IConfigService configService;
        ICloudConnectService cloudConnectService;

        internal SoftpayImplementation(IConfigService cfg, ICloudConnectService connectService)
        {
            configService = cfg;
            cloudConnectService = connectService;
        }
        public bool AddSecrets(string id, string secret)
        {
            configService?.StoreValue(ConfigService.AppSettings.ClientId, id);
            configService?.StoreValue(ConfigService.AppSettings.Secret, secret);
            return true;
        }
        public bool HasSecret()
        {
            return (configService?.GetValue(ConfigService.AppSettings.ClientId) != string.Empty &&
                configService?.GetValue(ConfigService.AppSettings.Secret) != string.Empty);
        }
        public bool HasAccessToken()
        {
            return (cloudConnectService?.IsAccessTokenValid ?? false);
        }
        public async Task<bool> RetrieveAccessToken()
        {
            return await cloudConnectService?.RetrieveAccessToken();
        }
        public async Task<bool> GetMerchantsAsync()
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = "0";
            query["size"] = "20";

            SoftpayRequest request = new SoftpayRequest.SoftpayRequestBuilder()
                .SetBaseUrl(configService?.GetValue(ConfigService.AppSettings.BaseURL))
                .SetUri(configService?.GetValue(ConfigService.AppSettings.MerchantUri))
                .SetQuery(query)
                .SetAccessToken(cloudConnectService.AccessTokenObject.Token)
                .Build();

            var response = await request.SendAsync();
            string responsestring = await response.Content.ReadAsStringAsync();
            JsonDocument jdoc = JsonDocument.Parse(responsestring);
            var options = new JsonSerializerOptions() { WriteIndented = true };

            Console.WriteLine(JsonSerializer.Serialize(jdoc.RootElement, options));


            /*            string x = ;
                        var builder = new UriBuilder(configService?.GetValue(ConfigService.AppSettings.BaseURL));
                        builder.Path = configService?.GetValue(ConfigService.AppSettings.MerchantUri);

                        NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
                        query["page"] = "0";
                        query["size"] = "20";
                        builder.Query = query.ToString();

                        Uri uri = builder.Uri;

            /            Object[] parms = new Object[] { cloudConnectService, uri, "* /*" };

                        IPoster? cp = Activator.CreateInstance(typeof(CloudPoster), parms) as IPoster;
                        if (cp != null)
                        {
                            var responsestring = await cp.PostAsync("");

                            JsonDocument jdoc = JsonDocument.Parse(responsestring);
                            var options = new JsonSerializerOptions() { WriteIndented = true };

                            Console.WriteLine(JsonSerializer.Serialize(jdoc.RootElement,options));

                        } 
            */
            return true;
        }
    }
}