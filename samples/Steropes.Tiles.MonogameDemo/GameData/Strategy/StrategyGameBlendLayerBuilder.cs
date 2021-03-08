using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Monogame.Blending;
using Steropes.Tiles.Monogame.Tiles;
using Steropes.Tiles.MonogameDemo.GameData.Util;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy
{
  public class StrategyGameBlendLayerBuilder
  {
    readonly TerrainToGraphicTagMapping mappingHelper;
    readonly GameRenderingConfig renderingConfig;
    readonly ITileRegistry<ITexturedTile> tileRegistry;
    readonly IStrategyGameTileSet tileSet;

    public StrategyGameBlendLayerBuilder(GameRenderingConfig renderingConfig,
                                         StrategyGameData gameData,
                                         IStrategyGameTileSet tileSet,
                                         ITileRegistry<ITexturedTile> tileRegistry)
    {
      this.renderingConfig = renderingConfig;
      this.tileSet = tileSet;
      this.tileRegistry = tileRegistry;
      GameData = gameData;
      mappingHelper = new TerrainToGraphicTagMapping(gameData.Rules.TerrainTypes, tileSet);
    }

    public StrategyGameData GameData { get; }

    public ITileMatcher<ITexturedTile, Nothing> BuildBlendLayer(GraphicsDevice device)
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

      var reg = new BlendingTileGeneratorRegistry(device, 
                                                  tileRegistry, 
                                                  tileSet.RenderType, 
                                                  tileSet.TileSize)
        .WithPopulatedCache(blendGraphics);
      return new BlendNeighboursSelector2<ITexturedTile, Nothing>(reg, 
                                                                     renderingConfig.MatcherNavigator, 
                                                                     MapQuery, 
                                                                     IsBlending);
    }
  }
}