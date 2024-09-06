using SoldierWatcher.Data.Entities;

namespace SoldierWatcher.Data.MemoryCache.Entities;

internal class TrainingLocationEntity
{
    public Guid Id { get; init; } = Guid.Empty;
    public string LocationName { get; init; } = string.Empty;
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}
