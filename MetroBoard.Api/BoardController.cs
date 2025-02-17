using MetroBoard.Api.Model;
using MetroBoard.Api.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetroBoard.Api;

[ApiController]
[Route("[controller]")]
public class BoardController(ScreenService screenService, StationService stationService, ArrivalsService arrivalsService) : ControllerBase
{
    [HttpGet("CurrentScreen")]
    public async Task<Matrix> GetCurrentScreen()
    {
        var screen = screenService.CurrentScreen;
        Console.WriteLine(screen);

        if (screen.Screen == Screen.Arrivals)
        {
            return await arrivalsService.DrawArrivalsScreenAsync();
        }
        else
        {
            return await stationService.DrawStationScreenAsync(screen.TimesRemaining);
        }
    }
}