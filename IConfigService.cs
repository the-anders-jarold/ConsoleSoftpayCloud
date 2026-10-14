using static ConsoleSoftpayCloud.ConfigService;

namespace ConsoleSoftpayCloud
{
    public interface IConfigService
    {
        bool StoreValue(ConfigService.AppSettings name, string value);
        string GetValue(AppSettings name);
    }
}