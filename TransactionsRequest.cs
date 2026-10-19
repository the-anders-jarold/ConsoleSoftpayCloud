using System;
using System.Collections.Generic;
using System.Text;
using static ConsoleSoftpayCloud.PaymentRequest;

namespace ConsoleSoftpayCloud
{
    public class TransactionRequest : SoftpayRequest
    {

        public class TransactionRequestBuilder: SoftpayRequestBuilder 
        {

            // Request inherited from SoftpayRequest;
            protected TransactionRequestBuilder(ISoftpayImplementation impl, SoftpayRequest arequest) : base()
            {
                init(impl, arequest);
            }
            public TransactionRequestBuilder(ISoftpayImplementation impl)
            {
                init(impl, new TransactionRequest());
            }
            private void init(ISoftpayImplementation impl, SoftpayRequest arequest)
            {
                request = arequest as SoftpayRequest;
                request.BaseUrl = impl.BaseUrl;
                request.Uri = impl.TransactionUri;
                request.AccessToken = impl.AccessToken;
                request.MerchantReference = impl.MerchantReference;
                request.Method = HttpMethod.Get;

            }
            virtual public TransactionRequestBuilder SetRequestId(string id)
            {
                request.Uri = request.Uri + "/" + id;
                return this;
            }

        }

    }

}
