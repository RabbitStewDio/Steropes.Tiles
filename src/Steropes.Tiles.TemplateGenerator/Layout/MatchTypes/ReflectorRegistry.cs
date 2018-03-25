using Steropes.Tiles.Matcher.Registry;

namespace Steropes.Tiles.TemplateGenerator.Layout.MatchTypes
{
  class ReflectorRegistry : ITileRegistry<string>
  {
    public string Find(string tag)
    {
      return tag;
    }

    public bool TryFind(string tag, out string tile)
    {
      tile = tag;
      return true;
    }
  }
}