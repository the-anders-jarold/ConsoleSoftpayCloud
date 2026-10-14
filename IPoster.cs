namespace ConsoleSoftpayCloud
{
    public interface IPoster
    {
        Task<string> PostAsync(string nexomessage);
    }
}