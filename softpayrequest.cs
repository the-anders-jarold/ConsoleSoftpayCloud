using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace ConsoleSoftpayCloud
{
    public class SoftpayRequest
    {
        internal string BaseUrl { get; set; }
        internal string Uri { get; set; }
        internal string? AccessToken { get; set; }
        internal string Accept { get; set; } = "*/*";
        internal string MerchantReference { get; set; }
        internal HttpMethod Method { get; set; } = HttpMethod.Get;
        private NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
        internal NameValueCollection Query { 
            get { return query; } 
            set { query.Add(value); } 
        }
        internal string Content { get; set; }

        protected HttpRequestMessage? Message = null;

        HttpClient httpClient;
        private SoftpayRequest()
        {

        }
        SoftpayRequest Build()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var builder = new UriBuilder(BaseUrl);
            builder.Path = Uri;
            if (query.Count > 0)
            {
                builder.Query = Query.ToString();
            }

            var msg = new HttpRequestMessage(Method, builder.Uri);
            msg.Headers.Add("accept", Accept);
            if (!string.IsNullOrEmpty(MerchantReference))
                msg.Headers.Add("X-Softpay-Merchant-Reference", MerchantReference);
            if (!string.IsNullOrEmpty(Content))
            {
                msg.Content = new StringContent(Content);
            }
            Message = msg;

            return this;
        }
        public async Task<HttpResponseMessage> SendAsync()
        {
            if (Message == null)
                return new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.BadRequest};

            return await httpClient.SendAsync(Message);
        }
        public class SoftpayRequestBuilder
        {
            private SoftpayRequest request = null;

            public SoftpayRequestBuilder()
            {
                request = new SoftpayRequest();

            }
            public SoftpayRequestBuilder SetBaseUrl(string baseurl)
            {
                request.BaseUrl = baseurl;
                return this;
            }
            public SoftpayRequestBuilder SetUri(string uristring)
            {
                request.Uri = uristring;
                return this;
            }
            public SoftpayRequestBuilder SetQuery(NameValueCollection query)
            {
                query = query;
                return this;
            }
            public SoftpayRequestBuilder SetAccessToken(string token)
            {
                request.AccessToken = token;
                return this;
            }
            public SoftpayRequestBuilder SetAcceptHeader(string accept)
            {
                request.Accept = accept;
                return this;
            }
            public SoftpayRequestBuilder SetMerchantReference(string merchantreference)
            {
                return this;
            }
            public SoftpayRequest Build()
            {
                request.Build();
                return request;
            }
        }
        
    }
}
