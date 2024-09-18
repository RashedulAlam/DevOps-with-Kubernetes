using Microsoft.EntityFrameworkCore;
using PingPong.Infrastructure.Context;

namespace PingPong.Infrastructure.Repository;

public class Repository<TEntity>(PingPongContext context) : IRepository<TEntity>
    where TEntity : class
{
    public Task<TEntity?> GetFirst()
    {
        return context.Set<TEntity>().FirstOrDefaultAsync();
    }

    public Task<int> Add(TEntity entity)
    {
        context.Set<TEntity>().AddAsync(entity);

        return context.SaveChangesAsync();
    }

    public void Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);
    }
}