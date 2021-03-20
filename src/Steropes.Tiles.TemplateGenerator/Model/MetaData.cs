using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Steropes.Tiles.TemplateGenerator.Model
{
    public static class MetaData
    {
        public static IReadOnlyDictionary<string, string> Updated(this IReadOnlyDictionary<string, string> original, string key, string value)
        {
            var d = CreateDictionary(original);
            d[key] = value;
            return new ReadOnlyDictionary<string, string>(d);
        }

        public static IReadOnlyDictionary<string, string> Removed(this IReadOnlyDictionary<string, string> original, string key)
        {
            if (!original.ContainsKey(key))
            {
                return original;
            }

            var d = CreateDictionary(original);
            d.Remove(key);
            return original;
        }

        static Dictionary<string, string> CreateDictionary(IReadOnlyDictionary<string, string> original)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            foreach (var pair in original)
            {
                d[pair.Key] = pair.Value;
            }

            return d;
        }
    }

    public interface IFormattingInfoProvider
    {
        FormattingMetaData FormattingMetaData { get; }
    }
}
