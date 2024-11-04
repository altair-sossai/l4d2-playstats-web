namespace L4D2PlayStats.Infrastructure.Structures;

public class TimedList<T>(int maxItems = 100, TimeSpan? expireIn = null, TimeSpan? delay = null)
{
    private readonly TimeSpan _delay = delay ?? TimeSpan.FromSeconds(30);
    private readonly TimeSpan _expireIn = expireIn ?? TimeSpan.FromMinutes(10);

    public List<T> Items { get; } = [];

    public event EventHandler<T>? ItemAdded;
    public event EventHandler<T>? ItemRemoved;

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
        ItemRemoved?.Invoke(this, t);
    }

    public static implicit operator List<T>(TimedList<T> timedList)
    {
        return timedList.Items;
    }
}