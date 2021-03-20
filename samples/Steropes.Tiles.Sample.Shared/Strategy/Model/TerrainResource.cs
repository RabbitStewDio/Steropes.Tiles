namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Model
{
  /// <summary>
  ///  Defines a resource bonus for terrain tiles. This is something like
  ///  gold, iron or oil. Resources can grant a fixed production bonus,
  ///  which will increase even more with terrain improvements.
  /// </summary>
  public interface ITerrainResource : IRuleElement
  {
    Resources BonusResources { get; }
  }

  public class TerrainResource : RuleElement, ITerrainResource
  {
    public TerrainResource(string id,
                           char asciiId,
                           string name,
                           Resources bonusResources,
                           string graphicTag,
                           params string[] alternativeGraphicTags) :
      base(id, asciiId, name, graphicTag, alternativeGraphicTags)
    {
      BonusResources = bonusResources;
    }

    public Resources BonusResources { get; }
  }
}