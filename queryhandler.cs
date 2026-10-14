using MenuBuilder;
using System.ComponentModel.Design;

public static class QueryHandler
{
    public static bool YesNo(string prompt)
    {
        ConsoleUI.DisplayQueryMessage($"{prompt} (Y/n)");
        string? answ = Console.ReadLine();
        if (string.IsNullOrEmpty(answ) || string.Compare("y", answ.ToLower()) == 0)
        {
            return true;
        }
        return false;
    }
    public static T GetValueOrDefault<T>(string prompt, T defaultValue)
    {
        ConsoleUI.DisplayQueryMessage($"{prompt} (default: {defaultValue})");
        string? answ = Console.ReadLine();
        if (string.IsNullOrEmpty(answ))
        {
            return defaultValue;
        }
        else
        {
            if (typeof(T) == typeof(Enum))
            {
                object result;
                if (Enum.TryParse(typeof(T), answ, out result)) return (T)result;
                else return defaultValue;
            }
            else if (typeof(T) == typeof(decimal))
            {
                if (decimal.TryParse(answ, out decimal result))
                {
                    return (T)(object)result;
                }
                else
                {
                    return defaultValue;
                }
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(answ, out int result))
                {
                    return (T)(object)result;
                }
                else
                {
                    return defaultValue;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)answ;

            }
        }
        return defaultValue;
    }
    public static string GetValueFromList(string prompt, List<string> optionsList)
    {
        string[] options = optionsList.ToArray();
        ConsoleUI.DisplayQueryMessage($"{prompt}");

        (int left, int top) cursorPosition = Console.GetCursorPosition();
        Menu optionMenu = new Menu(options, cursorPosition.top, 0);
        optionMenu.ModifyMenuLeftJustified();
        optionMenu.SetConsoleTextColor(ConsoleUI.QueryColor, Console.BackgroundColor);
        int selectedIndex = optionMenu.RunMenu();
        ConsoleUI.SetDefaultColor();
        string selected = options[selectedIndex];
        return selected;
    }

}