using System;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon
{
  public class DungeonGameItemLocationResolver<TTile, TContext>: IRenderCallbackFilter<TTile, IItem, TTile, TContext>
  {
    readonly IItemService itemService;
    readonly IMapViewport viewPort;

    public DungeonGameItemLocationResolver(IItemService itemService, 
                                           IMapViewport viewPort)
    {
      this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
      this.viewPort = viewPort ?? throw new ArgumentNullException(nameof(viewPort));
    }

    public IRenderCallback<TTile, TContext> RenderTarget { get; set; }

    public void StartDrawing()
    {
      RenderTarget.StartDrawing();
    }

    public void StartLine(int logicalLine, ContinuousViewportCoordinates screen)
    {
      RenderTarget.StartLine(logicalLine, screen);
    }

    public void Draw(TTile tile, 
                     IItem context, 
                     SpritePosition pos, 
                     ContinuousViewportCoordinates screenLocation)
    {
      if (itemService.TraitFor(context, out ILocationTrait location))
      {
        var offset = ComputeItemOffsetInTile(location);
        RenderTarget.Draw(tile, default(TContext), pos, screenLocation + offset);
      }
      else
      {
        // item is on a fixed position.
        // Debug.WriteLine("Rendering fixed: " + context);
        RenderTarget.Draw(tile, default(TContext), pos, screenLocation);
      }

    }

    public void EndLine(int logicalLine, ContinuousViewportCoordinates screen)
    {
      RenderTarget.EndLine(logicalLine, screen);
    }

    public void FinishedDrawing()
    {
      RenderTarget.FinishedDrawing();
    }
    
    /// <summary>
    ///  This computes a intra-tile offset. With staggered isometric maps and tile sets we do not
    ///  have the luxury of an continuous coordinate system. 
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    ContinuousViewportCoordinates ComputeItemOffsetInTile(ILocationTrait location)
    {
      var position = location.Position;
      var mappedCont = viewPort.MapPositionToScreenPosition(position);

      var mapPosition = position.ToMapCoordinate().ToPoint();
      var mappedDisc = viewPort.MapPositionToScreenPosition(mapPosition);

      return mappedCont - mappedDisc;
    }

  }
}