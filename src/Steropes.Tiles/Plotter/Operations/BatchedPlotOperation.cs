using System;
using System.Collections.Generic;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Renderer;
using Steropes.Tiles.Renderer.Graphics;

namespace Steropes.Tiles.Plotter.Operations
{
    /// <summary>
    ///  An aggregate operation that batches multiple renderer call into a single operation per map position. 
    ///  It is common that all operations contained in here share the same renderer.
    /// </summary>
    public class BatchedPlotOperation<TRenderTile, TContext> : IRenderPlotOperation<TRenderTile, TContext>
    {
        readonly List<IRenderPlotOperation<TRenderTile, TContext>> plotOperations;
        readonly BatchedSpriteRenderer<TRenderTile, TContext> activeRenderer;

        public BatchedPlotOperation(IRenderCallback<TRenderTile, TContext> renderer,
                                    params IRenderPlotOperation<TRenderTile, TContext>[] plots)
        {
            plotOperations = new List<IRenderPlotOperation<TRenderTile, TContext>>();
            activeRenderer = new BatchedSpriteRenderer<TRenderTile, TContext>();
            Renderer = renderer ?? throw new ArgumentNullException();

            foreach (var plot in plots)
            {
                Add(plot);
            }
        }

        public IRenderCallback<TRenderTile, TContext> ActiveRenderer => activeRenderer;

        public void Add(IRenderPlotOperation<TRenderTile, TContext> op)
        {
            op.Renderer = activeRenderer;
            plotOperations.Add(op);
        }

        public void StartDrawing()
        {
            for (var i = 0; i < plotOperations.Count; i++)
            {
                var op = plotOperations[i];
                op.StartDrawing();
            }
        }

        public void RenderAt(in MapCoordinate screenPosition, in MapCoordinate mapPosition)
        {
            for (var i = 0; i < plotOperations.Count; i++)
            {
                var op = plotOperations[i];
                op.RenderAt(screenPosition, mapPosition);
            }
        }

        public void FinishedDrawing()
        {
            for (var i = plotOperations.Count - 1; i >= 0; i--)
            {
                var op = plotOperations[i];
                op.FinishedDrawing();
            }
        }

        public void StartLine(int logicalLine, in MapCoordinate screenPos)
        {
            for (var i = 0; i < plotOperations.Count; i++)
            {
                var op = plotOperations[i];
                op.StartLine(logicalLine, screenPos);
            }
        }

        public void EndLine(int logicalLine, in MapCoordinate screenPos)
        {
            for (var i = plotOperations.Count - 1; i >= 0; i--)
            {
                var op = plotOperations[i];
                op.EndLine(logicalLine, screenPos);
            }
        }

        public IRenderCallback<TRenderTile, TContext> Renderer
        {
            get { return activeRenderer.Parent; }
            set
            {
                activeRenderer.Parent = value;
            }
        }

        public void Invalidate(in MapCoordinate mapPosition, int range)
        {
            for (var i = plotOperations.Count - 1; i >= 0; i--)
            {
                var op = plotOperations[i];
                op.Invalidate(mapPosition, range);
            }
        }

        public void InvalidateAll()
        {
            for (var i = plotOperations.Count - 1; i >= 0; i--)
            {
                var op = plotOperations[i];
                op.InvalidateAll();
            }
        }
    }
}
