namespace RandomDataPicker.Web.Features.Status;

public class Get : BaseHandler
{
    public Get(HandlerDependencies handlerDependencies) : base(handlerDependencies)
    {
    }

    public override async Task Execute(HttpContext s)
    {
        await s.Response.WriteAsJsonAsync(await GetStatus(s.RequestServices));
    }
}
