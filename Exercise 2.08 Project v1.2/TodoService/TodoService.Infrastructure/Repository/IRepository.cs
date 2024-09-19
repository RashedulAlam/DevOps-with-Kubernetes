namespace TodoService.Infrastructure.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetFirst();
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<IReadOnlyCollection<TEntity>> GetAll();
    }
}
