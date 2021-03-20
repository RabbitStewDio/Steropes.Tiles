using System;

namespace Steropes.Tiles.Matcher
{
    /// <summary>
    ///  A tile matcher that converts context information from one format to another.
    /// </summary>
    /// <typeparam name="TRenderTile"></typeparam>
    /// <typeparam name="TContextSource"></typeparam>
    /// <typeparam name="TContextTarget"></typeparam>
    public class TransformingTileMatcher<TRenderTile, TContextSource, TContextTarget> : TransformingTileMatcherBase<TRenderTile, TContextSource, TContextTarget>
    {
        readonly Func<int, int, TRenderTile, TContextSource, TContextTarget> converter;

        public TransformingTileMatcher(ITileMatcher<TRenderTile, TContextSource> parent,
                                       Func<int, int, TRenderTile, TContextSource, TContextTarget> converter) : base(parent)
        {
            this.converter = converter;
        }

        protected override TContextTarget Convert(int x, int y, TRenderTile tile, TContextSource source)
        {
            return converter(x, y, tile, source);
        }
    }
}
