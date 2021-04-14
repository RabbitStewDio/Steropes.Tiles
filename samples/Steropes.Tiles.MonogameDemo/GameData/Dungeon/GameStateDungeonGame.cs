using Microsoft.Xna.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Monogame;
using Steropes.Tiles.MonogameDemo.Gui;
using Steropes.Tiles.Sample.Shared.Dungeon;
using Steropes.UI;
using Steropes.UI.Components;
using Steropes.UI.Platform;
using Steropes.UI.State;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon
{
    class GameStateDungeonGame : GameStateFadeTransition
    {
        readonly IUIManager ui;
        readonly DungeonGameData gd;
        readonly DebugOverlayRenderer overlay;
        readonly GameRendering gameRendering;
        readonly IWidget rootComponent;

        public GameStateDungeonGame(Game game,
                                    IUIManager ui) : base(CreateDrawingService(game))
        {
            this.ui = ui;
            gd = new DungeonGameData();

            var contentLoader = new MonoGameContentLoader(game.Content);
            var textureOps = new MonoGameTextureOperations(game.GraphicsDevice);
            var tileProducer = new MonoGameTileProducer(textureOps);

            var renderFactory = new DefaultGameRenderCallbackFactory<Nothing>(game);
            var tileSet = new DungeonTileSet<MonoGameTile, XnaTexture, XnaRawTexture>(contentLoader, tileProducer);
            var config = new GameRenderingConfig(RenderType.IsoDiamond,
                                                 new Range(0, gd.Map.Width),
                                                 new Range(0, gd.Map.Height));
            var grf = new DungeonGameRenderingFactory<MonoGameTile>(config, gd, tileSet);

            gameRendering = new GameRendering(game, grf.RenderingConfig, grf.RenderControl);
            gameRendering.AddLayers(grf.Create(renderFactory, Nothing.Instance, Nothing.Instance));
            gameRendering.TrackScreenSize();
            gameRendering.DrawOrder = -10;

            overlay = new DebugOverlayRenderer(game) {DrawOrder = -5};

            var navigationUI = new NavigationUI(ui, gameRendering);
            rootComponent = navigationUI.RootComponent;
        }

        static IBatchedDrawingService CreateDrawingService(Game game)
        {
            return new BatchedDrawingService(game);
        }

        public override void Start()
        {
            ui.Root.Content = rootComponent;
        }

        public override void Draw(GameTime elapsedTime)
        {
            gameRendering.Draw(elapsedTime);
            overlay.Draw(elapsedTime);
        }

        public override void Update(GameTime elapsedTime)
        {
            gd.Update((float)elapsedTime.TotalGameTime.TotalSeconds);
        }
    }
}
