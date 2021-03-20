using System;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Plotter.Operations;

namespace Steropes.Tiles
{
    public class TileMatchControl<TTexureTile, TContext>
    {
        public bool Cachable { get; }
        public ITileMatcher<TTexureTile, TContext> Matcher { get; }
        public Action<IPlotOperation> CacheControl { get; }

        public TileMatchControl(ITileMatcher<TTexureTile, TContext> matcher,
                                Action<IPlotOperation> cacheControl = null)
        {
            Matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
            Cachable = true;
            CacheControl = cacheControl ?? CacheControlNoOp;
        }

        public static void CacheControlNoOp(IPlotOperation op)
        { }
    }
}
