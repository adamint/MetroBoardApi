using MetroBoard.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace MetroBoard.Api;

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