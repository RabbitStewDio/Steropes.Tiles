using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Demo.TextMode
{
    public class ClippingRenderer : IRenderCallback<TextTile, Nothing>
    {
        readonly IntRect clipRect;
        readonly IRenderCallback<TextTile, Nothing> parent;

        public ClippingRenderer(IRenderCallback<TextTile, Nothing> parent, IntRect clipRect)
        {
            this.parent = parent;
            this.clipRect = clipRect;
        }

        public void StartDrawing()
        {
            parent.StartDrawing();
        }

        public void StartLine(int line, in ContinuousViewportCoordinates screenPos)
        {
            parent.StartLine(line, screenPos);
        }

        public void EndLine(int line, in ContinuousViewportCoordinates screenPos)
        {
            parent.EndLine(line, screenPos);
        }

        public void FinishedDrawing()
        {
            parent.FinishedDrawing();
        }

        public void Draw(TextTile tile, Nothing context, SpritePosition pos, in ContinuousViewportCoordinates c)
        {
            var x = c.X;
            var y = c.Y;
            if (x < clipRect.X || x > clipRect.X + clipRect.Width)
            {
                return;
            }

            if (y < clipRect.Y || y > clipRect.Y + clipRect.Height)
            {
                return;
            }

            parent.Draw(tile, context, pos, c);
        }
    }
}
