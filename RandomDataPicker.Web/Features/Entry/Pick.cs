using AutoMapper;
using RandomDataPicker.Contracts;

namespace RandomDataPicker.Web.Features.Entry;

public class Pick : BaseHandler
{
    public Pick(HandlerDependencies handlerDependencies) : base(handlerDependencies)
    {
    }

    public override async Task Execute(HttpContext s)
    {
        int numberOfEntries = 5;
        if (s.Request.Query.TryGetValue("numberOfEntries", out var numberOfEntriesValue)
            && int.TryParse(numberOfEntriesValue, out numberOfEntries))
        {

        }

        HandlerDependencies.EntryPicker ??= s.RequestServices.GetRequiredService<IEntryPicker>();
        var entries = HandlerDependencies.EntryPicker.PickEntries(await GetEntries(s.RequestServices), numberOfEntries);

        if(HandlerDependencies.EntryRepository == null)
        {
            HandlerDependencies.RepositoryFactory ??= s.RequestServices.GetRequiredService<IRepositoryFactory>();

            HandlerDependencies.EntryRepository = HandlerDependencies.RepositoryFactory
                .GetRepository<Persistence.Models.Entry>();

            HandlerDependencies.Mapper ??= s.RequestServices.GetRequiredService<IMapper>();

            var index = 0;
            foreach(var entry in entries)
            {
                await HandlerDependencies.EntryRepository
                    .Save(HandlerDependencies
                            .Mapper.Map<Persistence.Models.Entry>(entry), index++ == entries.Count());
            }
        }

        await s.Response.WriteAsJsonAsync(entries);
    }
}
