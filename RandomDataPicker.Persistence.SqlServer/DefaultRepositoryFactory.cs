using Microsoft.Extensions.Internal;
using RandomDataPicker.Contracts;

namespace RandomDataPicker.Persistence.SqlServer;

public class DefaultRepositoryFactory : IRepositoryFactory
{
    private readonly RandomDataPickerContext context;
    private readonly ISystemClock systemClock;

    public DefaultRepositoryFactory(RandomDataPickerContext context,
        ISystemClock systemClock)
    {
        this.context = context;
        this.systemClock = systemClock;
    }

    public IRepository<T> GetRepository<T>()
        where T : class
    {
        return new RandomDataPickerEntityRepository<T>(context, systemClock);
    }
}
