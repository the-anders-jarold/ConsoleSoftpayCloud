using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSoftpayCloud
{
    public class CloudPoster : IPoster
    {
        readonly CloudConnectService cloudConnectService;
        readonly Uri? uri;
        readonly string contentType;
        public CloudPoster(CloudConnectService ccs, Uri Uri, string contentType)
        {
            cloudConnectService = ccs;
            uri = Uri;
            contentType = contentType;
        }
        public async Task<string> PostAsync(string nexomessage)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                                cloudConnectService.AccessTokenObject.Token);

            var msg = new HttpRequestMessage(HttpMethod.Get, uri);

            msg.Headers.Add("accept", "*/*");
            //msg.Content = new StringContent(nexomessage, Encoding.UTF8, contentType);
            if (!string.IsNullOrEmpty(cloudConnectService.MerchantReference))
            {
                msg.Headers.Add("X-Softpay-Merchant-Reference", cloudConnectService.MerchantReference);
            }

            var response = await httpClient.SendAsync(msg);
            //var response = await httpClient.GetAsync(uri);
            
            return await response.Content.ReadAsStringAsync();
        }
    }
}
