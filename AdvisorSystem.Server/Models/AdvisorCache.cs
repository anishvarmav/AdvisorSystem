public class AdvisorCache<Integer, Advisor>
{
    private readonly int _capacity;
    // Dictionary to map keys to linked list nodes for fast access
    private readonly Dictionary<int, LinkedListNode<AdvisorCacheItem>> _cacheMap;
    // Linked list to maintain the order of cache items
    private readonly LinkedList<AdvisorCacheItem> _cacheList;

    public AdvisorCache(int capacity = 5)
    {
        _capacity = capacity;
        _cacheMap = new Dictionary<int, LinkedListNode<AdvisorCacheItem>>();
        _cacheList = new LinkedList<AdvisorCacheItem>();
    }

    // Retrieves an item from the cache by its key
    public Advisor? Get(int key)
    {
        if (_cacheMap.TryGetValue(key, out LinkedListNode<AdvisorCacheItem> node))
        {
            // Move the accessed node to the front of the linked list
            _cacheList.Remove(node);
            _cacheList.AddFirst(node);
            return node.Value.Value;
        }

        return default;
    }

    // Adds a new item or updates an existing item in the cache
    public void Put(int key, Advisor value)
    {
        if (_cacheMap.TryGetValue(key, out LinkedListNode<AdvisorCacheItem> node))
        {
            // Update the value and move the node to the front
            node.Value.Value = value;
            _cacheList.Remove(node);
            _cacheList.AddFirst(node);
        }
        else
        {
            // Check if the cache has reached its capacity
            if (_cacheList.Count >= _capacity)
            {
                // Remove the most recently used item from the cache
                LinkedListNode<AdvisorCacheItem> lastNode = _cacheList.Last;
                _cacheList.RemoveLast();
                _cacheMap.Remove(lastNode.Value.Key);
            }

            // Create a new cache item and add it to the front of the linked list
            AdvisorCacheItem cacheItem = new AdvisorCacheItem(key, value);
            LinkedListNode<AdvisorCacheItem> newNode = new LinkedListNode<AdvisorCacheItem>(cacheItem);
            _cacheList.AddFirst(newNode);
            _cacheMap[key] = newNode;
        }
    }

    // Deletes an item from the cache by its key
    public void Delete(int key)
    {
        if (_cacheMap.TryGetValue(key, out LinkedListNode<AdvisorCacheItem> node))
        {
            // Remove the node from the linked list and the map
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
