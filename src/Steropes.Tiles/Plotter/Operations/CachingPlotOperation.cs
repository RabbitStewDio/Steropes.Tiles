using System;
using System.Collections.Generic;
using System.ComponentModel;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter.Operations
{
    public class CachingPlotOperation<TRenderTile, TContext> : IRenderPlotOperation<TRenderTile, TContext>
    {
        readonly IRenderPlotOperation<TRenderTile, TContext> parent;
        readonly RecordingRenderer recordingRenderer;

        public CachingPlotOperation(IRenderPlotOperation<TRenderTile, TContext> parent,
                                    IMapNavigator<GridDirection> mapNavigator,
                                    IMapRenderArea viewport)
        {
            this.recordingRenderer = new RecordingRenderer(viewport, mapNavigator, parent.ActiveRenderer);
            this.parent = parent;
            this.parent.Renderer = recordingRenderer;
        }

        public void StartDrawing()
        {
            parent.StartDrawing();
        }

        public IRenderCallback<TRenderTile, TContext> ActiveRenderer => recordingRenderer;

        public void RenderAt(MapCoordinate screenPosition, MapCoordinate mapPosition)
        {
            if (!recordingRenderer.Replay(mapPosition, screenPosition))
            {
                // cached version does not exist or is invalid. 
                // re-record ..
                recordingRenderer.Record(parent, screenPosition, mapPosition);
            }
        }

        public void FinishedDrawing()
        {
            parent.FinishedDrawing();
        }

        public void StartLine(int logicalLine, MapCoordinate screenPos)
        {
            parent.StartLine(logicalLine, screenPos);
        }

        public void EndLine(int logicalLine, MapCoordinate screenPos)
        {
            parent.EndLine(logicalLine, screenPos);
        }

        public IRenderCallback<TRenderTile, TContext> Renderer
        {
            get { return recordingRenderer.Renderer; }
            set { recordingRenderer.Renderer = value; }
        }

        public void Invalidate(MapCoordinate mapPosition, int range)
        {
            recordingRenderer.Invalidate(mapPosition, range);
        }

        public void InvalidateAll()
        {
            recordingRenderer.InvalidateAll();
        }

        /// <summary>
        ///  For testing support.
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static bool IsRecordingRenderer(IRenderCallback<TRenderTile, TContext> r)
        {
            return r is RecordingRenderer;
        }

        class RecordingRenderer : IRenderCallback<TRenderTile, TContext>
        {
            class RenderAtCall
            {
                readonly TRenderTile tile;
                readonly TContext context;
                readonly SpritePosition pos;

                public RenderAtCall(TRenderTile tile, TContext context, SpritePosition pos)
                {
                    this.tile = tile;
                    this.context = context;
                    this.pos = pos;
                }

                public void Replay(IRenderCallback<TRenderTile, TContext> parent, ContinuousViewportCoordinates vp)
                {
                    parent.Draw(tile, context, pos, vp);
                }
            }

            class RenderAtRecord
            {
                RenderAtCall[] Calls { get; }
                public int Epoch { get; set; }

                public RenderAtRecord(RenderAtCall[] calls, int epoch)
                {
                    Calls = calls ?? throw new ArgumentNullException(nameof(calls));
                    Epoch = epoch;
                }

                public void Replay(IRenderCallback<TRenderTile, TContext> parent, ContinuousViewportCoordinates vp)
                {
                    var calls = Calls;
                    for (var i = 0; i < calls.Length; i++)
                    {
                        calls[i].Replay(parent, vp);
                    }
                }

                public bool NoOp => Calls.Length == 0;
            }

            readonly IGridDictionary<RenderAtRecord> renderAtRecords;
            readonly List<RenderAtCall> recordingBuffer;
            readonly RendererAdapter<TRenderTile, TContext> adapter;
            readonly IMapRenderArea viewport;
            readonly IMapNavigator<GridDirection> navigator;
            readonly Func<RenderAtRecord, bool> expiredDelegate;
            int epoch;
            int tilesTouched;

            public RecordingRenderer(IMapRenderArea viewport,
                                     IMapNavigator<GridDirection> mapNavigator,
                                     IRenderCallback<TRenderTile, TContext> renderer)
            {
                this.viewport = viewport;
                this.navigator = mapNavigator;

                recordingBuffer = new List<RenderAtCall>();
                adapter = new RendererAdapter<TRenderTile, TContext>(viewport.RenderType);
                Renderer = renderer;

                renderAtRecords = new GridDictionary<RenderAtRecord>();
                renderAtRecords.UpdateBounds(viewport.CenterPointInMapCoordinates, viewport.RenderInsets);
                expiredDelegate = Expired;

                viewport.PropertyChanged += OnViewPropertyChanged;
            }

            void OnViewPropertyChanged(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == nameof(IMapRenderArea.CenterPointInMapCoordinates) ||
                    args.PropertyName == nameof(IMapRenderArea.RenderedArea))
                {
                    renderAtRecords.UpdateBounds(viewport.CenterPointInMapCoordinates, viewport.RenderInsets);
                }
            }

            public IRenderCallback<TRenderTile, TContext> Renderer { get; set; }

            public bool Replay(MapCoordinate mapPosition, MapCoordinate screenPosition)
            {
                if (renderAtRecords.TryGetValue(mapPosition, out RenderAtRecord r))
                {
                    if (!r.NoOp)
                    {
                        adapter.NextTile(screenPosition.X, screenPosition.Y);
                        r.Replay(Renderer, adapter.Screen);
                    }

                    cacheHit += 1;
                    r.Epoch = epoch;
                    tilesTouched += 1;
                    return true;
                }

                cacheMiss += 1;
                return false;
            }

            int cacheHit;
            int cacheMiss;

            public void StartDrawing()
            {
                tilesTouched = 0;
                epoch += 1;
                cacheHit = 0;
                cacheMiss = 0;
                Renderer.StartDrawing();
            }

            public void StartLine(int logicalLine, ContinuousViewportCoordinates screenY)
            {
                Renderer.StartLine(logicalLine, screenY);
            }

            public void Draw(TRenderTile tile, TContext context, SpritePosition pos, ContinuousViewportCoordinates vp)
            {
                recordingBuffer.Add(new RenderAtCall(tile, context, pos));
                Renderer.Draw(tile, context, pos, vp);
            }

            public void EndLine(int logicalLine, ContinuousViewportCoordinates screenY)
            {
                Renderer.EndLine(logicalLine, screenY);
            }

            public void FinishedDrawing()
            {
                Renderer.FinishedDrawing();
                RemoveObsolete();
            }

            void RemoveObsolete()
            {
                renderAtRecords.RemoveWhere(tilesTouched, expiredDelegate);
            }

            bool Expired(RenderAtRecord v)
            {
                return v.Epoch != epoch;
            }

            public void Record(IRenderPlotOperation<TRenderTile, TContext> parent,
                               MapCoordinate screenPosition,
                               MapCoordinate mapPosition)
            {
                recordingBuffer.Clear();
                adapter.NextTile(screenPosition.X, screenPosition.Y);
                parent.RenderAt(screenPosition, mapPosition);
                renderAtRecords.Store(mapPosition, new RenderAtRecord(recordingBuffer.ToArray(), epoch));
                tilesTouched += 1;
            }

            /// <summary>
            ///  Invalidates the current map position and all neighbours. This ensures that 
            ///  tile matchers that rely on neighbouring cells are also refreshed.
            /// </summary>
            /// <param name="mapPosition"></param>
            /// <param name="range"></param>
            public void Invalidate(MapCoordinate mapPosition, int range)
            {
                navigator.NavigateTo(GridDirection.NorthWest, mapPosition, out MapCoordinate ul, range);
                var size = range * 2 + 1;
                for (var x = 0; x < size; x += 1)
                {
                    var lp = ul;
                    for (var y = 0; y < size; y += 1)
                    {
                        renderAtRecords.Remove(lp);
                        navigator.NavigateTo(GridDirection.East, lp, out lp);
                    }

                    navigator.NavigateTo(GridDirection.South, ul, out ul);
                }
            }

            public void InvalidateAll()
            {
                renderAtRecords.RemoveWhere(0, r => true);
            }
        }
    }
}
