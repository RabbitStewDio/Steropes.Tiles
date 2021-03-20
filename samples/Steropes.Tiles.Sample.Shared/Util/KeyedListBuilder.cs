using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Steropes.Tiles.Demo.Core.Util
{
    /// <summary>
    ///  A small helper class to make it a bit less verbose to 
    ///  assemble grouped dictionaries (key to list of values).
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyedListBuilder<TKey, TValue>
    {
        readonly Dictionary<TKey, List<TValue>> backend;

        public KeyedListBuilder()
        {
            backend = new Dictionary<TKey, List<TValue>>();
        }

        public void AddRange(TKey k, IEnumerable<TValue> v)
        {
            foreach (var value in v)
            {
                Add(k, value);
            }
        }

        public void Add(TKey k, TValue v)
        {
            if (!backend.TryGetValue(k, out List<TValue> vs))
            {
                vs = new List<TValue>();
                backend.Add(k, vs);
            }

            vs.Add(v);
        }

        public IReadOnlyDictionary<TKey, IReadOnlyList<TValue>> Build()
        {
            var d = new Dictionary<TKey, IReadOnlyList<TValue>>();
            foreach (var pair in backend)
            {
                d.Add(pair.Key, new List<TValue>(pair.Value).AsReadOnly());
            }

            return new ReadOnlyDictionary<TKey, IReadOnlyList<TValue>>(d);
        }
    }
}
