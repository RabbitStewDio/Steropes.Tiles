using System;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Demo.TextMode
{
    public class ConsoleRenderer : IRenderCallback<TextTile, Nothing>
    {
        int discarded;
        int drawn;
        IntDimension tileSize;

        public void StartDrawing()
        {
            tileSize = new IntDimension(2, 2);
            drawn = 0;
            discarded = 0;
            Console.Clear();
        }

        public void StartLine(int line, in ContinuousViewportCoordinates screenPos)
        { }

        public void EndLine(int line, in ContinuousViewportCoordinates screenPos)
        { }

        public void FinishedDrawing()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, Console.WindowHeight - 2);
            Console.Write($"Drawn: {drawn} Discarded: {discarded} Total: {drawn + discarded}");
        }

        public virtual void Draw(TextTile tile, Nothing context, SpritePosition pos, in ContinuousViewportCoordinates p)
        {
            if (pos != SpritePosition.Whole)
            {
                return;
            }

            var pixels = p.ToPixels(tileSize);
            DrawOnScreen(tile, (int)Math.Floor(pixels.X), (int)Math.Floor(pixels.Y));
        }

        protected void DrawOnScreen(TextTile tile, int screenX, int screenY)
        {
            if (screenX < 0 || screenX >= Console.BufferWidth)
            {
                discarded += 1;
                return;
            }

            if (screenY < 0 || screenY >= Console.BufferHeight)
            {
                discarded += 1;
                return;
            }

            Console.SetCursorPosition(screenX, screenY);
            Console.ForegroundColor = tile.Color;
            Console.Write(tile.Rendering);
            drawn += 1;
        }
    }
}
