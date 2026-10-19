using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleSoftpayCloud
{
    public class ReceiptRequest : TransactionRequest
    {
        public class ReceiptRequestBuilder : TransactionRequestBuilder
        {
            public ReceiptRequestBuilder(ISoftpayImplementation impl) : base(impl)
            {

            }
            public override ReceiptRequestBuilder SetRequestId(string requestid)
            {
                base.SetRequestId(requestid + "/receipt");
                return this;
            }
        }
    }
}
