using System;
using System.Collections.Generic;

namespace Steropes.Tiles.Sample.Shared.Util
{
    public interface ITypeRegistry<out T> : IEnumerable<T>
    {
        T DefaultValue { get; }
        T this[int idx] { get; }
        int Count { get; }
    }

    public static class TypeRegistryExtensions
    {
        public static int IndexOf<T>(this ITypeRegistry<T> reg, T data)
        {
            int idx = 0;
            foreach (var v in reg)
            {
                if (Equals(v, data))
                {
                    return idx;
                }

                idx += 1;
            }

            return -1;
        }

        public static Dictionary<char, byte> ToIndexDict<T>(this IEnumerable<T> reg, Func<T, char> keyFn)
        {
            var byCharId = new Dictionary<char, byte>();
            var idx = 0;
            foreach (var roadType in reg)
            {
                byCharId.Add(keyFn(roadType), (byte)idx);
                idx += 1;
                if (idx > 255)
                {
                    throw new ArgumentException("Too many terrain types in this registry.");
                }
            }

            return byCharId;
        }

        public static Dictionary<char, T> ToDict<T>(this IEnumerable<T> reg, Func<T, char> keyFn)
        {
            return ToDict(reg, keyFn, t => t);
        }

        public static Dictionary<char, TResult> ToDict<T, TResult>(this IEnumerable<T> reg, Func<T, char> keyFn, Func<T, TResult> r)
        {
            var byCharId = new Dictionary<char, TResult>();
            var idx = 0;
            foreach (var roadType in reg)
            {
                byCharId.Add(keyFn(roadType), r(roadType));
                idx += 1;
                if (idx > 255)
                {
                    throw new ArgumentException("Too many terrain types in this registry.");
                }
            }

            return byCharId;
        }
    }
}
