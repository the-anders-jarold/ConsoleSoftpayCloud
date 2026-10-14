using ConsoleSoftpayCloud;

Console.WriteLine("Welcome to test app for Pay Softpos using cloud");

internal partial class Program
{
    static IConfigService? Config;
    static ICloudConnectService? cloudConnectService;
    static SoftpayImplementation? impl;
    static async Task Main(string[] args)
    {
        ConsoleUI.DisplayQueryMessage("Enter a configuration path or hit enter to use default App.config");
        string configPath = Console.ReadLine();

        Config = ConfigService.Create(configPath);

        cloudConnectService = new CloudConnectService(Config);
        impl = new SoftpayImplementation(Config, cloudConnectService);

        if (!QueryHandler.YesNo("Use current credentials?"))
        {
            ConsoleUI.DisplayQueryMessage("Enter Client id: ");
            string s1 = Console.ReadLine();
            ConsoleUI.DisplayQueryMessage("Enter Client secret: ");
            string s2 = Console.ReadLine();
            impl.AddSecrets(s1, s2);
        }
        if (!impl.HasSecret())
        {
            ConsoleUI.DisplayFailureMessage("No valid credentials found.");
            return;
        }
        if (!cloudConnectService.IsAccessTokenValid)
        {
            await cloudConnectService.RetrieveAccessToken();
            if (cloudConnectService.IsAccessTokenValid)
            {
                ConsoleUI.DisplaySuccessMessage("Access token retrieved successfully.");
                ConsoleUI.DisplaySuccessMessage($"Token: {cloudConnectService.AccessTokenObject.Token}");
            }
            else
            {
                ConsoleUI.DisplayFailureMessage("Failed to retrieve access token.");
            }
        }
        await new MainProcessLoop(impl).Run();

    }
}
