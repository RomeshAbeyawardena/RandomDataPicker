using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using RandomDataPicker.Contracts;
using RandomDataPicker.Core;
using RandomDataPicker.Models;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices("entries.json")
    .AddDistributedMemoryCache();
var app = builder.Build();

IDistributedCache? cache  = null;
IEntryProvider? entryProvider = null;
IEntryInjector? entryInjector = null;
IEntryPicker? entryPicker = null;

Random? random = null;
var standardOptions = MessagePackSerializerOptions.Standard;
const string ENTRY_KEY = "entries";

async Task<IEnumerable<Entry>> GetEntries(IServiceProvider services)
{
    cache ??= services.GetRequiredService<IDistributedCache>();

    var entryBytes = await cache.GetAsync(ENTRY_KEY);

    IEnumerable<Entry> entries;
    
    if (entryBytes != null)
    {
        entries = MessagePackSerializer.Deserialize<List<Entry>>(entryBytes,
            standardOptions, out _);
    }
    else
    {
        entryProvider ??= services.GetRequiredService<IEntryProvider>();
        entries = entryProvider.GetEntries();
        int ct = 0;
        foreach (var entry in entries)
        {
            entry.Id = ++ct;
        }
        cache.Set(ENTRY_KEY, MessagePackSerializer.Serialize(entries, standardOptions, CancellationToken.None));
    }

    return entries;
}

app.MapGet("/", async(s) => {
    await s.Response.WriteAsJsonAsync(GetEntries(s.RequestServices));
});

app.MapGet("/populate", async (s) =>
{
    entryInjector ??= s.RequestServices.GetRequiredService<IEntryInjector>();
    random ??= s.RequestServices.GetRequiredService<Random>();

    var entries = await GetEntries(s.RequestServices);

    int ct = 0;
    foreach (var entry in entries.ToArray())
    {
        entries = entryInjector.InjectEntry(entries, entry, random.Next(150, 150 * 3));
    }

    ct = 0;
    foreach (var entry in entries)
    {
        entry.Id = ++ct;
    }

    cache ??= s.RequestServices.GetRequiredService<IDistributedCache>();

    cache.Set(ENTRY_KEY, MessagePackSerializer.Serialize(entries, standardOptions, CancellationToken.None));

    await s.Response.WriteAsJsonAsync(entries);
});

app.MapGet("/pick", async(s) =>
{
    entryPicker ??= s.RequestServices.GetRequiredService<IEntryPicker>();
    entryPicker.PickEntries(await GetEntries(s.RequestServices), 5);
});

app.Run();