namespace SoldierWatcher.Data.MemoryCache.Entities;

internal class SensorEntity
{
    public Guid Id { get; init; }
    public string SensorName { get; set; } = string.Empty;
    public string SensorType { get; set; } = string.Empty;
}
