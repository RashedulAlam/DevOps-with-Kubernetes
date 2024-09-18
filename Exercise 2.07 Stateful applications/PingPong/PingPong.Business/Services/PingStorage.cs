using PingPong.Domain.Entity;
using PingPong.Infrastructure.Repository;

namespace PingPong.Business.Services;

public class PingStorage(IUnitOfWork unitOfWork) : IPingStorage
{
    private readonly IRepository<PingCount> _repository = unitOfWork.GetRepository<PingCount>();

    public async Task<int> GetCount()
    {
        var existingCount = await this._repository.GetFirst();

        if (existingCount is not null)
        {
            existingCount.Count += 1;

            await this._repository.Update(existingCount);

            return ((await this._repository.GetFirst())!).Count;
        };

        var newCount = new PingCount
        {
            Count = 1
        };

        await this._repository.Add(newCount);

        return newCount.Count;

    }

    public async Task RestCount()
    {
        var existingCount = await this._repository.GetFirst();

        if (existingCount is not null)
        {
            existingCount.Count = 0;

            await this._repository.Update(existingCount);
        }
    }
}