using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Web;

namespace ConsoleSoftpayCloud
{
    public class SoftpayImplementation : ISoftpayImplementation
    {
        IConfigService configService;
        ICloudConnectService cloudConnectService;
        public string BaseUrl { get; set; }
        public string TransactionUri { get; set; }
        private string merchantReference = string.Empty;
        public string MerchantReference
        {
            get { return merchantReference; }
            set
            {
                if (0 != value.CompareTo(merchantReference))
                {
                    configService.StoreValue(ConfigService.AppSettings.MerchantReference, value);
                    merchantReference = value;
                }
            }
        }
        private string storeId = string.Empty;
        public string StoreId
        {
            get { return storeId; }
            set
            {
                if (0 != value.CompareTo(storeId))
                {
                    configService.StoreValue(ConfigService.AppSettings.StoreId, value);
                    storeId = value;
                }
            }
        }
        private string appId = string.Empty;
        public string AppId
        {
            get { return appId; }
            set
            {
                if (0 != value.CompareTo(appId))
                {
                    configService.StoreValue(ConfigService.AppSettings.AppId, value);
                    AppId = value;
                }
            }
        }
        private string asseccToken = string.Empty;
        public string AccessToken
        {
            get { return cloudConnectService.AccessTokenObject.Token; }
            set
            {
                if (0 != value.CompareTo(appId))
                {
                    configService.StoreValue(ConfigService.AppSettings.AppId, value);
                    AppId = value;
                }

            }
        }

        internal SoftpayImplementation(IConfigService cfg, ICloudConnectService connectService)
        {
            configService = cfg;
            cloudConnectService = connectService;
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
        }
        public void Init()
        {
            merchantReference = configService.GetValue(ConfigService.AppSettings.MerchantReference);
            storeId = configService.GetValue(ConfigService.AppSettings.StoreId);
            appId = configService.GetValue(ConfigService.AppSettings.AppId);
            BaseUrl = configService.GetValue(ConfigService.AppSettings.BaseURL);
            TransactionUri = configService.GetValue(ConfigService.AppSettings.TransactionsUri);
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

            return true;
        }
        public async Task<bool> GetStoresAsync()
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = "0";
            query["size"] = "20";

            SoftpayRequest request = new SoftpayRequest.SoftpayRequestBuilder()
                .SetBaseUrl(configService?.GetValue(ConfigService.AppSettings.BaseURL))
                .SetUri("/api-gateway/v3/api/cloud/stores")
                .SetQuery(query)
                .SetAccessToken(cloudConnectService.AccessTokenObject.Token)
                .SetMerchantReference(MerchantReference)
                .Build();

            var response = await request.SendAsync();
            string responsestring = await response.Content.ReadAsStringAsync();
            JsonDocument jdoc = JsonDocument.Parse(responsestring);
            var options = new JsonSerializerOptions() { WriteIndented = true, };

            Console.WriteLine(JsonSerializer.Serialize(jdoc.RootElement, options));


            return true;
        }
        public async Task<bool> GetTerminalsAsync(string storeid)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = "0";
            query["size"] = "20";

            SoftpayRequest request = new SoftpayRequest.SoftpayRequestBuilder()
                .SetBaseUrl(configService?.GetValue(ConfigService.AppSettings.BaseURL))
                .SetUri($"/api-gateway/v3/api/cloud/stores/{storeid}/terminals")
                .SetQuery(query)
                .SetAccessToken(cloudConnectService.AccessTokenObject.Token)
                .SetMerchantReference(MerchantReference)
                .Build();

            var response = await request.SendAsync();
            string responsestring = await response.Content.ReadAsStringAsync();
            try
            {
                JsonDocument jdoc = JsonDocument.Parse(responsestring);
                var options = new JsonSerializerOptions() { WriteIndented = true };
                Console.WriteLine(JsonSerializer.Serialize(jdoc.RootElement, options));
            }
            catch (Exception e)
            {
                ConsoleUI.DisplayFailureMessage(responsestring);
            }

            return true;
        }
        public async Task<bool> MakePayment(int amount)
        {
            string requestId = await CreatePayment();
            if (string.IsNullOrEmpty(requestId))
                return false;

            var builder = new PaymentRequest.PaymentRequestBuilder(this);
            var request = builder.SetRequestId(requestId)
                .SetAmount(amount)
                .SetCurrenceyCode("SEK")
                .Build();

            var response = await request.SendAsync();
            string responsestring = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responsestring);

            return true;
        }
        protected async Task<string> CreatePayment()
        {
            string result = string.Empty;

            var builder = new SoftpayRequest.SoftpayRequestBuilder()
                .SetBaseUrl(configService?.GetValue(ConfigService.AppSettings.BaseURL))
                .SetUri(configService.GetValue(ConfigService.AppSettings.TransactionsUri))
                .SetAccessToken(cloudConnectService.AccessTokenObject.Token)
                .SetMethod(HttpMethod.Post)
                .SetMerchantReference(MerchantReference);

            var json = JsonSerializer.Serialize(
                new ConsoleSoftpayCloud.model.CreateTransaction() { Action = "PAYMENT" },
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

            SoftpayRequest request = builder.SetContent(json).Build();

            var response = await request.SendAsync();
            string responsestring = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responsestring);
            try
            {
                JsonDocument jdoc = JsonDocument.Parse(responsestring);
                jdoc.RootElement.TryGetProperty("requestId", out JsonElement requestId);

                result = requestId.ToString() ?? string.Empty;
            }
            catch (Exception e)
            {
                ConsoleUI.DisplayFailureMessage($"Faild to get request Id: {responsestring}");
            }
            return result;
        }
    }
}