using LinqKit;
using RandomDataPicker.Contracts;

namespace RandomDataPicker.Web.Features.Entry.Winners;

public class Get : BaseHandler
{
    public Get(HandlerDependencies handlerDependencies) : base(handlerDependencies)
    {
    }

    public async override Task Execute(HttpContext s)
    {
        var totalItems = 25;

        if (int.TryParse(s.Request.Query["totalItems"], out var total))
        {
            totalItems = total;
        }

        var searchFilter = s.Request.Query["q"];

        var entryRepository = s.RequestServices.GetRequiredService<IRepositoryFactory>()
           .GetRepository<Persistence.Models.Entry>();

        var expressionBuilder = PredicateBuilder.New<Persistence.Models.Entry>();
        expressionBuilder.DefaultExpression = a => true;
        if (!string.IsNullOrWhiteSpace(searchFilter))
        {
            expressionBuilder.And(s => s.Name!.Contains(searchFilter!));
            expressionBuilder.Or(s => s.Email!.Contains(searchFilter!));
            expressionBuilder.Or(s => s.City!.Contains(searchFilter!));
        }

        await s.Response.WriteAsJsonAsync(await entryRepository.FindAsync(expressionBuilder,
            s => { return s.Count() > totalItems 
                    ? s.OrderByDescending(a => a.Created).Take(totalItems) 
                    : s.OrderByDescending(n => n.Created).ThenBy(a => a.Name); 
            }));
    }
}
