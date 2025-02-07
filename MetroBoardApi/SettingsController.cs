using MetroBoardApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace MetroBoardApi;

[ApiController]
[Route("[controller]")]
public class SettingsController : ControllerBase
{
    [HttpGet("Palette")]
    public Dictionary<int, int> Palette()
    {
        var palette = new Dictionary<int, int>
        {
            { 0, 0x000000 },
            { (int)Color.Blue, 0x0000FF },
            { (int)Color.Green, 0x00FF00 },
            { (int)Color.Orange, 0xC76E00 },
            { (int)Color.Red, 0xFF0000 },
            { (int)Color.Silver, 0xC0C0C0 },
            { (int)Color.Yellow, 0xFFFF00 },
            { (int)Color.White, 0x000000 }
        };

        return palette;
    }
}