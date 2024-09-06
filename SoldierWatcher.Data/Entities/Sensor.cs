namespace SoldierWatcher.Data.Entities;

public record Sensor
{
    public static readonly Sensor Empty = new()
    { 
        Id = Guid.Empty,
        SensorName = string.Empty,
        SensorType = string.Empty,
    };

    public required Guid Id { get; init; }
    public required string SensorName { get; set; }
    public required string SensorType { get; set; }
}
