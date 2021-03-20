using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.TemplateGenerator.Layout;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Test
{
    public class GridGeneratorTest
    {
        TextureCollection collection;

        [SetUp]
        public void SetUp()
        {
            var textureCollection = new TextureCollection();

            var tf = new TextureFile
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

            var g = new GridGenerator();
            g.Regenerate(collection);

            grid.Tiles.Count.Should().Be(32);
        }
    }
}
