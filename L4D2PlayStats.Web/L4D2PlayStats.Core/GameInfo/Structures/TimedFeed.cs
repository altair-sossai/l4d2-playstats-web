using L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

namespace L4D2PlayStats.Core.GameInfo.Structures;

public class TimedFeed(TimeSpan delay, TimeSpan expireIn, int maxItems = 250)
{
    private readonly List<FeedItem> _items = [];
    private readonly Lock _lock = new();

    public IReadOnlyCollection<FeedItem> Items
    {
        get
        {
            lock (_lock)
            {
                return [.. _items];
            }
        }
    }

    public void Add(FeedItem item, bool delayed = true)
    {
        if (!delayed || delay <= TimeSpan.Zero)
        {
            Insert(item);
            return;
        }

        _ = AddAfterDelayAsync(item);
    }

    private async Task AddAfterDelayAsync(FeedItem item)
    {
        await Task.Delay(delay);
        Insert(item);
    }

    private void Insert(FeedItem item)
    {
        lock (_lock)
        {
            _items.Add(item);
            _items.Sort((a, b) => a.When.CompareTo(b.When));

            while (_items.Count > maxItems)
                _items.RemoveAt(0);
        }

        _ = RemoveAfterDelayAsync(item);
    }

    private async Task RemoveAfterDelayAsync(FeedItem item)
    {
        await Task.Delay(expireIn);
        Remove(item);
    }

    private void Remove(FeedItem item)
    {
        lock (_lock)
        {
            _items.Remove(item);
        }
    }
}