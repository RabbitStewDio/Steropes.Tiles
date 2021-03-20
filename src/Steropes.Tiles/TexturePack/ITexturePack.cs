using System.Collections.Generic;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.TexturePack
{
    /// <summary>
    ///  A texture pack contains the sum of all textures for a given game. The
    ///  textures are provided as one or more texture files.
    /// </summary>
    public interface ITexturePack<TTile>
    {
        IEnumerable<TTile> Tiles { get; }
        IntDimension TileSize { get; }
        TextureType TextureType { get; }
    }
}