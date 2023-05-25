using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using RandomDataPicker.Persistence.SqlServer;
using RandomDataPicker.Contracts;
using RandomDataPicker.Core;
using RandomDataPicker.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices("entries.json")
    .AddDataServices(s => s.GetRequiredService<IConfiguration>().GetConnectionString("default") ?? throw new NullReferenceException(),
        c => c.UseQuerySplittingBehavior(Microsoft.EntityFrameworkCore.QuerySplittingBehavior.SingleQuery))
    .AddCors()
    .AddDistributedMemoryCache();
var app = builder.Build();

IDistributedCache? cache  = null;
IEntryProvider? entryProvider = null;
IEntryInjector? entryInjector = null;
IEntryPicker? entryPicker = null;

Random? random = null;
var standardOptions = MessagePackSerializerOptions.Standard;
const string ENTRY_KEY = "entries";
const string IS_POPULATED_KEY = "isPopulated";
const string URL_PREFIX = "/api";

async Task<T?> GetCachedValue<T>(IServiceProvider services, string key)
{
    cache ??= services.GetRequiredService<IDistributedCache>();

    var cachedBytes = await cache.GetAsync(key);

    if (cachedBytes != null)
    {
        return MessagePackSerializer.Deserialize<T>(cachedBytes,
            standardOptions, out _);
    }

    return default;
}

async Task SetCachedValue<T>(IServiceProvider services, string key, T value)
{
    cache ??= services.GetRequiredService<IDistributedCache>();
    await cache.SetAsync(key, MessagePackSerializer.Serialize(value, standardOptions, CancellationToken.None));
}

async Task SetEntries(IServiceProvider services, IEnumerable<Entry> entries)
{
    await SetCachedValue(services, ENTRY_KEY, entries);
}

async Task<IEnumerable<Entry>> GetEntries(IServiceProvider services)
{
    IEnumerable<Entry>? entries = await GetCachedValue<List<Entry>>(services, ENTRY_KEY);
    
    if (entries == null)
    {
        entryProvider ??= services.GetRequiredService<IEntryProvider>();
        entries = entryProvider.GetEntries();
        int ct = 0;
        foreach (var entry in entries)
        {
            entry.Id = ++ct;
        }
        await SetEntries(services, entries);
    }

    return entries;
}

async Task<EntryStatus> GetStatus(IServiceProvider services)
{
    IEnumerable<Entry>? entries = await GetCachedValue<List<Entry>>(services, ENTRY_KEY);
    return new EntryStatus
    {
        IsLoaded = entries != null,
        IsPopulated = await GetCachedValue<bool>(services, IS_POPULATED_KEY),
        TotalNumberOfEntries = entries?.Count()
    };
}

app.MapGet($"{URL_PREFIX}", async(s) => {
    await GetEntries(s.RequestServices);
    await s.Response.WriteAsJsonAsync(await GetStatus(s.RequestServices));
});

app.MapGet($"{URL_PREFIX}/status", async (s) =>
{
    await s.Response.WriteAsJsonAsync(await GetStatus(s.RequestServices));
});

app.MapGet($"{URL_PREFIX}/populate", async (s) =>
{
    var isPopulated = await GetCachedValue<bool>(s.RequestServices, IS_POPULATED_KEY);

    if(isPopulated)
    {
        await s.Response.WriteAsJsonAsync(await GetStatus(s.RequestServices));
        return;
    }

    entryInjector ??= s.RequestServices.GetRequiredService<IEntryInjector>();
    random ??= s.RequestServices.GetRequiredService<Random>();

    var entries = await GetEntries(s.RequestServices);

    int ct = 0;
    foreach (var entry in entries.ToArray())
    {
        entries = entryInjector.InjectEntry(entries, entry, random.Next(150, 150 * random.Next(1, 10)));
    }

    ct = 0;
    foreach (var entry in entries)
    {
        entry.Id = ++ct;
    }

    cache ??= s.RequestServices.GetRequiredService<IDistributedCache>();

    await SetEntries(s.RequestServices, entries);

    await SetCachedValue(s.RequestServices, IS_POPULATED_KEY, true);

    await s.Response.WriteAsJsonAsync(await GetStatus(s.RequestServices));
});

app.MapGet($"{URL_PREFIX}/pick", async(s) =>
{
    int numberOfEntries = 5;
    if(s.Request.Query.TryGetValue("numberOfEntries", out var numberOfEntriesValue)
        && int.TryParse(numberOfEntriesValue, out numberOfEntries))
    {

    }

    entryPicker ??= s.RequestServices.GetRequiredService<IEntryPicker>();
    var entries = entryPicker.PickEntries(await GetEntries(s.RequestServices), numberOfEntries);
    await s.Response.WriteAsJsonAsync(entries);
});

app.MapPost($"{URL_PREFIX}/inject", async (s) =>
{
    var name = s.Request.Form["name"];
    var email = s.Request.Form["email"];
    var city = s.Request.Form["city"];
    var numberOfEntriesValue = s.Request.Form["numberOfEntries"];

    var entries = await GetEntries(s.RequestServices);
    entryInjector ??= s.RequestServices.GetRequiredService<IEntryInjector>();
    
    if(int.TryParse(numberOfEntriesValue, out var numberofEntries))
    {
        entries = entryInjector.InjectEntry(entries, new Entry
        {
            Name = name,
            Email = email,
            City = city,
        }, numberofEntries, true);
        s.Response.StatusCode = 201;
    }
    else
    {
        s.Response.StatusCode = 400;
    }

    var ct = 0;
    foreach (var entry in entries)
    {
        entry.Id = ++ct;
    }

    await SetEntries(s.RequestServices, entries);

    await s.Response.WriteAsJsonAsync(await GetStatus(s.RequestServices));
});

app.UseCors(policy => policy.AllowAnyOrigin());

app.Run();