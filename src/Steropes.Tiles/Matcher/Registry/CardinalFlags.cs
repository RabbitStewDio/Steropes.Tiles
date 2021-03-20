using System;

namespace Steropes.Tiles.Matcher.Registry
{
    [Flags]
    public enum CardinalFlags
    {
        None = 0,
        North = 1,
        East = 2,
        South = 4,
        West = 8
    }
}
