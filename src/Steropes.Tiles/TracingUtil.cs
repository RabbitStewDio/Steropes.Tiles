using System;
using System.Diagnostics;

namespace Steropes.Tiles
{
    public static class TracingUtil
    {
        public static TraceSource Create<T>()
        {
            var name = NameWithoutGenerics(typeof(T));
            return new TraceSource(name, SourceLevels.Warning);
        }

        internal static string NameWithoutGenerics(Type t)
        {
            var name = t.FullName ?? t.Name;
            var index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }
    }
}
