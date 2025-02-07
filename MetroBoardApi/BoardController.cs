using MetroBoardApi.Model;
using MetroBoardApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace MetroBoardApi;

[ApiController]
[Route("[controller]")]
public class BoardController : ControllerBase
{
    [HttpGet("Stations")]
    public async Task<Matrix> GetStationsAsync()
    {
        var stations = StationService
    }
}