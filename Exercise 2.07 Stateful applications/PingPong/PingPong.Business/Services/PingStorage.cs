using PingPong.Domain.Entity;
using PingPong.Infrastructure.Repository;

namespace PingPong.Business.Services;

public class PingStorage(IUnitOfWork unitOfWork) : IPingStorage
{
    public async Task<int> GetCount()
    {
        var existingCount = await unitOfWork.GetRepository<PingCount>().GetFirst();

        if (existingCount is not null) return existingCount.Count;

        var newCount = new PingCount
        {
            Count = 1
        };

        await unitOfWork.GetRepository<PingCount>().Add(newCount);

        return newCount.Count;

    }

    public async Task RestCount()
    {
        var existingCount = await unitOfWork.GetRepository<PingCount>().GetFirst();

        if (existingCount is not null)
        {
            existingCount.Count = 0;

            unitOfWork.GetRepository<PingCount>().Update(existingCount);
        }
    }
}