namespace PingPong.Infrastructure.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetFirst();
        Task<int> Add(TEntity entity);
        Task<int> Update(TEntity entity);
    }
}
