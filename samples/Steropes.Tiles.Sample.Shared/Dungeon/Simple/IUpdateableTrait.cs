using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple
{
    public interface IUpdateableTrait : IItemTrait
    {
        void Update(float t);
    }
}
