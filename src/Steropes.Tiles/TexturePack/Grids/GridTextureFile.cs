using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.TexturePack.Grids
{
    /// <summary>
    ///   A texture file that contains one ore more regular grids. All tiles
    ///   inside the grid have a uniform size and anchor point. FreeCiv uses
    ///   this schema for its graphics packs.
    ///
    ///   This is purely a data file. 
    /// </summary>
    public class GridTextureFile<TTile, TTexture, TRawTexture> : ITextureFile<TTile>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
        where TRawTexture : IRawTexture<TTexture>
    {
        readonly IntDimension tileSize;
        readonly ITileProducer<TTile, TTexture, TRawTexture> producer;
        readonly IContentLoader<TRawTexture> loader;

        public GridTextureFile(string name,
                               IntDimension tileSize,
                               [NotNull] ITileProducer<TTile, TTexture, TRawTexture> producer,
                               [NotNull] IContentLoader<TRawTexture> loader,
                               params TileGrid[] grids)
        {
            if (grids == null)
            {
                throw new ArgumentNullException(nameof(grids));
            }

            this.tileSize = tileSize;
            this.producer = producer ?? throw new ArgumentNullException(nameof(producer));
            this.loader = loader ?? throw new ArgumentNullException(nameof(loader));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Grids = new List<TileGrid>(grids);
        }

        public string Name { get; }
        public List<TileGrid> Grids { get; }

        public IEnumerable<TTile> ProduceTiles()
        {
            var texture = loader.LoadTexture(Name);
            foreach (var grid in Grids)
            {
                foreach (var tile in grid.Tiles)
                {
                    var tileWidth = grid.TileWidth;
                    var tileHeight = grid.TileHeight;
                    var tileX = grid.OffsetX + tile.GridX * (tileWidth + grid.BorderX);
                    var tileY = grid.OffsetY + tile.GridY * (tileHeight + grid.BorderY);

                    var tileBounds = new IntRect(tileX, tileY, tileWidth, tileHeight);
                    var anchor = new IntPoint(tile.AnchorX ?? grid.AnchorX, tile.AnchorY ?? grid.AnchorY);

                    foreach (var r in producer.ProduceAll(tileSize, tileBounds, anchor, tile.Tags, texture))
                    {
                        yield return r;
                    }
                }
            }
        }
    }
}
