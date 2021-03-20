using System;
using System.Collections;
using System.Collections.Generic;

namespace Steropes.Tiles.Demo.Core.Util
{
    public class TraitCollection<T> : IEnumerable<T>
        where T : class
    {
        readonly List<T> contents;

        public TraitCollection()
        {
            contents = new List<T>();
        }

        public void Add(object o)
        {
            if (!(o is T))
            {
                return;
            }

            var t = (T)o;
            contents.Add(t);
        }

        public void Remove(object o)
        {
            if (!(o is T))
            {
                return;
            }

            for (var i = 0; i < contents.Count; i++)
            {
                if (ReferenceEquals(o, contents[i]))
                {
                    contents.RemoveAt(i);
                    return;
                }
            }
        }

        int FindByRef(object o)
        {
            for (var i = 0; i < contents.Count; i++)
            {
                if (ReferenceEquals(o, contents[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public void ForAll<TContext>(Action<T, TContext> a, TContext context)
        {
            for (var i = 0; i < contents.Count; i++)
            {
                a(contents[i], context);
            }
        }

        public List<T>.Enumerator GetEnumerator() => contents.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
