namespace MetroBoard.Api;

public static class Settings
{
    public static readonly int SleepTimeSeconds = GetSleepTime();
    public static readonly string WmataApiKey = Environment.GetEnvironmentVariable("WMATA_API_KEY") ?? throw new ArgumentException("WMATA_API_KEY environment variable not set");

    private static int GetSleepTime()
    {
        var sleepTime = Environment.GetEnvironmentVariable("SLEEP_TIME");
        return int.TryParse(sleepTime, out var sleepTimeSeconds) ? sleepTimeSeconds : 10;
    }
}