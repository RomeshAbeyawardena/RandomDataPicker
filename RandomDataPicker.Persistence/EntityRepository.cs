using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using RandomDataPicker.Contracts;
using System.Linq.Expressions;

namespace RandomDataPicker.Persistence;
public abstract class EntityRepository<TDbContext, T> : IRepository<T>
    where TDbContext : DbContext
    where T : class
{
    private readonly TDbContext dbContext;
    private readonly ISystemClock systemClock;
    private readonly DbSet<T> dbSet;
    public EntityRepository(TDbContext dbContext, ISystemClock systemClock)
    {
        this.dbContext = dbContext;
        this.systemClock = systemClock;
        dbSet = dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> findExpression, 
        Func<IQueryable<T>, IQueryable<T>>? configure = null, 
        CancellationToken cancellationToken = default)
    {
        var query = configure == null
            ? dbSet.Where(findExpression)
            : configure(dbSet.Where(findExpression));

        return await query
            .ToArrayAsync(cancellationToken);
    }

    public async Task<T> Save(T entity, bool commitChanges = true, CancellationToken cancellationToken = default)
    {
        if(entity is IIdentity identity)
        {
            if(identity.Id == Guid.Empty)
            {
                if(entity is ICreated created)
                {
                    created.Created = systemClock.UtcNow;
                }

                dbSet.Add(entity);
            }
            else
            {
                dbSet.Update(entity);
            }
        }
        else
        {
            dbSet.Add(entity);
        }

        if (commitChanges)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
        return entity;
    }
}
