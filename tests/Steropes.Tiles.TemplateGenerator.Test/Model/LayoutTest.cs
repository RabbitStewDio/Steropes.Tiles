using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TemplateGen.Models;
using Steropes.Tiles.TemplateGen.Models.Prefs;
using Steropes.Tiles.TemplateGen.Models.Rendering;

namespace Steropes.Tiles.TemplateGenerator.Test.Model
{
    public class LayoutTest
    {
        readonly GeneratorPreferences prefs = new GeneratorPreferences();

        [Test]
        public void TextureGridEffectiveSize_When_Corner()
        {
            var g = new TextureGrid
            {
                Parent = new TileTextureCollection
                {
                    Parent = new TextureSetFile
                    {
                        Width = 64,
                        Height = 32,
                        TileType = TileType.Isometric
                    }
                },
                MatcherType = MatcherType.Corner
            };

            g.EffectiveTileSize.Should().Be(new IntDimension(32, 16));
            g.EffectiveCellSize.Should().Be(new IntDimension(32, 16));
        }

        [Test]
        public void TextureGridEffectiveSize_When_NotCorner()
        {
            var g = new TextureGrid
            {
                Parent = new TileTextureCollection
                {
                    Parent = new TextureSetFile
                    {
                        Width = 64,
                        Height = 32,
                        TileType = TileType.Isometric
                    }
                },
                MatcherType = MatcherType.Basic
            };

            g.EffectiveTileSize.Should().Be(new IntDimension(64, 32));
            g.EffectiveCellSize.Should().Be(new IntDimension(64, 32));
        }

        [Test]
        public void SizeValidation_WhenCorner()
        {
            var g = new TextureGrid
            {
                Parent = new TileTextureCollection
                {
                    Parent = new TextureSetFile
                    {
                        Width = 64,
                        Height = 32,
                        TileType = TileType.Isometric
                    }
                },
                MatcherType = MatcherType.Corner
            };

            var size = g.EffectiveCellSize;
            var node = new TextureGridLayoutNode(prefs, g);
            node.Size.Should().Be(new IntDimension(8 * size.Width, 4 * size.Height));
        }

        [Test]
        public void Size_With_Border()
        {
            var g = new TextureGrid
            {
                Parent = new TileTextureCollection
                {
                    Parent = new TextureSetFile
                    {
                        Width = 64,
                        Height = 32,
                        TileType = TileType.Isometric
                    }
                },
                MatcherType = MatcherType.Basic
            };
            g.Tiles.Add(new TextureTile(false, 0, 0));
            g.Tiles.Add(new TextureTile(false, 1, 0));
            g.FormattingMetaData.Border = 1;
            g.FormattingMetaData.Padding = 2;

            var node = new TextureGridLayoutNode(prefs, g);
            node.ContentSize.Should().Be(new IntDimension(64 * 2, 32), "tile size of 64x32");
            node.Size.Should().Be(new IntDimension((64 * 2) + (2 * 2) + (1 * 2), 32 + (2 * 2) + (1 * 2)), "tile size of 64x32 and a 1px border around the grid and 2px padding around the grid.");
        }

        [Test]
        public void Size_With_CellSpacing()
        {
            var g = new TextureGrid
            {
                Parent = new TileTextureCollection
                {
                    Parent = new TextureSetFile
                    {
                        Width = 64,
                        Height = 32,
                        TileType = TileType.Isometric
                    }
                },
                MatcherType = MatcherType.Basic
            };
            g.Tiles.Add(new TextureTile(false, 0, 0));
            g.Tiles.Add(new TextureTile(false, 1, 0));
            g.CellSpacing = 1;

            var node = new TextureGridLayoutNode(prefs, g);
            node.ContentSize.Should().Be(new IntDimension((64 * 2) + 1, 32), "tile size of 64x32 with 1px spacing");
            node.Size.Should().Be(new IntDimension((64 * 2) + 1, 32), "tile size of 64x32 and a 1px cell spacing between the two tiles.");
        }
    }
}
