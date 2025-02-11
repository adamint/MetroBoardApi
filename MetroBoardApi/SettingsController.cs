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
        return ColorUtils.Palette;
    }
}