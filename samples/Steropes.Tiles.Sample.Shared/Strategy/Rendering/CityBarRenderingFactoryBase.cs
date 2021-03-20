using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Demo.Core.GameData.Strategy;
using Steropes.Tiles.Demo.Core.GameData.Strategy.Model;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Plotter.Operations.Builder;
using Steropes.Tiles.Renderer;
using System;

namespace Steropes.Tiles.Sample.Shared.Strategy.Rendering
{
    public abstract class CityBarRenderingFactoryBase
    {
        protected CityBarRenderingFactoryBase(GameRenderingConfig renderingConfig,
                                              StrategyGameData gameData,
                                              IntDimension tileSize)
        {
            RenderingConfig = renderingConfig ?? throw new ArgumentNullException(nameof(RenderingConfig));
            GameData = gameData ?? throw new ArgumentNullException(nameof(GameData));

            RenderControl = new RendererControl(tileSize, renderingConfig.RenderType);
        }

        public RendererControl RenderControl { get; }
        public GameRenderingConfig RenderingConfig { get; }
        public StrategyGameData GameData { get; }

        public IPlotOperation CreateCityBarRenderer()
        {
            var map = GameData.Terrain;

            bool CityMapper(byte index, out ISettlement s, out Nothing context)
            {
                if (index == 0)
                {
                    s = null;
                    context = null;
                    return false;
                }

                s = GameData.Settlements[index];
                context = null;
                return true;
            }

            var settlementMatcher =
                new DirectMappingTileMatcher<byte, ISettlement, Nothing>((x, y) => map[x, y].City, CityMapper);

            return PlotOperations.FromContext(RenderingConfig)
                                 .Create(settlementMatcher)
                                 .ForViewport()
                                 .WithRenderer(CreateRenderer())
                                 .Build();
        }

        protected abstract IRenderCallback<ISettlement, Nothing> CreateRenderer();
    }
}
