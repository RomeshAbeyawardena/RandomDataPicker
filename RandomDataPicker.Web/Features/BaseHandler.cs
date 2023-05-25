using MessagePack;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Caching.Distributed;
using RandomDataPicker.Contracts;
using RandomDataPicker.Models;

namespace RandomDataPicker.Web.Features;

public abstract class BaseHandler
{
    public BaseHandler(HandlerDependencies handlerDependencies)
    {
        this.handlerDependencies = handlerDependencies;
    }

    protected readonly MessagePackSerializerOptions standardOptions = MessagePackSerializerOptions.Standard;
    private readonly HandlerDependencies handlerDependencies;
    const string ENTRY_KEY = "entries";
    const string IS_POPULATED_KEY = "isPopulated";

    protected HandlerDependencies HandlerDependencies => handlerDependencies;

    protected async Task<T?> GetCachedValue<T>(IServiceProvider services, string key)
    {
        handlerDependencies.Cache ??= services.GetRequiredService<IDistributedCache>();

        var cachedBytes = await handlerDependencies.Cache.GetAsync(key);

        if (cachedBytes != null)
        {
            return MessagePackSerializer.Deserialize<T>(cachedBytes,
                standardOptions, out _);
        }

        return default;
    }

    protected async Task SetCachedValue<T>(IServiceProvider services, string key, T value)
    {
        handlerDependencies.Cache ??= services.GetRequiredService<IDistributedCache>();
        await handlerDependencies.Cache.SetAsync(key, MessagePackSerializer.Serialize(value, standardOptions, CancellationToken.None));
    }

    protected async Task SetEntries(IServiceProvider services, IEnumerable<Models.Entry> entries)
    {
        await SetCachedValue(services, ENTRY_KEY, entries);
    }

    protected async Task<IEnumerable<Models.Entry>> GetEntries(IServiceProvider services)
    {
        IEnumerable<Models.Entry>? entries = await GetCachedValue<List<Models.Entry>>(services, ENTRY_KEY);

        if (entries == null)
        {
            handlerDependencies.EntryProvider ??= services.GetRequiredService<IEntryProvider>();
            entries = handlerDependencies.EntryProvider.GetEntries();
            int ct = 0;
            foreach (var entry in entries)
            {
                entry.Id = ++ct;
            }
            await SetEntries(services, entries);
        }

        return entries;
    }

    protected async Task<EntryStatus> GetStatus(IServiceProvider services)
    {
        IEnumerable<Models.Entry>? entries = await GetCachedValue<List<Models.Entry>>(services, ENTRY_KEY);

        var entryRepository = services.GetRequiredService<IRepositoryFactory>()
           .GetRepository<Persistence.Models.Entry>();

        return new EntryStatus
        {
            IsLoaded = entries != null,
            IsPopulated = await GetCachedValue<bool>(services, IS_POPULATED_KEY),
            TotalNumberOfEntries = entries?.Count(),
            TotalPersistedEntries = await entryRepository.CountAsync(a => true)
        };
    }

    public abstract Task Execute(HttpContext content);
}
