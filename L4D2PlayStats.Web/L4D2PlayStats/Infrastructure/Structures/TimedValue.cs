namespace L4D2PlayStats.Infrastructure.Structures;

public class TimedValue<T>(T defaultValue = default!, TimeSpan? expirationTime = null, TimeSpan? delayTime = null)
{
    private readonly T _defaultValue = defaultValue;
    private readonly TimeSpan _delayTime = delayTime ?? TimeSpan.FromSeconds(60);
    private readonly TimeSpan _expirationTime = expirationTime ?? TimeSpan.FromSeconds(60);

    private DateTime _lastUpdate = DateTime.MinValue;
    private T _value = defaultValue;

    private bool Expired => DateTime.UtcNow >= _lastUpdate + _expirationTime;

    public T Value
    {
        get
        {
            if (!Expired)
                return _value;

            _value = _defaultValue;
            _lastUpdate = DateTime.MinValue;

            return _value;
        }
        set
        {
            if (_delayTime == TimeSpan.Zero)
            {
                _value = value;
                _lastUpdate = DateTime.UtcNow;
                return;
            }

            Task.Delay(_delayTime).ContinueWith(_ =>
            {
                _value = value;
                _lastUpdate = DateTime.UtcNow;
            });
        }
    }

    public static implicit operator T(TimedValue<T> timedValue)
    {
        return timedValue.Value;
    }
}