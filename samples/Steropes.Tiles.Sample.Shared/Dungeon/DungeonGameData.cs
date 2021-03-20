using System;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple;
using Steropes.Tiles.Demo.Core.Util;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Util;
using Random = System.Random;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon
{
  /// <summary>
  ///   A holder for all logical game data, like rules and maps. This represents the game world
  ///   and game model.
  /// </summary>
  public class DungeonGameData
  {
    public const int MapWidth = 100;
    public const int MapHeight = 100;

    readonly CharacterService charService;
    readonly ContainerReferenceProvider containerRefs;
    readonly ContainerService containerSrv;
    readonly ItemService itemService;

    public DungeonGameData()
    {
      Rules = new DungeonGameRules();
      containerRefs = new ContainerReferenceProvider();
      containerSrv = new ContainerService();
      itemService = new ItemService(containerSrv, containerSrv, containerRefs);
      charService = new CharacterService(itemService);

      Map = new DungeonMap(100, 100, Rules);
      Map.FloorLayer[98, 98] = Rules.Floors.Stone;

      Map.FloorLayer[0, 0] = Rules.Floors.Stone;
      Map.FloorLayer[1, 0] = Rules.Floors.Stone;
      Map.FloorLayer[2, 0] = Rules.Floors.Stone;
      Map.FloorLayer[2, 1] = Rules.Floors.Stone;
      Map.FloorLayer[3, 1] = Rules.Floors.Stone;
      Map.FloorLayer[4, 1] = Rules.Floors.Stone;
      Map.FloorLayer[5, 1] = Rules.Floors.Stone;
      Map.FloorLayer[2, 2] = Rules.Floors.Stone;
      Map.FloorLayer[2, 3] = Rules.Floors.Stone;
      Map.FloorLayer[5, 3] = Rules.Floors.Stone;
      Map.FloorLayer[2, 4] = Rules.Floors.Stone;
      Map.FloorLayer[3, 4] = Rules.Floors.Stone;
      Map.FloorLayer[4, 4] = Rules.Floors.Stone;
      Map.FloorLayer[5, 4] = Rules.Floors.Stone;
      Map.WallLayer[0, 0] = Rules.Walls.Stone;
      Map.WallLayer[1, 0] = Rules.Walls.Stone;
      Map.WallLayer[2, 0] = Rules.Walls.Stone;
      Map.WallLayer[2, 1] = Rules.Walls.Stone;
      Map.WallLayer[3, 1] = Rules.Walls.Stone;
      Map.WallLayer[4, 1] = Rules.Walls.Stone;
      Map.WallLayer[5, 1] = Rules.Walls.Stone;
      Map.WallLayer[2, 2] = Rules.Walls.Stone;
      Map.WallLayer[5, 2] = Rules.Walls.Stone;
      Map.WallLayer[2, 3] = Rules.Walls.Stone;
      Map.WallLayer[5, 3] = Rules.Walls.Stone;
      Map.WallLayer[2, 4] = Rules.Walls.Stone;
      Map.WallLayer[3, 4] = Rules.Walls.Stone;
      Map.WallLayer[4, 4] = Rules.Walls.Passage;
      Map.WallLayer[5, 4] = Rules.Walls.Stone;
      Map.DecorationLayer[2, 2] = Rules.Decorations.Shelf;
      Map.DecorationLayer[4, 1] = Rules.Decorations.Shelf;
      Map.DecorationLayer[2, 3] = Rules.Decorations.Window;
      Map.DecorationLayer[5, 2] = Rules.Decorations.Window;


      Map.WallLayer[10, 10] = Rules.Walls.Stone;
      Map.WallLayer[10, 12] = Rules.Walls.Stone;
      Map.WallLayer[12, 12] = Rules.Walls.Stone;
      Map.WallLayer[12, 10] = Rules.Walls.Stone;

      var chest = CreateItemAt(Rules.Items.Chest, 5, 5);

      SpawnJesters(1000);
    }

    void SpawnJesters(int jesters)
    {

        var random = new Random();
        for (var x = 0; x < jesters; x += 1)
        {
            var xStart = random.Next((double) Map.Width);
            var yStart = random.Next((double) Map.Height);
            var xEnd = random.Next((double) Map.Width);
            var yEnd = random.Next((double) Map.Height);

            var player = charService.Create("Bozo " + x, Rules.Characters.Jester);
            MoveTo(player.Avatar, xStart, yStart);
            itemService.AddTrait(player.Avatar,
                new MoveTrait(itemService, player.Avatar, new DoublePoint(xStart, yStart),
                    new DoublePoint(xEnd, yEnd)));
        }
    }

    public void Update(float gameTime)
    {
      itemService.Update(gameTime);
      charService.Update(gameTime);
    }

    public IItemService ItemService
    {
      get { return itemService; }
    }

    public DungeonGameRules Rules { get; }

    public DungeonMap Map { get; }

    public ReadOnlyListWrapper<IItem> QueryItems(int x, int y)
    {
        var containerReference = containerRefs.From(new MapCoordinate(x, y));
        return containerSrv.Contents(containerReference);
    }

    public IItem MoveTo(IItem item, double x, double y)
    {
      if (itemService.TraitFor(item, out ILocationTrait locationTrait))
      {
        locationTrait.Position = new DoublePoint(x, y);
      }
      return item;
    }

    IItem CreateItemAt(IItemType itemType, double x, double y)
    {
      var item = itemService.Create(itemType);
      return MoveTo(item, x, y);
    }


    public class LerpValue2 : AnimatedValue
    {
      public LerpValue2(float start, float end, float duration, float delay, AnimationLoop loop)
      {
        Start = start;
        End = end;
        Duration = duration;
        Delay = delay;
        Time = 0.0;
        Loop = loop;
        Direction = AnimationDirection.Forward;
      }

      public LerpValue2(float start, float end, float duration, AnimationLoop loop)
        : this(start, end, duration, 0.0f, loop)
      {
      }

      public LerpValue2(float start, float end, float duration, float delay)
        : this(start, end, duration, delay, AnimationLoop.NoLoop)
      {
      }

      public LerpValue2(float start, float end, float duration)
        : this(start, end, duration, 0.0f, AnimationLoop.NoLoop)
      {
      }

      public override float CurrentValue
      {
        get
        {
          if (Math.Abs(Duration) < 0.0005)
          {
            return Start;
          }
          var amount = (float) ((Time - Delay) / Duration);
          return Lerp(Start, End, amount);
        }
      }

      float Lerp(float v0, float v1, float t)
      {
          return (1 - t) * v0 + t * v1;
      }

      public float End { get; set; }

      public float Start { get; set; }

      public override string ToString()
      {
        return string.Format("LerpValue={{Start: {0}, End: {1}, CurrentValue: {2}}}", Start, End, CurrentValue);
      }
    }

    class MoveTrait : IUpdateableTrait
    {
      readonly IItem item;
      readonly IItemService itemService;
      readonly AnimatedValue lerpX;
      readonly AnimatedValue lerpY;
      ILocationTrait ls;

      public MoveTrait(IItemService itemService, IItem item, DoublePoint start, DoublePoint end)
      {
        this.itemService = itemService;
        this.item = item;
        var distance = Distance(start, end) * 4;
        lerpX = new LerpValue2((float) start.X, (float) end.X, distance, AnimationLoop.LoopBackAndForth);
        lerpY = new LerpValue2((float) start.Y, (float) end.Y, distance, AnimationLoop.LoopBackAndForth);
      }

      public void Update(float time)
      {
        lerpX.Update(time);
        lerpY.Update(time);

        if (ls == null)
        {
          itemService.TraitFor(item, out ls);
        }
        if (ls != null)
        {
          ls.Position = new DoublePoint(lerpX.CurrentValue, lerpY.CurrentValue);
        }
      }

      static float Distance(DoublePoint start, DoublePoint end)
      {
        var dx = start.X - end.X;
        var dy = start.Y - end.Y;
        return (float) Math.Sqrt(dx * dx + dy * dy);
      }
    }
  }
}