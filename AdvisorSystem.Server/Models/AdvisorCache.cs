public class AdvisorCache<Integer, Advisor>
{
    private readonly int _capacity;
    private readonly Dictionary<int, LinkedListNode<AdvisorCacheItem>> _cacheMap;
    private readonly LinkedList<AdvisorCacheItem> _cacheList;

    public AdvisorCache(int capacity = 5)
    {
        _capacity = capacity;
        _cacheMap = new Dictionary<int, LinkedListNode<AdvisorCacheItem>>();
        _cacheList = new LinkedList<AdvisorCacheItem>();
    }

    public Advisor? Get(int key)
    {
        if (_cacheMap.TryGetValue(key, out LinkedListNode<AdvisorCacheItem> node))
        {
            _cacheList.Remove(node);
            _cacheList.AddFirst(node);
            return node.Value.Value;
        }

        return default;
    }

    public void Put(int key, Advisor value)
    {
        if (_cacheMap.TryGetValue(key, out LinkedListNode<AdvisorCacheItem> node))
        {
            node.Value.Value = value;
            _cacheList.Remove(node);
            _cacheList.AddFirst(node);
        }
        else
        {
            if (_cacheList.Count >= _capacity)
            {
                LinkedListNode<AdvisorCacheItem> lastNode = _cacheList.Last;
                _cacheList.RemoveLast();
                _cacheMap.Remove(lastNode.Value.Key);
            }

            AdvisorCacheItem cacheItem = new AdvisorCacheItem(key, value);
            LinkedListNode<AdvisorCacheItem> newNode = new LinkedListNode<AdvisorCacheItem>(cacheItem);
            _cacheList.AddFirst(newNode);
            _cacheMap[key] = newNode;
        }
    }

    public void Delete(int key)
    {
        if (_cacheMap.TryGetValue(key, out LinkedListNode<AdvisorCacheItem> node))
        {
            _cacheList.Remove(node);
            _cacheMap.Remove(key);
        }
    }

    private class AdvisorCacheItem
    {
        public int Key { get; }
        public Advisor Value { get; set; }

        public AdvisorCacheItem(int key, Advisor value)
        {
            Key = key;
            Value = value;
        }
    }
}
