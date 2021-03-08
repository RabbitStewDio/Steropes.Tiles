using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Steropes.UI;
using Steropes.UI.Components;
using Steropes.UI.Platform;
using Steropes.UI.State;
using Steropes.UI.Widgets;
using Steropes.UI.Widgets.Container;

namespace Steropes.Tiles.MonogameDemo
{
  internal class GameStateInitialSelection : GameStateFadeTransition
  {
    readonly INamedStateManager stateService;

    public GameStateInitialSelection(Game game,
                                     INamedStateManager stateService,
                                     IUIManager uiManager) : base(CreateDrawingService(game))
    {
      this.stateService = stateService;
      uiManager.Root.Content = CreateContent(uiManager.UIStyle);
    }

    void OnDungeonDemoSelected(object s, EventArgs args)
    {
      if (stateService.IsSwitching)
      {
        return;
      }

      var state = stateService.States["dungeon"];
      if (state != null)
      {
        stateService.SwitchState(state);
      }
    }

    void OnStrategyDemoSelected(object s, EventArgs args)
    {
      if (stateService.IsSwitching)
      {
        return;
      }

      var state = stateService.States["strategy"];
      if (state != null)
      {
        stateService.SwitchState(state);
      }
    }

    IWidget CreateContent(IUIStyle style)
    {
      var btSelectDungeonDemo = new Button(style, "Dungeon Game Demo");
      btSelectDungeonDemo.ActionPerformed += OnDungeonDemoSelected;

      var btSelectStrategyDemo = new Button(style, "Strategy Game Demo");
      btSelectStrategyDemo.ActionPerformed += OnStrategyDemoSelected;

      var g = new Group(style)
      {
        new BoxGroup(style, Orientation.Vertical, 5)
        {
          btSelectDungeonDemo,
          btSelectStrategyDemo
        }
      };
      g.Anchor = AnchoredRect.CreateCentered();
      return g;
    }

    static IBatchedDrawingService CreateDrawingService(Game game)
    {
      return new BatchedDrawingService(game);
    }

    public override void Draw(GameTime time)
    {
    }

    public override void Update(GameTime elapsedTime)
    {
    }
  }
}