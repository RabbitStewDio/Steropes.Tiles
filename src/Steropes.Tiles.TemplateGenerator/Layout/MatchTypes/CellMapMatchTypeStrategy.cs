using System;
using System.Collections.Generic;
using System.Drawing;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;
using Steropes.Tiles.TemplateGenerator.Model;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
    public class CellMapMatchTypeStrategy : IMatchTypeStrategy
    {
        string FormatCellMapSelectorKey(CellMapTileSelectorKey k)
        {
            return $"{k.MatchA.Index},{k.MatchB.Index},{k.MatchC.Index},{k.MatchD.Index}";
        }

        public Size GetTileArea(TextureGrid grid)
        {
            var cellMapCount = Math.Max(1, grid.EffectiveCellMapElements.Count);
            var tileCount = Math.Pow(cellMapCount, 4);
            var width = (int)Math.Ceiling(Math.Sqrt(tileCount));
            var height = (int)Math.Ceiling(tileCount / width);
            return new Size(width, height);
        }

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var cellMapEntries = grid.EffectiveCellMapElements;
            if (cellMapEntries.Count <= 1)
            {
                cellMapEntries = new List<string>
                {
                    "0",
                    "1"
                };
            }

            var selectionFactory = TileTagEntrySelectionFactory.FromTags(cellMapEntries.ToArray());
            var selections = selectionFactory.ToSelectionArray();
            var tileCount = Math.Pow(selections.Length, 4);
            var width = (int)Math.Ceiling(Math.Sqrt(tileCount));
            var retval = new List<TextureTile>();
            foreach (var m4 in selections)
            {
                foreach (var m3 in selections)
                {
                    foreach (var m2 in selections)
                    {
                        foreach (var m1 in selections)
                        {
                            var key = new CellMapTileSelectorKey(m1, m2, m3, m4);
                            var tileName = key.Format(grid.Name, grid.Pattern);
                            var x = key.LinearIndex % width;
                            var y = key.LinearIndex / width;
                            retval.Add(new TextureTile(true, x, y, tileName)
                            {
                                SelectorHint = FormatCellMapSelectorKey(key)
                            });
                        }
                    }
                }
            }

            return retval;
        }
    }
}
