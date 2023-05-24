using Microsoft.Extensions.DependencyInjection;
using RandomDataPicker.Contracts;

namespace RandomDataPicker.Core;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, 
        string jsonPath)
    {
        return services
            .AddSingleton<IApplicationConfiguration>(new DefaultApplicationConfiguration { JsonPath = jsonPath })
            .AddSingleton<IEntryProvider, JsonEntryProvider>()
            .AddSingleton<Random>()
            .AddSingleton<IEntryPicker, RandomEntryPicker>()
            .AddSingleton<IEntryInjector, DefaultEntryInjector>();
    }
}
