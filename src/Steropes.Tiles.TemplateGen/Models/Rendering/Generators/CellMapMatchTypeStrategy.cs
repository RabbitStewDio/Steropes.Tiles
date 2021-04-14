using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Steropes.Tiles.TemplateGen.Models.Rendering.Generators
{
    public class CellMapMatchTypeStrategy : IMatchTypeStrategy
    {
        string FormatCellMapSelectorKey(CellMapTileSelectorKey k)
        {
            return $"{k.MatchA.Tag} {k.MatchB.Tag} {k.MatchC.Tag} {k.MatchD.Tag}";
        }

        public IntDimension GetTileArea(TextureGrid grid)
        {
            var cellMapCount = Math.Max(1, grid.CellMappings.Count);
            var tileCount = Math.Pow(cellMapCount, 4);
            var width = (int)Math.Ceiling(Math.Sqrt(tileCount));
            var height = (int)Math.Ceiling(tileCount / width);
            return new IntDimension(width, height);
        }

        public List<TextureTile> Generate(TextureGrid grid)
        {
            var cellMapEntries = grid.CellMappings.Where(e => !string.IsNullOrWhiteSpace(e.Key)).ToList();
            if (cellMapEntries.Count <= 1)
            {
                return new List<TextureTile>();
            }

            var selectionFactory = TileTagEntrySelectionFactory.FromTagsAsTextKey(cellMapEntries.Select(e => e.Key ?? "").ToArray());
            var selections = selectionFactory.ToSelectionArray();
            var tileCount = Math.Pow(selections.Length, 4);
            var width = (int)Math.Ceiling(Math.Sqrt(tileCount));

            TextureTile Create(ITileTagEntrySelection m1,
                               ITileTagEntrySelection m2,
                               ITileTagEntrySelection m3,
                               ITileTagEntrySelection m4)
            {
                var key = new CellMapTileSelectorKey(m1, m2, m3, m4);
                var tileName = key.Format(grid.Name, grid.Pattern);
                var x = key.LinearIndex % width;
                var y = key.LinearIndex / width;
                
                return new TextureTile(true, x, y, tileName)
                {
                    SelectorHint = FormatCellMapSelectorKey(key)
                };
            }

            var x = from m4 in selections
                    from m3 in selections
                    from m2 in selections
                    from m1 in selections
                    select Create(m1, m2, m3, m4);

            return x.ToList();
        }
    }
}
