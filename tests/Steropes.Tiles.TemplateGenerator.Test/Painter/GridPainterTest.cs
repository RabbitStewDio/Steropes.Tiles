using Avalonia.Media;
using FluentAssertions;
using NUnit.Framework;
using SkiaSharp;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.TemplateGen.Models;
using Steropes.Tiles.TemplateGen.Models.Prefs;
using Steropes.Tiles.TemplateGen.Models.Rendering;
using Steropes.Tiles.TemplateGen.Models.Rendering.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

#pragma warning disable 618

namespace Steropes.Tiles.TemplateGenerator.Test.Painter
{
    public class GridPainterTest
    {
        readonly GeneratorPreferences prefs = new GeneratorPreferences();
        readonly Color background = Colors.Blue;
        readonly Color border = Colors.Red;
        readonly Color outline = Colors.Gray;

        public readonly struct TextureSetFixture
        {
            public readonly TextureSetFile File;
            public readonly TileTextureCollection Collection;
            public readonly TextureGrid Grid;

            public TextureSetFixture(TextureSetFile file, TileTextureCollection collection, TextureGrid grid)
            {
                File = file;
                Collection = collection;
                Grid = grid;
            }

            public void Deconstruct(out TextureSetFile file, out TileTextureCollection collection, out TextureGrid grid)
            {
                file = File;
                collection = Collection;
                grid = Grid;
            }
        }

        public TextureSetFixture CreateTextureSet(int width, int height, TileType tileType)
        {
            var textureSet = new TextureSetFile
            {
                Width = width,
                Height = height,
                TileType = tileType
            };

            var gridParent = new TileTextureCollection
            {
                Parent = textureSet
            };

            var grid = new TextureGrid
            {
                MatcherType = MatcherType.Basic
            };
            
            grid.Tiles.Add(new TextureTile(false, 0, 0));
            grid.Tiles.Add(new TextureTile(false, 1, 0));
            grid.FormattingMetaData.Border = 1;
            grid.FormattingMetaData.BorderColor = border;
            grid.FormattingMetaData.Padding = 1;
            grid.FormattingMetaData.BackgroundColor = background;
            grid.TextureTileFormattingMetaData.TileOutlineColor = outline;
            grid.CellSpacing = 1;

            gridParent.Grids.Add(grid);
            return new TextureSetFixture(textureSet, gridParent, grid);
        }

        [Test]
        public void ValidateBitmapSize()
        {
            var p = new TextureCollectionPainter(prefs);
            var (_, gridParent, _) = CreateTextureSet(8, 4, TileType.Isometric);
            using var btmp = p.CreateBitmap(gridParent);
            btmp.Width.Should().Be(16 + 5);
            btmp.Height.Should().Be(4 + 4);
        }

        [Test]
        public void ValidateBitmapBorder()
        {
            var p = new TextureCollectionPainter(prefs);
            var (_, gridParent, _) = CreateTextureSet(8, 4, TileType.Isometric);
            var btmp = p.CreateBitmap(gridParent);

            var stream = btmp.Write();
            File.WriteAllBytes("g:/tmp.png", stream.ToArray());

            btmp.Width.Should().Be(16 + 5);
            btmp.Height.Should().Be(4 + 4);

            var pixMap = btmp.PeekPixels();

            pixMap.GetPixelColor(0, 5).Should().BeSameColor(border);
            pixMap.GetPixelColor(btmp.Width - 1, 5).Should().BeSameColor(border);
            pixMap.GetPixelColor(5, 0).Should().BeSameColor(border);
            pixMap.GetPixelColor(5, btmp.Height - 1).Should().BeSameColor(border);
        }

        [Test]
        public void ValidateNoOverdrawEvenWidth()
        {
            var p = new TextureCollectionPainter(prefs);
            var (_, gridParent, _) = CreateTextureSet(8, 4, TileType.Isometric);
            var btmp = p.CreateBitmap(gridParent);
            var pixMap = btmp.PeekPixels();

            var asText = Print(pixMap);
            Console.WriteLine(asText);

            btmp.Width.Should().Be(16 + 5);
            btmp.Height.Should().Be(4 + 4);

            var goldenSample = @"
ccccccccccccccccccccc
c...................c
c.##dddd##.##dddd##.c
c.dd####dd.dd####dd.c
c.dd####dd.dd####dd.c
c.##dddd##.##dddd##.c
c...................c
ccccccccccccccccccccc
".Replace("\r\n", "\n")
 .TrimStart();

            asText.Should().Be(goldenSample);
        }

