namespace Steropes.Tiles.MonogameDemo.GameData.Strategy.Model
{
  /// <summary>
  ///  Terrain extras are stored in a bit vector, and thus we 
  ///  need to assign them a globally valid index position for
  ///  that vector.
  /// </summary>
  public interface ITerrainExtra : IRuleElement
  {
    /// <summary>
    ///  A system generated id. We assume that this ID is 
    ///  generated in a deterministic way so that subsequent
    ///  instantiations of the same rule set receive the same 
    ///  id. However, it is not necessary to generate the 
    ///  same id across different rule sets or different 
    ///  versions of the same ruleset.
    /// </summary>
    int DataId { get; }

    /// <summary>
    ///  Defines a dependency between terrain extras. This 
    ///  is used for road and railroads.
    /// </summary>
    ITerrainExtra Requires { get; }
  }
}