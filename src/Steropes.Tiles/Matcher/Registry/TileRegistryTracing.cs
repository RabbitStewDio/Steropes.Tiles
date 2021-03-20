using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Steropes.Tiles.Matcher.Registry
{
    public class TileRegistryTracing
    {
        TileRegistryTracing()
        { }

        public static readonly ILogAdapter MissingTilesTracer = LogProvider.CreateLogger<TileRegistryTracing>();

        public static void EmitMissingTileWarning(string tag)
        {
            MissingTilesTracer.Trace("Missing tile in registry for tag ({0})", tag);
        }

        public static void EmitMissingTileWarning(string tag, IReadOnlyList<string> alts)
        {
            if (alts == null || alts.Count == 0)
            {
                EmitMissingTileWarning(tag);
            }
            else if (MissingTilesTracer.IsTraceEnabled)
            {
                var t = string.Join(",", alts);
                MissingTilesTracer.Trace("Missing tile in registry for tag ({0}) or alternative tags {1}", tag, t);
            }
        }
    }
}
