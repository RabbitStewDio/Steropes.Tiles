using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steropes.Tiles.Monogame;
using Steropes.Tiles.MonogameDemo.Gui;
using Steropes.Tiles.TexturePack.Atlas;
using Steropes.Tiles.TexturePack.Grids;
using Steropes.UI;
using Steropes.UI.Components.Window;
using Steropes.UI.Input;
using Steropes.UI.State;
using Steropes.UI.Util;
using XnaPoint = Microsoft.Xna.Framework.Point;

namespace Steropes.Tiles.MonogameDemo
{
    /// <summary>
    ///   A simple dungeon demo game.
    /// </summary>
    /// This game demonstrates how the tile library can be used in a closer to real-world scenario.
    /// The game (if you can call it that) here is simple: Walk though a pre-generated dungeon to
    /// find the exit. 
    /// 
    /// The game world consists of multiple layers:
    /// (1) The floor. Floor can either be stone, mud or fire. Don't try to walk on fire.
    /// (2) The wall layer. This contains static, immobile items. The static
    /// things, like wall are stored in a byte-array similar to the terrain. Separating
    /// the static bits from the changing bits allows us to greatly reduce the complexity
    /// of the system. Walls and other items in the static layer simply exist as value types, 
    /// they do not have an identity on their own. 
    /// (3) The item layer. This contains changing items the user can manipulate. The entries
    /// are pointers into the item-manager. Each tile is assumed to be a container in itself,
    /// and can contain other container (tile containing chests containing bags)
    /// NPCs and PCs are considered items too. Each item in the item layer has an identity,
    /// and moving the item to a different location or container does not change that identity.
    /// (4) The air/effects layer. This contains things like fireball explosions, smoke and other effects.
    /// 
    /// When rendering, we have to render all layers at once. So we iterate over the visible grid
    /// positions, and then for each (x,y) positon we render the ground, wall, item and air layer
    /// in that order.
    internal class SimpleGame : Game
    {
        readonly FrameRateCalculator frameRateCalculator;
        IGameWindowService windowService;

        public SimpleGame()
        {
            Content.RootDirectory = "Content";
            var resolution = new XnaPoint(1393, 720);
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = resolution.X,
                PreferredBackBufferHeight = resolution.Y
            };

            IsFixedTimeStep = false;
            Graphics.SynchronizeWithVerticalRetrace = false;

            frameRateCalculator = new FrameRateCalculator();
        }

        public GraphicsDeviceManager Graphics { get; }

        void HandleClientSizeChanged(object sender, EventArgs e)
        {
            Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            Graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();

            var sizeChangeGuardian = WindowSizeChangeGuardian.Install(this);
            sizeChangeGuardian.WindowSizeChanged += HandleClientSizeChanged;

            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            var inputManager = new InputManager(this);
            inputManager.UpdateOrder = -10;

            var uiComponent = UIManagerComponent.CreateAndInit(this, inputManager, "Content");
            uiComponent.DrawOrder = 10;
            uiComponent.UpdateOrder = -5;
            windowService = uiComponent.Manager.ScreenService.WindowService;
            NavigationUI.SetupStyles(uiComponent.Manager);

            var stateManager = new NamedGameStateManager(this);
            stateManager.UpdateOrder = 0;
            stateManager.DrawOrder = 0;
            stateManager.States["start"] = new GameStateInitialSelection(this, stateManager, uiComponent.Manager);
            stateManager.States["dungeon"] = new GameStateDungeonGame(this, uiComponent.Manager);
            stateManager.States["strategy"] = new GameStateStrategyGame(this, uiComponent.Manager);
            stateManager.SwitchState(stateManager.States["start"]);
            Components.Add(stateManager);

            this.CenterOnScreen();
        }

        protected override bool BeginDraw()
        {
            GraphicsDevice.Clear(Color.White);
            return base.BeginDraw();
        }

        protected override void Update(GameTime gameTime)
        {
            frameRateCalculator.BeginTime();
            base.Update(gameTime);
            frameRateCalculator.EndTime();

            frameRateCalculator.Update(gameTime);
            windowService.Title = frameRateCalculator.ToString();
        }

        protected override void Draw(GameTime gameTime)
        {
            frameRateCalculator.BeginTime();

            base.Draw(gameTime);

            frameRateCalculator.EndTime();
            frameRateCalculator.Draw();
        }
    }
}
