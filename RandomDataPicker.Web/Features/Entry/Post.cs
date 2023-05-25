using RandomDataPicker.Contracts;

namespace RandomDataPicker.Web.Features.Entry;

public class Post : BaseHandler
{
    public Post(HandlerDependencies handlerDependencies) : base(handlerDependencies)
    {
    }

    public override async Task Execute(HttpContext s)
    {
        var name = s.Request.Form["name"];
        var email = s.Request.Form["email"];
        var city = s.Request.Form["city"];
        var numberOfEntriesValue = s.Request.Form["numberOfEntries"];

        var entries = await GetEntries(s.RequestServices);
        HandlerDependencies.EntryInjector ??= s.RequestServices.GetRequiredService<IEntryInjector>();

        if (int.TryParse(numberOfEntriesValue, out var numberofEntries))
        {
            entries = HandlerDependencies.EntryInjector.InjectEntry(entries, new Models.Entry
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
    }
}