        [Test]
        public void ValidateHighlightPositions()
        {
            var (_, _, grid) = CreateTextureSet(32, 16, TileType.Isometric);
            grid.MatcherType = MatcherType.CardinalFlags;
            grid.Tiles[0].SelectorHint = "0101";
            grid.Tiles[1].SelectorHint = "1010";

            var painter = new IsoTilePainter(prefs, grid);
            var area = painter.GetTileArea(grid.Tiles[0]);
            var baseShape = IsoTilePainter.CreateShape(area);
            baseShape.GetHighlightFor(NeighbourIndex.North).Should().BeEquivalentTo(new IntPoint(15, 0), new IntPoint(30, 7));
            baseShape.GetHighlightFor(NeighbourIndex.West).Should().BeEquivalentTo(new IntPoint(1, 7), new IntPoint(16, 0));

            var highlight = baseShape.ToHighlight();
            highlight.GetHighlightFor(NeighbourIndex.North).Should().BeEquivalentTo(new IntPoint(15, 2), new IntPoint(26, 7));
            highlight.GetHighlightFor(NeighbourIndex.West).Should().BeEquivalentTo(new IntPoint(5, 7), new IntPoint(16, 2));
        }

        [Test]
        public void ValidateHighlightsEvenWidth_Cardinals_Iso()
        {
            var p = new TextureCollectionPainter(prefs);
            var (_, gridParent, grid) = CreateTextureSet(16, 8, TileType.Isometric);
            grid.MatcherType = MatcherType.CardinalFlags;
            grid.Tiles[0].SelectorHint = "0101";
            grid.Tiles[1].SelectorHint = "1010";

            var btmp = p.CreateBitmap(gridParent);
            var pixMap = btmp.PeekPixels();

            var asText = Print(pixMap);
            Console.WriteLine(asText);

            var goldenSample = @"
ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc
c.....................................................................c
c.######dddd######.######dddd######...................................c
c.####dd####dd####.####dd####dd####...................................c
c.##dd###ee###dd##.##dd###ee###dd##...................................c
c.dd###ee#######dd.dd#######ee###dd...................................c
c.dd#######ee###dd.dd###ee#######dd...................................c
c.##dd###ee###dd##.##dd###ee###dd##...................................c
c.####dd####dd####.####dd####dd####...................................c
c.######dddd######.######dddd######...................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc
".Replace("\r\n", "\n")
 .TrimStart();

            asText.Should().Be(goldenSample);
        }

        [Test]
        public void ValidateNoOverdrawOddWidth_Iso()
        {
            var p = new TextureCollectionPainter(prefs);
            var (set, gridParent, grid) = CreateTextureSet(7, 5, TileType.Isometric);

            set.Width = 7;
            set.Height = 5;
            grid.CellWidth = 10;
            grid.CellHeight = 10;
            
            var btmp = p.CreateBitmap(gridParent);
            btmp.Width.Should().Be(20 + 5);
            btmp.Height.Should().Be(10 + 4);
            var pixMap = btmp.PeekPixels();

            var asText = Print(pixMap);
            Console.WriteLine(asText);

            var goldenSample = @"
ccccccccccccccccccccccccc
c.......................c
c.....d..........d......c
c...dd.dd......dd.dd....c
c..d.....d....d.....d...c
c..ddd.ddd....ddd.ddd...c
c..d..d..d....d..d..d...c
c..d##d##d....d##d##d...c
c..ddddddd....ddddddd...c
c..d##d##d....d##d##d...c
c..#ddddd#....#ddddd#...c
c..###d###....###d###...c
c.......................c
ccccccccccccccccccccccccc
".Replace("\r\n", "\n")
 .TrimStart();
            /*             
                         "ccccccccccccccccccccccccc\n" +
                         "c.......................c\n" +
                         "c.....d..........d......c\n" +
                         "c...dd.dd......dd.dd....c\n" +
                         "c..d.....d....d.....d...c\n" +
                         "c..ddd.ddd....ddd.ddd...c\n" +
                         "c..d..d..d....d..d..d...c\n" +
                         "c..d##d##d....d##d##d...c\n" +
                         "c..ddddddd....ddddddd...c\n" +
                         "c..d##d##d....d##d##d...c\n" +
                         "c..#ddddd#....#ddddd#...c\n" +
                         "c..###d###....###d###...c\n" +
                         "c.......................c\n" +
                         "ccccccccccccccccccccccccc\n";
         */
            asText.Should().Be(goldenSample);
        }

