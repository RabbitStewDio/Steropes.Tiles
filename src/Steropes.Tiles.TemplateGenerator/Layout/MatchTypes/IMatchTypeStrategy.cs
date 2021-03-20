using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
    public interface IMatchTypeStrategy
    {
        Size GetTileArea(TextureGrid g);

        List<TextureTile> Generate(TextureGrid grid);
    }
}
