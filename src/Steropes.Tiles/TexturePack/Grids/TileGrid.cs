using System;
using System.Collections;
using System.Collections.Generic;

namespace Steropes.Tiles.TexturePack.Grids
{
    /// <summary>
    ///   Related to GridTextureFile
    /// </summary>
    public class TileGrid : IEnumerable<GridTileDefinition>
    {
        public TileGrid(int tileWidth,
                        int tileHeight,
                        int offsetX,
                        int offsetY,
                        int anchorX,
                        int anchorY,
                        params GridTileDefinition[] tiles) :
            this(tileWidth, tileHeight, offsetX, offsetY, anchorX, anchorY, 0, 0, tiles)
        { }

        public TileGrid(int tileWidth,
                        int tileHeight,
                        int offsetX,
                        int offsetY,
                        int anchorX,
                        int anchorY,
                        int borderX,
                        int borderY,
                        params GridTileDefinition[] tiles)
        {
            Tiles = new List<GridTileDefinition>(tiles);
            TilesGroups = new List<GridTileDefinition>();
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            OffsetX = offsetX;
            OffsetY = offsetY;
            AnchorX = anchorX;
            AnchorY = anchorY;
            BorderX = borderX;
            BorderY = borderY;
        }

        public List<GridTileDefinition> Tiles { get; }
        public List<GridTileDefinition> TilesGroups { get; }
        public int TileWidth { get; }
        public int TileHeight { get; }
        public int OffsetX { get; }
        public int OffsetY { get; }
        public int AnchorX { get; }
        public int AnchorY { get; }
        public int BorderX { get; }
        public int BorderY { get; }

        public void Add(GridTileDefinition tile)
        {
            Tiles.Add(tile ?? throw new ArgumentNullException(nameof(tile)));
        }

        public IEnumerator<GridTileDefinition> GetEnumerator()
        {
            return Tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
