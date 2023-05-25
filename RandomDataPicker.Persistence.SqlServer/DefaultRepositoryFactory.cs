using RandomDataPicker.Contracts;

namespace RandomDataPicker.Persistence.SqlServer;

public class DefaultRepositoryFactory : IRepositoryFactory
{
    private readonly RandomDataPickerContext context;

    public DefaultRepositoryFactory(RandomDataPickerContext context)
    {
        this.context = context;
    }

    public IRepository<T> GetRepository<T>()
        where T : class
    {
        return new RandomDataPickerEntityRepository<T>(context);
    }
}
