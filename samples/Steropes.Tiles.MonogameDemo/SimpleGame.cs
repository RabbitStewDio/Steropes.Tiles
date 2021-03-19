using System;
using Microsoft.Xna.Framework;
using Steropes.Tiles.Monogame;
using Steropes.Tiles.Monogame.Tiles;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon;
using Steropes.Tiles.MonogameDemo.GameData.Strategy;
using Steropes.Tiles.MonogameDemo.Gui;
using Steropes.UI;
using Steropes.UI.Components;
using Steropes.UI.Components.Window;
using Steropes.UI.Input;
using Steropes.UI.Platform;
using Steropes.UI.State;
using Steropes.UI.Util;
using Steropes.UI.Widgets.Container;
using Insets = Steropes.Tiles.DataStructures.Insets;
using Range = Steropes.Tiles.DataStructures.Range;
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

      var tileSet = new DungeonTileSet();
      var config = new GameRenderingConfig(RenderType.IsoDiamond, 
                                           new Range(0, gd.Map.Width),
                                           new Range(0, gd.Map.Height));
      var grf = new DungeonGameRenderingFactory(config, gd, tileSet, game);

      gameRendering = grf.Create();
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
      gd.Update(elapsedTime);
    }
  }

  class GameStateStrategyGame : GameStateFadeTransition
  {
    readonly IUIManager ui;
    readonly StrategyGameData gd;
    readonly IWidget rootContent;
    readonly DebugOverlayRenderer overlay;
    readonly GameRendering gameRendering;

    public GameStateStrategyGame(Game game,
                                 IUIManager ui) : base(CreateDrawingService(game))
    {
      this.ui = ui;
      gd = new StrategyGameData();

      var tileSet = CreateTileSet();
      var grf = CreateRenderingFactory(game, gd, tileSet);

      var mapComponent = new Group(ui.UIStyle)
      {
        Anchor = AnchoredRect.Full
      };

      gameRendering = grf.Create(mapComponent);
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

    static IBatchedDrawingService CreateDrawingService(Game game)
    {
      return new BatchedDrawingService(game);
    }

    public override void Start()
    {
      base.Start();
      ui.Root.Content = rootContent;
    }

    StrategyGameRenderingFactory CreateRenderingFactory(Game game,
                                                        StrategyGameData gameData,
                                                        StrategyGameTileSet tileSet)
    {
      var config = new GameRenderingConfig(tileSet.RenderType,
                                           new Range(0, gameData.TerrainWidth),
                                           new Range(0, gameData.TerrainHeight));
      return new StrategyGameRenderingFactory(config, gd, tileSet, game);
    }

    StrategyGameTileSet CreateTileSet()
    {
      var tp = TexturePackLoader.Read("Content/Tiles/Civ/tiles.xml");
      var rt = tp.TextureType == TextureType.Grid ? RenderType.Grid : RenderType.IsoDiamond;
      var tileSet = new StrategyGameTileSet(tp, rt);
      StrategyGameTileSet.Init(tileSet, gd.Rules);
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
      gd.Update(elapsedTime);
    }
  }
}