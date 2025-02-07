using System.Diagnostics;
using MetroBoardApi.Model;

namespace MetroBoardApi.Service;

public class StationService
{
    private Matrix?

    private static Dictionary<string, NormalizedCoordinate> GetStationNormalizedCoordinates(Station[] stations)
    {

    }

    private static async Task<Station[]> GetStationsAsync()
    {
        var response = await HttpUtils.GetAsync<StationResponse>("/Rail.svc/json/jStations");
        Debug.Assert(response is not null);

        return response.Stations;
    }
}

public record NormalizedCoordinate(int Lat, int Lon);