namespace SoldierWatcher.Data.Entities;

public record Country
{
    public static readonly Country Empty = new() { 
        Id = Guid.Empty,
        Name = string.Empty
    };

    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
