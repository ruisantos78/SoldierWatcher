using SoldierWatcher.Data.Entities;
using System.Text.Json.Serialization;

namespace SoldierWatcher.Data.MemoryCache.Entities;

internal class SoldierEntity
{
    public Guid Id { get; init; }
    public Guid CountryId { get; set; }

    public string Name { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Rank Rank { get; set; }    
}
