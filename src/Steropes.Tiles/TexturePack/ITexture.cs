using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TexturePack.Operations;

namespace Steropes.Tiles.TexturePack
{
    /// <summary>
    ///  Represents raw renderable data, usually a texture atlas.
    ///  This interface only contains management data. Subclass the interface
    ///  to provide your own additional properties.
    ///
    ///  This texture type uses native coordinates (whatever your engine's native
    ///  coordinate may be. 
    /// </summary>
    public interface ITexture
    {
        string Name { get; }
        TextureCoordinateRect Bounds { get; }
        bool Valid { get; }
    }

    /// <summary>
    ///  Represents raw renderable data, usually a texture atlas.
    ///  This interface only contains management data. Subclass the interface
    ///  to provide your own additional properties.
    ///
    ///  This texture type uses grid coordinates. Grid coordinates have the origin
    ///  (0,0) at the upper left corner.
    /// </summary>
    public interface IRawTexture<TTexture> 
        where TTexture: ITexture
    {
        string Name { get; }
        IntRect Bounds { get; }
        bool Valid { get; }

        TTexture CreateSubTexture(string name, TextureCoordinateRect bounds);
    }
}