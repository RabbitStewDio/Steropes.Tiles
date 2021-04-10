using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Renderer;
using Steropes.Tiles.Sample.Shared.Strategy;
using Steropes.Tiles.Sample.Shared.Strategy.Model;
using Steropes.Tiles.Sample.Shared.Strategy.Rendering;
using Steropes.UI.Widgets.Container;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy
{
    public class CityBarRenderingFactory : CityBarRenderingFactoryBase
    {
        readonly Group widgetParent;

        public CityBarRenderingFactory(GameRenderingConfig renderingConfig,
                                       StrategyGameData gameData,
                                       IntDimension tileSize,
                                       Group widgetParent) : base(renderingConfig, gameData, tileSize)
        {
            this.widgetParent = widgetParent;
        }

        protected override IRenderCallback<ISettlement, Nothing> CreateRenderer()
        {
            return new CityBarRenderer(widgetParent, RenderControl);
        }
    }
}
