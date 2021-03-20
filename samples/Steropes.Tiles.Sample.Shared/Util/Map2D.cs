using System;

namespace Steropes.Tiles.Demo.Core.GameData.Util
{
    public class Map2D<TEntity> : IMap2D<TEntity>
        where TEntity : class
    {
        public int Width { get; }
        public int Height { get; }
        readonly Func<int, int, TEntity> fallback;
        readonly TEntity[,] contents;

        public Map2D(int width, int height, Func<int, int, TEntity> fallback)
        {
            Width = width;
            Height = height;
            this.fallback = fallback;
            this.contents = new TEntity[width, height];
        }

        public TEntity this[int x, int y]
        {
            get
            {
                return contents[x, y] ?? fallback(x, y);
            }
            set
            {
                contents[x, y] = value;
            }
        }
    }
}
