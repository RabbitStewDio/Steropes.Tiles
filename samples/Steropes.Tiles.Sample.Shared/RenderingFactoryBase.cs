using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Sample.Shared.Dungeon;
using System;

namespace Steropes.Tiles.Sample.Shared
{
    [Obsolete]
    public abstract class RenderingFactoryBase<TTile> : IRenderingFactoryConfig<TTile>
    {
        protected RenderingFactoryBase(GameRenderingConfig renderConfig,
                                       ITileSet<TTile> tileSet)
        {
            RenderingConfig = renderConfig;
            this.RenderControl = new RendererControl(tileSet.TileSize, RenderingConfig.RenderType);
            this.Tiles = tileSet.LoadTexturePack();
        }

        public GameRenderingConfig RenderingConfig { get; }
        public RendererControl RenderControl { get; }
        public ITileRegistry<TTile> Tiles { get; }
    }

    public interface IRenderingFactoryConfig<TTile>
    {
        GameRenderingConfig RenderingConfig { get; }
        RendererControl RenderControl { get; }
        ITileRegistry<TTile> Tiles { get; }
    }
}
