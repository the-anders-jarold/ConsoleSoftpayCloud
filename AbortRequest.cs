using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoftpayCloud
{
    public class AbortRequest : TransactionRequest
    {
        public class AbortRequestBuilder : TransactionRequestBuilder
        {
            public AbortRequestBuilder(ISoftpayImplementation impl) : base(impl)
            {
                request.Method = HttpMethod.Put;
            }
            public override AbortRequestBuilder SetRequestId(string requestid)
            {
                base.SetRequestId(requestid + "/abort");
                return this;
            }
        }
    }
}
