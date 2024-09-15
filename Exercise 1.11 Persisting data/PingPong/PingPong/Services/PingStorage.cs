namespace PingPong.Services;

public class PingStorage : IPingStorage
{
    private int _count = 0;

    public int GetCount()
    {
        _count += 1;

        return _count;
    }

    public void RestCount()
    {
        this._count = 0;
    }
}