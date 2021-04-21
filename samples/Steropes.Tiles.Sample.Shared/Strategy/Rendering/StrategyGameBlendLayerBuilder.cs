using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Sample.Shared.Util;
using Steropes.Tiles.TexturePack;
using Steropes.Tiles.TexturePack.Blending;
using Steropes.Tiles.TexturePack.Grids;
using Steropes.Tiles.TexturePack.Operations;

namespace Steropes.Tiles.Sample.Shared.Strategy.Rendering
{
    public class StrategyGameBlendLayerBuilder<TTile, TTexture, TColor>
        where TTile : ITexturedTile<TTexture>
        where TTexture : ITexture
    {
        readonly TerrainToGraphicTagMapping mappingHelper;
        readonly GameRenderingConfig renderingConfig;
        readonly IDerivedTileProducer<TTile, TTexture> tileProducer;
        readonly ITextureOperations<TTexture, TColor> textureOperations;
        readonly ITileRegistry<TTile> tileRegistry;
        readonly IStrategyGameTileSet tileSet;

        public StrategyGameBlendLayerBuilder(GameRenderingConfig renderingConfig,
                                             IDerivedTileProducer<TTile, TTexture> tileProducer,
                                             ITextureOperations<TTexture, TColor> textureOperations,
                                             StrategyGameData gameData,
                                             IStrategyGameTileSet tileSet,
                                             ITileRegistry<TTile> tileRegistry)
        {
            this.renderingConfig = renderingConfig;
            this.tileProducer = tileProducer;
            this.textureOperations = textureOperations;
            this.tileSet = tileSet;
            this.tileRegistry = tileRegistry;
            GameData = gameData;
            mappingHelper = new TerrainToGraphicTagMapping(gameData.Rules.TerrainTypes, tileSet);
        }

        public StrategyGameData GameData { get; }

        public bool TryBuildBlendLayer(out ITileMatcher<TTile, Nothing> result)
        {
            var blendSelf = new bool[GameData.Rules.TerrainTypes.Count];
            var blendGraphics = new string[GameData.Rules.TerrainTypes.Count];

            // Precompute the blending information ..
            // 
            // This locates the declared blend texture for all terrain types and 
            foreach (var t in GameData.Rules.TerrainTypes)
            {
                var graphic = mappingHelper.Find(t);
                var tilename = graphic.GetBlendGraphicFor(tileSet.BlendLayer);

                if (tilename == null || !tileRegistry.TryFind(tilename, out var _))
                {
                    continue;
                }

                var indexOf = GameData.Rules.TerrainTypes.IndexOf(t);
                blendGraphics[indexOf] = tilename;
                blendSelf[indexOf] = graphic.DrawInBlendLayer;
            }

            var terrain = GameData.Terrain;

            string MapQuery(int x, int y) => blendGraphics[terrain[x, y].TerrainIdx];
            bool IsBlending(int x, int y) => blendSelf[terrain[x, y].TerrainIdx];

            if (BlendingTileGeneratorRegistry.TryCreate(out var reg,
                                                        tileRegistry,
                                                        tileSet.RenderType,
                                                        textureOperations,
                                                        tileProducer,
                                                        tileSet.TileSize))
            {
                var p = reg.Populate(blendGraphics);
                result = new BlendNeighboursSelector2<TTile, Nothing>(p,
                                                                      renderingConfig.MatcherNavigator,
                                                                      MapQuery,
                                                                      IsBlending);
                return true;
            }

            result = default;
            return false;
        }
    }
}
