namespace RandomDataPicker.Web.Features.Entry;

public class Get : BaseHandler
{
    public Get(HandlerDependencies handlerDependencies) : base(handlerDependencies)
    {
    }

    public override async Task Execute(HttpContext context)
    {
        await GetEntries(context.RequestServices);
        await context.Response.WriteAsJsonAsync(await GetStatus(context.RequestServices));
    }
}
