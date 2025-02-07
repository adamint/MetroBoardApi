namespace MetroBoardApi.Model;

public record StationResponse(Station[] Stations);

public record Station(
    Address Address,
    string Code,
    double Lat,
    double Lon,
    string? LineCode1,
    string? LineCode2,
    string? LineCode3,
    string? LineCode4,
    string Name,
    string StationTogether1,
    string StationTogether2
);

public record Address(string City, string State, string Street, string Zip);