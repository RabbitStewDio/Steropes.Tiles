using System.Collections.Generic;

namespace Steropes.Tiles.MonogameDemo.Util
{
  public class LRUCache<TKey, TValue>
  {
    readonly int capacity;
    readonly LinkedList<LRUCacheItem> lruList;
    readonly Dictionary<TKey, LinkedListNode<LRUCacheItem>> cacheMap;

    public LRUCache(int capacity)
    {
      this.capacity = capacity;
      lruList = new LinkedList<LRUCacheItem>();
      cacheMap = new Dictionary<TKey, LinkedListNode<LRUCacheItem>>(capacity);
    }

    struct LRUCacheItem
    {
      public LRUCacheItem(TKey k, TValue v)
      {
        key = k;
        value = v;
      }

      public readonly TKey key;
      public readonly TValue value;
    }

    public bool TryGet(TKey key, out TValue retval)
    {
      LinkedListNode<LRUCacheItem> node;
      if (cacheMap.TryGetValue(key, out node))
      {
        TValue value = node.Value.value;
        lruList.Remove(node);
        lruList.AddLast(node);
        retval = value;
        return true;
      }
      retval = default(TValue);
      return false;
    }

    public void Add(TKey key, TValue val)
    {
      if (cacheMap.Count >= capacity)
      {
        RemoveFirst();
      }

      var cacheItem = new LRUCacheItem(key, val);
      var node = new LinkedListNode<LRUCacheItem>(cacheItem);
      lruList.AddLast(node);
      cacheMap.Add(key, node);
    }

    void RemoveFirst()
    {
      LinkedListNode<LRUCacheItem> node = lruList.First;
      if (lruList.First != null)
      {
        lruList.RemoveFirst();
        cacheMap.Remove(node.Value.key);
      }
    }
  }
}