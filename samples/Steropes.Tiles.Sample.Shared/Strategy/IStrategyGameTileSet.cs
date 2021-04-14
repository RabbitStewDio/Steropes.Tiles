using Steropes.Tiles.DataStructures;
using System.Collections.Generic;

namespace Steropes.Tiles.Sample.Shared.Strategy
{
    public interface IStrategyGameTileSet
    {
        IntDimension TileSize { get; }
        RenderType RenderType { get; }
        int Layers { get; }
        int BlendLayer { get; }

        bool TryGetTerrain(string tag, out TerrainGraphic g);
        IEnumerable<TerrainGraphic> TerrainsForLayer(int layer);
    }
}
