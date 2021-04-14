using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace Steropes.Tiles.Monogame
{
    public interface IWindowSizeChangeGuardian
    {
        event EventHandler WindowSizeChanged;
    }

    /// <summary>
    ///  Monogame does not report Window size changes if a window is minimized or maximized. 
    ///  This component works around this problem by simply polling the size before each draw.
    ///  If needed, it then generates an event for all interested listeners. 
    /// </summary>
    public class WindowSizeChangeGuardian : DrawableGameComponent, IWindowSizeChangeGuardian
    {
        readonly ILogAdapter logger = LogProvider.CreateLogger<WindowSizeChangeGuardian>();
        XnaPoint currentSize;

        WindowSizeChangeGuardian(Game game) : base(game)
        {
            // Try to be the first in line on draw events. This allows renderers to update before they 
            // produce partial and probably invalid work.
            DrawOrder = int.MinValue;
        }

        public event EventHandler WindowSizeChanged;

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            var newSize = Game.Window.ClientBounds.Size;
            if (currentSize != newSize)
            {
                if (logger.IsTraceEnabled)
                {
                    logger.Trace("Screen size change detected. Was {0}, now {1}", currentSize, newSize);
                }
                currentSize = newSize;
                WindowSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public static IWindowSizeChangeGuardian Install(Game game)
        {
            var srv = game.Services.GetService<IWindowSizeChangeGuardian>();
            if (srv != null)
            {
                return srv;
            }

            var created = new WindowSizeChangeGuardian(game);
            game.Services.AddService(typeof(IWindowSizeChangeGuardian), created);
            game.Components.Add(created);
            return created;
        }

        public static IWindowSizeChangeGuardian Lookup(Game game)
        {
            return game.Services.GetService<IWindowSizeChangeGuardian>();
        }
    }
}
