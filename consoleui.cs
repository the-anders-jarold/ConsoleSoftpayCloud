using System.Data;

public static class ConsoleUI
{
    public const ConsoleColor MenuColor = ConsoleColor.White;
    public const ConsoleColor SuccessColor = ConsoleColor.Green;
    public const ConsoleColor FailureColor = ConsoleColor.Red;
    public const ConsoleColor AbortColor = ConsoleColor.Red;
    public const ConsoleColor QueryColor = ConsoleColor.Cyan;
    public const ConsoleColor OptionListColor = ConsoleColor.Magenta;
    public const ConsoleColor EventColor = ConsoleColor.Yellow;
    public const ConsoleColor InfoColor = ConsoleColor.Blue;
    public const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
    public const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;

    public static void DisplayAnyKey(string message="Press any key to continue...")
    {
        Console.ForegroundColor = ConsoleUI.DefaultBackgroundColor;
        Console.BackgroundColor = ConsoleUI.MenuColor;
        Console.WriteLine(message);
        SetDefaultColor();
    }
    public static void DisplayFailureMessage(string message)
    {
        Console.ForegroundColor = ConsoleUI.FailureColor;
        Console.WriteLine(message);
        SetDefaultColor();
    }
    public static void DisplaySuccessMessage(string message)
    {
        Console.ForegroundColor = ConsoleUI.SuccessColor;
        Console.WriteLine(message);
        SetDefaultColor();
    }
    public static void DisplayQueryMessage(string message)
    {
        Console.ForegroundColor = ConsoleUI.QueryColor;
        Console.WriteLine(message);
        SetDefaultColor();
    }
    public static void DisplayEventMessage(string message)
    {
        var pos = Console.GetCursorPosition();
        Console.SetCursorPosition(0, Console.WindowHeight - 3);
        Console.ForegroundColor = ConsoleUI.EventColor;
        string trimmedMessage = message.Replace("\r\n", " ");
        string[] strings = trimmedMessage.Split('\n');
        foreach (string str in strings)
        {
            if (!string.IsNullOrEmpty(str)) {
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
            }
        }
        trimmedMessage = trimmedMessage.PadRight(Console.WindowWidth - 1);
        Console.Write(trimmedMessage);
        SetDefaultColor();
        Console.SetCursorPosition(pos.Left, pos.Top);
    }
    public static void DisplayInfoMessage(string message)
    {
        Console.ForegroundColor = ConsoleUI.InfoColor;
        Console.WriteLine(message);
        SetDefaultColor();
    }
    public static void SetDefaultColor()
    {
        Console.ForegroundColor = ConsoleUI.DefaultForegroundColor;
        Console.BackgroundColor = ConsoleUI.DefaultBackgroundColor;
    }
}
 