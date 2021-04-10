using System;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.Monogame
{
    public static class GameRenderingExtensions
    {
        class ScreenTrackingSubscription : IDisposable
        {
            readonly GameRendering gameRendering;
            readonly EventHandler registeredListener;

            public ScreenTrackingSubscription(GameRendering gameRendering, EventHandler registeredListener)
            {
                this.gameRendering = gameRendering ?? throw new ArgumentNullException(nameof(gameRendering));
                this.registeredListener = registeredListener ?? throw new ArgumentNullException(nameof(registeredListener));
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            void Dispose(bool disposing)
            {
                if (disposing)
                {
                    var changeGuardian = WindowSizeChangeGuardian.Lookup(gameRendering.Game);
                    if (changeGuardian != null)
                    {
                        changeGuardian.WindowSizeChanged -= registeredListener;
                    }
                }
            }
        }

        public static IDisposable TrackScreenSize(this GameRendering r, Insets insets = new Insets())
        {
            void UpdateBounds()
            {
                var cb = r.Game.Window.ClientBounds;
                var x = insets.Left;
                var y = insets.Top;
                var w = Math.Max(1, cb.Width - insets.Left - insets.Right);
                var h = Math.Max(1, cb.Height - insets.Top - insets.Bottom);
                r.Bounds = new Rect(x, y, w, h);
            }

            void Handler(object o, EventArgs eventArgs) => UpdateBounds();

            var changeGuardian = WindowSizeChangeGuardian.Install(r.Game);
            changeGuardian.WindowSizeChanged += Handler;

            // finally initialize once 
            UpdateBounds();
            return new ScreenTrackingSubscription(r, Handler);
        }
    }
}
