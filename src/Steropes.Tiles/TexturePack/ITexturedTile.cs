using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.TexturePack
{
    /// <summary>
    /// A representation of a tile rendering operation. 
    /// <para>
    /// There is one instance for each tagged tile, possibly sharing underlying
    /// raw rendering data objects (ie textures).
    /// </para>
    /// </summary>
    public interface ITexturedTile<TTexture>
    {
        /// <summary>
        ///  Anchor is given in pixels.
        /// </summary>
        IntPoint Anchor { get; }

        bool HasTexture { get; }
        TTexture Texture { get; }
        string Tag { get; }
    }
}
