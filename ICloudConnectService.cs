namespace ConsoleSoftpayCloud
{
    internal interface ICloudConnectService
    {
        AccessToken AccessTokenObject { get; set; }
        Uri? BaseURL { get; }
        string ChannelId { get; set; }
        bool IsAccessTokenValid { get; }
        Uri? NexoUri { get; }
        Uri? EventsUri { get; }
        bool RenewNow { get; }

        bool HasChannelID();
        Task<bool> RetrieveAccessToken();
        bool StoreChannelId(string json);
    }
}