        [Test]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public void ValidateHighlightsEvenWidth_Cardinals_Grid()
        {
            var p = new TextureCollectionPainter(prefs);
            var (_, gridParent, grid) = CreateTextureSet(16, 8, TileType.Grid);
            grid.MatcherType = MatcherType.CardinalFlags;
            grid.Tiles.Add(new TextureTile(false, 2, 0));
            grid.Tiles.Add(new TextureTile(false, 3, 0));
            grid.Tiles[0].SelectorHint = "0101";
            grid.Tiles[1].SelectorHint = "1010";
            grid.Tiles[2].SelectorHint = "1100";
            grid.Tiles[3].SelectorHint = "1011";

            var btmp = p.CreateBitmap(gridParent);
            var pixMap = btmp.PeekPixels();

            var asText = Print(pixMap);
            Console.WriteLine(asText);

            var goldenSample = @"
ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc
c.....................................................................c
c.dddddddddddddddd.dddddddddddddddd.dddddddddddddddd.dddddddddddddddd.c
c.d##############d.d##############d.d##############d.d##############d.c
c.d#e##########e#d.d#eeeeeeeeeeee#d.d#eeeeeeeeeeee#d.d#eeeeeeeeeeee#d.c
c.d#e##########e#d.d##############d.d############e#d.d#e############d.c
c.d#e##########e#d.d##############d.d############e#d.d#e############d.c
c.d#e##########e#d.d#eeeeeeeeeeee#d.d############e#d.d#eeeeeeeeeeee#d.c
c.d##############d.d##############d.d##############d.d##############d.c
c.dddddddddddddddd.dddddddddddddddd.dddddddddddddddd.dddddddddddddddd.c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
c.....................................................................c
ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc
".Replace("\r\n", "\n")
 .TrimStart();

            asText.Should().Be(goldenSample);
        }

        [Test]
        public void ValidateNoOverdrawOddWidth_Grid()
        {
            var p = new TextureCollectionPainter(prefs);
            var (_, gridParent, grid) = CreateTextureSet(7, 5, TileType.Grid);

            grid.CellWidth = 10;
            grid.CellHeight = 10;

            const int borderSize = 2;
            const int paddingSize = 2;
            const int spacing = 1;
            
            var btmp = p.CreateBitmap(gridParent);
            btmp.Width.Should().Be(20 + borderSize + paddingSize + spacing);
            btmp.Height.Should().Be(10 + borderSize + paddingSize);
            var pixMap = btmp.PeekPixels();

            var asText = Print(pixMap);
            Console.WriteLine(asText);

            var goldenSample = @"
ccccccccccccccccccccccccc
c.......................c
c.......................c
c.......................c
c.......................c
c.......................c
c.......................c
c..ddddddd....ddddddd...c
c..d#####d....d#####d...c
c..d#####d....d#####d...c
c..d#####d....d#####d...c
c..ddddddd....ddddddd...c
c.......................c
ccccccccccccccccccccccccc
".Replace("\r\n", "\n")
 .TrimStart();
            asText.Should().Be(goldenSample);
        }

        public string Print(SKPixmap b)
        {
            var colorMappings = new Dictionary<uint, char>
            {
                [0] = '.',
                [background.ToUint32()] = '#'
            };

            var sb = new StringBuilder();
            for (var y = 0; y < b.Height; y += 1)
            {
                for (var x = 0; x < b.Width; x += 1)
                {
                    var c = (uint)b.GetPixelColor(x, y);
                    if (colorMappings.TryGetValue(c, out var p))
                    {
                        sb.Append(p);
                    }
                    else
                    {
                        var px = (char)('a' + colorMappings.Count);
                        colorMappings[c] = px;
                        sb.Append(px);
                    }
                }

                sb.Append("\n");
            }

            return sb.ToString();
        }

        [Test]
        public void ValidateCellSize()
        {
            var (_, _, grid) = CreateTextureSet(32, 16, TileType.Isometric);

            var painter = new IsoTilePainter(prefs, grid);
            var area = painter.GetTileArea(grid.Tiles[0]);
            area.Should().Be(new IntRect(0, 0, 32, 16));
        }

        [Test]
        public void ValidateIsoShape()
        {
            var (_, _, grid) = CreateTextureSet(32, 16, TileType.Grid);

            var painter = new IsoTilePainter(prefs, grid);
            var area = painter.GetTileArea(grid.Tiles[0]);
            var shape = IsoTilePainter.CreateShape(area);
            shape.Should().BeAssignableTo<PixelPerfectIsoShape>();

            shape.Top.Should().Be(new IntPoint(16, 0));
            shape.Left.Should().Be(new IntPoint(0, 8));
            shape.Right.Should().Be(new IntPoint(31, 8));
            shape.Bottom.Should().Be(new IntPoint(16, 15));
        }

        [Test]
        public void ValidateIsoShapeNonStandard()
        {
            var (_, _, grid) = CreateTextureSet(30, 16, TileType.Isometric);

            var painter = new IsoTilePainter(prefs, grid);
            var area = painter.GetTileArea(grid.Tiles[0]);
            var shape = IsoTilePainter.CreateShape(area);

            shape.Top.Should().Be(new IntPoint(15, 0));
            shape.Left.Should().Be(new IntPoint(0, 8));
            shape.Right.Should().Be(new IntPoint(29, 8));
            shape.Bottom.Should().Be(new IntPoint(15, 15));
        }
    }
}
