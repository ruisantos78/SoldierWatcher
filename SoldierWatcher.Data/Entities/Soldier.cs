namespace SoldierWatcher.Data.Entities;

public record Soldier
{
    public static readonly Soldier Empty = new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Rank = Rank.Private,
        Country = Country.Empty 
    };

    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required Rank Rank { get; set; }
    public required Country Country { get; set; }
}
