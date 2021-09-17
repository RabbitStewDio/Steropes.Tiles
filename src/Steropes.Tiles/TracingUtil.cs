using System;

namespace Steropes.Tiles
{
    public static class TracingUtil
    {
        internal static string NameWithoutGenerics(Type t)
        {
            var name = t.FullName ?? t.Name;
            var index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }
    }
}
