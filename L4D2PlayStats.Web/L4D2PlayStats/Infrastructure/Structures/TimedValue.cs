﻿namespace L4D2PlayStats.Infrastructure.Structures;

public class TimedValue<T>(T defaultValue = default!, TimeSpan? expireIn = null, TimeSpan? delay = null)
{
    private readonly T _defaultValue = defaultValue;
    private readonly TimeSpan _delay = delay ?? TimeSpan.FromSeconds(30);
    private readonly TimeSpan _expireIn = expireIn ?? TimeSpan.FromMinutes(2);

    private DateTime _lastUpdate = DateTime.MinValue;
    private T _value = defaultValue;

    private bool Expired => DateTime.UtcNow >= _lastUpdate + _expireIn;

    public T Value
    {
        get
        {
            if (Expired)
                UpdateValue(_defaultValue);

            return _value;
        }
        set
        {
            if (_delay == TimeSpan.Zero)
            {
                UpdateValue(value);
                return;
            }

            Task.Delay(_delay).ContinueWith(_ => UpdateValue(value));
        }
    }

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