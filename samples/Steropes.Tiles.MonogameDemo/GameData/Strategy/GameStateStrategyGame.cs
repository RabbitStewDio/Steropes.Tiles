using Microsoft.Xna.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Demo.Core.GameData.Strategy;
using Steropes.Tiles.Monogame;
using Steropes.Tiles.MonogameDemo.GameData.Strategy;
using Steropes.Tiles.MonogameDemo.Gui;
using Steropes.Tiles.TexturePack;
using Steropes.UI;
using Steropes.UI.Components;
using Steropes.UI.Platform;
using Steropes.UI.State;
using Steropes.UI.Widgets.Container;
using Insets = Steropes.Tiles.DataStructures.Insets;

namespace Steropes.Tiles.MonogameDemo
{
    class GameStateStrategyGame : GameStateFadeTransition
    {
        readonly IUIManager ui;
        readonly StrategyGameData gd;
        readonly IWidget rootContent;
        readonly DebugOverlayRenderer overlay;
        readonly GameRendering gameRendering;

        public GameStateStrategyGame(Game game,
                                     IUIManager ui) : base(new BatchedDrawingService(game))
        {
            this.ui = ui;
            gd = new StrategyGameData();

            var textureOperations = new MonoGameTextureOperations(game.GraphicsDevice);
            var tileProducer = new MonoGameTileProducer(textureOperations);

            var tileSet = CreateTileSet(game, tileProducer, textureOperations);
            var grf = CreateRenderingFactory(game, gd, tileSet, tileProducer, textureOperations);

            var mapComponent = new Group(ui.UIStyle)
            {
                Anchor = AnchoredRect.Full
            };

            gameRendering = new GameRendering(game, grf.RenderingConfig, grf.RenderControl);
            gameRendering.AddLayers(grf.Create(new DefaultGameRenderCallbackFactory<Nothing>(game)));
            gameRendering.AddLayer(new CityBarRenderingFactory(grf.RenderingConfig, gd, tileSet.TileSize, mapComponent).CreateCityBarRenderer());
            gameRendering.TrackScreenSize(new Insets(10, 10, 10, 10));
            gameRendering.CenterPointInMapCoordinates = gd.Settlements[1].Location;
            gameRendering.DrawOrder = -10;

            overlay = new DebugOverlayRenderer(game) {DrawOrder = -5};

            var navigationUI = CreateNavigationUi(ui);

            rootContent = new Group(ui.UIStyle)
            {
                mapComponent,
                navigationUI.RootComponent
            };
        }

        public override void Start()
        {
            base.Start();
            ui.Root.Content = rootContent;
        }

        StrategyGameRenderingFactory<MonoGameTile, XnaTexture, Color> CreateRenderingFactory(Game game,
                                                                                             StrategyGameData gameData,
                                                                                             StrategyGameTileSet<MonoGameTile> tileSet,
                                                                                             MonoGameTileProducer tileProducer,
                                                                                             MonoGameTextureOperations textureOperations)
        {
            var config = new GameRenderingConfig(tileSet.RenderType,
                                                 new Range(0, gameData.TerrainWidth),
                                                 new Range(0, gameData.TerrainHeight));
            return new StrategyGameRenderingFactory<MonoGameTile, XnaTexture, Color>(config, gameData, tileSet, tileProducer, textureOperations);
        }

        StrategyGameTileSet<MonoGameTile> CreateTileSet(Game game,
                                                        MonoGameTileProducer tileProducer,
                                                        MonoGameTextureOperations textureOperations)
        {
            var contentLoader = new MonoGameContentLoader(game.Content);
            var tp = new TexturePackLoader<MonoGameTile, XnaTexture, XnaRawTexture>(contentLoader, tileProducer).Read("Tiles/Civ/tiles.xml");
            var rt = tp.TextureType == TextureType.Grid ? RenderType.Grid : RenderType.IsoDiamond;
            var tileSet = new StrategyGameTileSet<MonoGameTile>(tp, rt);
            tileSet.InitializeBlendingRules(gd.Rules);
            return tileSet;
        }

        NavigationUI CreateNavigationUi(IUIManager ui)
        {
            var navigationUI = new NavigationUI(ui, gameRendering);
            var pname = nameof(NavigationUI.TileUnderMouseCursor);
            navigationUI.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == pname)
                {
                    gd.MousePosition = navigationUI.TileUnderMouseCursor;
                }
            };
            return navigationUI;
        }

        public override void Draw(GameTime t)
        {
            gameRendering.Draw(t);
            overlay.Draw(t);
        }

        public override void Update(GameTime elapsedTime)
        {
        }
    }
}
