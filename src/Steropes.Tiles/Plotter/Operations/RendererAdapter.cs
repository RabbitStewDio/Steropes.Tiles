using System;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter.Operations
{
    public class RendererAdapter<TRenderTile, TContext>
    {
        readonly MapToScreenMapper mapToScreenMapper;

        public RendererAdapter(RenderType renderType,
                               IRenderCallback<TRenderTile, TContext> renderer = null)
        {
            Renderer = renderer;
            mapToScreenMapper = ScreenCoordinateMapping.CreateMapToScreenMapper(renderType);
        }

        public IRenderCallback<TRenderTile, TContext> Renderer { get; set; }

        public ContinuousViewportCoordinates Screen { get; set; }

        public void NextTile(int x, int y)
        {
            Screen = mapToScreenMapper(x, y);
        }

        public void MatchFound(SpritePosition pos, TRenderTile result, TContext context)
        {
            if (result == null)
            {
                throw new ArgumentNullException();
            }

            Renderer.Draw(result, context, pos, Screen);
        }
    }
}
