using MetroBoardApi.Model;
using MetroBoardApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetroBoardApi;

[ApiController]
[Route("[controller]")]
public class BoardController : ControllerBase
{
    private StationService StationService { get; } = new();

    [HttpGet("CurrentScreen")]
    public async Task<Matrix> GetCurrentScreen()
    {
        return await StationService.DrawStationScreenAsync(2);
    }
}