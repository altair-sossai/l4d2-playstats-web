using Serilog;

namespace L4D2PlayStats.Core.Infrastructure.Structures;

public class AsyncCache<T>(TimeSpan refreshInterval)
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private T? _cachedValue;
    private bool _firstCall = true;
    private bool _isRefreshing;
    private DateTime _lastUpdate = DateTime.MinValue;

    public async Task<T?> GetAsync(Func<Task<T?>> handler, CancellationToken cancellationToken)
    {
        if (_firstCall)
        {
            await _semaphoreSlim.WaitAsync(cancellationToken);

            try
            {
                if (_firstCall)
                {
                    _cachedValue = await handler();
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

        var elapsed = DateTime.UtcNow - _lastUpdate;

        if (_isRefreshing || elapsed < refreshInterval)
            return _cachedValue;

        await _semaphoreSlim.WaitAsync(cancellationToken);

        try
        {
            if (_isRefreshing)
                return _cachedValue;

            _isRefreshing = true;

            _ = Task.Run(async () =>
            {
                try
                {
                    _cachedValue = await handler();
                    _lastUpdate = DateTime.UtcNow;
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Error refreshing cache");
                }
                finally
                {
                    _isRefreshing = false;
                }
            }, cancellationToken);
        }
        finally
        {
            _semaphoreSlim.Release();
        }

        return _cachedValue;
    }
}