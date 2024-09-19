using TodoService.Infrastructure.Context;

namespace TodoService.Infrastructure.Repository;

public class UnitOfWork(TodoContext context) : IUnitOfWork, IAsyncDisposable
{
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        return new Repository<TEntity>(context);
    }


    public void Dispose()
    {
        context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
    }
}