namespace Steropes.Tiles.Matcher.TileTags
{
  public static class TileTagEntrySelectionFactoryExtensions
  {
    public static ITileTagEntrySelection<TSelector>[] ToSelectionArray<TSelector>(this ITileTagEntrySelectionFactory<TSelector> factory)
    {
      var retval = new ITileTagEntrySelection<TSelector>[factory.Count];
      for (var i = 0; i < retval.Length; i++)
      {
        retval[i] = factory[i];
      }
      return retval;
    }
  }
}