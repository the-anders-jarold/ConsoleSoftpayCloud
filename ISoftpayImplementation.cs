namespace ConsoleSoftpayCloud
{
    public interface ISoftpayImplementation
    {
        string AccessToken { get; set; }
        string AppId { get; set; }
        string BaseUrl { get; set; }
        string MerchantReference { get; set; }
        string StoreId { get; set; }
        string TransactionUri { get; set; }
    }
}