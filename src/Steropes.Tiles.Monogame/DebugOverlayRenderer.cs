using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Steropes.Tiles.Monogame
{
    /// <summary>
    ///  Draws a crosshair through the centre of the screen. This produces a quick and dirty
    ///  visual reference point to check animations and movement against.
    /// </summary>
    public class DebugOverlayRenderer : DrawableGameComponent
    {
        readonly ILogAdapter logger = LogProvider.CreateLogger<DebugOverlayRenderer>();
        readonly MetricsPrinter metricsPrinter;
        readonly int sampleFrequency;
        int frameCounter;

        public DebugOverlayRenderer(Game game) : base(game)
        {
            metricsPrinter = new MetricsPrinter(game);
            sampleFrequency = 1000;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawDebugInfo();
        }

        void DrawDebugInfo()
        {
            var spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();
            var screenSize = Game.Window.ClientBounds;

            var width = screenSize.Width;
            var height = screenSize.Height;
            spriteBatch.DrawLine(new Vector2(width / 2.0f, 0), new Vector2(width / 2.0f, height), Color.Blue);
            spriteBatch.DrawLine(new Vector2(0, height / 2.0f), new Vector2(width, height / 2.0f), Color.Blue);
            spriteBatch.End();
            spriteBatch.Dispose();

            frameCounter += 1;
            if (frameCounter > sampleFrequency)
            {
                logger.Trace("GraphicsDevice Profiling: {0}", metricsPrinter);
                frameCounter = 0;
            }
        }

        class MetricsPrinter
        {
            readonly Game game;

            public MetricsPrinter(Game game)
            {
                this.game = game;
            }

            public override string ToString()
            {
                var metrics = game.GraphicsDevice.Metrics;
                return $"ClearCount={metrics.ClearCount}, DrawCount={metrics.DrawCount}, Sprites={metrics.SpriteCount}, Targets={metrics.TargetCount}";
            }
        }
    }
}
