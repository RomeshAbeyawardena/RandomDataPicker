using Microsoft.Extensions.Caching.Distributed;
using RandomDataPicker.Contracts;

namespace RandomDataPicker.Web.Features.Entry;

public class Populate : BaseHandler
{
    private const string IS_POPULATED_KEY = "isPopulated";

    public Populate(HandlerDependencies handlerDependencies) : base(handlerDependencies)
    {
    }

    public override async Task Execute(HttpContext s)
    {
        var isPopulated = await GetCachedValue<bool>(s.RequestServices, IS_POPULATED_KEY);

        if (isPopulated)
        {
            await s.Response.WriteAsJsonAsync(await GetStatus(s.RequestServices));
            return;
        }

        HandlerDependencies.EntryInjector ??= s.RequestServices.GetRequiredService<IEntryInjector>();
        HandlerDependencies.Random ??= s.RequestServices.GetRequiredService<Random>();

        var entries = await GetEntries(s.RequestServices);

        int ct = 0;
        foreach (var entry in entries.ToArray())
        {
            entries = HandlerDependencies.EntryInjector.InjectEntry(entries, entry,
                HandlerDependencies.Random.Next(150, 150 * HandlerDependencies.Random.Next(1, 10)));
        }

        ct = 0;
        foreach (var entry in entries)
        {
            entry.Id = ++ct;
        }

        HandlerDependencies.Cache ??= s.RequestServices.GetRequiredService<IDistributedCache>();

        await SetEntries(s.RequestServices, entries);

        await SetCachedValue(s.RequestServices, IS_POPULATED_KEY, true);

        await s.Response.WriteAsJsonAsync(await GetStatus(s.RequestServices));
    }
}