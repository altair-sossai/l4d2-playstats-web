namespace L4D2PlayStats.Core.Infrastructure.Structures;

public class AsyncCache<T>(TimeSpan refreshInterval)
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private T _cachedValue = default!;
    private bool _firstCall = true;
    private bool _isRefreshing;
    private DateTime _lastUpdate = DateTime.MinValue;

    public async Task<T> GetAsync(Func<Task<T>> factory)
    {
        if (_firstCall)
        {
            await _semaphoreSlim.WaitAsync();

            try
            {
                if (_firstCall)
                {
                    _cachedValue = await factory();
                    _lastUpdate = DateTime.UtcNow;
                    _firstCall = false;
                }
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return _cachedValue;
        }

        if (_isRefreshing || refreshInterval < DateTime.UtcNow - _lastUpdate)
            return _cachedValue;

        await _semaphoreSlim.WaitAsync();

        try
        {
            if (_isRefreshing)
                return _cachedValue;

            _isRefreshing = true;

            _ = Task.Run(async () =>
            {
                try
                {
                    _cachedValue = await factory();
                    _lastUpdate = DateTime.UtcNow;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                finally
                {
                    _isRefreshing = false;
                }
            });
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return _cachedValue;
    }
}