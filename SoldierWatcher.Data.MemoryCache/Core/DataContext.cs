using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Text.Json;

namespace SoldierWatcher.Data.MemoryCache.Core;

internal interface IDataContext
{
    Task<TEntity[]> GetEntitiesAsync<TEntity>(string resourceName, CancellationToken cancellationToken);
}

internal class DataContext: IDataContext
{
    private readonly Assembly assembly = typeof(DataContext).Assembly;
    private readonly IMemoryCache memoryCache;

    public DataContext(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }

    public async Task<TEntity[]> GetEntitiesAsync<TEntity>(string resourceName, CancellationToken cancellationToken)
    {
        var manifestResourceName = $"SoldierWatcher.Data.MemoryCache.Resources.{resourceName}.json";

        if (memoryCache.TryGetValue(manifestResourceName, out TEntity[]? result) && result is not null)
            return result;

        using var stream = assembly.GetManifestResourceStream(manifestResourceName);
        if (stream is null)
            return [];

        var entities = await JsonSerializer.DeserializeAsync<TEntity[]>(stream,  cancellationToken: cancellationToken);
        if (entities is null)
            return [];

        memoryCache.Set(resourceName, entities);
        return entities;
    }
}
