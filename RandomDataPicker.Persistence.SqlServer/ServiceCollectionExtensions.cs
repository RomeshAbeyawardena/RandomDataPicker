using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RandomDataPicker.Contracts;

namespace RandomDataPicker.Persistence.SqlServer;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, 
        Func<IServiceProvider,string> getConnectionString,
        Action<Microsoft.EntityFrameworkCore.Infrastructure.SqlServerDbContextOptionsBuilder>? configureSqlServer)
    {
        return services
            .AddDbContext<RandomDataPickerContext>((s, options) => options.UseSqlServer(getConnectionString(s), configureSqlServer))
            .AddScoped<IRepositoryFactory, DefaultRepositoryFactory>();
    }
}
