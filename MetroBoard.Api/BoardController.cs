using MetroBoard.Api.Model;
using MetroBoard.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetroBoard.Api;

[ApiController]
[Route("[controller]")]
public class BoardController(ScreenService screenService) : ControllerBase
{
    private StationService StationService { get; } = new();
    private ArrivalsService ArrivalsService { get; } = new();

    [HttpGet("CurrentScreen")]
    public async Task<Matrix> GetCurrentScreen()
    {
        var screen = screenService.CurrentScreen;
        Console.WriteLine(screen);

        if (screen.Screen == Screen.Arrivals)
        {
            return await ArrivalsService.DrawArrivalsScreenAsync();
        }
        else
        {
            return await StationService.DrawStationScreenAsync(screen.TimesRemaining);
        }
    }
}