using Microsoft.Extensions.Internal;

namespace RandomDataPicker.Persistence.SqlServer;

public class RandomDataPickerEntityRepository<T> : EntityRepository<RandomDataPickerContext, T>
    where T : class
{
    public RandomDataPickerEntityRepository(RandomDataPickerContext dbContext, 
        ISystemClock systemClock) : base(dbContext, systemClock)
    {
    }
}
