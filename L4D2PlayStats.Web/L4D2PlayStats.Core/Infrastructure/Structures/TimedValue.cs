namespace L4D2PlayStats.Core.Infrastructure.Structures;

public class TimedValue<T>(TimeSpan delay, TimeSpan expireIn, T initialValue = default!)
{
    private DateTime _lastUpdate = DateTime.MinValue;
    private T _value = initialValue;

    private bool Expired => DateTime.UtcNow >= _lastUpdate + expireIn;

    public T Value
    {
        get
        {
            if (Expired)
                UpdateValue(field);

            return _value;
        }
        set
        {
            if (delay == TimeSpan.Zero)
            {
                UpdateValue(value);
                return;
            }

            Task.Delay(delay).ContinueWith(_ => UpdateValue(value));
        }
    } = initialValue;

    public event EventHandler<T>? ValueUpdated;

    private void UpdateValue(T value)
    {
        _value = value;
        _lastUpdate = DateTime.UtcNow;

        ValueUpdated?.Invoke(this, value);
    }

    public static implicit operator T(TimedValue<T> timedValue)
    {
        return timedValue.Value;
    }
}