using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.TemplateGen.Models;
using Steropes.Tiles.TemplateGen.Models.Rendering;

namespace Steropes.Tiles.TemplateGenerator.Test
{
    public class GridGeneratorTest
    {
        TileTextureCollection collection;

        [SetUp]
        public void SetUp()
        {
            var textureCollection = new TileTextureCollection();

            var tf = new TextureSetFile
            {
                Width = 64,
                Height = 32,
                TileType = TileType.Grid
            };
            tf.Collections.Add(textureCollection);

            collection = textureCollection;
        }

        [Test]
        public void Corner_Generation()
        {
            var grid = new TextureGrid
            {
                CellSpacing = 5,
                MatcherType = MatcherType.Corner
            };
            collection.Grids.Add(grid);

            TextureTileGenerator.Regenerate(collection);

            grid.Tiles.Count.Should().Be(32);
        }
    }
}
