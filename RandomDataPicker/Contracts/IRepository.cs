using System.Linq.Expressions;

namespace RandomDataPicker.Contracts;

public interface IRepository<T>
{
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> getExpression, CancellationToken cancellationToken = default);
    Task<T> Save(T entity, CancellationToken cancellationToken = default);
}
