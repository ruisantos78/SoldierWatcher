namespace SoldierWatcher.Data.MemoryCache.Entities;

internal class CountryEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
}
