using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TemplateGen.Models;
using Steropes.Tiles.TemplateGen.Models.Prefs;
using Steropes.Tiles.TemplateGen.Models.Rendering;
using System;

namespace Steropes.Tiles.TemplateGenerator.Test
{
    public class TextureGridAutoLayoutTest
    {
        TileTextureCollection collection;

        [SetUp]
        public void SetUp()
        {
            collection = new TileTextureCollection
            {
                Parent = new TextureSetFile()
                {
                    Width = 64,
                    Height = 32
                }
            };
        }

        [Test]
        public void Test4x4()
        {
            collection.Grids.Add(new TextureGrid
            {
                MatcherType = MatcherType.CardinalFlags,
                Name = "Grid-A"
            });
            collection.Grids.Add(new TextureGrid
            {
                MatcherType = MatcherType.CardinalFlags,
                Name = "Grid-B"
            });
            collection.Grids.Add(new TextureGrid
            {
                MatcherType = MatcherType.CardinalFlags,
                Name = "Grid-C"
            });
            collection.Grids.Add(new TextureGrid
            {
                MatcherType = MatcherType.CardinalFlags,
                Name = "Grid-D"
            });

            TextureTileGenerator.Regenerate(collection);
            TextureGridAutoLayout.ArrangeGrids(new GeneratorPreferences(), collection);

            collection.Grids[0].Position.Should().Be(new IntPoint());
            collection.Grids[1].Position.Should().Be(new IntPoint(0, 128));
            collection.Grids[2].Position.Should().Be(new IntPoint(0, 256));
            collection.Grids[3].Position.Should().Be(new IntPoint(256, 0));
        }

        [Test]
        public void TestSplitQuad()
        {
            var root = new ArrangeNode<string>(100, 100);
            root.Insert(new IntDimension(10, 10), "A");
            root.Insert(new IntDimension(10, 10), "B");
            root.Insert(new IntDimension(10, 10), "C");

            var ip = root.FindInsertPosition(new IntDimension(10, 10));
            ip.Content.Should().BeNull();
            ip.X.Should().Be(10);
            ip.Y.Should().Be(10);
        }

        [Test]
        public void TestSplitLandscape()
        {
            var root = new ArrangeNode<string>(100, 90);
            var split = root.Split(new IntDimension(10, 5));
            var left = split.Item1;
            var bottom = split.Item2;

            Console.WriteLine(root);
            Console.WriteLine("  left   = " + left);
            Console.WriteLine("  bottom = " + bottom);

            left.SpaceAvailableY.Should().Be(5);
            bottom.SpaceAvailableX.Should().Be(100);
        }

        [Test]
        public void TestSplitPortrait()
        {
            var root = new ArrangeNode<string>(90, 100);
            var split = root.Split(new IntDimension(10, 5));
            var left = split.Item1;
            var bottom = split.Item2;

            left.SpaceAvailableY.Should().Be(5);
            bottom.SpaceAvailableX.Should().Be(90);
        }

        [Test]
        public void TestSplitSquare()
        {
            var root = new ArrangeNode<string>(100, 100);
            var split = root.Split(new IntDimension(10, 5));
            var left = split.Item1;
            var bottom = split.Item2;

            left.SpaceAvailableY.Should().Be(5);
            bottom.SpaceAvailableX.Should().Be(100);
        }
    }
}
