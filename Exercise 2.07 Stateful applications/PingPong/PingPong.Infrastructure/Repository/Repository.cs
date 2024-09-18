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

    public async Task<int> Add(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);

        return await context.SaveChangesAsync();
    }

    public Task<int> Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);

        return context.SaveChangesAsync();
    }
}