namespace Steropes.Tiles
{
  public static class RenderTypeExtensions
  {
    public static bool IsStaggered(this RenderType t)
    {
      return t == RenderType.IsoStaggered || t == RenderType.Hex;
    }

    public static bool IsIsometric(this RenderType t)
    {
      return t == RenderType.IsoStaggered || t == RenderType.IsoDiamond;
    }
  }
}