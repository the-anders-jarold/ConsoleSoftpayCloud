namespace ConsoleSoftpayCloud
{
    internal interface ICloudConnectService
    {
        AccessToken AccessTokenObject { get; set; }
        Uri? BaseURL { get; }
        bool IsAccessTokenValid { get; }
        bool RenewNow { get; }

        Task<bool> RetrieveAccessToken();
    }
}