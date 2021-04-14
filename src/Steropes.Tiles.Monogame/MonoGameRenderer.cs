using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Monogame
{
    /// <summary>
    ///   A XNA renderer.
    /// <para/>
    ///  This renderer uses an internal scaling factor of 2 when rendering sprites using
    ///  a transformation matrix on the sprite-batch. The sprite-batch uses the Rectangle
    ///  class (which is integer based) for rendering. When the tiles have a 
    ///  
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class MonoGameRenderer<TContext> : IRenderCallback<MonoGameTile, TContext>
    {
        
        public IRendererControl Viewport { get; }
        static readonly ILogAdapter Logger = LogProvider.CreateLogger<MonoGameRenderer<TContext>>(); 
        readonly ViewportCoordinates[] offsetsBySpritePosition;
        readonly RasterizerState enableScissorTest;
        readonly IntDimension tileSize;

        public MonoGameRenderer(IGraphicsDeviceService graphicsDeviceService,
                                IRendererControl viewport)
        {
            Viewport = viewport ?? throw new ArgumentNullException(nameof(viewport));
            SpriteBatch = new SpriteBatch(graphicsDeviceService.GraphicsDevice);

            this.GraphicsDeviceService = graphicsDeviceService;
            offsetsBySpritePosition = SpritePositionExtensions.OffsetsFor(viewport.ActiveRenderType);
            tileSize = viewport.TileSize;
            enableScissorTest = RasterizerState.CullCounterClockwise.Copy();
            enableScissorTest.ScissorTestEnable = true;
        }

        protected IGraphicsDeviceService GraphicsDeviceService { get; }

        protected SpriteBatch SpriteBatch { get; }

        public void StartLine(int line, in ContinuousViewportCoordinates screenPos)
        { }

        public void EndLine(int line, in ContinuousViewportCoordinates screenPos)
        { }


        public void StartDrawing()
        {
            var s = Viewport.Bounds;
            var o = Viewport.CalculateTileOffset();

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, null, enableScissorTest, null,
                              Matrix.CreateTranslation(o.X, o.Y, 0) * Matrix.CreateScale(0.5f));
            SpriteBatch.GraphicsDevice.ScissorRectangle = new Rectangle((int)s.X * 2, (int)s.Y * 2, (int)Math.Ceiling(s.Width) * 2, (int)Math.Ceiling(s.Height) * 2);
        }

        public void Draw(MonoGameTile tile, TContext context, SpritePosition pos, in ContinuousViewportCoordinates c)
        {
            if (!tile.HasTexture)
            {
                // This must be a dummy tile. Tile matchers never return null after all.
                return;
            }

            var texture = tile.Texture;
            var anchor = tile.Anchor;
            var offset = offsetsBySpritePosition[(int)pos];

            var source = texture.Bounds;
            var screenPos = new ContinuousViewportCoordinates(offset.X + c.X, offset.Y + c.Y);
            var renderPos = screenPos.ToPixels(tileSize);

            // round to full pixel coordinates to combat rounding errors that show up as rendering artefacts
            // Tiles are given in integer sizes, so this is a safe option here.
            var destPos = new Rectangle((int)(2 * Math.Floor(renderPos.X - anchor.X)),
                                        (int)(2 * Math.Floor(renderPos.Y - anchor.Y)),
                                        source.Width * 2,
                                        source.Height * 2);

            if (Logger.IsTraceEnabled)
            {
                Logger.Trace("Rendering tag '{0}' at pos={1} (sprite-pos={2}), map=({3}, {4}), texture-bounds={5}", tile.Tag, destPos, pos, c.X, c.Y, source);
            }

            var tint = Color.White;
            SpriteBatch.Draw(texture.Texture, destPos, source.ToXna(), tint);
        }

        public void FinishedDrawing()
        {
            SpriteBatch.End();
        }
    }
}
