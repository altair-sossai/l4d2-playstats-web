namespace L4D2PlayStats.Core.Infrastructure.Structures;

public class TimedList<T>(int maxItems = 250, TimeSpan? expireIn = null, TimeSpan? delay = null)
{
#if DEBUG
    private const int DefaultDelaySeconds = 1;
    private const int DefaultExpireInMinutes = 99999;
#else
    private const int DefaultDelaySeconds = 30;
    private const int DefaultExpireInMinutes = 60;
#endif

    private readonly TimeSpan _delay = delay ?? TimeSpan.FromSeconds(DefaultDelaySeconds);
    private readonly TimeSpan _expireIn = expireIn ?? TimeSpan.FromMinutes(DefaultExpireInMinutes);

    public List<T> Items { get; } = [];

    public event EventHandler<T>? ItemAdded;

    public void Add(T t)
    {
        if (_delay == TimeSpan.Zero)
        {
            AddItem(t);
            return;
        }

        Task.Delay(_delay).ContinueWith(_ => AddItem(t));
    }

    private void AddItem(T t)
    {
        while (Items.Count >= maxItems)
            Items.RemoveAt(0);

        Items.Add(t);
        ItemAdded?.Invoke(this, t);

        if (_expireIn == TimeSpan.Zero)
        {
            RemoveItem(t);
            return;
        }

        Task.Delay(_expireIn).ContinueWith(_ => RemoveItem(t));
    }

    private void RemoveItem(T t)
    {
        Items.Remove(t);
    }

    public static implicit operator List<T>(TimedList<T> timedList)
    {
        return timedList.Items;
    }
}