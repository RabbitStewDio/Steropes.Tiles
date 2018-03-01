using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Matcher
{
    /// <summary>
    ///  A callback that is invoked whenever a tile has been matched. This avoids 
    ///  the complexities of having to deal with multiple tiles per match and how
    ///  to represent them. 
    /// </summary>
    /// <typeparam name="TRenderTile">The texture or tile that represents the rendering.</typeparam>
    /// <typeparam name="TContext">Tile context. Additional information that may be neccessary for rendering.</typeparam>
    public delegate void TileResultCollector<in TRenderTile, in TContext>(SpritePosition pos, TRenderTile result, TContext context);
}