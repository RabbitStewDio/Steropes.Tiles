using System;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Renderer.Graphics
{
    /// <summary>
    ///  Used internally in conjunction with the BatchedPlotOperation.
    /// </summary>
    /// <typeparam name="TRenderTile"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class BatchedSpriteRenderer<TRenderTile, TContext> : IRenderCallback<TRenderTile, TContext>
    {
        IRenderCallback<TRenderTile, TContext> parent;
        int childCount;
        int startLineCount;

        public BatchedSpriteRenderer(IRenderCallback<TRenderTile, TContext> parent = null)
        {
            this.parent = parent;
        }

        public IRenderCallback<TRenderTile, TContext> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public void StartDrawing()
        {
            if (parent == null)
            {
                throw new InvalidOperationException("No parent renderer set for batched sprite rendering. Check your set-up code.");
            }

            if (childCount == 0)
            {
                parent.StartDrawing();
                startLineCount = 0;
            }

            childCount += 1;
        }

        public void StartLine(int logicalLine, ContinuousViewportCoordinates screen)
        {
            if (startLineCount == 0)
            {
                parent.StartLine(logicalLine, screen);
            }

            startLineCount += 1;
        }

        public void Draw(TRenderTile tile, TContext context, SpritePosition pos, ContinuousViewportCoordinates screenLocation)
        {
            parent.Draw(tile, context, pos, screenLocation);
        }

        public void EndLine(int logicalLine, ContinuousViewportCoordinates screen)
        {
            if (startLineCount == 0)
            {
                throw new InvalidOperationException("Cannot call EndLine without first calling StartLine");
            }

            startLineCount -= 1;

            if (startLineCount == 0)
            {
                parent.EndLine(logicalLine, screen);
            }
        }

        public void FinishedDrawing()
        {
            if (childCount == 0)
            {
                throw new InvalidOperationException("Cannot call FinishedDrawing without first calling StartDrawing");
            }

            childCount -= 1;

            if (childCount == 0)
            {
                parent.FinishedDrawing();
            }
        }
    }
}
