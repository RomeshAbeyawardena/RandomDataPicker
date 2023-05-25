namespace RandomDataPicker.Persistence.SqlServer;

public class RandomDataPickerEntityRepository<T> : EntityRepository<RandomDataPickerContext, T>
    where T : class
{
    public RandomDataPickerEntityRepository(RandomDataPickerContext dbContext) : base(dbContext)
    {
    }
}
