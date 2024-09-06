namespace SoldierWatcher.Data.MemoryCache.Entities;

internal class SoldierMarkerEntity
{
    public Guid Id { get; init; }
    public Guid SoldierId { get; init; }
    public Guid SensorId { get; init; }
    public Guid TrainingLocationId { get; init; }

    public string TrainingInfo { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string SerialNumber { get; set; } = string.Empty;
}
