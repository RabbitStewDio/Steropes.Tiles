using Microsoft.Xna.Framework;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Simple
{
  public interface IUpdateableTrait: IItemTrait
  {
    void Update(GameTime t);
  }
}