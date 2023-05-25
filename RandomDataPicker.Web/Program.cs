using RandomDataPicker.Persistence.SqlServer;
using RandomDataPicker.Core;
using EntryFeature = RandomDataPicker.Web.Features.Entry;
using StatusFeature = RandomDataPicker.Web.Features.Status;
using RandomDataPicker.Web.Features;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices("entries.json")
    .AddDataServices(s => s.GetRequiredService<IConfiguration>().GetConnectionString("default") ?? throw new NullReferenceException(),
        c => c.UseQuerySplittingBehavior(Microsoft.EntityFrameworkCore.QuerySplittingBehavior.SingleQuery))
    .AddCors()
    .AddDistributedMemoryCache();
var app = builder.Build();

const string URL_PREFIX = "/api";
var handlerDependencies = new HandlerDependencies();
app.MapGet($"{URL_PREFIX}", new EntryFeature.Get(handlerDependencies).Execute);

app.MapGet($"{URL_PREFIX}/status", new StatusFeature.Get(handlerDependencies).Execute);

app.MapGet($"{URL_PREFIX}/populate", new EntryFeature.Populate(handlerDependencies).Execute);

app.MapGet($"{URL_PREFIX}/pick", new EntryFeature.Pick(handlerDependencies).Execute);

app.MapPost($"{URL_PREFIX}/inject", new EntryFeature.Post(handlerDependencies).Execute);

app.UseCors(policy => policy.AllowAnyOrigin());

app.Run();