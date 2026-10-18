using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleSoftpayCloud
{
    public class PaymentRequest : SoftpayRequest
    {

        public string AppId { get; set; } = "";
        public int Amount { get; set; }
        public string CurrencyCode { get; set; } = "SEK";
        public int SchemeId { get; set; }
        public string PosReferenceNumber { get; set; } = "";
        public PaymentOptions Options { get; set; } = new();
        public string EmployeeReference { get; set; } = "";

        public class PaymentOptions
        {
            public string Receipt { get; set; } = "LOCAL";
            public string CardHolderLocale { get; set; } = "en";
            public bool AmountIsoCodes { get; set; } = false;
            public string BatchType { get; set; } = "0";
            public int SwitchBackTimeout { get; set; } = 5000;
            public string ActivityToResume { get; set; } = "MainActivity";
            public bool SuppressAppNotification { get; set; } = false;
            public Tipping Tipping { get; set; } = new();
            public bool Surcharge { get; set; } = false;
            public bool Dcc { get; set; } = false;
        }

        public class Tipping
        {
            public string Indicator { get; set; } = "LOCAL_OPTIONS";
            public string Variant { get; set; } = "TOTAL_BEFORE_TAP";
            public int[] Percentages { get; set; } =  {5,10,15};
            public int HighTip { get; set; } = 100;
            public bool TipPrompt { get; set; } = true;
        }
        void  Build()
        {
            base.Build();
            
        }
        public class PaymentRequestBuilder : SoftpayRequestBuilder
        {
            private PaymentRequest request = new PaymentRequest();
            public PaymentRequestBuilder(ISoftpayImplementation impl) : base()
            {
                request.BaseUrl = impl.BaseUrl;
                request.Uri = impl.TransactionUri;
                request.Method = HttpMethod.Put;
                request.AccessToken = impl.AccessToken;
                request.MerchantReference = impl.MerchantReference;
                request.AppId = impl.AppId;
            }
            public PaymentRequestBuilder SetRequestId(string id)
            {
                request.Uri = request.Uri + "/" + id;
                return this;
            }
            public PaymentRequestBuilder SetAmount(int amount)
            {
                request.Amount = amount;
                return this;
            }
            public PaymentRequestBuilder SetCurrenceyCode(string code)
            {
                request.CurrencyCode = code;
                return this;
            }

            public PaymentRequest Build()
            {
                var json = JsonSerializer.Serialize(request,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    });
                request.Content = json;
                request.Build();
                return request;
            }
        }
    }
}
