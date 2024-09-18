using PingPong.Infrastructure.Context;

namespace PingPong.Infrastructure.Repository;

public class UnitOfWork(PingPongContext context) : IUnitOfWork, IAsyncDisposable
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