namespace MetroBoard.Api.Service;

public class ScreenService : IHostedService, IDisposable
{
    private Timer? _timer;

    public ScreenDisplay CurrentScreen { get; } = new(Screen.Stations, 4);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(ChangeScreen, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(Settings.SleepTimeSeconds));

        return Task.CompletedTask;
    }

    private void ChangeScreen(object? state)
    {
        CurrentScreen.TimesRemaining--;
        if (CurrentScreen.TimesRemaining == 0)
        {
            CurrentScreen.Screen = CurrentScreen.Screen == Screen.Arrivals
                ? Screen.Stations
                : Screen.Arrivals;
            CurrentScreen.TimesRemaining = 4;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}

public class ScreenDisplay(Screen screen, int timesRemaining)
{
    public Screen Screen { get; set; } = screen;
    public int TimesRemaining { get; set; } = timesRemaining;
}

public enum Screen
{
    Arrivals,
    Stations
}