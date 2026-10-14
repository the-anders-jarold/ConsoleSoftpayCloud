namespace ConsoleSoftpayCloud
{
    using MenuBuilder;
    using System.ComponentModel;
    using System.Configuration;
    using System.Threading.Tasks;

   
    internal class MainProcessLoop: INotifyPropertyChanged
    {
    readonly SoftpayImplementation impl;
    private enum MenuOptions
    {
        UpdateAccessToken,
        GetMerchants,
        Exit
    }
    private List<string> menuOptions = new List<string> (){ 
        "Update Access Token",
        "Get Merchants",
        "Exit" 
    };

    public event PropertyChangedEventHandler? PropertyChanged;

    public MainProcessLoop(SoftpayImplementation softpayimpl)
    {
        impl = softpayimpl;
    }
    public async Task Run()
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        Console.Clear();

        Task task;

        int menuSelection = -1;
        (int left, int top) cursorPosition = (0, menuOptions.Count + 2); // Initial cursor position below the menu
        while (menuSelection != menuOptions.Count - 1) // Loop until 'Exit' is selected
        {
            var menu = new Menu(menuOptions.ToArray(),1, 1);
            menu.ModifyMenuLeftJustified();
            Console.CursorVisible = false;
            
            if (menuSelection != -1)
            {
                ConsoleUI.DisplayAnyKey();
                Console.ForegroundColor = ConsoleUI.MenuColor;
                Console.ReadKey();
                ScrollToTop();
                Console.SetCursorPosition(0, 0); 
                for (int i = 0; i < menuOptions.Count + 2; i++) // Clear the area below the menu
                {
                    Console.SetCursorPosition(0, i);
                    Console.WriteLine(new string(' ', Console.WindowWidth)); // Clear the line
                }
            }
            menuSelection = menu.RunMenu();
            Console.CursorLeft = cursorPosition.left;  
            Console.CursorTop = cursorPosition.top;
            Console.WriteLine(menuOptions[menuSelection] + "                   ");
            Console.CursorVisible = true;

            switch (menuSelection)
            {
                case (int)MenuOptions.UpdateAccessToken:
                    ConsoleUI.DisplayInfoMessage("Update Access Token");
                    await impl.RetrieveAccessToken();
                    break;

                case (int)MenuOptions.GetMerchants:
                    ConsoleUI.DisplayInfoMessage("Get Merchants");
                    await impl.GetMerchantsAsync();
                    break;

                case (int)MenuOptions.Exit:
                    // Exit option
                    ConsoleUI.DisplayInfoMessage("Exit");
                    ConsoleUI.DisplayInfoMessage("Exiting the application...");
                    Console.ResetColor();
                    break;

                default:
                    ConsoleUI.DisplayFailureMessage("Invalid selection. Please try again.");
                    break;
            }
        }
    }

       
    static int GetAmountFromUser()
    {
        ConsoleUI.DisplayQueryMessage("Enter amount:".PadRight(Console.WindowWidth - 1));
        string? amountInput = Console.ReadLine();
        if (int.TryParse(amountInput, out int amount))
        {
            return amount;
        }
        else
        {
            ConsoleUI.DisplayFailureMessage("Invalid amount. Please enter a valid integer value.");
            return GetAmountFromUser(); // Recursive call until a valid amount is entered
        }
    }
    protected string GetTransactionIdFromUser()
    {
        ConsoleUI.DisplayQueryMessage("Enter transaction ID:");
        string? transactionId = Console.ReadLine();
        return transactionId;
    }
    private void RaisePropertyChanged(string propertyName)
    {
        // take a copy to prevent thread issues
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    static void ScrollToTop()
    {
        (int left, int top) cursorPosition = Console.GetCursorPosition();
        for (int i = cursorPosition.top; i > 1; i--)
        {
            Console.WriteLine();
        }
    }

} //class
} //namespace
