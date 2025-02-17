namespace MetroBoard.Api.Model;

public record TrainPositionResponse(TrainPosition[] TrainPositions);

public record TrainPosition(
    string TrainId,
    string TrainNumber,
    int CarCount,
    int DirectionNum,
    int CircuitId,
    string? DestinationStationCode,
    string? LineCode,
    int SecondsAtLocation,
    string ServiceType);