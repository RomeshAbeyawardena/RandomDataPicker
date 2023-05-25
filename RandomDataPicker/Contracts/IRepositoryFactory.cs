namespace RandomDataPicker.Contracts;

public interface IRepositoryFactory
{
    IRepository<T> GetRepository<T>()
        where T : class;
}
