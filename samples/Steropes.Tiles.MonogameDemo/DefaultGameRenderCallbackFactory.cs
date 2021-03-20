using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.Monogame;
using Steropes.Tiles.Renderer;
using Steropes.Tiles.Sample.Shared;
using Steropes.Tiles.Unit2D.Demo.Components;

namespace Steropes.Tiles.MonogameDemo
{
    public class DefaultGameRenderCallbackFactory<TParameter> : IRenderCallbackFactory<TParameter, MonoGameTile>
    {
        public Game Game { get; }

        public DefaultGameRenderCallbackFactory(Game game)
        {
            Game = game;
        }

        public IRenderCallback<MonoGameTile, TContext> CreateRenderer<TContext>(IRenderingFactoryConfig<MonoGameTile> tileSetSource, TParameter p)
        {
            return new MonoGameRenderer<TContext>(Game.Services.GetService<IGraphicsDeviceService>(), tileSetSource.RenderControl);
        }
    }
}
