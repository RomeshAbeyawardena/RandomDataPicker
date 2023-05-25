using System.Linq.Expressions;

namespace RandomDataPicker.Contracts;

public interface IRepository<T>
{
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> getExpression,
        Func<IQueryable<T>, IQueryable<T>>? configure = null,
        CancellationToken cancellationToken = default);
    Task<T> Save(T entity, bool commitChanges = true,
        CancellationToken cancellationToken = default);
}
