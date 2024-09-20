using Microsoft.EntityFrameworkCore;
using TodoService.Infrastructure.Context;

namespace TodoService.Infrastructure.Repository;

public class Repository<TEntity>(TodoContext context) : IRepository<TEntity>
    where TEntity : class
{
    public Task<TEntity?> GetFirst()
    {
        return context.Set<TEntity>().FirstOrDefaultAsync();
    }

    public async Task<TEntity> Add(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);

        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        context.Set<TEntity>().Update(entity);

        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<IReadOnlyCollection<TEntity>> GetAll()
    {
        return await context.Set<TEntity>().ToArrayAsync();
    }
}