namespace PingPong.Business.Services
{
    public interface IPingStorage
    {
        Task<int> GetCount();

        Task RestCount();
    }
}
