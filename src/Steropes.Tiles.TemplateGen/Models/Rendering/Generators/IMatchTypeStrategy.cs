using Steropes.Tiles.DataStructures;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Generators
{
    public interface IMatchTypeStrategy
    {
        IntDimension GetTileArea(TextureGrid g);

        List<TextureTile> Generate(TextureGrid grid);
    }
}
