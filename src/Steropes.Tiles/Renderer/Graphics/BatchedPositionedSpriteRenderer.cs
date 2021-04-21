using System;
using System.Collections.Generic;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Renderer.Graphics
{
    /// <summary>
    ///    A render callback that correctly handles z-order in free moving
    ///    sprites (like NPCs and items placed freely on the map without
    ///    adhering to the grid). 
    /// </summary>
    /// <typeparam name="TRenderTile"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class BatchedPositionedSpriteRenderer<TRenderTile, TContext> : IRenderCallback<TRenderTile, TContext>
    {
        readonly IRenderCallback<TRenderTile, TContext> parent;
        int batchCounter;
        double batchOffset;
        BatchBuffer<PositionedSprite> spritesPostOffset;
        BatchBuffer<PositionedSprite> spritesPreOffset;
        readonly Action<PositionedSprite> drawDelegate;

        public BatchedPositionedSpriteRenderer(IRenderCallback<TRenderTile, TContext> parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            spritesPreOffset = new BatchBuffer<PositionedSprite>(new PositionedSpriteComparer());
            spritesPostOffset = new BatchBuffer<PositionedSprite>(new PositionedSpriteComparer());
            drawDelegate = DrawTile;
        }

        public void StartDrawing()
        {
            parent.StartDrawing();

            spritesPreOffset.Clear();
            spritesPostOffset.Clear();
            batchCounter = 0;
        }

        public void StartLine(int line, in ContinuousViewportCoordinates screenPos)
        {
            parent.StartLine(line, screenPos);
            batchOffset = screenPos.Y;
        }

        public void EndLine(int line, in  ContinuousViewportCoordinates screenPos)
        {
            spritesPreOffset.Consume(drawDelegate);
            spritesPreOffset.Clear();

            Swap(ref spritesPreOffset, ref spritesPostOffset);

            parent.EndLine(line, screenPos);
        }

        public void Draw(TRenderTile tile, TContext context, SpritePosition pos, in ContinuousViewportCoordinates coords)
        {
            var posSprite = new PositionedSprite(tile, context, pos, coords, batchCounter);
            batchCounter += 1;

            if (posSprite.Gy < batchOffset)
            {
                spritesPreOffset.Add(posSprite);
            }
            else
            {
                spritesPostOffset.Add(posSprite);
            }
        }

        public void FinishedDrawing()
        {
            spritesPreOffset.Consume(drawDelegate);
            spritesPreOffset.Clear();
            spritesPostOffset.Consume(drawDelegate);
            spritesPostOffset.Clear();

            parent.FinishedDrawing();
        }

        void DrawTile(PositionedSprite sprite)
        {
            parent.Draw(sprite.Tile, sprite.Context, sprite.Pos, sprite.Coordinates);
        }


        void Swap<T>(ref T one, ref T two)
        {
            var tmp = one;
            one = two;
            two = tmp;
        }

        class PositionedSpriteComparer : IComparer<PositionedSprite>
        {
            public int Compare(PositionedSprite x, PositionedSprite y)
            {
                return x.CompareTo(y);
            }
        }

        readonly struct PositionedSprite : IComparable<PositionedSprite>
        {
            public readonly TRenderTile Tile;
            public readonly TContext Context;
            public readonly SpritePosition Pos;
            public readonly ContinuousViewportCoordinates Coordinates;
            readonly int renderOrder;
            readonly double gx;
            public readonly double Gy;

            public PositionedSprite(TRenderTile tile,
                                    TContext context,
                                    SpritePosition pos,
                                    ContinuousViewportCoordinates coordinates,
                                    int renderOrder)
            {
                this.Tile = tile;
                this.Context = context;
                this.Pos = pos;
                this.Coordinates = coordinates;
                this.renderOrder = renderOrder;

                gx = coordinates.X;
                Gy = coordinates.Y;
            }

            public int CompareTo(PositionedSprite other)
            {
                var cmp = Gy.CompareTo(other.Gy);
                if (cmp != 0)
                {
                    return cmp;
                }

                cmp = gx.CompareTo(other.gx);
                if (cmp != 0)
                {
                    return cmp;
                }

                return renderOrder.CompareTo(other.renderOrder);
            }
        }
    }
}
